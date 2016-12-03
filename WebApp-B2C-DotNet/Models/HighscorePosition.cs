using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class HighscorePosition
    {
        public User User { get; set; }
        public int Value { get; set; }
    }
}