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

public partial class NewAreaLocationReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtAreaLocation.Focus();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2000; i <= 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2000;            
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        string strUserType = "" + Session["UserType"];
        if (strUserType != "1")
        {
            grdReport.ShowFooter = false;
            btnExport.Visible = false;
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;        
        if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToShortDateString() + " 00:00:00";
            strToDate = dt.AddMonths(1).ToShortDateString() + " 00:00:00";
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
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_AreaWiseClothBookingReport";
        cmd.Parameters.Add(new SqlParameter("BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("AreaType", txtAreaLocation.Text));        

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
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
                    ViewState["SavedDS"]=dsMain;
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
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float OrderCount = 0, TotalCostCount = 0;
        for (int r = 0; r < rc; r++)
        {
            OrderCount++;
            if (grdReport.Rows[r].Cells[1].Text.Trim().ToUpper() != "CANCELLED")
            {
                TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text);                
            }
        }
        grdReport.FooterRow.Cells[1].Text = TotalCostCount.ToString();       
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
    //private void checkCancelBooking(string sql)
    //{
    //    SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
    //    SqlDataReader sdr = null;
    //    string str = "";
    //    try
    //    {
    //        sqlcon.Open();
    //        SqlCommand cmd = new SqlCommand();
    //        cmd.Connection = sqlcon;
    //        for (int r = 0; r < grdReport.Rows.Count; r++)
    //        {
    //            sql = "Select BookingStatus From EntBookings Where BookingNumber='" + grdReport.Rows[r].Cells[1] + "'";
    //            cmd.CommandText = sql;
    //            sdr = cmd.ExecuteReader();
    //            if (sdr.Read())
    //            {
    //                str = "" + sdr.GetValue(0);
    //                if (str == "5")
    //                {
    //                    grdReport.Rows[r].BackColor = System.Drawing.Color.Yellow;
    //                    grdReport.Rows[r].Cells[2].Text = "Cancelled";
    //                    grdReport.Rows[r].Cells[3].Text = "Cancelled";
    //                    grdReport.Rows[r].Cells[4].Text = "0";
    //                }
    //            }
    //            sdr.Close();
    //        }
    //    }
    //    catch (Exception excp)
    //    {
    //        if (sdr != null)
    //        {
    //            sdr.Close();
    //        }

    //    }
    //    finally
    //    {
    //        sqlcon.Close();
    //    }
    //}
   
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
