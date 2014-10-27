namespace DTO
{
    public class PackageMaster
    {
        private string _PackageId;
        private string _PackageName;
        private string _PackageType;
        private float _PackageCost;
        private string _BenefitType;
        private float _BenefitValue;
        private string _Active;
        private string _BranchId;

        /// <summary>
        /// Assign Package Variable
        /// </summary>

        private string _AssignId;
        private string _CustomerCode;
        private string _StartDate;
        private string _EndDate;
        private float _StartValue;
        private string _PackageComplete;
        private string _CustomerMobile;
        private string _CustomerEmailID;

        public string PackageId
        {
            get { return this._PackageId; }
            set { this._PackageId = value; }
        }

        public string PackageName
        {
            get { return this._PackageName; }
            set { this._PackageName = value; }
        }

        public string PackageType
        {
            get { return this._PackageType; }
            set { this._PackageType = value; }
        }

        public float PackageCost
        {
            get { return this._PackageCost; }
            set { this._PackageCost = value; }
        }

        public string BenefitType
        {
            get { return this._BenefitType; }
            set { this._BenefitType = value; }
        }

        public float BenefitValue
        {
            get { return this._BenefitValue; }
            set { this._BenefitValue = value; }
        }

        public string Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public string AssignId
        {
            get { return this._AssignId; }
            set { this._AssignId = value; }
        }

        public string CustomerCode
        {
            get { return this._CustomerCode; }
            set { this._CustomerCode = value; }
        }

        public string StartDate
        {
            get { return this._StartDate; }
            set { this._StartDate = value; }
        }

        public string EndDate
        {
            get { return this._EndDate; }
            set { this._EndDate = value; }
        }

        public float StartValue
        {
            get { return this._StartValue; }
            set { this._StartValue = value; }
        }

        public string PackageComplete
        {
            get { return this._PackageComplete; }
            set { this._PackageComplete = value; }
        }

        public string CustomerMobile
        {
            get { return this._CustomerMobile;  }
            set { this._CustomerMobile = value; }
        }
        public string CustomerEmailID
        {
            get { return this._CustomerEmailID; }
            set { this._CustomerEmailID = value; }
        }

        public string BarCode { get; set; }

        public string MembershipId { get; set; }

        public string PrevMemberShipId { get; set; }

        public string PrevBarCode { get; set; }

        public string PaymentDetails { get; set; }

        public string PaymentTypes { get; set; }

        public string CurDiscount { get; set; }

        public string Recurrence { get; set; }

        public string TaxType { get; set; }

        public float PackageRate { get; set; }

        public int TotalQty { get; set; }

        public float PackageTotalCost { get; set; }
    }
}