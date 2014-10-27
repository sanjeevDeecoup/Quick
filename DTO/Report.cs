using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DTO
{
    public class Report
    {
        private string _description;
        private string _FromDate;
        private string _UptoDate;
        private string _Year;
        private string _month;
        private string _day;
        private string _CustId;
        private string _InvoiceNo;
        private string _Date;
        private string _BranchId;
        private float _totalQty;
        private float _qtyPending;
        private float _bookedQty;
        private float _deliveredQty;
        private float _totalAmount;
        private float _bookedAmt;
        private float _amountPending;
        private float _amountRcvd;
        private string _custCodesStr;
        private DataSet _datasetMain;
        private DataSet _datasetOther;
        private DataTable _datatableMain;
        private DataTable _datatableOther;
        private DataView _dataview;
        private int _counter;
        private string _reportCaption;
        private string _strSqlQuery;
        private string _grdCaption;
        private string[] _strArray;
        private List<string> _lstCodes;
        private string _strCodes;
        private StringBuilder _strBuilder;
        private int _totalInvoice;
        private List<float> _fltTotals;
        private float _fltTotalCounter;
        private string _rptCaption;
        private static bool _bFlag;
        private string _HNumber;
        private string _UserID;

        public string HNumber
        {
            get { return this._HNumber; }
            set { this._HNumber = value; }
        }

        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        public string FromDate
        {
            get { return this._FromDate; }
            set { this._FromDate = value; }
        }

        public string UptoDate
        {
            get { return this._UptoDate; }
            set { this._UptoDate = value; }
        }

        public string Year
        {
            get { return this._Year; }
            set { this._Year = value; }
        }

        public string Month
        {
            get { return this._month; }
            set { this._month = value; }
        }

        public string Day
        {
            get { return this._day; }
            set { this._day = value; }
        }

        public string CustId
        {
            get { return this._CustId; }
            set { this._CustId = value; }
        }

        public string InvoiceNo
        {
            get { return this._InvoiceNo; }
            set { this._InvoiceNo = value; }
        }

        public string Date
        {
            get { return this._Date; }
            set { this._Date = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public float QtyTotal
        {
            get { return this._totalQty; }
            set { this._totalQty = value; }
        }

        public float QtyPending
        {
            get { return this._qtyPending; }
            set { this._qtyPending = value; }
        }

        public float QtyBooked
        {
            get { return this._bookedQty; }
            set { this._bookedQty = value; }
        }

        public float QtyDelivered
        {
            get { return this._deliveredQty; }
            set { this._deliveredQty = value; }
        }

        public float AmountTotal
        {
            get { return this._totalAmount; }
            set { this._totalAmount = value; }
        }

        public float AmountBkd
        {
            get { return this._bookedAmt; }
            set { this._bookedAmt = value; }
        }

        public float AmountPending
        {
            get { return this._amountPending; }
            set { this._amountPending = value; }
        }

        public float AmountRcvd
        {
            get { return this._amountRcvd; }
            set { this._amountRcvd = value; }
        }

        public string CustCodeStr
        {
            get { return this._custCodesStr; }
            set { this._custCodesStr = value; }
        }

        public List<string> LstCodes
        {
            get { return this._lstCodes; }
            set
            {
                this._lstCodes = value;
            }
        }

        public DataSet DatasetMain
        {
            get { return this._datasetMain; }
            set { this._datasetMain = value; }
        }

        public DataSet DatasetOther
        {
            get { return this._datasetOther; }
            set { this._datasetOther = value; }
        }

        public DataTable DatatableMain
        {
            get { return this._datatableMain; }
            set { this._datatableMain = value; }
        }

        public DataTable DataTableOther
        {
            get { return this._datatableOther; }
            set { this._datatableOther = value; }
        }

        public DataView DataViewMain
        {
            get { return this._dataview; }
            set { this._dataview = value; }
        }

        public int Counter
        {
            get { return this._counter; }
            set { this._counter = value; }
        }

        public string ReportCaption
        {
            get { return this._reportCaption; }
            set { this._reportCaption = value; }
        }

        public string StrSqlQuery
        {
            get { return this._strSqlQuery; }
            set { this._strSqlQuery = value; }
        }

        public string GrdCaption
        {
            get { return this._grdCaption; }
            set { this._grdCaption = value; }
        }

        public string[] StrArray
        {
            get { return this._strArray; }
            set { this._strArray = value; }
        }

        public string StrCodes
        {
            get { return this._strCodes; }
            set { this._strCodes = value; }
        }

        public StringBuilder StrBuilder
        {
            get { return this._strBuilder; }
            set { this._strBuilder = value; }
        }

        public int TotalInvoice
        {
            get { return this._totalInvoice; }
            set { this._totalInvoice = value; }
        }

        public List<float> FltTotals
        {
            get { return this._fltTotals; }
            set { this._fltTotals = value; }
        }

        public float FltTotalsCounter
        {
            get { return this._fltTotalCounter; }
            set { this._fltTotalCounter = value; }
        }

        public string RptCaption
        {
            get { return this._rptCaption; }
            set { this._rptCaption = value; }
        }

        public static bool BFlag
        {
            get { return _bFlag; }
            set { _bFlag = value; }
        }
        public string UserID
        {
            get { return this._UserID; }
            set { this._UserID = value; }
        }

        public string EndDate { get; set; }

        public string StartBkNum { get; set; }

        public string EndBkNum { get; set; }

        public string[] ItemsArray { get; set; }
    }
}