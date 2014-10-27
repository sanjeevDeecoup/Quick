using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DAL
{
    public class DAL_FactoryChallanDetail
    {
        public int SaveFactoryChallanDetail(DTO.FactoryChallanDetail Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_FactoryChallanDetail");
            FlagType flag;
            if (Ob.FactoryDetailID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_FactoryChallanDetail(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_FactoryChallanDetail(SqlCommand cmd, FlagType flag, DTO.FactoryChallanDetail Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@FactoryChallanDetailID", Ob.FactoryChallanDetailID));
            cmd.Parameters.Add(new SqlParameter("@FactoryChallanHeaderID", Ob.FactoryChallanHeaderID));
            cmd.Parameters.Add(new SqlParameter("@TrackingID", Ob.TrackingID));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchID", Ob.BranchID));
            return cmd;
        }

        public DataSet GetAllFactoryChallanDetail(DTO.FactoryChallanDetail Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_FactoryChallanDetail");
            cmd = AddParameters_FactoryChallanDetail(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteFactoryChallanDetail(DTO.FactoryChallanDetail Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_FactoryChallanDetail");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_FactoryChallanDetail(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public void SaveTempRecord(GridView grdNewChallan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ChallanInProc";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 10);
            PrjClass.ExecuteNonQuery(cmd);
            string res = string.Empty;
            for (int r = 0; r < grdNewChallan.Rows.Count; r++)
            {
                if (((CheckBox)grdNewChallan.Rows[r].FindControl("chkSelect")).Checked)
                {
                    string[] gridbooking = ((Label)grdNewChallan.Rows[r].FindControl("lblBarcode")).Text.Split('-');
                    string gridBranchId = gridbooking[2].Replace("*", "").Trim();
                    string duedate = grdNewChallan.Rows[r].Cells[2].Text;

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "sp_ChallanInProc";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@Branchid", gridBranchId);
                    cmd1.Parameters.AddWithValue("@duedateTime", duedate);
                    cmd1.Parameters.AddWithValue("@Flag", 11);
                    res = PrjClass.ExecuteNonQuery(cmd1);
                }
            }
        }
    }
}