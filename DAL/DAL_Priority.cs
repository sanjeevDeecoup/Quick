using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Priority
    {
        public string SavePriority(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Priority";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Priority", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdatePriority(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Priority";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Priority", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllPriority(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Priority";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchPriority(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Priority";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Priority", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 4);
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

        public string findAllPriority(string BID)
        {
            string resultPriority = string.Empty;
            string resultPriorityCode = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            string result = string.Empty;
            try
            {
                cmd.CommandText = "sp_Priority";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    resultPriority += sdr.GetValue(0).ToString().ToUpperInvariant() + ":";
                    resultPriorityCode += sdr.GetValue(1).ToString().ToUpperInvariant() + ":";
                }
                resultPriority = resultPriority.Substring(0, resultPriority.Length - 1);
                resultPriorityCode = resultPriorityCode.Substring(0, resultPriorityCode.Length - 1);
                result = resultPriority + "_" + resultPriorityCode;
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
            return result;
        }
    }
}