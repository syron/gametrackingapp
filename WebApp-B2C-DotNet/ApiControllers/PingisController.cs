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
    public class PingisController : ApiController
    {
        [HttpGet]
        [Route("api/pingis/isregistered")]
        public bool IsRegistered()
        {
            Users users = new Users();

            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var user = users.GetByUserId(objectId.Value);

            if (user == null) return false;
            return true;
        }

        [HttpGet]
        [Route("api/pingis/register")]
        public bool Register()
        {
            UserEntity entity = new UserEntity(); 
            Users users = new Users();

            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            Claim objectId =   ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var user = users.GetByUserId(objectId.Value);

            if (user == null)
            {
                users.Register(new UserEntity(objectId.Value, displayName.Value));
                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("api/pingis/unregister")]
        public bool Unregister()
        {
            UserEntity entity = new UserEntity();
            Users users = new Users();

            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var user = users.GetByUserId(objectId.Value);

            if (user != null)
            {
                users.Unregister(user);
                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("api/pingis/users")]
        public List<UserEntity> Users()
        {
            Users users = new Users();

            return users.GetAll();
        }

        [HttpGet]
        [Route("api/pingis/users/toplist")]
        public IEnumerable<HighscorePosition> Toplist(string by, int top=5)
        {
            Users usersDal = new Pingis.DAL.Users();
            Matches matchesDal = new Matches();
            var users = usersDal.GetAll().Select(u => new User(u, matchesDal, usersDal));
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
            else { return null; }
            return highscore.OrderByDescending(hs => hs.Value).Take(top);
        }
    }
}
