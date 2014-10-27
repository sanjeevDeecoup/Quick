using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;

public partial class Reports_VendorReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["BID"] != null)
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Session["BID"].ToString());
                txtReportFrom.Text = date[0].ToString();
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2009; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
                if (grdProcessSelection.Rows.Count > 0)
                {
                    btnShowReport.Visible = true;
                }
                else
                {
                    lblMsg.Text = "No process has been set for vendor report";
                    btnShowReport.Visible = false;
                    return;
                }
            }
        }
        else
        {
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl, false);
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        string strUserType = "" + Session["UserType"];
        if (strUserType != "1")
        {
            grdVendorReport.ShowFooter = false;
            btnExportToExcel.Visible = false;
        }
    }
    private void FillGridView()
    {
        btnExportToExcel.Visible = false;
        DataTable dt = new DataTable();
        dt.Columns.Add("BookingDate");
        dt.Columns.Add("BookingNumber");
        dt.Columns.Add("ProcessCost");
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);


        string strProcessList = string.Empty;
        for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
        {
            if (((CheckBox)grdProcessSelection.Rows[r].FindControl("chkSelect")).Checked)
            {
                strProcessList += "," + ((HiddenField)grdProcessSelection.Rows[r].FindControl("hdnProcessCode")).Value.Trim();
            }
        }
        if (strProcessList != "")
        {
            strProcessList = strProcessList.Substring(1);
        }

        string strItemList = "";
        for (int r = 0; r < grdItemSelection.Rows.Count; r++)
        {
            if (((CheckBox)grdItemSelection.Rows[r].FindControl("chkSelect")).Checked)
            {
                strItemList += "," + ((Label)grdItemSelection.Rows[r].FindControl("lblItemName")).Text.Trim();
            }
        }
        if (strItemList != "")
        {
            strItemList = strItemList.Substring(1);
        }

      
        ArrayList TotalAmount = new ArrayList();
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        ViewState["exlquery"] = null;
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text + " 00:00:00";
            DateTime dte = DateTime.Parse(txtReportUpto.Text);
            strToDate = dte.ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strGridCap = "Vendor Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");

        }
        else if (radReportMonthly.Checked)
        {
            DateTime dte = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dte.ToShortDateString() + " 00:00:00";
            strToDate = dte.AddMonths(1).ToShortDateString() + " 00:00:00";
            strGridCap = "Vendor report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }
        ShowVendorDetails(strProcessList, strFromDate, strToDate, strItemList);        
    }
    private void ShowVendorDetails(string strProcessCode, string strStartDate, string strToDate, string strItemName)
    {       
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_VendorReport";
        cmd.Parameters.Add(new SqlParameter("@BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("@BookDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("@ProcessCode", strProcessCode));
        cmd.Parameters.Add(new SqlParameter("@ItemName", strItemName));
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataAdapter sadp = new SqlDataAdapter();
        DataSet dsMain = new DataSet();
        try
        {
            sqlcon.Open();
            cmd.Connection = sqlcon;
            sadp.SelectCommand = cmd;
            sadp.Fill(dsMain);
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                }
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sqlcon = null;
        }
        if (dsMain.Tables.Count > 0)
        {
            grdVendorReport.DataSource = dsMain.Tables[0];
            grdVendorReport.DataBind();
            if (grdVendorReport.Rows.Count > 0)
            {
                CalculateGrid();
                btnExportToExcel.Visible = true;
            }
        }
        else
        {
            lblMsg.Text = "Could not get report.";
        }
    }
    private void CalculateGrid()
    {
        float TotalAmount = 0;
        float TotalPieces = 0;
        foreach (GridViewRow gdr in grdVendorReport.Rows)
        {
            TotalAmount += float.Parse(gdr.Cells[2].Text);
        }
        foreach (GridViewRow gdr in grdVendorReport.Rows)
        {
            TotalPieces += float.Parse(gdr.Cells[3].Text);
        }
        grdVendorReport.FooterRow.Cells[2].Text = TotalAmount.ToString();
        grdVendorReport.FooterRow.Cells[3].Text = TotalPieces.ToString();
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdVendorReport);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave,false);
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        FillGridView();
    }   
}
