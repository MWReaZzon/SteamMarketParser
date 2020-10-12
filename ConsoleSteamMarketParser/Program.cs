using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Dynamic;
using ClosedXML.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ConsoleSteamMarketParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string baseUri = "https://steamcommunity.com/market/search/render/";
            Dictionary<string, string> paramsDict = new Dictionary<string, string>()
            {
                { "norender", "1" },
                { "appid", "582810" },
                //{ "sort_column", "name" },
                //{ "sort_dir", "asc" }

            };
            MarketplaceQuery query = new MarketplaceQuery(baseUri, paramsDict);

            SMParser.AddCookiesHeader("steamLoginSecure=76561198118722085%7C%7C8F6A4223DB75718FDF3C63350947C04ADD410F05");

            await SMParser.ParseToExcel(query);

            Console.WriteLine("\n Done! Press any key to close the console");
            Console.ReadKey();
        }
    }
}
