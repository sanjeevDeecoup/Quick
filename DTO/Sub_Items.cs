namespace DTO
{
    public class Sub_Items
    {
        private int _SubItemID;
        private int _SubID;
        private int _ItemID;
        private int _SubItemRefID;
        private int _DefaultSelected;
        private int _Active;
        private string _BranchID;
        private string _DateCreated;
        private string _DateModified;

        public int SubItemID
        {
            get { return this._SubItemID; }
            set { this._SubItemID = value; }
        }

        public int SubID
        {
            get { return this._SubID; }
            set { this._SubID = value; }
        }

        public int ItemID
        {
            get { return this._ItemID; }
            set { this._ItemID = value; }
        }

        public int SubItemRefID
        {
            get { return this._SubItemRefID; }
            set { this._SubItemRefID = value; }
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

        public string BranchID
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