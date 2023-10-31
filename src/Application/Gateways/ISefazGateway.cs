namespace Application.Gateways;
public interface ISefazGateway
{
    Task<Buy> FindPurchaseInfos(string URL, CancellationToken cancellationToken);
}

public record Buy(string URL, Market Market, IEnumerable<Item> Items);

public record Market(string Name, string CNPJ, string Address, string FantasyName);

public record Item(string Name, decimal Quantity, string Unit, decimal UnitPrice, decimal TotalPrice);