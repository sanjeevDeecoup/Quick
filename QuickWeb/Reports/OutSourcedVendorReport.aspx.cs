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

public partial class VendorReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.DataBind();
            txtReportFrom.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            txtReportUpto.Text = DateTime.Today.ToShortDateString();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2009; i < 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2009;           
            
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
       
        string vendorid = string.Empty;
        vendorid = drpVendor.SelectedItem.Value;
        string strProcessList = "";
        
        if (strProcessList != "")
        {
            strProcessList = strProcessList.Substring(1);
        }

        string strItemList = "";
        
        if (strItemList != "")
        {
            strItemList = strItemList.Substring(1);
        }
       
        ArrayList TotalAmount = new ArrayList();

        string strFromDate = "", strToDate = "", strGridCap = "";
        ViewState["exlquery"] = null;
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text + " 00:00:00";
            DateTime dte = DateTime.Parse(txtReportUpto.Text);
            strToDate = dte.AddDays(1).ToShortDateString() + " 00:00:00";
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
        ShowVendorDetails(strFromDate, strToDate, vendorid);        
    }


    private void ShowVendorDetails(string strStartDate, string strToDate, string vendorid)
    {       
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Report_NewVendorReport";
        cmd.Parameters.Add(new SqlParameter("@BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("@BookDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("@VendorId", vendorid));     

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
        float TotalAmount = 0, TotalQty = 0;
        foreach (GridViewRow gdr in grdVendorReport.Rows)
        {
            TotalAmount += float.Parse(gdr.Cells[2].Text);
            TotalQty += float.Parse(gdr.Cells[3].Text);
        }
        grdVendorReport.FooterRow.Cells[2].Text = TotalAmount.ToString();
        grdVendorReport.FooterRow.Cells[3].Text = TotalQty.ToString();
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
