using System;
using System.Web.UI.WebControls;


namespace QuickWeb.Factory
{
    public partial class frmWorkshopUser : System.Web.UI.Page
    {
        private DTO.UserMaster Ob = new DTO.UserMaster();
        private static string _barCode = string.Empty;
        private static string _Pin = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPageForFactory(this.Page);
                RefreshForm();
            }
        }

        private void BindGrid()
        {
            Ob.BranchId = Globals.BranchID;
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BL_UserMaster.BindWorkShopUserGrid(Ob);
            grdSearchResult.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        public DTO.UserMaster SetValue()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.UserId = txtUserId.Text;
            Ob.UserPassword = txtUserPassword.Text;
            Ob.UserBranchCode = "HO";
            Ob.UserTypeCode = Convert.ToInt32(drpUserType.SelectedItem.Value);
            Ob.UserName = txtUserName.Text;
            Ob.UserAddress = txtUserAddress.Text;
            Ob.UserMobileNumber = txtUserMobile.Text;
            Ob.UserPhoneNumber = txtUserPhone.Text;
            Ob.UserEmailId = txtUserEmailId.Text;
            Ob.UserActive = (chkActive.Checked ? "1" : "0");
            Ob.Updatepassword = (checkUpdatePassword.Checked ? "TRUE" : "FALSE");
            Ob.PreUserBarcode = _barCode;
            Ob.PreUserPin = _Pin;
            Ob.UserBarcode = txtUserbarcode.Text;
            Ob.UserPin = txtUserPin.Text;
            return Ob;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text == "" || txtUserName.Text == "")
            {
                lblErr.Text = "Please enter User Id, Password, UserName to save.";
                return;
            }
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BL_UserMaster.SaveFactoryUser(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = res;
                RefreshForm();
            }
            else
                lblErr.Text = res;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text == "" || txtUserName.Text == "")
            {
                lblErr.Text = "Please enter User Id, Password, UserName to save.";
                return;
            }
            string res = string.Empty;
            SetValue();
            Ob.UserId = hdnId.Value;
            res = BAL.BALFactory.Instance.BL_UserMaster.UpdateWorkshopUser(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Updated";
                RefreshForm();               
            }
            else
                lblErr.Text = res;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                lblErr.Text = "Please Enter some text in User name for searching.";
                return;
            }
            SearchAndShowAll(txtUserName.Text.Trim());
            EnableAndDisableControl();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            SearchAndShowAll("");
        }

        private void SearchAndShowAll(string strUserName)
        {
            Ob.UserName = strUserName;
            Ob.BranchId = Globals.BranchID;
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BL_UserMaster.WorkshopSearchAndShowAll(Ob);
            grdSearchResult.DataBind();           
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            _barCode = ((Label)grdSearchResult.SelectedRow.FindControl("lblUserBarcode")).Text;
            _Pin = ((Label)grdSearchResult.SelectedRow.FindControl("lblUserPin")).Text;
            txtUserId.Text = grdSearchResult.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");            
            EnableAndDisableControl();           
            hdnId.Value = grdSearchResult.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
            PrjClass.SetItemInDropDown(drpUserType, grdSearchResult.SelectedRow.Cells[2].Text, true, false);
            txtUserName.Text = grdSearchResult.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
            txtUserAddress.Text = grdSearchResult.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
            txtUserPhone.Text = grdSearchResult.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
            txtUserMobile.Text = grdSearchResult.SelectedRow.Cells[6].Text.Replace("&nbsp;", "");
            txtUserEmailId.Text = grdSearchResult.SelectedRow.Cells[7].Text.Replace("&nbsp;", "");
            txtUserbarcode.Text = ((Label)grdSearchResult.SelectedRow.FindControl("lblUserBarcode")).Text;
            txtUserPin.Text = ((Label)grdSearchResult.SelectedRow.FindControl("lblUserPin")).Text;
            chkActive.Checked = ((CheckBox)grdSearchResult.SelectedRow.FindControl("chkActive")).Checked;
            txtUserPassword.Attributes.Add("Value", ((Label)grdSearchResult.SelectedRow.FindControl("lblUserPassword")).Text);
            btnSave.Visible = false;
            btnEdit.Visible = true;
            checkUpdatePassword.Visible = true;
            txtUserId.Focus();
            txtUserId.Attributes.Add("onfocus", "javascript:select();");
        }

        protected void grdSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSearchResult.PageIndex = e.NewPageIndex;
            BindGrid();

        }
        protected void RefreshForm()
        {
            txtUserId.Text = "";
            txtUserPassword.Text = "";
            drpUserType.SelectedIndex = -1;
            txtUserName.Text = "";
            txtUserAddress.Text = "";
            txtUserPhone.Text = "";
            txtUserMobile.Text = "";
            txtUserEmailId.Text = "";           
            BindGrid();
            txtUserId.Focus();
            btnSave.Visible = true;
            btnEdit.Visible = false;
            checkUpdatePassword.Visible = false;
            txtUserPin.Text = "";
            txtUserbarcode.Text = "";
            chkActive.Enabled = true;
            txtUserId.Enabled = true;
        }
        public void EnableAndDisableControl()
        {
            if (txtUserId.Text.ToUpper() == "ADMIN")
            {
                chkActive.Enabled = false;
                txtUserId.Enabled = false;
            }
            else
            {
                chkActive.Enabled = true;
                txtUserId.Enabled = true;
            }
        }
        
    }
}