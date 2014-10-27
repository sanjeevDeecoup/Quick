using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace QuickWeb.LaserPrinter
{
    public partial class InvoiceStatementSlip : System.Web.UI.Page
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected string strPageName = "~/new_booking/DeliverySlip.aspx", BranchDetails = string.Empty, BookingNumber = string.Empty, CustomerName = string.Empty, CustomerAddress = string.Empty, ReceiptDate = string.Empty, DueDate = string.Empty, DueTime = string.Empty, UrgentDelivery = string.Empty, TotalAmount = "0", DiscountRate = "0", RebateAmount = "0", NetAmount = "0", AdvPayment = "0", PreviousDue = "0", DuePayment = "0", DiscountOnPayment = "0", Remarks = string.Empty, strProcess = string.Empty;     
        protected bool height = false;
        public string strPreview = string.Empty;      
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CustCode"] == null || Request.QueryString["CustCode"] == "" || Request.QueryString["fromDate"] == null || Request.QueryString["fromDate"] == "")
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl, false);
                }
                else
                {
                    hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
                    if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() != "")
                    {
                        hdnDirectPrint.Value = "true";
                    }
                    if (Request.QueryString["CloseWindow"] != null && Request.QueryString["CloseWindow"].ToString() != "")
                    {
                        hdnCloseWindow.Value = "true";
                    }
                     GetBookingDetailsForBookingNumber(Request.QueryString["CustCode"].ToString(), Request.QueryString["fromDate"].ToString(), Request.QueryString["ToDate"].ToString());
                }                
            }
        }
      
        public string GetBookingDetailsForBookingNumber(string CustCode, string fromDate, string Todate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                DataSet dsMain = new DataSet();
                DataSet dsInvoiceStatement = new DataSet();
                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 2);
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                ds = AppClass.GetData(cmd);
                dsInvoiceStatement = BAL.BALFactory.Instance.Bal_Report.GetInvoiceStatementForCustomer(Globals.BranchID, fromDate, Todate, CustCode);
                cmd = new SqlCommand();
                cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNumber", dsInvoiceStatement.Tables[1].Rows[0]["BookingNumber"].ToString());
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                dsMain = AppClass.GetData(cmd);
                double balance = 0;
                string advance = "0", ad = "0", pend = "";
                double pending = 0;
                bool StoreCopy = true, PrintTaxDetail = false;
                StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());

                string strTotalInvoice = string.Empty;
                if (dsInvoiceStatement.Tables[2].Rows.Count>0)
                {
                    for (int j = 0; j < dsInvoiceStatement.Tables[2].Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            strTotalInvoice = dsInvoiceStatement.Tables[2].Rows[j]["BookingNumber"].ToString();
                        }
                        else
                        {
                            strTotalInvoice = strTotalInvoice+", " + dsInvoiceStatement.Tables[2].Rows[j]["BookingNumber"].ToString();
                        }
                    } 
                }
                double wholeAmt = 0, disWhole = 0, WhAmt = 0, bal = 0;
                int totalItem = 0;                
                double discount = 0, serviceTax = 0, SlipVat = 0, SlipCess = 0, SlipHCess = 0;
                totalItem = Convert.ToInt32(dsInvoiceStatement.Tables[0].Compute("Sum(Qty)", string.Empty));
                for (int i = 0; i < dsInvoiceStatement.Tables[0].Rows.Count; i++)
                {
                    serviceTax += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["MainTax"].ToString());
                    SlipVat += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["MainTax"].ToString());
                    serviceTax += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Cess"].ToString());
                    SlipCess += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Cess"].ToString());
                    serviceTax += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["HCess"].ToString());
                    SlipHCess += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["HCess"].ToString());                   
                }
                double totalWholeAmt = 0;
                double dis = 0;
                if (disWhole > 0)
                {
                    totalWholeAmt = wholeAmt;
                    totalWholeAmt -= disWhole;
                    totalWholeAmt = Math.Round(Convert.ToDouble(totalWholeAmt) + Convert.ToDouble(serviceTax), 2);
                }
                else
                {
                    totalWholeAmt = wholeAmt + serviceTax;
                }
                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) != "True")
                {
                    totalWholeAmt = Math.Ceiling(totalWholeAmt);
                }
                bool Flag = false, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintSubItems = false;
                string logoLeftRight = string.Empty;
                prePrintedOrBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
                headerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                footerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
                termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
                logoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
                logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
                tableBorder = Convert.ToBoolean(ds.Tables[0].Rows[0]["TableBorder"].ToString());
                PrintSubItems = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintSubItem"].ToString());
                PrintTaxDetail = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTaxDetail"].ToString());
                // First Section
                int invoice = 1;
            START:
                if (prePrintedOrBanner)
                {
                    if (Convert.ToInt32(hdnId.Value) > 14)
                    {
                        strPreview += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";
                        height = true;
                    }
                    if (tableBorder)
                    {
                        strPreview += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";
                    }
                    else
                    {
                        strPreview += "<table class='TableData' style='width:7.9in;'><tr><td>";
                    }
                    strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";
                    if (logoLeftRight == "1")
                    {
                        Flag = false;
                        if (logoOnReceipt)
                        {
                            strPreview += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";

                            strPreview += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                        else
                        {

                            strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                    }
                    else
                    {
                        if (logoOnReceipt)
                        {

                            strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td></tr></table></td></tr>";
                        }
                        else
                        {

                            strPreview += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";
                        }
                    }
                }
                else
                {
                    Flag = true;

                    if (Convert.ToInt32(hdnId.Value) > 14)
                    {
                        strPreview += "<table style='width:7.9in;height:5.11in'><tr><td></td><tr></table>";
                        height = true;
                        strPreview += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                    }
                    else
                    {
                        strPreview += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
                    }
                }
                //

                // Second Section
                if (!Flag)
                {
                    if (StoreCopy)
                        strPreview += "<tr><td><table style='width:7.9in;'><tr>";
                }
                else
                {

                }
                //if (tableBorder)
                if (dsInvoiceStatement.Tables[0].Rows.Count >= 16)
                {
                    invoice++;
                }
                else
                {

                }

                if (barcode)
                {

                }
                else
                {

                }

                // Third Section

                if (headerBanner)
                {
                    // strPreview += "<tr><td colspan='4' align='center'><table style='width: 7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
                }
                // Forth Section
                string amtFormat = BAL.BALFactory.Instance.BAL_New_Bookings.DisplayAmountFormat(Globals.BranchID);
                double Advance1 = 0, TotalAdvance = 0, DeliveryDiscount = 0;


                // Fifth Section And // Six Section
                
                if (prePrintedOrBanner)
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='5' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px' nowrap='nowrap'>M/s :&nbsp;" + dsInvoiceStatement.Tables[0].Rows[0]["CustomerName"].ToString() + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px' nowrap='nowrap'>From&nbsp" + Request.QueryString["fromDate"].ToString() + "&nbsp;</td><td style='font-weight:bold;font-size:12px' nowrap='nowrap'>To&nbsp;" + Request.QueryString["ToDate"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Bill Date :&nbsp;</td><td style='font-size:12px'> " + Convert.ToDateTime(DateTime.Today.ToString("dd MMM yyyy")).DayOfWeek.ToString() + " / " + DateTime.Today.ToString("dd MMM yyyy") + "</td></tr></table></td></tr></table></td></tr>";
                    strPreview += "<tr style='font-size:12px'><td colspan='5'><table style='width: 7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'>S.No</td><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:4.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='center'>Qty</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Rate</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                }
                else
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='5' align='center'><table style='width: 7.9in; border: thin solid #000000;'><tr><td style='font-weight:bold;font-size:12px' nowrap='nowrap'>M/s :&nbsp;" + dsInvoiceStatement.Tables[0].Rows[0]["CustomerName"].ToString() + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px' nowrap='nowrap'>From&nbsp;" + Request.QueryString["fromDate"].ToString() + "&nbsp;</td><td style='font-weight:bold;font-size:12px' nowrap='nowrap'>To&nbsp;" + Request.QueryString["ToDate"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Bill Date :&nbsp;</td><td style='font-size:12px'> " + Convert.ToDateTime(DateTime.Today.ToString("dd MMM yyyy")).DayOfWeek.ToString() + " / " + DateTime.Today.ToString("dd MMM yyyy") + "</td></tr></table></td></tr></table></td></tr>";
                    strPreview += "<tr style='font-size:12px'><td colspan='5'><table style='width: 7.9in; border: thin solid #000000;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'>S.No</td><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:4.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='center'>Qty</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Rate</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                }
                int incRow = 1;
                if (Convert.ToInt32(hdnId.Value) < 15)
                {
                    balance = 0;
                    advance = "0";
                    wholeAmt = 0;
                }
                int checkCondition = 0;
                if (Convert.ToInt32(hdnId.Value) < 15)
                    checkCondition = (dsInvoiceStatement.Tables[0].Rows.Count > 15 ? 15 : dsInvoiceStatement.Tables[0].Rows.Count);
                else
                {
                    checkCondition = Convert.ToInt32(hdnId.Value) + 15;
                    if (checkCondition >= dsInvoiceStatement.Tables[0].Rows.Count)
                    {
                        checkCondition = dsInvoiceStatement.Tables[0].Rows.Count - Convert.ToInt32(hdnId.Value);
                        checkCondition = Convert.ToInt32(hdnId.Value) + checkCondition;
                    }
                }
                for (int i = (Convert.ToInt32(hdnId.Value)); i < checkCondition; i++)
                {
                    strPreview += "<tr style='font-size:12px'><td >" + dsInvoiceStatement.Tables[0].Rows[i]["SNo"].ToString() + "</td><td ></td><td>" + "<bolder>" + dsInvoiceStatement.Tables[0].Rows[i]["Garment"].ToString() + "</td><td align='center'>" + dsInvoiceStatement.Tables[0].Rows[i]["Qty"].ToString() + "</td><td align='right'>" + string.Format("{0:0.00}", Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Rate"].ToString()))  + "</td><td align='right'>" + string.Format("{0:0.00}", Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Amount"].ToString())) + "</td></tr>";
                    incRow++;
                    hdnId.Value = (Convert.ToInt32(hdnId.Value) + 1).ToString();
                    wholeAmt += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Amount"].ToString());
                }

                if ((Convert.ToInt32(hdnId.Value) % 15) != 0)
                {
                    int rev = 0;
                    int mod = checkCondition % 15;
                    //rev = (rev * 8) + mod;
                    //checkCondition = checkCondition / 8;

                    for (int i = 2; i <= (15 - mod); i++)
                    {
                        strPreview += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                    }
                }

                // Seventh Section
                termAndCondition = false;
                if (Convert.ToInt32(hdnId.Value) < 15)
                {
                    if (termAndCondition)
                    {
                        if (PrintTaxDetail)
                            strPreview += "<tr style='font-size:10px'><td colspan='4' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", SlipVat) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", SlipCess) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", SlipHCess) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                        else
                            strPreview += "<tr style='font-size:10px'><td colspan='4' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Discount (" + string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString())) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Discount (" + string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString())) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                            }
                        }
                        else
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px' ><td colspan='6' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                        if (PrintTaxDetail)
                            strPreview += "<tr style='font-size:10px'><td colspan='3' rowspan='2' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000;font-size: 12px;'>" + strTotalInvoice + "</td><td rowspan='3' align='center' valign='top' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000;'>" + totalItem + "</td></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                        else
                            strPreview += "<tr style='font-size:10px'><td colspan='3' rowspan='2'  style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000;font-size: 12px;'>" + strTotalInvoice + "</td><td rowspan='3' align='center' valign='top' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000;'>" + totalItem + "</td></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";

                        // strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                        if (St)
                        {
                            if (serviceTax > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;</td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;</td></tr>";
                            }
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "</td></tr>";
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                        }
                    }

                    strPreview += "</table></td></tr>";
                }
                else
                {
                    if (hdnId.Value != dsInvoiceStatement.Tables[0].Rows.Count.ToString())
                    {
                        if (termAndCondition)
                        {
                            strPreview += "<tr style='font-size:10px'><td colspan='6' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td></tr>";

                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px' ><td colspan='6' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                            strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            if (St)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right'></td><td align='right'></td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }

                            strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                        }

                        strPreview += "</table></td></tr>";
                    }
                    else
                    {
                        if (termAndCondition)
                        {
                            if (PrintTaxDetail)
                                strPreview += "<tr style='font-size:10px'><td colspan='4' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", SlipVat) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", SlipCess) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", SlipHCess) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                            else
                                strPreview += "<tr style='font-size:10px'><td colspan='4' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                            //strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                                }
                            }
                            else
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td  colspan='4'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                                }
                            }
                        }
                        else
                        {
                           // strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                           // strPreview += "<tr style='font-size:10px'><td colspan='4' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000' align='center'>Tax Bifercation<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", SlipVat) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", SlipCess) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", SlipHCess) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td colspan='3' rowspan='2' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000;font-size: 12px;'>" + strTotalInvoice + "</td><td rowspan='3' align='center' valign='top' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000;'>" + totalItem + "</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                            if (St)
                            {
                                if (serviceTax > 0)
                                {
                                    //strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;</td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;</td></tr>";
                                }
                            }                            
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Grand Total</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='4'></td><td align='right' style='font-size:12px;font-weight:bold'>Grand Total</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                            }
                        }

                        strPreview += "</table></td></tr>";
                    }


                    if (hdnId.Value != dsInvoiceStatement.Tables[0].Rows.Count.ToString())
                    {
                        hdnTemp.Value = "1";

                        strPreview += "</table>";

                        strPreview += "</td></tr></table>";
                        goto START;
                    }
                }

                strPreview += "</table>";
                if (hdnId.Value != dsInvoiceStatement.Tables[0].Rows.Count.ToString())
                {
                    strPreview += "</td></tr></table>";
                    goto START;
                }
            }
            catch (Exception ex) { }

            ViewState["Msg"] = strPreview;           
            hdnId.Value = "0";
            return strPreview;
        }
    }
}