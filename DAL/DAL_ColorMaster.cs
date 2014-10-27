using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class DAL_ColorMaster
    {
        public string SaveColorMaster(DTO.ColorMaster Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ColorName", Ob.ColorName);
            cmd.Parameters.AddWithValue("@ColorImage", Ob.ImageName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@DateCreated", date[0].ToString());
            cmd.Parameters.AddWithValue("@DateModified", date[0].ToString());
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateColorMaster(DTO.ColorMaster Ob)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ColorName", Ob.ColorName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@ColorImage", Ob.ImageName);
            cmd.Parameters.AddWithValue("@ColorId", Ob.ColorID);
            cmd.Parameters.AddWithValue("@DateModified", date[0].ToString());
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGridView(DTO.ColorMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ColorName", Ob.ColorName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string deleteColorMaster(DTO.ColorMaster Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@ColorId", Ob.ColorID);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet CheckRemote(string BranchId)
        { 
            DataSet ds=new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(cmd);

            return ds;
        }
        public DataSet CheckBackupEmail(string BranchId)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@flag", 17);
            ds = PrjClass.GetData(cmd);

            return ds;
        }
        public bool CheckCloud(string BranchId)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ColorMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(cmd);
            bool active = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBackupActive"].ToString());
            return  active;
        }
        public string SaveLoginHistoryData(string BID, string LoginDate, string LoginTime, string Success, string ReasonID, string UserID)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_LoginHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LoginDate", LoginDate);
            cmd.Parameters.AddWithValue("@LoginTime",LoginTime);
            cmd.Parameters.AddWithValue("@Success", Success);
            cmd.Parameters.AddWithValue("@ReasonID", ReasonID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@BranchId",BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

    }
}