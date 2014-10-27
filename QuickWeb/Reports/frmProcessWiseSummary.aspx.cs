
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.IO;



namespace QuickWeb.Reports
{
    public partial class frmProcessWiseSummary : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["PWC"] != null || Request.QueryString["PWC"].ToString().Length != 0)
                ShowReport();

            return;
        }

        // [Conditional("max")]
        protected void ShowReport()
        {
            SetData();
            Ob.DatasetMain = BAL.BALFactory.Instance.Bal_Report.GetProcessWiseSummary(Ob);
            grdReport.DataSource = Ob.DatasetMain;
            grdReport.DataBind();
            AppClass.CalcuateAndSetGridFooter(ref grdReport);
            //SetCoputedDataSet(Ob, grdReport);
          
            btnExportExcel.Visible = grdReport.Rows.Count > 1;
            if (grdReport.Rows.Count > 0)
            {
                blnRight = AppClass.CheckExportToExcelRightOnPage();
                if (blnRight)
                {
                    btnExportExcel.Visible = true;
                }
                else
                {
                    btnExportExcel.Visible = false;
                }
            }
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
            }
            catch (Exception ex)
            { }
        }

        protected void SetData()
        {
            Ob.StrArray = Request.QueryString["PWC"].ToString().Split(',');
            // the string code
            Ob.StrCodes = Ob.StrArray[0];
            Ob.CustCodeStr = Ob.StrArray[0];
            Ob.FromDate = Ob.StrArray[1];
            Ob.UptoDate = Ob.StrArray[2];
            Ob.InvoiceNo = "2";
            Ob.BranchId = Globals.BranchID;
            hdnFromDate.Value = Ob.FromDate;
            hdnToDate.Value = Ob.UptoDate;
            lblDescription.Text = Ob.StrCodes;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnFromDate.Value, hdnToDate.Value, "Service Wise Order Summary Report - " + lblDescription.Text, false);
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
                HiddenField lblDueDate = (HiddenField)e.Row.FindControl("hdnDeliveryDate");
                string strDuedate = lblDueDate.Value;                
                hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
            }
        }
    }
}