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
using System.Web.Mail;
using System.Threading.Tasks;
/// <summary>
/// Summary description for AppClass
/// </summary>
public class AppClass
{
	public static string sqlConStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
	public AppClass()
	{
		//
		// TODO: Add constructor logic here
		//
	}
	public static string GetPrefixForCurrentYear()
	{
		string strRet = string.Empty;
		//strRet = DateTime.Now.Year.ToString() + "/";
		return strRet;
	}

    public static string MakeLaserPackageSlip(string _CustomerName, string _CustomerAddress, string _memberShipNo, string _PackageSaleDate, string _PackageStartDate, string _PackageEndDate, string _packageName, string _PackageCost, string _checkdiscount, string Branchid, string PackageMode, string storename, string BusinessName, string _pkgType, string _pkgBenefitValue)
    {
        string strPreview = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        cmd.Parameters.AddWithValue("@BranchId", Branchid);
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
        strPreview += "<table style='width: 7.9in;font-family: Verdana; font-size: 11px;'>";

        strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Dear&nbsp;" + _CustomerName + ",</td></tr>";
        strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>&nbsp;</td></tr>";

        if (PackageMode == "Creation")
        {
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Welcome to the world of great savings. Your membership details are as follows.</td></tr>";

        }
        if (PackageMode == "Expire")
        {
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>This is to bring to your notice that your membership package with us, expired on "+_PackageEndDate+".</td></tr>";
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>We request you to renew/purchase your package to keep enjoying the membership benefits.</td></tr>";
        }
        if (PackageMode == "Renewal")
        {
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>This is to bring to your notice that your membership package would expire on " + _PackageEndDate + ". </td></tr>";
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>We request you to renew/purchase your package to keep enjoying the membership benefits.</td></tr>";
        }

        strPreview += "<tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>&nbsp;</td><td style='width: 2.5in; font-size: 14px'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Package Name:</td><td style='width: 2.5in; font-size: 14px'>" + _packageName + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr>";

        if (_pkgType == "Value / Benefit")
        {
            strPreview += "<tr><td style='width: 1.5in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Package Cost:</td><td style='width: 2.5in; font-size: 14px'>" + _PackageCost + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr>";
            strPreview += "<tr><td style='width: 1.5in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Package Value:</td><td style='width: 2.5in; font-size: 14px'>" + _pkgBenefitValue + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr>";        
        }
        else
        {
            strPreview += "<tr><td style='width: 1.5in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Package Amount:</td><td style='width: 2.5in; font-size: 14px'>" + _PackageCost + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr>";        
        }

        strPreview += "<tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Valid Till:</td><td style='width: 2.5in; font-size: 14px'>" + _PackageEndDate + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Membership No:</td><td style='width: 2.5in; font-size: 14px'>" + _memberShipNo + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>Address:</td><td style='width: 2.5in; font-size: 14px'>" + _CustomerAddress + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>&nbsp;</td><td style='width: 2.5in; font-size: 14px'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr>";


        if (PackageMode == "Creation")
        {
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>We hope you enjoy our services and thanks for your patronage.</td></tr>";

        }
        if (PackageMode == "Expire")
        {
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Thanks once again for choosing us.</td></tr>";
        }
        if (PackageMode == "Renewal")
        {
            strPreview += "<tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Look forward to your continued association.</td></tr>";
        }
        strPreview += "<tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>&nbsp;</td><td style='width: 2.5in; font-size: 14px'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='width: 1.2in; font-size: 14px' nowrap='nowrap'>Best regards,</td><td style='width: 2.5in; font-size: 14px'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>&nbsp;</td><td style='width: 2.5in; font-size: 14px'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>"+storename+"</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>"+BusinessName+"</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr>";

        strPreview += "</table>";       

        return strPreview;
    }
    public static string MakeThermalPackageSlip(string _CustomerName, string _CustomerAddress, string _memberShipNo, string _PackageSaleDate, string _PackageStartDate, string _PackageEndDate, string _packageName, string _PackageCost, string _checkdiscount, string Branchid, string PackageMode, string storename)
    {
        string strPreview = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_ReceiptConfigSetting";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 2);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        ds = AppClass.GetData(cmd);
        bool Flag = false, logoOnReceipt = false, logo = false, tableBorder = false, prePrintedOrBanner = false, barcode = false, headerBanner = false, previousDue = false, footerBanner = false, termAndCondition = false, St = false, PrintDueDate = false, PrintSubItems = false;
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
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Kind Attention: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;' nowrap='nowrap'>" + _CustomerName + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Membership No: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + _memberShipNo + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Address: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;' nowrap='nowrap'>" + _CustomerAddress + "</td></tr>";
        //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' nowrap='nowrap'>Address:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; ' ></td>" + Request.QueryString["CA"].ToString() + "</tr>";
        //strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Dated:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + Request.QueryString["PSD"].ToString() + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Validity:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + _PackageStartDate + " to " + _PackageEndDate + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Package Name: &nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + _packageName + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>Amount:&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>" + _PackageCost + "</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold'>&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; width: 192px;'>&nbsp;</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Dear Valued Member,&nbsp;</td></tr>";
        strPreview += "<tr><td  style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>&nbsp;</td></tr>";
        if (PackageMode == "Creation")
        {
            strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman;' colspan='2'>It is our pleasure to notify you that a membership account has been approved in your name. We Welcome you as a new member and hope that you enjoy the convenience of your Membership Card.</td></tr>";
            strPreview += "<tr><td width='3in' colspan='2' height='5Px'></td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Thanks once again for choosing us.</td></tr>";
        }
        if (PackageMode == "Expire")
        {
            strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman;' colspan='2'>It is to notify that your membership account has been expired and mark completed. We suggest you to</td></tr>";
            strPreview += "<tr><td width='3in' colspan='2' height='5Px'></td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Renew/ Purchase Package to enjoy the convenience of your Membership Card.</td></tr>";
        }
        if (PackageMode == "Renewal")
        {
            strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman;' colspan='2'>It is to notify that your membership account will be expired on "+ _PackageEndDate+". We suggest you to </td></tr>";
            strPreview += "<tr><td width='3in' colspan='2' height='5Px'></td></tr>";
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Renew/ Purchase Package to enjoy the convenience of your Membership Card.</td></tr>";
        }
        strPreview += " <tr><td style='font-size: 10px; font-family: Times New Roman;' colspan='2'>&nbsp;</td></tr>";
        strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Best regards,</td></tr>";
        strPreview += " <tr><td style='font-size: 10px; font-family: Times New Roman;' colspan='2'>&nbsp;</td></tr>";
        strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>Authorized Signatory</td></tr>";
        strPreview += " <tr><td style='font-size: 10px; font-family: Times New Roman;' colspan='2'>&nbsp;</td></tr>";
        strPreview += " <tr><td style='font-size: 12px; font-family: Times New Roman; font-weight:bold' colspan='2' nowrap='nowrap'>"+ storename +"</td></tr>";
        strPreview += "</table></td></tr>";
        return strPreview;
    }

    public static string sendPackageReportSms(GridView grdReport, DropDownList drpsmstemplate)
    {
        string res = string.Empty, custCode = string.Empty, PackageName = string.Empty, expiryDate = string.Empty, strCustomerName = string.Empty, strCustomerMobileNo = string.Empty, strDump = string.Empty;
        DataSet dsMain = new DataSet();
        for (int i = 0; i < grdReport.Rows.Count; i++)
        {
            if (((CheckBox)grdReport.Rows[i].FindControl("chkSelect")).Checked)
            {
                custCode = grdReport.Rows[i].Cells[2].Text;
                dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(custCode, Globals.BranchID);
                strCustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
                strCustomerMobileNo = dsMain.Tables[0].Rows[0]["CustomerMobile"].ToString();
                PackageName = grdReport.Rows[i].Cells[4].Text;
                expiryDate = grdReport.Rows[i].Cells[6].Text;
                if (strCustomerMobileNo != "")
                {
                    AppClass.GoPendingReportExpireOrRenewalMsg(Globals.BranchID, strCustomerMobileNo, drpsmstemplate.SelectedValue, strCustomerName, expiryDate, PackageName);
                }               
            }
        }       
        return res;
    }
    public static string sendPackageReportEmail(GridView grdReport, string EmailType,string PrinterName,string PackageType,string StoreName, string BusinessName)
    {
        string res = string.Empty, custCode = string.Empty, PackageName = string.Empty, expiryDate = string.Empty, strCustomerName = string.Empty, strCustomerMobileNo = string.Empty, CustomerAddress = string.Empty, memberShipNo = string.Empty, PackageSaleDate = string.Empty, PackageCost = string.Empty, _checkdiscount = string.Empty, EmailId = string.Empty, _pkgType= string.Empty,  pkgBenefitValue = string.Empty;
        DataSet dsMain = new DataSet();
        for (int i = 0; i < grdReport.Rows.Count; i++)
        {
            if (((CheckBox)grdReport.Rows[i].FindControl("chkSelect")).Checked)
            {
                custCode = grdReport.Rows[i].Cells[2].Text;
                dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(custCode, Globals.BranchID);

                strCustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
                strCustomerMobileNo = dsMain.Tables[0].Rows[0]["CustomerMobile"].ToString();
                CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
                memberShipNo = dsMain.Tables[0].Rows[0]["MemberShipId"].ToString();
                EmailId = dsMain.Tables[0].Rows[0]["CustomerEmailId"].ToString();

                if (PackageType == "Discount")
                {
                    _checkdiscount = "true";
                }
                else
                {
                    _checkdiscount = "false";
                }

                PackageName = grdReport.Rows[i].Cells[4].Text;
                PackageSaleDate = grdReport.Rows[i].Cells[5].Text;
                expiryDate = grdReport.Rows[i].Cells[6].Text;
                PackageCost = grdReport.Rows[i].Cells[7].Text;
                pkgBenefitValue = grdReport.Rows[i].Cells[8].Text;

                string slipdetail = string.Empty, subject = string.Empty;
                if (EmailType == "Expire")
                {
                   // subject = "Dear " + strCustomerName + " Your " + PackageName + " is expired on " + expiryDate + "";
                    subject = "" + BusinessName + " - " + StoreName + " | Package Expiry Notification: " + expiryDate + "";
                }
                else
                {
                   // subject = "Dear " + strCustomerName + " Your " + PackageName + " is about to expire on " + expiryDate + "";
                    subject = "" + BusinessName + " - " + StoreName + " | Package Expiry Notification: " + expiryDate + "";
                }
                slipdetail = MakeLaserPackageSlip(strCustomerName, CustomerAddress, memberShipNo, PackageSaleDate, PackageSaleDate, expiryDate, PackageName, PackageCost, _checkdiscount, Globals.BranchID, EmailType, StoreName, BusinessName ,PackageType,pkgBenefitValue);
                if (EmailId != "")
                {
                    sendMail(slipdetail, EmailId, subject);
                }
            }
        }
        return res;
    }

