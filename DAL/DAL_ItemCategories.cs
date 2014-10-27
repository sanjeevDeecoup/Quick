using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_ItemCategories
    {
        public int SaveItemCategory(DTO.Item_Categories Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemCategories");
            FlagType flag;
            if (Ob.ItemCatID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_ItemCategory(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_ItemCategory(SqlCommand cmd, FlagType flag, DTO.Item_Categories Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@ItemCategoryID", Ob.ItemCategoryID));
            cmd.Parameters.Add(new SqlParameter("@ItemID", Ob.ItemID));
            cmd.Parameters.Add(new SqlParameter("@CategoryID", Ob.CategoryID));
            cmd.Parameters.Add(new SqlParameter("@DefaultSelected", Ob.DefaultSelected));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllItemCategory(DTO.Item_Categories Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemCategories");
            cmd = AddParameters_ItemCategory(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteItemCategory(DTO.Item_Categories Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemCategories");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_ItemCategory(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}