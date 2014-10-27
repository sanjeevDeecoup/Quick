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
using Microsoft.Reporting.WebForms;

public partial class Accounts_VoucherTransacionReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PrjClass.sqlConStr);
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            //DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            txtReportFrom.Text = date[0].ToString();
            txtReportUpto.Text = date[0].ToString();
            for (int i = 2009; i <= 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
        }
        FillExistingRecord();
        
    }
    protected void ChkCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkCustomer.Checked)
        {
            txtCustomerName.Visible = true;
            txtCustomerName.Focus();
        }

        else
        {
            txtCustomerName.Visible = false;
            txtCustomerName.Text = "";
            ReportViewer1.Visible = false;
        }
        
    }
    public void FillExistingRecord()
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 18);
        ds = AppClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblStoreName.Text = ds.Tables[0].Rows[0]["StoreName"].ToString();
            lblAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
        }
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        DataSet ds = new DataSet();
        if (radReportFrom.Checked)
        {
            ds = GetData(Ob);
        }
        if (radReportMonthly.Checked)
        {
            if (drpMonthList.SelectedValue != "0")
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                Ob.FromDate = dt.ToString("dd MMM yyyy");
                Ob.UptoDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                ds = GetData(Ob);
            }
            else
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse("1"), 1);
                Ob.FromDate = dt.ToString("dd MMM yyyy");
                Ob.UptoDate = dt.AddMonths(12).AddDays(-1).ToString("dd MMM yyyy");
                ds = GetData(Ob);
            }
        }
        ReportViewer1.Reset();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ReportViewer1.Visible = true;
            btnPrint.Visible = true;
            if (ChkCustomer.Checked)
            {
                if (drpOption.SelectedItem.Text == "Summary")
                    ReportViewer1.LocalReport.ReportPath = "RDLC/MonthlyReport.rdlc";
                else
                    ReportViewer1.LocalReport.ReportPath = "RDLC/MonthlyDetailedReport.rdlc";
            }
            else
            {
                if (drpOption.SelectedItem.Text == "Summary")
                    ReportViewer1.LocalReport.ReportPath = "RDLC/MonthAll.rdlc";
                else
                    ReportViewer1.LocalReport.ReportPath = "RDLC/MonthAllReport.rdlc";
            }
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("StoreName", lblStoreName.Text);
            parameters[1] = new ReportParameter("HeaderName", lblAddress.Text);
            parameters[2] = new ReportParameter("ClosingDate", Ob.UptoDate);
            parameters[3] = new ReportParameter("StartDate", Ob.FromDate);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            if (ChkCustomer.Checked)
            {
                if (drpOption.SelectedItem.Text == "Summary")
                    rds.Name = "MonthlyDataSet_sp_Monthly";
                else
                    rds.Name = "MonthlyDetailedDataSet_sp_Detailed";
            }
            else
            {
                if (drpOption.SelectedItem.Text == "Summary")
                    rds.Name = "MonthlyDetailedDataSet_sp_Monthly";
                else
                    rds.Name = "MonthlyDataSet_sp_All";
            }
            rds.Value = ds.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
        }
        else
        {
            lblMsg.Text = "No Records Found";
            ReportViewer1.Visible = false;
            btnPrint.Visible = false;
        }


    }
    public DTO.Report SetValue()
    {
        DTO.Report Ob = new DTO.Report();
        Ob.FromDate = txtReportFrom.Text;
        Ob.UptoDate = txtReportUpto.Text;
        Ob.CustId = hdnCustId.Value.Trim();
        Ob.BranchId = Globals.BranchID;
        return Ob;
    }
    protected void radReportFrom_CheckedChanged(object sender, EventArgs e)
    {
        //txtCustomerName.Visible = false;
    }
    protected void radReportMonthly_CheckedChanged(object sender, EventArgs e)
    {
        //txtCustomerName.Visible = false;
    }
    public DataSet GetData(DTO.Report Ob)
    {
        DataSet ds = new DataSet();
        if (!ChkCustomer.Checked)
        {
            if (drpOption.SelectedItem.Text == "Summary")
                ds = BAL.BALFactory.Instance.Bal_Report.GetMonthlyStausDetailed(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetMonthlStatusSubItem(Ob);
        }
        else
        {
            if (drpOption.SelectedValue == "Detailed")
                ds = BAL.BALFactory.Instance.Bal_Report.GetMonthlyStausByCustomer(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetMonthlyStausByDetailed(Ob);

        }
        return ds;
    }
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        string[] CustomerName = txtCustomerName.Text.Split('-');
        hdnCustId.Value = CustomerName[0].ToString();
        setCustvalue(CustomerName[0].ToString());
    }
    public void setCustvalue(string customerName)
    {
        DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
        if (DS_CustInfo.Tables[0].Rows.Count > 0)
            txtCustomerName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingAmountReport.aspx", false);
    }
    protected void drpOption_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string reportType = "PDF";
        string mimeType;
        string encoding;
        string fileNameExtension;
        string deviceInfo =
    "<DeviceInfo>" +
    "  <OutputFormat>PDF</OutputFormat>" +
    "  <PageWidth>8.5in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.2in</MarginTop>" +
    "  <MarginLeft>0.5in</MarginLeft>" +
    "  <MarginRight>0.5in</MarginRight>" +
    "  <MarginBottom>0.2in</MarginBottom>" +
    "</DeviceInfo>";
        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;
        renderedBytes = ReportViewer1.LocalReport.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);
        Response.Clear();
        Response.ContentType = mimeType;
        Response.BinaryWrite(renderedBytes);
        Response.End();
    }
}

