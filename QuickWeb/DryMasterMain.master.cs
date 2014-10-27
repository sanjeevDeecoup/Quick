using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class DryMasterMain : System.Web.UI.MasterPage
{
    protected string AppTitle = ConfigurationManager.AppSettings["AppTitle"];
    protected string CurrentUserName = "";
    protected string checkPath = "";
    public string strVersion = PrjClass.GetSoftwareVersionName();
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
            cmd.Parameters.AddWithValue("@Flag", 18);
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
        MenuItem mnuItem = null;
        DataRow[] drMenuLevel1 = dtMenu.Select("MenuItemLevel='1' AND RightToView = true");
        string strMenuTitle = "", strMenuNaviToUrl = "", strMenuParent = "", strMenuPosition = "";
        foreach (DataRow drTopMenu in drMenuLevel1)
        {
            strMenuTitle = "" + drTopMenu["PageTitle"].ToString();
            strMenuNaviToUrl = "" + drTopMenu["FileName"].ToString();
            strMenuParent = "" + drTopMenu["ParentMenu"].ToString();
            strMenuPosition = "" + drTopMenu["MenuPosition"].ToString();
            mnuItem = new MenuItem();
            mnuItem.Text = strMenuTitle;
            mnuItem.Value = strMenuTitle;
            mnuItem.NavigateUrl = strMenuNaviToUrl;
            MainMenu.Items.Add(mnuItem);
        }
        for (int c = 0; c < MainMenu.Items.Count; c++)
        {
            DataRow[] dtMenuLevel2 = dtMenu.Select("MenuItemLevel='2' AND RightToView = true AND ParentMenu = '" + MainMenu.Items[c].Text + "'");
            foreach (DataRow drSubMenu in dtMenuLevel2)
            {
                strMenuTitle = "" + drSubMenu["PageTitle"].ToString();
                strMenuNaviToUrl = "" + drSubMenu["FileName"].ToString();
                strMenuParent = "" + drSubMenu["ParentMenu"].ToString();
                strMenuPosition = "" + drSubMenu["MenuPosition"].ToString();
                mnuItem = new MenuItem();
                mnuItem.Text = strMenuTitle;
                mnuItem.Value = strMenuTitle;
                mnuItem.NavigateUrl = strMenuNaviToUrl;
                MainMenu.Items[c].ChildItems.Add(mnuItem);
            }
        }
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
            lblStoreName.Text = ds.Tables[0].Rows[0]["StoreName"].ToString();
            lblFooterMessage.Text = ds.Tables[0].Rows[0]["FooterName"].ToString();
            lblAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
        }
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

    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        bool active = BAL.BALFactory.Instance.BL_ColorMaster.CheckCloud(Globals.BranchID);
        if (active == false)
        { 
        SetBackup();
        }
    }
}