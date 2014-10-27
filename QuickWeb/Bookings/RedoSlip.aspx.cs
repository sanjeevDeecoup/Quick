using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace QuickWeb.Bookings
{
    public partial class RedoSlip : System.Web.UI.Page
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected string strPageName = "~/Bookings/RedoSlip.aspx", BranchDetails = string.Empty, BookingNumber = string.Empty, CustomerName = string.Empty, CustomerAddress = string.Empty, ReceiptDate = string.Empty, DueDate = string.Empty, DueTime = string.Empty, UrgentDelivery = string.Empty, TotalAmount = "0", DiscountRate = "0", RebateAmount = "0", NetAmount = "0", AdvPayment = "0", PreviousDue = "0", DuePayment = "0", DiscountOnPayment = "0", Remarks = string.Empty, strProcess = string.Empty;
        protected bool chkBookingNumber = false;
        protected bool CheckStoreCopy = false;
        protected bool height = false;
        public string strPreview = string.Empty;
        public string strPreview1 = string.Empty;
        public string strPreview2 = string.Empty;
        public string strPreview3 = string.Empty;
        public string strPreview4 = string.Empty;
        public string strFinal = string.Empty;
        protected string Inch = string.Empty;
        private string _pkgType = string.Empty;
        private DataSet dschk = new DataSet();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["BN"] == null || Request.QueryString["BN"] == "")
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl, false);
                }
                else
                {
                    string BookNum = Request.QueryString["BN"].ToString();
                    string[] data = BookNum.Split('-');
                    string date = data[1].Substring(1);
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
                        hdnBookingNumber.Value = data[0];
                    }
                    if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() == "true")
                    {
                        hdnRedirectBack.Value = "true";
                    }
                    dschk = BAL.BALFactory.Instance.Bal_Report.GetSlipDetails(data[0], Globals.BranchID, "Sp_REDOSlip");
                    //_pkgType = dschk.Tables[0].Rows[0]["PackageType"].ToString();
                    if (_pkgType == "Flat Qty" || _pkgType == "Qty / Item")
                    {
                        GetBookingDetailsForBookingNumberPackageDetail(data[0], date);
                    }
                    else
                    {
                        GetBookingDetailsForBookingNumber(data[0], date);
                        GetBookingDetailsForBookingNumberProcessRate(data[0], date);
                    }
                    if (Request.QueryString["Email"] != null && Request.QueryString["Email"].ToString() == "true")
                    {
                        btnEmail_Click(null, EventArgs.Empty);
                    }
                }
            }
            //btnf2.Click += (sender2, args) => Response.Redirect("~/New_Booking/frm_New_Booking.aspx?option=Edit&BkNo=" + Request.QueryString["BN"].Split('-')[0].ToString());
        }


        public string GetBookingDetailsForBookingNumber(string BookNumber, string date, bool isComparing = false)
        {
            try
            {
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
                    cmd.Parameters.Add(new SqlParameter("@BookingBackUpId", BookNumber));
                }
                else
                {
                    cmd.CommandText = "Sp_REDOSlip";
                    cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookNumber));
                }

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
                dsMain = AppClass.GetData(cmd);

                DataSet dsPending = new DataSet();
                cmd = new SqlCommand();
                cmd.CommandText = "Sp_Sel_BookingPrevoiusDue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CustCode", dsMain.Tables[0].Rows[0]["CustomerCode"].ToString()));
                // if date is null, that would be if we passed from compare, we need to find the due date, that is passed to this date
                if (date == null)
                {
                    var cmdDate = new SqlCommand();
                    cmdDate.CommandText = "SELECT convert(varchar, BookingDeliveryDate, 106) from EntBookings where BookingNumber = " + dsMain.Tables[0].Rows[0]["BookingNumber"].ToString()
                        + " AND BranchId = " + Globals.BranchID + "";
                    date = PrjClass.ExecuteScalar(cmdDate);
                }
                cmd.Parameters.Add(new SqlParameter("@Date", date.ToString()));
                cmd.Parameters.Add(new SqlParameter("@BN", BookNumber));
                cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));

                dsPending = AppClass.GetData(cmd);
                if (hdnEmailId != null)
                    hdnEmailId.Value = BookNumber;
                double balance = 0;
                string advance = "0", ad = "0", pend = "";
                double pending = 0;
                bool StoreCopy = true, PrintTaxDetail = false;
                StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
                if (StoreCopy)
                    CheckStoreCopy = StoreCopy;
                pending = Convert.ToDouble(dsPending.Tables[0].Rows[0]["DuePayment"].ToString());

                double wholeAmt = 0, disWhole = 0, WhAmt = 0, bal = 0;
                int totalItem = 0;
                totalItem = Convert.ToInt32(dsMain.Tables[0].Rows[0]["TotalQty"].ToString());
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    //string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();

                    //string[] rate1 = rate.Split(';');
                    //for (int j = 0; j < rate1.Length; j++)
                    //{
                    //    string[] tempRate = rate1[j].Split('@');
                    //    wholeAmt += Convert.ToDouble((Convert.ToDouble(tempRate[0].ToString()) * Convert.ToDouble(tempRate[1].ToString())));
                    //    WhAmt = wholeAmt;
                    //}

                    //if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                    //{
                    //    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
                    //    WhAmt = wholeAmt;
                    //    //totalItem += Convert.ToInt32(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString());
                    //}
                    //if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                    //{
                    //    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString());
                    //    WhAmt = wholeAmt;
                    //    //totalItem += Convert.ToInt32(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString());
                    //}
                }
                double discount = 0, serviceTax = 0;
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                }
                //discount = Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString());
                disWhole = Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString());
                double totalWholeAmt = 0;
                double dis = 0;
                if (disWhole > 0)
                {
                    totalWholeAmt = wholeAmt;
                    // dis = Math.Round(disWhole * discount / 100, 5);
                    totalWholeAmt -= disWhole;
                    totalWholeAmt = Math.Round(Convert.ToDouble(totalWholeAmt) + Convert.ToDouble(serviceTax), 2);
                }
                else
                {
                    totalWholeAmt = wholeAmt + serviceTax;
                }
                bool Flag = false, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintDueDate = false, PrintSubItems = false;
                string logoLeftRight = string.Empty;
                if (hdnId == null)
                {
                    hdnId = new HiddenField();
                    hdnId.Value = "0";
                }
                if (hdnTemp == null)
                {
                    hdnTemp = new HiddenField();
                    hdnTemp.Value = "0";
                }
                PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
                prePrintedOrBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
                headerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                footerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
                termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                PrintSubItems = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintSubItem"].ToString());
                PrintTaxDetail = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTaxDetail"].ToString());
                if (dsMain.Tables[0].Rows[0]["TaxTypes"].ToString() == "EXCLUSIVE")
                {
                    St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                }
                else
                {
                    St = false;
                }
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
                logoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
                logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
                tableBorder = Convert.ToBoolean(ds.Tables[0].Rows[0]["TableBorder"].ToString());
                // First Section
                int invoice = 1;
                strPreview2 = "<table style='font-size:12px;font-weight:bold'><tr><td>Dear Customer ,</td></tr>";
                strPreview2 += "<tr><td>Thank you for your association with " + ds.Tables[0].Rows[0]["Headertext"].ToString() + ".We are pleased to provide you details around your recent transaction with us.</td></tr>";
                strPreview2 += "<tr><td>Invoice No :" + dsMain.Tables[0].Rows[0]["BookingNumber"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Customer Name :" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Address :" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Phone No. :" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Booking Date :" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td></tr>";
            START:
                if (prePrintedOrBanner)
                {
                    if (Convert.ToInt32(hdnId.Value) > 8)
                    {
                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";

                        strPreview += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";
                        height = true;
                    }
                    if (tableBorder)
                    {
                        if (StoreCopy)
                            strPreview1 += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";

                        strPreview += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<table class='TableData' style='width:7.9in;'><tr><td>";

                        strPreview += "<table class='TableData' style='width:7.9in;'><tr><td>";
                    }
                    if (StoreCopy)
                        strPreview1 += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";

                    strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";
                    if (logoLeftRight == "1")
                    {
                        Flag = false;
                        if (logoOnReceipt)
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";

                            strPreview += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";
                            if (StoreCopy)
                                strPreview1 += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                    }
                    else
                    {
                        if (logoOnReceipt)
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";
                        }
                    }
                }
                else
                {
                    Flag = true;

                    if (Convert.ToInt32(hdnId.Value) > 8)
                    {
                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:5.12in'><tr><td></td><tr></table>";

                        strPreview += "<table style='width:7.9in;height:5.11in'><tr><td></td><tr></table>";
                        height = true;

                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                        strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                        strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
                    }
                }
                //

                // Second Section
                if (!Flag)
                {
                    if (StoreCopy)
                        strPreview1 += "<tr><td><table style='width:7.9in;'><tr>";

                    strPreview += "<tr><td><table style='width:7.9in;'><tr>";
                }
                else
                {
                    if (tableBorder)
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr><td><table style='width:7.9in;border: thin solid #000000;'><tr>";

                        strPreview += "<tr><td><table style='width:7.9in;border: thin solid #000000;'><tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr><td><table style='width:7.9in;'><tr>";

                        strPreview += "<tr><td><table style='width:7.9in;'><tr>";
                    }
                }
                //if (tableBorder)
                if (dsMain.Tables[1].Rows.Count >= 10)
                {
                    if (StoreCopy)
                        strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                    strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                    invoice++;
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";

                    strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                }

                if (barcode)
                {
                    if (StoreCopy)
                        strPreview1 += "<td align='right' style='width:3.0in;font-family: c39hrp24dhtt;font-size:32px'  valign='top'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td>";

                    strPreview += "<td align='right' style='width:3.0in;font-family: c39hrp24dhtt;font-size:32px'  valign='top'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<td style='width:3.0in'>.</td>";

                    strPreview += "<td style='width:3.0in'>.</td>";
                }
                if (StoreCopy)
                    strPreview1 += "<td style='width:3.0in;font-size:11px' align='center'><table><tr><td align='center'  valign='top' style='width:2.0in'><table><tr><td style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "-" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + " " + "</td></tr><tr><td align='center'  valign='top' style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                strPreview += "<td style='width:3.0in;font-size:11px' align='center'><table><tr><td align='center'  valign='top' style='width:2.0in'><table><tr><td style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "-" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + " " + "</td></tr><tr><td align='center'  valign='top' style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                // Third Section

                if (headerBanner)
                {
                    if (StoreCopy)
                        strPreview1 += "<tr><td colspan='4' align='center'><table style='width:7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";

                    strPreview += "<tr><td colspan='4' align='center'><table style='width:7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
                }
                // Forth Section
                string amtFormat = BAL.BALFactory.Instance.BAL_New_Bookings.DisplayAmountFormat(Globals.BranchID);
                double Advance1 = 0;
                if (previousDue)
                {
                    if (Convert.ToInt32(hdnId.Value) < 11)
                    {
                        balance = 0;
                        try
                        {
                            for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
                                Advance1 += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());

                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                balance = (((pending + totalWholeAmt) - Advance1));
                            }
                            else
                            {
                                balance = Math.Ceiling(((pending + totalWholeAmt) - Advance1));
                            }
                            bal = balance;

                            advance = Advance1.ToString();
                            ad = Advance1.ToString();
                        }
                        catch (Exception ex)
                        {
                            balance = ((pending + totalWholeAmt) - Convert.ToDouble("0"));
                            bal = balance;
                        }
                    }
                    //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    //{
                    //    if (StoreCopy)
                    //        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center'>Previous Due</td></td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center'>Previous Due</td></td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    //else
                    //{
                    //    if (StoreCopy)
                    //        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center'>Previous Due</td><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center'>Previous Due</td><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 0)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 0)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32((totalWholeAmt - Advance1)) + "</td></tr>";
                    pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
                }
                else
                {
                    if (Convert.ToInt32(hdnId.Value) < 11)
                    {
                        balance = 0;
                        try
                        {
                            for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
                                Advance1 += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                balance = (((totalWholeAmt) - Advance1));
                            }
                            else
                            {
                                balance = Math.Ceiling((((totalWholeAmt) - Advance1)));
                            }

                            bal = balance;
                            advance = Advance1.ToString();
                            ad = Advance1.ToString();
                        }
                        catch (Exception ex)
                        {
                            balance = totalWholeAmt - Convert.ToDouble("0");
                            bal = balance;
                        }
                    }
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(bal)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(bal)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    //{
                    //    if (StoreCopy)
                    //        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    //else
                    //{
                    //    if (StoreCopy)
                    //        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32(Math.Round(bal, 0)) + "</td></tr>";
                    pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
                }
                strPreview2 += "<tr><td>Due Delivery Date :" + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                // Fifth Section
                if (PrintDueDate)
                {
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:12px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                    strPreview += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:12px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";
                    strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='right' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'></td></tr></table></td></tr></table></td></tr>";

                    strPreview += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='right' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'</td><td style='font-size:12px'></td></tr></table></td></tr></table></td></tr>";
                    strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";
                }
                // Six Section
                if (PrintSubItems)
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px' nowrap='nowrap'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                    strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px' nowrap='nowrap'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                }
                else
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                    strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:12px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                }

                strPreview2 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:12px'>S.No.</td><td style='width:0.5in;font-weight: bold;font-size:12px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:12px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:12px' align='right'>Amount</td></tr>";
                strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                GetTempSave(BookNumber, totalItem);
                int incRow = 1;
                if (Convert.ToInt32(hdnId.Value) < 9)
                {
                    balance = 0;
                    advance = "0";
                    wholeAmt = 0;
                }
                int checkCondition = 0;
                if (Convert.ToInt32(hdnId.Value) < 9)
                    checkCondition = (dsMain.Tables[1].Rows.Count > 9 ? 9 : dsMain.Tables[1].Rows.Count);
                else
                {
                    checkCondition = Convert.ToInt32(hdnId.Value) + 9;
                    if (checkCondition >= dsMain.Tables[1].Rows.Count)
                    {
                        checkCondition = dsMain.Tables[1].Rows.Count - Convert.ToInt32(hdnId.Value);
                        checkCondition = Convert.ToInt32(hdnId.Value) + checkCondition;
                    }
                }
                for (int i = (Convert.ToInt32(hdnId.Value)); i < checkCondition; i++)
                {
                    string ItemName = string.Empty, Color = string.Empty, Remarks = string.Empty, UnitDesc = string.Empty;
                    string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
                    string[] rate1 = rate.Split(';');
                    string[] rate2 = rate.Split('@');
                    Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
                    UnitDesc = dsMain.Tables[1].Rows[i]["UnitDesc"].ToString();
                    Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
                    string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["StatusName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookNumber, Globals.BranchID);
                    string[] DeliverOrUnDeliver = ItemStatus.Split('/');
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
                    if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                    {
                        if (PrintSubItems)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                            strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                            strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                    }
                    else
                    {
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                        {
                            if (PrintSubItems)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                            {
                                if (PrintSubItems)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", float.Parse(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString())) + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (PrintSubItems)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                        }
                    }
                    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());

                    incRow++;
                    hdnId.Value = (Convert.ToInt32(hdnId.Value) + 1).ToString();
                }

                if ((Convert.ToInt32(hdnId.Value) % 9) != 0)
                {
                    int rev = 0;
                    int mod = checkCondition % 9;
                    //rev = (rev * 8) + mod;
                    //checkCondition = checkCondition / 8;

                    for (int i = 2; i <= (9 - mod); i++)
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";

                        strPreview += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                        strPreview2 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                    }
                }

                // Seventh Section

                if (Convert.ToInt32(hdnId.Value) < 9)
                {
                    if (termAndCondition)
                    {
                        if (PrintTaxDetail)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp;<span style='font-weight: bold; font-size: 10px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>CurrentDue</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                        strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                        if (PrintTaxDetail)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (St)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                    }
                    if (StoreCopy)
                        strPreview1 += "</table></td></tr>";
                    strPreview += "</table></td></tr>";

                    strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";
                    if (St)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "</td></tr>";
                    }
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Advance</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", ad) + "</td></tr>";
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", pend) + "</td></tr>";
                    }
                    else
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", pend) + "</td></tr>";
                    }
                }
                else
                {
                    if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                    {
                        if (termAndCondition)
                        {
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy&nbsp; <span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td> <td style='font-weight: bold; border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;Terms and Conditions (R.O.A.C) &nbsp;<span style='font-weight: bold; font-size: 10px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ": " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td> <td style='font-weight: bold; border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size: 10px'><td ></td><td align='right'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";

                            //if (StoreCopy)
                            //    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            //strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (St)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right'></td><td align='right'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right'></td><td align='right'></td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                        }
                        if (StoreCopy)
                            strPreview1 += "</table></td></tr>";
                        strPreview += "</table></td></tr>";
                    }
                    else
                    {
                        if (termAndCondition)
                        {
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy <span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp;<span style='font-weight: bold; font-size: 10px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }

                            //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            //{
                            //    if (StoreCopy)
                            //        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2) + "</td></tr>";
                            //    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2) + "</td></tr>";
                            //}
                            //else
                            //{
                            //    if (StoreCopy)
                            //        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            //    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            //}
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (St)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        if (StoreCopy)
                            strPreview1 += "</table></td></tr>";
                        strPreview += "</table></td></tr>";
                    }

                    strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";
                    if (St)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", dis) + "</td></tr>";
                    }
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Advance</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", ad) + "</td></tr>";
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", pend) + "</td></tr>";
                    }
                    else
                    {
                        strPreview2 += "<tr style='font-size:11px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", pend) + "</td></tr>";
                    }

                    if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                    {
                        hdnTemp.Value = "1";
                        if (StoreCopy)
                            strPreview1 += "</table>";
                        strPreview += "</table>";
                        if (StoreCopy)
                            strPreview1 += "</td></tr></table>";
                        strPreview += "</td></tr></table>";
                        goto START;
                    }
                }
                if (StoreCopy)
                    strPreview1 += "</table></td></tr></table>";
                strPreview += "</table></td></tr></table>";
                if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                {
                    if (StoreCopy)
                        strPreview1 += "</td></tr></table>";
                    strPreview += "</td></tr></table>";
                    goto START;
                }
                strPreview2 += "<tr><td nowrap='nowrap' colspan='3'>Assuring you of our best service.</td></tr>";
                strPreview2 += "<tr><td colspan='3'>Warm Regards.</td></tr>";
                strPreview2 += "<tr><td colspan='3'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</td></tr>";
                strPreview2 += "</table>";
            }
            catch (Exception ex) { }
            string MyHiddenValue = "";
            MyHiddenValue = Globals.StorePrint;
            if (MyHiddenValue != "ST_COPY")
            {
                ViewState["Msg"] = strPreview;
                ViewState["Msg1"] = strPreview2;
                hdnId.Value = "0";
                strFinal = strPreview;
            }
            else if (MyHiddenValue == "ST_COPY")
            {
                ViewState["Msg"] = strPreview1;
                ViewState["Msg1"] = strPreview2;
                hdnId.Value = "0";
                strFinal = strPreview1;
            }
            return strFinal;
        }

        public string GetBookingDetailsForBookingNumberProcessRate(string BookNumber, string date)
        {
            try
            {
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
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BookingPrefix", Request.QueryString["BookingPrefix"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookNumber));
                cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
                dsMain = AppClass.GetData(cmd);

                DataSet dsPending = new DataSet();
                cmd = new SqlCommand();
                cmd.CommandText = "Sp_Sel_BookingPrevoiusDue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CustCode", dsMain.Tables[0].Rows[0]["CustomerCode"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@Date", date.ToString()));
                cmd.Parameters.Add(new SqlParameter("@BN", BookNumber));
                cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));

                dsPending = AppClass.GetData(cmd);
                hdnEmailId.Value = BookNumber;
                double balance = 0;
                string advance = "0", ad = "0", pend = "";
                double pending = 0;
                bool StoreCopy = true, PrintRate = false, PrintProcess = false, PrintTaxDetail = false;
                StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
                PrintRate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintRate"].ToString());
                PrintProcess = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintProcess"].ToString());
                if (StoreCopy)
                    CheckStoreCopy = StoreCopy;
                pending = Convert.ToDouble(dsPending.Tables[0].Rows[0]["DuePayment"].ToString());

                double wholeAmt = 0, disWhole = 0, WhAmt = 0, bal = 0;
                int totalItem = 0, RowCount = 0;
                RowCount = dsMain.Tables[1].Rows.Count;
                totalItem = Convert.ToInt32(dsMain.Tables[0].Rows[0]["TotalQty"].ToString());
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    //string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();

                    //string[] rate1 = rate.Split(';');
                    //for (int j = 0; j < rate1.Length; j++)
                    //{
                    //    string[] tempRate = rate1[j].Split('@');
                    //    wholeAmt += Convert.ToDouble((Convert.ToDouble(tempRate[0].ToString()) * Convert.ToDouble(tempRate[1].ToString())));
                    //    WhAmt = wholeAmt;
                    //}

                    //if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                    //{
                    //    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
                    //    WhAmt = wholeAmt;
                    //    //totalItem += Convert.ToInt32(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString());
                    //}
                    //if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                    //{
                    //    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString());
                    //    WhAmt = wholeAmt;
                    //    //totalItem += Convert.ToInt32(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString());
                    //}
                }
                double discount = 0, serviceTax = 0;
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                }
                //discount = Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString());
                disWhole = Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString());
                double totalWholeAmt = 0;
                double dis = 0;
                if (disWhole > 0)
                {
                    totalWholeAmt = wholeAmt;
                    // dis = Math.Round(disWhole * discount / 100, 5);
                    totalWholeAmt -= disWhole;
                    totalWholeAmt = Math.Round(Convert.ToDouble(totalWholeAmt) + Convert.ToDouble(serviceTax), 2);
                }
                else
                {
                    totalWholeAmt = wholeAmt + serviceTax;
                }
                bool Flag = false, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintDueDate = false, PrintSubItems = false;
                string logoLeftRight = string.Empty;
                PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
                prePrintedOrBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
                headerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                footerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
                termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                PrintSubItems = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintSubItem"].ToString());
                PrintTaxDetail = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTaxDetail"].ToString());
                if (dsMain.Tables[0].Rows[0]["TaxTypes"].ToString() == "EXCLUSIVE")
                {
                    St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                }
                else
                {
                    St = false;
                }
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
                logoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
                logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
                tableBorder = Convert.ToBoolean(ds.Tables[0].Rows[0]["TableBorder"].ToString());
                // First Section
                int invoice = 1;
                strPreview2 = "<table style='font-size:12px;font-weight:bold'><tr><td>Dear Customer ,</td></tr>";
                strPreview2 += "<tr><td>Thank you for your association with " + ds.Tables[0].Rows[0]["Headertext"].ToString() + ".We are pleased to provide you details around your recent transaction with us.</td></tr>";
                strPreview2 += "<tr><td>Invoice No :" + dsMain.Tables[0].Rows[0]["BookingNumber"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Customer Name :" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Address :" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Phone No. :" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Booking Date :" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td></tr>";
            START:
                if (prePrintedOrBanner)
                {
                    if (Convert.ToInt32(hdnId.Value) > 8)
                    {
                        if (StoreCopy)
                            strPreview4 += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";

                        strPreview3 += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";
                        height = true;
                    }
                    if (tableBorder)
                    {
                        if (StoreCopy)
                            strPreview4 += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";

                        strPreview3 += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview4 += "<table class='TableData' style='width:7.9in;'><tr><td>";

                        strPreview3 += "<table class='TableData' style='width:7.9in;'><tr><td>";
                    }
                    if (StoreCopy)
                        strPreview4 += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";

                    strPreview3 += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";
                    if (logoLeftRight == "1")
                    {
                        Flag = false;
                        if (logoOnReceipt)
                        {
                            if (StoreCopy)
                                strPreview4 += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";

                            strPreview3 += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";
                            if (StoreCopy)
                                strPreview4 += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview3 += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview4 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";

                            strPreview3 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview3 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview4 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";

                            strPreview3 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                    }
                    else
                    {
                        if (logoOnReceipt)
                        {
                            if (StoreCopy)
                                strPreview4 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview3 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview4 += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td></tr></table></td></tr>";

                            strPreview3 += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview3 += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview4 += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";

                            strPreview3 += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";
                        }
                    }
                }
                else
                {
                    Flag = true;

                    if (Convert.ToInt32(hdnId.Value) > 8)
                    {
                        if (StoreCopy)
                            strPreview4 += "<table style='width:7.9in;height:5.12in'><tr><td></td></tr></table>";

                        strPreview3 += "<table style='width:7.9in;height:5.12in'><tr><td></td></tr></table>";
                        height = true;

                        if (StoreCopy)
                            strPreview4 += "<table style='width:7.9in;height:1.3in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                        strPreview3 += "<table style='width:7.9in;height:1.3in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (RowCount > 9)
                        {
                            if (StoreCopy)
                                strPreview4 += "<table style='width:7.9in;height:1.3in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                            strPreview3 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<table style='width:7.9in;height:1.3in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                            strPreview3 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
                        }
                    }
                }
                //

                // Second Section
                if (!Flag)
                {
                    if (StoreCopy)
                        strPreview4 += "<tr><td><table style='width:7.9in;'><tr>";

                    strPreview3 += "<tr><td><table style='width:7.9in;'><tr>";
                }
                else
                {
                    if (tableBorder)
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr><td><table style='width:7.9in;border: thin solid #000000;'><tr>";

                        strPreview3 += "<tr><td><table style='width:7.9in;border: thin solid #000000;'><tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr><td><table style='width:7.9in;'><tr>";

                        strPreview3 += "<tr><td><table style='width:7.9in;'><tr>";
                    }
                }
                //if (tableBorder)
                if (dsMain.Tables[1].Rows.Count >= 10)
                {
                    if (StoreCopy)
                        strPreview4 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                    strPreview3 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                    invoice++;
                }
                else
                {
                    if (StoreCopy)
                        strPreview4 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";

                    strPreview3 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                }

                if (barcode)
                {
                    if (StoreCopy)
                        strPreview4 += "<td align='right' style='width:3.0in;font-family: c39hrp24dhtt;font-size:32px'  valign='top'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td>";

                    strPreview3 += "<td align='right' style='width:3.0in;font-family: c39hrp24dhtt;font-size:32px'  valign='top'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview4 += "<td style='width:3.0in'>.</td>";

                    strPreview3 += "<td style='width:3.0in'>.</td>";
                }
                if (StoreCopy)
                    strPreview4 += "<td style='width:3.0in;font-size:11px' align='center'><table><tr><td align='center'  valign='top' style='width:2.0in'><table><tr><td style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "-" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + " " + "</td></tr><tr><td align='center'  valign='top' style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                strPreview3 += "<td style='width:3.0in;font-size:11px' align='center'><table><tr><td align='center'  valign='top' style='width:2.0in'><table><tr><td style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "-" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + " " + "</td></tr><tr><td align='center'  valign='top' style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                // Third Section

                if (headerBanner)
                {
                    if (StoreCopy)
                        strPreview4 += "<tr><td colspan='4' align='center'><table style='width:7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";

                    strPreview3 += "<tr><td colspan='4' align='center'><table style='width:7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
                }
                // Forth Section
                string amtFormat = BAL.BALFactory.Instance.BAL_New_Bookings.DisplayAmountFormat(Globals.BranchID);
                double Advance1 = 0;
                if (previousDue)
                {
                    if (Convert.ToInt32(hdnId.Value) < 11)
                    {
                        balance = 0;
                        try
                        {
                            for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
                                Advance1 += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                balance = (((pending + totalWholeAmt) - Advance1));
                            }
                            else
                            {
                                balance = Math.Ceiling(((pending + totalWholeAmt) - Advance1));
                            }
                            bal = balance;

                            advance = Advance1.ToString();
                            ad = Advance1.ToString();
                        }
                        catch (Exception ex)
                        {
                            balance = ((pending + totalWholeAmt) - Convert.ToDouble("0"));
                            bal = balance;
                        }
                    }
                    //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    //{
                    //    if (StoreCopy)
                    //        strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center'>Previous Due</td></td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center'>Previous Due</td></td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    //else
                    //{
                    //    if (StoreCopy)
                    //        strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center'>Previous Due</td><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center'>Previous Due</td><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 0)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 0)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32((totalWholeAmt - Advance1)) + "</td></tr>";
                    pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
                }
                else
                {
                    if (Convert.ToInt32(hdnId.Value) < 11)
                    {
                        balance = 0;
                        try
                        {
                            for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
                                Advance1 += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                balance = (((totalWholeAmt) - Advance1));
                            }
                            else
                            {
                                balance = Math.Ceiling((((totalWholeAmt) - Advance1)));
                            }
                            bal = balance;
                            advance = Advance1.ToString();
                            ad = Advance1.ToString();
                        }
                        catch (Exception ex)
                        {
                            balance = totalWholeAmt - Convert.ToDouble("0");
                            bal = balance;
                        }
                    }
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 0)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 0)) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 0)) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    //{
                    //    if (StoreCopy)
                    //        strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    //else
                    //{
                    //    if (StoreCopy)
                    //        strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32(Math.Round(bal, 0)) + "</td></tr>";
                    pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
                }
                strPreview2 += "<tr><td>Due Delivery Date :" + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                // Fifth Section

                if (PrintDueDate)
                {
                    if (StoreCopy)
                        strPreview4 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:12px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                    strPreview3 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:12px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";
                    strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview3 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='right' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'></td></tr></table></td></tr></table></td></tr>";

                    strPreview3 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='right' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'</td><td style='font-size:12px'></td></tr></table></td></tr></table></td></tr>";
                    strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";
                }
                // Six Section
                strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                if (PrintSubItems)
                {
                    strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px' nowrap='nowrap'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                    //strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (StoreCopy)
                        strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px' nowrap='nowrap'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                }
                else
                {
                    strPreview3 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                    //strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (StoreCopy)
                        strPreview4 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:12px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
                }
                strPreview2 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:12px'>S.No.</td><td style='width:0.5in;font-weight: bold;font-size:12px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:12px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:12px' align='right'>Amount</td></tr>";
                strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                GetTempSave(BookNumber, totalItem);
                int incRow = 1;
                if (Convert.ToInt32(hdnId.Value) < 9)
                {
                    balance = 0;
                    advance = "0";
                    wholeAmt = 0;
                }
                int checkCondition = 0;
                if (Convert.ToInt32(hdnId.Value) < 9)
                    checkCondition = (dsMain.Tables[1].Rows.Count > 9 ? 9 : dsMain.Tables[1].Rows.Count);
                else
                {
                    checkCondition = Convert.ToInt32(hdnId.Value) + 9;
                    if (checkCondition >= dsMain.Tables[1].Rows.Count)
                    {
                        checkCondition = dsMain.Tables[1].Rows.Count - Convert.ToInt32(hdnId.Value);
                        checkCondition = Convert.ToInt32(hdnId.Value) + checkCondition;
                    }
                }
                for (int i = (Convert.ToInt32(hdnId.Value)); i < checkCondition; i++)
                {
                    string ItemName = string.Empty, Color = string.Empty, Remarks = string.Empty, Process = string.Empty, ExProcess1 = string.Empty, ExProcess2 = string.Empty, Rate = string.Empty, Rate1 = string.Empty, Rate2 = string.Empty, UnitDesc = string.Empty;
                    string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
                    string[] rate1 = rate.Split(';');
                    string[] rate2 = rate.Split('@');
                    Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
                    UnitDesc = dsMain.Tables[1].Rows[i]["UnitDesc"].ToString();
                    Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
                    string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["StatusName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookNumber, Globals.BranchID);
                    string[] DeliverOrUnDeliver = ItemStatus.Split('/');
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
                    if (PrintProcess)
                    {
                        Process = dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString();
                        ExProcess1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString();
                        ExProcess2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString();
                    }
                    else
                    {
                        Process = "";
                        ExProcess1 = "";
                        ExProcess2 = "";
                    }
                    if (PrintRate)
                    {
                        Rate = string.Format("{0:0.00}", rate2[1].ToString());
                        Rate1 = string.Format("{0:0.00}", dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
                        Rate2 = string.Format("{0:0.00}", dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString());
                    }
                    else
                    {
                        Rate = "";
                        Rate1 = "";
                        Rate2 = "";
                    }
                    if (PrintProcess == true && PrintRate == true)
                    {
                        Process = Process + "@";
                        ExProcess1 = ExProcess1 + "@";
                        ExProcess2 = ExProcess2 + "@";
                    }
                    if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                    {
                        if (PrintSubItems)
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                            strPreview3 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                            strPreview3 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                    }
                    else
                    {
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                        {
                            if (PrintSubItems)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                strPreview3 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                strPreview3 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                            {
                                if (PrintSubItems)
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview3 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview3 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (PrintSubItems)
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview3 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + " " + Process + Rate + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";

                                    strPreview3 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + Process + Rate + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + " " + Process + Rate + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                        }
                    }
                    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());

                    incRow++;
                    hdnId.Value = (Convert.ToInt32(hdnId.Value) + 1).ToString();
                }

                if ((Convert.ToInt32(hdnId.Value) % 9) != 0)
                {
                    int rev = 0;
                    int mod = checkCondition % 9;
                    //rev = (rev * 8) + mod;
                    //checkCondition = checkCondition / 8;

                    for (int i = 2; i <= (9 - mod); i++)
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";

                        strPreview3 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                        strPreview2 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                    }
                }

                // Seventh Section

                if (Convert.ToInt32(hdnId.Value) < 9)
                {
                    if (termAndCondition)
                    {
                        if (PrintTaxDetail)
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp;<span style='font-weight: bold; font-size: 10px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(disWhole, 2) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Dis (" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()), 2)) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        //  strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview4 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                        strPreview3 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                        if (PrintTaxDetail)
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (St)
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                    }
                    if (StoreCopy)
                        strPreview4 += "</table></td></tr>";
                    strPreview3 += "</table></td></tr>";

                    strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    if (St)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(serviceTax, 2) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(disWhole, 2) + "</td></tr>";
                    }
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Advance</td><td align='right' style='font-size:12px;font-weight:bold'>" + ad + "</td></tr>";
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }
                    else
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }
                }
                else
                {
                    if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                    {
                        if (termAndCondition)
                        {
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy&nbsp; <span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td> <td style='font-weight: bold; border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;Terms and Conditions (R.O.A.C) &nbsp;<span style='font-weight: bold; font-size: 10px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td> <td style='font-weight: bold; border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td></tr>";
                            }
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size: 10px'><td></td><td align='right' style='font-size: 12px;></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td ></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td ></td><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            //  strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";

                            //if (StoreCopy)
                            //    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            //strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview3 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (St)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right'></td><td align='right'></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right'></td><td align='right'></td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                            strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                        }
                        if (StoreCopy)
                            strPreview4 += "</table></td></tr>";
                        strPreview3 += "</table></td></tr>";
                    }
                    else
                    {
                        if (termAndCondition)
                        {
                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy <span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp;<span style='font-weight: bold; font-size: 10px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            //  strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }

                            //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            //{
                            //    if (StoreCopy)
                            //        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2) + "</td></tr>";
                            //    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2) + "</td></tr>";
                            //}
                            //else
                            //{
                            //    if (StoreCopy)
                            //        strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            //    strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            //}
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview4 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview3 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                            if (PrintTaxDetail)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " )</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'><span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", wholeAmt) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (St)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview4 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview3 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        if (StoreCopy)
                            strPreview4 += "</table></td></tr>";
                        strPreview3 += "</table></td></tr>";
                    }

                    strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    if (St)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(serviceTax, 2) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + dis + "</td></tr>";
                    }
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Advance</td><td align='right' style='font-size:12px;font-weight:bold'>" + ad + "</td></tr>";
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }
                    else
                    {
                        strPreview2 += "<tr style='font-size:11px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }

                    if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                    {
                        hdnTemp.Value = "1";
                        if (StoreCopy)
                            strPreview4 += "</table>";
                        strPreview3 += "</table>";
                        if (StoreCopy)
                            strPreview4 += "</td></tr></table>";
                        strPreview3 += "</td></tr></table>";
                        goto START;
                    }
                }
                if (StoreCopy)
                    strPreview4 += "</table>";
                strPreview3 += "</table>";
                if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                {
                    if (StoreCopy)
                        strPreview4 += "</td></tr></table>";
                    strPreview3 += "</td></tr></table>";
                    goto START;
                }
                strPreview2 += "<tr><td nowrap='nowrap' colspan='3'>Assuring you of our best service.</td></tr>";
                strPreview2 += "<tr><td colspan='3'>Warm Regards.</td></tr>";
                strPreview2 += "<tr><td colspan='3'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</td></tr>";
                strPreview2 += "</table>";
            }
            catch (Exception ex) { }

            ViewState["Msg"] = strPreview3;
            ViewState["Msg1"] = strPreview2;
            hdnId.Value = "0";
            return strPreview3;
        }

        protected void btnGoForNewBooking_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchCustomer.aspx");
        }

        protected void btnGoForNewOrder_Click(object sender, EventArgs e)
        {
            //Response.Redirect("../New_Booking/frm_New_Booking.aspx");
            Response.Redirect("../New_Booking/frm_New_Booking.aspx");
        }

        protected void btnNextSlip_Click(object sender, EventArgs e)
        {
            string[] bncol = Request.QueryString["BN"].ToString().Split('/');
            string strNextNumber = (int.Parse(bncol[0]) + 1).ToString();
            Response.Redirect(strPageName + "?BN=" + strNextNumber.ToString());
        }

        protected void btnPreviousSlip_Click(object sender, EventArgs e)
        {
            string[] bncol = Request.QueryString["BN"].ToString().Split('/');
            string strPreNumber = (int.Parse(bncol[0]) - 1).ToString();
            Response.Redirect(strPageName + "?BN=" + strPreNumber.ToString());
        }

        private string invoice = string.Empty;
        private string sqlMain = string.Empty;

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //    try
            //    {
            //        string reportType = "PDF";
            //        string mimeType;
            //        string encoding;
            //        string fileNameExtension;
            //        string deviceInfo =
            //   "<DeviceInfo>" +
            //"  <OutputFormat>PDF</OutputFormat>" +
            //"  <PageWidth>3in</PageWidth>" +
            //"  <PageHeight>6in</PageHeight>" +
            //"  <MarginTop>0.0in</MarginTop>" +
            //"  <MarginLeft>0.0in</MarginLeft>" +
            //"  <MarginRight>0.0in</MarginRight>" +
            //"  <MarginBottom>0.0in</MarginBottom>" +
            //"</DeviceInfo>";
            //        Warning[] warnings;
            //        string[] streams;
            //        byte[] renderedBytes;
            //        renderedBytes = ReportViewer1.LocalReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.BinaryWrite(renderedBytes);
            //        FileStream fs = System.IO.File.Create("c:\\" + "Report" + ".pdf");
            //        fs.Write(renderedBytes, 0, renderedBytes.Length);
            //        fs.Close();
            //        Process p = new Process();
            //        p.StartInfo.FileName = "C:\\Report.pdf";
            //        p.StartInfo.Verb = "Print";
            //        p.StartInfo.CreateNoWindow = true;
            //        p.StartInfo.UseShellExecute = true;
            //        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //        Process.Start(p.StartInfo);
            //        Response.End();
            //    }
            //    catch (Exception)
            //    {
            //    }
        }

        protected void GetTempSave(string BookingNumber, int totalqty)
        {
            SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
            SqlTransaction stx = null;
            try
            {
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                sqlMain = "UPDATE EntBookings SET Qty='" + totalqty + "'";
                sqlMain += " WHERE BookingNumber='" + BookingNumber + "' AND BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sqlMain;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (stx != null) stx.Rollback();
            }
        }

        protected int ReturnQty(string BookingNumber)
        {
            SqlCommand cmd = new SqlCommand();
            int qty = 0;
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNo", BookingNumber);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                qty = Convert.ToInt32(ds.Tables[0].Rows[0]["ItemTotalQuantity"].ToString());
            }
            return qty;
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            bool SSL = false;
            SqlCommand cmd = new SqlCommand();
            string eMail = string.Empty;
            DataSet ds = new DataSet(); SqlDataReader sdr = null;
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
                    string mailBody = ViewState["Msg1"].ToString();
                    SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                    bool IsMailed = BasePage.SendMail(FEmail, "Booking slip of your clothes", mailBody, true, "Booking Slip.pdf", ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL);
                    strPreview = ViewState["Msg"].ToString();
                    if (IsMailed)
                        lblMsg.Text = "Email send successfully..";
                    else
                        lblMsg.Text = "Email not send..";
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

        protected void btnGoHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.html", false);
        }

        public string GetBookingDetailsForBookingNumberPackageDetail(string BookNumber, string date, bool isComparing = false)
        {
            try
            {
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
                    cmd.Parameters.Add(new SqlParameter("@BookingBackUpId", BookNumber));
                }
                else
                {
                    cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
                    cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookNumber));
                    cmd.Parameters.Add(new SqlParameter("@BookingPrefix", Request.QueryString["BookingPrefix"].ToString()));
                }

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
                dsMain = AppClass.GetData(cmd);

                DataSet dsPending = new DataSet();
                cmd = new SqlCommand();
                cmd.CommandText = "Sp_Sel_BookingPrevoiusDue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CustCode", dsMain.Tables[0].Rows[0]["CustomerCode"].ToString()));
                // if date is null, that would be if we passed from compare, we need to find the due date, that is passed to this date
                if (date == null)
                {
                    var cmdDate = new SqlCommand();
                    cmdDate.CommandText = "SELECT convert(varchar, BookingDeliveryDate, 106) from EntBookings where BookingNumber = " + dsMain.Tables[0].Rows[0]["BookingNumber"].ToString()
                        + " AND BranchId = " + Globals.BranchID + "";
                    date = PrjClass.ExecuteScalar(cmdDate);
                }
                cmd.Parameters.Add(new SqlParameter("@Date", date.ToString()));
                cmd.Parameters.Add(new SqlParameter("@BN", BookNumber));
                cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));

                dsPending = AppClass.GetData(cmd);
                if (hdnEmailId != null)
                    hdnEmailId.Value = BookNumber;
                double balance = 0;
                string advance = "0", ad = "0", pend = "";
                double pending = 0;
                bool StoreCopy = true;
                StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
                if (StoreCopy)
                    CheckStoreCopy = StoreCopy;
                pending = Convert.ToDouble(dsPending.Tables[0].Rows[0]["DuePayment"].ToString());

                double wholeAmt = 0, disWhole = 0, WhAmt = 0, bal = 0;
                int totalItem = 0;
                totalItem = Convert.ToInt32(dsMain.Tables[0].Rows[0]["TotalQty"].ToString());
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    //string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();

                    //string[] rate1 = rate.Split(';');
                    //for (int j = 0; j < rate1.Length; j++)
                    //{
                    //    string[] tempRate = rate1[j].Split('@');
                    //    wholeAmt += Convert.ToDouble((Convert.ToDouble(tempRate[0].ToString()) * Convert.ToDouble(tempRate[1].ToString())));
                    //    WhAmt = wholeAmt;
                    //}

                    //if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                    //{
                    //    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
                    //    WhAmt = wholeAmt;
                    //    //totalItem += Convert.ToInt32(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString());
                    //}
                    //if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                    //{
                    //    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString());
                    //    WhAmt = wholeAmt;
                    //    //totalItem += Convert.ToInt32(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString());
                    //}
                }
                double discount = 0, serviceTax = 0;
                for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                {
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                    serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                }
                //discount = Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString());
                disWhole = Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString());
                double totalWholeAmt = 0;
                double dis = 0;
                if (disWhole > 0)
                {
                    totalWholeAmt = wholeAmt;
                    // dis = Math.Round(disWhole * discount / 100, 5);
                    totalWholeAmt -= disWhole;
                    totalWholeAmt = Math.Round(Convert.ToDouble(totalWholeAmt) + Convert.ToDouble(serviceTax), 2);
                }
                else
                {
                    totalWholeAmt = wholeAmt + serviceTax;
                }
                bool Flag = false, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintDueDate = false, PrintSubItems = false;
                string logoLeftRight = string.Empty;
                if (hdnId == null)
                {
                    hdnId = new HiddenField();
                    hdnId.Value = "0";
                }
                if (hdnTemp == null)
                {
                    hdnTemp = new HiddenField();
                    hdnTemp.Value = "0";
                }
                PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
                prePrintedOrBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
                headerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                footerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
                termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                PrintSubItems = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintSubItem"].ToString());
                if (dsMain.Tables[0].Rows[0]["TaxTypes"].ToString() == "EXCLUSIVE")
                {
                    St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                }
                else
                {
                    St = false;
                }
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
                logoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
                logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
                tableBorder = Convert.ToBoolean(ds.Tables[0].Rows[0]["TableBorder"].ToString());
                // First Section
                int invoice = 1;
                strPreview2 = "<table style='font-size:12px;font-weight:bold'><tr><td>Dear Customer ,</td></tr>";
                strPreview2 += "<tr><td>Thank you for your association with " + ds.Tables[0].Rows[0]["Headertext"].ToString() + ".We are pleased to provide you details around your recent transaction with us.</td></tr>";
                strPreview2 += "<tr><td>Invoice No :" + dsMain.Tables[0].Rows[0]["BookingNumber"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Customer Name :" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Address :" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Phone No. :" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + "</td></tr>";
                strPreview2 += "<tr><td>Booking Date :" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td></tr>";
            START:
                if (prePrintedOrBanner)
                {
                    if (Convert.ToInt32(hdnId.Value) > 8)
                    {
                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";

                        strPreview += "<table style='width:7.9in;height:5.2in'><tr><td></td><tr></table>";
                        height = true;
                    }
                    if (tableBorder)
                    {
                        if (StoreCopy)
                            strPreview1 += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";

                        strPreview += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<table class='TableData' style='width:7.9in;'><tr><td>";

                        strPreview += "<table class='TableData' style='width:7.9in;'><tr><td>";
                    }
                    if (StoreCopy)
                        strPreview1 += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";

                    strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr>";
                    if (logoLeftRight == "1")
                    {
                        Flag = false;
                        if (logoOnReceipt)
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";

                            strPreview += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";
                            if (StoreCopy)
                                strPreview1 += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                        }
                    }
                    else
                    {
                        if (logoOnReceipt)
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><table><tr><td align='center'> <span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' width='110px' height='96px' /></td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";

                            strPreview += "<td style='width: 1.5in'>.</td><td align='center' style='width: 5.0in;'><table><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr><tr><td align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr></table>";
                            if (StoreCopy)
                                strPreview1 += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";

                            strPreview += "</td><td style='width: 1.5in'></td></tr></table></td></tr>";
                        }
                    }
                }
                else
                {
                    Flag = true;

                    if (Convert.ToInt32(hdnId.Value) > 8)
                    {
                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:5.12in'><tr><td></td><tr></table>";

                        strPreview += "<table style='width:7.9in;height:5.11in'><tr><td></td><tr></table>";
                        height = true;

                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                        strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                        strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
                    }
                }
                //

                // Second Section
                if (!Flag)
                {
                    if (StoreCopy)
                        strPreview1 += "<tr><td><table style='width:7.9in;'><tr>";

                    strPreview += "<tr><td><table style='width:7.9in;'><tr>";
                }
                else
                {
                    if (tableBorder)
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr><td><table style='width:7.9in;border: thin solid #000000;'><tr>";

                        strPreview += "<tr><td><table style='width:7.9in;border: thin solid #000000;'><tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr><td><table style='width:7.9in;'><tr>";

                        strPreview += "<tr><td><table style='width:7.9in;'><tr>";
                    }
                }
                //if (tableBorder)
                if (dsMain.Tables[1].Rows.Count >= 10)
                {
                    if (StoreCopy)
                        strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                    strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                    invoice++;
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";

                    strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + dsMain.Tables[0].Rows[0]["BookingTime"].ToString() + "</td></tr></table></td>";
                }

                if (barcode)
                {
                    if (StoreCopy)
                        strPreview1 += "<td align='right' style='width:3.0in;font-family: c39hrp24dhtt;font-size:32px'  valign='top'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td>";

                    strPreview += "<td align='right' style='width:3.0in;font-family: c39hrp24dhtt;font-size:32px'  valign='top'>" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<td style='width:3.0in'>.</td>";

                    strPreview += "<td style='width:3.0in'>.</td>";
                }
                if (StoreCopy)
                    strPreview1 += "<td style='width:3.0in;font-size:11px' align='center'><table><tr><td align='center'  valign='top' style='width:2.0in'><table><tr><td style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "-" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + " " + "</td></tr><tr><td align='center'  valign='top' style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                strPreview += "<td style='width:3.0in;font-size:11px' align='center'><table><tr><td align='center'  valign='top' style='width:2.0in'><table><tr><td style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "-" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + " " + "</td></tr><tr><td align='center'  valign='top' style='font-family:" + ds.Tables[0].Rows[0]["NAFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NAFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NAB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NAU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NAI"].ToString() + "'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                // Third Section

                if (headerBanner)
                {
                    if (StoreCopy)
                        strPreview1 += "<tr><td colspan='4' align='center'><table style='width:7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";

                    strPreview += "<tr><td colspan='4' align='center'><table style='width:7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
                }
                // Forth Section
                string amtFormat = BAL.BALFactory.Instance.BAL_New_Bookings.DisplayAmountFormat(Globals.BranchID);
                double Advance1 = 0;
                if (previousDue)
                {
                    if (Convert.ToInt32(hdnId.Value) < 11)
                    {
                        balance = 0;
                        try
                        {
                            for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
                                Advance1 += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());

                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                balance = (((pending + totalWholeAmt) - Advance1));
                            }
                            else
                            {
                                balance = Math.Ceiling(((pending + totalWholeAmt) - Advance1));
                            }
                            bal = balance;

                            advance = Advance1.ToString();
                            ad = Advance1.ToString();
                        }
                        catch (Exception ex)
                        {
                            balance = ((pending + totalWholeAmt) - Convert.ToDouble("0"));
                            bal = balance;
                        }
                    }
                    //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    //{
                    //    if (StoreCopy)
                    //        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center'>Previous Due</td></td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center'>Previous Due</td></td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    //else
                    //{
                    //    if (StoreCopy)
                    //        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center'>Previous Due</td><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                    //    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center'>Previous Due</td><td style='width:2.0in'>.</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 0) + "</td></tr><tr><td style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>-&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-size:9px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;'>=&nbsp;&nbsp;</td><td style='width:2.0in;font-size:12px'><table><tr><td style='font-weight:bold;font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    //}
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 2) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(totalWholeAmt, 2) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 2) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 2) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Ceiling(totalWholeAmt) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";

                        strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(pending, 0) + "</td></tr><tr><td align='center' style='font-size:12px'>Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Ceiling(totalWholeAmt) + "</td></tr><tr><td align='center' style='font-size:12px'>Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(Convert.ToDecimal(ad), 0) + "</td></tr><tr><td align='center' style='font-size:12px'>Advance</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight:bold;font-size:12px'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + Math.Round(bal, 0) + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    }
                    strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32((totalWholeAmt - Advance1)) + "</td></tr>";
                    pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
                }
                else
                {
                    if (Convert.ToInt32(hdnId.Value) < 11)
                    {
                        balance = 0;
                        try
                        {
                            for (int i = 0; i < dsMain.Tables[2].Rows.Count; i++)
                                Advance1 += Convert.ToDouble(dsMain.Tables[2].Rows[i]["Payment"].ToString());
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                balance = (((totalWholeAmt) - Advance1));
                            }
                            else
                            {
                                balance = Math.Ceiling((((totalWholeAmt) - Advance1)));
                            }

                            bal = balance;
                            advance = Advance1.ToString();
                            ad = Advance1.ToString();
                        }
                        catch (Exception ex)
                        {
                            balance = totalWholeAmt - Convert.ToDouble("0");
                            bal = balance;
                        }
                    }

                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Package Name</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'></td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Expiry Date</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'></td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Membership Id</td></tr></table></td></tr></table></td></tr>";

                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Package Name</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'></td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Expiry Date</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'></td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Membership Id</td></tr></table></td></tr></table></td></tr>";

                    strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32(Math.Round(bal, 0)) + "</td></tr>";
                    pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
                }
                strPreview2 += "<tr><td>Due Delivery Date :" + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                // Fifth Section
                if (PrintDueDate)
                {
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:12px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";

                    strPreview += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:12px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";
                    strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='right' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'></td></tr></table></td></tr></table></td></tr>";

                    strPreview += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='right' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'</td><td style='font-size:12px'></td></tr></table></td></tr></table></td></tr>";
                    strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";
                }
                // Six Section
                if (PrintSubItems)
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px' nowrap='nowrap'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'></td></tr>";
                    strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px' nowrap='nowrap'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'></td></tr>";
                }
                else
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'></td></tr>";
                    strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (StoreCopy)
                        strPreview1 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:12px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'></td></tr>";
                }

                strPreview2 += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:12px'>S.No.</td><td style='width:0.5in;font-weight: bold;font-size:12px'>Qty</td><td style='width:5.0in;font-weight: bold;font-size:12px' align='left'>Particular's</td><td style='width:1.0in;font-weight: bold' align='right'></td><td style='width:1.0in;font-weight: bold;font-size:12px' align='right'>Amount</td></tr>";
                strPreview2 += "<tr style='font-size:9px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                GetTempSave(BookNumber, totalItem);
                int incRow = 1;
                if (Convert.ToInt32(hdnId.Value) < 9)
                {
                    balance = 0;
                    advance = "0";
                    wholeAmt = 0;
                }
                int checkCondition = 0;
                if (Convert.ToInt32(hdnId.Value) < 9)
                    checkCondition = (dsMain.Tables[1].Rows.Count > 9 ? 9 : dsMain.Tables[1].Rows.Count);
                else
                {
                    checkCondition = Convert.ToInt32(hdnId.Value) + 9;
                    if (checkCondition >= dsMain.Tables[1].Rows.Count)
                    {
                        checkCondition = dsMain.Tables[1].Rows.Count - Convert.ToInt32(hdnId.Value);
                        checkCondition = Convert.ToInt32(hdnId.Value) + checkCondition;
                    }
                }
                for (int i = (Convert.ToInt32(hdnId.Value)); i < checkCondition; i++)
                {
                    string ItemName = string.Empty, Color = string.Empty, Remarks = string.Empty, UnitDesc = string.Empty;
                    string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
                    string[] rate1 = rate.Split(';');
                    string[] rate2 = rate.Split('@');
                    Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
                    UnitDesc = dsMain.Tables[1].Rows[i]["UnitDesc"].ToString();
                    Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
                    string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["StatusName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookNumber, Globals.BranchID);
                    string[] DeliverOrUnDeliver = ItemStatus.Split('/');
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
                    if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                    {
                        if (PrintSubItems)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                            strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                            strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                            strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                            strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        }
                    }
                    else
                    {
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                        {
                            if (PrintSubItems)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                                strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                                strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                                strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                                strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                            {
                                if (PrintSubItems)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                            else
                            {
                                if (PrintSubItems)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";

                                    strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'></td></tr>";
                                    strPreview2 += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "</td><td align='right'></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                            }
                        }
                    }
                    wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());

                    incRow++;
                    hdnId.Value = (Convert.ToInt32(hdnId.Value) + 1).ToString();
                }

                if ((Convert.ToInt32(hdnId.Value) % 9) != 0)
                {
                    int rev = 0;
                    int mod = checkCondition % 9;
                    //rev = (rev * 8) + mod;
                    //checkCondition = checkCondition / 8;

                    for (int i = 2; i <= (9 - mod); i++)
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";

                        strPreview += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                        strPreview2 += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                    }
                }

                // Seventh Section

                if (Convert.ToInt32(hdnId.Value) < 9)
                {
                    if (termAndCondition)
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'></td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'></td><td align='right' style='font-size:12px;font-weight:bold'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                    }
                    else
                    {
                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                        strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                        if (StoreCopy)
                            strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: medium; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        if (St)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                    }
                    if (StoreCopy)
                        strPreview1 += "</table></td></tr>";
                    strPreview += "</table></td></tr>";

                    strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    if (St)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(serviceTax, 2) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(disWhole, 2) + "</td></tr>";
                    }
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Advance</td><td align='right' style='font-size:12px;font-weight:bold'>" + ad + "</td></tr>";
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }
                    else
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }
                }
                else
                {
                    if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                    {
                        if (termAndCondition)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td> <td style='font-weight: bold; border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td></tr>";

                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size: 10px'><td ></td><td align='right'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td align='right' style='font-size:12px;font-weight:bold'></td><td>Continue ...</td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                                }
                            }
                            //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";

                            //if (StoreCopy)
                            //    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            //strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'><span style='font-size: medium; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            if (St)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right'></td><td align='right'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right'></td><td align='right'></td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                        }
                        if (StoreCopy)
                            strPreview1 += "</table></td></tr>";
                        strPreview += "</table></td></tr>";
                    }
                    else
                    {
                        if (termAndCondition)
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size: 10px'><td align='center' colspan='3' style='font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000; font-size: 25Px; color: Gray'rowspan='4'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'></td></tr>";
                            if (St)
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td ></td><td></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                                }
                            }
                            //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }
                            else
                            {
                                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                                else
                                {
                                    if (StoreCopy)
                                        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                    strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                }
                            }

                            //if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            //{
                            //    if (StoreCopy)
                            //        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2) + "</td></tr>";
                            //    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2) + "</td></tr>";
                            //}
                            //else
                            //{
                            //    if (StoreCopy)
                            //        strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            //    strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            //}
                        }
                        else
                        {
                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                            strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                            if (StoreCopy)
                                strPreview1 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td align='center' colspan='3' rowspan='4' style='font-size: 25px;color:Gray'>Store Copy &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: small; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'><span style='font-size: medium; color: #000000;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            if (St)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                if (StoreCopy)
                                    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        if (StoreCopy)
                            strPreview1 += "</table></td></tr>";
                        strPreview += "</table></td></tr>";
                    }

                    strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + wholeAmt + "</td></tr>";
                    if (St)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(serviceTax, 2) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Dis</td><td align='right' style='font-size:12px;font-weight:bold'>" + dis + "</td></tr>";
                    }
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Advance</td><td align='right' style='font-size:12px;font-weight:bold'>" + ad + "</td></tr>";
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }
                    else
                    {
                        strPreview2 += "<tr style='font-size:11px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + pend + "</td></tr>";
                    }

                    if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                    {
                        hdnTemp.Value = "1";
                        if (StoreCopy)
                            strPreview1 += "</table>";
                        strPreview += "</table>";
                        if (StoreCopy)
                            strPreview1 += "</td></tr></table>";
                        strPreview += "</td></tr></table>";
                        goto START;
                    }
                }
                if (StoreCopy)
                    strPreview1 += "</table></td></tr></table>";
                strPreview += "</table></td></tr></table>";
                if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
                {
                    if (StoreCopy)
                        strPreview1 += "</td></tr></table>";
                    strPreview += "</td></tr></table>";
                    goto START;
                }
                strPreview2 += "<tr><td nowrap='nowrap' colspan='3'>Assuring you of our best service.</td></tr>";
                strPreview2 += "<tr><td colspan='3'>Warm Regards.</td></tr>";
                strPreview2 += "<tr><td colspan='3'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</td></tr>";
                strPreview2 += "</table>";
            }
            catch (Exception ex) { }
            string MyHiddenValue = "";
            MyHiddenValue = Globals.StorePrint;
            if (MyHiddenValue != "ST_COPY")
            {
                ViewState["Msg"] = strPreview;
                ViewState["Msg1"] = strPreview2;
                hdnId.Value = "0";
                strFinal = strPreview;
            }
            else if (MyHiddenValue == "ST_COPY")
            {
                ViewState["Msg"] = strPreview1;
                ViewState["Msg1"] = strPreview2;
                hdnId.Value = "0";
                strFinal = strPreview1;
            }
            strPreview3 = strPreview;
            strPreview4 = strPreview1;
            return strFinal;
        }


    }
}