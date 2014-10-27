using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace QuickWeb
{
    public partial class StoreMain : System.Web.UI.MasterPage
    {
        protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
        protected string CurrentUserName = "";
        protected string checkPath = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl, false);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null || Session["UserName"] == null || Session["UserType"] == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl, false);
            }
            else
            {
                CurrentUserName = Session["UserName"].ToString();
            }
            if (!IsPostBack)
            {                
                try
                {
                    SetMenuRightsNew(Session["UserType"].ToString());
                    FillExistingRecord();
                }
                catch (Exception)
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect(FormsAuthentication.LoginUrl, false);
                }               
            }

        }
        protected void FillExistingRecord()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 18);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
              //  lblStoreName.Text = ds.Tables[0].Rows[0]["StoreName"].ToString();
                //lblFooterMessage.Text = ds.Tables[0].Rows[0]["FooterName"].ToString();
                //lblAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            }
        }

        private void SetSubMenuItemFalse()
        {
            //bookingcancel.Visible = false;
            //custMerge.Visible = false;

        }
        private void SetMenuRightsNew(string strUserTypeValue)
        {
            DataSet dsMain = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.Parameters.AddWithValue("@UserTypeId", strUserTypeValue);
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 40);
                dsMain = AppClass.GetData(cmd);
            }
            catch (Exception excp)
            {
                Response.Write(excp.ToString());
            }
            finally
            {
            }
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    ShowMenuFromTable(dsMain.Tables[0]);
                }
            }

        }
        private void ShowMenuFromTable(DataTable dtMenu)
        {
            DataTable MasterCust = new DataTable();
            MasterCust.Columns.Add("CustomerTitle");
            MasterCust.Columns.Add("CustomerFileName");


            DataTable MasterDrop = new DataTable();
            MasterDrop.Columns.Add("DropTitle");
            MasterDrop.Columns.Add("DropFileName");

            DataTable ProcessDt = new DataTable();
            ProcessDt.Columns.Add("ProcessTitle");
            ProcessDt.Columns.Add("ProcessFileName");

            DataTable PickUpDt = new DataTable();
            PickUpDt.Columns.Add("PickUpTitle");
            PickUpDt.Columns.Add("pickUpFileName");


            DataTable MasterDt = new DataTable();
            MasterDt.Columns.Add("MasterpageTitle");
            MasterDt.Columns.Add("MasterFileName");

          

            DataTable AccountDt = new DataTable();
            AccountDt.Columns.Add("AccountPageTitle");
            AccountDt.Columns.Add("AccountFileName");

            DataTable ReportDt = new DataTable();
            ReportDt.Columns.Add("ReportPageTitle");
            ReportDt.Columns.Add("ReportFileName");

            DataTable AdminDt = new DataTable();
            AdminDt.Columns.Add("AdminPageTitle");
            AdminDt.Columns.Add("AdminFileName");

          


            for (int i = 0; i < dtMenu.Rows.Count; i++)
            {
                if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Customer")
                {
                    MasterCust.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Drop")
                {
                    MasterDrop.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Process")
                {
                    ProcessDt.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "PickUp")
                {
                    PickUpDt.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Accounts")
                {           
                    AccountDt.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Reports")
                {
                    ReportDt.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Master Data")
                {
                    MasterDt.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
                else if (dtMenu.Rows[i]["ParentMenu"].ToString().Trim() == "Admin")
                {
                    AdminDt.Rows.Add(dtMenu.Rows[i]["PageTitle"].ToString(), dtMenu.Rows[i]["FileName"].ToString().Replace("~", ".."));
                }
            }


            repCustomer.DataSource = MasterCust;
            repCustomer.DataBind();

            repDrop.DataSource = MasterDrop;
            repDrop.DataBind();

            repProcess.DataSource = ProcessDt;
            repProcess.DataBind();

            repReport.DataSource = ReportDt;
            repReport.DataBind();

            repPickUp.DataSource = PickUpDt;
            repPickUp.DataBind();
         
            repAccount.DataSource = AccountDt;
            repAccount.DataBind();

            repMaster.DataSource = MasterDt;
            repMaster.DataBind();

            repAdmin.DataSource = AdminDt;
            repAdmin.DataBind();              
        }
           

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            //var httpContext = HttpContext.Current;
            //var cookies = httpContext.Request.Cookies["StartTime"];
            //httpContext.Response.Cookies.Remove("StartTime");
            //httpContext.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMinutes(-1);
            //Session.Abandon();
            Response.Redirect("~/Login.aspx?Backup=" + "BackUp" + "");
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
        protected void btnBackUp_Click(object sender, EventArgs e)
        {
            SetBackup();
        }

        protected void btnF1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/New_Booking/frm_New_Booking.aspx");
        }

        protected void btnF4_Click(object sender, EventArgs e)
        {
            if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckEditBookingRights(Session["UserType"].ToString(), Globals.BranchID) != false)
                Response.Redirect("~/New_Booking/frm_New_Booking.aspx?option=Edit");
            else
            {
                Session["ReturnMsg"] = "You are not authorised to edit the booking.";
                Response.Expires = -1;
                return;
            }
        }

        protected void btnDelivery_Click(object sender, EventArgs e)
        {
            string status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, "Delivery", Session["UserType"].ToString());
            if (status == "True")
            {
                Response.Redirect("~/Bookings/Delivery.aspx");
            }
            else
            {
                Session["ReturnMsg"] = "You are not authorized !";
            }
        }

        protected void btnSearchByInvoice_Click(object sender, EventArgs e)
        {
            string status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.SearchOrderRight, Session["UserType"].ToString());
            if (status == "True")
            {
                Response.Redirect("~/Reports/SearchByInvoice.aspx");
            }
            else
            {
                Session["ReturnMsg"] = "You are not authorized !";
            }
        }

    }
}