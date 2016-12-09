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

<<<<<<< HEAD
=======
        public void Create()
        {
            
        }

>>>>>>> master
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
