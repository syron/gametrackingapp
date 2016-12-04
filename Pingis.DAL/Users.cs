using Microsoft.WindowsAzure.Storage.Table;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingis.DAL
{
    public class Users : DALBase
    {
        CloudTable table = null;

        public Users() : base()
        {
            table = tableClient.GetTableReference("Users");
            table.CreateIfNotExists();
        }

        public List<UserEntity> GetAll()
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>();
            return table.ExecuteQuery(query).ToList();
        }

        public void Register(UserEntity user)
        {
            TableOperation insertOperation = TableOperation.Insert(user);
            table.Execute(insertOperation);
        }
        
        public UserEntity GetByDisplayName(string displayName)
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, displayName));
            return table.ExecuteQuery(query).FirstOrDefault();
        }

        public UserEntity GetByUserId(string userId)
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            return table.ExecuteQuery(query).FirstOrDefault();
        }

        public void Unregister(UserEntity user)
        {
            TableOperation deleteOperation = TableOperation.Delete(user);

            table.Execute(deleteOperation);
        }

        public void Update(UserEntity user)
        {
            TableOperation updateOperation = TableOperation.InsertOrReplace(user);
            table.Execute(updateOperation);
        }

    }
}
