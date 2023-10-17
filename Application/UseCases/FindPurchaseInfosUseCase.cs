using Application.Gateways;

namespace Application.UseCases;

public class FindPurchaseInfosUseCase
{
    private readonly ISefazGateway _sefazGateway;
    private readonly IBrasilApiGateway _brasilApiGateway;
    public record FindPurchaseInfosUseCaseInput(string URL);

    public FindPurchaseInfosUseCase(ISefazGateway sefazGateway, IBrasilApiGateway brasilApiGateway)
    {
        _sefazGateway = sefazGateway;
        _brasilApiGateway = brasilApiGateway;
    }

    public async Task<Buy> ExecuteAsync(FindPurchaseInfosUseCaseInput input)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(input.URL))
        {
            errors.Add("URL é obrigatoria");
        }
        if (!input.URL?.Contains("?p=") ?? false)
        {
            errors.Add("URL deve conter na query string o caractere ?p=");
        }
        if (errors.Any())
        {
            throw new Exception(string.Join(Environment.NewLine, errors));
        }
        var buy = await _sefazGateway.FindPurchaseInfos(input.URL!);
        var enterprise = await _brasilApiGateway.FindEnterpriseByCNPJ(buy.Market.CNPJ);
        return buy with
        {
            Market = buy.Market with { FantasyName = enterprise?.FantasyName ?? string.Empty }
        };
    }
}
