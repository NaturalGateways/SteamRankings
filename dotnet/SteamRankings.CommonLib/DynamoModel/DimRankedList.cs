using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.DynamoModel
{
    public class DimRankedList
    {
        public DimRankedListItem[] Items { get; set; }
    }

    public class DimRankedListItem
    {
        public int GameID { get; set; }

        public string GameName { get; set; }

        public int StreamIndex { get; set; }

        public DateTime RankDateTimeUtc { get; set; }

        public int Rating { get; set; }
    }
}
