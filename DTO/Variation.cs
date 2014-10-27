namespace DTO
{
    public class Variation
    {
        private string _VariationId;
        private string _VariationName;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _BranchId;

        public string VariationId
        {
            get { return this._VariationId; }
            set { this._VariationId = value; }
        }

        public string VariationName
        {
            get { return this._VariationName; }
            set { this._VariationName = value; }
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