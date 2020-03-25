using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MedHealthSolutions.Classes;
using System.Collections.Specialized;
using System.Data;

namespace MedHealthSolutions
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lib.remove_temp_files_image_viewer();

            if (!HttpContext.Current.Request.IsSecureConnection && config.URLProtocol() == "https")
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["URL"]);
            }

            clsUser User = new clsUser();
            //if (!User.IsLogin)
            //{
            //    if (config.UseSSOTokenForAuthentication()) {
            //        //string tokenString = Request.ServerVariables["HTTP_AUTHORIZATION"]; // 
            //        //Brandon Token
            //        //string tokenString = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIyNFdZaW8wdjREVXRSUXZVUlhrSHVOd2lTaHpQclVSLXR5VjJQTGRUcVU0In0.eyJqdGkiOiI2N2IyNDFjYy00NjcxLTRkYmItOGEwMC0yMDcwYjY3ZTdjYWIiLCJleHAiOjE1MTY5Nzk0ODAsIm5iZiI6MCwiaWF0IjoxNTE2OTc5MTgwLCJpc3MiOiJodHRwczovL2tjLWRldi5jZW50YXVyaWhzLmNvbS9hdXRoL3JlYWxtcy9DSFNfSW50ZXJuYWwiLCJhdWQiOiJjbXMtaW50ZXJuYWwtY2xpZW50Iiwic3ViIjoiMWI0MTM2YWUtZGI0Yi00YTY2LThhYTYtZmRlMDFmZDViM2ExIiwidHlwIjoiSUQiLCJhenAiOiJjbXMtaW50ZXJuYWwtY2xpZW50IiwiYXV0aF90aW1lIjoxNTE2OTc5MTgwLCJzZXNzaW9uX3N0YXRlIjoiYTU5ZDhjNWItMmQzNy00ZmNlLTk5YWEtNjYzNzk5MWQ1YjVlIiwiYWNyIjoiMSIsIm5hbWUiOiJCcmFuZG9uIEhQU0ogVXNlciIsInByZWZlcnJlZF91c2VybmFtZSI6ImJyYW5kb25faHBzaiIsImludGVybmFsRmxhZyI6IjEiLCJnaXZlbl9uYW1lIjoiQnJhbmRvbiIsImZhbWlseV9uYW1lIjoiSFBTSiBVc2VyIiwiY2hzQWRtaW4iOiIwIiwiZW1haWwiOiJicmFuZG9uQGhwc2ouY29tIn0.QlZuY0IDYrSYJjmqXTjS_y0YpdyH9S-dJBW2aDaQpihQIkT57a8NcGSAHoX4noa6Zs07FVwe2BDS-v-iZI10v2QuUp9xCBRnjl0ZHwTjuyNiBECNkHx1ozQtEGTi70NRELVpdQr16X5PLNe57OA2naw6nEkLgMxbJdggmV6j7k6FwU_JNCdLwmtZwWHsLrlpkn8ZyTGU52GPhmczIOq_cBAHHR5YzSL2QoOUBggyKUgf3TiSM_c5TSdfYCmZ2OeRz7n5USApAY0OUViGYZxjvk2dqbEf-msPVaNEwd_dDNYvJeorKBb_gBlBoNVqYgIWz5vX07n4TvU7efkoSkIxOQ";
            //        string tokenString = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI4NjFqQm9zcExIc0JaQXNNZzVsTmxrcHRPaTZsdUlUa0tYdVZ5dUdCbHo4In0.eyJqdGkiOiI0NDI3NDIzOC02YTFlLTRhYTUtODM0ZS01YTI2NTQzOGIyOTUiLCJleHAiOjE1Mzc5OTU1ODgsIm5iZiI6MCwiaWF0IjoxNTM3OTk1Mjg4LCJpc3MiOiJodHRwOi8vMTAuMjIyLjEwMi4xMjQ6ODA4MC9hdXRoL3JlYWxtcy9DZW50YXVyaUhTIiwiYXVkIjoiYWR2YW5jZS1jaGFydG5ldC1jbGllbnQiLCJzdWIiOiI3MWJlMTIxMy1iMDE5LTRjNDktYjEzNS1lNTNhNjEyYzFkZTIiLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJhZHZhbmNlLWNoYXJ0bmV0LWNsaWVudCIsImF1dGhfdGltZSI6MTUzNzk5NTI4OCwic2Vzc2lvbl9zdGF0ZSI6ImNiOTRkMWQxLWY1ZDAtNDlmZS1iOTc2LTE1Mjc2ZmQyMTQ2NCIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOltdLCJyZWFsbV9hY2Nlc3MiOnsicm9sZXMiOlsidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhZHZhbmNlLWNoYXJ0bmV0LWNsaWVudCI6eyJyb2xlcyI6WyJBbGxNb2R1bGVzIiwiUm9sZS5DaGFydE5ldC1NYW5hZ2VyIiwiQWxsUm9sZXMiLCJSb2xlLkFsbG93LVVwbG9hZCIsIlJvbGUuQWxsb3ctRG93bmxvYWQiLCJSb2xlLkNoYXJ0TmV0LUFic3RyYWN0b3IiLCJNb2R1bGUuRGFzaGJvYXJkIiwiUm9sZS5DaGFydE5ldC1SZXZpZXdlci1ERSIsIk1vZHVsZS5NaWxlc3RvbmUtTWFuYWdlciIsIlJvbGUuQ2hhcnROZXQtUmV2aWV3ZXIiLCJNb2R1bGUuSEVESVMtQ29uc29sZSIsIk1vZHVsZS5IRURJUy1BYnN0cmFjdGlvbiIsIk1vZHVsZS5IRURJUy1TY2hlZHVsZXIiLCJNb2R1bGUuSEVESVMtSVJSIiwiTW9kdWxlLkFic3RyYWN0b3ItQXNzaWdubWVudCJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwibmFtZSI6IkNoYXJ0bmV0IFRlc3QgVXNlciIsInByZWZlcnJlZF91c2VybmFtZSI6ImNoYXJ0bmV0LnRlc3QiLCJnaXZlbl9uYW1lIjoiQ2hhcnRuZXQiLCJmYW1pbHlfbmFtZSI6IlRlc3QgVXNlciIsImVtYWlsIjoidGVzdDJAdGVzdC5jb20ifQ.orD-JmpJ9CBOEw2755dcGgMaJxeaFSrtMT80vGvkia5tb8A463ZCz5iwQuQxKu2hATTQmRizvSKZXa3IB-5p3TnCH8QIHZiuKPyopfkkd_eR69hFEdbKBsgFxPwD62X1tFB5PwIoqZseOyfEO0sf35pkBoOXOYqtm-Vjm9rTjkcIK6QaSh_wuEImQmUiq695OO5GeCXNGk-O52ZfKUm5nNmnTHdUvYcWKJvqEItEtzv4qcfougQ08psSMSZvQZJ5wVnz3VYEkEQKSPwQC9BGdzR9ZGw-LSxbuJlB0rzCGmX4xLCxjY-d_IAUDUO0rwcJqF3N5lgJl1S8w17aQoeuOQ";
            //        User = new clsUser(tokenString);
            //        if (!User.IsLogin) {
            //            Response.Redirect("Login.aspx?s=token" + HttpContext.Current.Session["Email_Address"]);
            //        }
            //        else
            //        {
            //            //HttpContext.Current.Session["User_PK"] = 1;
            //            //HttpContext.Current.Session["Username"] = token.Claims.First(c => c.Type == "preferred_username").Value;
            //            //HttpContext.Current.Session["Firstname"] = token.Claims.First(c => c.Type == "given_name").Value;
            //            //HttpContext.Current.Session["Lastname"] = token.Claims.First(c => c.Type == "family_name").Value;
            //            //HttpContext.Current.Session["Email_Address"] = token.Claims.First(c => c.Type == "email").Value;
            //            clsDB DB = new clsDB();
            //            DataSet dsPermissions = DB.getDS("EXEC um_getPages_SSO @pages='" + HttpContext.Current.Session["SSO_Pages"] + "',	@Username='" + HttpContext.Current.Session["Username"] + "',	@Firstname='" + HttpContext.Current.Session["Firstname"] + "',	@Lastname='" + HttpContext.Current.Session["Lastname"] + "', @Email='" + HttpContext.Current.Session["Email_Address"] + "', @ACN_Manager=" + (Convert.ToBoolean(HttpContext.Current.Session["IsACN_Manager"]) ? "1" : "0") + ", @ACN_Reviewer=" + (Convert.ToBoolean(HttpContext.Current.Session["IsACN_Reviewer"]) ? "1" : "0") + ", @ACN_Abstractor=" + (Convert.ToBoolean(HttpContext.Current.Session["IsACN_Abstractor"]) ? "1" : "0") + ", @AllowDownload=" + (Convert.ToBoolean(HttpContext.Current.Session["IsAllowDownload"]) ? "1" : "0") + ", @AllowUpload=" + (Convert.ToBoolean(HttpContext.Current.Session["isAllowUpload"]) ? "1" : "0"), true);
            //            HttpContext.Current.Session["User_PK"] = lib.cStr(dsPermissions.Tables[2].Rows[0]["User_PK"]);
            //            User.setPermissions(dsPermissions, portalVersion.info());
            //            config.SetFilters(dsPermissions);
            //        }
            //    }
            //    else { 
            //        string tck = "";
            //        if (Request.QueryString["tck"] != null)
            //            tck = "&t=" + Request.QueryString["tck"];
            //        if (Request.ServerVariables["URL"].ToString().ToLower()=="/default.aspx")
            //            Response.Redirect("Login.aspx?" + MedHealthSolutions.Classes.portalVersion.info());
            //        else
            //            Response.Redirect("Login.aspx?s=session&u=" + Request.ServerVariables["URL"] + tck + "&" + MedHealthSolutions.Classes.portalVersion.info());
            //    }
            //}

            //if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("default.aspx") < 0)
            string page_name = Request.ServerVariables["SCRIPT_NAME"];
            if (page_name.LastIndexOf("/") > 0)
                page_name = page_name.Substring(page_name.LastIndexOf("/") + 1);
            if (!User.IsAllowedPage(page_name))
                Response.Redirect("AccessDenied.aspx?p=" + page_name);

            if (config.UseSSOTokenForAuthentication())
                lblLogout.Text = "<a href='/sso/logout'>Log Out</a>";

            lblFullname.Text = User.Fullname + "<input type=hidden id='user_info' value='" + User.User_PK + "~" + User.Username + "~" + User.Lastname + "~" + User.Firstname + "~" + User.Email_Address + "'>";
            lblMenu.Text = User.TOP_Menu;

            try
            {
                clsDB DB = new clsDB();                
                DB.executeSQL("al_insertUserSessionUrlLog @UserSession=" + User.UserSession + ", @AccessedPage='" + Request.ServerVariables["SCRIPT_NAME"].ToString().Replace("'", "") + "';");
            }
            catch { }                       
        }
    }
}
