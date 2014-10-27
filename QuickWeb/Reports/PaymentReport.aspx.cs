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


public partial class PaymentReport : System.Web.UI.Page
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
                for (int i = 2009; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
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
        string strSqlQuery="";
        ViewState["exlquery"] = null;
        string strFromDate = "", strToDate = "", strGridCap = "";
        if (radReportFrom.Checked)
        {           
		if(txtReportUpto.Text==""){txtReportUpto.Text=txtReportFrom.Text;}
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            //strToDate = dt.ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strFromDate = txtReportFrom.Text + " 00:00:00";
            strToDate = txtReportUpto.Text + " 00:00:00";
            strGridCap = "Payment Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
            
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToShortDateString() + " 00:00:00";
            strToDate = dt.AddMonths(1).ToShortDateString() + " 00:00:00";
            strGridCap = "Payment Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }        
        ShowBookingDetails(strFromDate, strToDate);        
        if (grdReport.Rows.Count > 0)
        {
            CalculateGridReport();
            ViewState["exlquery"] = strSqlQuery;
            btnExport.Visible = true;
            grdReport.Visible = true;
        }
        else
        {
            btnExport.Visible = false;
            grdReport.Visible = true;
        }
    }

    private void ShowBookingDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_PaymentReport";
        cmd.Parameters.Add(new SqlParameter("@PaymentDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("@PaymentDate2", strToDate));

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
            sqlcon = null;
        }

    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float BookingCount = 0, TotalBookingAmountCount = 0, TotalNetPaidCount = 0, TotalDueCount = 0;
        for (int r = 0; r < rc; r++)
        {
            BookingCount++;
            TotalBookingAmountCount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text.Replace("&nbsp;",""));
            TotalNetPaidCount += float.Parse("0" + grdReport.Rows[r].Cells[3].Text.Replace("&nbsp;", ""));
            TotalDueCount += float.Parse("0" + grdReport.Rows[r].Cells[4].Text.Replace("&nbsp;", ""));
        }
        grdReport.FooterRow.Cells[1].Text = BookingCount.ToString();
        grdReport.FooterRow.Cells[3].Text = TotalNetPaidCount.ToString();
        grdReport.FooterRow.Cells[4].Text = TotalDueCount.ToString();  
    }

       
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;

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
