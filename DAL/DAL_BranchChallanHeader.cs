using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BranchChallanHeader
    {
        public int SaveBranchChallanHeader(DTO.BranchChallanHeader Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_BranchChallanHeader");
            FlagType flag;
            if (Ob.ChallanHeaderId > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_BranchChallanHeader(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_BranchChallanHeader(SqlCommand cmd, FlagType flag, DTO.BranchChallanHeader Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@BranchChallanHeaderID", Ob.BranchChallanHeaderId));
            cmd.Parameters.Add(new SqlParameter("@BookingChallanNumber", Ob.BookingChallanNumber));
            cmd.Parameters.Add(new SqlParameter("@TotalItemCount", Ob.TotalItemCount));
            cmd.Parameters.Add(new SqlParameter("@OutDatetime", Ob.OutDateTime));
            cmd.Parameters.Add(new SqlParameter("@InDatetime", Ob.InDateTime));
            cmd.Parameters.Add(new SqlParameter("@BranchChallanStatus", Ob.BranchChallanStatus));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllBranchChallanHeader(DTO.BranchChallanHeader Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_BranchChallanHeader");
            cmd = AddParameters_BranchChallanHeader(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBranchChallanHeader(DTO.BranchChallanHeader Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_BranchChallanHeader");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_BranchChallanHeader(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}