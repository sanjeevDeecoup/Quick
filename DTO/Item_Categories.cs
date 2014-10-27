namespace DTO
{
    public class Item_Categories
    {
        private int _ItemCatID;
        private int _ItemCategoryID;
        private int _ItemID;
        private int _CategoryID;
        private int _DefaultSelected;
        private int _Active;
        private int _BranchID;
        private string _DateCreated;
        private string _DateModified;

        public int ItemCategoryID
        {
            get { return this._ItemCategoryID; }
            set { this._ItemCategoryID = value; }
        }

        public int ItemCatID
        {
            get { return this._ItemCatID; }
            set { this._ItemCatID = value; }
        }

        public int ItemID
        {
            get { return this._ItemID; }
            set { this._ItemID = value; }
        }

        public int CategoryID
        {
            get { return this._CategoryID; }
            set { this._CategoryID = value; }
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