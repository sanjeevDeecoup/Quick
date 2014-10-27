using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Reports_AllDelivery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtReportFrom.Text = DateTime.Today.ToShortDateString();
            txtReportUpto.Text = DateTime.Today.ToShortDateString();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2009; i < 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
            string strUserType = "" + Session["UserType"];
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string strSqlQuery = "";
        ViewState["exlquery"] = null;
        string strFromDate = "", strToDate = "", strGridCap = "";
        if (radReportFrom.Checked)
        {
            strFromDate = txtReportFrom.Text;
            strFromDate = strFromDate + " 00:00:00";
            if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            strToDate = dt.AddDays(1).ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strGridCap = "Delivery Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");

        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToShortDateString() + " 00:00:00";
            strToDate = dt.AddMonths(1).ToShortDateString() + " 00:00:00";
            strGridCap = "Delivery Report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }

        ShowDeliveryDetails(strFromDate, strToDate);
        if (grdReport.Rows.Count > 0)
        {
            grdReport.Caption = strGridCap;
            btnExport.Visible = true;
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
        }
        else
        {
            btnExport.Visible = false;
        }
    }

    private void ShowDeliveryDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_AllDeliveryReport";
        cmd.Parameters.Add(new SqlParameter("BookingDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookingDate2", strToDate));

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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strRepTitle = grdReport.Caption;
        string[] Resp = PrjClass.GenerateExcelReportFromGridView(grdReport, strRepTitle);
        if (Resp[0] == "1")
            Response.Redirect(Resp[1]);
        else
        {
            lblMsg.Text = Resp[1];
            Resp = PrjClass.GenerateCSVReportFromGridView(grdReport, strRepTitle);
            if (Resp[0] == "1")
                Response.Redirect(Resp[1]);
            else
                lblMsg.Text = " Could not provide Report at the time. Please try after some time." + Resp[1];
        }
    }

}
