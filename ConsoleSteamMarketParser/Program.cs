using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Dynamic;
using System.Reflection.Emit;
using ClosedXML.Excel;
using System.IO;

namespace ConsoleSteamMarketParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string link = "https://steamcommunity.com/market/search/render/" +
                "?query=&start=0" +
                "&count=100" +
                "&currency=2" +
                "&norender=1" +
                "&search_descriptions=1" +
                "&sort_column=price" +
                "&sort_dir=asc" +
                "&appid=582810";
            Console.WriteLine(link);
            dynamic responseExpando;
            dynamic mpItemExpando;
            dynamic mpItemDescription;
            MarketplaceQuerryResponse response;
            try
            {
                client.DefaultRequestHeaders.Add("Cookie", "steamLoginSecure=76561198118722085%7C%7CE9851F9BF37D3C77410335144DDBA92ECFF2AF11");
                string responseBody = await client.GetStringAsync(link);
                responseExpando = JsonSerializer.Deserialize<ExpandoObject>(responseBody);
                mpItemExpando = JsonSerializer.Deserialize<ExpandoObject>(responseExpando.results[0].GetRawText());
                mpItemDescription = JsonSerializer.Deserialize<ExpandoObject>(mpItemExpando.asset_description.GetRawText());
                response = JsonSerializer.Deserialize<MarketplaceQuerryResponse>(responseBody);
                //Console.WriteLine(response.ToString());
                Console.WriteLine(responseBody);

                XLWorkbook workbook = new XLWorkbook();
                IXLWorksheet worksheet = workbook.Worksheets.Add("Coins");
                worksheet.Cell(1, 1).Value = "Link";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Code";
                worksheet.Cell(1, 4).Value = "Color";
                worksheet.Cell(1, 5).Value = "Amount";
                worksheet.Cell(1, 6).Value = "Price";
                worksheet.Cells(true).Style.Font.SetBold();

                for (int i = 0; i < response.results.Count; i++)
                {
                    MarketplaceItemsResponse item = response.results[i];

                    worksheet.Cell(i + 2, 1).Hyperlink = new XLHyperlink(@item.url);
                    worksheet.Cell(i + 2, 1).Value = item.url;
                    worksheet.Cell(i + 2, 2).Value = item.name;
                    worksheet.Cell(i + 2, 3).Value = item.asset_description.descriptions[0].value;
                    worksheet.Cell(i + 2, 4).Value = "#" + item.asset_description.name_color;
                    worksheet.Cell(i + 2, 4).Style.Fill.BackgroundColor = XLColor.FromArgb(0xFFFFFF);
                    worksheet.Cell(i + 2, 5).Value = item.sell_listings;
                    worksheet.Cell(i + 2, 6).Value = item.sell_price_text;
                }

                workbook.SaveAs("d:\\file.xlsx");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            

            Console.ReadKey();
        }
    }
}
