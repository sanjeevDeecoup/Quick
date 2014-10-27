namespace DTO
{
    public class Booking_Items
    {
        private int _BookingItemID;
        private int _BookingID;
        private int _ItemID;
        private int _SubItemCount;
        private int _ProcessCount;
        private int _Sequence;
        private int _CategoryID;
        private int _VariationID;
        private CommonFields _commonFields = new CommonFields();
        private int _Quantity;

        public int Quantity
        {
            get { return this._Quantity; }
            set { this._Quantity = value; }
        }

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int BookingID
        {
            get { return this._BookingID; }
            set { this._BookingID = value; }
        }

        public int ItemID
        {
            get { return this._ItemID; }
            set { this._ItemID = value; }
        }

        public int SubItemCount
        {
            get { return this._SubItemCount; }
            set { this._SubItemCount = value; }
        }

        public int ProcessCount
        {
            get { return this._ProcessCount; }
            set { this._ProcessCount = value; }
        }

        public int Sequence
        {
            get { return this._Sequence; }
            set { this._Sequence = value; }
        }

        public int CategoryID
        {
            get { return this._CategoryID; }
            set { this._CategoryID = value; }
        }

        public int VariationId
        {
            get { return this._VariationID; }
            set { this._VariationID = value; }
        }

    }
}