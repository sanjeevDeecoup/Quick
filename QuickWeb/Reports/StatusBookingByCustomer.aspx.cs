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

public partial class StatusBookingByCustomer : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.DataBind();
            if (grdChallan.Rows.Count > 0)
            {
                CalculateGrid();
                btnExportToExcel.Visible = true;
            }
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            txtReportFrom.Text = date[0].ToString();
            txtReportUpto.Text = date[0].ToString();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2009; i < 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
        }
    }
    private void CalculateGrid()
    {
        //float Sent = 0, Received = 0, Pending = 0;
        //for (int r = 0; r < grdChallan.Rows.Count; r++)
        //{
        //    Sent += float.Parse("0" + ((Label)grdChallan.Rows[r].Cells[7].FindControl("lblTotalQtySent")).Text);
        //    Received += float.Parse("0" + ((Label)grdChallan.Rows[r].Cells[8].FindControl("lblTotalQtyReceived")).Text);
        //    Pending += float.Parse("0" + ((Label)grdChallan.Rows[r].Cells[9].FindControl("txtItemsReceived")).Text);
        //}
        //grdChallan.FooterRow.Cells[7].Text = Sent.ToString();
        //grdChallan.FooterRow.Cells[8].Text = Received.ToString();
        //grdChallan.FooterRow.Cells[9].Text = Pending.ToString();
    }

    protected void grdChallan_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {       
        if (e.CommandName == "AddOneItem")
        { int rownum = int.Parse(e.CommandArgument.ToString());
            int Lastitemcount = int.Parse("0" + ((TextBox)grdChallan.Rows[rownum].Cells[5].FindControl("txtItemsReceived")).Text);
            ((Label)grdChallan.Rows[rownum].Cells[5].FindControl("txtItemsReceived")).Text = (Lastitemcount+1).ToString();
        }        
        else if (e.CommandName == "ShowPriority")
        {
            int rownum = int.Parse(e.CommandArgument.ToString());
            Label lblPriority = (Label)grdChallan.Rows[rownum].Cells[1].FindControl("lblPriority");
            lblPriority.Visible = !lblPriority.Visible;
        }
    }

    private void BindGrid(string strStartDate, string strToDate, string vendorid)    
    {
		grdChallan.DataSource=null;
		grdChallan.DataBind();        
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand();
        DataSet dsMain = new DataSet();
        try
        {
            sqlcon.Open();
            cmd.Connection = sqlcon;
            cmd.CommandText = "Sp_Sel_CustomerStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BookDate1", strStartDate));
            cmd.Parameters.Add(new SqlParameter("@BookDate2", strToDate));
            cmd.Parameters.Add(new SqlParameter("@CustomerId", vendorid));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));
            SqlDataAdapter sadp = new SqlDataAdapter(cmd);
            sadp.Fill(dsMain);
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sqlcon.Dispose();
        }
        if (dsMain.Tables.Count > 0)
        {
            ShowGridForDs(dsMain);           
        }
    }

    private void ShowGridForDs(DataSet dsMain)
    {
        DataTable dtTmp = new DataTable();
        dtTmp.Columns.Add("BookingNumber");
        //dtTmp.Columns.Add("ChallanNumber");
        dtTmp.Columns.Add("ISN");
        dtTmp.Columns.Add("SubItemName");
        dtTmp.Columns.Add("Qty");
        dtTmp.Columns.Add("Urgent");
        dtTmp.Columns.Add("ItemProcessType");
        dtTmp.Columns.Add("ItemExtraProcessType1");
        dtTmp.Columns.Add("ItemExtraProcessType2");
        dtTmp.Columns.Add("ItemTotalQuantitySent");
        dtTmp.Columns.Add("ItemsReceivedFromVendor");
		dtTmp.Columns.Add("ItemsPending");
        dtTmp.Columns.Add("ChallanDate");
        
        DataRow drTmp = null;
        
        if (dsMain.Tables.Count > 0)
        {
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r < dsMain.Tables[0].Rows.Count; r++)
                {
                    drTmp = dtTmp.NewRow();
                    drTmp["BookingNumber"] = "" + dsMain.Tables[0].Rows[r]["BookingNumber"].ToString();
                    drTmp["ChallanDate"] = "" + dsMain.Tables[0].Rows[r]["BookingDate"].ToString();
                    drTmp["ISN"] = "" + dsMain.Tables[0].Rows[r]["ISN"].ToString();
                    drTmp["SubItemName"] = "" + dsMain.Tables[0].Rows[r]["ItemName"].ToString();
                    drTmp["Qty"] = "" + dsMain.Tables[0].Rows[r]["Qty"].ToString();
                    drTmp["Urgent"] = "" + dsMain.Tables[0].Rows[r]["IsUrgent"].ToString();
                    drTmp["ItemProcessType"] = "" + dsMain.Tables[0].Rows[r]["ItemProcessType"].ToString();
                    //drTmp["ItemExtraProcessType1"] = "" + dsMain.Tables[0].Rows[r]["ItemExtraProcessType1"].ToString();
                    //drTmp["ItemExtraProcessType2"] = "" + dsMain.Tables[0].Rows[r]["ItemExtraProcessType2"].ToString();
                    drTmp["ItemTotalQuantitySent"] = "" + dsMain.Tables[0].Rows[r]["ItemTotalQuantity"].ToString();
                    drTmp["ItemsReceivedFromVendor"] = "" + dsMain.Tables[0].Rows[r]["DeliveredQty"].ToString();
                    drTmp["ItemsPending"] = "" + dsMain.Tables[0].Rows[r]["ItemsPending"].ToString();                    
                    dtTmp.Rows.Add(drTmp);
                }
                dtTmp.AcceptChanges();
                grdChallan.DataSource = dtTmp;
                grdChallan.DataBind();
               
            }
        }
    }    
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;

        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdChallan);

        string strFilePathToSave = string.Empty;
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
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        ViewState["exlquery"] = null;
        if (radReportFrom.Checked)
        {
            //strFromDate = txtReportFrom.Text + " 00:00:00";
            DateTime dte = DateTime.Parse(txtReportUpto.Text);
            //strToDate = dte.ToShortDateString() + " 00:00:00";
            DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
            DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
            strFromDate = txtReportFrom.Text + " 00:00:00";
            strToDate = txtReportUpto.Text + " 00:00:00";
            strGridCap = "Vendor Report from " + dt1.ToString("dd-MMM-yyyy") + " to " + dt2.ToString("dd-MMM-yyyy");
        }
        else if (radReportMonthly.Checked)
        {
            DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
            strFromDate = dt.ToString("dd MMM yyyy");
            strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            strGridCap = "Vendor report for " + drpMonthList.SelectedItem.Text + ", " + drpYearList.SelectedItem.Text;
        }
        string vendorid = drpVendor.SelectedItem.Value;
        BindGrid(strFromDate, strToDate, vendorid);
        if (grdChallan.Rows.Count > 0)
        {
            grdChallan.Caption = "Update Item Return from vendor.";
            CalculateGrid();
            btnExportToExcel.Visible = true;
        }
        else
            grdChallan.Caption = "";
    }
}
