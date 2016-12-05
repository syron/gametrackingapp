using Pingis.DAL;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using WebApp_OpenIDConnect_DotNet_B2C.Models;

namespace WebApp_OpenIDConnect_DotNet_B2C.ApiControllers
{
    [Authorize]
    public class PingisController : BaseApiController
    {
        [HttpGet]
        [Route("api/pingis/isregistered")]
        public bool IsRegistered()
        {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var user = Users.GetByUserId(objectId.Value);

            if (user == null) return false;
            return true;
        }

        [HttpGet]
        [Route("api/pingis/user")]
        public UserEntity User(string userId)
        {
            var user = Users.GetByUserId(userId);
            return user;
        }

        [HttpGet]
        [Route("api/pingis/register")]
        public bool Register()
        {
            UserEntity entity = new UserEntity(); 
            
            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            Claim objectId =   ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var user = Users.GetByUserId(objectId.Value);

            if (user == null)
            {
                
                Users.Register(new UserEntity(objectId.Value, displayName.Value));
                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("api/pingis/unregister")]
        public bool Unregister()
        {
            UserEntity entity = new UserEntity();
            
            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var user = Users.GetByUserId(objectId.Value);

            if (user != null)
            {
                Users.Unregister(user);
                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("api/pingis/users")]
        public List<UserEntity> GetUsers()
        {
            return Users.GetAll();
        }

        [HttpGet]
        [Route("api/pingis/users/toplist")]
        public IEnumerable<HighscorePosition> Toplist(string by, int top=5)
        {
            var users = Users.GetAll().Select(u => new User(u, Matches, Users));
            List<HighscorePosition> highscore = new List<HighscorePosition>();
            if (by == "matchCount")
            {
                foreach (var user in users)
                {
                    var matches = user.Matches.Where(m => m.Status == 2);
                    int value = 0;
                    if (matches != null)
                    {
                        value = matches.Count();
                    }
                    highscore.Add(new HighscorePosition() { User = user, Value = value });
                }
            }
            else if (by == "winCount")
            {
                foreach (var user in users)
                {
                    var matches = user.Matches.Where(m => m.Status == 2);

                    var matchesUserChallengeWin = matches.Where(m => m.Challenger.UserId == user.UserId && m.ChallengerPoints > m.OpponentPoints);
                    var matchesUserOpponentWin = matches.Where(m => m.Opponent.UserId == user.UserId && m.ChallengerPoints < m.OpponentPoints);

                    var userWins = matchesUserChallengeWin.Count() + matchesUserOpponentWin.Count();

                    highscore.Add(new HighscorePosition() { User = user, Value = userWins });
                }
            }
            else if (by == "ELO")
            {
                foreach (var user in users)
                {
                    highscore.Add(new HighscorePosition() { User = user, Value = (int)user.EloRating });
                }
            }
            else { return null; }
            return highscore.OrderByDescending(hs => hs.Value).Take(top);
        }
    }
}
