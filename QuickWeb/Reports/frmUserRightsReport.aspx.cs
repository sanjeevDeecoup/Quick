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

namespace QuickWeb.Reports
{
    public partial class frmUserRightsReport : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        ArrayList date = new ArrayList();
        public static StringWriter sw;
        public static string strAllContents = string.Empty;        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtReportFrom.Text = date[0].ToString();
                txtReportUpto.Text = date[0].ToString();               
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2000; i <= 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
                BindDropDown();
            }            
        }
        private void BindDropDown()
        {
            if (Globals.UserType != "1")
            {
                lblUserType.Visible = false;
                drpUserType.Visible = false;
            }
            else
            {
                Ob.BranchId = Globals.BranchID;
                drpUserType.DataSource = BAL.BALFactory.Instance.Bal_Report.BindUserTypeDropDown(Ob);
                drpUserType.DataTextField = "UserType";
                drpUserType.DataValueField = "UserTypeId";
                drpUserType.DataBind();
            }
        }
        private void ShowBookingDetails(string frmDate, string toDate)
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            Ob.FromDate = frmDate;
            Ob.UptoDate = toDate;
            Ob.BranchId = Globals.BranchID;
            Ob.StrCodes = drpSearchType.SelectedItem.Text;
            Ob.CustCodeStr = Globals.UserType;
            if (Globals.UserType != "1")
            {
                Ob.CustId = Globals.UserName;
            }
            else
            {
                Ob.CustId = drpUserType.SelectedItem.Value;
            }            
            grdReport.DataSource = BAL.BALFactory.Instance.Bal_Report.BindUserRightsReport(Ob);
            grdReport.DataBind();
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ViewState["exlquery"] = null;
            string strSqlQuery = "";
            if (!chkInvoice.Checked)
            {
                if (radReportFrom.Checked)
                {
                    if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
                    DateTime dt = DateTime.Parse(txtReportUpto.Text);
                    DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
                    DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
                    strFromDate = txtReportFrom.Text;
                    strToDate = txtReportUpto.Text;
                    strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
                }
                else if (radReportMonthly.Checked)
                {
                    DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                    strFromDate = dt.ToString("dd MMM yyyy");
                    strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                    strGridCap = "Booking Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
                }              
                ShowBookingDetails(strFromDate, strToDate);
            }
            else
            {
                ShowBookingDetails(strFromDate, strToDate);
            }
            if (grdReport.Rows.Count > 0)
            {
                CalculateGridReport();
                btnExport.Visible = true;              
                grdReport.Visible = true;
                ViewState["exlquery"] = strSqlQuery;
            }
            else
            {
                btnExport.Visible = false;
            }
        }
        
        private void CalculateGridReport()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                float Paid = 0, St = 0, Ad = 0, Bal = 0, OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0;
                for (int r = 0; r < rc; r++)
                {
                    OrderCount++;
                    if (grdReport.Rows[r].Cells[3].Text.Trim().ToUpper() != "CANCELLED")
                    {
                        TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[6].Text);
                        TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[7].Text);
                        St += float.Parse("0" + grdReport.Rows[r].Cells[8].Text);
                        Ad += float.Parse("0" + grdReport.Rows[r].Cells[10].Text);
                        Bal += float.Parse("0" + grdReport.Rows[r].Cells[11].Text);
                        Paid += float.Parse("0" + grdReport.Rows[r].Cells[12].Text);
                        BalanceAmount += float.Parse("0" + grdReport.Rows[r].Cells[9].Text);
                        TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[13].Text);
                    }
                }
                grdReport.FooterRow.Cells[2].Text = OrderCount.ToString();
                grdReport.FooterRow.Cells[6].Text = TotalPaid.ToString();
                grdReport.FooterRow.Cells[7].Text = TotalDue.ToString();
                grdReport.FooterRow.Cells[9].Text = BalanceAmount.ToString();
                grdReport.FooterRow.Cells[8].Text = St.ToString();
                grdReport.FooterRow.Cells[10].Text = Ad.ToString();
                grdReport.FooterRow.Cells[11].Text = Bal.ToString();
                grdReport.FooterRow.Cells[12].Text = Paid.ToString();
                grdReport.FooterRow.Cells[13].Text = TotalCostCount.ToString();
            }
            catch (Exception ex)
            { }
        }        
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();           
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }
        protected void chkInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInvoice.Checked)
            {
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Visible = true;
                txtInvoiceNo.Focus();
            }
            else
                txtInvoiceNo.Visible = false;
        }        
    }
}