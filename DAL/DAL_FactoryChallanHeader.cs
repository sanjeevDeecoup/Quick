using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_FactoryChallanHeader
    {
        public int SaveFactoryChallanHeader(DTO.FactoryChallanHeader Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_FactoryChallanHeader");
            FlagType flag;
            if (Ob.FactoryChallanID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_FactoryChallanHeader(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_FactoryChallanHeader(SqlCommand cmd, FlagType flag, DTO.FactoryChallanHeader Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@FactoryChallanHeaderID", Ob.FactoryChallanHeaderID));
            cmd.Parameters.Add(new SqlParameter("@FactoryChallanNumber", Ob.FactoryChallanNumber));
            cmd.Parameters.Add(new SqlParameter("@TotalItemCount", Ob.TotalItemCount));
            cmd.Parameters.Add(new SqlParameter("@OutDatetime", Ob.OutDateTime));
            cmd.Parameters.Add(new SqlParameter("@InDatetime", Ob.InDateTime));
            cmd.Parameters.Add(new SqlParameter("@FactoryChallanStatus", Ob.FactoryChallanStatus));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModifIed));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllFactoryChallanHeader(DTO.FactoryChallanHeader Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_FactoryChallanHeader");
            cmd = AddParameters_FactoryChallanHeader(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteFactoryChallanHeader(DTO.FactoryChallanHeader Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_FactoryChallanHeader");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_FactoryChallanHeader(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}