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
using System.Collections.Generic;

namespace QuickWeb.Reports
{
    public partial class frmInvoiceStatement : BasePage
    {
        string strFromDate = "", strToDate = "", strGridCap = "";
        ArrayList date = new ArrayList();
        DTO.Report ob = new DTO.Report();
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            }
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string[] CustName = txtCustomerName.Text.Split('-');
            hdnCustId.Value = CustName[0].ToString();
            setCustvalue(CustName[0].ToString());
            btnShowReport_Click(null, EventArgs.Empty);
        }
        public void setCustvalue(string customerName)
        {
            DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
            if (DS_CustInfo.Tables[0].Rows.Count > 0)
                txtCustomerName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString() + " "+ DS_CustInfo.Tables[0].Rows[0]["CustAddress"].ToString();
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();
            ShowBookingDetails(strFromDate, strToDate, ob.CustId);
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
         private void ShowBookingDetails(string strStartDate, string strToDate, string CustId)
         {
             DataSet ds = new DataSet();
             grdReport.DataSource = null;
             grdReport.DataBind();
             ob.BranchId = Globals.BranchID;
             ob.FromDate = strStartDate;
             ob.UptoDate = strToDate;
             hdnStartDate.Value = strStartDate;
             hdnEndDate.Value = strToDate;
             ob.CustId = hdnCustId.Value;
             ds = BAL.BALFactory.Instance.Bal_Report.GetInvoiceStatement(ob);            
             try
             {
                 grdReport.DataSource = ds;
                 grdReport.DataBind();
                 hdntmpColCount.Value = ds.Tables[0].Columns.Count.ToString();
                 AppClass.CalcuateAndSetGridFooter(ref grdReport);
                 DataColumnCollection columns = ds.Tables[0].Columns;
                 int colIndex= -1;
                 if (columns.Contains("Rate"))
                 {
                     colIndex = columns.IndexOf("Rate");
                 }
                 grdReport.FooterRow.Cells[colIndex].Text = "";
                 btnExport.Visible = true;
                 grdReport.FooterRow.BackColor = System.Drawing.Color.Silver;
                 grdReport.FooterRow.Font.Bold = true;
                 btnInvoicePrint.Visible = true;
             }                
             catch (Exception e)
             {
                 grdReport.DataSource = null;
                 grdReport.DataBind();
                 btnExport.Visible = false;
                 btnInvoicePrint.Visible = false;
             }  
         }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "Invoice statement from " + hdnStartDate.Value + " to " + hdnEndDate.Value + ".xls";
            Response.Expires = 0;
            Response.Buffer = true;

            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForDynamicColumn(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Invoice statement Report", hdntmpColCount.Value, false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }       

        protected void drpReportFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnShowReport_Click(null, null);
        }
        string strPrinterName = string.Empty;
        protected void btnInvoicePrint_Click(object sender, EventArgs e)
        {
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
            var urlToOpen = "../" + strPrinterName + "/InvoiceStatementSlip.aspx?CustCode=" + hdnCustId.Value + "&fromDate=" + hdnStartDate.Value + "&ToDate=" + hdnEndDate.Value;
            urlToOpen += "&DirectPrint=true&RedirectBack=true&closeWindow=true";       
            OpenUrlFromBasePage(urlToOpen);
        }
        protected void OpenUrlFromBasePage(string urlToOpen)
        {
            OpenWindow(this.Page, urlToOpen);
        }
        
    }
}