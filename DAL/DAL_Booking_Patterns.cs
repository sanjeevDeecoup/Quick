using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Booking_Patterns
    {
        public int SaveBookingItemPatterns(DTO.Booking_Items_Patterns Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemPatterns");
            FlagType flag;
            if (Ob.Booking_ItemPatternID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItemPattern(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public SqlCommand AddParameters_BookingItemPattern(SqlCommand cmd, FlagType flag, DTO.Booking_Items_Patterns Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@FLAG", flag));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemPatternID", Ob.Booking_ItemPatternID));
            cmd.Parameters.Add(new SqlParameter("@PatternID", Ob.PatternID));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemID", Ob.BookingItemID));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.CommonFields.BranchId));
            return cmd;
        }

        public DataSet GetAllBookingItemPatterns(DTO.Booking_Items_Patterns Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemPatterns");
            cmd = AddParameters_BookingItemPattern(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBookingItemPatterns(DTO.Booking_Items_Patterns Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemPatterns");
            cmd = AddParameters_BookingItemPattern(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}