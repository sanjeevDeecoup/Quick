using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Variation
    {
        public string SaveVariation(DTO.Variation Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Variation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VariationName", Ob.VariationName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateVariation(DTO.Variation Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Variation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VariationId", Ob.VariationId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@VariationName", Ob.VariationName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@DateModified", Ob.DateModified);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGrid(DTO.Variation Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Variation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindGridView(DTO.Variation Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Variation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VariationName", Ob.VariationName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string Deletevariation(DTO.Variation Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "Sp_Variation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VariationId", Ob.VariationId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 6);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
    }
}