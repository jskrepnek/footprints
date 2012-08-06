using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using footprints.web.Models;
using log4net;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;

namespace footprints.web
{
    public class PrintsBlob
    {
        CloudBlob Blob { get; set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(PrintsBlob));

        public PrintsBlob(CloudBlob blob)
        {
            if (null == blob)
            {
                throw new ArgumentException();
            }

            Blob = blob;
        }

        public bool Exists()
        {
            try
            {
                Blob.FetchAttributes();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public PrintsModel AsPrintsModel
        {
            get
            {
                var prints = new PrintsModel();

                if (Exists())
                {
                    // get the blob as text
                    string blobJson = Blob.DownloadText();

                    // try to deserialize it into JSon
                    try
                    {
                        prints = JsonConvert.DeserializeObject<PrintsModel>(blobJson);
                    }
                    catch (Exception e)
                    {
                        // the simple choice is to delete a corrupted blob when discovered
                        log.Error("There was an exception thrown while deserializing the blob of footprints.", e);
                        log.Info("Deleting corrupt blob from storage account");
                        Blob.Delete();
                    }
                }

                return prints;
            }
        }
     
    }
}