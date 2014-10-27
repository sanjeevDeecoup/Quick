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

namespace QuickWeb.Bookings
{
   
    public partial class PackageSlip : System.Web.UI.Page
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        public string strPreview = string.Empty;
        public string strPreview1 = string.Empty;
        public string chkdiscount = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MakePackageSlip();
                hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
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
                if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() == "true")
                {
                    hdnRedirectBack.Value = "true";
                }
            }
        }
        public string MakePackageSlip()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 2);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            ds = AppClass.GetData(cmd);
            bool Flag = false, logoOnReceipt = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintDueDate = false, PrintSubItems = false;
            string logoLeftRight = string.Empty;
            PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
            prePrintedOrBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
            headerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
            footerBanner = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
            termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
            PrintSubItems = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintSubItem"].ToString());
            barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
            previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
            logoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
            logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
            tableBorder = Convert.ToBoolean(ds.Tables[0].Rows[0]["TableBorder"].ToString());

            // START FIRST SECTION

            if (prePrintedOrBanner)
            {
                if (tableBorder)
                {
                    strPreview += "<table class='TableData' style='width:7.9in;border: thin solid #000000'><tr><td>";
                }
                else
                {
                    strPreview += "<table class='TableData' style='width:7.9in;'><tr><td>";
                }
                strPreview += "<table style='width:7.9in;height:1.1in;'><tr>";
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
                strPreview += "<table style='width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000'><tr><td colspan='3' align='center'></td></tr></table>";
            }

            strPreview += "</table>";

            //// Body Part Means Second And Last Part of this Slip

            strPreview += "<tr><td>";
            strPreview += "<table style='width: 7.9in; border: thin solid #000000;'>";
            strPreview += " <tr style='font-size: 12px'>";
            strPreview += "<td colspan='4'>";
            strPreview += " <table style='width: 7.9in;'>";

            /// Content Part

            strPreview += "<tr><td style='width: 0.5in; font-weight: bold; font-size: 14px' nowrap='nowrap'><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Kind Attention:</span></b><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'> </span></td><td style='width: 0.5in; font-weight: bold; font-size: 14px'></td><td style='width: 5.0in; font-size: 14px' align='left'>" + Request.QueryString["CN"].ToString() + "</td><td style='width: 1.0in; font-weight: bold' align='right'></td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'></td></tr>";
            strPreview += "<tr style='font-size: 12px'><td nowrap='nowrap'><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Membership No:</span></b></td><td>&nbsp;</td><td><bolder>" + Request.QueryString["MN"].ToString() + "</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            strPreview += "<tr style='font-size: 12px'><td><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Address:</span></b></td><td></td><td><bolder>" + Request.QueryString["CA"].ToString() + "</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            //strPreview += "<tr style='font-size: 12px'><td><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Dated:</span></b></td><td></td><td><bolder>" + Request.QueryString["PSD"].ToString() + "</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            strPreview += "<tr style='font-size: 12px'><td><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Validity:</span></b></td><td></td><td><bolder>" + Request.QueryString["PSTD"].ToString() + " to " + Request.QueryString["PED"].ToString() + "</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            strPreview += "<tr style='font-size: 12px'><td nowrap='nowrap'><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Package Name:</span></b></td><td></td><td><bolder>" + Request.QueryString["PN"].ToString() + "</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            
            strPreview += "<tr style='font-size: 12px'><td><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Cost:</span></b></td><td></td><td><bolder>" + Request.QueryString["PC"].ToString() + "</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            
            chkdiscount = Request.QueryString["CHDIS"].ToString();
           
            if (chkdiscount == "true")
            {
                strPreview += "<tr style='font-size: 12px'><td><b style='mso-bidi-font-weight:normal'><span style='font-size:11.0pt;line-height:115%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:black;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Discount:</span></b></td><td></td><td><bolder>" + Request.QueryString["DIS"].ToString() + "%"+"</td><td align='right'></td><td align='right'>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>";
            }

            strPreview += "<tr style='font-size: 10px'><td height='20Px'>&nbsp;</td><td></td></tr>";
            strPreview += "<tr style='font-size: 14px'><td colspan='5'>Dear Valued Member,</td></tr>";
            strPreview += "<tr style='font-size: 10px'><td height='20Px'>&nbsp;</td><td></td></tr>";
            strPreview += "<tr style='font-size: 14px'><td colspan='5'>It is our pleasure to notify you that a membership account has been approved in your name. We Welcome you as a new</td></tr>";
            strPreview += "<tr style='font-size: 14px'><td colspan='5'>member and hope that you enjoy the convenience of your Membership Card.</td></tr>";
            strPreview += "<tr style='font-size: 10px'><td height='20Px'>&nbsp;</td><td></td></tr>";
            strPreview += "<tr style='font-size: 14px'><td colspan='5'>Thanks once again for choosing us.</td></tr>";
            strPreview += "<tr style='font-size: 10px'><td height='15Px'>&nbsp;</td><td></td></tr>";
            strPreview += "<tr style='font-size: 14px'><td colspan='5'>Best regards,</td></tr>";
            strPreview += "<tr style='font-size: 10px'><td height='20Px'>&nbsp;</td><td></td></tr>";
            strPreview += "<tr style='font-size: 14px'><td colspan='5'>Authorized Signatory</td></tr>";            

            strPreview += "</table>";
            strPreview += "</td></tr>";
            strPreview += "</table>";
            strPreview += "</td></tr>";

            return strPreview;
        }
    }
}