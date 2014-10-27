using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

public partial class PrintChallanEntry : System.Web.UI.Page
{
    private string ChallanNo = "", Date = "", strChShift = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["CR"] != null && ((Request.QueryString["DirectPrint"] == null)))
            {
                ChallanNo = "" + Request.QueryString["CR"];
                ShowChallanReport(ChallanNo);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Print", "javascript:window.print();javascript:window.close();", true);
            }
            else if (Request.QueryString["DirectPrint"] == null)
            {
                ChallanNo = "" + Request.QueryString["ChallanNo"];
                Date = "" + Request.QueryString["Date"];
                strChShift = "" + Request.QueryString["ShiftName"];
                ShowChallan();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Print", "javascript:window.print();javascript:window.close();", true);
            }
            else if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
            {
                ChallanNo = "" + Request.QueryString["CR"];
                ShowChallanReport(ChallanNo);
                hdnDirectPrint.Value = "true";
            }
            if (Request.QueryString["CloseWindow"] != null && Request.QueryString["CloseWindow"].ToString() == "true")
            {
                hdnCloseWindow.Value = "true";
            }
            if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() != string.Empty)
            {
                hdnRedirectBack.Value = Request.QueryString["RedirectBack"].ToString();
            }
            hdnPrinterName.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
        }
    }

    private void ShowChallan()
    {
        DataTable dt = ((DataTable)Session["BookingListTable"]);
        DataSet ds = new DataSet();
        if (dt.Rows.Count > 0)
        {
            StringBuilder strTable = new StringBuilder();

            string strSNo = "", strBookingDate = "", strProcess = "", strRemarks = "", strDeliverDate = "", strBookingNumber = "", strExtProcess = "", strItemName = "";
            int rowTotal = 0;
            if (dt.Rows.Count > 0)
            {
                strTable.Append("<table cellpadding='0' cellspacing='0' border='1'>");
                strTable.Append("<tr><td style='font-weight:bold;'>Delivery Note No:</td><td colspan='2'>" + ChallanNo + "</td><td style='font-weight:bold;'>Date :</td><td colspan='2'>" + Date + "</td><td style='font-weight:bold;'></td><td colspan='2'>" + "" + "</td></tr>");
                strTable.Append("<tr><td colspan='8' style='height:20px'></td></tr>");
                strTable.Append("<tr><td style='font-weight:bold;'>S.NO.</td><td style='font-weight:bold;'>Order Date</td><td style='font-weight:bold;'>Booking NO</td><td style='font-weight:bold;'>Cloth Name</td><td style='font-weight:bold;'>Service</td><td style='font-weight:bold;'>Extra Service</td><td style='font-weight:bold;'>Remarks</td><td style='font-weight:bold;'>Delivery Date</td></tr>");
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    rowTotal++;
                    ds = GetSomeDetails(dt.Rows[row]["BN"].ToString(), dt.Rows[row]["SN"].ToString());
                    strSNo = rowTotal.ToString();
                    strBookingDate = ds.Tables[0].Rows[0]["BookingDate"].ToString();
                    strBookingNumber = dt.Rows[row]["BN"].ToString();
                    strProcess = dt.Rows[row]["PR"].ToString();
                    strExtProcess = dt.Rows[row]["EP"].ToString();
                    strRemarks = ds.Tables[0].Rows[0]["ItemRemarks"].ToString();
                    strDeliverDate = ds.Tables[0].Rows[0]["DueDate"].ToString();
                    strItemName = dt.Rows[row]["IN"].ToString();
                    strTable.Append("<tr><td style='font-weight:bold;'>" + strSNo + "</td><td>" + strBookingDate + "</td><td>" + strBookingNumber + "</td><td>" + strItemName + "</td><td>" + strProcess + "</td><td>" + strExtProcess + "</td><td>" + strRemarks + "</td><td>" + strDeliverDate + "</td></tr>");
                }
                strTable.Append("</table>");
                litChallan.Text = strTable.ToString();
            }
        }
    }

    private DataSet GetSomeDetails(string BookingNo, string ItemSno)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 27);
        cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
        cmd.Parameters.AddWithValue("@RowIndex", ItemSno);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);
        return ds;
    }

    private DataSet GetChallanReportDetails(string ChallanNo)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "Sp_Report_ChallanReport";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 5);
        cmd.Parameters.AddWithValue("@ChallanNumber", ChallanNo);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);
        return ds;
    }

    private void ShowChallanReport(string ChallanNo)
    {
        DataSet dsMain = GetChallanReportDetails(ChallanNo);

        if (dsMain.Tables[0].Rows.Count > 0)
        {
            StringBuilder strTable = new StringBuilder();

            string strSNo = "", strBookingDate = "", strProcess = "", strRemarks = "", strDeliverDate = "", strBookingNumber = "", strExtProcess = "", strItemName = "";
            int rowTotal = 0;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                strTable.Append("<table cellpadding='0' cellspacing='0' border='1'>");
                strTable.Append("<tr><td style='font-weight:bold;'>Delivery Note No:</td><td colspan='2'>" + ChallanNo + "</td><td style='font-weight:bold;'>Date :</td><td colspan='2'>" + dsMain.Tables[0].Rows[0]["ChallanDate"].ToString() + "</td><td style='font-weight:bold;'></td><td colspan='2'>" + "" + "</td></tr>");
                strTable.Append("<tr><td colspan='8' style='height:20px'></td></tr>");
                strTable.Append("<tr><td style='font-weight:bold;'>S.NO.</td><td style='font-weight:bold;'>Order Date</td><td style='font-weight:bold;'>Booking NO</td><td style='font-weight:bold;'>Cloth Name</td><td style='font-weight:bold;'>Service</td><td style='font-weight:bold;'>Extra Service</td><td style='font-weight:bold;'>Remarks</td><td style='font-weight:bold;'>Delivery Date</td></tr>");
                for (int row = 0; row < dsMain.Tables[0].Rows.Count; row++)
                {
                    rowTotal++;
                    strSNo = rowTotal.ToString();
                    strBookingDate = dsMain.Tables[0].Rows[row]["BookingDate"].ToString();
                    strBookingNumber = dsMain.Tables[0].Rows[row]["BookingNumber"].ToString();
                    strProcess = dsMain.Tables[0].Rows[row]["ProcessType"].ToString();
                    strExtProcess = dsMain.Tables[0].Rows[row]["ExtraProcessType"].ToString();
                    strRemarks = dsMain.Tables[0].Rows[row]["ItemRemarks"].ToString();
                    strDeliverDate = dsMain.Tables[0].Rows[row]["DueDate"].ToString();
                    strItemName = dsMain.Tables[0].Rows[row]["ItemName"].ToString();
                    strTable.Append("<tr><td style='font-weight:bold;'>" + strSNo + "</td><td>" + strBookingDate + "</td><td>" + strBookingNumber + "</td><td>" + strItemName + "</td><td>" + strProcess + "</td><td>" + strExtProcess + "</td><td>" + strRemarks + "</td><td>" + strDeliverDate + "</td></tr>");
                }
                strTable.Append("</table>");
                litChallan.Text = strTable.ToString();
            }
        }
    }
}