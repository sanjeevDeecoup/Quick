using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

namespace QuickWeb.Reports
{
    public partial class MarkedReadyReport : System.Web.UI.Page
    {
        private DTO.Report Obj = new DTO.Report();
        public static bool blnRight = false;
        private ArrayList date = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUsers();
                BindItems();
                BindDropDown();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
                BtnShowReportClick(null, null);
            }
        }

        protected void BindUsers()
        {
            var allUsers = BAL.BALFactory.Instance.BL_UserMaster.GetAllUsers(Globals.BranchID);
            ddlChkUsers.DataSource = allUsers;
            ddlChkUsers.DataBind();
        }

        protected void BindItems()
        {
            var allItems = BAL.BALFactory.Instance.BAL_Item.GetAllItemsDetailed(Globals.BranchID);
            ddlChkItems.DataSource = allItems;
            ddlChkItems.DataBind();
        }

        protected void BtnShowReportClick(object sender, EventArgs e)
        {
            string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
            var dates = SetDuration();
            var users = hdnUsers.Value;
            var items = hdnItems.Value;
            SetDto(dates, ref users, ref items);
            ShowReport();
            btnPrint.Visible = true;
            ShowMarkedReadyDetails(Obj.FromDate, Obj.UptoDate);
        }
        protected void BtnExcel(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnFromDate.Value, hdnToDate.Value, "Marked Ready Report", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        private Tuple<string, string> SetDuration()
        {
            string startDate = string.Empty, endDate = string.Empty;

            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            startDate = DateFromAndTo[0].Trim();
            endDate = DateFromAndTo[1].Trim();
            hdnFromDate.Value = startDate;
            hdnToDate.Value = endDate;
            return Tuple.Create<string, string>(startDate, endDate);
        }

        protected void SetDto(Tuple<string, string> startEndDates, ref string users, ref string items)
        {
            string BookingPrefix = string.Empty;
            Obj.BranchId = Globals.BranchID;
            Obj.FromDate = startEndDates.Item1;
            Obj.UptoDate = startEndDates.Item2;
            Obj.CustCodeStr = users;
            Obj.StrCodes = items;
            if (txtStartBkNo.Text != "" && txtEndBkNo.Text != "")
            {
                BookingPrefix = drpBookingPreFix.SelectedItem.Text.Trim();
            }
            Obj.StartBkNum = BookingPrefix.Trim() + txtStartBkNo.Text;
            Obj.EndBkNum = BookingPrefix.Trim() + txtEndBkNo.Text;
            Obj.InvoiceNo = "4";
        }

        protected void ShowReport()
        {
            btnExportToExcel.Visible = false;
            var dataSet = BAL.BALFactory.Instance.Bal_Report.GetMarkeReadyData(Obj);
            grdReport.DataSource = dataSet;
            grdReport.DataBind();
            if (grdReport.Rows.Count > 0)
            {
                blnRight = AppClass.CheckExportToExcelRightOnPage();
                if (blnRight)
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
                CalculateGridReport();
            }
        }
        private void CalculateGridReport()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                int OrderCount = 0;
                for (int r = 0; r < rc; r++)
                {
                    OrderCount++;
                }
                grdReport.FooterRow.Cells[4].Text = OrderCount.ToString();
                grdReport.FooterRow.Cells[3].Text = "Total garment";
            }
            catch (Exception ex)
            { }

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
    "  <PageWidth>10in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.5in</MarginTop>" +
    "  <MarginLeft>0.5in</MarginLeft>" +
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
            var fileName = "OutPut.pdf";
            PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);
        }

        private void ShowMarkedReadyDetails(string strStartDate, string strToDate)
        {

            DataSet dsMain = new DataSet();
            string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetMarkeReadyData(Obj);
            ReportViewer1.LocalReport.ReportPath = "RDLC/MarkedReadyReport.rdlc";
            //ReportViewer1.LocalReport.EnableHyperlinks = true;
            //ReportViewer1.LocalReport.EnableExternalImages = true;
            bool rights = AppClass.GetShowFooterRightsUser();
            string rvalue = string.Empty;
            if (rights == true)
                rvalue = "1";
            else
                rvalue = "0";
            ReportParameter[] parameters = new ReportParameter[7];
            string str = Request.Url.Authority;
            string newstr = str + Request.ApplicationPath;

            parameters[0] = new ReportParameter("FromDate", Obj.FromDate);
            parameters[1] = new ReportParameter("ToDate", Obj.UptoDate);
            parameters[2] = new ReportParameter("StrLink", newstr);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
            parameters[5] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[6] = new ReportParameter("TotalFooter", rvalue);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = dsMain.Tables[0];
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                //    ReportViewer1.Visible = true;
                btnPrint.Visible = true;
            }
            else
            {
                ReportViewer1.Visible = false;
                btnPrint.Visible = false;
            }
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

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
    }
}