using System.Data;

namespace BAL
{
    public class BAL_BookingItemBrands
    {
        public int SaveBookingItemBrand(DTO.Booking_Items_Brands Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_ItemBrands.SaveBookingItemBrand(Ob);
        }

        public DataSet GetAllBookingItemBrands(DTO.Booking_Items_Brands Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_ItemBrands.GetAllBookingItemBrands(Ob);
        }

        public int DeleteBookingItemBrands(DTO.Booking_Items_Brands Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking_ItemBrands.DeleteBookingItemBrands(Ob);
        }
    }
}