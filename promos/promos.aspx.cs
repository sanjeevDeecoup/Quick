using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace promos
{
    public partial class promos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //First time load of the page, show the first promotional screen
                ClientScript.RegisterStartupScript(this.GetType(), "",
                            @"InitializeSteps(""f0""); ", true);
            }
        }
    }
}