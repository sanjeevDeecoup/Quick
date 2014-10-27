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

namespace QuickWeb.ThermalPrinter
{
    public partial class PackageSlip : System.Web.UI.Page
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        public string strPreview = string.Empty;
        public string strPreview1 = string.Empty;
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
            bool Flag = false, logoOnReceipt = false,logo=false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintDueDate = false, PrintSubItems = false;
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
            logo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());

            // START FIRST SECTION
            string custPhone = string.Empty, displayPhone = string.Empty, leftMessage = string.Empty, rightMessage = string.Empty;
           
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
            strPreview += "<table style='width:3in; page-break-after: always;'>";
            strPreview += "<tr><td>";
            strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
            if (logo)
            {
                strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='40px' height='40px' /></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            }
            else
            {               
                strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' width='40Px'></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            }
            
            strPreview += "<tr><td align='center' colspan='5'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr>";
           
            strPreview += "<tr><td width='3in' style='font-size: 15px; font-family: Times New Roman; ' colspan='5' align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr>";
            strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            strPreview += "</table>";
            strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Kind Attention: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;' nowrap='nowrap'>" + Request.QueryString["CN"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Package Name: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["PN"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Cost:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["PC"].ToString() + "</td></tr>";
            if (Request.QueryString["PKGTYPE"].ToString() == "Value / Benefit")
            {
                strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Value:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["PKGBVALUE"].ToString() + "</td></tr>";                
            } 
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Validity:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["PSTD"].ToString() + " to " + Request.QueryString["PED"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Membership: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["MN"].ToString() + "</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Address: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;' nowrap='nowrap'>" + Request.QueryString["CA"].ToString() + "</td></tr>";
            //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Address:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; ' ></td>" + Request.QueryString["CA"].ToString() + "</tr>";
            //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Dated:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["PSD"].ToString() + "</td></tr>";
            
            
            
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>&nbsp;</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Dear Valued Member,&nbsp;</td></tr>";
            strPreview += "<tr><td  style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>&nbsp;</td></tr>";
            strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman;' colspan='2'>It is our pleasure to notify you that a membership account has been approved in your name. We Welcome you as a new member and hope that you enjoy the convenience of your Membership Card.</td></tr>";
            strPreview += "<tr><td width='3in' colspan='2' height='5Px'></td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Thanks once again for choosing us.</td></tr>";
            strPreview += " <tr><td style='font-size: 10px; font-family: Times New Roman;' colspan='2'>&nbsp;</td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Best regards,</td></tr>";
            strPreview += " <tr><td style='font-size: 10px; font-family: Times New Roman;' colspan='2'>&nbsp;</td></tr>";
            strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Authorized Signatory</td></tr>";
            strPreview += "</table></td></tr>";
            return strPreview;
        }
    }
}