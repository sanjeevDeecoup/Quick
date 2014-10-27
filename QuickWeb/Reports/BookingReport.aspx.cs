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

public partial class BookingReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["BID"] != null)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Session["BID"].ToString());
                txtReportFrom.Text = date[0].ToString();
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2000; i <= 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
                btnShowReport_Click(null, null);
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
            grdReport.ShowFooter = false;
            btnExport.Visible = false;
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        bool rights = AppClass.GetShowFooterRightsUser();
        string strReportCaption = string.Empty, strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            strToDate = dt.AddDays(1).ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }
        else if (radReportMonthly.Checked)
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
        cmd.CommandText = "Sp_Sel_BookingReport";
        cmd.Parameters.Add(new SqlParameter("BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookDate2", strToDate));

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
            
            sadp.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
        }

    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, Discount = 0;
        for (int r = 0; r < rc; r++)
        {
            OrderCount++;
            if (grdReport.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
            {
                TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);
                Discount += float.Parse("0" + grdReport.Rows[r].Cells[5].Text);
            }
        }
        grdReport.FooterRow.Cells[1].Text = OrderCount.ToString();
        grdReport.FooterRow.Cells[2].Text = TotalCostCount.ToString();
        grdReport.FooterRow.Cells[3].Text = TotalPaid.ToString();
        grdReport.FooterRow.Cells[4].Text = TotalDue.ToString();
        grdReport.FooterRow.Cells[5].Text = Discount.ToString();
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

        string strFilePathToSave = string.Empty;
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
		string strStat=string.Empty;
        for (int r = 0; r < grdReport.Rows.Count; r++)
        {
            strStat = "" + ((HiddenField)grdReport.Rows[r].FindControl("hidBookingStatus")).Value;
            if (strStat == "5")
            {
                grdReport.Rows[r].BackColor = System.Drawing.Color.Yellow;
                grdReport.Rows[r].Cells[2].Text = "Cancelled";
                grdReport.Rows[r].Cells[3].Text = "Cancelled";
                grdReport.Rows[r].Cells[4].Text = "0";
            }
        }
    }

}
