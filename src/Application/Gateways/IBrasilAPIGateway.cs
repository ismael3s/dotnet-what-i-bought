namespace Application.Gateways;
public interface IBrasilApiGateway
{
    Task<Enterprise?> FindEnterpriseByCNPJAsync(string cnpj, CancellationToken cancellationToken);
}


public record Enterprise(string FantasyName);