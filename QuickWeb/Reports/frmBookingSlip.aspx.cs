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

public partial class Reports_frmBookingSlip : System.Web.UI.Page
{
    public string strPreview = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            SetReceipt();
    }
    public string SetReceipt()
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        ds = AppClass.GetData(cmd);

        DataSet dsMain = new DataSet();
        cmd = new SqlCommand();
        cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@BookingNumber", 109));
        dsMain = AppClass.GetData(cmd);

        DataSet dsPending = new DataSet();
        cmd = new SqlCommand();
        cmd.CommandText = "Sp_Sel_CustomerAllPrevoiusDue";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@CustCode", "cust1"));
        dsPending = AppClass.GetData(cmd);
        double pending = 0;
        for (int i = 0; i < dsPending.Tables[0].Rows.Count; i++)
        {
            pending += Convert.ToDouble(dsPending.Tables[0].Rows[i]["DuePayment"].ToString());
        }

        double wholeAmt = 0;

        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
        {
            string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
            string[] rate1 = rate.Split('@');
            wholeAmt += Convert.ToDouble(float.Parse(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString()) * float.Parse(rate1[1]));
            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
            {
                wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
            }
        }
        double discount = 0, serviceTax = 0;
        for (int i = 0; i < dsMain.Tables[3].Rows.Count; i++)
        {
            serviceTax += Convert.ToDouble(dsMain.Tables[3].Rows[i]["Amount"].ToString());
        }
        discount = Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString());
        double totalWholeAmt = 0;
        double dis = 0;
        if (discount > 0)
        {
            totalWholeAmt = wholeAmt + serviceTax;
            dis = totalWholeAmt * discount / 100;
            totalWholeAmt -= dis;
        }
        else
        {
            totalWholeAmt = wholeAmt + serviceTax;
        }
        bool logoOnReceipt=false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false;
        string logoLeftRight = "";
        prePrintedOrBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
        headerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
        footerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
        termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
        St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
        barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
        previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
        logoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
        logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
        strPreview = "<table class='TableData' style='width:8.0in;border: thin solid #000000'><tr><td>";
        if (prePrintedOrBanner)
        {
            strPreview += "<table style='width:8.0in;height:1.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr>";
            if (logoLeftRight == "1")
            {
                if (logoOnReceipt)
                {
                    strPreview += "<td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' /></td><td align='center' style='width: 5.0in;'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span>";
                    strPreview += "</br><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                }
                else
                {
                    strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span>";
                    strPreview += "</br><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                }
            }
            else
            {
                if (logoOnReceipt)
                {
                    strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span>";
                    strPreview += "</br><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td><td style='width: 1.5in'><img alt='' src='../ReceiptLogo/DRY.jpg' /></td></tr></table></td></tr>";
                }
                else
                {
                    strPreview += "<td style='width: 1.5in'></td><td align='center' style='width: 5.0in;'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span>";
                    strPreview += "</br><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td><td style='width: 1.5in'>.</td></tr></table></td></tr>";
                }
            }
        }
        else
            strPreview += "<table style='width:8.0in;height:1.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table></td></tr>";
        strPreview += "<tr><td><table style='width: 8.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr>";
        strPreview += "<td style='width:2.0in'>Invoice No : " + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "</br>Date :" + DateTime.Today.ToString("dd-MMM-yyyy") + "</td>";
        if (barcode)
            strPreview += "<td align='center' style='width:4.0in;font-family: IDAutomationHC39M;' >*" + dsMain.Tables[1].Rows[0]["BookingNumber"].ToString() + "*</td>";
        else
            strPreview += "<td style='width:4.0in'>.</td>";
        strPreview += "<td style='width:2.0in'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "(" + dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString() + ")" + "</br>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + "</td></tr></table></td></tr>";
        if (headerBanner)
            strPreview += "<tr><td colspan='3'><table style='width:8.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["SloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["SloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr></table></td></tr>";
        double balance = 0;
        string advance = "0";
        if (previousDue)
        {
            balance = 0;
            try
            {
                balance = ((pending + totalWholeAmt) - Convert.ToDouble(dsMain.Tables[2].Rows[0]["Payment"].ToString()));
                advance = dsMain.Tables[2].Rows[0]["Payment"].ToString();
            }
            catch (Exception)
            { balance = totalWholeAmt - Convert.ToDouble("0"); }
            strPreview += "<tr><td colspan='3'><table  style='width: 8.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td style='width:2.0in'>Rs." + pending + "</br>Previous Due&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp+</td><td style='width:2.0in'>Rs." + wholeAmt + "</br>Current Due&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-</td><td style='width:2.0in'>Rs." + advance + "</br>Advance&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;=</td><td style='width:2.0in'>Rs." + balance + "</br>Balance</td></tr></table></td></tr>";
        }
        else
        {
            balance = 0;
            try
            {
                balance = totalWholeAmt - Convert.ToDouble(dsMain.Tables[2].Rows[0]["Payment"].ToString());
                advance = dsMain.Tables[2].Rows[0]["Payment"].ToString();
            }
            catch (Exception )
            { balance = totalWholeAmt - Convert.ToDouble("0"); }

            strPreview += "<tr><td colspan='3'><table style='width: 8.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td style='width:2.0in'>.</td><td style='width:2.0in'>Rs." + Math.Round(totalWholeAmt, 2) + "</br>Current Due&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-</td><td style='width:2.0in'>Rs." + advance + "</br>Advance&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;=</td><td style='width:2.0in'>Rs." + Math.Round(balance, 2) + "</br>Balance</td></tr></table></td></tr>";
        }
        strPreview += "<tr><td colspan='3'><table style='width: 8.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td style='width:2.0in;border-right-style: solid; border-right-width: thin; border-right-color: #000000'>" + dsMain.Tables[1].Rows[0]["ItemRemark"].ToString() + "</td><td style='width:2.0in;border-right-style: solid; border-right-width: thin; border-right-color: #000000'>Total Items : " + dsMain.Tables[1].Rows.Count + "</td><td style='width:4.0in'>Due Date: " + dsMain.Tables[0].Rows[0]["DeliveryTime"].ToString() + " / " + Convert.ToDateTime(dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString()).DayOfWeek.ToString() + " / " + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr></table></td></tr>";
        //looping for items
        strPreview += "<tr><td colspan='3'><table style='width: 8.0in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td style='width:0.5in;font-weight: bold'>S.No.</td><td style='width:0.5in;font-weight: bold'>Qty</td><td style='width:5.0in;font-weight: bold' align='center'>Particular</td><td style='width:1.0in;font-weight: bold'>Rate</td><td style='width:1.0in;font-weight: bold'>Amount</td></tr>";


        int incRow = 1;

        wholeAmt = 0;
        for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
        {
            string rate = dsMain.Tables[1].Rows[i]["ItemQuantityAndRate"].ToString();
            string[] rate1 = rate.Split('@');
            strPreview += "<tr><td>" + incRow + "</td><td>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "  " + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "</td><td>" + rate1[1] + "</td><td>" + (float.Parse(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString()) * float.Parse(rate1[1])) + "</td></tr>";
            wholeAmt += Convert.ToDouble(float.Parse(dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString()) * float.Parse(rate1[1]));
            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
            {
                strPreview += "<tr><td></td><td></td><td>" + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + "</td><td>" + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + "</td><td>" + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + "</td></tr>";
                wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString());
            }
            incRow++;
        }
        if (termAndCondition)
        {
            strPreview += "<tr ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'>Terms and Conditions Area</td>";
            if (ds.Tables[0].Rows[0]["Term1"].ToString() != "")
                strPreview += "<tr><td  colspan='3'>" + ds.Tables[0].Rows[0]["Term1"].ToString() + "</td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Subtotal</td><td>" + wholeAmt + "</td></tr>";
            else
                strPreview += "<tr><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'> Subtotal</td><td>" + wholeAmt + "</td></tr>";
            if (ds.Tables[0].Rows[0]["Term2"].ToString() != "")
            {
                strPreview += "<tr><td  colspan='3'>" + ds.Tables[0].Rows[0]["Term2"].ToString() + "</td>";
                if (St)
                    strPreview += "<td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Tax</td><td>" + serviceTax + "</td></tr>";
                else
                    strPreview += "<td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'></td><td></td></tr>";
            }
            else
            {
                strPreview += "<tr><td  colspan='3'></td>";
                if (St)
                    strPreview += "<td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Tax</td><td>" + serviceTax + "</td></tr>";
                else
                    strPreview += "<td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'></td><td></td></tr>";

            }
            if (ds.Tables[0].Rows[0]["Term3"].ToString() != "")
            {
                if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                    strPreview += "<tr><td  colspan='3'>" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Discount</td><td>" + dis + "</td></tr>";
                else
                    strPreview += "<tr><td  colspan='3'>" + ds.Tables[0].Rows[0]["Term3"].ToString() + "</td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'></td><td></td></tr>";
            }
            else
                strPreview += "<tr><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Discount</td><td>" + Math.Round(dis,2) + "</td></tr>";
            if (ds.Tables[0].Rows[0]["Term4"].ToString() != "")
                strPreview += "<tr><td  colspan='3'>" + ds.Tables[0].Rows[0]["Term4"].ToString() + "</td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Total</td><td>" + Math.Round((totalWholeAmt- Convert.ToDouble(advance)),2) + "</td></tr>";
            else
                strPreview += "<tr><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Total</td><td>" + Math.Round((totalWholeAmt - Convert.ToDouble(advance)), 2) + "</td></tr>";
        }
        else
        {
            strPreview += "<tr ><td colspan='5' style='font-weight: bold;border-top-style: solid; border-top-width: thin; border-top-color: #000000'></td>";
            strPreview += "<tr style='border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Subtotal</td><td>" + Math.Round(wholeAmt, 2) + "</td></tr>";
            if (St)
                strPreview += "<tr><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Tax</td><td>" + serviceTax + "</td></tr>";
            if (Convert.ToDouble(dsMain.Tables[0].Rows[0]["Discount"].ToString()) > 0)
                strPreview += "<tr><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Discount</td><td>" + Math.Round(dis, 2) + "</td></tr>";
            strPreview += "<tr><td  colspan='3'></td><td style='border-left-style: solid; border-left-width: thin; border-left-color: #000000'>Total</td><td>" + Math.Round((totalWholeAmt - Convert.ToDouble(advance)), 2) + "</td></tr>";
        }
        strPreview += "</table></td></tr>";
        if (footerBanner)
            strPreview += "<tr><td colspan='3'><table style='width:8.0in;height:0.5in;border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000'><tr><td align='center' style='font-family:" + ds.Tables[0].Rows[0]["FooterSloganFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["FooterSloganFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["FooterSloganFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["FooterSloganUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["FooterSloganItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["FooterSloganText"].ToString() + "</td></tr></table></td></tr>";
        strPreview += "</table>";
        return strPreview;
    }
}
