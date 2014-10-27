using System.Data;

namespace BAL
{
    public class BAL_Booking_ItemColors
    {
        public int SaveBookingItemColors(DTO.Booking_Items_Colors Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItem_Colors.SaveBookingItemColors(Ob);
        }

        public DataSet GetAllBookingItemColors(DTO.Booking_Items_Colors Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItem_Colors.GetAllBookingItemColors(Ob);
        }

        public int DeleteBookingItemColors(DTO.Booking_Items_Colors Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItem_Colors.DeleteBookingItemColors(Ob);
        }
    }
}