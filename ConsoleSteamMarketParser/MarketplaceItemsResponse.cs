using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace ConsoleSteamMarketParser
{
    class MarketplaceQuerryResponse
    {
        public List<MarketplaceItemsResponse> results { get; set; }
    }
    class MarketplaceItemsResponse
    {
        public string name { get; set; }
        public string url { get
            {
                return "https://steamcommunity.com/market/listings/582810/" + name;
            } }
        public string sale_price_text { get; set; }
        public int sell_listings { get; set; }
        public int sell_price { get; set; }
        public string sell_price_text { get; set; }
        public AssetDescription asset_description { get; set; }
    }

    class AssetDescription
    {
        public string name_color { get; set; }
        public List<ItemDescription> descriptions { get; set; }

    }

    class ItemDescription
    {
        public string type { get; set; }
        public string value { get; set; }
    }
}
