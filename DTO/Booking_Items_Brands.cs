namespace DTO
{
    public class Booking_Items_Brands
    {
        private int _Booking_ItemBrandID;
        private int _BookingItemID;
        private int _BrandID;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int Booking_ItemBrandID
        {
            get { return this._Booking_ItemBrandID; }
            set { this._Booking_ItemBrandID = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int BrandID
        {
            get { return this._BrandID; }
            set { this._BrandID = value; }
        }
    }
}