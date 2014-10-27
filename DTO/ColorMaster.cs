namespace DTO
{
    public class ColorMaster
    {
        private int _ColorID;
        private string _ColorName;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _ImageName;
        private string _BranchId;

        public int ColorID
        {
            get { return this._ColorID; }
            set { this._ColorID = value; }
        }

        public string ColorName
        {
            get { return this._ColorName; }
            set { this._ColorName = value; }
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