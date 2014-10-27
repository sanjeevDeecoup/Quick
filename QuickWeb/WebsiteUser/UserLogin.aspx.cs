using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace QuickWeb.WebsiteUsers
{
    public partial class UserLogin : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDrop();
                bindStoreAddress();               
            }
            ShowLogo();
        }

        public void BindDrop()
        {
            drpBranchName.DataSource = BAL.BALFactory.Instance.BAL_Color.BindDropDown();
            drpBranchName.DataValueField = "BranchId";
            drpBranchName.DataTextField = "BranchName";
            drpBranchName.DataBind();
        }       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string NextPage = "~/WebsiteUser/ChangePassword.aspx?BID=" + drpBranchName.SelectedValue + "&CustCode=" + txtUserName.Text + "&BranchName=" + drpBranchName.SelectedItem.Text + "&BranchAdress=" + lblStoreAddress.Text;
            string MainPage = "~/WebsiteUser/frmUserAndgarmentdetail.aspx?BID=" + drpBranchName.SelectedValue + "&CustCode=" + txtUserName.Text + "&BranchName=" + drpBranchName.SelectedItem.Text + "&BranchAdress=" + lblStoreAddress.Text;
            Boolean blnUserFound = false;
            string Statusid = string.Empty;                   
            string pwd = txtPwd.Text;
            SqlDataReader sdr = null;
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@BranchId", drpBranchName.SelectedValue);
            CMD.Parameters.AddWithValue("@UserName", txtUserName.Text);           
            CMD.Parameters.AddWithValue("@Flag", 14);
            sdr = PrjClass.ExecuteReader(CMD);
            try
            {
                if (sdr.Read())
                {
                    string upwd = "" + sdr.GetValue(3);
                    if (string.Equals(upwd, pwd))
                    {
                        blnUserFound = true;
                        Statusid = "" + sdr.GetValue(4);
                    }
                    else
                    {
                        lblResult.Visible = true;
                        lblResult.Text = "Password does not match";
                    }
                }
                else
                {
                    lblResult.Visible = true;
                    lblResult.Text = "User Id not found.";
                }
            }
            catch (Exception excp)
            {
                lblResult.Text = excp.Message;
            }
            finally
            {               
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            if (blnUserFound)
            {
                Session["UniqueIDPBU"] = Guid.NewGuid().ToString();
                Session["CustCode"] = txtUserName.Text;
                Session["UniqueIDPBU"] = Guid.NewGuid();
                var dict = new Dictionary<string, int>();
                dict.Add(Session["UniqueIDPBU"].ToString(), 0);
                Globals.SessionWiseUserNumber = dict;
                Session["BID"] = drpBranchName.SelectedValue;
                HttpContext.Current.Response.Cookies["UserId"]["uID"] = Globals.StartCount.ToString();
                Globals.BranchID = Session["BID"].ToString();
                if (Statusid == "1")
                {
                    Response.Redirect(NextPage, false);
                }
                else
                {
                    Response.Redirect(MainPage, false);
                }
            }
        }

        public void ClearAll()
        {
            txtUserName.Text = "";
            drpBranchName.SelectedIndex = 0;
        }
        private void bindStoreAddress()
        {
            DataSet dsStore = new DataSet();
            dsStore= BAL.BALFactory.Instance.BAL_Color.GetStoreNameAddress(drpBranchName.SelectedValue);
            lblStoreName.Text = dsStore.Tables[0].Rows[0]["BranchName"].ToString();
            lblStoreAddress.Text = dsStore.Tables[0].Rows[0]["BranchAddress"].ToString();
        }

        public void ShowLogo()
        {                       
            string path = "../ReceiptLogo/DRY" + drpBranchName.SelectedItem.Value + ".jpg";           
            StoreLOGO.Src = path;
            StoreLOGO.Height = 120;
            StoreLOGO.Width = 90;
        }
    }
}