using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

public partial class Dot_DeliverySlip : System.Web.UI.Page
{
    protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
    protected string StoreName = string.Empty;
    protected string BookingNo = string.Empty;
    protected string ImagePath = string.Empty;
    protected bool logo = false;
    protected string strPageName = "~/Bookings/BookingSlip.aspx", BranchDetails = string.Empty, BookingNumber = string.Empty, CustomerName = string.Empty, CustomerAddress = string.Empty, ReceiptDate = string.Empty, DueDate = string.Empty, DueTime = string.Empty, UrgentDelivery = string.Empty, TotalAmount = "0", DiscountRate = "0", RebateAmount = "0", NetAmount = "0", AdvPayment = "0", PreviousDue = "0", DuePayment = "0", DiscountOnPayment = "0", Remarks = string.Empty, strProcess = string.Empty;
    protected bool chkBookingNumber = false;
    public string strPreview = string.Empty;
    protected string Inch = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["BN"] == null || Request.QueryString["BN"] == "")
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
            else
            {
                string BookNum = Request.QueryString["BN"].ToString();
                string[] no = BookNum.Split('-');
                GetBookingDetailsForBookingNumber(no[0]);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Print", "javascript:window.print();javascript:window.close();", true);
            }
        }
    }

    protected bool GetBookingDetailsForBookingNumber(string BookNumber)
    {
        bool ReturnVal = false;
        if (BookNumber == "")
        {
            return ReturnVal;
        }
        RebateAmount = "0";
        int totalQty = 0;
        bool HeaderSlogan = false, logoOnReceipt = false, barcode = false, previousDue = false, termAndCondition = false, St = false, Rounded = false;
        double totalAmt = 0;
        string custPhone = "", displayPhone = "";
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet dsMain = new DataSet();
            cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookNumber));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
            dsMain = AppClass.GetData(cmd);
            SqlCommand cmd1 = new SqlCommand();
            DataSet ds = new DataSet();
            cmd1.CommandText = "sp_ReceiptConfigSetting";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd1.Parameters.AddWithValue("@Flag", 2);
            ds = AppClass.GetData(cmd1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                termAndCondition = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                St = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                logoOnReceipt = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
                logo = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintlogoonReceipt"].ToString());
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                previousDue = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
                HeaderSlogan = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                ImagePath = ds.Tables[0].Rows[0]["ImagePath"].ToString();
            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    custPhone = dsMain.Tables[0].Rows[0]["CustomerPhone"].ToString();
                    if (custPhone != "")
                    {
                        displayPhone = "(" + custPhone + ")";
                    }
                    else
                    {
                        displayPhone = "";
                    }
                    StoreName = dsMain.Tables[0].Rows[0]["StoreName"].ToString();
                    BookingNumber = dsMain.Tables[0].Rows[0]["BookingNumber"].ToString();
                    strPreview += "<table width='3in' style='font-size: 12px; font-family: Courier New, Courier, monospace;'>";
                    strPreview += "<tr><td style='font-family: Arial; color: #000000; font-size: 25Px;'  align='center' valign='middle' >" + BookingNumber + "</td><td align='right'></td></tr>";
                    strPreview += "<tr><td align='center' nowrap='nowrap' style='line-height:2Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview += "<tr><td colspan='2' style='font-family: Arial; color: #FF0000; font-size: 16Px;' align='center'>" + dsMain.Tables[0].Rows[0]["StoreName"].ToString() + "</td></tr>";
                    strPreview += "<tr><td colspan='2' style=' font-size:15Px;' align='center'>" + dsMain.Tables[0].Rows[0]["StoreAddress"].ToString() + "</td></tr>";
                    strPreview += "<tr><td align='center' nowrap='nowrap' style='line-height:5Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview += "<tr><td style='color: #FF0000; font-family: Arial Black;' nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                    strPreview += "<tr><td  colspan='2' >###############################</td></tr>";
                    strPreview += "<tr ><td align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
                    strPreview += "<tr><td nowrap='nowrap' align='left'>" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td><td align='left'>" + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                    //strPreview1 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview += "<tr><td colspan='2'><table width='" + "3in" + "' style='font-size:12px;'><tr><td colspan='4'></tr>";
                    strPreview += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        string ItemName = "", Color = "", Remarks = "";
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
                        string ClothesStatus = "";
                        if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) == Convert.ToInt32(DeliverOrUnDeliver[1].ToString()))
                            ClothesStatus = "{Del}";
                        else
                        {
                            if (Convert.ToInt32(DeliverOrUnDeliver[0].ToString()) > 0)
                                ClothesStatus = "{Del" + "-" + ItemStatus + "}";
                        }
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                            strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + " " + ClothesStatus + "]" + "</td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                            {
                                strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + Color + " " + Remarks + " " + ClothesStatus + "]" + "</td></tr></table></td></tr>";
                            }
                            else
                            {
                                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                                {
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + " " + ClothesStatus + "]" + "</td></tr></table></td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'></td><td colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + " " + ClothesStatus + "]" + "</td></tr></table></td></tr>";
                                }
                            }
                        }
                        totalQty += Convert.ToInt32(dsMain.Tables[1].Rows[i]["SubPieces"].ToString());
                        totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    }
                    double serviceTax = 0, ServiceRate = 0, Bal = 0, amt1 = 0;
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                    }
                    Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
                    strPreview += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview += "<tr><td nowrap='nowrap'>" + totalQty + "&nbsp;Pcs</td><td style='color: #FF0000;' nowrap='nowrap'>G Amt.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + totalAmt + "</td></tr> ";
                    if (St)
                    {
                        strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Tax.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + serviceTax.ToString() + "</td></tr> ";
                    }
                    strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Paid.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + dsMain.Tables[0].Rows[0]["PaymentMade"].ToString() + "</td></tr> ";
                    if (Rounded)
                    {
                        ServiceRate = Math.Round(serviceTax, 2);
                        amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
                        Bal = Math.Round(amt1 - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()), 2);
                    }
                    else
                    {
                        ServiceRate = Math.Round(serviceTax, 0);
                        amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 0);
                        Bal = Math.Round(amt1 - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()), 0);
                    }
                    Bal = Math.Round(amt1 - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()), 0);
                    strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Bal.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + Bal.ToString() + "</td></tr> ";
                    strPreview += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                    if (HeaderSlogan)
                    {
                        //strPreview += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                        strPreview += "<tr><td colspan='4' align='left' >" + ds.Tables[0].Rows[0]["FooterSloganText"].ToString() + "</td></tr>";
                        strPreview += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                    }
                    if (termAndCondition)
                    {
                        strPreview += "<tr><td  >T&C </td><td colspan='3'>" + "" + "</td></tr>";
                        strPreview += "<tr><td align='left' colspan='4' style='font-size:11Px;'>" + "* " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";
                        strPreview += "<tr><td align='left' colspan='4' style='font-size:11Px;'>" + "* " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                        strPreview += "<tr><td align='left' colspan='4' style='font-size:11Px;'>" + "* " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                    }
                    //strPreview += "<tr><td colspan='4' >------------------------------</td></tr>";
                    strPreview += "</table></td></tr></table>";
                }
            }
            else
            {
                lblMsg.Text = "Invalid Booking Number selection.";
                return false;
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = excp.Message;
        }
        finally
        {
        }
        return ReturnVal;
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
}