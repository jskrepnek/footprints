using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace footprints.web
{
    public interface IPrintsRepositoryFactory
    {
        IPrintsRepository Create();
    }
}