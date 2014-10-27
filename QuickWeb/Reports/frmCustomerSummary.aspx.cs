using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections;

namespace QuickWeb.Reports
{
    public partial class frmCustomerSummary : System.Web.UI.Page
    {
        static DTO.Report Ob = new DTO.Report();
        static DataTable dataTableGridSource;
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //SqlSourceItems.SelectCommand = "SELECT [ItemID], [ItemName] FROM [ItemMaster] Where BranchId='" + Globals.BranchID + "' ORDER BY ItemName";
                //SqlSourceItems.DataBind();

                hdnDateFromAndTo.Value = PrjClass.GetFromAndToDateOfCurrentYear();
                var date = hdnDateFromAndTo.Value.Split('-');
                //drpMonthList.SelectedIndex = 0;
                //drpYearList.Items.Clear();
                //for (int i = 2010; i <= 2050; i++)
                //{
                //    drpYearList.Items.Add(i.ToString());
                //}
                //drpYearList.SelectedIndex = DateTime.Today.Year - 2010;
                dataTableGridSource = null;
                //btnShowReport_Click(null, null);
                //txtReportFrom.Text = DateTime.Today.ToString("dd MMM yyyy");
                //txtReportUpto.Text = DateTime.Today.ToString("dd MMM yyyy");
                //setValues(txtReportFrom.Text, txtReportUpto.Text);
                setValues(date[0].ToString().Trim(), date[1].ToString().Trim());
                btnShowReport_Click(sender, e);
                setValues(date[0].ToString().Trim(), date[1].ToString().Trim());
                txtCustomerName.Focus();
            }
          //  lblCaption1.Text = ReportCaptions.MainCaption;
            lblCaption2.Text = ReportCaptions.CustomerWise;
           // radReportFrom.Text = ReportLabels.From;
          //  radReportMonthly.Text = ReportLabels.MonthlyReport;
         //   lblCustomer.Text = ReportLabels.CustomerName;
            lblReportDesc.Text = ReportDescriptions.CustomerWise;
            binddrpsms();
            binddrpdefaultsms();
        }
        private void binddrpsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.ShowAll(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpsmstemplate.DataSource = ds.Tables[0];
                drpsmstemplate.DataTextField = "template";
                drpsmstemplate.DataValueField = "smsid";
                drpsmstemplate.DataBind();
            }
        }

        private void binddrpdefaultsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
            PrjClass.SetItemInDropDown(drpsmstemplate, ds.Tables[4].Rows[0]["Template"].ToString(), true, false);
        }
        public void setValues(string fromDate, string uptoDate)
        {
            Ob.BranchId = Globals.BranchID;
            Ob.FromDate = fromDate;
            Ob.UptoDate = uptoDate;
            Ob.Counter = 0;
            Ob.DatasetMain = null;
            Ob.LstCodes = new List<string>();
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ViewState["exlquery"] = null;
            ViewState["DataSource"] = null;
            ViewState["DataSourceCalc"] = null;
            checkDate();
            hdnFromCustomer.Value = "false";
            hdnPrevCodes.Value = "";
            ShowBookingDetails(Ob.FromDate, Ob.UptoDate);
            //checkCancelBooking(strSqlQuery);
            if (grdReport.Rows.Count > 0)
            {
                //CalculateGridReport();
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
            btnShowReport.Attributes.Add("style", "display:none");
        }

        protected void checkDate()
        {
            //if (radReportMonthly.Checked)
            //{
            //    DateTime dt = DateTime.Today;

            //    if (drpMonthList.SelectedItem.Text == "All")
            //    {
            //        dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), 1, 1);
            //        Ob.FromDate = dt.ToString("dd MMM yyyy");
            //        Ob.UptoDate = dt.AddYears(+1).AddDays(-1).ToString("dd MMM yyyy");

            //    }
            //    else
            //    {
            //        dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            //        Ob.FromDate = dt.ToString("dd MMM yyyy");
            //        Ob.UptoDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            //    }
            //    Ob.GrdCaption = "Booking Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
            //}
            //else if (radReportFrom.Checked)
            //{
            //    if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            //    DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            //    DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            //    Ob.FromDate = txtReportFrom.Text;
            //    Ob.UptoDate = txtReportUpto.Text;
            //    Ob.GrdCaption = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
            //}

            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            Ob.FromDate = DateFromAndTo[0].Trim();
            Ob.UptoDate = DateFromAndTo[1].Trim();
            Ob.GrdCaption = "Booking Report from " + Ob.FromDate + " to " + Ob.UptoDate;

            hdnStartDate.Value = Ob.FromDate;
            hdnEndDate.Value = Ob.UptoDate;
            setValues(Ob.FromDate, Ob.UptoDate);
        }

        private void ShowBookingDetails(string strStartDate, string strToDate)
        {
            Ob.Counter = 0;
            grdReport.DataSource = null;
            //SqlGridSource.SelectCommand = "sp_customersummary";
            grdReport.DataBind();
            // set the flag to 1
            Ob.InvoiceNo = "1";
            Ob.CustId = "0";
            Ob.StrCodes = "";
            Ob.LstCodes = new List<string>();
            BindAndCalculate();
            //SetCoputedDataSet(Ob, grdReport);
        }

        private void BindAndCalculate()
        {
            Ob.DatasetMain = BAL.BALFactory.Instance.Bal_Report.GetCustomerWiseSummary(Ob);
            grdReport.DataSource = Ob.DatasetMain;
            dataTableGridSource = Ob.DatasetMain.Tables[0];
            Ob.DataViewMain = new DataView(Ob.DatasetMain.Tables[0]);
            ViewState["DataSourceCalc"] = Ob.DatasetMain.Tables[0];
            grdReport.DataBind();
            AppClass.CalcuateAndSetGridFooter(ref grdReport, 0, 2);
            if (grdReport.Rows.Count > 0)
            {
                grdReport.FooterRow.Cells[4].Text = "";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport,hdnStartDate.Value,hdnEndDate.Value,"Accounts Receivable Report", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

            try
            {
                Ob.Counter = 0;
                grdReport.DataSource = null;
                grdReport.DataBind();
                hdnFromCustomer.Value = "true";
                checkDate();
                if (hdnFromCustomer.Value == "true" && hdnPrevCodes.Value != "")
                {
                    Ob.LstCodes = new List<string>();
                    Ob.LstCodes.Add(txtCustomerName.Text.Split('-')[0].Trim());
                    Ob.LstCodes.AddRange(hdnPrevCodes.Value.Split(','));
                    Ob.StrCodes = MakeStringOfList(Ob.LstCodes);
                    Ob.CustId = "";
                    Ob.InvoiceNo = "3";
                }
                else
                {
                    Ob.InvoiceNo = "2";
                    Ob.CustId = txtCustomerName.Text.Split('-')[0].ToString().Trim();
                    Ob.StrCodes = "";
                }
                BindAndCalculate();
                //SetCoputedDataSet(Ob, grdReport);
                txtCustomerName.Text = "";
                //ViewState["DataSource"] = null;
                //ViewState["DataSourceCalc"] = null;
                txtCustomerName.Focus();
                hdnFromCustomer.Value = "true";
                Ob.StrArray = hdnPrevCodes.Value.Split(',');
                for (int i = 0; i < Ob.StrArray.Count(); i++)
                {
                    if (!Ob.StrArray.Contains(Ob.DatasetMain.Tables[0].Rows[i][0].ToString()))
                        hdnPrevCodes.Value = hdnPrevCodes.Value + "," + Ob.DatasetMain.Tables[0].Rows[i][0].ToString();
                }


            }
            catch (Exception)
            { }
        }

        protected void BindGridCustomerSearch(string CustName)
        {
            //SqlCommand cmd = new SqlCommand();
            //DataSet ds = new DataSet();
            //cmd.CommandText = "sp_AccountEntries";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@CustCode", CustName);
            //cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            //ds = AppClass.GetData(cmd);

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Ob.LstCodes = null;
            hdnCustCodes.Value = "";
            hdnFromCustomer.Value = "false";
            hdnPrevCodes.Value = "";
            txtCustomerName.Text = "";
            ViewState["DataSource"] = null;
            ViewState["DataSourceCalc"] = null;
            btnShowReport_Click(sender, e);
        }

        protected void btnCompare_Click(object sender, EventArgs e)
        {
            //if ((hdnFromCustomer.Value == "false" && hdnCustCodes.Value == "") || (hdnFromCustomer.Value == "true" && hdnPrevCodes.Value == ""))
            //    return;

            checkDate();
            Ob.LstCodes = null;

            // find all the selected custcodes
            Ob.StrArray = FindSelectedCustCodes(grdReport);
            hdnFromCustomer.Value = "false";
            hdnPrevCodes.Value = "";
            ViewState["DataSource"] = null;
            ViewState["DataSourceCalc"] = null;


            //if (hdnFromCustomer.Value == "true")
            //{
            //    if (hdnPrevCodes.Value.Split(',').Count() > 1)
            //    {
            //        lst = hdnPrevCodes.Value.Split(',');
            //    }
            //}
            //else if (hdnCustCodes.Value != "")
            //{
            //    hdnPrevCodes.Value = "";
            //    lst = hdnCustCodes.Value.Split(',');
            //}
            Ob.StrCodes = MakeStringOfList(Ob.StrArray);
            Ob.CustId = "";
            Ob.InvoiceNo = "3";
            Ob.Counter = 0;
            grdReport.DataSource = null;
            grdReport.DataBind();
            BindAndCalculate();
            btnShowReport.Attributes.Add("style", "display:inline");
            //SetCoputedDataSet(Ob, grdReport);
            hdnFromCustomer.Value = "false";
            return;
        }

        protected string MakeStringOfList(ICollection<string> arg)
        {
            if (arg == null) return "";
            if (arg.Count == 0) return "";
            Ob.StrBuilder = new StringBuilder();
            for (int i = 0; i < arg.Count; i++)
            {
                if (arg.ElementAt(i) == "") continue;
                Ob.StrBuilder.Append(arg.ElementAt(i) + ", ");
            }
            if (Ob.StrBuilder.Length > 2)
                return Ob.StrBuilder.ToString().Substring(0, Ob.StrBuilder.Length - 2);
            else
                return "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Text = "";
            hdnPrevCodes.Value = "";
            hdnFromCustomer.Value = "false";
            Ob.Counter = 0;
            ViewState["DataSource"] = null;
            ViewState["DataSourceCalc"] = null;
            grdReport.DataSource = null;
            grdReport.DataBind();
        }

        protected string[] FindSelectedCustCodes(GridView grdView)
        {
            Ob.LstCodes = new List<string>();
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                if (((CheckBox)grdView.Rows[i].FindControl("chkSelect")).Checked)
                    Ob.LstCodes.Add(((Label)grdView.Rows[i].FindControl("lblCustCodes")).Text);
            }
            return Ob.LstCodes.ToArray();
        }

        protected string[] FindAllCustCodes(GridView grdView)
        {
            Ob.LstCodes = new List<string>();
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                Ob.LstCodes.Add(((Label)grdView.Rows[i].FindControl("lblCustCodes")).Text);
            }
            return Ob.LstCodes.ToArray();
        }

        protected void SetCoputedDataSet(DTO.Report Ob, GridView grdvw)
        {
            try
            {
                Ob.DatatableMain = Ob.DatasetMain.Tables[0].Copy();
                Ob.DatasetOther = new DataSet();
                Ob.DatasetOther.Tables.Add(Ob.DatatableMain);
                if (Ob.DatasetMain.Tables[0].Rows.Count > 0)
                    Ob.DatasetMain.Tables[0].Rows.RemoveAt(Ob.DatasetMain.Tables[0].Rows.Count - 1);
                Ob.Counter = 0;

                ViewState["DataSource"] = Ob.DatasetMain.Tables[0];
                ViewState["DataSourceCalc"] = Ob.DatasetMain.Tables[0];
                DataView view = new DataView(Ob.DatasetMain.Tables[0]);
                Ob.DataViewMain = view;
                grdReport.DataSource = Ob.DatasetMain;
                grdReport.DataBind();
            }
            catch (Exception ex)
            { }
        }
       
        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            # region try
            string reporttype = string.Empty;
            reporttype = "AccountsReceivable";
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (ViewState["DataSourceCalc"] != null && Ob.DataViewMain != null && (Ob.FromDate != null || Ob.FromDate != "") && (Ob.UptoDate != null || Ob.UptoDate != ""))
                    {
                        ((HyperLink)e.Row.Cells[2].FindControl("hplNavigate")).NavigateUrl =
                               //String.Format("~//Reports/BookingByCustomerReport.aspx?BC={0},{1},{2},{3}", Ob.DataViewMain[Ob.Counter][0], Ob.FromDate, Ob.UptoDate, radReportFrom.Checked);
                        String.Format("~//Reports/BookingByCustomerReport.aspx?BC={0},{1},{2},{3},{4}", Ob.DataViewMain[Ob.Counter][0], Ob.FromDate, Ob.UptoDate, true, reporttype);

                        //Ob.LstCodes.Add(Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString());
                        Ob.Counter++;
                    }
                    else if (Ob.DatasetMain != null && Ob.DatasetMain.Tables.Count != 0 && Ob.DatasetMain.Tables[0] != null && Ob.DatasetMain.Tables[0].Rows.Count != 0)
                    {
                        if ((Ob.FromDate != null || Ob.FromDate != "") && (Ob.UptoDate != null || Ob.UptoDate != ""))
                        {

                            ((HyperLink)e.Row.Cells[2].FindControl("hplNavigate")).NavigateUrl =
                                String.Format("~//Reports/BookingByCustomerReport.aspx?BC={0},{1},{2},{3},{4}", Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], Ob.FromDate, Ob.UptoDate, true, reporttype);
                                //String.Format("~//Reports/BookingByCustomerReport.aspx?BC={0},{1},{2},{3}", Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], Ob.FromDate, Ob.UptoDate, radReportFrom.Checked);

                            //  Ob.LstCodes.Add(Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString());
                            Ob.Counter++;
                        }
                    }
                    if (hdnFromCustomer.Value == "true")
                    {
                        ((CheckBox)e.Row.Cells[0].FindControl("chkSelect")).Checked = true;
                    }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    /*
                    if (Ob.DatasetOther.Tables[0].Rows.Count != 0)
                    {
                        e.Row.Cells[2].Text = "Total";
                        e.Row.Cells[3].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][2].ToString();
                        e.Row.Cells[4].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][3].ToString();
                        e.Row.Cells[5].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][6].ToString();
                        e.Row.Cells[6].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][7].ToString();
                        //ViewState["DataSource"] = Ob.DatasetMain.Tables[0];
                        ViewState["DataSourceCalc"] = Ob.DatasetMain.Tables[0];
                    }*/
                }
            }
            # endregion
            catch (Exception ex)
            { }
        }

        protected void grdReport_Sorted(object sender, EventArgs e)
        {
            grdReport.Visible = true;
            //            ShowBookingDetails("20-Aug-2012", "25-Aug-2012");
        }

        protected void grdReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                // DataTable sourceTable = grdReport.DataSource as DataTable;
                var sourceTable = dataTableGridSource;

                if (sourceTable == null)
                    if (ViewState["DataSource"] != null)
                        sourceTable = ViewState["DataSource"] as DataTable;

                checkDate();
                DataView view = new DataView(sourceTable);
                Ob.DataViewMain = view;
                if (ViewState["sortExpression"] == null)
                    ViewState["sortExpression"] = e.SortExpression + " " + "ASC";


                string[] sortData = ViewState["sortExpression"].ToString().Trim().Split(' ');
                if (e.SortExpression == sortData[0])
                {
                    if (sortData[1] == "ASC")
                    {
                        view.Sort = e.SortExpression + " " + "DESC";
                        this.ViewState["sortExpression"] = e.SortExpression + " " + "DESC";
                    }
                    else
                    {
                        view.Sort = e.SortExpression + " " + "ASC";
                        this.ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
                    }
                }
                else
                {
                    view.Sort = e.SortExpression + " " + "ASC";
                    this.ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
                }
                grdReport.DataSource = null;
                grdReport.DataBind();
                grdReport.DataSource = view;
                grdReport.DataBind();
                AppClass.CalcuateAndSetGridFooter(ref grdReport, 0, 2);               
            }
            catch (Exception ex)
            {
            }
        }

        protected void radReportFrom_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["DataSource"] = null;
            ViewState["DataSourceCalc"] = null;
        }

        protected void radReportMonthly_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["DataSource"] = null;
            ViewState["DataSourceCalc"] = null;

        }
        protected void btnSms_Click(object sender, EventArgs e)
        {
            AppClass.SendPendingAmountSMS(grdReport, Globals.BranchID, drpsmstemplate);           
        }
    }
}