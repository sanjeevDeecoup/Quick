using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
namespace QuickWeb
{
    public partial class frmVerification : System.Web.UI.Page
    {
        public string strVersion = PrjClass.GetSoftwareVersionName();
        private static string _storename = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["BID"] == null)
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect(FormsAuthentication.LoginUrl, false);
                }               
                divPasswordGen.Attributes.Add("style", "display:block");
                txtDeviceName.Focus();
                var ds = BAL.BALFactory.Instance.BL_Branch.ShowBranch(Int32.Parse(Request.QueryString["BID"].ToString()));
                _storename = ds.Tables[0].Rows[0]["BranchName"].ToString();
            }
        }  
        protected void btnGenCode_Click(object sender, EventArgs e)
        {
            try
            {
                string res = "";
                string cookies = string.Empty;
                if (Request.Cookies["BoundToMachineCookies" + Request.QueryString["BID"].ToString()+ Request.QueryString["DB"].ToString()] != null)
                {
                    cookies = Request.Cookies["BoundToMachineCookies" + Request.QueryString["BID"].ToString() + Request.QueryString["DB"].ToString()].Value;
                }
                res = BAL.BALFactory.Instance.BL_CustomerMaster.CreatePasswordToBoundMachine(txtDeviceName.Text, cookies, Request.QueryString["BID"].ToString());
                if (res == "Record Saved")
                {
                    SendMail(cookies, Request.QueryString["BID"].ToString());
                    divLogIn.Visible = true;
                    divPasswordGen.Attributes.Add("style", "display:none");
                    txtCode.Focus();
                }
                else
                {                    
                    lblMsg.Text = res;
                    divLogIn.Visible = false;
                    divPasswordGen.Attributes.Add("style", "display:block");
                    hdnMsg.Value = "1";
                }
            }
            catch (Exception ex)
            {                
                lblMsg.Text = ex.Message.ToString();
                hdnMsg.Value = "1";
            }
        }

        private void SendSMS(string MobileNo, string databasemessage, string Password)
        {
            string strMessage = string.Empty;
            WebClient Client = new WebClient();
            Dictionary<string, string> replacements = new Dictionary<string, string>()
            {			
			{"[Mobile No]", MobileNo},			
			{"[code]", Password},
            {"[Store Name]", _storename}
			};
            foreach (var r in replacements)
            {
                databasemessage = databasemessage.Replace(r.Key, r.Value);
            }
            strMessage = databasemessage.Replace("amp;", "").Trim();
            string DecodedString = WebUtility.HtmlDecode(strMessage);
            try
            {
                Stream data = Client.OpenRead(DecodedString);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();                
            }
            catch (Exception)
            {
               
            }
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
        protected void btnVerify_Click(object sender, EventArgs e)
        {  
            try
            {
                ArrayList arydate = new ArrayList();
                arydate = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Request.QueryString["BID"].ToString());

                string StrBID = string.Empty;
                string StrUserID = string.Empty;

                bool IsPwdMatch = false;
                bool checkremember = false;
                checkremember = chkremember.Checked;
                string cookies = string.Empty;
                if (Request.Cookies["BoundToMachineCookies" + Request.QueryString["BID"].ToString() + Request.QueryString["DB"].ToString()] != null)
                {
                    cookies = Request.Cookies["BoundToMachineCookies" + Request.QueryString["BID"].ToString() + Request.QueryString["DB"].ToString()].Value;
                }
                IsPwdMatch = BAL.BALFactory.Instance.BL_CustomerMaster.CheckVerificationCode(txtCode.Text, cookies, Request.QueryString["BID"].ToString()); ;
                if (IsPwdMatch == true)
                {
                    if (!chkremember.Checked)
                    {
                        chkStatusRememberMe(Request.Cookies["BoundToMachineCookies" + Request.QueryString["BID"].ToString() + Request.QueryString["DB"].ToString()].Value, Request.QueryString["BID"].ToString());
                    }
                    var dict = new Dictionary<string, int>();
                    dict.Add(Session["UniqueIDPBU"].ToString(), 0);
                    Globals.SessionWiseUserNumber = dict;
                    Session["BID"] = Request.QueryString["BID"].ToString();
                    HttpContext.Current.Response.Cookies["UserId"]["uID"] = Globals.StartCount.ToString();
                    Globals.BranchID = Session["BID"].ToString();
                    Globals.UserId = Session["UserId"].ToString();
                    Globals.UserType = Session["UserType"].ToString();
                    Globals.WorkshopUserType = Session["WorkshopUserType"].ToString();
                    Globals.UserBranch = Session["UserBranch"].ToString();
                    Globals.UserName = Session["UserName"].ToString();

                    Session["Time"] = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);

                    Globals.date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                    DAL.DALFactory.Instance.DAL_Branch.CurrentUser(Globals.UserName);

                    if (Globals.UserType.ToString() != "4")
                    {
                        if (Globals.UserType.ToString() != "1")
                        {
                            StrBID = Globals.BranchID;
                            StrUserID = Globals.UserId;
                            Task t = Task.Factory.StartNew
                              (
                              () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(StrBID, arydate[0].ToString(), arydate[1].ToString(), "Success", "5", StrUserID); }
                              );
                        }
                        Server.Transfer("home.html");
                    }
                    else
                    {
                        Response.Redirect("Factory/frmFactoryHome.aspx", false);
                    }
                }
                else
                {
                    if (Session["UserType"].ToString() != "1")
                    {
                        StrUserID =Session["UserId"].ToString();
                        StrBID = Request.QueryString["BID"].ToString();
                        Task t = Task.Factory.StartNew
                         (
                         () => { BAL.BALFactory.Instance.BL_ColorMaster.SaveLoginHistoryData(StrBID, arydate[0].ToString(), arydate[1].ToString(), "Deny", "3", StrUserID); }
                         );
                    }

                    lblMsg.Text = "Verification Password does not match.";                   
                    achrForGetPwd.Attributes.Add("style", "display:inline");
                    txtCode.Focus();
                    hdnMsg.Value = "1";
                }
            }
            catch (Exception excp)
            {               
                lblMsg.Text = excp.Message;
                hdnMsg.Value = "1";
            }
            
        }

        private void SendMail(string cookies,string BID)
        {
            try
            {
                bool SSL = false;
                string DeviceName = string.Empty;
                SqlCommand cmd1 = new SqlCommand();
                DataSet ds1 = new DataSet();
                cmd1.CommandText = "proc_BindToMachine";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@BranchId", BID);
                cmd1.Parameters.AddWithValue("@Flag", 9);
                ds1 = AppClass.GetData(cmd1);
                string FEmail = ds1.Tables[0].Rows[0]["BranchEmail"].ToString();
                string ToEmail = ds1.Tables[0].Rows[0]["ToEmailId"].ToString();
                string Mobile = ds1.Tables[0].Rows[0]["Mobile"].ToString();
                string SMSAPI = ds1.Tables[0].Rows[0]["SMSAPI"].ToString();
                string storeName = ds1.Tables[0].Rows[0]["StoreName"].ToString();
                var Email = ToEmail.Split('@');
                DataSet dspwd = new DataSet();
                dspwd = BAL.BALFactory.Instance.BL_CustomerMaster.GetPassword(cookies, BID);
                string Password = dspwd.Tables[0].Rows[0]["SMSText"].ToString();
                DeviceName = dspwd.Tables[0].Rows[0]["DeviceName"].ToString();
                string subjectLine = "Your Quick Dry Cleaning Verification code for Store " + storeName + " is " + Password + ".";

                string mailBody = "<table style='width: 7.9in;'><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>Hello " + DeviceName + ",</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>Greetings!</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Congratulations for enabling Authenticated Access Control feature for Quick DryCleaning Software. This will help you to keep </td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>&nbsp;your business information safe. Kindly use the following code to proceed with the Authentication process.</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 20px; font-weight: bold' nowrap='nowrap' colspan='2'>Verification Code : " + Password + "</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>You will have to authenticate all the computer systems, tablets, smart phones from where you access Quick Dry Cleaning Software.</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>You can generate code for all the machines and authenticate the same. You only need to authenticate any device only once.</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>At any point in time you can revoke access rights for any machine or completely disable this feature through Admin-> Authenticated </td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Access Control screen</b>. However,we suggest keeping this feature always enabled to keep your information safe.</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Feel free to reach out to us in case you face any issues or have questions around this.</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='2'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Sincerely,</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Quick Dry Cleaning Software Support Team</td></tr><tr><td style='width: 1.2in; font-weight: bold; font-size: 14px' nowrap='nowrap'>&nbsp;</td><td style='width: 2.5in; font-size: 14px'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='left'>&nbsp;</td><td style='width: 1.0in; font-weight: bold' align='right'>&nbsp;</td><td style='width: 1.0in; font-weight: bold; font-size: 14px' align='right'>&nbsp;</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Mobile: + 91 9211140404</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Office: +91 11-45558316, Support: 9212663156</td></tr><tr><td style='font-size: 14px; color: #FF0000' nowrap='nowrap' colspan='5'>Quick Dry Cleaning Software – The Most Agile Software Solution for Dry Cleaners</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>www.quickdrycleaning.com</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>www.drycleaningcloud.com</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Skype: qdcchat</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Gtalk: qdccustomercare</td></tr><tr><td style='font-size: 14px' nowrap='nowrap' colspan='5'>Support Timing: Mon – Fri 9:00 am to 6:00 pm</td></tr></table>";
                SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                if (Mobile != "")
                {
                    Task t = Task.Factory.StartNew
                     (
                        () => { SendSMS(Mobile, SMSAPI, Password); }
                     );

                }
                Task tt = Task.Factory.StartNew
                          (
                             () => { BasePage.SendMail(ToEmail, subjectLine, mailBody, true, "Test.pdf", ds1.Tables[0].Rows[0]["HostName"].ToString(), FEmail, ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL); }
                          );
                lblEmail.Text = Email[0].Substring(0, 1) + "********" + Email[0].Substring(Email[0].Length - 1, 1) + "@" + Email[1];
                lblMobile.Text = Mobile.Substring(Mobile.Length - 2, 2);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();
                hdnMsg.Value = "1";
            }
        }
    }
}