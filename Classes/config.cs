using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Net.Mail;

namespace MedHealthSolutions.Classes
{
    public class config
    {
        public static bool SetConfigSession(DataTable dtVals, DataTable dtNulls, clsDB DB)
        {
            int i = 0;
            if (dtNulls != null)
            {
                for (i = 0; i < dtNulls.Rows.Count; i++)
                {
                    try
                    {
                        HttpContext.Current.Session[lib.cStr(dtNulls.Rows[i]["KeyName"])] = System.Configuration.ConfigurationManager.AppSettings[lib.cStr(dtNulls.Rows[i]["KeyName"])].ToString();
                        DB.executeSQL("Update [config].[appSettings] SET KeyValue='" + (System.Configuration.ConfigurationManager.AppSettings[lib.cStr(dtNulls.Rows[i]["KeyName"])]) + "' WHERE KeyName='" + lib.cStr(dtNulls.Rows[i]["KeyName"]) + "'");
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            for (i = 0; i < dtVals.Rows.Count; i++)
            {
                HttpContext.Current.Session[lib.cStr(dtVals.Rows[i]["KeyName"])] = lib.cStr(dtVals.Rows[i]["KeyValue"]);
            }

            return true;
        }

        public static void SetFilters(DataSet ds,int start) {
            DataTable dt;
            string output;
            for (int t = start; t <= start+6; t++)
            {
                output = "";
                dt = ds.Tables[t];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                        output += "~";
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        if (c > 0)
                            output += "^";
                        output += lib.cStr(dt.Rows[i][c]);
                    }
                }
                //}

                if (t - start == 0)
                    HttpContext.Current.Session["ChaseList_Filter"] = output;
                else if (t - start == 1)
                    HttpContext.Current.Session["Channel_Filter"] = output;
                else if (t - start == 2)
                    HttpContext.Current.Session["Project_Filter"] = output;
                else if (t - start == 3)
                    HttpContext.Current.Session["State_Filter"] = output;
                else if (t - start == 4)
                    HttpContext.Current.Session["StatusType_Filter"] = output;
                else if (t - start == 5)
                    HttpContext.Current.Session["ChaseStatus_Filter"] = output;
                else if (t - start == 6)
                    HttpContext.Current.Session["ChaseResCode_Filter"] = output;
            }

        }

        public static void checkConfigSession() { 
            if (HttpContext.Current.Session["IsProduction"] == null) {
                clsDB DB = new clsDB();
                SetConfigSession(DB.getDS("SELECT * FROM [config].[appSettings]").Tables[0], null, DB);
            }
        }

        public static bool EnableOtherProvider()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["EnableOtherProvider"]); }
            catch { return false; }
        }

        public static bool Enable3YearDisplay()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["Enable3YearDisplay"]); }
            catch { return false; }
        }

        public static bool EnableSubChaseInChaseManager()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["EnableSubChaseInChaseManager"]); }
            catch { return false; }
        }

        public static bool EnableBAAOption4CoverLetter()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["EnableBAAOption4CoverLetter"]); }
            catch { return false; }
        }

        public static bool PreserveTopFilters()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["PreserveTopFilters"]); }
            catch { return false; }
        }

        public static string ChannelFilterTitle()
        {
            checkConfigSession();
            try { return HttpContext.Current.Session["ChannelFilterTitle"].ToString(); }
            catch { return ""; }
        }

        public static string ProjectFilterTitle()
        {
            checkConfigSession();
            try { return HttpContext.Current.Session["ProjectFilterTitle"].ToString(); }
            catch { return ""; }
        }

        public static bool EnableNonCodeableEvent()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["EnableNonCodeableEvent"]); }
            catch { return false; }
        }

        public static bool UseSSOTokenForAuthentication()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["UseSSOTokenForAuthentication"]); }
            catch { return false; }
        }

        

        public static bool IsProduction() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsProduction"]);}
            catch { return false; }
        }

        public static bool IsTraining() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsTraining"]);}
            catch { return false; }
        }

        public static bool IsProspective() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsProspective"]);}
            catch { return false; }
        }

        public static bool IsAdvanceChartNet()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["IsAdvanceChartNet"]); }
            catch { return false; }
        }

        public static bool IsRetrospective() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsRetrospective"]);}
            catch { return false; }
        }

        public static bool IsHEDIS() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsHEDIS"]);}
            catch { return false; }
        }

        public static string PortalClient() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["PortalClient"]).ToUpper();}
            catch {return "";}
        }

        public static string DisplayName()
        {
            checkConfigSession();
            try { return Convert.ToString(HttpContext.Current.Session["DisplayName"]); }
            catch { return ""; }
        }

        public static string PortalURL() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["PortalURL"]);}
            catch {return "";}
        }

        public static string InsightURL()
        {
            checkConfigSession();
            try { return Convert.ToString(HttpContext.Current.Session["InsightURL"]); }
            catch { return ""; }
        }

        public static string Support_Email() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["Support_Email"]);}
            catch {return "";}
        }
        
                //<!-- Common Paths -->
        public static string ExtractionStore() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["ExtractionStore"]);}
            catch {return "";}
        }

        public static string ScannedPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["ScannedPath"]);}
            catch {return "";}
        }

        public static string ScannedPath_Archive()
        {
            checkConfigSession();
            try { return Convert.ToString(HttpContext.Current.Session["ScannedPath-Archive"]); }
            catch { return ""; }
        }

        public static string ScannedPath_InActive()
        {
            checkConfigSession();
            try { return Convert.ToString(HttpContext.Current.Session["ScannedPath-InActive"]); }
            catch { return ""; }
        }

        public static string FTP_ScannedPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["FTP_ScannedPath"]);}
            catch {return "";}
        }

        public static string TempScannedPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["TempScannedPath"]);}
            catch {return "";}
        }

        public static string WebMapping4ScannedPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["WebMapping4ScannedPath"]);}
            catch {return "";}
        }

        public static string AttachmentPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["AttachmentPath"]);}
            catch {return "";}
        }

        public static string URLProtocol()
        {
            checkConfigSession();
            try { return Convert.ToString(HttpContext.Current.Session["URL_Protocol"]).ToLower(); }
            catch { return "https"; }
        }

        public static string InvoiceAttachmentPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["InvoiceAttachmentPath"]);}
            catch {return "";}
        }

        //<!-- Paths only used for Prospective-->
        public static string ProviderAttachmentPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["ProviderAttachmentPath"]);}
            catch {return "";}
        }

        public static string ProspectiveOutputPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["ProspectiveOutputPath"]);}
            catch {return "";}
        }

        public static string Prospective_AttachmentPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["Prospective_AttachmentPath"]);}
            catch {return "";}
        }

        public static string Prospective_SignPath() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["Prospective_SignPath"]);}
            catch {return "";}
        }

        //<!-- Coding Configrations -->
        
        public static bool IsCodingAllowWithoutImage() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsCodingAllowWithoutImage"]);}
            catch { return false; }
        }
        public static bool IsCPT_Allowed() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsCPT_Allowed"]);}
            catch { return false; }
        }
        public static bool IsCommercial() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsCommercial"]);}
            catch { return false; }
        }
	    public static string ValidateType() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["ValidateType"]);}
            catch {return "";}
        }
		public static int YearsBack() {
            checkConfigSession();
            try {return Convert.ToInt16(HttpContext.Current.Session["YearsBack"]);}
            catch {return 0;}
        }

        public static bool EnableValidationDropdown() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableValidationDropdown"]);}
            catch { return false; }
        }

        public static bool ForceDxErrorFlagEntry() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["ForceDxErrorFlagEntry"]);}
            catch { return false; }
        }

        public static bool EnableFreeTextInDxErrorFlag() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableFreeTextInDxErrorFlag"]);}
            catch { return false; }
        }

        public static bool EnableAllDxErrorFlag()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["EnableAllDxErrorFlag"]); }
            catch { return false; }
        }

        public static bool EnableAutomaticEscalation()
        {
            checkConfigSession();
            try { return Convert.ToBoolean(HttpContext.Current.Session["EnableAutomaticEscalation"]); }
            catch { return false; }
        }

        public static bool EnableChartNote() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableChartNote"]);}
            catch { return false; }
        }

        public static bool EnableFreeTextInChartNote() {
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableFreeTextInChartNote"]);}
            catch { return false; }
        }

        public static bool EnableHistoricalData() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableHistoricalData"]);}
            catch { return false; }
        }

        public static bool EnableValidation4HistoricalData() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableValidation4HistoricalData"]);}
            catch { return false; }
        }

        public static bool EnableExternalData() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableExternalData"]);}
            catch { return false; }
        }

        public static bool EnableValidation4ExternalData() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableValidation4ExternalData"]);}
            catch { return false; }
        }
        
        //<!-- Multi-Level Coding Configrations -->
        public static int AllowedCodingLevels() {
            checkConfigSession();
            try {return Convert.ToInt16(HttpContext.Current.Session["AllowedCodingLevels"]);}
            catch { return 1; }
        }
        public static bool Level2_ForceBlindCoding() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["Level2_ForceBlindCoding"]);}
            catch { return false; }
        }
        public static bool Level2_ParallelAssignment() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["Level2_ParallelAssignment"]);}
            catch { return false; }
        }

        //<!-- Office Group Settings -->
        public static bool EnableOfficeGroup() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["EnableOfficeGroup"]);}
            catch { return false; }
        }
		public static string OfficeGroupTitle() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["OfficeGroupTitle"]);}
            catch {return (EnableOfficeGroup()?"Group":"");}
        }

        //<!-- Other Settings-->
        public static int ProjectGoal() {
            checkConfigSession();
            try {return Convert.ToInt16(HttpContext.Current.Session["ProjectGoal"]);}
            catch { return 0; }
        }
        public static int PasswordLifeInDays()
        {

            checkConfigSession();
            try { return Convert.ToInt16(HttpContext.Current.Session["PasswordLifeInDays"]); }
            catch { return 0; }
        }
        public static int PasswordCanbeReusedAfterDays()
        {
            checkConfigSession();
            try { return Convert.ToInt16(HttpContext.Current.Session["PasswordCanbeReusedAfterDays"]); }
            catch { return 0; }
        }
        public static bool AllowCustomExportInStatusReport() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["AllowCustomExportInStatusReport"]);}
            catch { return false; }
        }
        public static bool IsShowSchedulednCNADashboard() {
            checkConfigSession();
            try {return Convert.ToBoolean(HttpContext.Current.Session["IsShowSchedulednCNADashboard"]);}
            catch { return false; }
        }
        //<!-- SFax Settings -->
		public static string fax_username() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["fax_username"]);}
            catch {return "";}
        }
		public static string fax_Apikey() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["fax_Apikey"]);}
            catch {return "";}
        }
		public static string fax_EncryptionKey() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["fax_EncryptionKey"]);}
            catch {return "";}
        }
		public static string fax_EncryptionInitVector() {
            checkConfigSession();
            try {return Convert.ToString(HttpContext.Current.Session["fax_EncryptionInitVector"]);}
            catch {return "";}
        }

        public static bool IsLevelBlind(int level) {
            checkConfigSession();
            if (level == 1 || (level == 2 && config.Level2_ForceBlindCoding() && config.AllowedCodingLevels() != 2))
                return true;
            else
                return false;        
        }
    }
}