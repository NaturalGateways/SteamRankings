using System;
using System.Collections.Generic;
using System.Text;

using Natural.Aws.DynamoDB;

namespace SteamRankings.ApiServices
{
    public static class GameListReader
    {
        public static async Task<DynamoModel.FactGamesList> ReadGamesListAsync(IDynamoTable dataTable)
        {
            DynamoModel.FactGamesList gamesList = await dataTable.GetItemValueAsNullableObjectAsync<DynamoModel.FactGamesList>("Fact", "GameList", "DataJson");
            return gamesList;
        }
    }
}
