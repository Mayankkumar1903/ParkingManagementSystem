using ParkingManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParkingManagement.Business;
using System.Web.Mvc;
using System.Xml;
using ParkingManagement.Utils;
using System.Web.Helpers;
using System.Text.RegularExpressions;
using ParkingManagement.Logger;

namespace ParkingManagement.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(SignupModel userdata)
        {
            try
            {
                if (new Service().FindEmail(userdata.Email) == true)
                {
                    bool data = new Service().Signup(userdata);
                    if (data == true)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        TempData["SignupErrorMessage"] = "Error!!";
                        return RedirectToAction("Index", "Signup");
                    }
                }
                else
                {
                    TempData["SignupErrorMessage"] = "Error!!";
                    return RedirectToAction("Index", "Signup");
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }

            return View(userdata);
        }

        static bool IsValidEmail(string email)
        {
            string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            Regex regex = new Regex(emailRegex);

            return regex.IsMatch(email);
        }

        public ActionResult EmailCheck(string email)
        {
            return Json(IsValidEmail(email) && new Service().FindEmail(email), JsonRequestBehavior.AllowGet);
        }

    }
}