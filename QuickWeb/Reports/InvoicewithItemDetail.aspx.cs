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
using QuickWeb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;
using QuickWeb.Reports;
using System.Management;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;


namespace QuickWeb.Reports
{
    public partial class InvoicewithItemDetail : System.Web.UI.Page
    {
        string CheckReport = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckReport = "2";
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand();
                DataSet dsReport = new ChallanDataSet();
                cmd = new SqlCommand();
                cmd.CommandText = "Sp_InvoiceWithItemDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                if (Request.QueryString["Bookings"] != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Bookings"].ToString()))
                    {
                        cmd.Parameters.AddWithValue("@Bookings", Request.QueryString["Bookings"].ToString());
                        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                        cmd.Parameters.AddWithValue("@Flag", 2);
                    }
                }
                else
                {
                    if (Request.QueryString["Date"] == null || Request.QueryString["Date"] == "" || Request.QueryString["Date1"] == null || Request.QueryString["Date1"] == "" || Request.QueryString["Challan"] == null || Request.QueryString["Challan"] == "")
                    {
                        Response.Redirect(FormsAuthentication.DefaultUrl, false);
                    }
                    if (Request.QueryString["Date"] == null || Request.QueryString["Date"] == "" || Request.QueryString["Date1"] == null || Request.QueryString["Date1"] == "" || Request.QueryString["Challan"] == null || Request.QueryString["Challan"] == "")
                    {
                        Response.Redirect(FormsAuthentication.DefaultUrl, false);
                    }
                    if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                    {
                        hdnDirectPrint.Value = "true";
                    }
                    if (Request.QueryString["CloseWindow"] != null && Request.QueryString["CloseWindow"].ToString() == "true")
                    {
                        hdnCloseWindow.Value = "true";
                    }
                    if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() != string.Empty)
                    {
                        hdnRedirectBack.Value = Request.QueryString["RedirectBack"].ToString();
                    }
                    cmd.Parameters.AddWithValue("@ChallanNo", Request.QueryString["Challan"].ToString());
                    cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                }
                //hdnPrinterName.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
               
               

                dsReport = PrjClass.GetData(cmd);
                ds = PrjClass.GetData(cmd);
                string FactoryName = string.Empty, Factory = string.Empty;
                if (Request.QueryString["Challan"] != null)
                {
                    CheckReport = "1";
                    FactoryName = BAL.BALFactory.Instance.BL_Branch.GetWorkShopName(Globals.BranchID, Request.QueryString["Challan"].ToString());
                    if (FactoryName == "")
                    {
                        Factory = "";
                    }
                }
                else
                {
                    Factory = "WorkShop";
                }
                
                if (CheckReport!="2")
                ReportViewer1.LocalReport.ReportPath = "RDLC/InvoicewithItemDetail.rdlc";
                else
                    ReportViewer1.LocalReport.ReportPath = "RDLC/BookingWithItemDetails.rdlc";
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet1";
                bool rights = AppClass.GetShowFooterRightsUser();
                string rvalue = string.Empty;
                if (rights == true)
                    rvalue = "1";
                else
                    rvalue = "0";
                ReportParameter[] parameters = new ReportParameter[9];
                parameters[0] = new ReportParameter("FDate", (Request.QueryString["Date"] ?? "").ToString());
                parameters[1] = new ReportParameter("UDate", (Request.QueryString["Date1"] ?? "").ToString());
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        lblStoreName.Text += ds.Tables[1].Rows[i]["Item"].ToString();
                    lblQuantity.Text += ds.Tables[2].Rows[0]["QTY"].ToString();
                }
                string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
                parameters[2] = new ReportParameter("details", lblStoreName.Text);
                parameters[3] = new ReportParameter("Qty", lblQuantity.Text);
                parameters[4] = new ReportParameter("Factoryname", FactoryName);
                parameters[5] = new ReportParameter("Factory", Factory);
                parameters[6] = new ReportParameter("UserName", Globals.UserName);
                parameters[7] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
                parameters[8] = new ReportParameter("TotalFooter", rvalue);
                ReportViewer1.LocalReport.SetParameters(parameters);
                rds.Value = dsReport.Tables[0];
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
            "  <PageWidth>9.0in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.4in</MarginRight>" +
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

                if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                {
                    var redirectURL = Request.QueryString["RedirectBack"] as string;
                    if (redirectURL != null)
                    {
                        OpenNewWindow(redirectURL);
                    }
                }

                Response.End();
                if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                {
                    hdnDirectPrint.Value = "true";
                    ImgPrintButton_Click(null, EventArgs.Empty);
                }
            }
        }
        //private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        //{
        //    string file = Server.MapPath("~/SQL/");
        //    Stream stream = new FileStream(file + name +
        //       "." + fileNameExtension, FileMode.Create);
        //    m_streams.Add(stream);
        //    return stream;
        //}

        //protected void makePrint()
        //{

        //    var setPrinter = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);

        //    if (m_streams == null || m_streams.Count == 0)
        //        return;
        //    PrintDocument printDoc = new PrintDocument();
        //    printDoc.PrinterSettings.PrinterName = setPrinter;
        //    printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //    printDoc.Print();

        //}



        protected void ImgPrintButton_Click(object sender, EventArgs e)
        {
            # region UnusedCode
            /*
        string deviceInfo =
    "<DeviceInfo>" +
    "  <OutputFormat>EMF</OutputFormat>" +
    "  <PageWidth>8.5in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.2in</MarginTop>" +
    "  <MarginLeft>0.2in</MarginLeft>" +
    "  <MarginRight>0.5in</MarginRight>" +
    "  <MarginBottom>0.2in</MarginBottom>" +
    "</DeviceInfo>";
        Warning[] warnings;
        m_streams = new List<Stream>();
        //string[] streams;
        //byte[] renderedBytes;
        //renderedBytes = ReportViewer1.LocalReport.Render(
        //reportType,
        //deviceInfo,
        //out mimeType,
        //out encoding,
        //out fileNameExtension,
        //out streams,
        //out warnings);
        ReportViewer1.LocalReport.Render("Image", deviceInfo, CreateStream, out warnings);
        foreach (Stream stream in m_streams)
            stream.Position = 0;
        //Response.Clear();
        //Response.ContentType = mimeType;
        //Response.BinaryWrite(renderedBytes);
        if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
        {
            //if (saveCurReport(renderedBytes))
            //{
            makePrint();
            Dispose();
            Response.Redirect("~/Bookings/NewChallan.aspx");
            //}
        }
         */
            # endregion

        }

        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }

       
    }
}
