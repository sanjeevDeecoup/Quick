using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System;

namespace DAL
{
    public class DAL_BarcodeLable
    {
        public string SaveBarcodeLable(DTO.BacodeLable Ob)
        {
            string res = "";
            SqlCommand CMD = new SqlCommand();
            CMD.CommandText = "Proc_BarCodeLabels";
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            CMD.Parameters.AddWithValue("@BookingNo", Ob.BNO);
            CMD.Parameters.AddWithValue("@RowIndex", Ob.RowIndex);
            CMD.Parameters.AddWithValue("@Active", Ob.Active);
            CMD.Parameters.AddWithValue("@Flag", 3);
            res = PrjClass.ExecuteNonQuery(CMD);
            return res;
        }

        public string savedemo(DTO.BacodeLable Ob)
        {
            string res = "";
            SqlCommand CMD = new SqlCommand();
            CMD.CommandText = "Proc_BarCodeLabels";
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.Parameters.AddWithValue("@BookingNo", Ob.BNO);
            CMD.Parameters.AddWithValue("@RowIndex", Ob.RowIndex);
            CMD.Parameters.AddWithValue("@Flag", 9);
            res = PrjClass.ExecuteNonQuery(CMD);
            return res;
        }

        public string delbarcodelable()
        {
            string res = "";
            SqlCommand CMD = new SqlCommand();
            CMD.CommandText = "Proc_BarCodeLabels";
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.Parameters.AddWithValue("@Flag", 8);
            res = PrjClass.ExecuteNonQuery(CMD);
            return res;
        }

        public string UpdateBarcodeLabel(DTO.BacodeLable ob)
        {
            string res = "";
            SqlCommand CMD = new SqlCommand();
            CMD.CommandText = "Proc_BarCodeLabels";
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.Parameters.AddWithValue("@BookingNo", ob.BNO);
            CMD.Parameters.AddWithValue("@RowIndex", ob.RowIndex);
            CMD.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(CMD);
            return res;
        }

        public DataSet SearchBarcodeLabel(DTO.BacodeLable ob)
        {
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandText = "Proc_BarCodeLabels";
            CMD.CommandType = CommandType.StoredProcedure;
            //CMD.Parameters.AddWithValue("@BookingNo", ob.BNO);
            //CMD.Parameters.AddWithValue("@RowIndex", ob.RowIndex);
            CMD.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public bool CheckRecord()
        {
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                CMD.CommandText = "Proc_BarCodeLabels";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@Flag", 13);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    status= true;
                else
                    status= false;
            }
            catch (Exception ex)
            { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return status;
        }

        public bool CheckAllCheckBoxesInGrid(GridView grd)
        {
            bool status = false;
            for (int r = 0; r < grd.Rows.Count; r++)
            {
                if (((CheckBox)grd.Rows[r].FindControl("chkSelect")).Checked)
                    status = true;
                else
                {
                    status = false;
                    break;
                }
            }
            return status;
        }
    }
}