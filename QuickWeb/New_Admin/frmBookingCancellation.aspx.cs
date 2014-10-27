using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
namespace QuickWeb.New_Admin
{
    public partial class frmBookingCancellation : System.Web.UI.Page
    {
        private static int _AssingId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtReceiptNumber.Focus();
                hdnPrefixForCurrentYear.Value = AppClass.GetPrefixForCurrentYear();
            }

        }

        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            if (txtReceiptNumber.Text == "")
            {
                lblErr.Text = "Please enter valid receipt number.";
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
                lblErr.Text = excp.Message;
                dtvBookingDetails.Visible = false;
                btnCancel.Visible = false;
                idRemakrs.Visible = false;
            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    dtvBookingDetails.DataSource = dsMain.Tables[0];
                    dtvBookingDetails.DataBind();
                    dtvBookingDetails.Visible = true;
                    idRemakrs.Visible = true;
                    txtRemarks.Focus();
                    btnCancel.Visible = true;
                }
            }
            //if (dsMain.Tables.Count > 0)
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                string BookinsStatus = dsMain.Tables[0].Rows[0]["BookingStatus"].ToString();
                if (BookinsStatus == "5")
                {
                    btnCancel.Visible = false;
                    lblError.Text = "Booking already cancelled.";
                }
                else
                {
                    BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtReceiptNumber.Text.Trim().ToUpper(), Globals.UserName, Globals.BranchID, bookingCancelSrcMsg.BookingCancel, ScreenName.CancelBooking,12);                
                }
            }
            else
            {                
                lblError.Text = "No Record found for this Receipt No.";
                txtReceiptNumber.Focus();
                btnCancel.Visible = false;
                idRemakrs.Visible = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            DataSet dsBookingPrefix = new DataSet();
            string strNewBookingNo = string.Empty;
            string strNewBookingPrefix = string.Empty;
            if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckCorrectRemoveReason(Globals.BranchID, txtRemarks.Text) == true)
            {
                string strTransactionDate = DateTime.Today.ToShortDateString();
                string strBookingNumber = dtvBookingDetails.Rows[0].Cells[1].Text;
                dsBookingPrefix =  BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(strBookingNumber);
                strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim();
                strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString().Trim();

                string sql = string.Empty;
                SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
                SqlCommand cmd = new SqlCommand();
                SqlTransaction stx = null;
                SqlDataReader sdr = null;
                float fltCustAmount = 0;
                try
                {
                    sqlcon.Open();
                    stx = sqlcon.BeginTransaction();
                    cmd.Connection = sqlcon;
                    cmd.Transaction = stx;
                    sql = "Update EntBookings Set BookingCancelReason='" + txtRemarks.Text + "', BookingStatus='" + Convert.ToInt32(GStatus.Cancel_Booking) + "', BookingCancelDate=GetDate() Where BookingNumber='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "'  AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update BarcodeTable Set StatusId='" + Convert.ToInt32(GStatus.Cancel_Booking) + "' Where BookingNo='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update EntBookingDetails Set ItemStatus='" + Convert.ToInt32(GStatus.Cancel_Booking) + "' Where BookingNumber='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update EntPackageConsume Set Iscancel='TRUE' Where BookingNumber='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "'  AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update EntPayment Set PaymentMade='0' Where BookingNumber='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "'  AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "DELETE EntChallan Where BookingNumber='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "'  AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Select NetAmount From EntBookings Where BookingNumber='" + strNewBookingNo + "' AND BookingPreFix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        fltCustAmount = float.Parse("0" + sdr.GetValue(0));
                    }
                    else
                    {
                    }
                    sdr.Close();

                    string strCustomerLedgerName = "" + ((Label)dtvBookingDetails.Rows[1].Cells[1].FindControl("lblCustomerCode")).Text;
                    float fltSalesPreBalance = 0, fltSalesPostBalance = 0, fltCustPreBalance = 0, fltCustPostBalance = 0;

                    sql = "Select CurrentBalance From LedgerMaster Where LedgerName='Sales' AND BranchId='" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        string strtmp = sdr.GetValue(0).ToString();
                        fltSalesPreBalance = float.Parse(strtmp);
                    }
                    sdr.Close();

                    sql = "Select CurrentBalance From LedgerMaster Where LedgerName='" + strCustomerLedgerName + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        fltCustPreBalance = float.Parse("" + sdr.GetValue(0));
                    }
                    sdr.Close();

                    fltSalesPostBalance = fltSalesPreBalance + fltCustAmount;
                    fltCustPostBalance = fltCustPreBalance - fltCustAmount;

                    sql = "DELETE FROM EntLedgerEntries WHERE BranchId='" + Globals.BranchID + "' AND rtrim(ltrim(substring(Narration,(charindex('NUMBER',UPPER(Narration))+6),100))) = '" + strBookingNumber + "' AND BookingPreFix='" + strNewBookingPrefix + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    //sql = "Insert into EntLedgerEntries Values(GetDate(),'Sales','By " + strCustomerLedgerName + "','" + fltSalesPreBalance + "','" + fltCustAmount + "','0','" + fltSalesPostBalance + "','On cancellation of booking Number " + strBookingNumber + "','" + Globals.BranchID + "','" + "" + "','" + "" + "','" + Globals.UserName + "','" + res + "')";
                    //cmd.CommandText = sql;
                    //cmd.ExecuteNonQuery();

                    sql = "Update LedgerMaster Set CurrentBalance='" + fltCustPostBalance + "' Where LedgerName='" + strCustomerLedgerName + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update LedgerMaster Set CurrentBalance='" + fltSalesPostBalance + "' Where LedgerName='Sales' AND BranchId='" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    stx.Commit();
                    lblMsg.Text = "Booking Number " + strBookingNumber + " Cancelled.";
                    btnCancel.Visible = false;
                      dtvBookingDetails.Visible = false;
                      idRemakrs.Visible = false;
                      BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(strBookingNumber, Globals.UserName, Globals.BranchID, "Order cancelled for : " + txtRemarks.Text.Trim() + ".", ScreenName.CancelBooking, 12);                
                }
                catch (Exception excp)
                {
                    if (stx != null)
                    {
                        stx.Rollback();
                        stx = null;
                    }
                  //  Session["ReturnMsg"] = "Could not cancel this booking. : " + excp.Message;
                    lblError.Text = "Could not cancel this booking. : " + excp.Message;
                }
                finally
                {
                    sdr.Close();
                    sdr.Dispose();
                    cmd.Dispose();
                    sqlcon.Close();
                }
            }
            else
            {
                //Session["ReturnMsg"] = "Reason not available in pre defined cause list.";
                  lblError.Text = "Reason not available in pre defined cause list.";
                   txtRemarks.Focus();
                   txtRemarks.Attributes.Add("onfocus", "javascript:select();");
            }
        }
    }
}