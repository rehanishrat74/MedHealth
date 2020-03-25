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
using System.Globalization;
using System.Threading;
using System.Text;
using System.Web.Script.Serialization;
using MedHealthSolutions.Classes;

namespace MedHealthSolutions
{
    public partial class _Default : System.Web.UI.Page
    {
        public string UsrID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());
            UsrID = User.User_PK;
            
            lib.remove_temp_files_dashboard_chart_images();
        }

        [System.Web.Services.WebMethod]
        public static string[] initializeFilters()
        {
            if (config.UseSSOTokenForAuthentication())
            {
                clsDB DB = new clsDB();
                DataSet dsPermissions = DB.getDS("EXEC um_initializeFilters 1,1"); 
                config.SetFilters(dsPermissions, 0);
            }
            string[] sReturn = new string[9];
            try
            {
                sReturn[0] = HttpContext.Current.Session["ChaseList_Filter"].ToString();
                sReturn[1] = HttpContext.Current.Session["Channel_Filter"].ToString();
                sReturn[2] = HttpContext.Current.Session["Project_Filter"].ToString();
                sReturn[3] = HttpContext.Current.Session["State_Filter"].ToString();
                sReturn[4] = HttpContext.Current.Session["StatusType_Filter"].ToString();
                sReturn[5] = HttpContext.Current.Session["ChaseStatus_Filter"].ToString();
                sReturn[6] = HttpContext.Current.Session["ChaseResCode_Filter"].ToString();

                sReturn[7] = config.ChannelFilterTitle();
                sReturn[8] = config.ProjectFilterTitle();
            }
            catch (Exception ex) {
                sReturn[8] = ex.Message;
            }

             return sReturn;
        }

        [System.Web.Services.WebMethod]
        public static string updateCookie(string Cookie)
        {
            clsUser Usr = new clsUser();
            clsDB DB = new clsDB();
            DataSet dsPermissions = DB.getDS("EXEC um_updateCookie @User="+ Usr.User_PK + ",@Cookie='"+ Cookie + "'");
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string getCumulativeProgress(DataTable dt)
        {
            int chart_height = 255;
            int chart_width = 1200;
            // create the chart
            var chart = new Chart();
            chart.Width = chart_width;
            chart.Height = chart_height;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.BackColor = Color.FromArgb(255, 255, 255);
            chart.ChartAreas.Add(chartArea);

            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 7); //
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 7); //

            //objSeriesCOPD.Points.AddXY(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]));
            Series objSeriesCHF = new Series("CHF");
            Series objSeriesCOPD = new Series("COPD");
            Series objSeriesAMI = new Series("AMI");
            Series objSeriesPNA = new Series("PNA");
            Series objSeriesSEP = new Series("SEP");
            float CHF = 0;
            float COPD = 0;
            float AMI = 0;
            float PNA = 0;
            float SEP = 0;
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                CHF = CHF + lib.cFloat(dt.Rows[r]["CHF"]);
                COPD = COPD + lib.cFloat(dt.Rows[r]["COPD"]);
                AMI = AMI + lib.cFloat(dt.Rows[r]["AMI"]);
                PNA = PNA + lib.cFloat(dt.Rows[r]["PNA"]);
                SEP = SEP + lib.cFloat(dt.Rows[r]["SEPSIS"]);
                objSeriesCHF.Points.AddY(CHF);
                objSeriesCOPD.Points.AddY(COPD);
                objSeriesAMI.Points.AddY(AMI);
                objSeriesPNA.Points.AddY(PNA);
                objSeriesSEP.Points.AddY(SEP);

                objSeriesCHF.Points[objSeriesCHF.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesCOPD.Points[objSeriesCOPD.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesAMI.Points[objSeriesAMI.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesPNA.Points[objSeriesPNA.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesSEP.Points[objSeriesSEP.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
            }

            objSeriesCHF.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesCHF.ChartType = SeriesChartType.Spline;
            objSeriesCHF.BorderWidth = 3;
            chart.Series.Add(objSeriesCHF);

            objSeriesCOPD.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesCOPD.ChartType = SeriesChartType.Spline;
            objSeriesCOPD.BorderWidth = 3;
            chart.Series.Add(objSeriesCOPD);

            objSeriesAMI.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesAMI.ChartType = SeriesChartType.Spline;
            objSeriesAMI.BorderWidth = 3;
            chart.Series.Add(objSeriesAMI);

            objSeriesPNA.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesPNA.ChartType = SeriesChartType.Spline;
            objSeriesPNA.BorderWidth = 3;
            chart.Series.Add(objSeriesPNA);

            objSeriesSEP.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesSEP.ChartType = SeriesChartType.Spline;
            objSeriesSEP.BorderWidth = 3;
            chart.Series.Add(objSeriesSEP);

            string Legends = "CHF,COPD,AMI,PNA,SEPSIS";
            string[] lgd = Legends.Split(',');
            foreach (string legend in lgd)
            {
                Legend l = new Legend(legend);
                l.Alignment = StringAlignment.Near;
                l.LegendStyle = LegendStyle.Column;
                l.Docking = Docking.Right;
                l.Position.Auto = true;
                l.BackColor = Color.Transparent;
                l.Position = new ElementPosition(92, 3, 10, 40);
                l.TextWrapThreshold = 200;

                chart.Legends.Add(l);
            }

            // write out a file
            chart.RenderType = RenderType.ImageMap;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"charts\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"charts\"));
            }
            string chart_img = "CumProg.png";
            try
            {
                chart.SaveImage(HttpContext.Current.Server.MapPath(@"charts\") + chart_img, ChartImageFormat.Png);
            }
            catch { }

            StringBuilder sbOutput = new StringBuilder();

            sbOutput.Append("<center>");
            //sbOutput.Append("<span>Cumulative Progress</span>");
            sbOutput.Append("<div>");
            sbOutput.Append("<img class='prg_bar' src='charts/" + chart_img + "?" + DateTime.Now.ToString("yyMMddmmssfff") + "' width='" + chart_width + "' height='" + chart_height + "' alt='Form Analysis' usemap='#RAP'>" + chart.GetHtmlImageMap("RAP"));
            sbOutput.Append("</div>");
            sbOutput.Append("</center>");

            return sbOutput.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string getMonthlyProgress(DataTable dt)
        {
            int chart_height = 255;
            int chart_width = 1200;
            // create the chart
            var chart = new Chart();
            chart.Width = chart_width;
            chart.Height = chart_height;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.BackColor = Color.FromArgb(255, 255, 255);
            chart.ChartAreas.Add(chartArea);

            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 7); //
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 7); //

            //chartArea.Area3DStyle.Enable3D = true;

            //objSeriesCOPD.Points.AddXY(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]));
            Series objSeriesCHF = new Series("CHF");
            Series objSeriesCOPD = new Series("COPD");
            Series objSeriesAMI = new Series("AMI");
            Series objSeriesPNA = new Series("PNA");
            Series objSeriesSEP = new Series("SEP");
            float CHF = 0;
            float COPD = 0;
            float AMI = 0;
            float PNA = 0;
            float SEP = 0;
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                CHF = CHF + lib.cFloat(dt.Rows[r]["CHF"]);
                COPD = COPD + lib.cFloat(dt.Rows[r]["COPD"]);
                AMI = AMI + lib.cFloat(dt.Rows[r]["AMI"]);
                PNA = PNA + lib.cFloat(dt.Rows[r]["PNA"]);
                SEP = SEP + lib.cFloat(dt.Rows[r]["SEPSIS"]);
                objSeriesCHF.Points.AddY(CHF);
                objSeriesCOPD.Points.AddY(COPD);
                objSeriesAMI.Points.AddY(AMI);
                objSeriesPNA.Points.AddY(PNA);
                objSeriesSEP.Points.AddY(SEP);

                objSeriesCHF.Points[objSeriesCHF.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesCOPD.Points[objSeriesCOPD.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesAMI.Points[objSeriesAMI.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesPNA.Points[objSeriesPNA.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
                objSeriesSEP.Points[objSeriesSEP.Points.Count - 1].AxisLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lib.cInt(dt.Rows[r]["DD_Month"])) + " " + lib.cStr(dt.Rows[r]["DD_Year"]);
            }

            objSeriesCHF.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesCHF.ChartType = SeriesChartType.RangeColumn;
            objSeriesCHF.BorderWidth = 3;
            chart.Series.Add(objSeriesCHF);

            objSeriesCOPD.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesCOPD.ChartType = SeriesChartType.RangeColumn;
            objSeriesCOPD.BorderWidth = 3;
            chart.Series.Add(objSeriesCOPD);

            objSeriesAMI.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesAMI.ChartType = SeriesChartType.RangeColumn;
            objSeriesAMI.BorderWidth = 3;
            chart.Series.Add(objSeriesAMI);

            objSeriesPNA.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesPNA.ChartType = SeriesChartType.RangeColumn;
            objSeriesPNA.BorderWidth = 3;
            chart.Series.Add(objSeriesPNA);

            objSeriesSEP.Font = new Font("Arial", 7, FontStyle.Bold);
            objSeriesSEP.ChartType = SeriesChartType.RangeColumn;
            objSeriesSEP.BorderWidth = 3;
            chart.Series.Add(objSeriesSEP);

            string Legends = "CHF,COPD,AMI,PNA,SEPSIS";
            string[] lgd = Legends.Split(',');
            foreach (string legend in lgd)
            {
                Legend l = new Legend(legend);
                l.Alignment = StringAlignment.Near;
                l.LegendStyle = LegendStyle.Column;
                l.Docking = Docking.Right;
                l.Position.Auto = true;
                l.BackColor = Color.Transparent;
                l.Position = new ElementPosition(92, 3, 10, 40);
                l.TextWrapThreshold = 200;

                chart.Legends.Add(l);
            }

            // write out a file
            chart.RenderType = RenderType.ImageMap;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"charts\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"charts\"));
            }
            string chart_img = "MonthlyProg.png";
            try
            {
                chart.SaveImage(HttpContext.Current.Server.MapPath(@"charts\") + chart_img, ChartImageFormat.Png);
            }
            catch { }

            StringBuilder sbOutput = new StringBuilder();

            sbOutput.Append("<center>");
            //sbOutput.Append("<div>");
            sbOutput.Append("<img class='prg_bar' src='charts/" + chart_img + "?" + DateTime.Now.ToString("yyMMddmmssfff") + "' width='" + chart_width + "' height='" + chart_height + "' alt='Form Analysis' usemap='#RAP'>" + chart.GetHtmlImageMap("RAP"));
            sbOutput.Append("</div>");
            sbOutput.Append("</center>");

            return sbOutput.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string prepareDashboard()
        {
            clsDB DB = new clsDB();
            DataSet ds = DB.getDS("EXEC db_Progress", true);
            DataTable dt;
            StringBuilder sb = new StringBuilder();
            dt = ds.Tables[1];
            sb.Append("<div id='dashboard' class='cnt'>" + getCensus(dt) + "</div>");
            dt = ds.Tables[2];
            sb.Append("<div id='dashboard' class='cnt'><table align=center>");
            sb.Append("<td colspan=2 class=lft><h1>YTD Breakdown</h1></td><td class=s3 rowspan=2></td><td colspan=2 class=lft><h1>Current Month Breakdown</h1></td>");
            sb.Append("<tr>" + getCurrentMonthPie(dt, "YTD"));
            dt = ds.Tables[3];
            sb.Append("" + getCurrentMonthPie(dt, "Current Month") + "</tr></table></div>");
            dt = ds.Tables[0];
            sb.Append("<div id='dashboard' class='cnt'>" + getCumulativeProgress(dt)+ "</div>");
            sb.Append("<div id='dashboard' class='cnt'>" + getMonthlyProgress(dt) + "</div>");
            return sb.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string getCensus(DataTable dt)
        {
            int chart_height = 255;
            int chart_width = 1000;
            // create the chart
            var chart = new Chart();
            chart.Width = chart_width;
            chart.Height = chart_height;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chartArea.BackColor = Color.FromArgb(255, 255, 255);
            chart.ChartAreas.Add(chartArea);
            chart.BackColor = Color.FromArgb(255, 255, 255);

            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 7); //
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 7); //

            chartArea.Area3DStyle.Enable3D = true;
            chartArea.Area3DStyle.PointDepth = 30;

            Series objSeries = new Series("CensusSeries");
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                //Series objSeries = new Series(lib.cStr(dt.Rows[r]["Description"]));
                objSeries.Points.AddY(lib.cFloat(dt.Rows[r]["Forms"]));
                objSeries.ChartType = SeriesChartType.Bar;
                objSeries.Points[objSeries.Points.Count - 1].AxisLabel = lib.cStr(dt.Rows[r]["Description"]);
                objSeries.Points[objSeries.Points.Count - 1].ToolTip = lib.cStr(dt.Rows[r]["Forms"]) + " Forms ";
                if (lib.cStr(dt.Rows[r]["Description"])== "Current Months Census")
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(112, 173, 71);
                else if (lib.cStr(dt.Rows[r]["Description"]) == "Previous Month Census")
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(255, 192, 0);
                else if (lib.cStr(dt.Rows[r]["Description"]) == "YTD Census")
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(68, 114, 196);
                else if (lib.cStr(dt.Rows[r]["Description"]) == "YTD Discharged Census")
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(237, 125, 49);
            }
            objSeries.Palette = ChartColorPalette.Bright;
            chart.Series.Add(objSeries);
            chart.ApplyPaletteColors();

            // write out a file
            chart.RenderType = RenderType.ImageMap;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"charts\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"charts\"));
            }
            string chart_img = "Census.png";
            try
            {
                chart.SaveImage(HttpContext.Current.Server.MapPath(@"charts\") + chart_img, ChartImageFormat.Png);
            }
            catch { }

            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<img class='prg_bar' src='charts/" + chart_img + "?" + DateTime.Now.ToString("yyMMddmmssfff") + "' width='" + chart_width + "' height='" + chart_height + "' alt='Form Analysis' usemap='#Census'>" + chart.GetHtmlImageMap("Census"));

            return sbOutput.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string getCurrentMonthPie(DataTable dt, string MonthlyYTD)
        {
            int chart_height = 300;
            int chart_width = 350;
            // create the chart
            var chart = new Chart();
            chart.Width = chart_width;
            chart.Height = chart_height;

            var chartArea = new ChartArea();

            chart.BackColor = Color.FromArgb(255, 255, 255);
            chart.ChartAreas.Add(chartArea);

            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 7); //
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 7); //

            chartArea.Area3DStyle.Enable3D = true;
            chartArea.Area3DStyle.PointDepth = 200;
            chartArea.Area3DStyle.Inclination = 60;

            Series objSeries = new Series("CensusSeriesPie");
            objSeries.ChartType = SeriesChartType.Pie;
            objSeries["PieLabelStyle"] = "Outside";

            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<th class='s3 vtop'>");
            sbOutput.Append("<table class='rpt_tbl_in'>");

            for (int c = 0; c < dt.Columns.Count; c++)
            {
                objSeries.Points.AddY(lib.cFloat(dt.Rows[0][c]));
                objSeries.Points[objSeries.Points.Count - 1].AxisLabel = dt.Columns[c].ColumnName;
                objSeries.Points[objSeries.Points.Count - 1].ToolTip = lib.cStr(dt.Rows[0][c]) + " Forms " + MonthlyYTD;
                if (dt.Columns[c].ColumnName.ToUpper() == "COPD")
                {
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(66, 112, 193);
                    sbOutput.Append("<tr><th style='background-color:#4270c1;'>" + dt.Columns[c].ColumnName + "</th><th style='background-color:#4270c1;'>" + lib.cStr(dt.Rows[0][c]) + "</th></tr>");
                }
                else if (dt.Columns[c].ColumnName.ToUpper() == "PNA")
                {
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(233, 124, 48);
                    sbOutput.Append("<tr><th style='background-color:#e97c30;'>" + dt.Columns[c].ColumnName + "</th><th style='background-color:#e97c30;'>" + lib.cStr(dt.Rows[0][c]) + "</th></tr>");
                }
                else if (dt.Columns[c].ColumnName.ToUpper() == "CHF")
                {
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(163, 163, 163);
                    sbOutput.Append("<tr><th style='background-color:#a3a3a3;'>" + dt.Columns[c].ColumnName + "</th><th style='background-color:#a3a3a3;'>" + lib.cStr(dt.Rows[0][c]) + "</th></tr>");
                }
                else if (dt.Columns[c].ColumnName.ToUpper() == "AMI")
                {
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(251, 189, 0);
                    sbOutput.Append("<tr><th style='background-color:#fbbd00;'>" + dt.Columns[c].ColumnName + "</th><th style='background-color:#fbbd00;'>" + lib.cStr(dt.Rows[0][c]) + "</th></tr>");
                }
                else if (dt.Columns[c].ColumnName.ToUpper() == "SEPSIS")
                {
                    objSeries.Points[objSeries.Points.Count - 1].Color = Color.FromArgb(90, 153, 211);
                    sbOutput.Append("<tr><th style='background-color:#5a99d3;'>" + dt.Columns[c].ColumnName + "</th><th style='background-color:#5a99d3;'>" + lib.cStr(dt.Rows[0][c]) + "</th></tr>");
                }
            }

            objSeries.Palette = chart.Palette;
            chart.Series.Add(objSeries);
            chart.ApplyPaletteColors();

            // write out a file
            chart.RenderType = RenderType.ImageMap;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"charts\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"charts\"));
            }
            string chart_img = "CensusPie"+ MonthlyYTD + ".png";
            try
            {
                chart.SaveImage(HttpContext.Current.Server.MapPath(@"charts\") + chart_img, ChartImageFormat.Png);
            }
            catch { }
            sbOutput.Append("</table>");
            sbOutput.Append("</td><td>");
            sbOutput.Append("<img class='prg_bar' src='charts/" + chart_img + "?" + DateTime.Now.ToString("yyMMddmmssfff") + "' width='" + chart_width + "' height='" + chart_height + "' alt='Form Analysis' usemap='#CensusPie"+ MonthlyYTD + "'>" + chart.GetHtmlImageMap("CensusPie" + MonthlyYTD));
            sbOutput.Append("</th>");
            return sbOutput.ToString();
        }

    }
}
