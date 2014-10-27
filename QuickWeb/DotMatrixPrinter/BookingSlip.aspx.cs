using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

public partial class Dot_BookingSlip : System.Web.UI.Page
{
    protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
    protected string StoreName = "";
    protected string BookingNo = "";
    protected string ImagePath = "";
    protected string TableWeight = "bold";
    protected bool logo = false;
    protected bool StoreCopy = false;
    protected string strPageName = "~/Bookings/BookingSlip.aspx", BranchDetails = "", BookingNumber = "", CustomerName = "", CustomerAddress = "", ReceiptDate = "", DueDate = "", DueTime = "", UrgentDelivery = "", TotalAmount = "0", DiscountRate = "0", RebateAmount = "0", NetAmount = "0", AdvPayment = "0", PreviousDue = "0", DuePayment = "0", DiscountOnPayment = "0", Remarks = "", strProcess = "";
    protected bool chkBookingNumber = false;
    public string strPreview = "";
    public string strPreview1 = "";
    public string strPreview2 = string.Empty;
    public string strPreview3 = string.Empty;
    protected string Inch = "";

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
                GetBookingDetailsForBookingNumberStoreCopy(no[0]);
                GetBookingDetailsForBookingNumberPrint(no[0]);
                GetBookingDetailsForBookingNumberStoreCopyPrint(no[0]);
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
        string custPhone = "", displayPhone = "", homeDelivery = "", Urgent = "";
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
            hdnEmailId.Value = BookNumber;
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
                    GetTempSave(BookNumber);
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
                    homeDelivery = dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString();
                    Urgent = dsMain.Tables[0].Rows[0]["Urgent"].ToString();
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
                    if (Urgent != "" && homeDelivery != "")
                    {
                        strPreview += "<tr><td nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "," + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
                    }
                    else if (homeDelivery != "")
                    {
                        strPreview += "<tr><td nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "</td></tr>";
                    }
                    else if (Urgent != "")
                    {
                        strPreview += "<tr><td nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
                    }
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
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                            strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                            {
                                strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                            }
                            else
                            {
                                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                                {
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                }
                                else
                                {
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    strPreview += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                }
                            }
                        }
                        totalQty += Convert.ToInt32(dsMain.Tables[1].Rows[i]["SubPieces"].ToString());
                        totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    }
                    double serviceTax = 0, ServiceRate = 0, amt1 = 0;
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                    }
                    strPreview += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview += "<tr><td nowrap='nowrap'>" + totalQty + "&nbsp;Pcs</td><td style='color: #FF0000;' nowrap='nowrap'>G Amt.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + totalAmt + "</td></tr> ";
                    strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Adv.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + dsMain.Tables[0].Rows[0]["PaymentMade"].ToString() + "</td></tr> ";
                    if (St)
                    {
                        strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Tax.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + Math.Round(serviceTax, 2).ToString() + "</td></tr> ";
                    }
                    if (dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() != "0")
                    {
                        strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Dis.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() + "</td></tr> ";
                    }
                    Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
                    if (Rounded)
                    {
                        ServiceRate = Math.Round(serviceTax, 2);
                        amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
                    }
                    else
                    {
                        ServiceRate = Math.Round(serviceTax, 0);
                        amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 0);
                    }
                    strPreview += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>N.Amt.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + amt1.ToString() + "</td></tr> ";
                    strPreview += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                    if (HeaderSlogan)
                    {
                        //strPreview += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                        strPreview += "<tr><td colspan='4' align='left' >" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr>";
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
        ViewState["Msg"] = strPreview;
        return ReturnVal;
    }

    protected bool GetBookingDetailsForBookingNumberStoreCopy(string BookNumber)
    {
        bool ReturnVal = false;
        if (BookNumber == "")
        {
            return ReturnVal;
        }
        RebateAmount = "0";
        int totalQty = 0;
        bool barcode = false;
        double totalAmt = 0;
        string custPhone = "", displayPhone = "";
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet dsMain = new DataSet();
            cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
            cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookNumber));
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
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    GetTempSave(BookNumber);
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
                    strPreview1 += "<table width='3in' style='font-size: 12px; font-family: Courier New, Courier, monospace;'>";
                    strPreview1 += "<tr><td style='height: 50Px' colspan='2'></td></tr>";
                    strPreview1 += "<tr><td style='font-family: Arial; color: #000000; font-size: 25Px;'  align='center' valign='middle' >" + BookingNumber + "</td><td style='font-family: Arial; color: #000000; font-size: 22Px;' align='left'>Store Copy</td></tr>";
                    strPreview1 += "<tr><td align='center' nowrap='nowrap' style='line-height:16Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview1 += "<tr><td align='center' nowrap='nowrap' style='line-height:5Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview1 += "<tr><td style='color: #FF0000; font-family: Arial Black;' nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                    strPreview1 += "<tr><td  colspan='2' >###############################</td></tr>";
                    strPreview1 += "<tr ><td align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
                    strPreview1 += "<tr><td nowrap='nowrap' align='left'>" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td><td align='left'>" + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                    //strPreview1 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview1 += "<tr><td colspan='2'><table width='" + "3in" + "' style='font-size:12px;'><tr><td colspan='4'></tr>";
                    strPreview1 += "<tr><td  colspan='4' >------------------------------</td></tr>";
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
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                            strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                            {
                                strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + "- " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                            }
                            else
                            {
                                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                                {
                                    strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + '-' + " " + dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() + '@' + dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                }
                                else
                                {
                                    strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    strPreview1 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td colspan='2'>" + "[" + dsMain.Tables[1].Rows[i]["ItemProcessType"].ToString() + "@" + rate2[1].ToString() + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                }
                            }
                        }
                        totalQty += Convert.ToInt32(dsMain.Tables[1].Rows[i]["SubPieces"].ToString());
                        totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    }
                    double serviceTax = 0, TaxRate = 0;
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                    }
                    TaxRate = Math.Round(serviceTax, 0);
                    strPreview1 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview1 += "<tr><td nowrap='nowrap'>" + totalQty + "&nbsp;Pcs</td><td></td><td style='color: #FF0000;' nowrap='nowrap'></td><td nowrap='nowrap'></td></tr> ";
                    double amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(TaxRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);

                    //if (barcode)
                    //{
                    //    strPreview1 += "<tr><td colspan='4' >&nbsp;</td></tr>";
                    //    strPreview1 += "<tr><td style='font-family: c39hrp24dhtt; font-size:32Px;' colspan='4' align='center' >" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td></tr>";
                    //    strPreview1 += "<tr><td colspan='4' >&nbsp;</td></tr>";
                    //}
                    strPreview1 += "<tr><td colspan='4' >------------------------------</td></tr>";
                    strPreview1 += "</table></td></tr></table>";
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

    protected bool GetBookingDetailsForBookingNumberPrint(string BookNumber)
    {
        bool ReturnVal = false, PrintRate = false, PrintProcess = false, PrintLine = true;
        if (BookNumber == "")
        {
            return ReturnVal;
        }
        RebateAmount = "0";
        int totalQty = 0;
        bool HeaderSlogan = false, logoOnReceipt = false, barcode = false, previousDue = false, termAndCondition = false, St = false, Rounded = false;
        double totalAmt = 0;
        string custPhone = "", displayPhone = "", homeDelivery = "", Urgent = "";
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
            hdnEmailId.Value = BookNumber;
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
                PrintRate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintRate"].ToString());
                PrintProcess = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintProcess"].ToString());
            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    GetTempSave(BookNumber);
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
                    homeDelivery = dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString();
                    Urgent = dsMain.Tables[0].Rows[0]["Urgent"].ToString();
                    BookingNumber = dsMain.Tables[0].Rows[0]["BookingNumber"].ToString();
                    strPreview2 += "<table width='3in' style='font-size: 12px; font-family: Courier New, Courier, monospace;'>";
                    strPreview2 += "<tr><td style='font-family: Arial; color: #000000; font-size: 25Px;'  align='center' valign='middle' >" + BookingNumber + "</td><td align='right'></td></tr>";
                    strPreview2 += "<tr><td align='center' nowrap='nowrap' style='line-height:2Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview2 += "<tr><td colspan='2' style='font-family: Arial; color: #FF0000; font-size: 16Px;' align='center'>" + dsMain.Tables[0].Rows[0]["StoreName"].ToString() + "</td></tr>";
                    strPreview2 += "<tr><td colspan='2' style=' font-size:15Px;' align='center'>" + dsMain.Tables[0].Rows[0]["StoreAddress"].ToString() + "</td></tr>";
                    strPreview2 += "<tr><td align='center' nowrap='nowrap' style='line-height:5Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview2 += "<tr><td style='color: #FF0000; font-family: Arial Black;' nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                    strPreview2 += "<tr><td  colspan='2' >###############################</td></tr>";
                    strPreview2 += "<tr ><td align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
                    strPreview2 += "<tr><td nowrap='nowrap' align='left'>" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td><td align='left'>" + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                    if (Urgent != "" && homeDelivery != "")
                    {
                        strPreview2 += "<tr><td nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "," + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
                    }
                    else if (homeDelivery != "")
                    {
                        strPreview2 += "<tr><td nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["HomeDelivery"].ToString() + "</td></tr>";
                    }
                    else if (Urgent != "")
                    {
                        strPreview2 += "<tr><td nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["Urgent"].ToString() + "</td></tr>";
                    }
                    //strPreview1 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview2 += "<tr><td colspan='2'><table width='" + "3in" + "' style='font-size:12px;'><tr><td colspan='4'></tr>";
                    strPreview2 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        string ItemName = "", Color = "", Remarks = "", Process = "", ExProcess1 = "", ExProcess2 = "", Rate = "", Rate1 = "", Rate2 = "";
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
                            Rate = rate2[1].ToString();
                            Rate1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString();
                            Rate2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString();
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
                        if (PrintProcess == false && PrintRate == false)
                        {
                            PrintLine = false;
                        }
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                            if (PrintLine)
                            {
                                strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                            }
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                            {
                                strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                if (PrintLine)
                                {
                                    strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                }
                            }
                            else
                            {
                                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                                {
                                    strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    if (PrintLine)
                                    {
                                        strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                    }
                                }
                                else
                                {
                                    strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    if (PrintLine)
                                    {
                                        strPreview2 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td colspan='2'>" + "[" + Process + Rate + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                    }
                                }
                            }
                        }
                        totalQty += Convert.ToInt32(dsMain.Tables[1].Rows[i]["SubPieces"].ToString());
                        totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    }
                    double serviceTax = 0, ServiceRate = 0, amt1 = 0;
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                    }
                    strPreview2 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview2 += "<tr><td nowrap='nowrap'>" + totalQty + "&nbsp;Pcs</td><td style='color: #FF0000;' nowrap='nowrap'>G Amt.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + totalAmt + "</td></tr> ";
                    strPreview2 += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Adv.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + dsMain.Tables[0].Rows[0]["PaymentMade"].ToString() + "</td></tr> ";
                    if (St)
                    {
                        strPreview2 += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Tax.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + Math.Round(serviceTax, 2).ToString() + "</td></tr> ";
                    }
                    if (dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() != "0")
                    {
                        strPreview2 += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>Dis.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString() + "</td></tr> ";
                    }
                    Rounded = Convert.ToBoolean(dsMain.Tables[0].Rows[0]["Rounded"].ToString());
                    if (Rounded)
                    {
                        ServiceRate = Math.Round(serviceTax, 2);
                        amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);
                    }
                    else
                    {
                        ServiceRate = Math.Round(serviceTax, 0);
                        amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(ServiceRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 0);
                    }
                    strPreview2 += "<tr><td nowrap='nowrap'></td><td style='color: #FF0000;' nowrap='nowrap'>N.Amt.</td><td nowrap='nowrap'>" + ds.Tables[0].Rows[0]["CurrencyType"].ToString() + "</td><td nowrap='nowrap'>" + amt1.ToString() + "</td></tr> ";
                    strPreview2 += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                    if (HeaderSlogan)
                    {
                        //strPreview += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                        strPreview2 += "<tr><td colspan='4' align='left' >" + ds.Tables[0].Rows[0]["SloganText"].ToString() + "</td></tr>";
                        strPreview2 += "<tr><td style=' line-height:10Px;' colspan='4' >------------------------------</td></tr>";
                    }
                    if (termAndCondition)
                    {
                        strPreview2 += "<tr><td  >T&C </td><td colspan='3'>" + "" + "</td></tr>";
                        strPreview2 += "<tr><td align='left' colspan='4' style='font-size:11Px;'>" + "* " + dsMain.Tables[0].Rows[0]["TC1"].ToString() + "</td></tr>";
                        strPreview2 += "<tr><td align='left' colspan='4' style='font-size:11Px;'>" + "* " + dsMain.Tables[0].Rows[0]["TC2"].ToString() + "</td></tr>";
                        strPreview2 += "<tr><td align='left' colspan='4' style='font-size:11Px;'>" + "* " + dsMain.Tables[0].Rows[0]["TC3"].ToString() + "</td></tr>";
                    }
                    //strPreview += "<tr><td colspan='4' >------------------------------</td></tr>";
                    strPreview2 += "</table></td></tr></table>";
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
        ViewState["Msg"] = strPreview2;
        return ReturnVal;
    }

    protected bool GetBookingDetailsForBookingNumberStoreCopyPrint(string BookNumber)
    {
        bool ReturnVal = false, PrintRate = false, PrintProcess = false, PrintLine = true;
        if (BookNumber == "")
        {
            return ReturnVal;
        }
        RebateAmount = "0";
        int totalQty = 0;
        bool barcode = false;
        double totalAmt = 0;
        string custPhone = "", displayPhone = "";
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet dsMain = new DataSet();
            cmd.CommandText = "Sp_Sel_BookingDetailsForReceipt";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
            cmd.Parameters.Add(new SqlParameter("@BookingNumber", BookNumber));
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
                barcode = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                StoreCopy = Convert.ToBoolean(ds.Tables[0].Rows[0]["StoreCopy"].ToString());
                PrintRate = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintRate"].ToString());
                PrintProcess = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintProcess"].ToString());
            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    GetTempSave(BookNumber);
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
                    strPreview3 += "<table width='3in' style='font-size: 12px; font-family: Courier New, Courier, monospace;'>";
                    strPreview3 += "<tr><td style='height: 50Px' colspan='2'></td></tr>";
                    strPreview3 += "<tr><td style='font-family: Arial; color: #000000; font-size: 25Px;'  align='center' valign='middle' >" + BookingNumber + "</td><td style='font-family: Arial; color: #000000; font-size: 22Px;' align='left'>Store Copy</td></tr>";
                    strPreview3 += "<tr><td align='center' nowrap='nowrap' style='line-height:16Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview3 += "<tr><td align='center' nowrap='nowrap' style='line-height:5Px;'>" + "&nbsp;" + "</td></tr>";
                    strPreview3 += "<tr><td style='color: #FF0000; font-family: Arial Black;' nowrap='nowrap' align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                    strPreview3 += "<tr><td  colspan='2' >###############################</td></tr>";
                    strPreview3 += "<tr ><td align='left' colspan='2'>" + dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString() + displayPhone + "</td></tr>";
                    strPreview3 += "<tr><td nowrap='nowrap' align='left'>" + dsMain.Tables[0].Rows[0]["BookingDate"].ToString() + "</td><td align='left'>" + dsMain.Tables[0].Rows[0]["DeliveryDate"].ToString() + "</td></tr>";
                    //strPreview1 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview3 += "<tr><td colspan='2'><table width='" + "3in" + "' style='font-size:12px;'><tr><td colspan='4'></tr>";
                    strPreview3 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        string ItemName = "", Color = "", Remarks = "", Process = "", ExProcess1 = "", ExProcess2 = "", Rate = "", Rate1 = "", Rate2 = "";
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
                            Rate = rate2[1].ToString();
                            Rate1 = dsMain.Tables[1].Rows[i]["ItemExtraProcessRate1"].ToString();
                            Rate2 = dsMain.Tables[1].Rows[i]["ItemExtraProcessRate2"].ToString();
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
                        if (PrintProcess == false && PrintRate == false)
                        {
                            PrintLine = false;
                        }
                        if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None" && dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                        {
                            strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                            if (PrintLine)
                            {
                                strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                            }
                        }
                        else
                        {
                            if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType1"].ToString() != "None")
                            {
                                strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                if (PrintLine)
                                {
                                    strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + Process + Rate + " " + "- " + ExProcess1 + Rate1 + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                }
                            }
                            else
                            {
                                if (dsMain.Tables[1].Rows[i]["ItemExtraProcessType2"].ToString() != "None")
                                {
                                    strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    if (PrintLine)
                                    {
                                        strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td  colspan='2'>" + "[" + Process + Rate + " " + '-' + " " + ExProcess2 + Rate2 + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                    }
                                }
                                else
                                {
                                    strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td style='font-size: 13Px; font-weight: bolder;Width:10px'>" + dsMain.Tables[1].Rows[i]["ItemTotalQuantity"].ToString() + "</td><td>" + "<bolder>" + dsMain.Tables[1].Rows[i]["ItemName"].ToString() + "</td></tr></table></td><td align='right'>" + (Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString())) + "</td></tr>";
                                    if (PrintLine)
                                    {
                                        strPreview3 += "<tr><td colspan='3'><table cellspacing='0' cellpadding='0'><tr><td></td><td colspan='2'>" + "[" + Process + Rate + " " + Color + " " + Remarks + "]" + "</td></tr></table></td></tr>";
                                    }
                                }
                            }
                        }
                        totalQty += Convert.ToInt32(dsMain.Tables[1].Rows[i]["SubPieces"].ToString());
                        totalAmt += Convert.ToDouble(dsMain.Tables[1].Rows[i]["ItemSubTotal"].ToString());
                    }
                    double serviceTax = 0, TaxRate = 0;
                    for (int i = 0; i < dsMain.Tables[1].Rows.Count; i++)
                    {
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STPAmt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP1Amt"].ToString());
                        serviceTax += Convert.ToDouble(dsMain.Tables[1].Rows[i]["STEP2Amt"].ToString());
                    }
                    TaxRate = Math.Round(serviceTax, 0);
                    strPreview3 += "<tr><td  colspan='4' >------------------------------</td></tr>";
                    strPreview3 += "<tr><td nowrap='nowrap'>" + totalQty + "&nbsp;Pcs</td><td></td><td style='color: #FF0000;' nowrap='nowrap'></td><td nowrap='nowrap'></td></tr> ";
                    double amt1 = Math.Round(Convert.ToDouble(totalAmt) + Convert.ToDouble(TaxRate) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["PaymentMade"].ToString()) - Convert.ToDouble(dsMain.Tables[0].Rows[0]["DiscountAmt"].ToString()), 2);

                    //if (barcode)
                    //{
                    //    strPreview3 += "<tr><td colspan='4' >&nbsp;</td></tr>";
                    //    strPreview3 += "<tr><td style='font-family: c39hrp24dhtt; font-size:32Px;' colspan='4' align='center' >" + dsMain.Tables[0].Rows[0]["Barcode"].ToString() + "</td></tr>";
                    //    strPreview3 += "<tr><td colspan='4' >&nbsp;</td></tr>";
                    //}
                    strPreview3 += "<tr><td colspan='4' >------------------------------</td></tr>";
                    strPreview3 += "</table></td></tr></table>";
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

    private string sqlMain = "";

    protected void GetTempSave(string BookingNumber)
    {
        int totalqty = 0;
        totalqty = ReturnQty(BookingNumber);
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
            sqlcon.Close();
            sqlcon.Dispose();
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
        string eMail = "";
        DataSet ds = new DataSet(); SqlDataReader sdr = null;
        try
        {

            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNo", hdnEmailId.Value);
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
                cmd1.Parameters.AddWithValue("@Flag", 2);
                ds1 = AppClass.GetData(cmd1);

                string FEmail = eMail;
                SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                string mailBody = ViewState["Msg"].ToString();
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
        catch (Exception)
        {
        }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        GetBookingDetailsForBookingNumber(hdnEmailId.Value);
    }
}