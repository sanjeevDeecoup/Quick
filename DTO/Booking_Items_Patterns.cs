namespace DTO
{
    public class Booking_Items_Patterns
    {
        private int _Booking_ItemPatternID;
        private int _BookingItemID;
        private int _PatternID;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int Booking_ItemPatternID
        {
            get { return this._Booking_ItemPatternID; }
            set { this._Booking_ItemPatternID = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int PatternID
        {
            get { return this._PatternID; }
            set { this._PatternID = value; }
        }
    }
}