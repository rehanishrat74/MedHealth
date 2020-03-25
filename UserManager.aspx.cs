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
    public partial class UserManager : System.Web.UI.Page
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
        public static string uploadSign(string user_id,HttpPostedFileBase sign_file) { 
            return "123";
        }

        [System.Web.Services.WebMethod]
        public static string searchZip(string zip)
        {
            string output="";
            clsDB DB = new clsDB();
            DataTable dt = DB.getDS("um_searchZip '" + zip + "'", true).Tables[0];
            output += "<table class='srch_tbl'>";
            if (dt.Rows.Count == 0)
            {
                output += "<tr><td>No matching zip code found in the database</td></tr>";
            }
            else
            {
                output += "<tr><td>Click on the zip code to add in working areas</td></tr>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    output += "<tr><th id='" + lib.cStr(dt.Rows[i]["ZipCode"]) + "'>" + lib.cStr(dt.Rows[i]["ZipCode"]) + " " + lib.cStr(dt.Rows[i]["City"]) + ", " + lib.cStr(dt.Rows[i]["County"]) + "&nbsp;" + lib.cStr(dt.Rows[i]["State"]) + "</th></tr>";
                }
            }
            output += "</table>";
            return output;
        }

        [System.Web.Services.WebMethod]
        public static string searchAddZip(string zip)
        {
            string sSQL = "select distinct ZC.ZipCode from tblZipCode ZC";
            zip = zip.ToUpper();
            if (zip.IndexOf(" TO ") > 0)
            {
                sSQL += " WHERE zipcode >= '" + zip.Split(new string[] { " TO " }, StringSplitOptions.None)[0] + "' AND zipcode <= '" + zip.Split(new string[] { " TO " }, StringSplitOptions.None)[1] + "'";
            }
            else if (zip.IndexOf("C") > 0)
            {
                sSQL += " INNER JOIN tblZipcode T ON T.zipcode='" + zip.Replace("C", "") + "' AND T.State=ZC.State AND T.County=ZC.County AND T.City=ZC.City";
            }
            else if (zip.IndexOf("K") > 0)
            {
                sSQL += " INNER JOIN tblZipcode T ON T.zipcode='" + zip.Replace("K", "") + "' AND T.State=ZC.State AND T.County=ZC.County";
            }
            else if (zip.IndexOf("S") > 0)
            {
                sSQL += " INNER JOIN tblZipcode T ON T.zipcode='" + zip.Replace("S", "") + "' AND T.State=ZC.State";
            }
            else if (zip.IndexOf("Z") > 0)
            {
                sSQL += " INNER JOIN tblZoneZipcode T ON T.ZipCode_PK=ZC.ZipCode_PK";
                sSQL += " INNER JOIN tblZoneZipcode ZZC ON T.Zone_PK = ZZC.Zone_PK AND ZZC.ZipCode_PK=" + zip.Replace("Z", "");
            }
            else
            {
                sSQL += " WHERE zipcode like '" + zip.ToLower().Split(' ')[0] + "%'";
            }
            string output = "";
            clsDB DB = new clsDB();
            DataTable dt = DB.getDS(sSQL, true).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                        output += ",";
                    output += lib.cStr(dt.Rows[i]["ZipCode"]);
                }

            return output;
        }

        [System.Web.Services.WebMethod]
        public static string buildList(int page, string alpha, string sSort, string sOrder, int uStatus, int uRole, string uRoleId)
        {
            clsDB DB = new clsDB();
            int pageSize = 25;
            DataSet ds = DB.getDS("um_getUsers " + page + "," + pageSize + ",'" + alpha + "','" + sSort + "','" + sOrder + "'," + uStatus + "," + uRole + ",'" + uRoleId + "'", true);
            DataTable dt = ds.Tables[0];
            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<table class='rpt_tbl tp_rht'>");
            sbOutput.Append("<tr><th><input type=checkbox onclick='cAll(this)'></th>");
            sbOutput.Append(lib.getHeader("Active", "AC", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Username", "UN", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Fullname", "NAME", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Email", "EMAIL", sSort, sOrder, "sortUsr"));
            sbOutput.Append("<th>Roles</th>");
            sbOutput.Append("</tr>");
            //,, 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbOutput.Append("<tr d='" + lib.cStr(dt.Rows[i]["User_PK"]) + "'><td><input type=checkbox></td><td>" + lib.cTickMember(dt.Rows[i]["IsActive"], null, null, "Active") + "</td><td>" + lib.cStr(dt.Rows[i]["Username"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Fullname"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Email"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Roles"]) + "</td></tr>");
            }
            sbOutput.Append("</table>");
            sbOutput.Append(lib.paging(ds.Tables[1], pageSize, 0, "getUsrs(#P#,'#A#','" + sSort + "','" + sOrder + "')", page, alpha));

            return sbOutput.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string[] getUserCredentials(string user_id)
        {
            string[] arOutput = new string[2];
            string output = "";
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("um_getUserCredentials '" + user_id + "'", true);
            DataTable dt;
            dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    output += ";";
                output += lib.cStr(dt.Rows[i]["Page_PK"]);
            }            
            arOutput[0] = output;

            output = "";
            dt = ds.Tables[1];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > 0)
                    output += "~";

                if (dt.Columns[i].ColumnName.ToLower().Substring(0,2)=="is")
                    output += lib.cInt(dt.Rows[0][i]);
                else
                    output += lib.cStr(dt.Rows[0][i]);
            }
            arOutput[1] = output;

            return arOutput;
        }

        [System.Web.Services.WebMethod]
        public static string checkUser(string usr)
        {
            clsDB DB = new clsDB();
            if (lib.cInt(DB.getDS("um_checkUsername '" + usr + "'", true).Tables[0].Rows[0][0]) == 0)
                return "";
            else
                return "Username already exists";
        }

        [System.Web.Services.WebMethod]
        public static string saveUser(string id, string username, string email, string pwd, string lastname, string firstname, int IsActive, int IsAdmin, int IsManager, int IsAgent, int IsChangePasswordOnFirstLogin, string pw,string modules)
        {
            clsDB DB = new clsDB();
            DB.executeSQL("um_UpdateAddUser '" + id + "', '" + username + "', '" + email + "', '" + pwd + "', '" + lastname + "', '" + firstname + "', " + IsActive + ", " + IsAdmin + ", " + IsManager + ", " + IsAgent + ", " + IsChangePasswordOnFirstLogin + ", '" + modules + "'");
            DB.closeConnection();

            //if (id == "0")
            //{
            //    string email_body = "Hi " + firstname + ",\n\n";
            //    email_body += "Welcome to MedHealth!\n\n";
            //    email_body += "New user account Is created for you, please use following credentials to get in:\n\n";
            //    email_body += "URL:\t " + config.URLProtocol() + "://" + config.PortalClient().ToLower() + "." + config.PortalURL() + " \n";
            //    email_body += "Username:\t " + username + " \n";
            //    email_body += "Password:\t " + pw + " \n\n";
            //    email_body += "Have any questions? Just shoot us an email at support@ramtraxs.com. We’re always here to help.\n\n";
            //    email_body += "\nMedHealth Support";

            //    lib.sendEmail(email, "", "[" + username + "] User Account Created", email_body);
            //}
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string removeUser(string user)
        {
            clsUser Usr = new clsUser();
            clsDB DB = new clsDB();
            DB.executeSQL("um_removeUsers '" + user + "', " + Usr.User_PK);
            DB.closeConnection();
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string[] getList()
        {
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("um_initialize", true);
            DataTable dt = ds.Tables[0];
            DataRow[] tp = dt.Select("parent_pk=0");
            StringBuilder mnu = new StringBuilder();

            for (int t = 0; t < tp.Length; t++)
            {
                mnu.Append("      <ul>");
                if (lib.cBool(tp[t]["isPage"]))
                {
                    mnu.Append("   <li><label class='check-label'><input type=checkbox id='pg_" + lib.cStr(tp[t]["page_pk"]) + "' value='" + lib.cStr(tp[t]["page_pk"]) + "'> " + lib.cStr(tp[t]["page_name"]) + "<span class='checkmark'></span></label></li>");
                }
                else
                {
                    mnu.Append("   <li><b>" + lib.cStr(tp[t]["page_name"]) + "</b>");
                    mnu.Append("      <ul>");
                    DataRow[] sub = dt.Select("parent_pk=" + lib.cStr(tp[t]["page_pk"]));
                    for (int s = 0; s < sub.Length; s++)
                    {
                        mnu.Append("   <li><label class='check-label'><input type=checkbox id='pg_" + lib.cStr(sub[s]["page_pk"]) + "' value='" + lib.cStr(sub[s]["page_pk"]) + "'> " + lib.cStr(sub[s]["page_name"]) + "<span class='checkmark'></span></label></li>");
                    }
                    mnu.Append("      </ul>");
                    mnu.Append("   </li>");
                }
                mnu.Append("      </ul>");
            }
            int IsReadOnly = 0;
            clsUser Usr = new clsUser();
            if (Usr.IsAdmin == false) // && Usr.IsUserViewer == true
                IsReadOnly = 1;

            return new string[] { mnu.ToString(), IsReadOnly.ToString() };
        }

        [System.Web.Services.WebMethod]
        public static string updateProfile(int id, string email, string pwd, string lastname, string firstname)
        {
            clsDB DB = new clsDB();
            string sql="";
            if (pwd != "")
            {
                sql = "SELECT * FROM tblUserPasswordLog WHERE user_pk=" + id + " AND password='" + pwd + "' AND DATEDIFF(day,dtPassword,GetDate())<" + config.PasswordCanbeReusedAfterDays();
                if (DB.getDS(sql).Tables[0].Rows.Count > 0)
                    return "You can not reuse a password within " + config.PasswordCanbeReusedAfterDays() + " days, please use a different password";

                sql = "INSERT INTO tblUserPasswordLog(user_pk,password,dtPassword) VALUES(" + id + ",'" + pwd + "',GetDate());";
            }

            sql += "Update tblUser SET lastname='" + lastname.Replace("'", "") + "',firstname='" + firstname.Replace("'", "") + "',email_address='" + email.Replace("'", "") + "'";
            if (pwd!="")
                sql += ",password='" + pwd + "'";
            sql += " WHERE user_pk=" + id;
            DB.executeSQL(sql);
            DB.closeConnection();
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string getProfileInfo(int id)
        {
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("um_getProfileInfo " + id + "", true);
            DataTable dt = ds.Tables[0];
            StringBuilder sbOutput = new StringBuilder();

            sbOutput.Append("<h1>Profile Role:</h1>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (lib.cBool(dt.Rows[0][i]))
                    sbOutput.Append("<div class='fleft s4'>" + dt.Columns[i].ColumnName + "</div>");
            }
            sbOutput.Append("<br class='clear'>");
                     
            return sbOutput.ToString();
        }
    }
}
