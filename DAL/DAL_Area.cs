using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Area
    {
        public string SaveArea(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Area";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Area", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateArea(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Area";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Area", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllArea(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Area";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchArea(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Area";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Area", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet FillWebsiteCustomerTextBoxes(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet SetDataInvoiceWise(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.Input);
            cmd.Parameters.AddWithValue("@BookingNumber", Ob.Result);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet SetData(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@From", Ob.Path);
            cmd.Parameters.AddWithValue("@Todate", Ob.LedgerName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.Input);
            cmd.Parameters.AddWithValue("@BookingNumber", Ob.Result);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string CheckBlanckEntries(string Labelname, string rate, string DayOffset)
        {
            string res = string.Empty;
            if (rate == "")
            {
                rate = "0";
            }
            if (DayOffset == "")
            {
                DayOffset = "0";
            }
            if (Labelname != "" && rate != "0" && DayOffset != "0")
            {
                res = "Done";
                return res;
            }
            if (Labelname == "" && rate == "0" && DayOffset == "0")
            {
                res = "Done";
                return res;
            }
            if (Labelname == "")
            {
                res = "label name";
                return res;
            }
            if (rate == "")
            {
                res = "rate";
                return res;
            }
            if (DayOffset == "")
            {
                res = "day off set";
                return res;
            }
            return res;
        }

        public string findAllArea(string BID)
        {
            string resultArea = string.Empty;
            string resultAreaCode = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {               
                cmd.CommandText = "sp_Area";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    resultArea += sdr.GetValue(0).ToString().ToUpperInvariant() + ":";
                    resultAreaCode += sdr.GetValue(1).ToString().ToUpperInvariant() + ":";
                }
                resultArea = resultArea.Substring(0, resultArea.Length - 1);
                resultAreaCode = resultAreaCode.Substring(0, resultAreaCode.Length - 1);
                var result = resultArea + "_" + resultAreaCode;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
        }

        public string DeleteBooking(string BranchId, string BookingNo)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_DeleteBooking";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@BookingNo", BookingNo);
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateCustomerDetailWebsite(DTO.Common Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CommunicationMeans", Ob.LedgerName);
            cmd.Parameters.AddWithValue("@CustomerMobile", Ob.Path);
            cmd.Parameters.AddWithValue("@CustomerPhone", Ob.Result);
            cmd.Parameters.AddWithValue("@CustomerEmailId", Ob.UserId);
            cmd.Parameters.AddWithValue("@BirthDate", Ob.Id);
            cmd.Parameters.AddWithValue("@AnniversaryDate", Ob.ChangeName);
            cmd.Parameters.AddWithValue("@Branchid", Ob.BID);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 8);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet GetGridData(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@From", Ob.Path);
            cmd.Parameters.AddWithValue("@Todate", Ob.LedgerName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.Input);
            cmd.Parameters.AddWithValue("@BookingNumber", Ob.Result);
            cmd.Parameters.AddWithValue("@Flag", 15);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet GetCustomerMobileno(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);          
            cmd.Parameters.AddWithValue("@Flag", 17);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet GetuserName(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_UserMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 13);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
    }
}