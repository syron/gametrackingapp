using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Pingis.Models;
using Pingis.DAL;

namespace PingisTests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void RegisterUserTest()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Users");
            table.CreateIfNotExists();

            // insert
            UserEntity entity = new UserEntity("ROMA", "Robert Mayer");

            TableOperation insertOperation = TableOperation.Insert(entity);
            table.Execute(insertOperation);

            // get
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "ROMA"));

            var ent = table.ExecuteQuery(query).First();

            // delete
            TableOperation deleteOperation = TableOperation.Delete(ent);

            table.Execute(deleteOperation);
        }

        [TestMethod]
        public void DAL_UsersTable_CRUD()
        {
            Users users = new Users();
            
            UserEntity entity = new UserEntity("ROMA", "Robert Mayer");
            users.Register(entity);
            
            var ent = users.GetByUserId("ROMA");

            users.Unregister(ent);
        }

        [TestMethod]
        public void DAL_UsersTable_GetAll()
        {
            Users users = new Users();

            UserEntity entity1 = new UserEntity("ROMA1", "Robert Mayer");
            users.Register(entity1);
            UserEntity entity2 = new UserEntity("ROMA2", "Robert Winston Mayer");
            users.Register(entity2);
            UserEntity entity3 = new UserEntity("ROMA3", "Robert Winston Leonard Mayer");
            users.Register(entity3);

            var allUserEntities = users.GetAll();
            Assert.IsTrue(3 <= allUserEntities.Count);

            users.Unregister(entity1);
            users.Unregister(entity2);
            users.Unregister(entity3);
        }

        [TestMethod]
        public void DAL_UsersTable_GetById()
        {
            Users users = new Users();

            UserEntity entity1 = new UserEntity("ROMAGetById", "Robert Mayer");
            users.Register(entity1);

            var entity = users.GetByUserId("ROMAGetById");
            Assert.IsNotNull(entity);

            if (entity != null)
                users.Unregister(entity1);
        }
        
        [TestMethod]
        public void DAL_UsersTable_GetByDisplayName()
        {
            Users users = new Users();

            UserEntity entity1 = new UserEntity("ROMAGetById", "Robert Mayer");
            users.Register(entity1);

            var entity = users.GetByDisplayName("Robert Mayer");
            Assert.IsNotNull(entity);

            if (entity != null)
                users.Unregister(entity1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DAL_UsersTable_Unregister_ExceptionTest()
        {
            Users users = new Users();
            users.Unregister(new UserEntity());
        }
        
        [TestMethod]
        public void DAL_UsersTable_Unregister()
        {
            Users users = new Users();
            
            UserEntity entity1 = new UserEntity("ROMAGetById", "Robert Mayer");
            users.Register(entity1);
            
            users.Unregister(users.GetByUserId("ROMAGetById"));
            
            var entity = users.GetByUserId("ROMAGetById");
            Assert.IsNull(entity);
        }
    }
}