    private static string sendMail(string mailBody, string eMail,string subject)
    {
        bool SSL = false;
        SqlCommand cmd1 = new SqlCommand();
        DataSet ds1 = new DataSet();
        try
        {
            cmd1.CommandText = "sp_ReceiptConfigSetting";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd1.Parameters.AddWithValue("@Flag", 2);
            ds1 = AppClass.GetData(cmd1);

            string FEmail = eMail;
            SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
            int port = Convert.ToInt32(ds1.Tables[0].Rows[0]["Port"].ToString());
            Task tuy = Task.Factory.StartNew
                     (
                        () => { AppClass.SendMail(FEmail, subject, mailBody, true, port, ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL); }
                     );
        }
        catch (Exception ex)
        {
        }
        finally
        {

        }
        return "";
    }

    public static string SendMail(string to, string subject, string body, bool isBodyHtml, int Port, string hostName, string BranchEmail, string BranchPassword, bool SSL)
    {
        string result = "Message Sent Successfully..!!";
        string ssltemp = string.Empty;
        try
        {
            if (SSL)
            {
                ssltemp = "True";
            }
            else
            {
                ssltemp = "False";
            }
            // Setup mail message
            int cdoBasic = 1;
            int cdoSendUsingPort = 2;
            MailMessage msg = new MailMessage();
            if (BranchEmail.Length > 0)
            {
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", hostName);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", Port);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                if (SSL)
                {
                    msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", ssltemp);
                }
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", BranchEmail);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", BranchPassword);
            }
            msg.BodyFormat = MailFormat.Html;
            msg.To = to;
            msg.From = BranchEmail;
            msg.Subject = subject;
            msg.Body = body;
            
