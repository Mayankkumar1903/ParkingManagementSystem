using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkingManagement.Helper
{
    public class CustomFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["info"] == null)
            {
                filterContext.Result = new RedirectResult("/login/index");
            }
        }
    }
}