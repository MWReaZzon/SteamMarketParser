using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ConsoleSteamMarketParser
{
    public class MarketplaceItem
    {
        public static string BaseURL = @"https://steamcommunity.com/market/listings/";
        public MarketplaceItem(
            string name, 
            int gameID, 
            string code, 
            string colorStr, 
            int amount, 
            double price, 
            char priceCurrency
            )
        {
            Name = name;
            GameID = gameID;
            Code = code;
            ColorStr = colorStr;
            Amount = amount;
            Price = price;
            PriceCurrency = priceCurrency;
        }

        public string Name { get; set; }
        public int GameID { get; set; }
        public string Code { get; set; }
        private string colorStr;
        public int ColorHex { get; private set; }
        public Color Color { get; private set; }
        public string ColorStr
        {
            get
            {
                return "#" + colorStr;
            }
            set
            {
                colorStr = value;
                ColorHex = Convert.ToInt32(colorStr, 16);
                Color = Color.FromArgb(ColorHex);
            }
        }
        public int Amount { get; set; }
        public double Price { get; set; }
        public char PriceCurrency { get; set; }
        public string Link 
        {
            get
            {
                return BaseURL + GameID + "/" + Name.Replace('/', '-');
            } 
        }
    }
}
