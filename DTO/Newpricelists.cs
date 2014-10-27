namespace DTO
{
    public class NewPriceLists
    {
        private string _BranchId;

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        private string _ItemCode;
        private string _Active;
        private string _VariationId;
        private string _SubItemRefID;
        private string _CategoryID;
        private string _Processid;
        private string _Price;
        private string _DateCreated;
        private string _DateModified;

        public string DateModified
        {
            get { return this._DateModified; }
            set { this._DateModified = value; }
        }

        public string DateCreated
        {
            get { return this._DateCreated; }
            set { this._DateCreated = value; }
        }

        public string Price
        {
            get { return this._Price; }
            set { this._Price = value; }
        }

        public string Processid
        {
            get { return this._Processid; }
            set { this._Processid = value; }
        }

        public string VariationId
        {
            get { return this._VariationId; }
            set { this._VariationId = value; }
        }

        public string SubItemRefID
        {
            get { return this._SubItemRefID; }
            set { this._SubItemRefID = value; }
        }

        public string CategoryID
        {
            get { return this._CategoryID; }
            set { this._CategoryID = value; }
        }

        public string Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string ItemCode
        {
            get { return this._ItemCode; }
            set { this._ItemCode = value; }
        }
    }
}