using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;


namespace QuickWeb.Reports
{
    public partial class FactoryChallanSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Challan"] == null || Request.QueryString["Challan"] == "")
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl, false);
                }
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand();
                DataSet dsReport = new ChallanDataSet();
                cmd = new SqlCommand();
                cmd.CommandText = "sp_FactoryChallanSummary";
                cmd.CommandType = CommandType.StoredProcedure;
                string st1 = Request.QueryString["Challan"].ToString();
                string st2 = Request.QueryString["Branch"].ToString();
                cmd.Parameters.AddWithValue("ChallanNo", Request.QueryString["Challan"].ToString());
                cmd.Parameters.AddWithValue("BranchId", Request.QueryString["Branch"].ToString());

                dsReport = PrjClass.GetData(cmd);
                ds = PrjClass.GetData(cmd);
                ReportViewer1.LocalReport.ReportPath = "RDLC/ChallanSFReport.rdlc";
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "ChallanDataSet_sp_Challan";
                ReportParameter[] parameters = new ReportParameter[2];
                
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        lblStoreName.Text += ds.Tables[1].Rows[i]["Item"].ToString();
                    lblQuantity.Text += ds.Tables[2].Rows[0]["QTY"].ToString();
                }

                parameters[0] = new ReportParameter("details", lblStoreName.Text);
                parameters[1] = new ReportParameter("Qty", lblQuantity.Text);

                ReportViewer1.LocalReport.SetParameters(parameters);
                rds.Value = dsReport.Tables[0];
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ImgPrintButton_Click(object sender, EventArgs e)
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