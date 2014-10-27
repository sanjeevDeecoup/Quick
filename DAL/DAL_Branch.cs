using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Branch
    {
        public string SaveBranch(DTO.BranchMaster Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchCode", Ob.BranchCode);
            cmd.Parameters.AddWithValue("@BranchName", Ob.BranchName);
            cmd.Parameters.AddWithValue("@BranchAddress", Ob.BranchAddress);
            cmd.Parameters.AddWithValue("@BranchPhone", Ob.BranchPhone);
            cmd.Parameters.AddWithValue("@BranchSlogan", Ob.BranchSlogan);
            cmd.Parameters.AddWithValue("@IsFactory", Ob.IsBF);
            cmd.Parameters.AddWithValue("@IsChallan", Ob.IsChallan);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@BranchMobile", Ob.BranchMobile);
            cmd.Parameters.AddWithValue("@BranchEmail", Ob.BranchEmail);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateBranch(DTO.BranchMaster Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchCode", Ob.BranchCode);
            cmd.Parameters.AddWithValue("@BranchName", Ob.BranchName);
            cmd.Parameters.AddWithValue("@BranchAddress", Ob.BranchAddress);
            cmd.Parameters.AddWithValue("@BranchPhone", Ob.BranchPhone);
            cmd.Parameters.AddWithValue("@BranchSlogan", Ob.BranchSlogan);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@IsFactory", Ob.IsBF);
            cmd.Parameters.AddWithValue("@IsChallan", Ob.IsChallan);
            cmd.Parameters.AddWithValue("@BranchMobile", Ob.BranchMobile);
            cmd.Parameters.AddWithValue("@BranchEmail", Ob.BranchEmail);
            cmd.Parameters.AddWithValue("@BusinessName", Ob.BusinessName);
            cmd.Parameters.AddWithValue("@IsLoginTime", Ob.IsLoginTime);
            cmd.Parameters.AddWithValue("@LoginStartTime", Ob.LoginStartTime);
            cmd.Parameters.AddWithValue("@LoginEndTime", Ob.LoginEndTime);
            cmd.Parameters.AddWithValue("@WeeklyOFF", Ob.WeeklyOFF);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGrid(DTO.BranchMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet FillTextBoxes(DTO.BranchMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        private bool CheckRecord(DTO.BranchMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_BranchMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 5);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status= true;
                else
                    status= false;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }

        public string DeleteBranch(DTO.BranchMaster Ob)
        {
            string res = "";
            if (CheckRecord(Ob) == false)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_BranchMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 6);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            else
            {
                res = "Branch use in bookings.";
                return res;
            }
            return res;
        }

        public DataSet ShowBranch()
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string GetWorkShopName(string BID, string ChallanNo)
        {
            string res = "";
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "sp_ChallanInProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@ChallanNumber", ChallanNo);
                cmd.Parameters.AddWithValue("@Flag", 19);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    res = sdr.GetValue(0).ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        public DataSet ShowBranch(int BranchId)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 8);
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public void CurrentUser(String UserName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_CurrentUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", UserName);

            PrjClass.ExecuteNonQuery(cmd);
        }
    }
}