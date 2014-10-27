namespace DTO
{
    public class BarCodeSetting
    {
        private int _BarCodePosition;
        private int _Loopi;
        private int _Loopj;

        private bool _OptBooking;
        private bool _OptProcess;
        private bool _OptExtraProcess;
        private bool _OptExtraProcessSecond;
        private bool _OptSubtotal;
        private bool _OptRemark;
        private bool _OptColour;
        private bool _OptPrint;
        private bool _OptItem;
        private bool _OptDueDate;
        private bool _OptTime;
        private bool _OptCustomer;
        private bool _OptAddress;
        private bool _OptBarCode;
        private bool _OptBookingDate;

        private string _StrPreview;
        private string _StrPreviewBarcode;
        private string _BarcodeWidth;
        private string _BarcodeHeight;
        private string _OldBarcodeWidth;
        private string _OldBarcodeHeight;
        private string _BookingFont;
        private string _BookingSize;
        private string _BookingAlign;
        private string _BookingBold;
        private string _BookingItalic;
        private string _BookingUnderline;
        private string _ProcessFont;
        private string _ProcessSize;
        private string _ProcessAlign;
        private string _ProcessBold;
        private string _ProcessItalic;
        private string _ProcessUnderline;
        private string _RemarkFont;
        private string _RemarkSize;
        private string _RemarkAlign;
        private string _RemarkBold;
        private string _RemarkItalic;
        private string _RemarkUnderline;
        private string _BarcodeAlign;
        private string _DueDateFont;
        private string _DueDateSize;
        private string _DueDateAlign;
        private string _DueDateBold;
        private string _DueDateItalic;
        private string _DueDateUnderline;
        private string _ItemFont;
        private string _ItemSize;
        private string _ItemAlign;
        private string _ItemBold;
        private string _ItemItalic;
        private string _ItemUnderline;
        private string _CustomerFont;
        private string _CustomerSize;
        private string _CustomerAlign;
        private string _CustomerBold;
        private string _CustomerItalic;
        private string _CustomerUnderline;
        private string _AddressFont;
        private string _AddressSize;
        private string _AddressAlign;
        private string _AddressBold;
        private string _AddressItalic;
        private string _AddressUnderline;

        private string _BookingPosition;
        private string _BarPosition;
        private string _CustomerPosition;
        private string _AddressPosition;
        private string _ProcessPosition;
        private string _ItemTotalPosition;
        private string _RemarkPosition;
        private string _ColourPosition;
        private string _DueDatePosition;
        private string _TimePosition;
        private string _ItemPosition;
        private string[] _StrArray;
        private string _CustName;
        private string _StrMainBarCode;
        private string _BookingNo;
        private string _Address;
        private string _BarCode;
        private string _Time;
        private string _Item;
        private string _Date;
        private string _Remark;
        private string _ItemName;
        private string _Customer;
        private string _Process;
        private string _ExtProcess;
        private string _ExtProcessSecond;
        private string _STotal;
        private string _DueDate;
        private string _CurrentTime;
        private string _StrPreviewC;
        private double _Barcodeheight;
        private double _DivHeight;

        private string _OptBooking1;
        private string _OptProcess1;
        private string _OptExtraProcess1;
        private string _OptExtraProcessSecond1;
        private string _OptSubtotal1;
        private string _OptRemark1;
        private string _OptColour1;

        private string _OptItem1;
        private string _OptDueDate1;
        private string _OptTime1;
        private string _OptCustomer1;
        private string _OptAddress1;

        private bool _OptLogo;
        private bool _OptShopOption;
        private string _OptLogo1;
        private string _OptShowOption1;
        private string _BarCodeFontName;
        private string _BarCodeFontSize;
        private string _LogoAlign;
        private string _ShopName;
        private string _PrinterName;
        private string _BranchId;
        private string _OptBarCode1;
        private string _LogoPosition;
        private double _LineHeight;
        private string _LogoUrl;
        private string _ShopDiv;
        private string _ShopAlign;
        private string _ShopSize;
        private string _bookingnoposition;
        private string _customerposition;
        private string _processposition;
        private string _remarkposition;
        private string _barcodeposition;
        private string _itemposition;
        private string _itemposition1;
        private string _addressposition;
        private string _msg;
        private bool _IsDueIncrease;
        private string _LogoSize;
        private int _daysValue;
        private string _bookingdate;
        private string _bdatefont;
        private string _bdatesize;
        private string _bdatealign;
        private string _bdatebold;
        private string _bdateitalic;
        private string _bdateunderline;
        private string _bdateposition;
        private int _pagebreak;
        private bool _wet;
        private string _child1;
        private string _child2;

        public string child1
        {
            get { return this._child1; }
            set { this._child1 = value; }
        }

        public string child2
        {
            get { return this._child2; }
            set { this._child2 = value; }
        }

        public bool wet
        {
            get { return this._wet; }
            set { this._wet = value; }
        }

        public int pagebreak
        {
            get { return this._pagebreak; }
            set { this._pagebreak = value; }
        }

        public bool OptBookingDate
        {
            get { return this._OptBookingDate; }
            set { this._OptBookingDate = value; }
        }

        public string bdatefont
        {
            get { return this._bdatefont; }
            set { this._bdatefont = value; }
        }

        public string bdatesize
        {
            get { return this._bdatesize; }
            set { this._bdatesize = value; }
        }

        public string bdatealign
        {
            get { return this._bdatealign; }
            set { this._bdatealign = value; }
        }

        public string bdatebold
        {
            get { return this._bdatebold; }
            set { this._bdatebold = value; }
        }

        public string bdateitalic
        {
            get { return this._bdateitalic; }
            set { this._bdateitalic = value; }
        }

        public string bdateunderline
        {
            get { return this._bdateunderline; }
            set { this._bdateunderline = value; }
        }

        public string bdateposition
        {
            get { return this._bdateposition; }
            set { this._bdateposition = value; }
        }

        public string bookingdate
        {
            get { return this._bookingdate; }
            set { this._bookingdate = value; }
        }

        public int daysValue
        {
            get { return this._daysValue; }
            set { this._daysValue = value; }
        }

        public string LogoSize
        {
            get { return this._LogoSize; }
            set { this._LogoSize = value; }
        }

        public bool IsDueIncrease
        {
            get { return this._IsDueIncrease; }
            set { this._IsDueIncrease = value; }
        }

        public string msg
        {
            get { return this._msg; }
            set { this._msg = value; }
        }

        public string addressposition
        {
            get { return this._addressposition; }
            set { this._addressposition = value; }
        }

        public string itemposition1
        {
            get { return this._itemposition1; }
            set { this._itemposition1 = value; }
        }

        public string itemposition
        {
            get { return this._itemposition; }
            set { this._itemposition = value; }
        }

        public string barcodeposition
        {
            get { return this._barcodeposition; }
            set { this._barcodeposition = value; }
        }

        public string remarkposition
        {
            get { return this._remarkposition; }
            set { this._remarkposition = value; }
        }

        public string processposition
        {
            get { return this._processposition; }
            set { this._processposition = value; }
        }

        public string customerposition
        {
            get { return this._customerposition; }
            set { this._customerposition = value; }
        }

        public string bookingnoposition
        {
            get { return this._bookingnoposition; }
            set { this._bookingnoposition = value; }
        }

        public string ShopSize
        {
            get { return this._ShopSize; }
            set { this._ShopSize = value; }
        }

        public string ShopAlign
        {
            get { return this._ShopAlign; }
            set { this._ShopAlign = value; }
        }

        public string ShopDiv
        {
            get { return this._ShopDiv; }
            set { this._ShopDiv = value; }
        }

        public string LogoUrl
        {
            get { return this._LogoUrl; }
            set { this._LogoUrl = value; }
        }

        public double LineHeight
        {
            get { return this._LineHeight; }
            set { this._LineHeight = value; }
        }

        public string LogoPosition
        {
            get { return this._LogoPosition; }
            set { this._LogoPosition = value; }
        }

        public string BarCodeFontSize
        {
            get { return this._BarCodeFontSize; }
            set { this._BarCodeFontSize = value; }
        }

        public string BarCodeFontName
        {
            get { return this._BarCodeFontName; }
            set { this._BarCodeFontName = value; }
        }

        public bool OptBarCode
        {
            get { return this._OptBarCode; }
            set { this._OptBarCode = value; }
        }

        public string OptBarCode1
        {
            get { return this._OptBarCode1; }
            set { this._OptBarCode1 = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public string LogoAlign
        {
            get { return this._LogoAlign; }
            set { this._LogoAlign = value; }
        }

        public string ShopName
        {
            get { return this._ShopName; }
            set { this._ShopName = value; }
        }

        public string PrinterName
        {
            get { return this._PrinterName; }
            set { this._PrinterName = value; }
        }

        public bool OptShopOption
        {
            get { return this._OptShopOption; }
            set { this._OptShopOption = value; }
        }

        public bool OptLogo
        {
            get { return this._OptLogo; }
            set { this._OptLogo = value; }
        }

        public string OptLogo1
        {
            get { return this._OptLogo1; }
            set { this._OptLogo1 = value; }
        }

        public string OptShowOption1
        {
            get { return this._OptShowOption1; }
            set { this._OptShowOption1 = value; }
        }

        public string OptAddress1
        {
            get { return this._OptAddress1; }
            set { this._OptAddress1 = value; }
        }

        public string OptCustomer1
        {
            get { return this._OptCustomer1; }
            set { this._OptCustomer1 = value; }
        }

        public string OptTime1
        {
            get { return this._OptTime1; }
            set { this._OptTime1 = value; }
        }

        public string OptDueDate1
        {
            get { return this._OptDueDate1; }
            set { this._OptDueDate1 = value; }
        }

        public string OptItem1
        {
            get { return this._OptItem1; }
            set { this._OptItem1 = value; }
        }

        public string OptColour1
        {
            get { return this._OptColour1; }
            set { this._OptColour1 = value; }
        }

        public string OptRemark1
        {
            get { return this._OptRemark1; }
            set { this._OptRemark1 = value; }
        }

        public string OptSubtotal1
        {
            get { return this._OptSubtotal1; }
            set { this._OptSubtotal1 = value; }
        }

        public string OptExtraProcessSecond1
        {
            get { return this._OptExtraProcessSecond1; }
            set { this._OptExtraProcessSecond1 = value; }
        }

        public string OptExtraProcess1
        {
            get { return this._OptExtraProcess1; }
            set { this._OptExtraProcess1 = value; }
        }

        public string OptProcess1
        {
            get { return this._OptProcess1; }
            set { this._OptProcess1 = value; }
        }

        public string OptBooking1
        {
            get { return this._OptBooking1; }
            set { this._OptBooking1 = value; }
        }

        public string StrPreviewC
        {
            get { return this._StrPreviewC; }
            set { this._StrPreviewC = value; }
        }

        public string CurrentTime
        {
            get { return this._CurrentTime; }
            set { this._CurrentTime = value; }
        }

        public string DueDate
        {
            get { return this._DueDate; }
            set { this._DueDate = value; }
        }

        public string STotal
        {
            get { return this._STotal; }
            set { this._STotal = value; }
        }

        public string ExtProcessSecond
        {
            get { return this._ExtProcessSecond; }
            set { this._ExtProcessSecond = value; }
        }

        public string ExtProcess
        {
            get { return this._ExtProcess; }
            set { this._ExtProcess = value; }
        }

        public string Process
        {
            get { return this._Process; }
            set { this._Process = value; }
        }

        public string Customer
        {
            get { return this._Customer; }
            set { this._Customer = value; }
        }

        public int Loopj
        {
            get { return this._Loopj; }
            set { this._Loopj = value; }
        }

        public string CustName
        {
            get { return this._CustName; }
            set { this._CustName = value; }
        }

        public string StrMainBarCode
        {
            get { return this._StrMainBarCode; }
            set { this._StrMainBarCode = value; }
        }

        public string BookingNo
        {
            get { return this._BookingNo; }
            set { this._BookingNo = value; }
        }

        public string Address
        {
            get { return this._Address; }
            set { this._Address = value; }
        }

        public string BarCode
        {
            get { return this._BarCode; }
            set { this._BarCode = value; }
        }

        public string Time
        {
            get { return this._Time; }
            set { this._Time = value; }
        }

        public string Item
        {
            get { return this._Item; }
            set { this._Item = value; }
        }

        public string Date
        {
            get { return this._Date; }
            set { this._Date = value; }
        }

        public string Remark
        {
            get { return this._Remark; }
            set { this._Remark = value; }
        }

        public string ItemName
        {
            get { return this._ItemName; }
            set { this._ItemName = value; }
        }

        public string[] StrArray
        {
            get { return this._StrArray; }
            set { this._StrArray = value; }
        }

        public double DivHeight
        {
            get { return this._DivHeight; }
            set { this._DivHeight = value; }
        }

        public double Barcodeheight
        {
            get { return this._Barcodeheight; }
            set { this._Barcodeheight = value; }
        }

        public string BookingPosition
        {
            get { return this._BookingPosition; }
            set { this._BookingPosition = value; }
        }

        public string BarPosition
        {
            get { return this._BarPosition; }
            set { this._BarPosition = value; }
        }

        public string CustomerPosition
        {
            get { return this._CustomerPosition; }
            set { this._CustomerPosition = value; }
        }

        public string AddressPosition
        {
            get { return this._AddressPosition; }
            set { this._AddressPosition = value; }
        }

        public string ProcessPosition
        {
            get { return this._ProcessPosition; }
            set { this._ProcessPosition = value; }
        }

        public string ItemTotalPosition
        {
            get { return this._ItemTotalPosition; }
            set { this._ItemTotalPosition = value; }
        }

        public string RemarkPosition
        {
            get { return this._RemarkPosition; }
            set { this._RemarkPosition = value; }
        }

        public string ColourPosition
        {
            get { return this._ColourPosition; }
            set { this._ColourPosition = value; }
        }

        public string DueDatePosition
        {
            get { return this._DueDatePosition; }
            set { this._DueDatePosition = value; }
        }

        public string TimePosition
        {
            get { return this._TimePosition; }
            set { this._TimePosition = value; }
        }

        public string ItemPosition
        {
            get { return this._ItemPosition; }
            set { this._ItemPosition = value; }
        }

        public string AddressFont
        {
            get { return this._AddressFont; }
            set { this._AddressFont = value; }
        }

        public string AddressSize
        {
            get { return this._AddressSize; }
            set { this._AddressSize = value; }
        }

        public string AddressAlign
        {
            get { return this._AddressAlign; }
            set { this._AddressAlign = value; }
        }

        public string AddressBold
        {
            get { return this._AddressBold; }
            set { this._AddressBold = value; }
        }

        public string AddressItalic
        {
            get { return this._AddressItalic; }
            set { this._AddressItalic = value; }
        }

        public string AddressUnderline
        {
            get { return this._AddressUnderline; }
            set { this._AddressUnderline = value; }
        }

        public string CustomerFont
        {
            get { return this._CustomerFont; }
            set { this._CustomerFont = value; }
        }

        public string CustomerSize
        {
            get { return this._CustomerSize; }
            set { this._CustomerSize = value; }
        }

        public string CustomerAlign
        {
            get { return this._CustomerAlign; }
            set { this._CustomerAlign = value; }
        }

        public string CustomerBold
        {
            get { return this._CustomerBold; }
            set { this._CustomerBold = value; }
        }

        public string CustomerItalic
        {
            get { return this._CustomerItalic; }
            set { this._CustomerItalic = value; }
        }

        public string CustomerUnderline
        {
            get { return this._CustomerUnderline; }
            set { this._CustomerUnderline = value; }
        }

        public string DueDateFont
        {
            get { return this._DueDateFont; }
            set { this._DueDateFont = value; }
        }

        public string DueDateSize
        {
            get { return this._DueDateSize; }
            set { this._DueDateSize = value; }
        }

        public string DueDateAlign
        {
            get { return this._DueDateAlign; }
            set { this._DueDateAlign = value; }
        }

        public string DueDateBold
        {
            get { return this._DueDateBold; }
            set { this._DueDateBold = value; }
        }

        public string DueDateItalic
        {
            get { return this._DueDateItalic; }
            set { this._DueDateItalic = value; }
        }

        public string DueDateUnderline
        {
            get { return this._DueDateUnderline; }
            set { this._DueDateUnderline = value; }
        }

        public string ItemFont
        {
            get { return this._ItemFont; }
            set { this._ItemFont = value; }
        }

        public string ItemSize
        {
            get { return this._ItemSize; }
            set { this._ItemSize = value; }
        }

        public string ItemAlign
        {
            get { return this._ItemAlign; }
            set { this._ItemAlign = value; }
        }

        public string ItemBold
        {
            get { return this._ItemBold; }
            set { this._ItemBold = value; }
        }

        public string ItemItalic
        {
            get { return this._ItemItalic; }
            set { this._ItemItalic = value; }
        }

        public string ItemUnderline
        {
            get { return this._ItemUnderline; }
            set { this._ItemUnderline = value; }
        }

        public string BarcodeAlign
        {
            get { return this._BarcodeAlign; }
            set { this._BarcodeAlign = value; }
        }

        public string RemarkFont
        {
            get { return this._RemarkFont; }
            set { this._RemarkFont = value; }
        }

        public string RemarkSize
        {
            get { return this._RemarkSize; }
            set { this._RemarkSize = value; }
        }

        public string RemarkAlign
        {
            get { return this._RemarkAlign; }
            set { this._RemarkAlign = value; }
        }

        public string RemarkBold
        {
            get { return this._RemarkBold; }
            set { this._RemarkBold = value; }
        }

        public string RemarkItalic
        {
            get { return this._RemarkItalic; }
            set { this._RemarkItalic = value; }
        }

        public string RemarkUnderline
        {
            get { return this._RemarkUnderline; }
            set { this._RemarkUnderline = value; }
        }

        public string ProcessFont
        {
            get { return this._ProcessFont; }
            set { this._ProcessFont = value; }
        }

        public string ProcessSize
        {
            get { return this._ProcessSize; }
            set { this._ProcessSize = value; }
        }

        public string ProcessAlign
        {
            get { return this._ProcessAlign; }
            set { this._ProcessAlign = value; }
        }

        public string ProcessBold
        {
            get { return this._ProcessBold; }
            set { this._ProcessBold = value; }
        }

        public string ProcessItalic
        {
            get { return this._ProcessItalic; }
            set { this._ProcessItalic = value; }
        }

        public string ProcessUnderline
        {
            get { return this._ProcessUnderline; }
            set { this._ProcessUnderline = value; }
        }

        public string BookingFont
        {
            get { return this._BookingFont; }
            set { this._BookingFont = value; }
        }

        public string BookingSize
        {
            get { return this._BookingSize; }
            set { this._BookingSize = value; }
        }

        public string BookingAlign
        {
            get { return this._BookingAlign; }
            set { this._BookingAlign = value; }
        }

        public string BookingBold
        {
            get { return this._BookingBold; }
            set { this._BookingBold = value; }
        }

        public string BookingItalic
        {
            get { return this._BookingItalic; }
            set { this._BookingItalic = value; }
        }

        public string BookingUnderline
        {
            get { return this._BookingUnderline; }
            set { this._BookingUnderline = value; }
        }

        public string OldBarcodeHeight
        {
            get { return this._OldBarcodeHeight; }
            set { this._OldBarcodeHeight = value; }
        }

        public string OldBarcodeWidth
        {
            get { return this._OldBarcodeWidth; }
            set { this._OldBarcodeWidth = value; }
        }

        public string BarcodeHeight
        {
            get { return this._BarcodeHeight; }
            set { this._BarcodeHeight = value; }
        }

        public string BarcodeWidth
        {
            get { return this._BarcodeWidth; }
            set { this._BarcodeWidth = value; }
        }

        public string StrPreviewBarCode
        {
            get { return this._StrPreviewBarcode; }
            set { this._StrPreviewBarcode = value; }
        }

        public string StrPreview
        {
            get { return this._StrPreview; }
            set { this._StrPreview = value; }
        }

        public bool OptAddress
        {
            get { return this._OptAddress; }
            set { this._OptAddress = value; }
        }

        public bool OptCustomer
        {
            get { return this._OptCustomer; }
            set { this._OptCustomer = value; }
        }

        public bool OptTime
        {
            get { return this._OptTime; }
            set { this._OptTime = value; }
        }

        public bool OptDueDate
        {
            get { return this._OptDueDate; }
            set { this._OptDueDate = value; }
        }

        public bool OptItem
        {
            get { return this._OptItem; }
            set { this._OptItem = value; }
        }

        public bool OptPrint
        {
            get { return this._OptPrint; }
            set { this._OptPrint = value; }
        }

        public bool OptColour
        {
            get { return this._OptColour; }
            set { this._OptColour = value; }
        }

        public bool OptRemark
        {
            get { return this._OptRemark; }
            set { this._OptRemark = value; }
        }

        public bool OptSubtotal
        {
            get { return this._OptSubtotal; }
            set { this._OptSubtotal = value; }
        }

        public bool OptExtraProcessSecond
        {
            get { return this._OptExtraProcessSecond; }
            set { this._OptExtraProcessSecond = value; }
        }

        public bool OptExtraProcess
        {
            get { return this._OptExtraProcess; }
            set { this._OptExtraProcess = value; }
        }

        public bool OptProcess
        {
            get { return this._OptProcess; }
            set { this._OptProcess = value; }
        }

        public bool OptBooking
        {
            get { return this._OptBooking; }
            set { this._OptBooking = value; }
        }

        public int Loopi
        {
            get { return this._Loopi; }
            set { this._Loopi = value; }
        }

        public int BarCodePosition
        {
            get { return this._BarCodePosition; }
            set { this._BarCodePosition = value; }
        }
    }
}