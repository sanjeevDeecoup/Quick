namespace DTO
{
    public class FactoryChallanDetail
    {
        private int _FactoryDetailID;
        private int _FactoryChallanDetailID;
        private int _FactoryChallanHeaderID;
        private int _TrackingID;
        private int _BranchID;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private int _CreatedBy;
        private int _ModifiedBy;
        private string _BID;

        public string BID
        {
            get { return this._BID; }
            set { this._BID = value; }
        }

        public int FactoryDetailID
        {
            get { return this._FactoryDetailID; }
            set { this._FactoryDetailID = value; }
        }

        public int FactoryChallanDetailID
        {
            get { return this._FactoryChallanDetailID; }
            set { this._FactoryChallanDetailID = value; }
        }

        public int FactoryChallanHeaderID
        {
            get { return this._FactoryChallanHeaderID; }
            set { this._FactoryChallanHeaderID = value; }
        }

        public int TrackingID
        {
            get { return this._TrackingID; }
            set { this._TrackingID = value; }
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

        public int ModifiedBy
        {
            get { return this._ModifiedBy; }
            set { this._ModifiedBy = value; }
        }
    }
}