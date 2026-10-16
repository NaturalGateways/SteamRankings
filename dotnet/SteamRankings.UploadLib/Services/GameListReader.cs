using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.UploadLib
{
    public static class GameListReader
    {
        public static DynamoModel.FactGamesList ReadGamesList(string dirpath, string extractFilename, string overidesFilename)
        {
            // Read games
            JsonModel.GameListExtractItem[] gamesExtractArray = ReadGamesExtract(dirpath, extractFilename);
            JsonModel.ManualOverrides overrides = ReadOverrides(dirpath, overidesFilename);

            // Create blacklist hashset
            HashSet<int> blacklistHashset = GetBlacklistHashset(overrides);

            // Create list of games
            DynamoModel.FactGamesListItem[] gameItemArray = gamesExtractArray.Select(x => GetBlacklistHashset(x, blacklistHashset)).Where(x => x != null).ToArray();

            // Return
            return new DynamoModel.FactGamesList
            {
                Items = gameItemArray
            };
        }

        private static JsonModel.GameListExtractItem[] ReadGamesExtract(string dirpath, string extractFilename)
        {
            string filepath = Path.Join(dirpath, extractFilename);
            string fileContents = File.ReadAllText(filepath);
            return Natural.Json.JsonHelper.DeserialiseObject<JsonModel.GameListExtractItem[]>(fileContents);
        }

        private static JsonModel.ManualOverrides ReadOverrides(string dirpath, string overidesFilename)
        {
            string filepath = Path.Join(dirpath, overidesFilename);
            string fileContents = File.ReadAllText(filepath);
            return Natural.Json.JsonHelper.DeserialiseObject<JsonModel.ManualOverrides>(fileContents);
        }

        private static HashSet<int> GetBlacklistHashset(JsonModel.ManualOverrides overrides)
        {
            List<int> blacklistIdList = new List<int>();
            blacklistIdList.AddRange(overrides.BlacklistIds);
            blacklistIdList.AddRange(overrides.Exclusions.SelectMany(x => x.Ids));
            blacklistIdList.AddRange(overrides.Dependencies.SelectMany(x => x.Skip(1)));
            return blacklistIdList.Distinct().ToHashSet();
        }

        private static DynamoModel.FactGamesListItem GetBlacklistHashset(JsonModel.GameListExtractItem jsonItem, HashSet<int> blacklistHashset)
        {
            // Parse the ID
            int idInt = Int32.Parse(jsonItem.id);

            // Check black list
            if (blacklistHashset.Contains(idInt))
            {
                return null;
            }

            // Return
            return new DynamoModel.FactGamesListItem
            {
                GameID = idInt,
                GameName = jsonItem.name
            };
        }
    }
}
