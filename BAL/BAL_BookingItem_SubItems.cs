using System.Data;

namespace BAL
{
    public class BAL_BookingItem_SubItems
    {
        public int SaveBookingItem_SubItems(DTO.Booking_Items_SubItems Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItem_SubItems.SaveBookingItem_SubItems(Ob);
        }

        public DataSet GetAllBookingItems_SubItems(DTO.Booking_Items_SubItems Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItem_SubItems.GetAllBookingItems_SubItems(Ob);
        }

        public int DeleteBookingItems_SubItems(DTO.Booking_Items_SubItems Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItem_SubItems.DeleteBookingItems_SubItems(Ob);
        }
    }
}