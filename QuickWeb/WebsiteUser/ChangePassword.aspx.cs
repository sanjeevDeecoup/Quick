using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace QuickWeb.Website_User
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BID"] == null || Request.QueryString["BID"] == "" || Request.QueryString["CustCode"] == null || Request.QueryString["CustCode"] == "" || Request.QueryString["BranchName"] == null || Request.QueryString["BranchName"] == "" || Request.QueryString["BranchAdress"] == null || Request.QueryString["BranchAdress"] == "")
            {
                Response.Redirect("~/WebsiteUser/UserLogin.aspx", false);
            }
            else
            {
                lblCustCode.Text = Request.QueryString["CustCode"];               
                hdnBranchID.Value = Request.QueryString["BID"];
                ShowLogo();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            string BranchId = Request.QueryString["BID"].ToString();
            string UserName = Request.QueryString["CustCode"].ToString();
            string BranchName = Request.QueryString["BranchName"].ToString();
            string NextPage = "frmUserAndGarmentDetail.aspx?BID=" + BranchId + "&CustCode=" + UserName + "&BranchName=" + BranchName + "&BranchAdress=" + Request.QueryString["BranchAdress"].ToString();
            res = BAL.BALFactory.Instance.BAL_Color.UpdatePassword(BranchId, UserName, txtPwd.Text, txtConfirmPassword.Text);
            if (res == "Record Saved")
            {
                Response.Write("<script language='javascript'>window.alert('Password has Changed');window.location='" + NextPage + "';</script>");
            }
        }

        public void ShowLogo()
        {
            string path = "../ReceiptLogo/DRY" + hdnBranchID.Value + ".jpg";
            StoreLOGO.Src = path;
            StoreLOGO.Height = 120;
            StoreLOGO.Width = 90;
        }
    }
}