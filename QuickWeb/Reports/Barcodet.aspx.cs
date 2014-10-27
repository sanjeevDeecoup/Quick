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
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System.Text; 

public partial class Reports_Barcodet : System.Web.UI.Page
{
    public string strPreview = string.Empty;
    public string OldBarCodeWidth = string.Empty;
    public string OldBarCodeHeight = string.Empty;

    DataSet ds = new DataSet();
    DTO.BarCodeSetting Ob = new DTO.BarCodeSetting();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["bookingNo"] == null)
            Response.Redirect("~/Login.aspx", false);

        FetchBarCodeSetting();
        if (Request.QueryString["Index"] != null && Request.QueryString["Index"].ToString() != "")
        {
            hdnIndexValue.Value = Request.QueryString["Index"].ToString();
        }

        // this checks if the user is directly printing the page from frm_New_Booking
        // if yes it will set the hidden field indicating the same
        // which will be checked in doucment ready and further processing will be performed
        if (Request.QueryString["PrintBarCode"] != null && Request.QueryString["PrintBarCode"].ToString() == "true")
        {
            hdnComingFrom.Value = "PrintBarCode";
    
        }

        //if printbarcode is true, print the barcode too
        if (Request.QueryString["PrintBookingSlip"] != null && Request.QueryString["PrintBookingSlip"].ToString() == "true")
        {
            hdnPrintBookingSlip.Value = "true";
            hdnBookingNumber.Value = Request.QueryString["bookingNo"].ToString();
            hdnBookingDate.Value = Request.QueryString["bookingDate"].ToString();
            // BasePage.OpenWindow(this, "../Bookings/BookingSlip.aspx?BN=" + hdnBookingNumber.Value + "-1" + hdnBookingDate.Value + "&DirectPrint=true");
           
        }

        if (Request.QueryString["RedirectBack"] != null && Request.QueryString["RedirectBack"].ToString() == "true")
        {
            hdnRedirectBack.Value = "true";
        }
        if (Request.QueryString["CloseWindow"] != null && Request.QueryString["CloseWindow"].ToString() == "true")
        {
            hdnCloseWindow.Value = "true";
        }
        if (Request.QueryString["bookingNo"] != null && Request.QueryString["bookingNo"].ToString() !="")
        {
            hdnTmpBookingNo.Value = Request.QueryString["bookingNo"].ToString().Trim();
        }
        if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
        {
            hdnTmpItemRowIndex.Value = Request.QueryString["id"].ToString().Trim();
        }
        
    }



    public void FetchBarCodeSetting()
    {
        Ob.BranchId = Globals.BranchID;

        ds = BAL.BALFactory.Instance.BAL_Barcodesetting.fetchbarcodeconfig1(Ob);
        string ImageUrl = Server.MapPath("~/ProcessWiseLogo/");
        Ob = BAL.BALFactory.Instance.BAL_Barcodesetting.BarCodeData(ds, Request.QueryString["Id"].ToString(), Request.QueryString["bookingNo"].ToString(), Ob.BranchId, ImageUrl);

        OldBarCodeWidth = Ob.OldBarcodeWidth;
        OldBarCodeHeight = Ob.OldBarcodeHeight;

        hdnValue.Value = Ob.PrinterName;
        strPreview = Ob.StrPreviewBarCode;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        
   }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=\"Javascript\">window.open = self;window.close();</script>");
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        if (hdnPrintBookingSlip.Value == "true")
        {
            // PrjClass.GetAndGo("", "../Bookings/BookingSlip.aspx?BN=" + hdnBookingNumber.Value + "-1" + hdnBookingDate.Value + "&DirectPrint=true");
            // BasePage.OpenWindow(this, "../Bookings/BookingSlip.aspx?BN=" + hdnBookingNumber.Value + "-1" + hdnBookingDate.Value + "&DirectPrint=true");
            
        }
    }


}

