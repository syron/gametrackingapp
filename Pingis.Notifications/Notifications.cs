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
                EmailNotification notification = new EmailNotification();
                
                if (type == NotificationType.Challenged)
                {
                    if (string.IsNullOrWhiteSpace(opponent.Email))
                        return;

                    // opponent should receive notification 
                    notification.Title = $"PingisApp: {challenger.DisplayName} has challenged you.";
                    notification.Message = $"{challenger.DisplayName} has challenged you! Please visit https://afpingisapp.azurewebsites.net/pingis to accept or decline the challenge! To turn notifications off, please contact Robert Mayer <robert.mayer@afconsult.com>!";
                    notification.Receiver = opponent.Email;
                }
                else if (type == NotificationType.ChallengeAccepted)
                {
                    if (string.IsNullOrWhiteSpace(challenger.Email))
                        return;

                    // challenger should receive notification
                    notification.Title = $"PingisApp: {opponent.DisplayName} accepted your challenge.";
                    notification.Message = $"{opponent.DisplayName} has accepted your challenge! Please visit https://afpingisapp.azurewebsites.net/pingis to fill in the score after the match has taken place! To turn notifications off, please contact Robert Mayer <robert.mayer@afconsult.com>!";
                    notification.Receiver = challenger.Email;
                }
                else if (type == NotificationType.ChallengeDeclined)
                {
                    if (string.IsNullOrWhiteSpace(challenger.Email))
                        return;

                    // challenger should receive notification
                    notification.Title = $"PingisApp: {opponent.DisplayName} declined your challenge.";
                    notification.Message = $"{challenger.DisplayName} has declined your challenge! Visit https://afpingisapp.azurewebsites.net/pingis to send a new challenge or to challenge another player! To turn notifications off, please contact Robert Mayer <robert.mayer@afconsult.com>!";
                    notification.Receiver = challenger.Email;
                }
                else
                {
                    return;
                }
                
                var message = new BrokeredMessage(JsonConvert.SerializeObject(notification));
                this.Client.Send(message);   
        }
    }
}
