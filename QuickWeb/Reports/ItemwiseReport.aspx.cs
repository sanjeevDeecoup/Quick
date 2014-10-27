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
using System.Text;
using System.IO;

public partial class ItemwiseReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SqlSourceItems.SelectCommand = "SELECT [ItemID], [ItemName] FROM [ItemMaster] Where BranchId='" + Globals.BranchID + "' ORDER BY ItemName";
            SqlSourceItems.DataBind();
            drpItemNames.Focus();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2000; i <= 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
            //btnShowReport_Click(null, null);
        }
    }    

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strReportCaption = string.Empty, strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;        
        if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            strGridCap = "Booking Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }

        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        ShowBookingDetails(strFromDate, strToDate);
        //checkCancelBooking(strSqlQuery);
        checkCancelBooking(); 
        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport();
            btnExport.Visible = true;
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
            
        }
        else
        {
            btnExport.Visible = false;
        }
    }

    private void ShowBookingDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        string ItemName = "";
        string[] array;
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_ItemWiseReport";
        cmd.Parameters.Add(new SqlParameter("BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("ItemName", drpItemNames.SelectedItem.Text));
        cmd.Parameters.Add(new SqlParameter("BranchId", Globals.BranchID));

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
                    DataTable dt = new DataTable();// 
                    DataRow dr = null;                    
                    DataSet ds1 = new DataSet("table");
                    string headerText = "BookingDate,BookingNO,DeliveryDate,QTY,RATE";
                    string[] header = headerText.Split(',');
                    dt = new DataTable();
                    for (int iColumns = 0; iColumns < 5; iColumns++)
                        dt.Columns.Add(header[iColumns]);
                    for (int iRow = 0; iRow < dsMain.Tables[0].Rows.Count; iRow++)
                    {
                        dr = dt.NewRow();
                        dr[0] = dsMain.Tables[0].Rows[iRow]["BookingDate"].ToString();
                        dr[1] = dsMain.Tables[0].Rows[iRow]["BookingNumber"].ToString();
                        dr[2] = dsMain.Tables[0].Rows[iRow]["Deliverydate"].ToString();
                        ItemName = dsMain.Tables[0].Rows[iRow]["ItemQuantityAndRate"].ToString();
                        array = ItemName.Split('@');
                        dr[3] = array[0].ToString();
                        dr[4] = array[1].ToString();
                        dt.Rows.Add(dr);
                    }
                    dt.AcceptChanges();
                    grdReport.DataSource = dt;
                    grdReport.DataBind();
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
    }
    private void CalculateGridReport()
    {
        try
        {
            int rc = grdReport.Rows.Count;
            int cc = grdReport.Columns.Count;
            float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0;
            for (int r = 0; r < rc; r++)
            {
                OrderCount++;
                if (grdReport.Rows[r].Cells[3].Text.Trim().ToUpper() != "CANCELLED")
                {
                    TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                    TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);
                }
            }
            grdReport.FooterRow.Cells[3].Text = TotalCostCount.ToString();
            grdReport.FooterRow.Cells[4].Text = TotalPaid.ToString();
        }
        catch (Exception ex) { }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
		GridView grd=new GridView();
		grd.DataSource=(DataSet)ViewState["SavedDS"];
		grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave,false);
    }    
   
    private void checkCancelBooking()
    {
        //string strStat="";
        //for (int r = 0; r < grdReport.Rows.Count; r++)
        //{
        //    strStat = "" + ((HiddenField)grdReport.Rows[r].FindControl("hidBookingStatus")).Value;
        //    if (strStat == "5")
        //    {
        //        grdReport.Rows[r].BackColor = System.Drawing.Color.Yellow;
        //        grdReport.Rows[r].Cells[2].Text = "Cancelled";
        //        grdReport.Rows[r].Cells[3].Text = "Cancelled";
        //        grdReport.Rows[r].Cells[4].Text = "0";
        //    }
        //}
    }

}
