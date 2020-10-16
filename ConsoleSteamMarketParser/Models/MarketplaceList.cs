using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsoleSteamMarketParser
{
    public class MarketplaceList : List<MarketplaceItem>
    {
        public readonly int? GameID;
        public MarketplaceList(int? gameID = null)
        {
            GameID = gameID;
        }
    }
}
