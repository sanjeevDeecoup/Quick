namespace DTO
{
    public class Category
    {
        private string _CategoryID;
        private string _CategoryName;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _ImageName;
        private string _BranchId;

        public string CategoryID
        {
            get { return this._CategoryID; }
            set { this._CategoryID = value; }
        }

        public string CategoryName
        {
            get { return this._CategoryName; }
            set { this._CategoryName = value; }
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

        public string ImageName
        {
            get { return this._ImageName; }
            set { this._ImageName = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }
    }
}