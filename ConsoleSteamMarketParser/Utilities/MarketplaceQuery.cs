using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSteamMarketParser
{
    public class MarketplaceQuery
    {
        public MarketplaceQuery(string baseUri, Dictionary<string, string> paramsDict = null)
        {
            BaseUri = baseUri;
            ParamsDict = paramsDict;
        }

        public string BaseUri { get; private set; }
        public Dictionary<string, string> ParamsDict { get; }

        public Uri Uri {
            get {

                if (!ParamsDict.ContainsKey("appid"))
                {
                    throw new GameNotSpecifiedException();
                }

                MarketplaceQueryBuilder builder = new MarketplaceQueryBuilder(BaseUri);
                builder.AddParam(ParamsDict);
                return builder.Uri;
            }
        }

        public string GetParam(string key)
        {
            return ParamsDict[key];
        }

        public int GetParamInt(string key)
        {
            return Convert.ToInt32(ParamsDict[key]);
        }

        public void AddParam(string name, string value)
        {
            ParamsDict[name] = value;
        }

        public void AddParam(string name, int value)
        {
            ParamsDict[name] = Convert.ToString(value);
        }

        public bool ContainsParam(string name)
        {
            return ParamsDict.ContainsKey(name);
        }
    }

    internal class MarketplaceQueryBuilder : UriBuilder
    {
        internal MarketplaceQueryBuilder() : base(@"https://steamcommunity.com/market/search/render/") { }
        internal MarketplaceQueryBuilder(string baseUrl) : base(baseUrl) { }

        internal string CreateParam(string name, string value)
        {
            return "&" + name + "=" + value;
        }
        internal void AddParam(string name, string value)
        {
            AddParam(CreateParam(name, value));
        }
        public void AddParam(string param)
        {
            if (Query == null || Query == "")
                Query = "query=";
            Query += param;
        }

        public void AddParam(IDictionary<string, string> paramDict)
        {
            if (paramDict is null) return;

            foreach (KeyValuePair<string, string> pair in paramDict)
            {
                AddParam(pair.Key, pair.Value);
            }
        }
    }

    internal class GameNotSpecifiedException : Exception {
        public GameNotSpecifiedException()
            : base("For proper query results \"appid\" should be specified in paramethers dictionary")
        {

        }
    }
}
