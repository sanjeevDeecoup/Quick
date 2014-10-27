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
    public partial class InvoiceStatementSlip : System.Web.UI.Page
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        public string strPreview = string.Empty;     
        string _pkgType = string.Empty;
        DataSet dschk = new DataSet();
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
                    GetBookingDetails(Request.QueryString["CustCode"].ToString(), Request.QueryString["fromDate"].ToString(), Request.QueryString["ToDate"].ToString());
                }
            }
        }

        public string GetBookingDetails(string CustCode, string fromDate, string Todate)
        {
            // Create variables ///
            string custPhone = string.Empty, displayPhone = string.Empty, leftMessage = string.Empty, rightMessage = string.Empty;
            bool logo = false, termAndCondition = false, St = false, Rounded = false, barcode = false, PrintDueDate = false, ShowHeaderSlogan = false, PrintTermConditionOnStoreCopy = false, PrintPhoneNo = false, PrintCustomerSignature = false, PrinttaxDetail = false;
            double wholeAmt = 0, totalAmt = 0;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            DataSet dsInvoiceStatement = new DataSet();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 2);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            ds = AppClass.GetData(cmd);
            dsInvoiceStatement = BAL.BALFactory.Instance.Bal_Report.GetInvoiceStatementForCustomer(Globals.BranchID, fromDate, Todate, CustCode);
            DataSet dsMain = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", dsInvoiceStatement.Tables[1].Rows[0]["BookingNumber"].ToString());
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            dsMain = AppClass.GetData(cmd);
            int totalItem = 0;
            double discount = 0, serviceTax = 0, SlipVat = 0, SlipCess = 0, SlipHCess = 0;
            totalItem = Convert.ToInt32(dsInvoiceStatement.Tables[0].Compute("Sum(Qty)", string.Empty));

            string strTotalInvoice = string.Empty;
            if (dsInvoiceStatement.Tables[2].Rows.Count > 0)
            {
                for (int j = 0; j < dsInvoiceStatement.Tables[2].Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        strTotalInvoice = dsInvoiceStatement.Tables[2].Rows[j]["BookingNumber"].ToString();
                    }
                    else
                    {
                        strTotalInvoice = strTotalInvoice + ", " + dsInvoiceStatement.Tables[2].Rows[j]["BookingNumber"].ToString();
                    }
                }
            }
            ///// Set Variables ////
            if (ds.Tables[0].Rows.Count > 0)
            {
                termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                logo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());              
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                PrintDueDate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
                ShowHeaderSlogan = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                PrintTermConditionOnStoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTermConditionOnStoreCopy"].ToString());
                PrintPhoneNo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintPhoneNo"].ToString());
                PrintCustomerSignature = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintCustomerSignature"].ToString());
                PrinttaxDetail = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTaxDetail"].ToString());
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

                strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center'><img alt='' src='../ReceiptLogo/DRY" + Globals.BranchID + ".jpg' width='40px' height='40px' /></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            }
            else
            {
               
                strPreview += "<tr><td style='font-size: 12px; font-family: Arial;' align='left' valign='middle'>" + leftMessage + "</td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' width='40Px'></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='font-size: 12px; font-family: Arial;' valign='middle' align='right'>" + rightMessage + "</td></tr>";
            }
            
            strPreview += "<tr><td align='center' colspan='5'><span style='font-family:" + ds.Tables[0].Rows[0]["HeaderFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["HeaderFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() + "; text-decoration:" + ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextFontItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["Headertext"].ToString() + "</span></td></tr>";
           
            strPreview += "<tr><td width='3in' style='font-size: 15px; font-family: Times New Roman; ' colspan='5' align='center'><span style='font-family:" + ds.Tables[0].Rows[0]["AddressFontName"].ToString() + ";font-size:" + ds.Tables[0].Rows[0]["AddressFontSize"].ToString() + "px; font-weight:" + ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() + ";text-decoration:" + ds.Tables[0].Rows[0]["TextAddressUL"].ToString() + ";font-style:" + ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() + "'>" + ds.Tables[0].Rows[0]["AddressText"].ToString() + "</span></td></tr>";

            if (ds.Tables[1].Rows[0]["MainTagline"].ToString() != "")
            {
                
                strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
                
                strPreview += "<tr><td colspan='5' style='font-family: Arial; font-size: 11Px;' align='center'>" + ds.Tables[1].Rows[0]["MainTagline"].ToString() + "</td></tr>";
               
                strPreview += "<tr><td width='3in' colspan='5'> <hr style='width: 100%; border-bottom: 2px #5081A1 solid;' /></td></tr>";
            }           
            strPreview += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";            
          
           
            strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";

            strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman; font-weight: Bold;' colspan='5' >" + Convert.ToDateTime(DateTime.Today.ToString("dd MMM yyyy")).DayOfWeek.ToString() + " / " + DateTime.Today.ToString("dd MMM yyyy") + "</td></tr>";

            strPreview += "<tr><td align='left' style='font-size: 15px; font-family: Times New Roman;' colspan='5'>From " + fromDate + "  to " + Todate + "</td></tr>";
            
            strPreview += "<tr><td width='3in' colspan='5' height='10Px'></td></tr>";
            
            strPreview += "</table>";
            // Item Section
          
            strPreview += "<table width='3in' style='font-size: 15px; font-family: Courier New, Courier, monospace;'>";
            strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' colspan='2'>Particular's&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='0.5in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Qty&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td width='0.5in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Rate</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>Amount</td></tr>";           
            strPreview += "<tr><td width='3in' colspan='5'>----------------------------</td></tr>";
            for (int i = 0; i < dsInvoiceStatement.Tables[0].Rows.Count; i++)
            {               
                //// Makeing Dynamic Item list               
                strPreview += "<tr><td width='2in' style='font-size: 12px; font-family: Times New Roman;' colspan='2'>" + dsInvoiceStatement.Tables[0].Rows[i]["Garment"].ToString() + "</td><td width='0.5in' style='font-size: 12px; font-family: Times New Roman; '>" + dsInvoiceStatement.Tables[0].Rows[i]["Qty"].ToString() + "</td><td width='0.5in' style='font-size: 12px; font-family: Times New Roman; '>" +  dsInvoiceStatement.Tables[0].Rows[i]["Rate"].ToString() + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + dsInvoiceStatement.Tables[0].Rows[i]["Amount"].ToString() + "</td></tr>";
                
                //wholeAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                //totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
            }
            double totalWholeAmt = 0;
            for (int i = 0; i < dsInvoiceStatement.Tables[0].Rows.Count; i++)
            {
                serviceTax += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["MainTax"].ToString());
                SlipVat += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["MainTax"].ToString());
                serviceTax += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Cess"].ToString());
                SlipCess += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Cess"].ToString());
                serviceTax += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["HCess"].ToString());
                SlipHCess += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["HCess"].ToString());
                wholeAmt += Convert.ToDouble(dsInvoiceStatement.Tables[0].Rows[i]["Amount"].ToString());
            }
            totalWholeAmt = Math.Round(Convert.ToDouble(wholeAmt) + Convert.ToDouble(serviceTax), 2);
            strPreview += "<tr><td width='3in' colspan='5'>----------------------------</td></tr>";
            strPreview += "<tr><td width='1in' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;'>T.Pcs&nbsp;&nbsp;" + totalItem + "</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '></td></td><td width='1in' colspan='2' style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='right'>G Amt.</td><td width='1in' style='font-size: 12px; font-family: Times New Roman; '>" + string.Format("{0:0.00}", wholeAmt) + "</td></tr>";            
            if (St)
            {
                if (serviceTax > 0)
                {
                    strPreview += "<tr><td colspan='3' rowspan='2' style='font-size: 12px; font-family: Times New Roman;font-weight: Bold '>" + strTotalInvoice + "</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>Tax</td><td style='font-size: 12px; font-family: Times New Roman; '>" + string.Format("{0:0.00}", Math.Round(serviceTax, 2).ToString()) + "</td></tr>";
                }
                else
                {
                    strPreview += "<tr><td colspan='3' rowspan='2' style='font-size: 12px; font-family: Times New Roman;font-weight: Bold '>" + strTotalInvoice + "</td><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>&nbsp;</td><td style='font-size: 12px; font-family: Times New Roman; '>&nbsp;</td></tr>";
                }
            }
            strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' align='center'>N.Amt</td><td style='font-size: 12px; font-family: Times New Roman; '>" + string.Format("{0:0.00}", totalWholeAmt) + "</td></tr>";
            
            strPreview += "<tr><td  colspan='5' >----------------------------</td></tr>";
            if (termAndCondition)
            {               
                strPreview += "<tr><td style='font-size: 12px; font-family: Times New Roman; font-weight: Bold;' colspan='5'>Terms and Conditions</td></tr>";               
                strPreview += "<tr><td  colspan='5' >---------------------------</td></tr>";
                if (dsMain.Tables[0].Rows[0]["TC1"].ToString() != "")
                {                   
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5' >" + "# " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";                    
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";
                }
                if (dsMain.Tables[0].Rows[0]["TC2"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5' >" + "# " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                   
                }
                if (dsMain.Tables[0].Rows[0]["TC3"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                   
                }
                if (ds.Tables[0].Rows[0]["Term4"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5' >" + "# " + ds.Tables[0].Rows[0]["Term4"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term5"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5' >" + "# " + ds.Tables[0].Rows[0]["Term5"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term6"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5' >" + "# " + ds.Tables[0].Rows[0]["Term6"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term7"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term7"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term8"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term8"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term9"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term9"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term10"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term10"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term11"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term11"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term12"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term12"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term13"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term13"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term14"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term14"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
                if (ds.Tables[0].Rows[0]["Term15"].ToString() != "")
                {
                    strPreview += "<tr><td style='font-size: 10px; font-family: Times New Roman; ' colspan='5'>" + "# " + ds.Tables[0].Rows[0]["Term15"].ToString() + "</td></tr>";
                    strPreview += "<tr><td width='3in' colspan='5' height='5Px'></td></tr>";                    
                }
            }
            if (PrintCustomerSignature)
            {               
                strPreview += "<tr><td  colspan='5' >---------------------------</td></tr>";               
                strPreview += "<tr><td  colspan='5' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='center'>Signature</td></tr>";                
                strPreview += "<tr><td width='3in' colspan='5' height='25Px'></td></tr>";                
                strPreview += "<tr><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='left'>Customer</td><td  colspan='2' style='font-family: Arial; font-size: 12Px; font-weight: Bold;' align='right'>Salesman</td></tr>";
            }          
            strPreview += "</table>";
            strPreview += "</td></tr>";
            strPreview += "</table>";          
            ViewState["Msg"] = strPreview;
            return strPreview;
        }
    }
}