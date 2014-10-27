using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_BookingCancellation : System.Web.UI.Page
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
        if (dtvBookingDetails.Rows.Count > 0)
        {
            if (dtvBookingDetails.Rows.Count < 7)
            {
                throw new Exception("Could not get booking details.");
            }
            int BookinsStatus = int.Parse("0" + dtvBookingDetails.Rows[6].Cells[1].Text.Replace("&nbsp;", ""));
            if (BookinsStatus == 5)
            {
                btnCancel.Visible = false;
                Session["ReturnMsg"] = "Booking already cancelled.";
            }
        }
        else
        {
            Session["ReturnMsg"] = "No Record found for this Receipt No.";
            txtReceiptNumber.Focus();
            btnCancel.Visible = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand();
        SqlTransaction stx = null;
        SqlDataReader sdr = null;
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
        if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckCorrectRemoveReason(Globals.BranchID, txtRemarks.Text) == true)
        {
            string strTransactionDate = DateTime.Today.ToShortDateString();
            string strBookingNumber = dtvBookingDetails.Rows[0].Cells[1].Text;
            string sql = string.Empty;

            float fltCustAmount = 0;
            try
            {
                sqlcon.Open();
                stx = sqlcon.BeginTransaction();
                cmd.Connection = sqlcon;
                cmd.Transaction = stx;
                sql = "Update EntBookings Set BookingCancelReason='" + txtRemarks.Text + "', BookingStatus='" + Convert.ToInt32(GStatus.Cancel_Booking) + "', BookingCancelDate=GetDate() Where BookingNumber='" + strBookingNumber + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update BarcodeTable Set StatusId='" + Convert.ToInt32(GStatus.Cancel_Booking) + "' Where BookingNo='" + strBookingNumber + "'  AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update EntBookingDetails Set ItemStatus='" + Convert.ToInt32(GStatus.Cancel_Booking) + "' Where BookingNumber='" + strBookingNumber + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update EntPackageConsume Set Iscancel='TRUE' Where BookingNumber='" + strBookingNumber + "'  AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "DELETE EntPayment Where PaymentMade<>0 AND BookingNumber='" + strBookingNumber + "'  AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "DELETE EntChallan Where BookingNumber='" + strBookingNumber + "'  AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Select NetAmount From EntBookings Where BookingNumber='" + txtReceiptNumber.Text.Trim() + "' AND BranchId = '" + Globals.BranchID + "'";
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

                sql = "DELETE FROM EntLedgerEntries WHERE BranchId='" + Globals.BranchID + "' AND rtrim(ltrim(substring(Narration,(charindex('NUMBER',UPPER(Narration))+6),100))) = '" + txtReceiptNumber.Text.Trim() + "'";
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
            }
            catch (Exception excp)
            {
                if (stx != null)
                {
                    stx.Rollback();
                    stx = null;
                }
                Session["ReturnMsg"] = "Could not cancel this booking. : " + excp.Message;
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
            Session["ReturnMsg"] = "Reason not available in pre defined cause list.";
            txtRemarks.Focus();
            txtRemarks.Attributes.Add("onfocus", "javascript:select();");
        }
    }
}