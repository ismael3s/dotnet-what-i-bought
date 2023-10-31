using Application.Exceptions;
using Application.Gateways;

namespace Application.UseCases;

public class FindPurchaseInfosUseCase
{
    private readonly ISefazGateway _sefazGateway;
    private readonly IBrazilApiGateway _brazilApiGateway;
    public record FindPurchaseInfosUseCaseInput(string Url = "");

    public FindPurchaseInfosUseCase(ISefazGateway sefazGateway, IBrazilApiGateway brazilApiGateway)
    {
        _sefazGateway = sefazGateway;
        _brazilApiGateway = brazilApiGateway;
    }

    public async Task<Buy> ExecuteAsync(FindPurchaseInfosUseCaseInput input, CancellationToken cancellationToken)
    {
        ValidateInput(input);
        var buy = await _sefazGateway.FindPurchaseInfos(input.Url, cancellationToken);
        var enterprise = await _brazilApiGateway.FindEnterpriseByCNPJAsync(buy.Market.CNPJ, cancellationToken);
        return buy with
        {
            Market = buy.Market with { FantasyName = enterprise?.FantasyName ?? string.Empty }
        };
    }

    private void ValidateInput(FindPurchaseInfosUseCaseInput input)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(input.Url))
        {
            errors.Add("URL é obrigatoria");
        }
        var uri = new Uri(input.Url);
        if (!uri.Host.Contains(".sefaz", StringComparison.OrdinalIgnoreCase))
        {
            errors.Add("A URL deve ser do site da sefaz");
        }
        if (!input.Url.Contains("?p="))
        {
            errors.Add("URL deve conter na query string o caractere ?p=");
        }
        if (errors.Any())
        {
            throw new InvalidInputException(errors: errors);
        }
    }
}
