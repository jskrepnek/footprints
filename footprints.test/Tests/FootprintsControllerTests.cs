using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using footprints.web;
using footprints.web.Controllers;
using footprints.web.Models;
using NUnit.Framework;
using Rhino.Mocks;

namespace footprints.test
{
    [TestFixture]
    public class FootprintsControllerTests
    {
        [Test]
        public void AddPrintAction_OnGet_ReturnsViewResult()
        {
            var expectedViewName = "AddPrint";

            var controller = new FootprintsController(
                MockRepository.GenerateStub<ICommandAgent>(),
                MockRepository.GenerateStub<IPrintsRepositoryFactory>());

            // act
            var result = controller.AddPrint() as ViewResult;

            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName);
        }

        // This test is not working.  The call to AddModelError is failing on a 
        // NullReferenceException
        //[Test]
        //public void AddPrintAction_OnPostModelIsInvalid_ReturnsViewResult()
        //{
        //    var expectedViewName = "AddPrint";

        //    var controller = new FootprintsController(
        //        MockRepository.GenerateStub<ICommandAgent>(),
        //        MockRepository.GenerateStub<IPrintsRepositoryFactory>());

        //    controller.ModelState.AddModelError(String.Empty, "Error");

        //    // act
        //    var result = controller.AddPrint(new PrintModel()) as ActionResult;

        //    Assert.IsNotNull(result, "Should have returned a ActionResult");
        //}

         //This test is not working.  The call to AddModelError is failing on a 
         //NullReferenceException
       // [Test]
       // public void AddPrintAction_OnPostModelIsInvalid_DoesNotCallSendCommand()
       // {
       //     var mockCommandAgent = MockRepository.GenerateMock<ICommandAgent>();

       //     var controller = new FootprintsController(
       //         mockCommandAgent,
       //         MockRepository.GenerateStub<IPrintsRepositoryFactory>());
       //     controller.ControllerContext = new ControllerContext();

       //     controller.ModelState.AddModelError(String.Empty, "Error");

       //     // act
       //     controller.AddPrint(new PrintModel());

       //     mockCommandAgent.AssertWasNotCalled(ca => ca.SendCommand(Arg<object>.Is.Anything));
       //} 

        [Test]
        public void AddPrintAction_OnPostModelIsValid_SameModelIsUsedByCommandAgent()
        {
            var mockCommandAgent = MockRepository.GenerateMock<ICommandAgent>();

            var controller = new FootprintsController(
                mockCommandAgent,
                MockRepository.GenerateStub<IPrintsRepositoryFactory>());

            var print = new PrintModel ()
            {
                FirstName = "John",
                LastName = "Doe",
                Phrase = "Phrase",
                Date = new DateTime(1900, 1, 1)
            };

            mockCommandAgent.Expect(o => o.SendCommand(Arg<PrintModel>.Matches(p => 
                p.FirstName.Equals("John") &&
                p.LastName.Equals("Doe") &&
                p.Phrase.Equals("Phrase"))));
            
            // act
            controller.AddPrint(print);

            mockCommandAgent.VerifyAllExpectations();
        }

        [Test]
        public void AddPrintAction_OnPostModelIsValid_SetsDate()
        {
            var mockCommandAgent = MockRepository.GenerateMock<ICommandAgent>();

            var controller = new FootprintsController(
                mockCommandAgent,
                MockRepository.GenerateStub<IPrintsRepositoryFactory>());

            var print = new PrintModel()
            {
                Date = DateTime.MinValue
            };

            mockCommandAgent.Expect(o => o.SendCommand(Arg<PrintModel>.Matches(p =>
                p.Date.CompareTo(DateTime.MinValue) != 0)));

            // act
            controller.AddPrint(print);

            mockCommandAgent.VerifyAllExpectations();
        }

        [Test]
        public void ViewPrintsAction_OnGet_ReturnsViewPrints()
        {
            var prints = new List<PrintModel>();
            prints.Add(new PrintModel());

            var printsRepository = MockRepository.GenerateStub<IPrintsRepository>();
            printsRepository.Stub(repository => repository.GetPrints()).Return(prints.AsEnumerable());

            var printsRepositoryFactory = MockRepository.GenerateStub<IPrintsRepositoryFactory>();
            printsRepositoryFactory.Stub(factory => factory.Create()).Return(printsRepository);

            var controller = new FootprintsController(
                MockRepository.GenerateStub<ICommandAgent>(),
                printsRepositoryFactory);

            // act
            var result = controller.ViewPrints() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, "ViewPrints");           
        }
    }
}
