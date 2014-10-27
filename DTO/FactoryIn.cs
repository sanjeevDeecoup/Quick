namespace DTO
{
    public class FactoryIn
    {
        public string _BookingNo;
        public int _RowIndex;
        public string _BranchId;

        public string BookingNo
        {
            get { return this._BookingNo; }
            set { this._BookingNo = value; }
        }

        public int RowIndex
        {
            get { return this._RowIndex; }
            set { this._RowIndex = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }
    }
}