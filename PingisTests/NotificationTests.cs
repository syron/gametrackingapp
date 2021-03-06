﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ServiceBus.Messaging;
using Pingis.Models;
using Newtonsoft.Json;

namespace PingisTests
{
    /// <summary>
    /// Summary description for NotificationTests
    /// </summary>
    [TestClass]
    public class NotificationTests
    {
        public NotificationTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //

            var connectionString = "";
            var queueName = "notifications";

            EmailNotification notification = new EmailNotification();
            notification.Receiver = "rwlmayer@gmail.com";
            notification.Title = "Test";
            notification.Message = "Hello World";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            
            var message = new BrokeredMessage(JsonConvert.SerializeObject(notification));
            client.Send(message);
        }
    }
}
