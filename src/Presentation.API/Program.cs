using System.Threading.RateLimiting;
using Application.UseCases;
using Infra.IoC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using WhatIBoughtAPI.Middlewares;
using WhatIBoughtAPI.Options;
using ILogger = Serilog.ILogger;


var builder = WebApplication.CreateBuilder(args);
var customRateLimiterOption = new RateLimiterOption();
builder.Configuration.GetSection("RateLimiter")
    .Bind(customRateLimiterOption);
builder.Host.
    UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)
);
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCustomHttpClient()
    .AddGateways()
    .AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddFixedWindowLimiter("fixed", limiterOptions =>
        {
            limiterOptions.PermitLimit = customRateLimiterOption.PermitLimit;
            limiterOptions.QueueLimit = customRateLimiterOption.QueueLimit;
            limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            limiterOptions.Window = TimeSpan.FromSeconds(customRateLimiterOption.WindowSeconds);
        });
    })
    .AddUseCases();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/v1/find-nf", async ([FromQuery] string url,
        FindPurchaseInfosUseCase findPurchaseInfosUseCase,
        CancellationToken cancellationToken
    ) =>
    {   
        var result = await findPurchaseInfosUseCase.ExecuteAsync(
            new FindPurchaseInfosUseCase.FindPurchaseInfosUseCaseInput(url),
            cancellationToken
        );
        return Results.Ok(result);
    })
    .WithDisplayName("FindNFData")
    .WithName("FindNFData")
    .WithSummary("Find NF Data by URL and return it as JSON")
    .WithDescription("Should recieve an URL the URL must be from fazenda host")
    .WithTags("NF")
    .WithOpenApi()
    .RequireRateLimiting("fixed");


app.MapGet("/api/v1/health-check", () =>
    {
        return Results.Ok(new
        {
            Status = "Healthy"
        });
    })
    .WithName("Health check")
    .WithOpenApi();


app.UseMiddleware<ExceptionMiddleware>()
    .UseRateLimiter()
    .UseSerilogRequestLogging();
app.Run();