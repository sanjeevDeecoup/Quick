using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BAL_New_Bookings
    {
        public DataSet BindPriority(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindPriority(BID);
        }

        public DataSet BindPriorityCustom(string BID, string Priority)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindPriorityCustom(BID, Priority);
        }

        public double RetrunRate(double rate1, double rate2)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.RetrunRate(rate1, rate2);
        }

        public string SavePriority(DTO.NewBooking Obj)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SavePriority(Obj);
        }

        public DataSet SaveCustomer(DTO.NewBooking Obj)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveCustomer(Obj);
        }

        public DataSet FillCustomer(string Obj, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.FillCustomer(Obj, BID);
        }

        public DataSet FillHeaderInfo(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.FillHeaderInfo(BID);
        }

        public DataSet BindCheckedBy(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindCheckedBy(BID);
        }

        public DataTable BindGridView(ArrayList GridItems, string Flag, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindGridView(GridItems, Flag, BID);
        }

        public string SetCustSearchCriteria()
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SetCustSearchCriteria();
        }

        public DataSet SetCustSearchGrid(DTO.NewBooking Obj)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SetCustSearchGrid(Obj);
        }

        public string CheckItemCode(string ItemCode, string itemName, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckItemCode(ItemCode, itemName, BID);
        }

        public string SaveItemMaster(DTO.NewBooking Obj, ListBox lst)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveItemMaster(Obj, lst);
        }

        public string SaveProcessMaster(DTO.NewBooking Obj)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveProcessMaster(Obj);
        }

        public void SaveRemarks(string Remarks, string BID)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.SaveRemarks(Remarks, BID);
        }

        public void SaveColor(string ColorName)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.SaveColor(ColorName);
        }

        public int GetCustomerDiscount(string CustCode)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetCustomerDiscount(CustCode);
        }

        public double GetItemRateAccordingProcess(string ItemName, string Process, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetItemRateAccordingProcess(ItemName, Process, BID);
        }

        public double GridTotal(GridView grdSender, string LabelName)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GridTotal(grdSender, LabelName);
        }

        public string GetLastBookinNumber(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetLastBookinNumber(BID);
        }

        public double getServiceTaxAccordingProcess(string Process, double Amt, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.getServiceTaxAccordingProcess(Process, Amt, BID);
        }

        public double getServiceTaxAccordingProcessWhenAfterCondition(string Process, double Amt, string BID, double disAmt)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(Process, Amt, BID, disAmt);
        }

        public double CalculatAllTax(DataTable dt)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CalculatAllTax(dt);
        }

        public DataSet SaveBooking(DTO.NewBooking Ob)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveBooking(Ob);
        }

        public string SaveBookingDetails(DTO.NewBooking Ob)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveBookingDetails(Ob);
        }

        public string SaveBarCode(DTO.NewBooking Ob)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveBarCode(Ob);
        }

        public string SaveAccountEntries(DTO.NewBooking Ob)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SaveAccountEntries(Ob);
        }

        public string ReadColorCode(string colorName)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.ReadColorCode(colorName);
        }

        public string ReadColorName(string colorCode)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.ReadColorName(colorCode);
        }

        public DataSet ReadDataForEdit(string bookingNumber, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.ReadDataForEdit(bookingNumber, BID);
        }

        public DataSet EditBooking(DTO.NewBooking Ob)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.EditBooking(Ob);
        }

        public string CheckGridEntries(string ItemName, string ProcessName, string ExtP1, string ExtP2, string rate, string rate1, string rate2, string amt, string advance, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckGridEntries(ItemName, ProcessName, ExtP1, ExtP2, rate, rate1, rate2, amt, advance, BID);
        }

        public string GetDefaultColor()
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetDefaultColor();
        }

        public DataSet GetSMSInformation(string BookingNo, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetSMSInformation(BookingNo, BID);
        }

        public string GetItemNameWhenDataSave(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetItemNameWhenDataSave(BID);
        }

        public bool CheckInvoiceNo(string InvoiceNo, string BID, string BookingPrefix)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckInvoiceNo(InvoiceNo, BID,BookingPrefix);
        }

        public double GetDiscountAmountForDeliveryForm(double Amt, double Discount)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetDiscountAmountForDeliveryForm(Amt, Discount);
        }

        public bool CheckDiscountApplicationOnProces(string ProcessName, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckDiscountApplicationOnProces(ProcessName, BID);
        }

        public string DisplayNetAmountFlatOrFolat(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.DisplayNetAmountFlatOrFolat(BID);
        }

        public string DisplayAmountFormat(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.DisplayAmountFormat(BID);
        }

        public string GetNoOfClothesReceived(string itemName, string ISN, string bookingNo, string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetNoOfClothesReceived(itemName, ISN, bookingNo, BranchId);
        }

        public string GetNoOfClothesReceivedForComarping(string itemName, string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetNoOfClothesReceivedForComarping(itemName, BranchId);
        }

        public bool CheckEditBookingRights(string Userid, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckEditBookingRights(Userid, BID);
        }

        public DataSet FirstTimeDefaultData(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.FirstTimeDefaultData(BID);
        }

        public string CountNoOfSubItem(string ItemName, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CountNoOfSubItem(ItemName, BID);
        }

        public string FindTotalTaxActive(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.FindTotalTaxActive(BID);
        }

        public string BindRemarksInUI(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindRemarksInUI(BID);
        }

        public string BindColorsInUI(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindColorsInUI(BID);
        }

        public string BindColorsInUINew(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BindColorsInUINew(BID);
        }

        public double GetNextDayRate(string BID, string Flag)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetNextDayRate(BID, Flag);
        }

        public string GetNextDayRateAndDayOffset(string BID, string Flag)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetNextDayRateAndDayOffset(BID, Flag);
        }

        public string FindDefaultPrinter(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.FindDefaultPrinter(BID);
        }

        public bool CheckIfCustomerExists(string CustName, string BranchID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckIfCustomerExists(CustName, BranchID);
        }

        public bool CheckIfCustomerExists(string CustName, string CustAddr, string BranchID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckIfCustomerExists(CustName, CustAddr, BranchID);
        }

        public string LoadAllItems(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.LoadAllItems(BID);
        }

        public string LoadAllProcesses(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.LoadAllProcesses(BID);
        }

        public string LoadAllTax(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.LoadAllTax(BID);
        }

        public string LoadInclusiveExclusive(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.LoadInclusiveExclusive(BID);
        }

        public bool BackDateBookingAvailable(string UserID, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.BackDateBookingAvailable(UserID, BID);
        }

        public string checkIfDesAndColorEnabled(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.checkIfDesAndColorEnabled(BID);
        }

        public string checkDescAndColorForBinding(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.checkDescAndColorForBinding(BID);
        }

        public string CheckLenBredth(string BID, string argItemName)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckLenBredth(BID, argItemName);
        }

        public bool SetQtySpaceOrOne(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SetQtySpaceOrOne(BID);
        }

        public string FindDefaultDiscountType(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.FindDefaultDiscountType(BID);
        }

        public bool ConfirmDelivery(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.ConfirmDelivery(BID);
        }

        public bool SetItemRateForProcess(string ItemName, string ProcessName, string argExtraProcess, string argExtraProcess2, double rate, double rate1, double rate2, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.SetItemRateForProcess(ItemName, ProcessName, argExtraProcess, argExtraProcess2, rate, rate1, rate2, BID);
        }

        public bool CheckIfBindColorToQty(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckIfBindColorToQty(BID);
        }

        public string CheckMeansOfCommunication(string customerCode, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckMeansOfCommunication(customerCode, BID);
        }

        public string CheckDetailsOfPackage(string customerCode, string bookingDate, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.CheckDetailsOfPackage(customerCode, bookingDate, BID);
        }

        public void UpdateCustomer(string CustId, string mobileNo, string emailId, string commPref, string BID)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.UpdateCustomer(CustId, mobileNo, emailId, commPref, BID);
        }

        public string LoadThePassWords(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.LoadThePassWords(BID);
        }

        public void BackUpBookings(string bookingNumber, string userName, string editBookingRemarks, string branchId)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.BackUpBookings(bookingNumber, userName, editBookingRemarks, branchId);
        }

        public void BackUpBookingDetails(string bookingNumber, string userName, string branchId)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.BackUpBookingDetails(bookingNumber, userName, branchId);
        }

        public string LoadDefaultSearchCriteriaForCustomer(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.LoadDefaultSearchCriteriaForCustomer(branchId);
        }

        public bool checkForEditRemarks(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.checkForEditRemarks(branchId);
        }

        public string checkForDelDiscountPwd(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.checkForDelDiscountPwd(branchId);
        }

        public void BackUpPayment(string bookingNumber, string userName, string branchId)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.BackUpPayment(bookingNumber, userName, branchId);
        }

        public bool IsMobileUnique(string mobileNo, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.IsMobileUnique(mobileNo, branchId);
        }

        public void BackUpWholeBooking(string bookingNumber, string userName, string editingRemark, string branchId)
        {
            DAL.DALFactory.Instance.DAL_New_Bookings.BackUpWholeBooking(bookingNumber, userName, editingRemark, branchId);
        }
        public DataSet GetBookingPrefix(string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetBookingPrefix(BID);
        }
        public string InvoiceEventHistorySaveData(string bookingNumber, string userName, string BID, string Reason, string ScreenName,int ScreenID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.InvoiceEventHistorySaveData(bookingNumber, userName, BID, Reason, ScreenName, ScreenID);
        }
        public DataSet GetInvoiceBookingData(string BID, string BookingNo)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.GetInvoiceBookingData(BID, BookingNo);
        }
        public string updateInvoiceHistoryDeleteData(string bookingNumber, string BID)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.updateInvoiceHistoryDeleteData(bookingNumber,  BID);
        }
        public string InsertInvoiceHistoryData(string BookingNo, string ItemRowIndex, string BID, string UserName)
        {
            return DAL.DALFactory.Instance.DAL_New_Bookings.InsertInvoiceHistoryData(BookingNo, ItemRowIndex,BID ,UserName);
        }
    }
}