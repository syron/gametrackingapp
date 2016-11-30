using Pingis.DAL;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;

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
    }
}
