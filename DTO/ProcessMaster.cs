namespace DTO
{
    public class ProcessMaster
    {
        private string _ProcessCode;
        private string _ProcessName;
        private string _UsedForvendorReport;
        private string _Discount;
        private double _ServiceTax;
        private string _IsActiveServiceTax;
        private string _IsDiscount;
        private string _IsChallanApplicable;
        private string _ImagePath;
        private string _BID;
        private double _CssTax;
        private double _EcesjTax;
        private string _IsReady;

        public string ProcessCode
        {
            get { return this._ProcessCode; }
            set { this._ProcessCode = value; }
        }

        public string IsChallanApplicable
        {
            get { return this._IsChallanApplicable; }
            set { this._IsChallanApplicable = value; }
        }

        public string ProcessName
        {
            get { return this._ProcessName; }
            set { this._ProcessName = value; }
        }

        public string UsedForvendorReport
        {
            get { return this._UsedForvendorReport; }
            set { this._UsedForvendorReport = value; }
        }

        public string Discount
        {
            get { return this._Discount; }
            set { this._Discount = value; }
        }

        public double ServiceTax
        {
            get { return this._ServiceTax; }
            set { this._ServiceTax = value; }
        }

        public string IsActiveServiceTax
        {
            get { return this._IsActiveServiceTax; }
            set { this._IsActiveServiceTax = value; }
        }

        public string IsDiscount
        {
            get { return this._IsDiscount; }
            set { this._IsDiscount = value; }
        }

        public string ImagePath
        {
            get { return this._ImagePath; }
            set { this._ImagePath = value; }
        }

        public string BID
        {
            get { return this._BID; }
            set { this._BID = value; }
        }

        public double CssTax
        {
            get { return this._CssTax; }
            set { this._CssTax = value; }
        }

        public double EcesjTax
        {
            get { return this._EcesjTax; }
            set { this._EcesjTax = value; }
        }
        public string IsReady
        {
            get { return this._IsReady; }
            set { this._IsReady = value; }
        }
    }
}