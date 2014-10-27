using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace QuickWeb.Reports
{
    public partial class PackageReportDetails : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Request.QueryString["CustId"] != null || Request.QueryString["CustId"].ToString().Length != 0) && (Request.QueryString["PkgId"] != null || Request.QueryString["PkgId"].ToString().Length != 0))
                ShowReport();

            return;
        }

        // [Conditional("max")]
        protected void ShowReport()
        {
            SetData();
            Ob.DatasetMain = BAL.BALFactory.Instance.Bal_Report.GetPackageReportDetail(Ob);
            if (Ob.DatasetMain == null) return;
            if (Ob.DatasetMain.Tables.Count == 0) return;
            /*
            grdReport.DataSource = Ob.DatasetMain;
            grdReport.DataBind();*/
            SetCoputedDataSet(Ob, grdReport);
            /*
            lblCaption1.Text = ReportCaptions.MainCaption;
            lblCaption2.Text = ReportCaptions.ItemWiseSummary;
            */
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

        protected void SetData()
        {
            
            Ob.InvoiceNo = "24";
            // for the bk Num
            Ob.BranchId = Globals.BranchID;
            Ob.CustId = Request.QueryString["CustId"].ToString();
            Ob.StrCodes = Request.QueryString["PkgId"].ToString();
            //Task.Factory.StartNew(() => { lblCustName.Text = BAL.BALFactory.Instance.BL_CustomerMaster.GetCustNameFromCode(Ob.CustId, Ob.BranchId); });
            lblCustName.Text = BAL.BALFactory.Instance.BL_CustomerMaster.GetCustNameFromCode(Ob.CustId, Ob.BranchId);
            //lblDescription.Text = Ob.StrCodes;
        }

        

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingWithoutDate(grdReport, "Package Details Report", false);
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
                if (Request.QueryString["Type"].ToString() == "Discount")
                {
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[6].Visible = false;
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[2].Text = "Order Date";
                        e.Row.Cells[3].Text = "Order Amount";
                        e.Row.Cells[4].Text = "Discount Amount";
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    
                    if (Ob.DatasetOther.Tables[0].Rows.Count != 0)
                    {
                        e.Row.Cells[0].Text = "Total";
                        if (Request.QueryString["Type"].ToString() == "Discount")
                        {
                            e.Row.Cells[3].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][1].ToString();
                            e.Row.Cells[4].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][4].ToString();
                        }
                        //e.Row.Cells[6].Text = Ob.DatasetOther.Tables[0].Rows[Ob.DatasetOther.Tables[0].Rows.Count - 1][6].ToString();
                    }
                }
                DataRowView drview = e.Row.DataItem as DataRowView;
                if (strPrinterName == "")
                {
                    strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
                    string strBookinNo = hypBookingNo.Text;
                    HiddenField lblDueDate = (HiddenField)e.Row.FindControl("hdnDate");
                    string strDuedate = lblDueDate.Value;
                    hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
                }
            }
            catch (Exception ex) { }
        }
    }
}