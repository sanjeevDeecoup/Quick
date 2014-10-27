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

public partial class Reports_ChallanSummary : BasePage
{
    private int m_currentPageIndex;
    private IList<Stream> m_streams;

    protected void Page_Load(object sender, EventArgs e)
    {
        hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
        if (!IsPostBack)
        {
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
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            DataSet dsReport = new ChallanDataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_ChallanSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ChallanNo", Request.QueryString["Challan"].ToString());
            cmd.Parameters.AddWithValue("BranchId", Globals.BranchID);
            dsReport = PrjClass.GetData(cmd);
            ds = PrjClass.GetData(cmd);
            string FactoryName = string.Empty, Factory = string.Empty;
            FactoryName = BAL.BALFactory.Instance.BL_Branch.GetWorkShopName(Globals.BranchID, Request.QueryString["Challan"].ToString());
            if (FactoryName == "")
            {
                Factory = "";
            }
            else
            {
                Factory = "WorkShop";
            }
            ReportViewer1.LocalReport.ReportPath = "RDLC/ChallanSReport.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "ChallanDataSet_sp_Challan";
            ReportParameter[] parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("FDate", Request.QueryString["Date"].ToString());
            parameters[1] = new ReportParameter("UDate", Request.QueryString["Date1"].ToString());
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    lblStoreName.Text += ds.Tables[1].Rows[i]["Item"].ToString();
                lblQuantity.Text += ds.Tables[2].Rows[0]["QTY"].ToString();
            }
            parameters[2] = new ReportParameter("details", lblStoreName.Text);
            parameters[3] = new ReportParameter("Qty", lblQuantity.Text);
            parameters[4] = new ReportParameter("FactoryName", FactoryName);
            parameters[5] = new ReportParameter("Factory", Factory);
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
       
            if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
            {
                hdnDirectPrint.Value = "true";
                
                ImgPrintButton_Click(null, EventArgs.Empty);

            }
        }
    }

    private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
    {
        string file = Server.MapPath("~/SQL/");
       
        Stream stream = new FileStream(file + name +
           "." + fileNameExtension, FileMode.Create);
        m_streams.Add(stream);
       
        return stream;
    }

    
   
    protected void ImgPrintButton_Click(object sender, EventArgs e)
    {
        # region UnusedCode
        /*
        //string reportType = "PDF";
 //       string mimeType;
 //       string encoding;
 //       string fileNameExtension;
        string deviceInfo =
    "<DeviceInfo>" +
    //"  <OutputFormat>PDF</OutputFormat>" +
    "  <OutputFormat>EMF</OutputFormat>" +
    "  <PageWidth>8.5in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.2in</MarginTop>" +
    "  <MarginLeft>0.2in</MarginLeft>" +
    "  <MarginRight>0.5in</MarginRight>" +
    "  <MarginBottom>0.2in</MarginBottom>" +
    "</DeviceInfo>";
        Warning[] warnings;
        m_streams =new List<Stream>();
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
        ReportViewer1.LocalReport.Render("Image", deviceInfo, CreateStream,out warnings);
        foreach (Stream stream in m_streams)
            stream.Position = 0;
        
        if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
        {
            
                makePrint();
                Dispose();
                Response.Redirect("~/Bookings/NewChallan.aspx");

        }
        */
        # endregion
        

    }

    public void OpenNewWindow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }

    private void PrintPage(object sender, PrintPageEventArgs ev)
    {
        Metafile pageImage = new
           Metafile(m_streams[m_currentPageIndex]);
        ev.Graphics.DrawImage(pageImage, ev.PageBounds);
        m_currentPageIndex++;
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        pageImage.Dispose(); 

    }

    protected void makePrint()
    {

        var setPrinter = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);

        if (m_streams == null || m_streams.Count == 0)
             throw new Exception("Error: no stream to print.");
        //return;
        PrintDocument printDoc = new PrintDocument();
        printDoc.PrinterSettings.PrinterName = setPrinter;
        if (!printDoc.PrinterSettings.IsValid)
        {
            throw new Exception("Error: cannot find the default printer.");
        }
        else
        {

            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            m_currentPageIndex = 0;
            printDoc.Print();
        }

    }

    public void Dispose()
    {
        if (m_streams != null)
        {
            foreach (Stream stream in m_streams)
                stream.Close();
            m_streams = null;
        }
    }
    
}
