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

public partial class DeliveryReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["BID"] != null)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Session["BID"].ToString());
                txtReportFrom.Text = Convert.ToDateTime(date[0].ToString()).ToString("dd MMM yyyy");
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2009; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
                string strUserType = "" + Session["UserType"];
                btnShowReport_Click(null, null);
                txtCustomerWise.Focus();
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
        string strSqlQuery=string.Empty;
        ViewState["exlquery"] = null;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        if (radReportFrom.Checked)
        {            
            
		if(txtReportUpto.Text==""){txtReportUpto.Text=txtReportFrom.Text;}
            DateTime dt = DateTime.Parse(txtReportUpto.Text);
            //strToDate = dt.ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strFromDate = txtReportFrom.Text + " 00:00:00";
            strToDate = txtReportUpto.Text + " 00:00:00";
            strGridCap = "Delivery Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
            
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
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
          //  btnExport.Visible = false;
            grdReport.Caption = "";
        }        
    }

    private void ShowDeliveryDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_DeliveryReport";
        cmd.Parameters.Add(new SqlParameter("BookingDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookingDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("BranchId",Session["BID"].ToString()));
        if (txtCustomerWise.Text != "")
        {
            cmd.Parameters.Add(new SqlParameter("@BookingNumber", txtCustomerWise.Text));

            cmd.Parameters.Add(new SqlParameter("@Flag", 1));
        }
        else
            cmd.Parameters.Add(new SqlParameter("@Flag", 2));
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
                    CalculateGridReport();
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
    protected void rdbCustomerWise_CheckedChanged(object sender, EventArgs e)
    {
        txtCustomerWise.Visible = true;
        txtMobileNo.Visible = false;
        txtCustomerWise.Focus();
        lblName.Text = "Invoice Number : ";
    }
    protected void rdbMobileWise_CheckedChanged(object sender, EventArgs e)
    {
        txtMobileNo.Visible = true;
        txtCustomerWise.Visible = false;
        txtMobileNo.Focus();
        lblName.Text = "Customer Mobile No.";
    }

    public bool CheckCorrectData(string NameMobile,string Flag)
    {
        bool status = false;
        SqlDataReader sdr = null;
        SqlCommand cmd = new SqlCommand();
        try
        {
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", Flag);
            cmd.Parameters.AddWithValue("@SearchText", NameMobile);
            cmd.Parameters.AddWithValue("@BranchId", Session["BID"].ToString());
            sdr = AppClass.ExecuteReader(cmd);
            if (sdr.Read())
                status = true;
            else
            {
                status = false;
            }
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        return status;
    }

    public string ReturnCorrectData(string TextSearch)
    {
        SqlCommand cmd = new SqlCommand();
        string SearchText = "";
        SqlDataReader sdr = null;
        try
        {
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 25);
            cmd.Parameters.AddWithValue("@SearchText", TextSearch);
            cmd.Parameters.AddWithValue("@BranchId", Session["BID"].ToString());
            sdr = AppClass.ExecuteReader(cmd);
            if (sdr.Read())
            {
                SearchText = "" + sdr.GetValue(0);
            }
            else
            {
                SearchText = "";
            }
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        return SearchText;
    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float St = 0, Ad = 0, Bal = 0, OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0;
        for (int r = 0; r < rc; r++)
        {
            OrderCount++;
            if (grdReport.Rows[r].Cells[3].Text.Trim().ToUpper() != "CANCELLED")
            {
                TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[4].Text);
                TotalPaid += float.Parse("0" + grdReport.Rows[r].Cells[5].Text);
                TotalDue += float.Parse("0" + grdReport.Rows[r].Cells[6].Text);

                St += float.Parse("0" + grdReport.Rows[r].Cells[7].Text);
                Ad += float.Parse("0" + grdReport.Rows[r].Cells[9].Text);
                Bal += float.Parse("0" + grdReport.Rows[r].Cells[10].Text);

                BalanceAmount += float.Parse("0" + grdReport.Rows[r].Cells[8].Text);
            }
        }
        grdReport.FooterRow.Cells[1].Text = OrderCount.ToString();
        grdReport.FooterRow.Cells[4].Text = TotalCostCount.ToString();
        grdReport.FooterRow.Cells[5].Text = TotalPaid.ToString();
        grdReport.FooterRow.Cells[6].Text = TotalDue.ToString();
        grdReport.FooterRow.Cells[8].Text = BalanceAmount.ToString();
        grdReport.FooterRow.Cells[7].Text = St.ToString();
        grdReport.FooterRow.Cells[9].Text = Ad.ToString();
        grdReport.FooterRow.Cells[10].Text = Bal.ToString();
    }
}
