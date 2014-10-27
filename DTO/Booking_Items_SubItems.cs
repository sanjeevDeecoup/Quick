namespace DTO
{
    public class Booking_Items_SubItems
    {
        private int _Booking_ItemSubItemID;
        private int _SubItemID;
        private int _BookingItemID;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int Booking_ItemSubItemID
        {
            get { return this._Booking_ItemSubItemID; }
            set { this._Booking_ItemSubItemID = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int SubItemID
        {
            get { return this._SubItemID; }
            set { this._SubItemID = value; }
        }
    }
}