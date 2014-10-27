using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public enum FlagType
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        Select = 4
    }

    public static class SqlHelper
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }

        public static DataSet GetDS(SqlCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet retDS = new DataSet();
            da.Fill(retDS);
            return retDS;
        }

        public static SqlCommand CreateSqlCommand(string spName)
        {
            SqlCommand cmd = new SqlCommand(spName);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public static object ExecuteStoredProc(SqlCommand cmd)
        {
            object retVal = null;
            cmd.Connection = new SqlConnection(GetConnectionString());
            cmd.Connection.Open();
            int flag = 4; //Default Value
            if (cmd.Parameters.Contains("@Flag"))
            {
                flag = Convert.ToInt32(cmd.Parameters["@Flag"].Value);
            }
            switch (flag)
            {
                case (int)FlagType.Insert:
                    retVal = Convert.ToInt32(cmd.ExecuteScalar());
                    break;

                case (int)FlagType.Update:
                case (int)FlagType.Delete:
                    retVal = cmd.ExecuteNonQuery();
                    break;

                case (int)FlagType.Select:
                    retVal = GetDS(cmd);
                    break;

                default:
                    break;
            }
            cmd.Connection.Close();
            return retVal;
        }

        public static void ExecuteInsertQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = new SqlConnection(GetConnectionString());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}