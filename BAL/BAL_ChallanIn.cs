using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BAL_ChallanIn
    {
        public string SaveBarcodeWiseChallan(GridView grdChallan, string Shift, string RowIndex, string BID, string challanNo, string ExternalBID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveBarcodeWiseChallan(grdChallan, Shift, RowIndex, BID, challanNo, ExternalBID);
        }

        public string SaveEntryWiseChallan(GridView grdChallan, string Shift, string BID, string challanNo, string ExternalBID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryWiseChallan(grdChallan, Shift, BID, challanNo, ExternalBID);
        }

        public string SaveEntryWiseChallanFromRowData(string rowData, string Shift, string BID, string challanNo, string ExternalBID, string AcceptedByUser)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryWiseChallanFromRowData(rowData, Shift, BID, challanNo, ExternalBID, AcceptedByUser);
        }
        public string SaveInStickerTableDataFromWorkShop(string BID, int printFrom, string ChallanNumber)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveInStickerTableDataFromWorkShop(BID, printFrom, ChallanNumber);
        }
        public DataSet BindGrid(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindGrid(Ob);
        }

        public string GetChallanNumberFromBookingAndItemSNo(string bookingNumber, string itemSerialNumber, string BID, bool isFactory = false)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetChallanNumberFromBookingAndItemSNo(bookingNumber, itemSerialNumber, BID, isFactory);
        }

        public string findDefaultChallanType(string BID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.findDefaultChallanType(BID);
        }

        public string SaveEntryWiseChallanReturnRowData(string rowData, string BID, string Satatus, string AcceptedByUser)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryWiseChallanReturnRowData(rowData, BID, Satatus, AcceptedByUser);
        }

        public string SaveRemoveChallan(string allRemoveReasonData, string DeliverItemStatus, string UserName, string BID, string Flag, string ScreenName)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveRemoveChallan(allRemoveReasonData, DeliverItemStatus, UserName, BID, Flag, ScreenName);
        }

        public DataSet BindGridView(string BID, DropDownList drpProcess, TextBox txtHolidayDate, HiddenField hdnCheckStatus, HiddenField hdnInvoiceNo, HiddenField hdnRowNo, bool challanStatus = false)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindGridView(BID, drpProcess, txtHolidayDate, hdnCheckStatus, hdnInvoiceNo, hdnRowNo, challanStatus);
        }
        public DataSet BindRightGrid(string BID, int status,string userId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindRightGrid(BID, status, userId);
        }

        public string SaveInStickerTable(string rowData, string BID, int PrintFrom)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveInStickerTable(rowData, BID, PrintFrom);
        }

        public string SaveInStickerTableFromFactory(string rowData, string BID, int PrintFrom)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveInStickerTableFromFactory(rowData, BID, PrintFrom);
        }
        public DataSet BindCustomerWise(string BID, string CustomerName)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindCustomerWise(BID, CustomerName);
        }
        public bool CheckStatus()
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.CheckStatus();
        }

        public DataSet GetDataChallanReturnScreenFirst(string BID, string BookNumberFrom, string BookNumberUpto, string DueDate, string ChallanShift, string Process, string challanNumber = "")
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetDataChallanReturnScreenFirst(BID, BookNumberFrom, BookNumberUpto, DueDate, ChallanShift, Process, challanNumber);
        }

        public DataSet GetDataChallanReturnScreenSecond(string BID, string BookNumberFrom, string BookNumberUpto, string DueDate, string ProcessCode)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetDataChallanReturnScreenSecond(BID, BookNumberFrom, BookNumberUpto, DueDate, ProcessCode);
        }

        public DataSet BindFactoryInGrid(string BID, string EXBID, string BookingNo, string BookingDate, string DueDate, bool IsUrgent)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindFactoryInGrid(BID, EXBID, BookingNo, BookingDate, DueDate, IsUrgent);
        }

        public string SaveEntryWiseFactoryInRowData(string rowData, string BID, string AcceptedByUser)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryWiseFactoryInRowData(rowData, BID, AcceptedByUser);
        }

        public DataSet BindFactoryOutGrid(string BID, string EXBID, string BookingNo, string BookingDate, string DueDate, bool IsUrgent)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindFactoryOutGrid(BID, EXBID, BookingNo, BookingDate, DueDate, IsUrgent);
        }

        public string SaveEntryWiseFactoryOutRowData(string rowData, string BID, string AcceptedByUser, string workshopNote)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryWiseFactoryOutRowData(rowData, BID, AcceptedByUser, workshopNote);
        }

        public DataSet GetDataSteamPressScreen(string BID, string BookNumberFrom, string DueDate, string Process)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetDataSteamPressScreen(BID, BookNumberFrom, DueDate, Process);
        }

        public DataSet GetDataForPritingLablesForWorkshop(string challanNum, string externalBranchId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetDataForPritingLablesForWorkshop(challanNum, externalBranchId);
        }
        public DataSet GetDataForAllPrintingLabelsForWorkshop(string challanNum, string externalBranchId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetDataForAllPrintingLabelsForWorkshop(challanNum, externalBranchId);
        }

        public string AskForBarCodeAndPIN(string branchId, string UserId, string UserType)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AskForBarCodeAndPIN(branchId, UserId, UserType);
        }

        public string SavePinInBarcode(string PIN, string barCodes, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SavePinInBarcode(PIN, barCodes, branchId);
        }

        public string DeliveryEntStatus(string branchId, string UserId, string DeliveryDate, string BookingId, string deliverTime, int StatusID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.DeliveryEntStatus(branchId, UserId, DeliveryDate, BookingId, deliverTime, StatusID);
        }

        public DataSet GetAllActiveChallans(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetAllActiveChallans(branchId);
        }

        public bool ChangeChallanStatus(string barCodes, int challanStatus, string branchId,string userId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.ChangeChallanStatus(barCodes, challanStatus, branchId, userId);
        }
        public string XMLSaveEntryWiseChallanFromRowData(string rowData, string BID, string ExternalBID, string AcceptedByUser)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.XMLSaveEntryWiseChallanFromRowData(rowData, BID, ExternalBID, AcceptedByUser);
        }
        public bool SaveChallanDetails(string BID, string EBID, string strdate, string strTime, string UserName, bool PrintSticker, int Possition)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveChallanDetails(BID, EBID, strdate, strTime, UserName, PrintSticker, Possition);
        }

        public bool SendForFinishingOrReadyDetail(string BID, string Status, string strdate, string strTime, string UserName, bool PrintSticker, int Possition, int ScreenStatus, string PinNo)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SendForFinishingOrReadyDetail(BID, Status, strdate, strTime, UserName, PrintSticker, Possition, ScreenStatus, PinNo);
        }
        public bool SaveChallanAndWorkShopNoteAndPrintStickerDetail(string BID, string Status, string strdate, string strTime, string UserName, bool PrintSticker, int Possition, int ScreenStatus, string PinNo)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveChallanAndWorkShopNoteAndPrintStickerDetail(BID, Status, strdate, strTime, UserName, PrintSticker, Possition, ScreenStatus, PinNo);
        }
        public bool ChangeWorkshopChallanStatus(string barCodes, int challanStatus, string ExBID,string UserId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.ChangeWorkshopChallanStatus(barCodes, challanStatus, ExBID,UserId);
        }
        public DataSet BindWorkshopRightGrid(string BID, int status, string UserId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindWorkshopRightGrid(BID, status, UserId);
        }
        public bool SaveReceiveFromStoreDetail(string BID, string Status, string strdate, string strTime, string UserName, int ScreenStatus)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveReceiveFromStoreDetail(BID, Status, strdate, strTime, UserName, ScreenStatus);
        }
        public bool SaveSendToStoreDetail(string BID, string Status, string strdate, string strTime, string UserName, bool PrintSticker, int Possition, int ScreenStatus, string WorkShopNote)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveSendToStoreDetail(BID, Status, strdate, strTime, UserName, PrintSticker, Possition, ScreenStatus, WorkShopNote);
        }
        public bool ChangeChallanStatusForPrintStickerData(string barCodes, int challanStatus, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.ChangeChallanStatusForPrintStickerData(barCodes, challanStatus, branchId);
        }
        public DataSet BindRightGridForSticker(string BID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindRightGridForSticker(BID);
        }
        public string SaveInStickerTableData(string BID, int PrintFrom)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveInStickerTableData(BID, PrintFrom);
        }
        public bool ChangeChallanStatusForPrintStickerWSData(string barCodes, int challanStatus, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.ChangeChallanStatusForPrintStickerWSData(barCodes, challanStatus, branchId);
        }
        public DataSet BindRightGridForWSSticker(string BID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindRightGridForWSSticker(BID);
        }
        public string SaveInWorkShopStickerTableData(string BID, int PrintFrom)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveInWorkShopStickerTableData(BID, PrintFrom);
        }
        public DataSet GetBookingNoAndPrefix(string BookingNo)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.GetBookingNoAndPrefix(BookingNo);
        }

        public DataSet BindGridWorkshopMenuRight(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindGridWorkshopMenuRight(Ob);
        }
    }
}