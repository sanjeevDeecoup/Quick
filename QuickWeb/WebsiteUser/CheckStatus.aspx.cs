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
using System.Web.Security;
using System.Configuration;

namespace QuickWeb.Website_User
{
    public partial class CheckStatus : System.Web.UI.Page
    {
        DataSet dsMain = new DataSet();
        string BranchId = string.Empty;
        string CustCode = string.Empty;
        string BranchName = string.Empty;
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Branch"] == null || Request.QueryString["Branch"] == "" || Request.QueryString["UserName"] == null || Request.QueryString["UserName"] == "" || Request.QueryString["BranchName"] == null || Request.QueryString["BranchName"] == "")
            {
                Response.Redirect("~/Website User/UserLogin.aspx", false);
            }
            else
            {
                BranchId = Request.QueryString["Branch"].ToString();
                CustCode = Request.QueryString["UserName"].ToString();
                BranchName = Request.QueryString["BranchName"].ToString();
                dsMain = BAL.BALFactory.Instance.BAL_Color.BindGridCustomerSearch(BranchId, CustCode);
                grdCustomerWiseReport.DataSource = dsMain;
                grdCustomerWiseReport.DataBind();
                if (dsMain.Tables[0].Rows.Count > 0)
                {                   
                    AppClass.CalcuateAndSetGridFooter(ref grdCustomerWiseReport);
                }        
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            //string NextPage = "MainMenu.aspx?Branch=" + BranchId + "&UserName=" + CustCode + "&BranchName=" + BranchName;
            //Response.Redirect(NextPage, false);
            Response.Redirect("~/Website User/UserLogin.aspx", false);

        }
        
    }
}