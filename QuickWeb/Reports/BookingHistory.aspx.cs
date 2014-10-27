using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;

namespace QuickWeb.Reports
{
    public partial class BookingHistory : System.Web.UI.Page
    {       
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;       
        ArrayList date = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtReportFrom.Text = Convert.ToDateTime(date[0].ToString()).ToString("dd MMM yyyy");
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2009; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
                btnShow_Click(null, null);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {           
            GrdEditHistoryBooking.DataSource = null;
            GrdEditHistoryBooking.DataBind();
            DataSet dsEditHistory = new DataSet();
            if (radReportFrom.Checked)
            {
                if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
                DateTime dt = DateTime.Parse(txtReportUpto.Text);
                DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
                DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
                strFromDate = txtReportFrom.Text;
                strToDate = txtReportUpto.Text;        
            }
            else if (radReportMonthly.Checked)
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                strFromDate = dt.ToString("dd MMM yyyy");
                strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");               
            }
            var status = string.Empty;
            if (chkInvoiceNo.Checked)
                status = "BookingNo";
            else
                status = "Date";

            GrdEditHistoryBooking.DataSource = BAL.BALFactory.Instance.Bal_Report.LoadBookingHistoryForBookingNumber(txtBookingNumber.Text.Split('-')[0], Globals.BranchID, status, strFromDate, strToDate);
            GrdEditHistoryBooking.DataBind();
           
            txtBookingNumber.Text = "";
            txtBookingNumber.Focus();
        }

        protected void hypBtnShowDetails_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            string BookingNumber = lnk.Text;
            OpenNewWindow("../Reports/BookingHistoryList.aspx?BN=" + BookingNumber + "");      
        }
        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }
        protected void chkInvoiceNo_CheckedChanged(object sender, EventArgs e)
        {          
            if (chkInvoiceNo.Checked)
            {
                txtBookingNumber.Text = "";
                txtBookingNumber.Visible = true;
                txtBookingNumber.Focus();
            }
            else
                txtBookingNumber.Visible = false;
        }

    }
}