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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace QuickWeb.Reports
{
    public partial class BookingHistoryList : System.Web.UI.Page
    {
        ArrayList date = new ArrayList();
        DataSet dsMain;
        int iBkCount, iCounter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["BN"] == null || Request.QueryString["BN"] == "")
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl);
                }
                else
                {
                    string BookNum = Request.QueryString["BN"].ToString();
                    var ds = BAL.BALFactory.Instance.Bal_Report.LoadBookingHistoryForBookingNumber(BookNum, Globals.BranchID, "Booking", "", "");
                    if (ds == null) return;
                    if (ds.Tables.Count == 0) return;
                    if (ds.Tables[0].Rows.Count == 0) return;
                    dsMain = ds;
                    grdHistory.DataSource = ds;
                    grdHistory.DataBind();
                }
            }
        }        

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;

            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdHistory);

            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        protected void grdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            string BookNum = Request.QueryString["BN"].ToString();
            if (e.Row.RowType == DataControlRowType.DataRow && ++iCounter < dsMain.Tables[0].Rows.Count)
            {
                ((HyperLink)e.Row.FindControl("hplNavigate")).NavigateUrl =
                     String.Format("~//Reports/CompareBooking.aspx?BC={0},{1}&BN={2}", dsMain.Tables[0].Rows[iCounter - 1][0].ToString(), dsMain.Tables[0].Rows[iCounter - 0][0].ToString(), BookNum);
                ((HyperLink)e.Row.FindControl("hplNavigate")).Text = "Compare";

                ((Label)e.Row.FindControl("lblRevisionNo")).Text = iCounter.ToString();

                // just hide the last row
                //if ((iCounter+ 1) == dsMain.Tables[0].Rows.Count) e.Row.Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Visible = false;
        }
        protected void grdHistory_DataBinding(object sender, EventArgs e)
        {
            iCounter = 0;
            iBkCount = 0;
        }

        protected void grdHistory_DataBound(object sender, EventArgs e)
        {

        }
    }
}