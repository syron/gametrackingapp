using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pingis.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace Pingis.DAL
{
    public class Matches : DALBase
    {
        CloudTable table = null;

        public Matches() : base()
        {
            table = tableClient.GetTableReference("Matches");
            table.CreateIfNotExists();
        }
        
        public void RegisterMatch(MatchEntity match)
        {
            TableOperation insertOperation = TableOperation.Insert(match);
            var tableResult = table.Execute(insertOperation);
        }

        public MatchEntity GetMatchById(Guid matchId)
        {
            TableQuery<MatchEntity> query = new TableQuery<MatchEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, matchId.ToString()));
            return table.ExecuteQuery(query).First();
        }

        public List<MatchEntity> GetAll(MatchStatus? status = null)
        {
            TableQuery<MatchEntity> query = null;

            if (status == null)
            {
                query = new TableQuery<MatchEntity>();
            }
            else
            {
                query = new TableQuery<MatchEntity>().Where(TableQuery.GenerateFilterCondition("Status", QueryComparisons.Equal, status.Value.ToString()));
            }

            var matches = table.ExecuteQuery(query);
            if (matches == null)
                return null;

            return matches.ToList();
        }

        public void Update(MatchEntity match)
        {
            TableOperation updateOperation = TableOperation.InsertOrReplace(match);
            table.Execute(updateOperation);
        }

        public void Delete(MatchEntity match)
        {
            TableOperation deleteOperation = TableOperation.Delete(match);

            table.Execute(deleteOperation);
        }

        public void DeleteTable()
        {
            table.Delete();
        }

        public List<MatchEntity> GetMatchesByUserId(string userId)
        {
            List<MatchEntity> playedMatches = new List<MatchEntity>();

            TableQuery<MatchEntity> query = new TableQuery<MatchEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            var result = table.ExecuteQuery(query);

            if (result != null)
            {
                playedMatches.AddRange(result);
            }

            query = new TableQuery<MatchEntity>().Where(TableQuery.GenerateFilterCondition("OpponentId", QueryComparisons.Equal, userId));
            result = table.ExecuteQuery(query);
            
            if (result != null)
            {
                playedMatches.AddRange(result);
            }

            if (playedMatches.Count > 0)
                return playedMatches;

            return null;
        }
    }
}
