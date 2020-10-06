using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace ConsoleSteamMarketParser
{
    class MarketplaceQueryResponse
    {
        public List<MarketplaceQueryItem> results { get; set; }

        public void AddToMarketplaceList(MarketplaceList list)
        {
            List<MarketplaceItem> newItemsList = new List<MarketplaceItem>();

            foreach(MarketplaceQueryItem queryItem in results)
            {
                newItemsList.Add(queryItem.ConvertToItem());
            }
            list.AddRange(newItemsList);
        }
    }
    class MarketplaceQueryItem
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

        public MarketplaceItem ConvertToItem()
        {
            MarketplaceItem item = new MarketplaceItem(
                name,
                asset_description.appid,
                asset_description.descriptions[0].value,
                asset_description.name_color,
                sell_listings,
                sell_price / 100.0,
                sell_price_text.Last()
                );
            return item;
        }
    }

    class AssetDescription
    {
        public int appid { get; set; }
        public string name_color { get; set; }
        public List<ItemDescription> descriptions { get; set; }

    }

    class ItemDescription
    {
        public string type { get; set; }
        public string value { get; set; }
    }
}
