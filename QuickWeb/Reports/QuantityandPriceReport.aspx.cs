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
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Reporting.WebForms;

public partial class QuantityandPriceReport : System.Web.UI.Page
{
    DTO.Report Ob = new DTO.Report();
    string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
    ArrayList date = new ArrayList();
    public static StringWriter sw;
    public static string strAllContents = string.Empty;
    DTO.PackageMaster Obp = new DTO.PackageMaster();
    public static bool blnRight = false;
    //# region userControlControls

    //TextBox ucTxtRptFrom, ucTxtRptTo;
    //DropDownList ucDrpMnthLst, ucDrpYrLst;
    //RadioButton ucRadFrom, ucRadMnth;
  //  CheckBox ucChkHome;

    //# endregion

    public static string hdnIsPrintingForMany()
    {
        //return this.hdnDTOReportsBFlag.Value.ToString();
        return "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        # region setUserControl

        //ucTxtRptFrom = ((TextBox)uc1.FindControl("txtReportFrom"));
        //ucTxtRptTo = ((TextBox)uc1.FindControl("txtReportUpto"));
        //ucDrpMnthLst = ((DropDownList)uc1.FindControl("drpMonthList"));
        //ucDrpYrLst = ((DropDownList)uc1.FindControl("drpYearList"));
        //ucRadFrom = ((RadioButton)uc1.FindControl("radReportFrom"));
        //ucRadMnth = ((RadioButton)uc1.FindControl("radReportMonthly"));
      // ucChkHome = ((CheckBox)uc1.FindControl("chkShowOnlyHome"));

        # endregion
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();

            //ucTxtRptFrom.Text = date[0].ToString();
            //ucTxtRptTo.Text = date[0].ToString();
            strFromDate = date[0].ToString();
            ShowBookingDetails(strFromDate, strFromDate);
            //ucDrpMnthLst.SelectedIndex = DateTime.Today.Month - 1;
            //ucDrpYrLst.Items.Clear();
            //for (int i = 2000; i <= 2050; i++)
            //{
            //    ucDrpYrLst.Items.Add(i.ToString());
            //}
            //ucDrpYrLst.SelectedIndex = DateTime.Today.Year - 2000;            
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
            txtInvoiceNo.Focus();
        }
        DTO.Report.BFlag = false;

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "false";

        hdnDTOReportsBFlag.Value = "false";
        hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
        var btn = Request.Params["__EVENTTARGET"] as string;
        if (btn != null && btn == "ctl00$ContentPlaceHolder1$txtCustomerName")
        {
            txtCustomerName_TextChanged(null, EventArgs.Empty);
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
       
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strSqlQuery = "";
        btnRight.Visible = false;
       // if (!chkInvoice.Checked)
        if(txtInvoiceNo.Text.Trim() =="")
        {            
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();

            hdnStartDate.Value = strFromDate;
            hdnEndDate.Value = strToDate;
            hdnFromDate.Value = strFromDate;
            hdnToDate.Value = strToDate;
            ShowBookingDetails(strFromDate, strToDate);
        }
        else
        {
            ShowBookingDetails(strFromDate, strToDate);
        }      

        if (grdReport.Rows.Count > 0)
        {           
            btnExport.Visible = true;
            btnPrint.Visible = true;
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
            btnRight.Visible = true;
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
            btnRight.Visible = false;
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
        ObMain.UserID = txtUserID.Text;
        string IsHome = string.Empty;
        hdnFromDate.Value = strStartDate;
        hdnToDate.Value = strToDate;
        //if (chkInvoice.Checked)
        //    ObMain.Description = "3";
        //else if (chkCustomer.Checked)
        //    ObMain.Description = "2";
        //else       
            ObMain.Description = "1";
        DataSet dsMain = new DataSet();
        if (drpDiscount.SelectedValue == "1")
        {
           
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReport(ObMain);
        }
        if (drpDiscount.SelectedValue == "2")
        {
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReportBookingDiscount(ObMain);
        }
        if (drpDiscount.SelectedValue == "3")
        {
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReportDeliveryDiscount(ObMain);
        }
            var dt = dsMain.Tables[0].Clone();
        if (chkShowOnlyHome.Checked)
        {
            for (var i = 0; i < dsMain.Tables[0].Rows.Count; i++)
            {
                if (dsMain.Tables[0].Rows[i]["HomeDelivery"].ToString() == "Yes")
                    dt.ImportRow(dsMain.Tables[0].Rows[i]);
            }

        }
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
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
                    if (chkShowOnlyHome.Checked)
                        IsHome = "Booking Report    ( Home Delivery )";
                    else
                        IsHome = "Booking Report";
                    grdReport.DataSource = dsMain.Tables[0];
                    
                    bool rights = AppClass.GetShowFooterRightsUser();
                    string rvalue=string.Empty;
                    if (rights==true)
                        rvalue="1";
                    else
                        rvalue="0";
                    grdReport.DataBind();
                  
                    ReportViewer1.LocalReport.ReportPath = "RDLC/BookingReport.rdlc";
                    ReportDataSource rds = new ReportDataSource();
                    ReportParameter[] parameters = new ReportParameter[7];
                    parameters[0] = new ReportParameter("UserName", Globals.UserName);
                    parameters[1] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
                    parameters[2] = new ReportParameter("FDate", strStartDate);
                    parameters[3] = new ReportParameter("LDate", strToDate);
                    parameters[4] = new ReportParameter("UserTypeId", Globals.UserType);
                    parameters[5] = new ReportParameter("BookingText", IsHome.ToString());
                    
                    parameters[6] = new ReportParameter("TotalFooter", rvalue);
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
                    AppClass.CalcuateAndSetGridFooter(ref grdReport);
                    grdReport.FooterRow.Cells[5].Text = "";
                    grdReport.FooterRow.Cells[0].Text = "";
                    grdReport.FooterRow.Cells[1].Text = "Total Order";
                    if (grdReport.Rows.Count > 0)
                    {                       
                        grdReport.Visible = true;                      
                        btnRight.Visible = true;
                        grdReport.FooterRow.Cells[2].Text = grdReport.Rows.Count.ToString();
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
                        btnRight.Visible = false;
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
    

    protected void grdReportDataBound(object sender, EventArgs e)
    {
        if (grdReport == null) return;

        if (grdReport.FooterRow == null) return;

        if (Globals.UserType == "1")
            grdReport.FooterRow.Visible = true;
        else
            grdReport.FooterRow.Visible = false;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {       
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        //GridView grd = new GridView();

        //grd.DataSource = dsMain.Tables[1];
        //grd.DataBind();
        DTO.Report.BFlag = false;

        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
        else
            HttpContext.Current.Items["IsPrintingForMany"] = "false";

        hdnDTOReportsBFlag.Value = "false";
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport,hdnFromDate.Value,hdnToDate.Value,"Booking Report", false);
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
        if (hdnTempInvoice.Value.Trim() == "1")
        {
            rowindex = 1;
            hdnTempInvoice.Value = "0";
        }
        else
        {
            rowindex = 0;
        }
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

        var querystring = "Print";
        string urlToOpen = "&DirectPrint=true&RedirectBack=true&closeWindow=true";
        BasePage.OpenWindow(this.Page, "../Reports/ListBooking.aspx?Bookings=" + querystring + urlToOpen);
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
            txtInvoiceNo.Text = "";
            txtUserID.Text = "";
            customerName = txtCustomerName.Text.Split('-');
            lblCustomerCode.Text = customerName[0].ToString().Trim();
            txtCustomerName.Text = customerName[1].ToString().Trim();
            Obp.BranchId = Globals.BranchID;
            Obp.CustomerCode = lblCustomerCode.Text;
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalCustomer(Obp) != true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Please enter valid customer.";
                txtCustomerName.Text = "";              
                grdReport.DataSource = null;
                grdReport.DataBind();
            }
            btnShowReport_Click(null, null);
            txtCustomerName.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Please enter valid customer.";
            txtCustomerName.Text = "";
            txtCustomerName.Focus();
            grdReport.DataSource = null;
            grdReport.DataBind();
        }
    }

    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        txtCustomerName.Text="";
        lblCustomerCode.Text = "";
        txtUserID.Text = "";
        btnShowReport_Click(null, null);
        txtInvoiceNo.Focus();
    }

