using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BranchChallanDetail
    {
        public int SaveBranchChallanDetail(DTO.BranchChallanDetail Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_BranchChallanDetail");
            FlagType flag;
            if (Ob.BranchDetailID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BranchChallanDetail(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_BranchChallanDetail(SqlCommand cmd, FlagType flag, DTO.BranchChallanDetail Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@BranchChallanDetailID", Ob.BranchChallanDetailID));
            cmd.Parameters.Add(new SqlParameter("@FactoryChallanHeaderID", Ob.BranchChallanHeaderId));
            cmd.Parameters.Add(new SqlParameter("@TrackingID", Ob.TrackingId));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllBranchChallanDetail(DTO.BranchChallanDetail Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_BranchChallanDetail");
            cmd = AddParameters_BranchChallanDetail(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBranchChallanDetail(DTO.BranchChallanDetail Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_BranchChallanDetail");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_BranchChallanDetail(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}