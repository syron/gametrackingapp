using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class Match
    {
        public User Challenger { get; set; }
        public User Opponent { get; set; }
        public int ChallengerPoints { get; set; }
        public int OpponentPoints { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}