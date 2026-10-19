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

        public int? PickedIndex { get; set; }

        public FactGamesListItemRanked Ranked { get; set; }
    }

    public class FactGamesListItemRanked
    {
        public int StreamIndex { get; set; }

        public DateTime RankDateTimeUtc { get; set; }

        public int Rating { get; set; }
    }
}
