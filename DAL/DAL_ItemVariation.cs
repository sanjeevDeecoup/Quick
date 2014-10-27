using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_ItemVariation
    {
        public int SaveItemVariation(DTO.Item_Variations Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemVariations");
            FlagType flag;
            if (Ob._ItemVarId > 0)
            {
                flag = FlagType.Update;
            }

            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_ItemVariation(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public SqlCommand AddParameters_ItemVariation(SqlCommand cmd, FlagType flag, DTO.Item_Variations Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@ItemVariationID", Ob._ItemVariationId));
            cmd.Parameters.Add(new SqlParameter("@ItemID", Ob.ItemID));
            cmd.Parameters.Add(new SqlParameter("@VariationID", Ob.VariationID));
            cmd.Parameters.Add(new SqlParameter("@DefaultSelected", Ob.DefaultSelected));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllItemVariations(DTO.Item_Variations Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemVariations");
            cmd = AddParameters_ItemVariation(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteItemVariation(DTO.Item_Variations Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_ItemVariations");
            cmd = AddParameters_ItemVariation(cmd, FlagType.Delete, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}