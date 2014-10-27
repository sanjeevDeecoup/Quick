using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmPackageMaster : System.Web.UI.Page
    {
        private DTO.PackageMaster Ob = new DTO.PackageMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetAllField();
                SetGridForEntryWhenNoDataIsPresent();
                SetDropDown();
                txtTitle.Focus();
            }
            var btn = Request.Params["__EVENTTARGET"] as string;

            if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSave")
            {
                btnSave_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnUpdate")
            {
                btnUpdate_Click(null, EventArgs.Empty);
            }           
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnDelete")
            {
                btnDelete_Click(null, EventArgs.Empty);
            }           
        }

        private void SetDropDown()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 28);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                PrjClass.SetItemInDropDown(drpServiceTaxType, ds.Tables[0].Rows[0]["InclusiveExclusive"].ToString(), true, false);
            }
        }

        public void ResetAllField()
        {
        //    rdbDiscount.Checked = true;
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            grdEntry.DataSource = BAL.BALFactory.Instance.BL_PackageMaster.ShowAllPackage(Ob);
            grdEntry.DataBind();
            txtDiscount.Text = "";
            foreach (var ctrl in Form.FindControl("ContentPlaceHolder1").Controls)
            {
                switch (ctrl.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.TextBox": ((TextBox)ctrl).Text = "";
                        break;

                    default:
                        break;
                }
            }

            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            btnSave.Visible = true;
           // rdbAmount.Checked = true;
            txtAmount.Enabled = true;
            txtTitle.Enabled = true;
            txtCost.Enabled = true;
            txtAmount.Enabled = true;
            //rdbDisActive.Checked = false;
            //rdbActive.Checked = true;
         
            drpPackageType.SelectedIndex = -1;
            grdBind.DataSource = null;
            grdBind.DataBind();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Now.AddYears(1).ToString("dd MMM yyyy");
            txtTitle.Focus();
        }

        public DTO.PackageMaster IntialiazeValueInGlobal()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.CustomerCode = lblDuplicateItem.Text;
            Ob.PackageId = lblId.Text;
            Ob.PackageName = txtTitle.Text;
            Ob.PackageType = drpPackageType.SelectedItem.Text;
            Ob.PackageCost = float.Parse(txtCost.Text);
           // Ob.BenefitType = "Discount";
            if (drpPackageType.SelectedValue == "2")
            {
                Ob.BenefitType = "Amount";
            }
            else
            {
                Ob.BenefitType = "Discount";
            }
            Ob.Active = "1";   
            Ob.TaxType = drpServiceTaxType.SelectedItem.Text;
            Ob.Recurrence = txtRecurrence.Text;
            Ob.StartDate = txtStartDate.Text;
            Ob.EndDate = txtEndDate.Text;
            try
            {
                Ob.TotalQty = Convert.ToInt32(txtFlatQty.Text);
            }
            catch (Exception ex) { Ob.TotalQty = 0; }
            try
            {
                if (Ob.BenefitType == "Amount")
                    Ob.BenefitValue = float.Parse(txtAmount.Text);
                else
                    Ob.BenefitValue = float.Parse(txtDiscount.Text);
            }
            catch (Exception ex)
            {
                Ob.BenefitValue = 0;
            }
            return Ob;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();
            string res = string.Empty;
            if (Ob.PackageType != "Qty / Item" && Ob.PackageType != "Flat Qty")
            {
                grdBind.DataSource = null;
                grdBind.DataBind();
            }
            res = BAL.BALFactory.Instance.BL_PackageMaster.SavePackage(Ob, grdBind);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package" + " " + txtTitle.Text + " " + "created successfully.";
                ResetAllField();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res.ToString();
            }
            grdBind.DataSource = null;
            grdBind.DataBind();
            SetGridForEntryWhenNoDataIsPresent();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();
            string res = string.Empty;
            if (Ob.PackageType != "Qty / Item" && Ob.PackageType != "Flat Qty")
            {
                grdBind.DataSource = null;
                grdBind.DataBind();
            }

            res = BAL.BALFactory.Instance.BL_PackageMaster.UpdatePackage(Ob, grdBind);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package" + " " + txtTitle.Text + " " + "updated successfully.";
                ResetAllField();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res.ToString();
            }
            SetGridForEntryWhenNoDataIsPresent();
        }

        //protected void btnShowall_Click(object sender, EventArgs e)
        //{
        //    DataSet ds = new DataSet();
        //    Ob.BranchId = Globals.BranchID;
        //    Ob.PackageName = txtTitle.Text;
        //    grdEntry.DataSource = BAL.BALFactory.Instance.BL_PackageMaster.SearchPackage(Ob);
        //    grdEntry.DataBind();
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            Ob.BranchId = Globals.BranchID;
            Ob.PackageId = lblId.Text;
            res = BAL.BALFactory.Instance.BL_PackageMaster.DeletePackage(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package" + " " + txtTitle.Text + " " + "deleted successfully.";
                ResetAllField();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res.ToString();
            }
            SetGridForEntryWhenNoDataIsPresent();
        }
       

        protected void grdEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblId.Text = ((Label)grdEntry.SelectedRow.FindControl("lblPackageId")).Text;
            txtTitle.Text = grdEntry.SelectedRow.Cells[3].Text == "&nbsp;" ? "" : grdEntry.SelectedRow.Cells[3].Text;
            txtCost.Text = grdEntry.SelectedRow.Cells[4].Text == "&nbsp;" ? "" : grdEntry.SelectedRow.Cells[4].Text;
            PrjClass.SetItemInDropDown(drpPackageType, grdEntry.SelectedRow.Cells[2].Text, true, false);
            string BenefitType = string.Empty, Active = string.Empty;
            BenefitType = ((Label)grdEntry.SelectedRow.FindControl("lblBenefitType")).Text;
            lblDuplicateItem.Text = grdEntry.SelectedRow.Cells[3].Text == "&nbsp;" ? "" : grdEntry.SelectedRow.Cells[3].Text;
            Active = grdEntry.SelectedRow.Cells[7].Text == "&nbsp;" ? "" : grdEntry.SelectedRow.Cells[7].Text;
            PrjClass.SetItemInDropDown(drpServiceTaxType, ((Label)grdEntry.SelectedRow.FindControl("lblTaxType")).Text, true, false);
            if (BenefitType == "Amount")
            {
               // rdbAmount.Checked = true;
              //  rdbDiscount.Checked = false;
                txtAmount.Text = ((Label)grdEntry.SelectedRow.FindControl("lblBenefitValue")).Text;
                txtAmount.Enabled = true;
                txtDiscount.Enabled = false;
                txtDiscount.Text = "0";
            }
            else
            {
              //  rdbDiscount.Checked = true;
               // rdbAmount.Checked = false;
                txtDiscount.Text = ((Label)grdEntry.SelectedRow.FindControl("lblBenefitValue")).Text;
                txtDiscount.Enabled = true;
                txtAmount.Enabled = false;
                txtAmount.Text = "0";
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "testCustom(" + drpPackageType.ClientID + ");", true);
            if (grdEntry.SelectedRow.Cells[2].Text == "Qty / Item" || grdEntry.SelectedRow.Cells[2].Text == "Flat Qty")
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = BAL.BALFactory.Instance.BL_PackageMaster.GetPackageQtyDetail(lblId.Text, Globals.BranchID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    grdBind.DataSource = dt;
                    grdBind.DataBind();
                    ((Label)grdBind.FooterRow.Cells[0].FindControl("lblNewSno")).Text = GetNewSNo().ToString();
                    ViewState["VSEntryTable"] = dt;
                    txtFlatQty.Text = ((Label)grdEntry.SelectedRow.FindControl("lblTotalQty")).Text;
                }
                else
                {
                    SetGridForEntryWhenNoDataIsPresent();
                    txtFlatQty.Text = ((Label)grdEntry.SelectedRow.FindControl("lblTotalQty")).Text;
                }
                txtRecurrence.Text = ((Label)grdEntry.SelectedRow.FindControl("lblRecurrence")).Text;
                txtStartDate.Text = ((Label)grdEntry.SelectedRow.FindControl("lblStartDate")).Text;
                txtEndDate.Text = ((Label)grdEntry.SelectedRow.FindControl("lblEndDate")).Text;
                hdnIsRecurrenceValid.Value = "true";
            }
            else
            {
                SetGridForEntryWhenNoDataIsPresent();
                txtStartDate.Text = ((Label)grdEntry.SelectedRow.FindControl("lblStartDate")).Text;
                txtEndDate.Text = ((Label)grdEntry.SelectedRow.FindControl("lblEndDate")).Text;
            }            
            txtTitle.Focus();
            txtTitle.Attributes.Add("onfocus", "javascript:select();");
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;          
            Ob.PackageId = lblId.Text;
            Ob.BranchId = Globals.BranchID;
            if (BenefitType == "Amount")
            {
                txtAmount.Enabled = true;
                txtDiscount.Enabled = false;
            }
            else
            {
                txtDiscount.Enabled = true;
                txtAmount.Enabled = false;
            }
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageInAssignTable(Ob) == true)
            {
                btnSave.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;            
                txtTitle.Enabled = false;
                txtCost.Enabled = false;
                txtAmount.Enabled = false;
                txtDiscount.Enabled = false;
                SetTextBox(false);
                grdBind.FooterRow.Visible = false;
                txtFlatQty.Enabled = false;
            }
            else
            {
                txtTitle.Enabled = true;
                txtCost.Enabled = true;
                txtAmount.Enabled = true;
                txtDiscount.Enabled = true;
                SetTextBox(true);
                grdBind.FooterRow.Visible = true;
                txtFlatQty.Enabled = true;
            }
        }

        protected void grdBind_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
        }

        protected void grdBind_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddNewRecord")
                {
                    string strSno = ((Label)grdBind.FooterRow.FindControl("lblNewSno")).Text.Trim();
                    string strDescription = ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Text.Trim();
                    string qty = ((TextBox)grdBind.FooterRow.FindControl("txtFQty")).Text.Trim();
                    if (((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                        lblErr.Text = "Kindly enter garment name.";                       
                        ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
                        return;
                    }
                    if (drpPackageType.SelectedItem.Value != "4")
                    {
                        if (((TextBox)grdBind.FooterRow.FindControl("txtFQty")).Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                            lblErr.Text = "Kindly enter qty.";                           
                            ((TextBox)grdBind.FooterRow.FindControl("txtFQty")).Focus();
                            return;
                        }
                    }
                    DataTable dtEntry = new DataTable();
                    dtEntry = (DataTable)ViewState["VSEntryTable"];
                    if (dtEntry.Rows[0][0].ToString() == "0")
                    {
                        dtEntry.Rows[0].Delete();
                        dtEntry.AcceptChanges();
                    }
                    DataRow dr = dtEntry.NewRow();
                    dr[0] = strSno;
                    dr[1] = strDescription;
                    dr[2] = qty;
                    dtEntry.Rows.Add(dr);
                    dtEntry.AcceptChanges();
                    dr = null;
                    ViewState["VSEntryTable"] = dtEntry;
                    grdBind.DataSource = dtEntry;
                    grdBind.DataBind();
                    ((Label)grdBind.FooterRow.Cells[0].FindControl("lblNewSno")).Text = GetNewSNo().ToString();
                    ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
                }
                else if (e.CommandName == "DeleteRecord")
                {
                    int RowIndex = int.Parse(e.CommandArgument.ToString());
                    DataTable dtEntry = new DataTable();
                    dtEntry = (DataTable)ViewState["VSEntryTable"];
                    dtEntry.Rows[RowIndex].Delete();
                    dtEntry.AcceptChanges();
                    ResetSno(dtEntry);
                    ViewState["VSEntryTable"] = dtEntry;
                    grdBind.DataSource = dtEntry;
                    grdBind.DataBind();
                    ((Label)grdBind.FooterRow.Cells[0].FindControl("lblNewSno")).Text = GetNewSNo().ToString();
                    ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
                    if (grdBind.Rows.Count == 0)
                    {
                        SetGridForEntryWhenNoDataIsPresent();
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void grdBind_RowEditing(object sender, GridViewEditEventArgs e)
        {
            hdnRowInsex.Value = e.NewEditIndex.ToString();
            grdBind.EditIndex = e.NewEditIndex;
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataSet ds = new DataSet();
            dt = GetNewTableForEntry();
            for (int iRow = 0; iRow < grdBind.Rows.Count; iRow++)
            {
                dr = dt.NewRow();
                try
                {
                    dr["S_No"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblSrNo")).Text;
                }
                catch (Exception ex)
                {
                    dr["S_No"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblESrNo")).Text;
                }
                try
                {
                    dr["ItemName"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblDescription")).Text;
                }
                catch (Exception ex)
                {
                    dr["ItemName"] = ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Text;
                }
                try
                {
                    dr["Qty"] = ((Label)grdBind.Rows[iRow].Cells[1].FindControl("lblQty")).Text;
                }
                catch (Exception ex)
                {
                    dr["Qty"] = ((TextBox)grdBind.Rows[iRow].Cells[1].FindControl("txtEQty")).Text;
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            grdBind.DataSource = dt;
            grdBind.DataBind();
            ((TextBox)grdBind.Rows[Convert.ToInt32(hdnRowInsex.Value)].Cells[0].FindControl("txtEDescription")).Focus();
            ((TextBox)grdBind.Rows[Convert.ToInt32(hdnRowInsex.Value)].Cells[0].FindControl("txtEDescription")).Attributes.Add("onfocus", "javascript:select();");
        }

        protected void grdBind_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataSet ds = new DataSet();
            dt = GetNewTableForEntry();
            for (int iRow = 0; iRow < grdBind.Rows.Count; iRow++)
            {
                dr = dt.NewRow();
                try
                {
                    dr["S_No"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblSrNo")).Text;
                }
                catch (Exception ex)
                {
                    dr["S_No"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblESrNo")).Text;
                }
                try
                {
                    dr["ItemName"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblDescription")).Text;
                }
                catch (Exception ex)
                {
                    dr["ItemName"] = ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Text;
                }
                try
                {
                    dr["Qty"] = ((Label)grdBind.Rows[iRow].Cells[1].FindControl("lblQty")).Text;
                }
                catch (Exception ex)
                {
                    dr["Qty"] = ((TextBox)grdBind.Rows[iRow].Cells[1].FindControl("txtEQty")).Text;
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            grdBind.DataSource = dt;
            grdBind.EditIndex = -1;
            grdBind.DataBind();
            ViewState["VSEntryTable"] = dt;
            ((Label)grdBind.FooterRow.Cells[0].FindControl("lblNewSno")).Text = GetNewSNo().ToString();
            ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
        }

        private DataTable GetNewTableForEntry()
        {
            DataTable dtEntry = new DataTable();
            dtEntry.Columns.Add("S_No");
            dtEntry.Columns.Add("ItemName");
            dtEntry.Columns.Add("Qty");
            return dtEntry;
        }

        private void SetGridForEntryWhenNoDataIsPresent()
        {
            DataTable dtEntry = GetNewTableForEntry();
            ViewState["VSEntryTable"] = dtEntry;
            DataRow dr = null;
            dr = dtEntry.NewRow();
            dr[0] = "0";
            dr[1] = "";
            dr[2] = "";
            dtEntry.Rows.Add(dr);
            grdBind.DataSource = dtEntry;
            grdBind.DataBind();
            ((Label)grdBind.FooterRow.Cells[0].FindControl("lblNewSno")).Text = GetNewSNo().ToString();
            grdBind.Rows[0].Visible = false;
        }

        private int GetNewSNo()
        {
            int NewSNo = 0;
            for (int r = 0; r < grdBind.Rows.Count; r++)
            {
                NewSNo = int.Parse("0" + ((Label)grdBind.Rows[r].Cells[0].FindControl("lblSrNo")).Text);
            }
            NewSNo = NewSNo + 1;
            return NewSNo;
        }

        private void ResetSno(DataTable dtTmp)
        {
            for (int r = 0; r < dtTmp.Rows.Count; r++)
            {
                dtTmp.Rows[r][0] = (r + 1).ToString();
            }
        }

        private string[] itemname;

        protected void txtFDescription_TextChanged(object sender, EventArgs e)
        {
            try
            {
                itemname = ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Text.Split('-');
                ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Text = itemname[1].Trim();
                ((TextBox)grdBind.FooterRow.FindControl("txtFQty")).Focus();
                hdnFireRowCommand.Value = "FireAdd";
                if (BAL.BALFactory.Instance.Bal_Processmaster.CheckCorrectItem(Globals.BranchID, itemname[1].Trim()) == false)
                {                   
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Kindly enter garment name.";  
                    ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
                    ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Attributes.Add("onfocus", "javascript:select();");
                    hdnFireRowCommand.Value = "";
                }

                if (hdnFireRowCommand.Value == "FireAdd")
                {
                    var cmdArgs = new CommandEventArgs("AddNewRecord", null);
                    grdBind_RowCommand(null, new GridViewCommandEventArgs(null, cmdArgs));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Kindly enter valid garment name.";  
                ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
                ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Attributes.Add("onfocus", "javascript:select();");
            }
        }

        protected void txtEDescription_TextChanged(object sender, EventArgs e)
        {
            int iRow = Convert.ToInt32(hdnRowInsex.Value);
            try
            {
                itemname = ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Text.Split('-');
                ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Text = itemname[1].Trim();
                ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEQty")).Focus();
                if (BAL.BALFactory.Instance.Bal_Processmaster.CheckCorrectItem(Globals.BranchID, itemname[1].Trim()) == false)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Kindly enter valid garment name.";  
                    ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Focus();
                    ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Attributes.Add("onfocus", "javascript:select();");
                }
                if (hdnFireRowCommand.Value == "FireAdd")
                {
                    //var cmdArgs = new CommandEventArgs("Update", null);
                    grdBind_RowUpdating(null, null);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Kindly enter valid garment name.";  
                ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Focus();
                ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Attributes.Add("onfocus", "javascript:select();");
            }
        }

        private void SetTextBox(bool Status)
        {
            drpPackageType.Enabled = Status;
            foreach (var ctrl in Form.FindControl("ContentPlaceHolder1").Controls)
            {
                switch (ctrl.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.TextBox": ((TextBox)ctrl).Enabled = Status;
                        break;

                    default:
                        break;
                }
            }
            drpServiceTaxType.Enabled = Status;
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataSet ds = new DataSet();
            dt = GetNewTableForEntry();
            for (int iRow = 0; iRow < grdBind.Rows.Count; iRow++)
            {
                dr = dt.NewRow();
                try
                {
                    dr["S_No"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblSrNo")).Text;
                }
                catch (Exception ex)
                {
                    dr["S_No"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblESrNo")).Text;
                }
                try
                {
                    dr["ItemName"] = ((Label)grdBind.Rows[iRow].Cells[0].FindControl("lblDescription")).Text;
                }
                catch (Exception ex)
                {
                    dr["ItemName"] = ((TextBox)grdBind.Rows[iRow].Cells[0].FindControl("txtEDescription")).Text;
                }
                try
                {
                    dr["Qty"] = ((Label)grdBind.Rows[iRow].Cells[1].FindControl("lblQty")).Text;
                }
                catch (Exception ex)
                {
                    dr["Qty"] = ((TextBox)grdBind.Rows[iRow].Cells[1].FindControl("txtEQty")).Text;
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            grdBind.DataSource = dt;
            grdBind.EditIndex = -1;
            grdBind.DataBind();
            ViewState["VSEntryTable"] = dt;
            ((Label)grdBind.FooterRow.Cells[0].FindControl("lblNewSno")).Text = GetNewSNo().ToString();
            ((TextBox)grdBind.FooterRow.FindControl("txtFDescription")).Focus();
        }
    }
}