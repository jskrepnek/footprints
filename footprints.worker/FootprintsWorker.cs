using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using footprints.web;
using footprints.web.Models;
using Newtonsoft.Json;

namespace footprints.worker
{
    public class FootPrintsWorker
    {
        static void Main(string[] args)
        {
            IQueue queue = new AzurePrintsQueue();
            IStorage storage = new AzurePrintsStorage();
            IKeyReader reader = new KeyReader();

            Execute(queue, storage, reader);
        }

        public static void Execute(IQueue queue, IStorage storage, IKeyReader reader)
        {
            while (true)
            {
                // Get the next message
                string msgId;
                string popReceipt;
                string retrievedMessage = queue.GetMessage(out msgId, out popReceipt);

                //Process the message in less than 30 seconds, and then delete the message
                if (!String.IsNullOrEmpty(retrievedMessage))
                {
                    Console.WriteLine("Top message");
                    Console.WriteLine("Json:");
                    Console.WriteLine(retrievedMessage);

                    try
                    {
                        PrintModel print = JsonConvert.DeserializeObject<PrintModel>(retrievedMessage);

                        Console.WriteLine("Deserialized Print Model");
                        Console.WriteLine("First Name: " + print.FirstName);
                        Console.WriteLine("Last Name: " + print.LastName);
                        Console.WriteLine("Phrase: " + print.Phrase);

                        // download the blob
                        // deserialize it
                        // add the new print
                        // serialize it
                        // upload the blob

                        // the container where we'll put existing prints and the new print
                        var prints = new PrintsModel();

                        if (storage.Exists())
                        {
                            // existing content
                            string blobJson = storage.DownloadText();

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
                        else
                        {
                            // no existing blob
                        }

                        // add the new print
                        prints.Add(print);

                        // serialize it
                        string jsonWithNewPrint = JsonConvert.SerializeObject(prints);

                        // upload the blob
                        storage.UploadText(jsonWithNewPrint);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error deserializing object");
                    }
                    finally
                    {
                        Console.WriteLine("Deleting message");
                        queue.DeleteMessage(msgId, popReceipt);
                    }
                }
                else
                {
                    Console.WriteLine("No messages");
                }

                // check again in 5 seconds
                System.Threading.Thread.Sleep(5000);

                if (reader.KeyAvailable)
                {
                    break;
                }
            }
        }
    }
}
