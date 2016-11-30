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

        public string UserId { get; set; }
        public string DisplayName { get; set; }
    }
}