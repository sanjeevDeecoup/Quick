using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Management.Smo;
using System.Net.NetworkInformation;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

public partial class Login : System.Web.UI.Page
{
    public string strVersion = PrjClass.GetSoftwareVersionName();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserId.Focus();
            drpBind();
            if (Request.QueryString["Backup"] != null)
            {
                 bool active1 = BAL.BALFactory.Instance.BL_ColorMaster.CheckCloud(Globals.BranchID);
                 if (active1 == false)
                 {
                     SetBackup();
                 }
            }
            if (Request.QueryString["option"] != null)
            {
                bool active = BAL.BALFactory.Instance.BL_ColorMaster.CheckCloud(Globals.BranchID);
                if (active == false)
                {
                    SetBackup();
                }
            }
        }
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
            cmd.Parameters.Add(new SqlParameter("@fileName", fileName));
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["ReturnMsg"] = "System Data Backup Sucessfull.";
                Response.Expires = -1;
            }
        }
        catch (Exception ex)
        {
            Session["ReturnMsg"] = PrjClass.BackFailMsg;
            Response.Expires = -1;
        }
    }
    private string GetRandomNumber()
    {
        Random random = new Random();
        int value = random.Next(10000);
        string text = value.ToString("000120");
        return text;
    }
    private void UpdateSucessTrue(string cookies,string BID)
    {
        ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "proc_BindToMachine";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MacId", cookies);
        cmd.Parameters.AddWithValue("@LastLoginDate", date[0]);
        cmd.Parameters.AddWithValue("@LastLoginTime", date[1]);
        cmd.Parameters.AddWithValue("@BranchId", BID);
        cmd.Parameters.AddWithValue("@Flag", 5);
        PrjClass.ExecuteNonQuery(cmd);
    }
    private void chkStatusRememberMe(string cookies, string BID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "proc_BindToMachine";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MacId", cookies);
        cmd.Parameters.AddWithValue("@BranchId", BID);
        cmd.Parameters.AddWithValue("@Flag", 13);
        PrjClass.ExecuteNonQuery(cmd);
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        ArrayList arydate = new ArrayList();
        arydate = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(drpBranch.SelectedValue);
        string strBID = string.Empty;
        string strUserID = string.Empty;
        string databaseName = string.Empty;
        bool IsLoginTimeOn = CheckOperatingTimeOn();
        if (IsLoginTimeOn == true)
        {
            if (hdnWeekly.Value == "1")
            {
                Task t = Task.Factory.StartNew
                (
                () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(drpBranch.SelectedValue, arydate[0].ToString(), arydate[1].ToString(), "Deny", "4", txtUserId.Text); }
                );
                lblMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Today is weekly OFF. Please contact your branch manager.";
                hdnWeekly.Value = "0";
                return;
            }
            bool IsLoginActive = CheckIsLoginTime();
            if (IsLoginActive == false)
            {
                Task t = Task.Factory.StartNew
                (
                () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(drpBranch.SelectedValue, arydate[0].ToString(), arydate[1].ToString(), "Deny", "1", txtUserId.Text); }
                );                
                lblMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Login not allowed as Branch is closed. Please contact your branch manager.";
                return;
            }
        }
        Boolean blnUserFound = false;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);       
        //string pwd=FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text,"SHA1");
        string pwd = txtPassword.Text;
        string sql = "Select * From UserMaster Where UserId COLLATE Latin1_General_CS_AS ='" + txtUserId.Text + "' AND BranchId='" + drpBranch.SelectedValue + "'";
        SqlCommand cmd = new SqlCommand(sql, sqlcon);
        sqlcon.Open();
        databaseName = sqlcon.Database;
        SqlDataReader sdr = cmd.ExecuteReader();
        try
        {                    
            if (sdr.Read())
            {
                if (sdr.GetBoolean(9) == true)
                {
                    string upwd = "" + sdr.GetValue(1);
                    if (string.Equals(upwd, pwd))
                    {
                        Session["UniqueIDPBU"] = Guid.NewGuid().ToString();
                        Session["UserId"] = txtUserId.Text;
                        Session["UserType"] = "" + sdr.GetValue(2);
                        Session["UserBranch"] = "" + sdr.GetValue(3);
                        Session["UserName"] = "" + sdr.GetValue(4);
                        Session["WorkshopUserType"] = "" + sdr.GetValue(14);
                        Session["UniqueIDPBU"] = Guid.NewGuid();
                        var StrStoreName = drpBranch.SelectedItem.Text.Split('-');
                        Session["StoreName"] = StrStoreName[1].Trim();
                        blnUserFound = true;
                    }
                    else
                    {
                        //if (sdr.GetValue(2).ToString() != "1")
                        //{
                            Task t = Task.Factory.StartNew
                            (
                            () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(drpBranch.SelectedValue, arydate[0].ToString(), arydate[1].ToString(), "Deny", "3", txtUserId.Text); }
                            );
                      //  }
                        lblMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                        lblMsg.Text = "Password does not match";
                    }
                }
                else
                {
                    if (sdr.GetValue(2).ToString() != "1")
                    {
                        Task t = Task.Factory.StartNew
                        (
                        () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(drpBranch.SelectedValue, arydate[0].ToString(), arydate[1].ToString(), "Deny", "7", txtUserId.Text); }
                        );
                    }
                    lblMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblMsg.Text = "You are not an active user.";
                }
            }
            else
            {
                Task t = Task.Factory.StartNew
                (
                () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(drpBranch.SelectedValue, arydate[0].ToString(), arydate[1].ToString(), "Deny", "2", txtUserId.Text);}
                );
                lblMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "User Id not found.";
            }
        }
        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        if (blnUserFound)
        {            
            string cookies = string.Empty;
            if (Request.Cookies["BoundToMachineCookies" + drpBranch.SelectedItem.Value + databaseName] != null)
            {
                cookies = Request.Cookies["BoundToMachineCookies" + drpBranch.SelectedItem.Value + databaseName].Value;
            }
            bool checkBoundToMachine = BAL.BALFactory.Instance.BL_CustomerMaster.CheckBoundToMachine(cookies, drpBranch.SelectedItem.Value);
            if (!checkBoundToMachine)
            {
                HttpCookie cookie = new HttpCookie("BoundToMachineCookies" + drpBranch.SelectedItem.Value + databaseName);
                cookie.Value = GetRandomNumber();
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.SetCookie(cookie);
                UpdateSucessTrue(cookie.Value, drpBranch.SelectedItem.Value);
                if (Session["UserType"].ToString() != "1")
                {
                    Task t = Task.Factory.StartNew
                      (
                      () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(drpBranch.SelectedItem.Value, arydate[0].ToString(), arydate[1].ToString(), "Deny", "6", txtUserId.Text); }
                      );
                }
                Response.Redirect("frmVerification.aspx?BID=" + drpBranch.SelectedItem.Value + "&DB=" + databaseName + "");
            }

            var dict = new Dictionary<string, int>();
            dict.Add(Session["UniqueIDPBU"].ToString(), 0);
            Globals.SessionWiseUserNumber = dict;
            Session["BID"] = drpBranch.SelectedValue;
            HttpContext.Current.Response.Cookies["UserId"]["uID"] = Globals.StartCount.ToString();
            Globals.BranchID = Session["BID"].ToString();
            Globals.UserId = Session["UserId"].ToString();
            Globals.UserType = Session["UserType"].ToString();
            Globals.WorkshopUserType = Session["WorkshopUserType"].ToString();
            Globals.UserBranch = Session["UserBranch"].ToString();
            Globals.UserName = Session["UserName"].ToString();
            Globals.StoreName = Session["StoreName"].ToString();

            Session["Time"] = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);

            Globals.date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            DAL.DALFactory.Instance.DAL_Branch.CurrentUser(Globals.UserName);            

            if (Globals.UserType.ToString() != "4")
            {
                //if (Globals.UserType.ToString() != "1")
                //{
                     strBID = Globals.BranchID;
                     strUserID = Globals.UserId;
                    Task t = Task.Factory.StartNew
                      (
                      () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(strBID, arydate[0].ToString(), arydate[1].ToString(), "Success", "5",strUserID); }
                      );
             //   }
                Server.Transfer("home.html");
            }
            else
            {
                Response.Redirect("Factory/frmFactoryHome.aspx", false);
            }
        }
    }
    public void drpBind()
    {
        drpBranch.DataSource = BAL.BALFactory.Instance.BL_Branch.ShowBranch();
        drpBranch.DataTextField = "BranchName";
        drpBranch.DataValueField = "BranchId";
        drpBranch.DataBind();
    }

    public bool CheckIsLoginTime()
    {
        bool IsLoginTime = false;

        ArrayList date = new ArrayList();
        date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(drpBranch.SelectedValue);

        //DateTime Currentdt = DateTime.Now;
        //string HrTime = Currentdt.ToString("hh:mm tt");

        string HrTime = date[1].ToString();
        HrTime = HrTime.Remove(4, 3);
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_BranchMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", drpBranch.SelectedValue);
        cmd.Parameters.AddWithValue("@CurrentTime", HrTime);
        cmd.Parameters.AddWithValue("@Flag", 15);
        ds = PrjClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
           IsLoginTime = Convert.ToBoolean(ds.Tables[0].Rows[0]["loginTime"].ToString());        
        }
        return IsLoginTime;
    }

    public bool CheckOperatingTimeOn()
    {

        bool isLoginOn = false;
        string strWeekOff = string.Empty;
        string dtrCurrentDay = string.Empty;
        SqlCommand cmd = new SqlCommand();
        SqlCommand cmd1 = new SqlCommand();
        DataSet ds2 = new DataSet();
        DataSet dsUserType = new DataSet();

        //bool IsCloudActive = BAL.BALFactory.Instance.BL_ColorMaster.CheckCloud(drpBranch.SelectedValue);
        //if (IsCloudActive == false)
        //{
        //    isLoginOn = false;
        //    return false;
        //}

        cmd1.CommandText = "sp_BranchMaster";
        cmd1.CommandType = CommandType.StoredProcedure;
        cmd1.Parameters.AddWithValue("@BranchId", drpBranch.SelectedValue);
        cmd1.Parameters.AddWithValue("@UserId", txtUserId.Text);
        cmd1.Parameters.AddWithValue("@Flag", 17);
        dsUserType = PrjClass.GetData(cmd1);
        if (dsUserType.Tables[0].Rows.Count > 0)
        {
            if (dsUserType.Tables[0].Rows[0]["UserTypeCode"].ToString() == "1")
            {
                isLoginOn = false;
                return false;
            }            
        }
        ArrayList date1 = new ArrayList();
        date1 = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(drpBranch.SelectedValue);
        DateTime dt3 = Convert.ToDateTime(date1[0].ToString());
        dtrCurrentDay = dt3.ToString("ddd");

        cmd.CommandText = "sp_BranchMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", drpBranch.SelectedValue);       
        cmd.Parameters.AddWithValue("@Flag", 16);
        ds2 = PrjClass.GetData(cmd);

        if (ds2.Tables[0].Rows.Count > 0)
        {
            isLoginOn = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsLoginTime"].ToString());
            if (isLoginOn == true)
            {
                if (ds2.Tables[1].Rows.Count > 0)
                {
                    strWeekOff = ds2.Tables[1].Rows[0]["weeklyoff"].ToString();
                    if (strWeekOff == dtrCurrentDay)
                    {                       
                        hdnWeekly.Value = "1";
                    }
                    else
                    {                        
                        hdnWeekly.Value = "0";
                    }
                }
                isLoginOn = true;
            }
            else
            {
                isLoginOn = false;
            }
        } 
        return isLoginOn;
    }
}