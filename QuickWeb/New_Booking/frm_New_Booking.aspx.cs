using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuickWeb.New_Booking
{
    public partial class frmBookingScreen : BasePage
    {
        public string strVersion = PrjClass.GetSoftwareVersionName();
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected string MSG = "ENTER = SAVE";
        protected string MSG1 = "ESC = EXIT";
        protected string MSG2 = "TAB = SELECT";
        protected string MSG3 = "F10 = SAVE";
        /*************************/
        protected string BkScrItmDim = ReportLabelsNew.BkScrItmDim;
        protected string BkScrItmLen = ReportLabelsNew.BkScrItmLen;
        protected string BkScrItmWth = ReportLabelsNew.BkScrItmWth;
        protected string BkScrItmDimCaption = ReportLabelsNew.BkScrItmDimCaption;
        protected string BkScrItmPnl = ReportLabelsNew.BkScrItmPnl;
        protected string BkScrItmPnlCaption = ReportLabelsNew.BkScrItmPnlCaption;
        protected string BkScrItmPnlDesc = ReportLabelsNew.BkScrItmPnlDesc;
        private bool blnRight = false;
        public static bool blnRight2 = false;
        /*************************/
        private static string CustId = string.Empty, editOption = "-1";
        private static string ItemName = string.Empty, Process = string.Empty;
        protected string CurrentUserName = string.Empty;
        DTO.NewBooking Obj = new DTO.NewBooking();
        ArrayList date = new ArrayList();
        protected void Page_Init(object sender, EventArgs e)
        {
            BranchId.Value = Globals.BranchID;
            //Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            //Response.AddHeader("Content-Encoding", "gzip");
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);            
        }       
        protected void BindDataList()
        {
            hdnValues.Text = BAL.BALFactory.Instance.BAL_New_Bookings.BindRemarksInUI(Globals.BranchID);
            LblValuesColor.Text = BAL.BALFactory.Instance.BAL_New_Bookings.BindColorsInUI(Globals.BranchID);
        }
        protected void CheckBookingDateRights()
        {
            var _bBackDateBookingAvailable = BAL.BALFactory.Instance.BAL_New_Bookings.BackDateBookingAvailable(Session["UserType"].ToString(), Globals.BranchID);
            if (_bBackDateBookingAvailable)
            {
                txtDate.Enabled = true;
            }
            else
            {
                txtDate.Enabled = false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUserName = Globals.UserName;
            this.ClientScript.GetPostBackEventReference(this, string.Empty);
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPage(this.Page);
                blnRight2 = AppClass.CheckButtonRights(SpecialAccessRightName.DiscountOnBookingSrc);
                hdnCheckDiscountRight.Value = blnRight2.ToString();
                if (blnRight2)
                {
                    txtDiscount.Enabled = true;
                    rdrPercentage.Enabled = true;
                    rdrAmt.Enabled = true;
                }
                else
                {
                    txtDiscount.Enabled = false;
                    rdrPercentage.Enabled = false;
                    rdrAmt.Enabled = false;
                }
                SetMenuRightsNew(Globals.UserType);
                FillExistingRecord();
                Deafaultsetting();
                txtItemSubQty_TextChanged(null, null);
                txtCustomerName.Focus();
                Session["Custcode"] = "";
                BindPriorty();
                binddrpsms();
                binddrpdefaultsms();
                BindDataList();
                CheckBookingDateRights();
                if (Request.QueryString["option"] != null)
                {
                    btnEditBooking_Click(null, null);
                }
                if (Request.QueryString["BkNo"] != null)
                {
                    txtEdit.Text = Request.QueryString["BkNo"].ToString();
                    hdnFireEdit.Value = "true";
                }
                if (Request.QueryString["CustBN"] != null)
                {
                    hdnCustBooking.Value = Request.QueryString["CustBN"].ToString();
                }
                drpDate.Items.Clear();
                for (var i = 1; i <= 31; i++)
                {
                    drpDate.Items.Add(i.ToString());
                }
                //drpMonth.Items.Clear();
                drpYear.Items.Clear();
                for (int i = 0; i < 12; i++)
                {
                    drpYear.Items.Add(DateTime.Now.AddYears(i).Year.ToString());
                }
                BindRateListDropDown();
            }
            else
            {
                //try
                //{
                //    if (!(Request.Form["_EventTarget"] != null && Request.Form["_EventTarget"] == "myDblclick"))
                //    {

                //        grdEntry.Rows[int.Parse(Request.Form["__EVENTARGUMENT"].ToString())].BackColor = System.Drawing.Color.Silver;
                //        int rows = Convert.ToInt32(((Label)grdEntry.Rows[int.Parse(Request.Form["__EVENTARGUMENT"].ToString())].FindControl("lblSNO")).Text);
                //        if (rows != -1)
                //        {
                //            grdEntry.EditIndex = rows;
                //            setEditValue(grdEntry.EditIndex);
                //        }
                //    }
                //}
                //catch (Exception)
                //{ }
            }
            if (Session["Custcode"] != null && Session["Custcode"].ToString() != "")
            {
                setCustSearchvalues();
                if (grdCustomerSearch.Rows.Count > 0)
                {
                    txtDiscount.Text = Convert.ToString(BAL.BALFactory.Instance.BAL_New_Bookings.GetCustomerDiscount(Session["Custcode"].ToString()));
                    txtCustomerName.Text = ((LinkButton)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerName")).Text;
                    lblAddress.Text = ((LinkButton)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerAddress")).Text;
                    lblMobileNo.Text = ((LinkButton)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerMobile")).Text;
                    lblPriority.Text = ((LinkButton)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerPriority")).Text;
                    lblRemarks.Text = ((LinkButton)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerRemark")).Text;
                    CustId = ((HiddenField)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerCode")).Value;
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
                    Session["Custcode"] = "";
                }
            }
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_New_Bookings.FirstTimeDefaultData(Globals.BranchID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkToday.Text = ds.Tables[0].Rows[0][2].ToString();
                if (chkToday.Text == "")
                    chkToday.Visible = false;
                chkNextDay.Text = ds.Tables[0].Rows[0][3].ToString();
                if (chkNextDay.Text == "")
                    chkNextDay.Visible = false;
            }
            blnRight = AppClass.CheckButtonRights(SpecialAccessRightName.DefaultDiscount);
            if (blnRight)
            {
                txtDefaultDiscount.Attributes.Add("style", "display:inline");
                trdiscount.Attributes.Add("style", "display:inline");
                trpercentage.Attributes.Add("style", "display:inline");
                trdot.Attributes.Add("style", "display:inline");
            }
            else
            {
                txtDefaultDiscount.Attributes.Add("style", "display:none");
                trdiscount.Attributes.Add("style", "display:none");
                trpercentage.Attributes.Add("style", "display:none");
                trdot.Attributes.Add("style", "display:none");
            }

        }

        protected void BindRateListDropDown()
        {
            ddlRateList.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindAllListMasters(Globals.BranchID);
            ddlRateList.DataTextField = "name";
            ddlRateList.DataValueField = "rateListId";
            ddlRateList.DataBind();

            ddlRateListNewCustomer.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindAllListMasters(Globals.BranchID);
            ddlRateListNewCustomer.DataTextField = "name";
            ddlRateListNewCustomer.DataValueField = "rateListId";
            ddlRateListNewCustomer.DataBind();
        }

        public void Deafaultsetting()
        {
            lblLastBooking.Text = BAL.BALFactory.Instance.BAL_New_Bookings.GetLastBookinNumber(BranchId.Value);
            txtCustomerName.Text = "";
            txtCustomerName.Focus();
            imgBtnCustomerAdd.Attributes.Add("onClick", "ShowAddCustomer();");
            ArrayList DateAndTime = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            txtDate.Text = DateAndTime[0].ToString();
            DataSet Ds_Header = BAL.BALFactory.Instance.BAL_New_Bookings.FillHeaderInfo(BranchId.Value);
            if (Ds_Header.Tables[0].Rows.Count > 0)
            {
                txtDueDate.Text = (DateTime.Parse(txtDate.Text).AddDays(Convert.ToInt32(Ds_Header.Tables[0].Rows[0]["DateOffSet"].ToString()))).ToString("dd MMM yyyy");
                hdnDefaultDateOffset.Value = Convert.ToInt32(Ds_Header.Tables[0].Rows[0]["DateOffSet"].ToString()).ToString();
                txtTime.Text = Ds_Header.Tables[0].Rows[0]["DueTime"].ToString();
            }
            drpCheckedBy.DataSource = BAL.BALFactory.Instance.BAL_New_Bookings.BindCheckedBy(BranchId.Value);
            drpCheckedBy.DataTextField = "EmployeeName";
            drpCheckedBy.DataValueField = "EmployeeCode";
            drpCheckedBy.DataBind();
            BindGridView();

            SetCommanSetting();
            txtName_TextChanged(null, null);
            setCustSearchvalues();
        }
        string[] customerName;
        
        public void setCustvalue(string customerName, string BID)
        {
            DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, BID);
            if (DS_CustInfo.Tables[0].Rows.Count > 0)
            {
                CustId = customerName;
                txtCustomerName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
                lblAddress.Text = DS_CustInfo.Tables[0].Rows[0]["CustAddress"].ToString();
                lblMobileNo.Text = DS_CustInfo.Tables[0].Rows[0]["CustMobile"].ToString();
                lblPriority.Text = DS_CustInfo.Tables[0].Rows[0]["CustPriority"].ToString();
                lblRemarks.Text = DS_CustInfo.Tables[0].Rows[0]["CustRemarks"].ToString();
                txtDiscount.Text = Convert.ToString(BAL.BALFactory.Instance.BAL_New_Bookings.GetCustomerDiscount(customerName.ToString()));
            }
            else
            {
                imgBtnCustomerAdd_Click(null, null);
            }
            SetBalance();
        }
        protected void btnNewBooking_Click(object sender, EventArgs e)
        {
            //Response.Redirect("frmBookingScreen.aspx", false);
            Response.Redirect("frm_New_Booking.aspx", false);
        }
        protected void btnEditBooking_Click(object sender, EventArgs e)
        {
            if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckEditBookingRights(Globals.UserType, Globals.BranchID) != false)
            {
                txtEdit.Text = "";
                tblEntry.Visible = false;
                tblEntry1.Visible = false;
                tblEntry2.Visible = false;
                tblSearch.Visible = true;                
                tblSearch.Attributes["style"] = "display:block;";
                txtEdit.Focus();
            }
            else
            {
                lblError.Text = "You are not authorised to edit the booking.";
                if (Request.QueryString["BkNo"] != null)
                {
                    var tmpBranchID5 = Globals.BranchID;
                    var tmpUserName5 = Globals.UserName;
                    Task t = Task.Factory.StartNew
                    (
                    () => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(Request.QueryString["BkNo"].ToString().ToUpper(), tmpUserName5, tmpBranchID5, bookingSrcMsg.BookingEditRight, ScreenName.BookingScreen,1); }
                    );
                }
                return;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            imgBtnCustomerAdd.Attributes.Add("onClick", "ShowAddCustomer();");
        }
        protected void btnOption_Click1(object sender, EventArgs e)
        {

        }
        private void BindGridView()
        {
            DataTable dt = new DataTable();
            ArrayList Items = new ArrayList();
            Items.Add("SNO");
            Items.Add("QTY");
            Items.Add("ITEMNAME");
            Items.Add("REMARKS");
            Items.Add("COLOR");
            Items.Add("PROCESS");
            Items.Add("RATE");
            Items.Add("EXPROC1");
            Items.Add("RATE1");
            Items.Add("EXPROC2");
            Items.Add("RATE2");
            Items.Add("AMOUNT");
            Items.Add("ITEMNAME1");
            Items.Add("PROCESS1");
            Items.Add("STPAmt");
            Items.Add("STEP1Amt");
            Items.Add("STEP2Amt");
            Items.Add("COLORCODE");
            Items.Add("DisAmt");
            Items.Add("DisAmt1");
            Items.Add("DisAmt2");

            // < MOHIT CHAUHAN >
            Items.Add("STPAmtEcess");
            Items.Add("STPAmtSHECess");

            Items.Add("STPAmt1Ecess");
            Items.Add("STPAmt1SHECess");

            Items.Add("STPAmt2Ecess");
            Items.Add("STPAmt2SHECess");
            // </ MOHIT CHAUHAN >

            dt = BAL.BALFactory.Instance.BAL_New_Bookings.BindGridView(Items, "1", BranchId.Value);
            grdEntry.DataSource = dt;
            grdEntry.DataBind();
            ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text = "1";
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text = "1";
            ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = dt.Rows[0]["ITEMNAME"].ToString().ToUpper();
            ItemName = dt.Rows[0]["ITEMNAME"].ToString().ToUpper();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = dt.Rows[0]["PROCESS"].ToString().ToUpper();
            Process = dt.Rows[0]["PROCESS"].ToString().ToUpper();
            dt.Rows[0].Delete();
            dt.AcceptChanges();
            ViewState["Entry"] = dt;
            grdEntry.Rows[0].Visible = false;
        }
        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            txtCNameSearch_TextChanged(null, null);
            txtCNameSearch.Focus();
            ModalPopupExtender5.Show();
        }
        protected void txtCNameSearch_TextChanged(object sender, EventArgs e)
        {
            setCustSearchvalues();
            txtCNameSearch.Focus();
            txtCNameSearch.Attributes.Add("onfocus", "javascript:select();");
            SetCommanSetting();
            txtAddress.Text = "";
            txtPhoneNo.Text = "";
            clearRecord();
        }
        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            setCustSearchvalues();
            txtAddress.Focus();
            txtAddress.Attributes.Add("onfocus", "javascript:select();");
            SetCommanSetting();
            txtPhoneNo.Text = "";
            clearRecord();
        }
        protected void txtPhoneNo_TextChanged(object sender, EventArgs e)
        {

            setCustSearchvalues();
            txtPhoneNo.Focus();
            txtPhoneNo.Attributes.Add("onfocus", "javascript:select();");
            SetCommanSetting();
            clearRecord();
        }
        public void clearRecord()
        {
            if (grdCustomerSearch.Rows.Count <= 0)
            {
                lblAddress.Text = "";
            }
        }
        public void setCustSearchvalues()
        {
            DTO.NewBooking obj = new DTO.NewBooking();
            obj.CustName = txtCNameSearch.Text;
            obj.CustAddress = txtAddress.Text;
            obj.CustMobile = txtPhoneNo.Text;
            obj.BID = BranchId.Value;

            // DataSet ds = new DataSet();
            // ds = BAL.BALFactory.Instance.BAL_New_Bookings.SetCustSearchGrid(obj);
            // grdCustomerSearch.DataSource = ds;
            // grdCustomerSearch.DataBind();
            //if (grdCustomerSearch.Rows.Count > 0)
            //{
            //    grdCustomerSearch.Rows[0].BackColor = System.Drawing.Color.Silver;
            //    Session["Custcode"] = ((HiddenField)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerCode")).Value;
            //}
        }
        protected void grdCustomerSearch_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "SelectCustomer")
            //{
            //    int rownum = int.Parse(e.CommandArgument.ToString());
            //    if (rownum < grdCustomerSearch.Rows.Count)
            //    {

            //        Session["Custcode"] = ((HiddenField)grdCustomerSearch.Rows[rownum].FindControl("lnkBtnCustomerCode")).Value;
            //        CustId = ((HiddenField)grdCustomerSearch.Rows[rownum].FindControl("lnkBtnCustomerCode")).Value;
            //        txtCustomerName.Text = ((LinkButton)grdCustomerSearch.Rows[rownum].FindControl("lnkBtnCustomerName")).Text;
            //        lblAddress.Text = ((LinkButton)grdCustomerSearch.Rows[rownum].FindControl("lnkBtnCustomerAddress")).Text;
            //        lblMobileNo.Text = ((LinkButton)grdCustomerSearch.Rows[rownum].FindControl("lnkBtnCustomerMobile")).Text;
            //        lblPriority.Text = ((LinkButton)grdCustomerSearch.Rows[0].FindControl("lnkBtnCustomerPriority")).Text;
            //        lblRemarks.Text = ((LinkButton)grdCustomerSearch.Rows[rownum].FindControl("lnkBtnCustomerRemark")).Text;
            //        btnHideCustSearch_Click(null, null);
            //    }
            //}
        }
        protected void btnHideCustSearch_Click(object sender, EventArgs e)
        {
            ModalPopupExtender5.Hide();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
        }
        protected void txtItemSubQty_TextChanged(object sender, EventArgs e)
        {
            SetTextbox1();
            txtNewItemName.Focus();
        }
        protected void btnItemSave_Click(object sender, EventArgs e)
        {
            string rescheck = string.Empty;
            int TotalSubQty = 0;
            if (txtItemName.Text == "")
            {
                lblErr.Text = "Please enter item name.";
                txtItemName.Focus();
                return;
            }
            try
            {
                TotalSubQty = int.Parse(txtItemSubQty.Text.Trim());
            }
            catch (Exception) { lblErr.Text = "Invalid sub items"; txtItemSubQty.Focus(); return; }
            int TotalSubQty1 = Convert.ToInt32(txtItemSubQty.Text);

            if (TotalSubQty1 < 1 || TotalSubQty1 > 15)
            {
                lblErr.Text = "Invalid Sub Items";
                // lstSubItem.Visible = false;
                txtItemSubQty.Focus();
                return;
            }
            if (TotalSubQty1 == 1)
            {
                // txtNewItemName.Visible = false;
                // lstSubItem.Visible = false;
                txtItemSubQty.Focus();
                lstSubItem.Items.Clear();
            }
            if (lstSubItem.Items.Count > 0)
            {
                if (lstSubItem.Items.Count != Convert.ToInt32(txtItemSubQty.Text))
                {
                    lblErr.Text = "Item List Should be match with Sub Items Quantity";
                    txtNewItemName.Focus();
                    return;
                }
            }
            rescheck = BAL.BALFactory.Instance.BAL_New_Bookings.CheckItemCode(txtItemCode.Text, txtItemName.Text, BranchId.Value);
            if (rescheck == "done")
            {
                string res = string.Empty;
                SetValueIntoProperties();
                Obj.BID = BranchId.Value;
                res = BAL.BALFactory.Instance.BAL_New_Bookings.SaveItemMaster(Obj, lstSubItem);
                if (res != "Record Saved")
                {
                    lblErr.Text = res.ToString();
                    txtItemName.Focus();
                    txtItemName.Attributes.Add("onfocus", "javascript:select();");
                    return;
                }
                else
                {
                    lblMsg.Text = res.ToString();
                    ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text = BAL.BALFactory.Instance.BAL_New_Bookings.GetItemNameWhenDataSave(BranchId.Value);
                    ClearAll();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(txtItemName.Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();
                    //ModalPopupExtender4.Hide();
                    txtItemSubQty_TextChanged(null, null);
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Focus();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Attributes.Add("onfocus", "javascript:select();");
                }
            }
            else
            {
                lblErr.Text = rescheck;
                if (rescheck == "Item name already exist")
                    txtItemName.Focus();
                else
                    txtItemCode.Focus();
                return;
            }
        }
        public void ClearAll()
        {
            txtItemCode.Text = "";
            txtItemSubQty.Text = "1";
            // txtNewItemName.Visible = false;
            // lstSubItem.Visible = false;
            txtProcessName.Text = "";
            txtProcessCode.Text = "";
        }
        public void SetValueIntoProperties()
        {
            Obj.ItemName = txtItemName.Text;
            Obj.ItemCode = txtItemCode.Text;
            Obj.SubItems = txtItemSubQty.Text;
            Obj.ProcessCode = txtProcessCode.Text;
            Obj.ProcessName = txtProcessName.Text;
        }
        protected void imgBtnItemAdd_Click(object sender, EventArgs e)
        {
            ClearAll();
            btnAddItem_Click(null, null);
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //ModalPopupExtender4.Show();
            txtItemName.Text = ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text;
            txtItemName.Focus();
            txtItemName.Attributes.Add("onfocus()", "javascript:select();");
        }
        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] Itemname = ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text.Split('-');
                ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text = Itemname[1].ToString().ToUpper().Trim();
                ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Attributes.Add("onfocus", "javascript:select();");
                ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();
            }
            catch (Exception)
            {
                ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text.ToUpper();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();

                // ((Button)grdEntry.HeaderRow.Cells[0].FindControl("imgBtnItemAdd")).Focus();
            }
            if (txtExtraProcess1.Text != "")
            {
                txtExtraRate1.Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, txtExtraProcess1.Text, BranchId.Value)).ToString();
            }
            if (txtExtraProcess2.Text != "")
            {
                txtExtraRate2.Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, txtExtraProcess2.Text, BranchId.Value)).ToString();
            }

        }
        protected void btnPopUpProcess_Click(object sender, EventArgs e)
        {

        }
        protected void imgBtnProcessAdd_Click(object sender, EventArgs e)
        {
            txtProcessName.Text = "";
            txtProcessCode.Text = "";
            ModalPopupExtender7.Show();
            txtProcessCode.Focus();
            SetCommanSetting();
        }
        protected void btnProcessSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;

            SetValueIntoProperties();
            res = BAL.BALFactory.Instance.BAL_New_Bookings.SaveProcessMaster(Obj);
            if (res != "Record Saved")
            {
                lblPrcErr.Text = res.ToString();
                if (res == "Please enter process name.")
                    txtProcessName.Focus();
                else
                    txtProcessCode.Focus();
                return;
            }
            else
            {
                ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = txtProcessCode.Text;
                ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text, txtProcessName.Text, BranchId.Value)).ToString();
                ClearAll();
                ModalPopupExtender7.Hide();
            }
            SetCommanSetting();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Attributes.Add("onfocus", "javascript:select();");
        }
        private int GridRowCount()
        {
            int newSNo = 0;
            for (int r = 0; r < grdEntry.Rows.Count; r++)
            {
                newSNo = int.Parse("0" + ((Label)grdEntry.Rows[r].Cells[0].FindControl("lblSNO")).Text);
            }
            return newSNo;
        }
        public string CheckError()
        {
            string res = BAL.BALFactory.Instance.BAL_New_Bookings.CheckGridEntries(((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text, txtExtraProcess1.Text, txtExtraProcess2.Text, ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text, txtExtraRate1.Text, txtExtraRate2.Text, txtDiscountAmt.Text, txtAdvance.Text, BranchId.Value);
            if (res == DAL.DAL_New_Bookings.CheckError.Process1.ToString())
            {
                lblError.Text = "Invalid third process.";
                ModalPopupExtender8.Show();
                txtExtraProcess1.Focus();
                txtExtraProcess1.Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Process2.ToString())
            {
                lblError.Text = "Invalid second process.";
                ModalPopupExtender8.Show();
                txtExtraProcess2.Focus();
                txtExtraProcess2.Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Item.ToString())
            {
                lblError.Text = "Item name.";
                ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Process.ToString())
            {
                lblError.Text = "Invalid second process.";
                ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Rate.ToString())
            {
                lblError.Text = "Invalid rate.";
                ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Rate1.ToString())
            {
                lblError.Text = "Invalid second process rate.";
                ModalPopupExtender8.Show();
                txtExtraRate1.Focus();
                txtExtraRate1.Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Rate2.ToString())
            {
                lblError.Text = "Invalid third process rate.";
                ModalPopupExtender8.Show();
                txtExtraRate2.Focus();
                txtExtraRate2.Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Discount_Amount.ToString())
            {
                lblError.Text = "Invalid discout amount.";
                txtDiscountAmt.Focus();
                txtDiscountAmt.Attributes.Add("onfocus", "javascript:select();");
            }
            if (res == DAL.DAL_New_Bookings.CheckError.Advance_Amount.ToString())
            {
                lblError.Text = "Invalid advance amount.";
                txtAdvance.Focus();
                txtAdvance.Attributes.Add("onfocus", "javascript:select();");
            }
            return res;
        }
        protected void imgBtnGridEntry_Click(object sender, EventArgs e)
        {

            //if (chkToday.Checked == false || chkNextDay.Checked == false)
            //{                
            chkToday.Enabled = false;
            chkNextDay.Enabled = false;
            // chkOldBooking.Enabled = false;
            //}


            string res = CheckError();
            if (res == "Done")
            {
                DataTable dt = new DataTable();
                dt = SetValuesIntoDatatable(Convert.ToInt32(editOption));
                ViewState["Entry"] = dt;
                grdEntry.DataSource = dt;
                grdEntry.DataBind();
                int count = 0;
                if (grdEntry.Rows.Count > 0)
                {
                    for (int i = 0; i < grdEntry.Rows.Count; i++)
                        count += (Convert.ToInt32(((Label)grdEntry.Rows[i].FindControl("lblQty")).Text) * Convert.ToInt32(BAL.BALFactory.Instance.BAL_New_Bookings.CountNoOfSubItem(((Label)grdEntry.Rows[i].FindControl("lblItemName")).Text, Globals.BranchID)));
                }
                else
                    count = 0;
                lblCount.Text = count.ToString();
                SetCommanSetting();
                txtSrTax.Text = (Math.Round(BAL.BALFactory.Instance.BAL_New_Bookings.CalculatAllTax(dt), 2)).ToString();
                ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text = (1 + (grdEntry.Rows.Count)).ToString();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text = "1";
                ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = ItemName;
                ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = Process;
                ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();


                if (rdrPercentage.Checked)
                {
                    rdrPercentage_CheckedChanged(null, null);
                    //       txtDiscount_TextChanged(null, null);
                }
                else
                {
                    rdrPercentage_CheckedChanged(null, null);
                    //       txtDiscountAmt_TextChanged(null, null);
                }
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
                SetBalance();
                hdnItems.Value = "";
                hdnFirstAdd.Value = "Yes";
            }
        }
        public DataTable SetValuesIntoDatatable(int rowNo)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            double rate = 0, rate1 = 0, rate2 = 0;
            string status = string.Empty;
            status = BAL.BALFactory.Instance.BAL_New_Bookings.FindTotalTaxActive(Globals.BranchID);
            ds = BAL.BALFactory.Instance.BAL_New_Bookings.FirstTimeDefaultData(Globals.BranchID);
            dt = (DataTable)ViewState["Entry"];
            if (chkToday.Checked)
            {
                rate = Convert.ToDouble(BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text), Convert.ToDouble(ds.Tables[0].Rows[0][4].ToString())));
                rate1 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate1.Text == "" ? "0" : txtExtraRate1.Text), Convert.ToDouble(ds.Tables[0].Rows[0][4].ToString()));
                rate2 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate2.Text == "" ? "0" : txtExtraRate2.Text), Convert.ToDouble(ds.Tables[0].Rows[0][4].ToString()));
            }
            else
            {
                if (chkNextDay.Checked)
                {
                    rate = Convert.ToDouble(BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text), Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString())));
                    rate1 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate1.Text == "" ? "0" : txtExtraRate1.Text), Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()));
                    rate2 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate2.Text == "" ? "0" : txtExtraRate2.Text), Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()));
                }
                else
                {
                    rate = Convert.ToDouble(((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text);
                    rate1 = Convert.ToDouble(txtExtraRate1.Text == "" ? "0" : txtExtraRate1.Text);
                    rate2 = Convert.ToDouble(txtExtraRate2.Text == "" ? "0" : txtExtraRate2.Text);
                }
            }
            if (editOption == "-1")
            {
                DataRow NewRow = dt.NewRow();
                NewRow[0] = ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text;
                NewRow[1] = (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "" ? "1" : (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "0" ? "1" : ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text));
                NewRow[2] = ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text;
                NewRow[3] = ((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text;
                NewRow[4] = ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Text;
                NewRow[5] = "(" + ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text + "@" + rate + ")";
                NewRow[6] = rate.ToString();
                NewRow[7] = (txtExtraProcess1.Text == "" ? "None" : txtExtraProcess1.Text);
                NewRow[8] = rate1;
                NewRow[9] = (txtExtraProcess2.Text == "" ? "None" : txtExtraProcess2.Text);
                NewRow[10] = rate2;
                NewRow[11] = SumGridEntryAmount();
                NewRow[12] = ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text;
                NewRow[13] = ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text;
                if (status == "True")
                {
                    NewRow[15] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(txtExtraProcess1.Text, rate1, BranchId.Value);
                }
                else
                {
                    NewRow[15] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(txtExtraProcess1.Text, rate1, BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
                }
                if (status == "True")
                {
                    NewRow[16] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(txtExtraProcess2.Text, rate2, BranchId.Value);
                }
                else
                {
                    NewRow[16] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(txtExtraProcess2.Text, rate2, BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
                }
                NewRow[17] = ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Text;
                int tempVar = 0;
                if (txtExtraProcess1.Text != "")
                {
                    NewRow[5] = NewRow[5] + ",(" + txtExtraProcess1.Text + "@" + rate1 + ")";
                    if (txtExtraProcess2.Text != "")
                    {
                        NewRow[5] += ",(" + txtExtraProcess2.Text + "@" + rate2 + ")";
                        tempVar = 1;
                    }
                    txtExtraProcess1.Text = "";
                    txtExtraProcess2.Text = "";
                    txtExtraRate1.Text = "0";
                    txtExtraRate2.Text = "0";
                }
                if (txtExtraProcess2.Text != "")
                {
                    if (tempVar == 0)
                        NewRow[5] = NewRow[5] + ",(" + txtExtraProcess2.Text + "@" + rate2 + ")";
                    txtExtraProcess2.Text = "";
                    txtExtraRate2.Text = "0";
                }
                if (status == "True")
                {
                    NewRow[14] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text, int.Parse((((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "" ? "1" : (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "0" ? "1" : ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text))) * rate, BranchId.Value);
                }
                else
                {
                    NewRow[14] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text, int.Parse((((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "" ? "1" : (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "0" ? "1" : ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text))) * rate, BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
                }
                if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(NewRow[13].ToString(), BranchId.Value) == true)
                    NewRow[18] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * (Convert.ToInt32(NewRow[1].ToString()) * Convert.ToDouble(NewRow[6].ToString()))) / 100, 2));
                else
                    NewRow[18] = "0";
                if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(NewRow[7].ToString(), BranchId.Value) == true)
                    NewRow[19] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * Convert.ToDouble(NewRow[8].ToString())) / 100, 2));
                else
                    NewRow[19] = "0";
                if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(NewRow[9].ToString(), BranchId.Value) == true)
                    NewRow[20] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * Convert.ToDouble(NewRow[10].ToString())) / 100, 2));
                else
                    NewRow[20] = "0";
                dt.Rows.Add(NewRow);
                if (((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text != "")
                    BAL.BALFactory.Instance.BAL_New_Bookings.SaveRemarks(((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text, Globals.BranchID);
            }
            else
            {
                int RowIndex = rowNo;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SNO"].ToString() == RowIndex.ToString())
                    {
                        dt.Rows[i]["SNO"] = ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text;
                        dt.Rows[i]["QTY"] = (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "" ? "1" : (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "0" ? "1" : ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text));
                        dt.Rows[i]["ITEMNAME"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text;
                        dt.Rows[i]["REMARKS"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text;
                        dt.Rows[i]["ITEMNAME1"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text;
                        dt.Rows[i]["PROCESS"] = "(" + ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text + "@" + rate + ")";
                        dt.Rows[i]["PROCESS1"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text;
                        dt.Rows[i]["RATE"] = rate;
                        dt.Rows[i]["AMOUNT"] = SumGridEntryAmount();
                        if (status == "True")
                        {
                            dt.Rows[i]["STEP1Amt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(txtExtraProcess1.Text, rate1, BranchId.Value);
                        }
                        else
                        {
                            dt.Rows[i]["STEP1Amt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(txtExtraProcess1.Text, rate1, BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
                        }
                        if (status == "True")
                        {
                            dt.Rows[i]["STEP2Amt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(txtExtraProcess2.Text, rate2, BranchId.Value);
                        }
                        else
                        {
                            dt.Rows[i]["STEP2Amt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(txtExtraProcess2.Text, rate2, BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
                        }
                        dt.Rows[i]["EXPROC1"] = (txtExtraProcess1.Text == "" ? "None" : txtExtraProcess1.Text);
                        dt.Rows[i]["RATE1"] = rate1;
                        dt.Rows[i]["EXPROC2"] = (txtExtraProcess2.Text == "" ? "None" : txtExtraProcess2.Text);
                        dt.Rows[i]["RATE2"] = rate2;
                        dt.Rows[i]["REMARKS"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text;
                        dt.Rows[i]["COLOR"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Text;
                        dt.Rows[i]["COLORCODE"] = ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Text;
                        int tempVar = 0;
                        if (txtExtraProcess1.Text != "")
                        {
                            dt.Rows[i]["PROCESS"] = dt.Rows[i]["PROCESS"].ToString() + ",(" + txtExtraProcess1.Text + "@" + rate1 + ")";
                            if (txtExtraProcess2.Text != "")
                            {
                                dt.Rows[i]["PROCESS"] += ",(" + txtExtraProcess2.Text + "@" + rate2 + ")";
                                tempVar = 1;
                            }
                            txtExtraProcess1.Text = "";
                            txtExtraProcess2.Text = "";
                            txtExtraRate1.Text = "0";
                            txtExtraRate2.Text = "0";
                        }
                        if (txtExtraProcess2.Text != "")
                        {
                            if (tempVar == 0)
                                dt.Rows[i]["PROCESS"] = dt.Rows[i]["PROCESS"].ToString() + ",(" + txtExtraProcess2.Text + "@" + rate2 + ")";
                            txtExtraProcess2.Text = "";
                            txtExtraRate2.Text = "0";
                        }
                        string a = rate.ToString();
                        if (status == "True")
                        {
                            dt.Rows[i]["STPAmt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text, Convert.ToDouble(Convert.ToInt32(((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text) * rate), BranchId.Value).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["STPAmt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text, Convert.ToDouble(Convert.ToInt32(((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text) * rate), BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text))).ToString();
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text, BranchId.Value) == true)
                        {
                            dt.Rows[i]["DisAmt"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * (Convert.ToInt32(((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text) * rate)) / 100, 2));
                        }
                        else
                            dt.Rows[i]["DisAmt"] = "0";
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(txtExtraProcess1.Text, BranchId.Value) == true)
                            dt.Rows[i]["DisAmt1"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * rate1) / 100, 2));
                        else
                            dt.Rows[i]["DisAmt1"] = "0";
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(txtExtraProcess1.Text, BranchId.Value) == true)
                            dt.Rows[i]["DisAmt2"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * rate2) / 100, 2));
                        else
                            dt.Rows[i]["DisAmt2"] = "0";
                        if (((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text != "")
                            BAL.BALFactory.Instance.BAL_New_Bookings.SaveRemarks(((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text, BranchId.Value);
                        editOption = "-1";
                        break;
                    }
                }
            }

            dt.AcceptChanges();
            int rowIncriment = 1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["SNO"] = rowIncriment.ToString();
                rowIncriment++;
                dt.Rows[i]["QTY"] = dt.Rows[i]["QTY"].ToString();
                dt.Rows[i]["ITEMNAME"] = dt.Rows[i]["ITEMNAME"].ToString();
                dt.Rows[i]["ITEMNAME1"] = dt.Rows[i]["ITEMNAME1"].ToString();
                dt.Rows[i]["PROCESS"] = dt.Rows[i]["PROCESS"].ToString();
                dt.Rows[i]["PROCESS1"] = dt.Rows[i]["PROCESS1"].ToString();
                dt.Rows[i]["RATE"] = dt.Rows[i]["RATE"].ToString();
                dt.Rows[i]["AMOUNT"] = dt.Rows[i]["AMOUNT"].ToString();
                dt.Rows[i]["STEP1Amt"] = dt.Rows[i]["STEP1Amt"].ToString();
                dt.Rows[i]["STEP2Amt"] = dt.Rows[i]["STEP2Amt"].ToString();
                dt.Rows[i]["EXPROC1"] = dt.Rows[i]["EXPROC1"].ToString();
                dt.Rows[i]["RATE1"] = dt.Rows[i]["RATE1"].ToString();
                dt.Rows[i]["EXPROC2"] = dt.Rows[i]["EXPROC2"].ToString();
                dt.Rows[i]["RATE2"] = dt.Rows[i]["RATE2"].ToString();
                dt.Rows[i]["REMARKS"] = dt.Rows[i]["REMARKS"].ToString();
                dt.Rows[i]["COLOR"] = dt.Rows[i]["COLOR"].ToString();
                dt.Rows[i]["COLORCODE"] = dt.Rows[i]["COLORCODE"].ToString();
                dt.Rows[i]["STPAmt"] = dt.Rows[i]["STPAmt"].ToString();
                if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(dt.Rows[i]["PROCESS1"].ToString(), BranchId.Value) == true)
                    dt.Rows[i]["DisAmt"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * (Convert.ToInt32(dt.Rows[i]["QTY"].ToString()) * Convert.ToDouble(dt.Rows[i]["RATE"].ToString()))) / 100, 2));
                else
                    dt.Rows[i]["DisAmt"] = "0";
                if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(dt.Rows[i]["EXPROC1"].ToString(), BranchId.Value) == true)
                    dt.Rows[i]["DisAmt1"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * Convert.ToDouble(dt.Rows[i]["RATE1"].ToString())) / 100, 2));
                else
                    dt.Rows[i]["DisAmt1"] = "0";
                if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(dt.Rows[i]["EXPROC2"].ToString(), BranchId.Value) == true)
                    dt.Rows[i]["DisAmt2"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * Convert.ToDouble(dt.Rows[i]["RATE2"].ToString())) / 100, 2));
                else
                    dt.Rows[i]["DisAmt2"] = "0";
                editOption = "-1";

            }
            txtWholeRemark.Text = "";
            dt.AcceptChanges();
            dt.DefaultView.Sort = "SNO DESC";
            editOption = "-1";

            return dt;
        }
        public string SumGridEntryAmount()
        {
            string amount = "0";
            DataSet ds = new DataSet();
            double rate = 0, rate1 = 0, rate2 = 0;
            ds = BAL.BALFactory.Instance.BAL_New_Bookings.FirstTimeDefaultData(Globals.BranchID);
            if (chkToday.Checked)
            {
                rate = Convert.ToDouble(BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text), Convert.ToDouble(ds.Tables[0].Rows[0][4].ToString())));
                rate1 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate1.Text == "" ? "0" : txtExtraRate1.Text), Convert.ToDouble(ds.Tables[0].Rows[0][4].ToString()));
                rate2 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate2.Text == "" ? "0" : txtExtraRate2.Text), Convert.ToDouble(ds.Tables[0].Rows[0][4].ToString()));
            }
            else
            {
                if (chkNextDay.Checked)
                {
                    rate = Convert.ToDouble(BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text), Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString())));
                    rate1 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate1.Text == "" ? "0" : txtExtraRate1.Text), Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()));
                    rate2 = BAL.BALFactory.Instance.BAL_New_Bookings.RetrunRate(Convert.ToDouble(txtExtraRate2.Text == "" ? "0" : txtExtraRate2.Text), Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()));
                }
                else
                {
                    rate = Convert.ToDouble(((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text);
                    rate1 = Convert.ToDouble(txtExtraRate1.Text == "" ? "0" : txtExtraRate1.Text);
                    rate2 = Convert.ToDouble(txtExtraRate2.Text == "" ? "0" : txtExtraRate2.Text);
                }
            }
            try
            {
                amount = Math.Round(rate1 + rate2 + ((Convert.ToInt32((((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "" ? "1" : (((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text == "0" ? "1" : ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text))) * rate)), 2).ToString();
            }
            catch (Exception) { }
            return amount;
        }
        protected void txtExtraProcess1_TextChanged(object sender, EventArgs e)
        {
            txtExtraRate1.Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, txtExtraProcess1.Text, BranchId.Value)).ToString();
            SetCommanSetting();
            txtExtraRate1.Focus();
            txtExtraRate1.Attributes.Add("onfocus", "javascript:select();");
        }
        protected void txtExtraProcess2_TextChanged(object sender, EventArgs e)
        {
            txtExtraRate2.Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, txtExtraProcess2.Text, BranchId.Value)).ToString();
            SetCommanSetting();
            txtExtraRate2.Focus();
            txtExtraRate2.Attributes.Add("onfocus", "javascript:select();");
        }
        protected void txtProcess_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();
                ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtRate")).Focus();
                ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtRate")).Attributes.Add("onfocus", "javascript:select();");
            }
            catch (Exception) { }
        }
        protected void btnSaveBooking_Click(object sender, EventArgs e)
        {
            string res1 = "Done";
            if (res1 == "Done")
            {
                if (CustId == "" || CustId == "0")
                {
                    CustId = hdnCustCode.Value;
                }
                if (hdnCustCode.Value != "" && hdnCustCode.Value != "0")
                {
                    CustId = hdnCustCode.Value;
                }
                if (CustId != "")
                {
                    DateTime dtBook;
                    DateTime dtDue;
                    try
                    {
                        dtBook = DateTime.Parse(txtDate.Text);
                    }
                    catch (Exception)
                    {
                        lblError.Text = "Invalid booking Date.";
                        return;
                    }
                    try
                    {
                        dtDue = DateTime.Parse(txtDueDate.Text);
                    }
                    catch (Exception)
                    {
                        lblError.Text = "Invalid due Date.";
                        return;
                    }
                    if (dtBook > dtDue)
                    {
                        lblError.Text = "Due date can not be earlier than booking date";
                        txtDueDate.Focus();
                        txtDueDate.Attributes.Add("onfocus", "javascript:select();");
                        return;
                    }
                    Obj = SetValueIntoPropertiesSaveTime();

                    var gid = Globals.BranchID;
                    Task.Factory.StartNew(() => BAL.BALFactory.Instance.BAL_New_Bookings.UpdateCustomer(CustId, lblMobileNo.Text, lblEmailId.Text, drpCommLbl.SelectedValue, gid));

                    // save the booking in entbookings
                    Obj.BID = BranchId.Value;
                    string res = string.Empty;
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    dt = (DataTable)ViewState["Entry"];
                    if (hdnUpdate.Value == "0")
                        ds = BAL.BALFactory.Instance.BAL_New_Bookings.SaveBooking(Obj);
                    else
                    {
                        Obj.BOOKINGNUMBER = txtEdit.Text.ToUpper();
                       // Obj.BookingPrefix=drpBookingPreFix.SelectedItem.Value;
                        /// <Mac>
                        /// Back up bookings
                        /*
                        BackUpBookings(Obj.BOOKINGNUMBER, Globals.BranchID);
                        /// <Mac>
                        /// Back up booking detail
                        //if (isInEditMode.Value == "true")
                        BackUpBookingDetails(Obj.BOOKINGNUMBER, Globals.BranchID);
                        BackUpPayment(Obj.BOOKINGNUMBER, Globals.BranchID);
                         * */
                        BackUpWholeBooking(Obj.BOOKINGNUMBER, Globals.BranchID);
                        ds = BAL.BALFactory.Instance.BAL_New_Bookings.EditBooking(Obj);
                    }

                    // save the booking in entbookingdetails
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return;

                    var strBookingNumber = ds.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim() + ds.Tables[0].Rows[0]["Bookingnumber"].ToString();
                    if (strBookingNumber == null || strBookingNumber == "")
                        return;

                    // string strItemName = "ItemName_";
                    var grdEntryCount = Int32.Parse(hdnGrdRowCount.Value.ToString());


                    // find all the taxes
                    string[] st = hdnAllTax.Value.Split('_');
                    //string[] stNew;
                    //// check if st contains null
                    //foreach (var item in st)
                    //{
                    //    if (item == "")
                    //    {
                    //        stNew = new string[st.Length - 1];
                    //    }
                    //}
                    // for each row we can access as 0, 1, etc
                    string[] allRowData = hdnAllGridData.Value.Split('_');
                    string prcArrary = string.Empty;
                    var dataForPackageConsume = new StringBuilder();
                    for (int i = 0; i <= grdEntryCount - 1; i++)
                    {
                        Obj = new DTO.NewBooking();
                        Obj.ISN = (i + 1).ToString();
                        Obj.BOOKINGNUMBER = strBookingNumber;
                        Obj.ItemName = allRowData[i].Split(':')[2];
                        Obj.ItemTotalQuantity = allRowData[i].Split(':')[1];
                        // this will hold the following format
                        // prc is in the form of (prc@rate),(prc1@rate1),(prc2@rate2) and so on
                        prcArrary = allRowData[i].Split(':')[3];
                        // this will split them and return each value
                        var ls = splitPrcRateFromArray(prcArrary);
                        // now its just point of splitting the data
                        // ls will hold data as
                        // 0=> prc 1=> rate 2=> prc1 3=>rate1 4=> prc2 5=> rate2
                        Obj.ItemProcessType = ls.ElementAt(0);
                        Obj.ItemQuantityAndRate = (allRowData[i].Split(':')[1] + "@" + string.Format("{0:0.00}", float.Parse(ls.ElementAt(1))));
                        Obj.ItemExtraProcessType1 = ls.ElementAt(2);
                        Obj.ItemExtraProcessRate1 = string.Format("{0:0.00}", float.Parse(ls.ElementAt(3)));
                        Obj.ItemExtraProcessType2 = ls.ElementAt(4);
                        Obj.ItemExtraProcessRate2 = string.Format("{0:0.00}", float.Parse(ls.ElementAt(5)));
                        Obj.ItemSubTotal = allRowData[i].Split(':')[6];
                        Obj.ItemRemark = allRowData[i].Split(':')[4];
                        Obj.Color = allRowData[i].Split(':')[5];
                        // this holds the data of all tax in the format tax:tax1:tax2_tax:tax1:tax2
                        var tx = hdnAllTax.Value.Split('`')[i];
                        var txCurRow = tx.Split('_');
                        // prc1, all taxes

                        /*
                        Obj.STPAmt = Math.Round(float.Parse(txCurRow[0].Split(':')[0]), 3).ToString();
                        Obj.STPAmtEcess = Math.Round(float.Parse(txCurRow[0].Split(':')[1]), 3).ToString();
                        Obj.STPAmtSHECess = Math.Round(float.Parse(txCurRow[0].Split(':')[2]), 3).ToString();
                        // prc1, all taxes
                        Obj.STEP1Amt = Math.Round(float.Parse(txCurRow[1].Split(':')[0]), 3).ToString();
                        Obj.STP1AmtEcess = Math.Round(float.Parse(txCurRow[1].Split(':')[1]), 3).ToString();
                        Obj.STPAmt1SHECess = Math.Round(float.Parse(txCurRow[1].Split(':')[2]), 3).ToString();
                        // prc1, all taxes
                        Obj.STEP2Amt = Math.Round(float.Parse(txCurRow[2].Split(':')[0]), 3).ToString();
                        Obj.STP2AmtEcess = Math.Round(float.Parse(txCurRow[2].Split(':')[1]), 3).ToString();
                        Obj.STPAmt2SHECess = Math.Round(float.Parse(txCurRow[2].Split(':')[2]), 3).ToString();
                        */


                        Obj.STPAmt = float.Parse(txCurRow[0].Split(':')[0]).ToString();
                        Obj.STPAmtEcess = float.Parse(txCurRow[0].Split(':')[1]).ToString();
                        Obj.STPAmtSHECess = float.Parse(txCurRow[0].Split(':')[2]).ToString();
                        // prc1, all taxes
                        Obj.STEP1Amt = float.Parse(txCurRow[1].Split(':')[0]).ToString();
                        Obj.STP1AmtEcess = float.Parse(txCurRow[1].Split(':')[1]).ToString();
                        Obj.STPAmt1SHECess = float.Parse(txCurRow[1].Split(':')[2]).ToString();
                        // prc1, all taxes
                        Obj.STEP2Amt = float.Parse(txCurRow[2].Split(':')[0]).ToString();
                        Obj.STP2AmtEcess = float.Parse(txCurRow[2].Split(':')[1]).ToString();
                        Obj.STPAmt2SHECess = float.Parse(txCurRow[2].Split(':')[2]).ToString();
                        //////////////////////////////////////



                        /******** ITEM PIECES **********/
                        var allItemUnits = hdnItemUnit.Value.Split('_');
                        // for itemname
                        var itmTemp = string.Empty;
                        // find the last index of space
                        var index = -1;

                        if (allItemUnits[i] == "Pcs")
                        {
                            Obj.UnitDesc = "";
                        }
                        else if (allItemUnits[i] == "P")
                        {
                            Obj.UnitDesc = hdnItemUnitValue.Value.Split('_')[i].Split(':')[1] + "PL";
                            if (Obj.UnitDesc.Contains("PLPL"))
                            {
                                Obj.UnitDesc = Obj.UnitDesc.Substring(0, Obj.UnitDesc.Length - 2);
                            }
                            // if its PL, change the item name, so that it doesn't saves like 'Curtain 10PL'
                            itmTemp = Obj.ItemName;
                            // find the last index of space
                            index = itmTemp.LastIndexOf(" ");
                            itmTemp = itmTemp.Substring(0, index);
                            Obj.ItemName = itmTemp;

                        }
                        else if (allItemUnits[i] == "LB")
                        {
                            Obj.UnitDesc = hdnItemUnitValue.Value.Split('_')[i].Split(':')[0];
                            // if its PL, change the item name, so that it doesn't saves like 'Curtain 10PL'
                            itmTemp = Obj.ItemName;
                            // find the last index of space
                            index = itmTemp.LastIndexOf(" ");
                            itmTemp = itmTemp.Substring(0, index);
                            Obj.ItemName = itmTemp;
                        }


                        /******** ITEM PIECES **********/

                        Obj.BID = BranchId.Value;                       
                        dataForPackageConsume.Append(Obj.ItemName + ":");
                        dataForPackageConsume.Append(Obj.ItemTotalQuantity + "_");
                        res = BAL.BALFactory.Instance.BAL_New_Bookings.SaveBookingDetails(Obj);
                    }
                    if (!string.IsNullOrEmpty(dataForPackageConsume.ToString()))
                        dataForPackageConsume.Remove(dataForPackageConsume.Length - 1, 1);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        if (res == "Record Saved")
                        {
                            Obj = new DTO.NewBooking();
                            Obj = SetValueIntoPropertiesSaveTime();
                            Obj.BOOKINGNUMBER =ds.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim() + ds.Tables[0].Rows[0]["BookingNumber"].ToString();
                            Obj.BID = BranchId.Value;                           
                            Obj.UserBookingId = Globals.UserName;
                            res = BAL.BALFactory.Instance.BAL_New_Bookings.SaveAccountEntries(Obj);
                            if (res == "Record Saved")
                            {
                                res = BAL.BALFactory.Instance.BAL_New_Bookings.SaveBarCode(Obj);
                                if (res == "Record Saved")
                                {
                                    // if package type is "Qty / Item" save in consumed table"
                                    if ((hdnPackageTypeRecNo.Value.Split(':')[0] == "Qty / Item" || hdnPackageTypeRecNo.Value.Split(':')[0] == "Flat Qty") && Obj.PaymentMode == "Package")
                                        SaveDataInPackageConsume(dataForPackageConsume.ToString());


                                    Deafaultsetting();
                                    if (chkSendsms.Checked)
                                    {
                                        var tmp = Globals.BranchID;
                                        Task t = Task.Factory.StartNew
                                                 (
                                                    () => { AppClass.GoMsg(tmp, ds.Tables[0].Rows[0]["BookingPreFix"].ToString().Trim() + ds.Tables[0].Rows[0]["BookingNumber"].ToString(), drpsmstemplate.SelectedValue); }
                                                 );
                                    }

                                    if (hdnUpdate.Value != "0")
                                    {
                                        SaveInvoiceHistoryData();
                                    }
                                    else if (hdnUpdate.Value == "0")
                                    {
                                        var strBID = Globals.BranchID;
                                        var strUID = Globals.UserName;
                                        Task t = Task.Factory.StartNew
                                        (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(strBookingNumber, strUID, strBID, bookingSrcMsg.BookingCreation, ScreenName.BookingScreen,1); }
                                        );
                                    }

                                    # region buttonChecked
                                    var ss = sender as Button;
                                    if (ss != null)
                                    {
                                        // ClientScript.RegisterStartupScript(this.GetType(), "closewnd", String.Format("<script>window.close();</script>')"));
                                        // if we are coming from save print
                                        // we will directly print the page
                                        // if btnEmail is checked, then send it in queryString
                                        var sendEmail = string.Empty;
                                        var sendPrefix = string.Empty;                                       
                                        sendPrefix = ds.Tables[0].Rows[0]["BookingPreFix"].ToString().Trim() + ds.Tables[0].Rows[0]["BookingNumber"].ToString();
                                        string strPrinterName = string.Empty;
                                        strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
                                        if (chkEmail.Checked)
                                            sendEmail = "&Email=true";
                                        if (ss.ID == "btnSavePrint")
                                        {
                                            // BasePage.OpenWindow(this, "../Bookings/BookingSlip.aspx?BN=" + ds.Tables[0].Rows[0]["BookingNumber"].ToString() + "-1" + date[0].ToString() + "&DirectPrint=true");
                                            Response.Write(PrjClass.GetAndGo("Booking Accepted. Booking Number is : " + ds.Tables[0].Rows[0]["BookingPreFix"].ToString() + "" + ds.Tables[0].Rows[0]["BookingNumber"].ToString(), "../" + strPrinterName + "/BookingSlip.aspx?BN=" + sendPrefix + "-1" + date[0].ToString() + "&DirectPrint=true&RedirectBack=true&closeWindow=true" + sendEmail));
                                          var urlToOpen = "../" + strPrinterName + "/BookingSlip.aspx?BN=" + sendPrefix + "-1" + date[0].ToString() + "&DirectPrint=true&RedirectBack=true&closeWindow=true" + sendEmail;
                                          OpenUrlFromBasePage(urlToOpen);
                                        }
                                        // if user if printing barcode
                                        else if (ss.ID == "btnPrintBarCode")
                                        {
                                            // BasePage.OpenWindow(this, "../Reports/Barcodet.aspx?bookingNo=" + ds.Tables[0].Rows[0]["BookingNumber"].ToString() + "&id=0&PrintBarCode=true");
                                            Response.Write(PrjClass.GetAndGo("Booking Accepted. Booking Number is : " + ds.Tables[0].Rows[0]["BookingPreFix"].ToString() + "" + ds.Tables[0].Rows[0]["BookingNumber"].ToString(), "../Reports/Barcodet.aspx?bookingNo=" + sendPrefix + "&id=0&Index=0&PrintBarCode=true&RedirectBack=true&CloseWindow=true" + sendEmail));
                                            //var urlToOpen = "../Reports/Barcodet.aspx?bookingNo=" + sendPrefix + "&id=0&Index=0&PrintBarCode=true&RedirectBack=true&CloseWindow=true" + sendEmail;
                                            //OpenUrlFromBasePage(urlToOpen);
                                        }
                                        // the user is printing the slip and barcode
                                        // if user if printing barcode
                                        else if (ss.ID == "btnSavePrintBarCode")
                                        {                                            
                                            Response.Write(PrjClass.GetAndGo("Booking Accepted. Booking Number is : " + ds.Tables[0].Rows[0]["BookingPreFix"].ToString() + "" + ds.Tables[0].Rows[0]["BookingNumber"].ToString(), "../" + strPrinterName + "/BookingSlip.aspx?BN=" + sendPrefix + "-1" + date[0].ToString() + "&DirectPrint=true&RedirectBack=true&PrintBarCode=true" + sendEmail));
                                            //var urlToOpen = "../" + strPrinterName + "/BookingSlip.aspx?BN=" + sendPrefix + "-1" + date[0].ToString() + "&DirectPrint=true&RedirectBack=true&PrintBarCode=true" + sendEmail;
                                            //OpenUrlFromBasePage(urlToOpen);
                                        }
                                        else
                                        {
                                            Response.Write(PrjClass.GetAndGo("Booking Accepted. Booking Number is : " + ds.Tables[0].Rows[0]["BookingPreFix"].ToString() + "" + ds.Tables[0].Rows[0]["BookingNumber"].ToString(), "../" + strPrinterName + "/BookingSlip.aspx?BN=" + sendPrefix + "-1" + date[0].ToString() + sendEmail));
                                            //var urlToOpen = "../" + strPrinterName + "/BookingSlip.aspx?BN=" + sendPrefix + "-1" + date[0].ToString() + sendEmail;
                                            //OpenUrlFromBasePage(urlToOpen);
                                        }
                                    }
                                    # endregion
                                    else
                                        lblError.Text = res;
                                }
                            }
                        }
                    }
                    hdnUpdate.Value = "0";
                    SetCommanSetting();                    
                }
                else
                {
                    lblError.Text = "Please refresh your browser and select a customer.";
                    txtCustomerName.Focus();
                }
                ((Button)grdEntry.HeaderRow.FindControl("imgBtnGridEntry")).Text = "Add";
            }
        }
        protected void OpenUrlFromBasePage(string urlToOpen)
        {
            OpenWindow(this.Page, urlToOpen);
        }
        private void SaveDataInPackageConsume(string dataForPackageConsume)
        {

            // if editing, remove the previous rows
            // rather, set the isDeleted to true
            /*
            using (var dbContext = new DataContext(PrjClass.sqlConStr))
            {
                var cc = from a in dbContext.
                         select a;
            }
            */
            string strNewBookingNo = string.Empty;
            string strNewBookingPrefix = string.Empty;
            DataSet dsBookingPrefix = new DataSet();
            dsBookingPrefix = BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(Obj.BOOKINGNUMBER);
            strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim();
            strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString().Trim();

            if (hdnUpdate.Value == "1")
            {
                var commandText = "UPDATE EntPackageConsume SET IsDeleted = 1 WHERE BookingNumber = '" + strNewBookingNo +
                                        "' AND BranchId = " + Globals.BranchID + "'and BookingPrefix='" + strNewBookingPrefix;
                PrjClass.ExecuteNonQuery(new SqlCommand { CommandText = commandText });
            }

            var dt = new DataTable();
            dt.Columns.Add("TID");
            dt.Columns.Add("RID");
            dt.Columns.Add("AssignId");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ConsumeQty");
            dt.Columns.Add("ConsumeDate");
            dt.Columns.Add("BookingNumber");
            dt.Columns.Add("BalQty");
            dt.Columns.Add("BranchId");
            dt.Columns.Add("BookingPrefix");

            /**** WARNING! REGEX AHEAD ****/
            var allItemsAry = Regex.Replace(dataForPackageConsume, "[\\:]+\\d+", "").Split('_');
            var allQtyAry = Regex.Replace(dataForPackageConsume, "[^_:]+:", "").Split('_'); // could also use [^\d+:]+|:(?!\d+)|(?<!:)\d+

            var items = new List<string>();
            var qty = new List<string>();
            var idx = 0;
            for (int i = 0; i < allItemsAry.Length; i++)
            {
                if (!items.Contains(allItemsAry[i]))
                {
                    items.Add(allItemsAry[i]);
                    qty.Add(allQtyAry[i]);
                }
                else
                {
                    idx = items.IndexOf(allItemsAry[i]);
                    qty[idx] = (int.Parse(qty[idx]) + int.Parse(allQtyAry[i])).ToString();
                }
            }


            var RcnNo = hdnPackageTypeRecNo.Value.Split(':')[1];
            var assignId = hdnAssignId.Value;
            var tid = PrjClass.getNewIDAccordingBID("entPackageConsume", "TID", Globals.BranchID);

            for (var i = 0; i < items.Count; i++)
            {
                var dr = dt.NewRow();
                dr["TID"] = tid + i;
                dr["RID"] = RcnNo;
                dr["AssignId"] = assignId;
                dr["ItemName"] = items[i];
                dr["ConsumeQty"] = qty[i];
                dr["ConsumeDate"] = txtDate.Text;
                dr["BookingNumber"] = strNewBookingNo;
                dr["BalQty"] = 0;
                dr["BranchId"] = Globals.BranchID;
                dr["BookingPrefix"] = strNewBookingPrefix;
                dt.Rows.Add(dr);
            }
            AppClass.SaveBulkDataThroughDataTable("EntPackageConsume", dt);

        }


        // this splits the prc and rate from the array
        protected List<string> splitPrcRateFromArray(string argPrcRateArray)
        {
            string firstPrc, firstRate;
            string SecondPrc = string.Empty;
            string SecondRate = string.Empty; ;
            string ThirdPrc = string.Empty;
            string ThirdRate = string.Empty;
            List<string> lstResult = new List<string>();

            if (argPrcRateArray.Contains(','))
            {
                // there is more than 1 process
                var PrcNameAndRateArray = argPrcRateArray.Split(',');
                // split 3 times
                // first rate and prc
                firstPrc = PrcNameAndRateArray[0].Split('@')[0].Substring(1) + "";
                firstRate = PrcNameAndRateArray[0].Split('@')[1] + "";
                firstRate = firstRate.Substring(0, firstRate.Length - 1);

                // check for the count 
                if (PrcNameAndRateArray.Length > 1)
                {
                    // second rate and prc
                    SecondPrc = PrcNameAndRateArray[1].Split('@')[0].Substring(1);
                    SecondRate = PrcNameAndRateArray[1].Split('@')[1];
                    SecondRate = SecondRate.Substring(0, SecondRate.Length - 1);
                }

                // again check
                if (PrcNameAndRateArray.Length > 2)
                {
                    // third rate and prc
                    ThirdPrc = PrcNameAndRateArray[2].Split('@')[0].Substring(1);
                    ThirdRate = PrcNameAndRateArray[2].Split('@')[1];
                    ThirdRate = ThirdRate.Substring(0, ThirdRate.Length - 1);
                }
            }
            else
            {
                // there is just one item
                firstPrc = argPrcRateArray.Split('@')[0].Substring(1) + "";
                firstRate = argPrcRateArray.Split('@')[1] + "";
                firstRate = firstRate.Substring(0, firstRate.Length - 1);
            }

            // check for null and etc now
            if (firstPrc == "") { throw new Exception("First Process can't be null"); }
            if (SecondPrc == "") { SecondPrc = "None"; }
            if (ThirdPrc == "") { ThirdPrc = "None"; }
            if (firstRate == "") { firstRate = "0"; }
            if (SecondRate == "") { SecondRate = "0"; }
            if (ThirdRate == "") { ThirdRate = "0"; }

            lstResult.Add(firstPrc);
            lstResult.Add(firstRate);
            lstResult.Add(SecondPrc);
            lstResult.Add(SecondRate);
            lstResult.Add(ThirdPrc);
            lstResult.Add(ThirdRate);

            return lstResult;
        }

        // this saves the booking and prints it
        protected void btnSavePrint_Click(object sender, EventArgs e)
        {
            // add item(s) to http context, so that we know its coming from direct printing
            //if (HttpContext.Current.Items.Contains("DirectPrint"))
            //{
            //    HttpContext.Current.Items["DirectPrint"] = "true";
            //}
            //else
            //{
            //    HttpContext.Current.Items.Add("DirectPrint", "true");
            //}
            // HttpCookie ck = new HttpCookie("DirectPrint");
            // Response.Cookies.Add(new HttpCookie("DirectPrint", "true"));
            // call the button print
            btnSaveBooking_Click(btnSavePrint, e);
        }

        protected void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            btnSaveBooking_Click(btnPrintBarCode, e);
        }

        protected void btnSavePrintBarCode_Click(object sender, EventArgs e)
        {
            btnSaveBooking_Click(btnSavePrintBarCode, e);
        }

        // this will check if the click occured by print button or by direct clicking
        //protected void SetContext(object sender)
        //{
        //    // cast sender as button
        //    //Button b = sender as Button;
        //    //if (b.ID == "btnSavePrint")
        //    //{
        //    //    HttpContext.Current.Items
        //    //}
        //}

        public void SetCommanSetting()
        {
            //((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('hdnOption').value=2;return true;} if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=2;document.getElementById('grdEntry_ctl01_txtRemarks').focus();return false;}}else {return true}; ");
            //((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('hdnOption').value=2;return true;} if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=2;document.getElementById('grdEntry_ctl01_txtColor').focus();return false;}}else {return true}; ");
            //((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('hdnOption').value=2;return true;} if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=2;document.getElementById('grdEntry_ctl01_txtProcess').select();return false;}}else {return true}; ");
            //((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Attributes.Add("onkeydown", "if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('hdnOption').value=3;return true;};if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=3;document.getElementById('grdEntry_ctl01_txtRate').focus();document.getElementById('grdEntry_ctl01_txtRate').select();return false;}} else {return true}; ");
            //((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('grdEntry_ctl01_imgBtnGridEntry').click();return false;}} else {return true}; ");
            //txtCNameSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + txtCustomerName.ID + "').value='';$find('ModalPopupExtender5').hide();document.getElementById('" + txtCustomerName.ID + "').focus();return false;}} else {return true}; ");
            //txtAddress.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + txtCustomerName.ID + "').value='';$find('ModalPopupExtender5').hide();document.getElementById('" + txtCustomerName.ID + "').focus();return false;}} else {return true}; ");
            //txtPhoneNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + txtCustomerName.ID + "').value='';$find('ModalPopupExtender5').hide();document.getElementById('" + txtCustomerName.ID + "').focus();return false;}} else {return true}; ");
            //txtCustomerName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('hdnOption').value=5;document.getElementById('grdEntry_ctl01_txtQty').focus();document.getElementById('grdEntry_ctl01_txtQty').select();return false;}if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=5;document.getElementById('grdEntry_ctl01_txtQty').focus();document.getElementById('grdEntry_ctl01_txtQty').select();return false;}} else {return true}; ");
            //// txtCurrentDue.Text = ((Label)grdEntry.HeaderRow.FindControl("lblHAmount")).Text = Math.Round(Convert.ToDouble(BAL.BALFactory.Instance.BAL_New_Bookings.GridTotal(grdEntry, "lblAmount")), 2).ToString();
            //txtEdit.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=2;document.getElementById('" + btnEdit.ID + "').click();document.getElementById('txtCustomerName').focus();return false;}} else {return true}; ");
            //txtCName.Attributes.Add("onkeydown", "if(document.getElementById('" + txtCName.ID + "').value==''){if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').click();return false;}} else {return true};}else{if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + txtCAddress.ID + "').focus();return false;}}}");
            //txtCAddress.Attributes.Add("onkeydown", "if(document.getElementById('" + txtCAddress.ID + "').value==''){if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').click();return false;}} else {return false};}else{if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnOkay.ID + "').click();return false;}}}");
            //txtMobile.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').focus();return true;}} else {return false};");
            //txtAreaLocaton.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').focus();return true;}} else {return false};");
            //txtRemarks1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').focus();return true;}} else {return false};");
            //txtBDate.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').focus();return true;}} else {return false};");
            //txtADate.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnOkay.ID + "').focus();return true;}} else {return false};");
            //txtNewPriority.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=6;document.getElementById('" + btnAddNewPriority.ID + "').focus();return true;}} else {return false};");
            //txtExtraRate1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnExtraProcSave.ID + "').focus();return true;}} else {return false};");
            //txtExtraRate2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnExtraProcSave.ID + "').focus();return true;}} else {return false};");
            //txtItemName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnItemSave.ID + "').focus();return true;}} else {return false};");
            ////txtItemSubQty.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnItemSave.ID + "').focus();return true;}} else {return false};");
            //txtItemCode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnItemSave.ID + "').focus();return true;}} else {return false};");
            ////txtSubItem1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnItemSave.ID + "').focus();return true;}} else {return false};");
            ////txtSubItem2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnItemSave.ID + "').focus();return true;}} else {return false};");
            ////txtSubItem3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnItemSave.ID + "').focus();return true;}} else {return false};");
            //((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('hdnOption').value=2;return true;} if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('hdnOption').value=2;document.getElementById('grdEntry_ctl01_txtName').focus();document.getElementById('grdEntry_ctl01_txtName').select();return false;}}else {return true}; ");

        }
        protected void btnExtraProcSave_Click(object sender, EventArgs e)
        {
            ModalPopupExtender8.Hide();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Attributes.Add("onfocus()", "javascript:select();");
        }
        public void SetBalance()
        {
            //double diacountAmt = 0, disAmt = 0;
            //int i = 0;
            //string status = string.Empty;
            //DataTable dt = new DataTable();
            //dt = (DataTable)ViewState["Entry"];
            //if (dt.Rows.Count > 0)
            //{
            //    try
            //    {
            //        if (rdrPercentage.Checked)
            //        {
            //            for (i = 0; i < dt.Rows.Count; i++)
            //            {
            //                disAmt += Math.Round(((Convert.ToDouble(dt.Rows[i]["DisAmt"].ToString()) + Convert.ToDouble(dt.Rows[i]["DisAmt1"].ToString()) + Convert.ToDouble(dt.Rows[i]["DisAmt2"].ToString()))), 2);
            //                txtDiscountAmt.Text = Math.Round(disAmt, 2).ToString();
            //                diacountAmt = Convert.ToDouble(txtDiscountAmt.Text);
            //            }
            //        }
            //        else
            //            diacountAmt = Convert.ToDouble(txtDiscountAmt.Text);
            //    }
            //    catch (Exception)
            //    {
            //        if (rdrPercentage.Checked)
            //        {
            //            disAmt += Math.Round(((Convert.ToDouble(txtDiscountAmt.Text))), 2);
            //            txtDiscountAmt.Text = disAmt.ToString();
            //            diacountAmt = Convert.ToDouble(txtDiscountAmt.Text);
            //        }
            //        else
            //            diacountAmt = Convert.ToDouble(txtDiscountAmt.Text);
            //    }
            //}
            //else
            //{
            //    txtDiscountAmt.Text = "0";
            //}

            // status = BAL.BALFactory.Instance.BAL_New_Bookings.FindTotalTaxActive(Globals.BranchID);
            //if (status == "True")
            //{
            //  txtTotal.Text = Math.Round((Convert.ToDouble(txtCurrentDue.Text) - diacountAmt) + Convert.ToDouble(txtSrTax.Text == "" ? "0" : txtSrTax.Text), 2).ToString();
            //}
            //else
            //{

            //    double tax = Math.Round((Convert.ToDouble(txtCurrentDue.Text) - diacountAmt) * 12.36 / 100, 2);
            //    txtSrTax.Text = tax.ToString();
            //    txtTotal.Text = Math.Round((Convert.ToDouble(txtCurrentDue.Text) - diacountAmt) + Convert.ToDouble(txtSrTax.Text == "" ? "0" : txtSrTax.Text), 2).ToString();
            //}
            //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(BranchId.Value) == "True")
            //    txtBalance.Text = Math.Round(Convert.ToDouble((Convert.ToDouble((txtTotal.Text == "" ? "0" : txtTotal.Text))) - Convert.ToDouble((txtAdvance.Text == "" ? "0" : txtAdvance.Text))), 2).ToString();
            //else
            //    txtBalance.Text = Convert.ToInt32(Math.Round(Convert.ToDouble((Convert.ToDouble((txtTotal.Text == "" ? "0" : txtTotal.Text))) - Convert.ToDouble((txtAdvance.Text == "" ? "0" : txtAdvance.Text))), 2)).ToString();
        }
        //protected void txtDiscount_TextChanged(object sender, EventArgs e)
        //{
        //    int rowIncriment = 1;
        //    DataTable dt = new DataTable();
        //    string status = string.Empty;
        //    status = BAL.BALFactory.Instance.BAL_New_Bookings.FindTotalTaxActive(Globals.BranchID);
        //    dt = (DataTable)ViewState["Entry"];
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dt.Rows[i]["SNO"] = rowIncriment.ToString();
        //        rowIncriment++;
        //        dt.Rows[i]["QTY"] = dt.Rows[i]["QTY"].ToString();
        //        dt.Rows[i]["ITEMNAME"] = dt.Rows[i]["ITEMNAME"].ToString();
        //        dt.Rows[i]["ITEMNAME1"] = dt.Rows[i]["ITEMNAME1"].ToString();
        //        dt.Rows[i]["PROCESS"] = dt.Rows[i]["PROCESS"].ToString();
        //        dt.Rows[i]["PROCESS1"] = dt.Rows[i]["PROCESS1"].ToString();
        //        dt.Rows[i]["RATE"] = dt.Rows[i]["RATE"].ToString();
        //        dt.Rows[i]["AMOUNT"] = dt.Rows[i]["AMOUNT"].ToString();
        //        if (status == "True")
        //        {
        //            dt.Rows[i]["STEP1Amt"] = dt.Rows[i]["STEP1Amt"].ToString();
        //        }
        //        else
        //        {
        //            dt.Rows[i]["STEP1Amt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(dt.Rows[i]["EXPROC1"].ToString(), Convert.ToDouble(dt.Rows[i]["RATE1"].ToString()), BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
        //        }
        //        if (status == "True")
        //        {
        //            dt.Rows[i]["STEP2Amt"] = dt.Rows[i]["STEP2Amt"].ToString();
        //        }
        //        else
        //        {
        //            dt.Rows[i]["STEP2Amt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(dt.Rows[i]["EXPROC2"].ToString(), Convert.ToDouble(dt.Rows[i]["RATE2"].ToString()), BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
        //        }
        //        dt.Rows[i]["EXPROC1"] = dt.Rows[i]["EXPROC1"].ToString();
        //        dt.Rows[i]["RATE1"] = dt.Rows[i]["RATE1"].ToString();
        //        dt.Rows[i]["EXPROC2"] = dt.Rows[i]["EXPROC2"].ToString();
        //        dt.Rows[i]["RATE2"] = dt.Rows[i]["RATE2"].ToString();
        //        dt.Rows[i]["REMARKS"] = dt.Rows[i]["REMARKS"].ToString();
        //        dt.Rows[i]["COLOR"] = dt.Rows[i]["COLOR"].ToString();
        //        dt.Rows[i]["COLORCODE"] = dt.Rows[i]["COLORCODE"].ToString();
        //        if (status == "True")
        //        {
        //            dt.Rows[i]["STPAmt"] = dt.Rows[i]["STPAmt"].ToString();
        //        }
        //        else
        //        {
        //            dt.Rows[i]["STPAmt"] = BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(dt.Rows[i]["PROCESS1"].ToString(), (Convert.ToInt32(dt.Rows[i]["QTY"].ToString()) * Convert.ToDouble(dt.Rows[i]["RATE"].ToString())), BranchId.Value, Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)));
        //        }
        //        if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(dt.Rows[i]["PROCESS1"].ToString(), BranchId.Value) == true)
        //            dt.Rows[i]["DisAmt"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * (Convert.ToInt32(dt.Rows[i]["QTY"].ToString()) * Convert.ToDouble(dt.Rows[i]["RATE"].ToString()))) / 100, 2));
        //        else
        //            dt.Rows[i]["DisAmt"] = "0";
        //        if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(dt.Rows[i]["EXPROC1"].ToString(), BranchId.Value) == true)
        //            dt.Rows[i]["DisAmt1"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * Convert.ToDouble(dt.Rows[i]["RATE1"].ToString())) / 100, 2));
        //        else
        //            dt.Rows[i]["DisAmt1"] = "0";
        //        if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckDiscountApplicationOnProces(dt.Rows[i]["EXPROC2"].ToString(), BranchId.Value) == true)
        //            dt.Rows[i]["DisAmt2"] = (Math.Round((Convert.ToDouble((txtDiscount.Text == "" ? "0" : txtDiscount.Text)) * Convert.ToDouble(dt.Rows[i]["RATE2"].ToString())) / 100, 2));
        //        else
        //            dt.Rows[i]["DisAmt2"] = "0";
        //    }
        //    txtWholeRemark.Text = "";
        //    dt.AcceptChanges();
        //    dt.DefaultView.Sort = "SNO DESC";       
        //    SetBalance();
        //    lblDisAmt.Text = ":" + Math.Round(Convert.ToDouble(txtDiscountAmt.Text), 2).ToString();       
        //    txtSrTax.Text = (Math.Round(BAL.BALFactory.Instance.BAL_New_Bookings.CalculatAllTax(dt), 2)).ToString();
        //    SetBalance();
        //}
        protected void txtAdvance_TextChanged(object sender, EventArgs e)
        {
            string res = CheckError();
            if (res == "Done")
            {
                //if (rdrPercentage.Checked)
                // //   txtDiscount_TextChanged(null, null);
                //else
                //    // txtDiscountAmt_TextChanged(null, null);
                btnSaveBooking.Focus();
            }
        }
        public DTO.NewBooking SetValueIntoPropertiesSaveTime()
        {
            Obj.BookingAcceptedByUserId = Globals.UserName;
            Obj.IsUrgent = (chkToday.Checked ? "1" : chkNextDay.Checked ? "1" : "0");
            // if is today or next day, it's already urgent, but it not, then check if urgent is checked
            if (Obj.IsUrgent == "0") Obj.IsUrgent = chkUrgent.Checked ? "1" : "0";
            Obj.BookingDate = txtDate.Text.Trim();
            Obj.BookingDeliveryDate = txtDueDate.Text.Trim();
            Obj.BookingDeliveryTime = txtTime.Text.Trim();
            Obj.TotalCost = txtCurrentDue.Text.Trim();

            //Obj.Discount = txtDiscount.Text.Trim();
            Obj.Discount = hdnDiscountPerc.Value;
            if (isNetAmountInDecimal.Value == "true")
                Obj.NetAmount = txtTotal.Text == string.Empty ? string.Empty : (Convert.ToDouble(txtTotal.Text)).ToString();
            else
                Obj.NetAmount = txtTotal.Text == string.Empty ? string.Empty : (Convert.ToInt32(Math.Ceiling(float.Parse(txtTotal.Text)))).ToString();

            /*
            # region VivekSirValueBenefitTest
            if (!string.IsNullOrEmpty(hdnPackageTypeRecNo.Value))
            {
                if (hdnPackageTypeRecNo.Value.Split(':')[0].ToString() == "Value / Benefit")
                    Obj.NetAmount = (float.Parse(txtCurrentDue.Text.Trim()) + float.Parse(txtSrTax.Text.Trim())).ToString();
            }
            #endregion
            */

            Obj.Remarks = txtRemarks.Text.Trim();
            Obj.ItemTotalQuantity = hdnGrdRowCount.Value;
            // Obj.HomeDelivery = (chkHD.Checked ? "1" : "0");
            Obj.HomeDelivery = (drpHD.SelectedItem.Text == "Yes" ? "1" : "0");
            Obj.CheckedByEmployee = drpCheckedBy.SelectedValue;
            Obj.BookingByCustomer = CustId;
            Obj.AdvanceAmt = txtAdvance.Text;
            Obj.BookingRemarks = txtRemarks.Text;
            Obj.STTax = txtSrTax.Text;
            Obj.DiscountAmt = hdnDiscountValue.Value;
            Obj.DiscountOption = (rdrPercentage.Checked ? "0" : "1");
            Obj.TodaNext = (chkToday.Checked ? "1" : chkNextDay.Checked ? "2" : " ");
            Obj.TaxableAmt = double.Parse(hdnTaxableAmt.Value);
            Obj.TaxType = hdnTaxType.Value;
            Obj.WorkshopNote = txtWorkshopNotes.Text;
            Obj.BookingAcceptedByUserId = Globals.UserName;
            if (hdnPackageTypeRecNo.Value.Split(':')[0] == "Flat Qty")
            {
                Obj.PaymentMode = hdnTmpAdvType.Value;
            }
            else
            {
                Obj.PaymentMode = (Request["drpAdvanceType"] ?? drpAdvanceType.SelectedItem.Text).ToString();
            }
            // Now if the user selects "Cash" or some other type of from dropdown, then the assignId will be wrongly saved in the 
            // table if the customer was of packge, so if Obj.PamentMode == "Package" then only set the assign ID else not
            Obj.AssignId = (Obj.PaymentMode.ToString() == "Package" ? hdnAssignId.Value : (hdnPackageTypeRecNo.Value.Split(':')[0] == "Discount" ? hdnAssignId.Value : "0"));
            Obj.isDiscountPackge = hdnPackageTypeRecNo.Value.Split(':')[0] == "Discount" ? true : false;
            // added for rate list id
            Obj.RateListId = ddlRateList.SelectedValue;
            //DataSet dsBookingPrefix = BAL.BALFactory.Instance.BAL_New_Bookings.GetBookingPrefix(Globals.BranchID);
            //Obj.BookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPreFix"].ToString();
            return Obj;
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            chkToday.Checked = false;
            chkNextDay.Checked = false;
            //chkOldBooking.Checked = false;
            chkToday.Enabled = false;
            chkNextDay.Enabled = false;
            //chkOldBooking.Enabled = false;
            SetCommanSetting();
            BindGridView();
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_New_Bookings.ReadDataForEdit(txtEdit.Text, BranchId.Value);
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Entry"];
            int srNo = 1;
            double stTax = 0;
            string allTaxArray = string.Empty;
            var rowTax = string.Empty;
            var allItemsForCalcSubItems = new List<string>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                // btnReset.Visible = false;
                string shopin = ds.Tables[0].Rows[0]["Shopin"].ToString();
                string cancleBooking = ds.Tables[0].Rows[0]["BookingCancelStatus"].ToString();
                if (cancleBooking != "5")
                {
                    if ((shopin == "2" || shopin == "1") && ds.Tables[0].Rows[0]["IsPackage"].ToString() == "False")
                    {
                        CustId = ds.Tables[0].Rows[0]["CustCode"].ToString();
                        PrjClass.SetItemInDropDown(drpAdvanceType, ds.Tables[0].Rows[0]["PaymentMode"].ToString(), false, false);
                        hdnCustCode.Value = ds.Tables[0].Rows[0]["CustCode"].ToString();
                        txtCustomerName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
                        lblAddress.Text = ds.Tables[0].Rows[0]["CustAddress"].ToString();
                        lblMobileNo.Text = ds.Tables[0].Rows[0]["CustPhone"].ToString();
                        lblEmailId.Text = ds.Tables[0].Rows[0]["EmailId"].ToString();
                        lblPriority.Text = ds.Tables[0].Rows[0]["CustPriority"].ToString();
                        lblRemarks.Text = ds.Tables[0].Rows[0]["CustRemarks"].ToString();
                        txtDate.Text = ds.Tables[0].Rows[0]["BookingDate"].ToString();
                        txtDueDate.Text = ds.Tables[0].Rows[0]["BookingDeliveryDate"].ToString();
                        txtTime.Text = ds.Tables[0].Rows[0]["BookingDeliveryTime"].ToString();
                        var allItemsUnit = string.Empty;
                        var allItemsUnitValue = string.Empty;


                        PrjClass.SetItemInDropDown(ddlRateList, ds.Tables[0].Rows[0]["RateListId"].ToString(), false, false);
                        ddlRateList.Attributes.Add("disabled", "disabled");

                        # region Details

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //Items.Add("COLORCODE");
                            int flag = 0;
                            DataRow NewRow = dt.NewRow();
                            NewRow[0] = srNo.ToString();
                            NewRow[1] = ds.Tables[0].Rows[i]["ItemTotalQuantity"].ToString();
                            NewRow[2] = ds.Tables[0].Rows[i]["ItemName"].ToString();
                            allItemsForCalcSubItems.Add(NewRow[2].ToString());
                            NewRow[3] = ds.Tables[0].Rows[i]["ItemRemark"].ToString();
                            NewRow[17] = ds.Tables[0].Rows[i]["ItemColor"].ToString();
                            //NewRow[5] = ds.Tables[0].Rows[i]["ItemQuantityAndRate"].ToString();
                            string[] Rate = ds.Tables[0].Rows[i]["ItemQuantityAndRate"].ToString().Split('@');
                            NewRow[5] = "(" + ds.Tables[0].Rows[i]["ItemProcessType"].ToString() + '@' + Rate[1].ToString() + ")";
                            flag = 0;
                            if (ds.Tables[0].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                            {
                                NewRow[5] += ",(" + ds.Tables[0].Rows[i]["ItemExtraProcessType1"].ToString() + "@" + ds.Tables[0].Rows[i]["ItemExtraProcessRate1"].ToString() + ")";
                                if (ds.Tables[0].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                                {
                                    NewRow[5] += ",(" + ds.Tables[0].Rows[i]["ItemExtraProcessType2"].ToString() + "@" + ds.Tables[0].Rows[i]["ItemExtraProcessRate2"].ToString() + ")";
                                    flag = 1;
                                }
                            }
                            if (ds.Tables[0].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                            {
                                if (flag == 0)
                                    NewRow[5] += ",(" + ds.Tables[0].Rows[i]["ItemExtraProcessType2"].ToString() + "@" + ds.Tables[0].Rows[i]["ItemExtraProcessRate2"].ToString() + ")";
                            }
                            NewRow[6] = Rate[1].ToString();
                            NewRow[7] = (ds.Tables[0].Rows[i]["ItemExtraProcessType1"].ToString() == "None" ? "None" : ds.Tables[0].Rows[i]["ItemExtraProcessType1"].ToString());
                            NewRow[8] = ds.Tables[0].Rows[i]["ItemExtraProcessRate1"].ToString();
                            NewRow[9] = (ds.Tables[0].Rows[i]["ItemExtraProcessType2"].ToString() == "None" ? "None" : ds.Tables[0].Rows[i]["ItemExtraProcessType2"].ToString());
                            NewRow[10] = ds.Tables[0].Rows[i]["ItemExtraProcessRate2"].ToString();
                            NewRow[11] = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["ItemSubtotal"].ToString()), 2).ToString();
                            NewRow[12] = ds.Tables[0].Rows[i]["ItemName"].ToString();
                            NewRow[13] = ds.Tables[0].Rows[i]["ItemProcessType"].ToString();


                            // <MOHIT CHAUHAN>

                            // added code
                            // for tax
                            // Stored in format
                            // rowPrcTax:rowPrcTax1:rowPrcTax:2_rowPrc1Tax:rowPrc1Tax1:rowPrc1Tax:2_rowPrc2Tax:rowPrc2Tax1:rowPrc2Tax:2 
                            // `
                            // row1PrcTax:row1PrcTax1:row1PrcTax:2_row1Prc1Tax:row1Prc1Tax1:row1Prc1Tax:2_row1Prc2Tax:row1Prc2Tax1:row1Prc2Tax:2
                            // `
                            // where ` is the delimeter for the NextRow
                            NewRow[14] = ds.Tables[0].Rows[i]["STPAmt"].ToString();

                            // this is first process first tax
                            rowTax = NewRow[14].ToString();
                            rowTax += ":" + ds.Tables[0].Rows[i]["STPAmtEcess"].ToString();
                            NewRow[21] = ds.Tables[0].Rows[i]["STPAmtEcess"].ToString();
                            rowTax += ":" + ds.Tables[0].Rows[i]["STPAmtSHECess"].ToString() + "";
                            NewRow[22] = ds.Tables[0].Rows[i]["STPAmtSHECess"].ToString() + "";
                            // first tax compelte, add second process tax
                            NewRow[15] = ds.Tables[0].Rows[i]["STEP1Amt"].ToString();
                            rowTax += "_" + NewRow[15].ToString();
                            rowTax += ":" + ds.Tables[0].Rows[i]["STPAmt1Ecess"].ToString();
                            NewRow[23] = ds.Tables[0].Rows[i]["STPAmt1Ecess"].ToString();
                            rowTax += ":" + ds.Tables[0].Rows[i]["STPAmt1SHECess"].ToString() + "";
                            NewRow[24] = ds.Tables[0].Rows[i]["STPAmt1SHECess"].ToString() + "";
                            // second tax compelte, add third and final process tax
                            NewRow[16] = ds.Tables[0].Rows[i]["STEP2Amt"].ToString();
                            rowTax += "_" + NewRow[16].ToString();
                            rowTax += ":" + ds.Tables[0].Rows[i]["STPAmt2Ecess"].ToString();
                            NewRow[25] = ds.Tables[0].Rows[i]["STPAmt2Ecess"].ToString();
                            rowTax += ":" + ds.Tables[0].Rows[i]["STPAmt2SHECess"].ToString() + "";
                            NewRow[26] = ds.Tables[0].Rows[i]["STPAmt2SHECess"].ToString() + "";
                            // all tax complete,
                            // add to allTax Array and append a backtick
                            allTaxArray += rowTax + "`";

                            // </MOHIT CHAUHAN>


                            NewRow[4] = ds.Tables[0].Rows[i]["ItemColor"].ToString();
                            dt.Rows.Add(NewRow);
                            srNo++;
                            stTax += Convert.ToDouble(ds.Tables[0].Rows[i]["STPAmt"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["STPAmt"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[i]["STEP1Amt"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["STEP1Amt"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[i]["STEP2Amt"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["STEP2Amt"].ToString());

                            txtDiscountAmt.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["DiscountAmt"].ToString()), 2).ToString();
                            var tmpUnit = ds.Tables[0].Rows[i]["UnitDesc"].ToString();
                            // change the item name
                            var itemName = string.Empty;
                            if (tmpUnit == "")
                            {
                                allItemsUnit += "Pcs" + "_";
                                allItemsUnitValue += "1X1:1" + "_";
                            }
                            else if (tmpUnit.Contains("X"))
                            {
                                allItemsUnit += "LB" + "_";
                                allItemsUnitValue += tmpUnit + ":1_";
                                NewRow[2] = NewRow[2] + " " + tmpUnit;
                            }
                            else
                            {
                                allItemsUnit += "P" + "_";
                                // -2 because we don't want to save PL in item value
                                allItemsUnitValue += "1X1:" + tmpUnit.Substring(0, tmpUnit.Length - 2) + "_";
                                NewRow[2] = NewRow[2] + " " + tmpUnit;
                            }

                        }

                        # endregion

                        allItemsUnit = allItemsUnit.Substring(0, allItemsUnit.Length - 1);
                        allItemsUnitValue = allItemsUnitValue.Substring(0, allItemsUnitValue.Length - 1);

                        hdnItemUnit.Value = allItemsUnit;
                        hdnItemUnitValue.Value = allItemsUnitValue;

                        dt.AcceptChanges();
                        ViewState["Entry"] = dt;
                        dt.DefaultView.Sort = "SNO DESC";
                        grdEntry.DataSource = dt;
                        grdEntry.DataBind();



                        txtAdvance.Text = (ds.Tables[0].Rows[0]["Advance"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["Advance"].ToString());

                        txtSrTax.Text = Math.Round(stTax, 2).ToString();
                        txtCurrentDue.Text = (ds.Tables[0].Rows[0]["TotalCost"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["TotalCost"].ToString());


                        // check here for discount
                        // if discount option is 1
                        // flat discount
                        if (ds.Tables[0].Rows[0]["DiscountOption"].ToString() == "1")
                        {
                            txtDiscountAmt.Text = (ds.Tables[0].Rows[0]["DiscountAmt"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["DiscountAmt"].ToString());
                            // hide the discount and show the discountAmt
                            // txtDiscountAmt.Attributes.Add("style=\"display", "none");
                            // lblDisAmt.Text = txtDiscountAmt.Text;


                            // make the percentage
                            // var _disPerc = 100 * parseFloat($('#txtDiscountAmt').val()) / parseFloat($('#txtCurrentDue').val());
                            var disPerc = 100 * double.Parse(txtDiscountAmt.Text) / double.Parse(txtCurrentDue.Text);
                            disPerc = Math.Round(disPerc, 2);
                            // set this perctage to text amount
                            //$('#txtDiscount').val(_disPerc);
                            txtDiscount.Text = disPerc.ToString();
                            // set the label to current value
                            // $('#lblDisAmt').text($(this).val());
                            lblDisAmt.Text = txtDiscountAmt.Text;
                            //$('#hdnDiscountValue').text($(this).val());
                            hdnDiscountValue.Value = txtDiscountAmt.Text;

                            // set the hidden recomp value to this
                            // $('#hdnDisAmtRecomp').val($(this).val());
                            hdnDisAmtRecomp.Value = txtDiscountAmt.Text;
                        }
                        else
                        // percentage discount
                        {
                            txtDiscount.Text = (ds.Tables[0].Rows[0]["Discount"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["Discount"].ToString());
                            // hide the discount and show the discountAmt
                            // txtDiscountAmt.Attributes.Add("style=\"display", "none");
                            // lblDisAmt.Text = txtDiscountAmt.Text;

                            //// set the label to current value
                            //// $('#lblDisAmt').text($(this).val());
                            //lblDisAmt.Text = txtDiscountAmt.Text;
                            ////$('#hdnDiscountValue').text($(this).val());
                            //hdnDiscountValue.Value = txtDiscountAmt.Text;

                            //// set the hidden recomp value to this
                            //// $('#hdnDisAmtRecomp').val($(this).val());
                            //hdnDisAmtRecomp.Value = txtDiscountAmt.Text;
                        }


                        ((Label)grdEntry.HeaderRow.FindControl("lblHAmount")).Text = txtCurrentDue.Text;
                        // lblDisAmt.Text = txtDiscountAmt.Text;
                        txtBalance.Text = (ds.Tables[0].Rows[0]["NetAmount"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["NetAmount"].ToString());
                        PrjClass.SetItemInDropDown(drpCheckedBy, ds.Tables[0].Rows[0]["CheckedByEmployee"].ToString(), false, false);
                        if (ds.Tables[0].Rows[0]["HomeDelivery"].ToString() == "1") /* chkHD.Checked = true; */ drpHD.SelectedIndex = 1;
                        else drpHD.SelectedIndex = 0;
                        if (ds.Tables[0].Rows[0]["IsUrgent"].ToString() == "1")
                        {
                            var argFlag = string.Empty;
                            // here set the urngent rate
                            if (ds.Tables[0].Rows[0]["TN"].ToString() == "1")
                            { chkToday.Checked = true; chkUrgent.Checked = true; chkUrgent.Checked = false; argFlag = "37"; }
                            else if (ds.Tables[0].Rows[0]["TN"].ToString() == "2")
                            { chkNextDay.Checked = true; chkUrgent.Checked = true; chkUrgent.Checked = false; argFlag = "38"; }
                            else if (ds.Tables[0].Rows[0]["TN"].ToString().Trim() == string.Empty || ds.Tables[0].Rows[0]["TN"].ToString() == "NULL")
                            {
                                chkToday.Checked = false; chkNextDay.Checked = false; chkUrgent.Checked = true;
                            }
                            chkToday.Enabled = false;
                            chkNextDay.Enabled = false;
                            // chkOldBooking.Enabled = false;
                            //Task.Factory.StartNew(() =>
                            //{
                            var urRate = string.IsNullOrEmpty(argFlag) ? "0:0" : BAL.BALFactory.Instance.BAL_New_Bookings.GetNextDayRateAndDayOffset(Globals.BranchID, argFlag);
                            hdnUrgentRateApplied.Value = urRate.Split(':')[0];
                            //});
                        }
                        else
                            hdnUrgentRateApplied.Value = "0";

                        txtRemarks.Text = ds.Tables[0].Rows[0]["BookingRemarks"].ToString();
                        txtWorkshopNotes.Text = ds.Tables[0].Rows[0]["WorkShopNote"].ToString();
                        SetCommanSetting();
                        //SetBalance();
                        txtSrTax.Text = (Math.Round(BAL.BALFactory.Instance.BAL_New_Bookings.CalculatAllTax(dt), 2)).ToString();
                        ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text = (1 + (grdEntry.Rows.Count)).ToString();
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text = "1";
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = ItemName;
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = Process;
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();
                        // txtCurrentDue.Text = ((Label)grdEntry.HeaderRow.FindControl("lblHAmount")).Text;
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtCurrentDue.Text) - Convert.ToDouble(txtDiscountAmt.Text) + Convert.ToDouble((txtSrTax.Text == "" ? "0" : txtSrTax.Text)), 2).ToString();
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(BranchId.Value) == "True")
                            txtBalance.Text = Math.Round(Convert.ToDouble((Convert.ToDouble((txtTotal.Text == "" ? "0" : txtTotal.Text))) - Convert.ToDouble((txtAdvance.Text == "" ? "0" : txtAdvance.Text))), 2).ToString();
                        else
                            txtBalance.Text = Convert.ToInt32(Math.Round(Convert.ToDouble((Convert.ToDouble((txtTotal.Text == "" ? "0" : txtTotal.Text))) - Convert.ToDouble((txtAdvance.Text == "" ? "0" : txtAdvance.Text))), 2)).ToString(); ;
                        tblEntry.Visible = true;
                        tblSearch.Attributes.Add("style", "display:none;");
                        tblEntry1.Visible = true;
                        tblEntry2.Visible = true;
                        hdnUpdate.Value = "1";
                        hdnCurrentValue.Value = (ds.Tables[0].Rows.Count + 1).ToString();
                        // set hidden fields
                        hdnTaxAmtRecomp.Value = txtSrTax.Text;
                        hdnDisAmtRecomp.Value = lblDisAmt.Text;
                        hdnDiscountValue.Value = lblDisAmt.Text;
                        hdnTotal.Value = txtTotal.Text;
                        hdnBalance.Value = txtBalance.Text;

                        // make the array for storing all tax
                        // already made in the previous for loop, where the newRow[14] etc is calculate
                        // remove the last one because that is an extra '_'
                        hdnAllTax.Value = allTaxArray.Substring(0, allTaxArray.Length - 1);

                        // set is in edit mode to true
                        isInEditMode.Value = "true";

                        if (ds.Tables[0].Rows[0]["DiscountOption"].ToString() == "0")
                        {
                            rdrPercentage.Checked = true;
                            rdrAmt.Checked = false;
                            rdrPercentage_CheckedChanged(null, null);
                            // txtDiscount_TextChanged(null, null);
                        }
                        else
                        {
                            rdrAmt.Checked = true;
                            rdrPercentage.Checked = false;
                            rdrAmt_CheckedChanged(null, null);
                            // txtDiscountAmt_TextChanged(null, null);
                        }
                        // check the way of communication
                        if (ds.Tables[0].Rows[0]["CommunicationMeans"].ToString().ToUpperInvariant() == "EMAIL")
                        { chkEmail.Checked = true; chkSendsms.Checked = false; drpCommLbl.SelectedIndex = 1; }
                        else if (ds.Tables[0].Rows[0]["CommunicationMeans"].ToString().ToUpperInvariant() == "SMS")
                        { chkSendsms.Checked = true; chkEmail.Checked = false; drpCommLbl.SelectedIndex = 2; }
                        else if (ds.Tables[0].Rows[0]["CommunicationMeans"].ToString().ToUpperInvariant() == "BOTH")
                        { chkSendsms.Checked = true; chkEmail.Checked = true; drpCommLbl.SelectedIndex = 3; }
                        else
                        { chkSendsms.Checked = false; chkEmail.Checked = false; drpCommLbl.SelectedIndex = 0; }

                        ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
                        // ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
                    }
                    else if (ds.Tables[0].Rows[0]["IsPackage"].ToString() == "True")
                    {
                        lblSearchError.Text = "This Order is done in Package, Please cancel it and Create New Order.";
                        txtEdit.Focus();
                        txtEdit.Attributes.Add("onfocus", "javascript:select();");
                    }
                    else
                    {
                        var msg = string.Empty;
                        if (shopin == "20")
                            msg = "Cloth sent to workshop, cannot edit";
                        else if (shopin == "30")
                            msg = "Cloth received from workshop, cannot edit";
                        else if (shopin == "50")
                            msg = "Cloth marked for finishing, cannot edit";
                        else if (shopin == "3")
                            msg = "Cloth ready, cannot edit.";
                        else if (shopin == "4")
                            msg = "Cloth already delivered, cannot edit";
                        else if (shopin == "5")
                            msg = "This booking number already canceled.";

                        lblSearchError.Text = msg;
                        txtEdit.Focus();
                        txtEdit.Attributes.Add("onfocus", "javascript:select();");
                    }
                    var tmpBranchID = Globals.BranchID;
                    var tmpUserName = Globals.UserName;
                    Task t = Task.Factory.StartNew
                    (
                    () => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtEdit.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, bookingSrcMsg.BookingScrSearch,ScreenName.BookingScreen,1); }
                    );
                }
                else
                {
                    lblSearchError.Text = "This booking number already canceled.";
                    txtEdit.Focus();
                    txtEdit.Attributes.Add("onfocus", "javascript:select();");
                }
            }
            else
            {
                lblSearchError.Text = "Invalid booking number.Please enter correct booking number.";
                txtEdit.Focus();
                txtEdit.Attributes.Add("onfocus", "javascript:select();");
            }
            int count = 0;

            if (grdEntry.Rows.Count > 0 && allItemsForCalcSubItems != null && allItemsForCalcSubItems.Count != 0)
            {
                allItemsForCalcSubItems.Reverse();
                for (int i = 0; i < grdEntry.Rows.Count; i++)
                {
                    count += (Convert.ToInt32(((Label)grdEntry.Rows[i].FindControl("lblQty")).Text) * Convert.ToInt32(BAL.BALFactory.Instance.BAL_New_Bookings.CountNoOfSubItem(allItemsForCalcSubItems[i], Globals.BranchID)));
                }
            }
            else
                count = 0;


            // this is for that error of wrong count
            lblCount.Text = count.ToString();
        }
        public void SetBackup()
        {
            SqlCommand DBCommand = new SqlCommand();
            DBCommand.CommandType = CommandType.StoredProcedure;
            DBCommand.CommandText = "sp_Backup";
            AppClass.ExecuteNonQuery(DBCommand);
        }
        private void SetMenuRightsNew(string strUserTypeValue)
        {
            DataSet dsMain = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.Parameters.AddWithValue("@UserTypeId", strUserTypeValue);
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 18);
                dsMain = AppClass.GetData(cmd);
            }
            catch (Exception excp)
            {
                Response.Write(excp.ToString());
            }
            finally
            {

            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    ShowMenuFromTable(dsMain.Tables[0]);
                }
            }
        }
        private void ShowMenuFromTable(DataTable dtMenu)
        {
            MenuItem mnuItem = null;
            DataRow[] drMenuLevel1 = dtMenu.Select("MenuItemLevel='1' AND RightToView = true");
            string strMenuTitle = string.Empty, strMenuNaviToUrl = string.Empty, strMenuParent = string.Empty, strMenuPosition = string.Empty;
            foreach (DataRow drTopMenu in drMenuLevel1)
            {
                strMenuTitle = "" + drTopMenu["PageTitle"].ToString();
                strMenuNaviToUrl = "" + drTopMenu["FileName"].ToString();
                strMenuParent = "" + drTopMenu["ParentMenu"].ToString();
                strMenuPosition = "" + drTopMenu["MenuPosition"].ToString();
                mnuItem = new MenuItem();
                mnuItem.Text = strMenuTitle;
                mnuItem.Value = strMenuTitle;
                mnuItem.NavigateUrl = strMenuNaviToUrl;
                MainMenu.Items.Add(mnuItem);
            }
            for (int c = 0; c < MainMenu.Items.Count; c++)
            {
                DataRow[] dtMenuLevel2 = dtMenu.Select("MenuItemLevel='2' AND RightToView = true AND ParentMenu = '" + MainMenu.Items[c].Text + "'");
                foreach (DataRow drSubMenu in dtMenuLevel2)
                {
                    strMenuTitle = "" + drSubMenu["PageTitle"].ToString();
                    strMenuNaviToUrl = "" + drSubMenu["FileName"].ToString();
                    strMenuParent = "" + drSubMenu["ParentMenu"].ToString();
                    strMenuPosition = "" + drSubMenu["MenuPosition"].ToString();
                    mnuItem = new MenuItem();
                    mnuItem.Text = strMenuTitle;
                    mnuItem.Value = strMenuTitle;
                    mnuItem.NavigateUrl = strMenuNaviToUrl;
                    MainMenu.Items[c].ChildItems.Add(mnuItem);
                }
            }
        }
        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx?Backup=" + "BackUp" + "");
        }
        protected void FillExistingRecord()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 18);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblStoreName.Text = ds.Tables[0].Rows[0]["StoreName"].ToString();
                //lblFooterMessage.Text = ds.Tables[0].Rows[0]["FooterName"].ToString();
                //Label1.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            }
        }
        protected void btnCustAdd_Click(object sender, EventArgs e)
        {

        }
        protected void imgBtnCustomerAdd_Click(object sender, EventArgs e)
        {
            //ModalPopupExtender1.Show();
            txtCName.Text = txtCustomerName.Text;
            txtCName.Focus();
            txtCName.Attributes.Add("onfocus", "javascript:select()");
        }
        protected void btnOkay_Click(object sender, EventArgs e)
        {
            DataSet DS_CustInfo = new DataSet();
            try
            {
                SetValueIntoProperties1();
                Obj.BID = BranchId.Value;
                DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.SaveCustomer(Obj);
                if (DS_CustInfo.Tables[0].Rows.Count > 0)
                {
                    setCustvalue(DS_CustInfo.Tables[0].Rows[0]["CustCode"].ToString(), BranchId.Value);
                    CleanValues();
                    BindPriorty();
                    //ModalPopupExtender1.Hide();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
                    CustId = DS_CustInfo.Tables[0].Rows[0]["CustCode"].ToString();
                }

            }
            catch (Exception)
            {
                CleanValues();
                lblCustSave.Text = "Customer name and address exists.";
                BindPriorty();
            }
        }
        public void BindPriorty()
        {
            //drpPriority.DataSource = BAL.BALFactory.Instance.BAL_New_Bookings.BindPriority(BranchId.Value);
            //drpPriority.DataTextField = "Priority";
            //drpPriority.DataValueField = "PriorityID";
            //drpPriority.DataBind();
        }
        public void SetValueIntoProperties1()
        {
            //Obj.AddPriority = txtNewPriority.Value;
            // Obj.Priority = drpPriority.SelectedItem.Value;
            Obj.CustAddress = txtCAddress.Text.ToUpper();
            Obj.CustAreaAndLocation = txtAreaLocaton.Text.ToUpper();
            Obj.CustMobile = txtMobile.Text.ToUpper();
            Obj.CustName = txtCName.Text.ToUpper();
            Obj.CustRemarks = txtRemarks1.Text.ToUpper();
            Obj.CustTitle = drpTitle.SelectedItem.Text;
            //Obj.BDate = txtBDate.Text;
            //Obj.ADate = txtADate.Text;
        }
        protected void btnAddNewPriority_Click(object sender, EventArgs e)
        {
            //if (txtNewPriority.Value == "")
            //{
            //    Session["ReturnMsg"] = "No new priority was provided to add..";
            //    btnOkay.Focus();
            //    return;
            //}
            SetValueIntoProperties1();
            Obj.BID = BranchId.Value;
            string res = BAL.BALFactory.Instance.BAL_New_Bookings.SavePriority(Obj);
            if (res != "Record Saved")
            {
                Session["ReturnMsg"] = res;
                CleanValues();
            }
            else
            {
                BindPriorty();
                // PrjClass.SetItemInDropDown(drpPriority, txtNewPriority.Value, true, false);
                btnOkay.Focus();
            }
        }
        public void CleanValues()
        {
            // txtNewPriority.Value = "";
            txtCAddress.Text = "";
            txtAreaLocaton.Text = "";
            txtMobile.Text = "";
            txtCName.Text = "";
            txtRemarks1.Text = "";
            //txtBDate.Text = "";
            //txtADate.Text = "";
            drpTitle.SelectedIndex = -1;
            // drpPriority.Focus();
        }
        protected void btnAddRAndC_Click(object sender, EventArgs e)
        {
            btnAddRemarks_Click(null, null);
        }
        protected void btnAddRemarks_Click(object sender, EventArgs e)
        {
            SetCommanSetting();
            Remarks_ModalPopup.Show();
            txtWholeRemark.Focus();
        }
        protected void btnExtra_Click(object sender, EventArgs e)
        {
            txtExtraProcess1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('check').value=0;return false;}} else if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('check').value=0;return false;} else {return true}; ");
            txtExtraProcess2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('check').value=0;return false;}} else if ((event.which == 9) || (event.keyCode == 9)) {document.getElementById('check').value=0;return false;} else {return true}; ");
            ModalPopupExtender8.Show();
            txtExtraProcess1.Focus();
            txtExtraProcess1.Attributes.Add("onfocus", "javascript:select();");
        }
        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }
        protected void grdEntry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='silver';this.style.cursor='pointer';this.style.textDecoration='underline';";
                // e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';this.style.textDecoration='none';this.style.color='black';";
                //e.Row.Attributes.Add("ondblclick", "Javascript:__doPostBack('myDblClick','" + e.Row.RowIndex + "');");
            }
            catch (Exception) { }
        }
        protected void grdEntry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteItemDetails"))
            {
                int RowNum = int.Parse(e.CommandArgument.ToString());
                RowNum = int.Parse(((Label)grdEntry.Rows[RowNum].FindControl("lblSNo")).Text);
                DataTable dt = (DataTable)ViewState["Entry"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SNO"].ToString() == RowNum.ToString())
                    {
                        dt.Rows[i].Delete();
                        dt.AcceptChanges();
                        txtSrTax.Text = (BAL.BALFactory.Instance.BAL_New_Bookings.CalculatAllTax(dt)).ToString();
                        if (dt.Rows.Count > 0)
                        {
                            for (int r = 0; r < dt.Rows.Count; r++)
                            {
                                dt.Rows[r][0] = (r + 1).ToString();
                            }
                        }
                    }
                }
                ViewState["Entry"] = dt;
                dt.DefaultView.Sort = "SNO DESC";
                grdEntry.DataSource = dt;
                grdEntry.DataBind();

                if (dt.Rows.Count > 0)
                {
                    int count = 0;
                    if (grdEntry.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdEntry.Rows.Count; i++)
                            count += (Convert.ToInt32(((Label)grdEntry.Rows[i].FindControl("lblQty")).Text) * Convert.ToInt32(BAL.BALFactory.Instance.BAL_New_Bookings.CountNoOfSubItem(((Label)grdEntry.Rows[i].FindControl("lblItemName")).Text, Globals.BranchID)));
                    }
                    else
                        count = 0;
                    lblCount.Text = count.ToString();

                }
                else
                {
                    if (dt.Rows.Count == 0)
                    {
                        lblCount.Text = "0";
                        BindGridView();
                    }
                }
                SetCommanSetting();
                string count1 = ((dt.Rows.Count == 0 ? 1 : (grdEntry.Rows.Count + 1))).ToString();
                ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text = count1;
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text = "1";
                ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = ItemName;
                ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = Process;
                ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = (BAL.BALFactory.Instance.BAL_New_Bookings.GetItemRateAccordingProcess(((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtName")).Text, ((TextBox)grdEntry.HeaderRow.Cells[0].FindControl("txtProcess")).Text, BranchId.Value)).ToString();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
            }
            if (e.CommandName.Equals("EDITItemDetails"))
            {
                return;
                ((Button)grdEntry.HeaderRow.FindControl("imgBtnGridEntry")).Text = "Update";
                int RowIndex = int.Parse(e.CommandArgument.ToString());
                editOption = ((Label)grdEntry.Rows[RowIndex].FindControl("lblSNo")).Text;
                RowIndex = int.Parse(((Label)grdEntry.Rows[RowIndex].FindControl("lblSNo")).Text);
                DataTable dt = (DataTable)ViewState["Entry"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SNO"].ToString() == RowIndex.ToString())
                    {
                        ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text = dt.Rows[i]["SNO"].ToString();
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text = dt.Rows[i]["QTY"].ToString();
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = dt.Rows[i]["ITEMNAME1"].ToString();
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = dt.Rows[i]["PROCESS1"].ToString();
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = dt.Rows[i]["RATE"].ToString();
                        if (dt.Rows[i]["EXPROC1"].ToString() != "None")
                            txtExtraProcess1.Text = dt.Rows[i]["EXPROC1"].ToString();
                        txtExtraRate1.Text = dt.Rows[i]["RATE1"].ToString();
                        if (dt.Rows[i]["EXPROC2"].ToString() != "None")
                            txtExtraProcess2.Text = dt.Rows[i]["EXPROC2"].ToString();
                        txtExtraRate2.Text = dt.Rows[i]["RATE2"].ToString();
                        if (hdnItems.Value == "" || hdnItems.Value == null)
                            ((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text = dt.Rows[i]["REMARKS"].ToString();
                        else
                        {
                            ((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text = hdnItems.Value;
                            //hdnValues.Text = hdnItems.Value;

                        }
                        ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Text = dt.Rows[i]["COLOR"].ToString();

                    }
                }
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
                ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");

            }
            SetBalance();
            SetCommanSetting();
        }
        public void setEditValue(int rowIndex)
        {
            ((Button)grdEntry.HeaderRow.FindControl("imgBtnGridEntry")).Text = "Update";
            int RowIndex = rowIndex;
            editOption = rowIndex.ToString();
            DataTable dt = (DataTable)ViewState["Entry"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["SNO"].ToString() == RowIndex.ToString())
                {
                    ((Label)grdEntry.HeaderRow.FindControl("lblHSNo")).Text = dt.Rows[i]["SNO"].ToString();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Text = dt.Rows[i]["QTY"].ToString();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtName")).Text = dt.Rows[i]["ITEMNAME1"].ToString();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtProcess")).Text = dt.Rows[i]["PROCESS1"].ToString();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtRate")).Text = dt.Rows[i]["RATE"].ToString();
                    if (dt.Rows[i]["EXPROC1"].ToString() != "None")
                        txtExtraProcess1.Text = dt.Rows[i]["EXPROC1"].ToString();
                    else
                        txtExtraProcess1.Text = "";
                    txtExtraRate1.Text = dt.Rows[i]["RATE1"].ToString();
                    if (dt.Rows[i]["EXPROC2"].ToString() != "None")
                        txtExtraProcess2.Text = dt.Rows[i]["EXPROC2"].ToString();
                    else
                        txtExtraProcess2.Text = "";
                    txtExtraRate2.Text = dt.Rows[i]["RATE2"].ToString();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text = dt.Rows[i]["REMARKS"].ToString();
                    ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Text = dt.Rows[i]["COLOR"].ToString();
                }
            }
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
            SetCommanSetting();
        }
        //protected void txtDiscountAmt_TextChanged(object sender, EventArgs e)
        //{
        //    string res = CheckError();
        //    DataTable dt = new DataTable();       
        //    dt = (DataTable)ViewState["Entry"];
        //    if (res == "Done")
        //    {
        //        try
        //        {
        //            double diacountAmt = 0;               
        //            txtDiscount.Text = (Math.Round(Convert.ToDouble((Convert.ToDecimal(txtDiscountAmt.Text) / Convert.ToDecimal(txtCurrentDue.Text)) * 100), 2)).ToString();
        //            diacountAmt = Convert.ToDouble(txtDiscountAmt.Text);
        //            txtTotal.Text = Math.Round((Convert.ToDouble(txtCurrentDue.Text) - diacountAmt) + Convert.ToDouble((txtSrTax.Text == "" ? "0" : txtSrTax.Text)), 2).ToString();
        //            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(BranchId.Value) == "True")
        //                txtBalance.Text = Math.Round(Convert.ToDouble((Convert.ToDouble((txtTotal.Text == "" ? "0" : txtTotal.Text))) - Convert.ToDouble((txtAdvance.Text == "" ? "0" : txtAdvance.Text))), 2).ToString();
        //            else
        //                txtBalance.Text = Convert.ToInt32(Math.Round(Convert.ToDouble((Convert.ToDouble((txtTotal.Text == "" ? "0" : txtTotal.Text))) - Convert.ToDouble((txtAdvance.Text == "" ? "0" : txtAdvance.Text))), 2)).ToString();
        //            txtAdvance.Focus();
        //            txtDiscount_TextChanged(null, null);
        //            txtSrTax.Text = (Math.Round(BAL.BALFactory.Instance.BAL_New_Bookings.CalculatAllTax(dt), 2)).ToString();
        //            SetBalance();
        //        }
        //        catch (Exception) { }
        //    }
        //}
        protected void drpDiscountOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDiscountOption.SelectedValue == "0")
            {
                txtDiscount.Focus();
                //txtDiscount.Visible = true;
                //txtDiscountAmt.Visible = false;
                //lblDisAmt.Visible = true;
            }
            else
            {
                txtDiscountAmt.Focus();
                // txtDiscount.Visible = false;
                //txtDiscountAmt.Visible = true;
                // lblDisAmt.Visible = false;
            }
        }
        protected void rdrAmt_CheckedChanged(object sender, EventArgs e)
        {
            if (rdrPercentage.Checked)
            {
                txtDiscount.Focus();
                //txtDiscount.Visible = true;
                //txtDiscountAmt.Visible = false;
                //lblDisAmt.Visible = true;
                //            txtDiscount_TextChanged(null, null);
            }
            else
            {
                txtDiscountAmt.Focus();
                //txtDiscount.Visible = false;
                //txtDiscountAmt.Visible = true;
                //lblDisAmt.Visible = false;
            }
        }
        protected void rdrPercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (rdrPercentage.Checked)
            {
                txtDiscount.Focus();
                //txtDiscount.Visible = true;
                //txtDiscountAmt.Visible = false;
                //lblDisAmt.Visible = true;
                //txtDiscount_TextChanged(null, null);
            }
            else
            {
                txtDiscountAmt.Focus();
                //txtDiscount.Visible = false;
                //txtDiscountAmt.Visible = true;
                //lblDisAmt.Visible = false;
            }
        }
        protected void btnF8_Click(object sender, EventArgs e)
        {
            btnSaveBooking_Click(btnSaveBooking, null);
        }
        protected void btnF2_Click(object sender, EventArgs e)
        {
            string status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.SearchOrderRight, Session["UserType"].ToString());
            if (status == "True")
            {
                Response.Redirect("~/Reports/SearchByInvoice.aspx");
            }
            else
            {
                Session["ReturnMsg"] = "You are not authorized !";
            }

        }
        protected void txtNewItemName_TextChanged(object sender, EventArgs e)
        {
            int tq = 0;
            lstSubItem.Items.Add(txtNewItemName.Text);
            txtNewItemName.Text = "";
            txtNewItemName.Focus();
            int TSQ = Convert.ToInt32(txtItemSubQty.Text);

            for (int i = 0; i < lstSubItem.Items.Count; i++)
            {
                tq += 1;

            }
            // if (TSQ == tq)
            // txtNewItemName.Visible = false;
        }
        protected void lstSubItem_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblSaveOption.Text == "2")
            {
                txtNewItemName.Text = lstSubItem.SelectedValue;
                lstSubItem.Items.Remove(lstSubItem.SelectedValue.ToString());
            }
        }
        protected void SetTextbox1()
        {
            try
            {
                int TotalSubQty = Convert.ToInt32(txtItemSubQty.Text);

                if (TotalSubQty < 1 || TotalSubQty > 10)
                {
                    lblErr.Text = "Invalid Sub Items";
                    // lstSubItem.Visible = false;
                    // // txtNewItemName.Visible = false;
                    return;
                }
                if (TotalSubQty == 1)
                {
                    // txtNewItemName.Visible = false;
                    // lstSubItem.Visible = false;
                    txtItemSubQty.Focus();
                    lstSubItem.Items.Clear();
                    return;
                }
                if (TotalSubQty >= 2)
                {
                    // txtNewItemName.Visible = true;
                    txtNewItemName.Focus();
                    //if (lblSaveOption.Text != "2")
                    //{
                    //    lstSubItem.Items.Clear();
                    //}
                    // lstSubItem.Visible = true;
                    return;
                }
            }
            catch (Exception)
            {
                lblErr.Text = "Invalid sub items";
            }
        }
        static string name = "";
        protected void chkToday_CheckedChanged1(object sender, EventArgs e)
        {
            name = txtCustomerName.Text;
            if (chkToday.Checked)
            {
                DataSet ds = new DataSet();
                ds = BAL.BALFactory.Instance.BAL_New_Bookings.FirstTimeDefaultData(Globals.BranchID);
                DateTime dt = DateTime.Parse(date[0].ToString());
                txtDueDate.Text = dt.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0][6].ToString())).ToString("dd MMM yyyy");
                chkNextDay.Checked = false;
            }
            else
                Deafaultsetting();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
            txtCustomerName.Text = name;
        }
        protected void chkNextDay_CheckedChanged1(object sender, EventArgs e)
        {
            name = txtCustomerName.Text;
            if (chkNextDay.Checked)
            {
                DataSet ds = new DataSet();
                ds = BAL.BALFactory.Instance.BAL_New_Bookings.FirstTimeDefaultData(Globals.BranchID);
                DateTime dt = DateTime.Parse(date[0].ToString());
                txtDueDate.Text = dt.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0][7].ToString())).ToString("dd MMM yyyy");
                chkToday.Checked = false;
            }
            else
                Deafaultsetting();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
            txtCustomerName.Text = name;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            string name = txtCustomerName.Text;
            Session["Custcode"] = null;
            BindGridView();
            txtItemSubQty_TextChanged(null, null);
            //chkOldBooking.Enabled = true;
            chkToday.Enabled = true;
            chkNextDay.Enabled = true;
            chkToday.Checked = false;
            chkNextDay.Checked = false;
            //chkOldBooking.Checked = false;
            Deafaultsetting();
            txtTotal.Text = "0";
            txtBalance.Text = "0";
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtQty")).Attributes.Add("onfocus", "javascript:select();");
            txtCustomerName.Text = name;
        }
        protected void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            // BAL.BALFactory.Instance.BAL_New_Bookings.SaveRemarks(((TextBox)grdEntry.HeaderRow.FindControl("txtRemarks")).Text, Globals.BranchID);
            ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Focus();
            ((TextBox)grdEntry.HeaderRow.FindControl("txtColor")).Attributes.Add("onfocus", "javascript:select();");
        }
        private void binddrpdefaultsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
            PrjClass.SetItemInDropDown(drpsmstemplate, ds.Tables[3].Rows[0]["Template"].ToString(), true, false);

        }
        private void binddrpsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.ShowAll(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpsmstemplate.DataSource = ds.Tables[0];
                drpsmstemplate.DataTextField = "template";
                drpsmstemplate.DataValueField = "smsid";
                drpsmstemplate.DataBind();
            }
        }

        /// <summary>
        /// Back up the entBookings in case of edit booking
        /// </summary>
        /// <param name="bookingNumber">Booking number</param>
        /// <param name="branchId">Branch Id</param>
        private void BackUpBookingDetails(string bookingNumber, string branchId)
        {
            BAL.BALFactory.Instance.BAL_New_Bookings.BackUpBookingDetails(bookingNumber, Globals.UserName, branchId);
        }

        /// <summary>
        /// Back up the entBookingDetails in case of edit booking
        /// </summary>
        /// <param name="bookingNumber">Booking Number</param>
        /// <param name="branchId">Branch Id</param>
        private void BackUpBookings(string bookingNumber, string branchId)
        {
            BAL.BALFactory.Instance.BAL_New_Bookings.BackUpBookings(bookingNumber, Globals.UserName, isInEditMode.Value, branchId);
        }
        /// <summary>
        /// Back up the payments
        /// </summary>
        /// <param name="bookingNumber"></param>
        /// <param name="branchId"></param>
        private void BackUpPayment(string bookingNumber, string branchId)
        {
            BAL.BALFactory.Instance.BAL_New_Bookings.BackUpPayment(bookingNumber, Globals.UserName, branchId);
        }

        /// <summary>
        /// Back the the whole booking, including EntBookings, EntBookingDetails and EnyPayment
        /// </summary>
        /// <param name="bookingNumber">Booking number to make back up of</param>
        /// <param name="branchId">BranchId</param>
        private void BackUpWholeBooking(string bookingNumber, string branchId)
        {
            BAL.BALFactory.Instance.BAL_New_Bookings.BackUpWholeBooking(bookingNumber, Globals.UserName, isInEditMode.Value, branchId);
        }

        private void SaveInvoiceHistoryData()
        {
            try
            {               
                var tmpBranchid2 = Globals.BranchID;
                var tmpBookingNo = Obj.BOOKINGNUMBER;
                var tmpUserName1 = Globals.UserName;
                Task t = Task.Factory.StartNew
                (
                    () => { InsertInvoiceHistoryData(tmpBranchid2, tmpBookingNo, tmpUserName1); }
                );
            }
            catch (Exception ex) { }
        }


        private void InsertInvoiceHistoryData(string tmpBranchid2, string tmpBookingNo, string tmpUserName1)
        {
            DataSet DsInvoiceData = new DataSet();
             DsInvoiceData = BAL.BALFactory.Instance.BAL_New_Bookings.GetInvoiceBookingData(tmpBranchid2, tmpBookingNo);
             if (DsInvoiceData.Tables[0].Rows.Count > 0)
             {
                 if (DsInvoiceData.Tables[0].Rows[0]["BookingByCustomer"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["BookingByCustomer"].ToString().Trim())
                 {
                     var strCustNameMsg = "Customer modified : " + DsInvoiceData.Tables[1].Rows[0]["BookingByCustomer"].ToString().Trim() + " ," + DsInvoiceData.Tables[1].Rows[0]["CustomerName"].ToString().Trim() + " changed to " + DsInvoiceData.Tables[0].Rows[0]["BookingByCustomer"].ToString().Trim() + " ," + DsInvoiceData.Tables[0].Rows[0]["CustomerName"].ToString().Trim();
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strCustNameMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["BookingDeliveryDate"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["BookingDeliveryDate"].ToString().Trim())
                 {
                     var strDelDateMsg = "Due Date edited :  date changed from " + DateTime.Parse(DsInvoiceData.Tables[1].Rows[0]["BookingDeliveryDate"].ToString().Trim()).ToString("dd MMM yyyy") + " to " + DateTime.Parse(DsInvoiceData.Tables[0].Rows[0]["BookingDeliveryDate"].ToString().Trim()).ToString("dd MMM yyyy");
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strDelDateMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["IsUrgent"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["IsUrgent"].ToString().Trim())
                 {
                     var strUrgentMsg = DsInvoiceData.Tables[0].Rows[0]["IsUrgent"].ToString().Trim() == "True" ? "Order status changed to Urgent." : "Order status changed from Urgent to regular.";
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strUrgentMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["BookingDeliveryTime"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["BookingDeliveryTime"].ToString().Trim())
                 {
                     var strDelTimeMsg = "Due Time edited : time changed from " + DsInvoiceData.Tables[1].Rows[0]["BookingDeliveryTime"].ToString().Trim() + " to " + DsInvoiceData.Tables[0].Rows[0]["BookingDeliveryTime"].ToString().Trim();
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strDelTimeMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["HomeDelivery"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["HomeDelivery"].ToString().Trim())
                 {
                     var strHomeDelMsg = DsInvoiceData.Tables[0].Rows[0]["HomeDelivery"].ToString().Trim() == "True" ? "Order delivery point marked to home delivery." : "Order delivery point changed from home delivery to store pick up.";
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strHomeDelMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["BookingRemarks"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["BookingRemarks"].ToString().Trim())
                 {
                     var strOrderRemarkMsg = "Oder notes edited : changed from " + DsInvoiceData.Tables[1].Rows[0]["BookingRemarks"].ToString().Trim() + " to " + DsInvoiceData.Tables[0].Rows[0]["BookingRemarks"].ToString().Trim()+".";
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strOrderRemarkMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["WorkShopNote"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["WorkShopNote"].ToString().Trim())
                 {
                     if (DsInvoiceData.Tables[0].Rows[0]["WorkShopNote"].ToString().Trim() != ",")
                     {
                         var strWorkshopRemarkMsg = "Workshop notes edited : changed from " + DsInvoiceData.Tables[1].Rows[0]["WorkShopNote"].ToString().Trim() + " to " + DsInvoiceData.Tables[0].Rows[0]["WorkShopNote"].ToString().Trim()+".";
                         BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strWorkshopRemarkMsg, ScreenName.BookingScreen,1);
                     }
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["CheckedByEmployee"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["CheckedByEmployee"].ToString().Trim())
                 {
                     var strCheckedMsg = "Garment inspected by user changed from " + DsInvoiceData.Tables[1].Rows[0]["EmployeeName"].ToString().Trim() + " to " + DsInvoiceData.Tables[0].Rows[0]["EmployeeName"].ToString().Trim()+".";
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strCheckedMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["DiscountOption"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["DiscountOption"].ToString().Trim())
                 {
                     string DiscountMsg = string.Empty;
                     if (Convert.ToInt32(DsInvoiceData.Tables[0].Rows[0]["DiscountOption"].ToString().Trim()) == 1)
                     {
                         DiscountMsg = "Discount modified : Mode changed from  % to Flat.";
                     }
                     else
                     {
                         DiscountMsg = "Discount modified : Mode changed from Flat to %.";
                     }
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, DiscountMsg, ScreenName.BookingScreen,1);
                 }
                 if (DsInvoiceData.Tables[0].Rows[0]["DiscountAmt"].ToString().Trim() != DsInvoiceData.Tables[1].Rows[0]["DiscountAmt"].ToString().Trim())
                 {
                     string strDisAmtMsg = string.Empty;
                     if (Convert.ToInt32(DsInvoiceData.Tables[0].Rows[0]["DiscountOption"].ToString().Trim()) == 0)
                     {
                         strDisAmtMsg = "Discount modified : Discount applied for " + DsInvoiceData.Tables[0].Rows[0]["Discount"].ToString().Trim() + "%.";
                     }
                     else
                     {
                         strDisAmtMsg = "Discount modified : Discount applied for the amount " + DsInvoiceData.Tables[0].Rows[0]["DiscountAmt"].ToString().Trim() + ".";
                     }
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strDisAmtMsg, ScreenName.BookingScreen,1);
                 }
                 // Item Details Check

                 var ItemDataForCompare = hdnAllGridData.Value.Split('_');
                 string strCurrentItem = string.Empty, strOldItem = string.Empty, strItemUpdateMsg = string.Empty;
                 string strNewItemAddMsg = string.Empty, strAllItem = string.Empty;
                 DataTable FilterBackupData = new DataTable();
                 DataTable dtCurrentBookingDetail = new DataTable();
                 string QtyMsg = string.Empty, ItemProcessTypeMsg = string.Empty, itemExtProc1Msg = string.Empty, itemExtProcRate1Msg = string.Empty;
                 string rateMsg = string.Empty, itemExtProc2Msg = string.Empty, itemExtProcRate2Msg = string.Empty, descMsg = string.Empty, colorMsg = string.Empty; ;
                 var ItemRate = 0.0;
                 var tmpItemRate = 0.0;
                 int RecordCount = 0, currCounter = 0;
                 string strIsUpdateProc1 = string.Empty, strIsUpdateProc2 = string.Empty;
                 for (int i = 0; i < ItemDataForCompare.Length; i++)
                 {
                     strIsUpdateProc1 = "";
                     strIsUpdateProc2 = "";
                     strCurrentItem = ItemDataForCompare[i].Split(':')[2].Trim();
                     strOldItem = ItemDataForCompare[i].Split(':')[7].Trim();
                     if ((strCurrentItem != strOldItem) && (strOldItem != ""))
                     {
                         strAllItem = strAllItem + strOldItem + ",";
                         strItemUpdateMsg = "Garment details modified : Garment changed from " + strOldItem + " to " + strCurrentItem + ".";
                         BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strItemUpdateMsg, ScreenName.BookingScreen,1);
                     }
                     else if (strOldItem == "")
                     {
                         strAllItem = strAllItem + strCurrentItem + ",";
                         strNewItemAddMsg = "New garment added to order: Qty = " + ItemDataForCompare[i].Split(':')[1].Trim() + " Garment = " + strCurrentItem + ", Services = " + ItemDataForCompare[i].Split(':')[3].Trim();
                         BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strNewItemAddMsg, ScreenName.BookingScreen,1);
                     }
                     else
                     {
                         strAllItem = strAllItem + strCurrentItem + ",";
                     }
                     // Compare all field value
                     if (strOldItem != "")
                     {
                         RecordCount = DsInvoiceData.Tables[3].Select("[ItemName] ='" + strOldItem + "'").Count();
                         FilterBackupData.Clear();
                         if (RecordCount != 0)
                         {
                             FilterBackupData = DsInvoiceData.Tables[3].Select("[ItemName] ='" + strOldItem + "'").CopyToDataTable();
                         }

                         currCounter = DsInvoiceData.Tables[3].Select("[ItemName] ='" + strCurrentItem + "'").Count();
                         dtCurrentBookingDetail.Clear();
                         if (RecordCount != 0)
                         {
                             dtCurrentBookingDetail = DsInvoiceData.Tables[2].Select("[ItemName] ='" + strCurrentItem + "'").CopyToDataTable();
                         }
                         if ((FilterBackupData.Rows.Count > 0) && (dtCurrentBookingDetail.Rows.Count > 0))
                         {
                             if (Convert.ToInt32(dtCurrentBookingDetail.Rows[0]["ItemTotalQuantity"].ToString()) != Convert.ToInt32(FilterBackupData.Rows[0]["ItemTotalQuantity"].ToString()))
                             {
                                 QtyMsg = "Garment details modified : Qty changed for garment  " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + "  from " + FilterBackupData.Rows[0]["ItemTotalQuantity"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemTotalQuantity"].ToString() + ".";
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, QtyMsg, ScreenName.BookingScreen,1);
                             }

                             ItemRate = Convert.ToDouble(dtCurrentBookingDetail.Rows[0]["ItemQuantityAndRate"].ToString().Split('@').ElementAt(1));
                             tmpItemRate = Convert.ToDouble(FilterBackupData.Rows[0]["ItemQuantityAndRate"].ToString().Split('@').ElementAt(1));
                             if (ItemRate != tmpItemRate)
                             {
                                 rateMsg = "Garment details modified : Service " + dtCurrentBookingDetail.Rows[0]["ItemProcessType"].ToString() + " rate change for garment  " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + "  from " + tmpItemRate + " to " + ItemRate;
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, rateMsg, ScreenName.BookingScreen,1);
                             }

                             if (dtCurrentBookingDetail.Rows[0]["ItemProcessType"].ToString() != FilterBackupData.Rows[0]["ItemProcessType"].ToString())
                             {
                                 ItemProcessTypeMsg = "Garment details modified : Service modified for  - " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " changed from " + FilterBackupData.Rows[0]["ItemProcessType"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemProcessType"].ToString() + ".";
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, ItemProcessTypeMsg, ScreenName.BookingScreen,1);
                             }
                             if (dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType1"].ToString() != FilterBackupData.Rows[0]["ItemExtraProcessType1"].ToString())
                             {
                                 if (FilterBackupData.Rows[0]["ItemExtraProcessType1"].ToString() == "None")
                                 {
                                     itemExtProc1Msg = "Garment details modified : Add second service : " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " - (" + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType1"].ToString() + "@" + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate1"].ToString() + ").";
                                     strIsUpdateProc1 = "1";
                                 }
                                 else if ((dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType1"].ToString() == "None") && ( Convert.ToDouble(dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate1"].ToString()) == 0))
                                 {
                                    itemExtProc1Msg = "Garment details modified : Remove second service : " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " - (" + FilterBackupData.Rows[0]["ItemExtraProcessType1"].ToString() + "@" + FilterBackupData.Rows[0]["ItemExtraProcessRate1"].ToString() + ").";
                                    strIsUpdateProc1 = "1";
                                 }
                                 else
                                 {
                                     itemExtProc1Msg = "Garment details modified : Change second service : " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " from " + FilterBackupData.Rows[0]["ItemExtraProcessType1"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType1"].ToString() + ".";                                     
                                 }
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, itemExtProc1Msg, ScreenName.BookingScreen,1);
                             }

                             if (Convert.ToDouble(dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate1"].ToString()) != Convert.ToDouble(FilterBackupData.Rows[0]["ItemExtraProcessRate1"].ToString()))
                             {
                                 if (strIsUpdateProc1 != "1")
                                 {
                                     itemExtProcRate1Msg = "Garment details modified : Change second service " + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType1"].ToString() + " rate  " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " from " + FilterBackupData.Rows[0]["ItemExtraProcessRate1"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate1"].ToString() + ".";
                                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, itemExtProcRate1Msg, ScreenName.BookingScreen,1);
                                 }
                             }
                             if (dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType2"].ToString() != FilterBackupData.Rows[0]["ItemExtraProcessType2"].ToString())
                             {
                                 if (FilterBackupData.Rows[0]["ItemExtraProcessType2"].ToString() == "None")
                                 {
                                     itemExtProc2Msg = "Garment details modified : Add third service : " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " - (" + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType2"].ToString() + "@" + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate2"].ToString() + ").";
                                     strIsUpdateProc2 = "1";
                                 }
                                 else if ((dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType2"].ToString() == "None") && (Convert.ToDouble(dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate2"].ToString()) == 0))
                                 {
                                     itemExtProc2Msg = "Garment details modified : Remove third service : " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " - (" + FilterBackupData.Rows[0]["ItemExtraProcessType2"].ToString() + "@" + FilterBackupData.Rows[0]["ItemExtraProcessRate2"].ToString() + ").";
                                     strIsUpdateProc2 = "1";
                                 }
                                 else
                                 {
                                     itemExtProc2Msg = "Garment details modified : Change third service : " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " from " + FilterBackupData.Rows[0]["ItemExtraProcessType2"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType2"].ToString() + ".";                                    
                                 }
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, itemExtProc2Msg, ScreenName.BookingScreen,1);
                             }
                             if (Convert.ToDouble(dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate2"].ToString()) != Convert.ToDouble(FilterBackupData.Rows[0]["ItemExtraProcessRate2"].ToString()))
                             {
                                 if (strIsUpdateProc2 != "1")
                                 {
                                     itemExtProcRate2Msg = "Garment details modified : Change third service " + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessType2"].ToString() + " rate  " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " from " + FilterBackupData.Rows[0]["ItemExtraProcessRate2"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemExtraProcessRate2"].ToString() + ".";
                                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, itemExtProcRate2Msg, ScreenName.BookingScreen,1);
                                 }
                             }
                             if (dtCurrentBookingDetail.Rows[0]["ItemRemark"].ToString().Trim() != FilterBackupData.Rows[0]["ItemRemark"].ToString().Trim())
                             {
                                 if (FilterBackupData.Rows[0]["ItemRemark"].ToString() == "")
                                 {
                                     descMsg = "Garment details modified : Add Remarks  " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " - " + dtCurrentBookingDetail.Rows[0]["ItemRemark"].ToString() + ".";
                                 }
                                 else
                                 {
                                     descMsg = "Garment details modified : Remarks Modified for " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " from " + FilterBackupData.Rows[0]["ItemRemark"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemRemark"].ToString() + ".";
                                 }
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, descMsg, ScreenName.BookingScreen,1);
                             }

                             if (dtCurrentBookingDetail.Rows[0]["ItemColor"].ToString().Trim() != FilterBackupData.Rows[0]["ItemColor"].ToString().Trim())
                             {
                                 if (FilterBackupData.Rows[0]["ItemColor"].ToString() == "")
                                 {
                                     colorMsg = "Garment details modified : Add Colours " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " - " + dtCurrentBookingDetail.Rows[0]["ItemColor"].ToString() + ".";
                                 }
                                 else
                                 {
                                     colorMsg = "Garment details modified : Colours modified for " + dtCurrentBookingDetail.Rows[0]["ItemName"].ToString() + " from " + FilterBackupData.Rows[0]["ItemColor"].ToString() + " to " + dtCurrentBookingDetail.Rows[0]["ItemColor"].ToString() + ".";
                                 }
                                 BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, colorMsg, ScreenName.BookingScreen,1);
                             }
                         }
                     }
                 }

                 strAllItem = strAllItem.Substring(0, strAllItem.Length - 1);
                 DataSet dsDeleteItems = new DataSet();
                 dsDeleteItems = GetDeleteItemData(tmpBranchid2, tmpBookingNo, strAllItem);
                 string strProcess1 = string.Empty, strProcess2 = string.Empty;
                 string strDelMsg = string.Empty;
                 if (dsDeleteItems.Tables[0].Rows.Count > 0)
                 {
                     for (int j = 0; j < dsDeleteItems.Tables[0].Rows.Count; j++)
                     {
                         strProcess1 = dsDeleteItems.Tables[0].Rows[j]["ItemExtraProcessType1"].ToString() != "None" ? ",(" + dsDeleteItems.Tables[0].Rows[j]["ItemExtraProcessType1"].ToString() + "@" + dsDeleteItems.Tables[0].Rows[j]["ItemExtraProcessRate1"].ToString()+")" : "";
                         strProcess2 = dsDeleteItems.Tables[0].Rows[j]["ItemExtraProcessType2"].ToString() != "None" ? ",(" + dsDeleteItems.Tables[0].Rows[j]["ItemExtraProcessType2"].ToString() + "@" + dsDeleteItems.Tables[0].Rows[j]["ItemExtraProcessRate2"].ToString() + ")" : "";
                         strDelMsg = "Garment deleted from order : Qty = " + dsDeleteItems.Tables[0].Rows[j]["ItemQuantityAndRate"].ToString().Split('@').ElementAt(0) + ", Garment = " + dsDeleteItems.Tables[0].Rows[j]["ItemName"].ToString() + ", Services = (" + dsDeleteItems.Tables[0].Rows[j]["ItemQuantityAndRate"].ToString() + ")" + strProcess1 + strProcess2 + ".";
                         BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, strDelMsg, ScreenName.BookingScreen,1);
                     }
                 }
             }
            // Payment Check
             if (DsInvoiceData.Tables[4].Rows.Count > 0)
             {
                 if (DsInvoiceData.Tables[4].Rows[0]["PaymentMade"].ToString().Trim() != DsInvoiceData.Tables[5].Rows[0]["PaymentMade"].ToString().Trim())
                 {
                     var AdvanceCashMsg = "Advance value modified : amount changed from " + DsInvoiceData.Tables[5].Rows[0]["PaymentMade"].ToString().Trim() + " to " + DsInvoiceData.Tables[4].Rows[0]["PaymentMade"].ToString().Trim();
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, AdvanceCashMsg, ScreenName.BookingScreen,1);
                 }

                 if (DsInvoiceData.Tables[4].Rows[0]["PaymentMode"].ToString().Trim() != DsInvoiceData.Tables[5].Rows[0]["PaymentMode"].ToString().Trim())
                 {
                     var AdvanceModeMsg = "Advance value modified : Payment mode changed from " + DsInvoiceData.Tables[5].Rows[0]["PaymentMode"].ToString().Trim() + " to " + DsInvoiceData.Tables[4].Rows[0]["PaymentMode"].ToString().Trim();
                     BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(tmpBookingNo, tmpUserName1, tmpBranchid2, AdvanceModeMsg, ScreenName.BookingScreen,1);
                 }
             }
        }
        private DataSet GetDeleteItemData(string BID, string BookingNo, string strAllItem)
        {
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_Dry_EditInvoiceHistory";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@BookingNumber", BookingNo);
            CMD.Parameters.AddWithValue("@AllItem", strAllItem);
            CMD.Parameters.AddWithValue("@Flag", 2);
            ds = PrjClass.GetData(CMD);
            return ds;  
        }
    }
}