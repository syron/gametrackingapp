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
    public class MatchTests
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
        public void MatchesTable_CRUD()
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

        [TestMethod]
        public void DAL_MatchesTable_CRUD()
        {
            Matches matches = new Matches();

            var matchGuid = Guid.NewGuid();
            MatchEntity entity = new MatchEntity(matchGuid, "ROMA", "RORO");

            matches.RegisterMatch(entity);

            var ent = matches.GetMatchById(matchGuid);

            matches.Delete(ent);
        }

        [TestMethod]
        public void DAL_MatchesTable_GetAll()
        {
            Matches matches = new Matches();

            var matchGuid = Guid.NewGuid();
            MatchEntity entity = new MatchEntity(matchGuid, "ROMA", "RORO");
            matches.RegisterMatch(entity);
            
            var matchGuid2 = Guid.NewGuid();
            entity = new MatchEntity(matchGuid2, "RORO", "ROMA");
            matches.RegisterMatch(entity);

            var matchGuid3 = Guid.NewGuid();
            entity = new MatchEntity(matchGuid3, "RORO", "ERAN");
            matches.RegisterMatch(entity);
            
            var playedMatches = matches.GetAll();
            Assert.AreEqual(3, playedMatches.Count);
            
            matches.Delete(matches.GetMatchById(matchGuid));
            matches.Delete(matches.GetMatchById(matchGuid2));
            matches.Delete(matches.GetMatchById(matchGuid3));
        }

        [TestMethod]
        public void DAL_MatchesTable_Update()
        {
            Matches matches = new Matches();

            var matchGuid = Guid.NewGuid();
            MatchEntity entity = new MatchEntity(matchGuid, "ROMA", "RORO");
            matches.RegisterMatch(entity);

            entity = matches.GetMatchById(matchGuid);
            entity.OpponentPoints = 2;
            entity.ChallengerPoints = 11;

            entity.Status = 1;
            
            matches.Update(entity);

            entity = matches.GetMatchById(matchGuid);

            Assert.AreEqual(2, entity.OpponentPoints);
            
            matches.Delete(matches.GetMatchById(matchGuid));
        }

        [TestMethod]
        public void DAL_MatchesTable_GetByUser()
        {
            Matches matches = new Matches();

            var matchGuid = Guid.NewGuid();
            MatchEntity entity = new MatchEntity(matchGuid, "ROMA", "RORO");
            var matchGuid1 = Guid.NewGuid();
            MatchEntity entity1 = new MatchEntity(matchGuid1, "RORO", "ROMA");
            var matchGuid2 = Guid.NewGuid();
            MatchEntity entity2 = new MatchEntity(matchGuid2, "ERAN", "JOOS");

            matches.RegisterMatch(entity);
            matches.RegisterMatch(entity1);
            matches.RegisterMatch(entity2);

            var mymatches = matches.GetMatchesByUserId("ROMA");
            Assert.AreEqual(2, mymatches.Count);

            matches.Delete(matches.GetMatchById(matchGuid));
            matches.Delete(matches.GetMatchById(matchGuid1));
            matches.Delete(matches.GetMatchById(matchGuid2));
        }
    }
}
