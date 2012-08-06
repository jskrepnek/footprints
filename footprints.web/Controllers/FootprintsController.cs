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
        IPrintsRepository PrintsRepository { get; set; }

        public FootprintsController(ICommandAgent commandAgent, IPrintsRepositoryFactory printsListFactory)
        {
            CommandAgent = commandAgent;
            PrintsRepository = printsListFactory.Create();
        }

        public ActionResult ViewPrints()
        {
            return View(PrintsRepository.GetPrints());
        }

        public ActionResult AddPrint()
        {
            return View();
        }

        public ActionResult Added()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPrint(PrintModel model)
        {
            if (ModelState.IsValid)
            {
                model.Date = DateTime.Now;
                CommandAgent.SendCommand(model);

                return RedirectToAction("Added");
            }

            return View();
        }
    }
}
