using Pingis.DAL;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models.Simple
{
    public class SimpleUser
    {
        public SimpleUser() { }
        public SimpleUser(UserEntity entity)
        {
            this.UserId = entity.UserId;
            this.DisplayName = entity.DisplayName;
            this.Wins = 0;
            this.Losses = 0;
        }
        public SimpleUser(UserEntity entity, Matches matches, Users users)
        {
            this.UserId = entity.UserId;
            this.DisplayName = entity.DisplayName;
            this.EloRating = entity.EloRating;
            this.Wins = 0;
            this.Losses = 0;

            this.CalculateStatistics(this.UserId, matches, users);
        }

        public SimpleUser(Models.User user, Matches matches, Users users)
        {
            this.UserId = user.UserId;
            this.DisplayName = user.DisplayName;
            this.EloRating = user.EloRating;
            this.Wins = 0;
            this.Losses = 0;

            this.CalculateStatistics(this.UserId, matches, users);
        }

        private void CalculateStatistics(string userId, Matches matches, Users users)
        {
            var matchEntities = matches.GetMatchesByUserId(userId, 2);
            if (matchEntities != null && matchEntities.Count > 0)
            {
                foreach (var match in matchEntities)
                {
                    if (match.ChallengerId == this.UserId)
                    {
                        if (match.ChallengerPoints > match.OpponentPoints)
                        {
                            this.Wins++;
                        }
                        else
                        {
                            this.Losses++;
                        }
                    }
                    else if (match.OpponentId == this.UserId)
                    {
                        if (match.OpponentPoints > match.ChallengerPoints)
                        {
                            this.Wins++;
                        }
                        else
                        {
                            this.Losses++;
                        }
                    }
                }
            }
        }

        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public double EloRating { get; set; }
        private List<Match> Matches { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}