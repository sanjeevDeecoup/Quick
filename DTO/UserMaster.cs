namespace DTO
{
    public class UserMaster
    {
        private string _UserId;
        private string _UserPassword;
        private int _UserTypeCode;
        private string _UserBranchCode;
        private string _UserName;
        private string _UserAddress;
        private string _UserPhoneNumber;
        private string _UserMobileNumber;
        private string _UserEmailId;
        private string _UserActive;
        private string _BranchId;
        private string _ExternalBranchId;
        private string _Updatepassword;
        private string _UserBarcode;
        private string _UserPin;
        private string _PrevUserBarcode;
        private string _PreUserPin;

        public string UserId
        {
            get { return this._UserId; }
            set { this._UserId = value; }
        }

        public string UserBarcode
        {
            get { return this._UserBarcode; }
            set { this._UserBarcode = value; }
        }

        public string UserPin
        {
            get { return this._UserPin; }
            set { this._UserPin = value; }
        }

        public string PreUserBarcode
        {
            get { return this._PrevUserBarcode; }
            set { this._PrevUserBarcode = value; }
        }

        public string PreUserPin
        {
            get { return this._PreUserPin; }
            set { this._PreUserPin = value; }
        }

        public string UserPassword
        {
            get { return this._UserPassword; }
            set { this._UserPassword = value; }
        }

        public int UserTypeCode
        {
            get { return this._UserTypeCode; }
            set { this._UserTypeCode = value; }
        }

        public string UserBranchCode
        {
            get { return this._UserBranchCode; }
            set { this._UserBranchCode = value; }
        }

        public string UserName
        {
            get { return this._UserName; }
            set { this._UserName = value; }
        }

        public string UserAddress
        {
            get { return this._UserAddress; }
            set { this._UserAddress = value; }
        }

        public string UserPhoneNumber
        {
            get { return this._UserPhoneNumber; }
            set { this._UserPhoneNumber = value; }
        }

        public string UserMobileNumber
        {
            get { return this._UserMobileNumber; }
            set { this._UserMobileNumber = value; }
        }

        public string UserEmailId
        {
            get { return this._UserEmailId; }
            set { this._UserEmailId = value; }
        }

        public string UserActive
        {
            get { return this._UserActive; }
            set { this._UserActive = value; }
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

        public string Updatepassword
        {
            get { return this._Updatepassword; }
            set { this._Updatepassword = value; }
        }
    }
}