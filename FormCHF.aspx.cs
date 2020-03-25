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
    public partial class FormCHF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());

            //if (User.IsLogin)
            //    Response.Redirect("AccessDenied.aspx?p=" + Request.ServerVariables["SCRIPT_NAME"]);

        }
        
        [System.Web.Services.WebMethod]
        public static string SaveValue(string vlu,string obj)
        {
            string sSQL = "";
            clsDB DB = new clsDB();
            if (obj == "Provider")
            {
                string[] v = vlu.Replace("'", "").Split('~');
                vlu = v[0] + ", " + v[1];
                if (v[2] == "CHF")
                    v[2] = "IsCardiologist";
                else if (v[2] == "COPD")
                    v[2] = "IsPulmonologist";
                else if (v[2] == "PNA")
                    v[2] = "IsMD";

                sSQL = "IF NOT EXISTS (SELECT 1 FROM tblProvider WHERE Lastname+', '+Firstname='"+ vlu + "') INSERT INTO tblProvider(Lastname,Firstname,"+ v[2] + ") Values('" + v[0] + "','" + v[1] + "',1); SELECT Provider_PK,LastName+', '+FirstName ProviderName FROM tblProvider WHERE "+ v[2] + "=1 ORDER BY Lastname,FirstName;";
                return makeList(DB.getDS(sSQL).Tables[0],vlu);
            }
            else {
                vlu = vlu.Replace(",", "");
                sSQL = "IF NOT EXISTS (SELECT 1 FROM tbl" + obj + " WHERE " + obj + "='" + vlu + "') INSERT INTO tbl" + obj + "("+ obj + ") Values('" + vlu + "'); SELECT " + obj + "_PK," + obj + " FROM tbl" + obj + " ORDER BY " + obj + ";";
                return makeList(DB.getDS(sSQL).Tables[0],vlu);
            }
        }

        [System.Web.Services.WebMethod]
        public static string[] InitializeLists(string FormName)
        {
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("[dbo].["+ FormName + "_initialize]");
            string[] arReturn = new string[ds.Tables.Count];
            for (int t = 0; t < ds.Tables.Count; t++) {
                arReturn[t] = makeList(ds.Tables[t], "");
            }
            return arReturn;
        }

        [System.Web.Services.WebMethod]
        public static string CommonInitializeLists()
        {
            clsDB DB = new clsDB();
            StringBuilder sb = new StringBuilder();
            DataTable dt = DB.getDS("[dbo].[common_initialize]").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<label class='check-label'><input type=radio name=ContactNote value='" + lib.cStr(dt.Rows[i]["ContactNote_PK"]) + "'> " + lib.cStr(dt.Rows[i]["ContactNote"]) + "<span class='radio'></span></label>");
            }
            return sb.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string getContactLog(int PK)
        {
            clsDB DB = new clsDB();
            StringBuilder sb = new StringBuilder();
            DataTable dt = DB.getDS("EXEC getContactLog @FormPK="+PK).Tables[0];
            if (dt.Rows.Count == 0)
                return "No log available";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<h6>" + lib.cStr(dt.Rows[i]["Coder"]) + " on " + lib.cDateTime(dt.Rows[i]["dtInsert"]) + "</h6><div>" + lib.cStr(dt.Rows[i]["ContactNote"]) + "</div>");
            }
            return sb.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string saveContactLog(int PK,int NotePK, string additionalDetail)
        {
            clsUser Usr = new clsUser();
            clsDB DB = new clsDB();
            string sql = "EXEC saveContactLog @FormPK="+PK+", @NotePK="+NotePK+", @AdditionDetail='"+additionalDetail+"', @User="+Usr.User_PK;            
            DB.executeSQL(sql);
            return "";
        }


        public static string makeList(DataTable dt,string vlu)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++) {
                sb.Append("<option value='" + lib.cStr(dt.Rows[i][0]) + "'" + (lib.cStr(dt.Rows[i][1]) == vlu ? " selected":"") +">" + lib.cStr(dt.Rows[i][1]) + "</option>");
            }
            return sb.ToString(); ;
        }

        [System.Web.Services.WebMethod]
        public static string savePCP(string PK, string PCP_ID, string Lastname, string Firstname)
        {
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("EXEC common_savePCP @PK=" + PK + ", @PCP_ID='" + PCP_ID + "', @Lastname='" + Lastname + "', @Firstname='" + Firstname + "'", true);
            DB.closeConnection();
            if (ds.Tables.Count == 0)
                return "";
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                return lib.cStr(dr["Provider_PK"]) + "~" + lib.cStr(dr["Provider_ID"]) + "~" + lib.cStr(dr["Firstname"]) + "~" + lib.cStr(dr["Lastname"]);
            }
        }

        [System.Web.Services.WebMethod]
        public static string saveForm(int PK,int Patient_PK,int PCP_PK,string DischargeDate,string Insurance,string FU_Data, string FormName)
        {
            clsUser Usr = new clsUser();
            clsDB DB = new clsDB();
            string sql = "EXEC common_saveForm @PK=" + PK + ", @Patient_PK=" + Patient_PK + ", @PCP_PK=" + PCP_PK + ", @DischargeDate=" + (DischargeDate == "" ? "null" : "'" + DischargeDate + "'") + ", @Insurance=" + (Insurance == "null" ? "null" : "'" + Insurance + "'") + ", @FormName='"+ FormName + "',@User=" + Usr.User_PK;
            //return sql;
            PK = lib.cInt(DB.getDS(sql, true).Tables[0].Rows[0][0]);
            string[] arFU = FU_Data.Split('~');
            StringBuilder sb_sql = new StringBuilder();
            for (int f = 0; f < arFU.Length; f++) {
                string[] arData = arFU[f].Split('^');                
                if (arData[1] != "0")
                {
                    sb_sql.Append("IF NOT EXISTS(SELECT 1 FROM tblForm"+ FormName + " WHERE Form_PK=" + PK + " AND Followup_PK=" + arData[0] + ") INSERT INTO tblForm" + FormName + "(Form_PK,Followup_PK,dtCreated,Created_User_PK) VALUES(" + PK + "," + arData[0] + ",GetDate()," + Usr.User_PK + "); ");
                    sb_sql.Append("UPDATE tblForm" + FormName + " SET dtLastUpdated=GetDate(),LastUpdated_User_PK=" + Usr.User_PK+"");
                    for (int d = 2; d < arData.Length; d++) {
                        string[] arCol = arData[d].Split('|');
                        if (arCol[0].Substring(0, 2) != "Is" && arCol[0].Substring(arCol[0].Length - 2, 2) != "PK")
                        {
                            if (arCol[1]=="null")
                                sb_sql.Append(", " + arCol[0] + "=" + arCol[1]);
                            else
                                sb_sql.Append(", " + arCol[0] + "='" + arCol[1].Replace("'", "") + "'");
                        }
                        else
                        {
                            sb_sql.Append(", " + arCol[0] + "=" + arCol[1]);
                        }
                    }
                    sb_sql.Append(" WHERE Form_PK=" + PK + " AND Followup_PK=" + arData[0] + ";");
                }
                else
                    sb_sql.Append("DELETE FROM tblForm" + FormName + " WHERE Form_PK=" + PK + " AND Followup_PK=" + arData[0] + "; ");
            }
            sb_sql.Append("EXEC saveContactLog @FormPK=" + PK + ", @NotePK=1, @AdditionDetail='', @User=" + Usr.User_PK+";");
            DB.executeSQL(sb_sql.ToString());
            return sb_sql.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string savePatient(string PK, string Patient_ID, string Lastname, string Firstname, string DOB, string PhoneNumber, string AlternatePhoneNumber,string Email)
        {
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("EXEC common_savePatient @PK="+PK+ ", @Patient_ID='" + Patient_ID + "', @Lastname='" + Lastname + "', @Firstname='" + Firstname + "', @DOB=" + (DOB==""?"null":"'"+DOB+"'") + ", @PhoneNumber='" + PhoneNumber + "', @AlternatePhoneNumber='" + AlternatePhoneNumber + "', @Email='" + Email + "'",true);
            DB.closeConnection();
            if (ds.Tables.Count==0)
                return "";
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                return lib.cStr(dr["Patient_PK"]) + "~" + lib.cStr(dr["Patient_ID"]) + "~" + lib.cStr(dr["Firstname"]) + "~" + lib.cStr(dr["Lastname"]) + "~" + lib.cDate(dr["DOB"]) + "~" + lib.cStr(dr["PhoneNumber"]) + "~" + lib.cStr(dr["AlternatePhoneNumber"]) + "~" + lib.cStr(dr["Email"]);
            }
        }
        
        [System.Web.Services.WebMethod]
        public static string buildList(int page, string sSort, string sOrder,string FormName,int isArc,int OnlyFollowup,string Search)
        {            
            clsDB DB = new clsDB();
            int pageSize = 100;
            DataSet ds = DB.getDS("common_getForms @FormName='" + FormName + "',@isArc="+ isArc + ", @OnlyFollowup="+ OnlyFollowup + ", @Search='"+ Search + "'", true);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count==0)
                return "<div class='no-result'>No " + FormName + " forms are available</div>";

            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<table class='rpt_tbl tp_rht'>");
            sbOutput.Append("<tr>");
            sbOutput.Append(lib.getHeader("Patient ID", "PID", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Patient Name", "PN", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Contact Number", "CN", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("PCP ID", "PID", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("PCP Name", "PN", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Discharge Date", "DD", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Last Completed Followup", "LFU", sSort, sOrder, "sortUsr"));
            sbOutput.Append(lib.getHeader("Next Due Followup", "NFU", sSort, sOrder, "sortUsr"));
            sbOutput.Append("</tr>");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbOutput.Append("<tr d='" + lib.cStr(dt.Rows[i]["Form_PK"]) + "'><td>" + lib.cStr(dt.Rows[i]["Patient ID"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Patient Name"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Phone Number"]) + "</td><td>" + lib.cStr(dt.Rows[i]["PCP ID"]) + "</td><td>" + lib.cStr(dt.Rows[i]["PCP Name"]) + "</td><td>" + lib.cDate(dt.Rows[i]["DischargeDate"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Last Followup"]) + "</td><td>" + lib.cStr(dt.Rows[i]["Due Followup"]) + "</td></tr>");
            }
            sbOutput.Append("</table>");
            sbOutput.Append(lib.paging(lib.cInt(ds.Tables[1].Rows[0][0]), pageSize, "getUsrs(#P#,'#A#','" + sSort + "','" + sOrder + "')", page));

            return sbOutput.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string[] getForm(int PK,string FormName)
        {
            string[] arReturn = new string[10];
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("common_getForm @PK=" + PK + ", @FormName='" + FormName + "'", true);
            DataTable dt = ds.Tables[0];
            StringBuilder sbReturn = new StringBuilder();
            sbReturn.Append(lib.cStr(dt.Rows[0]["Patient_PK"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["Patient_ID"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["Lastname"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["Firstname"]));
            sbReturn.Append("~" + lib.cDate(dt.Rows[0]["DOB"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["PhoneNumber"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["AlternatePhoneNumber"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["Email"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["PCP_Provider_PK"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["Provider_ID"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["PCP_Lastname"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["PCP_Firstname"]));
            sbReturn.Append("~" + lib.cDate(dt.Rows[0]["DischargeDate"]));
            sbReturn.Append("~" + lib.cStr(dt.Rows[0]["Insurance_PK"]));

            arReturn[0] = sbReturn.ToString();
            dt = ds.Tables[1];
            
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                sbReturn = new StringBuilder();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    if (c > 0)
                        sbReturn.Append("~");
                    sbReturn.Append(dt.Columns[c].ColumnName + "|" + lib.cStr(dt.Rows[r][c]));
                }
                arReturn[r+1] = sbReturn.ToString();
            }
            return arReturn;
        }
    }
}
