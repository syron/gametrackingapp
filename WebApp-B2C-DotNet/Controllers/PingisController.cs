using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_OpenIDConnect_DotNet_B2C.Controllers
{
    [Authorize]
    public class PingisController : BaseController
    {
        // GET: Pingis
        public ActionResult Index()
        {
            ViewBag.CurrentUserId = currentUserId;
            return View();
        }
    }
}