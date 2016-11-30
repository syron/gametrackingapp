using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Pingis.DAL;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebApp_OpenIDConnect_DotNet_B2C.Models;

namespace WebApp_OpenIDConnect_DotNet_B2C.ApiControllers
{
    [Authorize]
    public class MatchesController : ApiController
    {
        [HttpGet]
        public List<Match> Get(MatchStatus? status = null)
        {
            Matches matches = new Matches();
            Users users = new Users();

            var playedMatches = matches.GetAll(status);
            List<Match> result = new List<Match>();
            foreach (var match in playedMatches)
            {
                Match m = new Match();
                m.ChallengerPoints = match.ChallengerPoints;
                m.OpponentPoints = match.OpponentPoints;
                m.Timestamp = match.Timestamp;

                m.Opponent = new Models.User(users.GetByUserId(match.OpponentId));
                m.Challenger = new Models.User(users.GetByUserId(match.ChallengerId));

                result.Add(m);
            }

            return result;
        }

        [HttpGet]
        [Route("api/matches/{matchId}")]
        public MatchEntity Get(Guid matchId)
        {
            Matches matches = new Matches();

            return matches.GetMatchById(matchId);
        }

        [HttpGet]
        [Route("api/matches/challenge")]
        public MatchEntity Challenge(string opponentId)
        {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            var matchId = Guid.NewGuid();
            matches.RegisterMatch(new MatchEntity(matchId, objectId.Value, opponentId));

            return matches.GetMatchById(matchId);
        }
    }
}