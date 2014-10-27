using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;

namespace QuickWeb.Reports
{
    public partial class CompareBooking : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        ArrayList date = new ArrayList();
        public static StringWriter sw;
        public static string strAllContents = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["BC"])) return;

            BookingSlip bs = new BookingSlip();
            Ob.StrArray = Request.QueryString["BC"].ToString().Split(',');
            DTO.Report.BFlag = true;
            //hdnDTOReportsBFlag.Value = "ture";
            
            if (!HttpContext.Current.Items.Contains("IsPrintingForMany"))
                HttpContext.Current.Items.Add("IsPrintingForMany", "true");
            else
                HttpContext.Current.Items["IsPrintingForMany"] = "true";
            

            for (int i = 0; i < Ob.StrArray.Count(); i++)
            {
                this.Form.Target = "_blank";
            }
            sw = new StringWriter();
            strAllContents = "";
            string temp = string.Empty, test = string.Empty;
            var isLaser = BAL.BALFactory.Instance.Bal_Report.IsPrinterLaser(Globals.BranchID);
            for (int i = 0; i < Ob.StrArray.Count(); i++)
            {
                sw.Flush();                
                this.Form.Target = "_blank";
                BookingSlip bsp = new BookingSlip();
                Thermal_BookingSlip tbs = new Thermal_BookingSlip();
                if (!isLaser)
                {

                    if (Ob.StrArray[i].ToString() == "-1")
                    {
                        temp += tbs.GetBookingDetails(Request.QueryString["BN"].ToString()).Item1;
                    }
                    else
                    {
                        temp += tbs.GetBookingDetails(Ob.StrArray[i].ToString(), true).Item1;
                    }

                }
                else
                {
                    if (Ob.StrArray[i].ToString() == "-1")
                    {
                        temp += bsp.GetBookingDetailsForBookingNumber(Request.QueryString["BN"].ToString(), null);                       
                    }
                    else
                    {
                        temp += bsp.GetBookingDetailsForBookingNumber(Ob.StrArray[i].ToString(), null, true);
                        temp += "</table></td></tr></table></td></tr></table>";
                    }
                }

                if (HttpContext.Current.Items.Contains("CheckStoreCopy") && HttpContext.Current.Items["CheckStoreCopy"].ToString() == "true")
                {
                    string Preview = "";
                    Preview = "<table style='width:7.6in;height:5.12in'><tr><td></td><tr></table>";
                    Preview += bsp.strPreview1;
                    Response.Write(Preview);
                    Preview += " </td>";
                    Preview += "</tr>";
                    Preview += "</table>";

                    temp += Preview;
                }
                else
                {

                }               
            }
            strAllContents = temp;

            //BasePage.OpenWindow(this.Page, "../Reports/ListBooking.aspx");
            //btnShowReport_Click(null, null);
            //DTO.Report.BFlag = false;
            //hdnDTOReportsBFlag.Value = "false";
            //for (int i = 0; i < Ob.StrArray.Count(); i++)
            //{
            //    this.Form.Target = "_blank";
            //}

            Response.Write(strAllContents);
            hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "helpprint();", true);
        }
    }
}