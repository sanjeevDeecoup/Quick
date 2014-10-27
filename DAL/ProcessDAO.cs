using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ProcessDAO
    {
        public DataSet GetAllProcesses(int BranchID)
        {
            SqlCommand com = SqlHelper.CreateSqlCommand("usp_GetAllProcesses");
            com.Parameters.Add(new SqlParameter("@BranchID", BranchID));
            return (DataSet)SqlHelper.ExecuteStoredProc(com);
        }
    }
}