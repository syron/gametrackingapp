using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingis.Models
{
    public class EmailNotification
    {
        public EmailNotification() { }

        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Receiver: {this.Receiver}, Title: {this.Title}, Message: {this.Message}";
        }
    }
}
