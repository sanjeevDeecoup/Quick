namespace DTO
{
    public class BacodeLable
    {
        private string _BarCode;
        private string _BNO;
        private string _RowIndex;
        private int _Active;

        public string BarCode
        {
            get { return this._BarCode; }
            set { this._BarCode = value; }
        }

        public string BNO
        {
            get { return this._BNO; }
            set { this._BNO = value; }
        }

        public string RowIndex
        {
            get { return this._RowIndex; }
            set { this._RowIndex = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }
    }
}