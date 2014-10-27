using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Pendingstockreport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    DTO.Report Ob = new DTO.Report();
    static private string _allItems;
    DTO.PackageMaster Obp = new DTO.PackageMaster();
    string strFromDate = string.Empty, strToDate = string.Empty;
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {       
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        if (!IsPostBack)
        {
            _allItems = string.Empty;
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SELECT dbo.EntSubItemDetails.SubItemName FROM dbo.EntSubItemDetails WHERE BranchId='" + Globals.BranchID + "' ORDER By SubItemName";
            cmd.CommandType = CommandType.Text;
            ds = AppClass.GetData(cmd);
            drpItemNames.DataSource = ds.Tables[0];
            drpItemNames.DataTextField = "SubItemName";
            // drpItemNames.DataValueField = "ItemID";
            drpItemNames.DataBind();
            drpItemNames.Items.Insert(0, new ListItem("All", "0"));
            btnShowReport_Click(null, null);
            txtCName.Focus();
            Gridexcel.Visible = false;           
        }
        var btn = Request.Params["__EVENTTARGET"] as string;
        if (btn != null && btn == "ctl00$ContentPlaceHolder1$txtItemName")
        {
            txtItemName_TextChanged(null, EventArgs.Empty);
        } 
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();        
        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        ShowBookingDetails();
        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport();
            btnExport.Visible = true;          
            grdReport.Visible = true;           
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
        }
        else
        {
            btnExport.Visible = false;
        }
    }
    private void CalculateGridReport()
    {
        try
        {
            int rc = grdReport.Rows.Count;
            int cc = grdReport.Columns.Count;
            float OrderCount = 0;
            for (int r = 0; r < rc; r++)
            {
                OrderCount++;
            }
            grdReport.FooterRow.Cells[4].Text = "Total";
            grdReport.FooterRow.Cells[5].Text = OrderCount.ToString();
            Gridexcel.FooterRow.Cells[4].Text = "Total";
            Gridexcel.FooterRow.Cells[5].Text = OrderCount.ToString();
        }
        catch (Exception ex) { }
    }
    public DTO.Report IntialiazeValueInGlobal()
    {
        Ob.BranchId = Globals.BranchID;
        if (txtCName.Text != "")
            Ob.CustId = hdnCustId.Value;
        else
            Ob.CustId = "";
        Ob.Date = date[0].ToString();
        Ob.Day = drpdats.SelectedItem.Value;
        Ob.Description = drpItemNames.SelectedItem.Text;
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        Ob.CustCodeStr = _allItems;
        Ob.StartBkNum = strFromDate;
        Ob.EndBkNum = strToDate;
        return Ob;
    }
    private void ShowpendingDetails()
    {
        grdReport.DataSource = null;
        grdReport.DataBind();        
        DataSet dsMain = new DataSet();
        try
        {
            var bIsAll = true;
            if (!string.IsNullOrEmpty(_allItems))
                bIsAll = false;
            Ob = IntialiazeValueInGlobal();
            dsMain = BAL.BALFactory.Instance.Bal_Report.StockPendingInvoice(Ob, bIsAll);
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
                    Gridexcel.DataSource = dsMain.Tables[0];
                    Gridexcel.DataBind();
                    CalculateGridReport();
                }
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
           
        }

    }
    private void ShowBookingDetails()
    {
        grdReport.DataSource = null;
        string[] strCustomerNamePart = txtCName.Text.Split('-');
        grdReport.DataBind();       
        DataSet dsMain = new DataSet();
        try
        {
            Ob = IntialiazeValueInGlobal();
            var bIsAll = true;
            if (!string.IsNullOrEmpty(_allItems))
                bIsAll = false;
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetPendingStockReport(Ob, bIsAll);
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
                    Gridexcel.DataSource = dsMain.Tables[0];
                    Gridexcel.DataBind();
                    CalculateGridReport();
                }
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
            
        }
    }    
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
       
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Pending Stock Report", false);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }    
    protected void txtCName_TextChanged(object sender, EventArgs e)
    {       
        try
        {
            string[] CustName = txtCName.Text.Split('-');
            hdnCustId.Value = CustName[0].ToString();
            setCustvalue(CustName[0].ToString());
            Obp.BranchId = Globals.BranchID;
            Obp.CustomerCode = hdnCustId.Value;
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalCustomer(Obp) != true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Please enter valid customer.";
                txtCName.Text = "";
                txtItemName.Attributes.Add("onfocus", "javascript:select();");
                grdReport.DataSource = null;
                grdReport.DataBind();
            }
            btnShowReport_Click(null, null);
            txtCName.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Please enter valid customer.";
            txtCName.Text = "";
            txtCName.Focus();
            txtItemName.Attributes.Add("onfocus", "javascript:select();");
            grdReport.DataSource = null;
            grdReport.DataBind();
        }
    }
    public void setCustvalue(string customerName)
    {
        DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName,Globals.BranchID);
        if (DS_CustInfo.Tables[0].Rows.Count > 0)
            txtCName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
    }    
    protected void txtItemName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var ItemName = txtItemName.Text.Split('-');
            txtItemName.Text = ItemName[1].ToString();
            if (BAL.BALFactory.Instance.BAL_Item.CheckCorrectItem(txtItemName.Text.Trim(), Globals.BranchID, true) == true)
            {
                if (string.IsNullOrEmpty(_allItems)) { _allItems += txtItemName.Text; }
                else { _allItems += ", " + txtItemName.Text.Trim(); }
                btnShowReport_Click(txtItemName, e);
                txtItemName.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Please enter valid item name.";
                txtItemName.Focus();
                txtItemName.Attributes.Add("onfocus", "javascript:select();");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Please enter valid item name.";
            txtItemName.Focus();
            txtItemName.Attributes.Add("onfocus", "javascript:select();");
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        btnShowReport_Click(btnClear, EventArgs.Empty);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Reports/Pendingstockreport.aspx");
    }
}