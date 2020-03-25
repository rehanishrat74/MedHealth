using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;


namespace MedHealthSolutions.Classes
{
    public static class XL
    {
        private static void formatedCell(ref ExcelWorksheet ws, string range, string text, Color FontColor, bool FontBold, float FontSize, ExcelHorizontalAlignment FontAlign, Color BgColor, ExcelBorderStyle CellBorder)
        {
            try
            {
                ws.Cells[range].Merge = true;
            }
            catch { };
            ws.Cells[range].Value = text;
            ws.Cells[range].Style.Font.Color.SetColor(FontColor);
            ws.Cells[range].Style.Font.SetFromFont(new Font("Arial", FontSize, FontBold ? FontStyle.Bold : FontStyle.Regular));
            ws.Cells[range].Style.HorizontalAlignment=FontAlign;
            ws.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[range].Style.Fill.BackgroundColor.SetColor(BgColor);
            if (CellBorder==ExcelBorderStyle.None)
                ws.Cells[range].Style.Border.BorderAround(CellBorder);
        }

        private static void formatCell(ref ExcelWorksheet ws, string range, Color FontColor, bool FontBold, float FontSize, ExcelHorizontalAlignment FontAlign, Color BgColor, ExcelBorderStyle CellBorder)
        {
            ws.Cells[range].Merge = true;
            ws.Cells[range].Style.Font.Color.SetColor(FontColor);
            ws.Cells[range].Style.Font.SetFromFont(new Font("Arial", FontSize, FontBold ? FontStyle.Bold : FontStyle.Regular));
            ws.Cells[range].Style.HorizontalAlignment = FontAlign;
            ws.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[range].Style.Fill.BackgroundColor.SetColor(BgColor);
            ws.Cells[range].Style.Border.BorderAround(CellBorder);
        }

