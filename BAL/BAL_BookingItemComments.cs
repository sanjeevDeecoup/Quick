using System.Data;

namespace BAL
{
    public class BAL_BookingItemComments
    {
        public int SaveBookingItemComments(DTO.Booking_Items_Comments Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItemComment.SaveBookingItemComments(Ob);
        }

        public DataSet GetAllBookingItemCOmments(DTO.Booking_Items_Comments Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItemComment.GetAllBookingItemComments(Ob);
        }

        public int DeleteBookingItemComments(DTO.Booking_Items_Comments Ob)
        {
            return DAL.DALFactory.Instance.DAL_BookingItemComment.DeleteBookingItemComments(Ob);
        }
    }
}