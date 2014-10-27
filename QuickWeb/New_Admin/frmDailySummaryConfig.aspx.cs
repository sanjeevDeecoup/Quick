using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using Microsoft.Reporting.WebForms;


namespace QuickWeb.New_Admin
{
    public partial class frmDailySummaryConfig : System.Web.UI.Page
    {
        # region userControlControls

        TextBox ucBookingTxtRptFrom, ucBookingTxtRptTo, ucSalesTxtRptFrom, ucSalesTxtRptTo, ucDeliveryTxtRptFrom, ucDeliveryTxtRptTo, ucPaymentTypeTxtRptFrom, ucPaymentTypeTxtRptTo, ucDailyCustomerTxtRptFrom, ucDailyCustomerTxtRptTo, ucDetailCashBookTxtRptFrom, ucDetailCashBookTxtRptTo;
        DropDownList ucBookingDrpMnthLst, ucBookingDrpYrLst, ucSalesDrpMnthLst, ucSalesDrpYrLst, ucDeliveryDrpMnthLst, ucDeliveryDrpYrLst, ucPaymentTypeDrpMnthLst, ucPymentTypeDrpYrLst, ucDailyCustomerDrpMnthLst, ucDailyCustomerDrpYrLst, ucDetailCashBookDrpMnthLst, ucDetailCashBookDrpYrLst;
        RadioButton ucBookingRadFrom, ucBookingRadMnth, ucSalesRadFrom, ucSalesRadMnth, ucDeliveryRadFrom, ucDeliveryRadMnth, ucPaymentTypeRadFrom, ucPaymentTypeRadMnth, ucDailyCustomerRadFrom, ucDailyCustomerRadMnth, ucDetailCashBookRadFrom, ucDetailCashBookRadMnth;


