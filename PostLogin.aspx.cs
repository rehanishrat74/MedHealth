using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using System.Data;
using MedHealthSolutions.Classes;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Web.Script.Serialization;

namespace MedHealthSolutions
{
    public partial class PostLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            clsUser Usr = new clsUser();
            if (!Usr.IsLogin)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string redirectPage = "Default.aspx";
            int redirectCount = 0;
            //if (Usr.IsClient || Usr.IsAdmin)
            //{
            //    redirectCount++;
            //    redirectPage="Default.aspx";
            //}
            //if (Usr.IsScanTech || Usr.IsScanTechSV)
            //{
            //    redirectCount++;
            //    redirectPage="ChartManagerII.aspx";
            //}
            //if ((Usr.IsScheduler || Usr.IsSchedulerSV || Usr.IsSchedulerManager) && config.IsRetrospective())
            //{
            //    redirectCount++;
            //    redirectPage="RAScheduler.aspx";
            //}
            //if (Usr.IsCoderOffsite || Usr.IsCoderOnsite || Usr.IsCodingManager)
            //{
            //    redirectCount++;
            //    redirectPage="RACoder.aspx";
            //}
            //if (Usr.IsQACoder || Usr.IsQAManager || Usr.IsQAManagerVendor)
            //{
            //    redirectCount++;
            //    redirectPage="QA.aspx";
            //}
            //if (Usr.IsInvoiceAccountant || Usr.IsBillingAccountant)
            //{
            //    redirectCount++;
            //    redirectPage="InvoiceManagement.aspx";
            //}
            //if (Usr.IsQCC)
            //{
            //    redirectCount++;
            //    redirectPage="QCC_Calendar.aspx";
            //}
            //if ((Usr.IsScheduler || Usr.IsSchedulerSV || Usr.IsSchedulerManager) && config.IsProspective())
            //{
            //    redirectCount++;
            //    redirectPage="ProspectiveScheduler.aspx";
            //}
            //if (Usr.IsAdmin)
            //{
            //    redirectCount=1;
            //    redirectPage = "ApplicationSettings.aspx";
            //}                

            if (redirectCount>1 || redirectCount==0)
                Response.Redirect("Default.aspx");
            else
                Response.Redirect(redirectPage);
        }
    }
}
