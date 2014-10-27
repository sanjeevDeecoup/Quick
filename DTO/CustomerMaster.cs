using System;

namespace DTO
{
    public class CustomerMaster
    {
        private int _Id;
        private int _CustID;
        private string _CustomerCode;
        private string _BookingNumber;
        private string _CustomerSalutation;
        private string _CustomerName;
        private string _CustomerAddress;
        private string _CustomerPhone;
        private string _CustomerMobile;
        private string _CustomerEmailId;
        private string _CustomerPriority;
        private string _CustomerRefferredBy;
        private DateTime _CustomerRegisterDate;
        private bool _CustomerIsActive;
        private DateTime _CustomerCancelDate;
        private float _DefaultDiscountRate;
        private string _Remarks;
        private string _BirthDate;
        private string _AnniversaryDate;
        private string _CityName;
        private string _AreaLocation;
        private string _Profession;
        private string _BranchId;
        private bool _IsWebsite;

        public bool IsWebsite
        {
            get { return this._IsWebsite; }
            set { this._IsWebsite = value; }
        }

        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        public int CustID
        {
            get { return this.CustID; }
            set { this.CustID = value; }
        }

        public string CustomerCode
        {
            get { return this._CustomerCode; }
            set { this._CustomerCode = value; }
        }

        public string BookingNumber
        {
            get { return this._BookingNumber; }
            set { this._BookingNumber = value; }
        }

        public string CustomerSalutation
        {
            get { return this._CustomerSalutation; }
            set { this._CustomerSalutation = value; }
        }

        public string CustomerName
        {
            get { return this._CustomerName; }
            set { this._CustomerName = value; }
        }

        public string CustomerAddress
        {
            get { return this._CustomerAddress; }
            set { this._CustomerAddress = value; }
        }

        public string CustomerPhone
        {
            get { return this._CustomerPhone; }
            set { this._CustomerPhone = value; }
        }

        public string CustomerMobile
        {
            get { return this._CustomerMobile; }
            set { this._CustomerMobile = value; }
        }

        public string CustomerEmailId
        {
            get { return this._CustomerEmailId; }
            set { this._CustomerEmailId = value; }
        }

        public string CustomerPriority
        {
            get { return this._CustomerPriority; }
            set { this._CustomerPriority = value; }
        }

        public string CustomerRefferredBy
        {
            get { return this._CustomerRefferredBy; }
            set { this._CustomerRefferredBy = value; }
        }

        public DateTime CustomerRegisterDate
        {
            get { return this._CustomerRegisterDate; }
            set { this._CustomerRegisterDate = value; }
        }

        public bool CustomerIsActive
        {
            get { return this._CustomerIsActive; }
            set { this._CustomerIsActive = value; }
        }

        public DateTime CustomerCancelDate
        {
            get { return this._CustomerCancelDate; }
            set { this._CustomerCancelDate = value; }
        }

        public float DefaultDiscountRate
        {
            get { return this._DefaultDiscountRate; }
            set { this._DefaultDiscountRate = value; }
        }

        public string Remarks
        {
            get { return this._Remarks; }
            set { this._Remarks = value; }
        }

        public string BirthDate
        {
            get { return this._BirthDate; }
            set { this._BirthDate = value; }
        }

        public string AnniversaryDate
        {
            get { return this._AnniversaryDate; }
            set { this._AnniversaryDate = value; }
        }

        public string City
        {
            get { return this._CityName; }
            set { this._CityName = value; }
        }

        public string AreaLocation
        {
            get { return this._AreaLocation; }
            set { this._AreaLocation = value; }
        }

        public string Profession
        {
            get { return this._Profession; }
            set { this._Profession = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public string CommunicationMeans { get; set; }

        public string MemberShipId { get; set; }

        public string BarCode { get; set; }

        public string PrevMemberId { get; set; }

        public string PrevBarcode { get; set; }

        public string PrevMobile { get; set; }

        public string RateListId { get; set; }
    }
}