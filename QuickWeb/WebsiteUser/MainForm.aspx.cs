using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace QuickWeb.Website_User
{
    public partial class MainForm : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Branch"] == null || Request.QueryString["Branch"] == "" || Request.QueryString["UserName"] == null || Request.QueryString["UserName"] == "" || Request.QueryString["BranchName"] == null || Request.QueryString["BranchName"] == "")
            {
                Response.Redirect("~/Website User/UserLogin.aspx", false);
            }
            else
            {

                lblCustomer.Text = Request.QueryString["UserName"].ToString();
                string BranchId = Request.QueryString["Branch"].ToString();
                ds = BAL.BALFactory.Instance.BAL_Color.FindCustomerName(BranchId, lblCustomer.Text);
                lblCustValue.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();

                lblBranch.Text = Request.QueryString["BranchName"].ToString();
                lblAddress.Text = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                lblMobile.Text = ds.Tables[0].Rows[0]["CustomerMobile"].ToString();
                lblEmail.Text = ds.Tables[0].Rows[0]["CustomerEmailId"].ToString();
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {

            string NextPage = "~/Website User/ChangePassword.aspx?Branch=" + Request.QueryString["Branch"].ToString() + "&UserName=" + lblCustomer.Text + "&BranchName=" + lblBranch.Text;
            Response.Redirect(NextPage, false);
        }

        protected void CheckStatus_Click(object sender, EventArgs e)
        {
            string NextPage = "~/Website User/CheckStatus.aspx?Branch=" + Request.QueryString["Branch"].ToString() + "&UserName=" + lblCustomer.Text + "&BranchName=" + lblBranch.Text;
            Response.Redirect(NextPage, false);
        }

        protected void linkLabel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Website User/UserLogin.aspx", false);
        }
    }
}