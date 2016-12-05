﻿using Pingis.DAL;
using Pingis.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApp_OpenIDConnect_DotNet_B2C.ApiControllers
{
    public class BaseApiController : ApiController
    {
        internal Notification Notifications { get; set; }
        internal Matches Matches { get; set; }
        internal Users Users { get; set; }

        public BaseApiController() : base()
        {
            Notifications = new Notification();
            Matches = new Matches();
            Users = new Users();
        }
    }
}