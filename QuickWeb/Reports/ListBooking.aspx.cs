using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Reports
{
    public partial class ListBooking : System.Web.UI.Page
    {
               
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(QuantityandPriceReport.strAllContents);
            hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
            if (Request.QueryString["Bookings"].ToString() != null && Request.QueryString["Bookings"].ToString() == "Print")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "helpprint();", true);
            }
        }
    }
}