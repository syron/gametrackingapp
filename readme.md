# Game Tracking App

Open source project... using Azure AD B2C & Azure Table Storage.

## Requirements
The requirements to run this applications are

* Valid Azure Subscription
* Azure AD B2C tenant
* Azure Storage Account

## Notifications
Notifications have been implemented for

* Challenge user

### How it works
The webapp, whenever someone is being challenged and if the challenged user has an email address, will send a json Notification message to an Azure Service Bus queue called "notifications". The message will then be retrieved by an Azure Function App, which will check if the message is of type "Notification" and will then send the notification.
