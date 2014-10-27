using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;

namespace QuickWeb.WebsiteUsers
{
    public partial class UserLogin : System.Web.UI.Page
    {
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDrop();
            }


        }

        public void BindDrop()
        {
            drpBranchName.DataSource = BAL.BALFactory.Instance.Bal_Registration.BindDropDown();
            drpBranchName.DataValueField = "BranchId";
            drpBranchName.DataTextField = "BranchName";
            drpBranchName.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string NextPage = "~/ChangePassword.aspx?Branch=" + drpBranchName.SelectedValue + "&UserName=" + txtUserName.Text + "&BranchName=" + drpBranchName.SelectedItem.Text;
            string MainPage = "~/MainMenu.aspx?Branch=" + drpBranchName.SelectedValue + "&UserName=" + txtUserName.Text + "&BranchName=" + drpBranchName.SelectedItem.Text;
            ds = BAL.BALFactory.Instance.Bal_Registration.GetStatus(drpBranchName.SelectedValue, txtUserName.Text, txtPwd.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserStatus"].ToString() == "1")
                    Response.Redirect(NextPage, false);

                else
                {
                    Response.Redirect(MainPage, false);
                }
            }
            else
            {
                lblResult.Text = "UserName Or Password is InValid";




            }

        }

        public void ClearAll()
        {

            txtUserName.Text = "";
            drpBranchName.SelectedIndex = 0;
        }
    }
}