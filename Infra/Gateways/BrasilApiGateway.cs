using Application.Gateways;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Infra.Gateways;
public class BrasilApiGateway : IBrasilApiGateway
{

    private sealed class BrasilApiEnterpriseResponse
    {
        [JsonPropertyName("nome_fantasia")]
        public string FantasyName { get; set; } = string.Empty;
    }
    private readonly IHttpClientFactory httpClientFactory;

    public BrasilApiGateway(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<Enterprise?> FindEnterpriseByCNPJAsync(string cnpj, CancellationToken cancellationToken)
    {
        var cnpjOnlyNumbers = Regex.Replace(cnpj, "[^0-9]", "");
        var client = httpClientFactory.CreateClient("brasilAPI");
        var response = await client.GetFromJsonAsync<BrasilApiEnterpriseResponse>($"/api/cnpj/v1/{cnpjOnlyNumbers}", cancellationToken);
        return response is null ? null : new Enterprise(FantasyName: response.FantasyName);
    }
}
