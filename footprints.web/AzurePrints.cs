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

        private static CloudStorageAccount _StorageAccountInstance = null;
        private static CloudBlobClient _BlobClientInstance = null;
        private static CloudBlobContainer _BlobContainerInstance = null;
        private static CloudBlob _BlobInstance = null;

        private static CloudQueueClient _QueueClientInstance = null;
        private static CloudQueue _QueueInstance = null;

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
                if (_StorageAccountInstance == null)
                {
                    _StorageAccountInstance = CloudStorageAccount.Parse(ConnectionString);
                }
                return _StorageAccountInstance;
            }
        }

        public static CloudBlobClient CloudBlobClient
        {
            get
            {
                if (_BlobClientInstance == null)
                {
                    _BlobClientInstance = CloudStorageAccount.CreateCloudBlobClient();
                }
                return _BlobClientInstance;
            }
        }

        public static CloudBlobContainer CloudBlobContainer
        {
            get
            {
                if (_BlobContainerInstance == null)
                {
                    _BlobContainerInstance = CloudBlobClient.GetContainerReference(BLOB_CONTAINER_NAME);
                    _BlobContainerInstance.CreateIfNotExist();
                }
                return _BlobContainerInstance;
            }
        }

        public static CloudBlob CloudBlob
        {
            get
            {
                if (_BlobInstance == null)
                {
                    _BlobInstance = CloudBlobContainer.GetBlobReference(BLOB_NAME);
                }
                return _BlobInstance;
            }
        }

        public static CloudQueueClient CloudQueueClient
        {
            get
            {
                if (_QueueClientInstance == null)
                {
                    _QueueClientInstance = CloudStorageAccount.CreateCloudQueueClient();
                }
                return _QueueClientInstance;
            }
        }

        public static CloudQueue CloudQueue
        {
            get
            {
                if (_QueueInstance == null)
                {
                    _QueueInstance = CloudQueueClient.GetQueueReference(QUEUE_NAME);
                    _QueueInstance.CreateIfNotExist();
                }
                return _QueueInstance;
            }
        }
    }
}