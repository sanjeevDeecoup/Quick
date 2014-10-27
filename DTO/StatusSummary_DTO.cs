using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class StatusSummary_DTO
    {
        // Set Text Box
        private string _ucBookingTxtRptFrom;
        private string _ucBookingTxtRptTo;
        private string _ucSalesTxtRptFrom;
        private string _ucSalesTxtRptTo;
        private string _ucDeliveryTxtRptFrom;
        private string _ucDeliveryTxtRptTo;
        private string _ucPaymentTypeTxtRptFrom;
        private string _ucPaymentTypeTxtRptTo;
        private string _ucDailyCustomerTxtRptFrom;
        private string _ucDailyCustomerTxtRptTo;
        private string _ucDetailCashBookTxtRptFrom;
        private string _ucDetailCashBookTxtRptTo;
        private string _ReportTime;
        private string _FooterRights;
        private string _Description;

        private int _TotNewBookingOrder;
        private double _TotBookingAmount;
        private double _TotBookingAdvance;

        private int _TotDelQty;
        private int _TotalQty;
        private int _TotDelBalQty;

        private int _SalesTotOrder;
        private double _SalesTotPaymentMade;
        private double _SalesTotDisAmt;
        private double _SalesTotBalAmt;

        private double _CashBookOpeningBalAmt;
        private double _CashBookReceivedAmt;
        private double _CashBookPaymentAmt;
        private double _CashBookBalanceAmt;

        private int _DailyCustomerNewAdded;
        private double _DailyCustomerTotAmtNewCust;
        private double _DailyCustomerTotBuseAmt;
        private double _DailyCustomerPercent;
        private string _DailyCustomerCurrencyType;
                
        //////// Text Box Get Set        

        public string Description
        {
            get { return this._Description; }
            set { this._Description = value; }
        }

        public string ucBookingTxtRptFrom
        {
            get { return this._ucBookingTxtRptFrom; }
            set { this._ucBookingTxtRptFrom = value; }
        }
        public string ucBookingTxtRptTo
        {
            get { return this._ucBookingTxtRptTo; }
            set { this._ucBookingTxtRptTo = value; }
        }
        public string ucSalesTxtRptFrom
        {
            get { return this._ucSalesTxtRptFrom; }
            set { this._ucSalesTxtRptFrom = value; }
        }
        public string ucSalesTxtRptTo
        {
            get { return this._ucSalesTxtRptTo; }
            set { this._ucSalesTxtRptTo = value; }
        }
        public string ucDeliveryTxtRptFrom
        {
            get { return this._ucDeliveryTxtRptFrom; }
            set { this._ucDeliveryTxtRptFrom = value; }
        }
        public string ucDeliveryTxtRptTo
        {
            get { return this._ucDeliveryTxtRptTo; }
            set { this._ucDeliveryTxtRptTo = value; }
        }
        public string ucPaymentTypeTxtRptFrom
        {
            get { return this._ucPaymentTypeTxtRptFrom; }
            set { this._ucPaymentTypeTxtRptFrom = value; }
        }
        public string ucPaymentTypeTxtRptTo
        {
            get { return this._ucPaymentTypeTxtRptTo; }
            set { this._ucPaymentTypeTxtRptTo = value; }
        }
        public string ucDailyCustomerTxtRptFrom
        {
            get { return this._ucDailyCustomerTxtRptFrom; }
            set { this._ucDailyCustomerTxtRptFrom = value; }
        }
        public string ucDailyCustomerTxtRptTo
        {
            get { return this._ucDailyCustomerTxtRptTo; }
            set { this._ucDailyCustomerTxtRptTo = value; }
        }       
        public string ucDetailCashBookTxtRptFrom
        {
            get { return this._ucDetailCashBookTxtRptFrom; }
            set { this._ucDetailCashBookTxtRptFrom = value; }
        }
        public string ucDetailCashBookTxtRptTo
        {
            get { return this._ucDetailCashBookTxtRptTo; }
            set { this._ucDetailCashBookTxtRptTo = value; }
        }
        public string ReportTime
        {
            get { return this._ReportTime; }
            set { this._ReportTime = value; }
        }
        public string FooterRights
        {
            get { return this._FooterRights; }
            set { this._FooterRights = value; }
        }

        public int TotNewBookingOrder
        {
            get { return this._TotNewBookingOrder; }
            set { this._TotNewBookingOrder = value; }
        }

        public double TotBookingAmount
        {
            get { return this._TotBookingAmount; }
            set { this._TotBookingAmount = value; }
        }

        public double TotBookingAdvance
        {
            get { return this._TotBookingAdvance; }
            set { this._TotBookingAdvance = value; }
        }

        public int TotDelQty
        {
            get { return this._TotDelQty; }
            set { this._TotDelQty = value; }
        }

        public int TotalQty
        {
            get { return this._TotalQty; }
            set { this._TotalQty = value; }
        }

        public int TotDelBalQty
        {
            get { return this._TotDelBalQty; }
            set { this._TotDelBalQty = value; }
        }


        public int SalesTotOrder
        {
            get { return this._SalesTotOrder; }
            set { this._SalesTotOrder = value; }
        }
        public double SalesTotPaymentMade
        {
            get { return this._SalesTotPaymentMade; }
            set { this._SalesTotPaymentMade = value; }
        }

        public double  SalesTotDisAmt
        {
            get { return this._SalesTotDisAmt; }
            set { this._SalesTotDisAmt = value; }
        }

        public double SalesTotBalAmt
        {
            get { return this._SalesTotBalAmt; }
            set { this._SalesTotBalAmt = value; }
        }

        public double CashBookOpeningBalAmt
        {
            get { return this._CashBookOpeningBalAmt; }
            set { this._CashBookOpeningBalAmt = value; }
        }

        public double CashBookReceivedAmt
        {
            get { return this._CashBookReceivedAmt; }
            set { this._CashBookReceivedAmt = value; }
        }

        public double CashBookPaymentAmt
        {
            get { return this._CashBookPaymentAmt; }
            set { this._CashBookPaymentAmt = value; }
        }

        public double CashBookBalanceAmt
        {
            get { return this._CashBookBalanceAmt; }
            set { this._CashBookBalanceAmt = value; }
        }        

        public int  DailyCustomerNewAdded
        {
            get { return this._DailyCustomerNewAdded; }
            set { this._DailyCustomerNewAdded = value; }
        }

        public double DailyCustomerTotAmtNewCust
        {
            get { return this._DailyCustomerTotAmtNewCust; }
            set { this._DailyCustomerTotAmtNewCust = value; }
        }
        public double DailyCustomerTotBuseAmt
        {
            get { return this._DailyCustomerTotBuseAmt; }
            set { this._DailyCustomerTotBuseAmt = value; }
        }
        public double DailyCustomerPercent
        {
            get { return this._DailyCustomerPercent; }
            set { this._DailyCustomerPercent = value; }
        }
        public string DailyCustomerCurrencyType
        {
            get { return this._DailyCustomerCurrencyType; }
            set { this._DailyCustomerCurrencyType = value; }
        }
                  
    }
}
