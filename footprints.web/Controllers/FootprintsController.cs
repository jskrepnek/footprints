using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace footprints.web.Controllers
{
    public class FootprintsController : Controller
    {
        //
        // GET: /Footprints/

        public ActionResult ViewPrints()
        {
            return View();
        }

        public ActionResult AddPrint()
        {
            return View();
        }
    }
}
