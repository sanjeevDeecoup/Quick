using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BookingItem_Colors
    {
        public int SaveBookingItemColors(DTO.Booking_Items_Colors Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemColors");
            FlagType flag;
            if (Ob.Booking_ItemColorID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItemCOlors(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public SqlCommand AddParameters_BookingItemCOlors(SqlCommand cmd, FlagType flag, DTO.Booking_Items_Colors Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@FLAG", flag));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemColorID", Ob.Booking_ItemColorID));
            cmd.Parameters.Add(new SqlParameter("@ColorID", Ob.ColorID));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemID", Ob.BookingItemID));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.CommonFields.BranchId));
            return cmd;
        }

        public DataSet GetAllBookingItemColors(DTO.Booking_Items_Colors Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemColors");
            cmd = AddParameters_BookingItemCOlors(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBookingItemColors(DTO.Booking_Items_Colors Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemColors");
            cmd = AddParameters_BookingItemCOlors(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}