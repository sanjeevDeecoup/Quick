using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


namespace QuickWeb.Admin
{
    public partial class RedoGarment : System.Web.UI.Page
    {
        DataSet ds=new DataSet();
        String TempOrderNo = string.Empty;
        ArrayList date = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        }

        public void BindGrid(string bno)
        {

            ds = BAL.BALFactory.Instance.BAL_City.RedoGarment(Globals.BranchID, bno);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grdReport.DataSource = ds.Tables[0];
                lblCustName.Visible = true;
                lblOrderda.Visible = true;
                lblOrder.Visible = true;
                lblCustomer.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                lblOrderdate.Text = ds.Tables[0].Rows[0]["BookingDate"].ToString();
                lblOrderNo.Text = ds.Tables[0].Rows[0]["BookingNo"].ToString();
                TempOrderNo = lblOrderNo.Text;
                grdReport.DataBind();
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
        BindGrid(txtInvoiceNo.Text);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strIsn = string.Empty;
            string res = string.Empty;
            string process = string.Empty;
            string duedate = string.Empty;
            int TotalQty = 0;
            DataSet Ds_Header = BAL.BALFactory.Instance.BAL_New_Bookings.FillHeaderInfo(Globals.BranchID);
            if (Ds_Header.Tables[0].Rows.Count > 0)
            {
                duedate = (DateTime.Parse(date[0].ToString()).AddDays(Convert.ToInt32(Ds_Header.Tables[0].Rows[0]["DateOffSet"].ToString()))).ToString("dd MMM yyyy");
                
            }

            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                if (((CheckBox)grdReport.Rows[i].FindControl("chkSelect")).Checked)
                {
                    strIsn = grdReport.Rows[i].Cells[1].Text;
                    process = grdReport.Rows[i].Cells[4].Text;
                    TotalQty += 1;
                    res = BAL.BALFactory.Instance.BAL_City.SaveRedo(Globals.BranchID, txtInvoiceNo.Text, Convert.ToInt32(strIsn), process,duedate,TotalQty);
                   
                   
                }
            
            }
            if (res == "Record Saved")
            {
                lblErr.Text = "Record Sucessfully";
                BindGrid(txtInvoiceNo.Text);
            }


            Response.Write(PrjClass.GetAndGo("Booking Accepted. Booking Number is : " + txtInvoiceNo.Text, "../Bookings/RedoSlip.aspx?BN=" + txtInvoiceNo.Text + "-1" + date[0].ToString() + "&DirectPrint=true&RedirectBack=true&closeWindow=true" + "false"));
        }
      
    }
}