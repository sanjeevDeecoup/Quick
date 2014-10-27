namespace DTO
{
    public class FactoryChallanHeader
    {
        private int _FactoryChallanID;
        private int _FactoryChallanHeaderID;
        private string _FactoryChallanNumber;
        private int _TotalItemCount;
        private string _OutDatetime;
        private string _InDatetime;
        private int _FactoryChallanStatus;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private int _CreatedBy;
        private int _ModifiedBy;
        private int _BranchID;

        public int FactoryChallanID
        {
            get { return this._FactoryChallanID; }
            set { this._FactoryChallanID = value; }
        }

        public int FactoryChallanHeaderID
        {
            get { return this._FactoryChallanHeaderID; }
            set { this._FactoryChallanHeaderID = value; }
        }

        public string FactoryChallanNumber
        {
            get { return this._FactoryChallanNumber; }
            set { this._FactoryChallanNumber = value; }
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
            get { return this._InDatetime; }
            set { this._InDatetime = value; }
        }

        public int FactoryChallanStatus
        {
            get { return this._FactoryChallanStatus; }
            set { this._FactoryChallanStatus = value; }
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

        public string DateModifIed
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

        public int BranchID
        {
            get { return this._BranchID; }
            set { this._BranchID = value; }
        }
    }
}