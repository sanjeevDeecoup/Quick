using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

public partial class CustomerMaster : System.Web.UI.Page
{
    private DTO.CustomerMaster Ob = new DTO.CustomerMaster();

    private static DTO.Report Ob1 = new DTO.Report();
    private string status = string.Empty;
    private static string _memberShipId = string.Empty, _prevMobile = string.Empty;
    private static string _barCode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserBranch"] == null || Session["UserType"] == null || Session["UserName"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            BindDefault();          
        }
    }  

    private void BindDefault()
    {
        status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.AllowCustExcel, Session["UserType"].ToString());
        if (status == "True")
        {
            btnExport.Visible = true;
        }
        else
        {
            btnExport.Visible = false;
        }
        BindPriroity();
        BindRateList();
        BindDummyRow();
        divCustDetail.Visible = false;
    }

    protected void BindRateList()
    {
        ddlRateList.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindAllListMasters(Globals.BranchID);
        ddlRateList.DataTextField = "name";
        ddlRateList.DataValueField = "rateListId";
        ddlRateList.DataBind();
    }

    protected void btnAddNewPriority_Click(object sender, EventArgs e)
    {
        if (txtNewPriority.Text == "")
        {
            lblErr.Text = "No new priority was provided to add.";
            return;
        }
        string res = string.Empty;
        Ob.BranchId = Globals.BranchID;
        Ob.CustomerName = txtNewPriority.Text;
        res = BAL.BALFactory.Instance.BL_CustomerMaster.SavePriority(Ob);
        if (res == "Record Saved")
        {
            BindPriroity();
            PrjClass.SetItemInDropDown(drpPriority, txtNewPriority.Text, true, false);
            txtNewPriority.Text = "";
            drpPriority.Focus();
        }
        else
        {
            lblErr.Text = " Duplicate Priority was provided.";
        }
    }

    protected void BindPriroity()
    {
        Ob.BranchId = Globals.BranchID;
        drpPriority.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.BindPriority(Ob);
        drpPriority.DataTextField = "Priority";
        drpPriority.DataValueField = "PriorityID";
        drpPriority.DataBind();
    }   

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dtTmp = new DataTable();
        DataSet ds = new DataSet();
        Ob.BranchId = Globals.BranchID;
        ds = BAL.BALFactory.Instance.BL_CustomerMaster.ExportToExcel(Ob);
        dtTmp = ds.Tables[0];
        if (dtTmp.Rows.Count > 0)
        {
            GridView grd = new GridView();
            grd.DataSource = dtTmp;
            grd.DataBind();
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grd);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }
        else
        {
            lblErr.Text = "No customer record was found.";
        }
    }   

    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("CustomerCode");
        dummy.Columns.Add("CustomerName");
        dummy.Columns.Add("CustomerAddress");
        dummy.Columns.Add("CustomerPhone");
        dummy.Columns.Add("CustomerMobile");
        dummy.Columns.Add("CustomerEmailId");
        dummy.Columns.Add("Priority");
        dummy.Columns.Add("CustomerRefferredBy");
        dummy.Columns.Add("DefaultDiscountRate");
        dummy.Columns.Add("IsWebsite");
        dummy.Rows.Add();
        grdSearchResult.DataSource = dummy;
        grdSearchResult.DataBind();
       
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        divCustDetail.Visible = true;
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.GetData(Globals.BranchID);
        grdSearchResult.DataBind();
    }        
}