using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickWeb;

public partial class Reports_ItemWiseRateList : System.Web.UI.Page
{
    private string status = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;

        var forEdit = (Request.QueryString["ForInput"] ?? "").ToString();
        var rateListName = (Request.QueryString["RateListName"] ?? "Default").ToString();
        lblRateListName.Text = rateListName + " Rate List";
        if (forEdit == string.Empty || forEdit == "false")
        {
            hdnForEdit.Value = "";
            btnSave.Visible = false;
            btnExport.Visible = true;
        }
        else if (forEdit == "true")
        {
            hdnForEdit.Value = "true";
            btnExport.Visible = false;
        }

        var rateListId = (Request.QueryString["RateListId"] ?? "0").ToString();

        var ds = new DataSet();
        var cmd = new SqlCommand();
        DataSet dsReport = new ChallanDataSet();
        cmd = new SqlCommand();
        cmd.CommandText = "sp_ItemWiseRate";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@RateListId", rateListId);
        dsReport = PrjClass.GetData(cmd);
        // ds = PrjClass.GetData(cmd);
        ds = dsReport;
        grdDetails.DataSource = dsReport;
        grdDetails.DataBind();
        SetTheProcesses();
        status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.PrintRateList, Session["UserType"].ToString());
        if (status == "True")
        {
            btnExport.Visible = true;
        }
        else
        {
            btnExport.Visible = false;           
        }
    }

    private void SetTheProcesses()
    {
        var sqlCommand = new SqlCommand { CommandText = "sp_ItemWiseRate", CommandType = CommandType.StoredProcedure };
        sqlCommand.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        sqlCommand.Parameters.AddWithValue("@Flag", 2);
        var processList = string.Empty;
        var sqlDataReader = PrjClass.ExecuteReader(sqlCommand);
        while (sqlDataReader != null && sqlDataReader.Read())
        {
            processList += sqlDataReader.GetString(0) + ":";
        }
        if (sqlDataReader != null)
            sqlDataReader.Close();

        if (processList.Length >= 1)
            processList = processList.Substring(0, processList.Length - 1);

        hdnProcesses.Value = processList;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Rate_list.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdDetails);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }

    protected void BtnSaveClick(Object sender, EventArgs e)
    {
        var rateListId = int.Parse((Request.QueryString["RateListId"] ?? "0").ToString());
        var rateListName = (Request.QueryString["RateListName"] ?? "Default").ToString();
        BAL.BALFactory.Instance.Bal_Report.SaveItemWiseRateList(hdnProcesses.Value, hdnRates.Value, rateListId, Globals.BranchID);
        Response.Redirect("~/Reports/ItemWiseRateList.aspx?RateListId=" + rateListId + "&ForInput=false&RateListName=" + rateListName, false);
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=\"Javascript\">window.opener = self;window.close();</script>");
    }

    protected void GrdDetailsRowEdit(Object sender, GridViewEditEventArgs e)
    {

    }

    protected void GrdDetailsRowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }
}