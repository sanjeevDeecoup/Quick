namespace DTO
{
    public class Employee
    {
        private string _ID;
        private string _EmpCode;
        private string _Title;
        private string _EmpName;
        private string _Address;
        private string _PhoneNo;
        private string _Mobile;
        private string _EmailId;
        private string _BID;

        public string ID
        {
            get { return this._ID; }
            set { this._ID = value; }
        }

        public string EmpCode
        {
            get { return this._EmpCode; }
            set { this._EmpCode = value; }
        }

        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        public string EmpName
        {
            get { return this._EmpName; }
            set { this._EmpName = value; }
        }

        public string Address
        {
            get { return this._Address; }
            set { this._Address = value; }
        }

        public string PhoneNo
        {
            get { return this._PhoneNo; }
            set { this._PhoneNo = value; }
        }

        public string Mobile
        {
            get { return this._Mobile; }
            set { this._Mobile = value; }
        }

        public string EmailId
        {
            get { return this._EmailId; }
            set { this._EmailId = value; }
        }

        public string BID
        {
            get { return this._BID; }
            set { this._BID = value; }
        }
    }
}