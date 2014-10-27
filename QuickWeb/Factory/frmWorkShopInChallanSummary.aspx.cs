using System;
using System.Data;
using System.Web.Security;
using Microsoft.Reporting.WebForms;

namespace QuickWeb.Factory
{
    public partial class frmWorkShopInChallanSummary : System.Web.UI.Page
    {
        //private int m_currentPageIndex;
        //private IList<Stream> m_streams;
        private DTO.Report Ob = new DTO.Report();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Challan"] == null)
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect(FormsAuthentication.LoginUrl, false);
                }
                Ob.InvoiceNo = Request.QueryString["Challan"].ToString();
                Ob.BranchId = Request.QueryString["ShopId"].ToString();
                DataSet ds = new DataSet();
                //DataSet dsReport = new ChallanDataSet();
                ds = BAL.BALFactory.Instance.Bal_Report.BindWorkShopChallan(Ob);
                //dsReport = ds;
                ReportViewer1.LocalReport.ReportPath = "RDLC/BranchInvoiceChallan.rdlc";

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet1";
                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("ChallanNo", Request.QueryString["Challan"].ToString());
                ////parameters[1] = new ReportParameter("UDate", Request.QueryString["Date1"].ToString());
                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //        lblStoreName.Text += ds.Tables[1].Rows[i]["Item"].ToString();
                //    lblQuantity.Text += ds.Tables[2].Rows[0]["QTY"].ToString();
                //}
                //parameters[0] = new ReportParameter("details", lblStoreName.Text);
                //parameters[1] = new ReportParameter("Qty", lblQuantity.Text);
                //parameters[2] = new ReportParameter("Factoryname", Request.QueryString["ShopName"].ToString());
                //parameters[3] = new ReportParameter("Factory", "Shop Name");
                ReportViewer1.LocalReport.SetParameters(parameters);
                rds.Value = ds.Tables[0];
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>9in</PageWidth>" +
            "  <PageHeight>11.50in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
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
                // probably won't work
                Response.End();
            }
        }

        protected void imgCancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script language=\"Javascript\">window.open = self;window.close();</script>");
        }
    }
}