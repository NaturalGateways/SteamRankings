using Natural.Aws.DynamoDB;

namespace SteamRankings.UploadLib
{
    public static class UploadService
    {
        public static async Task UploadGameListAsync(UploadParameters parameters)
        {
            DynamoModel.FactGamesList gamesList = GameListReader.ReadGamesList(parameters.Dirpath, parameters.ExtractFilename, parameters.OverridesFilepath);

            using (Natural.Aws.IAwsService awsService = new Natural.Aws.LambdaAwsService(parameters.AwsCredentials))
            {
                IDynamoTable table = awsService.DynamoService.GetTable("BespokeSteamRankings", "PartId", "SortId");
                await table.PutItemAsync("Fact", "GameList", new ItemUpdate
                {
                    ObjectAttributes = new Dictionary<string, object>
                    {
                        { "DataJson", gamesList }
                    }
                });
            }
        }
    }
}
