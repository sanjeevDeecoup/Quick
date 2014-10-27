using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_ItemTracking
    {
        public int SaveItemTracking(DTO.Item_Tracking Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemTracking");
            FlagType flag;
            if (Ob.TrackId > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_ItemTracking(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public SqlCommand AddParameters_ItemTracking(SqlCommand cmd, FlagType flag, DTO.Item_Tracking Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@FLAG", flag));
            cmd.Parameters.Add(new SqlParameter("@TrackingID", Ob.TrackingId));
            cmd.Parameters.Add(new SqlParameter("@BookingID", Ob.BookingID));
            cmd.Parameters.Add(new SqlParameter("@ItemID", Ob.ItemID));
            cmd.Parameters.Add(new SqlParameter("@IsSubItem", Ob.IsSubItem));
            cmd.Parameters.Add(new SqlParameter("@SubItemID", Ob.SubItemID));
            cmd.Parameters.Add(new SqlParameter("@SubItemItemID", Ob.SubItemItemID));
            cmd.Parameters.Add(new SqlParameter("@TrackingStatus", Ob.TrackingStatus));
            cmd.Parameters.Add(new SqlParameter("@BarCode", Ob.BarCode));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.ModiFiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllItemTracking(DTO.Item_Tracking Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemTracking");
            cmd = AddParameters_ItemTracking(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteItemtracking(DTO.Item_Tracking Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemTracking");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_ItemTracking(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}