using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Dynamic;
using System.Reflection.Emit;
using ClosedXML.Excel;

namespace ConsoleSteamMarketParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string link = "https://steamcommunity.com/market/search/render/" +
                "?query=&start=0" +
                "&count=0" +
                "&currency=2" +
                "&norender=1" +
                "&search_descriptions=0" +
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
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Coins");
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "";

            Console.ReadKey();
        }
    }
}
