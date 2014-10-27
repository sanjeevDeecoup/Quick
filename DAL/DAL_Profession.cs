using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Profession
    {
        public string SaveProfession(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Profession";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Profession", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateProfession(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Profession";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Profession", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllProfession(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Profession";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchProfession(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Profession";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Profession", Ob.Input);
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

        public string findAllProfession(string BID)
        {
            string resultProfession = string.Empty;
            string resultProfessionCode = string.Empty;
            var result = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {               
                cmd.CommandText = "sp_Profession";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    resultProfession += sdr.GetValue(0).ToString().ToUpperInvariant() + ":";
                    resultProfessionCode += sdr.GetValue(1).ToString().ToUpperInvariant() + ":";
                }
                resultProfession = resultProfession.Substring(0, resultProfession.Length - 1);
                resultProfessionCode = resultProfessionCode.Substring(0, resultProfessionCode.Length - 1);
                result = resultProfession + "_" + resultProfessionCode;               
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

        public string SetButtonAccordingMenuRights(string BID, string PageName, string UID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            string statue = string.Empty;
            try
            {
                cmd.CommandText = "sp_Dry_DrawlMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@PageTitle", PageName);
                cmd.Parameters.AddWithValue("@UserTypeId", UID);
                cmd.Parameters.AddWithValue("@Flag", 37);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    statue = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return statue;
        }
    }
}