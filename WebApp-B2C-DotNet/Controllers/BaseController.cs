using Pingis.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebApp_OpenIDConnect_DotNet_B2C.Controllers
{
    public class BaseController : Controller
    {
        public Users users = null;
        public Matches matches = null;
        
        public BaseController() : base()
        {
            users = new Users();
            matches = new Matches();
        }

        public bool IsPlayerRegistered
        {
            get
            {
                Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
                if (users.GetByUserId(objectId.Value) == null) return false;
                
                return true;
            }
        }


        public string currentUserId
        {
            get {
                Claim objectId = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
                return objectId.Value;
            }
        }
}
}