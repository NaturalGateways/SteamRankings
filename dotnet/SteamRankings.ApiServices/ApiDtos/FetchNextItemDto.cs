using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.ApiServices.ApiDtos
{
    public class FetchNextItemResponseDto
    {
        public FetchNextItemResponseDtoItem PickedGame { get; set; }

        public FetchNextItemResponseDtoItem[] AvailableGameSpool { get; set; }
    }

    public class FetchNextItemResponseDtoItem
    {
        public int GameID { get; set; }

        public string GameName { get; set; }

        public FetchNextItemResponseDtoItem(DynamoModel.FactGamesListItem dynamoItem)
        {
            this.GameID = dynamoItem.GameID;
            this.GameName = dynamoItem.GameName;
        }
    }
}
