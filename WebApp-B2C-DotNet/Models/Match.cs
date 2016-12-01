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

            Opponent = new Models.User(users.GetByUserId(match.OpponentId));
            Challenger = new Models.User(users.GetByUserId(match.ChallengerId));

        }

        public User Challenger { get; set; }
        public User Opponent { get; set; }
        public int ChallengerPoints { get; set; }
        public int OpponentPoints { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public MatchStatus Status { get; set; }
    }
}