using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace footprints.web
{
    public static class AzurePrints
    {
        const string BLOB_CONTAINER_NAME = "footprints";
        const string BLOB_NAME = "footprints";
        const string QUEUE_NAME = "footprints";

        static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["StorageConnectionString"];
            }
        }

        public static CloudStorageAccount CloudStorageAccount
        {
            get
            {
                return CloudStorageAccount.Parse(ConnectionString);
            }
        }

        public static CloudBlobClient CloudBlobClient
        {
            get
            {
                return CloudStorageAccount.CreateCloudBlobClient();
            }
        }

        public static CloudBlobContainer CloudBlobContainer
        {
            get
            {
                var container = CloudBlobClient.GetContainerReference(BLOB_CONTAINER_NAME);
                container.CreateIfNotExist();
                return container;
            }
        }

        public static CloudBlob CloudBlob
        {
            get
            {
                return CloudBlobContainer.GetBlobReference(BLOB_NAME);
            }
        }

        public static CloudQueueClient CloudQueueClient
        {
            get
            {
                return CloudStorageAccount.CreateCloudQueueClient();
            }
        }

        public static CloudQueue CloudQueue
        {
            get
            {
                CloudQueue queue = CloudQueueClient.GetQueueReference(QUEUE_NAME);
                queue.CreateIfNotExist();
                return queue;
            }
        }
    }
}