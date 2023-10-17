namespace Application.Gateways;
public interface IBrasilApiGateway
{
    Task<Enterprise?> FindEnterpriseByCNPJ(string cnpj);
}


public record Enterprise(string FantasyName);