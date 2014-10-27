using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Shift
    {
        public string SaveShift(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ShiftMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShiftName", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "1");
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateShift(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ShiftMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShiftName", Ob.Input);
            cmd.Parameters.AddWithValue("@ShiftID", Ob.Id);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "2");
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllShift(DTO.Common Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ShiftMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "3");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchShift(DTO.Common Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ShiftMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ShiftName", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", "4");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetTaxDetails(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 9);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
    }
}