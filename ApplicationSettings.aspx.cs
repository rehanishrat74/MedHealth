using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MedHealthSolutions.Classes;
using System.Net.Mail;
using System.IO;
using System.Web.Script.Serialization;

namespace MedHealthSolutions
{
    public partial class ApplicationSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());
        }

        [System.Web.Services.WebMethod]
        public static string[] getInitialize()
        {
            clsDB DB = new clsDB();
            string[] arOutput = new string[2];
            StringBuilder sbOutput = new StringBuilder();
            string[] arCS = System.Configuration.ConfigurationManager.ConnectionStrings["MedHealthSolutions"].ConnectionString.Split(';');
            arOutput[0] = arCS[0].Split('=')[1] + "." + arCS[1].Split('=')[1];
            sbOutput.Append("<table class='rpt_tbl tp_rht'><tr><th></th><th>Value</th><th>Description</th><th>Group</th></tr>");
            DataTable dt = DB.getDS("config.getAppSettings").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++) {
                sbOutput.Append("<tr><th>" + lib.cStr(dt.Rows[i]["KeyName"]) + "</th><td>" + lib.cStr(dt.Rows[i]["KeyValue"]) + "</td><td>" + lib.cStr(dt.Rows[i]["KeyDesc"]) + "</td><td>" + lib.cStr(dt.Rows[i]["KeyGroup"]) + "</td></tr>");
            }
            sbOutput.Append("</table>");
            arOutput[1] = sbOutput.ToString();
            return arOutput;
        }
    }
}
