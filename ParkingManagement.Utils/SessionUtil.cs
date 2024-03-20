using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ParkingManagement.Model;

namespace ParkingManagement.Utils
{
    public class SessionUtil
    {
        public static SessionModel GetSession()
        {
            SessionModel Obj = HttpContext.Current.Session["info"] as SessionModel;
            return Obj != null ? Obj : new SessionModel();
        }

        public static void SetSession(SessionModel session)
        {
            HttpContext.Current.Session["info"] = session;
        }
    }
}
