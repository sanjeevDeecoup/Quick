using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BookingItemComment
    {
        public int SaveBookingItemComments(DTO.Booking_Items_Comments Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemComments");
            FlagType flag;
            if (Ob.Booking_ItemCommentID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItemComments(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public SqlCommand AddParameters_BookingItemComments(SqlCommand cmd, FlagType flag, DTO.Booking_Items_Comments Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@FLAG", flag));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemCommentID", Ob.Booking_ItemCommentID));
            cmd.Parameters.Add(new SqlParameter("@CommentID", Ob.CommentID));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemID", Ob.BookingItemID));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.CommonFields.BranchId));
            return cmd;
        }

        public DataSet GetAllBookingItemComments(DTO.Booking_Items_Comments Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemComments");
            cmd = AddParameters_BookingItemComments(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBookingItemComments(DTO.Booking_Items_Comments Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItemComments");
            cmd = AddParameters_BookingItemComments(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}