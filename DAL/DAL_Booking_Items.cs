using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Booking_Items
    {
        public int SaveBookingItemsData(DTO.Booking_Items Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking_Items");
            FlagType flag;
            if (Ob.BookingItemID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItems(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_BookingItems(SqlCommand cmd, FlagType flag, DTO.Booking_Items Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@BookingID", Ob.BookingID));
            cmd.Parameters.Add(new SqlParameter("@ItemID", Ob.ItemID));
            cmd.Parameters.Add(new SqlParameter("@SubItemCount", Ob.SubItemCount));
            cmd.Parameters.Add(new SqlParameter("@ProcessCount", Ob.ProcessCount));
            cmd.Parameters.Add(new SqlParameter("@Sequence", Ob.Sequence));
            cmd.Parameters.Add(new SqlParameter("@CategoryID", Ob.CategoryID));
            cmd.Parameters.Add(new SqlParameter("@VariationID", Ob.VariationId));
            cmd.Parameters.Add(new SqlParameter("@Quantity", Ob.Quantity));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.CommonFields.BranchId));
            return cmd;
        }

        public DataSet GetDS(DTO.Booking_Items Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking_Items");
            cmd = AddParameters_BookingItems(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBooking(DTO.Booking_Items Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking_Items");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_BookingItems(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}