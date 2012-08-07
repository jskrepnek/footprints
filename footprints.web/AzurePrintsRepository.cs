using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using footprints.web.Models;
using log4net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;

namespace footprints.web
{
    public class AzurePrintsRepository : IPrintsRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AzurePrintsRepository));

        public IEnumerable<PrintModel> GetPrints()
        {
            // use the PrintsBlob class as a handler for deserialization, etc
            var blob = new PrintsBlob(AzurePrints.CloudBlob);
            return blob.AsPrintsModel.List.AsEnumerable();
        }

        public void DeleteAll()
        {
            AzurePrints.CloudBlob.DeleteIfExists();
        }
    }
}
