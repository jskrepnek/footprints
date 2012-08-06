using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using log4net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace footprints.web
{
    public class AzureQueue : ICommandAgent
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PrintsBlob));

        public void SendCommand(object command)
        {
            // take the command to register a footprint and put the command 
            // on an azure queue inside of a brokered messsage

            // serialize the command
            string json = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        log.Error("There was an error serializing the command into JSON.", args.ErrorContext.Error);
                    }
                    
                });

            // create a message and add it to the queue
            var message = new CloudQueueMessage(json);

            AzurePrints.CloudQueue.AddMessage(message);
        }
    }
}