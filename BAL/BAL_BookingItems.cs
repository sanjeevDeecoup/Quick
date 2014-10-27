using System.Data;

namespace BAL
{
    public class BAL_BookingItems
    {
        public int SaveBookingItemsData(DTO.Booking_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_Items.SaveBookingItemsData(Ob);
        }

        public DataSet GetDS(DTO.Booking_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_Items.GetDS(Ob);
        }

        public int DeleteBooking(DTO.Booking_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_Items.DeleteBooking(Ob);
        }
    }
}