        public static void prepareDashboard4DownloadXL(int project,HttpResponse oResponse)
        {
            clsUser User = new clsUser();
            clsDB DB = new clsDB();

            DataSet ds = DB.getDS("sr_getExport " + project + ",0,'" + User.Projects + "'", true);

            ExcelPackage xls = new ExcelPackage();

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add("Charts");
            wsScheduled.Column(1).Width = 15;
            wsScheduled.Column(2).Width = 30;
            wsScheduled.Column(3).Width = 10;
            wsScheduled.Column(4).Width = 20;
            wsScheduled.Column(5).Width = 10;
            wsScheduled.Column(6).Width = 20;
            wsScheduled.Column(7).Width = 10;
            wsScheduled.Column(8).Width = 20;
            wsScheduled.Column(9).Width = 15;
            wsScheduled.Column(10).Width = 25;
            wsScheduled.Column(11).Width = 45;
            wsScheduled.Column(12).Width = 15;
            wsScheduled.Column(13).Width = 10;
            wsScheduled.Column(14).Width = 10;
            wsScheduled.Column(15).Width = 20;
            wsScheduled.TabColor = Color.DarkGreen;
            wsScheduled.Cells["A1"].LoadFromDataTable(ds.Tables[0], true, OfficeOpenXml.Table.TableStyles.Medium11);

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + "Dashboard_Details_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareOfficeDrill4Download(int project,int office,int drillType, HttpResponse oResponse)
        {
            clsUser User = new clsUser();
            clsDB DB = new clsDB();

            DataSet ds = DB.getDS("sr_exportOfficeDrill " + project + "," + office, true);
            ExcelPackage xls = new ExcelPackage();

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add("Charts");
            wsScheduled.Column(1).Width = 15;
            wsScheduled.Column(2).Width = 35;
            wsScheduled.Column(3).Width = 15;
            wsScheduled.Column(3).Style.Numberformat.Format = "MM/dd/yyyy";
            wsScheduled.Column(4).Width = 11;
            wsScheduled.Column(5).Width = 11;
            wsScheduled.Column(6).Width = 11;
            wsScheduled.Column(7).Width = 15;
            wsScheduled.Column(8).Width = 50;
            wsScheduled.TabColor = Color.DarkGreen;
            wsScheduled.Cells["A1"].LoadFromDataTable(ds.Tables[0], true, OfficeOpenXml.Table.TableStyles.Medium11);

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + "Office_Charts_Details_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareStatusReport4DownloadXL(string project_group, string channel, string projects, string states, string status1, string status2, string segment, HttpResponse oResponse, string[] arHeader)
        {
            clsUser User = new clsUser();
            clsDB DB = new clsDB();

            DataSet ds = DB.getDS("sr_getExportSummary  '" + project_group + "','" + channel + "','" + projects + "','" + states + "','" + status1 + "','" + status2 + "'," + segment + ", " + User.User_PK, true);

            ExcelPackage xls = new ExcelPackage();
 
            /****************************************************
             * Summary Sheet
             * *************************************************/
            ExcelWorksheet wsSummary = xls.Workbook.Worksheets.Add("Summary");
            wsSummary.Column(1).Width = 35;
            wsSummary.Column(2).Width = 15;
            
            DataRow drSummary = ds.Tables[0].Rows[0];
            formatedCell(ref wsSummary, "A1:B1", "Project Tracking Summary", Color.White, false, 14, ExcelHorizontalAlignment.Center, Color.FromArgb(31, 78, 121), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "A2", "Total Number of Chases", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B2", lib.cStr(drSummary["TotalCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A3:B3", "Summary View Chases", Color.White, false, 12, ExcelHorizontalAlignment.Center, Color.FromArgb(31, 78, 121), ExcelBorderStyle.Thin);
            //Scheduled
            formatedCell(ref wsSummary, "A4", "Scheduled", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B4", lib.cStr(drSummary["ScheduledCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A5", "% Scheduled", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B5", lib.cPercent(drSummary["ScheduledCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            //Not Scheduled
            formatedCell(ref wsSummary, "A6", "Pending (Not Scheduled)", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B6", lib.cStr(lib.cInt(drSummary["TotalCharts"]) - lib.cInt(drSummary["ScheduledCharts"])), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A7", "% Pending (Not Scheduled)", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B7", lib.cPercent(lib.cInt(drSummary["TotalCharts"]) - lib.cInt(drSummary["ScheduledCharts"]), drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Chart Not Available 
            formatedCell(ref wsSummary, "A8", "Closed", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B8", lib.cStr(drSummary["CNACharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A9", "% Closed", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B9", lib.cPercent(drSummary["CNACharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Scanned 
            formatedCell(ref wsSummary, "A10", "Retrieved", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B10", lib.cStr(drSummary["ScannedCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A11", "% Retrieved", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B11", lib.cPercent(drSummary["ScannedCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Audited
            formatedCell(ref wsSummary, "A12", "Coded Complete", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B12", lib.cStr(drSummary["AuditedCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A13", "% Coded Complete", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B13", lib.cPercent(drSummary["AuditedCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Issue Locations
            formatedCell(ref wsSummary, "A14", "Issue Locations", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B14", lib.cStr(drSummary["IssueOffices"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A15", "Charts @ Locations with Issue", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B15", lib.cStr(drSummary["IssueCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A16", "% Charts @ Locations with Issue", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B16", lib.cPercent(drSummary["IssueCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A17:B17", "Data Filters", Color.White, false, 12, ExcelHorizontalAlignment.Center, Color.FromArgb(31, 78, 121), ExcelBorderStyle.Thin);

            //--------------------------------------------------
            int ii=0;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsSummary, "A" + (ii + 18), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsSummary, "B" + (ii + 18), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            //--------------------------------------------------
            
            if (ds.Tables[7].Rows.Count > 0)
            {
                formatedCell(ref wsSummary, "A" + (ii + 18), "Segment", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                formatedCell(ref wsSummary, "B" + (ii + 18), lib.cStr(ds.Tables[7].Rows[0]["segment"]), Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
            }


            formatedCell(ref wsSummary, "A30", "Disclaimer", Color.Black, true, 10, ExcelHorizontalAlignment.Left, Color.White, ExcelBorderStyle.None);
            formatedCell(ref wsSummary, "A31:G31", "Totals will not correspond with latest dashboard metrics as historical Status is logged and kept.", Color.Black, false, 10, ExcelHorizontalAlignment.Left, Color.White, ExcelBorderStyle.None);

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add("Scheduled");
            wsScheduled.TabColor = Color.DarkGreen;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsScheduled, "A" + (ii + 1), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsScheduled, "B" + (ii + 1), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            ii++;
            wsScheduled.Cells["A" + ii].LoadFromDataTable(ds.Tables[1], true, OfficeOpenXml.Table.TableStyles.Medium11);
            for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
            {
                if (ds.Tables[1].Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsScheduled.Column(i + 1).Width = 11;
                    wsScheduled.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsScheduled.Column(i + 1).AutoFit();
            }

            ////Excel HyperLink in first column
            //for (int i = 1; i <= ds.Tables[1].Rows.Count;i++)
            //{
            //    string FileRootPath = "http://www.google.com/" + wsScheduled.Cells["A" + i].Text;
            //    wsScheduled.Cells["A" + i].Formula = "HYPERLINK(\"" + FileRootPath + "\",\"Download Now\")";
            //}

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsNotScheduled = xls.Workbook.Worksheets.Add("Not Scheduled");
            wsNotScheduled.TabColor = Color.DarkGreen;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsNotScheduled, "A" + (ii + 1), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsNotScheduled, "B" + (ii + 1), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            ii++;
            wsNotScheduled.Cells["A"+ii].LoadFromDataTable(ds.Tables[2], true, OfficeOpenXml.Table.TableStyles.Medium11);
            for (int i = 0; i < ds.Tables[2].Columns.Count; i++)
            {
                if (ds.Tables[2].Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsNotScheduled.Column(i + 1).Width = 11;
                    wsNotScheduled.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsNotScheduled.Column(i + 1).AutoFit();
            }

            /****************************************************
             * Issues Sheet
             * *************************************************/
            ExcelWorksheet wsIssues = xls.Workbook.Worksheets.Add("Issues");
            wsIssues.TabColor = Color.DarkRed;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsIssues, "A" + (ii + 1), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsIssues, "B" + (ii + 1), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            ii++;
            wsIssues.Cells["A"+ii].LoadFromDataTable(ds.Tables[3], true, OfficeOpenXml.Table.TableStyles.Medium10);
            for (int i = 0; i < ds.Tables[3].Columns.Count; i++)
            {
                if (ds.Tables[3].Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsIssues.Column(i + 1).Width = 11;
                    wsIssues.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsIssues.Column(i + 1).AutoFit();
            }

            ///****************************************************
            // * Issues Sheet
            // * *************************************************/
            //ExcelWorksheet wsIssuesDetails = xls.Workbook.Worksheets.Add("Issues Details");
            //wsIssuesDetails.Column(1).Width = 15;
            //wsIssuesDetails.Column(2).Width = 25;
            //wsIssuesDetails.Column(3).Width = 10;
            //wsIssuesDetails.Column(4).Width = 50;
            //wsIssuesDetails.Column(5).Width = 50;
            //wsIssuesDetails.Column(6).Width = 50;
            //wsIssuesDetails.TabColor = Color.DarkRed;
            //wsIssuesDetails.Cells["A1"].LoadFromDataTable(ds.Tables[4], true, OfficeOpenXml.Table.TableStyles.Medium10);

            /****************************************************
             * Remove From Chase Sheet
             * *************************************************/
            ExcelWorksheet wsRemove = xls.Workbook.Worksheets.Add("Remove From Chase");
            wsRemove.TabColor = Color.Yellow;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsRemove, "A" + (ii + 1), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsRemove, "B" + (ii + 1), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            ii++;
            wsRemove.Cells["A"+ii].LoadFromDataTable(ds.Tables[4], true, OfficeOpenXml.Table.TableStyles.Medium1);
            for (int i = 0; i < ds.Tables[4].Columns.Count; i++)
            {
                if (ds.Tables[4].Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsRemove.Column(i + 1).Width = 11;
                    wsRemove.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsRemove.Column(i + 1).AutoFit();
            }

            /****************************************************
            * Scanned Sheet
            * *************************************************/
            ExcelWorksheet wsScanned = xls.Workbook.Worksheets.Add("Retrieved");
            wsScanned.TabColor = Color.DarkGreen;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsScanned, "A" + (ii + 1), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsScanned, "B" + (ii + 1), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            ii++;
            wsScanned.Cells["A"+ii].LoadFromDataTable(ds.Tables[5], true, OfficeOpenXml.Table.TableStyles.Medium11);
            for (int i = 0; i < ds.Tables[5].Columns.Count; i++)
            {
                if (ds.Tables[5].Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsScanned.Column(i + 1).Width = 11;
                    wsScanned.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsScanned.Column(i + 1).AutoFit();
            }

            /****************************************************
            * Turnaround Time
            * *************************************************/
            ExcelWorksheet wsTrunaround = xls.Workbook.Worksheets.Add("Turnaround Time");
            wsTrunaround.TabColor = Color.DarkGreen;
            if (arHeader != null)
            {
                for (ii = 0; ii < arHeader.Length; ii++)
                {
                    string[] arRow = arHeader[ii].Split('~');
                    if (arRow.Length > 1)
                    {
                        // + ":G" + (i + 17)
                        formatedCell(ref wsTrunaround, "A" + (ii + 1), arRow[0], Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
                        formatedCell(ref wsTrunaround, "B" + (ii + 1), arRow[1], Color.Black, false, 11, ExcelHorizontalAlignment.Left, Color.LightGray, ExcelBorderStyle.Thin);
                    }
                }
            }
            ii++;
            wsTrunaround.Cells["A"+ii].LoadFromDataTable(ds.Tables[8], true, OfficeOpenXml.Table.TableStyles.Medium11);
            for (int i = 0; i < ds.Tables[8].Columns.Count; i++)
            {
                if (ds.Tables[8].Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsTrunaround.Column(i + 1).Width = 11;
                    wsTrunaround.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsTrunaround.Column(i + 1).AutoFit();
            }

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + "StatusReport_"+ DateTime.Now.ToString("MM_dd_hh_mm") +".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareHEDISReport4DownloadXL(int project, HttpResponse oResponse)
        {
            clsDB DB = new clsDB();

            DataSet ds = DB.getDS("sr_getExportSummary " + project, true);

            ExcelPackage xls = new ExcelPackage();


            /****************************************************
             * Summary Sheet
             * *************************************************/
            ExcelWorksheet wsSummary = xls.Workbook.Worksheets.Add("Summary");
            wsSummary.Column(1).Width = 35;
            wsSummary.Column(2).Width = 15;

            DataRow drSummary = ds.Tables[0].Rows[0];
            formatedCell(ref wsSummary, "A1:B1", "Project Tracking Summary", Color.White, false, 14, ExcelHorizontalAlignment.Center, Color.FromArgb(31, 78, 121), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "A2", "Total Number of Chases", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B2", lib.cStr(drSummary["TotalCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A3:B3", "Summary View Chases", Color.White, false, 12, ExcelHorizontalAlignment.Center, Color.FromArgb(31, 78, 121), ExcelBorderStyle.Thin);
            //Scheduled
            formatedCell(ref wsSummary, "A4", "Scheduled", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B4", lib.cStr(drSummary["ScheduledCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A5", "% Scheduled", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B5", lib.cPercent(drSummary["ScheduledCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            //Not Scheduled
            formatedCell(ref wsSummary, "A6", "Pending (Not Scheduled)", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B6", lib.cStr(lib.cInt(drSummary["TotalCharts"]) - lib.cInt(drSummary["ScheduledCharts"]) - lib.cInt(drSummary["CNACharts"]) - lib.cInt(drSummary["ScannedCharts"]) - lib.cInt(drSummary["AuditedCharts"])), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A7", "% Pending (Not Scheduled)", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B7", lib.cPercent(lib.cInt(drSummary["TotalCharts"]) - lib.cInt(drSummary["ScheduledCharts"]) - lib.cInt(drSummary["CNACharts"]) - lib.cInt(drSummary["ScannedCharts"]) - lib.cInt(drSummary["AuditedCharts"]), drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Chart Not Available 
            formatedCell(ref wsSummary, "A8", "Chart Not Available", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B8", lib.cStr(drSummary["CNACharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A9", "% Chart Not Available", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B9", lib.cPercent(drSummary["CNACharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Scanned 
            formatedCell(ref wsSummary, "A10", "Scanned", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B10", lib.cStr(drSummary["ScannedCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A11", "% Scanned", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B11", lib.cPercent(drSummary["ScannedCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Audited
            formatedCell(ref wsSummary, "A12", "Audited", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B12", lib.cStr(drSummary["AuditedCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A13", "% Audited", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B13", lib.cPercent(drSummary["AuditedCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            //Issue Locations
            formatedCell(ref wsSummary, "A14", "Issue Locations", Color.White, false, 11, ExcelHorizontalAlignment.Right, Color.FromArgb(127, 127, 127), ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B14", lib.cStr(drSummary["OfficeWithNotes"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A15", "Charts @ Locations with Issue", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B15", lib.cStr(drSummary["OfficeWithNotesCharts"]), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            formatedCell(ref wsSummary, "A16", "% Charts @ Locations with Issue", Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);
            formatedCell(ref wsSummary, "B16", lib.cPercent(drSummary["OfficeWithNotesCharts"], drSummary["TotalCharts"], 2), Color.Black, false, 11, ExcelHorizontalAlignment.Right, Color.LightGray, ExcelBorderStyle.Thin);

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add("Scheduled");
            wsScheduled.Column(1).Width = 15;
            wsScheduled.Column(2).Width = 25;
            wsScheduled.Column(3).Width = 30;
            wsScheduled.Column(4).Width = 35;
            wsScheduled.Column(5).Width = 10;
            wsScheduled.Column(6).Width = 10;
            wsScheduled.Column(7).Width = 10;
            wsScheduled.TabColor = Color.DarkGreen;
            wsScheduled.Cells["A1"].LoadFromDataTable(ds.Tables[1], true, OfficeOpenXml.Table.TableStyles.Medium11);

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsNotScheduled = xls.Workbook.Worksheets.Add("Not Scheduled");
            wsNotScheduled.Column(1).Width = 15;
            wsNotScheduled.Column(2).Width = 25;
            wsNotScheduled.Column(3).Width = 30;
            wsNotScheduled.Column(4).Width = 35;
            wsNotScheduled.Column(5).Width = 10;
            wsNotScheduled.Column(6).Width = 10;
            wsNotScheduled.Column(7).Width = 10;
            wsNotScheduled.TabColor = Color.DarkGreen;
            wsNotScheduled.Cells["A1"].LoadFromDataTable(ds.Tables[2], true, OfficeOpenXml.Table.TableStyles.Medium11);

            /****************************************************
             * Issues Sheet
             * *************************************************/
            ExcelWorksheet wsIssues = xls.Workbook.Worksheets.Add("Issues");
            wsIssues.Column(1).Width = 15;
            wsIssues.Column(2).Width = 25;
            wsIssues.Column(3).Width = 30;
            wsIssues.Column(5).Width = 10;
            wsIssues.TabColor = Color.DarkRed;
            wsIssues.Cells["A1"].LoadFromDataTable(ds.Tables[3], true, OfficeOpenXml.Table.TableStyles.Medium10);

            /****************************************************
             * Issues Sheet
             * *************************************************/
            ExcelWorksheet wsIssuesDetails = xls.Workbook.Worksheets.Add("Issues Details");
            wsIssuesDetails.Column(1).Width = 15;
            wsIssuesDetails.Column(2).Width = 25;
            wsIssuesDetails.Column(3).Width = 10;
            wsIssuesDetails.Column(4).Width = 50;
            wsIssuesDetails.Column(5).Width = 50;
            wsIssuesDetails.Column(6).Width = 50;
            wsIssuesDetails.TabColor = Color.DarkRed;
            wsIssuesDetails.Cells["A1"].LoadFromDataTable(ds.Tables[4], true, OfficeOpenXml.Table.TableStyles.Medium10);

            /****************************************************
             * Remove From Chase Sheet
             * *************************************************/
            ExcelWorksheet wsRemove = xls.Workbook.Worksheets.Add("Remove From Chase");
            wsRemove.Column(1).Width = 15;
            wsRemove.Column(2).Width = 25;
            wsRemove.Column(3).Width = 30;
            wsRemove.Column(4).Width = 50;
            wsRemove.Column(5).Width = 10;
            wsRemove.TabColor = Color.Yellow;
            wsRemove.Cells["A1"].LoadFromDataTable(ds.Tables[5], true, OfficeOpenXml.Table.TableStyles.Medium1);

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + "StatusReport_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void XprepareSchedulerList4DownloadXL(ref DataTable dt, HttpResponse oResponse)
        {
            ExcelPackage xls = new ExcelPackage();

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add("Offices");
            wsScheduled.Column(1).Width = 10;
            wsScheduled.Column(2).Width = 100;
            wsScheduled.Column(3).Width = 25;
            wsScheduled.Column(4).Width = 25;
            wsScheduled.Column(5).Width = 10;
            wsScheduled.Column(6).Width = 10;
            wsScheduled.Column(7).Width = 35;
            wsScheduled.Column(8).Width = 25;
            wsScheduled.Column(9).Width = 25;
            wsScheduled.Column(10).Width = 25;
            wsScheduled.Column(11).Width = 25;
            wsScheduled.Column(12).Width = 15;
            wsScheduled.Column(13).Width = 15;
            wsScheduled.Column(14).Width = 20;
            wsScheduled.Column(15).Width = 20;
            wsScheduled.Column(18).Width = 15;
            wsScheduled.Column(19).Width = 15;
            wsScheduled.Column(20).Width = 15;
            wsScheduled.Column(21).Width = 25;
            wsScheduled.Column(22).Width = 25;
            wsScheduled.TabColor = Color.DarkGreen;
            wsScheduled.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium11);

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + "SchedulerReport_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareDownloadXL(ref DataTable dt, HttpResponse oResponse,string output_file)
        {
            ExcelPackage xls = new ExcelPackage();

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add("Exported Data");

            wsScheduled.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium11);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsScheduled.Column(i + 1).Width = 11;
                    wsScheduled.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                }
                else
                    wsScheduled.Column(i + 1).AutoFit();
            }

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareDownloadXL_dt(ref DataTable dt, HttpResponse oResponse, string output_file, string[] arHeader)
        {
            ExcelPackage xls = new ExcelPackage();

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add(output_file);
            if (arHeader != null)
            {
                for (int i = 0; i < arHeader.Length; i++)
                {

                    string[] arRow = arHeader[i].Split('~');
                    if (arRow.Length > 1)
                    {
                        formatedCell(ref wsScheduled, "A" + (i + 1), arRow[0], Color.White, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(155, 187, 89), ExcelBorderStyle.Thin);
                        formatedCell(ref wsScheduled, "B" + (i + 1) + ":G" + (i + 1), arRow[1], Color.Black, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(215, 228, 188), ExcelBorderStyle.Thin);
                    }
                }

                wsScheduled.Cells["A" + (arHeader.Length + 2)].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium11);
            }
            else
            {
                wsScheduled.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium11);
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType.Name.ToString() == "DateTime")
                {
                    wsScheduled.Column(i + 1).Width = 11;
                    wsScheduled.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy"; // HH:mm
                }
                else
                    wsScheduled.Column(i + 1).AutoFit();
            }

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file.Replace(",", "_").Replace(" ", "_") + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareDownloadXL(ref DataTable dt, HttpResponse oResponse, string output_file, string[] arHeader)
        {
            ExcelPackage xls = new ExcelPackage();

            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            ExcelWorksheet wsScheduled = xls.Workbook.Worksheets.Add(output_file);
            if (arHeader != null)
            {
                for (int i = 0; i < arHeader.Length; i++)
                {

                    string[] arRow = arHeader[i].Split('~');
                    if (arRow.Length > 1)
                    {
                        formatedCell(ref wsScheduled, "A" + (i + 1), arRow[0], Color.White, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(155, 187, 89), ExcelBorderStyle.Thin);
                        formatedCell(ref wsScheduled, "B" + (i + 1) + ":G" + (i + 1), arRow[1], Color.Black, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(215, 228, 188), ExcelBorderStyle.Thin);
                    }
                }

                wsScheduled.Cells["A" + (arHeader.Length + 2)].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium11);
            }
            else
            {
                wsScheduled.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium11);
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToLower() == "dob" || dt.Columns[i].ColumnName.ToLower() == "receipt date")
                    wsScheduled.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                else if (dt.Columns[i].DataType.Name.ToString() == "DateTime")
                    wsScheduled.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy HH:mm"; 
                wsScheduled.Column(i + 1).AutoFit();
            }

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file.Replace(",", "_").Replace(" ", "_") + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            oResponse.End();
        }

        public static void prepareDownloadCSV(ref DataTable dt, HttpResponse oResponse, string output_file)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn col in dt.Columns)
            {
                sb.Append("\"" + col.ColumnName + "\",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append("\"" + lib.cStr(row[i]).Replace(Environment.NewLine,"") + "\",");
                }

                sb.Append(Environment.NewLine);
            }

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".csv");
            oResponse.Write(sb);
            oResponse.End();
        }

        public static void prepareCSVFile(DataTable dt, string output_file)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(output_file);
            StringBuilder sb = new StringBuilder();
            string line = "";

            foreach (DataColumn col in dt.Columns)
            {
                line += "\"" + col.ColumnName + "\",";
            }
            file.WriteLine(line);

            foreach (DataRow row in dt.Rows)
            {
                line = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    line += "\"" + lib.cStr(row[i]).Replace(Environment.NewLine, "") + "\",";
                }

                file.WriteLine(line);
            }
            file.Close();
            file.Dispose();
            dt.Dispose();
        }

        public static void prepareDownloadTXT(ref DataTable dt, HttpResponse oResponse, string output_file)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn col in dt.Columns)
            {
                sb.Append("\"" + col.ColumnName + "\"|");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append("\"" + lib.cStr(row[i]).Replace(Environment.NewLine, "") + "\"|");
                }

                sb.Append(Environment.NewLine);
            }

            /****************************************************
             * Download File
             * *************************************************/
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".csv");
            oResponse.Write(sb);
            oResponse.End();
        }

        public static void prepareDownloadXL_Progress(ref DataSet ds, HttpResponse oResponse, string output_file, string[] arHeader,int isOfficeLevelIncluded)
        {
            //string sFile = Path.Combine(Path.Combine(config.TempScannedPath(), "XL"), "prepareDownloadXL_Progress.xlsx");
            //FileInfo fileInfo = new FileInfo(sFile);
            ExcelPackage xls = new ExcelPackage();

            ExcelWorksheet wsSheet;
            bool process_sheet = false;
            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            for (int iSheet = 0; iSheet < ds.Tables.Count; iSheet++)
            {
                if (isOfficeLevelIncluded == iSheet)
                    process_sheet = false;
                else
                    process_sheet = true;
                if (process_sheet)
                {
                    if (isOfficeLevelIncluded == 1 && iSheet == 0)
                    {
                        wsSheet = xls.Workbook.Worksheets.Add("Office List");
                    }
                    else
                        wsSheet = xls.Workbook.Worksheets.Add(lib.cStr(ds.Tables[isOfficeLevelIncluded].Rows[iSheet - 1 - isOfficeLevelIncluded]["Project"]));
                    if (arHeader != null)
                    {
                        for (int i = 0; i < arHeader.Length; i++)
                        {

                            string[] arRow = arHeader[i].Split('~');
                            if (arRow.Length > 1)
                            {
                                formatedCell(ref wsSheet, "A" + (i + 1), arRow[0], Color.White, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(155, 187, 89), ExcelBorderStyle.Thin);
                                formatedCell(ref wsSheet, "B" + (i + 1) + ":G" + (i + 1), arRow[1], Color.Black, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(215, 228, 188), ExcelBorderStyle.Thin);
                            }
                        }

                        wsSheet.Cells["A" + (arHeader.Length + 2)].LoadFromDataTable(ds.Tables[iSheet], true, OfficeOpenXml.Table.TableStyles.Medium11);
                    }
                    else
                    {
                        wsSheet.Cells["A1"].LoadFromDataTable(ds.Tables[iSheet], true, OfficeOpenXml.Table.TableStyles.Medium11);
                    }
                    //wsSheet.Cells["A1"].LoadFromDataTable(ds.Tables[iSheet+1], true, OfficeOpenXml.Table.TableStyles.Medium11);
                    for (int i = 0; i < ds.Tables[iSheet].Columns.Count; i++)
                    {
                        if (ds.Tables[iSheet].Columns[i].DataType.Name.ToString() == "DateTime")
                        {
                            wsSheet.Column(i + 1).Width = 11;
                            wsSheet.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                        }
                        else
                            wsSheet.Column(i + 1).AutoFit();
                    }
                }
            }
            /****************************************************
             * Download File
             * *************************************************/            
            ds.Dispose();

            //lib.downloadFile(oResponse, sFile);
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file.Replace(" ", "_") + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            xls.Dispose();
            oResponse.End();

        }

        public static void prepareDownloadXL_InvoiceVendor(ref DataSet ds, HttpResponse oResponse, string output_file, string[] arHeader,int PaymentMaster)
        {
            ExcelPackage xls = new ExcelPackage();
            DataTable dt = ds.Tables[2];
            ExcelWorksheet wsSheet;
            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            string sTabName = "";
            for (int iSheet = 2; iSheet < ds.Tables.Count; iSheet++)
            {
                sTabName = lib.cStr(ds.Tables[1].Rows[iSheet - 2]["TabName"]);
                wsSheet = xls.Workbook.Worksheets.Add(sTabName);
                if (sTabName != "Summary" && sTabName != "Detail")
                {      
                    if (PaymentMaster==4)              
                        arHeader = new string[13];  
                    else
                        arHeader = new string[10];

                    arHeader[0] = "Payee Information~" + lib.cStr(dt.Rows[iSheet - 3]["Payee Information"]);
                    arHeader[1] = "Street Address~" + lib.cStr(dt.Rows[iSheet - 3]["Street Address"]);
                    arHeader[2] = "City~" + lib.cStr(dt.Rows[iSheet - 3]["City"]);
                    arHeader[3] = "State~" + lib.cStr(dt.Rows[iSheet - 3]["State"]);
                    arHeader[4] = "Zip~" + lib.cStr(dt.Rows[iSheet - 3]["Zip"]);
                    arHeader[5] = "Vendor Phone Number~" + lib.cStr(dt.Rows[iSheet - 3]["Vendor Phone Number"]);
                    arHeader[6] = "Total Number of Chases~" + lib.cStr(dt.Rows[iSheet - 3]["Total Number of Chases"]);
                    arHeader[7] = "Retrieved~" + lib.cStr(dt.Rows[iSheet - 3]["Retrieved"]);
                    arHeader[8] = "Not Retrieved~" + lib.cStr(dt.Rows[iSheet - 3]["Not Retrieved"]);
                    arHeader[9] = "Total Invoice Amount~" + lib.cStr(dt.Rows[iSheet - 3]["Total Invoice Amount"]);
                    if (PaymentMaster == 4) {
                        arHeader[10] = "Payment Type~" + lib.cStr(ds.Tables[1].Rows[iSheet - 2]["Payment Type"]);
                        arHeader[11] = "Transaction Number~" + lib.cStr(ds.Tables[1].Rows[iSheet - 2]["Check_Transaction_Number"]);
                        arHeader[12] = "Payment Date~" + lib.cDate(ds.Tables[1].Rows[iSheet - 2]["Payment Date"]);
                    }
                }

                if (arHeader != null)
                {
                    for (int i = 0; i < arHeader.Length; i++)
                    {

                        string[] arRow = arHeader[i].Split('~');
                        if (arRow.Length > 1)
                        {
                            formatedCell(ref wsSheet, "A" + (i + 1), arRow[0], Color.White, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(155, 187, 89), ExcelBorderStyle.Thin);
                            formatedCell(ref wsSheet, "B" + (i + 1) + ":G" + (i + 1), arRow[1], Color.Black, false, 10, ExcelHorizontalAlignment.Left, Color.FromArgb(215, 228, 188), ExcelBorderStyle.Thin);
                        }
                    }

                    wsSheet.Cells["A" + (arHeader.Length + 2)].LoadFromDataTable(ds.Tables[iSheet], true, OfficeOpenXml.Table.TableStyles.Medium11);
                }
                else
                {
                    wsSheet.Cells["A1"].LoadFromDataTable(ds.Tables[iSheet], true, OfficeOpenXml.Table.TableStyles.Medium11);
                }
                //wsSheet.Cells["A1"].LoadFromDataTable(ds.Tables[iSheet+1], true, OfficeOpenXml.Table.TableStyles.Medium11);
                for (int i = 0; i < ds.Tables[iSheet].Columns.Count; i++)
                {
                    if (ds.Tables[iSheet].Columns[i].DataType.Name.ToString() == "DateTime")
                    {
                        wsSheet.Column(i + 1).Width = 11;
                        wsSheet.Column(i + 1).Style.Numberformat.Format = "MM/dd/yyyy";
                    }
                    else
                        wsSheet.Column(i + 1).AutoFit();
                }
            }

            /****************************************************
             * Download File
             * *************************************************/
            ds.Dispose();

            //lib.downloadFile(oResponse, sFile);
            oResponse.Clear();
            oResponse.ContentType = "application/vnd.openxmlformats";
            oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file + ".xlsx");
            xls.SaveAs(oResponse.OutputStream);
            xls.Dispose();
            oResponse.End();

        }

        public static void prepareDownloadXL_ProgressXX(ref DataSet ds, HttpResponse oResponse, string output_file, string[] arHeader, int isOfficeLevelIncluded)
        {
            string sFolder = Path.Combine(config.TempScannedPath(), "XL");
            //string sFile = Path.Combine(sFolder, "prepareDownloadXL_Progress.xlsx");

            bool process_sheet = false;
            string SheetName="";
            /****************************************************
             * Scheduled Sheet
             * *************************************************/
            for (int iSheet = 1; iSheet < ds.Tables.Count; iSheet++)
            {
                SheetName = lib.cStr(ds.Tables[0].Rows[iSheet-1]["Project"]);

                prepareCSVFile(ds.Tables[iSheet], Path.Combine(sFolder, SheetName + ".csv"));

            }
            /****************************************************
             * Download File
             * *************************************************/
            ds.Dispose();

            lib.downloadFile(oResponse, Path.Combine(sFolder, SheetName + ".csv"));
            //oResponse.Clear();
            //oResponse.ContentType = "application/vnd.openxmlformats";
            //oResponse.AddHeader("Content-Disposition", "attachment; filename=" + output_file.Replace(" ","_") + "_" + DateTime.Now.ToString("MM_dd_hh_mm") + ".xlsx");
            //xls.SaveAs(oResponse.OutputStream);
            oResponse.End();

        }
    }
}