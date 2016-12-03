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
        public List<Match> Get(string userId = null, int? status = null)
        {
            Matches matches = new Matches();
            Users users = new Users();

            List<MatchEntity> playedMatches = null;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                playedMatches = matches.GetMatchesByUserId(userId.ToString());
            }
            else
            {
                playedMatches = matches.GetAll();
            }

            if (status.HasValue) {
                playedMatches = playedMatches.Where(s => s.Status == status).ToList();
            }
            
            List<Match> result = new List<Match>();
            foreach (var match in playedMatches)
            {
                Match m = new Match(match, matches, users);
               
                result.Add(m);
            }

            return result;
        }

        [HttpGet]
        [Route("api/matches/my/challenged")]
        public List<MatchEntity> MyChallengedMatches() {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;

            var mymatches = matches.GetMatchesByUserId(userId);
            return mymatches.Where(m => m.OpponentId == userId && m.Status == 0).ToList();
        }

        [HttpGet]
        [Route("api/matches/my/accepted")]
        public List<MatchEntity> MyAcceptedMatches()
        {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;

            var mymatches = matches.GetMatchesByUserId(userId);
            return mymatches.Where(m => m.Status == 1).ToList();
        }

        [HttpGet]
        [Route("api/matches/my/played")]
        public List<MatchEntity> MyPlayedMatches()
        {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;

            var mymatches = matches.GetMatchesByUserId(userId);
            return mymatches.Where(m => m.Status == 2).ToList();
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
            string userId = objectId.Value;

            var currentUserMatches = matches.GetMatchesByUserId(userId);

            var matchId = Guid.NewGuid();
            var matchEntity = new MatchEntity(matchId, objectId.Value, opponentId);
            if (currentUserMatches == null) 
                matches.RegisterMatch(matchEntity);

            if (currentUserMatches.Any(cu => cu.Status == 0 || cu.Status == 1))
            {
                // do nothing
                return null;
            }
            else
            {
                matches.RegisterMatch(matchEntity);
            }

            return matches.GetMatchById(matchId);
        }

        [HttpGet]
        [Route("api/matches/accept/{matchId}")]
        public bool Accept(string matchId)
        {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;
            var match = matches.GetMatchById(Guid.Parse(matchId));

            // if challengerid is equal to current user id... no accept is allowed.
            if (match.ChallengerId == userId) return false;

            // if match status is not equal to 0, accept is not possible.
            if (match.Status != 0) return false;

            match.Status = 1;
            matches.Update(match);

            return true;
        }

        [HttpGet]
        [Route("api/matches/finish/{matchId}")]
        public bool Finish(string matchId, int challengerPoints, int opponentPoints)
        {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;
            var match = matches.GetMatchById(Guid.Parse(matchId));

            if (match.ChallengerId == userId || match.OpponentId == userId)
            {
                if (match.Status != 1)
                    return false;

                match.Status = 2;
                match.ChallengerPoints = challengerPoints;
                match.OpponentPoints = opponentPoints;
                matches.Update(match);
                    
                return true;

            }
            else
            {
                return false;
            }

            // if match status is not equal to 0, accept is not possible.

            
        }

        [HttpGet]
        [Route("api/matches/decline/{matchId}")]
        public bool Decline(string matchId)
        {
            Matches matches = new Matches();
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;
            var match = matches.GetMatchById(Guid.Parse(matchId));

            // if challengerid is equal to current user id... no accept is allowed.
            if (match.ChallengerId == userId) return false;

            // if match status is not equal to 0, accept is not possible.
            if (match.Status != 0) return false;
            
            matches.Delete(match);

            return true;
        }
    }
}