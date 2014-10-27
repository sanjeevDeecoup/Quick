namespace DTO
{
    public class Holiday
    {
        private int _holidayid;
        private string _weeklyoff;
        private string _description;
        private string _date;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _BranchId;

        public int holidayid
        {
            get { return this._holidayid; }
            set { this._holidayid = value; }
        }

        public string weeklyoff
        {
            get { return this._weeklyoff; }
            set { this._weeklyoff = value; }
        }

        public string description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        public string date
        {
            get { return this._date; }
            set { this._date = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }
    }
}