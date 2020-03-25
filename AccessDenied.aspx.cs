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
using MedHealthSolutions.Classes;
using System.Globalization;

namespace MedHealthSolutions
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());
        }
    }
}
