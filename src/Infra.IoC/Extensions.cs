using Application.Gateways;
using Application.UseCases;
using Infra.Gateways;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC;
public static class Extensions
{
    public static IServiceCollection AddGateways(this IServiceCollection services)
    {
        services.AddScoped<ISefazGateway, HtmlAgilityPackSefazGateway>();
        services.AddScoped<IBrazilApiGateway, BrazilApiGateway>();
        return services;
    }

    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<FindPurchaseInfosUseCase>();
        return services;
    }
    public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("brasilAPI", context =>
        {
            context.BaseAddress = new Uri("https://brasilapi.com.br");
        });
        return services;
    }
}
