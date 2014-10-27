using System.Collections.Generic;

namespace DTO
{
    public class BookingLineItem
    {
        //Item Details
        private Booking_Items _itemDetails;

        //Patterns
        private List<Booking_Items_Patterns> _patterns;

        //Colors
        private List<Booking_Items_Colors> _colors;

        //SubItems
        private List<Booking_Items_SubItems> _subItems;

        //Brands
        private List<Booking_Items_Brands> _brands;

        //Comments
        private List<Booking_Items_Comments> _comments;

        //Processes
        private List<Booking_Items_Processes> _processes;

        public BookingLineItem()
        {
            _itemDetails = new Booking_Items();
            _patterns = new List<Booking_Items_Patterns>();
            _colors = new List<Booking_Items_Colors>();
            _subItems = new List<Booking_Items_SubItems>();
            _brands = new List<Booking_Items_Brands>();
            _comments = new List<Booking_Items_Comments>();
            _processes = new List<Booking_Items_Processes>();
        }

        //Item Details
        public Booking_Items ItemDetails
        {
            get { return _itemDetails; }
            set { _itemDetails = value; }
        }

        //Patterns
        public List<Booking_Items_Patterns> Patterns
        {
            get { return _patterns; }
            set { _patterns = value; }
        }

        //Colors
        public List<Booking_Items_Colors> Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        //SubItems
        public List<Booking_Items_SubItems> SubItems
        {
            get { return _subItems; }
            set { _subItems = value; }
        }

        //Brands
        public List<Booking_Items_Brands> Brands
        {
            get { return _brands; }
            set { _brands = value; }
        }

        //Comments
        public List<Booking_Items_Comments> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        //Processes
        public List<Booking_Items_Processes> Processes
        {
            get { return _processes; }
            set { _processes = value; }
        }

    }
}