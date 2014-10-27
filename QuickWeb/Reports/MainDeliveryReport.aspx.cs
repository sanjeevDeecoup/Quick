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

public partial class Reports_MainDeliveryReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            txtReportFrom.Text = date[0].ToString();
            txtReportUpto.Text = date[0].ToString();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2000; i <= 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
        }
    }
    protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CMDShowDetails")
        {
            string BookingNo = e.CommandArgument.ToString();
            ShowDetails(BookingNo);
            grdStockDetails.Caption = BookingNo + " Details";
        }
    }

    protected void ShowDetails(string BookingNo)
    {
        grdStockDetails.DataSource = null;
        grdStockDetails.DataBind();
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "Sp_Sel_NewDeliveryReport";
        cmd.CommandType = CommandType.StoredProcedure;        
        cmd.Parameters.AddWithValue("@BookingNo", BookingNo);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID); 
        cmd.Parameters.AddWithValue("@Flag", 2);
        ds = AppClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdStockDetails.DataSource = ds;
            grdStockDetails.DataBind();
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {
            //strFromDate = txtReportFrom.Text + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            //strToDate = dt.AddDays(1).ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strFromDate = txtReportFrom.Text + " 00:00:00";
            strToDate = txtReportUpto.Text + " 00:00:00";
            strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            strGridCap = "Booking Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }        
        ShowBookingDetails(strFromDate, strToDate);        
        if (grdReport.Rows.Count > 0)
        {           
            //btnExport.Visible = true;
            CalculateGridReport();
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
        }
        else
        {
            btnExport.Visible = false;
        }
    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0;
        for (int r = 0; r < rc; r++)
        {
            OrderCount++;
            if (grdReport.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
            {
                TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);               
            }
        }
        grdReport.FooterRow.Cells[1].Text = OrderCount.ToString();
        grdReport.FooterRow.Cells[2].Text = TotalCostCount.ToString();
        grdReport.FooterRow.Cells[3].Text = TotalPaid.ToString();
        grdReport.FooterRow.Cells[4].Text = TotalDue.ToString();        
    }

    private void ShowBookingDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        grdStockDetails.DataSource = null;
        grdStockDetails.DataBind();
        SqlCommand cmd = new SqlCommand();
        DataSet dsMain = new DataSet();
        cmd.CommandText = "Sp_Sel_NewDeliveryReport";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookingDate1", strStartDate);
        cmd.Parameters.AddWithValue("@BookingDate2", strToDate);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID); 
        cmd.Parameters.AddWithValue("@Flag", 1);
        dsMain = AppClass.GetData(cmd);              
        try
        {            
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
                    ViewState["SavedDS"] = dsMain;                    
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

    }
}
