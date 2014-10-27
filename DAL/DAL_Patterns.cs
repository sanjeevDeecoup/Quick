using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Patterns
    {
        public string SavePatternMaster(DTO.Patterns Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PatternsMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatternName", Ob.PatternName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@PatternImage", Ob.ImageName);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdatePatternMaster(DTO.Patterns Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PatternsMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatternName", Ob.PatternName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@PatternId", Ob.PatternID);
            cmd.Parameters.AddWithValue("@DateModified", Ob.DateModified);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@PatternImage", Ob.ImageName);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGridView(DTO.Patterns Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_PatternsMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatternName", Ob.PatternName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowAll(DTO.Patterns Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_PatternsMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeletePatterns(DTO.Patterns Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "sp_PatternsMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatternId", Ob.PatternID);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
    }
}