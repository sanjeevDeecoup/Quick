namespace DTO
{
    public class BranchMaster
    {
        private string _BranchCode;
        private string _BranchName;
        private string _BranchAddress;
        private string _BranchPhone;
        private string _BranchSlogan;
        private string _BranchId;
        private string _ExternalBranchId;
        private bool _IsBF;
        private bool _IsChallan;
        private string _BranchMobile;
        private string _BranchEmail;
        private string _BusinessName;
        private string _IsLoginTime;
        private string _LoginStartTime;
        private string _LoginEndTime;
        private string _WeeklyOFF;

        public bool IsChallan
        {
            get { return this._IsChallan; }
            set { this._IsChallan = value; }
        }

        public bool IsBF
        {
            get { return this._IsBF; }
            set { this._IsBF = value; }
        }

        public string BranchCode
        {
            get { return this._BranchCode; }
            set { this._BranchCode = value; }
        }

        public string BranchName
        {
            get { return this._BranchName; }
            set { this._BranchName = value; }
        }

        public string BranchAddress
        {
            get { return this._BranchAddress; }
            set { this._BranchAddress = value; }
        }

        public string BranchPhone
        {
            get { return this._BranchPhone; }
            set { this._BranchPhone = value; }
        }

        public string BranchSlogan
        {
            get { return this._BranchSlogan; }
            set { this._BranchSlogan = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public string ExternalBranchId
        {
            get { return this._ExternalBranchId; }
            set { this._ExternalBranchId = value; }
        }
        public string BranchMobile
        {
            get { return this._BranchMobile; }
            set { this._BranchMobile = value; }
        }

        public string BranchEmail
        {
            get { return this._BranchEmail; }
            set { this._BranchEmail = value; }
        }
        public string  BusinessName
        {
            get { return this._BusinessName; }
            set { this._BusinessName = value; }
        }
        public string IsLoginTime
        {
            get { return this._IsLoginTime; }
            set { this._IsLoginTime = value; }
        }
        public string LoginStartTime
        {
            get { return this._LoginStartTime; }
            set { this._LoginStartTime = value; }
        }
        public string LoginEndTime
        {
            get { return this._LoginEndTime; }
            set { this._LoginEndTime = value; }
        }
        public string  WeeklyOFF
        {
            get { return this._WeeklyOFF; }
            set { this._WeeklyOFF = value; }
        }
    }
}