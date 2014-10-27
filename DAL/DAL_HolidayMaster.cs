using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_HolidayMaster
    {
        public string SaveHolidayMaster(DTO.Holiday Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_holiday";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@weeklyoff", Ob.weeklyoff);
            cmd.Parameters.AddWithValue("@description", Ob.description);
            cmd.Parameters.AddWithValue("@date", Ob.date);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateHolidayMaster(DTO.Holiday Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_holiday";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@description", Ob.description);
            cmd.Parameters.AddWithValue("@date", Ob.date);
            cmd.Parameters.AddWithValue("@holidayid", Ob.holidayid);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGridView(DTO.Holiday Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_holiday";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@description", Ob.description);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string deleteHolidayMaster(DTO.Holiday Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_holiday";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@holidayid", Ob.holidayid);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateWeekend(DTO.Holiday Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_holiday";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@weeklyoff", Ob.weeklyoff);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
    }
}