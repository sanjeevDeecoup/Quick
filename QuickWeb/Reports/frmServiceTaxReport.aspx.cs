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
    public partial class frmServiceTaxReport : System.Web.UI.Page
    {
        string strFromDate = "", strToDate = "", strGridCap = "";
        ArrayList date = new ArrayList();
        DTO.Report ob = new DTO.Report();
        private string _LabelTax1;
        private string _LabelTax2;
        private string _LabelTax3;
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
            _LabelTax1 = allTax.Split(':')[0];
            _LabelTax2 = allTax.Split(':')[1];
            _LabelTax3 = allTax.Split(':')[2];
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
                var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
                strFromDate = DateFromAndTo[0].Trim();
                strToDate = DateFromAndTo[1].Trim();                             
                ShowBookingDetails(strFromDate, strFromDate);    
            }
            else
            {
                // it is the postback, most probably caused by that drop down, aint nothing else is there to do it as of now..
                // check if its delivery or booking, with is booking parameter as false

                
            }            
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ViewState["exlquery"] = null;
            string strSqlQuery = "";

            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();   
            var isBooking = (drpReportFrom.SelectedItem.Value == "Booking");
            ShowBookingDetails(strFromDate, strToDate, isBooking);
            if (grdReport.Rows.Count > 0)
            {
                AppClass.CalcuateAndSetGridFooter(ref grdReport);
              //  btnExport.Visible = true;
                grdReport.Visible = true;
                ViewState["exlquery"] = strSqlQuery;                
            }            
        }
        private void ShowBookingDetails(string strStartDate, string strToDate, bool bIsBooking = true)
        {
            DataSet ds = new DataSet();
            grdReport.DataSource = null;
            grdReport.DataBind();
            ob.BranchId = Globals.BranchID;
            ob.FromDate = strStartDate;
            ob.UptoDate = strToDate;
            hdnFromDate.Value = ob.FromDate;
            hdnToDate.Value = ob.UptoDate;
            ob.InvoiceNo = (bIsBooking) ? "1" : "2";
            if (drpReportFrom.SelectedItem.Value == "Summarised Booking")
                ob.InvoiceNo = "3";
            else if (drpReportFrom.SelectedItem.Value == "Summarised Delivery")
                ob.InvoiceNo = "4";
            ds = BAL.BALFactory.Instance.Bal_Report.BindServiceTax(ob);
            if (ob.InvoiceNo == "3" || ob.InvoiceNo == "4")
            {
                grdReportSummary.DataSource = ds;
                grdReport.DataSource = null;
                grdReport.Visible = false;
                grdReportSummary.Visible = true;
                
            }
            else
            {
                grdReportSummary.DataSource = null;
                grdReport.DataSource = ds;
                grdReport.Visible = true;
                grdReportSummary.Visible = false;
            }
            grdReportSummary.DataBind();
            grdReport.DataBind();

            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
                if (ds.Tables[0].Rows.Count > 0)
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }


            ds = BAL.BALFactory.Instance.Bal_Report.BindServiceTax(ob);
            string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            if (grdReport.Rows.Count > 0)
            {
                //CalculateGridReport();
                AppClass.CalcuateAndSetGridFooter(ref grdReport);
            }
            else if (grdReportSummary.Rows.Count > 0)
            {
                //CalculateGridReportSummary();
                AppClass.CalcuateAndSetGridFooter(ref grdReportSummary, 0, drpReportFrom.SelectedItem.Value == "Summarised Booking" ? 1 : 2);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ob.InvoiceNo == "1")
                    ReportViewer1.LocalReport.ReportPath = "RDLC/BookingServiceTax.rdlc";
                else
                    if(ob.InvoiceNo=="2")
                    ReportViewer1.LocalReport.ReportPath = "RDLC/DeliveryServiceTax.rdlc";
                    else
                        if(ob.InvoiceNo=="3")
                            ReportViewer1.LocalReport.ReportPath = "RDLC/SumBookingServiceTax.rdlc";
                else
                            ReportViewer1.LocalReport.ReportPath = "RDLC/SumDeliveryServiceTax.rdlc";
                ReportDataSource rds = new ReportDataSource();
                ReportParameter[] parameters = new ReportParameter[7];
                parameters[0] = new ReportParameter("UserName", Globals.UserName);
                parameters[1] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
                parameters[2] = new ReportParameter("FromDate", ob.FromDate);
                parameters[3] = new ReportParameter("ToDate", ob.UptoDate);
                parameters[4] = new ReportParameter("Tax1", _LabelTax1);
                parameters[5] = new ReportParameter("Tax2", _LabelTax2);
                parameters[6] = new ReportParameter("Tax3", _LabelTax3);

                //parameters[2] = new ReportParameter("Link", str);
                ReportViewer1.LocalReport.SetParameters(parameters);
                rds.Name = "DataSet1";
                rds.Value = ds.Tables[0];
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
            }
        }

        private void CalculateGridReportSummary()
        { /*
            try
            {

                int rc = grdReportSummary.Rows.Count;
                int cc = grdReportSummary.Columns.Count;
                var SumList = new List<int>();
                var value = 0.0;
                for (int c = 0; c < cc; c++)
                {
                    for (int r = 0; r < rc; r++)
                    {
                        if (float.TryParse(grdReportSummary.Rows[r].Cells[c].Text, out value)
                    }
                }
            }
            catch (Exception ex) { }
           */

            try
            {
                var FltTotals = new List<float>();
                var dict = new Dictionary<int, string>();
                var FltTotalsCounter = 0f;
                var floatValue = 0.0f;
                var str = string.Empty;
                var rc = grdReportSummary.Rows.Count;
                var cc = grdReportSummary.Columns.Count;

                for (int j = 1; j < cc; j++)
                {
                    FltTotalsCounter = 0;
                    for (int i = 0; i < rc; i++)
                    {
                        if (float.TryParse((grdReportSummary.Rows[i].Cells[j].Text), out floatValue))
                            FltTotalsCounter += floatValue;
                        else
                        {
                            dict.Add(j, string.Empty);
                            break;
                        }
                    }
                    FltTotals.Add((float)Math.Round(FltTotalsCounter, 2));
                }

                // return ds;

                if (rc != 0)
                {
                    for (int j = 1; j < cc; j++)
                    {
                        if (dict.ContainsKey(j))
                            grdReportSummary.FooterRow.Cells[j].Text = dict[j];
                        else
                            grdReportSummary.FooterRow.Cells[j].Text = Math.Round((FltTotals.ElementAt((j - 1))), 2).ToString();
                    }
                    if (drpReportFrom.SelectedItem.Value == "Summarised Booking")
                    {
                        //grdReportSummary.FooterRow.Cells[1].Text = "Total : " + rc.ToString();
                    }
                    else if (drpReportFrom.SelectedItem.Value == "Summarised Delivery")
                    {
                        //grdReportSummary.FooterRow.Cells[2].Text = "Total : " + rc.ToString();
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
            }
        }
        private void CalculateGridReport()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, Tax = 0, Cess = 0, SHECess = 0, Balance = 0, AdjustedAmt = 0, Payment = 0, BalanceDue = 0, NetAmt=0;
                for (int r = 0; r < rc; r++)
                {
                    OrderCount++;
                    if (grdReport.Rows[r].Cells[3].Text.Trim().ToUpper() != "CANCELLED")
                    {
                        TotalCostCount += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[4].Text), 2).ToString());
                        TotalPaid += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[5].Text), 2).ToString());
                        TotalDue += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[6].Text), 2).ToString());
                        Tax += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[7].Text), 2).ToString());
                        Balance += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[8].Text), 2).ToString());
                        Cess += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[9].Text), 2).ToString());
                        SHECess += float.Parse(grdReport.Rows[r].Cells[10].Text);
                        AdjustedAmt += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[11].Text), 2).ToString());
                        Payment += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[12].Text), 2).ToString());
                        BalanceDue += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[13].Text), 2).ToString());
                        NetAmt += float.Parse(Math.Round(double.Parse("0" + grdReport.Rows[r].Cells[14].Text), 2).ToString());
                    }
                }
                grdReport.FooterRow.Cells[0].Text = OrderCount.ToString();
                grdReport.FooterRow.Cells[1].Text = "Total";
                grdReport.FooterRow.Cells[4].Text = TotalCostCount.ToString();
                grdReport.FooterRow.Cells[5].Text = TotalPaid.ToString();
                grdReport.FooterRow.Cells[6].Text = TotalDue.ToString();
                grdReport.FooterRow.Cells[7].Text = Tax.ToString();
                grdReport.FooterRow.Cells[8].Text = Balance.ToString();
                grdReport.FooterRow.Cells[9].Text = Cess.ToString();
                grdReport.FooterRow.Cells[10].Text = Math.Round(SHECess,2).ToString();
                grdReport.FooterRow.Cells[11].Text = AdjustedAmt.ToString();
                grdReport.FooterRow.Cells[12].Text = Payment.ToString();
                grdReport.FooterRow.Cells[13].Text = BalanceDue.ToString();
                grdReport.FooterRow.Cells[14].Text = NetAmt.ToString();              
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
            var list = new List<int>();
            
            if (grdReport.Rows.Count > 0)
            {
                for (int i = 0; i < grdReport.Columns.Count; i++)
                {
                    if (grdReport.Columns[i].Visible == false)
                        list.Add(i);
                }
            }
            else
            {
                list.Add(0);
                for (int i = 1; i < grdReportSummary.Columns.Count; i++)
                {
                    if (grdReportSummary.Columns[i].Visible == false)
                        list.Add(i);
                }
            }
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridHiddenColumn(grdReport.Rows.Count > 0 ? grdReport : grdReportSummary, hdnFromDate.Value, hdnToDate.Value, "Service Tax Report - " + drpReportFrom.SelectedItem, false, list);
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
            // Mohit Chauhan
            // set the headers dynamically
            if (strPrinterName == "")
            {
                strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
            }
            if (drpReportFrom.SelectedItem.Text == "Booking" || drpReportFrom.SelectedItem.Text == "Delivery")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
                    string strBookinNo = hypBookingNo.Text;
                    Label lblDueDate = (Label)e.Row.FindControl("lblDate");
                    string strDuedate = lblDueDate.Text;
                    hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (drpReportFrom.SelectedItem.Text == "Booking" || drpReportFrom.SelectedItem.Text == "Delivery")
                {
                    // the tax col are 5 (main tax), 6 (second tax), 7 (third tax)
                    e.Row.Cells[10].Text = _LabelTax1;
                    e.Row.Cells[11].Text = _LabelTax2;
                    e.Row.Cells[12].Text = _LabelTax3;
                }
                else
                {
                    e.Row.Cells[9].Text = _LabelTax1;
                    e.Row.Cells[10].Text = _LabelTax2;
                    e.Row.Cells[11].Text = _LabelTax3;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.BackColor = System.Drawing.Color.GreenYellow;
            }           
        }

        protected void drpReportFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnShowReport_Click(null, null);
        }

        protected void grdReport_DataBinding(object sender, EventArgs e)
        {
            if (drpReportFrom.SelectedItem.Value == "Booking")
            {
                // grdReport.Columns[0].Visible = true;
                // grdReport.Columns[1].Visible = true;
                // grdReport.Columns[2].Visible = true;
                // grdReport.Columns[3].Visible = true;
                grdReport.Columns[4].Visible = true;
                grdReport.Columns[5].Visible = true;
                grdReport.Columns[6].Visible = true;
                grdReport.Columns[7].Visible = true;
                grdReport.Columns[8].Visible = false;
                grdReport.Columns[9].Visible = false;
                grdReport.Columns[10].Visible = true;
                grdReport.Columns[11].Visible = true;
                grdReport.Columns[12].Visible = true;
                grdReport.Columns[13].Visible = false;
                grdReport.Columns[14].Visible = false;
                grdReport.Columns[15].Visible = false;
                grdReport.Columns[16].Visible = true;
            }
            else if (drpReportFrom.SelectedItem.Value == "Delivery")
            {
                // grdReport.Columns[0].Visible = true;
                // grdReport.Columns[1].Visible = true;
                // grdReport.Columns[2].Visible = true;
                // grdReport.Columns[3].Visible = true;
                grdReport.Columns[4].Visible = false;
                grdReport.Columns[5].Visible = false;
                grdReport.Columns[6].Visible = false;
                grdReport.Columns[7].Visible = false;
                grdReport.Columns[8].Visible = true;
                grdReport.Columns[9].Visible = true;
                grdReport.Columns[10].Visible = true;
                grdReport.Columns[11].Visible = true;
                grdReport.Columns[12].Visible = true;
                grdReport.Columns[13].Visible = false;
                grdReport.Columns[14].Visible = false;
                grdReport.Columns[15].Visible = false;
                grdReport.Columns[16].Visible = false;
            }
            else if (drpReportFrom.SelectedItem.Value == "Summarised Booking")
            {
                grdReportSummary.Columns[0].Visible = false;
                grdReportSummary.Columns[1].Visible = true;
                grdReportSummary.Columns[2].Visible = false;
                grdReportSummary.Columns[3].Visible = true;
                grdReportSummary.Columns[4].Visible = true;
                grdReportSummary.Columns[5].Visible = true;
                grdReportSummary.Columns[6].Visible = false;
                grdReportSummary.Columns[7].Visible = false;
                grdReportSummary.Columns[8].Visible = true;
                
            }
            else
            {
                grdReportSummary.Columns[0].Visible = false;
                grdReportSummary.Columns[1].Visible = false;
                grdReportSummary.Columns[2].Visible = true;
                grdReportSummary.Columns[3].Visible = false;
                grdReportSummary.Columns[4].Visible = false;
                grdReportSummary.Columns[5].Visible = false;
                grdReportSummary.Columns[6].Visible = true;
                grdReportSummary.Columns[7].Visible = true;
                grdReportSummary.Columns[8].Visible = false;
               
            }
        }

        protected void grdReport_DataBound(object sender, EventArgs e)
        {

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
        "  <MarginTop>0.2in</MarginTop>" +
        "  <MarginLeft>0.7in</MarginLeft>" +
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
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(renderedBytes);
            //Response.End();
        }

       
    }
}