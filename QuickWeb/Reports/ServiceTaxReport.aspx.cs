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
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using QuickWeb;

public partial class ServiceTaxReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();

    # region userControlControls

    TextBox ucTxtRptFrom, ucTxtRptTo;
    DropDownList ucDrpMnthLst, ucDrpYrLst;
    RadioButton ucRadFrom, ucRadMnth;

    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        # region setUserControl
        
        ucTxtRptFrom = ((TextBox)uc1.FindControl("txtReportFrom"));
        ucTxtRptTo = ((TextBox)uc1.FindControl("txtReportUpto"));
        ucDrpMnthLst = ((DropDownList)uc1.FindControl("drpMonthList"));
        ucDrpYrLst = ((DropDownList)uc1.FindControl("drpYearList"));
        ucRadFrom = ((RadioButton)uc1.FindControl("radReportFrom"));
        ucRadMnth = ((RadioButton)uc1.FindControl("radReportMonthly"));

        # endregion

        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            ucTxtRptFrom.Text = date[0].ToString();
            ucTxtRptTo.Text = date[0].ToString();
            ucDrpMnthLst.SelectedIndex = DateTime.Today.Month - 1;
            ucDrpMnthLst.Items.Clear();
            for (int i = 2009; i < 2050; i++)
            {
                ucDrpMnthLst.Items.Add(i.ToString());
            }
            ucDrpMnthLst.SelectedIndex = DateTime.Today.Year - 2009;
            SDTProcesses.SelectCommand = "SELECT [ProcessCode], [ProcessName] FROM [ProcessMaster] WHERE BranchId='" + Globals.BranchID + "' order by ProcessName asc";
            SDTProcesses.DataBind();
        }
        //ReportViewer1.Visible = false;
    }
    private bool checkCheckbox()
    {
        int totalSelected = 0;
        for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
        {
            if (((CheckBox)grdProcessSelection.Rows[r].Cells[0].FindControl("chkSelect")).Checked) totalSelected++;
        }
        if (totalSelected > 0)
            return true;
        else
            return false;
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;        
        string strSqlQuery = string.Empty;
        bool status = false;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;        
        if (!checkCheckbox())
        {
            Session["ReturnMsg"] = "Please select atleast one checkbox.";           
        }
        else
        {
            if (ucRadFrom.Checked)
            {
                //strFromDate = txtReportFrom.Text + " 00:00:00";
                if (ucTxtRptTo.Text == "") { ucTxtRptTo.Text = ucTxtRptFrom.Text; }
                DateTime dt = DateTime.Parse(ucTxtRptTo.Text);
                DateTime dt3 = dt.AddDays(1);
                //strToDate = dt3.ToShortDateString() + " 00:00:00";
                DateTime dt1 = DateTime.Parse(ucTxtRptFrom.Text);
                DateTime dt2 = DateTime.Parse(ucTxtRptTo.Text);
                strFromDate = ucTxtRptFrom.Text;
                strToDate = ucTxtRptTo.Text;
                strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
                status = true;
            }
            else if (ucRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucDrpYrLst.SelectedItem.Text), int.Parse(ucDrpMnthLst.SelectedItem.Value), 1);
                strFromDate = dt.ToShortDateString() + " 00:00:00";
                strToDate = dt.AddMonths(1).AddDays(-1).ToShortDateString() + " 00:00:00";
                strGridCap = "Booking Report for " + ucDrpMnthLst.SelectedItem.Text + ", " + ucDrpYrLst.SelectedItem.Text;
            }

            try
            {
                string strProcessList = string.Empty, strProcessIncludeQuery = string.Empty;
                for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
                {
                    if (((CheckBox)grdProcessSelection.Rows[r].FindControl("chkSelect")).Checked)
                    {                        
                        strProcessList += "," + ((HiddenField)grdProcessSelection.Rows[r].FindControl("hdnProcessCode")).Value.Trim();
                    }
                }
                strProcessList = strProcessList.Substring(1);
                GetDetails(strProcessList, strFromDate, strToDate, status);
            }

            catch (Exception excp)
            {
                lblMsg.Text = "Error : " + excp.Message;
            }
            finally
            {

            }
        }
    }

    private void GetDetails(string ProcessList, string FromDate, string UptoDate,bool status)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "Sp_Report_ServiceTaxReport";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookDate1", FromDate);
        cmd.Parameters.AddWithValue("@BookDate2", UptoDate);        
        cmd.Parameters.AddWithValue("@SearchText", ProcessList);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ReportViewer1.Visible = true;
            DataSet ds1 = new ServiceTaxDataSet();
            btnPrintButton.Visible = true;
            DateTime dt1, dt2, dt3;
            if (status == true)
            {
                dt1 = DateTime.Parse(FromDate);
                dt2 = DateTime.Parse(UptoDate);
                //dt3 = dt2.AddDays(-1);
                //dt2 = dt3;
            }
            else
            {
                dt1 = DateTime.Parse(FromDate);
                dt2 = DateTime.Parse(UptoDate);
            }
            ds1 = ds;
            string str = "http://" + Request.Url.Authority;
            ReportViewer1.LocalReport.ReportPath = "RDLC/ServicetaxReport.rdlc";
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportParameter[] parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("FDate", dt1.ToString("dd-MMM-yyyy"));
            parameters[1] = new ReportParameter("UDate", dt2.ToString("dd-MMM-yyyy"));
            parameters[2] = new ReportParameter("Link", str);

            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "ServiceTaxDataSet_sp_service";
            rds.Value = ds1.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.DataBind();
            ReportViewer1.LocalReport.Refresh();
        }
        else
        {
            lblMsg.Text = "No record found.";
            btnPrintButton.Visible = false;
            ReportViewer1.Visible = false;
        }
            
    }

    protected void btnPrintButton_Click(object sender, EventArgs e)
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