using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.Text;
using System.Collections;


namespace QuickWeb
{
    public partial class BackupScript : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd;
        string filePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        protected void btnScript1_Click(object sender, EventArgs e)
        {
          List<string> Tables = new List<string>();
            String Tname = string.Empty;
            cmd = new SqlCommand("Select Table_Name from INFORMATION_SCHEMA.TABLES order by Table_Name",con);
            con.Open();
            SqlDataReader reader = PrjClass.ExecuteReader(cmd);
            while (reader.Read())
            {
                Tables.Add(reader["Table_Name"].ToString());
            }
         reader.Close();

            //string[] Tables = { Tname };
          filePath = "C:\\GenScript.gz";
            string FileName1 = string.Empty;
            FileStream Destinationfile = File.Create(filePath);
          
            FileName1 = "C:\\GenScript.sql";
            Server myServer = new Server(new ServerConnection(con));
            Database dbs = myServer.Databases["DrySoft1"];
            StringBuilder sb = new StringBuilder();
            ScriptingOptions option = new ScriptingOptions();
            option.ScriptData = true;
            option.ScriptDrops = false;
            option.FileName = FileName1;
            option.EnforceScriptingOptions = true;
            option.ScriptSchema = true;
            option.IncludeHeaders = true;
            option.AppendToFile = true;
            option.Indexes = true;
            option.WithDependencies = true;
            foreach (var tbl in Tables)
            {
                dbs.Tables[tbl].EnumScript(option);
            }

            FileStream sourcefile=File.OpenRead("C:\\GenScript.sql");
            byte[] buffer = new byte[sourcefile.Length];
            sourcefile.Read(buffer, 0, buffer.Length);
            using (GZipStream output = new GZipStream(Destinationfile, CompressionMode.Compress))
            {
                output.Write(buffer, 0, buffer.Length);
            
            }
           
            sourcefile.Close();
            Destinationfile.Close();
            if (File.Exists("C:\\GenScript.sql"))
                File.Delete("C:\\GenScript.sql");
            SendMail(filePath);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }


        private void SendMail(string emailfile)
        {
            try
            {
                bool SSL = false;
                SqlCommand cmd = new SqlCommand();
                string eMail = string.Empty;
                
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 16);
                SqlDataReader sdr = null;
                sdr = AppClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    eMail = "" + sdr.GetValue(0);
                }
                if (eMail != "")
                {
                    SqlCommand cmd1 = new SqlCommand();
                    DataSet ds1 = new DataSet();
                    cmd1.CommandText = "sp_ReceiptConfigSetting";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                    cmd1.Parameters.AddWithValue("@Flag", 2);
                    ds1 = AppClass.GetData(cmd1);

                    string FEmail = eMail;
                    string mailBody = "Backup Successfully";
                    SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                    bool IsMailed = BasePage.SendMailWithAttachmentfile(FEmail, "Database Backup", mailBody, true, "Backup Database", ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL, emailfile);
                    if (IsMailed)
                        lblsummarySucess.Text = "Email send successfully..";
                    else
                        lblsummaryErr.Text = "Email not send..";
                }
                else
                    lblsummarySucess.Text = "Sorry ! Email not found for this branch..";
            }
            catch (Exception ex)
            {
            }

        }
           
    }
}