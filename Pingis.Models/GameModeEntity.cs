using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pingis.Models
{
    public class GameModeEntity : TableEntity
    {
        public GameModeEntity(string gameModeId, string created, string name, string description)
        {
            this.PartitionKey = gameModeId;
            this.RowKey = created;
            this.Name = name;
            this.Description = description;
        }

        public GameModeEntity()
        {
            
        }

        public string GameModeId { get { return this.PartitionKey; } }
        public string Created { get { return this.RowKey; } }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}