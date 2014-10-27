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
using System.Text;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace QuickWeb.New_Admin
{
    public partial class frmBookingHistory : System.Web.UI.Page
    {
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        ArrayList date = new ArrayList();    
        DataSet dsMain;
        int iBkCount, iCounter;
        DTO.Report Ob = new DTO.Report();
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {                      
                //date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);              
                //hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
                hdnDateFromAndTo.Value = PrjClass.GetFromAndToDateOfCurrentMonth();
                var date = hdnDateFromAndTo.Value.Split('-');
                btnShow_Click(null, null);
            }            
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            GrdEditHistoryBooking.DataSource = null;
            string bookingnumber = string.Empty, branchid = string.Empty;
            var status = string.Empty;    
            GrdEditHistoryBooking.DataBind();

            if (txtBookingNumber.Text.Trim() != "")
            {
                string[] Bookingno = txtBookingNumber.Text.Split('-');
                status = "BookingNo";
                if (Bookingno.Length == 1)
                {
                    bookingnumber = txtBookingNumber.Text.Split('-')[0];
                    branchid = Globals.BranchID;
                }
                else if (Bookingno.Length == 3)
                {
                    bookingnumber = txtBookingNumber.Text.Split('-')[0];
                    branchid = txtBookingNumber.Text.Split('-')[2];
                }
                else if (Bookingno.Length == 2)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblMsg.Text = "Please enter a valid order no.";
                    txtBookingNumber.Focus();
                    txtBookingNumber.Attributes.Add("onfocus", "javascript:select();");
                    return;
                }
            }
            else
            {
                status = "Date";
                branchid = Globals.BranchID;
            }

            grdHistory.DataSource = null;
            grdHistory.DataBind();
            DataSet dsEditHistory = new DataSet();           

            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();                
            

            DataSet dsEditHistoy = new DataSet();
            dsEditHistoy = BAL.BALFactory.Instance.Bal_Report.LoadBookingHistoryForBookingNumber(bookingnumber, branchid, status, strFromDate, strToDate);

            if (dsEditHistoy.Tables[0].Rows.Count > 0)
            {
                GrdEditHistoryBooking.DataSource = dsEditHistoy;
                GrdEditHistoryBooking.DataBind();
            }
            else
            {
                if (txtBookingNumber.Text.Trim() != "")
                {
                    DataSet dsInvoiceData = BAL.BALFactory.Instance.Bal_Report.GetInvoiceHistoeyDetails(bookingnumber, branchid, "True", "-1","");
                    if (dsInvoiceData.Tables[0].Rows.Count > 0)
                    {
                        GrdEditHistoryBooking.DataSource = dsInvoiceData;
                        GrdEditHistoryBooking.DataBind();
                        foreach (GridViewRow row in GrdEditHistoryBooking.Rows)
                        {
                          //  row.Cells[2].Visible = false;
                            LinkButton lnkButton = (LinkButton)row.FindControl("hypBtnShowDetails");
                            lnkButton.Enabled = false;
                        }
                    }
                }
            }


            txtBookingNumber.Text = "";
            txtBookingNumber.Focus();

            grdInvoiceHistory.Visible = false;
            divScreenName.Visible = false;
            if (GrdEditHistoryBooking.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Order not available.";
                txtBookingNumber.Focus();
                txtBookingNumber.Attributes.Add("onfocus", "javascript:select();");
            }
        }

        protected void hypBtnShowDetails_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            //string BookingNumber = lnk.Text;

            LinkButton ddl = (LinkButton)sender;
            GridViewRow gv = ((GridViewRow)ddl.NamingContainer);
            int row = gv.RowIndex;
            string BookingNumber = GrdEditHistoryBooking.Rows[row].Cells[0].Text;
            lblOrderName.Visible = true;
            hdnBookingNo.Value = BookingNumber;
            lblOrderNo.Text = BookingNumber;
           // OpenNewWindow("../Reports/BookingHistoryList.aspx?BN=" + BookingNumber + "");

            //string BookNum = Request.QueryString["BN"].ToString();
           // var ds = BAL.BALFactory.Instance.Bal_Report.LoadBookingHistoryForBookingNumber(BookNum, Globals.BranchID, "Booking", "", "");
            var ds = BAL.BALFactory.Instance.Bal_Report.LoadBookingHistoryForBookingNumber(BookingNumber, Globals.BranchID, "Booking", "", "");
            if (ds == null) return;
            if (ds.Tables.Count == 0) return;
            if (ds.Tables[0].Rows.Count == 0) return;
            dsMain = ds;
            grdHistory.DataSource = ds;
            grdHistory.DataBind();
            grdHistory.Visible = true;
            grdInvoiceHistory.Visible = false;
            divScreenName.Visible = false;
        }


        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
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
           // string BookNum = Request.QueryString["BN"].ToString();
            string BookNum = hdnBookingNo.Value;
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
        protected void txtUserID_TextChanged(object sender, EventArgs e)
        {
            Ob.UserID = txtUserID.Text;
            Ob.BranchId=Globals.BranchID;
            if (txtUserID.Text != "")
            {
                if (BAL.BALFactory.Instance.Bal_Report.CheckOrginalUser(Ob) != true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblMsg.Text = "Please select a valid User ID";
                    txtUserID.Text = "";
                }
                ShowInvoiceHistryDetails(hdnBookingNo.Value, drpStatus.SelectedItem.Value, txtUserID.Text); ;
                txtUserID.Focus();
            }
            else
            {
                ShowInvoiceHistryDetails(hdnBookingNo.Value, drpStatus.SelectedItem.Value, txtUserID.Text); ;
            }
        }
        protected void grdInvoiceHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{               
            //    // e.Row.Cells[0].Text = "<img src='../img/Edit.png' width='15px' height='15px' />"; 

            //}
        }
        protected void hypBtnShowHistoryDetails_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            LinkButton ddl = (LinkButton)sender;
            GridViewRow gv = ((GridViewRow)ddl.NamingContainer);
            int row = gv.RowIndex;
            string BookingNumber = GrdEditHistoryBooking.Rows[row].Cells[0].Text;
            hdnBookingNo.Value = BookingNumber;
            lblOrderName.Visible = true;
            lblOrderNo.Text = BookingNumber;
            ShowInvoiceHistryDetails(BookingNumber, "0", txtUserID.Text);
            drpStatus.SelectedIndex = 0;
        }
        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowInvoiceHistryDetails(hdnBookingNo.Value, drpStatus.SelectedItem.Value, txtUserID.Text); ;
        }

        private void ShowInvoiceHistryDetails(string BookingNumber, string ScreenID, string UserName)
        {
            var ds = BAL.BALFactory.Instance.Bal_Report.GetInvoiceHistoeyDetails(BookingNumber.ToUpper(), Globals.BranchID, "False", ScreenID, UserName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dsMain = ds;
                grdInvoiceHistory.DataSource = ds;
                grdInvoiceHistory.DataBind();
                SetIcons();
            }
            else
            {
                grdInvoiceHistory.DataSource = null;
                grdInvoiceHistory.DataBind();
            }
            grdHistory.Visible = false;
            grdInvoiceHistory.Visible = true;
            divScreenName.Visible = true;
        }

        private void SetIcons()
        {
            foreach (GridViewRow row in grdInvoiceHistory.Rows)
            {
                if (row.Cells[5].Text.Trim().Contains(bookingSrcMsg.BookingCreation.Trim()))
                {
                    row.Cells[0].Text = "<i class='fa fa-list-ol' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains(bookingSrcMsg.BookingScrSearch.Trim()))
                {
                    row.Cells[0].Text = "<i class='fa fa-pencil-square-o' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains(bookingSrcMsg.BookingEditRight.Trim()))
                {
                   row.Cells[0].Text = "<i class='fa fa-pencil-square-o' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Customer modified"))
                {
                   row.Cells[0].Text = "<i class='fa fa-user' style='color:#FF8040'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order status changed to Urgent."))
                {
                    row.Cells[0].Text = "<i class='fa fa-check' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order status changed from Urgent to regular."))
                {
                    row.Cells[0].Text = "<i class='fa fa-check' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("New garment added to order"))
                {
                    row.Cells[0].Text = "<i class='fa fa-plus' style='color:Green'></i>";
                }
                else if ((row.Cells[5].Text.Trim().Contains("Garment deleted from order")) || (row.Cells[5].Text.Trim().Contains(bookingCancelSrcMsg.BookingDelete)))
                {
                    row.Cells[0].Text = "<i class='fa fa-trash-o' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garment details modified"))
                {
                    row.Cells[0].Text = "<i class='fa fa-pencil-square-o' style='color:#FF8040'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Discount modified"))
                {
                    row.Cells[0].Text = "<i class='fa fa-bell-slash' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Advance value modified"))
                {
                    row.Cells[0].Text = "<i class='fa fa-money' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order delivery point marked to home delivery"))
                {
                    row.Cells[0].Text = "<i class='fa fa-building' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order delivery point changed from home delivery to store pick up"))
                {
                   row.Cells[0].Text = "<i class='fa fa-building' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Oder notes edited"))
                {
                    row.Cells[0].Text = "<i class='fa fa-pencil' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Workshop notes edited"))
                {
                    row.Cells[0].Text = "<i class='fa fa-clipboard' style='color:Gray'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garment inspected by user changed"))
                {
                    row.Cells[0].Text = "<i class='fa fa-user-md' style='color:Gray'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Due Date edited"))
                {
                    row.Cells[0].Text = "<i class='fa fa-calendar' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Due Time edited"))
                {
                   row.Cells[0].Text = "<i class='fa fa-clock-o' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments returned unprocessed"))
                {
                    row.Cells[0].Text = "<i class='fa fa-unlink' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments shipped to workshop for processing"))
                {
                    row.Cells[0].Text = "<i class='fa fa-forward' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments sent for finishing"))
                {
                    row.Cells[0].Text = "<i class='fa fa-backward' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments marked Ready to be Delivered"))
                {
                    row.Cells[0].Text = "<i class='fa fa-check' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains(DeliverySrcMsg.InvoiceSearch.Trim()))
                {
                    row.Cells[0].Text = "<i class='fa fa-folder-open' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("and Print invoice"))
                {
                    row.Cells[0].Text = "<i class='fa fa-print' style='color:Blue'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Payment accepted : Amount"))
                {
                    row.Cells[0].Text = "<i class='fa fa-money' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Discount offered/amount adjusted Amount"))
                {
                    row.Cells[0].Text = "<i class='fa fa-gift' style='color:Blue'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Delivery status changed"))
                {
                    row.Cells[0].Text = "<i class='fa fa-unlock-alt' style='color:Blue'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Customer satisfaction index changed from"))
                {
                    row.Cells[0].Text = "<i class='fa fa-user' style='color:Green'></i>";
                }
                else if ((row.Cells[5].Text.Trim().Contains(DeliverySrcMsg.printWitAmtMsg.Trim())) || row.Cells[5].Text.Trim().Contains(DeliverySrcMsg.printWithoutAmtMsg.Trim()))
                {
                    row.Cells[0].Text = "<i class='fa fa-print' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments delivered on"))
                {
                    row.Cells[0].Text = "<i class='fa  fa-truck' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments delivered on"))
                {
                    row.Cells[0].Text = "<i class='fa  fa-truck' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Preview garment tags"))
                {
                    row.Cells[0].Text = "<i class='fa  fa-barcode' style='color:#FF8040'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Print garment tags"))
                {
                    row.Cells[0].Text = "<i class='fa  fa-barcode' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Payment accepted"))
                {
                    row.Cells[0].Text = "<i class='fa  fa-credit-card' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garment delivered to/picked up by client"))
                {
                    row.Cells[0].Text = "<i class='fa  fa-check-square-o' style='color:Green'></i>&nbsp;<i class='fa  fa-truck' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order accessed through Pending Stock report"))
                {
                    row.Cells[0].Text = "<i class='fa fa-book' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order accessed through Dashboard."))
                {
                    row.Cells[0].Text = "<i class='fa fa-dashboard' style='color:Green'></i>";
                }
                else if ((row.Cells[5].Text.Trim().Contains(bookingCancelSrcMsg.BookingCancel.Trim())) || (row.Cells[5].Text.Trim().Contains("Order cancelled for")))
                {
                    row.Cells[0].Text = "<i class='fa fa-times-circle' style='color:Red'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains(bookingCancelSrcMsg.InvoiceOpenMsg.Trim()))
                {
                    row.Cells[0].Text = "<i class='fa fa-binoculars' style='color:Green'></i>";
                }
                else if (row.Cells[4].Text.Trim().Contains(ScreenName.ScrReportName.Trim()))
                {
                    row.Cells[0].Text = "<i class='fa fa-bar-chart-o' style='color:Green'></i>&nbsp;<i class='fa fa-folder-open' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Deliver order screen: Notes added"))
                {
                    row.Cells[0].Text = "<i class='fa fa-stack-exchange' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments received at workshop"))
                {
                    row.Cells[0].Text = "<i class='fa fa-arrow-right' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Garments sent to store after processing"))
                {
                    row.Cells[0].Text = "<i class='fa fa-arrow-left' style='color:Green'></i>";
                }
                else if (row.Cells[5].Text.Trim().Contains("Order accessed through Home"))
                {
                    row.Cells[0].Text = "<i class='fa fa-home' style='color:Green'></i>";
                }
                else
                {
                    row.Cells[0].Text = "<i class='fa fa-pencil-square-o' style='color:red'></i>";
                }

            }

        }
    }
}