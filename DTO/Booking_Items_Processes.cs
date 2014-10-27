namespace DTO
{
    public class Booking_Items_Processes
    {
        private int _Booking_ItemProcessID;
        private int _BookingItemID;
        private int _ProcessID;
        private double _ProcessRate;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int Booking_ItemProcessID
        {
            get { return this._Booking_ItemProcessID; }
            set { this._Booking_ItemProcessID = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int ProcessID
        {
            get { return this._ProcessID; }
            set { this._ProcessID = value; }
        }

        public double ProcessRate
        {
            get { return this._ProcessRate; }
            set { this._ProcessRate = value; }
        }
    }
}