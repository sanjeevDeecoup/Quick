namespace DTO
{
    public class sms
    {
        private string _SmsId;
        private string _Template;
        private string _Massage;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _BranchId;
        private string _DefaultMsg;

        public string SmsId
        {
            get { return this._SmsId; }
            set { this._SmsId = value; }
        }

        public string DefaultMsg
        {
            get { return this._DefaultMsg; }
            set { this._DefaultMsg = value; }
        }

        public string Template
        {
            get { return this._Template; }
            set { this._Template = value; }
        }

        public string Massage
        {
            get { return this._Massage; }
            set { this._Massage = value; }
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

        private string _userid;
        private string _password;
        private string _api;
        private string _senderid;

        private string _massagedemo;
        private string _mobiledemo;
        private string _senderdemo;
        private string _useriddemo;
        private string _passworddemo;
        private string _senderposition;
        private string _userposition;
        private string _passwordposition;
        private string _mobileposition;
        private string _massageposition;

        public string massageposition
        {
            get { return this._massageposition; }
            set { this._massageposition = value; }
        }

        public string mobileposition
        {
            get { return this._mobileposition; }
            set { this._mobileposition = value; }
        }

        public string passwordposition
        {
            get { return this._passwordposition; }
            set { this._passwordposition = value; }
        }

        public string userposition
        {
            get { return this._userposition; }
            set { this._userposition = value; }
        }

        public string senderposition
        {
            get { return this._senderposition; }
            set { this._senderposition = value; }
        }

        public string passworddemo
        {
            get { return this._passworddemo; }
            set { this._passworddemo = value; }
        }

        public string useriddemo
        {
            get { return this._useriddemo; }
            set { this._useriddemo = value; }
        }

        public string senderdemo
        {
            get { return this._senderdemo; }
            set { this._senderdemo = value; }
        }

        public string mobiledemo
        {
            get { return this._mobiledemo; }
            set { this._mobiledemo = value; }
        }

        public string massagedemo
        {
            get { return this._massagedemo; }
            set { this._massagedemo = value; }
        }

        public string userid
        {
            get { return this._userid; }
            set { this._userid = value; }
        }

        public string password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        public string api
        {
            get { return this._api; }
            set { this._api = value; }
        }

        public string senderid
        {
            get { return this._senderid; }
            set { this._senderid = value; }
        }

        private string _bookingsms;
        private string _clothsms;
        private string _deliverysms;

        public string bookingsms
        {
            get { return this._bookingsms; }
            set { this._bookingsms = value; }
        }

        public string clothsms
        {
            get { return this._clothsms; }
            set { this._clothsms = value; }
        }

        public string deliverysms
        {
            get { return this._deliverysms; }
            set { this._deliverysms = value; }
        }
    }
}