using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Collections;

namespace QuickWeb.Reports
{
    public partial class PQDR : System.Web.UI.Page
    {
        Boolean checkstat;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string AID = string.Empty, CustCode = string.Empty;
                if (Request.QueryString["PkgId"] != null)
                    AID = Request.QueryString["PkgId"].ToString();
                if (Request.QueryString["CustId"] != null)
                    CustCode = Request.QueryString["CustId"].ToString();
                if (Request.QueryString["CloseMe"] != null)
                    hdnCloseMe.Value = "true";
                ShowPackageQtyDetail(AID, CustCode);               
                btnPrint_Click(null, null);
            }
        }      

        public void ShowPackageQtyDetail(string AID,string CustCode)
        {
            DataSet dsMain = new DataSet();
            
            checkstat = false;
            dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowPackageQty(Globals.BranchID, AID, CustCode);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                ReportViewer1.LocalReport.ReportPath = "RDLC/PQDR.rdlc";
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet1";
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
                checkstat = true;
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
            if (checkstat == false)
            {
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>9.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>.2n</MarginLeft>" +
            "  <MarginRight>.2in</MarginRight>" +
            "  <MarginBottom>.5in</MarginBottom>" +
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
                //var fileName = "OutPut.pdf";
                //PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);           
            }
        }
    }
}