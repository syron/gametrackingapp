using Microsoft.WindowsAzure.Storage.Table;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingis.DAL
{
    public class GameModes : DALBase
    {
        CloudTable table = null;

        public GameModes() : base()
        {
            table = tableClient.GetTableReference("GameModes");
            table.CreateIfNotExists();
        }
        
        public void Create(GameModeEntity entity)
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            table.Execute(insertOperation);
        }

        public void Delete(GameModeEntity entity)
        {
            TableOperation deleteOperation = TableOperation.Delete(user);

            table.Execute(deleteOperation);
        }

        public void Update(GameModeEntity entity)
        {
            TableOperation updateOperation = TableOperation.InsertOrReplace(user);
            table.Execute(updateOperation);
        }
        
        public List<GameModeEntity> GetAll()
        {
            TableQuery<GameModeEntity> query = new TableQuery<GameModeEntity>();
            return table.ExecuteQuery(query).ToList();
        }
        
        public GameModeEntity GetByGameModeId(string gameModeId)
        {
            TableQuery<GameModeEntity> query = new TableQuery<GameModeEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, gameModeId));
            return table.ExecuteQuery(query).FirstOrDefault();
        }
    }
}
