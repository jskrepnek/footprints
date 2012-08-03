using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using footprints.web.Controllers;
using NUnit.Framework;
using System.Web.Mvc;

namespace footprints.test
{
    [TestFixture]
    class HomeControllerTests
    {
        HomeController HomeController { get; set; }

        [SetUp]
        public void Setup()
        {
            HomeController = new HomeController();
        }

        [TearDown]
        public void TearDown()
        {
            HomeController = null;
        }

        [Test]
        public void IndexAction_ReturnsViewResult()
        {
            var result = HomeController.Index() as ActionResult;

            Assert.IsNotNull(result, "Should have returned an ActionResult");
        }
    }
}
