using Pingis.DAL;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class User
    {
        public User() { }
        public User(UserEntity entity)
        {
            this.UserId = entity.UserId;
            this.DisplayName = entity.DisplayName;
        }
        public User(UserEntity entity, Matches matches, Users users)
        {
            this.UserId = entity.UserId;
            this.DisplayName = entity.DisplayName;
            this.EloRating = entity.EloRating;

            this.Matches = new List<Match>();

            var matchEntities = matches.GetMatchesByUserId(entity.UserId);
            if (matchEntities != null && matchEntities.Count > 0)
                this.Matches = matchEntities.Select(m => new Match(m, matches, users)).ToList();
        }

        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public List<Match> Matches { get; set; }
        public double EloRating { get; set; }
    }
}