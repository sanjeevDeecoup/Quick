namespace DTO
{
    public class Booking_Items_Comments
    {
        private int _Booking_ItemCommentID;
        private int _BookingItemID;
        private int _CommentID;
        private CommonFields _commonFields = new CommonFields();

        public CommonFields CommonFields
        {
            get { return _commonFields; }
            set { _commonFields = value; }
        }

        public int Booking_ItemCommentID
        {
            get { return this._Booking_ItemCommentID; }
            set { this._Booking_ItemCommentID = value; }
        }

        public int BookingItemID
        {
            get { return this._BookingItemID; }
            set { this._BookingItemID = value; }
        }

        public int CommentID
        {
            get { return this._CommentID; }
            set { this._CommentID = value; }
        }
    }
}