using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSteamMarketParser
{
    class MarketplaceQueryBuilder : UriBuilder
    {
        public MarketplaceQueryBuilder() : base(@"https://steamcommunity.com/market/search/render/") {}

        public string CreateParam(string name, string value)
        {
            return "&" + name + "=" + value;
        }
        public void AddParam(string name, string value)
        {
            AddParam(CreateParam(name, value));
        }
        public void AddParam(string param)
        {
            if (Query == null || Query == "")
                Query = "query=" + param;
            Query += param;
        }

        public void AddParam(IDictionary<string, string> paramDict)
        {
            foreach(KeyValuePair<string, string> pair in paramDict)
            {
                AddParam(pair.Key, pair.Value);
            }
        }
    }
}
