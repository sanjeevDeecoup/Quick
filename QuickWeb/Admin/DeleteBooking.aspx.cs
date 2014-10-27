using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;

namespace QuickWeb.Admin
{
    public partial class DeleteBooking : System.Web.UI.Page
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

            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    dtvBookingDetails.DataSource = dsMain.Tables[0];
                    dtvBookingDetails.DataBind();
                    dtvBookingDetails.Visible = true;
                    btnShowDetails.Visible = true;


                }
                else
                {

                    Session["ReturnMsg"] = "No Record found ";
                    dtvBookingDetails.Visible = false;
                    btnShowDetails.Visible = false;
                    txtReceiptNumber.Focus();

                }
            }
        }


        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            string checktext = string.Empty;
            checktext = "2";
            res = BAL.BALFactory.Instance.BAL_Area.DeleteBooking(Globals.BranchID, txtReceiptNumber.Text);
            //if (txtReceiptNumber.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please enter correct Order No.')", true);
            //    txtReceiptNumber.Focus();
            //    checktext = "1";
            //}
            if (checktext == "2")
            {
                if (res == "Record Saved")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Order deleted Successfully')", true);
                    txtReceiptNumber.Focus();
                    txtReceiptNumber.Text = "";
                    dtvBookingDetails.Visible = false;
                    btnShowDetails.Visible = false;
                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please enter correct Order No.')", true);
                    txtReceiptNumber.Focus();
                }
            }
        }
    }
}