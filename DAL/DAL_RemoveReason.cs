using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System;

namespace DAL
{
    public class DAL_RemoveReason
    {
        public string SaveReason(DTO.Common Ob)
        {
            string res = "";
            if (CheckDuplicateReason(Ob) == false)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_RemoveReason";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RemoveReason", Ob.Input);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
                cmd.Parameters.AddWithValue("@Flag", 1);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            else
            {
                res = "Record allready exists.";
            }
            return res;
        }

        public string UpdateReason(DTO.Common Ob)
        {
            string res = "";
            if (Ob.Path != Ob.Input)
            {
                if (CheckDuplicateReason(Ob) == true)
                {
                    res = "Record allready exist.";
                    return res;
                }
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_RemoveReason";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RemoveReason", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }

        public DataSet ShowAllReason(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_RemoveReason";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchReason(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_RemoveReason";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@RemoveReason", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteReason(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_RemoveReason";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RemoveReason", Ob.Input);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        private bool status;

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

        public bool CheckCorrectRemoveReason(string BID, string Text)
        {
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.CommandText = "sp_Dry_BarcodeMaster";
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@RemoveReason", Text);
                CMD.Parameters.AddWithValue("@Flag", 32);
                sdr = PrjClass.ExecuteReader(CMD);
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
                CMD.Dispose();
            }
            return status;

        }

        public string DeleteReasonMain(DTO.Common Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_RemoveReason";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ID", Ob.Id);
            cmd.Parameters.AddWithValue("@Flag", 7);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        private bool CheckDuplicateReason(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
                cmd.Parameters.AddWithValue("@RemoveReason", Ob.Input);
                cmd.Parameters.AddWithValue("@Flag", 32);
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

        public bool CheckAcceptPaymentButtonAcess(string BID, string UserTypeId)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();           
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_Dry_BarcodeMaster";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@UserTypeId", UserTypeId);
            CMD.Parameters.AddWithValue("@Flag", 33);
            ds = PrjClass.GetData(CMD);
            if (ds.Tables[0].Rows[0]["RightToView"].ToString() == "True")
                status = true;
            else status = false;

            return status;
        }

        public string SaveTempIntoPaymentTable(string BookingNo, string DateTime, string Time, string BranchId)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewBooking_SaveProc";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNo", BookingNo);
            cmd.Parameters.AddWithValue("@DateTime", DateTime);
            cmd.Parameters.AddWithValue("@Time", Time);
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@Flag", 8);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string GetDateTime(string BID)
        {
            string res = string.Empty;           
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            res = date[1].ToString();
            return res;
        }
    }
}