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
using Microsoft.Reporting.WebForms;

public partial class Accounts_BookingByCustomerReport : System.Web.UI.Page
{   
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {                          
            // this is for the qurey string that is passed from customer wise business volume (or accounts receivable)
            if (Request.QueryString["BC"] != null && Request.QueryString["BC"].ToString() != "")
            {                
                //// set the leder name
                lblCustomerLedgerName.Text = Request.QueryString["BC"].ToString().Split(',')[0];
                hdnDateFromAndTo.Value = Request.QueryString["BC"].ToString().Split(',')[1] + " - " + Request.QueryString["BC"].ToString().Split(',')[2];
                lblCustName.Text = BAL.BALFactory.Instance.BL_CustomerMaster.GetCustNameFromCode(lblCustomerLedgerName.Text.Trim(), Globals.BranchID);
                //// now call the grid loader function
                btnShowReport_Click(null, EventArgs.Empty);               
            }
        }
    }   
    string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string strSqlQuery = string.Empty;
        btnExport.Visible = false;
        btnPrint.Visible = false;        
        grdCustomerWiseReport.DataSource = null;
        grdCustomerWiseReport.DataBind();

        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        hdnStartDate.Value = strFromDate;
        hdnEndDate.Value = strToDate;       
        ShowBookingDetails(strFromDate, strToDate, lblCustomerLedgerName.Text);
        grdCustomerWiseReport.Visible = true;
        if (grdCustomerWiseReport.Rows.Count > 0)
        {
            CalculateGridReport();
            btnExport.Visible = true;
            btnPrint.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }

        }
    }
    private void ShowBookingDetails(string strStartDate, string strToDate,string CustId)
    {
        DataSet dsMain = new DataSet();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_QuantityandBooking";
        cmd.Parameters.Add(new SqlParameter("BookDate1", strStartDate));
        cmd.Parameters.Add(new SqlParameter("BookDate2", strToDate));
        cmd.Parameters.Add(new SqlParameter("CustId", CustId));
        cmd.Parameters.Add(new SqlParameter("BranchId", Globals.BranchID));
        cmd.Parameters.Add(new SqlParameter("Flag", 2));
        dsMain = PrjClass.GetData(cmd);       
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);        
        try
        {           
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    ReportViewer1.LocalReport.ReportPath = "RDLC/Statement.rdlc";
                    ReportDataSource rds = new ReportDataSource();
                    ReportParameter[] parameters = new ReportParameter[2];
                    parameters[0] = new ReportParameter("UserName", Globals.UserName);
                    parameters[1] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
                    //parameters[2] = new ReportParameter("Link", str);
                    ReportViewer1.LocalReport.SetParameters(parameters);
                    rds.Name = "DataSet1";
                    rds.Value = dsMain.Tables[0];
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                    grdCustomerWiseReport.DataSource = dsMain.Tables[0];
                    grdCustomerWiseReport.DataBind();
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
           
        }
    }    
    private void CalculateGridReport()
    {
        try
        {
            int rc = grdCustomerWiseReport.Rows.Count;
            int cc = grdCustomerWiseReport.Columns.Count;
            float Paid = 0, St = 0, Ad = 0, Bal = 0, OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0, RemBal = 0, deldis = 0;
            for (int r = 0; r < rc; r++)
            {
                OrderCount++;
                if (grdCustomerWiseReport.Rows[r].Cells[3].Text.Trim().ToUpper() != "CANCELLED")
                {
                    TotalCostCount += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[4].Text);
                    TotalPaid += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[5].Text);
                    TotalDue += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[6].Text);
                    St += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[7].Text);
                    Ad += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[9].Text);
                    Bal += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[10].Text);
                    Paid += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[11].Text);
                    BalanceAmount += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[8].Text);
                    deldis += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[12].Text);
                    RemBal += float.Parse("0" + grdCustomerWiseReport.Rows[r].Cells[13].Text);
                }
            }
            grdCustomerWiseReport.FooterRow.Cells[1].Text = OrderCount.ToString();         
            grdCustomerWiseReport.FooterRow.Cells[5].Text = TotalPaid.ToString();
            grdCustomerWiseReport.FooterRow.Cells[6].Text = TotalDue.ToString();
            grdCustomerWiseReport.FooterRow.Cells[8].Text = BalanceAmount.ToString();
            grdCustomerWiseReport.FooterRow.Cells[7].Text = St.ToString();
            grdCustomerWiseReport.FooterRow.Cells[9].Text = Ad.ToString();
            grdCustomerWiseReport.FooterRow.Cells[10].Text = Bal.ToString();
            grdCustomerWiseReport.FooterRow.Cells[11].Text = Paid.ToString();
            grdCustomerWiseReport.FooterRow.Cells[12].Text = deldis.ToString();
            grdCustomerWiseReport.FooterRow.Cells[13].Text = Math.Round(RemBal, 2).ToString();
        }
        catch (Exception ex) { }
    }   
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Booking by customer report from " + hdnStartDate.Value + " to " + hdnEndDate.Value + ".xls";
        Response.Expires = 0;
        Response.Buffer = true;

        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdCustomerWiseReport, hdnStartDate.Value, hdnEndDate.Value, "Booking by customer report - " + lblCustName.Text, false);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string reportType = "PDF";
        string mimeType;
        string encoding;
        string fileNameExtension;
        string deviceInfo =
    "<DeviceInfo>" +
    "  <OutputFormat>PDF</OutputFormat>" +
    "  <PageWidth>9.5in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.5in</MarginTop>" +
    "  <MarginLeft>0.2n</MarginLeft>" +
    "  <MarginRight>0.5in</MarginRight>" +
    "  <MarginBottom>.5in</MarginBottom>" +
    "</DeviceInfo>";
        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;
        renderedBytes = ReportViewer1.LocalReport.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);
        var fileName = "OutPut.pdf";
        PrjClass.ShowPdfFromRdlc(this.Page, renderedBytes, fileName);  
        //Response.Clear();
        //Response.ContentType = mimeType;
        //Response.BinaryWrite(renderedBytes);
        //Response.End();
    
    }    
    string strPrinterName = string.Empty;
    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drview = e.Row.DataItem as DataRowView;
        if (strPrinterName == "")
        {
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypBookingNo = (HyperLink)e.Row.FindControl("hypBtnShowDetails");
            string strBookinNo = hypBookingNo.Text;
            Label lblDueDate = (Label)e.Row.FindControl("lblDate");
            string strDuedate = lblDueDate.Text;
            hypBookingNo.NavigateUrl = String.Format("~/" + strPrinterName + "/BookingSlip.aspx?BN={0}{1}{2}", strBookinNo, "-1", strDuedate);
        }
    }
}
