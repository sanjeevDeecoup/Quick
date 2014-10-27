using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BookingItem_SubItems
    {
        public int SaveBookingItem_SubItems(DTO.Booking_Items_SubItems Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItem_SubItems");
            FlagType flag;
            if (Ob.Booking_ItemSubItemID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BookingItem_SubItems(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_BookingItem_SubItems(SqlCommand cmd, FlagType flag, DTO.Booking_Items_SubItems Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@FLAG", flag));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemSubItemID", Ob.Booking_ItemSubItemID));
            cmd.Parameters.Add(new SqlParameter("@SubItemID", Ob.SubItemID));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@Booking_ItemID", Ob.BookingItemID));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.CommonFields.BranchId));
            return cmd;
        }

        public DataSet GetAllBookingItems_SubItems(DTO.Booking_Items_SubItems Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItem_SubItems");
            cmd = AddParameters_BookingItem_SubItems(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBookingItems_SubItems(DTO.Booking_Items_SubItems Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Proc_BookingItem_SubItems");
            cmd = AddParameters_BookingItem_SubItems(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}