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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

public partial class Thermal_BookingSlip : System.Web.UI.Page
{
    protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
    public string strPreview = string.Empty;
    public string strPreview1 = string.Empty;
    string _pkgType = string.Empty;
    DataSet dschk = new DataSet();
    string tmpBranchID = string.Empty, tmpUserName = string.Empty;  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["BN"] == null || Request.QueryString["BN"] == "")
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
            else
            {
                string BookNum = Request.QueryString["BN"].ToString();
                string[] no = BookNum.Split('-');
                hdnEmailId.Value = no[0].ToString();
                dschk = BAL.BALFactory.Instance.Bal_Report.GetSlipDetails(no[0].ToString(), Globals.BranchID, "Sp_Sel_BookingDetailsForReceipt");
                _pkgType = dschk.Tables[0].Rows[0]["PackageType"].ToString();
                if (_pkgType == "Flat Qty" || _pkgType == "Qty / Item" || _pkgType == "Value / Benefit")
                {
                    GetBookingDetailsPackage(no[0]);
                }
                else
                {
                    GetBookingDetails(no[0]);
                }

                if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                    pnlButtons.Style["visibility"] = "visible";
                else if (HttpContext.Current.Items["IsPrintingForMany"].ToString() == "false")
                    pnlButtons.Style["visibility"] = "visible";
                else
                    pnlButtons.Style["visibility"] = "hidden";

                hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
                //}
                if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                {
                    hdnTheCookie.Value = "true";
                }

                if (Request.QueryString["closeWindow"] != null && Request.QueryString["closeWindow"].ToString() == "false")
                {
                    hdncloseWindow.Value = "false";
                }
                else if (Request.QueryString["closeWindow"] != null && Request.QueryString["closeWindow"].ToString() == "true")
                {
                    hdncloseWindow.Value = "true";
                }

                if (Request.QueryString["PrintBarCode"] != null && Request.QueryString["PrintBarCode"].ToString() == "true")
                {
                    hdnPrintBarcode.Value = "true";
                    hdnBookingNumber.Value = no[0];
                }
                if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() == "true")
                {
                    hdnRedirectBack.Value = "true";
                }
                if (Request.QueryString["Email"] != null && Request.QueryString["Email"].ToString() == "true")
                {
                    sendEmail();
                }

                var url = Request.UrlReferrer.LocalPath;
                if ((url.Length > 0) && (!url.Contains("New_Booking/frm_New_Booking.aspx")))
                {
                    var strActionMsg = PrjClass.GetReasonAccordingToScreenName(url);
                    if (strActionMsg == "Booking by Customer")
                    {
                        var absoulteuri = Request.UrlReferrer.AbsoluteUri;
                        if (absoulteuri.Contains("AccountsReceivable"))
                        {
                            strActionMsg = "Accounts Receivable";
                        }
                        else
                        {
                            strActionMsg = "Daily Customer Addition ";
                        }
                    }
                    tmpBranchID = Globals.BranchID;
                    tmpUserName = Globals.UserName;
                    Task t = Task.Factory.StartNew
                    (
                    () => { BAL.BALFactory.Instance.BAL_New_Bookings.InvoiceEventHistorySaveData(no[0].ToUpper().Trim(), tmpUserName, tmpBranchID, "Order open from  " + strActionMsg, ScreenName.ScrReportName,14); }
                    );
                }
            }
        }
        btnf2.Click += (sender2, args) => Response.Redirect("~/New_Booking/frm_New_Booking.aspx?option=Edit&BkNo=" + Request.QueryString["BN"].Split('-')[0].ToString());
    }

    protected void btnGoHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Home.html", false);
    }
    protected void btnGoForNewOrder_Click(object sender, EventArgs e)
    {
        Response.Redirect("../New_Booking/frm_New_Booking.aspx");
    }

    private void sendEmail()
    {
        bool SSL = false;
        SqlCommand cmd = new SqlCommand();
        string eMail = string.Empty;
        DataSet ds = new DataSet();
        SqlDataReader sdr = null;
        try
        {
            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNo", hdnEmailId.Value);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 13);

            sdr = AppClass.ExecuteReader(cmd);
            if (sdr.Read())
            {
                eMail = "" + sdr.GetValue(0);
            }
            if (eMail != "")
            {
                //byte[] renderedBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Booking Slip.pdf"));
                SqlCommand cmd1 = new SqlCommand();
                DataSet ds1 = new DataSet();
                cmd1.CommandText = "sp_ReceiptConfigSetting";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd1.Parameters.AddWithValue("@Flag", 2);
                ds1 = AppClass.GetData(cmd1);

                string FEmail = eMail;
                string mailBody = ViewState["Msg"].ToString();
                SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                int port = Convert.ToInt32(ds1.Tables[0].Rows[0]["Port"].ToString());
                Task t = Task.Factory.StartNew
                          (
                             () => { AppClass.SendMail(FEmail, "Booking slip of your clothes", mailBody, true, port, ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL); }
                          );
            }
            else
                lblMsg.Text = "Sorry ! Email not found of this customer..";
        }
        catch (Exception ex)
        {

        }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
    }

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        string BookNum = Request.QueryString["BN"].ToString();
        string[] no = BookNum.Split('-');
        hdnEmailId.Value = no[0].ToString();
        dschk = BAL.BALFactory.Instance.Bal_Report.GetSlipDetails(no[0].ToString(), Globals.BranchID, "Sp_Sel_BookingDetailsForReceipt");
        _pkgType = dschk.Tables[0].Rows[0]["PackageType"].ToString();
        if (_pkgType == "Flat Qty" || _pkgType == "Qty / Item")
        {
            GetBookingDetailsPackage(no[0]);
        }
        else
        {
            GetBookingDetails(no[0]);
        }
        if (Request.QueryString["Email"] != null && Request.QueryString["Email"].ToString() == "true")
        {
            sendEmail();
        }
    }
    public Tuple<string, string> GetBookingDetails(string BookingNo, bool isComparing = false)
    {
        // Create variables ///
        string custPhone = string.Empty,printBookingTime=string.Empty, displayPhone = string.Empty, leftMessage = string.Empty, rightMessage = string.Empty;
        bool logo = false,BookingTime=true, termAndCondition = false, St = false, Rounded = false, StoreCopy = true, barcode = false, PrintDueDate = false, ShowHeaderSlogan = false, PrintTermConditionOnStoreCopy = false, PrintPhoneNo = false, PrintCustomerSignature = false, PrinttaxDetail = false;
        double wholeAmt = 0, totalAmt = 0;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);

        DataSet dsMain = new DataSet();
        cmd = new SqlCommand();


        if (isComparing)
        {
            cmd.CommandText = "Sp_Sel_BookingDetailsForReceiptBackUp";
            cmd.Parameters.Add(new SqlParameter("@BookingBackUpId", BookingNo));
        }
        else
        {
            cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
            cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookingNo));            
        }

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
        dsMain = AppClass.GetData(cmd);

        ///// Set Variables ////
        if (ds.Tables[0].Rows.Count > 0)
        {
            termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
            St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
            logo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
            StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
            barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
            PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
            ShowHeaderSlogan = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
            PrintTermConditionOnStoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTermConditionOnStoreCopy"].ToString());
            PrintPhoneNo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintPhoneNo"].ToString());
            PrintCustomerSignature = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintCustomerSignature"].ToString());
            PrinttaxDetail = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTaxDetail"].ToString());
            BookingTime = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBookingTime"].ToString());
            if (BookingTime)
            {
                printBookingTime = dsMain.Tables[0].Rows[0]["BookingTime"].ToString();
            }
            else
            {
                printBookingTime = "";
            }
        }
        if (ds.Tables[0].Rows[0]["LeftMessage"].ToString() == "")
        {
            leftMessage = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        else
        {
            leftMessage = ds.Tables[0].Rows[0]["LeftMessage"].ToString();
        }
        if (ds.Tables[0].Rows[0]["RightMessage"].ToString() == "")
        {
            rightMessage = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        else
        {
            rightMessage = ds.Tables[0].Rows[0]["RightMessage"].ToString();
        }
        // Makeing Table ////
        strPreview += "<table style='width:3in; page-break-after: always;'>";
        strPreview += "<tr><td>";
        strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        if (StoreCopy)
        {
            strPreview1 += "<table style='width:3in; page-break-after: always;'>";
            strPreview1 += "<tr><td>";
            strPreview1 += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        }
        // Check Logo Print On and Off //
        custPhone = dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString();
        if (!PrintPhoneNo)
            custPhone = "";
        if (custPhone != "")
        {
            displayPhone = "(" + custPhone + ")";
        }
        else
        {
            displayPhone = "";
        }
        if (logo)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='40px' height='40px' /></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='40px' height='40px' /></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
        }
        else
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' width='40Px'></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' width='40Px'></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td align='center' colspan='5'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr>";
        strPreview += "<tr><td align='center' colspan='5'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' style='font-size: 15px; font-family: Times New Roman; ' colspan='5' align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr>";
        strPreview += "<tr><td width='3in' style='font-size: 15px; font-family: Times New Roman; ' colspan='5' align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr>";

        if (ds.Tables[1].Rows[0]["MainTagline"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td colspan='5' style='font-family: Arial; font-size: 11Px;' align='center'>" + ds.Tables[1].Rows[0]["MainTagline"].ToString() + "</td></tr>";
            strPreview += "<tr><td colspan='5' style='font-family: Arial; font-size: 11Px;' align='center'>" + ds.Tables[1].Rows[0]["MainTagline"].ToString() + "</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        strPreview += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        if (barcode)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "&nbsp;&nbsp;<span style='font-family: c39HrP24DhTt; font-size: 32px'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</span> " + "</td></tr>";
            strPreview += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "&nbsp;&nbsp;<span style='font-family: c39HrP24DhTt; font-size: 32px'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</span> " + "</td></tr>";
        }
        else
        {
            if (StoreCopy)
                strPreview1 += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
            strPreview += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
        }
        //strPreview += "</table>";
        //strPreview += "<table width='3in'>";
        if (StoreCopy)
            strPreview1 += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
        strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td align='left' style='font-size: 12px; font-family: Arial; ' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
        strPreview += "<tr><td align='left' style='font-size: 12px; font-family: Arial; ' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5' >" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + " " + printBookingTime + "</td></tr>";
        strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5' >" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + " " + printBookingTime + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        strPreview += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        if (StoreCopy)
            strPreview1 += "</table>";
        strPreview += "</table>";
        // Item Section
        if (StoreCopy)
            strPreview1 += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Qty&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Service&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td></tr>";
        strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Qty&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Service&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        strPreview += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
        {
            string ItemName = "", Color = "", Remarks = "", Process = "", ExProcess1 = "", ExProcess2 = "", Rate = string.Empty, Rate1 = string.Empty, Rate2 = string.Empty;
            string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
            string[] rate1 = rate.Split(';');
            string[] rate2 = rate.Split('@');
            Process = dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString();
            ExProcess1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString();
            ExProcess2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString();
            Rate = string.Format("{0:0.00}", rate2[1].ToString());
            Rate1 =string.Format("{0:0.00}",   dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
            Rate2 =string.Format("{0:0.00}",  dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString());
            Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
            Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
            if (Color != "")
            { Color = "- " + Color; }
            else
            { Color = ""; }
            if (Remarks != "")
            { Remarks = "- " + Remarks; }
            else
            { Remarks = ""; }
            for (int k = 0; k < rate1.Length; k++)
            {
                ItemName += " " + rate1[k];
            }

            if (ExProcess1 != "None")
            {
                ExProcess1 = ExProcess1 + "@" + Rate1;
            }
            if (ExProcess2 != "None")
            {
                ExProcess2 = ExProcess2 + "@" + Rate2;
            }
            //// Makeing Dynamic Item list
            if (StoreCopy)
                strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" +string.Format("{0:0.00}",  (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
            strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" +string.Format("{0:0.00}",  (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + "</td></tr>";
            }
            wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
            totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
        }
        double serviceTax = 0, ServiceRate = 0, amt1 = 0, serviceTot = 0;
        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
        {
            serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
            serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
            serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
        }
        for (int i = 0; i < dsMain.Tables[7].Rows.Count; i++)
        {
            serviceTot += Convert.ToDouble(dsMain.Tables[7].Rows[i]["MainTax"].ToString());
            serviceTot += Convert.ToDouble(dsMain.Tables[7].Rows[i]["Cess"].ToString());
            serviceTot += Convert.ToDouble(dsMain.Tables[7].Rows[i]["HCess"].ToString());
        }
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        strPreview += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["TotalQty"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>G Amt.</td><td width='1in' style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";
        strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["TotalQty"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>G Amt.</td><td width='1in' style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";
        if (dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() != "0")
        {
            if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2)) + "</td></tr>";
                strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2)) + "</td></tr>";
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Dis</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2)) + "</td></tr>";
                strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Dis</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2)) + "</td></tr>";
            }
        }
        if (St)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Tax</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2).ToString()) + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Tax</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2).ToString()) + "</td></tr>";
        }
        Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
        if (Rounded)
        {
            ServiceRate = Math.Round(serviceTax, 2);
            amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
        }
        else
        {
            ServiceRate = serviceTax;
            amt1 = Math.Ceiling(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()));
        }
        if (StoreCopy)
            strPreview1 += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString())) + "</td></tr>";
        strPreview += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString())) + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000;font-weight: Bold;'>" + string.Format("{0:0.00}", amt1.ToString()) + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000;font-weight: Bold;'>" + string.Format("{0:0.00}", amt1.ToString()) + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
        strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
        if (PrinttaxDetail)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTot, 2)) + " )</span></td></tr>";
            strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTot, 2)) + " )</span></td></tr>";
        }
        if (PrintDueDate)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 13px; font-family: Times New Roman; ' align='center' colspan='4'>" + "<span style='font-weight: bold; color: #FF0000;'>Ready On: &nbsp;" + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + "</span>" + "<span style='color: #FF0000'>" + " - " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</span>" + "</td></tr>";
            strPreview += "<tr><td style='font-size: 13px; font-family: Times New Roman; ' align='center' colspan='4'>" + "<span style='font-weight: bold; color: #FF0000;'>Ready On: &nbsp;" + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + "</span>" + "<span style='color: #FF0000'>" + " - " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</span>" + "</td></tr>";
        }
        if (ds.Tables[1].Rows[0]["Holiday"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["Holiday"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["CLOSED"].ToString() + "</span>" + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["Holiday"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["CLOSED"].ToString() + "</span>" + "</td></tr>";
        }
        if (ds.Tables[1].Rows[0]["Timing"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["ST"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["Timing"].ToString() + "</span>" + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["ST"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["Timing"].ToString() + "</span>" + "</td></tr>";
        }
        if (dsMain.Tables[0].Rows[0]["PackageName"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Package :</span>" + " " + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Package :</span>" + " " + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Booked By :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</td></tr>";
        strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Booked By :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</td></tr>";
        if (dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Order Notes :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Order Notes :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr>";
        }
        if (ShowHeaderSlogan)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td align='center' colspan='4' >----------</td></tr>";
            strPreview += "<tr><td align='center' colspan='4' >----------</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td align='center' colspan='4' >----------</td></tr>";
            strPreview += "<tr><td align='center' colspan='4' >----------</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='3' height='10Px'></td></tr>";
        strPreview += "<tr><td width='3in' colspan='3' height='10Px'></td></tr>";
        if (termAndCondition)
        {
            if (!StoreCopy)
            {
                PrintTermConditionOnStoreCopy = false;
            }
            if (PrintTermConditionOnStoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' colspan='4'>Terms and Conditions</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' colspan='4'>Terms and Conditions</td></tr>";
            if (PrintTermConditionOnStoreCopy)
                strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
            strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
            if (dsMain.Tables[0].Rows[0]["TC1"].ToString() != "")
            {
                if (PrintTermConditionOnStoreCopy)
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";
                if (PrintTermConditionOnStoreCopy)
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
            }
            if (dsMain.Tables[0].Rows[0]["TC2"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (dsMain.Tables[0].Rows[0]["TC3"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term4"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term4"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term4"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term5"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term5"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term5"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term6"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term6"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term6"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term7"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term7"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term7"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term8"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term8"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term8"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term9"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term9"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term9"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term10"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term10"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term10"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term11"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term11"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term11"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term12"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term12"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term12"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term13"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term13"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term13"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term14"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term14"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term14"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term15"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term15"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term15"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
        }
        if (PrintCustomerSignature)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
            strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td  colspan='4' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='center'>Signature</td></tr>";
            strPreview += "<tr><td  colspan='4' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='center'>Signature</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td width='3in' colspan='4' height='25Px'></td></tr>";
            strPreview += "<tr><td width='3in' colspan='4' height='25Px'></td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='left'>Customer</td><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='right'>Salesman</td></tr>";
            strPreview += "<tr><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='left'>Customer</td><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='right'>Salesman</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "</table>";
        strPreview += "</table>";
        strPreview += "</td></tr>";
        strPreview += "</table>";
        if (StoreCopy)
        {
            strPreview1 += "<table style='width:3in;'>";
            strPreview1 += "<tr><td width='3in' colspan='4' align='left'><span style='font-family:Arial;font-size:36px; font-weight:Bold; text-decoration:;font-style:'>Store Copy</span></td></tr>";
            strPreview1 += "</table>";
        }
        strPreview1 += "</td></tr>";
        strPreview1 += "</table>";
        ViewState["Msg"] = strPreview;
        return Tuple.Create<string, string>(strPreview, strPreview1);
    }
    public Tuple<string, string> GetBookingDetailsPackage(string BookingNo, bool isComparing = false)
    {
        // Create variables ///
        string custPhone = string.Empty,printBookingTime=string.Empty, displayPhone = string.Empty, leftMessage = string.Empty, rightMessage = string.Empty;
        bool logo = false,BookingTime=true, termAndCondition = false, St = false, Rounded = false, StoreCopy = true, barcode = false, PrintDueDate = false, ShowHeaderSlogan = false, PrintTermConditionOnStoreCopy = false, PrintPhoneNo = false, PrintCustomerSignature = false;
        double wholeAmt = 0, totalAmt = 0;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);

        DataSet dsMain = new DataSet();
        cmd = new SqlCommand();


        if (isComparing)
        {
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetSlipDetails(BookingNo, Globals.BranchID, "Sp_Sel_BookingDetailsForReceiptBackUp");
        }
        else
        {
            dsMain = BAL.BALFactory.Instance.Bal_Report.GetSlipDetails(BookingNo, Globals.BranchID, "Sp_Sel_BookingDetailsForReceipt");
        }

        ///// Set Variables ////
        if (ds.Tables[0].Rows.Count > 0)
        {
            termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
            St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
            logo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
            StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
            barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
            PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
            ShowHeaderSlogan = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
            PrintTermConditionOnStoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTermConditionOnStoreCopy"].ToString());
            PrintPhoneNo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintPhoneNo"].ToString());
            PrintCustomerSignature = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintCustomerSignature"].ToString());
            BookingTime = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBookingTime"].ToString());
            if (BookingTime)
            {
                printBookingTime = dsMain.Tables[0].Rows[0]["BookingTime"].ToString();
            }
            else
            {
                printBookingTime = "";
            }
        }
        if (ds.Tables[0].Rows[0]["LeftMessage"].ToString() == "")
        {
            leftMessage = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        else
        {
            leftMessage = ds.Tables[0].Rows[0]["LeftMessage"].ToString();
        }
        if (ds.Tables[0].Rows[0]["RightMessage"].ToString() == "")
        {
            rightMessage = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        else
        {
            rightMessage = ds.Tables[0].Rows[0]["RightMessage"].ToString();
        }
        // Makeing Table ////
        strPreview += "<table style='width:3in; page-break-after: always;'>";
        strPreview += "<tr><td>";
        strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        if (StoreCopy)
        {
            strPreview1 += "<table style='width:3in; page-break-after: always;'>";
            strPreview1 += "<tr><td>";
            strPreview1 += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        }
        // Check Logo Print On and Off //
        custPhone = dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString();
        if (!PrintPhoneNo)
            custPhone = "";
        if (custPhone != "")
        {
            displayPhone = "(" + custPhone + ")";
        }
        else
        {
            displayPhone = "";
        }
        if (logo)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='40px' height='40px' /></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='40px' height='40px' /></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
        }
        else
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' width='40Px'></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' width='40Px'></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td align='center' colspan='5'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr>";
        strPreview += "<tr><td align='center' colspan='5'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' style='font-size: 15px; font-family: Times New Roman; ' colspan='5' align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr>";
        strPreview += "<tr><td width='3in' style='font-size: 15px; font-family: Times New Roman; ' colspan='5' align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr>";

        if (ds.Tables[1].Rows[0]["MainTagline"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td colspan='5' style='font-family: Arial; font-size: 11Px;' align='center'>" + ds.Tables[1].Rows[0]["MainTagline"].ToString() + "</td></tr>";
            strPreview += "<tr><td colspan='5' style='font-family: Arial; font-size: 11Px;' align='center'>" + ds.Tables[1].Rows[0]["MainTagline"].ToString() + "</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        strPreview += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        if (barcode)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "&nbsp;&nbsp;<span style='font-family: c39HrP24DhTt; font-size: 32px'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</span> " + "</td></tr>";
            strPreview += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "&nbsp;&nbsp;<span style='font-family: c39HrP24DhTt; font-size: 32px'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</span> " + "</td></tr>";
        }
        else
        {
            if (StoreCopy)
                strPreview1 += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
            strPreview += "<tr><td colspan='5' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + "26" + "px; text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + " <span style='font-weight: Bold; color: #FF0000;'>" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</span>" + " " + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
        }
        //strPreview += "</table>";
        //strPreview += "<table width='3in'>";
        if (StoreCopy)
            strPreview1 += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
        strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td align='left' style='font-size: 12px; font-family: Arial; ' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
        strPreview += "<tr><td align='left' style='font-size: 12px; font-family: Arial; ' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5' >" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + " " + printBookingTime + "</td></tr>";
        strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5' >" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + " " + printBookingTime + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        strPreview += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
        if (StoreCopy)
            strPreview1 += "</table>";
        strPreview += "</table>";
        // Item Section
        if (StoreCopy)
            strPreview1 += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Qty&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Service&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'></td></tr>";
        strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Qty&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Service&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'></td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        strPreview += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
        {
            string ItemName = "", Color = "", Remarks = "", Process = "", ExProcess1 = "", ExProcess2 = "", Rate = "", Rate1 = "", Rate2 = "";
            string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
            string[] rate1 = rate.Split(';');
            string[] rate2 = rate.Split('@');
            Process = dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString();
            ExProcess1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString();
            ExProcess2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString();
            Rate = rate2[1].ToString();
            Rate1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString();
            Rate2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString();
            Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
            Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
            if (Color != "")
            { Color = "- " + Color; }
            else
            { Color = ""; }
            if (Remarks != "")
            { Remarks = "- " + Remarks; }
            else
            { Remarks = ""; }
            for (int k = 0; k < rate1.Length; k++)
            {
                ItemName += " " + rate1[k];
            }

            if (ExProcess1 != "None")
            {
                ExProcess1 = ExProcess1 + "@" + Rate1;
            }
            if (ExProcess2 != "None")
            {
                ExProcess2 = ExProcess2 + "@" + Rate2;
            }
            //// Makeing Dynamic Item list
            if (StoreCopy)
                strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '></td></tr>";
            strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '></td></tr>";
            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + "</td></tr>";
            }
            wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
            totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
        }
        double serviceTax = 0, ServiceRate = 0, amt1 = 0;
        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
        {
            serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
            serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
            serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
        }
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        strPreview += "<tr><td width='3in' colspan='4'>---------------------------</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["TotalQty"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'></td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '></td></tr>";
        strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["TotalQty"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'></td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '></td></tr>";
        //if (dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() != "0")
        //{
        //    if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
        //    {
        //        if (StoreCopy)
        //            strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Dis (" + Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) + "%)</td><td style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() + "</td></tr>";
        //        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Dis (" + Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) + "%)</td><td style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() + "</td></tr>";
        //    }
        //    else
        //    {
        //        if (StoreCopy)
        //            strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Dis</td><td style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() + "</td></tr>";
        //        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Dis</td><td style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() + "</td></tr>";
        //    }
        //}
        //if (St)
        //{
        //    if (StoreCopy)
        //        strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Tax</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Math.Round(serviceTax, 2).ToString() + "</td></tr>";
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Tax</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Math.Round(serviceTax, 2).ToString() + "</td></tr>";
        //}
        Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
        if (Rounded)
        {
            ServiceRate = Math.Round(serviceTax, 2);
            amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
        }
        else
        {
            ServiceRate = serviceTax;
            amt1 = Math.Ceiling(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()));
        }
        //if (StoreCopy)
        //    strPreview1 += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["PaymentMade"].ToString() + "</td></tr>";
        //strPreview += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["PaymentMade"].ToString() + "</td></tr>";
        //if (StoreCopy)
        //    strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000;'>" + amt1.ToString() + "</td></tr>";
        //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000;'>" + amt1.ToString() + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
        strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
        if (PrintDueDate)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 13px; font-family: Times New Roman; ' align='center' colspan='4'>" + "<span style='font-weight: bold; color: #FF0000;'>Ready On: &nbsp;" + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + "</span>" + "<span style='color: #FF0000'>" + " - " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</span>" + "</td></tr>";
            strPreview += "<tr><td style='font-size: 13px; font-family: Times New Roman; ' align='center' colspan='4'>" + "<span style='font-weight: bold; color: #FF0000;'>Ready On: &nbsp;" + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + "</span>" + "<span style='color: #FF0000'>" + " - " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</span>" + "</td></tr>";
        }
        if (ds.Tables[1].Rows[0]["Holiday"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["Holiday"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["CLOSED"].ToString() + "</span>" + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["Holiday"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["CLOSED"].ToString() + "</span>" + "</td></tr>";
        }
        if (ds.Tables[1].Rows[0]["Timing"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["ST"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["Timing"].ToString() + "</span>" + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; ' align='center' colspan='4'><span style='font-weight: bold; color: #FF0000;'>" + ds.Tables[1].Rows[0]["ST"].ToString() + "</span>" + " " + "<span style='color: #FF0000'>" + ds.Tables[1].Rows[0]["Timing"].ToString() + "</span>" + "</td></tr>";
        }
        if (dsMain.Tables[0].Rows[0]["PackageName"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Package :</span>" + " " + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Package :</span>" + " " + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Membership Id :</span>" + " " + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Membership Id :</span>" + " " + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Expiry Date :</span>" + " " + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 14px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Expiry Date :</span>" + " " + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Booked By :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</td></tr>";
        strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Booked By :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</td></tr>";
        if (dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() != "")
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Order Notes :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold'>Order Notes :</span>" + " " + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr>";
        }
        if (ShowHeaderSlogan)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td align='center' colspan='4' >----------</td></tr>";
            strPreview += "<tr><td align='center' colspan='4' >----------</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td align='center' colspan='4' >----------</td></tr>";
            strPreview += "<tr><td align='center' colspan='4' >----------</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td width='3in' colspan='3' height='10Px'></td></tr>";
        strPreview += "<tr><td width='3in' colspan='3' height='10Px'></td></tr>";
        if (termAndCondition)
        {
            if (!StoreCopy)
            {
                PrintTermConditionOnStoreCopy = false;
            }
            if (PrintTermConditionOnStoreCopy)
                strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' colspan='4'>Terms and Conditions</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' colspan='4'>Terms and Conditions</td></tr>";
            if (PrintTermConditionOnStoreCopy)
                strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
            strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
            if (dsMain.Tables[0].Rows[0]["TC1"].ToString() != "")
            {
                if (PrintTermConditionOnStoreCopy)
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";
                if (PrintTermConditionOnStoreCopy)
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
            }
            if (dsMain.Tables[0].Rows[0]["TC2"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (dsMain.Tables[0].Rows[0]["TC3"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term4"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term4"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term4"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term5"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term5"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term5"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term6"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term6"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term6"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term7"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term7"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term7"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term8"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term8"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term8"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term9"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term9"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term9"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term10"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term10"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term10"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term11"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term11"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term11"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term12"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term12"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term12"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term13"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term13"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term13"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term14"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term14"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term14"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
            if (ds.Tables[0].Rows[0]["Term15"].ToString() != "")
            {
                strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4'>" + "# " + ds.Tables[0].Rows[0]["Term15"].ToString() + "</td></tr>";
                strPreview += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                if (PrintTermConditionOnStoreCopy)
                {
                    strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='4' >" + "# " + ds.Tables[0].Rows[0]["Term15"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td width='3in' colspan='4' height='5Px'></td></tr>";
                }
            }
        }
        if (PrintCustomerSignature)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
            strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td  colspan='4' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='center'>Signature</td></tr>";
            strPreview += "<tr><td  colspan='4' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='center'>Signature</td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td width='3in' colspan='4' height='25Px'></td></tr>";
            strPreview += "<tr><td width='3in' colspan='4' height='25Px'></td></tr>";
            if (StoreCopy)
                strPreview1 += "<tr><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='left'>Customer</td><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='right'>Salesman</td></tr>";
            strPreview += "<tr><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='left'>Customer</td><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='right'>Salesman</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "</table>";
        strPreview += "</table>";
        strPreview += "</td></tr>";
        strPreview += "</table>";
        if (StoreCopy)
        {
            strPreview1 += "<table style='width:3in;'>";
            strPreview1 += "<tr><td width='3in' colspan='4' align='left'><span style='font-family:Arial;font-size:36px; font-weight:Bold; text-decoration:;font-style:'>Store Copy</span></td></tr>";
            strPreview1 += "</table>";
        }
        strPreview1 += "</td></tr>";
        strPreview1 += "</table>";
        ViewState["Msg"] = strPreview;
        return Tuple.Create<string, string>(strPreview, strPreview1);
    }
}
