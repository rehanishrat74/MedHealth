using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;

namespace MedHealthSolutions.Classes
{
    public class clsUser
    {
        public clsUser()
        {
            //if (config.UseSSOTokenForAuthentication())
            //{
            //    init_class(HttpContext.Current.Request.ServerVariables["HTTP_AUTHORIZATION"]);
            //}
        }

        public clsUser(DataRow drUser)
        {
            HttpContext.Current.Session["User_PK"] = drUser["User_PK"];
            HttpContext.Current.Session["Username"] = drUser["Username"];
            HttpContext.Current.Session["Firstname"] = drUser["Firstname"];
            HttpContext.Current.Session["Lastname"] = drUser["Lastname"];
            HttpContext.Current.Session["Email_Address"] = drUser["Email_Address"];
            HttpContext.Current.Session["IsAdmin"] = lib.cBool(drUser["IsAdmin"]);
            HttpContext.Current.Session["IsAgent"] = lib.cBool(drUser["IsAgent"]);
            HttpContext.Current.Session["IsManager"] = lib.cBool(drUser["IsManager"]);
                       
            HttpContext.Current.Session["IsActive"] = lib.cBool(drUser["IsActive"]);
        }


        private bool ifExists(string sList, string sVal) {
            if (sList.IndexOf(sVal.ToLower()) > 0)
                return true;
            else
                return false;
        }

        public void validate(string portalVersionInfo) {
            if (!this.IsLogin)
            {
                string tck = "";
                if (HttpContext.Current.Request.QueryString["tck"] != null)
                    tck = "&t=" + HttpContext.Current.Request.QueryString["tck"];
                if (HttpContext.Current.Request.ServerVariables["URL"].ToString().ToLower() == "/default.aspx")
                    HttpContext.Current.Response.Redirect("Login.aspx?" + portalVersionInfo);
                else
                    HttpContext.Current.Response.Redirect("Login.aspx?s=session&u=" + HttpContext.Current.Request.ServerVariables["URL"] + tck + "&" + portalVersionInfo);

            }
        }

        public void setPermissions(DataSet ds,string App_Version)
        {
            DataTable dt = ds.Tables[0];
            DataRow[] tp = dt.Select("parent_pk=0");           
            string mnu;
            string allowed_pages="";
            mnu = "<div id='cssmenu'>";
            mnu += "<ul>#X#";                
                for (int t = 0; t < tp.Length; t++) {
                    if (lib.cBool(tp[t]["isPage"]))
                    {
                        mnu += "   <li><a href='" + lib.cStr(tp[t]["url"]) + "?" + App_Version + "' title='" + lib.cStr(tp[t]["page_caption"]) + "' "+(lib.cStr(tp[t]["url"]).ToLower().IndexOf("insights.aspx") >-1?" target=adv_insights":"") +"><span>" + lib.cStr(tp[t]["page_name"]) + "</span></a></li>";
                        allowed_pages += "," + lib.cStr(tp[t]["url"]).ToLower();
                    }
                    else
                    {
                        mnu += "   <li class='has-sub'><a href='#'><span>" + lib.cStr(tp[t]["page_name"]) + "</span></a>";
                        mnu += "      <ul>";
                        DataRow[] sub = dt.Select("parent_pk=" + lib.cStr(tp[t]["page_pk"]));
                        for (int s = 0; s < sub.Length; s++)
                        {
                            mnu += "   <li><a href='" + lib.cStr(sub[s]["url"]) + "?" + App_Version + "' title='" + lib.cStr(sub[s]["page_caption"]) + "'><span>" + lib.cStr(sub[s]["page_name"]) + "</span></a></li>";
                            allowed_pages += "," + lib.cStr(sub[s]["url"]).ToLower();
                        }
                        mnu += "      </ul>";
                        mnu += "   </li>";
                    }
                }
            //    mnu += "   <li><a href='Login.aspx?clear=y' title='Logout'><span>Logout</span></a></li>";
            //allowed_pages += "," + "Login.aspx";
            mnu += "</ul>";
            
            mnu += "</div>";

            mnu = mnu.Replace("#X#", "");

            HttpContext.Current.Session["mnu"] = lib.CompressString(mnu);
            HttpContext.Current.Session["alp"] = lib.CompressString(allowed_pages);
        }