        # endregion
        private DTO.StatusSummary_DTO Ob = new DTO.StatusSummary_DTO();
        DTO.Report ObAll = new DTO.Report();
        DataSet dsMain = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlCommand cmd = new SqlCommand();              
                SqlDataReader sdr = null;
                try
                {
                    cmd.CommandText = "sp_ReceiptConfigSetting";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                    cmd.Parameters.AddWithValue("@Flag", 16);
                   
                    sdr = AppClass.ExecuteReader(cmd);
                    if (sdr.Read())
                    {
                        lblBranchMailId.Text = "" + sdr.GetValue(0);
                    }
                    else
                        lblBranchMailId.Text = "";
                }
                catch (Exception ex) { }
                finally
                {
                    sdr.Close();
                    sdr.Dispose();
                    cmd.Dispose();
                }
            }
            

        }

        public DTO.StatusSummary_DTO SetValue()
        {
            # region SetuserControlControls
            //// register name space 

            // Booking
            ucBookingTxtRptFrom = ((TextBox)uc1.FindControl("txtReportFrom"));
            ucBookingTxtRptTo = ((TextBox)uc1.FindControl("txtReportUpto"));
            ucBookingDrpMnthLst = ((DropDownList)uc1.FindControl("drpMonthList"));
            ucBookingDrpYrLst = ((DropDownList)uc1.FindControl("drpYearList"));
            ucBookingRadFrom = ((RadioButton)uc1.FindControl("radReportFrom"));
            ucBookingRadMnth = ((RadioButton)uc1.FindControl("radReportMonthly"));

            // Sales
            ucSalesTxtRptFrom = ((TextBox)uc2.FindControl("txtReportFrom"));
            ucSalesTxtRptTo = ((TextBox)uc2.FindControl("txtReportUpto"));
            ucSalesDrpMnthLst = ((DropDownList)uc2.FindControl("drpMonthList"));
            ucSalesDrpYrLst = ((DropDownList)uc2.FindControl("drpYearList"));
            ucSalesRadFrom = ((RadioButton)uc2.FindControl("radReportFrom"));
            ucSalesRadMnth = ((RadioButton)uc2.FindControl("radReportMonthly"));

            // Delivery
            ucDeliveryTxtRptFrom = ((TextBox)uc3.FindControl("txtReportFrom"));
            ucDeliveryTxtRptTo = ((TextBox)uc3.FindControl("txtReportUpto"));
            ucDeliveryDrpMnthLst = ((DropDownList)uc3.FindControl("drpMonthList"));
            ucDeliveryDrpYrLst = ((DropDownList)uc3.FindControl("drpYearList"));
            ucDeliveryRadFrom = ((RadioButton)uc3.FindControl("radReportFrom"));
            ucDeliveryRadMnth = ((RadioButton)uc3.FindControl("radReportMonthly"));

            // Payment Type
            ucPaymentTypeTxtRptFrom = ((TextBox)uc4.FindControl("txtReportFrom"));
            ucPaymentTypeTxtRptTo = ((TextBox)uc4.FindControl("txtReportUpto"));
            ucPaymentTypeDrpMnthLst = ((DropDownList)uc4.FindControl("drpMonthList"));
            ucPymentTypeDrpYrLst = ((DropDownList)uc4.FindControl("drpYearList"));
            ucPaymentTypeRadFrom = ((RadioButton)uc4.FindControl("radReportFrom"));
            ucPaymentTypeRadMnth = ((RadioButton)uc4.FindControl("radReportMonthly"));

            // Daily Customer
            ucDailyCustomerTxtRptFrom = ((TextBox)uc5.FindControl("txtReportFrom"));
            ucDailyCustomerTxtRptTo = ((TextBox)uc5.FindControl("txtReportUpto"));
            ucDailyCustomerDrpMnthLst = ((DropDownList)uc5.FindControl("drpMonthList"));
            ucDailyCustomerDrpYrLst = ((DropDownList)uc5.FindControl("drpYearList"));
            ucDailyCustomerRadFrom = ((RadioButton)uc5.FindControl("radReportFrom"));
            ucDailyCustomerRadMnth = ((RadioButton)uc5.FindControl("radReportMonthly"));

            // Detail CashBook
            ucDetailCashBookTxtRptFrom = ((TextBox)uc6.FindControl("txtReportFrom"));
            ucDetailCashBookTxtRptTo = ((TextBox)uc6.FindControl("txtReportUpto"));
            ucDetailCashBookDrpMnthLst = ((DropDownList)uc6.FindControl("drpMonthList"));
            ucDetailCashBookDrpYrLst = ((DropDownList)uc6.FindControl("drpYearList"));
            ucDetailCashBookRadFrom = ((RadioButton)uc6.FindControl("radReportFrom"));
            ucDetailCashBookRadMnth = ((RadioButton)uc6.FindControl("radReportMonthly"));

            # endregion

            if (ucBookingRadFrom.Checked)
            {
                if (ucBookingTxtRptTo.Text == "") { ucBookingTxtRptTo.Text = ucBookingTxtRptFrom.Text; }
                Ob.ucBookingTxtRptFrom = ucBookingTxtRptFrom.Text;
                Ob.ucBookingTxtRptTo = ucBookingTxtRptTo.Text;
            }
            else if (ucBookingRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucBookingDrpYrLst.SelectedItem.Text), int.Parse(ucBookingDrpMnthLst.SelectedItem.Value), 1);
                Ob.ucBookingTxtRptFrom = dt.ToString("dd MMM yyyy");
                Ob.ucBookingTxtRptTo = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            if (ucSalesRadFrom.Checked)
            {
                if (ucSalesTxtRptTo.Text == "") { ucSalesTxtRptTo.Text = ucSalesTxtRptFrom.Text; }
                Ob.ucSalesTxtRptFrom = ucSalesTxtRptFrom.Text;
                Ob.ucSalesTxtRptTo = ucSalesTxtRptTo.Text;
            }
            else if (ucSalesRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucSalesDrpYrLst.SelectedItem.Text), int.Parse(ucSalesDrpMnthLst.SelectedItem.Value), 1);
                Ob.ucSalesTxtRptFrom = dt.ToString("dd MMM yyyy");
                Ob.ucSalesTxtRptTo = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            if (ucDeliveryRadFrom.Checked)
            {
                if (ucDeliveryTxtRptTo.Text == "") { ucDeliveryTxtRptTo.Text = ucDeliveryTxtRptFrom.Text; }
                Ob.ucDeliveryTxtRptFrom = ucDeliveryTxtRptFrom.Text;
                Ob.ucDeliveryTxtRptTo = ucDeliveryTxtRptTo.Text;
            }
            else if (ucDeliveryRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucDeliveryDrpYrLst.SelectedItem.Text), int.Parse(ucDeliveryDrpMnthLst.SelectedItem.Value), 1);
                Ob.ucDeliveryTxtRptFrom = dt.ToString("dd MMM yyyy");
                Ob.ucDeliveryTxtRptTo = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            if (ucPaymentTypeRadFrom.Checked)
            {
                if (ucPaymentTypeTxtRptTo.Text == "") { ucPaymentTypeTxtRptTo.Text = ucPaymentTypeTxtRptFrom.Text; }
                Ob.ucPaymentTypeTxtRptFrom = ucPaymentTypeTxtRptFrom.Text;
                Ob.ucPaymentTypeTxtRptTo = ucPaymentTypeTxtRptTo.Text;
            }
            else if (ucPaymentTypeRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucPymentTypeDrpYrLst.SelectedItem.Text), int.Parse(ucPaymentTypeDrpMnthLst.SelectedItem.Value), 1);
                Ob.ucPaymentTypeTxtRptFrom = dt.ToString("dd MMM yyyy");
                Ob.ucPaymentTypeTxtRptTo = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            if (ucDailyCustomerRadFrom.Checked)
            {
                if (ucDailyCustomerTxtRptTo.Text == "") { ucDailyCustomerTxtRptTo.Text = ucDailyCustomerTxtRptFrom.Text; }
                Ob.ucDailyCustomerTxtRptFrom = ucDailyCustomerTxtRptFrom.Text;
                Ob.ucDailyCustomerTxtRptTo = ucDailyCustomerTxtRptTo.Text;
            }
            else if (ucDailyCustomerRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucDailyCustomerDrpYrLst.SelectedItem.Text), int.Parse(ucDailyCustomerDrpMnthLst.SelectedItem.Value), 1);
                Ob.ucDailyCustomerTxtRptFrom = dt.ToString("dd MMM yyyy");
                Ob.ucDailyCustomerTxtRptTo = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            if (ucDetailCashBookRadFrom.Checked)
            {
                if (ucDetailCashBookTxtRptTo.Text == "") { ucDetailCashBookTxtRptTo.Text = ucDetailCashBookTxtRptFrom.Text; }
                Ob.ucDetailCashBookTxtRptFrom = ucDetailCashBookTxtRptFrom.Text;
                Ob.ucDetailCashBookTxtRptTo = ucDetailCashBookTxtRptTo.Text;
            }
            else if (ucDetailCashBookRadMnth.Checked)
            {
                DateTime dt = new DateTime(int.Parse(ucDetailCashBookDrpYrLst.SelectedItem.Text), int.Parse(ucDetailCashBookDrpMnthLst.SelectedItem.Value), 1);
                Ob.ucDetailCashBookTxtRptFrom = dt.ToString("dd MMM yyyy");
                Ob.ucDetailCashBookTxtRptTo = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }

            /// Set Right To View Report Viewer Footer
            bool rights = AppClass.GetShowFooterRightsUser();
            if (rights == true)
                Ob.FooterRights = "1";
            else
                Ob.FooterRights = "0";

            //// Set Report Time In Report Viewer
            Ob.ReportTime = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);

            return Ob;
        }

        protected void btnSendStatus_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            DataSet dsPayment = new DataSet();
            string files = string.Empty;
            BAL.BALFactory.Instance.BL_WorkShopAllFunction.FindAndKillProcess("AcroRd32");
            SetCurrenyType();
            switch (1)
            {
                case 1:
                    if (chkBookingReport.Checked)
                    {
                        CreateBookingPDF(Ob);
                        files += Server.MapPath("~/Docs/BookingReport.pdf") + ",";
                    }
                    goto case 2;

                case 2:
                    if (chkSalesReport.Checked)
                    {
                        CreateSalesPDF(Ob);
                        files += Server.MapPath("~/Docs/SalesReport.pdf") + ",";
                    }
                    goto case 3;
                case 3:
                    if (chkDeliveryReport.Checked)
                    {
                        CreateDeliveryPDF(Ob);
                        files += Server.MapPath("~/Docs/DeliveryReport.pdf") + ",";
                    }
                    goto case 4;
                case 4:
                    if (chkPaymentTypeReport.Checked)
                    {
                        dsPayment = CreatePaymentTypePDF(Ob);
                        files += Server.MapPath("~/Docs/PaymentTypeReport.pdf") + ",";
                    }
                    goto case 5;
                case 5:
                    if (chkDailyCustAdd.Checked)
                    {
                        CreateDailyCustomerAdditionPDF(Ob);
                        files += Server.MapPath("~/Docs/DailyCustomerAddition.pdf") + ",";
                    }
                    goto case 6;
                case 6:
                    if (chkDetailCashbook.Checked)
                    {
                        CreateDetailsCashBookPDF(Ob);
                        files += Server.MapPath("~/Docs/DetailsCashBookReport.pdf") + ",";
                    }
                    break;
            }

            CreateStatusHeadingPagePDF(dsPayment);
            string HeadingPagePath = Server.MapPath("~/Docs/HeadingPageReport.pdf") + ",";

            files = HeadingPagePath + files.Substring(0, files.Length - 1);
            string[] namesArray = files.Split(',');
            byte[] renderedBytes = MergeFiles(namesArray);
            var fileName = "AllStatuReport.pdf";

            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            SetTrueCheckBoxButton(true);
        //    SetFalseRadioButton(false);
            foreach (string filePath in namesArray)
            { File.Delete(filePath); }
            if (lblBranchMailId.Text == "")
            {
                PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);
            }
            else
            {
                SendMail(renderedBytes);
            }
        }
        private void SetTrueCheckBoxButton(bool status)
        {
            chkBookingReport.Checked = status;
            chkSalesReport.Checked = status;
            chkDeliveryReport.Checked = status;
            chkPaymentTypeReport.Checked = status;
            chkDailyCustAdd.Checked = status;
            chkDetailCashbook.Checked = status;
        }
        //private void SetFalseRadioButton(bool status)
        //{
        //    rdbBookingReportFalse.Checked = status;
        //    rdbSalesReportFalse.Checked = status;
        //    rdbDeliveryReportFalse.Checked = status;
        //    rdbPaymentTypeReportFalse.Checked = status;
        //    rdbabcFalse.Checked = status;
        //    rdbDetailCashBookReportFalse.Checked = status;
        //}
        private void CreateBookingPDF(DTO.StatusSummary_DTO Ob)
        {
            ObAll.FromDate = Ob.ucBookingTxtRptFrom;
            ObAll.UptoDate = Ob.ucBookingTxtRptTo;
            ObAll.Description = "1";
            ObAll.BranchId = Globals.BranchID;
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReport(ObAll);
            Ob.TotNewBookingOrder = dsMain.Tables[0].Rows.Count;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                Ob.TotBookingAmount = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(NetAmount)", string.Empty));
                Ob.TotBookingAdvance = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(PaymentMade)", string.Empty));
            }

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.ReportPath = "RDLC/BookingReport.rdlc";

            ReportParameter[] parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("UserName", Globals.UserName);
            parameters[1] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);
            parameters[2] = new ReportParameter("FDate", Ob.ucBookingTxtRptFrom);
            parameters[3] = new ReportParameter("LDate", Ob.ucBookingTxtRptTo);
            parameters[4] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[5] = new ReportParameter("BookingText", "Booking Report");

            parameters[6] = new ReportParameter("TotalFooter", Ob.FooterRights);
            //parameters[2] = new ReportParameter("Link", str);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = dsMain.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
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
            var fileName = "BookingReport.pdf";
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("BookingReport.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();
        }
        private void CreateSalesPDF(DTO.StatusSummary_DTO Ob)
        {
            ObAll.FromDate = Ob.ucSalesTxtRptFrom;
            ObAll.UptoDate = Ob.ucSalesTxtRptTo;
            ObAll.BranchId = Globals.BranchID;
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetSaleFrom_To_UptoDate(ObAll);

            Ob.SalesTotOrder = dsMain.Tables[0].Rows.Count;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                Ob.SalesTotPaymentMade = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(PaymentMade)", string.Empty));
                Ob.SalesTotDisAmt = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(DiscountAmt)", string.Empty));
                Ob.SalesTotBalAmt = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(BalAmt)", string.Empty));
            }

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.ReportPath = "RDLC/SalesReport.rdlc";
            ReportDataSource rds = new ReportDataSource();
            ReportParameter[] parameters = new ReportParameter[7];
            string str = Request.Url.Authority;
            string newstr = str + Request.ApplicationPath;
            parameters[0] = new ReportParameter("FromDate", ObAll.FromDate);
            parameters[1] = new ReportParameter("ToDate", ObAll.UptoDate);
            parameters[2] = new ReportParameter("StrLink", newstr);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);
            parameters[5] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[6] = new ReportParameter("TotalFooter", Ob.FooterRights);
            ReportViewer1.LocalReport.SetParameters(parameters);
            rds.Name = "ReportDelivery_Sales";
            rds.Value = dsMain.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

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
            var fileName = "SalesReport.pdf";
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {

                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("SalesReport.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

        }
        private void CreateDeliveryPDF(DTO.StatusSummary_DTO Ob)
        {
            ObAll.FromDate = Ob.ucDeliveryTxtRptFrom;
            ObAll.UptoDate = Ob.ucDeliveryTxtRptTo;
            ObAll.BranchId = Globals.BranchID;
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDeliveryFrom_To_UptoDate(ObAll);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                Ob.TotDelQty = Convert.ToInt32(dsMain.Tables[0].Compute("SUM(DelQty)", string.Empty));
                Ob.TotalQty = Convert.ToInt32(dsMain.Tables[0].Compute("SUM(TotalQty)", string.Empty));
                Ob.TotDelBalQty = Convert.ToInt32(dsMain.Tables[0].Compute("SUM(BalQty)", string.Empty));
            }

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.ReportPath = "RDLC/DeliveryReport.rdlc";

            ReportDataSource rds = new ReportDataSource();
            ReportParameter[] parameters = new ReportParameter[7];
            string str = Request.Url.Authority;
            string newstr = str + Request.ApplicationPath;
            parameters[0] = new ReportParameter("FromDate", ObAll.FromDate);
            parameters[1] = new ReportParameter("ToDate", ObAll.UptoDate);
            parameters[2] = new ReportParameter("StrLink", newstr);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);
            parameters[5] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[6] = new ReportParameter("TotalFooter", Ob.FooterRights);
            ReportViewer1.LocalReport.SetParameters(parameters);
            rds.Name = "ReportDelivery_Delivery";
            rds.Value = dsMain.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

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
            var fileName = "DeliveryReport.pdf";
            //PrjClass.ShowPdfFromRdlcDirect(this.Page, renderedBytes, fileName);
            //var fileNameInner = Path.Combine("../Docs/", fileName);
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {

                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("DeliveryReport.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            //iTextSharp.text.Rectangle psize = reader.GetPageSize(1);
            //float width = psize.Width;
            //float height = psize.Height;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();
            //   frmPrint.Attributes["src"] = "BookingReport.pdf";


        }
        private DataSet CreatePaymentTypePDF(DTO.StatusSummary_DTO Ob)
        {


            ObAll.FromDate = Ob.ucPaymentTypeTxtRptFrom;
            ObAll.UptoDate = Ob.ucPaymentTypeTxtRptTo;
            ObAll.BranchId = Globals.BranchID;
            dsMain = BAL.BALFactory.Instance.BL_PriceList.PaymentTypeGetDetails("Cash,Credit Card/Debit Card,Cheque/Bank", "Cash,Credit Card/Debit Card,Cheque/Bank", ObAll.FromDate, ObAll.UptoDate, false, "True", Globals.BranchID, "True", "false");

            ReportViewer1.Reset();
            string str = "http://" + Request.Url.Authority;
            ReportViewer1.LocalReport.ReportPath = "RDLC/PaymentTypeReport.rdlc";
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportDataSource rds = new ReportDataSource();
            ReportParameter[] parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("FDate", ObAll.FromDate);
            parameters[1] = new ReportParameter("UDate", ObAll.UptoDate);
            parameters[2] = new ReportParameter("Link", str);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);

            ReportViewer1.LocalReport.SetParameters(parameters);
            rds.Name = "PaymentTypeDataSet_sp_Payment";
            rds.Value = dsMain.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                   "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>9.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.0in</MarginRight>" +
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
            var fileName = "PaymentTypeReport.pdf";
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {

                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("PaymentTypeReport.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();
            return dsMain;
        }
        private void CreateDetailsCashBookPDF(DTO.StatusSummary_DTO Ob)
        {
            ObAll.FromDate = Ob.ucDetailCashBookTxtRptFrom;
            ObAll.UptoDate = Ob.ucDetailCashBookTxtRptTo;
            ObAll.BranchId = Globals.BranchID;
            dsMain = BAL.BALFactory.Instance.Bal_Processmaster.ShowDetailCashBook(Globals.BranchID, ObAll.FromDate, ObAll.UptoDate);
            int rowCount = dsMain.Tables[0].Rows.Count;
            if (rowCount > 0)
            {
                Ob.CashBookOpeningBalAmt = Convert.ToDouble(dsMain.Tables[0].Rows[0]["OpeningBalance"]);
                Ob.CashBookReceivedAmt = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(Debit)", string.Empty));
                Ob.CashBookPaymentAmt = Convert.ToDouble(dsMain.Tables[0].Compute("SUM(Credit)", string.Empty));
                Ob.CashBookBalanceAmt = Convert.ToDouble(dsMain.Tables[0].Rows[rowCount - 1]["ClosingBalance"]);
            }

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.ReportPath = "RDLC/DetailCashReport.rdlc";
            ReportDataSource rds = new ReportDataSource();
            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("FDate", ObAll.FromDate);
            parameters[1] = new ReportParameter("UDate", ObAll.UptoDate);
            parameters[2] = new ReportParameter("UserName", Globals.UserName);
            parameters[3] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);
            //parameters[2] = new ReportParameter("Link", str);
            ReportViewer1.LocalReport.SetParameters(parameters);
            rds.Name = "DetailCash_CashDetail";
            rds.Value = dsMain.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

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
            var fileName = "DetailsCashBookReport.pdf";
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {

                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("DetailsCashBookReport.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

        }
        private void CreateDailyCustomerAdditionPDF(DTO.StatusSummary_DTO Ob)
        {
            ObAll.FromDate = Ob.ucDailyCustomerTxtRptFrom;
            ObAll.UptoDate = Ob.ucDailyCustomerTxtRptTo;
            ObAll.BranchId = Globals.BranchID;
            ObAll.InvoiceNo = "1";
            ObAll.CustId = "0";
            ObAll.StrCodes = "";
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDailyCustomerAdditionReport(ObAll);
            Ob.DailyCustomerNewAdded = dsMain.Tables[0].Rows.Count;
            if (dsMain.Tables[2].Rows.Count > 0)
            {
                Ob.DailyCustomerTotBuseAmt = Convert.ToDouble(dsMain.Tables[2].Compute("SUM(volume)", string.Empty));
                if (dsMain.Tables[1].Rows.Count > 0)
                {
                    Ob.DailyCustomerTotAmtNewCust = Convert.ToDouble(dsMain.Tables[1].Compute("SUM(volume)", string.Empty));
                }
                if (Ob.DailyCustomerTotBuseAmt > 0)
                {
                    Ob.DailyCustomerPercent = Math.Round(((Ob.DailyCustomerTotAmtNewCust * 100) / Ob.DailyCustomerTotBuseAmt), 2);
                }
            }

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.ReportPath = "RDLC/DailyCustomerAdditionReport.rdlc";

            ReportParameter[] parameters = new ReportParameter[7];
            string str = Request.Url.Authority;
            string newstr = str + Request.ApplicationPath;

            parameters[0] = new ReportParameter("FromDate", ObAll.FromDate);
            parameters[1] = new ReportParameter("ToDate", ObAll.UptoDate);
            parameters[2] = new ReportParameter("StrLink", newstr);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);
            parameters[5] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[6] = new ReportParameter("TotalFooter", Ob.FooterRights);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = dsMain.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
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
        "  <MarginLeft>.2in</MarginLeft>" +
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
            var fileName = "DailyCustomerAddition.pdf";
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("DailyCustomerAddition.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();
        }
        private void CreateStatusHeadingPagePDF(DataSet ds)
        {
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.ReportPath = "RDLC/StatusHeadingPage.rdlc";
            ReportParameter[] parameters = new ReportParameter[29];

            parameters[0] = new ReportParameter("UserName", Globals.UserName);
            parameters[1] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + Ob.ReportTime);
            parameters[2] = new ReportParameter("FBookingDate", Ob.ucBookingTxtRptFrom);
            parameters[3] = new ReportParameter("LBookingDate", Ob.ucBookingTxtRptTo);
            parameters[4] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[5] = new ReportParameter("NewBookingOrder", Ob.TotNewBookingOrder.ToString());
            parameters[6] = new ReportParameter("TotBookingAmount", Ob.TotBookingAmount.ToString());
            parameters[7] = new ReportParameter("TotBookingAdvance", Ob.TotBookingAdvance.ToString());

            parameters[8] = new ReportParameter("DeliveryFromAndToDate", Ob.ucDeliveryTxtRptFrom + " - " + Ob.ucDeliveryTxtRptTo);
            parameters[9] = new ReportParameter("TotDelBalQty", Ob.TotDelBalQty.ToString());
            parameters[10] = new ReportParameter("TotalQty", Ob.TotalQty.ToString());
            parameters[11] = new ReportParameter("TotDelQty", Ob.TotDelQty.ToString());

            parameters[12] = new ReportParameter("SalesFromAndToDate", Ob.ucSalesTxtRptFrom + " - " + Ob.ucSalesTxtRptTo);
            parameters[13] = new ReportParameter("SalesTotOrder", Ob.SalesTotOrder.ToString());
            parameters[14] = new ReportParameter("SalesTotPaymentMade", Ob.SalesTotPaymentMade.ToString());
            parameters[15] = new ReportParameter("SalesTotDisAmt", Ob.SalesTotDisAmt.ToString());
            parameters[16] = new ReportParameter("SalesTotBalAmt", Ob.SalesTotBalAmt.ToString());

            parameters[17] = new ReportParameter("DailyCustFromAndToDate", Ob.ucDailyCustomerTxtRptFrom + " - " + Ob.ucDailyCustomerTxtRptTo);
            parameters[18] = new ReportParameter("DailyCustomerNewAdded", Ob.DailyCustomerNewAdded.ToString());
            parameters[19] = new ReportParameter("DailyCustomerTotBuseAmt", Ob.DailyCustomerTotBuseAmt.ToString());
            parameters[20] = new ReportParameter("DailyCustomerTotAmtNewCust", Ob.DailyCustomerTotAmtNewCust.ToString());
            parameters[21] = new ReportParameter("DailyCustomerPercent", Ob.DailyCustomerPercent.ToString());
            parameters[22] = new ReportParameter("DailyCustomerCurrencyType", Ob.DailyCustomerCurrencyType);

            parameters[23] = new ReportParameter("CashBookFromAndToDate", Ob.ucDetailCashBookTxtRptFrom + " - " + Ob.ucDetailCashBookTxtRptTo);
            parameters[24] = new ReportParameter("CashBookOpeningBalAmt", Ob.CashBookOpeningBalAmt.ToString());
            parameters[25] = new ReportParameter("CashBookReceivedAmt", Ob.CashBookReceivedAmt.ToString());
            parameters[26] = new ReportParameter("CashBookPaymentAmt", Ob.CashBookPaymentAmt.ToString());
            parameters[27] = new ReportParameter("CashBookBalanceAmt", Ob.CashBookBalanceAmt.ToString());

            parameters[28] = new ReportParameter("PaymentFromAndToDate", Ob.ucPaymentTypeTxtRptFrom + " - " + Ob.ucPaymentTypeTxtRptTo);

            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            //if (rdbPaymentTypeReportTrue.Checked)
            if (chkPaymentTypeReport.Checked)
            {
                rds.Value = ds.Tables[0];
            }
            else
            {
                DataTable tempdt = new DataTable();
                tempdt.Columns.Add("Amount");
                rds.Value = tempdt;
            }
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
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
        "  <MarginLeft>.3in</MarginLeft>" +
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
            var fileName = "HeadingPageReport.pdf";
            var fileNameInner = Path.Combine("../Docs/", fileName);
            using (var fs = new FileStream(Page.Server.MapPath(fileNameInner), FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();
            }
            iTextSharp.text.Rectangle pgsize = new Rectangle(684, 792);
            Document document = new Document(pgsize);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(fileNameInner));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("HeadingPageReport.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;
                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();
        }

        private void SetCurrenyType()
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {

                cmd.CommandText = "SELECT CurrencyType FROM mstReceiptConfig WHERE (BranchId = '" + Globals.BranchID + "') ";
                cmd.CommandType = CommandType.Text;
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    Ob.DailyCustomerCurrencyType = "" + sdr.GetValue(0).ToString();
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
        }
        

        public byte[] MergeFiles(string[] sourceFiles)
        {
            Document document = new Document();
            MemoryStream output = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                document.Open();
                PdfContentByte content = writer.DirectContent;
                for (int fileCounter = 0; fileCounter < sourceFiles.Length; fileCounter++)
                {
                    PdfReader reader = new PdfReader(sourceFiles[fileCounter]);
                    int numberOfPages = reader.NumberOfPages;
                    for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex));
                        document.NewPage();
                        PdfImportedPage importedPage =
                          writer.GetImportedPage(reader, currentPageIndex);
                        int pageOrientation = reader.GetPageRotation(currentPageIndex);
                        if ((pageOrientation == 90) || (pageOrientation == 270))
                        {
                            content.AddTemplate(importedPage, 0, -1f, 1f, 0, 0,
                               reader.GetPageSizeWithRotation(currentPageIndex).Height);
                        }
                        else
                        {
                            content.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("There has an unexpected exception" +
                      " occured during the pdf merging process.", exception);
            }
            finally
            {
                document.Close();
            }
            return output.GetBuffer();
        }
        private void SendMail(byte[] renderedBytes)
        {
            bool SSL = false;
            SqlCommand cmd = new SqlCommand();
            string eMail = string.Empty;
            DataSet ds = new DataSet(); SqlDataReader sdr = null;
            try
            {

                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 16);

                sdr = AppClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    eMail = "" + sdr.GetValue(0);
                }
                if (eMail != "")
                {
                    SqlCommand cmd1 = new SqlCommand();
                    DataSet ds1 = new DataSet();
                    cmd1.CommandText = "sp_ReceiptConfigSetting";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                    cmd1.Parameters.AddWithValue("@Flag", 2);
                    ds1 = AppClass.GetData(cmd1);

                    string FEmail = eMail;
                    string mailBody = "hii im manoj";
                    SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                    bool IsMailed = BasePage.SendMailWithAttachment(FEmail, "All Reports For your Bussiness", mailBody, true, "Booking Slip.pdf", ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL, renderedBytes);
                    if (IsMailed)
                        lblsummarySucess.Text = "Email send successfully..";
                    else
                        lblsummaryErr.Text = "Email not send..";
                }
                else
                    lblsummarySucess.Text = "Sorry ! Email not found for this branch..";
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
        }
        
    }
}