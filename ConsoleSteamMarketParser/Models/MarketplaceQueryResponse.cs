using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace ConsoleSteamMarketParser
{
    internal class MarketplaceQueryResponse
    {
        public int total_count { get; set; }
        public List<MarketplaceQueryItem> results { get; set; }

        public void AddToMarketplaceList(MarketplaceList list)
        {
            MarketplaceList newItemsList = new MarketplaceList();

            foreach(MarketplaceQueryItem queryItem in results)
            {
                newItemsList.Add(queryItem.ConvertToItem());
            }
            list.AddRange(newItemsList);
        }
    }
    public class MarketplaceQueryItem
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
        public int currency { get; set; }
        public AssetDescription asset_description { get; set; }

        //T: Convert to explicit operator
        public MarketplaceItem ConvertToItem()
        {
            var currency = sell_price_text.Except((sell_price / 100.0).ToString() + ' ').FirstOrDefault();
            MarketplaceItem item = new MarketplaceItem(
                name,
                name.Substring(name.Length-6),
                asset_description?.name_color,
                sell_listings,
                sell_price / 100.0,
                currency
                );
            return item;
        }
    }

    public class AssetDescription
    {
        public int appid { get; set; }
        public string name_color { get; set; }
        public List<ItemDescription> descriptions { get; set; }
    }

    public class ItemDescription
    {
        public string type { get; set; }
        public string value { get; set; }
    }
}
