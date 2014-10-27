using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace QuickWeb.WebsiteUser
{
    public partial class WebsiteUserMain : System.Web.UI.MasterPage
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("~/WebsiteUser/UserLogin.aspx", false);
            }
            DataSet ds = new DataSet();
            if (Request.QueryString["BID"] == null || Request.QueryString["BID"] == "" || Request.QueryString["CustCode"] == null || Request.QueryString["CustCode"] == "" || Request.QueryString["BranchName"] == null || Request.QueryString["BranchName"] == "" || Request.QueryString["BranchAdress"] == null || Request.QueryString["BranchAdress"] == "")
            {
                Response.Redirect("~/WebsiteUser/UserLogin.aspx", false);
            }
            else
            {             
                string BranchId = Request.QueryString["BID"].ToString();
                hdnBranchID.Value = BranchId;
                hdnCustCode.Value = Request.QueryString["CustCode"];
                ds = BAL.BALFactory.Instance.BAL_Color.FindCustomerName(BranchId, Request.QueryString["CustCode"]);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblCustName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                    lblBranch.Text = Request.QueryString["BranchName"].ToString();
                    lblAddress.Text = Request.QueryString["BranchAdress"];
                }
            }
        }
    }
}