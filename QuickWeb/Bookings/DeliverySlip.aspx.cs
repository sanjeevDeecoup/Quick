using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

public partial class Booking_DeliverySlip : System.Web.UI.Page
{
    protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
    protected string strPageName = "~/new_booking/DeliverySlip.aspx", BranchDetails = string.Empty, BookingNumber = string.Empty, CustomerName = string.Empty, CustomerAddress = string.Empty, ReceiptDate = string.Empty, DueDate = string.Empty, DueTime = string.Empty, UrgentDelivery = string.Empty, TotalAmount = "0", DiscountRate = "0", RebateAmount = "0", NetAmount = "0", AdvPayment = "0", PreviousDue = "0", DuePayment = "0", DiscountOnPayment = "0", Remarks = string.Empty, strProcess = string.Empty;
    protected bool chkBookingNumber = false;
    protected bool height = false;
    public string strPreview = string.Empty;
    public string strPreview1 = string.Empty;
    public string strPreview2 = string.Empty;
    protected string Inch = string.Empty;
    private string sqlMain = string.Empty;
    private string _pkgType = string.Empty;
    private DataSet dschk = new DataSet();

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
                    GetBookingDetailsForBookingNumberPackage(data[0], Request.QueryString["RS"].ToString(), date);
                }
                else
                {
                    GetBookingDetailsForBookingNumber(data[0], Request.QueryString["RS"].ToString(), date);
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

    public string GetBookingDetailsForBookingNumber(string BookNumber, string amount, string date)
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
            double balance = 0;
            string advance = "0", ad = "0", pend = "";
            double pending = 0;
            bool StoreCopy = true, PrintTaxDetail = false;
            StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
            // if (StoreCopy)
            pending = BAL.BALFactory.Instance.Bal_Report.ReturnCustomerPendingBalanceBookingWise(dsMain.Tables[0].Rows[0]["CustomerCode"].ToString(), Globals.BranchID, BookNumber);

            double wholeAmt = 0, disWhole = 0, WhAmt = 0, bal = 0;
            int totalItem = 0;
            totalItem = Convert.ToInt32(dsMain.Tables[0].Rows[0]["TotalQty"].ToString());
            for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
            {
                wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
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
            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) != "True")
            {
                totalWholeAmt = Math.Ceiling(totalWholeAmt);
            }
            bool Flag = false,BookingTime=true, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintSubItems = false;
            string logoLeftRight = string.Empty, printBookingTime = string.Empty;
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
            BookingTime = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBookingTime"].ToString());
            if (BookingTime)
            {
                printBookingTime = dsMain.Tables[0].Rows[0]["BookingTime"].ToString();
            }
            else
            {
                printBookingTime = "";
            }
            // First Section
            int invoice = 1;
            strPreview2 = "<table style='font-size:12px;font-weight:bold'><tr><td>Dear Customer ,</td></tr>";
            strPreview2 += "<tr><td>Thank you for your association with " + ds.Tables[0].Rows[0]["Headertext"].ToString() + ".We are pleased to provide you details around your recent transation with us.</td></tr>";
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
                            strPreview1 += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";

                        strPreview += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";
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
                            strPreview1 += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td></tr></table></td></tr>";

                        strPreview += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td></tr></table></td></tr>";
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

                    strPreview += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                    strPreview += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
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
                    strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";
                strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";
                invoice++;
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";

                strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";
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
                strPreview += "<tr><td colspan='4' align='center'><table style='width: 7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
            }
            // Forth Section
            string amtFormat = BAL.BALFactory.Instance.BAL_New_Bookings.DisplayAmountFormat(Globals.BranchID);
            double Advance1 = 0, TotalAdvance = 0, DeliveryDiscount = 0;
            if (previousDue)
            {
                if (Convert.ToInt32(hdnId.Value) < 11)
                {
                    balance = 0;
                    try
                    {
                        for (int i = 0; i < dsMain.Tables[3].Rows.Count; i++)
                            Advance1 += Convert.ToDouble(dsMain.Tables[3].Rows[i]["Payment"].ToString());

                        for (int i = 0; i < dsMain.Tables[4].Rows.Count; i++)
                            TotalAdvance += Convert.ToDouble(dsMain.Tables[4].Rows[i]["Payment"].ToString());

                        for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                            DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());

                        balance = (((pending + totalWholeAmt) - TotalAdvance - DeliveryDiscount));
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
                if (DeliveryDiscount != 0)
                {
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        strPreview += "<tr style='font-size:11px'><td colspan='4'><table style='width: 7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td><td align='center' style='font-weight: bolder; font-size: 30px'>-</td><td style='width: 2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round((DeliveryDiscount), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Cash Discount</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    else
                        strPreview += "<tr style='font-size:11px'><td colspan='4'><table style='width: 7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(totalWholeAmt, 2))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(Convert.ToDecimal(ad), 0))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td> <td align='center' style='font-weight: bolder; font-size: 30px'>-</td><td style='width: 2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(DeliveryDiscount, 2))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Cash Discount</td></tr></table></td><td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                }
                else
                {
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        strPreview += "<tr style='font-size:11px'><td colspan='4'><table style='width: 7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    else
                        strPreview += "<tr style='font-size:11px'><td colspan='4'><table style='width: 7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(pending, 2)) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Previous Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>+</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(totalWholeAmt, 2))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(Convert.ToDecimal(ad), 0))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                }

                pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
            }
            else
            {
                if (Convert.ToInt32(hdnId.Value) < 11)
                {
                    balance = 0;
                    try
                    {
                        for (int i = 0; i < dsMain.Tables[3].Rows.Count; i++)
                            Advance1 += Convert.ToDouble(dsMain.Tables[3].Rows[i]["Payment"].ToString());

                        for (int i = 0; i < dsMain.Tables[4].Rows.Count; i++)
                            TotalAdvance += Convert.ToDouble(dsMain.Tables[4].Rows[i]["Payment"].ToString());

                        for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                            DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());

                        balance = (((totalWholeAmt) - TotalAdvance - DeliveryDiscount));
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
                if (DeliveryDiscount == 0)
                {
                    for (int i = 0; i < dsMain.Tables[5].Rows.Count; i++)
                        DeliveryDiscount += Convert.ToDouble(dsMain.Tables[5].Rows[i]["Payment"].ToString());
                }
                if (DeliveryDiscount != 0)
                {
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        strPreview += "<tr style='font-size: 11px'><td colspan='4'><table style='width: 7.9in; border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000; border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width: 2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td><td align='center' style='font-weight: bolder; font-size: 30px'>-</td><td style='width: 2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round((DeliveryDiscount), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Cash Discount</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    else
                        strPreview += "<tr style='font-size: 11px'><td colspan='4'><table style='width: 7.9in; border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000; border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width: 2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(totalWholeAmt, 2))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(Convert.ToDecimal(ad), 0))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td><td align='center' style='font-weight: bolder; font-size: 30px'>-</td><td style='width: 2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(DeliveryDiscount, 2))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Cash Discount</td></tr></table></td><td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                }
                else
                {
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        strPreview += "<tr style='font-size: 11px'><td colspan='4'><table style='width: 7.9in; border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000; border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width: 2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(totalWholeAmt, 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(ad), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(bal, 2)) + "</td></tr><tr<td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                    else
                        strPreview += "<tr style='font-size: 11px'><td colspan='4'><table style='width: 7.9in; border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000; border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width: 2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Ceiling(totalWholeAmt)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;Current Due</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td style='font-size:14Px' nowrap='nowrap'>&nbsp;&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(Convert.ToDecimal(ad), 0))) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Already Paid</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>-</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Math.Round(Convert.ToDouble((amount == "" ? "0" : amount)), 2)) + "</td></tr><tr><td style='font-size:14Px' nowrap='nowrap'>Current Payment</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'>=</td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-weight: bold; font-size:14Px' nowrap='nowrap'>&nbsp;" + amtFormat + "&nbsp;:&nbsp;" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round(bal, 2))) + "</td></tr><tr><td style='font-weight: bold; font-size:14Px' nowrap='nowrap'>Balance Due</td></tr></table></td></tr></table></td></tr>";
                }

                pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
            }
            strPreview2 += "<tr><td>Due Delivery Date :" + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";

            // Fifth Section

            strPreview += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:13px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";
            strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";

            // Six Section

            if (PrintSubItems)
            {
                strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width: 7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:4.25in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.75in;font-weight: bold;font-size:14px' align='center'>Status</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
            }
            else
            {
                strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width: 7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:4.25in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.75in;font-weight: bold;font-size:14px' align='center'>Status</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'>Amount</td></tr>";
            }

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
                string ItemName = string.Empty, Color = string.Empty, Remarks = string.Empty, Rate = string.Empty, Rate1 = string.Empty, Rate2 = string.Empty;
                bool checkstatus = false;
                string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
                string[] rate1 = rate.Split(';');
                string[] rate2 = rate.Split('@');
                Color = dsMain.Tables[1].Rows[i]["ItemColor"].ToString();
                Remarks = dsMain.Tables[1].Rows[i]["ItemRemark"].ToString();
                Rate = string.Format("{0:0.00}", Convert.ToDouble(rate2[1].ToString()));
                Rate1 = string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString()));
                Rate2 = string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString()));
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
                string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["ItemName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookNumber, Globals.BranchID);
                string[] DeliverOrUnDeliver = ItemStatus.Split('/');
                string ClothesStatus = string.Empty;
                if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) == Convert.ToInt32(DeliverOrUnDeliver[1].ToString()))
                {
                    ClothesStatus = "Delivered" + " [" + DeliverOrUnDeliver[3] + "]";
                    checkstatus = true;
                }
                else
                {
                    if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) > 0)
                    {
                        ClothesStatus = "Delivered" + "-" + DeliverOrUnDeliver[0];                        
                    }
                }
                if (ClothesStatus != "" && checkstatus != true)
                {
                    ClothesStatus = ClothesStatus + "/" + dsMain.Tables[1].Rows[i]["SubPieces"].ToString() + " [" + DeliverOrUnDeliver[3] + "]";
                }
                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                {
                    if (PrintSubItems)
                    {
                        strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + string.Format("{0:0.00}", Rate) + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + string.Format("{0:0.00}", Rate1) + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + string.Format("{0:0.00}", Rate2) + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + Rate1 + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + Rate2 + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                    }
                }
                else
                {
                    if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                    {
                        if (PrintSubItems)
                        {
                            strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + Rate1 + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + Rate1 + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                        }
                    }
                    else
                    {
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            if (PrintSubItems)
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + Rate2 + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + Rate2 + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                            }
                        }
                        else
                        {
                            if (PrintSubItems)
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + Rate + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'>" + string.Format("{0:0.00}", (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString()))) + "</td></tr>";
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
                    strPreview += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                }
            }

            // Seventh Section

            if (Convert.ToInt32(hdnId.Value) < 9)
            {
                if (termAndCondition)
                {
                    if (PrintTaxDetail)
                        strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                    else
                        strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                    if (St)
                    {
                        if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                        }
                    }
                    if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                    {
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Discount (" + string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString())) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'>Discount (" + string.Format("{0:0.00}", Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString())) + "%)</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                        }
                    }
                    //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                    if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                    {
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                        }
                    }
                    else
                    {
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Ceiling(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                        }
                    }
                }
                else
                {
                    strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                    if (PrintTaxDetail)
                        strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                    else
                        strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";

                    // strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                    if (St)
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(disWhole, 2)) + "</td></tr>";
                    }
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                    }
                }

                strPreview += "</table></td></tr>";

                strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 0) + "</td></tr>";
                strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                if (St)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                }
                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
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
                        strPreview += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td></tr>";

                        if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                        strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        if (St)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right'></td><td align='right'></td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }

                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                    }

                    strPreview += "</table></td></tr>";
                }
                else
                {
                    if (termAndCondition)
                    {
                        if (PrintTaxDetail)
                            strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span>&nbsp;<span style='font-weight: bold; font-size: 10px;'>( " + dsMain.Tables[7].Rows[0]["TaxName1"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["MainTax"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName2"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["Cess"].ToString()) + ", " + dsMain.Tables[7].Rows[0]["TaxName3"].ToString() + string.Format("{0:0.00}", dsMain.Tables[7].Rows[0]["HCess"].ToString()) + " = " + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + " )</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                        else
                            strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                        //strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>Subtotal</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                            }
                        }
                        else
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                        strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                        if (St)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", disWhole) + "</td></tr>";
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2)) + "</td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Current Due</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Convert.ToInt32(Math.Round((Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax)) - Convert.ToDouble(disWhole), 2))) + "</td></tr>";
                        }
                    }

                    strPreview += "</table></td></tr>";
                }

                strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 0) + "</td></tr>";
                strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(wholeAmt, 2)) + "</td></tr>";
                if (St)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2)) + "</td></tr>";
                }
                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + string.Format("{0:0.00}", dis) + "</td></tr>";
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

                    strPreview += "</table>";

                    strPreview += "</td></tr></table>";
                    goto START;
                }
            }

            strPreview += "</table>";
            if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
            {
                strPreview += "</td></tr></table>";
                goto START;
            }
            strPreview2 += "<tr><td nowrap='nowrap' colspan='3'>Assuring you of our best service.</td></tr>";
            strPreview2 += "<tr><td colspan='3'>Warm Regards.</td></tr>";
            strPreview2 += "<tr><td colspan='3'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</td></tr>";
            strPreview2 += "</table>";
        }
        catch (Exception ex) { }

        ViewState["Msg"] = strPreview;
        ViewState["Msg1"] = strPreview2;
        hdnId.Value = "0";
        return strPreview;
    }

    public string GetBookingDetailsForBookingNumberPackage(string BookNumber, string amount, string date)
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
            double balance = 0;
            string advance = "0", ad = "0", pend = "";
            double pending = 0;
            bool StoreCopy = true;
            StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
            if (StoreCopy)
                pending = Convert.ToDouble(dsPending.Tables[0].Rows[0]["DuePayment"].ToString());

            double wholeAmt = 0, disWhole = 0, WhAmt = 0, bal = 0;
            int totalItem = 0;
            totalItem = Convert.ToInt32(dsMain.Tables[0].Rows[0]["TotalQty"].ToString());
            for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
            {
                wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
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
            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) != "True")
            {
                totalWholeAmt = Math.Ceiling(totalWholeAmt);
            }
            bool Flag = false,BookingTime=true, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintSubItems = false;
            string logoLeftRight = string.Empty, printBookingTime = string.Empty;
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
            BookingTime = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBookingTime"].ToString());
            if (BookingTime)
            {
                printBookingTime = dsMain.Tables[0].Rows[0]["BookingTime"].ToString();
            }
            else
            {
                printBookingTime = "";
            }
            // First Section
            int invoice = 1;
            strPreview2 = "<table style='font-size:12px;font-weight:bold'><tr><td>Dear Customer ,</td></tr>";
            strPreview2 += "<tr><td>Thank you for your association with " + ds.Tables[0].Rows[0]["Headertext"].ToString() + ".We are pleased to provide you details around your recent transation with us.</td></tr>";
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
                            strPreview1 += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";

                        strPreview += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td><td align='center' style='width: 5.0in;'>";
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
                            strPreview1 += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td></tr></table></td></tr>";

                        strPreview += "</td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='110px' height='96px' /></td></tr></table></td></tr>";
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

                    strPreview += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
                }
                else
                {
                    if (StoreCopy)
                        strPreview1 += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";

                    strPreview += "<table style='width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
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
                    strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";
                strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "/" + invoice + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";
                invoice++;
            }
            else
            {
                if (StoreCopy)
                    strPreview1 += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";

                strPreview += "<td nowrap='nowrap'  align='center' style='font-family:" + ds.Tables[0].Rows[0]["NDFName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["NDFSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["NDB"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["NDU"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["NDI"].ToString() + "' >" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</td><td style='width:1.0in'><table><tr><td align='Center' style='width:1.0in;font-size:14PX;font-weight:bold;'>" + dsMain.Tables[0].Rows[0]["Bookingdate"].ToString() + "</td><td  align='center' style='font-size:13px;font-weight:bold'></td></tr><tr><td nowrap='nowrap' align='center' style='font-family: Bauhaus 93; font-size: 14px; font-weight: Bold; text-decoration: ; font-style: '>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + " " + printBookingTime + "</td></tr></table></td>";
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
                strPreview += "<tr><td colspan='4' align='center'><table style='width: 7.9in; border-top-style: solid; border-top-width: thin;border-top-color: #000000;'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
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
                if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Package Name</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'></td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Expiry Date</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'></td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Membership Id</td></tr></table></td></tr></table></td></tr>";
                }
                else
                {
                    strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Package Name</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'></td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Expiry Date</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'></td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Membership Id</td></tr></table></td></tr></table></td></tr>";
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
                strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;border-top-style: solid; border-top-width: thin;border-top-color: #000000'><tr><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["PackageName"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Package Name</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:30px'></td><td style='width:2.0in' align='center'><table><tr><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["EndDate"].ToString() + "</td></tr><tr><td align='center' style='font-weight: bolder; font-size: 12px'>Expiry Date</td></tr></table></td> <td align='center' style='font-weight:bolder;font-size:25px'></td><td style='width:2.0in' align='center'><table><tr><td align='center' style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["MemberShipId"].ToString() + "</td></tr><tr><td style='font-weight:bold;font-size:12px' align='center'>Membership Id</td></tr></table></td></tr></table></td></tr>";

                strPreview2 += "<tr><td>Total Amount Due :" + Convert.ToInt32(Math.Round(bal, 0)) + "</td></tr>";
                pend = Convert.ToInt32((totalWholeAmt - Advance1)).ToString();
            }
            strPreview2 += "<tr><td>Due Delivery Date :" + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
            // Fifth Section

            strPreview += "<tr style='font-size:12px'><td colspan='4' align='center'><table style='width:7.9in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;'><tr><td style='font-weight:bold;font-size:12px'>Total Pcs :&nbsp;" + totalItem + "</td><td style='width:2.0in;' align='right'><table><tr><td style='font-weight:bold;font-size:12px'></td><td style='font-size:12px'>" + dsMain.Tables[0].Rows[0]["BookingRemarks"].ToString() + "</td></tr></table></td><td align='center' style='width:4.0in' ><table><tr><td style='font-weight:bold;font-size:12px'>Due Date :&nbsp;</td><td style='font-size:13px'> " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr></table></td></tr>";
            strPreview2 += "<tr><td>Total Pcs. :" + totalItem + "</td></tr>";

            // Six Section

            if (PrintSubItems)
            {
                strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width: 7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'>Pcs</td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:4.25in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.25in;font-weight: bold;font-size:14px' align='center'>Status</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'></td></tr>";
            }
            else
            {
                strPreview += "<tr style='font-size:12px'><td colspan='4'><table style='width: 7.9in;'><tr><td style='width:0.5in;font-weight: bold;font-size:14px'></td><td style='width:0.5in;font-weight: bold;font-size:14px'>Qty</td><td style='width:4.25in;font-weight: bold;font-size:14px' align='left'>Particular's</td><td style='width:1.25in;font-weight: bold;font-size:14px' align='center'>Status</td><td style='width:1.0in;font-weight: bold;font-size:14px' align='right'></td></tr>";
            }

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
                string ItemName = string.Empty, Color = string.Empty, Remarks = string.Empty;
                bool checkstatus = false;
                string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
                string[] rate1 = rate.Split(';');
                string[] rate2 = rate.Split('@');
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
                string ItemStatus = BAL.BALFactory.Instance.BAL_New_Bookings.GetNoOfClothesReceived(dsMain.Tables[1].Rows[i]["ItemName"].ToString(), dsMain.Tables[1].Rows[i]["ISN"].ToString(), BookNumber, Globals.BranchID);
                string[] DeliverOrUnDeliver = ItemStatus.Split('/');
                string ClothesStatus = string.Empty;
                if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) == Convert.ToInt32(DeliverOrUnDeliver[1].ToString()))
                {
                    ClothesStatus = "Delivered" + " [" + DeliverOrUnDeliver[3] + "]";
                    checkstatus = true;
                }
                else
                {
                    if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) > 0)
                    {
                        ClothesStatus = "Delivered" + "-" + DeliverOrUnDeliver[0];
                    }
                }
                if (ClothesStatus != "" && checkstatus != true)
                {
                    ClothesStatus = ClothesStatus + "/" + dsMain.Tables[1].Rows[i]["SubPieces"].ToString() + " [" + DeliverOrUnDeliver[3] + "]";
                }
                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                {
                    if (PrintSubItems)
                    {
                        strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                    }
                }
                else
                {
                    if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                    {
                        if (PrintSubItems)
                        {
                            strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                        }
                    }
                    else
                    {
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            if (PrintSubItems)
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                            }
                        }
                        else
                        {
                            if (PrintSubItems)
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + DeliverOrUnDeliver[1].ToString() + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:12px'><td >" + "" + "</td><td >" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<span style='font-family:Arial Black; font-size: 11px;'>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</span>" + " " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + " " + Color + " " + Remarks + " " + DeliverOrUnDeliver[2].ToString() + "</td><td align='center'>" + ClothesStatus + "</td><td align='right'></td></tr>";
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
                    strPreview += "<tr style='font-size:10px'><td >&nbsp;</td><td></td></tr>";
                }
            }

            // Seventh Section

            if (Convert.ToInt32(hdnId.Value) < 9)
            {
                if (termAndCondition)
                {
                    strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                    if (St)
                    {
                        if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                        }
                    }
                    if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                    {
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            if (Convert.ToBoolean(dsMain.Tables[0].Rows[0]["DiscountOption"]).ToString() == "False")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold' nowrap='nowrap'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                        }
                    }
                    //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                    if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                    {
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                    }
                    else
                    {
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                    }
                }
                else
                {
                    strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                    strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                    if (St)
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                    }
                    if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                    }
                    if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                    }
                }

                strPreview += "</table></td></tr>";

                strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 0) + "</td></tr>";
                strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                if (St)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(serviceTax, 2) + "</td></tr>";
                }
                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + disWhole + "</td></tr>";
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
                        strPreview += "<tr style='font-size:10px'><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td></tr>";

                        if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td ></td><td style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                            }
                        }
                        //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";

                        //if (StoreCopy)
                        //    strPreview1 += "<tr style='font-size:10px'><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                        //strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue ...</td></tr>";
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                        strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        if (St)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right'></td><td align='right'></td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }

                        strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'>Continue...</td></tr>";
                    }

                    strPreview += "</table></td></tr>";
                }
                else
                {
                    if (termAndCondition)
                    {
                        strPreview += "<tr style='font-size:10px'><td colspan='3' style='font-weight: bold; border-top-style: solid; border-top-width: thin;border-top-color: #000000'>Terms and Conditions (R.O.A.C) &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<span style='font-weight: bold; font-size: 12px;'>Booked By : " + dsMain.Tables[0].Rows[0]["BookingAcceptedByUserId"].ToString() + "</span></td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td><td align='right' style='font-size: 12px; font-weight: bold; font-weight: bold; border-top-style: solid;border-top-width: thin; border-top-color: #000000'>&nbsp;</td></tr>";
                        if (St)
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td ></td><td></td></tr>";
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td ></td><td></td></tr>";
                            }
                        }
                        //  strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(dis, 0) + "</td></tr>";
                        if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                        else
                        {
                            if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp; " + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                            else
                            {
                                strPreview += "<tr style='font-size:10px'><td  colspan='3'>&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                            }
                        }
                    }
                    else
                    {
                        strPreview += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";

                        strPreview += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td colspan='3'></td><td  align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        if (St)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        if (BAL.BALFactory.Instance.BAL_New_Bookings.DisplayNetAmountFlatOrFolat(Globals.BranchID) == "True")
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                        else
                        {
                            strPreview += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'></td><td align='right' style='font-size:12px;font-weight:bold'></td></tr>";
                        }
                    }

                    strPreview += "</table></td></tr>";
                }

                strPreview2 += "<tr style='font-size:10px' ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td></tr>";
                //strPreview2 += "<tr style='font-size:10px;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 0) + "</td></tr>";
                strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Subtotal</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(wholeAmt, 2) + "</td></tr>";
                if (St)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Tax</td><td align='right' style='font-size:12px;font-weight:bold'>" + Math.Round(serviceTax, 2) + "</td></tr>";
                }
                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                {
                    strPreview2 += "<tr style='font-size:10px'><td  colspan='3'></td><td align='right' style='font-size:12px;font-weight:bold'>Discount</td><td align='right' style='font-size:12px;font-weight:bold'>" + dis + "</td></tr>";
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

                    strPreview += "</table>";

                    strPreview += "</td></tr></table>";
                    goto START;
                }
            }

            strPreview += "</table>";
            if (hdnId.Value != dsMain.Tables[1].Rows.Count.ToString())
            {
                strPreview += "</td></tr></table>";
                goto START;
            }
            strPreview2 += "<tr><td nowrap='nowrap' colspan='3'>Assuring you of our best service.</td></tr>";
            strPreview2 += "<tr><td colspan='3'>Warm Regards.</td></tr>";
            strPreview2 += "<tr><td colspan='3'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</td></tr>";
            strPreview2 += "</table>";
        }
        catch (Exception ex) { }

        ViewState["Msg"] = strPreview;
        ViewState["Msg1"] = strPreview2;
        hdnId.Value = "0";
        return strPreview;
    }
}