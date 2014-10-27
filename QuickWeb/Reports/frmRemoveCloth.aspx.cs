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
using System.Text;
using System.IO;
using System.Data.SqlClient;

public partial class Reports_frmRemoveCloth : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    DTO.Common Ob = new DTO.Common();
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            hdnDate.Value = date[0].ToString().Trim();         
            Binddropdown();
            btnShowReport_Click(null,null);
        }
    }   
    protected void txtCName_TextChanged(object sender, EventArgs e)
    {
        string[] CustName = txtCName.Text.Split('-');
        hdnCustId.Value = CustName[0].ToString();
        setCustvalue(CustName[0].ToString());
        btnShowReport_Click(null, null);
    }
    public void setCustvalue(string customerName)
    {
        DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
        if (DS_CustInfo.Tables[0].Rows.Count > 0)
            txtCName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        btnExport.Visible = false;
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        Ob.FromDate = DateFromAndTo[0].Trim();
        Ob.UptoDate = DateFromAndTo[1].Trim();
        hdnStartDate.Value = Ob.FromDate;
        hdnEndDate.Value = Ob.UptoDate;


        //grdReport.DataSource = ViewState["Report"]= BAL.BALFactory.Instance.Bal_Report.GetReasonToRemove(Ob, (rdrReportFrom.Checked ? "1" : (rdrReportMonthly.Checked ? "2" : (chkInvoice.Checked ? "3" : ""))), (drpSelectOption.SelectedIndex == 0 ? "1" : (drpSelectOption.SelectedIndex == 1 ? "2" : (drpSelectOption.SelectedIndex == 2 ? "3" : ""))));
        grdReport.DataSource = ViewState["Report"] = BAL.BALFactory.Instance.Bal_Report.GetReasonToRemove(Ob, (txtInvoiceNo.Text.Trim() == "" ? "1" : (txtInvoiceNo.Text.Trim() !="" ? "3" : "")), (drpSelectOption.SelectedIndex == 0 ? "1" : (drpSelectOption.SelectedIndex == 1 ? "2" : (drpSelectOption.SelectedIndex == 2 ? "3" : ""))));
        
        grdReport.DataBind();
       // grdReport1.DataSource = BAL.BALFactory.Instance.Bal_Report.GetReasonToRemove(Ob, (rdrReportFrom.Checked ? "1" : (rdrReportMonthly.Checked ? "2" : (chkInvoice.Checked ? "3" : ""))), (drpSelectOption.SelectedIndex == 0 ? "1" : (drpSelectOption.SelectedIndex == 1 ? "2" : (drpSelectOption.SelectedIndex == 2 ? "3" : ""))));
        grdReport1.DataSource = BAL.BALFactory.Instance.Bal_Report.GetReasonToRemove(Ob, (txtInvoiceNo.Text.Trim() == "" ? "1" : (txtInvoiceNo.Text.Trim() != "" ? "3" : "")), (drpSelectOption.SelectedIndex == 0 ? "1" : (drpSelectOption.SelectedIndex == 1 ? "2" : (drpSelectOption.SelectedIndex == 2 ? "3" : ""))));
        grdReport1.DataBind();
        if (grdReport.Rows.Count > 0)
        {
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
    }
   
    public void  Binddropdown()        
    {
        Ob.BID=Globals.BranchID;
        drpSelectOption.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindRemoveDrop(Ob);
        drpSelectOption.DataTextField = "PageTitle";
        drpSelectOption.DataValueField = "Id";
        drpSelectOption.DataBind();
        drpSelectOption.Items.Insert(2, new ListItem(drpSelectOption.Items[0].Text + " & " + drpSelectOption.Items[1].Text, "3"));
    }
    public DTO.Report SetValue()
    {
        DTO.Report Ob = new DTO.Report();
        
        var DateFromAndTo1 = hdnDateFromAndTo.Value.Split('-');
        Ob.FromDate = DateFromAndTo1[0].Trim();
        Ob.UptoDate = DateFromAndTo1[1].Trim();
        hdnStartDate.Value = Ob.FromDate;
        hdnEndDate.Value = Ob.UptoDate;

        Ob.InvoiceNo = txtInvoiceNo.Text;
        Ob.CustId = hdnCustId.Value.Trim();
        Ob.Date = hdnDate.Value;
        Ob.BranchId = Globals.BranchID;
        return Ob;
    }
    protected void grdReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();      

        var DateFromAndTo2 = hdnDateFromAndTo.Value.Split('-');
        Ob.FromDate = DateFromAndTo2[0].Trim();
        Ob.UptoDate = DateFromAndTo2[1].Trim();
        hdnStartDate.Value = Ob.FromDate;
        hdnEndDate.Value = Ob.UptoDate;

        DataSet DS = new DataSet();     
        //DS = BAL.BALFactory.Instance.Bal_Report.GetReasonToRemove(Ob, (rdrReportFrom.Checked ? "1" : (rdrReportMonthly.Checked ? "2" : (chkInvoice.Checked ? "3" : ""))), (drpSelectOption.SelectedIndex == 0 ? "1" : (drpSelectOption.SelectedIndex == 1 ? "2" : (drpSelectOption.SelectedIndex == 2 ? "3" : ""))));
        DS = BAL.BALFactory.Instance.Bal_Report.GetReasonToRemove(Ob, (txtInvoiceNo.Text.Trim() == "" ? "1" : (txtInvoiceNo.Text.Trim() != "" ? "3" : "")), (drpSelectOption.SelectedIndex == 0 ? "1" : (drpSelectOption.SelectedIndex == 1 ? "2" : (drpSelectOption.SelectedIndex == 2 ? "3" : ""))));
        DataTable DT = DS.Tables[0];
        if (DT != null)
        {
            DataView DV = new DataView(DT);
            if ((string)System.Web.HttpContext.Current.Session["Direction"] == "Asc")
            {
                DV.Sort = e.SortExpression + " " + "ASC";
                System.Web.HttpContext.Current.Session["Direction"] = "Desc";               
            }
            else if ((string)System.Web.HttpContext.Current.Session["Direction"] == "Desc")
            {                
                DV.Sort = e.SortExpression + " " + "DESC";               
                System.Web.HttpContext.Current.Session["Direction"] = "Asc";              
            }
            else
            {
                DV.Sort = e.SortExpression + " " + "ASC";
                System.Web.HttpContext.Current.Session["Direction"] = "Desc";              
            }          
            grdReport.DataSource = DV;
            grdReport.DataBind();
            grdReport1.DataSource = DV;
            grdReport1.DataBind();
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {        
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Removed Garments Report", false);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
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
            HiddenField lblDueDate = (HiddenField)e.Row.FindControl("hdnDeliveryDate");
            string strDuedate = lblDueDate.Value;
            hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }
}