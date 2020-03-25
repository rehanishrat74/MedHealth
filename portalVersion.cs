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
    public partial class portalVersion
    {
        public static string info()
        {
            try
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}