namespace DTO
{
    public class Patterns
    {
        private string _PatternID;
        private string _PatternName;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _BranchId;
        private string _ImageName;

        public string ImageName
        {
            get { return this._ImageName; }
            set { this._ImageName = value; }
        }

        public string PatternID
        {
            get { return this._PatternID; }
            set { this._PatternID = value; }
        }

        public string PatternName
        {
            get { return this._PatternName; }
            set { this._PatternName = value; }
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

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }
    }
}