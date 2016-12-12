using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Pingis.DAL;
using Pingis.Logic;
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
    public class MatchesController : BaseApiController
    {
        [HttpGet]
        public List<Match> Get(string userId = null, int? status = null, int? top = null)
        {
            List<MatchEntity> playedMatches = null;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                playedMatches = Matches.GetMatchesByUserId(userId.ToString());
            }
            else
            {
                playedMatches = Matches.GetAll();
            }

            if (playedMatches == null) return null;

            if (status.HasValue) {
                var tempMatches = playedMatches.Where(s => s.Status == status);
                if (tempMatches == null)
                    return null;

                playedMatches = tempMatches.ToList();
            }
            
            List<Match> result = new List<Match>();
            foreach (var match in playedMatches)
            {
                Match m = new Match(match, Matches, Users);
               
                result.Add(m);
            }

            result = result.OrderByDescending(r => r.MatchUpdated ).ToList();

            if (top.HasValue)
            {
                return result.Take(top.Value).ToList();
            }
            return result;
        }

        [HttpGet]
        [Route("api/matches/count")]
        public int NumberOfMatches(string userId = null, int? status = null)
        {
            List<MatchEntity> playedMatches = null;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                playedMatches = Matches.GetMatchesByUserId(userId.ToString());
            }
            else
            {
                playedMatches = Matches.GetAll();
            }

            if (playedMatches == null || playedMatches.Count == 0)
                return 0;

            if (status.HasValue)
            {
                var tempMatches = playedMatches.Where(s => s.Status == status);
                if (tempMatches != null)
                    playedMatches = tempMatches.ToList();
                else
                    return 0;
            }
           

            return playedMatches.Count;
        }

        [HttpGet]
        [Route("api/matches/my/challenged")]
        public List<MatchEntity> MyChallengedMatches() {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;

            var mymatches = Matches.GetMatchesByUserId(userId);
            return mymatches.Where(m => m.OpponentId == userId && m.Status == 0).OrderByDescending(r => r.Timestamp).ToList();
        }

        [HttpGet]
        [Route("api/matches/my/accepted")]
        public List<MatchEntity> MyAcceptedMatches()
        {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;

            var mymatches = Matches.GetMatchesByUserId(userId);
            return mymatches.Where(m => m.Status == 1).OrderByDescending(r => r.Timestamp).ToList();
        }

        [HttpGet]
        [Route("api/matches/my/played")]
        public List<MatchEntity> MyPlayedMatches()
        {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;

            var mymatches = Matches.GetMatchesByUserId(userId);
            return mymatches.Where(m => m.Status == 2).OrderByDescending(r => r.Timestamp).ToList();
        }



        [HttpGet]
        [Route("api/matches/{matchId}")]
        public MatchEntity Get(Guid matchId)
        {
            return Matches.GetMatchById(matchId);
        }

        [HttpGet]
        [Route("api/matches/challenge")]
        public MatchEntity Challenge(string opponentId)
        {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            string userId = objectId.Value;

            if (opponentId == userId) return null;

            var currentUserMatches = Matches.GetMatchesByUserId(userId);

            var matchId = Guid.NewGuid();
            var matchEntity = new MatchEntity(matchId, objectId.Value, opponentId);
            if (currentUserMatches == null)
            {
                Matches.RegisterMatch(matchEntity);
            }
            else
            {

                if (currentUserMatches.Any(cu => (cu.Status == 0 || cu.Status == 1) && cu.OpponentId == opponentId))
                {
                    // do nothing
                    return null;
                }
                else
                {
                    Matches.RegisterMatch(matchEntity);
                }
            }

            var match = Matches.GetMatchById(matchId);

            Notifications.SendNotification(Pingis.Notifications.NotificationType.Challenged, match, Users.GetByUserId(userId), Users.GetByUserId(opponentId));

            return match;
        }

        [HttpGet]
        [Route("api/matches/accept/{matchId}")]
        public bool Accept(string matchId)
        {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;
            var match = Matches.GetMatchById(Guid.Parse(matchId));

            // if challengerid is equal to current user id... no accept is allowed.
            if (match.ChallengerId == userId) return false;

            // if match status is not equal to 0, accept is not possible.
            if (match.Status != 0) return false;

            match.Status = 1;
            Matches.Update(match);

            Notifications.SendNotification(Pingis.Notifications.NotificationType.ChallengeAccepted, match, Users.GetByUserId(match.ChallengerId), Users.GetByUserId(match.OpponentId));

            return true;
        }

        [HttpGet]
        [Route("api/matches/finish/{matchId}")]
        public bool Finish(string matchId, int challengerPoints, int opponentPoints)
        {
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;
            var match = Matches.GetMatchById(Guid.Parse(matchId));
            
            if (match.ChallengerId == userId || match.OpponentId == userId)
            {
                if (match.Status != 1)
                    return false;

                match.Status = 2;
                match.ChallengerPoints = challengerPoints;
                match.OpponentPoints = opponentPoints;
                Matches.Update(match);

                var challenger = Users.GetByUserId(match.ChallengerId);
                var opponent = Users.GetByUserId(match.OpponentId);

                // update users
                EloRating elo = new EloRating(challenger.EloRating, opponent.EloRating, challengerPoints, opponentPoints);
                challenger.EloRating = elo.FinalResult1;
                opponent.EloRating = elo.FinalResult2;

                Users.Update(challenger);
                Users.Update(opponent);
                    
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
            Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = objectId.Value;
            var match = Matches.GetMatchById(Guid.Parse(matchId));
            
            // if match status is not equal to 0, accept is not possible.
            if (match.Status != 0) return false;
            
            Matches.Delete(match);

            return true;
        }
    }
}