using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Data;
using System.Text;

namespace QuickWeb.Reports
{
    public partial class BookingReportWithoutSlip : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        ArrayList date = new ArrayList();
        public static StringWriter sw;
        public static string strAllContents = string.Empty;

        public static string hdnIsPrintingForMany()
        {
            //return this.hdnDTOReportsBFlag.Value.ToString();
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
                var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
                strFromDate = DateFromAndTo[0].Trim();
                strToDate = DateFromAndTo[1].Trim();
                ShowBookingDetails(strFromDate, strFromDate);
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Focus();
            }            
        }

        protected void SetDTOFalse()
        {
            //DTO.Report.BFlag = false;
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ViewState["exlquery"] = null;
            string strSqlQuery = "";
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();
            hdnStartDate.Value = strFromDate;
            hdnEndDate.Value = strToDate;
            ShowBookingDetails(strFromDate, strToDate);
            if (grdReport.Rows.Count > 0)
            {
                AppClass.CalcuateAndSetGridFooter(ref grdReport);          
                bool blnRight1 = AppClass.CheckExportToExcelRightOnPage();
                if (blnRight1)
                {
                    btnExport.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                }               
                grdReport.Visible = true;
                ViewState["exlquery"] = strSqlQuery;

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
            btnExport.Visible = false;
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
            if (txtInvoiceNo.Text!="")
                ObMain.Description = "6";
            else
                ObMain.Description = "5";
            DataSet dsMain = new DataSet();
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReport(ObMain);
            try
            {

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
                        DTO.Report.BFlag = false;

                        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
                        else
                            HttpContext.Current.Items["IsPrintingForMany"] = "false";

                        hdnDTOReportsBFlag.Value = "false";
                        ViewState["SavedDS"] = dsMain;
                        AppClass.CalcuateAndSetGridFooter(ref grdReport);
                        bool blnRight = AppClass.CheckExportToExcelRightOnPage();
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
            }
            catch (Exception excp)
            {
                lblErr.Text = "Error : " + excp.Message;
            }
            finally
            {
                txtInvoiceNo.Focus();
            }
        }       
        protected void btnExport_Click(object sender, EventArgs e)
        {           

            string strFileName = "Without Ticket Delivery from " + hdnStartDate.Value + " to " + hdnEndDate.Value + ".xls";
            Response.Expires = 0;
            Response.Buffer = true;

            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Without Ticket Delivery Report", false);
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

        protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
            btnShowReport_Click(null, null);
            txtInvoiceNo.Text = "";
            txtInvoiceNo.Focus();
        }
    }
}