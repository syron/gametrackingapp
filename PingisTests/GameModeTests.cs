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
            Matches matches = new Matches();
            // get all matches, loop through matches and delete...
            var playedMatches = matches.GetAll();
            if (playedMatches != null && playedMatches.Count > 0)
            {
                foreach (var match in playedMatches)
                {
                    matches.Delete(match);
                }
            }
        }

        [TestMethod]
        public void GameModesTable_Create()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Matches");
            table.CreateIfNotExists();

            // insert
            var matchGuid = Guid.NewGuid();
            MatchEntity entity = new MatchEntity(matchGuid, "ROMA", "RORO");
            
            TableOperation insertOperation = TableOperation.Insert(entity);
            table.Execute(insertOperation);

            // get
            TableQuery<MatchEntity> query = new TableQuery<MatchEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, matchGuid.ToString()));

            var ent = table.ExecuteQuery(query).First();

            // delete
            TableOperation deleteOperation = TableOperation.Delete(ent);
            
            table.Execute(deleteOperation);
        }
    }
}
