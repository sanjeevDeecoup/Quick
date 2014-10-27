using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Globalization;

public partial class Delivery : System.Web.UI.Page
{
    private string pgPageName = "Delivery.aspx";
    private DTO.Report Ob = new DTO.Report();
    private ArrayList date = new ArrayList();
    private DataTable dsSrc = new DataTable();
    private int _iRowsCounter = 0;
    private string _testCurRowText = string.Empty;
    string tmpBranchID =string.Empty,tmpUserName = string.Empty;  

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserType"] == null)
        {
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl, false);
        }
        else
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPage(this.Page);
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtDeliveryDate.Text = date[0].ToString();
                hdnTmpDelDate.Value = txtDeliveryDate.Text.Trim();
                CheckDeliveryDateRights();
                binddrpsms();
                binddrpdefaultsms();
                if (BAL.BALFactory.Instance.BAL_sms.SetSMSCheckBoxOnScreen(Globals.BranchID, "14") == true)
                {
                    chkSendSms.Checked = true;
                }
                else
                {
                    chkSendSms.Checked = false;
                }
                if (Request.QueryString["BN"] != null)
                {
                    txtBookingNumber.Text = Request.QueryString["BN"].ToString();
                    btnShow_Click(null, null);
                }
                if (Request.QueryString["CustCode"] != null)
                {
                    txtCustomerName_TextChanged(null, null);
                }
                txtBookingNumber.Focus();
                lblMsgStatus.Text = "";
            }
            txtBookingNumber.Focus();
            if (dtvBookingDetails.Visible == false)
                grdItemDetails.Visible = false;
        }
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

    private void binddrpdefaultsms()
    {
        DTO.sms Ob = new DTO.sms();
        DataSet ds = new DataSet();
        Ob.BranchId = Globals.BranchID;
        ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
        PrjClass.SetItemInDropDown(drpsmstemplate, ds.Tables[2].Rows[0]["Template"].ToString(), true, false);
    }

    private void CheckDeliveryDateRights()
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            cmd.CommandText = "SELECT RightToView FROM dbo.EntMenuRights WHERE (PageTitle = '" + SpecialAccessRightName.BackDateDel+ "') AND (UserTypeId = '" + Session["UserType"].ToString() + "') AND (BranchId = '" + Globals.BranchID + "') ";
            cmd.CommandType = CommandType.Text;
            sdr = PrjClass.ExecuteReader(cmd);
            string statue = string.Empty;
            if (sdr.Read())
                statue = "" + sdr.GetValue(0);
            if (statue == "True")
            {
                txtDeliveryDate.Enabled = true;
                hdnReportType.Value = "TRUE";
            }
            else
            {
                txtDeliveryDate.Enabled = false;
                hdnReportType.Value = "FALSE";
            }
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
    }

    private bool CheckForceMultipleDeliveryButtonRight()
    {
        bool ReturnID = false;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            cmd.CommandText = "SELECT RightToView FROM dbo.EntMenuRights WHERE (PageTitle = '" + SpecialAccessRightName.AllowDelBarcode+ "') AND (UserTypeId = '" + Session["UserType"].ToString() + "') AND (BranchId = '" + Globals.BranchID + "') ";
            cmd.CommandType = CommandType.Text;
            sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
                ReturnID = Convert.ToBoolean(sdr.GetValue(0));
        }
        catch
        { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        return ReturnID;
    }

    public DTO.Report SetValue()
    {
        Ob.InvoiceNo = txtBookingNumber.Text;
        Ob.UptoDate = txtDeliveryDate.Text;
        Ob.CustId = hdnReportType.Value;
        Ob.BranchId = Globals.BranchID;
        return Ob;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        //txtBookingNumber_TextChanged(null, null);
        string[] Bookingno = txtBookingNumber.Text.Split('-');
        txtBookingNumber.Text = Bookingno[0].ToString();
        int status = 0;
        var assignId = string.Empty;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 11);
            cmd.Parameters.AddWithValue("@BookingNumber", txtBookingNumber.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            sdr = AppClass.ExecuteReader(cmd);            
            if (sdr.Read())
            {
                status = Convert.ToInt32(sdr.GetValue(0));
                assignId = sdr.GetValue(1).ToString();
            }
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        if (status != 5)
        {
            if (BAL.BALFactory.Instance.BAL_sms.CheckDeliverSlipViewRight(Globals.BranchID, txtBookingNumber.Text, Globals.UserType) == false)
            {
                DataSet dsMain = new DataSet();
                dsMain = BAL.BALFactory.Instance.BAL_City.DeliveryDetail(Globals.BranchID, txtBookingNumber.Text);
                DetailsViewDeliverSlip.DataSource = dsMain.Tables[0];
                DetailsViewDeliverSlip.DataBind();
                DetailsViewDeliverSlip.Visible = true;
                DeliverSlip.Visible = true;
                Panel1.Visible = false;
                trBarcode.Visible = false;
                sendsms.Visible = false;
            }
            else
            {
                DetailsViewDeliverSlip.Visible = false;
                DeliverSlip.Visible = false;
                ShowDetails("");
                CheckDeliveredItems();
                if (status != 0)
                {
                    if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckAcceptPaymentButtonAcess(Globals.BranchID, Session["UserType"].ToString()) == true)
                    {
                        ((Button)dtvBookingDetails.Rows[10].FindControl("btnAcceptPayment")).Visible = true;
                        ((Button)dtvBookingDetails.Rows[10].FindControl("Button1")).Visible = true;
                    }
                    else
                    {
                        ((Button)dtvBookingDetails.Rows[10].FindControl("btnAcceptPayment")).Visible = false;
                        ((Button)dtvBookingDetails.Rows[10].FindControl("Button1")).Visible = false;
                    }
                }
                lblPkgText.Attributes.Add("style", "display: none;");
                lblPkgValue.Attributes.Add("style", "display: none;");
                if (dtvBookingDetails.Rows.Count > 0)
                {
                    bool rights = AppClass.CheckButtonRights(SpecialAccessRightName.DisFromDel);
                    if (rights == true)
                    {
                        ((LinkButton)dtvBookingDetails.FindControl("lnkShowDiscount")).Visible = true;
                    }
                    else
                    {
                        ((LinkButton)dtvBookingDetails.FindControl("lnkShowDiscount")).Visible = false;
                    }
                }
                if (assignId != null)
                {
                    if (assignId != string.Empty)
                    {
                        int assId;
                        if (Int32.TryParse(assignId, out assId) && assId != 0)
                        {
                            ((DropDownList)dtvBookingDetails.FindControl("drpPaymentType")).Items.Insert(0, "Package");
                            ((DropDownList)dtvBookingDetails.FindControl("drpPaymentType")).Enabled = false;
                            ((DropDownList)dtvBookingDetails.FindControl("drpPaymentType")).SelectedIndex = 0;
                            //((LinkButton)dtvBookingDetails.FindControl("lnkShowDiscount")).Attributes.Add("style", "display: none;");
                            var pkgValue = BAL.BALFactory.Instance.BAL_New_Bookings.CheckDetailsOfPackage(((Label)dtvBookingDetails.FindControl("lblCustomerCode")).Text, null, Globals.BranchID);
                            if (string.IsNullOrEmpty(pkgValue))
                            {
                                // this happens when package is marked complete
                                ((DropDownList)dtvBookingDetails.FindControl("drpPaymentType")).Items.Remove("Package");
                                ((LinkButton)dtvBookingDetails.FindControl("lnkShowDiscount")).Attributes["style"] = "block";
                            }
                            else
                            {
                                if (pkgValue.Split(':')[7] != "Discount")
                                {
                                    lblPkgValue.Text = pkgValue.Split(':')[6];
                                    lblPkgText.Attributes.Remove("style");
                                    lblPkgValue.Attributes.Remove("style");
                                    // if package is expired: i.e. the end date is less then datetime
                                    if (DateTime.Parse(txtDeliveryDate.Text) > DateTime.Parse(pkgValue.Split(':')[3]))
                                        hdnPackageExpired.Value = "true:" + pkgValue.Split(':')[4];
                                    else
                                        hdnPackageExpired.Value = "dummy:" + pkgValue.Split(':')[4];
                                }
                            }
                        }
                    }
                }
            }

            if (status != 0)
            {
               InsertHistoryData();
            }
        }
        else
        {
            Session["ReturnMsg"] = "This booking number is cancelled";
            txtBookingNumber.Focus();
            txtBookingNumber.Attributes.Add("onfocus", "javascript:select();");
            return;
        }
    }

    private void ShowDetails(string CustomerWiseBookingNo)
    {
        dtvBookingDetails.DataSource = null;
        dtvBookingDetails.DataBind();
        dtvBookingDetails.Visible = false;
        DetailsViewDeliverSlip.Visible = false;
        grdItemDetails.Visible = false;
        grdPaymentDetails.Visible = false;
        string strBookingNumber = "" + txtBookingNumber.Text.Trim();
        if (strBookingNumber == "")
        {
            strBookingNumber = CustomerWiseBookingNo;
            txtBookingNumber.Text = strBookingNumber;
        }

        DataSet dsMain = new DataSet();
        try
        {
            dsMain = BAL.BALFactory.Instance.BAL_City.DeliveryDetail(Globals.BranchID, txtBookingNumber.Text);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                dtvBookingDetails.DataSource = dsMain.Tables[0];
                dtvBookingDetails.DataBind();
                grdItemDetails.DataSource = dsMain.Tables[1];
                grdItemDetails.DataBind();
                hdnBookingDate.Value = dsMain.Tables[0].Rows[0]["BookingDate"].ToString();
                grdItemDetails.Visible = true;
                grdPaymentDetails.DataSource = dsMain.Tables[2];
                grdPaymentDetails.DataBind();
                grdPaymentDetails.Visible = true;
                checkTotalClotheUpdated();
                GetRemarksOnDelivery(txtBookingNumber.Text);
            }
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error : " + excp.Message;
        }
        finally
        {
        }
        if (GrdPendingClothesAndPayment.Rows.Count > 0)
        {
            CalculateGridReport();
            Panel1.Visible = true;
            txtCustomerName.Focus();
        }
        if (dtvBookingDetails.Rows.Count > 0)
        {
            if (grdItemDetails.Rows.Count > 0)
            {
                CalculateItemDetails();
            }
            bool rights = AppClass.CheckButtonRights(SpecialAccessRightName.DisFromDel);
            if (rights == true)
            {
                ((LinkButton)dtvBookingDetails.FindControl("lnkShowDiscount")).Visible = true;
            }
            else
            {
                ((LinkButton)dtvBookingDetails.FindControl("lnkShowDiscount")).Visible = false;
            }
            dtvBookingDetails.Visible = true;
            float fltPrevPayment = float.Parse(dtvBookingDetails.Rows[8].Cells[1].Text);
            float fltDuePayment = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
            if (fltDuePayment > 0)
            {
                dtvBookingDetails.Rows[10].Visible = true;
                ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtPaidAmount")).Text = fltDuePayment.ToString();
            }
            else
            {
                dtvBookingDetails.Rows[10].Visible = false;
            }
            Ob.InvoiceNo = txtBookingNumber.Text;
            Ob.BranchId = Globals.BranchID;
            if (BAL.BALFactory.Instance.Bal_Report.CheckDiscountOnBookingNumber(Ob) == true)
                ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtDiscountGiven")).Enabled = false;
            else
                ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtDiscountGiven")).Enabled = true;

            btnRefresh.Visible = true;
            trBarcode.Visible = true;
            sendsms.Visible = true;
            txtReadBarcode.Focus();
        }
        else
        {
            Session["ReturnMsg"] = "Booking number " + txtBookingNumber.Text + " not found. May be challan has not been generated for entered booking number. or the booking might have been cancelled.";
            txtBookingNumber.Focus();
            DeliverSlip.Visible = false;
            Panel1.Visible = false;
            trBarcode.Visible = false;
            sendsms.Visible = false;
        }
    }

    private double GetServiceTax(string BookingNumber)
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        double serviceTax = 0.0;
        try
        {
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
            cmd.Parameters.AddWithValue("@Flag", 7);
            sdr = AppClass.ExecuteReader(cmd);
            if (sdr.Read())
            {
                serviceTax = Convert.ToDouble(sdr.GetValue(0));
            }
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        return serviceTax;
    }

    private void GetRemarksOnDelivery(string BookingNumber)
    {
        divgrdNotes.Visible = false;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        string Remarks = string.Empty;
        cmd.CommandText = "sp_Dry_EmployeeMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
        cmd.Parameters.AddWithValue("@Flag", 8);
        ds = AppClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdNotes.DataSource = ds.Tables[0];
            grdNotes.DataBind();
            divgrdNotes.Visible = true;
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            chkSatisfiedCustomer.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["SatisfiedCustomer"].ToString());
            chkWithoutSlip.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["DeliveryWithoutSlip"].ToString());
            hdntmpSatisfiedCust.Value = ds.Tables[1].Rows[0]["SatisfiedCustomer"].ToString();
            hdnTmpDelWithoupSlip.Value = ds.Tables[1].Rows[0]["DeliveryWithoutSlip"].ToString();
        }
    }

    protected void btnAcceptPayment_Click(object sender, EventArgs e)
    {
        SetValue();
        try
        {
            if (CheckCorrectEntries() == true)
            {
                string strBookingDate = dtvBookingDetails.Rows[2].Cells[1].Text;
                ArrayList Date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                string strDeliveryDate = Date[0].ToString();
                DateTime dtBookingDate = DateTime.Parse(strBookingDate);
                float fltDueAmount = float.Parse("0" + dtvBookingDetails.Rows[8].Cells[1].Text);
                float fltCashPaymentMade = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Text);
                float fltPaymentMade = fltCashPaymentMade;
                float fltPrevDuePayment = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
                float fltDiscount = 0;
                if (fltCashPaymentMade <= fltPrevDuePayment)
                {
                    if (((TextBox)dtvBookingDetails.Rows[9].FindControl("txtDiscountGiven")).Visible)
                    {
                        fltDiscount = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[9].FindControl("txtDiscountGiven")).Text);
                    }
                    string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
                    fltPaymentMade = fltPaymentMade - fltDiscount;
                    AcceptPayment(res);
                    BAL.BALFactory.Instance.Bal_Report.ChangeStatusAccordingBooking(txtBookingNumber.Text, Globals.BranchID);
                    DataSet dsBookingPrefix = new DataSet();
                    string strNewBookingNo = string.Empty;
                    string strNewBookingPrefix = string.Empty;
                    dsBookingPrefix = BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(txtBookingNumber.Text);
                    strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim().ToUpper();
                    strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString();
                    if (chkSendSms.Checked)
                    {
                        var tmp = Globals.BranchID;
                        if (BAL.BALFactory.Instance.BAL_sms.CheckDeliverSMSStatus(Globals.BranchID, txtBookingNumber.Text) == true)
                        {
                            Task t = Task.Factory.StartNew
                                            (
                                               () => { AppClass.GoMsg(tmp,strNewBookingPrefix + strNewBookingNo, drpsmstemplate.SelectedValue); }
                                            );
                        }
                    }                   
                }
                else
                {
                    Session["ReturnMsg"] = "Payment cannot exceed the due amount.";
                    ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Focus();
                    ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Attributes.Add("onfocus", "javascript:select();");
                    return;
                }
            }
            else
            {
                Session["ReturnMsg"] = "Payment cannot exceed the due amount.";
                ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Focus();
                ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Attributes.Add("onfocus", "javascript:select();");
                return;
            }
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error : " + excp.Message;
        }
    }

    private bool CheckCorrectEntries()
    {
        float fltPrevPayment = 0, NetAmount = 0, fltPaymentMadeNow = 0, fltDiscount = 0;
        try
        {
            fltPrevPayment = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
            NetAmount = float.Parse(dtvBookingDetails.Rows[7].Cells[1].Text);
            fltPaymentMadeNow = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtPaidAmount")).Text);
            fltDiscount = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtDiscountGiven")).Text);
            if (NetAmount == fltPrevPayment)
            {
                fltPaymentMadeNow = fltPaymentMadeNow + fltDiscount;
                if (NetAmount >= fltPaymentMadeNow)
                    return true;
                else
                    return false;
            }
            else
            {
                fltPaymentMadeNow = fltPaymentMadeNow + fltDiscount;
                if (fltPrevPayment >= fltPaymentMadeNow)
                    return true;
                else
                    return false;
            }
        }
        catch (Exception ex)
        {
            Session["ReturnMsg"] = "Given amount not correct.";
            ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Focus();
            ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Attributes.Add("onfocus", "javascript:select();");
            return false;
        }
    }

    private void AcceptPayment(string deliverTime)
    {
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlTransaction stx = null;
        SqlDataReader sdr = null;
        ArrayList Date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        DTO.Report SetValue = new DTO.Report();
        SetValue.InvoiceNo = dtvBookingDetails.Rows[0].Cells[1].Text;
        SetValue.BranchId = Globals.BranchID;
        //string strPayAcceptDate = DateTime.Today.ToShortDateString();
        if (BAL.BALFactory.Instance.Bal_Report.GetStatusNegativeEntry(SetValue) == true)
        {
            try
            {
                float fltCashPreBalance = 0, fltCashPostBalance = 0, fltCustPreBalance = 0, fltCustPostBalance = 0, fltDiscountPreBalance = 0, fltDiscountPostBalanace = 0;
                float fltPaymentMadeNow = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtPaidAmount")).Text);
                string PaymentType = ((DropDownList)dtvBookingDetails.Rows[15].Cells[1].FindControl("drpPaymentType")).SelectedItem.Text;              
                string PaymentDetails = ((TextBox)dtvBookingDetails.Rows[14].Cells[1].FindControl("txtPaymentDetails")).Text;            
                float fltPrevPayment = float.Parse(dtvBookingDetails.Rows[8].Cells[1].Text);
                float fltDiscount = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtDiscountGiven")).Text);
                float fltTenderAmt = float.Parse("0" + ((TextBox)dtvBookingDetails.FindControl("txtTenderAmt")).Text);
                //fltPaymentMadeNow = fltPaymentMadeNow - fltDiscount;
                float fltTotalPaymentMade = fltPrevPayment + fltPaymentMadeNow;
                float fltPrevDuePayment = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
                float fltDuePaymentNew = fltPrevDuePayment - fltPaymentMadeNow;
                string strBookingNumber = dtvBookingDetails.Rows[0].Cells[1].Text;
                //string strDeliveryDate = DateTime.Now.ToString() + " " + DateTime.Now.ToString("hh:mm:ss tt");

                DataSet dsBookingPrefix = new DataSet();
                string strNewBookingNo = string.Empty;
                string strNewBookingPrefix = string.Empty;
                dsBookingPrefix = BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(strBookingNumber);
                strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString();
                strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString();

                string sql = string.Empty;
                string strUpdateQty = string.Empty, strISN = string.Empty;

                sqlcon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                stx = sqlcon.BeginTransaction();
                cmd.Transaction = stx;

                sql = "DELETE FROM ENTPAYMENT WHERE PAYMENTMADE=0 AND DeliveryMsg IS  NULL AND PaymentType IS NULL AND BOOKINGNUMBER='" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                var pkgId = "0";
                if (hdnPackageExpired.Value.IndexOf(':') > -1)
                    pkgId = hdnPackageExpired.Value.Split(':')[1];
                if (PaymentType != "Package")
                    pkgId = "0";

                sql = "INSERT INTO EntPayment (BookingNumber, PaymentDate, PaymentMade, DiscountOnPayment, PaymentType,PaymentRemarks,PaymentTime,BranchId,TenderAmount,AcceptByUser,packageAssignId,BookingPrefix) VALUES ('" + strNewBookingNo + "', '" + txtDeliveryDate.Text + "' , '" + fltPaymentMadeNow + "', '" + fltDiscount + "', '" + PaymentType + "', '" + PaymentDetails + "', '" + deliverTime + "', '" + Globals.BranchID + "', '" + fltTenderAmt + "', '" + Globals.UserName + "', " + pkgId + ",'" + strNewBookingPrefix+ "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "UPDATE EntBookings SET ClothDeliverDate='" + txtDeliveryDate.Text + "',Format='" + deliverTime + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                if (pkgId != "0")
                {
                    sql = "UPDATE EntBookings SET AssignId=" + pkgId + " WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                //cmd.CommandText = "UPDATE EntBookings SET ClothDeliverDate='" + txtDeliveryDate.Text + "',SatisfiedCustomer='" + SatisfiedPlan + "',Format='" + deliverTime + "' WHERE BookingNumber = '" + strBookingNumber + "' AND BranchId = '" + Globals.BranchID + "'";
                //cmd.ExecuteNonQuery();

                string custcode = ((Label)dtvBookingDetails.Rows[1].Cells[1].FindControl("lblCustomerCode")).Text;
                string custname = ((Label)dtvBookingDetails.Rows[1].Cells[1].FindControl("lblCustomerName")).Text;
                string strCustomerLedgerName = custcode;

                sql = "Select OpenningBalance From LedgerMaster Where LedgerName='Cash' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (sdr.Read())
                {
                    fltCashPreBalance = float.Parse("" + sdr.GetValue(0));
                }
                sdr.Close();

                sql = "Select CurrentBalance From LedgerMaster Where LedgerName='Discount' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (sdr.Read())
                {
                    fltDiscountPreBalance = float.Parse("" + sdr.GetValue(0));
                }
                sdr.Close();

                sql = "Select CurrentBalance From LedgerMaster Where LedgerName='" + strCustomerLedgerName + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (sdr.Read())
                {
                    fltCustPreBalance = float.Parse("" + sdr.GetValue(0));
                }
                sdr.Close();

                fltCashPostBalance = fltCashPreBalance + fltPaymentMadeNow;
                fltCustPostBalance = fltCustPreBalance - fltPaymentMadeNow;
                fltDiscountPostBalanace = fltDiscountPreBalance + fltDiscountPostBalanace;

                sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,BookingNumber) Values('" + txtDeliveryDate.Text + "','Cash','By " + strCustomerLedgerName + "','" + fltCashPreBalance + "','" + fltPaymentMadeNow + "','0','" + fltCashPostBalance + "','Received payment against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Insert Into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,BookingNumber) Values('" + txtDeliveryDate.Text + "','" + strCustomerLedgerName + "','To Cash','" + fltCustPreBalance + "','0','" + fltPaymentMadeNow + "','" + fltCustPostBalance + "','Received payment against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                if (fltDiscount > 0)
                {
                    sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,BookingNumber) Values('" + txtDeliveryDate.Text + "','Discount','By " + strCustomerLedgerName + "','" + fltDiscountPreBalance + "','" + fltDiscount + "', '0', '" + fltDiscountPostBalanace + "','Received Discount against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Insert Into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,BookingNumber) Values('" + txtDeliveryDate.Text + "','" + strCustomerLedgerName + "','To Discount','" + fltCustPreBalance + "', '0', '" + fltDiscount + "','" + fltCustPostBalance + "','Received Discount against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update LedgerMaster Set CurrentBalance='" + fltDiscountPostBalanace.ToString() + "' Where LedgerName='Discount' AND BranchId='" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                sql = "Update LedgerMaster Set CurrentBalance=CurrentBalance-" + fltPaymentMadeNow.ToString() + " Where LedgerName='" + strCustomerLedgerName + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update LedgerMaster Set CurrentBalance=CurrentBalance+" + fltPaymentMadeNow.ToString() + " Where LedgerName='Cash' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                for (int r = 0; r < grdItemDetails.Rows.Count; r++)
                {
                    if (((CheckBox)grdItemDetails.Rows[r].FindControl("txtItemDelivered")).Checked)
                    {
                        strUpdateQty = "1";
                    }
                    else
                    {
                        strUpdateQty = "0";
                    }
                    strISN = ((HiddenField)grdItemDetails.Rows[r].FindControl("hdnItemSN")).Value;
                    cmd.CommandText = "UPDATE EntBookingDetails SET DeliveredQty='" + strUpdateQty + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND ISN = '" + strISN + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.ExecuteNonQuery();
                }
                stx.Commit();
                //UpdateRemarks(strBookingNumber, DeliveryRemarks);
                SaveHistoryData(fltDiscount, fltPaymentMadeNow, PaymentType, PaymentDetails);
                RefreshAll();
                lblMsg.Text = "Payment Accepted for Receipt # " + strBookingNumber;
                Response.Write(PrjClass.GetAndGo(lblMsg.Text, pgPageName));
            }
            catch (Exception excp)
            {
                if (stx != null) stx.Rollback();
                lblErr.Text = "Error : " + excp.ToString();
            }
            finally
            {
                sqlcon.Close();
                sdr.Close();
                sdr.Dispose();
            }
        }
    }

    private void AcceptPayment1(string deliverTime)
    {
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlTransaction stx = null;
        SqlDataReader sdr = null;
        ArrayList Date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        DTO.Report SetValue = new DTO.Report();
        SetValue.InvoiceNo = dtvBookingDetails.Rows[0].Cells[1].Text;
        SetValue.BranchId = Globals.BranchID;
        //string strPayAcceptDate = DateTime.Today.ToShortDateString();
        if (BAL.BALFactory.Instance.Bal_Report.GetStatusNegativeEntry(SetValue) == true)
        {
            //string strPayAcceptDate = DateTime.Today.ToShortDateString();
            try
            {
                float fltCashPreBalance = 0, fltCashPostBalance = 0, fltCustPreBalance = 0, fltCustPostBalance = 0, fltDiscountPreBalance = 0, fltDiscountPostBalanace = 0;
                float fltPaymentMadeNow = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtPaidAmount")).Text);
                string PaymentType = ((DropDownList)dtvBookingDetails.Rows[15].Cells[1].FindControl("drpPaymentType")).SelectedItem.Text;             
                string PaymentDetails = ((TextBox)dtvBookingDetails.Rows[14].Cells[1].FindControl("txtPaymentDetails")).Text;               
                float fltPrevPayment = float.Parse(dtvBookingDetails.Rows[8].Cells[1].Text);
                float fltDiscount = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtDiscountGiven")).Text);                
                float fltTenderAmt = float.Parse("0" + ((TextBox)dtvBookingDetails.FindControl("txtTenderAmt")).Text);
                //fltPaymentMadeNow = fltPaymentMadeNow - fltDiscount;
                float fltTotalPaymentMade = fltPrevPayment + fltPaymentMadeNow;
                float fltPrevDuePayment = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
                float fltDuePaymentNew = fltPrevDuePayment - fltPaymentMadeNow;
                string strBookingNumber = dtvBookingDetails.Rows[0].Cells[1].Text;
                //string strDeliveryDate = DateTime.Now.ToString() + " " + DateTime.Now.ToString("hh:mm:ss tt");

                DataSet dsBookingPrefix = new DataSet();
                string strNewBookingNo = string.Empty;
                string strNewBookingPrefix = string.Empty;
                dsBookingPrefix = BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(strBookingNumber);
                strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString();
                strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString();

                string sql = string.Empty;
                string strUpdateQty = string.Empty, strISN = string.Empty;

                sqlcon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                stx = sqlcon.BeginTransaction();
                cmd.Transaction = stx;
                ArrayList date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                //sql = "DELETE FROM ENTPAYMENT WHERE PAYMENTMADE=0 AND PaymentType IS NULL AND BOOKINGNUMBER='" + strBookingNumber + "' AND BranchId = '" + Globals.BranchID + "'";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();

                var pkgId = "0";
                if (hdnPackageExpired.Value.IndexOf(':') > -1)
                    pkgId = hdnPackageExpired.Value.Split(':')[1];
                if (PaymentType != "Package")
                    pkgId = "0";
                // string res2 = BAL.BALFactory.Instance.BAL_RemoveReason.SaveTempIntoPaymentTable(txtBookingNumber.Text, date[0].ToString(), date[1].ToString(), Globals.BranchID);
                sql = "INSERT INTO EntPayment (BookingNumber, PaymentDate, PaymentMade, DiscountOnPayment, PaymentType,PaymentRemarks,PaymentTime,BranchId,TenderAmount,AcceptByUser, packageAssignId,BookingPrefix) VALUES ('" + strNewBookingNo + "', '" + txtDeliveryDate.Text + "' , '" + fltPaymentMadeNow + "', '" + fltDiscount + "', '" + PaymentType + "', '" + PaymentDetails + "', '" + deliverTime + "', '" + Globals.BranchID + "', '" + fltTenderAmt + "', '" + Globals.UserName + "', " + pkgId + ",'" + strNewBookingPrefix + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "UPDATE EntBookings SET ClothDeliverDate='" + txtDeliveryDate.Text + "',Format='" + deliverTime + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                if (pkgId != "0")
                {
                    sql = "UPDATE EntBookings SET AssignId=" + pkgId + " WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                string custcode = ((Label)dtvBookingDetails.Rows[1].Cells[1].FindControl("lblCustomerCode")).Text;
                string custname = ((Label)dtvBookingDetails.Rows[1].Cells[1].FindControl("lblCustomerName")).Text;
                string strCustomerLedgerName = custcode;

                sql = "Select OpenningBalance From LedgerMaster Where LedgerName='Cash' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (sdr.Read())
                {
                    fltCashPreBalance = float.Parse("" + sdr.GetValue(0));
                }
                sdr.Close();

                sql = "Select CurrentBalance From LedgerMaster Where LedgerName='Discount' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (sdr.Read())
                {
                    fltDiscountPreBalance = float.Parse("" + sdr.GetValue(0));
                }
                sdr.Close();

                sql = "Select CurrentBalance From LedgerMaster Where LedgerName='" + strCustomerLedgerName + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (sdr.Read())
                {
                    fltCustPreBalance = float.Parse("" + sdr.GetValue(0));
                }
                sdr.Close();

                fltCashPostBalance = fltCashPreBalance + fltPaymentMadeNow;
                fltCustPostBalance = fltCustPreBalance - fltPaymentMadeNow;
                fltDiscountPostBalanace = fltDiscountPreBalance + fltDiscountPostBalanace;

                sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,Bookingnumber) Values('" + txtDeliveryDate.Text + "','Cash','By " + strCustomerLedgerName + "','" + fltCashPreBalance + "','" + fltPaymentMadeNow + "','0','" + fltCashPostBalance + "','Received payment against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','"+strNewBookingNo+"')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Insert Into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,Bookingnumber) Values('" + txtDeliveryDate.Text + "','" + strCustomerLedgerName + "','To Cash','" + fltCustPreBalance + "','0','" + fltPaymentMadeNow + "','" + fltCustPostBalance + "','Received payment against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                if (fltDiscount > 0)
                {
                    sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,Bookingnumber) Values('" + txtDeliveryDate.Text + "','Discount','By " + strCustomerLedgerName + "','" + fltDiscountPreBalance + "','" + fltDiscount + "', '0', '" + fltDiscountPostBalanace + "','Received Discount against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Insert Into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,AcceptByUser,PaymentTime,BookingPrefix,Bookingnumber) Values('" + txtDeliveryDate.Text + "','" + strCustomerLedgerName + "','To Discount','" + fltCustPreBalance + "', '0', '" + fltDiscount + "','" + fltCustPostBalance + "','Received Discount against booking number" + strBookingNumber + "','" + Globals.BranchID + "','" + Globals.UserName + "','" + deliverTime + "','" + strNewBookingPrefix + "','" + strNewBookingNo + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Update LedgerMaster Set CurrentBalance='" + fltDiscountPostBalanace.ToString() + "' Where LedgerName='Discount' AND BranchId='" + Globals.BranchID + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                sql = "Update LedgerMaster Set CurrentBalance=CurrentBalance-" + fltPaymentMadeNow.ToString() + " Where LedgerName='" + strCustomerLedgerName + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update LedgerMaster Set CurrentBalance=CurrentBalance+" + fltPaymentMadeNow.ToString() + " Where LedgerName='Cash' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                for (int r = 0; r < grdItemDetails.Rows.Count; r++)
                {
                    if (((CheckBox)grdItemDetails.Rows[r].FindControl("txtItemDelivered")).Checked)
                    {
                        strUpdateQty = "1";
                    }
                    else
                    {
                        strUpdateQty = "0";
                    }
                    strISN = ((HiddenField)grdItemDetails.Rows[r].FindControl("hdnItemSN")).Value;
                    cmd.CommandText = "UPDATE EntBookingDetails SET DeliveredQty='" + strUpdateQty + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND ISN = '" + strISN + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.ExecuteNonQuery();
                }
                stx.Commit();
                SaveInvoiceHistoryData(fltDiscount, fltPaymentMadeNow, PaymentType, PaymentDetails);   
                RefreshAll();
                //lblMsg.Text = "Payment Accepted for Receipt # " + strBookingNumber;
                //Response.Write(PrjClass.GetAndGo(lblMsg.Text, pgPageName));
            }
            catch (Exception excp)
            {
                if (stx != null) stx.Rollback();
                lblErr.Text = "Error : " + excp.ToString();
            }
            finally
            {
                sqlcon.Close();
                sdr.Close();
                sdr.Dispose();
            }
        }
    }

    private void UpdateRemarks(string BookingNumber, string remarks)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_Dry_EmployeeMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 9);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
        cmd.Parameters.AddWithValue("@Remarks", remarks);
        AppClass.ExecuteNonQuery(cmd);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Delivery.aspx", false);
    }

    private void RefreshAll()
    {
        dtvBookingDetails.Visible = false;
        grdItemDetails.Visible = false;
        grdPaymentDetails.DataSource = null;
        grdPaymentDetails.DataBind();
        txtBookingNumber.ReadOnly = false;
        btnShow.Enabled = true;
        btnRefresh.Visible = false;
    }

    private bool checkCheckbox()
    {
        int totalSelected = 0;
        for (int r = 0; r < grdItemDetails.Rows.Count; r++)
        {
            if (((CheckBox)grdItemDetails.Rows[r].Cells[0].FindControl("txtItemDelivered")).Checked) totalSelected++;
        }
        if (totalSelected > 0)
            return true;
        else
            return false;
    }

    private void checkTotalClotheUpdated()
    {
        string _active = string.Empty;
        try
        {
            string satus = string.Empty;
            for (int r = 0; r < grdItemDetails.Rows.Count; r++)
            {
                if (((Label)grdItemDetails.Rows[r].Cells[0].FindControl("lblStatus")).Text == "Ready")
                {
                    ((Button)grdItemDetails.FooterRow.FindControl("btnUpdateItemDelivery")).Visible = true;
                    _active = "True";
                    break;
                }
                else
                {
                    ((Button)grdItemDetails.FooterRow.FindControl("btnUpdateItemDelivery")).Visible = false;
                    _active = "False";
                }
            }
            if (_active == "True")
            {
                if (CheckForceMultipleDeliveryButtonRight() == true)
                {
                    ((Button)grdItemDetails.FooterRow.FindControl("btnUpdateItemDelivery")).Visible = true;
                }
                else
                {
                    ((Button)grdItemDetails.FooterRow.FindControl("btnUpdateItemDelivery")).Visible = false;
                }
            }
        }
        catch (Exception ex) { }
    }

    private bool CheckDeliveredItems()
    {
        bool ReturnVal = false;
        try
        {
            for (int r = 0; r < grdItemDetails.Rows.Count; r++)
            {
                if (((Label)grdItemDetails.Rows[r].Cells[6].FindControl("lblStatus")).Text == "Ready")
                {
                    ((CheckBox)grdItemDetails.Rows[r].Cells[4].FindControl("txtItemDelivered")).Enabled = true;
                    ((CheckBox)grdItemDetails.Rows[r].Cells[4].FindControl("txtItemDelivered")).Checked = true;
                }
                else
                {
                    if (((Label)grdItemDetails.Rows[r].Cells[6].FindControl("lblDate")).Text != "")
                    {
                        ((CheckBox)grdItemDetails.Rows[r].Cells[4].FindControl("txtItemDelivered")).Enabled = false;
                        ((CheckBox)grdItemDetails.Rows[r].Cells[4].FindControl("txtItemDelivered")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)grdItemDetails.Rows[r].Cells[4].FindControl("txtItemDelivered")).Enabled = false;
                        ((CheckBox)grdItemDetails.Rows[r].Cells[4].FindControl("txtItemDelivered")).Checked = false;
                    }
                }
            }
            ReturnVal = true;
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error in Delivered Item Quantity.";
            return false;
        }
        return ReturnVal;
    }

    private void CalculateItemDetails()
    {
        int totalItems = 0;
        for (int r = 0; r < grdItemDetails.Rows.Count; r++)
        {
            totalItems += int.Parse("0" + grdItemDetails.Rows[r].Cells[1].Text);
        }
        grdItemDetails.FooterRow.Cells[1].Text = totalItems.ToString();
    }

    protected void grdItemDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UPDATEITEMDELIVERY")
        {
            if (!checkCheckbox())
            {
                Session["ReturnMsg"] = "Please select atleast one checkbox.";
                return;
            }
            SetValue();
            string res = string.Empty;
            res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            UpdateItemDelivey(res);
            checkTotalClotheUpdated();
            BAL.BALFactory.Instance.Bal_Report.ChangeStatusAccordingBooking(txtBookingNumber.Text, Globals.BranchID);
            if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckAcceptPaymentButtonAcess(Globals.BranchID, Session["UserType"].ToString()) == true)
            {
                ((Button)dtvBookingDetails.Rows[10].FindControl("btnAcceptPayment")).Visible = true;
                ((Button)dtvBookingDetails.Rows[10].FindControl("Button1")).Visible = true;
            }
            else
            {
                ((Button)dtvBookingDetails.Rows[10].FindControl("btnAcceptPayment")).Visible = false;
                ((Button)dtvBookingDetails.Rows[10].FindControl("Button1")).Visible = false;
            }
            if (lblPkgValue.Text != "")
            {
                ((DropDownList)dtvBookingDetails.FindControl("drpPaymentType")).Items.Insert(0, "Package");
                ((DropDownList)dtvBookingDetails.FindControl("drpPaymentType")).SelectedIndex = 0;
            }
        }
    }

    private void SaveMsgStatus()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookingNumber", txtBookingNumber.Text);
        cmd.Parameters.AddWithValue("@MsgStaus", true);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 11);
        string res = AppClass.ExecuteNonQuery(cmd);
    }

    private void UpdateItemDelivey(string deliverTime)
    {
        string sql = string.Empty, sql1 = string.Empty, BookingId = string.Empty;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand();
        SqlTransaction stx = null;
        SqlDataReader sdr = null;
        string strUpdateQty = string.Empty, strBookingNumber = string.Empty, strISN = string.Empty, strRowIndex = string.Empty, DeliverStatus = string.Empty, strStatus = string.Empty;
        strBookingNumber = "" + dtvBookingDetails.Rows[0].Cells[1].Text;
        ArrayList date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        string strTotalGarment = string.Empty;

        DataSet dsBookingPrefix = new DataSet();
        string strNewBookingNo = string.Empty;
        string strNewBookingPrefix = string.Empty;
        dsBookingPrefix = BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(strBookingNumber);
        strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim();
        strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString();


        try
        {
            sqlcon.Open();
            stx = sqlcon.BeginTransaction();
            cmd.Connection = sqlcon;
            cmd.Transaction = stx;

            //sql = "EXEC sp_MultipleDeliveryStatus @AcceptedByUser='" + Globals.UserName + "',@BranchId='" + Globals.BranchID + "',@BookingNumber='" + strBookingNumber + "'";
            //cmd.CommandText = sql;
            //cmd.ExecuteNonQuery();

            if (txtReadBarcode.Text != "")
            {
                for (int r = 0; r < grdTmp.Rows.Count; r++)
                {
                    try
                    {
                        strStatus = ((Label)grdTmp.Rows[r].FindControl("lblStatus")).Text;
                    }
                    catch (Exception ex) { strStatus = ""; }
                    if (strStatus == "Ready")
                    {
                        //sql = string.Empty,sql1=string.Empty;
                        strISN = ((HiddenField)grdTmp.Rows[r].FindControl("hdnItemSN")).Value;
                        strRowIndex = grdItemDetails.Rows[r].Cells[4].Text;
                        cmd.CommandText = "UPDATE EntBookingDetails SET DeliveredQty='" + 1 + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" +strNewBookingPrefix+ "' AND ISN = '" + strISN + "' AND BranchId = '" + Globals.BranchID + "'";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE BarcodeTable SET DeliveredQty='" + "True" + "',DeliverItemStaus='" + "Delivered" + "',StatusId='" + Convert.ToInt32(GStatus.Delivered) + "',DelQty='" + "1" + "',ClothDeliveryDate='" + txtDeliveryDate.Text + "' WHERE BookingNo = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND RowIndex = '" + strISN + "' AND BranchId = '" + Globals.BranchID + "'";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE EntBookings SET ClothDeliverDate='" + txtDeliveryDate.Text + "',Format='" + deliverTime + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                        cmd.ExecuteNonQuery();

                        sql1 = "Select Id From BarCodeTable Where BookingNo='" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND RowIndex='" + strISN + "' AND StatusId='" + Convert.ToInt32(GStatus.Delivered) + "' AND BranchId = '" + Globals.BranchID + "'";
                        cmd.CommandText = sql1;
                        sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (sdr.Read())
                        {
                            BookingId = "" + sdr.GetValue(0);
                        }
                        sdr.Close();

                        BAL.BALFactory.Instance.BAL_ChallanIn.DeliveryEntStatus(Globals.BranchID, Globals.UserName, txtDeliveryDate.Text, BookingId, deliverTime, Convert.ToInt32(GStatus.Delivered));

                        //sql = "INSERT INTO EntStatus (BookingId, Datetime,Time,UserName,Status) VALUES ('" + BookingId + "', '" + txtDeliveryDate.Text + "' , '" + deliverTime + "', '" + Globals.UserName + "', '" + Convert.ToInt32(GStatus.Delivered) + "')";
                        //cmd.CommandText = sql;
                        //cmd.ExecuteNonQuery();
                        strTotalGarment = strTotalGarment + ((Label)grdTmp.Rows[r].FindControl("lblItemName")).Text + ":" + grdTmp.Rows[r].Cells[7].Text + ",";
                    }
                    else
                    {
                        lblErr.Text = "Cloth not available";
                        txtReadBarcode.Focus();
                        return;
                    }
                }
            }
            else
            {
                for (int r = 0; r < grdItemDetails.Rows.Count; r++)
                {
                    if (((CheckBox)grdItemDetails.Rows[r].FindControl("txtItemDelivered")).Checked)
                    {
                        strUpdateQty = "1";
                    }
                    else
                    {
                        strUpdateQty = "0";
                    }
                    DeliverStatus = ((Label)grdItemDetails.Rows[r].FindControl("lblStatus")).Text;
                    try
                    {
                        int.Parse(strUpdateQty);
                    }
                    catch
                    {
                        throw new Exception("Invalid Update quantity.");
                    }
                    strISN = ((HiddenField)grdItemDetails.Rows[r].FindControl("hdnItemSN")).Value;
                    cmd.CommandText = "UPDATE EntBookingDetails SET DeliveredQty='" + strUpdateQty + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND ISN = '" + strISN + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.ExecuteNonQuery();
                    if (strUpdateQty != "0")
                    {
                        if (DeliverStatus == "Ready")
                        {
                            cmd.CommandText = "UPDATE BarcodeTable SET DeliveredQty='" + "True" + "',DeliverItemStaus='" + "Delivered" + "',StatusId='" + "4" + "',DelQty='" + "1" + "',ClothDeliveryDate='" + txtDeliveryDate.Text + "' WHERE BookingNo = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND RowIndex = '" + strISN + "' AND BranchId = '" + Globals.BranchID + "'";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "UPDATE EntBookings SET ClothDeliverDate='" + txtDeliveryDate.Text + "',Format='" + deliverTime + "' WHERE BookingNumber = '" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND BranchId = '" + Globals.BranchID + "'";
                            cmd.ExecuteNonQuery();

                            sql1 = "Select Id From BarCodeTable Where BookingNo='" + strNewBookingNo + "' AND BookingPrefix='" + strNewBookingPrefix + "' AND RowIndex='" + strISN + "' AND StatusId='" + Convert.ToInt32(GStatus.Delivered) + "' AND BranchId = '" + Globals.BranchID + "'";
                            cmd.CommandText = sql1;
                            sdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            if (sdr.Read())
                            {
                                BookingId = "" + sdr.GetValue(0);
                            }
                            sdr.Close();

                            BAL.BALFactory.Instance.BAL_ChallanIn.DeliveryEntStatus(Globals.BranchID, Globals.UserName, txtDeliveryDate.Text, BookingId, deliverTime, Convert.ToInt32(GStatus.Delivered));

                            //sql = "INSERT INTO EntStatus (BookingId, Datetime,Time,UserName,Status) VALUES ('" + BookingId + "', '" + txtDeliveryDate.Text + "' , '" + deliverTime + "', '" + Globals.UserName + "', '" + Convert.ToInt32(GStatus.Delivered) + "')";
                            //cmd.CommandText = sql;
                            //cmd.ExecuteNonQuery();
                            strTotalGarment = strTotalGarment + ((Label)grdItemDetails.Rows[r].FindControl("lblItemName")).Text + ":" + grdItemDetails.Rows[r].Cells[2].Text + ",";

                        }
                    }
                }
            }
            stx.Commit();
            lblMsg.Text = "Item Updated.";            
            if (chkSendSms.Checked)
            {
                if (BAL.BALFactory.Instance.BAL_sms.CheckDeliverSMSStatus(Globals.BranchID, txtBookingNumber.Text) == true)
                {
                    var gIdTmp = Globals.BranchID;
                    Task t = Task.Factory.StartNew
                                    (
                                       () => { AppClass.GoMsg(gIdTmp,strNewBookingPrefix + strNewBookingNo, drpsmstemplate.SelectedValue); }
                                    );
                }
            }


            if (strTotalGarment.Length != 0)
            {
                strTotalGarment = strTotalGarment.Substring(0, strTotalGarment.Length - 1);
            }
            tmpBranchID = Globals.BranchID;
            tmpUserName = Globals.UserName;
            Task t3 = Task.Factory.StartNew
            (() => {
                if (hdnTmpDelDate.Value != txtDeliveryDate.Text.Trim())
                {
                    BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, "Delivery date modified from " + hdnTmpDelDate.Value + " to " + txtDeliveryDate.Text + ".", ScreenName.DeliveryScreen, 7);
                }
                BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, "Garments delivered on " + txtDeliveryDate.Text.Trim() + " - Details -  " + strTotalGarment + ".", ScreenName.DeliveryScreen,7);               
            }
            );  

        }
        catch (Exception excp)
        {
            if (stx != null) { stx.Rollback(); }
            lblErr.Text = "Error : " + excp.Message;
        }
        finally
        {
            stx = null;
            sdr.Close();
            sdr.Dispose();
            sqlcon.Close();
            ShowDetails("");
            CheckDeliveredItems();
        }
    }

    protected void lnkShowDiscount_OnClick(object sender, EventArgs e)
    {
        TextBox txtdisc = (TextBox)dtvBookingDetails.FindControl("txtDiscountGiven");
        txtdisc.Text = "0";
        if (txtdisc.Visible)
        {
            txtdisc.Visible = false;
            ((LinkButton)sender).Focus();
            ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtDiscountGiven")).Text = "";
            ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtDiscountGiven")).Focus();
        }
        else
        {
            txtdisc.Visible = true;
            txtdisc.Focus();
            ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtDiscountGiven")).Text = "";
            ((TextBox)dtvBookingDetails.Rows[10].FindControl("txtDiscountGiven")).Focus();
        }
    }

    protected void txtBookingNumber_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string[] Bookingno = txtBookingNumber.Text.Split('-');
            txtBookingNumber.Text = Bookingno[0].ToString();
            btnShow_Click(null, null);
        }
        catch (Exception ex)
        {
            btnShow_Click(null, null);
        }
    }

    protected void txtReadBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string[] Bookingno = txtReadBarcode.Text.Split('-');
            txtReadBarcode.Text = Bookingno[0].ToString();
            hdnRowIndex.Value = Bookingno[1].ToString();
            if (txtBookingNumber.Text == Bookingno[0].ToString())
            {
                BindUpdatedGrid();
                txtReadBarcode.Text = "";
                txtReadBarcode.Focus();
            }
            else
            {
                lblErr.Text = "Booking no not match.";
                txtReadBarcode.Text = "";
                txtReadBarcode.Focus();
            }
            if (grdTmp.Rows.Count == 0)
            {
                lblErr.Text = "Cloth not available";
                txtReadBarcode.Text = "";
                txtReadBarcode.Focus();
            }
        }
        catch (Exception ex)
        {
        }
        if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckAcceptPaymentButtonAcess(Globals.BranchID, Session["UserType"].ToString()) == true)
        {
            ((Button)dtvBookingDetails.Rows[10].FindControl("btnAcceptPayment")).Visible = true;
            ((Button)dtvBookingDetails.Rows[10].FindControl("Button1")).Visible = true;
        }
        else
        {
            ((Button)dtvBookingDetails.Rows[10].FindControl("btnAcceptPayment")).Visible = false;
            ((Button)dtvBookingDetails.Rows[10].FindControl("Button1")).Visible = false;
        }
        txtReadBarcode.Focus();
    }

    private void BindUpdatedGrid()
    {
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataAdapter sadp = new SqlDataAdapter();
        DataSet dsMain = new DataSet();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "Sp_Sel_BookingDetailsForDelivery";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
        cmd.Parameters.Add(new SqlParameter("@BookingNumber", txtReadBarcode.Text));
        cmd.Parameters.Add(new SqlParameter("@RowIndex", hdnRowIndex.Value));
        try
        {
            sqlcon.Open();
            cmd.Connection = sqlcon;
            sadp.SelectCommand = cmd;
            sadp.Fill(dsMain);
            grdTmp.DataSource = dsMain.Tables[3];
            grdTmp.DataBind();
            string res = string.Empty;
            res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            if (grdTmp.Rows.Count > 0)
            {
                UpdateItemDelivey(res);
                txtReadBarcode.Text = "";
                txtReadBarcode.Focus();
            }
        }
        catch (Exception ex) { }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SetValue();
        if (BAL.BALFactory.Instance.Bal_Report.BookingDateNotEarlierDeliveryDate(Ob) == true)
        {
            if (CheckCorrectEntries() == true)
            {
                float fltCashPaymentMade = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Text);
                float fltPaymentMade = fltCashPaymentMade;
                float fltPrevDuePayment = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
                if (fltCashPaymentMade <= fltPrevDuePayment)
                {
                    string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
                    AcceptPayment1(res);
                    BAL.BALFactory.Instance.Bal_Report.ChangeStatusAccordingBooking(txtBookingNumber.Text, Globals.BranchID);
                    trBarcode.Visible = false;
                    sendsms.Visible = false;
                    ArrayList date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                    DataSet dsBookingPrefix = new DataSet();
                    string strNewBookingNo = string.Empty;
                    string strNewBookingPrefix = string.Empty;
                    dsBookingPrefix = BAL.BALFactory.Instance.BAL_ChallanIn.GetBookingNoAndPrefix(txtBookingNumber.Text);
                    strNewBookingPrefix = dsBookingPrefix.Tables[0].Rows[0]["BookingPrefix"].ToString().Trim().ToUpper();
                    strNewBookingNo = dsBookingPrefix.Tables[0].Rows[0]["Bookingnumber"].ToString();
                    if (chkSendSms.Checked)
                    {
                        if (BAL.BALFactory.Instance.BAL_sms.CheckDeliverSMSStatus(Globals.BranchID, txtBookingNumber.Text) == true)
                        {
                            var gIdTmp = Globals.BranchID;
                            Task t = Task.Factory.StartNew
                                            (
                                               () => { AppClass.GoMsg(gIdTmp,strNewBookingPrefix + strNewBookingNo, drpsmstemplate.SelectedValue); }
                                            );
                        }
                    }
                    strPrinterName = PrjClass.GetDeliveryPrinterName(Globals.BranchID);
                    OpenNewWindow("../" + strPrinterName + "/DeliverySlip.aspx?BN=" + txtBookingNumber.Text + "-1" + date[0].ToString() + "&RS=" + float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtPaidAmount")).Text) + "" + "&DirectPrint=true&CloseWindow=true");                           
                    txtBookingNumber.Text = "";
                    txtBookingNumber.Focus();
                }
                else
                {
                    Session["ReturnMsg"] = "Payment cannot exceed the due amount.";
                    ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Focus();
                    ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Attributes.Add("onfocus", "javascript:select();");
                    return;
                }
            }
            else
            {
                Session["ReturnMsg"] = "Payment cannot exceed the due amount.";
                ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Focus();
                ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Attributes.Add("onfocus", "javascript:select();");
                return;
            }
        }
        else
        {
            Session["ReturnMsg"] = "Delivery date cannot exceed the booking date.";
            return;
        }
    }

    public void OpenNewWindow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        DTO.Report Ob = new DTO.Report();
        if (Request.QueryString["CustCode"] != null)
        {
            Ob.CustId = Request.QueryString["CustCode"].ToString();
        }
        else
        {
            GrdPendingClothesAndPayment.DataSource = null;
            GrdPendingClothesAndPayment.DataBind();
            string[] CustName = txtCustomerName.Text.Split('-');
            hdnCustId.Value = CustName[0].ToString();
            setCustvalue(CustName[0].ToString());
            Ob.CustId = hdnCustId.Value.Trim();
        }
        GrdPendingClothesAndPayment.DataSource = BAL.BALFactory.Instance.Bal_Report.GetPendingReceiptParticularCustomer(Ob, Globals.BranchID);
        GrdPendingClothesAndPayment.DataBind();
        if (GrdPendingClothesAndPayment.Rows.Count > 0)
        {
            CalculateGridReport();
            Panel1.Visible = true;
            txtCustomerName.Focus();
        }
        else
        {
            Session["ReturnMsg"] = "No record found.";
            txtCustomerName.Focus();
        }
    }

    public void setCustvalue(string customerName)
    {
        DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
        if (DS_CustInfo.Tables[0].Rows.Count > 0)
            txtCustomerName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
    }

    protected void btnCheckGridBox_Click(object sender, EventArgs e)
    {
        GrdPendingClothesAndPayment.DataSource = null;
        GrdPendingClothesAndPayment.DataBind();
        string[] CustName = txtCustomerName.Text.Split('-');
        hdnCustId.Value = CustName[0].ToString();
        setCustvalue(CustName[0].ToString());
        DTO.Report Ob = new DTO.Report();
        Ob.CustId = hdnCustId.Value.Trim();
        GrdPendingClothesAndPayment.DataSource = BAL.BALFactory.Instance.Bal_Report.GetPendingReceiptParticularCustomer(Ob, Globals.BranchID);
        GrdPendingClothesAndPayment.DataBind();
        if (GrdPendingClothesAndPayment.Rows.Count > 0)
        {
            CalculateGridReport();
            Panel1.Visible = true;
            txtCustomerName.Focus();
        }
        else
        {
            Session["ReturnMsg"] = "No record found.";
            txtCustomerName.Focus();
        }
    }

    protected void hypBtnShowDetails_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string BookingNumber = lnk.Text;
        txtBookingNumber.Text = "";
        txtBookingNumber.Text = BookingNumber;
        btnShow_Click(null, null);
        //ShowDetails(BookingNumber);
    }

    private void CalculateGridReport()
    {
        try
        {
            int rc = GrdPendingClothesAndPayment.Rows.Count;
            int cc = GrdPendingClothesAndPayment.Columns.Count;
            float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0;
            for (int r = 0; r < rc; r++)
            {
                OrderCount++;
                TotalCostCount += float.Parse("0" + GrdPendingClothesAndPayment.Rows[r].Cells[3].Text);
                TotalPaid += float.Parse("0" + GrdPendingClothesAndPayment.Rows[r].Cells[4].Text);
            }
            GrdPendingClothesAndPayment.FooterRow.Cells[2].Text = "Total";
            GrdPendingClothesAndPayment.FooterRow.Cells[3].Text = TotalCostCount.ToString();
            GrdPendingClothesAndPayment.FooterRow.Cells[4].Text = TotalPaid.ToString();
        }
        catch (Exception ex) { }
    }

    protected void txtDiscountGiven_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        double d;
        if (!double.TryParse(txt.Text, out d))
        {
            lblErr.Text=("Please enter a valid number");
            ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtDiscountGiven")).Focus();
            ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtDiscountGiven")).Attributes.Add("onfocus", "javascript:select();");
            return;
        }       
        float fltPaymentMadeNow = float.Parse(dtvBookingDetails.Rows[9].Cells[1].Text);
        float fltDiscount = float.Parse("0" + ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtDiscountGiven")).Text);
        fltPaymentMadeNow = fltPaymentMadeNow - fltDiscount;
        ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtPaidAmount")).Text = fltPaymentMadeNow.ToString();
        ((TextBox)dtvBookingDetails.Rows[10].Cells[1].FindControl("txtPaidAmount")).Focus();
        ((TextBox)dtvBookingDetails.Rows[9].Cells[1].FindControl("txtPaidAmount")).Attributes.Add("onfocus", "javascript:select();");
    }
    

    protected void grdItemDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    string strPrinterName = string.Empty;
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ArrayList date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.SaveTempIntoPaymentTable(txtBookingNumber.Text, txtDeliveryDate.Text, date[1].ToString(), Globals.BranchID);
        strPrinterName = PrjClass.GetDeliveryPrinterName(Globals.BranchID);
        OpenNewWindow("../" + strPrinterName + "/DeliverySlip.aspx?BN=" + txtBookingNumber.Text + "-1" + date[0].ToString() + "&RS=" + "0" + "&DirectPrint=true&CloseWindow=true");

        tmpBranchID = Globals.BranchID;
        tmpUserName = Globals.UserName;
        Task t = Task.Factory.StartNew
        (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, DeliverySrcMsg.printWitAmtMsg, ScreenName.DeliveryScreen,7); }
        );
    }

    protected void btnPrintWithoutAmt_Click(object sender, EventArgs e)
    {
        ArrayList date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.SaveTempIntoPaymentTable(txtBookingNumber.Text, txtDeliveryDate.Text, date[1].ToString(), Globals.BranchID);
        strPrinterName = PrjClass.GetDeliveryPrinterName(Globals.BranchID);
        OpenNewWindow("../" + strPrinterName + "/DeliverySlipWithoutAmt.aspx?BN=" + txtBookingNumber.Text + "-1" + date[0].ToString() + "&RS=" + "0" + "&DirectPrint=true&CloseWindow=true");

        tmpBranchID = Globals.BranchID;
        tmpUserName = Globals.UserName;
        Task t = Task.Factory.StartNew
        (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, DeliverySrcMsg.printWithoutAmtMsg, ScreenName.DeliveryScreen,7); }
        );
    }  

    protected void btnSaveRemarks_Click(object sender, EventArgs e)
    {
        SqlCommand cmd2 = new SqlCommand();
        DataSet ds = new DataSet();
        string previousremarks = string.Empty;
        string Remarks = string.Empty;
        cmd2.CommandText = "sp_Dry_EmployeeMaster";
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd2.Parameters.AddWithValue("@BookingNumber", txtBookingNumber.Text);
        cmd2.Parameters.AddWithValue("@Flag", 8);
        ds = AppClass.GetData(cmd2);
        if (ds.Tables[0].Rows.Count > 0)
        {
            previousremarks = ds.Tables[0].Rows[0]["Notes"].ToString();
            previousremarks = previousremarks + " , ";
        }
        SqlCommand cmd = new SqlCommand();
        string res = string.Empty;
        cmd.CommandText = "sp_Dry_EmployeeMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 9);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@BookingNumber", txtBookingNumber.Text);

        cmd.Parameters.AddWithValue("@Remarks", previousremarks + txtDeliveryRemarks.Text);
        
        cmd.Parameters.AddWithValue("@SatisfiedCustomer", chkSatisfiedCustomer.Checked ? "1" : "0");
        cmd.Parameters.AddWithValue("@DeliveryWithoutSlip", chkWithoutSlip.Checked ? "1" : "0");
        res = AppClass.ExecuteNonQuery(cmd);
        lblMsgStatus.Text = "Info Saved.";

        tmpBranchID = Globals.BranchID;
        tmpUserName = Globals.UserName;
        SaveHistoryForDeliverySlip(tmpBranchID, tmpUserName);
  
        txtDeliveryRemarks.Text = ""; 
        GetRemarksOnDelivery(txtBookingNumber.Text);

    }

    private void SaveInvoiceHistoryData(float tmpDiscount, float fltPaymentMade, string strPaymentType, string strPaymentDetails)
    {
        tmpBranchID = Globals.BranchID;
        tmpUserName = Globals.UserName;
        hdnTmpBookingNo.Value = txtBookingNumber.Text.ToUpper().Trim();
        var StrMsg1 = string.Empty;
        if (strPaymentDetails.Trim() == "")
        {
            StrMsg1 = "Payment type : " + strPaymentType;
        }
        else
        {
            StrMsg1 = "Payment type : " + strPaymentType + " and Payment details : " + strPaymentDetails;
        }
        if (tmpDiscount > 0)
        {
            Task t2 = Task.Factory.StartNew
            (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(hdnTmpBookingNo.Value, tmpUserName, tmpBranchID, "Discount offered/amount adjusted Amount : " + tmpDiscount, ScreenName.DeliveryScreen, 7); }
            );
        }
        Task t1 = Task.Factory.StartNew
        (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(hdnTmpBookingNo.Value, tmpUserName, tmpBranchID, "Payment accepted : Amount " + fltPaymentMade + " , Payment date :" + txtDeliveryDate.Text.Trim() + " and Print invoice  and " + StrMsg1 + ".", ScreenName.DeliveryScreen,7); }
        );  
    }


    private void SaveHistoryData(float tmpDiscount, float fltPaymentMade, string strPaymentType, string strPaymentDetails)
    {
        tmpBranchID = Globals.BranchID;
        tmpUserName = Globals.UserName;
        var StrMsg = string.Empty;
        if (strPaymentDetails.Trim() == "")
        {
            StrMsg = "Payment type : " + strPaymentType;
        }
        else
        {
            StrMsg = "Payment type : " + strPaymentType + " and Payment details : " + strPaymentDetails;
        }
        if (tmpDiscount > 0)
        {
            Task t2 = Task.Factory.StartNew
            (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, "Discount offered/amount adjusted Amount : " + tmpDiscount, ScreenName.DeliveryScreen, 7); }
            );
        }
        Task t1 = Task.Factory.StartNew
        (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, "Payment accepted : Amount :" + fltPaymentMade + ", Payment date :" + txtDeliveryDate.Text.Trim() + " and " + StrMsg + ".", ScreenName.DeliveryScreen,7); }
        );  
    }

    private void SaveHistoryForDeliverySlip(string BID, string UID)
    {
        string strRemark = string.Empty;
        var TmpDelSlip = chkWithoutSlip.Checked ? "Yes" : "No";
        var TmpDelSlipPre = hdnTmpDelWithoupSlip.Value=="True" ? "Yes" : "No";
        var TmpSlipMsg = chkWithoutSlip.Checked ? "Delivery status changed : Order delivered/picked up without receipt." : "Delivery status changed : Order delivered/picked up with receipt.";

        var TmpStsCust = chkSatisfiedCustomer.Checked ? "Yes" : "No";
        var TmpStsCustPre = hdntmpSatisfiedCust.Value == "True" ? "Yes" : "No";
        if (TmpDelSlipPre != TmpDelSlip)
        {
            Task t = Task.Factory.StartNew
            (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper(), UID, BID, TmpSlipMsg, ScreenName.DeliveryScreen, 7); }
            );
        }
        if (TmpStsCustPre != TmpStsCust)
        {
            Task t1 = Task.Factory.StartNew
            (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper(), UID, BID, "Customer satisfaction index changed from " + TmpStsCustPre + " to " + TmpStsCust + ".", ScreenName.DeliveryScreen, 7); }
            );
        }
        strRemark = txtDeliveryRemarks.Text.Trim();
        if (txtDeliveryRemarks.Text.Trim() != "")
        {
        Task t2 = Task.Factory.StartNew
            (() => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper(), UID, BID, "Deliver order screen: Notes added - " + strRemark + " .", ScreenName.DeliveryScreen, 7); }
            );
        }
    
    }

    private void InsertHistoryData()
    {
        string strScreenName = string.Empty;
        string strScreenMsg = string.Empty;
        int ScreenId = 7;
        strScreenMsg = DeliverySrcMsg.InvoiceSearch;

        if (Request.QueryString["BN"] != null)
        {
            var url = Request.UrlReferrer.LocalPath;
            if (url.Contains("Multiplepayment.aspx"))
            {
                strScreenName = ScreenName.MultipleDel;
                strScreenMsg = "Invoice opened for delivery from multiple delivery and payment.";
                ScreenId = 11;
            }
            else if (url.Contains("PendingStockReport.aspx"))
            {
                strScreenName = ScreenName.PendigStock;
                strScreenMsg = "Order accessed through Pending Stock report.";
                ScreenId = 10;
            }
            else if (url.Contains("Default.aspx"))
            {
                strScreenName = ScreenName.DashBoard;
                strScreenMsg = "Order accessed through Dashboard.";
                ScreenId = 9;
            }
            else if (url.Contains("home.html"))
            {
                strScreenName = ScreenName.HomeSceen;
                strScreenMsg = "Order accessed through Home screen.";
                ScreenId = 16;
            }
            else
            {
                strScreenName = ScreenName.DeliveryScreen;            
            }
        }
        else
        {
            strScreenName = ScreenName.DeliveryScreen;
        }
        tmpBranchID = Globals.BranchID;
        tmpUserName = Globals.UserName;
        Task t = Task.Factory.StartNew
        (
        () => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(txtBookingNumber.Text.ToUpper().Trim(), tmpUserName, tmpBranchID, strScreenMsg, strScreenName, ScreenId); }
        );
    }
}