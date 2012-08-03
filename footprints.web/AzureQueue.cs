﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;

namespace footprints.web
{
    public class AzureQueue : ICommandAgent
    {
        public void SendCommand(object command)
        {
            // take the command to register a footprint and put the command
            // on an azure queue inside of a brokered messsage

            // retrieve the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            // create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // create and/or get a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("footprints");
            queue.CreateIfNotExist();

            // serialize the command
            string json = JsonConvert.SerializeObject(command);

            // add the command to the queue
            var message = new CloudQueueMessage(json);
            queue.AddMessage(message);
        }
    }
}