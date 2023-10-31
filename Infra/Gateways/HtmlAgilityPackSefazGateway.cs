using Application.Gateways;
using HtmlAgilityPack;


namespace Infra.Gateways;
public class HtmlAgilityPackSefazGateway : ISefazGateway
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HtmlAgilityPackSefazGateway(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Buy> FindPurchaseInfos(string URL, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(URL, cancellationToken);
        var html = await response.Content.ReadAsStringAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        List<Item> items = doc.DocumentNode.SelectNodes("//tr/td[1]")
         .Select((element) =>
         {
             var totalPrice = ExtractStringAsDecimal(element.ParentNode.SelectSingleNode("td[2]/span").InnerText);
             var itemName = element.SelectSingleNode("span[@class='txtTit']").InnerText?.Trim() ?? "";
             var unit = element.SelectSingleNode("span[@class='RUN']").InnerText
               ?.Split("UN: ")
               ?[1]
               ?.Trim();
             var quantity = ExtractStringAsDecimal(
                 element.SelectSingleNode("span[@class='Rqtd']").InnerText
                 , ":");
             var price = ExtractStringAsDecimal(
                 element.SelectSingleNode("span[@class='RvlUnit']").InnerText,
               ":");
             var item = new Item(Name: itemName, Unit: unit!, Quantity: quantity, UnitPrice: price, TotalPrice: totalPrice);
             return item;
         })
         .ToList();
        Market market = ExtractMarket(doc);
        var buy = new Buy(URL: URL, Market: market, Items: items);
        return buy;
    }

    private Market ExtractMarket(HtmlDocument doc)
    {
        var htmlMarketInfos = doc.DocumentNode.SelectNodes("//div[@id='conteudo']/div[@class='txtCenter']")
            .Descendants("div")
            .ToList();
        var marketName = htmlMarketInfos[0].InnerText.Trim();
        var marketCNPJ = htmlMarketInfos[1].InnerText?.Split(":")?[1]?.Trim() ?? string.Empty;
        var marketAddress = htmlMarketInfos[2].InnerText?.Replace("\r\n\t\t", " ") ?? string.Empty;
        var market = new Market(Name: marketName, CNPJ: marketCNPJ, Address: marketAddress, FantasyName: "");
        return market;
    }

    private decimal ExtractStringAsDecimal(string? str)
    {
        var rawString = str?.Replace(",", ".")?.Trim() ?? string.Empty;
        _ = decimal.TryParse(rawString, out var result);
        return result;
    }

    private decimal ExtractStringAsDecimal(string? str, string separator)
    {
        var rawString = str?.Split(separator)
          ?[1];
        return ExtractStringAsDecimal(rawString);
    }
}
