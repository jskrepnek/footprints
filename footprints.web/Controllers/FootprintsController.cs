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

        public ViewResult ViewPrints()
        {
            return View("ViewPrints", PrintsRepository.GetPrints().OrderBy(print => print.LastName));
        }

        public ActionResult Added()
        {
            return View();
        }

        public ViewResult AddPrint()
        {
            return View("AddPrint");
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

            return View("AddPrint");
        }
        
        public ActionResult DeleteAllPrints()
        {
            PrintsRepository.DeleteAll();
            return RedirectToAction("ViewPrints");
        }
    }
}
