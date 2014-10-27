using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class DAL_UserMaster
    {
        public string SaveUser(DTO.UserMaster Ob)
        {
            string res = "";
            if (CheckDuplicateUserPin(Ob) == true)
            {
                res = "User pin is already in Use, Please enter new Pin.";
                return res;
            }
            if (CheckDuplicateUserBarcode(Ob) == true)
            {
                res = "User Barcode is already in use, Please change the barcode.";
                return res;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", Ob.UserId);
            cmd.Parameters.AddWithValue("@UserPassword", Ob.UserPassword);
            cmd.Parameters.AddWithValue("@UserTypeCode", Ob.UserTypeCode);
            cmd.Parameters.AddWithValue("@UserBranchCode", Ob.UserBranchCode);
            cmd.Parameters.AddWithValue("@UserName", Ob.UserName);
            cmd.Parameters.AddWithValue("@UserAddress", Ob.UserAddress);
            cmd.Parameters.AddWithValue("@UserPhoneNumber", Ob.UserPhoneNumber);
            cmd.Parameters.AddWithValue("@UserMobileNumber", Ob.UserMobileNumber);
            cmd.Parameters.AddWithValue("@UserEmailId", Ob.UserEmailId);
            cmd.Parameters.AddWithValue("@UserActive", Ob.UserActive);
            cmd.Parameters.AddWithValue("@Userbarcode", Ob.UserBarcode);
            cmd.Parameters.AddWithValue("@UserPin", Ob.UserPin);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateUser(DTO.UserMaster Ob)
        {
            string res = "";
            if (Ob.PreUserPin != Ob.UserPin)
            {
                if (CheckDuplicateUserPin(Ob) == true)
                {
                    res = "User pin is already in Use, Please enter new Pin.";
                    return res;
                }
            }
            if (Ob.PreUserBarcode != Ob.UserBarcode)
            {
                if (CheckDuplicateUserBarcode(Ob) == true)
                {
                    res = "User Barcode is already in use, Please change the barcode.";
                    return res;
                }
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", Ob.UserId);
            cmd.Parameters.AddWithValue("@UserPassword", Ob.UserPassword);
            cmd.Parameters.AddWithValue("@UserTypeCode", Ob.UserTypeCode);
            cmd.Parameters.AddWithValue("@UserBranchCode", Ob.UserBranchCode);
            cmd.Parameters.AddWithValue("@UserName", Ob.UserName);
            cmd.Parameters.AddWithValue("@UserAddress", Ob.UserAddress);
            cmd.Parameters.AddWithValue("@UserPhoneNumber", Ob.UserPhoneNumber);
            cmd.Parameters.AddWithValue("@UserMobileNumber", Ob.UserMobileNumber);
            cmd.Parameters.AddWithValue("@UserEmailId", Ob.UserEmailId);
            cmd.Parameters.AddWithValue("@UserActive", Ob.UserActive);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Updatepassword", Ob.Updatepassword);
            cmd.Parameters.AddWithValue("@Userbarcode", Ob.UserBarcode);
            cmd.Parameters.AddWithValue("@UserPin", Ob.UserPin);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGrid(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchAndShowAll(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", Ob.UserName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet FillTextBoxes(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", Ob.UserId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        private bool CheckDuplicateUserPin(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_UserMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserPin", Ob.UserPin);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 7);
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

        private bool CheckDuplicateUserBarcode(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_UserMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Userbarcode", Ob.UserBarcode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
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

        public string[] GetAllUsers(string branchId)
        {
            var sqlCommand = new SqlCommand
                            {
                                CommandText = "sp_UserMaster",
                                CommandType = CommandType.StoredProcedure
                            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 8);
            // return PrjClass.GetData(sqlCommand);
            var array = new List<string>();
            var sqlDataReader = PrjClass.ExecuteReader(sqlCommand);
            while (sqlDataReader != null && sqlDataReader.Read())
            {
                array.Add(sqlDataReader.GetString(0));
            }
            if (sqlDataReader != null)
                sqlDataReader.Close();
            return array.ToArray();
        }

        public string SaveFactoryUser(DTO.UserMaster Ob)
        {
            string res = "";
            if (CheckDuplicateUserPin(Ob) == true)
            {
                res = "User pin is already in Use, Please enter new Pin.";
                return res;
            }
            if (CheckDuplicateUserBarcode(Ob) == true)
            {
                res = "User Barcode is already in use, Please change the barcode.";
                return res;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", Ob.UserId);
            cmd.Parameters.AddWithValue("@UserPassword", Ob.UserPassword);
            cmd.Parameters.AddWithValue("@UserTypeCode", Ob.UserTypeCode);
            cmd.Parameters.AddWithValue("@UserBranchCode", Ob.UserBranchCode);
            cmd.Parameters.AddWithValue("@UserName", Ob.UserName);
            cmd.Parameters.AddWithValue("@UserAddress", Ob.UserAddress);
            cmd.Parameters.AddWithValue("@UserPhoneNumber", Ob.UserPhoneNumber);
            cmd.Parameters.AddWithValue("@UserMobileNumber", Ob.UserMobileNumber);
            cmd.Parameters.AddWithValue("@UserEmailId", Ob.UserEmailId);
            cmd.Parameters.AddWithValue("@UserActive", Ob.UserActive);
            cmd.Parameters.AddWithValue("@Userbarcode", Ob.UserBarcode);
            cmd.Parameters.AddWithValue("@UserPin", Ob.UserPin);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 9);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateWorkshopUser(DTO.UserMaster Ob)
        {
            string res = "";
            if (Ob.PreUserPin != Ob.UserPin)
            {
                if (CheckDuplicateUserPin(Ob) == true)
                {
                    res = "User pin is already in Use, Please enter new Pin.";
                    return res;
                }
            }
            if (Ob.PreUserBarcode != Ob.UserBarcode)
            {
                if (CheckDuplicateUserBarcode(Ob) == true)
                {
                    res = "User Barcode is already in use, Please change the barcode.";
                    return res;
                }
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", Ob.UserId);
            cmd.Parameters.AddWithValue("@UserPassword", Ob.UserPassword);
            cmd.Parameters.AddWithValue("@UserTypeCode", Ob.UserTypeCode);
            cmd.Parameters.AddWithValue("@UserBranchCode", Ob.UserBranchCode);
            cmd.Parameters.AddWithValue("@UserName", Ob.UserName);
            cmd.Parameters.AddWithValue("@UserAddress", Ob.UserAddress);
            cmd.Parameters.AddWithValue("@UserPhoneNumber", Ob.UserPhoneNumber);
            cmd.Parameters.AddWithValue("@UserMobileNumber", Ob.UserMobileNumber);
            cmd.Parameters.AddWithValue("@UserEmailId", Ob.UserEmailId);
            cmd.Parameters.AddWithValue("@UserActive", Ob.UserActive);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Updatepassword", Ob.Updatepassword);
            cmd.Parameters.AddWithValue("@Userbarcode", Ob.UserBarcode);
            cmd.Parameters.AddWithValue("@UserPin", Ob.UserPin);
            cmd.Parameters.AddWithValue("@Flag", 10);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindWorkShopUserGrid(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet WorkshopSearchAndShowAll(DTO.UserMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", Ob.UserName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 12);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
    }
}