            SmtpMail.SmtpServer = hostName;
            SmtpMail.Send(msg);
        }       
        catch (Exception ex)
        {
            // TODO: Handle exception
            // Any exception that may occur during the send process.
            result = ex.ToString();
        }
        return result;
    }
    public static string SendPendingAmountSMS(GridView grdReport, string BID, DropDownList drpsmstemplate)
    {
        string res = string.Empty, pendingPcs = string.Empty, pendingAmount = string.Empty, strCustomerName = string.Empty, strCustomerMobileNo = string.Empty, strDump = string.Empty;
       
            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                if (((CheckBox)grdReport.Rows[i].FindControl("chkSelect")).Checked)
                {
                    strCustomerName = ((HyperLink)grdReport.Rows[i].FindControl("hplNavigate")).Text;
                    strCustomerMobileNo = grdReport.Rows[i].Cells[4].Text;
                    pendingPcs = grdReport.Rows[i].Cells[10].Text;
                    pendingAmount = grdReport.Rows[i].Cells[11].Text;
                    if (pendingPcs == "0" && pendingAmount == "0")
                    {
                        
                    }
                    else
                    {
                        if (strCustomerMobileNo != "&nbsp;")
                        {
                            AppClass.GoPendingStockMsg(BID, strCustomerMobileNo, drpsmstemplate.SelectedValue, strCustomerName, pendingPcs, pendingAmount);
                        }
                    }
                    ((CheckBox)grdReport.Rows[i].FindControl("chkSelect")).Checked = false;
                }
            }
            ((CheckBox)grdReport.HeaderRow.FindControl("CheckAll")).Checked = false;                          
        return res;
    }

	public static StringBuilder GetExcelContentForGrid(GridView grdSource)
	{
		StringBuilder str = new StringBuilder();
		string strValue = string.Empty, strCtlType=string.Empty;
		if (grdSource.Rows.Count > 0)
		{
			str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
			str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
			str.Append("<body topmargin=0 leftmargin=0>");
			str.Append("<table cellpadding='0' cellspacing='0'>");
			str.Append("<tr>");
			for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
			{
				str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + grdSource.HeaderRow.Cells[c].Text + "</th>");
			}
			str.Append("</tr>");
			for (int r = 0; r < grdSource.Rows.Count; r++)
			{
				str.Append("<tr>");
				for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
				{
					strValue = string.Empty;
					if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
					{
						strCtlType = "" + grdSource.Rows[r].Cells[c].Controls[1].GetType().ToString();
						if (strCtlType == "System.Web.UI.LiteralControl")
						{
							strValue = "" + ((LiteralControl)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.Label")
						{
							strValue = "" + ((Label)grdSource.Rows[r].Cells[c].Controls[1]).Text;                            
						}
						if (strCtlType == "System.Web.UI.WebControls.LinkButton")
						{
							strValue = "" + ((LinkButton)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.HyperLink")
						{
							strValue = "" + ((HyperLink)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
					}
					if (strValue == "")
					{
						strValue = "" + grdSource.Rows[r].Cells[c].Text;
					}
					str.Append("<td>" + strValue + "</td>");

				}
				str.Append("</tr>");
			}
			for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
			{
				str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
			}
			str.Append("</table>");
			str.Append("</body>");
		}
		return str;
	}

	/// <summary>
	/// Get the excel content from grid
	/// </summary>
	/// <param name="grdSource">the source grid</param>
	/// <param name="showHidden">weather to show hidden columns or not</param>
	/// <returns></returns>
	public static StringBuilder GetExcelContentForGridBooking(GridView grdSource, bool showHidden = true, IEnumerable<int> columnsToHide = null)
	{
		StringBuilder str = new StringBuilder();
		string strValue = string.Empty, strCtlType = string.Empty;
		if (grdSource.Rows.Count > 0)
		{
			str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
			str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
			str.Append("<body topmargin=0 leftmargin=0>");
			str.Append("<table cellpadding='0' cellspacing='0'>");
			str.Append("<tr>");
			for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
			{
				if (columnsToHide != null)
				{
					if (columnsToHide.Contains(c))
						continue;
				}

				var type = grdSource.HeaderRow.Cells[c].GetType().ToString();
				var lbl = string.Empty;
				switch (type)
				{
					case "System.Web.UI.WebControls.LiteralControl": lbl = ((LiteralControl)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
						break;
					case "System.Web.UI.WebControls.Label": lbl = ((Label)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
						break;
					case "System.Web.UI.WebControls.LinkButton": lbl = ((LinkButton)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
						break;
					case "System.Web.UI.WebControls.HyperLink": lbl = ((HyperLink)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
						break;
					case "System.Web.UI.WebControls.DataControlFieldHeaderCell":
						{
							if (grdSource.HeaderRow.Cells[c].Controls.Count >= 1)
							{
								var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
								switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
								{
									case "System.Web.UI.WebControls.DataControlLiteralControl": lbl = ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
										break;
									case "System.Web.UI.WebControls.DataControlLabel": lbl = ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
										break;
									case "System.Web.UI.WebControls.DataControlLinkButton": lbl = ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
										break;
									case "System.Web.UI.WebControls.DataControlHyperLink": lbl = ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
										break;
								}
							}
							else
							{
								lbl = grdSource.HeaderRow.Cells[c].Text;
							}
						}
						break;
					default: lbl = grdSource.HeaderRow.Cells[c].Text;
						break;
				}
				str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + lbl + "</th>");
			}
			str.Append("</tr>");
			for (int r = 0; r < grdSource.Rows.Count; r++)
			{
				if (!showHidden && grdSource.Rows[r].Visible == false)
					continue;



				str.Append("<tr>");
				for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
				{
					strValue = string.Empty;

					if (columnsToHide != null)
					{
						if (columnsToHide.Contains(c))
							continue;
					}

					if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
					{
						strCtlType = "" + (grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0]).GetType().ToString();
						if (strCtlType == "System.Web.UI.LiteralControl")
						{
							strValue = "" + ((LiteralControl)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.Label")
						{
							if (c == 3)
							{
								var label = grdSource.Rows[r].FindControl("lblCustomerAddress") as Label;
								if (label != null)
									strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text + "<br/> " + ((Label)grdSource.Rows[r].FindControl("lblCustomerAddress")).Text;
								else
									strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
							}
							else
							{
								strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
							}
						}
						if (strCtlType == "System.Web.UI.WebControls.LinkButton")
						{
							strValue = "" + ((LinkButton)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.HyperLink")
						{
							strValue = "" + ((HyperLink)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
						}
					}
					if (strValue == "")
					{
						strValue = "" + grdSource.Rows[r].Cells[c].Text;
					}
					str.Append("<td>" + strValue + "</td>");

				}
				str.Append("</tr>");
			}
			if (grdSource.FooterRow.Visible || showHidden)
			{

				for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
				{
					if (columnsToHide != null)
					{
						if (columnsToHide.Contains(c))
							continue;
					}

					var ct2 = grdSource.FooterRow.Cells[c].Controls.Count;
					if (ct2 == 0)
					{
						str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
					}
					else
					{
						ct2 = ct2 == 1 ? 0 : 1;
						switch (grdSource.FooterRow.Cells[c].Controls[ct2].GetType().ToString())
						{
							case "System.Web.UI.WebControls.LiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
								break;
							case "System.Web.UI.WebControls.Label": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
								break;
							case "System.Web.UI.WebControls.LinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
								break;
							case "System.Web.UI.WebControls.HyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
								break;
							case "System.Web.UI.WebControls.DataControlFieldFooterCell":
								{

									switch (grdSource.HeaderRow.Cells[c].Controls[ct2].GetType().ToString())
									{
										case "System.Web.UI.WebControls.DataControlLiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
											break;
										case "System.Web.UI.WebControls.DataControlLabel": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
											break;
										case "System.Web.UI.WebControls.DataControlLinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
											break;
										case "System.Web.UI.WebControls.DataControlHyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
											break;
									}
								}
								break;
							default: str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
								break;
						}
					}
				}
			}
			str.Append("</table>");
			str.Append("</body>");
		}
		return str;
	}

	/// <summary>
	/// Get the excel content from grid
	/// </summary>
	/// <param name="grdSource">the source grid</param>
	/// <param name="columnsToHide">array of indices o columns to hide</param>
	/// <returns></returns>
	public static StringBuilder GetExcelContentForGrid(GridView grdSource, IEnumerable<int> columnsToHide, bool showHidden = true)
	{
		StringBuilder str = new StringBuilder();
		string strValue = string.Empty, strCtlType = string.Empty;
		if (grdSource.Rows.Count > 0)
		{
			str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
			str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
			str.Append("<body topmargin=0 leftmargin=0>");
			str.Append("<table cellpadding='0' cellspacing='0'>");
			str.Append("<tr>");
			for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
			{
				if (columnsToHide.Contains(c)) // if this columns is to be hidden
					continue;

				if (!showHidden && grdSource.Columns[c].Visible == false)
					continue;

				if (!string.IsNullOrEmpty(grdSource.HeaderRow.Cells[c].Text))
					str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + grdSource.HeaderRow.Cells[c].Text + "</th>");
				else
				{
					strValue = string.Empty;
					var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
					switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
					{
						case "System.Web.UI.LiteralControl":
							strValue = "" + ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.Label":
							strValue = "" + ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.LinkButton":
							strValue = "" + ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.HyperLink":
							strValue = "" + ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.DataControlLiteralControl": strValue = "" + ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.DataControlLabel": strValue = "" + ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.DataControlLinkButton": strValue = "" + ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						case "System.Web.UI.WebControls.DataControlHyperLink": strValue = "" + ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
							break;
						default:
							strValue = "";
							break;
					}
					str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + strValue + "</th>");
				}
			}
			str.Append("</tr>");
			for (int r = 0; r < grdSource.Rows.Count; r++)
			{
				str.Append("<tr>");
				for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
				{
					if (columnsToHide.Contains(c)) // if this columns is to be hidden
						continue;

					if (!showHidden && grdSource.Columns[c].Visible == false)
						continue;

					strValue = string.Empty;
					if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
					{
						strCtlType = "" + grdSource.Rows[r].Cells[c].Controls[1].GetType().ToString();
						if (strCtlType == "System.Web.UI.LiteralControl")
						{
							strValue = "" + ((LiteralControl)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.Label")
						{
							strValue = "" + ((Label)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.LinkButton")
						{
							strValue = "" + ((LinkButton)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
						if (strCtlType == "System.Web.UI.WebControls.HyperLink")
						{
							strValue = "" + ((HyperLink)grdSource.Rows[r].Cells[c].Controls[1]).Text;
						}
					}
					if (strValue == "")
					{
						strValue = "" + grdSource.Rows[r].Cells[c].Text;
					}
					str.Append("<td>" + strValue + "</td>");

				}
				str.Append("</tr>");
			}
			for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
			{
				if (columnsToHide.Contains(c)) // if this columns is to be hidden
					continue;

				if (!showHidden && grdSource.Columns[c].Visible == false)
					continue;

				str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
			}
			str.Append("</table>");
			str.Append("</body>");
		}
		return str;
	}


	/// <summary>
	/// Method for making excel if the grid is made from js
	/// </summary>
	/// <param name="head">the head of grid concet by ":"</param>
	/// <param name="data">all data contect by "_" for row, and ":" for cell</param>
	/// <param name="foot">the foot in same format as head</param>
	/// <returns></returns>
	public static StringBuilder GetExcelContentForGrid(string head, string data, string foot)
	{
		StringBuilder str = new StringBuilder();
		string strValue = string.Empty, strCtlType = string.Empty;
		var hdCount = head.Split(':').Count();
		var dtCount = data.Split('_').Count();
		if (hdCount > 0 && dtCount > 0)
		{
			str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
			str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
			str.Append("<body topmargin=0 leftmargin=0>");
			str.Append("<table cellpadding='0' cellspacing='0'>");
			str.Append("<tr>");
			for (int c = 0; c < hdCount; c++)
			{
				str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + head.Split(':')[c].Trim() + "</th>");
			}
			str.Append("</tr>");
			for (int r = 0; r < dtCount; r++)
			{
				str.Append("<tr>");
				for (int c = 0; c < hdCount; c++)
				{
					strValue = string.Empty;
					strValue = "" + data.Split('_')[r].Split(':')[c].Trim();
					str.Append("<td>" + strValue + "</td>");
				}
				str.Append("</tr>");
			}
			for (int c = 0; c < foot.Split(':').Count(); c++)
			{
				str.Append("<th nowrap>" + foot.Split(':')[c] + "</th>");
			}
			str.Append("</table>");
			str.Append("</body>");
		}
		return str;
	}

	public static string ConvertInteger32ToWord(Int32 numToConvert)
	{
		string strNumWord = string.Empty, strNum=numToConvert.ToString();
		string strThWord = string.Empty, strHndWord = string.Empty, strTenWord = string.Empty;
		string[] strN1 = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty" };
		string[] strN2 = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
		int NumLength=strNum.Length, tmpNum=numToConvert;
		int q = 0; 

		q = Math.DivRem(tmpNum, 1000, out tmpNum);
		if (q > 20)
		{
			strThWord += " " + strN2[Math.DivRem(q, 10, out q)];
		}
		if (q > 0)
		{
			strThWord += " " + strN1[q];
		}
		strThWord = strThWord.Length > 0 ? strThWord + " thousand" : "";

		q = Math.DivRem(tmpNum, 100, out tmpNum);
		if (q > 20)
		{
			strHndWord += " " + strN2[Math.DivRem(q, 10, out q)];
		}
		if (q > 0)
		{
			strHndWord += " " + strN1[q];
		}
		strHndWord = strHndWord.Length > 0 ? strHndWord + " hundred" : "";

		q = tmpNum;
		if (q > 20)
		{
			strTenWord += " " + strN2[Math.DivRem(q, 10, out q)];
		}
		if (q > 0)
		{
			strTenWord += " " + strN1[q];
		}
		
		strNumWord = strThWord + strHndWord + strTenWord;
		return strNumWord.Trim();
	}

	public static string CheckApplicationName(System.Web.UI.Page pg)
	{
		string ApplicationName = string.Empty, ApplicationTrim = string.Empty;
		try
		{
			ApplicationName = pg.Request.ApplicationPath;
			ApplicationTrim = ApplicationName.Remove('/').ToString().Trim();
		}
		catch (Exception ex)
		{
			ApplicationTrim = "";
		}
		return ApplicationTrim;
	}

	public static string SaveBulkDataThroughDataTable(string TableName,DataTable dt)
	{
		string res = string.Empty;
		try
		{
			using (SqlBulkCopy bulkCopy =
								new SqlBulkCopy(sqlConStr, SqlBulkCopyOptions.FireTriggers))
			{
				bulkCopy.DestinationTableName = TableName;
				foreach (DataColumn col in dt.Columns)
				{
					bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
				}
				bulkCopy.WriteToServer(dt);
			} 
		}
		catch (Exception ex)
		{
			res = ex.Message.ToString();
		}
		return res;
	}

	public static bool CheckUserRightOnPage(System.Web.UI.Page pg)
	{
		string pageName = pg.Request.AppRelativeCurrentExecutionFilePath;
		string strUserTypeId = "" + pg.Session["UserType"];
		bool blnRight = false;
		SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
		SqlCommand cmd=null;
		SqlDataReader sdr=null;
        string sql = "SELECT RightToView From EntMenuRights WHERE ParentMenu<>'None' AND UserTypeId='" + strUserTypeId + "' AND FileName='" + pageName + "' AND BranchId='" + Globals.BranchID + "'";
		try
		{
			sqlcon.Open();
			cmd = new SqlCommand(sql, sqlcon);
			sdr = cmd.ExecuteReader();
			if (sdr.Read())
			{
				blnRight = sdr.GetBoolean(0);
			}
		}
		catch (Exception)
		{
			blnRight = false;
		}
		finally
		{
			sqlcon.Close();
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
		}
		if (!blnRight)
		{
			pg.Response.Redirect("~/Masters/Default.aspx?Acess=Right",false);
		}
		return blnRight;
	}
	public static bool CheckUserRightOnStockPage(System.Web.UI.Page pg)
	{
		string pageName = pg.Request.AppRelativeCurrentExecutionFilePath;
		string strUserTypeId = "" + pg.Session["UserType"];
		bool blnRight = false;
		SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
		SqlCommand cmd = null;
		SqlDataReader sdr = null;
        string sql = "SELECT RightToView From EntMenuRights WHERE PageTitle='" + SpecialAccessRightName.StockRecon + "' AND BranchId='" + Globals.BranchID + "'";
		try
		{
			sqlcon.Open();
			cmd = new SqlCommand(sql, sqlcon);
			sdr = cmd.ExecuteReader();
			if (sdr.Read())
			{
				blnRight = sdr.GetBoolean(0);
			}
		}
		catch (Exception)
		{
			blnRight = false;
		}
		finally
		{
			sqlcon.Close();
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
		}
		if (!blnRight)
		{
			pg.Response.Redirect("~/Masters/Default.aspx?Tab=False", false);
		}
		return blnRight;
	}

    public static bool CheckExportToExcelRightOnPage()
    {       
        bool blnRight = false;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = null;
        SqlDataReader sdr = null;
        string sql = "SELECT RightToView From EntMenuRights WHERE PageTitle='" + SpecialAccessRightName.ExpToExcel+ "' AND UserTypeId='" + Globals.UserType + "' AND BranchId='" + Globals.BranchID + "'";
        try
        {
            sqlcon.Open();
            cmd = new SqlCommand(sql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnRight = sdr.GetBoolean(0);
            }
        }
        catch (Exception)
        {
            blnRight = false;
        }
        finally
        {
            sqlcon.Close();
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }        
        return blnRight;
    }

	public static void SetItemInComboBox(AjaxControlToolkit.ComboBox Drp, string strMatchWith, bool matchText, bool CaseSensitiveMatch)
	{
		string strMatch = string.Empty;
		if (!CaseSensitiveMatch) strMatchWith = strMatchWith.ToUpper();
		for (int i = 0; i < Drp.Items.Count; i++)
		{
			if (matchText)
			{
				strMatch = Drp.Items[i].Text;
			}
			else
			{
				strMatch = Drp.Items[i].Value;
			}
			if (!CaseSensitiveMatch) strMatch = strMatch.ToUpper();

			if (string.Equals(strMatch, strMatchWith))
			{
				Drp.SelectedIndex = i;
				break;
			}
		}
	}

	public static DataSet GetData(SqlCommand cmd)
	{
		SqlConnection con = new SqlConnection(sqlConStr);
		SqlDataAdapter adap = new SqlDataAdapter();
		if (con.State == ConnectionState.Open)
		{
			con.Close();
		}
		else
		{
			con.Open();
		}
		DataSet ds = new DataSet();
		cmd.Connection = con;
		adap.SelectCommand = cmd;
		adap.Fill(ds, "table");
		con.Close();
		return ds;
	}
	public static SqlDataReader ExecuteReader(SqlCommand cmd)
	{

		SqlDataReader dr = null;
		SqlConnection con = null;
		try
		{
			con = new SqlConnection(sqlConStr);
			if (con.State == ConnectionState.Open)
			{
				con.Close();
			}
			else
			{
				con.Open();
			}
			cmd.Connection = con;
			dr = cmd.ExecuteReader();

		}
		catch (Exception )
		{

		}
		finally
		{
			//con.Close();
		}
		return dr;
	}
	public static string ExecuteNonQuery(SqlCommand cmd)
	{
		string saveSuccess = string.Empty;
		int execute = 0;
		SqlConnection con = null;
	  
		try
		{
			con = new SqlConnection(sqlConStr);
			if (con.State == ConnectionState.Open)
			{
				con.Close();
			}
			else
			{
				con.Open();
			}
			cmd.Connection = con;
			execute = cmd.ExecuteNonQuery();
			if (execute > 0)
			{
				saveSuccess = "Record Saved";
			}
		}
		catch (Exception excp)
		{
			if (excp.Message.Contains("Violation of PRIMARY KEY constraint"))
				saveSuccess = "unique record";
			else if (excp.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
				saveSuccess = "usae anywhere so u can not deleted";
			else if (excp.Message.Contains("Violation of UNIQUE KEY constraint"))
				saveSuccess = "unique record";
			else if (excp.Message.Contains("Object reference not set to an instance of an object"))
				saveSuccess = "Object are not set anywhere";
			else if (excp.Message.Contains("Error converting data type nvarchar to float."))
				saveSuccess = "Invalid opening balance.";
			else
				saveSuccess = excp.Message.ToString();
		}
		finally
		{
			con.Close();
		}
		return saveSuccess;
	}

	public static bool SendSMS(string strUid, string strPwd, string strSenderId, string strSMSToMobileNo, string strMessage)
	{
		WebClient Client = new WebClient();
		string sendString = "username=" + strUid + "&password=" + strPwd + "&sender=" + strSenderId + "&to=" + strSMSToMobileNo + "&message=" + strMessage + "&priority=" + "4";
		string strURLToSMS = "http://smsweb.co.in/pushsms.php?" + sendString;

		//string sendString = "username=" + strUid + "&password=" + strPwd + "&sendername=" + strSenderId + "&mobileno=" + strSMSToMobileNo + "&message=" + strMessage;
		//string strURLToSMS = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?"+sendString;

		try
		{
			Stream data = Client.OpenRead(strURLToSMS);
			StreamReader reader = new StreamReader(data);
			string s = reader.ReadToEnd();
			data.Close();
			reader.Close();
			return true;
			//Response.Write("Send");
		}
		catch (Exception)
		{
			//Response.Write(ex.Message);
			return false;
		}
	}
	public static bool SendSMSmassage(string strUid, string strPwd, string strSenderId, string strapi, string strSMSToMobileNo, string strMessage, string BID)
	{
		DTO.sms Ob = new DTO.sms();
		DataSet ds = new DataSet();
		Ob.BranchId = BID;
		ds = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);

		WebClient Client = new WebClient();
		string sendString = string.Empty;
		//"username=" + strUid + "&password=" + strPwd + "&sender=" + strSenderId + "&to=" + strSMSToMobileNo + "&message=" + strMessage ;

		for (int i = 2; i <= 6; i++)
		{
			if (i == Int32.Parse(ds.Tables[0].Rows[0]["userposition"].ToString()))
			{
				if (i == 2)
				{
					sendString = sendString + ds.Tables[0].Rows[0]["useriddemo"].ToString() + "=" + strUid;
				}
				else
				{
					sendString = sendString + "&" + ds.Tables[0].Rows[0]["useriddemo"].ToString() + "=" + strUid;
				}
			}
			if (i == Int32.Parse(ds.Tables[0].Rows[0]["passwordposition"].ToString()))
			{
				if (i == 2)
				{
					sendString = sendString + ds.Tables[0].Rows[0]["passworddemo"].ToString() + "=" + strPwd;
				}
				else
				{
					sendString = sendString + "&" + ds.Tables[0].Rows[0]["passworddemo"].ToString() + "=" + strPwd;
				}
			}
			if (i == Int32.Parse(ds.Tables[0].Rows[0]["senderposition"].ToString()))
			{
				if (i == 2)
				{
					sendString = sendString + ds.Tables[0].Rows[0]["senderdemo"].ToString() + "=" + strSenderId;
				}
				else
				{
					sendString = sendString + "&" + ds.Tables[0].Rows[0]["senderdemo"].ToString() + "=" + strSenderId;
				}
			}
			if (i == Int32.Parse(ds.Tables[0].Rows[0]["mobileposition"].ToString()))
			{
				if (i == 2)
				{
					sendString = sendString + ds.Tables[0].Rows[0]["mobiledemo"].ToString() + "=" + strSMSToMobileNo;
				}
				else
				{
					sendString = sendString + "&" + ds.Tables[0].Rows[0]["mobiledemo"].ToString() + "=" + strSMSToMobileNo;
				}
			}
			if (i == Int32.Parse(ds.Tables[0].Rows[0]["massageposition"].ToString()))
			{
				if (i == 2)
				{
					sendString = sendString + ds.Tables[0].Rows[0]["massagedemo"].ToString() + "=" + strMessage;
				}
				else
				{
					sendString = sendString + "&" + ds.Tables[0].Rows[0]["massagedemo"].ToString() + "=" + strMessage;
				}
			}

		}
		// Editing For Oman
		//string strURLToSMS = strapi + sendString;
		string strURLToSMS = strapi + sendString + "&source=" + "TLPSYS";

		try
		{

			Stream data = Client.OpenRead(strURLToSMS);
			StreamReader reader = new StreamReader(data);
			string s = reader.ReadToEnd();
			data.Close();
			reader.Close();
			return true;
			//Response.Write("Send");
		}
		catch (Exception)
		{
			//Response.Write(ex.Message);
			return false;
		}
	}
	public static string GoMsg(string BID, string BookingNo, string MsgType)
	{
		string msg = string.Empty;
		WebClient Client = new WebClient();
		try
		{
			int flag = 0;
			string mobileNo = string.Empty, CustName = string.Empty, duedate = string.Empty, process = string.Empty, CheckMsg = string.Empty;
			CheckMsg = BAL.BALFactory.Instance.BAL_sms.GetsmsTemplateName(BID, MsgType);
			if (CheckMsg != "No Message")
			{
				DataSet ds = new DataSet();
				ds = BAL.BALFactory.Instance.BAL_New_Bookings.GetSMSInformation(BookingNo,BID);
				if (ds.Tables[0].Rows.Count > 0)
				{
					mobileNo = ds.Tables[0].Rows[0]["CustMobile"].ToString();
					CustName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
					duedate = ds.Tables[0].Rows[0]["duedate"].ToString();
					string strUid = string.Empty, strPwd = string.Empty, strSenderId = string.Empty, strapi = string.Empty, strMessage = string.Empty, strSMSToMobileNo = "";
					strSMSToMobileNo += mobileNo;

					DTO.sms Ob = new DTO.sms();
					DataSet dsapi = new DataSet();
					Ob.BranchId = BID;
					dsapi = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);
					if (dsapi.Tables[0].Rows.Count > 0)
					{
						strUid = dsapi.Tables[0].Rows[0]["userid"].ToString();
						strPwd = dsapi.Tables[0].Rows[0]["password"].ToString();
						strSenderId = dsapi.Tables[0].Rows[0]["senderid"].ToString();
						strapi = dsapi.Tables[0].Rows[0]["api"].ToString();
					}

					Ob.bookingsms = MsgType;
					Ob.BranchId = BID;
					DataSet dspre = new DataSet();
					dspre = BAL.BALFactory.Instance.BAL_sms.previewbooking(Ob);
					string databasemessage = dspre.Tables[0].Rows[0]["massage"].ToString();
					Dictionary<string, string> replacements = new Dictionary<string, string>()
			{
			{"[Customer Name]",ds.Tables[0].Rows[0]["CustomerName"].ToString()},
			{"[Booking No]",BookingNo},
			{"[Amount]",ds.Tables[0].Rows[0]["TotalAmount"].ToString()},
			{"[Quantity]",ds.Tables[0].Rows[0]["TotalQty"].ToString()},
            {"[Booking Date]", ds.Tables[0].Rows[0]["bookingdate"].ToString()},
			{"[Delivery Date]", ds.Tables[0].Rows[0]["duedate"].ToString()},
			{"[Mobile No]", mobileNo},
			{"[SenderId]", strSenderId},
			{"[Password]",strPwd},
			{"[User Name]", strUid}            
			};

					foreach (var r in replacements)
					{
						databasemessage = databasemessage.Replace(r.Key, r.Value);
					}

					strMessage = databasemessage.Replace("amp;", "").Trim();
                    string DecodedString = WebUtility.HtmlDecode(strMessage);
				   // bool IsSms = AppClass.SendSMSmassage(strUid, strPwd, strSenderId, strapi, strSMSToMobileNo, strMessage, BID);
					try
					{
                        Stream data = Client.OpenRead(DecodedString);
						StreamReader reader = new StreamReader(data);
						string s = reader.ReadToEnd();
						data.Close();
						reader.Close();
						flag = 1;                        
					}
					catch (Exception)
					{
						flag = 0;
					}
				}               
			}
		}
		catch (Exception ex)
		{ }
		return msg;
	}

    public static string GoAssignPackagekMsg(string BID, string MobileNo, string MsgType, string CustomerName, string PackageName, string PackageAmount,string EndDate)
    {
        string msg = string.Empty;
        WebClient Client = new WebClient();
        string strUid = string.Empty, strPwd = string.Empty, strSenderId = string.Empty, strapi = string.Empty, strMessage = string.Empty;
        try
        {
            string CheckMsg = string.Empty;
            CheckMsg = BAL.BALFactory.Instance.BAL_sms.GetsmsTemplateName(BID, MsgType);
            if (CheckMsg != "No Message")
            {
                DTO.sms Ob = new DTO.sms();
                DataSet dsapi = new DataSet();
                Ob.BranchId = BID;
                dsapi = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);
                if (dsapi.Tables[0].Rows.Count > 0)
                {
                    strUid = dsapi.Tables[0].Rows[0]["userid"].ToString();
                    strPwd = dsapi.Tables[0].Rows[0]["password"].ToString();
                    strSenderId = dsapi.Tables[0].Rows[0]["senderid"].ToString();
                    strapi = dsapi.Tables[0].Rows[0]["api"].ToString();
                    Ob.bookingsms = MsgType;
                    Ob.BranchId = BID;
                    DataSet dspre = new DataSet();
                    dspre = BAL.BALFactory.Instance.BAL_sms.previewbooking(Ob);
                    string databasemessage = dspre.Tables[0].Rows[0]["massage"].ToString();
                    Dictionary<string, string> replacements = new Dictionary<string, string>()
			{
			{"[Customer Name]",CustomerName},
			{"[Package Name]",PackageName},
			{"[Amount]",PackageAmount},
			{"[Quantity]",""},
			{"[Expiry Date]", EndDate},
			{"[Mobile No]", MobileNo},
			{"[SenderId]", strSenderId},
			{"[Password]",strPwd},
			{"[User Name]", strUid}            
			};

                    foreach (var r in replacements)
                    {
                        databasemessage = databasemessage.Replace(r.Key, r.Value);
                    }
                    strMessage = databasemessage.Replace("amp;", "").Trim();
                    string DecodedString = WebUtility.HtmlDecode(strMessage);
                    try
                    {
                        Stream data = Client.OpenRead(DecodedString);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return msg;
    }

    public static string GoPendingReportExpireOrRenewalMsg(string BID, string MobileNo, string MsgType, string CustomerName, string expiryDate, string PackageName)
    {
        string msg = string.Empty;
        WebClient Client = new WebClient();
        string strUid = string.Empty, strPwd = string.Empty, strSenderId = string.Empty, strapi = string.Empty, strMessage = string.Empty;
        try
        {
            string CheckMsg = string.Empty;
            CheckMsg = BAL.BALFactory.Instance.BAL_sms.GetsmsTemplateName(BID, MsgType);
            if (CheckMsg != "No Message")
            {
                DTO.sms Ob = new DTO.sms();
                DataSet dsapi = new DataSet();
                Ob.BranchId = BID;
                dsapi = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);
                if (dsapi.Tables[0].Rows.Count > 0)
                {
                    strUid = dsapi.Tables[0].Rows[0]["userid"].ToString();
                    strPwd = dsapi.Tables[0].Rows[0]["password"].ToString();
                    strSenderId = dsapi.Tables[0].Rows[0]["senderid"].ToString();
                    strapi = dsapi.Tables[0].Rows[0]["api"].ToString();
                    Ob.bookingsms = MsgType;
                    Ob.BranchId = BID;
                    DataSet dspre = new DataSet();
                    dspre = BAL.BALFactory.Instance.BAL_sms.previewbooking(Ob);
                    string databasemessage = dspre.Tables[0].Rows[0]["massage"].ToString();
                    Dictionary<string, string> replacements = new Dictionary<string, string>()
			{
			{"[Customer Name]",CustomerName},
			{"[Booking No]",""},
			{"[Expiry Date]",expiryDate},
			{"[Package Name]",PackageName},
			{"[Delivery Date]", ""},
			{"[Mobile No]", MobileNo},
			{"[SenderId]", strSenderId},
			{"[Password]",strPwd},
			{"[User Name]", strUid}            
			};

                    foreach (var r in replacements)
                    {
                        databasemessage = databasemessage.Replace(r.Key, r.Value);
                    }
                    strMessage = databasemessage.Replace("amp;", "").Trim();
                    string DecodedString = WebUtility.HtmlDecode(strMessage);
                    try
                    {
                        Stream data = Client.OpenRead(DecodedString);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return msg;
    }

    public static string GoPendingStockMsg(string BID, string MobileNo, string MsgType, string CustomerName, string RemainingPcs, string RemainigAmount)
    {
        string msg = string.Empty;
        WebClient Client = new WebClient();
        string strUid = string.Empty, strPwd = string.Empty, strSenderId = string.Empty, strapi = string.Empty, strMessage = string.Empty;
        try
        {
            string CheckMsg = string.Empty;
            CheckMsg = BAL.BALFactory.Instance.BAL_sms.GetsmsTemplateName(BID, MsgType);
            if (CheckMsg != "No Message")
            {
                DTO.sms Ob = new DTO.sms();
                DataSet dsapi = new DataSet();
                Ob.BranchId = BID;
                dsapi = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);
                if (dsapi.Tables[0].Rows.Count > 0)
                {
                    strUid = dsapi.Tables[0].Rows[0]["userid"].ToString();
                    strPwd = dsapi.Tables[0].Rows[0]["password"].ToString();
                    strSenderId = dsapi.Tables[0].Rows[0]["senderid"].ToString();
                    strapi = dsapi.Tables[0].Rows[0]["api"].ToString();
                    Ob.bookingsms = MsgType;
                    Ob.BranchId = BID;
                    DataSet dspre = new DataSet();
                    dspre = BAL.BALFactory.Instance.BAL_sms.previewbooking(Ob);
                    string databasemessage = dspre.Tables[0].Rows[0]["massage"].ToString();
                    Dictionary<string, string> replacements = new Dictionary<string, string>()
			{
			{"[Customer Name]",CustomerName},
			{"[Booking No]",""},
			{"[Amount]",RemainigAmount},
			{"[Quantity]",RemainingPcs},
			{"[Delivery Date]", ""},
			{"[Mobile No]", MobileNo},
			{"[SenderId]", strSenderId},
			{"[Password]",strPwd},
			{"[User Name]", strUid}            
			};

                    foreach (var r in replacements)
                    {
                        databasemessage = databasemessage.Replace(r.Key, r.Value);
                    }
                    strMessage = databasemessage.Replace("amp;", "").Trim();
                    string DecodedString = WebUtility.HtmlDecode(strMessage);
                    try
                    {
                        Stream data = Client.OpenRead(DecodedString);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return msg;
    }
	/// <summary>
	/// Calculates the sum of all rows where applicable and shows the total in footer
	/// </summary>
	/// <param name="gridView">The grid to calculate total of</param>
	/// <param name="tableIndex">The table index of datatable which is the actual source of gridview, defaults to 0</param>
	/// <param name="showToalLabelOnColumn">The column where to show the text "Total" defaults to 0</param>
	internal static void CalcuateAndSetGridFooter(ref GridView gridView, int tableIndex = 0, int showToalLabelOnColumn = 0)
	{
		/*
		if (gridView == null || dataSource == null) return;
		if (dataSource.Tables.Count == 0) return;
		if (dataSource.Tables[tableIndex].Rows.Count == 0) return;
		*/


		try
		{

			var sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT RightToView FROM EntMenuRights WHERE PageTitle='" + SpecialAccessRightName.ShowTotal+ "' AND UserTypeId=" + Globals.UserType + " AND BranchId=" + Globals.BranchID + "";
			var sqlReader = PrjClass.ExecuteReader(sqlCommand);
			if (sqlReader != null && sqlReader.Read())
			{
				if (!sqlReader.GetBoolean(0))
					return;
			}
			sqlReader.Close();

			// the list that will hold the sum(s)
			var lstFltTotals = new List<float>();
			// the dict mapping that will tell us which colcumn/cell wasn't computable
			var dict = new Dictionary<int, string>();
			// the float value, if calulated
			var floatValue = 0.0f;
			// the string key, in case col is non computable
			var str = string.Empty;
			// for each col
			// Now in the expense report, the col count is 0, but still there are 4 cells
			// may be because it has autogenerate cols, (or may be not)
			// but to circumvent around it, we will ALSO check for cells
			var cellMaxCount = gridView.Columns.Count;
			if (gridView.Rows.Count != 0)
			{
				if (gridView.Rows[0].Cells.Count != gridView.Columns.Count)
					cellMaxCount = gridView.Rows[0].Cells.Count;
			}

			for (var c = 0; c < cellMaxCount; c++)
			{
				// this will hold the total for each cell
				var FltTotalsCounter = 0f;
				// for each row
				for (var r = 0; r < gridView.Rows.Count; r++)
				{
					var canParse = false;
					if (gridView.Rows[r].Cells[c].Controls.Count == 0)
						canParse = float.TryParse(gridView.Rows[r].Cells[c].Text, out floatValue);
					else if (gridView.Rows[r].Cells[c].Controls.Count == 1 || gridView.Rows[r].Cells[c].Controls.Count == 2 || gridView.Rows[r].Cells[c].Controls.Count == 3)
					{
						var controlToCheck = ((gridView.Rows[r].Cells[c].Controls.Count == 1) ? (0) : (gridView.Rows[r].Cells[c].Controls.Count == 3 ? 1 : 0));
						switch (gridView.Rows[r].Cells[c].Controls[controlToCheck].GetType().ToString())
						{
							case "System.Web.UI.WebControls.LiteralControl": canParse = float.TryParse(((LiteralControl)(gridView.Rows[r].Cells[c].Controls[controlToCheck])).Text, out floatValue);
								break;
							case "System.Web.UI.WebControls.Label": canParse = float.TryParse(((Label)(gridView.Rows[r].Cells[c].Controls[controlToCheck])).Text, out floatValue);
								break;
							case "System.Web.UI.WebControls.LinkButton": canParse = float.TryParse(((LinkButton)(gridView.Rows[r].Cells[c].Controls[controlToCheck])).Text, out floatValue);
								break;
							case "System.Web.UI.WebControls.HyperLink": canParse = float.TryParse(((HyperLink)(gridView.Rows[r].Cells[c].Controls[controlToCheck])).Text, out floatValue);
								break;
						}
					}

					// if the value can be parsed
					if (canParse)
						// add it to the totals
						FltTotalsCounter += floatValue;
					// if it cannot be parsed
					else
					{
						// add it to the dict
						dict.Add(c, string.Empty);
						break;
					}
				}
				// now add this total to the list of totals
				lstFltTotals.Add((float)Math.Round(FltTotalsCounter, 2));
			}

			// now for each footer
			gridView.FooterRow.Visible = true;
			gridView.FooterRow.BackColor = System.Drawing.Color.Aqua;
			for (var cell = 0; cell < cellMaxCount; cell++)
			{
				// if cell is same as the param passed, set the text to "Total"
				if (cell == showToalLabelOnColumn)
					gridView.FooterRow.Cells[cell].Text = "Total";
				else
				{
					// if the dict contains this key, that means this col is non computable, so we set the footer text to empty string
					if (dict.ContainsKey(cell))
						gridView.FooterRow.Cells[cell].Text = string.Empty;
					else
						gridView.FooterRow.Cells[cell].Text = Math.Round(lstFltTotals.ElementAt(cell), 2).ToString();
				}
			}
		}
		catch (Exception)
		{

		}

	}

    public static bool CheckButtonRights(string PageTitle)
    {
        bool rights = false;
        var sqlCommand = new SqlCommand();
        sqlCommand.CommandText = "SELECT RightToView FROM dbo.EntMenuRights WHERE (PageTitle = '" + PageTitle + "') AND UserTypeId=" + Globals.UserType + " AND BranchId=" + Globals.BranchID + "";
        var sqlReader = PrjClass.ExecuteReader(sqlCommand);
        if (sqlReader.Read())
        {
            rights = sqlReader.GetBoolean(0);
            sqlReader.Dispose();
            sqlReader.Close();
            sqlCommand.Dispose();            
        }
        else
        {
            rights = false;
            sqlReader.Dispose();
            sqlReader.Close();
            sqlCommand.Dispose();   
        }
        return rights;
    }

	public static bool GetShowFooterRightsUser()
	{
		bool rights = false;
		var sqlCommand = new SqlCommand();
        sqlCommand.CommandText = "SELECT RightToView FROM EntMenuRights WHERE PageTitle='" + SpecialAccessRightName.ShowTotal+ "' AND UserTypeId=" + Globals.UserType + " AND BranchId=" + Globals.BranchID + "";
		var sqlReader = PrjClass.ExecuteReader(sqlCommand);
		if (sqlReader.Read())
			rights = sqlReader.GetBoolean(0);
		else
			rights = false;

		return rights;
	}

	/// <summary>
	/// Gets the corresponding text of given column from a Grid where checkbox is checked on a row
	/// </summary>
	/// <param name="gridView">The GridView<GridView</param>
	/// <param name="checkBoxCellIndex">The index of checkbox column</param>
	/// <param name="checkBoxControlIndex">The index of checkbox in control collection of that cell/column</param>
	/// <param name="cellIndexToReturn">The index of cell for which the corresponding text would be returned</param>
	/// <param name="cellToReturnHasControls">weather that cell whose text will be returned also contains controls</param>
	/// <param name="cellToReturnControl">the control index in the cell to return the text</param>
	/// <returns></returns>
	internal static IEnumerable<string> GetCheckedCheckBox(GridView gridView, int checkBoxCellIndex, int checkBoxControlIndex, int cellIndexToReturn, bool cellToReturnHasControls = false, int cellToReturnControl = 0)
	{
		var returnEmpty = true;
		try
		{

			for (var i = 0; i <= gridView.Rows.Count - 1; i++)
			{
				if (gridView.Rows[i].RowType == DataControlRowType.Header || gridView.Rows[i].RowType == DataControlRowType.Footer)
					continue;

				if (((CheckBox)gridView.Rows[i].Cells[checkBoxCellIndex].Controls[checkBoxControlIndex]).Checked)
				{
					if (cellToReturnHasControls)
						yield return ((Label)gridView.Rows[i].Cells[cellIndexToReturn].Controls[cellToReturnControl]).Text;
					else
						yield return gridView.Rows[i].Cells[cellIndexToReturn].Text;
				}

			}
			returnEmpty = false;
		}
		finally
		{
		}
		if (returnEmpty)
			yield return string.Empty;
	}

    public static bool CheckUserRightOnPageForFactory(System.Web.UI.Page pg)
    {
        string pageName = pg.Request.AppRelativeCurrentExecutionFilePath;
        string strUserTypeId = "" + pg.Session["WorkshopUserType"].ToString();
        bool blnRight = false;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = null;
        SqlDataReader sdr = null;
        string sql = "SELECT RightToView From EntMenuRights WHERE UserTypeId=4   AND WorkshopUserType='" + strUserTypeId + "' AND FileName='" + pageName + "'";
        try
        {
            sqlcon.Open();
            cmd = new SqlCommand(sql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnRight = sdr.GetBoolean(0);
            }
        }
        catch (Exception)
        {
            blnRight = false;
        }
        finally
        {
            sqlcon.Close();
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        if (!blnRight)
        {
            pg.Response.Redirect("~/Factory/frmFactoryHome.aspx", false);
        }
        return blnRight;
    }

    public static StringBuilder GetExcelContentForGridBookingNew(GridView grdSource, string FromDate , string ToDate ,string ReportName, bool showHidden = true, IEnumerable<int> columnsToHide = null)
    {
        StringBuilder str = new StringBuilder();
        string strValue = string.Empty, strCtlType = string.Empty;
        ArrayList date = new ArrayList();
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        int grdColCount = grdSource.Columns.Count;
        if (grdSource.Rows.Count > 0)
        {
            str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
            str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
            str.Append("<body topmargin=0 leftmargin=0>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: medium;font-weight: bold'>" + ReportName + "</td></tr>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: font-size: 16px;'><span style='font-weight: bold;'> " + FromDate + "</span>&nbsp;&nbsp;To&nbsp;&nbsp;<span style='font-weight: bold;'>" + ToDate + "</span>&nbsp;&nbsp;&nbsp;&nbsp;Generated By&nbsp;<span style='font-weight: bold;'>" + Globals.UserName + "&nbsp;@&nbsp;" + date[0].ToString() + "&nbsp;" + date[1].ToString() + "</span> </td></tr>");
            str.Append("<tr>");
            for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
            {
                if (columnsToHide != null)
                {
                    if (columnsToHide.Contains(c))
                        continue;
                }

                var type = grdSource.HeaderRow.Cells[c].GetType().ToString();
                var lbl = string.Empty;
                switch (type)
                {
                    case "System.Web.UI.WebControls.LiteralControl": lbl = ((LiteralControl)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.Label": lbl = ((Label)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.LinkButton": lbl = ((LinkButton)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.HyperLink": lbl = ((HyperLink)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.DataControlFieldHeaderCell":
                        {
                            if (grdSource.HeaderRow.Cells[c].Controls.Count >= 1)
                            {
                                var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
                                switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.DataControlLiteralControl": lbl = ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLabel": lbl = ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLinkButton": lbl = ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlHyperLink": lbl = ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                }
                            }
                            else
                            {
                                lbl = grdSource.HeaderRow.Cells[c].Text;
                            }
                        }
                        break;
                    default: lbl = grdSource.HeaderRow.Cells[c].Text;
                        break;
                }
                str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + lbl + "</th>");
            }
            str.Append("</tr>");
            for (int r = 0; r < grdSource.Rows.Count; r++)
            {
                if (!showHidden && grdSource.Rows[r].Visible == false)
                    continue;



                str.Append("<tr>");
                for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
                {
                    strValue = string.Empty;

                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        strCtlType = "" + (grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0]).GetType().ToString();
                        if (strCtlType == "System.Web.UI.LiteralControl")
                        {
                            strValue = "" + ((LiteralControl)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.Label")
                        {
                            if (c == 3)
                            {
                                var label = grdSource.Rows[r].FindControl("lblCustomerAddress") as Label;
                                if (label != null)
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text + "<br/> " + ((Label)grdSource.Rows[r].FindControl("lblCustomerAddress")).Text;
                                else
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                            else
                            {
                                strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                        }
                        if (strCtlType == "System.Web.UI.WebControls.LinkButton")
                        {
                            strValue = "" + ((LinkButton)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.HyperLink")
                        {
                            strValue = "" + ((HyperLink)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                    }
                    if (strValue == "")
                    {
                        strValue = "" + grdSource.Rows[r].Cells[c].Text;
                    }
                    str.Append("<td>" + strValue + "</td>");

                }
                str.Append("</tr>");
            }
            if (grdSource.FooterRow.Visible || showHidden)
            {

                for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
                {
                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    var ct2 = grdSource.FooterRow.Cells[c].Controls.Count;
                    if (ct2 == 0)
                    {
                        str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                    }
                    else
                    {
                        ct2 = ct2 == 1 ? 0 : 1;
                        switch (grdSource.FooterRow.Cells[c].Controls[ct2].GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.LiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.Label": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.LinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.HyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.DataControlFieldFooterCell":
                                {

                                    switch (grdSource.HeaderRow.Cells[c].Controls[ct2].GetType().ToString())
                                    {
                                        case "System.Web.UI.WebControls.DataControlLiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLabel": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlHyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                    }
                                }
                                break;
                            default: str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                                break;
                        }
                    }
                }
            }
            str.Append("</table>");
            str.Append("</body>");
        }
        return str;
    }


    public static StringBuilder GetExcelContentForGridBookingWithoutDate(GridView grdSource, string ReportName, bool showHidden = true, IEnumerable<int> columnsToHide = null)
    {
        StringBuilder str = new StringBuilder();
        string strValue = string.Empty, strCtlType = string.Empty;
        ArrayList date = new ArrayList();
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);      
        int hideColCount =0;
        if (columnsToHide != null)
        {
            hideColCount = columnsToHide.Count();
        }
        int grdColCount = grdSource.HeaderRow.Cells.Count - hideColCount;
        if (grdSource.Rows.Count > 0)
        {
            str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
            str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
            str.Append("<body topmargin=0 leftmargin=0>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: medium;font-weight: bold'>" + ReportName + "</td></tr>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: font-size: 16px;'><span style='font-weight: bold;'>&nbsp;</span><span style='font-weight: bold;font-size:17px'>" +Globals.StoreName + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Generated By&nbsp;<span style='font-weight: bold;'>" + Globals.UserName + "&nbsp;@&nbsp;" + date[0].ToString() + "&nbsp;" + date[1].ToString() + "</span> </td></tr>");
            str.Append("<tr>");
            for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
            {
                if (columnsToHide != null)
                {
                    if (columnsToHide.Contains(c))
                        continue;
                }

                var type = grdSource.HeaderRow.Cells[c].GetType().ToString();
                var lbl = string.Empty;
                switch (type)
                {
                    case "System.Web.UI.WebControls.LiteralControl": lbl = ((LiteralControl)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.Label": lbl = ((Label)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.LinkButton": lbl = ((LinkButton)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.HyperLink": lbl = ((HyperLink)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.DataControlFieldHeaderCell":
                        {
                            if (grdSource.HeaderRow.Cells[c].Controls.Count >= 1)
                            {
                                var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
                                switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.DataControlLiteralControl": lbl = ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLabel": lbl = ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLinkButton": lbl = ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlHyperLink": lbl = ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                }
                            }
                            else
                            {
                                lbl = grdSource.HeaderRow.Cells[c].Text;
                            }
                        }
                        break;
                    default: lbl = grdSource.HeaderRow.Cells[c].Text;
                        break;
                }
                str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + lbl + "</th>");
            }
            str.Append("</tr>");
            for (int r = 0; r < grdSource.Rows.Count; r++)
            {
                if (!showHidden && grdSource.Rows[r].Visible == false)
                    continue;



                str.Append("<tr>");
                for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
                {
                    strValue = string.Empty;

                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        strCtlType = "" + (grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0]).GetType().ToString();
                        if (strCtlType == "System.Web.UI.LiteralControl")
                        {
                            strValue = "" + ((LiteralControl)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.Label")
                        {
                            if (c == 3)
                            {
                                var label = grdSource.Rows[r].FindControl("lblCustomerAddress") as Label;
                                if (label != null)
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text + "<br/> " + ((Label)grdSource.Rows[r].FindControl("lblCustomerAddress")).Text;
                                else
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                            else
                            {
                                strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                        }
                        if (strCtlType == "System.Web.UI.WebControls.LinkButton")
                        {
                            strValue = "" + ((LinkButton)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.HyperLink")
                        {
                            strValue = "" + ((HyperLink)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                    }
                    if (strValue == "")
                    {
                        strValue = "" + grdSource.Rows[r].Cells[c].Text;
                    }
                    str.Append("<td>" + strValue + "</td>");

                }
                str.Append("</tr>");
            }
            if (grdSource.FooterRow.Visible || showHidden)
            {

                for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
                {
                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    var ct2 = grdSource.FooterRow.Cells[c].Controls.Count;
                    if (ct2 == 0)
                    {
                        str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                    }
                    else
                    {
                        ct2 = ct2 == 1 ? 0 : 1;
                        switch (grdSource.FooterRow.Cells[c].Controls[ct2].GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.LiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.Label": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.LinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.HyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.DataControlFieldFooterCell":
                                {

                                    switch (grdSource.HeaderRow.Cells[c].Controls[ct2].GetType().ToString())
                                    {
                                        case "System.Web.UI.WebControls.DataControlLiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLabel": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlHyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                    }
                                }
                                break;
                            default: str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                                break;
                        }
                    }
                }
            }
            str.Append("</table>");
            str.Append("</body>");
        }
        return str;
    }

    public static StringBuilder GetExcelContentForDynamicColumn(GridView grdSource, string FromDate, string ToDate, string ReportName, string grdColCount, bool showHidden = true, IEnumerable<int> columnsToHide = null)
    {
        StringBuilder str = new StringBuilder();
        string strValue = string.Empty, strCtlType = string.Empty;
        ArrayList date = new ArrayList();
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);     
        if (grdSource.Rows.Count > 0)
        {
            str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
            str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
            str.Append("<body topmargin=0 leftmargin=0>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<tr><td colspan='" + Convert.ToInt32(grdColCount) + "' style='text-align:center;font-size: medium;font-weight: bold'>" + ReportName + "</td></tr>");
            str.Append("<tr><td colspan='" + Convert.ToInt32(grdColCount) + "' style='text-align:center;font-size: font-size: 16px;'><span style='font-weight: bold;'> " + FromDate + "</span>&nbsp;&nbsp;To&nbsp;&nbsp;<span style='font-weight: bold;'>" + ToDate + "</span>&nbsp;&nbsp;&nbsp;&nbsp;Generated By&nbsp;<span style='font-weight: bold;'>" + Globals.UserName + "&nbsp;@&nbsp;" + date[0].ToString() + "&nbsp;" + date[1].ToString() + "</span> </td></tr>");
            str.Append("<tr>");
            for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
            {
                if (columnsToHide != null)
                {
                    if (columnsToHide.Contains(c))
                        continue;
                }

                var type = grdSource.HeaderRow.Cells[c].GetType().ToString();
                var lbl = string.Empty;
                switch (type)
                {
                    case "System.Web.UI.WebControls.LiteralControl": lbl = ((LiteralControl)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.Label": lbl = ((Label)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.LinkButton": lbl = ((LinkButton)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.HyperLink": lbl = ((HyperLink)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.DataControlFieldHeaderCell":
                        {
                            if (grdSource.HeaderRow.Cells[c].Controls.Count >= 1)
                            {
                                var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
                                switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.DataControlLiteralControl": lbl = ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLabel": lbl = ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLinkButton": lbl = ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlHyperLink": lbl = ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                }
                            }
                            else
                            {
                                lbl = grdSource.HeaderRow.Cells[c].Text;
                            }
                        }
                        break;
                    default: lbl = grdSource.HeaderRow.Cells[c].Text;
                        break;
                }
                str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + lbl + "</th>");
            }
            str.Append("</tr>");
            for (int r = 0; r < grdSource.Rows.Count; r++)
            {
                if (!showHidden && grdSource.Rows[r].Visible == false)
                    continue;



                str.Append("<tr>");
                for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
                {
                    strValue = string.Empty;

                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        strCtlType = "" + (grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0]).GetType().ToString();
                        if (strCtlType == "System.Web.UI.LiteralControl")
                        {
                            strValue = "" + ((LiteralControl)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.Label")
                        {
                            if (c == 3)
                            {
                                var label = grdSource.Rows[r].FindControl("lblCustomerAddress") as Label;
                                if (label != null)
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text + "<br/> " + ((Label)grdSource.Rows[r].FindControl("lblCustomerAddress")).Text;
                                else
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                            else
                            {
                                strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                        }
                        if (strCtlType == "System.Web.UI.WebControls.LinkButton")
                        {
                            strValue = "" + ((LinkButton)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.HyperLink")
                        {
                            strValue = "" + ((HyperLink)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                    }
                    if (strValue == "")
                    {
                        strValue = "" + grdSource.Rows[r].Cells[c].Text;
                    }
                    str.Append("<td>" + strValue + "</td>");

                }
                str.Append("</tr>");
            }
            if (grdSource.FooterRow.Visible || showHidden)
            {

                for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
                {
                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    var ct2 = grdSource.FooterRow.Cells[c].Controls.Count;
                    if (ct2 == 0)
                    {
                        str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                    }
                    else
                    {
                        ct2 = ct2 == 1 ? 0 : 1;
                        switch (grdSource.FooterRow.Cells[c].Controls[ct2].GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.LiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.Label": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.LinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.HyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.DataControlFieldFooterCell":
                                {

                                    switch (grdSource.HeaderRow.Cells[c].Controls[ct2].GetType().ToString())
                                    {
                                        case "System.Web.UI.WebControls.DataControlLiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLabel": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlHyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                    }
                                }
                                break;
                            default: str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                                break;
                        }
                    }
                }
            }
            str.Append("</table>");
            str.Append("</body>");
        }
        return str;
    }


    public static StringBuilder GetExcelContentWithoutDate(GridView grdSource, string ReportName, bool showHidden = true, IEnumerable<int> columnsToHide = null)
    {
        StringBuilder str = new StringBuilder();
        string strValue = string.Empty, strCtlType = string.Empty;
        ArrayList date = new ArrayList();
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        int hideColCount = 0;
        if (columnsToHide != null)
        {
            hideColCount = columnsToHide.Count();
        }
        int grdColCount = grdSource.HeaderRow.Cells.Count - hideColCount;
        if (grdSource.Rows.Count > 0)
        {
            str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
            str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
            str.Append("<body topmargin=0 leftmargin=0>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: medium;font-weight: bold'>" + ReportName + "</td></tr>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: font-size: 16px;'><span style='font-weight: bold;'>&nbsp;</span><span style='font-weight: bold;font-size:17px'>&nbsp;</span>&nbsp;&nbsp;Generated By&nbsp;<span style='font-weight: bold;'>" + Globals.UserName + "&nbsp;@&nbsp;" + date[0].ToString() + "&nbsp;" + date[1].ToString() + "</span> </td></tr>");
            str.Append("<tr>");
            for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
            {
                if (columnsToHide != null)
                {
                    if (columnsToHide.Contains(c))
                        continue;
                }

                var type = grdSource.HeaderRow.Cells[c].GetType().ToString();
                var lbl = string.Empty;
                switch (type)
                {
                    case "System.Web.UI.WebControls.LiteralControl": lbl = ((LiteralControl)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.Label": lbl = ((Label)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.LinkButton": lbl = ((LinkButton)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.HyperLink": lbl = ((HyperLink)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.DataControlFieldHeaderCell":
                        {
                            if (grdSource.HeaderRow.Cells[c].Controls.Count >= 1)
                            {
                                var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
                                switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.DataControlLiteralControl": lbl = ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLabel": lbl = ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLinkButton": lbl = ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlHyperLink": lbl = ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                }
                            }
                            else
                            {
                                lbl = grdSource.HeaderRow.Cells[c].Text;
                            }
                        }
                        break;
                    default: lbl = grdSource.HeaderRow.Cells[c].Text;
                        break;
                }
                str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + lbl + "</th>");
            }
            str.Append("</tr>");
            for (int r = 0; r < grdSource.Rows.Count; r++)
            {
                if (!showHidden && grdSource.Rows[r].Visible == false)
                    continue;



                str.Append("<tr>");
                for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
                {
                    strValue = string.Empty;

                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        strCtlType = "" + (grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0]).GetType().ToString();
                        if (strCtlType == "System.Web.UI.LiteralControl")
                        {
                            strValue = "" + ((LiteralControl)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.Label")
                        {
                            if (c == 3)
                            {
                                var label = grdSource.Rows[r].FindControl("lblCustomerAddress") as Label;
                                if (label != null)
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text + "<br/> " + ((Label)grdSource.Rows[r].FindControl("lblCustomerAddress")).Text;
                                else
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                            else
                            {
                                strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                        }
                        if (strCtlType == "System.Web.UI.WebControls.LinkButton")
                        {
                            strValue = "" + ((LinkButton)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.HyperLink")
                        {
                            strValue = "" + ((HyperLink)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                    }
                    if (strValue == "")
                    {
                        strValue = "" + grdSource.Rows[r].Cells[c].Text;
                    }
                    str.Append("<td>" + strValue + "</td>");

                }
                str.Append("</tr>");
            }
            if (grdSource.FooterRow.Visible || showHidden)
            {

                for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
                {
                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    var ct2 = grdSource.FooterRow.Cells[c].Controls.Count;
                    if (ct2 == 0)
                    {
                        str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                    }
                    else
                    {
                        ct2 = ct2 == 1 ? 0 : 1;
                        switch (grdSource.FooterRow.Cells[c].Controls[ct2].GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.LiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.Label": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.LinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.HyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.DataControlFieldFooterCell":
                                {

                                    switch (grdSource.HeaderRow.Cells[c].Controls[ct2].GetType().ToString())
                                    {
                                        case "System.Web.UI.WebControls.DataControlLiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLabel": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlHyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                    }
                                }
                                break;
                            default: str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                                break;
                        }
                    }
                }
            }
            str.Append("</table>");
            str.Append("</body>");
        }
        return str;
    }


    public static StringBuilder GetExcelContentForGridHiddenColumn(GridView grdSource,string FromDate , string ToDate, string ReportName, bool showHidden = true, IEnumerable<int> columnsToHide = null)
    {
        StringBuilder str = new StringBuilder();
        string strValue = string.Empty, strCtlType = string.Empty;
        ArrayList date = new ArrayList();
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
        int hideColCount = 0;
        if (columnsToHide != null)
        {
            hideColCount = columnsToHide.Count();
        }
        int grdColCount = grdSource.HeaderRow.Cells.Count - hideColCount;
        if (grdSource.Rows.Count > 0)
        {
            str.Append("<html><head><meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
            str.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content='Microsoft Excel 12'>");
            str.Append("<body topmargin=0 leftmargin=0>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<table cellpadding='0' cellspacing='0'>");
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: medium;font-weight: bold'>" + ReportName + "</td></tr>");          
            str.Append("<tr><td colspan='" + grdColCount + "' style='text-align:center;font-size: font-size: 16px;'><span style='font-weight: bold;'> " + FromDate + "</span>&nbsp;&nbsp;To&nbsp;&nbsp;<span style='font-weight: bold;'>" + ToDate + "</span>&nbsp;&nbsp;&nbsp;&nbsp;Generated By&nbsp;<span style='font-weight: bold;'>" + Globals.UserName + "&nbsp;@&nbsp;" + date[0].ToString() + "&nbsp;" + date[1].ToString() + "</span> </td></tr>");
            str.Append("<tr>");
            for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
            {
                if (columnsToHide != null)
                {
                    if (columnsToHide.Contains(c))
                        continue;
                }

                var type = grdSource.HeaderRow.Cells[c].GetType().ToString();
                var lbl = string.Empty;
                switch (type)
                {
                    case "System.Web.UI.WebControls.LiteralControl": lbl = ((LiteralControl)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.Label": lbl = ((Label)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.LinkButton": lbl = ((LinkButton)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.HyperLink": lbl = ((HyperLink)(grdSource.HeaderRow.Cells[c].Controls.Count >= 1 ? grdSource.HeaderRow.Cells[c].Controls[0] : grdSource.HeaderRow.Cells[c])).Text;
                        break;
                    case "System.Web.UI.WebControls.DataControlFieldHeaderCell":
                        {
                            if (grdSource.HeaderRow.Cells[c].Controls.Count >= 1)
                            {
                                var ct = grdSource.HeaderRow.Cells[c].Controls.Count == 1 ? 0 : 1;
                                switch (grdSource.HeaderRow.Cells[c].Controls[ct].GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.DataControlLiteralControl": lbl = ((LiteralControl)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLabel": lbl = ((Label)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlLinkButton": lbl = ((LinkButton)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                    case "System.Web.UI.WebControls.DataControlHyperLink": lbl = ((HyperLink)grdSource.HeaderRow.Cells[c].Controls[ct]).Text;
                                        break;
                                }
                            }
                            else
                            {
                                lbl = grdSource.HeaderRow.Cells[c].Text;
                            }
                        }
                        break;
                    default: lbl = grdSource.HeaderRow.Cells[c].Text;
                        break;
                }
                str.Append("<th style='background-color:Silver; color=#000000;' nowrap>" + lbl + "</th>");
            }
            str.Append("</tr>");
            for (int r = 0; r < grdSource.Rows.Count; r++)
            {
                if (!showHidden && grdSource.Rows[r].Visible == false)
                    continue;



                str.Append("<tr>");
                for (int c = 0; c < grdSource.HeaderRow.Cells.Count; c++)
                {
                    strValue = string.Empty;

                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    if (grdSource.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        strCtlType = "" + (grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0]).GetType().ToString();
                        if (strCtlType == "System.Web.UI.LiteralControl")
                        {
                            strValue = "" + ((LiteralControl)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.Label")
                        {
                            if (c == 3)
                            {
                                var label = grdSource.Rows[r].FindControl("lblCustomerAddress") as Label;
                                if (label != null)
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text + "<br/> " + ((Label)grdSource.Rows[r].FindControl("lblCustomerAddress")).Text;
                                else
                                    strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                            else
                            {
                                strValue = "" + ((Label)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                            }
                        }
                        if (strCtlType == "System.Web.UI.WebControls.LinkButton")
                        {
                            strValue = "" + ((LinkButton)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                        if (strCtlType == "System.Web.UI.WebControls.HyperLink")
                        {
                            strValue = "" + ((HyperLink)(grdSource.Rows[r].Cells[c].Controls.Count > 1 ? grdSource.Rows[r].Cells[c].Controls[1] : grdSource.Rows[r].Cells[c].Controls[0])).Text;
                        }
                    }
                    if (strValue == "")
                    {
                        strValue = "" + grdSource.Rows[r].Cells[c].Text;
                    }
                    str.Append("<td>" + strValue + "</td>");

                }
                str.Append("</tr>");
            }
            if (grdSource.FooterRow.Visible || showHidden)
            {

                for (int c = 0; c < grdSource.FooterRow.Cells.Count; c++)
                {
                    if (columnsToHide != null)
                    {
                        if (columnsToHide.Contains(c))
                            continue;
                    }

                    var ct2 = grdSource.FooterRow.Cells[c].Controls.Count;
                    if (ct2 == 0)
                    {
                        str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                    }
                    else
                    {
                        ct2 = ct2 == 1 ? 0 : 1;
                        switch (grdSource.FooterRow.Cells[c].Controls[ct2].GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.LiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.Label": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.LinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.HyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                break;
                            case "System.Web.UI.WebControls.DataControlFieldFooterCell":
                                {

                                    switch (grdSource.HeaderRow.Cells[c].Controls[ct2].GetType().ToString())
                                    {
                                        case "System.Web.UI.WebControls.DataControlLiteralControl": str.Append("<th nowrap>" + ((LiteralControl)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLabel": str.Append("<th nowrap>" + ((Label)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlLinkButton": str.Append("<th nowrap>" + ((LinkButton)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                        case "System.Web.UI.WebControls.DataControlHyperLink": str.Append("<th nowrap>" + ((HyperLink)grdSource.FooterRow.Cells[c].Controls[ct2]).Text + "</th>");
                                            break;
                                    }
                                }
                                break;
                            default: str.Append("<th nowrap>" + grdSource.FooterRow.Cells[c].Text + "</th>");
                                break;
                        }
                    }
                }
            }
            str.Append("</table>");
            str.Append("</body>");
        }
        return str;
    }
}
