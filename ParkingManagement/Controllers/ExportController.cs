using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ParkingManagement.Business;
using ParkingManagement.Helper;

namespace ParkingManagement.Controllers
{
    [CustomFilterAttribute]
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Export(DateTime startDate, DateTime endDate)
        {
            var result = new Service().GenerateParkingReport(startDate, endDate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}