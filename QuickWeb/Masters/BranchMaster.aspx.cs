using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BranchMaster : System.Web.UI.Page
{
    private DTO.BranchMaster Ob = new DTO.BranchMaster();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
              //  txtBranchPhone.Text = dsMain.Tables[0].Rows[0]["BranchPhone"].ToString();
                txtBranchSlogan.Text = dsMain.Tables[0].Rows[0]["BranchSlogan"].ToString();
                txtBranchCode.Text = dsMain.Tables[0].Rows[0]["BranchCode"].ToString();
                rdrBranch.Checked = (dsMain.Tables[0].Rows[0]["IsFactory"].ToString() == "False" ? true : false);
                rdrFactory.Checked = (dsMain.Tables[0].Rows[0]["IsFactory"].ToString() == "True" ? true : false);
                chkChallan.Checked = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["IsChallan"].ToString());
                txtMobileNo.Text = dsMain.Tables[0].Rows[0]["BranchMobile"].ToString();
                txtEmailId.Text = dsMain.Tables[0].Rows[0]["BranchEmail"].ToString();
                txtBusinessName.Text = dsMain.Tables[0].Rows[0]["BusinessName"].ToString();
                chkOperatingTime.Checked = (dsMain.Tables[0].Rows[0]["IsLoginTime"].ToString() == "True" ? true : false);
                if(dsMain.Tables[0].Rows[0]["IsLoginTime"].ToString() == "True")
                {
                    divTiming.Attributes.Add("style", "display:block");                
                }
                else
                {
                    divTiming.Attributes.Add("style", "display:none");                
                }
                txtStartTime.Text = dsMain.Tables[0].Rows[0]["LoginStartTime"].ToString();
                txtEndTime.Text = dsMain.Tables[0].Rows[0]["LoginEndTime"].ToString();

                txtBranchCode.Focus();
                txtBranchCode.Attributes.Add("onfocus", "javascript:select();");
                if (rdrFactory.Checked)
                {
                    chkChallan.Visible = false;
                }
                else
                {
                    chkChallan.Visible = true;
                }
                hdnBranchId.Value = dsMain.Tables[0].Rows[0]["BranchId"].ToString();

                if (dsMain.Tables[1].Rows.Count > 0)
                    PrjClass.SetItemInDropDown(drpWeekend, dsMain.Tables[1].Rows[0]["weeklyoff"].ToString(), true, false);
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
       // txtBranchPhone.Text = "";
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
            lblErr.Text = "Please enter Store name to save.";
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

    protected void rdrBranch_CheckedChanged(object sender, EventArgs e)
    {
        ChallanApplicable();
    }

    protected void rdrFactory_CheckedChanged(object sender, EventArgs e)
    {
        ChallanApplicable();
    }

    public void ChallanApplicable()
    {
        if (rdrBranch.Checked)
        {
            chkChallan.Visible = true;
            chkChallan.Checked = true;
        }
        else
        {
            chkChallan.Visible = false;
        }
    }

    public DTO.BranchMaster SetValue()
    {
        Ob.BranchName = txtBranchName.Text;
        Ob.BranchCode = txtBranchCode.Text;
        Ob.BranchAddress = txtBranchAddress.Text;
        //Ob.BranchPhone = txtBranchPhone.Text;
        Ob.BranchPhone = "";
        Ob.BranchSlogan = txtBranchSlogan.Text;
        Ob.BranchId = hdnBranchId.Value;
        Ob.BranchEmail = txtEmailId.Text;
        Ob.BranchMobile = txtMobileNo.Text;
        Ob.BusinessName = txtBusinessName.Text;
        Ob.IsLoginTime = (chkOperatingTime.Checked ? "1" : "0");
        Ob.LoginStartTime = txtStartTime.Text;
        Ob.LoginEndTime = txtEndTime.Text;
        Ob.WeeklyOFF = drpWeekend.SelectedItem.Text;
        if (rdrBranch.Checked)
        {
            Ob.IsBF = false;
            if (chkChallan.Checked)
                Ob.IsChallan = true;
            else
                Ob.IsChallan = false;
        }
        else
        {
            Ob.IsBF = true;
            Ob.IsChallan = false;
        }
        return Ob;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtBranchName.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Please enter Store name to save.";
            return;
        }
        string res = string.Empty;
        SetValue();
        Ob.BranchId = hdnBranchId.Value;
        res = BAL.BALFactory.Instance.BL_Branch.UpdateBranch(Ob);
        if (res == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Store information updated sucessfully.";
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
            lblMsg.Text = "Store deleted sucessfully.";
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
       // ShowBranchDetails(grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text);
        ShowBranchDetails(strBranchID);
    }    
}