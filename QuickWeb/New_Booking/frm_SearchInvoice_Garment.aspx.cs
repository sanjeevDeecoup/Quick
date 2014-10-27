using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.Security;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QuickWeb.New_Booking
{
    public partial class frm_SearchInvoice_Garment : System.Web.UI.Page
    {
        private ArrayList date = new ArrayList();
        DataSet dsMain = new DataSet();
        DataSet dsInvoice = new DataSet();
        bool IsBarcode = false;
        private string[] BookingNo;
        public static string _OrderNo, _HomeDelivery, _WorkShopNote, _OrderNote = string.Empty;
        public static string _custAddress, _custPhone, _custMob, _custEmail = string.Empty;
        public static string _GrossAmt, _Discount, _Tax, _NetAmount, _Advance, _paid, _DelDiscount, _Balance = string.Empty;
        String rowdata = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPage(this.Page);
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            var btn = Request.Params["__EVENTTARGET"] as string;
            if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrOrder")
            {
                GetOrderDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrInProcess")
            {
                GetInProcessDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrWorkshopIn")
            {
                GetWorkshopInDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrWorkshopOut")
            {
                GetWorkshopOutDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrMarkForDel")
            {
                GetMarkForDeliveryDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrReady")
            {
                GetReadyDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrDelivery")
            {
                GetDeliveredDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrDelivery")
            {
                GetDeliveredDetails();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrRecvFromWorkshop")
            {
                GetReceivedFromWorkshopDetails();
            }

        }

        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            btnSearchInvoice_Click(null, EventArgs.Empty);
        }
        protected void btnSearchInvoice_Click(object sender, EventArgs e)
        {    int status = 0;
                SqlCommand cmd = new SqlCommand();
                SqlDataReader sdr = null;
            var assignId = string.Empty;
            try
            {
                divInvoiceDetail.Visible = false;
                BookingNo = txtBarcode.Text.Split('-');
                int BookingNoLength = BookingNo.Length;
                cmd.CommandText = "sp_Dry_EmployeeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 11);
                cmd.Parameters.AddWithValue("@BookingNumber", BookingNo[0]);
                if (BookingNo.Length == 1 || BookingNo.Length == 3)
                {
                    if (BookingNo.Length == 1)
                    {
                        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BranchId", BookingNo[2]);
                    }
                }
                sdr = AppClass.ExecuteReader(cmd);

                if (sdr.Read())
                {
                    status = Convert.ToInt32(sdr.GetValue(0));
                    assignId = sdr.GetValue(1).ToString();
                }

                if (status != 5)
                {
                    if (BookingNo.Length == 1 || BookingNo.Length == 3)
                    {
                        if (BookingNo.Length == 1)
                        {
                            dsMain = BAL.BALFactory.Instance.BAL_Sticker.GetBookingDetailsData(BookingNo[0], " ", Globals.BranchID);
                        }
                        else
                        {
                            dsMain = BAL.BALFactory.Instance.BAL_Sticker.GetBookingDetailsData(BookingNo[0], " ", BookingNo[2]);
                            //PrjClass.SetItemInDropDown(drpBranch, BookingNo[2].ToString(), false, false);
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Please enter valid input for search";
                        return;
                    }
                    divInprocess.Visible = false;
                    divWorkshopIn.Visible = false;
                    divWorkshopOut.Visible = false;
                    divRecvFromWorkshop.Visible = false;
                    divMarkForDel.Visible = false;
                    divReady.Visible = false;
                    divDelivery.Visible = false;
                    if (dsMain.Tables[0].Rows.Count > 0)
                    {
                        ViewState["dsViewInvoice"] = null;
                        ViewState["dsViewInvoice"] = dsMain;

                        //lblWorkShopName.Text = drpBranch.SelectedItem.Text;                      
                        lblBookingDate.Text = dsMain.Tables[0].Rows[0]["BookingDate"].ToString();
                        lblTotalQty.Text = dsMain.Tables[0].Rows[0]["Qty"].ToString();
                        _OrderNo = dsMain.Tables[0].Rows[0]["BookingNumber"].ToString();
                        lblBookDate.Text = dsMain.Tables[0].Rows[0]["BookingDate"].ToString();
                        lblDelDate.Text = dsMain.Tables[0].Rows[0]["BookingDeliveryDate"].ToString();
                        _HomeDelivery = dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString();
                        if (_HomeDelivery == "True")
                        {
                            HomeDelYes.Visible = true;
                            HomeDelNo.Visible = false;
                        }
                        else
                        {
                            HomeDelNo.Visible = true;
                            HomeDelYes.Visible = false;
                        }
                        lblDeliverDate.Text = dsMain.Tables[0].Rows[0]["ClothDeliverDate"].ToString();
                        DateTime dtDue = Convert.ToDateTime(lblDelDate.Text);

                        if (lblDeliverDate.Text != "")
                        {
                            DateTime dtDel = Convert.ToDateTime(lblDeliverDate.Text);
                            if (dtDel >= dtDue)
                            {
                                lblDeliverDate.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                lblDeliverDate.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        else
                        {
                            lblDeliverDate.ForeColor = System.Drawing.Color.Black;

                        }
                        _WorkShopNote = dsMain.Tables[0].Rows[0]["WorkShopNote"].ToString();
                        _OrderNote = dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString();

                        lblCustDetail.Text = dsMain.Tables[0].Rows[0]["Customer"].ToString();
                        _custAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
                        _custPhone = dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString();
                        _custMob = dsMain.Tables[0].Rows[0]["CustomerMobile"].ToString();
                        _custEmail = dsMain.Tables[0].Rows[0]["CustomerEmailId"].ToString();

                        lblPendingAmt.Text = dsMain.Tables[10].Rows[0]["balance"].ToString();
                        lblPendingCloth.Text = dsMain.Tables[10].Rows[0]["BalQty"].ToString();


                        _GrossAmt = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["TotalCost"].ToString()), 2));
                        _Discount = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["DiscountOnPayment"].ToString()), 2));
                        _Tax = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["ST"].ToString()), 2));
                        _NetAmount = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["NetAmount"].ToString()), 2));
                        _Advance = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["PaymentMade"].ToString()), 2));
                        _paid = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["Paid"].ToString()), 2));
                        _DelDiscount = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["DeliveryDiscount"].ToString()), 2));
                        _Balance = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["DuePayment"].ToString()), 2));
                        if (Convert.ToDouble(dsMain.Tables[7].Rows[0]["DiscountOnPayment"]) > 0)
                        {
                            if ((dsMain.Tables[7].Rows[0]["DiscountOption"].ToString()) == "0")
                            {
                                lblDiscountOption.Text = "(" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[7].Rows[0]["Discount"].ToString()), 2)) + "%)";
                            }
                            else
                            {
                                lblDiscountOption.Text = "";
                            }
                        }
                        grdPayment.DataSource = dsMain.Tables[8];
                        grdPayment.DataBind();

                        int TotPcsMove = 0;
                        TotPcsMove = dsMain.Tables[1].Rows.Count + dsMain.Tables[2].Rows.Count + dsMain.Tables[3].Rows.Count + dsMain.Tables[4].Rows.Count + dsMain.Tables[5].Rows.Count + dsMain.Tables[6].Rows.Count;

                        int OrderPcs = (Convert.ToInt32(dsMain.Tables[0].Rows[0]["Qty"]) - TotPcsMove);

                        lblInProcQty.Text = Convert.ToString(OrderPcs);
                        lblInProcessDate.Text = dsMain.Tables[0].Rows[0]["BookingDate"].ToString();
                        divInprocess.Visible = true;



                        if (dsMain.Tables[1].Rows.Count > 0)
                        {
                            lblWorkshopQty.Text = dsMain.Tables[1].Rows.Count.ToString();
                            lblWorkshopDate.Text = dsMain.Tables[1].Rows[0]["LastDate"].ToString();
                            divInprocess.Visible = true;
                            divWorkshopIn.Visible = true;

                        }
                        else
                        {
                            lblWorkshopQty.Text = dsMain.Tables[1].Rows.Count.ToString();
                            lblWorkshopDate.Text = dsMain.Tables[9].Rows[0]["MaxDate20"].ToString();

                        }

                        if (dsMain.Tables[2].Rows.Count > 0)
                        {
                            lblWorkshopOutQty.Text = dsMain.Tables[2].Rows.Count.ToString();
                            lblWorkshopOutDate.Text = dsMain.Tables[2].Rows[0]["LastDate"].ToString();

                            divInprocess.Visible = true;
                            divWorkshopIn.Visible = true;
                            divWorkshopOut.Visible = true;
                        }
                        else
                        {
                            lblWorkshopOutQty.Text = dsMain.Tables[2].Rows.Count.ToString();
                            lblWorkshopOutDate.Text = dsMain.Tables[9].Rows[0]["MaxDate30"].ToString();

                        }


                        if (dsMain.Tables[3].Rows.Count > 0)
                        {
                            lblRecvFromWorkshopQty.Text = dsMain.Tables[3].Rows.Count.ToString();
                            lblRecvFromWorkshopDate.Text = dsMain.Tables[3].Rows[0]["LastDate"].ToString();

                            divInprocess.Visible = true;
                            divWorkshopIn.Visible = true;
                            divWorkshopOut.Visible = true;
                            divRecvFromWorkshop.Visible = true;
                        }
                        else
                        {
                            lblRecvFromWorkshopQty.Text = dsMain.Tables[3].Rows.Count.ToString();
                            lblRecvFromWorkshopDate.Text = dsMain.Tables[9].Rows[0]["MaxDate2"].ToString();
                        }


                        if (dsMain.Tables[4].Rows.Count > 0)
                        {
                            lblMarkForDelQty.Text = dsMain.Tables[4].Rows.Count.ToString();
                            lblMarkForDelDate.Text = dsMain.Tables[4].Rows[0]["LastDate"].ToString();


                            divInprocess.Visible = true;
                            divWorkshopIn.Visible = true;
                            divWorkshopOut.Visible = true;
                            divRecvFromWorkshop.Visible = true;
                            divMarkForDel.Visible = true;
                        }
                        else
                        {
                            lblMarkForDelQty.Text = dsMain.Tables[4].Rows.Count.ToString();
                            lblMarkForDelDate.Text = dsMain.Tables[9].Rows[0]["MaxDate50"].ToString();

                        }
                        if (dsMain.Tables[5].Rows.Count > 0)
                        {
                            lblReadyQty.Text = dsMain.Tables[5].Rows.Count.ToString();
                            lblReadyDate.Text = dsMain.Tables[5].Rows[0]["LastDate"].ToString();

                            divInprocess.Visible = true;
                            divWorkshopIn.Visible = true;
                            divWorkshopOut.Visible = true;
                            divRecvFromWorkshop.Visible = true;
                            divMarkForDel.Visible = true;
                            divReady.Visible = true;
                        }
                        else
                        {
                            lblReadyQty.Text = dsMain.Tables[5].Rows.Count.ToString();
                            lblReadyDate.Text = dsMain.Tables[9].Rows[0]["MaxDate3"].ToString();
                        }

                        if (dsMain.Tables[6].Rows.Count > 0)
                        {
                            lblDeliveryQty.Text = dsMain.Tables[6].Rows.Count.ToString();
                            hdnDeliveryQty.Value = dsMain.Tables[6].Rows.Count.ToString();
                            lblDeliveryDate.Text = dsMain.Tables[6].Rows[0]["LastDate"].ToString();

                            divInprocess.Visible = true;
                            divWorkshopIn.Visible = true;
                            divWorkshopOut.Visible = true;
                            divMarkForDel.Visible = true;
                            divRecvFromWorkshop.Visible = true;
                            divReady.Visible = true;
                            divDelivery.Visible = true;
                        }
                        divInvoiceDetail.Visible = true;
                        applyCSS();
                        if (BookingNo.Length == 3)
                        {
                            BindNullGrid(grdInvoice);
                            grdInvoice.DataSource = dsMain.Tables[0];
                            grdInvoice.DataBind();
                            IsBarcode = true;
                            FindGridRowForHighLighted(grdInvoice, txtBarcode.Text.ToUpper());
                        }
                        statusVisible();

                        string BID = Globals.BranchID;
                        string UserName = Globals.UserName;
                        Task t = Task.Factory.StartNew (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(BookingNo[0].Trim().ToUpper(), UserName, BID, bookingCancelSrcMsg.InvoiceOpenMsg, ScreenName.SearchInvoice, 15); });
                    }
                    else
                    {
                        divInvoiceDetail.Visible = false;
                        lblMsg.Text = "No Record Found.";

                    }

                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
                else
                {
                    lblMsg.Text = "This booking number  is cancelled";

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }

        }
        private void GetOrderDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdInvoice);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdInvoice.DataSource = dsInvoice.Tables[0];
                grdInvoice.DataBind();
            }
            grdInvoice.Visible = true;
        }

        private void GetInProcessDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];

            BindNullGrid(grdInprocess);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdInprocess.DataSource = dsInvoice.Tables[0];
                grdInprocess.DataBind();
            }
            grdInprocess.Visible = true;

        }
        private void GetWorkshopInDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdWorkshopIn);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdWorkshopIn.DataSource = dsInvoice.Tables[0];
                grdWorkshopIn.DataBind();
            }

            grdWorkshopIn.Visible = true;

        }
        private void GetWorkshopOutDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdWorkShopOut);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdWorkShopOut.DataSource = dsInvoice.Tables[0];
                grdWorkShopOut.DataBind();
            }
            grdWorkShopOut.Visible = true;
        }

        private void GetReceivedFromWorkshopDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdRecvFromWorkShop);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdRecvFromWorkShop.DataSource = dsInvoice.Tables[0];
                grdRecvFromWorkShop.DataBind();
            }
            grdRecvFromWorkShop.Visible = true;

        }

        private void GetMarkForDeliveryDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdMarkForDel);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdMarkForDel.DataSource = dsInvoice.Tables[0];
                grdMarkForDel.DataBind();
            }
            grdMarkForDel.Visible = true;
        }

        private void GetReadyDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdReady);
            if (ViewState["dsViewInvoice"] != null)
            {
                grdReady.DataSource = dsInvoice.Tables[0];
                grdReady.DataBind();
            }
            grdReady.Visible = true;
        }
        private void GetDeliveredDetails()
        {
            dsInvoice = new DataSet();
            dsInvoice = (DataSet)ViewState["dsViewInvoice"];
            BindNullGrid(grdDelivery);
            grdDelivery.DataSource = dsInvoice.Tables[6];
            grdDelivery.DataBind();
            grdDelivery.Visible = true;
        }

        private void BindNullGrid(GridView grd)
        {
            grd.DataSource = null;
            grd.DataBind();
        }
        private void FindGridRowForHighLighted(GridView grd, string barcode)
        {
            string matchingbarcode = "*" + barcode + "*";

            for (int count = 0; count < grd.Rows.Count; count++)
            {
                if (matchingbarcode == ((Label)grd.Rows[count].FindControl("lblBarcode")).Text)
                {
                    int statusID = Convert.ToInt32(((Label)grd.Rows[count].FindControl("lblStatusId")).Text);
                    int ISN = Convert.ToInt32(((Label)grd.Rows[count].FindControl("lblISN")).Text);
                    ViewState["viewISN"] = null;
                    ViewState["viewISN"] = ISN;


                    if (statusID == 1)
                    {
                        GetInProcessDetails();
                        achrInProcess.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                    else if (statusID == 20)
                    {
                        GetWorkshopInDetails();
                        achrWorkshopIn.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                    else if (statusID == 30)
                    {
                        GetWorkshopOutDetails();
                        achrWorkshopOut.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                    else if (statusID == 2)
                    {
                        GetReceivedFromWorkshopDetails();
                        achrRecvFromWorkshop.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                    else if (statusID == 50)
                    {
                        GetMarkForDeliveryDetails();
                        achrMarkForDel.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                    else if (statusID == 3)
                    {
                        GetReadyDetails();
                        achrReady.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                    else if (statusID == 4)
                    {
                        GetDeliveredDetails();
                        achrDelivery.Attributes.Add("class", "btn btn-warning btn-block");
                    }
                }
            }
        }
        protected void grdInprocess_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (IsBarcode == false)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strRowData = (e.Row.Cells[3].Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (strRowData == "&nbsp;")
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            cell.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
            else
            {
                int matchingISN = 0;
                if (ViewState["viewISN"] != null)
                {
                    matchingISN = (int)ViewState["viewISN"];
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LbInproclISN = (Label)e.Row.FindControl("LbInproclISN");
                    int InProcISN = Convert.ToInt32(LbInproclISN.Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (matchingISN == InProcISN)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
            }
        }
        protected void grdWorkshopIn_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (IsBarcode == false)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strRowData = (e.Row.Cells[3].Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (strRowData == "&nbsp;")
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            cell.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
            else
            {
                int matchingISN = 0;
                if (ViewState["viewISN"] != null)
                {
                    matchingISN = (int)ViewState["viewISN"];
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LblWShInISN = (Label)e.Row.FindControl("LblWShInISN");
                    int workShopIN = Convert.ToInt32(LblWShInISN.Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (matchingISN == workShopIN)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }

        }
        protected void grdWorkShopOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsBarcode == false)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strRowData = (e.Row.Cells[3].Text);
                    rowdata = (e.Row.Cells[2].Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (strRowData == "&nbsp;")
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            if (rowdata == "&nbsp;" && strRowData != "&nbsp;")
                            {
                                e.Row.Cells[3].Text = "";
                                e.Row.Cells[4].Text = "";

                            }
                            else
                            {
                                cell.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }
            }
            else
            {
                int matchingISN = 0;
                if (ViewState["viewISN"] != null)
                {
                    matchingISN = (int)ViewState["viewISN"];
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LblWShOutISN = (Label)e.Row.FindControl("LblWShOutISN");
                    int workShopOut = Convert.ToInt32(LblWShOutISN.Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (matchingISN == workShopOut)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
            }

        }

        protected void grdRecvFromWorkShop_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsBarcode == false)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strRowData = (e.Row.Cells[3].Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (strRowData == "&nbsp;")
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            cell.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
            else
            {
                int matchingISN = 0;
                if (ViewState["viewISN"] != null)
                {
                    matchingISN = (int)ViewState["viewISN"];
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblRcfromWSISN = (Label)e.Row.FindControl("lblRcfromWSISN");
                    int RcvdFromworkShop = Convert.ToInt32(lblRcfromWSISN.Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (matchingISN == RcvdFromworkShop)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
            }

        }

        protected void grdMarkForDel_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (IsBarcode == false)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strRowData = (e.Row.Cells[3].Text);
                    rowdata = (e.Row.Cells[2].Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (strRowData == "&nbsp;")
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            if (rowdata == "&nbsp;" && strRowData != "&nbsp;")
                            {
                                e.Row.Cells[3].Text = "";
                                e.Row.Cells[4].Text = "";

                            }
                            else
                            {
                                cell.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }
            }
            else
            {
                int matchingISN = 0;
                if (ViewState["viewISN"] != null)
                {
                    matchingISN = (int)ViewState["viewISN"];
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblMarkForDelISN = (Label)e.Row.FindControl("lblMarkForDelISN");
                    int markForDel = Convert.ToInt32(lblMarkForDelISN.Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (matchingISN == markForDel)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
            }
        }

        protected void grdReady_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (IsBarcode == false)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strRowData = (e.Row.Cells[3].Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (strRowData == "&nbsp;")
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            cell.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
            else
            {
                int matchingISN = 0;
                if (ViewState["viewISN"] != null)
                {
                    matchingISN = (int)ViewState["viewISN"];
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblReadyISN = (Label)e.Row.FindControl("lblReadyISN");
                    int Ready = Convert.ToInt32(lblReadyISN.Text);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (matchingISN == Ready)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
            }

        }

        protected void grdDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            int matchingISN = 0;
            if (ViewState["viewISN"] != null)
            {
                matchingISN = (int)ViewState["viewISN"];
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDelISN = (Label)e.Row.FindControl("lblDelISN");
                int Deliverd = Convert.ToInt32(lblDelISN.Text);
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (matchingISN == Deliverd)
                    {
                        cell.BackColor = System.Drawing.Color.Orange;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }
        private void statusVisible()
        {
            if (dsMain.Tables[9].Rows[0]["MaxDate20"].ToString() == "")
            {
                divWorkshopIn.Visible = false;
            }
            if (dsMain.Tables[9].Rows[0]["MaxDate30"].ToString() == "")
            {
                divWorkshopOut.Visible = false;
            }
            if (dsMain.Tables[9].Rows[0]["MaxDate2"].ToString() == "")
            {
                divRecvFromWorkshop.Visible = false;
            }
            if (dsMain.Tables[9].Rows[0]["MaxDate50"].ToString() == "")
            {
                divMarkForDel.Visible = false;
            }
            if (dsMain.Tables[9].Rows[0]["MaxDate3"].ToString() == "")
            {
                divReady.Visible = false;
            }
            if (dsMain.Tables[9].Rows[0]["MaxDate4"].ToString() == "")
            {
                divDelivery.Visible = false;
            }
        }

        private void applyCSS()
        {
            achrInProcess.Attributes.Add("class", "btn btn-default btn-block active");
            achrWorkshopIn.Attributes.Add("class", "btn btn-default btn-block active");
            achrWorkshopOut.Attributes.Add("class", "btn btn-default btn-block active");
            achrWorkshopOut.Attributes.Add("class", "btn btn-default btn-block active");
            achrRecvFromWorkshop.Attributes.Add("class", "btn btn-default btn-block active");
            achrRecvFromWorkshop.Attributes.Add("class", "btn btn-default btn-block active");
            achrMarkForDel.Attributes.Add("class", "btn btn-default btn-block active");
            achrReady.Attributes.Add("class", "btn btn-default btn-block active");
            achrDelivery.Attributes.Add("class", "btn btn-default btn-block active");
        }

    }
}