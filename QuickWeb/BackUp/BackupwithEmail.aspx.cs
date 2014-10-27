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
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace QuickWeb.BackUp
{
    public partial class BackupwithEmail : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd2;
        string filePath = string.Empty;
        DataSet dsBkp = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {              
                BindDropDown1();
                Showdata();
                dsBkp = BAL.BALFactory.Instance.BL_ColorMaster.CheckRemote(Globals.BranchID);
                bool active = Convert.ToBoolean(dsBkp.Tables[0].Rows[0]["IsBackupActive"].ToString());
                if (active == true)
                {
                    divBkpMsg.Visible = true;
                }
                else
                {
                    divBackUp.Visible = true;
                }
            }           
            var btn = Request.Params["__EVENTTARGET"] as string;
            if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnBackup")
            {
                btnBackup_Click(null, EventArgs.Empty);
                Showdata();
            }
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            SqlCommand cmdEmail = new SqlCommand();
            cmdEmail.CommandText = "sp_ReceiptConfigSetting";
            cmdEmail.CommandType = CommandType.StoredProcedure;
            cmdEmail.Parameters.AddWithValue("@backupdrive", Drpdrive.SelectedItem.ToString());
            cmdEmail.Parameters.AddWithValue("@backuppath", "backup");
            if (chkEmail.Checked == true)
            {
                cmdEmail.Parameters.AddWithValue("@BackUpEmailID", txtEmailID.Text);
            }
            else
            {
                cmdEmail.Parameters.AddWithValue("@BackUpEmailID", hdnEmailID.Value);
            }  
            cmdEmail.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmdEmail.Parameters.AddWithValue("@Flag", 18);
            string res1 = PrjClass.ExecuteNonQuery(cmdEmail);
            if (res1 != "Record Saved")
            {
                return;
            }
            SqlCommand cmd1 = new SqlCommand();
            //DataSet ds3 = new DataSet();
            //cmd1.CommandText = "sp_ReceiptConfigSetting";
            //cmd1.CommandType = CommandType.StoredProcedure;
            //cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            //cmd1.Parameters.AddWithValue("@Flag", 11);
            //ds3 = PrjClass.GetData(cmd1);

            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            ds2 = BAL.BALFactory.Instance.BL_ColorMaster.CheckBackupEmail(Globals.BranchID);
            string HostName = ds2.Tables[0].Rows[0]["HostName"].ToString();
            string BranchEmail = ds2.Tables[0].Rows[0]["BranchEmail"].ToString();
            string BranchPassword = ds2.Tables[0].Rows[0]["BranchPassword"].ToString();
            string EmailId = ds2.Tables[0].Rows[0]["EmailId"].ToString();
            string strBackUpEmailID = ds2.Tables[0].Rows[0]["BackUpEmailID"].ToString();



            ds1 = BAL.BALFactory.Instance.BL_ColorMaster.CheckRemote(Globals.BranchID);
            bool active = Convert.ToBoolean(ds1.Tables[0].Rows[0]["IsBackupActive"].ToString());
            string databaseName = ds1.Tables[0].Rows[0]["BackupDatabase"].ToString();

            if (active == false)
            {
                SetBackup();
                if (chkEmail.Checked == true)
                {
                    if (HostName.ToString() == "" || BranchEmail.ToString() == "" || BranchPassword.ToString() == "")
                    {
                       // Session["ReturnMsg"] = "Please configure Email Details.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                        lblErr.Text = "Kindly configure Email Details.";
                        return;
                    }
                    else
                    {
                       // if (EmailId.ToString() == "")
                        if (strBackUpEmailID.ToString() == "")
                        {
                           // Session["ReturnMsg"] = "Reciever Email is not configured for Back Up.";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                            lblErr.Text = "Receiver Email is not configured for back up.";
                            return;
                        }
                        else
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
                            {
                              //  Session["ReturnMsg"] = "InterNet Connection Disconnect";
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                                lblErr.Text = "Internet connection disconnect";
                                return;
                            }
                            else
                            {
                                SetBackupEmail(databaseName);
                            }
                        }
                    }

                }

            }
            //else
            //{
            //    if (HostName.ToString() == "" || BranchEmail.ToString() == "" || BranchPassword.ToString() == "")
            //    {


            //        Session["ReturnMsg"] = "Please configure Email Details.";

            //        return;


            //    }
            //    else
            //    {
            //        if (EmailId.ToString() == "")
            //        {
            //            Session["ReturnMsg"] = "Reciever Email is not configured for Back Up.";
            //            return;
            //        }
            //        else
            //        {
            //            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
            //            {
            //                Session["ReturnMsg"] = "InterNet Connection Disconnect";
            //                return;
            //            }
            //            else
            //            {
            //                SetBackupEmail(databaseName);
            //            }
            //        }
            //    }
            //}
        }


        public void SetBackup()
        {
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                DataSet ds1 = new DataSet();
                cmd1.CommandText = "sp_ReceiptConfigSetting";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd1.Parameters.AddWithValue("@Flag", 11);
                ds1 = PrjClass.GetData(cmd1);
                string currentDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                //string fileName = ds1.Tables[0].Rows[0]["backupdrive"].ToString() + "DrySoft" + currentDate + ".bak";
                string filename1 = ds1.Tables[0].Rows[0]["backupdrive"].ToString() + "DCBackup";
                if (Directory.Exists(filename1))
                {
                    DirectoryInfo fi = new DirectoryInfo(filename1);
                    FileInfo[] files = fi.GetFiles("*.bak").Where(p => p.Extension == ".bak").ToArray();
                    foreach (FileInfo file in files)
                        try
                        {
                            file.Attributes = FileAttributes.Normal;
                            File.Delete(file.FullName);
                        }
                        catch { }
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(filename1);
                }
                string fileName = filename1 + "\\" + "DrySoft" + currentDate + ".bak";
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Backup";
                cmd.Parameters.AddWithValue("@fileName", fileName);
                ds = PrjClass.GetData(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                   // Session["ReturnMsg"] = "Back Up Created on " + fileName;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                    lblMsg.Text = "Back up done successfully at " + fileName; 
                    Response.Expires = -1;
                }
            }
            catch (Exception ex)
            {
             //   Session["ReturnMsg"] = PrjClass.BackFailMsg;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = PrjClass.BackFailMsg; 
                Response.Expires = -1;
            }
        }

        public void SetBackupEmail(string DatabaseName)
        {
            SqlCommand cmd1 = new SqlCommand();
            List<string> Tables = new List<string>();
            DataSet ds1 = new DataSet();
            cmd1.CommandText = "sp_ReceiptConfigSetting";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd1.Parameters.AddWithValue("@Flag", 11);
            ds1 = PrjClass.GetData(cmd1);
            String Tname = string.Empty;
            string Path1 = ds1.Tables[0].Rows[0]["backupdrive"].ToString() + "DCBackup";
            string currentDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
            cmd2 = new SqlCommand("Select Table_Name from INFORMATION_SCHEMA.TABLES order by Table_Name", con);
            con.Open();
            SqlDataReader reader = PrjClass.ExecuteReader(cmd2);
            while (reader.Read())
            {
                Tables.Add(reader["Table_Name"].ToString());
            }
            reader.Close();

            //string[] Tables = { Tname };
            filePath = Path1 + "\\" + "DrySoftBackup" + currentDate + ".gz";
            string FileName1 = string.Empty;
            FileStream Destinationfile = File.Create(filePath);

            FileName1 = Path1 + "\\" + "DrySoftBackup" + currentDate + ".sql";
            Server myServer = new Server(new ServerConnection(con));
            Database dbs = myServer.Databases[DatabaseName];
            StringBuilder sb = new StringBuilder();
            ScriptingOptions option = new ScriptingOptions();
            option.ScriptData = true;
            option.ScriptDrops = false;
            option.FileName = FileName1;
            option.EnforceScriptingOptions = true;
            option.ScriptSchema = false;
            option.IncludeHeaders = true;
            option.AppendToFile = true;
            option.Indexes = true;
            option.WithDependencies = true;
            option.ScriptDataCompression = true;
            foreach (var tbl in Tables)
            {
                dbs.Tables[tbl].EnumScript(option);
            }
            FileStream sourcefile = File.OpenRead(FileName1);
            byte[] buffer = new byte[sourcefile.Length];
            sourcefile.Read(buffer, 0, buffer.Length);
            using (GZipStream output = new GZipStream(Destinationfile, CompressionMode.Compress))
            {
                output.Write(buffer, 0, buffer.Length);

            }
            sourcefile.Close();
            Destinationfile.Close();
            con.Close();
            con.Dispose();
            if (File.Exists(FileName1))
                File.Delete(FileName1);
            SendMail(filePath);

        }

        private void SendMail(string emailfile)
        {
            SqlDataReader sdr = null; SqlCommand cmd = new SqlCommand();
            try
            {
                SqlCommand cmd2 = new SqlCommand();
                List<string> Tables = new List<string>();
                DataSet ds2 = new DataSet();
                cmd2.CommandText = "sp_ReceiptConfigSetting";
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd2.Parameters.AddWithValue("@Flag", 11);
                ds2 = PrjClass.GetData(cmd2);
                string Path1 = ds2.Tables[0].Rows[0]["backupdrive"].ToString() + "DCBackup";

                bool SSL = false;


                string eMail = string.Empty;
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 16);

                sdr = AppClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    eMail = "" + sdr.GetValue(1);
                }
                if (eMail != "")
                {
                    ds2 = BAL.BALFactory.Instance.BL_ColorMaster.CheckRemote(Globals.BranchID);
                    bool active = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsBackupActive"].ToString());

                    SqlCommand cmd1 = new SqlCommand();
                    DataSet ds1 = new DataSet();
                    cmd1.CommandText = "sp_ReceiptConfigSetting";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                    cmd1.Parameters.AddWithValue("@Flag", 2);
                    ds1 = AppClass.GetData(cmd1);

                    string FEmail = eMail;
                   // string mailBody = "Backup Successfully";
                    string mailBody = "<table style='width:550px'> <tr><td><br /><p>Hello,</p><p>Greetings!</p><br /> <p>Please find attached a copy of all the information(data) available in your database instance of Quick Dry Cleaning Software running at your store. </p> <p>  We advise you to kindly save this backup file at some safe location on your computer system or external hard disk which might be handy in case of any eventuality.</p><br /><p>Feel free to write back to us in case you have questions or need more information around this.</p><br /><p>Sincerely,<br />Quick Dry Cleaning Software Support Team </p> </td> </tr></table>";
                    SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());

                    Task t = Task.Factory.StartNew
                        (
                           () => { BasePage.SendMailWithAttachmentfile(FEmail, "Database Backup", mailBody, true, "Backup Database", ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL, emailfile); }
                        );
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                    lblMsg.Text = "Back up done successfully at " + Path1.ToString() + " and Sent to " + eMail.ToString();

                  //  bool IsMailed = BasePage.SendMailWithAttachmentfile(FEmail, "Database Backup", mailBody, true, "Backup Database", ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL, emailfile);
                    //if (IsMailed)
                    //{
                    //    if (active == true)
                    //    {
                    //        Session["ReturnMsg"] = "Back Up Sent to " + eMail.ToString();
                    //    }
                    //    else
                    //    {
                    //        Session["ReturnMsg"] = "Backup Up Created on " + Path1.ToString() + " and Sent to " + eMail.ToString();
                    //        Response.Expires = -1;
                    //    }
                    //}
                    //else
                    //{
                    //    Session["ReturnMsg"] = "Backup is not Send to Email " + eMail.ToString();
                    //    Response.Expires = -1;
                    //}
                }
                else
                {
                  //  Session["ReturnMsg"] = "Sorry ! Email not found for this branch..";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Sorry ! Email not found for this branch..";
                    Response.Expires = -1;
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
        }

        public void BindDropDown1()
        {
            Drpdrive.Focus();
            string[] DriveList = Environment.GetLogicalDrives();
            for (int i = 0; i < DriveList.Length; i++)
            {
                Drpdrive.Items.Add(new ListItem(DriveList[i].ToString(), DriveList[i].ToString()));
            }
        }

        private void Showdata()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            PrjClass.SetItemInDropDown(Drpdrive, ds.Tables[0].Rows[0]["backupdrive"].ToString(), true, false);
            txtEmailID.Text = ds.Tables[0].Rows[0]["BackUpEmailID"].ToString();
            // PrjClass.SetItemInDropDown(Drpfloder, ds.Tables[0].Rows[0]["backuppath"].ToString(), true, false);
        }


    }
}