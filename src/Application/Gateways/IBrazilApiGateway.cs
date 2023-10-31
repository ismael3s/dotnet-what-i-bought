namespace Application.Gateways;
public interface IBrazilApiGateway
{
    Task<Enterprise?> FindEnterpriseByCNPJAsync(string cnpj, CancellationToken cancellationToken);
}


public record Enterprise(string FantasyName);