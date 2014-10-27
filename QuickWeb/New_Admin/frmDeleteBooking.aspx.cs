using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;


namespace QuickWeb.New_Admin
{
    public partial class frmDeleteBooking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtReceiptNumber.Focus();
            }

        }

        protected void btnSDetails_Click(object sender, EventArgs e)
        {
            if (txtReceiptNumber.Text == "")
            {
                lblError.Text = "Please enter valid receipt number.";
                return;
            }
            string sql = string.Empty;
            SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_RecordsForBookingCancellation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("BookingNumber", txtReceiptNumber.Text.Trim()));
            cmd.Parameters.Add(new SqlParameter("BranchId", Globals.BranchID));
            SqlDataAdapter sadp = new SqlDataAdapter();
            DataSet dsMain = new DataSet();

            try
            {
                sqlcon.Open();
                cmd.Connection = sqlcon;
                sadp.SelectCommand = cmd;
                sadp.Fill(dsMain);
            }
            catch (Exception excp)
            {
                lblError.Text = excp.Message;
                dtvBookingDetails.Visible = false;
                pnlShowDetails.Visible = false;

            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    dtvBookingDetails.DataSource = dsMain.Tables[0];
                    dtvBookingDetails.DataBind();
                    dtvBookingDetails.Visible = true;
                    btnShowDetails.Visible = true;
                    pnlShowDetails.Visible = true;
                    BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtReceiptNumber.Text.Trim().ToUpper(), Globals.UserName, Globals.BranchID, bookingCancelSrcMsg.BookingDelete,ScreenName.DeleteBooking,13);
                }
                else
                {
                    lblError.Text = "Order No " + txtReceiptNumber.Text + " Not Found.";
                    dtvBookingDetails.Visible = false;
                    btnShowDetails.Visible = false;
                    txtReceiptNumber.Focus();
                    pnlShowDetails.Visible = false;
                }
            }
        }


        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            res = BAL.BALFactory.Instance.BAL_Area.DeleteBooking(Globals.BranchID, txtReceiptNumber.Text);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Order deleted Successfully";
                BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtReceiptNumber.Text.Trim().ToUpper(), Globals.UserName, Globals.BranchID, bookingCancelSrcMsg.BookingDeleted, ScreenName.DeleteBooking,13);
                BAL.BALFactory.Instance.BAL_New_Bookings.updateInvoiceHistoryDeleteData(txtReceiptNumber.Text.Trim().ToUpper(), Globals.BranchID);
                txtReceiptNumber.Focus();
                txtReceiptNumber.Text = "";
                dtvBookingDetails.Visible = false;
                btnShowDetails.Visible = false;
                pnlShowDetails.Visible = false;
            }
            else
            {
                lblError.Text = "Please enter correct Order No.";
                txtReceiptNumber.Focus();
            }
        }
      
    }
}