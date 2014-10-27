using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

public partial class SalesLedger : System.Web.UI.Page
{
    private ArrayList date = new ArrayList();

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
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Globals.UserType != "1")
        {
            grdReport.ShowFooter = false;
            btnExport.Visible = false;
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string strSqlQuery = string.Empty;
        ViewState["exlquery"] = null;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {
            //strFromDate = txtReportFrom.Text;
            //strFromDate = strFromDate + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            //strToDate = dt.AddDays(1).ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strFromDate = txtReportFrom.Text;
            strToDate = txtReportUpto.Text;
            strGridCap = "Sales Ledger Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            strGridCap = "Sales Ledger Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }

        ShowReport(strFromDate, strToDate, grdReport, 0);
        grdPaymentDetails.DataSource = null;
        grdPaymentDetails.DataBind();
        grdPaymentDetails.Visible = false;
        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport();
            ViewState["exlquery"] = strSqlQuery;
            btnExport.Visible = true;
            grdReport.Visible = true;
        }
        else
        {
            btnExport.Visible = false;
            grdReport.Visible = false;
            lblMsg.Text = "No Record Found";
        }
    }

    private void ShowReport(string strStartDate, string strToDate, GridView grd, int tableIndex)
    {
        grd.DataSource = null;
        grd.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_SalesLedgerReport";
        cmd.Parameters.Add(new SqlParameter("PaymentDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("PaymentDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("BranchId", Globals.BranchID));

        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataAdapter sadp = new SqlDataAdapter();
        DataSet dsMain = new DataSet();
        try
        {
            sqlcon.Open();
            cmd.Connection = sqlcon;
            sadp.SelectCommand = cmd;
            sadp.Fill(dsMain);
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[tableIndex].Rows.Count > 0)
                {
                    if (dsMain.Tables[tableIndex].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[tableIndex].Rows[0][1].ToString());
                    }
                    grd.DataSource = dsMain.Tables[tableIndex];
                    grd.DataBind();
                    grd.Visible = true;
                }
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sqlcon.Dispose();
        }
    }

    protected void grdReport_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CMDShowPaymentDetail")
        {
            string strPaymentDate = e.CommandArgument.ToString();
            ShowReport(strPaymentDate, strPaymentDate, grdPaymentDetails, 1);
        }
    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float TotalNetPaidCount = 0;
        for (int r = 0; r < rc; r++)
        {
            TotalNetPaidCount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text.Replace("&nbsp;", ""));
        }
        grdReport.FooterRow.Cells[1].Text = TotalNetPaidCount.ToString();
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

    protected void grdPaymentDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShowPaymentDetail")
        {
            string BookingNo = e.CommandArgument.ToString();
            ShowPaymentDetails(BookingNo);
        }
    }

    private void ShowPaymentDetails(string BookingNo)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_BarcodeMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookingNo", BookingNo);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 14);
        ds = AppClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdPaymentDate.DataSource = ds;
            grdPaymentDate.DataBind();
        }
    }
}