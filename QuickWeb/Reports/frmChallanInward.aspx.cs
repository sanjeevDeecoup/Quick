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
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace QuickWeb.Reports
{
    public partial class frmChallanInward : System.Web.UI.Page
    {
        ArrayList date = new ArrayList();
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtReportFrom.Text = Convert.ToDateTime(date[0].ToString()).ToString("dd MMM yyyy");
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
        protected void btnShowReport_Click(object sender, EventArgs e)
        {           
            string strSqlQuery = string.Empty;
            ViewState["exlquery"] = null;
            
            if (radReportFrom.Checked)
            {
                if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
                DateTime dt = DateTime.Parse(txtReportUpto.Text);
                DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
                DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
                strFromDate = txtReportFrom.Text;
                strToDate = txtReportUpto.Text;
                ShowChallanDetails();   
            }
            else if (radReportMonthly.Checked)
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                strFromDate = dt.ToString("dd MMM yyyy");
                strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                ShowChallanDetails();   
            }                      
        }       
        private void ShowChallanDetails()
        {
            lblItem.Text = "";
            lblQuantity.Text = "";
            SqlCommand cmd = new SqlCommand();
            DataSet ds1 = new ChallanInwardDataSet();
            DataSet ds2 = new ChallanInwardDetailedDataSet();
            if (drpOption.SelectedItem.Text == "Summary")
            {
                DataSet dsItem = new DataSet();
                cmd.CommandText = "sp_ChallanInwardSummary";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@BookingDate1", strFromDate);
                cmd.Parameters.AddWithValue("@BookingDate2", strToDate);
                dsItem = PrjClass.GetData(cmd);
                if (dsItem.Tables[0].Rows.Count > 0)
                {
                    ReportViewer1.Visible = true;
                    btnPrint.Visible = true;
                    lblmsg.Text = "";
                }
                else
                {
                    lblmsg.Text = "No Record Found";
                    ReportViewer1.Visible = false;
                    btnPrint.Visible = false;
                }
           
            ds1 = dsItem;
            ReportViewer1.LocalReport.ReportPath = "RDLC/ChallanInward.rdlc";          
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds1.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);           
            ReportViewer1.LocalReport.Refresh();
       
            }
            else
            {
                DataSet dsInward = new DataSet();
                cmd.CommandText = "proc_ChallanInwardSummary";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@BookingDate1", strFromDate);
                cmd.Parameters.AddWithValue("@BookingDate2", strToDate);
                dsInward = PrjClass.GetData(cmd);
                if (dsInward.Tables[0].Rows.Count > 0)
                {
                    ReportViewer1.Visible = true;
                    btnPrint.Visible = true;
                    lblmsg.Text = "";
                }
                else
                {
                    lblmsg.Text = "No Record Found";
                    ReportViewer1.Visible = false;
                    btnPrint.Visible = false;
                }
                ds2 = dsInward;
                ReportViewer1.LocalReport.ReportPath = "RDLC/ChallanInwardSummary.rdlc";
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "ChallanDetailed";
                rds.Value = ds2.Tables[0];
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
            }
           
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
        "  <MarginLeft>0.2in</MarginLeft>" +
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
}