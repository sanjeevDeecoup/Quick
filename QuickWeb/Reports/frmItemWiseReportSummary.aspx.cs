using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;

public partial class frmItemWiseReportSummary : System.Web.UI.Page
{
    DTO.Report Ob = new DTO.Report();
    static private string _allItems;
    public static bool blnRight = false;
    private ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            _allItems = string.Empty;
            SqlSourceItems.SelectCommand = "SELECT [ItemID], [ItemName] FROM [ItemMaster] Where BranchId='" + Globals.BranchID + "' ORDER BY ItemName";
            SqlSourceItems.DataBind();
            drpItemNames.Focus();

            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();

          
            setValues(date[0].ToString().Trim(), date[0].ToString().Trim());
            BindDropDown();
            btnShowReport_Click(sender, e);           
        }
        
    }

    public void setValues(string fromDate, string uptoDate)
    {
        string BookingPrefix = string.Empty;
        Ob.BranchId = Globals.BranchID;
        Ob.FromDate = fromDate;
        Ob.UptoDate = uptoDate;
        Ob.CustId = drpItemNames.SelectedItem.Text;
        Ob.QtyTotal = 0;
        Ob.AmountTotal = 0;
        Ob.Counter = 0;
        if (txtBkNoStart.Text != "" && txtBkNoEnd.Text != "")
        {
            BookingPrefix = drpBookingPreFix.SelectedItem.Text.Trim();
        }
        Ob.StartBkNum =  BookingPrefix.Trim()+ txtBkNoStart.Text;
        Ob.EndDate = BookingPrefix.Trim() + txtBkNoEnd.Text;
        Ob.CustCodeStr = _allItems;
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strReportCaption = string.Empty, strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;        

        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        strGridCap = "Booking Report from " + strFromDate + " to " + strToDate;

        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        if (!(sender is TextBox))
        {
            _allItems = string.Empty;
        }
        setValues(strFromDate, strToDate);
        ShowBookingDetails(strFromDate, strToDate);
        //checkCancelBooking(strSqlQuery);
        checkCancelBooking();

        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport();
            btnExport.Visible = true;
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
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

    private void ShowBookingDetails(string strStartDate, string strToDate)
    {
        bool bIsAll = false;
        if (drpItemNames.SelectedItem.Text == "All")
            bIsAll = true;
        grdReport.DataSource = null;
        grdReport.DataBind();
        DataSet ds = BAL.BALFactory.Instance.Bal_Report.GetDataItemReport(Ob, bIsAll);
        grdReport.DataSource = ds;
        grdReport.DataBind();
        if (grdReport.Rows.Count > 0)
        {           
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
        AppClass.CalcuateAndSetGridFooter(ref grdReport);       
    }
    private void CalculateGridReport()
    {
        try
        {
          
        }
        catch (Exception ex) { }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["SavedDS"];
        grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Item Wise Summary Report", false);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }

    private void checkCancelBooking()
    {
       
    }

    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Ob.DatasetMain == null || Ob.DatasetMain.Tables.Count == 0 || Ob.DatasetMain.Tables[0].Rows.Count == 0)
            Ob.DatasetMain = grdReport.DataSource as DataSet;

        if (Ob.DatasetMain == null || Ob.DatasetMain.Tables.Count == 0 || Ob.DatasetMain.Tables[0].Rows.Count == 0)
            return;

        if (e.Row.RowType == DataControlRowType.DataRow && Ob.DatasetMain.Tables[0].Rows.Count != Ob.Counter)
        {
            ((HyperLink)e.Row.Cells[0].FindControl("hplProcessLink")).NavigateUrl =
                String.Format("~//Reports/frmItemWiseDetailReport.aspx?PWC={0},{1},{2},{3},{4},{5}",
                    Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], Ob.FromDate, Ob.UptoDate, true, Ob.StartBkNum, Ob.EndDate);
            Ob.Counter++;
        }
    }

    protected void btnCompare_Click(object sender, EventArgs e)
    {
        
    }
    string[] ItemName;
    protected void txtItemName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ItemName = txtItemName.Text.Split('-');
            if (BAL.BALFactory.Instance.BAL_Item.CheckCorrectItem(ItemName[1].Trim(), Globals.BranchID) == true)
            {
                if (string.IsNullOrEmpty(_allItems)) { _allItems += txtItemName.Text.Split('-')[1].Trim(); txtItemName.Text = txtItemName.Text.Split('-')[1].Trim(); }
                else { _allItems += ", " + txtItemName.Text.Split('-')[1].Trim(); txtItemName.Text = txtItemName.Text.Split('-')[1].Trim(); }
                btnShowReport_Click(txtItemName, e);
                txtItemName.Focus();
            }
            else
            {
                Session["ReturnMsg"] = "Please enter valid item name.";
                txtItemName.Focus();
                txtItemName.Attributes.Add("onfocus", "javascript:select();");
            }
        }
        catch (Exception ex)
        {
            Session["ReturnMsg"] = "Please enter valid item name.";
            txtItemName.Focus();
            txtItemName.Attributes.Add("onfocus", "javascript:select();");
        }
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        // find the querystring and replace it to contains more values
        try
        {
            var query = ((HyperLink)grdReport.Rows[0].FindControl("hplProcessLink")).NavigateUrl;
            query = query.Substring(query.IndexOf("="));
            var queryItems = string.Empty;
            var i = 0;
            while (i < grdReport.Rows.Count)
            {
                queryItems += ((HyperLink)grdReport.Rows[i].FindControl("hplProcessLink")).Text + "-";
                i++;
            }
            queryItems = queryItems.Substring(0, queryItems.Length - 1);
            query = queryItems + query.Substring(query.IndexOf(','));
            Response.Redirect("~//Reports/frmItemWiseDetailReport.aspx?PWC=" + query, false);
        }
        catch (Exception ex)
        {

        }
    }

    private void BindDropDown()
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
            PrjClass.SetItemInDropDown(drpBookingPreFix, ds.Tables[0].Rows[0]["BookingPreFix"].ToString(), false, false);
        }
    }

}
