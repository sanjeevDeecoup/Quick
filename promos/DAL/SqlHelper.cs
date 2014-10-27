using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Xml;

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
        public static void LoadAllSchemeTemplates(string xmlFilesPath)
        {
            SqlCommand cmd = new SqlCommand("truncate table SchemeTemplates", new SqlConnection(GetConnectionString()));
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            //Get a list of all xml files in the current path
            string[] files = Directory.GetFiles(xmlFilesPath, @"*.xml");
            foreach (string file in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                //Get Scheme Name
                string schemeName = doc.SelectNodes("//schemeName")[0].InnerText;
                cmd.CommandText = "Insert into SchemeTemplates (SchemeName, Active) values('" + schemeName + "', 1) select scope_identity()";
                string schemeID = cmd.ExecuteScalar().ToString();
                doc.SelectNodes("//schemeTemplate")[0].Attributes["id"].Value = schemeID;
                cmd.CommandText = "Update SchemeTemplates set schemeXml = '" + doc.ChildNodes[1].OuterXml + "' where TemplateID=" + schemeID;
                cmd.ExecuteNonQuery();
            }
        }
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["default"].ToString();
        }
        public static DataSet GetDS(string sql)
        {
            if (ConfigurationManager.AppSettings["loadtemplates"].ToString() == "true")
            {
                LoadAllSchemeTemplates(@"D:\Aashish\dc\DrySoft\Discount\code\promos\xmls");
            }

            SqlDataAdapter da = new SqlDataAdapter(sql, GetConnectionString());
            DataSet retDS = new DataSet();
            da.Fill(retDS);
            return retDS;
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
