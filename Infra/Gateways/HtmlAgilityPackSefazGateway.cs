using Application.Gateways;
using HtmlAgilityPack;


namespace Infra.Gateways;
public class HtmlAgilityPackSefazGateway : ISefazGateway
{
    public async Task<Buy> FindPurchaseInfos(string URL)
    {
        var htmlWeb = new HtmlWeb();
        //TODO: make a HTTP  Request using HttpClient get the response and load it into the HtmlDocument

        //var doc = await htmlWeb.LoadFromWebAsync(URL);
        //List<Item> items = doc.DocumentNode.SelectNodes("//tr/td[1]")
        //  .Select((element) =>
        //  {
        //      var totalPrice = ExtractStringAsDecimal(element.ParentNode.SelectSingleNode("td[2]/span").InnerText);
        //      var itemName = element.SelectSingleNode("span[@class='txtTit']").InnerText?.Trim() ?? "";
        //      var unit = element.SelectSingleNode("span[@class='RUN']").InnerText
        //        ?.Split("UN: ")
        //        ?.ElementAt(1)
        //        ?.Trim();
        //      var quantity = ExtractStringAsDecimal(
        //          element.SelectSingleNode("span[@class='Rqtd']").InnerText
        //          , ":");
        //      var price = ExtractStringAsDecimal(
        //          element.SelectSingleNode("span[@class='RvlUnit']").InnerText,
        //        ":");
        //      var item = new Item(Name: itemName, Unit: unit!, Quantity: quantity, UnitPrice: price, TotalPrice: totalPrice);
        //      return item;
        //  })
        //  .ToList();
        //Market market = ExtractMarket(doc);
        var buy = new Buy(URL: URL, Market: new Market(Name: "Mock", CNPJ: "23.306.904/0001-45", Address: "teste", FantasyName: "Mock"), Items: new List<Item>());
        return buy;
    }

    internal Market ExtractMarket(HtmlDocument doc)
    {
        var htmlMarketInfos = doc.DocumentNode.SelectNodes("//div[@id='conteudo']/div[@class='txtCenter']")
            .Descendants("div")
            .ToList();
        var marketName = htmlMarketInfos.ElementAt(0).InnerText.Trim();
        var marketCNPJ = htmlMarketInfos.ElementAt(1).InnerText?.Split(":")?.ElementAt(1)?.Trim() ?? string.Empty;
        var marketAddress = htmlMarketInfos.ElementAt(2).InnerText?.Replace("\r\n\t\t", " ") ?? string.Empty;
        var market = new Market(Name: marketName, CNPJ: marketCNPJ, Address: marketAddress, FantasyName: "");
        return market;
    }

    internal decimal ExtractStringAsDecimal(string? str)
    {
        var rawString = str?.Replace(",", ".")?.Trim() ?? string.Empty;
        _ = decimal.TryParse(rawString, out var result);
        return result;
    }

    internal decimal ExtractStringAsDecimal(string? str, string separator)
    {
        var rawString = str?.Split(separator)
          ?.ElementAt(1);
        return ExtractStringAsDecimal(rawString);
    }




}
