using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Factory
{
    public partial class frmWorkshopDetails : System.Web.UI.Page
    {
        private DTO.BranchMaster Ob = new DTO.BranchMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPageForFactory(this.Page);
                RefreshForm();
                ShowBranchDetails(Globals.BranchID);
            }
        }

        private void BindGrid()
        {
            Ob.BranchId = Globals.BranchID;
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BL_Branch.BindGrid(Ob);
            grdSearchResult.DataBind();
        }

        private void ShowBranchDetails(string BranchId)
        {
            DataSet dsMain = new DataSet();
            try
            {
                Ob.BranchId = BranchId;
                dsMain = BAL.BALFactory.Instance.BL_Branch.FillTextBoxes(Ob);
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    txtBranchName.Text = dsMain.Tables[0].Rows[0]["BranchName"].ToString();
                    txtBranchAddress.Text = dsMain.Tables[0].Rows[0]["BranchAddress"].ToString();                  
                    txtBranchSlogan.Text = dsMain.Tables[0].Rows[0]["BranchSlogan"].ToString();
                    txtBranchCode.Text = dsMain.Tables[0].Rows[0]["BranchCode"].ToString();
                    rdrBranch.Checked = (dsMain.Tables[0].Rows[0]["IsFactory"].ToString() == "False" ? true : false);
                    rdrFactory.Checked = (dsMain.Tables[0].Rows[0]["IsFactory"].ToString() == "True" ? true : false);                
                    txtMobileNo.Text = dsMain.Tables[0].Rows[0]["BranchMobile"].ToString();
                    txtEmailId.Text = dsMain.Tables[0].Rows[0]["BranchEmail"].ToString();
                    txtBusinessName.Text = dsMain.Tables[0].Rows[0]["BusinessName"].ToString();
                    txtBranchCode.Focus();
                    txtBranchCode.Attributes.Add("onfocus", "javascript:select();");                   
                    hdnBranchId.Value = dsMain.Tables[0].Rows[0]["BranchId"].ToString();
                }
            }
            catch (Exception excp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Error ShowBranchDetails(): " + excp.Message;
            }
            finally
            {
            }
        }

        protected void RefreshForm()
        {
            txtBranchName.Text = "";
            txtBranchAddress.Text = "";         
            txtBranchSlogan.Text = "";
            hdnBranchId.Value = "";
            txtBranchCode.Text = "";
            txtEmailId.Text = "";
            txtMobileNo.Text = "";
            txtBusinessName.Text = "";
            txtBranchCode.Focus();
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtBranchName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Please enter workshop name to save.";
                return;
            }
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BL_Branch.SaveBranch(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = res;
                RefreshForm();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
            }
        }

        public DTO.BranchMaster SetValue()
        {
            Ob.BranchName = txtBranchName.Text;
            Ob.BranchCode = txtBranchCode.Text;
            Ob.BranchAddress = txtBranchAddress.Text;           
            Ob.BranchPhone = "";
            Ob.BranchSlogan = txtBranchSlogan.Text;
            Ob.BranchId = hdnBranchId.Value;
            Ob.BranchEmail = txtEmailId.Text;
            Ob.BranchMobile = txtMobileNo.Text;
            Ob.BusinessName = txtBusinessName.Text;           
            Ob.IsBF = true;          
            Ob.IsChallan = false;
           
            return Ob;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtBranchName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Please enter workshop name to save.";
                return;
            }
            string res = string.Empty;
            SetValue();
            Ob.BranchId = hdnBranchId.Value;
            res = BAL.BALFactory.Instance.BL_Branch.UpdateBranch(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Workshop information updated sucessfully.";
                RefreshForm();
                ShowBranchDetails(Globals.BranchID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            Ob.BranchId = hdnBranchId.Value;
            res = BAL.BALFactory.Instance.BL_Branch.DeleteBranch(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Workshop deleted sucessfully.";
                RefreshForm();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
            }
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strBranchID = grdSearchResult.SelectedDataKey.Value.ToString();           
            ShowBranchDetails(strBranchID);
        }    
    }
}