        public string TOP_Menu
        {
            get { try { return lib.DecompressString(HttpContext.Current.Session["mnu"].ToString()); } catch { return "&nbsp;"; } }
        }

        public string Projects
        {
            get { try { return lib.DecompressString(HttpContext.Current.Session["alpr"].ToString()); } catch { return "0"; } }
        }

        public string UserSession
        {
            get { try { return lib.DecompressString(HttpContext.Current.Session["UsrSession"].ToString()); } catch { return "0"; } }
        }

        public bool IsAllowedPage(string current_page)
        {
                try 
                {
                    current_page = current_page.ToLower().Replace("/mhs/", "");
                    current_page = current_page.Remove(0, 1).ToLower();
                    current_page = current_page.Replace("prospectivedefault.aspx", "default.aspx");
                    current_page = current_page.Replace("sr_stargapsummary.aspx", "default.aspx");
                    if (current_page == "accessdenied.aspx")
                        return true;
                    else if (lib.DecompressString(HttpContext.Current.Session["alp"].ToString()).IndexOf(current_page) > -1)
                        return true;
                    else
                        return false;
                } 
                catch 
                { 
                    return false; 
                } 
        }

        public string User_PK
        {
            get { try { return HttpContext.Current.Session["User_PK"].ToString(); } catch { return "1"; } }
            set { HttpContext.Current.Session["User_PK"] = value; }
        }

        public string Username
        {
            get { return HttpContext.Current.Session["Username"].ToString(); }
            set { HttpContext.Current.Session["Username"] = value; }
        }

        public string Firstname
        {
            get { return HttpContext.Current.Session["Firstname"].ToString(); }
            set { HttpContext.Current.Session["Firstname"] = value; }
        }

        public string Lastname
        {
            get { return HttpContext.Current.Session["Lastname"].ToString(); }
            set { HttpContext.Current.Session["Lastname"] = value; }
        }
        
        public string Fullname
        {
            get { return HttpContext.Current.Session["Lastname"].ToString() + ", " + HttpContext.Current.Session["Firstname"].ToString(); }
        }

        public string Email_Address
        {
            get { return HttpContext.Current.Session["Email_Address"].ToString(); }
            set { HttpContext.Current.Session["Email_Address"] = value; }
        }

        public bool IsAdmin
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsAdmin"]); }
            set { HttpContext.Current.Session["IsAdmin"] = value; }
        }

        public bool IsAgent
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsAgent"]); }
            set { HttpContext.Current.Session["IsAgent"] = value; }
        }

        public bool IsManager
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsManager"]); }
            set { HttpContext.Current.Session["IsManager"] = value; }
        }
        
        public bool IsActive
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsActive"]); }
            set { HttpContext.Current.Session["IsActive"] = value; }
        }

        public bool IsLogin
        {
            get {
                try { return Convert.ToBoolean(HttpContext.Current.Session["IsActive"]); }
                catch { return false; }
            }
        }

        public void clear()
        {
            try {
                clsDB DB = new clsDB();
                DB.executeSQL("UPDATE tblUserSession SET SessionEnd = GETDATE() WHERE SessionEnd IS NULL AND User_PK=" + User_PK);
            }
            catch { }
            HttpContext.Current.Session.Clear();
            var sessionCookie = new HttpCookie("Advance_SessionID", lib.CompressString(HttpContext.Current.Session.SessionID));
            sessionCookie.Domain = config.PortalURL();
            sessionCookie.HttpOnly = true;
            HttpContext.Current.Response.SetCookie(sessionCookie);
        }
    }
}