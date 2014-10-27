namespace DTO
{
    public class NewBooking
    {
        private string _Priority;
        private string _AddPriority;
        private string _CustTitle;
        private string _CustName;
        private string _CustCode;
        private string _CustAddress;
        private string _CustMobile;
        private string _CustRemarks;
        private string _CustAreaAndLocation;
        private string _ItemName;
        private string _SubItems;
        private string _ItemCode;
        private string _SubItem1;
        private string _SubItem2;
        private string _SubItem3;
        private string _BookingAcceptedByUserId;
        private string _BookingByCustomer;
        private string _IsUrgent;
        private string _BookingDate;
        private string _BookingDeliveryDate;
        private string _BookingDeliveryTime;
        private string _TotalCost;
        private string _Discount;
        private string _NetAmount;
        private string _BookingStatus;
        private string _BookingCancelDate;
        private string _BookingCancelReason;
        private string _BookingRemarks;
        private string _ItemTotalQuantity;
        private string _VendorOrderStatus;
        private string _HomeDelivery;
        private string _CheckedByEmployee;
        private string _BarCode;
        private string _BookingTime;
        private string _Format;
        private string _ReceiptDeliverd;
        private string _BOOKINGNUMBER;
        private string _ISN;
        private string _ItemProcessType;
        private string _ItemQuantityAndRate;
        private string _ItemExtraProcessType1;
        private string _ItemExtraProcessRate1;
        private string _ItemExtraProcessType2;
        private string _ItemExtraProcessRate2;
        private string _ItemSubTotal;
        private string _ItemStatus;
        private string _ItemRemark;
        private string _DeliveredQty;
        private string _ItemColor;
        private string _VendorItemStatus;
        private string _STPAmt;
        private string _STEP1Amt;
        private string _STEP2Amt;
        private string _UserBookingId;

        // ***************** //
        // added fields
        private string _STPAmtEcess;

        private string _STPAmtSHECess;

        // added fields
        private string _STPAmt1Ecess;

        private string _STPAmt1SHECess;

        // added fields
        private string _STPAmt2Ecess;

        private string _STPAmt2SHECess;
        // ***************** //

        // Add new fields for taxable amt and type //
        private double _taxableAmt;

        private string _taxType;

        private string _unitDesc;

        private string _CustomerCode;
        private string _TransDate;
        private string _AdvanceAmt;
        private string _ProcessCode;
        private string _ProcessName;
        private string _SrNo;
        private string _Qty;
        private string _Remarks;
        private string _Color;
        private string _Process;
        private string _Rate;
        private string _ExProc1;
        private string _Rate1;
        private string _ExProc2;
        private string _Rate2;
        private string _Amount;

        private string _ID;
        private string _StatusId;
        private string _SNo;
        private string _RowIndex;
        private string _DrawlStatus;
        private string _DrawlAlloted;
        private string _STTax;

        private string _BDate;
        private string _ADate;
        private string _DiscountAmt;
        private string _DiscountOption;
        private string _BID;
        private string _TodaNext;
        private string _BookingPrefix;

        public string TodaNext
        {
            get { return this._TodaNext; }
            set { this._TodaNext = value; }
        }
        public string BookingPrefix
        {
            get { return this._BookingPrefix; }
            set { this._BookingPrefix = value; }
        }

        public string BID
        {
            get { return this._BID; }
            set { this._BID = value; }
        }

        public string DiscountOption
        {
            get { return this._DiscountOption; }
            set { this._DiscountOption = value; }
        }

        public string DiscountAmt
        {
            get { return this._DiscountAmt; }
            set { this._DiscountAmt = value; }
        }

        public string BDate
        {
            get { return this._BDate; }
            set { this._BDate = value; }
        }

        public string ADate
        {
            get { return this._ADate; }
            set { this._ADate = value; }
        }

        public string STTax
        {
            get { return this._STTax; }
            set { this._STTax = value; }
        }

        public string ID
        {
            get { return this._ID; }
            set { this._ID = value; }
        }

        public string StatusId
        {
            get { return this._StatusId; }
            set { this._StatusId = value; }
        }

        public string SNo
        {
            get { return this._SNo; }
            set { this._SNo = value; }
        }

        public string RowIndex
        {
            get { return this._RowIndex; }
            set { this._RowIndex = value; }
        }

        public string DrawlStatus
        {
            get { return this._DrawlStatus; }
            set { this._DrawlStatus = value; }
        }

        public string DrawlAlloted
        {
            get { return this._DrawlAlloted; }
            set { this._DrawlAlloted = value; }
        }

        public string BookingAcceptedByUserId
        {
            get { return this._BookingAcceptedByUserId; }
            set { this._BookingAcceptedByUserId = value; }
        }

        public string BookingByCustomer
        {
            get { return this._BookingByCustomer; }
            set { this._BookingByCustomer = value; }
        }

        public string IsUrgent
        {
            get { return this._IsUrgent; }
            set { this._IsUrgent = value; }
        }

        public string UserBookingId
        {
            get { return this._UserBookingId; }
            set { this._UserBookingId = value; }
        }

        public string BookingDate
        {
            get { return this._BookingDate; }
            set { this._BookingDate = value; }
        }

        public string BookingDeliveryDate
        {
            get { return this._BookingDeliveryDate; }
            set { this._BookingDeliveryDate = value; }
        }

        public string BookingDeliveryTime
        {
            get { return this._BookingDeliveryTime; }
            set { this._BookingDeliveryTime = value; }
        }

        public string TotalCost
        {
            get { return this._TotalCost; }
            set { this._TotalCost = value; }
        }

        public string Discount
        {
            get { return this._Discount; }
            set { this._Discount = value; }
        }

        public string NetAmount
        {
            get { return this._NetAmount; }
            set { this._NetAmount = value; }
        }

        public string BookingStatus
        {
            get { return this._BookingStatus; }
            set { this._BookingStatus = value; }
        }

        public string BookingCancelDate
        {
            get { return this._BookingCancelDate; }
            set { this._BookingCancelDate = value; }
        }

        public string BookingCancelReason
        {
            get { return this._BookingCancelReason; }
            set { this._BookingCancelReason = value; }
        }

        public string BookingRemarks
        {
            get { return this._BookingRemarks; }
            set { this._BookingRemarks = value; }
        }

        public string ItemTotalQuantity
        {
            get { return this._ItemTotalQuantity; }
            set { this._ItemTotalQuantity = value; }
        }

        public string VendorOrderStatus
        {
            get { return this._VendorOrderStatus; }
            set { this._VendorOrderStatus = value; }
        }

        public string HomeDelivery
        {
            get { return this._HomeDelivery; }
            set { this._HomeDelivery = value; }
        }

        public string CheckedByEmployee
        {
            get { return this._CheckedByEmployee; }
            set { this._CheckedByEmployee = value; }
        }

        public string BarCode
        {
            get { return this._BarCode; }
            set { this._BarCode = value; }
        }

        public string BookingTime
        {
            get { return this._BookingTime; }
            set { this._BookingTime = value; }
        }

        public string Format
        {
            get { return this._Format; }
            set { this._Format = value; }
        }

        public string ReceiptDeliverd
        {
            get { return this._ReceiptDeliverd; }
            set { this._ReceiptDeliverd = value; }
        }

        public string BOOKINGNUMBER
        {
            get { return this._BOOKINGNUMBER; }
            set { this._BOOKINGNUMBER = value; }
        }

        public string ISN
        {
            get { return this._ISN; }
            set { this._ISN = value; }
        }

        public string ItemProcessType
        {
            get { return this._ItemProcessType; }
            set { this._ItemProcessType = value; }
        }

        public string ItemQuantityAndRate
        {
            get { return this._ItemQuantityAndRate; }
            set { this._ItemQuantityAndRate = value; }
        }

        public string ItemExtraProcessType1
        {
            get { return this._ItemExtraProcessType1; }
            set { this._ItemExtraProcessType1 = value; }
        }

        public string ItemExtraProcessRate1
        {
            get { return this._ItemExtraProcessRate1; }
            set { this._ItemExtraProcessRate1 = value; }
        }

        public string ItemExtraProcessType2
        {
            get { return this._ItemExtraProcessType2; }
            set { this._ItemExtraProcessType2 = value; }
        }

        public string ItemExtraProcessRate2
        {
            get { return this._ItemExtraProcessRate2; }
            set { this._ItemExtraProcessRate2 = value; }
        }

        public string ItemSubTotal
        {
            get { return this._ItemSubTotal; }
            set { this._ItemSubTotal = value; }
        }

        public string ItemStatus
        {
            get { return this._ItemStatus; }
            set { this._ItemStatus = value; }
        }

        public string ItemRemark
        {
            get { return this._ItemRemark; }
            set { this._ItemRemark = value; }
        }

        public string DeliveredQty
        {
            get { return this._DeliveredQty; }
            set { this._DeliveredQty = value; }
        }

        public string ItemColor
        {
            get { return this._ItemColor; }
            set { this._ItemColor = value; }
        }

        public string VendorItemStatus
        {
            get { return this._VendorItemStatus; }
            set { this._VendorItemStatus = value; }
        }

        public string STPAmt
        {
            get { return this._STPAmt; }
            set { this._STPAmt = value; }
        }

        public string STEP1Amt
        {
            get { return this._STEP1Amt; }
            set { this._STEP1Amt = value; }
        }

        public string STEP2Amt
        {
            get { return this._STEP2Amt; }
            set { this._STEP2Amt = value; }
        }

        // ************ //
        // added code
        public string STPAmtEcess
        {
            get { return this._STPAmtEcess; }
            set { this._STPAmtEcess = value; }
        }

        public string STPAmtSHECess
        {
            get { return this._STPAmtSHECess; }
            set { this._STPAmtSHECess = value; }
        }

        // added code
        public string STP1AmtEcess
        {
            get { return this._STPAmt1Ecess; }
            set { this._STPAmt1Ecess = value; }
        }

        public string STPAmt1SHECess
        {
            get { return this._STPAmt1SHECess; }
            set { this._STPAmt1SHECess = value; }
        }

        // added code
        public string STP2AmtEcess
        {
            get { return this._STPAmt2Ecess; }
            set { this._STPAmt2Ecess = value; }
        }

        public string STPAmt2SHECess
        {
            get { return this._STPAmt2SHECess; }
            set { this._STPAmt2SHECess = value; }
        }

        // ************ //
        public string CustomerCode
        {
            get { return this._CustomerCode; }
            set { this._CustomerCode = value; }
        }

        public string TransDate
        {
            get { return this._TransDate; }
            set { this._TransDate = value; }
        }

        public string AdvanceAmt
        {
            get { return this._AdvanceAmt; }
            set { this._AdvanceAmt = value; }
        }

        public string Priority
        {
            get { return this._Priority; }
            set { this._Priority = value; }
        }

        public string AddPriority
        {
            get { return this._AddPriority; }
            set { this._AddPriority = value; }
        }

        public string CustCode
        {
            get { return this._CustCode; }
            set { this._CustCode = value; }
        }

        public string CustTitle
        {
            get { return this._CustTitle; }
            set { this._CustTitle = value; }
        }

        public string CustName
        {
            get { return this._CustName; }
            set { this._CustName = value; }
        }

        public string CustAddress
        {
            get { return this._CustAddress; }
            set { this._CustAddress = value; }
        }

        public string CustMobile
        {
            get { return this._CustMobile; }
            set { this._CustMobile = value; }
        }

        public string CustRemarks
        {
            get { return this._CustRemarks; }
            set { this._CustRemarks = value; }
        }

        public string CustAreaAndLocation
        {
            get { return this._CustAreaAndLocation; }
            set { this._CustAreaAndLocation = value; }
        }

        public string ItemName
        {
            get { return this._ItemName; }
            set { this._ItemName = value; }
        }

        public string SubItems
        {
            get { return this._SubItems; }
            set { this._SubItems = value; }
        }

        public string ItemCode
        {
            get { return this._ItemCode; }
            set { this._ItemCode = value; }
        }

        public string SubItem1
        {
            get { return this._SubItem1; }
            set { this._SubItem1 = value; }
        }

        public string SubItem2
        {
            get { return this._SubItem2; }
            set { this._SubItem2 = value; }
        }

        public string SubItem3
        {
            get { return this._SubItem3; }
            set { this._SubItem3 = value; }
        }

        public string ProcessCode
        {
            get { return this._ProcessCode; }
            set { this._ProcessCode = value; }
        }

        public string ProcessName
        {
            get { return this._ProcessName; }
            set { this._ProcessName = value; }
        }

        public string SrNo
        {
            get { return this._SrNo; }
            set { this._SrNo = value; }
        }

        public string Qty
        {
            get { return this._Qty; }
            set { this._Qty = value; }
        }

        public string Remarks
        {
            get { return this._Remarks; }
            set { this._Remarks = value; }
        }

        public string Color
        {
            get { return this._Color; }
            set { this._Color = value; }
        }

        public string Process
        {
            get { return this._Process; }
            set { this._Process = value; }
        }

        public string Rate
        {
            get { return this._Rate; }
            set { this._Rate = value; }
        }

        public string ExProc1
        {
            get { return this._ExProc1; }
            set { this._ExProc1 = value; }
        }

        public string Rate1
        {
            get { return this._Rate1; }
            set { this._Rate1 = value; }
        }

        public string ExProc2
        {
            get { return this._ExProc2; }
            set { this._ExProc2 = value; }
        }

        public string Rate2
        {
            get { return this._Rate2; }
            set { this._Rate2 = value; }
        }

        public string Amount
        {
            get { return this._Amount; }
            set { this._Amount = value; }
        }

        public double TaxableAmt
        {
            get { return this._taxableAmt; }
            set { this._taxableAmt = value; }
        }

        public string TaxType
        {
            get { return this._taxType; }
            set { this._taxType = value; }
        }

        public string UnitDesc
        {
            get { return this._unitDesc; }
            set { this._unitDesc = value; }
        }

        public string WorkshopNote { get; set; }

        public string PaymentMode { get; set; }

        public string AssignId { get; set; }

        public bool isDiscountPackge { get; set; }

        public string RateListId { get; set; }
    }
}