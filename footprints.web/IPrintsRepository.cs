using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using footprints.web.Models;

namespace footprints.web
{
    public interface IPrintsRepository
    {
        IEnumerable<PrintModel> GetPrints();
    }
}