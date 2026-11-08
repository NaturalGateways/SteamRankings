using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.Console
{
    public static class ApiHelperFunctions
    {
        public static async Task<List<DynamoModel.FactGamesListItem>> PickLineupItemsAsync(Natural.Aws.IAwsService awsService)
        {
            object responseObject = await ApiServices.ApiService.HandleRequestAsync(awsService, new ApiDtos.ApiRequestDto
            {
                RequestType = ApiDtos.ApiRequestType.PickLineupItems
            });
            return (List<DynamoModel.FactGamesListItem>)responseObject;
        }

        public static async Task<ApiDtos.FetchNextItemResponseDto> FetchNextItemAsync(Natural.Aws.IAwsService awsService)
        {
            object responseObject = await ApiServices.ApiService.HandleRequestAsync(awsService, new ApiDtos.ApiRequestDto
            {
                RequestType = ApiDtos.ApiRequestType.FetchNextItem
            });
            return (ApiDtos.FetchNextItemResponseDto)responseObject;
        }

        public static async Task<DynamoModel.FactGamesListItem> RankItemAsync(Natural.Aws.IAwsService awsService, int gameId, int streamIndex, int rating)
        {
            ApiDtos.RankItemRequestDto requestPayload = new ApiDtos.RankItemRequestDto
            {
                GameId = gameId,
                StreamIndex = streamIndex,
                Rating = rating
            };
            object responseObject = await ApiServices.ApiService.HandleRequestAsync(awsService, new ApiDtos.ApiRequestDto
            {
                RequestType = ApiDtos.ApiRequestType.RankItem,
                PayloadJson = Natural.Json.JsonHelper.SerialiseObject(requestPayload)
            });
            return (DynamoModel.FactGamesListItem)responseObject;
        }

        public static async Task<DynamoModel.DimRankedList> FetchRankedListAsync(Natural.Aws.IAwsService awsService)
        {
            object responseObject = await ApiServices.ApiService.HandleRequestAsync(awsService, new ApiDtos.ApiRequestDto
            {
                RequestType = ApiDtos.ApiRequestType.FetchRankedList
            });
            return (DynamoModel.DimRankedList)responseObject;
        }
    }
}
