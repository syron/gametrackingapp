using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pingis.Models
{
    public class UserEntity : TableEntity
    {
        public UserEntity(string userId, string displayName)
        {
            this.PartitionKey = userId;
            this.RowKey = displayName;
        }

        public UserEntity() {
            
        }

        public string UserId { get { return this.PartitionKey; } }
        public string DisplayName { get { return this.RowKey; } }
    }
}