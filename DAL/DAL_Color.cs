using System.Data;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class DAL_Color
    {
        public string SaveColor(DTO.Common Ob)
        {
            string res = "";
            if (CheckDuplicateColor(Ob) == true)
            {
                res = "Record allready exist.";
                return res;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ColorName", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "1");
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateColor(DTO.Common Ob)
        {
            string res = "";
            if (Ob.Path != Ob.Input)
            {
                if (CheckDuplicateColor(Ob) == true)
                {
                    res = "Record allready exist.";
                    return res;
                }
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ColorName", Ob.Input);
            cmd.Parameters.AddWithValue("@ID ", Ob.Id);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "2");
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllColor(DTO.Common Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "3");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchColor(DTO.Common Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ColorName", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", "4");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteColor(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID ", Ob.Id);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", "5");
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        private bool CheckDuplicateColor(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_NewColorMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ColorName", Ob.Input);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
                cmd.Parameters.AddWithValue("@Flag", 6);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status = true;
                else
                    status = false;
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

        public DataSet BindDropDown()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@Flag", 2);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet GetStatus(string BID, string UserName, string Password)
        {

            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@UserName", UserName);
            CMD.Parameters.AddWithValue("@UserPassword", Password);
            CMD.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(CMD);
            return ds;

        }

        public string UpdatePassword(string BID, string UserName, string Password, string NewPassword)
        {
            string res = string.Empty;




            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@UserPassword", Password);
            cmd.Parameters.AddWithValue("@UserNewPassword", NewPassword);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);


            return res;
        }

        public DataSet FindCustomerName(string BID, string CustID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@CustomerCode", CustID);
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet BindGridCustomerSearch(string BID, string CustName)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_Sel_QuantityandBooking";
            cmd.Parameters.AddWithValue("@CustId", CustName);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 10);

            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindDeliveryStatus(string BookingNumber, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeliveryStatus";
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
            cmd.Parameters.AddWithValue("@BranchId", BID);


            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet GetStoreNameAddress(string BID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@Flag",9);
            CMD.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

    }
}