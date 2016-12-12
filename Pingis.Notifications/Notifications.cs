using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Pingis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingis.Notifications
{
    public class Notifications
    {
        QueueClient Client = null;
        public Notifications(string serviceBusConnectionString, string queueName)
        {
            this.Client = QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);
        }

        public void SendNotification(NotificationType type, MatchEntity match, UserEntity challenger, UserEntity opponent)
        {
            try
            {
                EmailNotification notification = new EmailNotification();

                if (!string.IsNullOrWhiteSpace(opponent.Email))
                    return;

                if (type == NotificationType.Challenged)
                {
                    // opponent should receive notification 
                    notification.Title = $"PingisApp: {challenger.DisplayName} has challenged you.";
                    notification.Message = $"{challenger.DisplayName} has challenged you!! Please visit https://afpingisapp.azurewebsites.net/pingis to accept or decline the challenge! To turn notifications off, please contact Robert Mayer <robert.mayer@afconsult.com>!";
                    
                }
                else if (type == NotificationType.ChallengeAccepted)
                {
                    // challenger should receive notification
                }
                else if (type == NotificationType.ChallengeDeclined)
                {
                    // challenger should receive notification
                }



                var message = new BrokeredMessage(JsonConvert.SerializeObject(notification));
                this.Client.Send(message);

            }
            catch (Exception ex)
            {
                // do nothing... todo: implement error handling...
            }
        }
    }
}
