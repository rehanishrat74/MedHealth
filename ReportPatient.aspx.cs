using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MedHealthSolutions.Classes;
using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;

namespace MedHealthSolutions
{
    public partial class ReportPatient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());

            if (Request.Form["param"] != null) {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Dictionary<string, string> dict = ser.Deserialize<Dictionary<string, string>>("{" + Request.Form["param"].ToString() + "}");
                clsDB DB = new clsDB();
                string dateFrom = "null";
                string dateTo = "null";
                if (dict["dateRangeType"] == "1")
                {
                    string[] arDate = dict["dateRange"].Split('-');
                    dateFrom = "'" + (new DateTime(Convert.ToInt16(arDate[0]), Convert.ToInt16(arDate[1]), 1)).ToString("yyyy-MM-dd") + "'";
                    dateTo = "'" + (new DateTime(Convert.ToInt16(arDate[0]), Convert.ToInt16(arDate[1]), 1).AddMonths(1).AddDays(-1)).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    string[] arDate = dict["dateRange"].Split('~');
                    dateFrom = toDate(arDate[0]);
                    dateTo = toDate(arDate[1]);
                }
                DataSet ds = DB.getDS("EXEC report_Patient @formType='" + dict["formType"] + "', @formStatus=" + dict["formStatus"] + ", @dateFrom=" + dateFrom + ", @dateTo =" + dateTo + "", true);

                DataTable dt = ds.Tables[0];
                dt.Columns.RemoveAt(1);
                XL.prepareDownloadXL(ref dt, Response, "AgentProductivityReport", null);
                ds.Dispose();
            }
        }

        public static string toDate(string strDate)
        {
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                return "'"+dt.ToString("yyyy-MM-dd")+"'";
            }
            catch
            {
                return "null";
            }
        }

        [System.Web.Services.WebMethod]
        public static string getReport(string formType, int formStatus, int dateRangeType, string dateRange)
        {
            try
            {
                clsDB DB = new clsDB();
                string dateFrom = "null";
                string dateTo = "null";
                if (dateRangeType == 1)
                {
                    string[] arDate = dateRange.Split('-');
                    dateFrom = "'" + (new DateTime(Convert.ToInt16(arDate[0]), Convert.ToInt16(arDate[1]), 1)).ToString("yyyy-MM-dd") + "'";
                    dateTo = "'" + (new DateTime(Convert.ToInt16(arDate[0]), Convert.ToInt16(arDate[1]), 1).AddMonths(1).AddDays(-1)).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    string[] arDate = dateRange.Split('~');
                    dateFrom = toDate(arDate[0]);
                    dateTo = toDate(arDate[1]);
                }
                DataSet ds = DB.getDS("EXEC report_Patient @formType='" + formType + "', @formStatus=" + formStatus + ", @dateFrom=" + dateFrom + ", @dateTo =" + dateTo + "", true);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                    return "<div class='no-result'>No data available for selected filters</div>";

                StringBuilder sbOutput = new StringBuilder();
                sbOutput.Append("<table class='rpt_tbl tp_rht'>");
                sbOutput.Append("<tr>");
                sbOutput.Append("<th>Patient ID</th>");
                sbOutput.Append("<th>Patient Name</th>");
                sbOutput.Append("<th>DOB</th>");
                sbOutput.Append("<th>Discharge Date</th>");
                sbOutput.Append("<th>Form</th>");
                sbOutput.Append("<th>Last Record Follow-up</th>");
                sbOutput.Append("<th>Agent Name</th>");
                sbOutput.Append("</tr>");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbOutput.Append("<tr><td>" + lib.cStr(dt.Rows[i]["Patient ID"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Patient Name"]) + "</td><td>" + lib.cDate(dt.Rows[i]["DOB"]) + "</td><td>" + lib.cDate(dt.Rows[i]["Discharge Date"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Form"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Last Recorded Follow-up"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Agent Name"]) + "</td></tr>");
                }
                sbOutput.Append("</table>");

                return sbOutput.ToString();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
        }
    }
}