    protected void txtUserID_TextChanged(object sender, EventArgs e)
    {
        txtCustomerName.Text = "";
        lblCustomerCode.Text = "";
        txtInvoiceNo.Text = "";
        Obp.BranchId = Globals.BranchID;
        Obp.CustomerCode = txtUserID.Text;
        if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalUser(Obp) != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Please select a valid User ID";
            txtUserID.Text = "";
            grdReport.DataSource = null;
            grdReport.DataBind();
        }
        btnShowReport_Click(null, null);
        txtUserID.Focus();
    }

    protected void btnPrintStore_Click(object sender, EventArgs e)
    {
        storeprint("Main");
    }

    protected void BtnPrintSummaryClick(object sender, EventArgs e)
    {
        var bkScrns = string.Empty;
        for (var i =0;i<=grdReport.Rows.Count -1; i++) {
            bkScrns += ((HyperLink)grdReport.Rows[i].FindControl("hypBtnShowDetails")).Text + ",";
        }
        if (bkScrns.Length > 1)
            bkScrns = bkScrns.Substring(0, bkScrns.Length -0);
        string urlToOpen = "&DirectPrint=true&RedirectBack=true&closeWindow=true";
        BasePage.OpenWindow(this.Page, "../Reports/InvoicewithItemDetail.aspx?Bookings=" + bkScrns + urlToOpen);
    }

    public void storeprint(string ButtonType)
    {
        int rowindex = 1;      
        if (hdnTempInvoice.Value.Trim() == "1")
        {
            rowindex = 1;
            hdnTempInvoice.Value = "0";
        }
        else
        {
            rowindex = 0;
        }
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
        var querystring="";
        if (ButtonType == "Main")
            querystring = "Print";
        else
            querystring = "Preview";
        string urlToOpen = "&DirectPrint=true&RedirectBack=true&closeWindow=true";
        BasePage.OpenWindow(this.Page, "../Reports/frmStoreCopyPrint.aspx?Jumbo=" + querystring + urlToOpen); 
        btnShowReport_Click(null, null);
        DTO.Report.BFlag = false;
        hdnDTOReportsBFlag.Value = "false";
        for (int i = 0; i < Ob.StrArray.Count(); i++)
        {
            this.Form.Target = "_blank";
        }
    }

    protected void btnPrintOrderPreview_Click(object sender, EventArgs e)
    {
        int rowindex = 0;      
        if (hdnTempInvoice.Value.Trim() == "1")
        {
            rowindex = 1;
            hdnTempInvoice.Value="0";
        }
        else
        {
            rowindex = 0;
        }

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
        var querystring = "Preview";
        string urlToOpen = "&DirectPrint=true&RedirectBack=true&closeWindow=true";
        BasePage.OpenWindow(this.Page, "../Reports/ListBooking.aspx?Bookings=" + querystring + urlToOpen);
        btnShowReport_Click(null, null);
        DTO.Report.BFlag = false;
        hdnDTOReportsBFlag.Value = "false";
        for (int i = 0; i < Ob.StrArray.Count(); i++)
        {
            this.Form.Target = "_blank";
        }
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
    
}
