using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingis.Models
{
    public enum MatchStatus
    {
        Challenged = 0,
        Accepted = 1,
        InProgress = 2,
        Done = 3,
        Aborted = 4
    }
}
