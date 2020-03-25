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

namespace MedHealthSolutions
{
    public partial class Hospital : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());
            if (Request["u_id"] != null) {
                try
                {
                    if (!Directory.Exists(Server.MapPath("/imgs/sign")))
                        Directory.CreateDirectory(Server.MapPath("/imgs/sign"));

                    if (!File.Exists(Server.MapPath("/imgs/sign/sign_" + Request["u_id"] + ".jpg")))
                        File.Delete(Server.MapPath("/imgs/sign/sign_" + Request["u_id"] + ".jpg"));

                    Bitmap bitmap = new Bitmap(Request.Files[0].InputStream);
                    bitmap.Save(Server.MapPath("/imgs/sign/sign_" + Request["u_id"] + ".jpg"),System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitmap = null;

                    Response.Write("/imgs/sign/sign_" + Request["u_id"] + ".jpg");
                    Response.End();
                }
                catch { }
            }
           
            if (User.IsLogin && !User.IsAdmin)
                Response.Redirect("AccessDenied.aspx?p=" + Request.ServerVariables["SCRIPT_NAME"]);
        }

        [System.Web.Services.WebMethod]
        public static string buildList(int page, string sSort, string sOrder)
        {
            clsDB DB = new clsDB();
            int pageSize = 25;
            DataSet ds = DB.getDS("hm_getHospital " + page + "," + pageSize + ",'" + sSort + "','" + sOrder + "'", true);
            DataTable dt = ds.Tables[0];
            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<table class='rpt_tbl tp_rht'>");
            sbOutput.Append("<tr>");
            sbOutput.Append(lib.getHeader("Hospital Name", "AC", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Address", "UN", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Contact Number", "EMAIL", sSort, sOrder, "sortUsr"));
            sbOutput.Append("</tr>");
            //,, 
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                sbOutput.Append("<tr d='" + lib.cStr(dt.Rows[i]["Hospital_PK"]) + "' onclick='newEntry(this)' z='" + lib.cStr(dt.Rows[i]["ZipCode_PK"]) + "' a='" + lib.cStr(dt.Rows[i]["HospitalAddress"]) + "' cn='" + lib.cStr(dt.Rows[i]["ContactNumber"]) + "'><td>" + lib.cStr(dt.Rows[i]["HospitalName"]) + "</td><td>" + lib.cStr(dt.Rows[i]["HospitalAddress"]) + "<br>" + lib.cStr(dt.Rows[i]["City"]) + ", " + lib.cStr(dt.Rows[i]["ZipCode"]) + " " + lib.cStr(dt.Rows[i]["State"]) + "</td><td>" + lib.cStr(dt.Rows[i]["ContactNumber"]) + "</td></tr>");
            }
            sbOutput.Append("</table>");
            sbOutput.Append(lib.paging(lib.cInt(dt.Rows[0][1]), pageSize, "getUsrs(#P#,'#A#','" + sSort + "','" + sOrder + "')", page));

            return sbOutput.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string saveHospital(string id, string HospitalName, string Address, string ZipCode, string ContactNumber)
        {
            clsDB DB = new clsDB();
            try
            {
                ZipCode = Int32.Parse(ZipCode).ToString();
            }
            catch {
                ZipCode = "null";
            }
            DB.executeSQL("EXEC hm_saveHospital @Id='" + id + "', @HospitalName='" + HospitalName + "', @Address='" + Address + "', @ZipCode=" + ZipCode + ", @ContactNumber='" + @ContactNumber + "'");
            DB.closeConnection();            
            return "";
        }
    }
}
