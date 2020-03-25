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
    public partial class FormPNA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            clsUser User = new clsUser();
            User.validate(MedHealthSolutions.Classes.portalVersion.info());

            //if (User.IsLogin)
            //    Response.Redirect("AccessDenied.aspx?p=" + Request.ServerVariables["SCRIPT_NAME"]);

        }        
    }
}
