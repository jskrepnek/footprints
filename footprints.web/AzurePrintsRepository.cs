using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using footprints.web.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;

namespace footprints.web
{
    public class AzurePrintsRepository : IPrintsRepository
    {
        const string BLOB_CONTAINER_NAME = "footprints";
        const string BLOB_NAME = "footprints";

        public IEnumerable<PrintModel> GetPrints()
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container
            CloudBlobContainer container = blobClient.GetContainerReference(BLOB_CONTAINER_NAME);
            container.CreateIfNotExist();

            var prints = new PrintsModel();

            // download the blob
            // deserialize it
            // add the new print
            // serialize it
            // upload the blob

            var blob = container.GetBlobReference(BLOB_NAME);

            try
            {
                // this tests if a previous blob exists
                blob.FetchAttributes();

                // existing blob
                string blobJson = blob.DownloadText();

                // try to deserialize it
                try
                {
                    prints = JsonConvert.DeserializeObject<PrintsModel>(blobJson);
                }
                catch (Exception)
                {
                    // couldn't deserialize the blob, so forget it, the new blob will
                    // overwrite it
                }
            }
            catch
            {
                // no existing blob
                // return an empty container
            }

            return prints.list.AsEnumerable();
        }
    }
}
