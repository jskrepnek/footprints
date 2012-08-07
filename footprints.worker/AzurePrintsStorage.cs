using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using footprints.web;

namespace footprints.worker
{
    class AzurePrintsStorage : IStorage
    {
        public string DownloadText()
        {
            return AzurePrints.CloudBlob.DownloadText();
        }

        public void UploadText(string content)
        {
            AzurePrints.CloudBlob.UploadText(content);
        }

        public bool Exists()
        {
            try
            {
                AzurePrints.CloudBlob.FetchAttributes();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
