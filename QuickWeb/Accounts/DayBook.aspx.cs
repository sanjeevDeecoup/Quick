using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

public partial class DayBook : System.Web.UI.Page
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
            if (Globals.UserType != "1")
            {
                txtReportUpto_CalendarExtender.Enabled = false;
                tdForMonthSelection.Visible = false;
            }
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string strSqlQuery = string.Empty;
        ViewState["exlquery"] = null;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text;
            strFromDate = strFromDate + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            strToDate = dt.ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strGridCap = "Daybook from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            strGridCap = "Daybook for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }

        ShowDayBookDetails(strFromDate, strToDate);
        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport();

            btnExport.Visible = true;
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
            if (radReportFrom.Checked)
                strGridCap = "Daybook from " + txtReportFrom.Text + " to " + txtReportUpto.Text;
            else if (radReportMonthly.Checked)
                strGridCap = "Daybook for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }
        else
        {
            btnExport.Visible = false;
            strGridCap = "";
        }
        grdReport.Caption = strGridCap;
    }

    private void ShowDayBookDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_DayBookReport";
        cmd.Parameters.Add(new SqlParameter("BookingDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookingDate2", strToDate));
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
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
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

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float OrderCount = 0, TotalDC = 0, TotalWC = 0, TotalCharges = 0;
        for (int r = 0; r < rc; r++)
        {
            OrderCount++;
            if (grdReport.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
            {
                TotalDC += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                TotalWC += float.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                TotalCharges += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);
            }
        }
        grdReport.FooterRow.Cells[1].Text = OrderCount.ToString();
        grdReport.FooterRow.Cells[2].Text = TotalDC.ToString();
        grdReport.FooterRow.Cells[3].Text = TotalWC.ToString();
        grdReport.FooterRow.Cells[4].Text = TotalCharges.ToString();
        grdReport.FooterRow.Cells[5].Text = "0";
        grdReport.FooterRow.Cells[6].Text = TotalCharges.ToString();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strRepTitle = grdReport.Caption;
        string[] Resp = PrjClass.GenerateExcelReportFromGridView(grdReport, strRepTitle);
        if (Resp[0] == "1")
            Response.Redirect(Resp[1]);
        else
        {
            lblMsg.Text = Resp[1];
            Resp = PrjClass.GenerateCSVReportFromGridView(grdReport, strRepTitle);
            if (Resp[0] == "1")
                Response.Redirect(Resp[1]);
            else
                lblMsg.Text = " Could not provide Report at the time. Please try after some time." + Resp[1];
        }
    }
}