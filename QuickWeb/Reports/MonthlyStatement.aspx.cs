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
using System.IO;
using System.Text;

public partial class MonthlyStatement : System.Web.UI.Page
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
            for (int i = 2009; i < 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2009;

        }
        SDTProcesses.SelectCommand = "SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] WHERE BranchId='"+ Globals.BranchID +"' ORDER BY [ProcessName]";
        SDTProcesses.DataBind();
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text;
		    if(txtReportUpto.Text==""){txtReportUpto.Text=txtReportFrom.Text;}
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            strToDate = txtReportUpto.Text;
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
            SqlCommand cmd = new SqlCommand();
            if (drpProcess.SelectedIndex == 0)
                DateWiseAllProcess(strFromDate, strToDate);
            else
                DayByDayWiseReport(strFromDate, strToDate);
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            strGridCap = "Booking Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
            if (drpProcess.SelectedIndex == 0)
                MonthlyReport(strFromDate, strToDate);
            else
                DateWiseReport(strFromDate, strToDate);
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "googlechart();", true);
    }
    public void DateWiseAllProcess(string date, string date1)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SP_AllAmountProcessWiseDayByDay";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FDATE", date);
        cmd.Parameters.AddWithValue("@UDATE", date1);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID); 
        grdReport.DataSource = PrjClass.GetData(cmd);
        grdReport.DataBind();
        CalculateGridReport1();
    }
    public void MonthlyReport(string date, string date1)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SP_AllAmountProcessWise";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FDATE", date);
        cmd.Parameters.AddWithValue("@UDATE", date1);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID); 
        grdReport.DataSource = PrjClass.GetData(cmd);
        grdReport.DataBind();
        CalculateGridReport();
    }
    public void DateWiseReport(string date ,string date1)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SP_AllAmountProcessWiseDateWise";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FDATE", date);
        cmd.Parameters.AddWithValue("@UDATE", date1);
        cmd.Parameters.AddWithValue("@PROCESSNAME", drpProcess.SelectedValue);
        cmd.Parameters.AddWithValue("@PROCESSCODE", drpProcess.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID); 
        grdReport.DataSource = PrjClass.GetData(cmd);
        grdReport.DataBind();
        CalculateGridReport();
    }
    public void DayByDayWiseReport(string date, string date1)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SP_AllAmountProcessWiseDateWiseDay";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FDATE", date);
        cmd.Parameters.AddWithValue("@UDATE", date1);
        cmd.Parameters.AddWithValue("@PROCESSNAME", drpProcess.SelectedValue);
        cmd.Parameters.AddWithValue("@PROCESSCODE", drpProcess.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID); 
        grdReport.DataSource = PrjClass.GetData(cmd);
        grdReport.DataBind();
        CalculateGridReport1();
    }
    private void CalculateGridReport()
    {
        try
        {
            int rc = grdReport.Rows.Count;
            int cc = grdReport.Columns.Count;
            float TotaltemsCount = 0, TotalNetAmount = 0; 
            for (int r = 0; r < rc; r++)
            {
                TotalNetAmount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text);
                TotaltemsCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
            }
            grdReport.FooterRow.Cells[0].Text = "Total";
            grdReport.FooterRow.Cells[1].Text = TotalNetAmount.ToString();
            grdReport.FooterRow.Cells[2].Text = TotaltemsCount.ToString();
        }
        catch (Exception)
        {            
        }
    }

    private void CalculateGridReport1()
    {
        try
        {
            int rc = grdReport.Rows.Count;
            int cc = grdReport.Columns.Count;
            float TotaltemsCount = 0, TotalNetAmount = 0;
            for (int r = 0; r < rc; r++)
            {
                TotalNetAmount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text);
                TotaltemsCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
            }
            grdReport.FooterRow.Cells[0].Text = "Total";
            grdReport.FooterRow.Cells[1].Text = TotalNetAmount.ToString();
            grdReport.FooterRow.Cells[2].Text = TotaltemsCount.ToString();
        }
        catch (Exception)
        {
        }
    }  
    protected void btnExport_Click1(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }   
}
