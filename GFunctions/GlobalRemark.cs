public enum GStatus
{
    Booking = 1,
    In_Processing = 2,
    Cloth_Ready = 3,
    Delivered = 4,
    Cancel_Booking = 5,
    Factory_In = 20,
    Factory_Out = 30,
    SendTo_SteamPress = 50
}

public struct ReportCaptions
{
    public static string MainCaption = "DrySoft";
    public static string ItemWise = "Item Wise Report Summary";
    public static string CustomerWise = "Accounts Receivable";
    public static string Duration = "Business Summary";
    public static string YearWise = "Year Wise Business Volume";
    public static string MonthWise = "Month Wise Business Volume";
    public static string DayWise = "Day Wise Business Volume";
    public static string ProcessWise = "Service Wise Booking";
    public static string ProcessWiseSummary = "Service Wise Report Summary";
    public static readonly string ItemWiseSummary = "Item Wise Report Summary";
}

//public enum ReportCaptionsEnum
//{
//   MainCaption = "DryCaption",
//   ItemWise = "Item Wise Report Summmary",
//   CustomerWise = "Customer Wise Business Volume",
//   Duration = "Duration Wise Business Volume",
//   YearWise = "Year Wise Business Volume",
//   MonthWise = "Month Wise Business Volme",
//   DayWise = "Day Wise Business Volume",
//   ProcessWise = "Process Wise Booking",
//   ProcessWiseSummary = "Process Wise Rerport Summary",
//}

public struct ReportLabels
{
    public static string From = "From";
    public static string Upto = "Upto";
    public static string MonthlyReport = "Monthly Report";
    public static string ItemName = "ItemName";
    public static string CustomerName = "Customer Name";
    public static string Default = "Default";
    public static string DefaultView = "Default View";
    public static string Process = "Process";
    public static string Status = "Status";
    public static string ErrorMessage = "Error!";
    public static string ErrorMessageDetailed = "Some error occured!";
    public static string SuccessMessage = "Success!";
    public static string FailureMessage = "Failed to retrive data!";
}

public struct ReportDescriptions
{
    public static string ItemWies = "This report can be used to view item wise summary in the selected period";
    public static string CustomerWise = "This report display: Total Receivables, Customer Business Volume, Total Pending Amount & Quantity, and Compare Customers.";
    public static string DurationWise = "This report gives information around: Total Booking Quantity & Amount, Total Delivered Quantity & Amount, Total Expenses, and Cash Balance. The information is available as Yearly, Monthly and Daily Balances.";
    public static string ProcessWiseSummary = "This report can be used to view Process wise report in a given duration";
}

public class GlobalStatus
{
    //public static int Booking = 1;
    //public static int Shopin = 2;
    //public static int Cancel_Booking = 5;
}

public struct ReportLabelsNew
{
    public static string BkScrItmDim = "Dimensions";
    public static string BkScrItmLen = "Length";
    public static string BkScrItmWth = "Breadth";
    public static string BkScrItmDimCaption = "Please Enter The Dimensions";
    public static string BkScrItmPnl = "Panel";
    public static string BkScrItmPnlCaption = "Please Enter The Number Of Panels";
    public static string BkScrItmPnlDesc = "Number Of Panels";
}

public struct ConfigLabel
{
    public static string Defaultsms = "Default SMS";
}

public struct ClothReturnCause
{
    public static string ClothReturnCauses = "Cloth Return Cause";
}

public struct bookingSrcMsg
{
    public static string BookingCreation = "Order created.";
    public static string BookingScrSearch = "Order opened in Edit mode.";
    public static string BookingCustEdit = "Customer name Change.";
    public static string BookingDisFlat = "Discount - flat  change.";
    public static string BookingDisPer = "Discount - Percent  change.";
    public static string BookingItemName = "Item name Change.";   
    public static string BookingItemDelete = "Item Delete.";
    public static string BookingEditRight = "Failed attempt to open order in Edit mode due to access rights limitations.";
}

public struct bookingCancelSrcMsg
{
    public static string BookingCancel = "Order accessed through Cancel Order screen.";  
    public static string BookingDelete = "Order accessed through Delete Order screen.";
    public static string BookingDeleted = "Invoice has been deleted.";
    public static string InvoiceOpenMsg = "Order accessed through Search Order screen."; 
}
public struct DeliverySrcMsg
{
    public static string InvoiceSearch = "Order accessed through Deliver Order screen";
    public static string printWitAmtMsg = "Print delivery note with amount.";
    public static string printWithoutAmtMsg = "Print delivery note without amount.";
    public static string SlipOpenMsg = "Invoice is open.";
}

public struct ScreenName
{
    public static string PrintBarcode = "Tags Printed";
    public static string MultipleDel = "Multiple orders delivered or payment accepted";
    public static string PendigStock = "Delivery screen accessed through Pending Stock report";
    public static string DashBoard = "Order accessed through dashboard";
    public static string CancelBooking = "Order Cancellation";
    public static string DeleteBooking = "Delete Order";
    public static string HomeSceen = "Deliver order accessed through Home";
    public static string ScrReportName = "Order details opened from various reports/ screen";
    public static string BookingScreen = "Order Creation / Editing";
    public static string DeliveryScreen = "Garments Delivered or Payment Accepted";
    public static string SearchInvoice = " Search order";
}

public struct SpecialAccessRightName
{
    public static string LedgerRight = "Create/Update Expense & Income Entry";
    public static string AllowDelBarcode = "Do not Force delivery with barcode scanning";
    public static string BackDateBooking = "Allow back date order generation";
    public static string BackDateDel = "Allow back date garments delivery";
    public static string AllowCustExcel = "Allow customer information export to excel";
    public static string MulPaymentAcpRight = "Multiple Payment Screen: Accept payment";
    public static string EditAndDelExp = "Edit/Delete Expense & Income";
    public static string RemoveChallan = "Allow return of unprocessed garments on Send to Workshop and Receive from Workshop screens";
    public static string PrintRateList = "Print Rate List";
    public static string ShowTotal = "Display total of balances in all reports";
    public static string SearchOrderRight = "Enable hot key F2 to search orders";
    public static string StockRecon = "Stock Reconciliation function to be active on one terminal at any given instance";
    public static string DefaultDiscount = "Specify default discount to customer at customer creation and update";
    public static string ExpToExcel = "Enable Export to excel feature through various reports";
    public static string DiscountOnBookingSrc = "Allow discount at the time of order generation";
    public static string SendtoWSMoveAll = "Send to Workshop: Allow complete list of garments to be selected through Move All button";
    public static string DisFromDel = "Allow discount through delivery screen";
    public static string MarkForDelMoveAll = "Mark for Delivery: Allow complete list of garments to be selected through Move All button";
    public static string RecvFromWSMoveAll = "Receive from Workshop: Allow complete list of garments to be selected through Move All button";
}