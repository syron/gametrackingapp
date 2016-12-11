using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Table;
using Pingis.Models;
using Pingis.DAL;

namespace PingisTests
{
    [TestClass]
    public class GameModeTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            GameModes gameModes = new GameModes();
            // get all matches, loop through matches and delete...
            var availableGameModes = gameModes.GetAll();
            if (availableGameModes != null && availableGameModes.Count > 0)
            {
                foreach (var gameMode in availableGameModes)
                {
                    gameModes.Delete(gameMode);
                }
            }
        }

        [TestMethod]
        public void GameModesTable_Create()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("GameModes");
            table.CreateIfNotExists();

            // insert
            GameModeEntity entity = GameModeEntity.Create("Sponge", "Famous game of sponge ping pong.");
            
            TableOperation insertOperation = TableOperation.Insert(entity);
            table.Execute(insertOperation);

            // get
            TableQuery<MatchEntity> query = new TableQuery<MatchEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, entity.GameModeId));

            var ent = table.ExecuteQuery(query).First();

            // delete
            TableOperation deleteOperation = TableOperation.Delete(ent);
            
            table.Execute(deleteOperation);
        }
    }
}
