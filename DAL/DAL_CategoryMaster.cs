using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_CategoryMaster
    {
        public string SaveCategoryMaster(DTO.Category Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Category";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryName", Ob.CategoryName);
            cmd.Parameters.AddWithValue("@CategoryImage", Ob.ImageName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@DateCreated", date[0].ToString());
            cmd.Parameters.AddWithValue("@DateModified", date[0].ToString());
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateCategoryMaster(DTO.Category Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Category";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryName", Ob.CategoryName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@CategoryImage", Ob.ImageName);
            cmd.Parameters.AddWithValue("@CategoryId", Ob.CategoryID);
            cmd.Parameters.AddWithValue("@DateModified", date[0].ToString());
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGridView(DTO.Category Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Category";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryName", Ob.CategoryName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowAll(DTO.Category Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Category";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteCategory(DTO.Category Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "sp_Category";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryId", Ob.CategoryID);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
    }
}