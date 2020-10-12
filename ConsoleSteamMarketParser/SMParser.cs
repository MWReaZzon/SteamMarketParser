using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSteamMarketParser
{
    static class SMParser
    {

        private static HttpClient Client = new HttpClient();
        public static int Delay = 20000;
        public static string SaveFolder = "Coins\\";

        public static void AddCookiesHeader(string cookies)
        {
            Client.DefaultRequestHeaders.Add("Cookie", cookies);
        }

        private async static Task<MarketplaceList> ParseAllPages(MarketplaceQuery query)
        {
            MarketplaceList items = new MarketplaceList();

            query.AddParam("start", 0);
            query.AddParam("count", 100);

            string responseBody = await Client.GetStringAsync(query.Uri);
            MarketplaceQueryResponse response = JsonSerializer.Deserialize<MarketplaceQueryResponse>(responseBody);
            Console.Write("Query 1 sent. ");
            Console.WriteLine("Returned {0} items", response.results.Count);
            response.AddToMarketplaceList(items);

            int count = response.total_count;

            if(count > 100)
            {
                try
                {
                    for (int pos = 100; pos <= count; pos += 100)
                    {
                        Console.WriteLine("({0}/{1})", pos, count);
                        Thread.Sleep(Delay);

                        query.AddParam("start", pos);

                        Console.Write("Query {0} sent. ", pos / 100 + 1);
                        items.AddRange(await ParsePage(query));
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0}", e.Message);
                    Console.WriteLine("Saving {0} items", items.Count);
                }
            }

            return items;
            
        }
        
        private async static Task<MarketplaceList> ParsePage(MarketplaceQuery query)
        {
            MarketplaceList items = new MarketplaceList();
            string responseBody = await Client.GetStringAsync(query.Uri);
            MarketplaceQueryResponse response = JsonSerializer.Deserialize<MarketplaceQueryResponse>(responseBody);
            Console.WriteLine("Returned {0} items", response.results.Count);
            response.AddToMarketplaceList(items);
            return items;
        }

        public async static Task ParseToExcel(MarketplaceQuery query, bool open == false)
        {
            MarketplaceList list = await ParseAllPages(query);
            ObjToExcel(list, open);
        }

        public static void ObjToExcel(MarketplaceList items, bool open = false)
        {
            Console.WriteLine("Creating xlsx file. This can take a while.");
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Coins");
            worksheet.Cell(1, 1).Value = "Link";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Code";
            worksheet.Cell(1, 4).Value = "Color";
            worksheet.Cell(1, 5).Value = "Amount";
            worksheet.Cell(1, 6).Value = "Price";

            worksheet.Cells(true).Style.Font.SetBold();

            for (int i = 0; i < items.Count; i++)
            {
                MarketplaceItem item = items[i];

                worksheet.Cell(i + 2, 1).Hyperlink = new XLHyperlink(@item.Link);
                worksheet.Cell(i + 2, 1).Value = @item.Link;
                worksheet.Cell(i + 2, 2).Value = item.Name;
                worksheet.Cell(i + 2, 3).Value = item.Code;
                worksheet.Cell(i + 2, 4).Value = item.ColorStr;
                worksheet.Cell(i + 2, 4).Style.Fill.BackgroundColor = XLColor.FromArgb(item.ColorHex);
                worksheet.Cell(i + 2, 5).Value = item.Amount;
                worksheet.Cell(i + 2, 6).Value = item.Price;
                worksheet.Cell(i + 2, 6).Style.NumberFormat.Format = "0.00" + item.PriceCurrency;
            }

            worksheet.Columns(2, 6).AdjustToContents(10.0, 40.0);

            DateTime current = DateTime.Now;

            string saveLocation = SaveFolder + current.ToString("yyyy.MM.dd HH.mm.ss") + ".xlsx";

            Console.WriteLine("Saving your file to {0}", saveLocation);
            workbook.SaveAs(@saveLocation);

            if(open)
            {
                System.Diagnostics.Process.Start(saveLocation);
            }
        }
    }
}
