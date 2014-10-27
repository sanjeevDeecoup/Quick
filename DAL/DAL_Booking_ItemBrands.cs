using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Booking_ItemBrands
    {
        public int SaveBookingItemBrand(DTO.Booking_Items_Brands Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemBrands");
            FlagType flag;
            if (Ob.Booking_ItemBrandID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItemBrands(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public SqlCommand AddParameters_BookingItemBrands(SqlCommand cmd, FlagType flag, DTO.Booking_Items_Brands Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@FLAG", flag));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemBrandID", Ob.Booking_ItemBrandID));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemID", Ob.BookingItemID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", Ob.BrandID));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.CommonFields.BranchId));
            return cmd;
        }

        public DataSet GetAllBookingItemBrands(DTO.Booking_Items_Brands Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemBrands");
            cmd = AddParameters_BookingItemBrands(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBookingItemBrands(DTO.Booking_Items_Brands Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemBrands");
            cmd = AddParameters_BookingItemBrands(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}