using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_City
    {
        public string SaveCity(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@City", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateCity(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@City", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllCity(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchCity(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@City", Ob.Input);
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

        public string findAllCity(string BID)
        {
            string resultCity = string.Empty;
            string resultCityCode = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "sp_City";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    resultCity += sdr.GetValue(0).ToString().ToUpperInvariant() + ":";
                    resultCityCode += sdr.GetValue(1).ToString().ToUpperInvariant() + ":";
                }
                resultCity = resultCity.Substring(0, resultCity.Length - 1);
                resultCityCode = resultCityCode.Substring(0, resultCityCode.Length - 1);
                var result = resultCity + "_" + resultCityCode;
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

        public DataSet ShowAllExpense(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ExpenseEntryScreen";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteExpenses(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ExpenseEntryScreen";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet UpdateLedgerName(DTO.Common Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ExpenseEntryScreen";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ChangeName", Ob.ChangeName);
            cmd.Parameters.AddWithValue("@LedgerName", Ob.LedgerName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet UpdateIncomeLedgerName(DTO.Common Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ExpenseEntryScreen";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ChangeName", Ob.ChangeName);
            cmd.Parameters.AddWithValue("@LedgerName", Ob.LedgerName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 5);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet RegDatabase()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ExpenseEntryScreen";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet DeliveryDetail(string BID, string BNo)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_BookingDetailsForDelivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", BNo);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }


        public DataSet RedoGarment(string BID, string BNo)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_RedoGarment";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", BNo);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;

        }

        public string SaveRedo(string BID,string BNo,int rowindex,string ProcessCode,string duedate,int TotalQty)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_RedoGarment";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId",BID);
            cmd.Parameters.AddWithValue("@BookingNumber", BNo);
            cmd.Parameters.AddWithValue("@ProcessCode", ProcessCode);
            cmd.Parameters.AddWithValue("@ISN", rowindex);
            cmd.Parameters.AddWithValue("@duedate", duedate);
            cmd.Parameters.AddWithValue("@TotalQty", TotalQty);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public float getPriceAccordingProcess(string BID, string Process, string itemName, int rateListId)
        {
            float rate = 0;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_City";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Process);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@ItemName", itemName);
                cmd.Parameters.AddWithValue("@RateListId", rateListId);
                cmd.Parameters.AddWithValue("@Flag", 8);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    rate = float.Parse(sdr.GetValue(0).ToString());
                else
                    rate = 0;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return rate;
        }

        public string getCorrectProcess(string BID, string Process)
        {
            var res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_City";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Process);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 9);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = sdr.GetValue(0).ToString();
                else
                    res = "Invalid Process";
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

        public string GetDefaultProcessAndRateForRateList(string itemName, int rateListId, string branchId)
        {
            SqlDataReader sdr = null;
            var sqlCommand = new SqlCommand { CommandText = "sp_city", CommandType = CommandType.StoredProcedure };
            sqlCommand.Parameters.AddWithValue("@ItemName", itemName);
            sqlCommand.Parameters.AddWithValue("@RateListId", rateListId);
            sqlCommand.Parameters.AddWithValue("@branchId", branchId);
            sqlCommand.Parameters.AddWithValue("@flag", 13);
            var ProcessAndRate = string.Empty;
            using (sdr = PrjClass.ExecuteReader(sqlCommand))
            {
                while (sdr != null && sdr.Read())
                {
                    ProcessAndRate = sdr.GetValue(0).ToString();
                    ProcessAndRate += ":" + sdr.GetValue(1).ToString();
                }
            }
            sdr.Close();
            sdr.Dispose();
            sqlCommand.Dispose();
            return ProcessAndRate;
        }
    }
}