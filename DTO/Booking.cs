using System;

namespace DTO
{
    public class Booking
    {
        private int _BookingID;
        private string _BookingNumber;
        private int _IsHomeReceipt;
        private string _HomeReceiptNumber;
        private int _CustomerID;
        private DateTime _DueDate;
        private string _DueTime;
        private int _IsUrgent;
        private int _IsSMS;
        private int _IsEmail;
        private string _ReceiptRemarks;
        private int _SalesManUserID;
        private int _CheckedByUserID;

        private double _TotalGrossAmount;
        private double _TotalDiscount;
        private double _TotalTax;
        private double _TotalAdvance;
        private int _ReceiptStatus;
        private string _XMLName;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int BookingID
        {
            get { return this._BookingID; }
            set { this._BookingID = value; }
        }

        public string BookingNumber
        {
            get { return this._BookingNumber; }
            set { this._BookingNumber = value; }
        }

        public int IsHomeReceipt
        {
            get { return this._IsHomeReceipt; }
            set { this._IsHomeReceipt = value; }
        }

        public string HomeReceiptNumber
        {
            get { return this._HomeReceiptNumber; }
            set { this._HomeReceiptNumber = value; }
        }

        public int CustomerID
        {
            get { return this._CustomerID; }
            set { this._CustomerID = value; }
        }

        public DateTime DueDate
        {
            get { return this._DueDate; }
            set { this._DueDate = value; }
        }

        public string DueTime
        {
            get { return this._DueTime; }
            set { this._DueTime = value; }
        }

        public int IsUrgent
        {
            get { return this._IsUrgent; }
            set { this._IsUrgent = value; }
        }

        public int IsSMS
        {
            get { return this._IsSMS; }
            set { this._IsSMS = value; }
        }

        public int IsEmail
        {
            get { return this._IsEmail; }
            set { this._IsEmail = value; }
        }

        public string ReceiptRemarks
        {
            get { return this._ReceiptRemarks; }
            set { this._ReceiptRemarks = value; }
        }

        public int SalesManUserID
        {
            get { return this._SalesManUserID; }
            set { this._SalesManUserID = value; }
        }

        public int CheckedByUserID
        {
            get { return this._CheckedByUserID; }
            set { this._CheckedByUserID = value; }
        }

        public double TotalGrossAmount
        {
            get { return this._TotalGrossAmount; }
            set { this._TotalGrossAmount = value; }
        }

        public double TotalDiscount
        {
            get { return this._TotalDiscount; }
            set { this._TotalDiscount = value; }
        }

        public double TotalTax
        {
            get { return this._TotalTax; }
            set { this._TotalTax = value; }
        }

        public double TotalAdvance
        {
            get { return this._TotalAdvance; }
            set { this._TotalAdvance = value; }
        }

        public int ReceiptStatus
        {
            get { return this._ReceiptStatus; }
            set { this._ReceiptStatus = value; }
        }

        public string XMLName
        {
            get { return this._XMLName; }
            set { this._XMLName = value; }
        }
    }
}