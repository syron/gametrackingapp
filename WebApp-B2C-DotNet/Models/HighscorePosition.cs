using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp_OpenIDConnect_DotNet_B2C.Models.Simple;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class HighscorePosition
    {
        public SimpleUser User { get; set; }
        public int Value { get; set; }
    }
}