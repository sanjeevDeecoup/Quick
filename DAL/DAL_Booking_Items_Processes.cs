using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Booking_Items_Processes
    {
        public int SaveBookingItemsData(DTO.Booking_Items_Processes Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking_Items_Process");
            FlagType flag;
            if (Ob.Booking_ItemProcessID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItems_Processes(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_BookingItems_Processes(SqlCommand cmd, FlagType flag, DTO.Booking_Items_Processes Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@ProcessID", Ob.ProcessID));
            cmd.Parameters.Add(new SqlParameter("@ProcessRate", Ob.ProcessRate));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.CommonFields.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemID", Ob.BookingItemID));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemProcessID", Ob.Booking_ItemProcessID));
            return cmd;
        }

        public DataSet GetDS(DTO.Booking_Items_Processes Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking_Items_Process");
            cmd = AddParameters_BookingItems_Processes(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBooking(DTO.Booking_Items_Processes Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking_Items_Process");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_BookingItems_Processes(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}