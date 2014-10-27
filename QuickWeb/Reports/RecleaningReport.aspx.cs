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

public partial class RecleaningReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    bool status;
    public int total = 0, pending = 0;
    string result = string.Empty;  
    private static LinkButton lastButton = null;
    DTO.Report Obj = new DTO.Report();
    string nstatus = string.Empty;
    public static bool blnRight = false;
    public static string strRptFromDate = string.Empty;
    # region userControlControls 
    static DataSet _dataSetReportSource;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {       

        if (!IsPostBack)
        {
            _dataSetReportSource = null;
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();

            grdProcessSelection.DataSource = BAL.BALFactory.Instance.Bal_Processmaster.GetAllProcess(Globals.BranchID);
            grdProcessSelection.DataBind();
            BindDropDown();        
              strRptFromDate = date[0].ToString(); 
            for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
            {
                ((CheckBox)grdProcessSelection.Rows[r].Cells[0].FindControl("chkSelect")).Checked = true;
            }
            lastButton = null;
            BtnShowClick(btnShowAll, null);
        }
        lblCustName.Text = string.Empty;
    }

    private void BindDropDown()
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 28);
        ds = AppClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            PrjClass.SetItemInDropDown(drpBookingPreFix, ds.Tables[0].Rows[0]["BookingPreFix"].ToString(), false, false);
        }
    }

    protected void BtnShowClick(object sender, EventArgs e)
    {      
        var button = sender as LinkButton;
        if (button == null) return;

        var dates = SetDuration();
        switch (button.ID)
        {
            case "btnShowAll": SetDto(btnShowAll, dates);
                break;
            case "btnShowNotReady": SetDto(btnShowNotReady, dates);
                break;
            case "btnShowNotDelivered": SetDto(btnShowNotDelivered, dates);
                break;
        }
        lastButton = button;
        ShowReport();
    }
  
    private Tuple<string, string> SetDuration()
    {
       
        string startDate = string.Empty, endDate = string.Empty;      

        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        startDate = DateFromAndTo[0].Trim();
        endDate = DateFromAndTo[1].Trim();


        return Tuple.Create<string, string>(startDate, endDate);
    }

    private void SetDto(object sender, Tuple<string, string> startEndDates, string custCode = "")
    {      
        var button = sender as LinkButton;
        string BookingPrefix = string.Empty;
        if (button == null) return;

        Obj.BranchId = Globals.BranchID;
        Obj.FromDate = startEndDates.Item1;
        Obj.UptoDate = startEndDates.Item2;
        if (txtStartBkNo.Text != "" && txtEndBkNo.Text != "")
        {
            BookingPrefix = drpBookingPreFix.SelectedItem.Text.Trim();
        }
        Obj.StartBkNum = BookingPrefix.Trim() + txtStartBkNo.Text;
        Obj.EndBkNum = BookingPrefix.Trim() + txtEndBkNo.Text;
        // the show only home delivery item
        Obj.CustCodeStr = chkShowOnlyHome.Checked ? "1" : "0, 1";
        
        // the process codes
        Obj.StrCodes = string.Empty;
        foreach (GridViewRow row in grdProcessSelection.Rows)
        {
            if (((CheckBox)row.FindControl("chkSelect")).Checked)
                Obj.StrCodes += ((Label)row.FindControl("lblProcessCode")).Text + ",";
        }
        Obj.StrCodes = Obj.StrCodes.Substring(0, Obj.StrCodes.Length - 1);
        Obj.CustId = custCode;

        switch (button.ID)
        {
            case "btnShowAll":
                {
                    Obj.InvoiceNo = "1";                 
                    result = "All Orders";

                }
                break;
            case "btnShowNotReady":
                {
                    Obj.InvoiceNo = "2";
                    result = "Not Ready Garments";
                }
                break;
            case "btnShowNotDelivered":
                {
                    Obj.InvoiceNo = "3";
                    result = "Not Delivered Garments";
                }
                break;
        }
        // Obj.CustId = custCode;
        if (!string.IsNullOrEmpty(hdnCustCode.Value)) 
            Obj.CustId = hdnCustCode.Value;

        hdnCustCode.Value = string.Empty;
    }

    private void ShowReport(DataSet datasetReportSource = null)
    {
        DataSet dataset, dsMain;

        if (datasetReportSource == null)
        {

            dataset = BAL.BALFactory.Instance.Bal_Report.GetGarmentReadyReport(Obj);
            dsMain = dataset;
            _dataSetReportSource = dataset;
        }
        else
        {

            dataset = datasetReportSource;
            dsMain = datasetReportSource;
        }

        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
        bool rights = AppClass.GetShowFooterRightsUser();
        string rvalue = string.Empty;
        if (rights == true)
            rvalue = "1";
        else
            rvalue = "0";
        ReportViewer1.LocalReport.ReportPath = "RDLC/TodayDeliveryReport.rdlc";       
        ReportParameter[] parameters = new ReportParameter[12];
        parameters[0] = new ReportParameter("MainHead", result);
        parameters[1] = new ReportParameter("UserName", Globals.UserName );
        parameters[2] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
        parameters[3] = new ReportParameter("FromDate", Obj.FromDate);
        parameters[4] = new ReportParameter("ToDate", Obj.UptoDate);
        parameters[5] = new ReportParameter("StartBK", Obj.StartBkNum);
        parameters[6] = new ReportParameter("EndBK", Obj.EndBkNum);
        parameters[7] = new ReportParameter("ProcessCode", Obj.StrCodes);
        parameters[8] = new ReportParameter("UserTypeId", Globals.UserType);
        parameters[9] = new ReportParameter("TotalFooter", rvalue);
        if (Obj.CustCodeStr=="1")
        parameters[11]=new ReportParameter("Home","- Home Delivery");
        else
            parameters[11] = new ReportParameter("Home", "");
       if(lblCustName.Text!="")
            parameters[10] = new ReportParameter("CustomerName", lblCustName.Text);
        else
            parameters[10] = new ReportParameter("CustomerName", "");
               
        ReportViewer1.LocalReport.SetParameters(parameters);
        ReportDataSource rds = new ReportDataSource();
        rds.Name = "DataSet1";
        rds.Value = dsMain.Tables[0];
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            ReportViewer1.Visible = true;
            blnRight = AppClass.CheckExportToExcelRightOnPage();
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
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "No pending delivery entry found.";
            btnPrint.Visible = false;
            ReportViewer1.Visible = false;
        }
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(rds);
        ReportViewer1.LocalReport.Refresh();       
    }


    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strSqlQuery = string.Empty;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;      
      
        strFromDate = strRptFromDate;
        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        DataSet dsMain = new DataSet();
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        dsMain = GetData(Ob, "btnShowReport");
        FillExistingRecord("btnShowReport");
        ReportViewer1.LocalReport.ReportPath = "RDLC/TodayDeliveryReport.rdlc";
        ReportParameter[] parameters = new ReportParameter[2];
        parameters[0] = new ReportParameter("Details", lblStoreName.Text);
        parameters[1] = new ReportParameter("Quantity", lblQuantity.Text);
        ReportViewer1.LocalReport.SetParameters(parameters);
        ReportDataSource rds = new ReportDataSource();
        rds.Name = "DeliveryDataSet_sp_delivery";
        rds.Value = dsMain.Tables[0];
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            ReportViewer1.Visible = true;
            btnPrint.Visible = true;
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                ReportViewer1.ShowExportControls = true;
            }
            else
            {
                ReportViewer1.ShowExportControls = false;
            }   
        }
        else
        {

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "No pending delivery entry found.";
            btnPrint.Visible = false;
            ReportViewer1.Visible = false;
        }
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(rds);
        ReportViewer1.LocalReport.Refresh();
        fusionGauge();      
    }
    public DTO.Report SetValue()
    {
        DTO.Report Ob = new DTO.Report();
      
        Ob.FromDate = strRptFromDate;
        Ob.BranchId = Globals.BranchID;
       
        return Ob;
    }
    public DataSet GetData(DTO.Report Ob, string Status)
    {
        DataSet ds = new DataSet();
        # region previousCode
        /*
        if (Status == "btnShowReport")
        {
            if (drpOption.SelectedItem.Text == "Summary")
                ds = BAL.BALFactory.Instance.Bal_Report.GetTodayDeliverysummary(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetTodayDeliverydetailed(Ob);
        }
        else
        {
            if (drpOption.SelectedItem.Text == "Summary")

                ds = BAL.BALFactory.Instance.Bal_Report.GetTodayDeliveryUpdatesummary(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetTodayDeliveryUpdatedetailed(Ob);
        }
         */
        # endregion
        return ds;
    }
    public void FillExistingRecord(string Checked)
    {
        lblStoreName.Text = " ";
        lblQuantity.Text = "";
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        if (Checked == "btnShowReport")
        {
            cmd.CommandText = "proc_Delivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@BookingDate1", strRptFromDate);           
        }
        else
        {
            cmd.CommandText = "proc_DeliveryUpdate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@BookingDate1", strRptFromDate);           
        }
        ds = AppClass.GetData(cmd);
        if (ds.Tables[1].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                lblStoreName.Text += ds.Tables[1].Rows[i]["ItemName"].ToString();
            lblQuantity.Text += ds.Tables[2].Rows[0]["Quantity"].ToString();
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
    "  <PageWidth>9in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.2in</MarginTop>" +
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

        var fileName = "OutPut.pdf";
        PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        ViewState["exlquery"] = null;
        string strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
       
        strFromDate = strRptFromDate;
        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        DataSet dsMain = new DataSet();
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        dsMain = GetData(Ob, "btnUpdate");
        FillExistingRecord("btnUpdate");
        ReportViewer1.LocalReport.ReportPath = "RDLC/TodayDeliveryReport.rdlc";
        ReportParameter[] parameters = new ReportParameter[2];
        parameters[0] = new ReportParameter("Details", lblStoreName.Text);
        parameters[1] = new ReportParameter("Quantity", lblQuantity.Text);
        ReportViewer1.LocalReport.SetParameters(parameters);
        ReportDataSource rds = new ReportDataSource();
        rds.Name = "DeliveryDataSet_sp_delivery";
        rds.Value = dsMain.Tables[0];
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            ReportViewer1.Visible = true;
            btnPrint.Visible = true;
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                ReportViewer1.ShowExportControls = true;
            }
            else
            {
                ReportViewer1.ShowExportControls = false;
            }   
        }
        else
        {

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "No pending delivery entry found.";
            btnPrint.Visible = false;
            ReportViewer1.Visible = false;
        }
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(rds);
        ReportViewer1.LocalReport.Refresh();
        fusionGauge();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "googlechart();", true);
        
    }

    public void fusionGauge()
    {

        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        DataSet dsMain = new DataSet();
        DataSet dsMain1 = new DataSet();
        dsMain = GetData(Ob, "btnShowReport");
        dsMain1 = GetData(Ob, "btnUpdate");
        for (int i = 0; i < dsMain.Tables["table"].Rows.Count; i++)
        {
            total = total + Convert.ToInt32(dsMain.Tables["table"].Rows[i]["Qty"].ToString());
        }
        for (int i = 0; i < dsMain1.Tables["table"].Rows.Count; i++)
        {
            pending = pending + Convert.ToInt32(dsMain1.Tables["table"].Rows[i]["Qty"].ToString());
        }

    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var customerName = txtCustomerName.Text.Split('-');

            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalCustomer(new DTO.PackageMaster { CustomerCode = customerName[0], BranchId = Globals.BranchID }) != true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);          
                lblMsg.Text = "Kindly enter valid customer.";
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
            }
            else
            {
                hdnCustCode.Value = customerName[0];              
                lblCustName.Text = customerName[1].ToString().Trim();
                txtCustomerName.Text = "";
                BtnShowClick(lastButton, EventArgs.Empty);                
                nstatus = "1";
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);          
            lblMsg.Text = "Kindly enter valid customer.";
            txtCustomerName.Text = "";
            txtCustomerName.Focus();
        }
    }

}
