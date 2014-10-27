using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Data;
using System.Text;

namespace QuickWeb.Reports
{
    public partial class SearchByInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtInvoiceNo.Focus();
            }           
        }
        string[] Bookingno;
        string strPrinterName = string.Empty;
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            DetailsViewDeliverSlip.Visible = false;
            ArrayList date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            try
            {
                Bookingno = txtInvoiceNo.Text.Split('-');
                txtInvoiceNo.Text = Bookingno[0].ToString();
            }
            catch (Exception ex)
            {
                
            }
            if (BAL.BALFactory.Instance.BAL_sms.CheckValidBookingNo(Globals.BranchID, txtInvoiceNo.Text) == true)
            {
                if (BAL.BALFactory.Instance.BAL_sms.CheckDeliverSlipViewRight(Globals.BranchID, txtInvoiceNo.Text, Globals.UserType) == true)
                {
                    strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
                    OpenNewWindow("../" + strPrinterName + "/BookingSlip.aspx?BN=" + txtInvoiceNo.Text + "-1" + date[0].ToString() + "&RS=" + "0" + "&DirectPrint=false&CloseWindow=false");
                    txtInvoiceNo.Text = "";
                    txtInvoiceNo.Focus();
                }
                else
                {
                    DataSet dsMain = new DataSet();
                    dsMain = BAL.BALFactory.Instance.BAL_City.DeliveryDetail(Globals.BranchID, txtInvoiceNo.Text);
                    DetailsViewDeliverSlip.DataSource = dsMain.Tables[0];
                    DetailsViewDeliverSlip.DataBind();
                    DetailsViewDeliverSlip.Visible = true;
                    lblMsg.Text = "Delivered";
                    txtInvoiceNo.Focus();
                    txtInvoiceNo.Attributes.Add("onfocus", "javascript:select();");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Please enter a valid order no.";
                DetailsViewDeliverSlip.Visible = false;
                txtInvoiceNo.Focus();
                txtInvoiceNo.Attributes.Add("onfocus", "javascript:select();");
            }
        }

        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }    
    }
}