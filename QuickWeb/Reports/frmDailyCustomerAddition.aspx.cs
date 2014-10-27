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
using System.Collections;

namespace QuickWeb.Reports
{
   
    public partial class frmDailyCustomerAddition : System.Web.UI.Page
    {
        static DTO.Report Ob = new DTO.Report();
        static DataTable dataTableGridSource;
        float TotBookingAmount, TotalAmount;
        double Percent;
        public static string _CurrencyType = string.Empty;
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
             if (!IsPostBack)
            {
                SetCurrenyType();
                ArrayList date = new ArrayList();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString(); 

                setValues(date[0].ToString().Trim(), date[0].ToString().Trim());
                btnShowReport_Click(sender, e);
                setValues(date[0].ToString().Trim(), date[0].ToString().Trim());
            }        

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
                    _CurrencyType = "" + sdr.GetValue(0).ToString();
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
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
                grdReport.Visible = true;                
            }
            else
            {
                btnExport.Visible = false;
            }
        }

        protected void checkDate()
        {        
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
            grdReport.DataBind();
            Ob.InvoiceNo = "1";
            Ob.CustId = "0";
            Ob.StrCodes = "";
            Ob.LstCodes = new List<string>();
            BindAndCalculate();          
            lblTotalBooking.Text = grdReport.Rows.Count.ToString();
            if (grdReport.Rows.Count > 0)
            {
                lblTotalAmount.Text = grdReport.FooterRow.Cells[2].Text.ToString();
            }
            else
            {
                lblTotalAmount.Text = "0";
            }
            lblTotBuseAmt.Text = TotBookingAmount.ToString(); 
            lblPercent.Text = Percent.ToString();
           
       
        }

        private void BindAndCalculate()
        {
            Ob.DatasetMain = BAL.BALFactory.Instance.Bal_Report.GetDailyCustomerAdditionReport(Ob);
            grdReport.DataSource = Ob.DatasetMain;
            dataTableGridSource = Ob.DatasetMain.Tables[0];
            Ob.DataViewMain = new DataView(Ob.DatasetMain.Tables[0]);
            ViewState["DataSourceCalc"] = Ob.DatasetMain.Tables[0];
            grdReport.DataBind();
            CalculatePercent();
            AppClass.CalcuateAndSetGridFooter(ref grdReport, 0, 0);
        }

        private void CalculatePercent()
        {
            try
            {
                TotBookingAmount = 0;
                TotalAmount = 0;
                Percent = 0;
                if (Ob.DatasetMain.Tables[2].Rows.Count > 0)
                {
                    TotBookingAmount = Convert.ToInt64(Ob.DatasetMain.Tables[2].Compute("SUM(volume)", string.Empty));
                    if (Ob.DatasetMain.Tables[1].Rows.Count > 0)
                    {
                        TotalAmount = Convert.ToInt64(Ob.DatasetMain.Tables[1].Compute("SUM(volume)", string.Empty));
                    }
                    if (TotBookingAmount > 0)
                    {
                        Percent = Math.Round(((TotalAmount * 100) / TotBookingAmount), 2);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Daily Customer Addition", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }
     
        protected void BindGridCustomerSearch(string CustName)
        {           

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
            try
            {
               
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                   if (ViewState["DataSourceCalc"] != null && Ob.DataViewMain != null && (Ob.FromDate != null || Ob.FromDate != "") && (Ob.UptoDate != null || Ob.UptoDate != ""))
                    {
                        ((HyperLink)e.Row.Cells[2].FindControl("hplNavigate")).NavigateUrl =
                                 String.Format("~//Reports/BookingByCustomerReport.aspx?BC={0},{1},{2},{3}", Ob.DataViewMain[Ob.Counter][0], Ob.DataViewMain.Table.Rows[Ob.Counter]["FirstBill"].ToString(), Ob.DataViewMain.Table.Rows[Ob.Counter]["LastBill"].ToString(), true);
                        
                        Ob.Counter++;
                    }
                    else if (Ob.DatasetMain != null && Ob.DatasetMain.Tables.Count != 0 && Ob.DatasetMain.Tables[0] != null && Ob.DatasetMain.Tables[0].Rows.Count != 0)
                    {
                        if ((Ob.FromDate != null || Ob.FromDate != "") && (Ob.UptoDate != null || Ob.UptoDate != ""))
                        {

                            ((HyperLink)e.Row.Cells[2].FindControl("hplNavigate")).NavigateUrl =
                            String.Format("~//Reports/BookingByCustomerReport.aspx?BC={0},{1},{2},{3}", Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], Ob.DatasetMain.Tables[0].Rows[Ob.Counter]["FirstBill"].ToString(), Ob.DatasetMain.Tables[0].Rows[Ob.Counter]["LastBill"].ToString(), true);                           
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
                   
                }
            }
            # endregion
            catch (Exception ex)
            { }
        }

        protected void grdReport_Sorted(object sender, EventArgs e)
        {
            grdReport.Visible = true;           
        }

        protected void grdReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
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
    }
}