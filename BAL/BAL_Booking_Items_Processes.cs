using System.Data;

namespace BAL
{
    public class BAL_Booking_Items_Processes
    {
        public int SaveBookingItemsData(DTO.Booking_Items_Processes Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_Items_Processes.SaveBookingItemsData(Ob);
        }

        public DataSet GetDS(DTO.Booking_Items_Processes Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_Items_Processes.GetDS(Ob);
        }

        public int DeleteBooking(DTO.Booking_Items_Processes Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_Items_Processes.DeleteBooking(Ob);
        }
    }
}