namespace BAL
{
    public class BALFactory
    {
        private static BALFactory _instance;

        public static BALFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BALFactory();
                return _instance;
            }
        }

        public BAL_New_Bookings BAL_New_Bookings
        {
            get { return new BAL_New_Bookings(); }
        }

        public BL_For_ImportFile BL_For_ImportFile
        {
            get { return new BL_For_ImportFile(); }
        }

        public BAL_DateAndTime BAL_DateAndTime
        {
            get { return new BAL_DateAndTime(); }
        }

        public Bal_Report Bal_Report
        {
            get { return new Bal_Report(); }
        }

        public Bal_Processmaster Bal_Processmaster
        {
            get { return new Bal_Processmaster(); }
        }

        public BAL_Remark BAL_Remark
        {
            get { return new BAL_Remark(); }
        }

        public BAL_Shift BAL_Shift
        {
            get { return new BAL_Shift(); }
        }

        public BL_CustomerMaster BL_CustomerMaster
        {
            get { return new BL_CustomerMaster(); }
        }

        public BL_PriceList BL_PriceList
        {
            get { return new BL_PriceList(); }
        }

        public BL_Branch BL_Branch
        {
            get { return new BL_Branch(); }
        }

        public BL_UserMaster BL_UserMaster
        {
            get { return new BL_UserMaster(); }
        }

        public BAL_ChallanIn BAL_ChallanIn
        {
            get { return new BAL_ChallanIn(); }
        }

        public BAL_Employee BAL_Employee
        {
            get { return new BAL_Employee(); }
        }

        public BAL_Priority BAL_Priority
        {
            get { return new BAL_Priority(); }
        }

        public BAL_Item BAL_Item
        {
            get { return new BAL_Item(); }
        }

        public BAL_Variation BAL_Variation
        {
            get { return new BAL_Variation(); }
        }

        public BL_BrandMaster BL_BrandMaster
        {
            get { return new BL_BrandMaster(); }
        }

        public BAL_Comment BAL_Comment
        {
            get { return new BAL_Comment(); }
        }

        public BL_ColorMaster BL_ColorMaster
        {
            get { return new BL_ColorMaster(); }
        }

        public BAL_Patterns BAL_Patterns
        {
            get { return new BAL_Patterns(); }
        }

        public BL_CategoryMaster BL_CategoryMaster
        {
            get { return new BL_CategoryMaster(); }
        }

        public BAL_BranchChallanHeader BAL_BranchChallanHeader
        {
            get { return new BAL_BranchChallanHeader(); }
        }

        public BAL_FactoryChallanHeader BAL_FactoryChallanHeader
        {
            get { return new BAL_FactoryChallanHeader(); }
        }

        public BAL_FactoryChallanDetail BAL_FactoryChallanDetail
        {
            get { return new BAL_FactoryChallanDetail(); }
        }

        public BAL_BranchChallanDetail BAL_BranchChallanDetail
        {
            get { return new BAL_BranchChallanDetail(); }
        }

        public BAL_ItemCategory BAL_ItemCategory
        {
            get { return new BAL_ItemCategory(); }
        }

        public BAL_ItemVariation BAL_ItemVariation
        {
            get { return new BAL_ItemVariation(); }
        }

        public BAL_SubItems BAL_SubItems
        {
            get { return new BAL_SubItems(); }
        }

        public BAL_ItemTracking BAL_ItemTracking
        {
            get { return new BAL_ItemTracking(); }
        }

        public BAL_Booking BAL_Booking
        {
            get { return new BAL_Booking(); }
        }

        public BAL_BookingItems BAL_BookingItems
        {
            get { return new BAL_BookingItems(); }
        }

        public BAL_BookingItemComments BAL_BookingItemComments
        {
            get { return new BAL_BookingItemComments(); }
        }

        public BAL_BookingItemBrands BAL_BookingItemBrands
        {
            get { return new BAL_BookingItemBrands(); }
        }

        public BAL_Booking_ItemColors BAL_Booking_ItemColors
        {
            get { return new BAL_Booking_ItemColors(); }
        }

        public BAL_Booking_Items_Processes BAL_Booking_Items_Processes
        {
            get { return new BAL_Booking_Items_Processes(); }
        }

        public BAL_BookingItem_SubItems BAL_BookingItem_SubItems
        {
            get { return new BAL_BookingItem_SubItems(); }
        }

        public SchemeBAO SchemeBAO
        {
            get { return new SchemeBAO(); }
        }

        public ProcessBAO ProcessBAO
        {
            get { return new ProcessBAO(); }
        }

        public ItemBAO ItemBAO
        {
            get { return new ItemBAO(); }
        }

        public CustomerBAO CustomerBAO
        {
            get { return new CustomerBAO(); }
        }

        public BookingBAO BokingBAO
        {
            get { return new BookingBAO(); }
        }

        public BAL_NewBookingScreen BAL_NewBookingScreen
        {
            get { return new BAL_NewBookingScreen(); }
        }

        public BAL_NewCustomer BAL_NewCustomer
        {
            get { return new BAL_NewCustomer(); }
        }

        public BAL_Barcodeconfig BAL_Barcodeconfig
        {
            get { return new BAL_Barcodeconfig(); }
        }

        public BAL_BarcodeLable BAL_BarcodeLable
        {
            get { return new BAL_BarcodeLable(); }
        }

        public BAL_NewPriceLists BAL_NewPriceLists
        {
            get { return new BAL_NewPriceLists(); }
        }

        public BAL_Sticker BAL_Sticker
        {
            get { return new BAL_Sticker(); }
        }

        public BL_HolidayMaster BL_HolidayMaster
        {
            get { return new BL_HolidayMaster(); }
        }

        public BAL_sms BAL_sms
        {
            get { return new BAL_sms(); }
        }

        public BAL_Barcodesetting BAL_Barcodesetting
        {
            get { return new BAL_Barcodesetting(); }
        }

        public BAL_RemoveReason BAL_RemoveReason
        {
            get { return new BAL_RemoveReason(); }
        }

        public BAL_Color BAL_Color
        {
            get { return new BAL_Color(); }
        }

        public BAL_Area BAL_Area
        {
            get
            {
                return new BAL_Area();
            }
        }

        public BAL_City BAL_City
        {
            get { return new BAL_City(); }
        }

        public BAL_Profession BAL_Profession
        {
            get { return new BAL_Profession(); }
        }

        public BL_PackageMaster BL_PackageMaster
        {
            get { return new BL_PackageMaster(); }
        }

        public BL_WorkShopAllFunction BL_WorkShopAllFunction
        {
            get { return new BL_WorkShopAllFunction(); }
        }
        
    }
}