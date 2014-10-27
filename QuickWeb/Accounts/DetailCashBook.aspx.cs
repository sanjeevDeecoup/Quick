using System;
using System.Collections;
using System.Web.UI;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace QuickWeb.Accounts
{
    public partial class DetailCashBook : System.Web.UI.Page
    {
        private ArrayList date = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);              
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();

                btnShowReport_Click(null, null);                
            }           
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            string strSqlQuery = string.Empty;
            ViewState["exlquery"] = null;
            string strFromDate = string.Empty, strToDate = string.Empty;
           
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();
            ShowCashDetails(strFromDate, strToDate);
        }

        private void ShowCashDetails(string strStartDate, string strToDate)
        {
            DataSet dsMain = new DataSet();
            string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowDetailCashBook(Globals.BranchID, strStartDate, strToDate);
            ReportViewer1.LocalReport.ReportPath = "RDLC/DetailCashReport.rdlc";
            //ReportViewer1.LocalReport.EnableHyperlinks = true;
            //ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("FDate", strStartDate);
            parameters[1] = new ReportParameter("UDate", strToDate);
            parameters[2] = new ReportParameter("UserName", Globals.UserName);
            parameters[3] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
            //parameters[2] = new ReportParameter("Link", str);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DetailCash_CashDetail";
            rds.Value = dsMain.Tables[0];
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                ReportViewer1.Visible = true;
                bool blnRight = AppClass.CheckExportToExcelRightOnPage();
                if (blnRight)
                {
                    ReportViewer1.ShowExportControls = true;
                }
                else
                {
                    ReportViewer1.ShowExportControls = false;
                }  
                btnPrint.Visible = true;
            }
            else
            {
                ReportViewer1.Visible = false;
                btnPrint.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "No detail cash book entry found.";
            }
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
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
        "  <PageWidth>10in</PageWidth>" +
        "  <PageHeight>11in</PageHeight>" +
        "  <MarginTop>0.2in</MarginTop>" +
        "  <MarginLeft>1in</MarginLeft>" +
        "  <MarginRight>.5in</MarginRight>" +
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

            var fileName = "OutPut.pdf";
            PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(renderedBytes);
            //Response.End();
        }
    }
}