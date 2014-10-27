using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class Bal_Report
    {
        public DataSet GetDeliveryFrom_To_UptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDeliveryFrom_To_UptoDate(Ob);
        }

        public DataSet GetDataMainReport(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataMainReport(Ob);
        }
        public DataSet GetDataMainReportBookingDiscount(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataMainReportBookingDiscount(Ob);
        }
        public DataSet GetDataMainReportDeliveryDiscount(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataMainReportDeliveryDiscount(Ob);
        }
        public DataSet GetDataMainReportAreaLocation(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataMainReportAreaLocation(Ob);
        }
        public DataSet GetDataMainReportEditRecord(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataMainReportEditRecord(Ob);
        }
        public DataSet GetDailyCustomerAdditionReport(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDailyCustomerAdditionReport(Ob);
        }

        public DataSet GetInvoiceStatement(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetInvoiceStatement(Ob);
        }

        public DataSet GetInvoiceStatementForCustomer(String BranchID, string Fromdate, string UptoDate, string CustCode)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetInvoiceStatementForCustomer( BranchID, Fromdate, UptoDate,CustCode);
        }

        public DataSet GetDataMainReportwithHome(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataMainReportwithHome(Ob);
        }

        public DataSet BindWorkShopChallan(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindWorkShopChallan(Ob);
        }

        public DataSet BindServiceTax(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindServiceTax(Ob);
        }

        public DataSet GetDeliveryByInvoiceNo(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDeliveryByInvoiceNo(Ob);
        }

        public DataSet GetSaleFrom_To_UptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetSaleFrom_To_UptoDate(Ob);
        }

        public DataSet GetSaleByInvoiceNo(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetSaleByInvoiceNo(Ob);
        }

        public DataSet GetDeliveryAndSalesFrom_To_UptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDeliveryAndSalesFrom_To_UptoDate(Ob);
        }

        public DataSet GetDeliveryAndSalesInvoiceNo(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDeliveryAndSalesInvoiceNo(Ob);
        }
        public bool CheckOrginalUser(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.CheckOrginalUser(Ob);
        }

        public DataSet GetDeliveryByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDeliveryByCustomer(Ob);
        }

        public DataSet PrintBarcodeDropDown(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.PrintBarcodeDropDown(Ob);
        }

        public DataSet GetSaleFrom_To_UptoDateByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetSaleFrom_To_UptoDateByCustomer(Ob);
        }

        public DataSet GetDeliveryAndSalesFrom_To_UptoDateByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDeliveryAndSalesFrom_To_UptoDateByCustomer(Ob);
        }

        public DataSet ImportRateList(string Path)
        {
            return DAL.DALFactory.Instance.DAL_Report.ImportRateList(Path);
        }

        public string SaveRateList(DataSet ds, ArrayList PName, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.SaveRateList(ds, PName, BID);
        }

        public DataSet GetReasonToRemove(DTO.Report Ob, string Type, string ID)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetReasonToRemove(Ob, Type, ID);
        }

        public DataSet GetMultipleDeliveryByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultipleDeliveryByCustomer(Ob);
        }

        public DataSet GetMultipleDeliveryByFromToUptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultipleDeliveryByFromToUptoDate(Ob);
        }

        public DataSet GetCancelDataMainReport(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetCancelDataMainReport(Ob);
        }

        public DataSet GetMultiplePaymentByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultiplePaymentByCustomer(Ob);
        }

        public DataSet GetMultiplePaymentByFromToUptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultiplePaymentByFromToUptoDate(Ob);
        }

        public DataSet GetMultipleBothOutPutByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultipleBothOutPutByCustomer(Ob);
        }

        public DataSet GetMultipleBothOutPutFromToUptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultipleBothOutPutFromToUptoDate(Ob);
        }

        public string SaveMultiplePendingCloth(GridView grdPendingCloth, string BID, string UserName)
        {
            return DAL.DALFactory.Instance.DAL_Report.SaveMultiplePendingCloth(grdPendingCloth, BID, UserName);
        }

        public string SaveMultiplePendingPayment(GridView grdPendingCloth, string BID, string userName, string PaymentMode, string PaymentDetails)
        {
            return DAL.DALFactory.Instance.DAL_Report.SaveMultiplePendingPayment(grdPendingCloth, BID, userName, PaymentMode, PaymentDetails);
        }

        public string SaveDeliveryAndPayment(GridView grdPendingCloth, string BID, string userName, string PaymentMode, string PaymentDetails)
        {
            return DAL.DALFactory.Instance.DAL_Report.SaveDeliveryAndPayment(grdPendingCloth, BID, userName, PaymentMode, PaymentDetails);
        }

        public DataSet GetPendingReceiptParticularCustomer(DTO.Report Ob, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetPendingReceiptParticularCustomer(Ob, BID);
        }

        public DataSet GetMonthlyStausByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMonthlyStausByCustomer(Ob);
        }

        public DataSet GetMonthlyStausByDetailed(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMonthlyStausByDetailed(Ob);
        }

        public DataSet GetMonthlyStausDetailed(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMonthlyStausDetailed(Ob);
        }

        public DataSet GetMonthlStatusSubItem(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMonthlStatusSubItem(Ob);
        }

        public bool BookingDateNotEarlierDeliveryDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BookingDateNotEarlierDeliveryDate(Ob);
        }

        public bool CheckDiscountOnBookingNumber(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.CheckDiscountOnBookingNumber(Ob);
        }

        public DataSet GetTodayDeliverysummary(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetTodayDeliverysummary(Ob);
        }

        public DataSet GetTodayDeliverydetailed(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetTodayDeliverydetailed(Ob);
        }

        public DataSet GetTodayDeliveryUpdatesummary(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetTodayDeliveryUpdatesummary(Ob);
        }

        public DataSet GetTodayDeliveryUpdatedetailed(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetTodayDeliveryUpdatedetailed(Ob);
        }

        public DataSet BindProcessDropDown(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindProcessDropDown(Ob);
        }

        public DataSet BinItemDropDown(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BinItemDropDown(Ob);
        }

        public DataSet BindItem(DTO.Sub_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindItem(Ob);
        }

        public DataSet BindStockReconcile(string Id, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindStockReconcile(Id, BID);
        }

        public DataSet BindStockMatchReconcile(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindStockMatchReconcile(BID);
        }

        public DataSet BindStockMatchNotReconcile(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindStockMatchNotReconcile(BID);
        }

        public bool GetStatusNegativeEntry(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetStatusNegativeEntry(Ob);
        }

        public string CheckClothStatus(GridView grdPendingCloth)
        {
            return DAL.DALFactory.Instance.DAL_Report.CheckClothStatus(grdPendingCloth);
        }

        public DataSet GetDataItemReport(DTO.Report Ob, bool bIsAll)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDataItemReport(Ob, bIsAll);
        }

        public DataSet GetCustomerWiseSummary(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetCustomerWiseSummary(Ob);
        }

        public DataSet GetDurationWiseReport(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetDurationWiseReport(Ob);
        }

        public DataSet GetProcessWiseSummary(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetProcessWiseSummary(Ob);
        }

        public DataSet GetServiceAndGarment(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetServiceAndGarment(Ob);
        }

        public string FindTaxLabels(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.FindTaxLabels(BID);
        }

        public DataSet BindUserTypeDropDown(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindUserTypeDropDown(Ob);
        }

        public DataSet BindUserRightsReport(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.BindUserRightsReport(Ob);
        }

        public void ChangeStatusAccordingBooking(string BookingNo, string BID)
        {
            DAL.DALFactory.Instance.DAL_Report.ChangeStatusAccordingBooking(BookingNo, BID);
        }

        public string PendingReceiptParticularCustomer(string CustId, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.PendingReceiptParticularCustomer(CustId, BID);
        }
        public double ReturnCustomerPendingBalance(string CustCode, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.ReturnCustomerPendingBalance(CustCode, BID);
        }
        public double ReturnCustomerPendingBalanceBookingWise(string CustCode, string BID, string BookingNo)
        {
            return DAL.DALFactory.Instance.DAL_Report.ReturnCustomerPendingBalanceBookingWise(CustCode, BID, BookingNo);
        }

        public string SetBusinessVolumeDefaultView(string defaultView, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.SetBusinessVolumeDefaultView(defaultView, BID);
        }

        public string GetBusinessVolumeDefaultView(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetBusinessVolumeDefaultView(BID);
        }

        public string findWorkShopRemark(string bookingNumber, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_Report.findWorkShopRemark(bookingNumber, branchId);
        }

        public DataSet GetPendingStockReport(DTO.Report Ob, bool bIsAll)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetPendingStockReport(Ob, bIsAll);
        }

        public DataSet StockPendingInvoice(DTO.Report Ob, bool bIsAll)
        {
            return DAL.DALFactory.Instance.DAL_Report.StockPendingInvoice(Ob, bIsAll);
        }

        public DataSet GetCustomerWiseSummaryPendingSms(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetCustomerWiseSummaryPendingSms(Ob);
        }
        public DataSet GetSlipDetails(string BookingNo, string BID, string procName)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetSlipDetails(BookingNo, BID, procName);
        }

        public DataSet GetItemWiseSummary(DTO.Report Ob)
        {
            // check if required field are not null

            // strCodes is the itemName
            if (string.IsNullOrEmpty(Ob.StrCodes))
                throw new ArgumentNullException("Item name was not supplied properly");

            /*
            DateTime fromDate;
            // strCodes is the startDate
            if (DateTime.TryParse(Ob.FromDate, out fromDate))
                throw new ArgumentNullException("Start date was not supplied properly");

            DateTime uptoDate;
            // strCodes is the endDate
            if (DateTime.TryParse(Ob.UptoDate, out uptoDate))
                throw new ArgumentNullException("Start date was not supplied properly");
            */

            // Invoice is flag
            if (string.IsNullOrEmpty(Ob.InvoiceNo))
                throw new ArgumentNullException("Flag was not supplied properly");

            // Invoice is BranchId
            if (string.IsNullOrEmpty(Ob.BranchId))
                throw new ArgumentNullException("Branch Id was not supplied properly");

            return DAL.DALFactory.Instance.DAL_Report.GetItemWiseSummary(Ob);
        }

        public DataSet LoadBookingHistoryForBookingNumber(string bookingNumber, string branchId, string status, string StartDate, string EndDate)
        {
            return DAL.DALFactory.Instance.DAL_Report.LoadBookingHistoryForBookingNumber(bookingNumber, branchId, status, StartDate, EndDate);
        }

        public DataSet GetPackageReportSummary(DTO.Report Obj, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetPackageReportSummary(Obj, branchId);
        }

        public DataSet GetPackageReportDetail(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetPackageReportDetail(Ob);
        }

        public bool IsPrinterLaser(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_Report.IsPrinterLaser(branchId: branchId);
        }

        public DataSet GetListOfEditedBookings(string strFromDate, string strToDate, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetListOfEditedBookings(strFromDate, strToDate, branchId);
        }

        public DataSet GetExpenseReport(string strStartDate, string strToDate, string strReportType, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetExpenseReport(strStartDate, strToDate, strReportType, BID);
        }

        public DataSet GetStockReconcilationReport(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetStockReconcilationReport(BID);
        }

        public DataSet GetCorrectBaroceNo(string Barcode, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetCorrectBaroceNo(Barcode, branchId);
        }

        public DataSet MatchClothesStockreconcilation(string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_Report.MatchClothesStockreconcilation(BranchId);
        }

        public string UpdateConsolidatedStatus(string barCodes, string branchID)
        {
            return DAL.DALFactory.Instance.DAL_Report.UpdateConsolidatedStatus(barCodes, branchID);
        }

        public void ResetRightGrid(string branchId)
        {
            DAL.DALFactory.Instance.DAL_Report.ResetRightGrid(branchId);
        }

        public void ResetAllGrid(string branchId)
        {
            DAL.DALFactory.Instance.DAL_Report.ResetAllGrid(branchId);
        }

        public void UpdateStatus(string branchId)
        {
            DAL.DALFactory.Instance.DAL_Report.UpdateStatus(branchId);
        }

        public DataSet GetGarmentReadyReport(DTO.Report Obj)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetGarmentReadyReport(Obj);
        }

        public DataSet GetMarkeReadyData(DTO.Report Obj)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMarkeReadyData(Obj);
        }

        public void SaveItemWiseRateList(string processes, string ItemWithRates, int rateListId, string branchId)
        {
            DAL.DALFactory.Instance.DAL_Report.SaveItemWiseRateList(processes, ItemWithRates, rateListId, branchId);
        }
        public string PendingOrderParticularCustomer(string CustId, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Report.PendingOrderParticularCustomer(CustId, BID);
        }
        public DataSet GetMultipleOutstandingGarmentByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetMultipleOutstandingGarmentByCustomer(Ob);
        }
        public DataSet GetOutstandinGarmentFromToUptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetOutstandinGarmentFromToUptoDate(Ob);
        }
        public DataSet GetPendingPaymentFromToUptoDate(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetPendingPaymentFromToUptoDate(Ob);
        }
        public DataSet GetOutstandingPaymentByCustomer(DTO.Report Ob)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetOutstandingPaymentByCustomer(Ob);
        }
        public DataSet GetInvoiceHistoeyDetails(string bookingNumber, string branchId, string isBookingNo, string ScreenID,string UserName)
        {
            return DAL.DALFactory.Instance.DAL_Report.GetInvoiceHistoeyDetails(bookingNumber, branchId, isBookingNo, ScreenID, UserName);
        }
       
    }
}