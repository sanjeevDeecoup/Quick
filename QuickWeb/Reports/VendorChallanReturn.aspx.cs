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

public partial class Report_VendorChallanReturn : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
         BindddBooking();
        ddlinvoice.Focus();
        ddlinvoice.Attributes.Add("onfocus", "javascript:select();");
        }       
    }  
    public void BindddBooking()
    {
    ddlinvoice.Items.Clear();
    SqlCommand cmd = new SqlCommand();
    cmd.CommandText = "sp_Dry_BindPrintBarcodeDropDown";
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);       
    DataSet ds = new DataSet();
    ds = AppClass.GetData(cmd);
    ddlinvoice.DataSource = ds.Tables["table"];
    ddlinvoice.DataTextField = "BookingNumber";
    ddlinvoice.DataValueField = "BookingNumber";
    ddlinvoice.DataBind();
    BindddRowindex();
    }
    public void BindddRowindex()
    {
        ddlno.Items.Clear();
        ddlno.Items.Insert(0, new ListItem("All", "0"));
        ddlno.AppendDataBoundItems = true;
        SqlCommand cmdnew = new SqlCommand();
        cmdnew.CommandText = "sp_rpt_barcodprint";
        cmdnew.CommandType = CommandType.StoredProcedure;
        cmdnew.Parameters.AddWithValue("@BookingNo", ddlinvoice.SelectedValue);
        cmdnew.Parameters.AddWithValue("@RowIndex", ddlno.SelectedValue);
        cmdnew.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmdnew.Parameters.AddWithValue("@Flag", 2);
        DataSet dsnew = new DataSet();
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataAdapter da = new SqlDataAdapter();
        cmdnew.Connection = sqlcon;
        da.SelectCommand = cmdnew;
        da.Fill(dsnew);
        ddlno.DataSource = dsnew.Tables["table"];
        ddlno.DataTextField = "RowIndex";
        ddlno.DataValueField = "RowIndex";
        ddlno.DataBind();
        ddlno.AppendDataBoundItems = false;
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_rpt_barcodprint";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookingNo", ddlinvoice.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 4);
        ds = AppClass.GetData(cmd);
        ReportViewer1.LocalReport.ReportPath = "RDLC/DynamicBarCodeReport.rdlc";
        ReportDataSource rds = new ReportDataSource();
        rds.Name = "DataSet1";
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
    "  <PageWidth>210mm</PageWidth>" +
    "  <PageHeight>297mm</PageHeight>" +
    " <Columns>3</Columns>" +
    " <ColumnSpacing>2.5mm</ColumnSpacing>"+
    "  <MarginTop>12.50mm</MarginTop>" +
    "  <MarginLeft>6.50mm</MarginLeft>" +
    "  <MarginRight>0in</MarginRight>" +
    "  <MarginBottom>0in</MarginBottom>" +
    
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

    protected void ddlinvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddRowindex();
    }

        
}

