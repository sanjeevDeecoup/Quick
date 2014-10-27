using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;

public partial class ExpenseReport : System.Web.UI.Page
{
    private ArrayList date = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
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
                BindDropDown();
                btnShowReport_Click(null, null);
            }
            catch (Exception) { }
        }
    }

    public void BindDropDown()
    {
        SDTExpenseLedgers.SelectCommand = "SELECT [LedgerName] FROM [LedgerMaster] WHERE BranchId='" + Globals.BranchID + "' AND (([LedgerName] NOT LIKE '%' + 'CASH' + '%') AND ([LedgerName] NOT LIKE '%' + 'CUST' + '%') AND ([LedgerName] NOT LIKE '%' + 'Sales' + '%')) ";
        SDTExpenseLedgers.DataBind();
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strReportCaption = string.Empty, strSqlQuery = string.Empty;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {
            //strFromDate = txtReportFrom.Text + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            //strToDate = dt.ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strFromDate = txtReportFrom.Text + " 00:00:00";
            strToDate = txtReportUpto.Text + " 00:00:00";
            strGridCap = "Expense Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToShortDateString() + " 00:00:00";
            strToDate = dt.AddMonths(1).ToShortDateString() + " 00:00:00";
            strGridCap = "Expense Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }
        DateTime dtStart = DateTime.Parse(strFromDate);
        DateTime dtEnd = DateTime.Parse(strToDate);
        DataTable dtTmp = new DataTable();
        DataSet ds = new DataSet();
        SqlDataAdapter sadp = new SqlDataAdapter();
        if (drpExpReportType.SelectedValue == "1")
        {
            dtTmp = new DataTable();
            dtTmp.Columns.Add("Expense Date");
            dtTmp.Columns.Add("Amount");
        }
        else if (drpExpReportType.SelectedValue == "2")
        {
            dtTmp = new DataTable();
            dtTmp.Columns.Add("Expense Date");
            dtTmp.Columns.Add("Particulars");
            dtTmp.Columns.Add("Account Type");
            dtTmp.Columns.Add("Amount");
        }
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        try
        {
            for (DateTime dt = dtStart; dt <= dtEnd; dt = dt.AddDays(1))
            {
                if (drpExpReportType.SelectedValue == "1")
                {
                    if (drpLedger.SelectedItem.Text == "All")
                        strSqlQuery = "SELECT '" + dt.ToString("dd MMM yyyy") + "' As Expense_Date, SUM(Credit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'To %') AND TransDate='" + dt.ToString("dd/MMM/yyyy") + "' AND BranchId='" + Globals.BranchID + "' GROUP BY LedgerName";
                    else
                        strSqlQuery = "SELECT '" + dt.ToString("dd MMM yyyy") + "' As Expense_Date, SUM(Credit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'To %') AND TransDate='" + dt.ToString("dd/MMM/yyyy") + "' AND BranchId='" + Globals.BranchID + "' AND Particulars='" + "To " + drpLedger.SelectedItem.Text + "' GROUP BY LedgerName";
                }
                else if (drpExpReportType.SelectedValue == "2")
                {
                    if (drpLedger.SelectedItem.Text == "All")
                        strSqlQuery = "SELECT '" + dt.ToString("dd MMM yyyy") + "' As Expense_Date, 'Paid '+Particulars+' ('+Narration+') ' As Particulars, SUBSTRING(Particulars,4,LEN(Particulars)) As [Account Type], SUM(Credit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'To %')  AND TransDate='" + dt.ToString("dd/MMM/yyyy") + "' AND BranchId='" + Globals.BranchID + "' GROUP BY LedgerName, Particulars,Narration";
                    else
                        strSqlQuery = "SELECT '" + dt.ToString("dd MMM yyyy") + "' As Expense_Date, 'Paid '+Particulars+' ('+Narration+') ' As Particulars, SUBSTRING(Particulars,4,LEN(Particulars)) As [Account Type], SUM(Credit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'To %')  AND TransDate='" + dt.ToString("dd/MMM/yyyy") + "' AND BranchId='" + Globals.BranchID + "' AND Particulars='" + "To " + drpLedger.SelectedItem.Text + "' GROUP BY LedgerName, Particulars,Narration";
                }
                sadp = new SqlDataAdapter(strSqlQuery, sqlcon);
                ds = new DataSet();
                sadp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        DataRow dr = dtTmp.NewRow();
                        for (int c = 0; c < ds.Tables[0].Columns.Count; c++)
                        {
                            dr[c] = ds.Tables[0].Rows[r][c].ToString();
                        }
                        dtTmp.Rows.Add(dr);
                    }
                }
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error:" + excp.Message;
        }
        finally
        {
            sqlcon.Close();
        }
        grdReport.DataSource = dtTmp;
        grdReport.DataBind();
        grdReport.Visible = true;
        if (grdReport.Rows.Count > 0)
        {
            //CalculateGridReport();
            AppClass.CalcuateAndSetGridFooter(ref grdReport);
            btnExport.Visible = true;
            if (radReportFrom.Checked)
                strGridCap = "Expense Report from " + txtReportFrom.Text + " to " + txtReportUpto.Text;
            else if (radReportMonthly.Checked)
                strGridCap = "Expense Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }
        else
        {
            btnExport.Visible = false;
            strGridCap = "";
        }
        grdReport.Caption = strGridCap;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "googlechart();", true);
    }

    private void CalculateGridReport()
    {
        try
        {
            int rc = grdReport.Rows.Count;
            int cc = grdReport.HeaderRow.Cells.Count;
            float TotalExpense = 0;
            int LastColIndex = cc - 1;
            for (int r = 0; r < rc; r++)
            {
                TotalExpense += float.Parse(grdReport.Rows[r].Cells[LastColIndex].Text);
            }
            grdReport.FooterRow.Cells[LastColIndex].Text = TotalExpense.ToString();
        }
        catch (Exception ex) { }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;

        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);

        string strFilePathToSave = string.Empty;
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }
}