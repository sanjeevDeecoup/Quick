using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_NewPriceLists
    {
        public DataSet fetchpricelist(DTO.NewPriceLists Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_itempricelist";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet fetchpricelistcoloum(DTO.NewPriceLists Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_itempricelistcoloum";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet fetchpricelistcoloumcount(DTO.NewPriceLists Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_itempricelistcoloumcount";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet fetchpricelistcoloumvaluecount(DTO.NewPriceLists Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_itempricelistcoloumvaluecount";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet fetchpricelistcoloumvaluecountvalue(DTO.NewPriceLists Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_itempricelistcoloumcountvalue";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public string SaveNewItemprice(DTO.NewPriceLists Ob)
        {
            string Result = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_itempricelistinsertdata";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemCode", Ob.ItemCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@ItemSubItemRef", Ob.SubItemRefID);
            cmd.Parameters.AddWithValue("@ItemCategoryId", Ob.CategoryID);
            cmd.Parameters.AddWithValue("@ItemVariationId", Ob.VariationId);
            cmd.Parameters.AddWithValue("@Processid", Ob.Processid);
            cmd.Parameters.AddWithValue("@Price", Ob.Price);
            cmd.Parameters.AddWithValue("@DateCreated", Ob.DateCreated);
            cmd.Parameters.AddWithValue("@DateModified", Ob.DateModified);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@Flag", 1);
            Result = PrjClass.ExecuteNonQuery(cmd);
            cmd.Dispose();
            return Result;
        }

        public DataSet fetchpricevalue(DTO.NewPriceLists Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_itempricelistinsertdata";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@flag", 2);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
    }
}