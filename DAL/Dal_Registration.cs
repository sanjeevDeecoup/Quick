using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Registration
    {
        bool status;
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

        public string SaveRegistration(string CustName, string CustAdd, string CustMob, string Email, string Area, string UserId, string UserPassword, string BranchId)
        {
            string res = string.Empty;




            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerName", CustName);
            cmd.Parameters.AddWithValue("@CustomerAddress", CustAdd);
            cmd.Parameters.AddWithValue("@CustomerMobile", CustMob);
            cmd.Parameters.AddWithValue("@CustomerEmailId", Email);
            cmd.Parameters.AddWithValue("@AreaLocation", Area);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@UserPassword", UserPassword);
            cmd.Parameters.AddWithValue("@UserName", CustName);
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);


            return res;
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
    }
}
