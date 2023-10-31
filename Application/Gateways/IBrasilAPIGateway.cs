namespace Application.Gateways;
public interface IBrasilApiGateway
{
    Task<Enterprise?> FindEnterpriseByCNPJAsync(string cnpj);
}


public record Enterprise(string FantasyName);