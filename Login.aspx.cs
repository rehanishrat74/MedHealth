using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MedHealthSolutions.Classes;

namespace MedHealthSolutions
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Request.IsSecureConnection && config.URLProtocol()=="https")
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["URL"]);
            }

            if (Request.QueryString["rs"] != null) {

            }
            if (Request.QueryString["clear"] != "")
                (new clsUser()).clear();
        }

        [System.Web.Services.WebMethod]
        public static string verifyReset(string rs)
        {
            try
            {
                string[] param = lib.DecompressString(HttpUtility.UrlDecode(rs)).Split('~');
                if (DateTime.Now.ToOADate() < Convert.ToDouble(param[2]))
                {
                    return doLogin(param[0], param[1])[0].Replace("success", "expired") + "," + param[0];
                }
                else
                    return "error";
            }
            catch {
                return "error";
            }
        }

        [System.Web.Services.WebMethod]
        public static string doReset(string usr)
        {
            clsDB DB = new clsDB();
            DataTable dtUser = DB.getDS("SELECT * FROM tblUser WHERE Username = '" + usr.Replace("'", "''") + "'", true).Tables[0];
            if (dtUser.Rows.Count == 0)
                return "error";
            else if (Convert.ToBoolean(dtUser.Rows[0]["IsActive"]))
            {
                string url = "https://" + config.PortalClient().ToLower() + "." + config.PortalURL() + "/Login.aspx?rs=" + HttpUtility.UrlEncode(lib.CompressString(lib.cStr(dtUser.Rows[0]["Username"]) + "~" + lib.cStr(dtUser.Rows[0]["Password"]) + "~" + DateTime.Now.AddMinutes(30).ToOADate()));
                string email_body = "Hi " + lib.cStr(dtUser.Rows[0]["Firstname"]) + ",\n\n";
                email_body += "Thank you for using MedHealth Solutions password recovery service.\n\n";
                email_body += "Please use the following URL to reset your password:\n\n";
                email_body += url + " \n\n'";
                email_body += "This link is a time-sensitive and will expire in 30 minutes\n\n";
                email_body += "Have any questions? Just shoot us an email at support@ramtraxs.com. We’re always here to help.\n\n";
                email_body += "Sincerely,\nMedHealth Solutions Support";

                lib.sendEmail(lib.cStr(dtUser.Rows[0]["Email_Address"]), "", "[" + lib.cStr(dtUser.Rows[0]["Username"]) + "] Password Recovery", email_body);
                return "success," + lib.cStr(dtUser.Rows[0]["Email_Address"]) + "," + lib.cStr(dtUser.Rows[0]["Username"]);
            }
            else
                return "inactive," + usr;
        }

        [System.Web.Services.WebMethod]
        public static string[] doLogin(string usr, string pwd)
        {
            string[] arRetrun = new string[2];
            arRetrun[1] = "";
            clsDB DB = new clsDB();
            DataTable dtUser = DB.getDS("SELECT * FROM tblUser WHERE Username = '"+ usr.Replace("'","") +"'",true).Tables[0];
            if (dtUser.Rows.Count==0)
                arRetrun[0]="Invalid username or password";
            else if (dtUser.Rows[0]["Password"].ToString()==pwd)
                if (Convert.ToBoolean(dtUser.Rows[0]["IsActive"]))
                {
                    clsUser Usr = new clsUser(dtUser.Rows[0]);                    
                    DataSet dsPermissions = DB.getDS("um_getPages " + Usr.User_PK + "," + (Usr.IsAdmin ? 1 : 0), true);
                    Usr.setPermissions(dsPermissions, portalVersion.info());
                    if (config.SetConfigSession(dsPermissions.Tables[1], dsPermissions.Tables[2], DB))
                    {
                        if (lib.cBool(dtUser.Rows[0]["IsChangePasswordOnFirstLogin"]))
                            arRetrun[0] = "expired";
                        if (lib.cInt(DB.getDS("SELECT DATEDIFF(day,MAX(dtPassword),GetDate()) FROM tblUserPasswordLog WHERE user_pk=" + Usr.User_PK + "").Tables[0].Rows[0][0]) > config.PasswordLifeInDays())
                            arRetrun[0] = "expired";

                        arRetrun[0] = "success";
                    }
                    else
                        arRetrun[0] = "System not configured, please contact system administrator or support staff";
                }
                else
                    arRetrun[0] = "User locked, please contact system administrator or support staff";
            else
                arRetrun[0] = "Invalid username or password";

            return arRetrun;
        }

        [System.Web.Services.WebMethod]
        public static string verify_session()
        {
            string username = "";
            try { username =HttpContext.Current.Session["Username"].ToString(); }
            catch { username=""; }
            if (username == "")
            {
                clsDB DB = new clsDB();
                if (DB.getDS("SELECT 1 FROM config.appSettings WHERE KeyName='UseSSOTokenForAuthentication' and KeyValue='true'", true).Tables[0].Rows.Count == 1)
                    return "SSO";
            }
            return username;
        }

        [System.Web.Services.WebMethod]
        public static string change_password(string pwd)
        {
            try
            {
                clsDB DB = new clsDB();
                clsUser Usr = new clsUser();
                string sql = "";
                sql = "SELECT * FROM tblUserPasswordLog WHERE user_pk=" + Usr.User_PK + " AND password='" + pwd + "' AND DATEDIFF(day,dtPassword,GetDate())<" + config.PasswordCanbeReusedAfterDays();
                if (DB.getDS(sql).Tables[0].Rows.Count > 0)
                    return "You can not reuse a password within " + config.PasswordCanbeReusedAfterDays() + " days, please use a different password";

                sql = "INSERT INTO tblUserPasswordLog(user_pk,password,dtPassword) VALUES(" + Usr.User_PK + ",'" + pwd + "',GetDate()); ";
                sql += "UPDATE tblUser SET IsChangePasswordOnFirstLogin=0,password='" + pwd + "' WHERE User_PK=" + Usr.User_PK + "; ";
                sql += "SELECT * FROM tblUser WHERE User_PK=" + Usr.User_PK + "; ";

                DataTable dtUser = DB.getDS(sql, true).Tables[0];

                Usr = new clsUser(dtUser.Rows[0]);
                DataSet dsPermissions = DB.getDS("um_getPages " + Usr.User_PK + "," + (Usr.IsAdmin ? 1 : 0), true);
                Usr.setPermissions(dsPermissions,portalVersion.info());
                return "success";
            }
            catch {
                return "error";
            }
        }
    }
}