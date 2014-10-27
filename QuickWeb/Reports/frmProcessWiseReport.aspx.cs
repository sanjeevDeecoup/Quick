using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;

namespace QuickWeb.Reports
{
    public partial class frmProcessWiseReport : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        private ArrayList date = new ArrayList();
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
                drpProcess.Focus();
                BindDrop();
                btnShowReport_Click(null, null);
            }              
        }
        public void BindDrop()
        {
            drpProcess.Items.Clear();

            drpProcess.DataSource = DAL.DALFactory.Instance.DAL_NewChallan.BindDropDown(Globals.BranchID);

            drpProcess.DataTextField = "ProcessCode";
            drpProcess.DataValueField = "ProcessCode";

            drpProcess.DataBind();
            drpProcess.Items.Insert(0, new ListItem("All"));
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
            var dates = SetDuration();
            var processes = SetProcesses();
            ShowReport(dates.Item1, dates.Item2, processes.Item1, processes.Item2);
        }

        private Tuple<string, string> SetDuration()
        {          

            string startDate = string.Empty, endDate = string.Empty;           

            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            startDate = DateFromAndTo[0].Trim();
            endDate = DateFromAndTo[1].Trim();

            return Tuple.Create<string, string>(startDate, endDate);
        }

        /// <summary>
        /// Finds the selected process based on dropdown
        /// </summary>
        /// <returns>Returns a tuple containing "ProcessCode" and "ProcessName", in case of all Process "All" is returned as process name</returns>
        protected Tuple<string, string> SetProcesses()
        {
            if (string.Compare(drpProcess.SelectedItem.Text,"All", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var prcCodes = string.Empty;
                for (var i = 0; i < drpProcess.Items.Count - 1; i++)
                {
                    if (string.Compare(drpProcess.Items[i].Text, "All", StringComparison.OrdinalIgnoreCase) == 0)
                        continue;

                    prcCodes += drpProcess.Items[i].Value + ", ";
                }
                if (prcCodes.IndexOf(", ") != -1)
                    prcCodes = prcCodes.Substring(0, prcCodes.Length - 2);

                return Tuple.Create(prcCodes, "All");
            }
            else
                return Tuple.Create(drpProcess.SelectedItem.Value, drpProcess.SelectedItem.Text);
        }

        # region old
        public void DateWiseAllProcess(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWiseDayByDay";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            grdReport.DataSource = PrjClass.GetData(cmd);
            grdReport.DataBind();
            CalculateGridReport1();
        }
        public void MonthlyReport(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            grdReport.DataSource = PrjClass.GetData(cmd);
            grdReport.DataBind();
            CalculateGridReport();
        }
        public void DateWiseReport(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWiseDateWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@PROCESSNAME", drpProcess.SelectedValue);
            cmd.Parameters.AddWithValue("@PROCESSCODE", drpProcess.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            grdReport.DataSource = PrjClass.GetData(cmd);
            grdReport.DataBind();
            CalculateGridReport();
        }
        public void DayByDayWiseReport(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWiseDateWiseDay";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@PROCESSNAME", drpProcess.SelectedValue);
            cmd.Parameters.AddWithValue("@PROCESSCODE", drpProcess.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            grdReport.DataSource = PrjClass.GetData(cmd);
            grdReport.DataBind();
            CalculateGridReport1();
        }
        # endregion

        public void ShowReport(string fromDate, string uptoDate, string processCode, string processName)
        {
            btnExport.Visible = false;
            var dataSet = BAL.BALFactory.Instance.Bal_Report.GetProcessWiseSummary(new DTO.Report
                                                                                    {
                                                                                        FromDate = fromDate,
                                                                                        UptoDate = uptoDate,
                                                                                        InvoiceNo = "1",
                                                                                        BranchId = Globals.BranchID,
                                                                                        CustCodeStr = processCode
                                                                                    });
            if (dataSet == null)
                return;

            if (dataSet.Tables.Count == 0)
                return;

            Session["PcsSummary_fromDate"] = fromDate;
            Session["PcsSummary_uptoDate"] = uptoDate;
            hdnFromDate.Value = fromDate;
            hdnToDate.Value = uptoDate;
            grdReport.DataSource = dataSet;
            grdReport.DataBind();
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
            AppClass.CalcuateAndSetGridFooter(ref grdReport);
            Session.Remove("PcsSummary_fromDate");
            Session.Remove("PcsSummary_uptoDate");
        }

        private void CalculateGridReport()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                float TotaltemsCount = 0, TotalNetAmount = 0, TotalPcs = 0;
                for (int r = 0; r < rc; r++)
                {
                    TotalNetAmount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text);
                    TotaltemsCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                    TotalPcs += float.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                }
                grdReport.FooterRow.Cells[0].Text = "Total";
                grdReport.FooterRow.Cells[1].Text = TotalNetAmount.ToString();
                grdReport.FooterRow.Cells[2].Text = TotaltemsCount.ToString();
                grdReport.FooterRow.Cells[3].Text = TotalPcs.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void CalculateGridReport1()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                float TotaltemsCount = 0, TotalNetAmount = 0;
                for (int r = 0; r < rc; r++)
                {
                    TotalNetAmount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text);
                    TotaltemsCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                }
                grdReport.FooterRow.Cells[0].Text = "Total";
                grdReport.FooterRow.Cells[1].Text = TotalNetAmount.ToString();
                grdReport.FooterRow.Cells[2].Text = TotaltemsCount.ToString();
            }
            catch (Exception)
            {
            }
        }
        protected void btnExport_Click1(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnFromDate.Value, hdnToDate.Value, "Service Wise Summary Report", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Ob.DatasetMain == null || Ob.DatasetMain.Tables.Count == 0 || Ob.DatasetMain.Tables[0].Rows.Count == 0)
                Ob.DatasetMain = grdReport.DataSource as DataSet;

            if (Ob.DatasetMain == null || Ob.DatasetMain.Tables.Count == 0 || Ob.DatasetMain.Tables[0].Rows.Count == 0)
                return;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((HyperLink)e.Row.Cells[0].FindControl("hplProcessLink")).NavigateUrl =
                    String.Format("~//Reports/frmProcessWiseSummary.aspx?PWC={0},{1},{2},{3}", Ob.DatasetMain.Tables[0].Rows[Ob.Counter]["ProcessCode"], Session["PcsSummary_fromDate"].ToString(), Session["PcsSummary_uptoDate"].ToString(), true);
                Ob.Counter++;
            }
        }
    }
}