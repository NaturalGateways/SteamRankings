using System;
using System.Collections.Generic;
using System.Text;

using Natural.Aws.DynamoDB;

namespace SteamRankings.ApiServices
{
    public static class GameListDataAccess
    {
        /// <summary>Fetcher for the fact data.</summary>
        public static async Task<DynamoModel.FactGamesList> ReadGamesListAsync(IDynamoTable dataTable)
        {
            return await dataTable.GetItemValueAsNullableObjectAsync<DynamoModel.FactGamesList>("Fact", "GameList", "DataJson");
        }

        /// <summary>Fetcher for a dimension item.</summary>
        public static async Task<DynamoModel.DimRankedList> ReadRankedListAsync(IDynamoTable dataTable)
        {
            return await dataTable.GetItemValueAsNullableObjectAsync<DynamoModel.DimRankedList>("Dim", "RankedList", "DataJson");
        }

        /// <summary>Writes a game list to the data table, including dimension data.</summary>
        public static async Task WriteGameListAsync(IDynamoTable dataTable, DynamoModel.FactGamesList gamesList)
        {
            // Do dimension conversions
            DynamoModel.DimRankedList dimRankedList = CreateRankedList(gamesList);

            // Store data
            await dataTable.PutItemAsync("Fact", "GameList", new ItemUpdate
            {
                ObjectAttributes = new Dictionary<string, object>
                {
                    { "DataJson", gamesList }
                }
            });
            await dataTable.PutItemAsync("Dim", "RankedList", new ItemUpdate
            {
                ObjectAttributes = new Dictionary<string, object>
                {
                    { "DataJson", dimRankedList }
                }
            });
        }

        /// <summary>Creates a dimension ranked list from the fact list.</summary>
        private static DynamoModel.DimRankedList CreateRankedList(DynamoModel.FactGamesList gamesList)
        {
            return new DynamoModel.DimRankedList
            {
                Items = gamesList.Items.Where(x => x.Ranked != null).Select(ConvertFactItemToRankedListItem).ToArray()
            };
        }

        /// <summary>Creates a dimension ranked list from the fact list.</summary>
        private static DynamoModel.DimRankedListItem ConvertFactItemToRankedListItem(DynamoModel.FactGamesListItem factItem)
        {
            return new DynamoModel.DimRankedListItem
            {
                GameID = factItem.GameID,
                GameName = factItem.GameName,
                StreamIndex = factItem.Ranked.StreamIndex,
                RankDateTimeUtc = factItem.Ranked.RankDateTimeUtc,
                Rating = factItem.Ranked.Rating
            };
        }
    }
}
