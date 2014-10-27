using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class ItemProcessPriceMaster : System.Web.UI.Page
{
    private DTO.Common Ob = new DTO.Common();
    public string stylebutton = "<span class='TDCaption'>Item Name :</span>";
    private string status = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserBranch"] == null || Session["UserType"] == null || Session["UserName"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.PrintRateList, Session["UserType"].ToString());
            if (status == "True")
            {
                btnRateList.Visible = true;
                btnPrint.Visible = true;
            }
            else
            {
                btnRateList.Visible = false;
                btnPrint.Visible = false;
            }
            
            BindGrid();
            if (drpItemNames.Items.Count == 0)
                lblErr.Text = "No Item has been set yet.";
            else
                drpItemNames_SelectedIndexChanged(null, null);

            txtItemNameSearch.Focus();
            BindDropDown();
        }
        
    }

    protected void BindDropDown()
    {
        Ob.BID = Globals.BranchID;
        drpItemNames.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindItemDropDown(Ob);
        drpItemNames.DataTextField = "ItemName";
        drpItemNames.DataValueField = "ItemID";
        drpItemNames.DataBind();

        ddlRateList.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindAllListMasters(Globals.BranchID);
        ddlRateList.DataTextField = "name";
        ddlRateList.DataValueField = "rateListId";
        ddlRateList.DataBind();
    }

    protected void BindGrid()
    {
        Ob.BID = Globals.BranchID;
        grdTable.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindGrid(Ob);
        grdTable.DataBind();
    }

    protected void drpItemNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearRates();
        SqlDataReader sdr = null;
        string strColProcess = "", strGrdProcess = "";
        try
        {
            Ob.BID = Globals.BranchID;
            Ob.Input = drpItemNames.SelectedItem.Text;
            sdr = BAL.BALFactory.Instance.BL_PriceList.ReadProcessRate(Ob);
            while (sdr.Read())
            {
                strColProcess = "" + sdr.GetValue(1);
                for (int r = 0; r < grdTable.Rows.Count; r++)
                {
                    strGrdProcess = ((HiddenField)grdTable.Rows[r].FindControl("hdnProcessCode")).Value;
                    if (strColProcess == strGrdProcess)
                    {
                        ((TextBox)grdTable.Rows[r].Cells[1].FindControl("txtProcessRate")).Text = "" + sdr.GetValue(2);
                        break;
                    }
                }
            }
            grdTable.Caption = "Rates for " + drpItemNames.SelectedItem.Text;
            btnSave.Visible = true;
        }
        catch (Exception excp)
        {
            lblErr.Text = "Could not get rates for selected item." + excp.Message; ;
        }
        finally
        {
            sdr.Close();
            sdr.Dispose();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string res = string.Empty;
        Ob.BID = Globals.BranchID;
        Ob.Input = drpItemNames.SelectedItem.Text;
        res = BAL.BALFactory.Instance.BL_PriceList.SaveDataInDataBase(grdTable, Ob);
        if (res == "Record Saved")
        {
            lblMsg.Text = "Process rates saved";
        }
        else
        {
            lblErr.Text = res;
        }
    }

    private void ClearRates()
    {
        for (int r = 0; r < grdTable.Rows.Count; r++)
        {
            ((TextBox)grdTable.Rows[r].Cells[1].FindControl("txtProcessRate")).Text = "0";
        }
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        if (drpItemNames.Items.Count > 0)
        {
            drpItemNames_SelectedIndexChanged(null, null);
        }
        else
        {
            lblErr.Text = "There is no item in the list. Fill Items in the item master first.";
        }
    }

    protected void btnRateList_Click(object sender, EventArgs e)
    {
        OpenNewWindow("../Bookings_New/frmItemWiseRateList.aspx?RateListId=" + ddlRateList.SelectedValue + "&RateListName=" + ddlRateList.SelectedItem.Text + "ForInput=false");
    }

    public void OpenNewWindow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }

    protected void btnImportRateList_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string filename = flpUpload.PostedFile.FileName;
        string[] fileExtension = filename.Split('.');
        if (fileExtension[1] == "xls")
        {
            filename = "Rate.xls";
            System.IO.File.Delete(Server.MapPath("~/SQL/" + filename));
            flpUpload.SaveAs(Server.MapPath("~/SQL/" + filename));
            ds = BAL.BALFactory.Instance.Bal_Report.ImportRateList(Server.MapPath("~/SQL/Rate.xls"));
            if (ds.Tables[2].Rows.Count == 0)
            {
                if (ds.Tables[3].Rows.Count == 0)
                {
                    ArrayList rows = new ArrayList();
                    for (int row = 1; row < ds.Tables[1].Rows.Count; row++)
                        rows.Add(ds.Tables[1].Rows[row]["COLUMN_NAME"].ToString().Replace('_', ' '));
                    string res = BAL.BALFactory.Instance.Bal_Report.SaveRateList(ds, rows, Globals.BranchID);
                    if (res == "Record Saved")
                    {
                        lblMsg.Text = PrjClass.SaveSuccess;
                        drpItemNames_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    lblErr.Text = "You are importing invalid process :";
                    lblErr.Text += "," + ds.Tables[3].Rows[0]["ProcessName"].ToString();
                    for (int i = 1; i < ds.Tables[3].Rows.Count; i++)
                        lblErr.Text += "," + ds.Tables[3].Rows[i]["ProcessName"].ToString();
                }
            }
            else
            {
                lblErr.Text = "You are importing invalid items :";
                lblErr.Text += " " + ds.Tables[2].Rows[0]["ItemName"].ToString();
                for (int i = 1; i < ds.Tables[2].Rows.Count; i++)
                    lblErr.Text += "," + ds.Tables[2].Rows[i]["ItemName"].ToString();
            }
        }
        else
            lblErr.Text = "Invalid file to import.Please select .xls file.";
    }

    private string[] itemname;

    protected void btnTemp_Click(object sender, EventArgs e)
    {
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
        }
        catch (Exception ex)
        {
            Session["ReturnMsg"] = "Please enter valid item name.";
            txtItemNameSearch.Focus();
            txtItemNameSearch.Attributes.Add("onfocus", "javascript:select();");
        }
        ClearRates();
        SqlDataReader sdr = null;
        string strColProcess = "", strGrdProcess = "";
        try
        {
            Ob.BID = Globals.BranchID;
            Ob.Input = itemname[1].Trim();
            sdr = BAL.BALFactory.Instance.BL_PriceList.ReadProcessRate(Ob);
            while (sdr.Read())
            {
                strColProcess = "" + sdr.GetValue(1);
                for (int r = 0; r < grdTable.Rows.Count; r++)
                {
                    strGrdProcess = ((HiddenField)grdTable.Rows[r].FindControl("hdnProcessCode")).Value;
                    if (strColProcess == strGrdProcess)
                    {
                        ((TextBox)grdTable.Rows[r].Cells[1].FindControl("txtProcessRate")).Text = "" + sdr.GetValue(2);
                        break;
                    }
                }
            }
            grdTable.Caption = "Rates for " + txtItemNameSearch.Text;
            ((TextBox)grdTable.Rows[0].Cells[1].FindControl("txtProcessRate")).Focus();
            ((TextBox)grdTable.Rows[0].Cells[1].FindControl("txtProcessRate")).Attributes.Add("onfocus", "javascript:select();");
            PrjClass.SetItemInDropDown(drpItemNames, itemname[1].Trim(), true, false);
            stylebutton = "<span class='Legend'>Item Name :</span>";
            btnSave.Visible = true;
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
        }
    }

    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        string res = string.Empty;
        Ob.BID = Globals.BranchID;
        Ob.Input = drpItemNames.SelectedItem.Text;
        res = BAL.BALFactory.Instance.BL_PriceList.SaveDataInDataBase(grdTable, Ob);
        if (res == "Record Saved")
        {
            lblMsg.Text = "Process rates saved";
            txtItemNameSearch.Text = "";
            txtItemNameSearch.Focus();
        }
        else
        {
            lblErr.Text = res;
        }
    }

    protected void BtnEditClick(object sender, EventArgs e)
    {
        OpenNewWindow("../Bookings_New/frmItemWiseRateList.aspx?RateListId=" + ddlRateList.SelectedValue + "&RateListName=" + ddlRateList.SelectedItem.Text + "&ForInput=true");
    }

    protected void BtnPrintClick(object sender, EventArgs e)
    {
        OpenNewWindow("../Bookings_New/frmItemWiseRateList.aspx?RateListId=" + ddlRateList.SelectedValue + "&RateListName=" + ddlRateList.SelectedItem.Text + "&ForInput=false");
    }

    protected void BtnCopyClick(object sender, EventArgs e)
    {
    }

    protected void BtnCreateNewListClick(object sender, EventArgs e)
    {
        CreateNewList();
    }

    protected void CreateNewList()
    {
        var listName = txtNewListName.Text;
        var rateListIdToCopyFrom = hdnIsBlankList.Value == "true" ? -1 : int.Parse(ddlRateList.SelectedValue);
        var result = BAL.BALFactory.Instance.BL_PriceList.SaveNewList(rateListIdToCopyFrom, listName, Globals.BranchID);
        //  BindDropDown();
        if (result == "Record Saved")
        {
            OpenNewWindow("../Bookings_New/frmItemWiseRateList.aspx?RateListId=" + (int.Parse(ddlRateList.Items[ddlRateList.Items.Count - 1].Value) + 1) + "&RateListName=" + listName + "&ForInput=true");
            txtNewListName.Text = string.Empty;
            BindDropDown();
        }
    }
}