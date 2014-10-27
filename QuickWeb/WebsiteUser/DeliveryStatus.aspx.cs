using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;

namespace QuickWeb.Website_User
{
    public partial class DeliveryStatus : System.Web.UI.Page
    {
        DataSet dsMain = new DataSet();
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BN"] == null || Request.QueryString["BN"] == "")
            {
                Response.Redirect("~/Website User/UserLogin.aspx", false);
            }
            string BookingNumber = Request.QueryString["BN"].ToString();
            string[] data = BookingNumber.Split('-');
            string BNumber = data[0].ToString();
            string BID = data[1].ToString();
            dsMain = BAL.BALFactory.Instance.BAL_Color.BindDeliveryStatus(BNumber, BID);
            grdItemDetails.DataSource = dsMain;
            grdItemDetails.DataBind();
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                AppClass.CalcuateAndSetGridFooter(ref grdItemDetails);
            }  
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Website User/UserLogin.aspx", false);

        }

    }
}