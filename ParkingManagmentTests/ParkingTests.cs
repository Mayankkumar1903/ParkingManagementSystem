using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingManagement.Controllers;
using ParkingManagement.Model;
using System;
using System.Web.Mvc;

namespace ParkingManagmentTests
{
    [TestClass]
    public class ParkingTests
    {
        [TestMethod]
        public void LoginTest()
        {
            LoginModel l = new LoginModel
            {
                Email = "ffff@gmail.com",
                Password = "ffff"
            };
            LoginController controller = new LoginController();
            var result = controller.Login(l);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SignupTest()
        {
            SignupController signupcontroller = new SignupController();
            var result = signupcontroller.Signup(new SignupModel { Email = "AS@gmail.com", Name = "AS", Password = "AS", Type = "1" });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ExportTest()
        {
            ExportController export = new ExportController();
            var result = export.Export(DateTime.Now, DateTime.Now);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddParkingTest()
        {
            AddParkingController controller = new AddParkingController();
            var result = controller.InitializeParkingData(new ParkingModel
            {
                ParkingZoneTitle="D",
                NumberOfSpaces=4,
            });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BookParkingSpaceTest()
        {
            DashboardController controller = new DashboardController();
            var result=controller.BookParkingSpace("WB 24 BJ 4118");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FreeParkingSpaceTest()
        {
            DashboardController controller = new DashboardController();
            var result = controller.FreeParkingSpace("WB 24 BJ 4118");
            Assert.IsNotNull(result);
        }
    }
}
