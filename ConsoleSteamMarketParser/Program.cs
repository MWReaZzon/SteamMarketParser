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
            try
            {
            string baseUri = "https://steamcommunity.com/market/search/render/";
            Dictionary<string, string> paramsDict = new Dictionary<string, string>()
            {
                { "norender", "1" },
                { "appid", "582810" },
                { "sort_column", "name" },
                { "sort_dir", "asc" },
            };
            MarketplaceQuery query = new MarketplaceQuery(baseUri, paramsDict);

            //SMParser.AddCookiesHeader("steamLoginSecure=76561198118722085%7C%7CD54F537019294E081A349BCA6DBF130B11695A67");

            await SMParser.ParseToExcel(query, true);
            } catch(Exception e)
            {
                Console.WriteLine("Exception occured! More info:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
            }
        }
    }
}