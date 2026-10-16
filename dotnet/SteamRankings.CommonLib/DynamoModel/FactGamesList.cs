using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.DynamoModel
{
    public class FactGamesList
    {
        public FactGamesListItem[] Items { get; set; }
    }

    public class FactGamesListItem
    {
        public int GameID { get; set; }

        public string GameName { get; set; }
    }
}
