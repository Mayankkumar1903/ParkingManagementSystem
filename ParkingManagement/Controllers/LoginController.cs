using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParkingManagement.Model;
using ParkingManagement.Utils;
using ParkingManagement.Business;
using System.Reflection;
using ParkingManagement.Logger;

namespace ParkingManagement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            try
            {
                SessionModel session = new Service().Login(login);
                if (session != null)
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    TempData["LoginErrorMessage"] = "Invalid credentials. Please try again.";
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            
            return View(login);
        }
    }
}