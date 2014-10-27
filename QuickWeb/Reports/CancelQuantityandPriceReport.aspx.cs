using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
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

public partial class CancelQuantityandPriceReport : System.Web.UI.Page
{
    DTO.Report Ob = new DTO.Report();
    string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
    ArrayList date = new ArrayList();
    public static StringWriter sw;
    public static string strAllContents = string.Empty;
    DTO.PackageMaster Obp = new DTO.PackageMaster();
    public static bool blnRight = false;
   
    public static string hdnIsPrintingForMany()
    {
        //return this.hdnDTOReportsBFlag.Value.ToString();
        return "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {           
           
            hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            btnShowReport_Click(null, null);
        }
        DTO.Report.BFlag = false;

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "false";

        hdnDTOReportsBFlag.Value = "false";
        hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
        blnRight = AppClass.CheckExportToExcelRightOnPage();
        if (blnRight)
        {
            btnExport.Visible = true;
        }
        else
        {
            btnExport.Visible = false;
        }
    }

    protected void SetDTOFalse()
    {
        //DTO.Report.BFlag = false;
    }

    protected void btnPrintReport_Click(object sender, EventArgs e)
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
        var fileName = "OutPut.pdf";
        PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);
        //Response.Clear();
        //Response.ContentType = mimeType;
        //Response.BinaryWrite(renderedBytes);
        //Response.End();
    
    
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strSqlQuery = "";      
        if (txtInvoiceNo.Text.Trim() =="")
        {
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();

            hdnStartDate.Value = strFromDate;
            hdnEndDate.Value = strToDate;
            ShowBookingDetails(strFromDate, strToDate);
        }
        else
        {
            ShowBookingDetails(strFromDate, strToDate);
        }
        checkCancelBooking();

        if (grdReport.Rows.Count > 0)
        {
            AppClass.CalcuateAndSetGridFooter(ref grdReport);
            btnExport.Visible = true;           
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
        }
        else
        {
            btnExport.Visible = false;
        }
    }
    private void ShowBookingDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        DTO.Report.BFlag = false;

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "false";

        hdnDTOReportsBFlag.Value = "false";

        DTO.Report ObMain = new DTO.Report();
        ObMain.FromDate = strStartDate;
        ObMain.UptoDate = strToDate;
        ObMain.InvoiceNo = txtInvoiceNo.Text;
        ObMain.BranchId = Globals.BranchID;
        ObMain.CustId = lblCustomerCode.Text;      
        if (txtInvoiceNo.Text.Trim() != "")
            ObMain.Description = "3";       
        else if (txtCustomerName.Text.Trim() !="")
            ObMain.Description = "2";
        else
            ObMain.Description = "1";       
        DataSet dsMain = new DataSet();
        dsMain = BAL.BALFactory.Instance.Bal_Report.GetCancelDataMainReport(ObMain);
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);

        var dt = dsMain.Tables[0].Clone();
        if (chkShowOnlyHome.Checked)
        {
            for (var i = 0; i < dsMain.Tables[0].Rows.Count; i++)
            {
                if (dsMain.Tables[0].Rows[i]["HomeDelivery"].ToString() == "Yes")
                    dt.ImportRow(dsMain.Tables[0].Rows[i]);
            }

        }
        try
        {
            if (chkShowOnlyHome.Checked /*dt.Rows.Count >= 1*/ )
            {
                var tempTable = dsMain.Tables[1].Copy();
                dsMain.Tables.RemoveAt(0);
                dsMain.Tables.RemoveAt(0);
                dsMain.Tables.Add(dt);
                dsMain.Tables.Add(tempTable);
            }
           
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
                    ReportViewer1.LocalReport.ReportPath = "RDLC/BookingReport.rdlc";
                    ReportDataSource rds = new ReportDataSource();
                    ReportParameter[] parameters = new ReportParameter[4];
                    parameters[0] = new ReportParameter("UserName", Globals.UserName);
                    parameters[1] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
                    parameters[2] = new ReportParameter("FDate", strStartDate);
                    parameters[3] = new ReportParameter("LDate", strToDate);
                    //parameters[2] = new ReportParameter("Link", str);
                    ReportViewer1.LocalReport.SetParameters(parameters);
                    rds.Name = "DataSet1";
                    rds.Value = dsMain.Tables[0];
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                    DTO.Report.BFlag = false;

                    if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                        HttpContext.Current.Items.Add("IsPrintingForMany", "false");
                    else
                        HttpContext.Current.Items["IsPrintingForMany"] = "false";

                    hdnDTOReportsBFlag.Value = "false";
                    gvUserInfo.DataSource = dsMain.Tables[1];
                    gvUserInfo.DataBind();
                    AppClass.CalcuateAndSetGridFooter(ref grdReport);
                    CalculatePrintGridReport();
                    var UserType = Globals.UserType;
                    if (UserType == "1")
                    {
                        grdReport.FooterRow.Visible = true;
                        gvUserInfo.FooterRow.Visible = true;                       
                    }
                    else
                    {
                        grdReport.FooterRow.Visible = false;
                        gvUserInfo.FooterRow.Visible = false;
                    }
                }
            }
        }
        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
           
        }
    }
    private void CalculatePrintGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float St = 0, Ad = 0, Bal = 0, TotalPaid = 0, TotalDue = 0;
        try
        {
            for (int r = 0; r < rc; r++)
            {
                TotalPaid += float.Parse("0" + gvUserInfo.Rows[r].Cells[4].Text);
                TotalDue += float.Parse("0" + gvUserInfo.Rows[r].Cells[5].Text);
                St += float.Parse("0" + gvUserInfo.Rows[r].Cells[6].Text);
                Ad += float.Parse("0" + gvUserInfo.Rows[r].Cells[7].Text);
                Bal += float.Parse("0" + gvUserInfo.Rows[r].Cells[8].Text);
             }
            gvUserInfo.FooterRow.Cells[2].Text = "Total";
            gvUserInfo.FooterRow.Cells[4].Text = TotalPaid.ToString();
            gvUserInfo.FooterRow.Cells[5].Text = TotalDue.ToString();
            gvUserInfo.FooterRow.Cells[6].Text = St.ToString();
            gvUserInfo.FooterRow.Cells[7].Text = Ad.ToString();
            gvUserInfo.FooterRow.Cells[8].Text = Bal.ToString();
        }
        catch (Exception ex)
        { }
    }
   
    private void checkCancelBooking()
    {
       
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {       
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;       
        DTO.Report.BFlag = false;

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "false";

        hdnDTOReportsBFlag.Value = "false";
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Cancel booking Report", false);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }   

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int rowindex = 1;       
        if (txtCustomerName.Text.Trim() !="")
            rowindex = 1;
        else
            rowindex = 0;
        if (hdnSelectedList.Value == "")
            return;
        BookingSlip bs = new BookingSlip();
        Ob.StrArray = hdnSelectedList.Value.Split(',');
        DTO.Report.BFlag = true;
        hdnDTOReportsBFlag.Value = "ture";

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "true");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "true";


        for (int i = 0; i < Ob.StrArray.Count(); i++)
        {
            this.Form.Target = "_blank";           
        }
        sw = new StringWriter();
        strAllContents = "";
        string temp = string.Empty;
        var isLaser = BAL.BALFactory.Instance.Bal_Report.IsPrinterLaser(Globals.BranchID);
        for (int i = rowindex; i < Ob.StrArray.Count(); i++)
        {
            sw.Flush();
            this.Form.Target = "_blank";          
            BookingSlip bsp = new BookingSlip();
            Thermal_BookingSlip tbs = new Thermal_BookingSlip();
            if (!isLaser)
                temp += tbs.GetBookingDetails(Ob.StrArray[i].Split(':')[1]).Item1;
            else
                temp += bsp.GetBookingDetailsForBookingNumber(Ob.StrArray[i].Split(':')[1], Ob.StrArray[i].Split(':')[0]);           

            if (HttpContext.Current.Items.Contains("CheckStoreCopy") && HttpContext.Current.Items["CheckStoreCopy"] == "true")
            {
                string Preview = "";
                Preview = "<table style='width:7.6in;height:5.12in'><tr><td></td><tr></table>";
                Preview += bsp.strPreview1;
                Response.Write(Preview);
                Preview += " </td>";
                Preview += "</tr>";
                Preview += "</table>";

                temp += Preview;
            }
            else
            {
               
            }           
        }
        strAllContents = temp;
       
        BasePage.OpenWindow(this.Page, "../Reports/ListBooking.aspx");
        btnShowReport_Click(null, null);
        DTO.Report.BFlag = false;
        hdnDTOReportsBFlag.Value = "false";
        for (int i = 0; i < Ob.StrArray.Count(); i++)
        {
            this.Form.Target = "_blank";            
        }
    }

    string[] customerName;
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            customerName = txtCustomerName.Text.Split('-');
            lblCustomerCode.Text = customerName[0].ToString().Trim();
            txtCustomerName.Text = customerName[1].ToString().Trim();
            Obp.BranchId = Globals.BranchID;
            Obp.CustomerCode = lblCustomerCode.Text;
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalCustomer(Obp) != true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Kindly enter valid customer.";
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
                return;
            }
            btnShowReport_Click(null, null);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Kindly enter valid customer.";
            txtCustomerName.Text = "";
            txtCustomerName.Focus();
        }
    }

    protected void btnPrintStore_Click(object sender, EventArgs e)
    {
        storeprint();
    }
    string strPrinterName = string.Empty;
    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drview = e.Row.DataItem as DataRowView;
        if (strPrinterName == "")
        {
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
            string strBookinNo = hypBookingNo.Text;
            Label lblDueDate = (Label)e.Row.FindControl("lblDate");
            string strDuedate = lblDueDate.Text;
            hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }
    public void storeprint()
    {
        int rowindex = 1;       
        if (txtCustomerName.Text.Trim() != "")
            rowindex = 1;
        else
            rowindex = 0;
        if (hdnSelectedList.Value == "")
            return;
        BookingSlip bs = new BookingSlip();
        Ob.StrArray = hdnSelectedList.Value.Split(',');
        DTO.Report.BFlag = true;
        hdnDTOReportsBFlag.Value = "ture";

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "true");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "true";


        for (int i = 0; i < Ob.StrArray.Count(); i++)
        {
            this.Form.Target = "_blank";
        }
        sw = new StringWriter();
        strAllContents = "";
        string temp = string.Empty;
        var isLaser = BAL.BALFactory.Instance.Bal_Report.IsPrinterLaser(Globals.BranchID);
        for (int i = rowindex; i < Ob.StrArray.Count(); i++)
        {
            sw.Flush();
            this.Form.Target = "_blank";
            BookingSlip bsp = new BookingSlip();

           Globals.StorePrint = "ST_COPY";

           Thermal_BookingSlip tbs = new Thermal_BookingSlip();
           if (!isLaser)
               temp += tbs.GetBookingDetails(Ob.StrArray[i].Split(':')[1]).Item2;
           else
                temp += bsp.GetBookingDetailsForBookingNumber(Ob.StrArray[i].Split(':')[1], Ob.StrArray[i].Split(':')[0]);
            
            Globals.StorePrint = " ";
            if (HttpContext.Current.Items.Contains("CheckStoreCopy") && HttpContext.Current.Items["CheckStoreCopy"] == "true")
            {
                string Preview = "";
                Preview = "<table style='width:7.6in;height:5.12in'><tr><td></td><tr></table>";
                Preview += bsp.strPreview1;
                Response.Write(Preview);
                Preview += " </td>";
                Preview += "</tr>";
                Preview += "</table>";

                temp += Preview;
            }
            else
            {

            }
        }
        strAllContents = temp;

        BasePage.OpenWindow(this.Page, "../Reports/frmStoreCopyPrint.aspx");
        btnShowReport_Click(null, null);
        DTO.Report.BFlag = false;
        hdnDTOReportsBFlag.Value = "false";
        for (int i = 0; i < Ob.StrArray.Count(); i++)
        {
            this.Form.Target = "_blank";
        }
    }
}
