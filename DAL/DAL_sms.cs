using System.Data;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class DAL_sms
    {
        public string Savesmsconfig(DTO.sms Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@template", Ob.Template);
            cmd.Parameters.AddWithValue("@massage", Ob.Massage);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@MsgScreen", Ob.DefaultMsg);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string Updatesmsconfigr(DTO.sms Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@template", Ob.Template);
            cmd.Parameters.AddWithValue("@massage", Ob.Massage);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@SmsId", Ob.SmsId);
            cmd.Parameters.AddWithValue("@DateModified", Ob.DateModified);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@MsgScreen", Ob.DefaultMsg);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGridView(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@template", Ob.Template);
            cmd.Parameters.AddWithValue("@massage", Ob.Massage);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowAll(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string Deletesms(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SmsId", Ob.SmsId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string apiupdate(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userid", Ob.userid);
            cmd.Parameters.AddWithValue("@password", Ob.password);
            cmd.Parameters.AddWithValue("@api", Ob.api);
            cmd.Parameters.AddWithValue("@senderid", Ob.senderid);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@massagedemo", Ob.massagedemo);
            cmd.Parameters.AddWithValue("@mobiledemo", Ob.mobiledemo);
            cmd.Parameters.AddWithValue("@senderdemo", Ob.senderdemo);
            cmd.Parameters.AddWithValue("@useriddemo", Ob.useriddemo);
            cmd.Parameters.AddWithValue("@passworddemo", Ob.passworddemo);
            cmd.Parameters.AddWithValue("@senderposition", Ob.senderposition);
            cmd.Parameters.AddWithValue("@userposition", Ob.userposition);
            cmd.Parameters.AddWithValue("@passwordposition", Ob.passwordposition);
            cmd.Parameters.AddWithValue("@mobileposition", Ob.mobileposition);
            cmd.Parameters.AddWithValue("@massageposition", Ob.massageposition);

            cmd.Parameters.AddWithValue("@Flag", 6);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet fetchapi(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string defaultsmsupdate(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookingsms", Ob.bookingsms);
            cmd.Parameters.AddWithValue("@clothsms", Ob.clothsms);
            cmd.Parameters.AddWithValue("@deliverysms", Ob.deliverysms);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 8);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string GetsmsTemplateName(string BID, string smsId)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "SP_smsconfig";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@smsId", smsId);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 17);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = sdr.GetValue(0).ToString();
                else
                    res = "";
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

        public DataSet fetchdropbooking(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 9);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet fetchdropcloth(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet fetchdropdelivery(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet previewbooking(DTO.sms Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "SP_smsconfig";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookingsms", Ob.bookingsms);
            cmd.Parameters.AddWithValue("@clothsms", Ob.clothsms);
            cmd.Parameters.AddWithValue("@deliverysms", Ob.deliverysms);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 12);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ReadyClothScreenSms(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_ChallanScreenSms";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public bool SetSMSCheckBoxOnScreen(string BID, string Flag)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            string status = string.Empty;
            bool returnvalue = false;
            try
            {
                cmd.CommandText = "SP_smsconfig";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", Flag);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    status = sdr.GetValue(0).ToString();
                }
                else
                {
                    status = "FALSE";
                }
                if (status == "TRUE")
                    returnvalue = true;
                else
                    returnvalue = false;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return returnvalue;
        }

        public bool CheckReadyClothSendSms(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "Sp_Sel_ChallanScreenSms";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
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

        public bool CheckValidBookingNo(string BID, string BookingNo)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "Sp_holiday";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
                cmd.Parameters.AddWithValue("@BranchId", BID);
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

        public bool CheckDeliverSlipViewRight(string BID, string BookingNo, string UserTypeId)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            DataSet ds = new DataSet();
            string status = string.Empty;
            cmd.CommandText = "Sp_holiday";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
            cmd.Parameters.AddWithValue("@UserTypeId", UserTypeId);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count <= 1)
            {
                res = ds.Tables[0].Rows[0]["Status"].ToString();
                if (res == "TRUE")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool CheckDeliverSMSStatus(string BID, string BookingNo)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            DataSet ds = new DataSet();
            string status = string.Empty;
            cmd.CommandText = "Sp_Sel_DeliveryScreenSmsStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count <= 1)
            {
                res = ds.Tables[0].Rows[0]["Status"].ToString();
                if (res == "TRUE")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}