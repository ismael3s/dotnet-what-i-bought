using Application.UseCases;
using Infra.IoC;
using Microsoft.AspNetCore.Mvc;
using WhatIBoughtAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCustomHttpClient()
    .AddGateways()
    .AddUseCases();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/v1/find-nf", async ([FromQuery] string url, FindPurchaseInfosUseCase findPurchaseInfosUseCase, CancellationToken cancellationToken) =>
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
    .WithOpenApi();


app.MapGet("/api/v1/health-check", () =>
{
    return Results.Ok(new
    {
        Status = "Healthy"
    });
})
.WithName("Health check")
.WithOpenApi();


app.UseMiddleware<ExceptionMiddleware>();
app.Run();