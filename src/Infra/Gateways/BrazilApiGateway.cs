using Application.Gateways;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Infra.Gateways;
public class BrazilApiGateway : IBrasilApiGateway
{

    private sealed class BrazilApiEnterpriseResponse
    {
        [JsonPropertyName("nome_fantasia")]
        public string FantasyName { get; set; } = string.Empty;
    }
    private readonly IHttpClientFactory _httpClientFactory;

    public BrazilApiGateway(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Enterprise?> FindEnterpriseByCNPJAsync(string cnpj, CancellationToken cancellationToken)
    {
        var cnpjOnlyNumbers = Regex.Replace(cnpj, "[^0-9]", "");
        var client = _httpClientFactory.CreateClient("brasilAPI");
        var response = await client.GetFromJsonAsync<BrazilApiEnterpriseResponse>($"/api/cnpj/v1/{cnpjOnlyNumbers}", cancellationToken);
        return response is null ? null : new Enterprise(FantasyName: response.FantasyName);
    }
}
