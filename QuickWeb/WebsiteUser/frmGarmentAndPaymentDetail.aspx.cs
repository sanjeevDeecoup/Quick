using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.WebsiteUser
{
    public partial class frmGarmentAndPaymentDetail : System.Web.UI.Page
    {
        DTO.Common Ob = new DTO.Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["BID"] == null || Request.QueryString["BID"] == "" || Request.QueryString["CustCode"] == null || Request.QueryString["CustCode"] == "" || Request.QueryString["BranchName"] == null || Request.QueryString["BranchName"] == "" || Request.QueryString["BN"] == "" || Request.QueryString["BN"] == null)
                {
                    Response.Redirect("~/WebsiteUser/UserLogin.aspx", false);
                }
                else
                {
                    BindGrid(Request.QueryString["BN"].ToString(), Request.QueryString["DueDate"].ToString());
                }
            }
        }

        private void BindGrid(string BookingNumber,string DeliveryDate)
        {
            lblInvoiceNumber.Text = BookingNumber;
            lblDeliveryDate.Text = DeliveryDate;
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_City.DeliveryDetail(Request.QueryString["BID"].ToString(), BookingNumber);
            grdClothDetails.DataSource = ds.Tables[1];
            grdClothDetails.DataBind();
            grdPaymentDetails.DataSource = ds.Tables[4];
            grdPaymentDetails.DataBind();
        }
    }
}