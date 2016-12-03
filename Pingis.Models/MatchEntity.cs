using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pingis.Models
{
    public class MatchEntity : TableEntity
    {
        public MatchEntity(Guid matchId, string challengerId, string oppenentId)
        {
            this.PartitionKey = challengerId;
            this.RowKey = matchId.ToString();
            this.OpponentId = oppenentId;
            this.Status = 0;
            this.MatchUpdated = DateTimeOffset.Now;
        }

        public MatchEntity() {
            this.Status = 0;
            this.MatchUpdated = DateTimeOffset.Now;
        }

        public string MatchId { get { return this.RowKey;  } }
        public string ChallengerId { get { return this.PartitionKey; } }
        public string OpponentId { get; set; }
        public int ChallengerPoints { get; set; }
        public int OpponentPoints { get; set; }
        public int Status { get; set; }
        public DateTimeOffset MatchUpdated { get; set; }
    }
}