namespace DTO
{
    public class BranchChallanDetail
    {
        private int _BranchChallanDetailID;
        private int _BranchChallanHeaderID;
        private int _BranchDetailID;
        private int _TrackingID;
        private int _Active;
        private int _BranchID;
        private string _DateCreated;
        private string _DateModified;
        private int _CreatedBy;
        private int _ModifiedBy;

        public int BranchChallanDetailID
        {
            get { return this._BranchChallanDetailID; }
            set { this._BranchChallanDetailID = value; }
        }

        public int BranchChallanHeaderId
        {
            get { return this._BranchChallanHeaderID; }
            set { this._BranchChallanHeaderID = value; }
        }

        public int BranchDetailID
        {
            get { return this._BranchDetailID; }
            set { this._BranchDetailID = value; }
        }

        public int TrackingId
        {
            get { return this._TrackingID; }
            set { this._TrackingID = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public int BranchID
        {
            get { return this._BranchID; }
            set { this._BranchID = value; }
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

        public int ModifiedBy
        {
            get { return this._ModifiedBy; }
            set { this._ModifiedBy = value; }
        }
    }
}