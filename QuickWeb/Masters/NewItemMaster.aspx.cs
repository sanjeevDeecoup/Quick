using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Masters_NewItemMaster : System.Web.UI.Page
{
    private DTO.Common Common = new DTO.Common();
    private DTO.Item Item = new DTO.Item();
    private DTO.Common Ob = new DTO.Common();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtItemSubQty_TextChanged(null, null);
            hdntemp.Value = "0";
            RefreshPage();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int x1 = 1, TotalSubQty = 0;

        for (int x = 0; x < lstSubItem.Items.Count; x++)
        {
            x1 = x + 1;
        }
        try
        {
            if (rdbinch.Checked)
            {
                TotalSubQty = 1;
                x1 = 1;
            }
            if (rdbpanel.Checked)
            {
                TotalSubQty = 1;
                x1 = 1;
            }
            if (rdbPcs.Checked)
            {
                TotalSubQty = int.Parse(txtItemSubQty.Text.Trim());
            }
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error: Invalid no. of sub items.";
            return;
        }
        if (txtProcess.Text != "")
        {
            if (BAL.BALFactory.Instance.BAL_Item.CheckIfProcessCodeExits(txtProcess.Text, Globals.BranchID) != true)
            {
                lblErr.Text = "Please select valid process.";
                txtProcess.Focus();
                txtProcess.Attributes.Add("onfocus", "javascript:select();");
                return;
            }
        }
        if (TotalSubQty == x1)
        {
            Item = SetValue();
            Common.Result = BAL.BALFactory.Instance.BAL_Item.SaveItem(Item);
            if (Common.Result == "Record Saved")
            {
                if (Item.NoOfItem == "1")
                {
                    Item.SubItemName = txtItemName.Text;
                    Item.SubItemOrder = "1";
                    Common.Result = DAL.DALFactory.Instance.DAL_Item.SaveSubItem(Item);
                    if (Common.Result == "Record Saved")
                    {
                        lblMsg.Text = "Item Saved Successfully";
                        SaveDefPrcAndRate(ref Item, Globals.BranchID);
                        RefreshPage();
                    }
                }
                else
                {
                    int inc = 0;
                    for (int i = 1; i <= lstSubItem.Items.Count; i++)
                    {
                        Item.SubItemName = lstSubItem.Items[inc].Text;
                        Item.SubItemOrder = i.ToString();
                        Common.Result = DAL.DALFactory.Instance.DAL_Item.SaveSubItem(Item);
                        if (Common.Result != "Record Saved")
                            break;
                        inc++;
                    }
                    if (Common.Result == "Record Saved")
                    {
                        lblMsg.Text = "Item Saved Successfully";
                        SaveDefPrcAndRate(ref Item, Globals.BranchID);
                        RefreshPage();
                    }
                    else
                    {
                        lblErr.Text = Common.Result;
                    }
                }
            }
            else
            {
                lblErr.Text = Common.Result;
                txtItemCode.Focus();
                txtItemCode.Attributes.Add("onfocus", "javascript:select();");
            }
        }
        else
        {
            lblErr.Text = "Item List Should be match with Sub Items Quantity";
            txtItemSubQty.Focus();
            return;
        }
    }

    public void BindGrid(string strItemName)
    {
        //Item = SetValue();
        //grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Item.ShowItem(Item);
        //grdSearchResult.DataBind();
        if (strItemName == "Pageing")
        {
            SqlGridSource.SelectCommand = "SELECT * FROM ItemMaster WHERE ItemName<>'' AND BranchId='" + Globals.BranchID + "' ORDER BY ItemName";
            SqlGridSource.DataBind();
        }
        else
        {
            SqlGridSource.SelectCommand = "SELECT * FROM ItemMaster WHERE ItemName<>'' AND BranchId='" + Globals.BranchID + "' AND ItemName LIKE '%'+'" + strItemName + "'+'%' OR ItemCode Like '%'+'" + strItemName + "'+'%' ORDER BY ItemName";
            SqlGridSource.DataBind();
        }
    }

    public void RefreshPage()
    {
        txtItemName.Text = "";
        txtItemCode.Text = "";
        txtItemSubQty.Text = "1";
        txtSubItem1.Text = "";
        lblSaveOption.Text = "";
        txtSubItem1.Visible = false;
        lstSubItem.Items.Clear();
        lstSubItem.Visible = false;
        txtItemNameSearch.Focus();
        SqlGridSource.SelectCommand = "SELECT * FROM ItemMaster WHERE ItemName<>'' AND BranchId='" + Globals.BranchID + "'  ORDER BY ItemName";
        SqlGridSource.DataBind();
        btnSave.Visible = true;
        btnCopy.Visible = false;
        btnUpdate.Visible = false;
        lblItem1.Visible = false;
        txtItemNameSearch.Text = "";
        rdbPcs.Checked = true;
        rdbinch.Checked = false;
        rdbpanel.Checked = false;
        txtItemSubQty.Enabled = true;
        txtItemName.Enabled = true;
        rdbinch.Enabled = true;
        rdbpanel.Enabled = true;
        rdbPcs.Enabled = true;
        txtDefaultRate.Text = "";
        txtProcess.Text = "";
        txtNewListName.Text = "";
        BindDropDown();
    }

    protected void BindDropDown()
    {
        ddlRateList.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindAllListMasters(Globals.BranchID);
        ddlRateList.DataTextField = "name";
        ddlRateList.DataValueField = "rateListId";
        ddlRateList.DataBind();
        Ob.BID = Globals.BranchID;
        ddlRateList1.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindItemDropDown(Ob);
        ddlRateList1.DataTextField = "ItemName";
        ddlRateList1.DataValueField = "ItemName";
        ddlRateList1.DataBind();
    }

    public DTO.Item SetValue()
    {
        Item.BranchId = Globals.BranchID;
        Item.ItemCode = txtItemCode.Text;
        Item.ItemName = txtItemName.Text;
        Item.ID = lblUpdateId.Text;
        Item.ItemImage = hdnItemCode.Value;
        Item.DefaultPrc = txtProcess.Text.ToUpperInvariant();
        Item.DefaultRate = txtDefaultRate.Text == "&nbsp;" ? "" : txtDefaultRate.Text;
        if (rdbinch.Checked)
        {
            Item.NoOfItem = "1";
            Item.VariationId = "LB";
        }
        if (rdbpanel.Checked)
        {
            Item.NoOfItem = "1";
            Item.VariationId = "P";
        }
        if (rdbPcs.Checked)
        {
            Item.NoOfItem = txtItemSubQty.Text;
            Item.VariationId = "Pcs";
        }
        return Item;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtItemName.Text == "")
        {
            lblErr.Text = "Please Enter some text in item name for searching.";
            return;
        }
        //Item = SetValue();
        //grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Item.SearchItem(Item);
        //grdSearchResult.DataBind();
        BindGrid(txtItemName.Text.Trim());
    }

    
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        //Item = SetValue();
        //grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Item.ShowAllItem(Item);
        //grdSearchResult.DataBind();
        SqlGridSource.SelectCommand = "SELECT * FROM ItemMaster WHERE BranchId='" + Globals.BranchID + "'  ORDER BY ItemName";
        SqlGridSource.DataBind();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        SqlTransaction stx = null;
        string strItemName = "";
        int TotalItems = 0;
        try
        {
            sqlcon.Open();
            stx = sqlcon.BeginTransaction();
            cmd.Connection = sqlcon;
            cmd.Transaction = stx;
            cmd.CommandText = "SELECT ItemName From ItemMaster WHERE ItemId='" + lblUpdateId.Text + "' AND BranchId='" + Globals.BranchID + "'";
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                strItemName = "" + sdr.GetValue(0);
            }
            sdr.Close();
            if (strItemName == "")
            {
                throw new Exception("Could not get selected item's name.");
            }
            cmd.CommandText = "SELECT COUNT(ItemName) FROM EntBookingDetails WHERE ItemName='" + strItemName + "' AND BranchId='" + Globals.BranchID + "'";
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                TotalItems = int.Parse("0" + sdr.GetValue(0));
            }
            sdr.Close();
            if (TotalItems > 0)
            {
                throw new Exception("Selected item has been received for booking. Item can not be removed.");
            }
            cmd.CommandText = "DELETE FROM ItemMaster WHERE ItemId='" + lblUpdateId.Text + "' AND BranchId='" + Globals.BranchID + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM ItemWiseProcessRate WHERE ItemName='" + strItemName + "' AND BranchId='" + Globals.BranchID + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "Delete from EntSubItemDetails where ItemName='" + strItemName + "' AND BranchId='" + Globals.BranchID + "'";
            cmd.ExecuteNonQuery();
            stx.Commit();
            btnAddNew_Click(null, null);
            lblMsg.Text = "Item Deleted Successfully";
        }
        catch (Exception excp)
        {
            if (sdr != null)
            {
                sdr.Close();
               
            }
            if (stx != null)
            {
                stx.Rollback();
            }
            lblErr.Text = "Error : " + excp.Message;
        }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            
            stx = null;
            sqlcon.Close();
            sqlcon.Dispose();
            
        }
        // BindGrid("");
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        DataSet dsMain = new DataSet();
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "SP_Sel_RecForItemIdUpdate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ItemId", lblUpdateId.Text));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
            SqlDataAdapter sadp = new SqlDataAdapter();
            sadp.SelectCommand = cmd;
            sadp.Fill(dsMain);
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sqlcon = null;
        }
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            DTO.Item Ob = new DTO.Item();
            Ob.ItemName = "" + dsMain.Tables[0].Rows[0]["ItemName"].ToString();
            Ob.BranchId = Globals.BranchID;
            txtItemName.Text = "" + dsMain.Tables[0].Rows[0]["ItemName"].ToString();
            txtItemSubQty.Text = "" + dsMain.Tables[0].Rows[0]["NumberOfSubItems"].ToString();
            hdntemp.Value = txtItemSubQty.Text;
            txtItemCode.Text = "" + dsMain.Tables[0].Rows[0]["ItemCode"].ToString();
            hdnItemCode.Value = txtItemCode.Text;
            string unit = "" + dsMain.Tables[0].Rows[0]["Unit"].ToString();
            SetTextbox();
            hdntemp.Value = "1";
            lstSubItem.Items.Clear();
            if (dsMain.Tables[1].Rows.Count > 1)
            {
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    lstSubItem.Items.Add(dsMain.Tables[1].Rows[i]["SubItemName"].ToString());
                }
            }
            if (dsMain.Tables[2].Rows.Count > 0)
            {
                txtDefaultRate.Text = "" + dsMain.Tables[2].Rows[0]["ProcessPrice"].ToString();
                txtProcess.Text = "" + dsMain.Tables[2].Rows[0]["ProcessCode"].ToString();
            }
            else
            {
                txtDefaultRate.Text = "";
                txtProcess.Text = "";
            }
            lblSaveOption.Text = "2";
            ViewState["ItemPrevName"] = txtItemName.Text;
            EnableModification(false);
            EnableModification(true);
            txtItemName.Focus();
            btnSave.Visible = false;
           // btnCopy.Visible = true;
            if (unit == "Pcs")
            {
                rdbPcs.Checked = true;
                rdbpanel.Checked = false;
                rdbinch.Checked = false;
            }
            else if (unit == "P")
            {
                rdbpanel.Checked = true;
                rdbinch.Checked = false;
                rdbPcs.Checked = false;
                txtItemSubQty.Enabled = false;
            }
            else if (unit == "LB")
            {
                rdbinch.Checked = true;
                rdbPcs.Checked = false;
                rdbpanel.Checked = false;
                txtItemSubQty.Enabled = false;
            }
            if (BAL.BALFactory.Instance.BAL_Item.CheckItemStatus(Ob) == true)
            {
                rdbinch.Enabled = false;
                rdbpanel.Enabled = false;
                rdbPcs.Enabled = false;
                btnUpdate.Visible = true;
                btnDelete.Visible = false;
                EnableModification(false);
                txtItemCode.Enabled = true;
                txtItemCode.Focus();
                txtItemCode.Attributes.Add("onfocus", "javascript:select();");
            }
            else
            {
                rdbinch.Enabled = true;
                rdbpanel.Enabled = true;
                rdbPcs.Enabled = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                txtItemName.Focus();
                txtItemName.Attributes.Add("onfocus", "javascript:select();");
            }
        }
        else
        {
            lblErr.Text += " Could not get details.";
            lstSubItem.Items.Clear();
        }
    }

    protected void EnableModification(bool NewSet)
    {
        txtItemName.Enabled = NewSet;
        txtItemName.Enabled = NewSet;
        txtItemSubQty.Enabled = NewSet;
        txtSubItem1.Enabled = NewSet;
        txtItemCode.Enabled = NewSet;
        lstSubItem.Enabled = NewSet;
    }

    protected void RefreshList()
    {
        if (lstSubItem.Items.Count > 0)
        {
            lstSubItem.SelectedIndex = 0;
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        //EnableModification(true);

        //btnSave.Text = "Save";
        //btnSave.Visible = true;
        //lblSaveOption.Text = "1";
        //txtItemName.Focus();
        //txtItemSubQty.Text = "1";
        //txtSubItem1.Text = "";
        //RefreshList();
        //txtItemCode.Text = "";
        //btnDelete.Visible = false;
        //SetTextbox();
        Response.Redirect("NewItemMaster.aspx", false);
    }
    protected void BtnCopyClick(object sender, EventArgs e)
    {
    }
    protected void btnCreateNewListClick(object sender, EventArgs e)
    {
        Ob.Input = txtItemName.Text;
        Ob.ChangeName = txtNewListName.Text.Trim();
        Ob.BID = Globals.BranchID;
        string res = string.Empty;
        res = BAL.BALFactory.Instance.BAL_Item.ItemRename(Ob);
        if (res == "Record Saved")
        {
            lblMsg.Text = "Item rename sucessfully.";
            RefreshPage();
        }
        else
        {
            lblErr.Text = res.ToString();
        }
    }
    protected void SetTextbox()
    {
        try
        {
            int TotalSubQty = Convert.ToInt32(txtItemSubQty.Text);

            if (TotalSubQty < 1 || TotalSubQty > 100)
            {
                lblErrorMsg.Text = "Invalid Sub Items";
                txtSubItem1.Visible = false;
                lblItem1.Visible = false;
                lstSubItem.Visible = false;
                return;
            }
            if (TotalSubQty == 1)
            {
                txtSubItem1.Visible = false;
                lblItem1.Visible = false;
                lstSubItem.Visible = false;
                lstSubItem.Items.Clear();
                return;
            }
            if (TotalSubQty >= 2)
            {
                txtSubItem1.Visible = true;
                txtSubItem1.Focus();
                lblItem1.Visible = true;
                if (lblSaveOption.Text != "2")
                {
                    // lstSubItem.Items.Clear();
                }
                lstSubItem.Visible = true;
                return;
            }
        }
        catch (Exception)
        {
            lblErr.Text = "Invalid sub items";
        }
    }

    protected void txtItemSubQty_TextChanged(object sender, EventArgs e)
    {
        SetTextbox();
    }

    protected void txtSubItem1_TextChanged(object sender, EventArgs e)
    {
        int tq = 0;
        lstSubItem.Items.Add(txtSubItem1.Text);
        txtSubItem1.Text = "";
        txtSubItem1.Focus();
        int TSQ = Convert.ToInt32(txtItemSubQty.Text);
        for (int i = 0; i < lstSubItem.Items.Count; i++)
        {
            tq += 1;
        }
        if (TSQ == tq)
            txtSubItem1.Visible = false;
    }

    protected void lstSubItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblSaveOption.Text == "2")
        {
            txtSubItem1.Text = lstSubItem.SelectedValue;
            lstSubItem.Items.Remove(lstSubItem.SelectedValue.ToString());
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtItemName.Text == "")
        {
            lblErr.Text = "Please enter item name to save.";
            return;
        }
        int x1 = 1, TotalSubQty = 0;

        for (int x = 0; x < lstSubItem.Items.Count; x++)
        {
            x1 = x + 1;
        }
        try
        {
            if (rdbinch.Checked)
            {
                TotalSubQty = 1;
                x1 = 1;
            }
            if (rdbpanel.Checked)
            {
                TotalSubQty = 1;
                x1 = 1;
            }
            if (rdbPcs.Checked)
            {
                TotalSubQty = int.Parse(txtItemSubQty.Text.Trim());
            }
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error: Invalid no. of sub items.";
            return;
        }
        if (txtProcess.Text != "")
        {
            if (BAL.BALFactory.Instance.BAL_Item.CheckIfProcessCodeExits(txtProcess.Text, Globals.BranchID) != true)
            {
                lblErr.Text = "Please select valid process.";
                txtProcess.Focus();
                txtProcess.Attributes.Add("onfocus", "javascript:select();");
                return;
            }
        }
        if (TotalSubQty == x1)
        {
            Item = SetValue();
            Common.Result = BAL.BALFactory.Instance.BAL_Item.UpdateItem(Item);
            if (Common.Result == "Record Saved")
            {
                Item.OldItemName = (string)ViewState["ItemPrevName"];
                Common.Result = BAL.BALFactory.Instance.BAL_Item.DeleteSubItem(Item);
                if (Common.Result == "Record Saved")
                {
                    if (Item.NoOfItem == "1")
                    {
                        Item.SubItemName = txtItemName.Text;
                        Item.SubItemOrder = "1";
                        Common.Result = DAL.DALFactory.Instance.DAL_Item.SaveSubItem(Item);
                        if (Common.Result == "Record Saved")
                        {
                            lblMsg.Text = "Item Updated Successfully";
                            SaveDefPrcAndRate(ref Item, Globals.BranchID, true);
                            RefreshPage();
                        }
                    }
                    else
                    {
                        int inc = 0;
                        for (int i = 1; i <= lstSubItem.Items.Count; i++)
                        {
                            Item.SubItemName = lstSubItem.Items[inc].Text;
                            Item.SubItemOrder = i.ToString();
                            Common.Result = DAL.DALFactory.Instance.DAL_Item.SaveSubItem(Item);
                            if (Common.Result != "Record Saved")
                                break;
                            inc++;
                        }
                        if (Common.Result == "Record Saved")
                        {
                            lblMsg.Text = PrjClass.UpdateSuccess;
                            SaveDefPrcAndRate(ref Item, Globals.BranchID, true);
                            RefreshPage();
                        }
                        else
                        {
                            lblErr.Text = Common.Result;
                        }
                    }
                }
            }
            else
            {
                lblErr.Text = Common.Result;
                txtItemCode.Focus();
                txtItemCode.Attributes.Add("onfocus", "javascript:select();");
            }
            if (Common.Result == "Record Saved")
            {
                Item.OldItemName = (string)ViewState["ItemPrevName"];
                Common.Result = BAL.BALFactory.Instance.BAL_Item.UpdateExitingItem(Item);
                RefreshPage();
            }
        }
        else
        {
            lblErr.Text = "Item List Should be match with Sub Items Quantity";
            txtItemSubQty.Focus();
            return;
        }
    }

    protected void grdSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSearchResult.PageIndex = e.NewPageIndex;
        grdSearchResult.Visible = true;
        BindGrid("Pageing");
    }

    protected void grdSearchResult_OnSorted(object sender, EventArgs e)
    {
        grdSearchResult.Visible = true;
        BindGrid("Pageing");
        txtItemName.Attributes.Add("onFocus", "javascript:select();");
    }

    private string[] itemname;

    protected void btnTemp_Click(object sender, EventArgs e)
    {
        DataSet dsMain = new DataSet();
        try
        {
            itemname = txtItemNameSearch.Text.Split('-');
            txtItemNameSearch.Text = itemname[1].Trim();
            if (BAL.BALFactory.Instance.Bal_Processmaster.CheckCorrectItem(Globals.BranchID, itemname[1].Trim()) == false)
            {
                Session["ReturnMsg"] = "Please enter valid item name.";
                txtItemNameSearch.Focus();
                txtItemNameSearch.Attributes.Add("onfocus", "javascript:select();");
            }
            else
            {
                ddlRateList.SelectedIndexChanged -= ddlRateListChange;
                ddlRateList.SelectedIndex = 0;
                ddlRateList.SelectedIndexChanged += ddlRateListChange;
                lblUpdateId.Text = BAL.BALFactory.Instance.Bal_Processmaster.GetItemId(Globals.BranchID, itemname[1].Trim(), itemname[0].Trim());
                dsMain = BAL.BALFactory.Instance.Bal_Processmaster.getItem(lblUpdateId.Text, Globals.BranchID);
                DTO.Item Ob = new DTO.Item();

                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    Ob.ItemName = "" + dsMain.Tables[0].Rows[0]["ItemName"].ToString();
                    Ob.BranchId = Globals.BranchID;
                    txtItemName.Text = "" + dsMain.Tables[0].Rows[0]["ItemName"].ToString();
                    txtItemSubQty.Text = "" + dsMain.Tables[0].Rows[0]["NumberOfSubItems"].ToString();
                    hdntemp.Value = txtItemSubQty.Text;
                    txtItemCode.Text = "" + dsMain.Tables[0].Rows[0]["ItemCode"].ToString();
                    hdnItemCode.Value = txtItemCode.Text;
                    SetTextbox();
                    hdntemp.Value = "1";
                    string unit = dsMain.Tables[0].Rows[0]["Unit"].ToString();
                    lstSubItem.Items.Clear();
                    if (dsMain.Tables[1].Rows.Count > 1)
                    {
                        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                        {
                            lstSubItem.Items.Add(dsMain.Tables[1].Rows[i]["SubItemName"].ToString());
                        }
                    }
                    if (dsMain.Tables[2].Rows.Count > 0)
                    {
                        txtDefaultRate.Text = "" + dsMain.Tables[2].Rows[0]["ProcessPrice"].ToString();
                        txtProcess.Text = "" + dsMain.Tables[2].Rows[0]["ProcessCode"].ToString();
                    }
                    else
                    {
                        txtDefaultRate.Text = "";
                        txtProcess.Text = "";
                    }
                    lblSaveOption.Text = "2";
                    ViewState["ItemPrevName"] = txtItemName.Text;
                    EnableModification(false);
                    EnableModification(true);
                    txtItemName.Focus();
                    btnSave.Visible = false;
                    //btnCopy.Visible = true;
                    if (unit == "Pcs")
                    {
                        rdbPcs.Checked = true;
                        rdbpanel.Checked = false;
                        rdbinch.Checked = false;
                    }
                    else if (unit == "P")
                    {
                        rdbpanel.Checked = true;
                        rdbinch.Checked = false;
                        rdbPcs.Checked = false;
                        txtItemSubQty.Enabled = false;
                    }
                    else if (unit == "LB")
                    {
                        rdbinch.Checked = true;
                        rdbPcs.Checked = false;
                        rdbpanel.Checked = false;
                        txtItemSubQty.Enabled = false;
                    }
                    if (BAL.BALFactory.Instance.BAL_Item.CheckItemStatus(Ob) == true)
                    {
                        rdbinch.Enabled = false;
                        rdbpanel.Enabled = false;
                        rdbPcs.Enabled = false;
                        btnUpdate.Visible = true;
                        btnDelete.Visible = false;
                        EnableModification(false);
                        txtItemCode.Enabled = true;
                        txtItemCode.Focus();
                        txtItemCode.Attributes.Add("onfocus", "javascript:select();");
                    }
                    else
                    {
                        rdbinch.Enabled = true;
                        rdbpanel.Enabled = true;
                        rdbPcs.Enabled = true;
                        btnUpdate.Visible = true;
                        btnDelete.Visible = true;
                        txtItemName.Focus();
                        txtItemName.Attributes.Add("onfocus", "javascript:select();");
                    }
                }
                else
                {
                    lblErr.Text += " Could not get details.";
                    lstSubItem.Items.Clear();
                }
            }
        }
        catch (Exception ex)
        {
            Session["ReturnMsg"] = "Please enter valid item name.";
            txtItemNameSearch.Focus();
            txtItemNameSearch.Attributes.Add("onfocus", "javascript:select();");
        }
    }

    /// <summary>
    /// Add or updates the default process and rate for the item
    /// </summary>
    /// <param name="Item">The <c ref="DTO.Item">DTO</c> Item</param>
    /// <param name="branchId">The Branch Id</param>
    protected void SaveDefPrcAndRate(ref DTO.Item Item, string branchId)
    {
        // if there is no process defined, and no rate is specified
        // then do nothing!

        SaveDefPrcAndRate(ref Item, branchId, false);
        
    }

    /// <summary>
    /// Add or updates the default process and rate for the item
    /// </summary>
    /// <param name="Item">The <see cref="DTO.Item">DTO</see> Item</param>
    /// <param name="isUpdating">Is updating the record</param>
    /// <param name="branchId">The branchId</param>
    protected void SaveDefPrcAndRate(ref DTO.Item Item, string branchId, bool isUpdating)
    {

        // not using DAL, because no real work is being done there, might as well skip it!
        if (!string.IsNullOrEmpty(txtProcess.Text.Trim()) && !string.IsNullOrEmpty(txtDefaultRate.Text.Trim()))
        {
            DAL.DALFactory.Instance.DAL_Item.SaveDefPrcAndRate(ref Item, branchId, isUpdating, int.Parse(ddlRateList.SelectedValue));
        }
    }

    protected void txtProcess_TextChanged(object sender, EventArgs e)
    {

        var dropDownList = sender as DropDownList;
        if (dropDownList != null)
        {
            /*
            if (string.IsNullOrEmpty(txtProcess.Text.Trim()))
            {
                txtProcess.Focus();
                return;
            }
             */
            var processAndRate = BAL.BALFactory.Instance.BAL_City.GetDefaultProcessAndRateForRateList(txtItemName.Text, int.Parse(ddlRateList.SelectedValue), Globals.BranchID);
            if (!string.IsNullOrEmpty(processAndRate.Trim()))
            {
                txtProcess.Text = processAndRate.Split(':')[0];
                txtDefaultRate.Text = processAndRate.Split(':')[1];
            }
            else
            {
                txtProcess.Text = string.Empty;
                txtDefaultRate.Text = string.Empty;
            }
            if (txtProcess.Attributes["onfocus"] != null)
                txtProcess.Attributes["onfocus"] = "select();";
            else
                txtProcess.Attributes.Add("onfocus", "select();");
            txtProcess.Focus();
            return;
        }

        var process = txtProcess.Text;
        var caseSensitiveProcess = BAL.BALFactory.Instance.BAL_City.getCorrectProcess(Globals.BranchID, process);
        if (caseSensitiveProcess != "Invalid Process")
        {
            txtProcess.Text = caseSensitiveProcess;
            float rate = BAL.BALFactory.Instance.BAL_City.getPriceAccordingProcess(Globals.BranchID, process, txtItemName.Text, int.Parse(ddlRateList.SelectedValue));
            txtDefaultRate.Text = rate.ToString();
            if (txtDefaultRate.Attributes["onfocus"] != null)
                txtDefaultRate.Attributes["onfocus"] = "select();";
            else
                txtDefaultRate.Attributes.Add("onfocus", "select();");
            txtDefaultRate.Focus();
        }
        else
        {
            Session["ReturnMsg"] = "Please enter correct process name.";
            txtProcess.Focus();
        }
    }

    protected void ddlRateListChange(object sender, EventArgs e)
    {
        txtProcess_TextChanged(ddlRateList, EventArgs.Empty);
    }

}