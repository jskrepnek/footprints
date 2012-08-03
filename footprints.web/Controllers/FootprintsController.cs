using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using footprints.web.Models;

namespace footprints.web.Controllers
{
    public class FootprintsController : Controller
    {
        ICommandAgent CommandAgent { get; set; }

        public FootprintsController(ICommandAgent commandAgent)
        {
            CommandAgent = commandAgent;
        }

        public ActionResult ViewPrints()
        {
            return View();
        }

        public ActionResult AddPrint()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPrint(PrintModel model)
        {
            if (ModelState.IsValid)
            {
                CommandAgent.SendCommand(model);
            }

            return View();
        }
    }
}
