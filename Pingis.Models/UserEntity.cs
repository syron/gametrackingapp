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
            this.DisplayName = displayName;
            this.EloRating = 1000;
            this.RowKey = DateTimeOffset.UtcNow.ToString();
        }

        public UserEntity()
        {
            this.RowKey = DateTimeOffset.UtcNow.ToString();
            this.EloRating = 1000;
        }
        public string Created { get { return this.RowKey;  } }
        public string UserId { get { return this.PartitionKey; } }
        public string DisplayName { get; set; }
        public Double EloRating { get; set; }
    }
}