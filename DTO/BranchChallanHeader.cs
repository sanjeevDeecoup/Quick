namespace DTO
{
    public class BranchChallanHeader
    {
        private int _ChallanHeaderId;
        private int _BranchChallanHeaderId;
        private string _BookingChallanNumber;
        private int _TotalItemCount;
        private string _OutDatetime;
        private string _InDatetime;
        private int _BranchChallanStatus;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private int _BranchId;
        private int _CreatedBy;
        private int _ModifiedBy;

        public int ChallanHeaderId
        {
            get { return this._BranchChallanHeaderId; }
            set { this._BranchChallanHeaderId = value; }
        }

        public int BranchChallanHeaderId
        {
            get { return this._BranchChallanHeaderId; }
            set { this._BranchChallanHeaderId = value; }
        }

        public string BookingChallanNumber
        {
            get { return this._BookingChallanNumber; }
            set { this._BookingChallanNumber = value; }
        }

        public int TotalItemCount
        {
            get { return this._TotalItemCount; }
            set { this._TotalItemCount = value; }
        }

        public string OutDateTime
        {
            get { return this._OutDatetime; }
            set { this._OutDatetime = value; }
        }

        public string InDateTime
        {
            get { return this._OutDatetime; }
            set { this._OutDatetime = value; }
        }

        public int BranchChallanStatus
        {
            get { return this._BranchChallanStatus; }
            set { this._BranchChallanStatus = value; }
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

        public int BranchID
        {
            get { return this.BranchID; }
            set { this.BranchID = value; }
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