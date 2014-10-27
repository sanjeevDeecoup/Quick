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

public partial class _Default : System.Web.UI.Page
{
    private string strFromDate = "", strCurrentDate = "";
    SqlConnection con = new SqlConnection(PrjClass.sqlConStr);
    SqlCommand cmd2;
    string filePath = string.Empty;
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ArrayList Date = BAL.BALFactory.Instance.BAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Session["BID"].ToString());
            if (Request.QueryString["Acess"] != null)
            {
                Session["ReturnMsg"] = "You are not authorized !";
                return;
            }
            if (Request.QueryString["Tab"] != null)
            {
                Session["ReturnMsg"] = "Stock reconcilliation is already open.";
                return;
            }
            bool status = DAL.DALFactory.Instance.DAL_NewChallan.DefaultFactory(Globals.BranchID);
            strCurrentDate = Date[0].ToString();
            if (status == true)
            {
                grdReport.Visible = false;
                grdDelivery.Visible = false;
                grdHomeDelivery.Visible = false;
                grdCustomerAnniversary.Visible = false;
                grdCustomerBirthDay.Visible = false;
                btnUrgentBookingShow.Visible = false;
                btnTodayDelivered.Visible = false;
                btnHomeDelivery.Visible = false;
                btnCustomerBirthday.Visible = false;
                btnAnniversary.Visible = false;
                lblCustAni.Visible = false; 
                Label4.Visible = false;
                lblCustBirt.Visible = false;
                Label3.Visible = false;
                lblHomDel.Visible = false;
                Label2.Visible = false;
                Label6.Visible = false;
                LblUBook.Visible = false;
                Label1.Visible = false;
            }
            else
            {
                strFromDate = strCurrentDate;
                ShowBookingDetails1(strFromDate);
                ShowBookingDetails2(strFromDate);
                ShowCustomerBirthday(strFromDate);
                ShowCustomerAnniversary(strFromDate);
                ShowBookingDetails(strFromDate);
                blnRight = AppClass.CheckExportToExcelRightOnPage();
                if (blnRight)
                {
                    btnHomeDelivery.Visible = true;
                    btnCustomerBirthday.Visible = true;
                    btnAnniversary.Visible = true;
                    btnTodayDelivered.Visible = true;
                    btnUrgentBookingShow.Visible = true;
                }
                else
                {
                    btnHomeDelivery.Visible = false;
                    btnCustomerBirthday.Visible = false;
                    btnAnniversary.Visible = false;
                    btnTodayDelivered.Visible = false;
                    btnUrgentBookingShow.Visible = false;
                }
            }
            if (Request.QueryString["Backup"] != null)
            {
                SetBackup();
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

    private DataSet ReturnCustomerDetail(string BookingNo)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 15);
        ds = AppClass.GetData(cmd);
        return ds;
    }

    private bool CheckFinallyMatchingDate(string Date)
    {
        bool status = false;
        string CurrentDate1 = "";
        CurrentDate1 = Date[0].ToString();
        if (Convert.ToDateTime(Date).ToString("dd MMM yyyy") == CurrentDate1)
            status = true;
        else
            status = false;
        return status;
    }

    private string ReturnDate(string BookingNo)
    {
        string Date = "";
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@Flag", 13);
        ds = AppClass.GetData(cmd);
        for (int iRow = 0; iRow < ds.Tables[0].Rows.Count; iRow++)
        {
            Date = ds.Tables[0].Rows[iRow]["date"].ToString();
            iRow = ds.Tables[0].Rows.Count;
        }
        Date = AddThreeDaysDate(Date);
        return Date;
    }

    private string AddThreeDaysDate(string Date)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        string Date1 = "";
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookDate1", Date);
        cmd.Parameters.AddWithValue("@Flag", 14);
        ds = AppClass.GetData(cmd);
        Date1 = ds.Tables[0].Rows[0]["BookingDate"].ToString();
        return Date1;
    }

    private void ShowBookingDetails(string strStartDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        DataSet dsMain = new DataSet();
        dsMain = BAL.BALFactory.Instance.BAL_Employee.ShowUrguntBooking(strStartDate, Globals.BranchID);
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            Label1.Text = "- " + dsMain.Tables[0].Rows[0]["DELDATE"].ToString();
        }
        grdReport.DataSource = dsMain.Tables[0];
        grdReport.DataBind();
        ViewState["UrgentBooking"] = dsMain;
        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport1(grdReport);
        }
    }

    private void ShowBookingDetails2(string strStartDate)
    {
        grdHomeDelivery.DataSource = null;
        grdHomeDelivery.DataBind();
        DataSet dsMain = new DataSet();
        dsMain = BAL.BALFactory.Instance.BAL_Employee.ShowHomeDelivery(strStartDate, Globals.BranchID);
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            Label2.Text = "- " + dsMain.Tables[0].Rows[0]["DELDATE"].ToString();
        }
        grdHomeDelivery.DataSource = dsMain.Tables[0];
        grdHomeDelivery.DataBind();
        ViewState["HomeDelivery"] = dsMain;
        if (grdHomeDelivery.Rows.Count > 0)
        {
            CalculateGridReport1(grdHomeDelivery);
        }
    }

    private void CalculateGridReport1(GridView grdReport)
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float TotalCostCount = 0, TotalDue = 0, OrderCount = 0, TotalPaid = 0;
        try
        {
            for (int r = 0; r < rc; r++)
            {
                OrderCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);              
                TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[5].Text);
                TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[6].Text);
            }
            grdReport.FooterRow.Cells[2].Text = OrderCount.ToString();
            grdReport.FooterRow.Cells[4].Text = TotalCostCount.ToString();           
            grdReport.FooterRow.Cells[5].Text = TotalPaid.ToString();
            grdReport.FooterRow.Cells[6].Text = TotalDue.ToString();
        }
        catch (Exception ex)
        { }
    }

    private void CalculateGridReport(GridView grdReport)
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0;
        try
        {
            for (int r = 0; r < rc; r++)
            {
                //OrderCount++;
                if (grdReport.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
                {
                    OrderCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                    TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                    TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);
                    TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[5].Text);
                    BalanceAmount += float.Parse("0" + grdReport.Rows[r].Cells[6].Text);
                }
            }
            grdReport.FooterRow.Cells[2].Text = OrderCount.ToString();
            grdReport.FooterRow.Cells[3].Text = TotalCostCount.ToString();
            grdReport.FooterRow.Cells[4].Text = TotalPaid.ToString();
            grdReport.FooterRow.Cells[5].Text = TotalDue.ToString();
            grdReport.FooterRow.Cells[6].Text = BalanceAmount.ToString();
        }
        catch (Exception ex)
        { }
    }

    private void ShowBookingDetails1(string strStartDate)
    {
        grdDelivery.DataSource = null;
        grdDelivery.DataBind();
        DataSet dsMain = new DataSet();
        dsMain = BAL.BALFactory.Instance.BAL_Employee.GetTodayPending(strStartDate, Globals.BranchID);
        if (dsMain.Tables[0].Rows.Count > 0)
        {
            Label6.Text = "- " + dsMain.Tables[0].Rows[0]["DELDATE"].ToString();
        }
        grdDelivery.DataSource = dsMain;
        grdDelivery.DataBind();
        ViewState["TodayBooking"] = dsMain;
        if (grdDelivery.Rows.Count > 0)
        {
            CalculateGridReport1(grdDelivery);
        }
    }

    private void ShowCustomerBirthday(string strStartDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        DataSet dsMain = new DataSet();
        dsMain = BAL.BALFactory.Instance.BAL_Employee.ShowCustomerBirthday(strStartDate, Globals.BranchID);
        grdCustomerBirthDay.DataSource = dsMain.Tables[0];
        grdCustomerBirthDay.DataBind();
        grdCustomerBirthDay.Visible = true;
        ViewState["BirthDay"] = dsMain;
    }

    private void ShowCustomerAnniversary(string strStartDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        DataSet dsMain = new DataSet();
        dsMain = BAL.BALFactory.Instance.BAL_Employee.ShowCustomerAnniversary(strStartDate, Globals.BranchID);
        grdCustomerAnniversary.DataSource = dsMain.Tables[0];
        grdCustomerAnniversary.DataBind();
        grdCustomerAnniversary.Visible = true;
        ViewState["Anniversary"] = dsMain;
    }

    private void CalculateGridReport1()
    {
        int rc = grdDelivery.Rows.Count;
        int cc = grdDelivery.Columns.Count;
        float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0, Discount = 0;
        try
        {
            for (int r = 0; r < rc; r++)
            {
                OrderCount++;
                if (grdDelivery.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
                {
                    TotalCostCount += float.Parse("0" + grdDelivery.Rows[r].Cells[2].Text);
                    TotalPaid += float.Parse("0" + grdDelivery.Rows[r].Cells[3].Text);
                    TotalDue += float.Parse("0" + grdDelivery.Rows[r].Cells[4].Text);
                    BalanceAmount += float.Parse("0" + grdDelivery.Rows[r].Cells[5].Text);
                    Discount += float.Parse("0" + grdDelivery.Rows[r].Cells[6].Text);
                }
            }
            grdDelivery.FooterRow.Cells[1].Text = OrderCount.ToString();
            grdDelivery.FooterRow.Cells[2].Text = TotalCostCount.ToString();
            grdDelivery.FooterRow.Cells[3].Text = TotalPaid.ToString();
            grdDelivery.FooterRow.Cells[4].Text = TotalDue.ToString();
            grdDelivery.FooterRow.Cells[5].Text = BalanceAmount.ToString();
            grdDelivery.FooterRow.Cells[6].Text = Discount.ToString();
        }
        catch (Exception ex)
        { }
    }

    protected void btnUrgentBookingShow_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["UrgentBooking"];
        grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }

    protected void btnTodayDelivered_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["TodayBooking"];
        grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdDelivery);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }

    protected void btnHomeDelivery_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["HomeDelivery"];
        grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdHomeDelivery);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }

    protected void btnCustomerBirthday_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["BirthDay"];
        grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdCustomerBirthDay);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }  

    protected void btnAnniversary_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["Anniversary"];
        grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdCustomerAnniversary);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }

    string strPrinterName = string.Empty;
    protected void grdDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drview = e.Row.DataItem as DataRowView;
        if (strPrinterName == "")
        {
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
            string strBookinNo = hypBookingNo.Text;
            HiddenField hdnDueDate = (HiddenField)e.Row.FindControl("hdnDueDate");
            string strDuedate = hdnDueDate.Value;
           // hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }
    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drview = e.Row.DataItem as DataRowView;
        if (strPrinterName == "")
        {
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
            string strBookinNo = hypBookingNo.Text;
            HiddenField hdnDueDate = (HiddenField)e.Row.FindControl("hdnDueDate");
            string strDuedate = hdnDueDate.Value;
            //hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }

    protected void grdHomeDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drview = e.Row.DataItem as DataRowView;
        if (strPrinterName == "")
        {
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
            string strBookinNo = hypBookingNo.Text;
            HiddenField hdnDueDate = (HiddenField)e.Row.FindControl("hdnDueDate");
            string strDuedate = hdnDueDate.Value;
            //hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }
}