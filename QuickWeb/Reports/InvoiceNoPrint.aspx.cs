using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;


public partial class Reports_InvoiceNoPrint : BasePage
{
    DTO.Report Ob = new DTO.Report();
    DTO.BarCodeSetting Ob1 = new DTO.BarCodeSetting();
    public string strPreview = string.Empty;
    public string OldBarCodeWidth = string.Empty;
    public string OldBarCodeHeight = string.Empty;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindddBooking();
            ddlinvoice.Focus();
            ddlinvoice.Attributes.Add("onfocus", "javascript:select();");

        }
    }
    public void BindddBooking()
    {
        ddlinvoice.Items.Clear();
        Ob.BranchId = Globals.BranchID;
        ddlinvoice.DataSource = BAL.BALFactory.Instance.Bal_Report.PrintBarcodeDropDown(Ob);
        ddlinvoice.DataTextField = "BookingNumber";
        ddlinvoice.DataValueField = "BookingNumber";
        ddlinvoice.DataBind();
        BindddRowindex();
    }

    public void BindddRowindex()
    {
        ddlno.Items.Clear();
        ddlno.Items.Insert(0, new ListItem("All", "0"));
        ddlno.AppendDataBoundItems = true;
        SqlCommand cmdnew = new SqlCommand();
        cmdnew.CommandText = "sp_rpt_barcodprint";
        cmdnew.CommandType = CommandType.StoredProcedure;
        cmdnew.Parameters.AddWithValue("@BookingNo", ddlinvoice.SelectedValue);
        cmdnew.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmdnew.Parameters.AddWithValue("@Flag", 2);
        DataSet dsnew = new DataSet();
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataAdapter da = new SqlDataAdapter();
        cmdnew.Connection = sqlcon;
        da.SelectCommand = cmdnew;
        da.Fill(dsnew);
        ddlno.DataSource = dsnew.Tables["table"];
        ddlno.DataTextField = "RowIndex";
        ddlno.DataValueField = "RowIndex";
        ddlno.DataBind();
        ddlno.AppendDataBoundItems = false;

        /* For Grid */
        BindGrid();

    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        GetSerialAndPrint();
    }

    protected void ddlinvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddRowindex();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        GetSerialAndPrint(true);
    }

    protected void BindGrid()
    {
        try
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            var sqlCommand = new SqlCommand { CommandText = "sp_rpt_barcodprint", CommandType = CommandType.StoredProcedure };
            sqlCommand.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            sqlCommand.Parameters.AddWithValue("@BookingNo", ddlinvoice.SelectedValue);
            sqlCommand.Parameters.AddWithValue("@Flag", 8);
            grdReport.DataSource = PrjClass.GetData(sqlCommand);
            grdReport.DataBind();
        }
        catch (Exception)
        {

        }
    }


    /// <summary>
    /// Calls the base page method to open the url
    /// </summary>
    /// <param name="urlToOpen">the url to open</param>
    protected void OpenUrlFromBasePage(string urlToOpen)
    {
        OpenWindow(this.Page, urlToOpen);
    }

    protected string GetItemSerialNumbers()
    {
        if (string.Equals(hdnPrintingFromGrid.Value, "false", StringComparison.Ordinal))
            return ddlno.SelectedValue;
        else
        {
            var allIndiciesString = string.Empty;
            var allIndicies = AppClass.GetCheckedCheckBox(grdReport, 0, 1, 1, true, 1);
            foreach (var index in allIndicies)
                allIndiciesString += index + ",";

            if (allIndiciesString.Length >= 1)
                allIndiciesString = allIndiciesString.Substring(0, allIndiciesString.Length - 1);

            return allIndiciesString;
        }
    }

    protected void GetSerialAndPrint(bool directPrint = false)
    {
        var itemSerialNumbers = GetItemSerialNumbers();
        var urlToOpen = "../Reports/Barcodet.aspx?bookingNo=" + ddlinvoice.SelectedValue + "&id=" + itemSerialNumbers + "&Index=" + ddlno.SelectedIndex;
        if (directPrint)
        {
            urlToOpen += "&PrintBarCode=true&CloseWindow=true";
        }
        OpenUrlFromBasePage(urlToOpen);
        SaveInvoiceHistory(directPrint, itemSerialNumbers);
        BindddBooking();     
    }

    private void SaveInvoiceHistory(bool directprint1, string itemSerialNumbers1)
    {
        string strData = string.Empty;
        if (itemSerialNumbers1 != "0")
        {
            foreach (GridViewRow row in grdReport.Rows)
            {
                CheckBox chk = row.Cells[0].FindControl("CheckBox1") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    strData = strData + row.Cells[3].Text + ":" + row.Cells[2].Text + ", ";
                }
            }           
        }
        else
        {
            foreach (GridViewRow row in grdReport.Rows)
            {
                strData = strData + row.Cells[3].Text + ":" + row.Cells[2].Text + ", ";
            }
        }
        if (strData.Length > 0)
            strData = strData.Substring(0, strData.Trim().Length - 1);

        if (directprint1 == false)
        {
            string PreviewMsg = string.Empty;
            PreviewMsg = "Preview garment tags : " + strData + ".";
            BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(ddlinvoice.SelectedItem.Text, Globals.UserName, Globals.BranchID, PreviewMsg, ScreenName.PrintBarcode,8);
        }
        else if (directprint1 == true)
        {
            string PrintMsg = string.Empty;
            PrintMsg = "Print garment tags : " + strData + ".";
            BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(ddlinvoice.SelectedItem.Text, Globals.UserName, Globals.BranchID, PrintMsg, ScreenName.PrintBarcode,8);
        }

    }

}