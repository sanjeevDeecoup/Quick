namespace DAL
{
    public class DALFactory
    {
        private static DALFactory _instance;

        public static DALFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DALFactory();
                return _instance;
            }
        }

        public DAL_New_Bookings DAL_New_Bookings
        {
            get { return new DAL_New_Bookings(); }
        }

        public DAL_For_ImportFile DAL_For_ImportFile
        {
            get { return new DAL_For_ImportFile(); }
        }

        public DAL_DateAndTime DAL_DateAndTime
        {
            get { return new DAL_DateAndTime(); }
        }

        public DAL_Report DAL_Report
        {
            get { return new DAL_Report(); }
        }

        public Dal_ProcessMaster Dal_ProcessMaster
        {
            get { return new Dal_ProcessMaster(); }
        }

        public DAL_Remarks DAL_Remarks
        {
            get { return new DAL_Remarks(); }
        }

        public DAL_Shift DAL_Shift
        {
            get { return new DAL_Shift(); }
        }

        public DAL_Priority DAL_Priority
        {
            get { return new DAL_Priority(); }
        }

        public DAL_Employee DAL_Employee
        {
            get { return new DAL_Employee(); }
        }

        public DAL_Customer DAL_Customer
        {
            get { return new DAL_Customer(); }
        }

        public DAL_PriceList DAL_PriceList
        {
            get { return new DAL_PriceList(); }
        }

        public DAL_Branch DAL_Branch
        {
            get { return new DAL_Branch(); }
        }

        public DAL_UserMaster DAL_UserMaster
        {
            get { return new DAL_UserMaster(); }
        }

        public DAL_NewChallan DAL_NewChallan
        {
            get { return new DAL_NewChallan(); }
        }

        public DAL_Item DAL_Item
        {
            get { return new DAL_Item(); }
        }

        public DAL_Variation DAL_Variation
        {
            get { return new DAL_Variation(); }
        }

        public Dal_BrandMaster Dal_BrandMaster
        {
            get { return new Dal_BrandMaster(); }
        }

        public DAL_Comment DAL_Comment
        {
            get { return new DAL_Comment(); }
        }

        public DAL_ColorMaster DAL_ColorMaster
        {
            get { return new DAL_ColorMaster(); }
        }

        public DAL_Patterns DAL_Patterns
        {
            get { return new DAL_Patterns(); }
        }

        public DAL_CategoryMaster DAL_CategoryMaster
        {
            get { return new DAL_CategoryMaster(); }
        }

        public DAL_BranchChallanHeader DAL_BranchChallanHeader
        {
            get { return new DAL_BranchChallanHeader(); }
        }

        public DAL_FactoryChallanHeader DAL_FactoryChallanHeader
        {
            get { return new DAL_FactoryChallanHeader(); }
        }

        public DAL_FactoryChallanDetail DAL_FactoryChallanDetail
        {
            get { return new DAL_FactoryChallanDetail(); }
        }

        public DAL_BranchChallanDetail DAL_BranchChallanDetail
        {
            get { return new DAL_BranchChallanDetail(); }
        }

        public DAL_ItemCategories DAL_ItemCategories
        {
            get { return new DAL_ItemCategories(); }
        }

        public DAL_ItemVariation DAL_ItemVariation
        {
            get { return new DAL_ItemVariation(); }
        }

        public DAL_SubItem DAL_SubItem
        {
            get { return new DAL_SubItem(); }
        }

        public DAL_ItemTracking DAL_ItemTracking
        {
            get { return new DAL_ItemTracking(); }
        }

        public DAL_Booking DAL_Booking
        {
            get { return new DAL_Booking(); }
        }

        public DAL_Booking_Items DAL_Booking_Items
        {
            get { return new DAL_Booking_Items(); }
        }

        public DAL_BookingItemComment DAL_BookingItemComment
        {
            get { return new DAL_BookingItemComment(); }
        }

        public DAL_Booking_ItemBrands DAL_Booking_ItemBrands
        {
            get { return new DAL_Booking_ItemBrands(); }
        }

        public DAL_Booking_Items_Processes DAL_Booking_Items_Processes
        {
            get { return new DAL_Booking_Items_Processes(); }
        }

        public DAL_BookingItem_Colors DAL_BookingItem_Colors
        {
            get { return new DAL_BookingItem_Colors(); }
        }

        public DAL_BookingItem_SubItems DAL_BookingItem_SubItems
        {
            get { return new DAL_BookingItem_SubItems(); }
        }

        public SchemeDAO SchemeDAO
        {
            get { return new SchemeDAO(); }
        }

        public ProcessDAO ProcessDAO
        {
            get { return new ProcessDAO(); }
        }

        public ItemDAO ItemDAO
        {
            get { return new ItemDAO(); }
        }

        public CustomerDAO CustomerDAO
        {
            get { return new CustomerDAO(); }
        }

        public BookingDAO BookingDAO
        {
            get { return new BookingDAO(); }
        }

        public DAL_Barcodeconfig DAL_Barcodeconfig
        {
            get { return new DAL_Barcodeconfig(); }
        }

        public DAL_Booking_Patterns DAL_Booking_Patterns
        {
            get { return new DAL_Booking_Patterns(); }
        }

        public DAL_BarcodeLable DAL_BarcodeLable
        {
            get { return new DAL_BarcodeLable(); }
        }

        public DAL_NewPriceLists DAL_NewPriceLists
        {
            get { return new DAL_NewPriceLists(); }
        }

        public DAL_Sticker DAL_Sticker
        {
            get { return new DAL_Sticker(); }
        }

        public DAL_HolidayMaster DAL_HolidayMaster
        {
            get { return new DAL_HolidayMaster(); }
        }

        public DAL_sms DAL_sms
        {
            get { return new DAL_sms(); }
        }

        public DAL_BarCodeSetting Dal_BarCodeSetting
        {
            get { return new DAL_BarCodeSetting(); }
        }

        public DAL_RemoveReason DAL_RemoveReason
        {
            get { return new DAL_RemoveReason(); }
        }

        public DAL_Color DAL_Color
        {
            get { return new DAL_Color(); }
        }

        public DAL_Area DAL_Area
        {
            get { return new DAL_Area(); }
        }

        public DAL_City DAL_City
        {
            get { return new DAL_City(); }
        }

        public DAL_Profession DAL_Profession
        {
            get { return new DAL_Profession(); }
        }

        public DAL_PackageMaster DAL_PackageMaster
        {
            get { return new DAL_PackageMaster(); }
        }

        public Dal_WorkShopAllFunction Dal_WorkShopAllFunction
        {
            get { return new Dal_WorkShopAllFunction(); }
        }

       
    }
}