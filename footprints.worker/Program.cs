using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using footprints.web.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;

namespace footprints.worker
{
    class Program
    {
        const string QUEUE_NAME = "footprints";
        const string BLOB_CONTAINER_NAME = "footprints";
        const string BLOB_NAME = "footprints";

        static void Main(string[] args)
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(QUEUE_NAME);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container
            CloudBlobContainer container = blobClient.GetContainerReference(BLOB_CONTAINER_NAME);
            container.CreateIfNotExist();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    break;
                }

                // Get the next message
                CloudQueueMessage retrievedMessage = queue.GetMessage();

                //Process the message in less than 30 seconds, and then delete the message
                if (retrievedMessage != null)
                {
                    string json = retrievedMessage.AsString;

                    Console.WriteLine("Top message");
                    Console.WriteLine("Json:");
                    Console.WriteLine(json);

                    try
                    {
                        PrintModel print = JsonConvert.DeserializeObject<PrintModel>(json);

                        Console.WriteLine("Deserialized Print Model");
                        Console.WriteLine("First Name: " + print.FirstName);
                        Console.WriteLine("Last Name: " + print.LastName);
                        Console.WriteLine("Phrase: " + print.Phrase);

                        // download the blob
                        // deserialize it
                        // add the new print
                        // serialize it
                        // upload the blob

                        var blob = container.GetBlobReference(BLOB_NAME);
                        var prints = new PrintsModel();

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
                                // couldn't deserialize the blob, so throw forget it, the new blob will
                                // overwrite it
                            }
                        }
                        catch
                        {
                            // no existing blob
                        }

                        // add the new print
                        prints.Add(print);

                        // serialize it
                        string jsonWithNewPrint = JsonConvert.SerializeObject(prints);

                        // upload the blob
                        blob.UploadText(jsonWithNewPrint);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error deserializing object");
                    }
                    finally
                    {
                        Console.WriteLine("Deleting message");
                        queue.DeleteMessage(retrievedMessage);
                    }
                }
                else
                {
                    Console.WriteLine("No messages");
                }

                // check again in 5 seconds
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}
