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
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace QuickWeb.Reports
{
    public partial class BillDetailReport : BasePage
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
                if (Request.QueryString["Date"] != null & Request.QueryString["Date1"] != null & Request.QueryString["Challan"] != null)
                {
                    SetDatesBasedOnQueryString(Request.QueryString["Date"] as string, Request.QueryString["Date1"] as string);
                    ShowCashDetails(Request.QueryString["Date"].ToString(), Request.QueryString["Date1"].ToString(), challanNumber: Request.QueryString["Challan"].ToString(CultureInfo.InvariantCulture));
                }
                if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                    hdnDirectPrint.Value = "true";
                
                if (Request.QueryString["CloseWindow"] != null && Request.QueryString["CloseWindow"].ToString() == "true")
                    hdnCloseWindow.Value = "true";
                
                if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() != string.Empty)
                    hdnRedirectBack.Value = Request.QueryString["RedirectBack"].ToString();

                if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                {
                    hdnDirectPrint.Value = "true";
                    btnPrint_Click(null, EventArgs.Empty);
                }
            }

        }

        protected void SetDatesBasedOnQueryString(string startDate, string endDate)
        {

        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            string strSqlQuery = string.Empty;
            
            //string strCustCode = "" + txt .Text.Substring(1, lblCustomerLedgerName.Text.IndexOf(')') - 1);
            ViewState["exlquery"] = null;
            string strFromDate = string.Empty, strToDate = string.Empty;
            if (radReportFrom.Checked)
            {
                strFromDate = txtReportFrom.Text;
                DateTime dt = DateTime.Parse(txtReportUpto.Text);
                strToDate = txtReportUpto.Text; 
            }
            else if (radReportMonthly.Checked)
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                strFromDate = dt.ToString("dd MMM yyyy");
                strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            ShowCashDetails(strFromDate, strToDate);
        }

        private void ShowCashDetails(string strStartDate, string strToDate, string challanNumber = "")
        {           
            DataSet dsMain = new DataSet();
            if (!string.IsNullOrEmpty(challanNumber))
            {
                /* I could have used optional parameter with the method below, but though there are benefits
                 * there are caveats too, major ones being, expression tree is not supported
                 * see http://lostechies.com/jimmybogard/2010/05/18/caveats-of-c-4-0-optional-parameters/
                 * for more details, or google "optional parameters vs method overloads"
                 */
                dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowBillDetail(Globals.BranchID, strStartDate, strToDate, challanNumber);
            }
            else
            {
                dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowBillDetail(Globals.BranchID, strStartDate, strToDate);
                if (!chkShowCustomer.Checked)
                {
                    dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowBillDetail(Globals.BranchID, strStartDate, strToDate);
                }
                else
                {
                    string CustCode = txtCustomerSearch.Text.Substring(0, txtCustomerSearch.Text.IndexOf('-') - 2);
                    dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowBillDetailwithCustomer(Globals.BranchID, strStartDate, strToDate, CustCode);
                }
            }
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                ReportViewer1.LocalReport.ReportPath = "RDLC/BillDetails.rdlc";
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "BillDetail";
                rds.Value = dsMain.Tables[0];
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    ReportViewer1.Visible = true;
                    btnPrint.Visible = true;
                }
                else
                {
                    ReportViewer1.Visible = false;
                    btnPrint.Visible = false;
                    //lblErr.Text = "No Record Found";
                }
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
        "  <MarginTop>0.1in</MarginTop>" +
        "  <MarginLeft>0.2in</MarginLeft>" +
        "  <MarginRight>0.2in</MarginRight>" +
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
            // probably won't work
            if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
            {
                var redirectURL = Request.QueryString["RedirectBack"] as string;
                if (redirectURL != null)
                {
                    OpenNewWindow(redirectURL);
                }
            }

            Response.End();
       
        }

        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }


        protected void chkShowCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowCustomer.Checked)
            {
                ReportViewer1.Visible = false;
                txtCustomerSearch.Text = "";
                txtCustomerSearch.Visible = true;
                txtCustomerSearch.Focus();

            }
            else
            {
                txtCustomerSearch.Visible = false;
                ReportViewer1.Visible = false;
 
            }   
        }
    }
}