using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace footprints.web
{
    public class AzurePrintsRepositoryFactory : IPrintsRepositoryFactory
    {
        public IPrintsRepository Create()
        {
            return new AzurePrintsRepository();
        }
    }
}
