using Amazon.S3.Model.Internal.MarshallTransformations;
using SteamRankings.DynamoModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.ApiServices
{
    public static class ApiService
    {
        public static async Task<object> HandleRequestAsync(Natural.Aws.IAwsService awsService, ApiDtos.ApiRequestDto request)
        {
            // Get table
            Natural.Aws.DynamoDB.IDynamoTable dataTable = awsService.DynamoService.GetTable("BespokeSteamRankings", "PartId", "SortId");

            // Handle request
            switch (request.RequestType)
            {
                case ApiDtos.ApiRequestType.PickLineupItems:
                    return await HandlePickLineupItemsRequestAsync(dataTable);
                case ApiDtos.ApiRequestType.FetchNextItem:
                    return await HandleFetchNextItemRequestAsync(dataTable);
                case ApiDtos.ApiRequestType.RankItem:
                    return await HandleRankItemRequestAsync(dataTable, request);
                default:
                    throw new Exception("Unredognised request type.");
            }
        }

        private static async Task<object> HandlePickLineupItemsRequestAsync(Natural.Aws.DynamoDB.IDynamoTable dataTable)
        {
            const int LINEUP_SIZE = 5;

            // Get games list form database
            DynamoModel.FactGamesList gamesList = await GameListReader.ReadGamesListAsync(dataTable);

            // Get picked items
            List<DynamoModel.FactGamesListItem> pickedItemList = gamesList.Items.Where(x => x.PickedIndex.HasValue && x.Ranked == null).OrderBy(x => x.PickedIndex.Value).ToList();

            // Pick more items if needed
            if (pickedItemList.Count < LINEUP_SIZE)
            {
                List<DynamoModel.FactGamesListItem> availableItemList = gamesList.Items.Where(x => x.PickedIndex.HasValue == false && x.Ranked == null).ToList();
                while (pickedItemList.Count < LINEUP_SIZE)
                {
                    int pickedIndex = Random.Shared.Next(availableItemList.Count);
                    pickedItemList.Add(availableItemList[pickedIndex]);
                    availableItemList.RemoveAt(pickedIndex);
                }
            }

            // Wipe sequence
            foreach (FactGamesListItem listItem in gamesList.Items)
            {
                listItem.PickedIndex = null;
            }
            // Restore sequence
            for (int itemIndex = 0; itemIndex != pickedItemList.Count; ++itemIndex)
            {
                pickedItemList[itemIndex].PickedIndex = itemIndex;
            }

            // Store new list
            await dataTable.PutItemAsync("Fact", "GameList", new Natural.Aws.DynamoDB.ItemUpdate
            {
                ObjectAttributes = new Dictionary<string, object>
                {
                    { "DataJson", gamesList }
                }
            });

            // Return the new picked list
            return pickedItemList;
        }

        private static async Task<object> HandleFetchNextItemRequestAsync(Natural.Aws.DynamoDB.IDynamoTable dataTable)
        {
            const int SPOOL_SIZE = 100;

            // Get games list form database
            DynamoModel.FactGamesList gamesList = await GameListReader.ReadGamesListAsync(dataTable);

            // Get the picked item
            DynamoModel.FactGamesListItem pickedListItem = gamesList.Items.Where(x => x.PickedIndex.HasValue && x.Ranked == null).OrderBy(x => x.PickedIndex.Value).FirstOrDefault();
            if (pickedListItem == null)
            {
                throw new Exception("No pickable items left.");
            }
            int pickedIndex = pickedListItem.PickedIndex.Value;

            // Create the spool
            List<DynamoModel.FactGamesListItem> availableItemList = gamesList.Items.Where(x => x.PickedIndex.HasValue == false || pickedIndex < x.PickedIndex.Value).ToList();
            List<ApiDtos.FetchNextItemResponseDtoItem> spoolItemList = new List<ApiDtos.FetchNextItemResponseDtoItem>();
            for (int spoolIndex = 0; spoolIndex != SPOOL_SIZE; ++spoolIndex)
            {
                int pickedSpoolIndex = Random.Shared.Next(availableItemList.Count);
                spoolItemList.Add(new ApiDtos.FetchNextItemResponseDtoItem(availableItemList[pickedSpoolIndex]));
                availableItemList.RemoveAt(pickedSpoolIndex);
            }

            // Return
            return new ApiDtos.FetchNextItemResponseDto
            {
                PickedGame = new ApiDtos.FetchNextItemResponseDtoItem(pickedListItem),
                AvailableGameSpool = spoolItemList.ToArray()
            };
        }

        private static async Task<object> HandleRankItemRequestAsync(Natural.Aws.DynamoDB.IDynamoTable dataTable, ApiDtos.ApiRequestDto request)
        {
            // Get payload
            if (string.IsNullOrEmpty(request.PayloadJson))
                throw new Exception("A rank item request must have a payload.");
            ApiDtos.RankItemRequestDto payloadDto = Natural.Json.JsonHelper.DeserialiseObject<ApiDtos.RankItemRequestDto>(request.PayloadJson);
            if (payloadDto == null)
                throw new Exception("A rank item request must have a not-null payload.");
            if (payloadDto.Rating < 0 || 5 < payloadDto.Rating)
                throw new Exception("A ranking must be between 0 and 5 inclusively.");

            // Get games list form database
            DynamoModel.FactGamesList gamesList = await GameListReader.ReadGamesListAsync(dataTable);

            // Get game
            FactGamesListItem rankedGame = gamesList.Items.FirstOrDefault(x => x.GameID == payloadDto.GameId);
            if (rankedGame == null)
                throw new Exception($"Cannot rank game with ID '{payloadDto.GameId}' since it doesn't exist.");
            if (rankedGame.Ranked != null)
                throw new Exception($"Cannot rank game with ID '{payloadDto.GameId}' since it is already ranked.");

            // Set the rank
            rankedGame.Ranked = new FactGamesListItemRanked
            {
                StreamIndex = payloadDto.StreamIndex,
                RankDateTimeUtc = DateTime.UtcNow,
                Rating = payloadDto.Rating
            };

            // Store new list
            await dataTable.PutItemAsync("Fact", "GameList", new Natural.Aws.DynamoDB.ItemUpdate
            {
                ObjectAttributes = new Dictionary<string, object>
                {
                    { "DataJson", gamesList }
                }
            });

            // Return
            return rankedGame;
        }
    }
}
