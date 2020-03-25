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
    public class lib
    {
        public static void downloadFile(HttpResponse oResponse,string File2Download) {
            System.IO.Stream iStream = null;

            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10000];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file name.
            string filename = System.IO.Path.GetFileName(File2Download);

            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(File2Download, System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.Read);


                // Total bytes to read:
                dataToRead = iStream.Length;

                oResponse.ContentType = "application/octet-stream";
                oResponse.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (oResponse.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream.
                        oResponse.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        oResponse.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                oResponse.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
                oResponse.Close();
            }
        }

        public static bool sendEmail(string toEmail, string toPerson, string subject, string message)
        {
            //GMAIL
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("ramtraxs@gmail.com", "Derrick!23");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //toEmail = "imsaawan@gmail.com";
            foreach (string eml in toEmail.Split(','))
            {
                mail.Bcc.Add(new MailAddress(eml, "", System.Text.Encoding.UTF8));
            }
            mail.From = new MailAddress("support@ramtraxs.com", "MedHealth Support", System.Text.Encoding.UTF8);

            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.Normal;

            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //Exception ex2 = ex;
                //string errorMessage = string.Empty;
                //while (ex2 != null)
                //{
                //    errorMessage += ex2.ToString();
                //    ex2 = ex2.InnerException;
                //}
                //Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
        }

        public static bool sendEmail(string toEmail,string bccEmail, string toPerson, string subject, string message)
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("ramtraxs@gmail.com", "Derrick!23");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //toEmail = "imsaawan@gmail.com";
            foreach (string eml in toEmail.Split(','))
            {
                if (eml.Trim()!="")
                    mail.To.Add(new MailAddress(eml, "", System.Text.Encoding.UTF8));
            }
            foreach (string eml in bccEmail.Split(','))
            {
                if (eml.Trim() != "")
                    mail.Bcc.Add(new MailAddress(eml, "", System.Text.Encoding.UTF8));
            }
            mail.From = new MailAddress("support@ramtraxs.com", "MedHealth Support", System.Text.Encoding.UTF8);
            mail.Headers.Add("auto-submitted", "NO");
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.Normal;

            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //Exception ex2 = ex;
                //string errorMessage = string.Empty;
                //while (ex2 != null)
                //{
                //    errorMessage += ex2.ToString();
                //    ex2 = ex2.InnerException;
                //}
                //Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
        }

        public static bool sendEmail(string toEmail, string toPerson, string subject, string message, StringBuilder str) //,string fromEmail, string fromPerson
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("ramtraxs@gmail.com", "Derrick!23");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;


            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //toEmail = "sajid@megaitsolutions.biz";
            mail.To.Add(new MailAddress(toEmail, toPerson, System.Text.Encoding.UTF8));
            mail.From = new MailAddress("support@ramtraxs.com", "MedHealth Support", System.Text.Encoding.UTF8);
            //if (fromEmail!="")
            //    mail.ReplyToList.Add(new MailAddress(fromEmail, fromPerson, System.Text.Encoding.UTF8));

            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = message;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            
            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
            ct.Parameters.Add("method", "REQUEST");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
            mail.AlternateViews.Add(avCal);

            try
            {
                client.Send(mail);
                return true;
            }
            catch //(Exception ex)
            {
                return false;
                //Exception ex2 = ex;
                //string errorMessage = string.Empty;
                //while (ex2 != null)
                //{
                //    errorMessage += ex2.ToString();
                //    ex2 = ex2.InnerException;
                //}
                //Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
        }

        //public static string sendEmail_attachment(string toEmail, string toPerson, string subject, string message, string fromEmail, string fromPerson,string attachment_file)
        //{
        //    SmtpClient client = new SmtpClient();
        //    client.Credentials = new System.Net.NetworkCredential(config.fax_username(), config.fax_password());
        //    client.Port = 26;
        //    //client.Host = "dime61.dizinc.com";
        //    client.Host = "mail.tacticalminc.com";
        //    //client.EnableSsl = false;
        //    //client.UseDefaultCredentials = false;


        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //    //toEmail = "sajid@megaitsolutions.biz";
        //    mail.To.Add(new MailAddress(toEmail, toPerson, System.Text.Encoding.UTF8));
        //    mail.From = new MailAddress(config.fax_username(), "MedHealth", System.Text.Encoding.UTF8);
        //    if (fromEmail != "")
        //        mail.ReplyToList.Add(new MailAddress(fromEmail, fromPerson, System.Text.Encoding.UTF8));

        //    mail.Subject = subject;
        //    mail.SubjectEncoding = System.Text.Encoding.UTF8;
        //    mail.Body = message;
        //    mail.BodyEncoding = System.Text.Encoding.UTF8;
        //    mail.IsBodyHtml = true;
        //    mail.Priority = MailPriority.High;

        //    System.Net.Mail.Attachment attachment;
        //    attachment = new System.Net.Mail.Attachment(attachment_file);
        //    mail.Attachments.Add(attachment);

        //    string sReturn = "";
        //    try
        //    {
        //        client.Send(mail);

        //    }
        //    catch (Exception ex)
        //    {
        //        sReturn = "Email Error" + ex.Message + "-U:"+config.fax_username()+"-P:"+config.fax_password();
        //        //return false;
        //        //Exception ex2 = ex;
        //        //string errorMessage = string.Empty;
        //        //while (ex2 != null)
        //        //{
        //        //    errorMessage += ex2.ToString();
        //        //    ex2 = ex2.InnerException;
        //        //}
        //        //Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
        //    }
        //    try {
        //        File.Delete(attachment_file);
        //    }
        //    catch (Exception ex)
        //    {
        //        sReturn += "<br>Deletion Error" + ex.Message;
        //    }
        //    return sReturn;            
        //}

        public static bool sendEmail_WithAttachment(string toEmail, string toPerson, string subject, string message, string attachment_file,Stream attachementStream)
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("ramtraxs@gmail.com", "Derrick!23");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;


            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(new MailAddress(toEmail, toPerson, System.Text.Encoding.UTF8));
            mail.From = new MailAddress("support@ramtraxs.com", "MedHealth Support", System.Text.Encoding.UTF8);

            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = message;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.High;

            System.Net.Mail.Attachment attachment;
            if (attachementStream!=null)
            {
                attachementStream.Position = 0;
                attachment = new System.Net.Mail.Attachment(attachementStream,attachment_file);
                mail.Attachments.Add(attachment);
            }
            else if (attachment_file != "")
            {
                attachment = new System.Net.Mail.Attachment(attachment_file);
                mail.Attachments.Add(attachment);
            }


            try
            {
                client.Send(mail);

            }
            catch (Exception ex)
            {
                //return false;
                //Exception ex2 = ex;
                //string errorMessage = string.Empty;
                //while (ex2 != null)
                //{
                //    errorMessage += ex2.ToString();
                //    ex2 = ex2.InnerException;
                //}
                //Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
            //try
            //{
            //    File.Delete(attachment_file);
            //}
            //catch { }
            return true;

        }

        public static void remove_temp_files_image_viewer()
        {
            try
            {
                string TempScannedPath = config.TempScannedPath();
                string[] files = Directory.GetFiles(TempScannedPath);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddMinutes(-1))
                        try
                        {
                            fi.Delete();
                        }
                        catch { }
                }                
            }
            catch { }
        }

        public static void remove_temp_files_dashboard_chart_images()
        {
            try
            {
                string TempScannedPath = HttpContext.Current.Server.MapPath(@"charts\");

                string[] files = Directory.GetFiles(TempScannedPath);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddMinutes(-5))
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public static bool cBool(object obj)
        {
            if (obj == DBNull.Value)
                return false;
            else
                return Convert.ToBoolean(obj);
        }

        public static string cStr(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            else
                return Convert.ToString(obj);
        }

        public static string StrDB(string obj)
        {
            return obj.Replace("`","''");
        }

        public static string cPercent(object val,object total,int decimal_limit)
        {
            double v, t;
            if (val == DBNull.Value)
                v = 0;
            else
                v = Convert.ToDouble(val);

            if (total == DBNull.Value)
                t = 0;
            else
                t = Convert.ToDouble(total);

            if (t == 0 || v == 0)
                return "0%";
            else
                return Math.Round(v/t*100,decimal_limit).ToString()+"%";

        }

        public static string cFormatNum(object val, bool isCurrency)
        {
            if (val == DBNull.Value)
                return " ";
            if (isCurrency)
                return String.Format("{0:C0}", Math.Round(Convert.ToDouble(val), 0));
            else
                return Math.Round(Convert.ToDouble(val), 3).ToString();
        }

        public static string cStrDB(object obj)
        {
            if (obj == DBNull.Value)
                return "null";
            else
                return "'" + Convert.ToString(obj).Replace("'", "") + "'";
        }

        public static string cIntDB(object obj)
        {
            if (obj == DBNull.Value)
                return "null";
            else
                return Convert.ToString(obj).Replace("'", "");
        }

        public static string cDateDB(object obj)
        {
            if (obj == DBNull.Value)
                return "null";
            else
                return "#" + Convert.ToDateTime(obj).ToString("MM/dd/yyyy HH:mm") + "#";
        }

        public static string cDateDB_S(object obj)
        {
            if (obj == DBNull.Value)
                return "null";
            else
                return "'" + Convert.ToDateTime(obj).ToString("MM/dd/yyyy HH:mm") + "'";
        }
        
        public static string cDateTime(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            else
                return Convert.ToDateTime(obj).ToString("MM/dd/yyyy HH:mm");
        }

        public static string cTime(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            else
                return Convert.ToDateTime(obj).ToString("HH:mm");
        }

        public static string cDate(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            else
                if (Convert.ToDateTime(obj).Year != 9999)
                    return Convert.ToDateTime(obj).ToString("MM/dd/yyyy");
                else
                    return "cont.";
        }

        public static string cDateFull(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            else
                return Convert.ToDateTime(obj).ToString("ddd, MMM d, yyyy");
        }

        public static string cDateMD(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            else
                return Convert.ToDateTime(obj).ToString("MM/dd");
        }

        public static int cInt(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        public static float cFloat(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            else
                return Convert.ToSingle(obj);
        }

        public static string cTickMember(object obj, object user, object action_date, string tick_type)
        {
            try
            {
                if (obj == DBNull.Value)
                    return "&nbsp;";
                else if (Convert.ToInt16(obj) == 0)
                    return "&nbsp;";
                else
                {
                    string img = "g_tck";
                    if (tick_type == "coded")
                        img = "g_tck";
                    else if (tick_type == "scanned")
                        img = "r_tck";
                    else if (tick_type == "remaining")
                        return "<img src='imgs/r_blk.png' title='Chart is scanned but not coded yet'>";
                    else if (tick_type == "CNA")
                        img = "r_crs";
                    else
                        return "<img src='imgs/g_tck.png' title='" + tick_type + "'>";

                    return "<img src='imgs/" + img + ".png' title='" + Convert.ToString(user) + " on " + Convert.ToDateTime(action_date).ToString("MM/dd/yyyy") + "'>";
                }
            }
            catch { return ""; }
        }

        public static string cRemaining(object obj,string caption)
        {
            try
            {
                if (obj == DBNull.Value)
                    return "&nbsp;";
                else if (Convert.ToInt16(obj) == 0)
                    return "&nbsp;";
                else
                {
                    return "<img src='imgs/r_blk.png' title='" + caption + "'>";
                }
            }
            catch { return "&nbsp;"; }
        }

        public static string cTick(object obj)
        {
            try
            {
                if (obj == DBNull.Value)
                    return "&nbsp;";
                else if (Convert.ToInt16(obj) == 0)
                    return "&nbsp;";
                else
                {
                    return "<img src='imgs/g_tck.png'>";
                }
            }
            catch { return "&nbsp;"; }
        }

        public static string cTick2(object cnt, object cna, object total_charts,string caption)
        {
            try
            {
                if (cnt == DBNull.Value)
                    return "&nbsp;";
                else if (Convert.ToInt16(cnt) == 0)
                    return "&nbsp;";
                else if (Convert.ToInt16(total_charts) - Convert.ToInt16(cna) - Convert.ToInt16(cnt) == 0)
                {
                    return "<img src='imgs/g_tck.png' title='" + Convert.ToInt16(cnt) + " " + caption + "'>";
                }
                else
                {
                    return "<img src='imgs/y_tck.png' title='" + Convert.ToInt16(cnt) + " " + caption + "'>";
                }
            }
            catch { return "&nbsp;"; }
        }

        public static string paging(DataTable dt, int pageSize, int colspan, string onclick, int page, string alpha)
        {
            string alpha1 = "";
            string alpha2 = "";
            if (alpha.Length > 0)
                alpha1 = alpha.Substring(0, 1);
            if (alpha.Length > 1)
                alpha2 = alpha.Substring(1, 1);

            string a1 = "";
            string a2 = "";

            string a1_paging = "";
            string a2_paging = "";
            string num_paging = "";

            int records = 0;
            int i1 = 0;
            int i2 = 0;
            int i = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (alpha.Length == 0) records = records + lib.cInt(dt.Rows[i]["records"]);
                if (lib.cStr(dt.Rows[i]["alpha1"]) != a1)
                {
                    i1 = i1 + 1;
                    a1 = lib.cStr(dt.Rows[i]["alpha1"]);
                    if (a1 == alpha1)
                        a1_paging += "<b>" + a1 + "</b>&nbsp;";
                    else
                        a1_paging += "<a href=\"javascript:onclick=" + onclick.Replace("#P#", "1").Replace("#A#", a1) + "\">" + a1 + "</a>&nbsp;";
                }

                if (alpha1 == lib.cStr(dt.Rows[i]["alpha1"]))
                {
                    if (alpha.Length == 1) records = records + lib.cInt(dt.Rows[i]["records"]);
                    i2 = i2 + 1;
                    a2 = lib.cStr(dt.Rows[i]["alpha2"]);
                    if (a2 == alpha2)
                    {
                        if (alpha.Length == 2) records = records + lib.cInt(dt.Rows[i]["records"]);
                        a2_paging += "<b>" + a2 + "</b>&nbsp;";
                    }
                    else
                        a2_paging += "<a href=\"javascript:onclick=" + onclick.Replace("#P#", "1").Replace("#A#", alpha1 + a2) + "\">" + a2 + "</a>&nbsp;";
                }
            }

            if (alpha.Length > 0)
                a1_paging += "&nbsp;&nbsp;<a href=\"javascript:onclick=" + onclick.Replace("#P#", "1").Replace("#A#", "") + "\">All</a>";

            int pages = Convert.ToInt32(Math.Ceiling(Convert.ToSingle(records) / pageSize));

            string sperator = ".............";
            for (i = 1; i <= pages; i++)
            {
                if (pages<=20 || i <= 3 || i >= pages - 2 || (i >= page - 5 && i <= page + 5))
                {
                    if (page == i)
                        num_paging += "<b>" + i + "</b>&nbsp;";
                    else
                        num_paging += "<a href=\"javascript:onclick=" + onclick.Replace("#P#", i.ToString()).Replace("#A#", alpha1 + alpha2) + "\">" + i + "</a>&nbsp;";

                    sperator = ".............";
                }
                else if (sperator != "")
                {
                    num_paging += sperator;
                    sperator = "";
                }

                //num_paging = num_paging.Replace("....", ".");
            }

            if (records <= pageSize && i1 <= 1 && i2 <= 1) return "";

            string output = "";
            if (colspan == 0)
                output = "<div class='page_td'>";
            else
                output = "<tr><td class='page_td' colspan=" + colspan + ">";
            if (i1 > 1) output += a1_paging + "<br>";
            if (i2 > 1) output += a2_paging + "<br>";
            if (records > pageSize) output += num_paging + "<br>";
            if (colspan == 0)
                output += "</div>";
            else
                output += "</td></tr>";

            return output;
        }

        public static string paging(int records, int pageSize, string onclick, int page)
        {
            int pages = Convert.ToInt32(Math.Ceiling(Convert.ToSingle(records) / pageSize));
            string num_paging = ""; 
            string sperator = ".............";
            for (int i = 1; i <= pages; i++)
            {
                if (pages <= 20 || i <= 3 || i >= pages - 2 || (i >= page - 5 && i <= page + 5))
                {
                    if (page == i)
                        num_paging += "<b>" + i + "</b>&nbsp;";
                    else
                        num_paging += "<a href=\"javascript:" + onclick.Replace("#P#", i.ToString()) + "\">" + i + "</a>&nbsp;";

                    sperator = ".............";
                }
                else if (sperator != "")
                {
                    num_paging += sperator;
                    sperator = "";
                }
            }

            if (records <= pageSize) return "";

            string output = "";
            output = "<div class='page_td'>";
            if (records > pageSize) output += num_paging + "<br>";
            output += "</div>";

            return output;
        }

        public static string getHeader(string sCaption, string sField, string sSort, string sOrder, string sSortFunction)
        {
            return getHeader(sCaption, sField, sSort, sOrder, sSortFunction, "");
        }
        public static string getHeader(string sCaption, string sField, string sSort, string sOrder, string sSortFunction, string extraAttr)
        {
            string sCSS = "";
            string sLinkOrder = "";
            if (sField == sSort)
                if (sOrder == "ASC")
                {
                    sCSS = "class='up'";
                    sLinkOrder = "DESC";
                }
                else
                {
                    sCSS = "class='dwn'";
                    sLinkOrder = "ASC";
                }
            else
            {
                sCSS = "class='sort'";
                sLinkOrder = "ASC";
            }

            return "<th " + sCSS + " onclick=\"" + sSortFunction + "('" + sField + "','" + sLinkOrder + "')\"" + extraAttr + ">" + sCaption + "</th>";
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static void XML2DS(string xml, ref DataSet ds)
        {
            ds = new DataSet();
            byte[] buffer = Encoding.UTF8.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                XmlReader reader = XmlReader.Create(stream);
                ds.ReadXml(reader);
            }
        }

        public static DateTime NextWeekday(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static string getYearHalf(Object obj, bool isPaid)
        {
            if (lib.cInt(obj) == 1)
                return "<sup>" + (isPaid ? "Paid" : "Projected") + "</sup><p>Jan-Jun</p>";
            else if (lib.cInt(obj) == 2)
                return "<sup>" + (isPaid ? "Paid" : "Projected") + "</sup><p>Jul-Aug</p>";
            else
                return "<sup>Paid</sup>";
        }

        public static string getYearHalf2(Object obj, bool isPaid)
        {
            if (lib.cInt(obj) == 1)
                return " [Jan-Jun]";
            else if (lib.cInt(obj) == 2)
                return " [Jul-Aug]";
            else
                return "";
        }

        public static string dispVal(Object obj, bool isPaid, bool isCurrency)
        {
            if (obj == DBNull.Value)
                return "<td> </td>";
            else
                return "<td class='" + (isPaid ? "paid" : "projected") + "'>" + lib.cFormatNum(obj, isCurrency) + "</td>";
        }

        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        public static string[] getExportHeader(clsDB DB, string channel, string project, string projectGroup,string states, string status1, string status2, bool isIncludeStatus)
        {
            string sReturn = "";
            DataSet ds = DB.getDS("rdb_getHeader '" + channel + "','" + project + "','" + projectGroup + "','" + states + "','" + status1 + "','" + status2 + "'", true);
            DataTable dt;
            dt = ds.Tables[2];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    sReturn += "Chase List~";
                else
                    sReturn += ", ";
                sReturn += lib.cStr(dt.Rows[i][0]);
            }

            dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    sReturn += "^Project~";
                else
                    sReturn += ", ";
                sReturn += lib.cStr(dt.Rows[i][0]);
            }

            dt = ds.Tables[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    sReturn += "^LOB~";
                else
                    sReturn += ", ";
                sReturn += lib.cStr(dt.Rows[i][0]);
            }

            dt = ds.Tables[3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    sReturn += "^States~";
                else
                    sReturn += ", ";
                sReturn += lib.cStr(dt.Rows[i][0]);
            }

            if (isIncludeStatus)
            {
                dt = ds.Tables[4];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^Retrieval Chase Status~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }

                dt = ds.Tables[5];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^Retrieval Chase Detail~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }
                ///////////////////////
                dt = ds.Tables[6];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^Invoice Chase Status~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }

                dt = ds.Tables[7];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^Invoice Chase Detail~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }
                ///////////////////////
                dt = ds.Tables[8];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^Coding Chase Status~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }

                dt = ds.Tables[9];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^Coding Chase Detail~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }
                ///////////////////////
                dt = ds.Tables[10];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^QA Coding Chase Status~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }

                dt = ds.Tables[11];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                        sReturn += "^QA Coding Chase Detail~";
                    else
                        sReturn += ", ";
                    sReturn += lib.cStr(dt.Rows[i][0]);
                }
            }
            return sReturn.Split('^');
        }
    }

        

}