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

public partial class Thermal_DeliverySlip : System.Web.UI.Page
{
    protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
    public string strPreview = string.Empty;
    public string strPreview1 = string.Empty;
    DataSet dschk = new DataSet();
    string _pkgType = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["BN"] == null || Request.QueryString["BN"] == "" || Request.QueryString["RS"] == null || Request.QueryString["RS"] == "")
            {
                Response.Redirect(FormsAuthentication.DefaultUrl, false);
            }
            else
            {
                string BookNum = Request.QueryString["BN"].ToString();
                string[] data = BookNum.Split('-');
                string date = data[1].Substring(1);
                dschk = BAL.BALFactory.Instance.Bal_Report.GetSlipDetails(data[0], Globals.BranchID, "Sp_Sel_BookingDetailsForReceipt");
                _pkgType = dschk.Tables[0].Rows[0]["PackageType"].ToString();
                if (_pkgType == "Flat Qty" || _pkgType == "Qty / Item")
                {
                    GetBookingDetailspackage(data[0], Request.QueryString["RS"].ToString(), date);
                }
                else
                {
                    GetBookingDetails(data[0], Request.QueryString["RS"].ToString(), date);
                }
               
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Print", "javascript:window.print();javascript:window.close();", true);
            }
            hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
            if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() != "")
            {
                hdnDirectPrint.Value = "true";
            }
            if (Request.QueryString["CloseWindow"] != null && Request.QueryString["CloseWindow"].ToString() != "")
            {
                hdnCloseWindow.Value = "true";
            }
        }
    }

    public void GetBookingDetails(string BookingNo, string amount, string date)
    {
        // Create variables ///
        string custPhone = string.Empty,printBookingTime=string.Empty, displayPhone = string.Empty, leftMessage = string.Empty, rightMessage = string.Empty;
        bool logo = false,BookingTime=true, termAndCondition = false, St = false, Rounded = false, StoreCopy = true, barcode = false, PrintDueDate = false, ShowHeaderSlogan = false, PrintTermConditionOnStoreCopy = false, PrintPhoneNo = false, PrintCustomerSignature = false, PrinttaxDetail = false;
        double wholeAmt = 0, totalAmt = 0, balance = 0, Advance1 = 0, TotalAdvance = 0, DeliveryDiscount = 0, totalWholeAmt = 0, bal = 0, advance = 0, ad = 0, ActualAdvance = 0;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);

        DataSet dsMain = new DataSet();
        cmd = new SqlCommand();
        cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
        cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookingNo));       
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
        StoreCopy = false;
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
            string ItemName = "", Color = "", Remarks = "", Process = "", ExProcess1 = "", ExProcess2 = "", Rate = "", Rate1 = "", Rate2 = "";
            string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
            string[] rate1 = rate.Split(';');
            string[] rate2 = rate.Split('@');
            Process = dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString();
            ExProcess1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString();
            ExProcess2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString();
            Rate = string.Format("{0:0.00}", rate2[1].ToString());
            Rate1 = string.Format("{0:0.00}", dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
            Rate2 = string.Format("{0:0.00}", dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString());
            Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
            Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
            string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["ItemName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookingNo, Globals.BranchID);
            string[] DeliverOrUnDeliver = ItemStatus.Split('/');
            string ClothesStatus = string.Empty;
            if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) == Convert.ToInt32(DeliverOrUnDeliver[1].ToString()))
                ClothesStatus = "Delivered";
            else
            {
                if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) > 0)
                    ClothesStatus = "Delivered" + "-" + DeliverOrUnDeliver[0];
            }
            if (ClothesStatus != "" && ClothesStatus != "Delivered")
            {
                ClothesStatus = ClothesStatus + "/" + dsMain.Tables[1].Rows[i]["SubPieces"].ToString();
            }
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
                strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" +string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
            strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" +string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + " " + ClothesStatus + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + " " + ClothesStatus + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + " " + ClothesStatus + "</td></tr>";
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + " " + ClothesStatus + "</td></tr>";
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
            strPreview1 += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["TotalQty"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>G Amt.</td><td width='1in' style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";
        strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsMain.Tables[0].Rows[0]["TotalQty"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>G Amt.</td><td width='1in' style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";
        if (dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() != "0")
        {
            if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()))) + "%)</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2)) + "</td></tr>";
                strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()))) + "%)</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2)) + "</td></tr>";
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
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Tax</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2).ToString()) + "</td></tr>";
        }
        for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
            ActualAdvance += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());
        Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
        if (Rounded)
        {
            ServiceRate = Math.Round(serviceTax, 2);
            amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - ActualAdvance - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
        }
        else
        {
            ServiceRate = serviceTax;
            amt1 = Math.Ceiling(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - ActualAdvance - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()));
        }      
        
        if (StoreCopy)
            strPreview1 += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", ActualAdvance) + "</td></tr>";
        strPreview += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", ActualAdvance) + "</td></tr>";
        if (StoreCopy)
            strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000;font-weight: Bold; '>" + string.Format("{0:0.00}", amt1.ToString()) + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000;font-weight: Bold; '>" + string.Format("{0:0.00}", amt1.ToString()) + "</td></tr>";
        //////// Variables
        balance = 0;
        double BookingDis = 0;
        BookingDis = Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
        try
        {
            for (int i = 0; i < dsMain.Tables[6].Rows.Count; i++)
                Advance1 += Convert.ToDouble(dsMain.Tables[6].Rows[i]["Payment"].ToString());

            for (int i = 0; i < dsMain.Tables[4].Rows.Count; i++)
                TotalAdvance += Convert.ToDouble(dsMain.Tables[4].Rows[i]["Payment"].ToString());

            for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());

            balance = (Math.Ceiling(Convert.ToDouble(amt1) + ActualAdvance - TotalAdvance - DeliveryDiscount));
            bal = balance;
            advance = Advance1;
            ad = Advance1;
        }
        catch (Exception ex)
        {
            balance = totalWholeAmt - Convert.ToDouble("0");
            bal = balance;
        }
        if (DeliveryDiscount == 0)
        {
            for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());
        }
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td  colspan='2' >-----------</td></tr>";
        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
        {
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' rowspan='2'>Delivery Details</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='2'>Already Paid</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Paid Now</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr>";
             if (DeliveryDiscount != 0)
                 strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Cash Dis</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round((DeliveryDiscount), 2)) + "</td></tr>";
             strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Balance</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr>";
        }
        else
        {
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' rowspan='2'>Delivery Details</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='2'>Already Paid</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(Convert.ToDecimal(ad), 0))) + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Paid Now</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr>";
             if (DeliveryDiscount != 0)
                 strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Cash Dis</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(DeliveryDiscount, 2))) + "</td></tr>";
             strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Balance</td><td style='font-size: 12px; font-family: Times New Roman;font-weight: Bold; '>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr>";
        }
        if (StoreCopy)
            strPreview1 += "<tr><td  colspan='4' >---------------------------</td></tr>";
        strPreview += "<tr><td  colspan='4' >---------------------------</td></tr>";
        if (PrinttaxDetail)
        {
            if (StoreCopy)
                strPreview1 += "<tr><td style='font-size: 10px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + " )</span></td></tr>";
            strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; '  colspan='4'><span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + " )</span></td></tr>";
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
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["FooterSloganText"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["FooterSloganText"].ToString() + "</td></tr>";
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
    }
    public void GetBookingDetailspackage(string BookingNo, string amount, string date)
    {
        // Create variables ///
        string custPhone = string.Empty,printBookingTime=string.Empty, displayPhone = string.Empty, leftMessage = string.Empty, rightMessage = string.Empty;
        bool logo = false,BookingTime=true, termAndCondition = false, St = false, Rounded = false, StoreCopy = true, barcode = false, PrintDueDate = false, ShowHeaderSlogan = false, PrintTermConditionOnStoreCopy = false, PrintPhoneNo = false, PrintCustomerSignature = false;
        double wholeAmt = 0, totalAmt = 0, balance = 0, Advance1 = 0, TotalAdvance = 0, DeliveryDiscount = 0, totalWholeAmt = 0, bal = 0, advance = 0, ad = 0, ActualAdvance = 0;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);

        DataSet dsMain = new DataSet();
        cmd = new SqlCommand();
        cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
        cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookingNo));        
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
        StoreCopy = false;
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
            string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["ItemName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookingNo, Globals.BranchID);
            string[] DeliverOrUnDeliver = ItemStatus.Split('/');
            string ClothesStatus = string.Empty;
            if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) == Convert.ToInt32(DeliverOrUnDeliver[1].ToString()))
                ClothesStatus = "Delivered";
            else
            {
                if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) > 0)
                    ClothesStatus = "Delivered" + "-" + DeliverOrUnDeliver[0];
            }
            if (ClothesStatus != "" && ClothesStatus != "Delivered")
            {
                ClothesStatus = ClothesStatus + "/" + dsMain.Tables[1].Rows[i]["SubPieces"].ToString();
            }
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
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + "," + ExProcess2 + " " + Color + " " + Remarks + " " + ClothesStatus + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess1 + " " + Color + " " + Remarks + " " + ClothesStatus + "</td></tr>";
            }
            else if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + ExProcess2 + " " + Color + " " + Remarks + " " + ClothesStatus + "</td></tr>";
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + "</td></tr>";
                strPreview += "<tr><td colspan='4' style='font-size: 12px; font-family: Times New Roman; '>" + " " + Color.Replace('-', ' ').Trim() + " " + Remarks + " " + ClothesStatus + "</td></tr>";
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
        //for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
        //    ActualAdvance += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());
        //Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
        //if (Rounded)
        //{
        //    ServiceRate = Math.Round(serviceTax, 2);
        //    amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - ActualAdvance - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
        //}
        //else
        //{
        //    ServiceRate = serviceTax;
        //    amt1 = Math.Ceiling(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - ActualAdvance - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()));
        //}

        //if (StoreCopy)
        //    strPreview1 += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman; '>" + ActualAdvance + "</td></tr>";
        //strPreview += "<tr><td style='font-size: 32px; font-family: C39HrP24DhTt; ' colspan='2' rowspan='2'></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Adv</td><td style='font-size: 12px; font-family: Times New Roman; '>" + ActualAdvance + "</td></tr>";
        //if (StoreCopy)
        //    strPreview1 += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000; '>" + amt1.ToString() + "</td></tr>";
        //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold; color: #FF0000;' align='center'>N.Amt.</td><td style='font-size: 12px; font-family: Times New Roman; color: #FF0000; '>" + amt1.ToString() + "</td></tr>";
        //////// Variables
        balance = 0;
        double BookingDis = 0;
        BookingDis = Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
        try
        {
            for (int i = 0; i < dsMain.Tables[6].Rows.Count; i++)
                Advance1 += Convert.ToDouble(dsMain.Tables[6].Rows[i]["Payment"].ToString());

            for (int i = 0; i < dsMain.Tables[4].Rows.Count; i++)
                TotalAdvance += Convert.ToDouble(dsMain.Tables[4].Rows[i]["Payment"].ToString());

            for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());

            balance = (Math.Ceiling(Convert.ToDouble(amt1) + ActualAdvance - TotalAdvance - DeliveryDiscount));
            bal = balance;
            advance = Advance1;
            ad = Advance1;
        }
        catch (Exception ex)
        {
            balance = totalWholeAmt - Convert.ToDouble("0");
            bal = balance;
        }
        if (DeliveryDiscount == 0)
        {
            for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());
        }
        //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td  colspan='2' >-----------</td></tr>";
        //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
        //{
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' rowspan='2'>Delivery Details</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='2'>Already Paid</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr>";
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Paid Now</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2) + "</td></tr>";
        //    if (DeliveryDiscount != 0)
        //        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Cash Dis</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Math.Round((DeliveryDiscount), 2) + "</td></tr>";
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Balance</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Convert.ToInt32(Math.Round(bal, 2)) + "</td></tr>";
        //}
        //else
        //{
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' rowspan='2'>Delivery Details</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='2'>Already Paid</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Convert.ToInt32(Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr>";
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Paid Now</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2) + "</td></tr>";
        //    if (DeliveryDiscount != 0)
        //        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Cash Dis</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Convert.ToInt32(Math.Round(DeliveryDiscount, 2)) + "</td></tr>";
        //    strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; '></td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Balance</td><td style='font-size: 12px; font-family: Times New Roman; '>" + Convert.ToInt32(Math.Round(bal, 2)) + "</td></tr>";
        //}
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
                strPreview1 += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["FooterSloganText"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 11px; font-family: Times New Roman; font-weight: Bold;' align='center' colspan='4'>" + ds.Tables[0].Rows[0]["FooterSloganText"].ToString() + "</td></tr>";
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
    }
   
}
