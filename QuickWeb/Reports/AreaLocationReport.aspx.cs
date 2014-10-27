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

public partial class AreaLocationReport : System.Web.UI.Page
{    
    public static bool blnRight = false;
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            txtAreaLocation.Text = "";
            txtAreaLocation.Focus();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            btnShowReport_Click(null, null);
        }
    }
    protected void txtAreaLocation_TextChanged(object sender, EventArgs e)
    {
        btnShowReport_Click(null, null);
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        //if (txtAreaLocation.Text == "")
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
        //    lblErr.Text = "Kindly enter area location.";
        //    txtAreaLocation.Focus();
        //    return;
        //}
        string strReportCaption = string.Empty, strSqlQuery = string.Empty;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        ShowBookingDetails(strFromDate, strToDate);       
        if (grdReport.Rows.Count > 0)
        {          
            btnExport.Visible = true;
            grdReport.Visible = true;
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
        DataSet dsMain = new DataSet();
        DTO.Report ObMain = new DTO.Report();
        ObMain.FromDate = strStartDate;
        ObMain.UptoDate = strToDate;
        ObMain.CustCodeStr = txtAreaLocation.Text;
        ObMain.BranchId = Globals.BranchID;
        dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReportAreaLocation(ObMain);
        if (dsMain.Tables.Count > 0)
        {
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                grdReport.DataSource = dsMain.Tables[0];
                grdReport.DataBind();
                AppClass.CalcuateAndSetGridFooter(ref grdReport);
                grdReport.FooterRow.Cells[4].Text = "";
            }
        }
    }   
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Area Location report from " + hdnStartDate.Value + " to " + hdnEndDate.Value + ".xls";
        Response.Expires = 0;
        Response.Buffer = true;
        string strName = string.Empty;
        if (txtAreaLocation.Text != "")
        {
            strName = " - " + txtAreaLocation.Text;
        }
        else
        {
            strName = "";
        }
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Area Location Report" + strName, false);
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
            Label lblDueDate = (Label)e.Row.FindControl("lblDate");
            string strDuedate = lblDueDate.Text;           
            hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }
    
}
