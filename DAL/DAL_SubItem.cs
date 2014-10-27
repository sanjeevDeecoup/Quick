using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_SubItem
    {
        public int SaveSubItem(DTO.Sub_Items Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_SubItems");
            FlagType flag;
            if (Ob.SubID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_SubItems(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_SubItems(SqlCommand cmd, FlagType flag, DTO.Sub_Items Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@SubItemID", Ob.SubItemID));
            cmd.Parameters.Add(new SqlParameter("@ItemID", Ob.ItemID));
            cmd.Parameters.Add(new SqlParameter("@SubItemRefID", Ob.SubItemRefID));
            cmd.Parameters.Add(new SqlParameter("@DefaultSelected", Ob.DefaultSelected));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllSubItems(DTO.Sub_Items Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_SubItems");
            cmd = AddParameters_SubItems(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteSubItems(DTO.Sub_Items Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_SubItems");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_SubItems(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}