using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Employee
    {
        private DTO.Common Common = new DTO.Common();

        public string SaveEmployee(DTO.Employee Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeSalutation", Ob.Title);
            cmd.Parameters.AddWithValue("@EmployeeName", Ob.EmpName);
            cmd.Parameters.AddWithValue("@EmployeeAddress", Ob.Address);
            cmd.Parameters.AddWithValue("@EmployeePhone", Ob.PhoneNo);
            cmd.Parameters.AddWithValue("@EmployeeMobile", Ob.Mobile);
            cmd.Parameters.AddWithValue("@EmployeeEmailId", Ob.EmailId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public string UpdateEmployee(DTO.Employee Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeSalutation", Ob.Title);
            cmd.Parameters.AddWithValue("@EmployeeName", Ob.EmpName);
            cmd.Parameters.AddWithValue("@EmployeeAddress", Ob.Address);
            cmd.Parameters.AddWithValue("@EmployeePhone", Ob.PhoneNo);
            cmd.Parameters.AddWithValue("@EmployeeMobile", Ob.Mobile);
            cmd.Parameters.AddWithValue("@EmployeeEmailId", Ob.EmailId);
            cmd.Parameters.AddWithValue("@EmployeeCode", Ob.EmpCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 2);
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public bool RecordAllreadyExists(DTO.Employee Ob)
        {
            bool ReturnValue = true;
            SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
            sqlcon.Open();
            string sql = "Select Count(ID) From EmployeeMaster Where EmployeeName='" + Ob.EmpName + "' AND EmployeeAddress='" + Ob.Address + "' AND BranchId='" + Ob.BID + "'";
            SqlCommand cmd = new SqlCommand(sql, sqlcon);
            SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            sdr.Read();
            if ((int)sdr.GetValue(0) > 0)
                ReturnValue = true;
            else
                ReturnValue = false;
            sdr.Dispose();
            sdr.Close();
            cmd.Dispose();
            return ReturnValue;
        }

        public DataSet ShowAllCustomer(DTO.Employee Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet FillEmployee(DTO.Employee Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeCode", Ob.EmpCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 12);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchEmployee(DTO.Employee Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Status", Ob.EmpName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        private string CheckEmployee(DTO.Employee Ob)
        {
            Common.Result = "";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeCode", Ob.EmpCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 13);
            sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
                Common.Result = "true";
            sdr.Dispose();
            sdr.Close();
            cmd.Dispose();
            return Common.Result;
        }

        public string DeleteEmployee(DTO.Employee Ob)
        {
            if (CheckEmployee(Ob) != "true")
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_Dry_EmployeeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeCode", Ob.EmpCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
                cmd.Parameters.AddWithValue("@Flag", 5);
                Common.Result = PrjClass.ExecuteNonQuery(cmd);
            }
            else
            {
                Common.Result = "NO";
            }
            return Common.Result;
        }

        public DataSet GetTodayPending(string Date, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_DashBoardTodayPending";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingDate1", Date);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowCustomerBirthday(string Date, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_DefaultBirthDayCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Date);
            cmd.Parameters.AddWithValue("@MainDate", Date);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowCustomerAnniversary(string Date, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_DefaultAnniveraryCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Date);
            cmd.Parameters.AddWithValue("@MainDate", Date);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowHomeDelivery(string Date, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_DefaultHomeDeliveryShow";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Date);
            cmd.Parameters.AddWithValue("@BookDate2", Date);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowUrguntBooking(string Date, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_DefaultUrgentShow";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Date);
            cmd.Parameters.AddWithValue("@BookDate2", Date);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
    }
}