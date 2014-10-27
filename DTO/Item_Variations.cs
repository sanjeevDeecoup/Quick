namespace DTO
{
    public class Item_Variations
    {
        private int _ItemVariationID;
        private int _ItemVarID;
        private int _ItemID;
        private int _VariationID;
        private int _DefaultSelected;
        private int _Active;
        private int _BranchID;
        private string _DateModified;
        private string _DateCreated;

        public int _ItemVariationId
        {
            get { return this._ItemVariationId; }
            set { this._ItemVariationId = value; }
        }

        public int _ItemVarId
        {
            get { return this._ItemVarId; }
            set { this._ItemVarId = value; }
        }

        public int ItemID
        {
            get { return this._ItemID; }
            set { this._ItemID = value; }
        }

        public int VariationID
        {
            get { return this._VariationID; }
            set { this._VariationID = value; }
        }

        public int DefaultSelected
        {
            get { return this._DefaultSelected; }
            set { this._DefaultSelected = value; }
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
    }
}