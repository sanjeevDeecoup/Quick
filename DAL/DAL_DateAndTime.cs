using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_DateAndTime
    {
        public ArrayList getDateAndTimeAccordingToZoneTime(string BID)
        {
            string zoneTime = "";
            ArrayList DateAndTime = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {               
                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 2);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    zoneTime = "" + sdr.GetValue(54);
                    TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(zoneTime);
                    DateTime tstTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, tst);
                    DateAndTime.Add(tstTime.Date.ToString("dd MMM yyyy"));
                    DateAndTime.Add(tstTime.ToLongTimeString());
                }
            }
            catch (Exception)
            {
                zoneTime = "India Standard Time";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return DateAndTime;
        }

        public bool CheckBookingNumber(string BookingNumber, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_Dry_DrawlMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 33);
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

        public bool CheckBookingNumberInFactory(string BookingNumber, string ExternalBID)
        {          
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
            cmd.Parameters.AddWithValue("@BranchId", ExternalBID);
            cmd.Parameters.AddWithValue("@Flag", 36);
            var sdr = PrjClass.ExecuteScalar(cmd);
            if (Int32.Parse(sdr) > 0)
                return true;
            else
                return false;
        }
    }
}