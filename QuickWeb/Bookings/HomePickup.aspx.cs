using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Bookings
{
    public partial class HomePickup : System.Web.UI.Page
    {
        private DTO.Report Ob = new DTO.Report();
        private string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        private ArrayList date = new ArrayList();
        public static StringWriter sw;
        public static string strAllContents = string.Empty;
        private DTO.PackageMaster Obp = new DTO.PackageMaster();

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
                DAL.DALFactory.Instance.DAL_Report.AndroidFirst();
                DAL.DALFactory.Instance.DAL_Report.AndroidSecond();
                DAL.DALFactory.Instance.DAL_Report.AndroidThird();
                txtReportFrom.Text = date[0].ToString();
                txtReportUpto.Text = date[0].ToString();
                strFromDate = txtReportFrom.Text + " 00:00:00";
                ShowBookingDetails(strFromDate, strFromDate);
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2000; i <= 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "googlechart();", true);
            }
            DTO.Report.BFlag = false;

            if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                HttpContext.Current.Items.Add("IsPrintingForMany", "false");
            else
                HttpContext.Current.Items["IsPrintingForMany"] = "false";

            hdnDTOReportsBFlag.Value = "false";
        }

        protected void SetDTOFalse()
        {
            //DTO.Report.BFlag = false;
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
                    strFromDate = txtReportFrom.Text + " 00:00:00";
                    strToDate = txtReportUpto.Text + " 00:00:00";
                    strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
                }
                else if (radReportMonthly.Checked)
                {
                    DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                    strFromDate = dt.ToString("dd MMM yyyy");
                    strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                    strGridCap = "Booking Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
                }

                hdnStartDate.Value = strFromDate;
                hdnEndDate.Value = strToDate;
                ShowBookingDetails(strFromDate, strToDate);
            }
            else
            {
                ShowBookingDetails(strFromDate, strToDate);
            }
            checkCancelBooking();

            if (grdReport.Rows.Count > 0)
            {
                CalculateGridReport();
                btnExport.Visible = true;
                btnPrint.Visible = true;
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
            ObMain.HNumber = txtHomePickup.Text;
            if (chkHomePickup.Checked)
                ObMain.Description = "6";
            else if (chkInvoice.Checked)
                ObMain.Description = "9";
            else if (chkCustomer.Checked)
                ObMain.Description = "7";
            else
                ObMain.Description = "8";
            DataSet dsMain = new DataSet();
            //dsMain.Clear();
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetDataMainReportwithHome(ObMain);
            try
            {
                if (chkHomePickup.Checked)
                {
                    grdReport.DataSource = dsMain.Tables[1];
                    grdReport.DataBind();
                    CalculateGridReport();
                }
                else
                {
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
                    CalculateGridReport();
                }

                //if (dsMain.Tables.Count > 0)
                //{
                //    if (dsMain.Tables[0].Rows.Count > 0)
                //    {
                //        if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                //        {
                //            throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                //        }
                //        grdReport.DataSource = dsMain.Tables[0];
                //        grdReport.DataBind();
                //        DTO.Report.BFlag = false;

                //        if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                //            HttpContext.Current.Items.Add("IsPrintingForMany", "false");
                //        else
                //            HttpContext.Current.Items["IsPrintingForMany"] = "false";

                //        hdnDTOReportsBFlag.Value = "false";
                //        ViewState["SavedDS"] = dsMain;
                //        checkCancelBooking();
                //        CalculateGridReport();
                //    }
                //}
            }
            catch (Exception excp)
            {
                lblMsg.Text = "Error : " + excp.Message;
            }
            finally
            {
            }
        }

        private void CalculateGridReport()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                float Paid = 0, St = 0, Ad = 0, Bal = 0, OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0, deldis = 0;
                for (int r = 0; r < rc; r++)
                {
                    OrderCount++;
                    if (grdReport.Rows[r].Cells[3].Text.Trim().ToUpper() != "CANCELLED")
                    {
                        TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[13].Text);
                        TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[7].Text);
                        St += float.Parse("0" + grdReport.Rows[r].Cells[8].Text);
                        Ad += float.Parse("0" + grdReport.Rows[r].Cells[10].Text);
                        Bal += float.Parse("0" + grdReport.Rows[r].Cells[11].Text);
                        Paid += float.Parse("0" + grdReport.Rows[r].Cells[12].Text);
                        BalanceAmount += float.Parse("0" + grdReport.Rows[r].Cells[9].Text);
                        deldis += float.Parse("0" + grdReport.Rows[r].Cells[14].Text);
                        TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[15].Text);
                    }
                }
                grdReport.FooterRow.Cells[2].Text = OrderCount.ToString();
                grdReport.FooterRow.Cells[6].Text = "";
                grdReport.FooterRow.Cells[7].Text = TotalDue.ToString();
                grdReport.FooterRow.Cells[9].Text = BalanceAmount.ToString();
                grdReport.FooterRow.Cells[8].Text = St.ToString();
                grdReport.FooterRow.Cells[10].Text = Ad.ToString();
                grdReport.FooterRow.Cells[11].Text = Bal.ToString();
                grdReport.FooterRow.Cells[12].Text = Paid.ToString();
                grdReport.FooterRow.Cells[13].Text = TotalPaid.ToString();
                grdReport.FooterRow.Cells[14].Text = deldis.ToString();
                grdReport.FooterRow.Cells[15].Text = TotalCostCount.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "googlechart();", true);
            }
            catch (Exception ex)
            { }
        }

        private void checkCancelBooking()
        {
            try
            {
                string strStat = "";
                for (int r = 0; r < grdReport.Rows.Count; r++)
                {
                    strStat = "" + ((HiddenField)grdReport.Rows[r].FindControl("hidBookingStatus")).Value;
                    if (strStat == "5")
                    {
                        grdReport.Rows[r].BackColor = System.Drawing.Color.Yellow;
                        grdReport.Rows[r].Cells[3].Text = "Cancelled";
                        grdReport.Rows[r].Cells[4].Text = "0";
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            DTO.Report.BFlag = false;

            if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                HttpContext.Current.Items.Add("IsPrintingForMany", "false");
            else
                HttpContext.Current.Items["IsPrintingForMany"] = "false";

            hdnDTOReportsBFlag.Value = "false";
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
            chkCustomer.Checked = false;
            txtCustomerName.Visible = false;
            chkHomePickup.Checked = false;
            txtHomePickup.Visible = false;
            if (chkInvoice.Checked)
            {
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Visible = true;
                txtInvoiceNo.Focus();
            }
            else
                txtInvoiceNo.Visible = false;
        }

        protected void chkHomePickup_CheckedChanged(object sender, EventArgs e)
        {
            chkInvoice.Checked = false;
            txtInvoiceNo.Visible = false;
            chkCustomer.Checked = false;
            txtCustomerName.Visible = false;
            if (chkHomePickup.Checked)
            {
                txtHomePickup.Text = "";
                txtHomePickup.Visible = true;
                txtHomePickup.Focus();
            }
            else
                txtHomePickup.Visible = false;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            int rowindex = 1;
            if (chkCustomer.Checked)
                rowindex = 1;
            else
                rowindex = 0;
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

            BasePage.OpenWindow(this.Page, "../Reports/ListBooking.aspx");
            btnShowReport_Click(null, null);
            DTO.Report.BFlag = false;
            hdnDTOReportsBFlag.Value = "false";
            for (int i = 0; i < Ob.StrArray.Count(); i++)
            {
                this.Form.Target = "_blank";
            }
        }

        protected void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            chkInvoice.Checked = false;
            txtInvoiceNo.Visible = false;
            chkHomePickup.Checked = false;
            txtHomePickup.Visible = false;
            if (chkCustomer.Checked)
            {
                txtCustomerName.Text = "";
                txtCustomerName.Visible = true;
                txtCustomerName.Focus();
            }
            else
                txtCustomerName.Visible = false;
        }

        private string[] customerName;

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                customerName = txtCustomerName.Text.Split('-');
                lblCustomerCode.Text = customerName[0].ToString().Trim();
                txtCustomerName.Text = customerName[1].ToString().Trim();
                Obp.BranchId = Globals.BranchID;
                Obp.CustomerCode = lblCustomerCode.Text;
                if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalCustomer(Obp) != true)
                {
                    lblMsg.Text = "Please enter valid customer.";
                    txtCustomerName.Text = "";
                    txtCustomerName.Focus();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Please enter valid customer.";
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
            }
        }

        protected void btnPrintStore_Click(object sender, EventArgs e)
        {
            storeprint();
        }

        public void storeprint()
        {
            int rowindex = 1;
            if (chkCustomer.Checked)
                rowindex = 1;
            else
                rowindex = 0;
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

            BasePage.OpenWindow(this.Page, "../Reports/frmStoreCopyPrint.aspx");
            btnShowReport_Click(null, null);
            DTO.Report.BFlag = false;
            hdnDTOReportsBFlag.Value = "false";
            for (int i = 0; i < Ob.StrArray.Count(); i++)
            {
                this.Form.Target = "_blank";
            }
        }
    }
}