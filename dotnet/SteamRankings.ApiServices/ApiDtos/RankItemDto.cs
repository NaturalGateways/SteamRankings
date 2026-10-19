using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.ApiServices.ApiDtos
{
    public class RankItemRequestDto
    {
        public int GameId { get; set; }

        public int StreamIndex { get; set; }

        public int Rating { get; set; }
    }
}
