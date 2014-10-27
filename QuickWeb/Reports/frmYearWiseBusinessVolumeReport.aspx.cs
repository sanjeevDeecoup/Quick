using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

namespace QuickWeb.Reports
{
    public partial class frmYearWiseBusinessVolumeReport : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        int _prvYear = 0;
        private bool _isYear;
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            // btnRefresh.Click += (o, ev) => hdnRefresh.Value = "true";

            if (!IsPostBack)
            {
                CheckQueryString();
                ShowReport();
                var defView = BAL.BALFactory.Instance.Bal_Report.GetBusinessVolumeDefaultView(Globals.BranchID);
                drpDefView.SelectedIndex = drpDefView.Items.IndexOf(drpDefView.Items.FindByText(defView));
            }           
            
        }

        protected void btnSaveViewSettings_click(object sender, EventArgs e)
        {
            var res = BAL.BALFactory.Instance.Bal_Report.SetBusinessVolumeDefaultView(drpDefView.SelectedItem.Text.Trim(), Globals.BranchID);
            lblStatus.Text = res;
        }

        protected void SetData(string flag, string year, string month)
        {
            Ob.BranchId = Globals.BranchID;

            if (year != null && year != "" && month != null && month != "" & month != "0")
            {
                Ob.InvoiceNo = "3";
                Ob.Year = year;
                Ob.Month = month;
            }
            else if (month != null && month != "")
            {
                Ob.InvoiceNo = "2";
                Ob.Year = year;
            }
            else
            {
                // I don't know what the fuck is "4" used for???
                // Okay, I got it! Its used for a given year, specified in argument or whatever!
                var defView = BAL.BALFactory.Instance.Bal_Report.GetBusinessVolumeDefaultView(Globals.BranchID);
                if (string.IsNullOrEmpty(defView))
                    Ob.InvoiceNo = "1";
                else
                {
                    if (defView == "Year" || defView == "Yearly")
                        Ob.InvoiceNo = "1";
                    else
                        Ob.InvoiceNo = "4";
                }
                Ob.Year = DateTime.Now.Year.ToString();
            }
            Ob.Counter = 0;
            // set the props
        }

        protected void ShowReport()
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            Ob.DatasetMain = BAL.BALFactory.Instance.Bal_Report.GetDurationWiseReport(Ob);
            if (Ob.DatasetMain == null || Ob.DatasetMain.Tables.Count == 0 || Ob.DatasetMain.Tables[0].Rows.Count == 0)                
                return;

            // SetComputedColumn(Ob, grdReport);
            grdReport.DataSource = Ob.DatasetMain;
            grdReport.DataBind();
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
            AppClass.CalcuateAndSetGridFooter(ref grdReport);
        }

        protected void CheckQueryString()
        {
            if (Request.QueryString["Duration"] == null)
            {
                SetData("1", null, null);
                return;
            }

            Ob.StrArray = Request.QueryString["Duration"].ToString().Split(',');
            Ob.Year = Ob.StrArray[1];
            Ob.Month = Ob.StrArray[2];
            if (Ob.Month != null && Ob.Month != "" && Ob.Month != "0")
                Ob.InvoiceNo = "3";
            else
                Ob.InvoiceNo = "2";

            SetData(Ob.InvoiceNo ?? "1", Ob.Year, Ob.Month);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentWithoutDate(grdReport, "Business Summary Report", false);            
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
            // the first one is flag, second is year, third is month
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                # region YearView
                if ((Request.QueryString["Duration"] == null || Request.QueryString["Duration"].ToString() == "")
                    && (Ob.DatasetMain != null || Ob.DatasetMain.Tables.Count != 0 || Ob.DatasetMain.Tables[0].Rows.Count != 0))
                {
                    _isYear = Int32.TryParse(Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString().Substring(2), out _prvYear);
                    if (_isYear)
                    {
                        ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).Text = Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() + "-" + (Int32.Parse(_prvYear.ToString()) + 1).ToString();
                        ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).NavigateUrl =
                        String.Format("~//Reports/frmYearWiseBusinessVolumeReport.aspx?Duration={0},{1},{2},{3}", 2, Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], 0, 0);
                    }
                    else
                    {
                        ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).Text = Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() + "-" + DateTime.Now.Year.ToString();
                        if (Ob.InvoiceNo == "4")
                        {
                            if
                                (
                                Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() == "Jan" ||
                                Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() == "Feb" ||
                                Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() == "Mar"
                                )
                            {
                                ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).NavigateUrl =
                                String.Format("~//Reports/frmYearWiseBusinessVolumeReport.aspx?Duration={0},{1},{2},{3}", 3, (DateTime.Now.Year - 1).ToString(), Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], 0);
                            }
                            else
                            {
                                ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).NavigateUrl =
                                String.Format("~//Reports/frmYearWiseBusinessVolumeReport.aspx?Duration={0},{1},{2},{3}", 3, DateTime.Now.Year.ToString(), Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], 0);
                            }
                        }
                        else
                        {
                            ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).NavigateUrl =
                            String.Format("~//Reports/frmYearWiseBusinessVolumeReport.aspx?Duration={0},{1},{2},{3}", 3, (DateTime.Now.Year - 1).ToString(), Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], 0);
                        }
                    }
                        Ob.Counter++;
                }
                # endregion
                
                else
                {
                    # region MonthView
                    // set the text
                    if (Request.QueryString["Duration"].ToString().Split(',')[2] == "0")
                    {
                        ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).NavigateUrl =
                        String.Format("~//Reports/frmYearWiseBusinessVolumeReport.aspx?Duration={0},{1},{2},{3}", 3, Request.QueryString["Duration"].ToString().Split(',')[1], Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0], 0);
                        // if it is jan, feb, or march its of the next year, not this one, indicate that..
                        if (Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() == "Jan" ||
                                Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() == "Feb" ||
                                Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() == "Mar")
                        {
                            ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).Text =
                               Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() + "-" +
                               (Int32.Parse(Request.QueryString["Duration"].ToString().Split(',')[1]) + 1).ToString();
                        }
                        else
                        {
                            ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).Text =
                               Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() + "-" + Request.QueryString["Duration"].ToString().Split(',')[1].ToString();
                        }
                    }
                    # endregion
                    # region dayView
                    else
                    {

                        // if it is jan, feb, or march its of the next year, not this one, indicate that..
                        if (Request.QueryString["Duration"].Split(',')[2] == "Jan" ||
                                Request.QueryString["Duration"].Split(',')[2] == "Feb" ||
                                Request.QueryString["Duration"].Split(',')[2] == "Mar")
                        {
                            ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).Text =
                          Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() + "-" +
                          Request.QueryString["Duration"].ToString().Split(',')[2].ToString() + "-" +
                          (Int32.Parse(Request.QueryString["Duration"].ToString().Split(',')[1]) + 1).ToString();
                        }
                        else
                        {
                            ((HyperLink)e.Row.Cells[0].FindControl("hplNavigate")).Text =
                          Ob.DatasetMain.Tables[0].Rows[Ob.Counter][0].ToString() + "-" +
                          Request.QueryString["Duration"].ToString().Split(',')[2].ToString() + "-" +
                          Request.QueryString["Duration"].ToString().Split(',')[1].ToString();
                        }
                    }
                    #endregion
                    Ob.Counter++;
                }
            }
            # region setFooter 
            /*
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (Ob.DatasetOther.Tables[0].Rows.Count != 0)
                {
                    e.Row.Cells[0].Text = "Total";
                    e.Row.Cells[1].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][1].ToString();
                    e.Row.Cells[2].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][2].ToString();
                    e.Row.Cells[3].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][3].ToString();
                    e.Row.Cells[4].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][4].ToString();
                    e.Row.Cells[5].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][5].ToString();
                    e.Row.Cells[6].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][6].ToString();
                    e.Row.Cells[7].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][7].ToString();
                    e.Row.Cells[8].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][8].ToString();
                    e.Row.Cells[9].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][9].ToString();
                }
            } */
            # endregion
        }

        protected void SetComputedColumn(DTO.Report Ob, GridView grdvw)
        {
            try
            {
                #region test

                var mainDS = new DataSet();
                mainDS.Tables.Add(Ob.DatasetMain.Tables[0].Copy());
                var len = mainDS.Tables[0].Rows.Count;
                for (var i = 0; i < len;)
                {
                    if (string.IsNullOrEmpty(mainDS.Tables[0].Rows[i]["Duration"].ToString().Trim()))
                    { mainDS.Tables[0].Rows.RemoveAt(i); len--; }
                    else
                        i++;
                }

                #endregion


                Ob.DatatableMain = mainDS.Tables[0].Copy();
                Ob.DatasetMain = mainDS;
                Ob.DatasetOther = new DataSet();
                Ob.DatasetOther.Tables.Add(Ob.DatatableMain);

                if (Ob.DatasetMain.Tables[0].Rows.Count > 0)
                    Ob.DatasetMain.Tables[0].Rows.RemoveAt(Ob.DatasetMain.Tables[0].Rows.Count - 1);
                Ob.Counter = 0;
                grdReport.DataSource = Ob.DatasetMain;
                grdReport.DataBind();
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
            catch (Exception ex)
            { }
        }
    }
}