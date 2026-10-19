using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.ApiServices.ApiDtos
{
    public enum ApiRequestType
    {
        PickLineupItems,
        FetchNextItem,
        RankItem
    }

    public class ApiRequestDto
    {
        public ApiRequestType RequestType { get; set; }

        public string PayloadJson { get; set; }
    }
}
