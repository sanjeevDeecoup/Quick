using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CustomerDAO
    {
        public DataSet GetAllCustomers()
        {
            SqlCommand com = new SqlCommand("usp_GetAllCustomerDetails");
            return (DataSet) SqlHelper.ExecuteStoredProc(com);
        }
        
    }
}
