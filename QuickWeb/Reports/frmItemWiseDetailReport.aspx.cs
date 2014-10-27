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
    public partial class frmItemWiseDetailReport : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["PWC"] != null || Request.QueryString["PWC"].ToString().Length != 0)
                ShowReport();

            return;
        }     
        protected void ShowReport()
        {
            SetData();
            Ob.DatasetMain = BAL.BALFactory.Instance.Bal_Report.GetItemWiseSummary(Ob);
            grdReport.DataSource = Ob.DatasetMain;
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
            //SetCoputedDataSet(Ob, grdReport);
            AppClass.CalcuateAndSetGridFooter(ref grdReport);          
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
                grdReport.DataSource = Ob.DatasetMain;
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
            }
            catch (Exception ex)
            { }
        }

        protected void SetData()
        {
            Ob.StrArray = Request.QueryString["PWC"].ToString().Split(',');
            // the string code
            Ob.StrCodes = Ob.StrArray[0];
            Ob.FromDate = Ob.StrArray[1];
            Ob.UptoDate = Ob.StrArray[2];
            Ob.InvoiceNo = "1";
            hdnStartDate.Value = Ob.FromDate;
            hdnEndDate.Value = Ob.UptoDate;
            // if its for multiple
            if (Ob.StrCodes.IndexOf("-") > -1)
                Ob.InvoiceNo = "2";
            // for the bk Num
            Ob.StartBkNum = Ob.StrArray[4];
            Ob.EndBkNum = Ob.StrArray[5];
            Ob.BranchId = Globals.BranchID;
            lblDescription.Text = Ob.StrCodes;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnStartDate.Value, hdnEndDate.Value, "Item Wise Order Summary Report - " + lblDescription.Text, false);
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
            try
            {
                if (strPrinterName == "")
                {
                    strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
                }
                if (Request.QueryString[0].Split(',')[0].IndexOf('-') == -1)
                    e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
                    string strBookinNo = hypBookingNo.Text;
                    HiddenField lblDueDate = (HiddenField)e.Row.FindControl("hdnDeliveryDate");
                    string strDuedate = lblDueDate.Value; 
                    hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (Ob.DatasetOther.Tables[0].Rows.Count != 0)
                    {
                        e.Row.Cells[0].Text = "Total";
                        e.Row.Cells[4].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][4].ToString();
                        e.Row.Cells[5].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][5].ToString();
                    }
                }                
            }
            catch (Exception ex)
            {

            }
        }
    }
}