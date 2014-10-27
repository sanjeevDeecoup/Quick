using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CustomerDAO
    {
        public DataSet GetAllCustomers(int BranchID)
        {
            SqlCommand com = SqlHelper.CreateSqlCommand("usp_GetAllCustomerDetails");
            com.Parameters.Add(new SqlParameter("@BranchID", BranchID));
            return (DataSet)SqlHelper.ExecuteStoredProc(com);
        }
    }
}