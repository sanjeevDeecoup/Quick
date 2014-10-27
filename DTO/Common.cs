namespace DTO
{
    public class Common
    {
        private string _Input;
        private string _Id;
        private string _BId;
        private string _Result;
        private string _Path;
        private string _UserID;
        private string _ChangeName;
        private string _LedgerName;

        public string ChangeName
        {
            get { return this._ChangeName; }
            set { this._ChangeName = value; }
        }

        public string LedgerName
        {
            get { return this._LedgerName; }
            set { this._LedgerName = value; }
        }

        public string Input
        {
            get { return this._Input; }
            set { this._Input = value; }
        }

        public string Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        public string Result
        {
            get { return this._Result; }
            set { this._Result = value; }
        }

        public string BID
        {
            get { return this._BId; }
            set { this._BId = value; }
        }

        public string Path
        {
            get { return this._Path; }
            set { this._Path = value; }
        }

        public string UserId
        {
            get { return this._UserID; }
            set { this._UserID = value; }
        }
    }
}