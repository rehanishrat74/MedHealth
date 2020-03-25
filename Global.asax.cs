using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MedHealthSolutions.Classes;

namespace MedHealthSolutions
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //var sessionCookie = new HttpCookie("ASP.NET_SessionId", Context.Session.SessionID);
            //sessionCookie.Domain = ".ramtraxs.com";
            //Context.Response.SetCookie(sessionCookie);
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
            var sessionCookie = new HttpCookie("Advance_SessionID", lib.CompressString(HttpContext.Current.Session.SessionID));
            sessionCookie.Domain = config.PortalURL();
            sessionCookie.HttpOnly = true;
            HttpContext.Current.Response.SetCookie(sessionCookie);
        }

    }
}
