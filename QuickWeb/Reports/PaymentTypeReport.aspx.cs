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
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using QuickWeb;

public partial class PaymentTypeReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProcessTypeGrid();
            for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
            {
                ((CheckBox)grdProcessSelection.Rows[r].Cells[0].FindControl("chkSelect")).Checked = true;
            }
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            btnShowReport_Click(null, null);
        }        
    }

    private void BindProcessTypeGrid()
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_EmployeeMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 10);
        ds = AppClass.GetData(cmd);
        grdProcessSelection.DataSource = ds.Tables[0];
        grdProcessSelection.DataBind();
        grdSecondGrid.DataSource = ds.Tables[1];
        grdSecondGrid.DataBind();
    }

    private bool checkCheckbox(GridView grdProcessSelection)
    {
        int totalSelected = 0;
        for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
        {
            if (((CheckBox)grdProcessSelection.Rows[r].Cells[0].FindControl("chkSelect")).Checked) totalSelected++;
        }
        if (totalSelected > 0)
            return true;
        else
            return false;
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        ViewState["exlquery"] = null;
        string strSqlQuery = string.Empty;
        bool status = false;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;

        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();

        try
        {
            string strAdvance = "", strDeliverString = "", Others = "False", PackageSale = "False", PackageBooking = "False";
            for (int r1 = 0; r1 < grdSecondGrid.Rows.Count; r1++)
            {
                if (((CheckBox)grdSecondGrid.Rows[r1].FindControl("chkSelect")).Checked)
                {
                    PackageBooking = "TRUE";
                }
            }
            for (int r = 0; r < grdProcessSelection.Rows.Count; r++)
            {
                if (((CheckBox)grdProcessSelection.Rows[r].FindControl("chkSelect")).Checked)
                {
                    var text = ((Label)grdProcessSelection.Rows[r].FindControl("lblProcessName")).Text.Trim();
                    if (text == "Other - Income")
                    {
                        Others = "TRUE";
                    }
                    else if (text == "Package - Sale")
                    {
                        PackageSale = "TRUE";
                    }
                    else
                    {
                        if (text.IndexOf("Advance") != -1)
                            strAdvance += text.Split('-')[1].Trim() + ",";
                        else
                            strDeliverString += text.Split('-')[1].Trim() + ",";
                    }
                }
            }
            if (strAdvance.Length != 0)
                strAdvance = strAdvance.Substring(0, strAdvance.Length - 1);
            if (strDeliverString.Length != 0)
                strDeliverString = strDeliverString.Substring(0, strDeliverString.Length - 1);
            GetDetails(strAdvance, strDeliverString, strFromDate, strToDate, status, Others, PackageSale, PackageBooking);
        }

        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {

        }       
    }

    private void GetDetails(string strAdvance, string strDeliverString, string FromDate, string UptoDate, bool status, string Others, string PackageSale, string PackageBooking)
    {
        SqlCommand cmd = new SqlCommand();
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
        DataSet ds = new DataSet();
        ds = BAL.BALFactory.Instance.BL_PriceList.PaymentTypeGetDetails(strAdvance, strDeliverString, FromDate, UptoDate, status, Others, Globals.BranchID, PackageSale, PackageBooking);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ReportViewer1.Visible = true;
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                ReportViewer1.ShowExportControls = true;
            }
            else
            {
                ReportViewer1.ShowExportControls = false;
            } 
            btnPrint.Visible = true;
            DataSet ds1 = new PaymentTypeDataSet();
            DateTime dt1, dt2;
            if (status == true)
            {
                dt1 = DateTime.Parse(FromDate);
                dt2 = DateTime.Parse(UptoDate);               
            }
            else
            {
                dt1 = DateTime.Parse(FromDate);
                dt2 = DateTime.Parse(UptoDate);
            }
            ds1 = ds;
            string str = "http://" + Request.Url.Authority;
            ReportViewer1.LocalReport.ReportPath = "RDLC/PaymentTypeReport.rdlc";
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportParameter[] parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("FDate", dt1.ToString("dd-MMM-yyyy"));
            parameters[1] = new ReportParameter("UDate", dt2.ToString("dd-MMM-yyyy"));
            parameters[2] = new ReportParameter("Link", str);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);

            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "PaymentTypeDataSet_sp_Payment";
            rds.Value = ds1.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblMsg.Text = "No record found.";
            ReportViewer1.Visible = false;
            btnPrint.Visible = false;
        }
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
    "  <PageWidth>11in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.2in</MarginTop>" +
    "  <MarginLeft>0.7in</MarginLeft>" +
    "  <MarginRight>0.5in</MarginRight>" +
    "  <MarginBottom>0.2in</MarginBottom>" +
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
}