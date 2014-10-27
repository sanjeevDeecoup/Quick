namespace DTO
{
    public class Booking_Items_Colors
    {
        private int _Booking_ItemColorID;
        private int _BookingItemID;
        private int _ColorID;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int Booking_ItemColorID
        {
            get { return this._Booking_ItemColorID; }
            set { this._Booking_ItemColorID = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int ColorID
        {
            get { return this._ColorID; }
            set { this._ColorID = value; }
        }
    }
}