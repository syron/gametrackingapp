using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pingis.Models
{
    public class SeasonEntity : TableEntity
    {
        public SeasonEntity(Guid seasonId, string name, DateTimeOffset startDate)
        {
            this.PartitionKey = seasonId.ToString();
            this.RowKey = name;
            this.StartDate = startDate;
        }

        public SeasonEntity() {
        }

        public string SeasonId { get { return this.RowKey;  } }
        public string Name { get { return this.PartitionKey; } }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}