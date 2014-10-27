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
using Microsoft.Reporting.WebForms;


public partial class ChallanReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
           
            btnShowReport_Click(null, null);
        }
    }  

    protected void btnShowReport_Click(object sender, EventArgs e)
    {       
        string strSqlQuery = string.Empty;
        ViewState["exlquery"] = null;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        hdnFromDate.Value = strFromDate;
        hdnToDate.Value = strToDate;
        ShowChallanDetails(strFromDate, strToDate);
        if (grdReport.Rows.Count > 0)
        {
            grdReport.Visible = true;           
            ViewState["exlquery"] = strSqlQuery;           
        }      
    }
    private void ShowChallanDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        ViewState["Date"] = DateTime.Parse(strStartDate).ToString("dd MMM yyyy");
        ViewState["Date1"] = DateTime.Parse(strToDate).AddDays(-1).ToString("dd MMM yyyy");
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Report_ChallanReport";
        cmd.Parameters.Add(new SqlParameter("@ChallanDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("@ChallanDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
        cmd.Parameters.Add(new SqlParameter("@Flag", 3));
        
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
            lblErr.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sqlcon.Dispose();
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                string Challantype = dsMain.Tables[0].Rows[0]["ChallanType"].ToString();
                for (int i = 0; i < grdReport.Rows.Count; i++)
                {
                    PrjClass.SetItemInDropDown(((DropDownList)grdReport.Rows[i].FindControl("drpOption")), Challantype, true, false);
                }
            }
        }
    }

    protected void grdReport_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    
    private void ShowProcessCount()
    {
        string targetString = string.Empty;
        string NewVal = string.Empty;
        int count = 0;
        ArrayList arr = new ArrayList();
        int pos = -1;
        for (int r = 0; r < grdReport.Rows.Count; r++)
        {
            NewVal = "" + grdReport.Rows[r].Cells[3].Text.Replace("&nbsp;","");
            if (NewVal == "")
            {
                continue;
            }
            pos = -1;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() == NewVal)
                {
                    pos = i;
                    break;
                }
            }
            if (pos < 0)
            {
                count = 0;
                for (int row = 0; row < grdReport.Rows.Count; row++)
                {
                    if (grdReport.Rows[row].Cells[3].Text == NewVal)
                    {
                        count += int.Parse(grdReport.Rows[row].Cells[5].Text);
                    }
                }
                targetString += NewVal + "(" + count + ")<br/>";
                arr.Add(NewVal);
            }
        }
        grdReport.FooterRow.Cells[3].Text = targetString;
        targetString = "";
        arr = new ArrayList();
        for (int r = 0; r < grdReport.Rows.Count; r++)
        {
            NewVal = "" + grdReport.Rows[r].Cells[4].Text.Replace("&nbsp;", "");
            if (NewVal == "")
            {
                continue;
            }
            pos = -1;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() == NewVal)
                {
                    pos = i;
                    break;
                }
            }
            if (pos < 0)
            {
                count = 0;
                for (int row = 0; row < grdReport.Rows.Count; row++)
                {
                    if (grdReport.Rows[row].Cells[4].Text == NewVal)
                    {
                        count += int.Parse(grdReport.Rows[row].Cells[5].Text);
                    }
                }
                targetString += NewVal + "(" + count + ")<br/>";
                arr.Add(NewVal);
            }
        }
        grdReport.FooterRow.Cells[4].Text = targetString;
    }

   

   
    public void OpenNewWindow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }
    protected void btnShowChallan_Click(object sender, EventArgs e)
    {
        LinkButton ddl = (LinkButton)sender;
        GridViewRow gv = ((GridViewRow)ddl.NamingContainer);
        int row = gv.RowIndex;
        if (((DropDownList)gv.FindControl("drpOption")).SelectedItem.Text != "--Select--")
        {
            if (((DropDownList)gv.FindControl("drpOption")).SelectedItem.Text == "Invoice Based Detailed")
                OpenNewWindow("ChallanSummary.aspx?Date=" + ViewState["Date"].ToString() + "&Date1=" + ViewState["Date1"].ToString() + "&Challan=" + grdReport.Rows[row].Cells[1].Text + "");
            if (((DropDownList)gv.FindControl("drpOption")).SelectedItem.Text == "Invoice Based")
                OpenNewWindow("ItemWise.aspx?Date=" + ViewState["Date"].ToString() + "&Date1=" + ViewState["Date1"].ToString() + "&Challan=" + grdReport.Rows[row].Cells[1].Text + "");
            if (((DropDownList)gv.FindControl("drpOption")).SelectedItem.Text == "Invoice With Item Detailed")
                OpenNewWindow("InvoicewithItemDetail.aspx?Date=" + ViewState["Date"].ToString() + "&Date1=" + ViewState["Date1"].ToString() + "&Challan=" + grdReport.Rows[row].Cells[1].Text + "");
        }
    }

    protected void grdReport_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
