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

public partial class TimeandColthReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["BID"] != null)
        {
            if (!IsPostBack)
            {
                txtAreaLocation.Focus();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Session["BID"].ToString());
                txtReportFrom.Text = date[0].ToString();
                txtReportUpto.Text = date[0].ToString();
            }
        }
        else
        {
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl, false);
        }

    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string  strSqlQuery = string.Empty;

        string strFromDate = string.Empty, strToDate = string.Empty;
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            strToDate = dt.AddDays(1).ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            //strGridCap = "Booking Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }

        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;
        ShowBookingDetails(strFromDate, strToDate);
        //checkCancelBooking(strSqlQuery);
      
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
        
        //BookingTime = txtAreaLocation.Text + " " + drpTime.SelectedItem.Text;
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_TimeWiseClothBookingReport";
        cmd.Parameters.Add(new SqlParameter("BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("BookingTime", txtAreaLocation.Text));
        cmd.Parameters.Add(new SqlParameter("BookingTime1", txtToArea.Text));
        cmd.Parameters.Add(new SqlParameter("Format", drpTime.SelectedItem.Text));

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
                    ViewState["SavedDS"] = dsMain;
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
            sqlcon.Dispose();
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
        GridView grd = new GridView();
        grd.DataSource = (DataSet)ViewState["SavedDS"];
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
   
    

}
