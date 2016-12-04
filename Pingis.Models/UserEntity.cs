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
            this.EloRating = 1000;
        }

        public UserEntity()
        {
            this.EloRating = 1000;
        }

        public string UserId { get { return this.PartitionKey; } }
        public string DisplayName { get { return this.RowKey; } }
        public Double EloRating { get; set; }
    }
}