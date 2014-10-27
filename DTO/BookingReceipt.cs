using System.Collections.Generic;

namespace DTO
{
    public class BookingReceipt
    {
        private BookingReceiptHeader _receiptHeader;
        private List<BookingLineItem> _lineItems;

        public BookingReceiptHeader ReceiptHeader
        {
            get { return _receiptHeader; }
            set { _receiptHeader = value; }
        }

        public List<BookingLineItem> LineItems
        {
            get { return _lineItems; }
            set { _lineItems = value; }
        }

        public BookingReceipt()
        {
            _receiptHeader = new BookingReceiptHeader();
            _lineItems = new List<BookingLineItem>();
        }
    }
}