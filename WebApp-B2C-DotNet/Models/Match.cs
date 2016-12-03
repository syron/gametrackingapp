using Pingis.DAL;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class Match
    {
        public Match() { }
        public Match(MatchEntity match, Matches matches, Users users)
        {
            ChallengerPoints = match.ChallengerPoints;
            OpponentPoints = match.OpponentPoints;
            Timestamp = match.Timestamp;
            MatchUpdated = match.MatchUpdated;
            MatchId = match.MatchId;
            Status = match.Status;

            Opponent = new User(users.GetByUserId(match.OpponentId));
            Challenger = new User(users.GetByUserId(match.ChallengerId));
        }
        public string MatchId { get; set; }
        public User Challenger { get; set; }
        public User Opponent { get; set; }
        public int ChallengerPoints { get; set; }
        public int OpponentPoints { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public DateTimeOffset MatchUpdated { get; set; }
        public int Status { get; set; }
    }
}