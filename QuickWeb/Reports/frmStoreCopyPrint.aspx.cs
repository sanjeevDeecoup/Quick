using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Reports
{
    public partial class frmStoreCopyPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(QuantityandPriceReport.strAllContents);
            hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
            if (Request.QueryString["Jumbo"].ToString() != null && Request.QueryString["Jumbo"].ToString() == "Print")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "helpprint();", true);
            }
        }
    }
}