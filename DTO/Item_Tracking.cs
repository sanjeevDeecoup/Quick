namespace DTO
{
    public class Item_Tracking
    {
        private int _TrackingId;
        private int _TrackId;
        private int _BookingID;
        private int _ItemID;
        private int _IsSubItem;
        private int _SubItemID;
        private int _SubItemItemID;
        private int _TrackingStatus;
        private string _Barcode;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private int _CreatedBy;
        private int _ModifiedBy;
        private int _BranchID;

        public int TrackingId
        {
            get { return this._TrackingId; }
            set { this._TrackingId = value; }
        }

        public int TrackId
        {
            get { return this._TrackId; }
            set { this._TrackId = value; }
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

        public int IsSubItem
        {
            get { return this._IsSubItem; }
            set { this._IsSubItem = value; }
        }

        public int SubItemID
        {
            get { return this._SubItemID; }
            set { this._SubItemID = value; }
        }

        public int SubItemItemID
        {
            get { return this._SubItemItemID; }
            set { this._SubItemItemID = value; }
        }

        public int TrackingStatus
        {
            get { return this._TrackingStatus; }
            set { this._TrackingStatus = value; }
        }

        public string BarCode
        {
            get { return this._Barcode; }
            set { this._Barcode = value; }
        }

        public int BranchID
        {
            get { return this._BranchID; }
            set { this._BranchID = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string DateCreated
        {
            get { return this._DateCreated; }
            set { this._DateCreated = value; }
        }

        public string DateModified
        {
            get { return this._DateModified; }
            set { this._DateModified = value; }
        }

        public int CreatedBy
        {
            get { return this._CreatedBy; }
            set { this._CreatedBy = value; }
        }

        public int ModiFiedBy
        {
            get { return this._ModifiedBy; }
            set { this._ModifiedBy = value; }
        }
    }
}