using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using footprints.web;
using Microsoft.WindowsAzure.StorageClient;

namespace footprints.worker
{
    public class AzurePrintsQueue : IQueue
    {
        public string GetMessage(out string msgId, out string popReceipt)
        {
            String text = String.Empty;
            msgId = String.Empty;
            popReceipt = String.Empty;
            CloudQueueMessage msg = AzurePrints.CloudQueue.GetMessage();

            if (null != msg)
            {
                msgId = msg.Id;
                popReceipt = msg.PopReceipt;
                text = msg.AsString;
            }

            return text;
        }

        /// <summary>
        /// Deletes the top message from the stack
        /// </summary>
        public void DeleteMessage(string msgId, string popReceipt)
        {
            AzurePrints.CloudQueue.DeleteMessage(msgId, popReceipt);
        }
    }
}
