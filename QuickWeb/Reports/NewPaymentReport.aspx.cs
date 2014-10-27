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
using Microsoft.Reporting.WebForms;


public partial class Reports_NewPaymentReport : System.Web.UI.Page
{
    ArrayList date = new ArrayList();
    public static bool blnRight = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);          
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            hdnDate.Value = date[0].ToString();
            btnShowReport_Click(null, null);
            txtCName.Focus();
        }        
    }   
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        DataSet ds = new DataSet();       
        if (txtInvoiceNo.Text.Trim() != "")
        {
            if (drpSelectOption.SelectedValue == "1")
                ds = BAL.BALFactory.Instance.Bal_Report.GetDeliveryByInvoiceNo(Ob);
            if (drpSelectOption.SelectedValue == "2")
                ds = BAL.BALFactory.Instance.Bal_Report.GetSaleByInvoiceNo(Ob);
            if (drpSelectOption.SelectedValue == "3")
                ds = BAL.BALFactory.Instance.Bal_Report.GetDeliveryAndSalesInvoiceNo(Ob);
        }
        else
        {
            var DateFromAndToTmp = hdnDateFromAndTo.Value.Split('-');
            Ob.FromDate = DateFromAndToTmp[0].Trim();
            Ob.UptoDate = DateFromAndToTmp[1].Trim();
            ds = GetData(Ob);
        }        

        ReportViewer1.Reset();
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
            if (drpSelectOption.SelectedValue == "1")
                ReportViewer1.LocalReport.ReportPath = "RDLC/DeliveryReport.rdlc";
            if (drpSelectOption.SelectedValue == "2")
                ReportViewer1.LocalReport.ReportPath = "RDLC/SalesReport.rdlc";
            if (drpSelectOption.SelectedValue == "3")
                ReportViewer1.LocalReport.ReportPath = "RDLC/SalesDel.rdlc";
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            bool rights = AppClass.GetShowFooterRightsUser();
            string rvalue = string.Empty;
            if (rights == true)
                rvalue = "1";
            else
                rvalue = "0";
            ReportParameter[] parameters = new ReportParameter[7];
            string str = Request.Url.Authority;
            string newstr = str + Request.ApplicationPath;
            parameters[0] = new ReportParameter("FromDate", Ob.FromDate);
            parameters[1] = new ReportParameter("ToDate", Ob.UptoDate);
            parameters[2] = new ReportParameter("StrLink", newstr);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
            parameters[5] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[6] = new ReportParameter("TotalFooter", rvalue);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            if (drpSelectOption.SelectedValue == "1")
                rds.Name = "ReportDelivery_Delivery";
            if (drpSelectOption.SelectedValue == "2")
                rds.Name = "ReportDelivery_Sales";
            if (drpSelectOption.SelectedValue == "3")
                rds.Name = "ReportDelivery_SalesDel";
            rds.Value = ds.Tables[0];
                     
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.DataBind();
            ReportViewer1.LocalReport.Refresh();
 		string drpvalue = drpSelectOption.SelectedValue;
            hdp_arr.Value = "";
            hdd_arr.Value = "";
            hdDateB.Value = "";
            hdAmountB.Value = "";
            for (int i = 0; i < ds.Tables["table"].Rows.Count; i++)
            {
                if (drpvalue == "1")
                {
                    hdp_arr.Value +=(Convert.ToDateTime(ds.Tables["table"].Rows[i][7].ToString())).ToString("dd/MM/yyyy") + ',';
                    hdd_arr.Value += ds.Tables["table"].Rows[i][5].ToString() + ',';
                }
                else if (drpvalue == "2")
                {
                    hdp_arr.Value += (Convert.ToDateTime(ds.Tables["table"].Rows[i][6].ToString())).ToString("dd/MM/yyyy") + ',';
                    hdd_arr.Value += ds.Tables["table"].Rows[i][5].ToString() + ',';
                }
                else if (drpvalue == "3")
                {
                    hdp_arr.Value += ds.Tables["table"].Rows[i][12].ToString() + ',';
                    hdd_arr.Value += ds.Tables["table"].Rows[i][5].ToString() + ',';
                    hdDateB.Value += ds.Tables["table"].Rows[i][12].ToString() + ',';
                    hdAmountB.Value += ds.Tables["table"].Rows[i][10].ToString() + ',';
                }                
            }                       
        }
        else
        {
            lblMsg.Text = "No delivery and sales  entry found.";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            ReportViewer1.Visible = false;
            btnPrint.Visible = false;
        }
    }
    protected void txtUserID_TextChanged(object sender, EventArgs e)
    {
        DTO.Report Ob = new DTO.Report();
        Ob = SetValue();
        if (txtUserID.Text != "")
        {
            if (BAL.BALFactory.Instance.Bal_Report.CheckOrginalUser(Ob) != true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Please select a valid User ID";
                txtUserID.Text = "";
            }
            btnShowReport_Click(null, null);
            txtUserID.Focus();
        }
        else
        {
            btnShowReport_Click(null, null);
        }
    }
    public DTO.Report SetValue()
    {
        DTO.Report Ob = new DTO.Report();       
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        Ob.FromDate = DateFromAndTo[0].Trim();
        Ob.UptoDate = DateFromAndTo[1].Trim();
        Ob.UserID = txtUserID.Text;
        Ob.InvoiceNo = txtInvoiceNo.Text;
        Ob.CustId = hdnCustId.Value.Trim();
        Ob.Date = hdnDate.Value;
        Ob.BranchId = Globals.BranchID;
        return Ob;
    }   
   
    public DataSet GetData(DTO.Report Ob)
    {
        DataSet ds = new DataSet();
        if (drpSelectOption.SelectedValue == "1")
        {
            //if (!chkCustomerSelection.Checked)
            if (txtCName.Text.Trim() =="")
                ds = BAL.BALFactory.Instance.Bal_Report.GetDeliveryFrom_To_UptoDate(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetDeliveryByCustomer(Ob);
        }
        if (drpSelectOption.SelectedValue == "2")
        {
            if (txtCName.Text.Trim() == "")
                ds = BAL.BALFactory.Instance.Bal_Report.GetSaleFrom_To_UptoDate(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetSaleFrom_To_UptoDateByCustomer(Ob);
        }
        if (drpSelectOption.SelectedValue == "3")
        {
            if (txtCName.Text.Trim() == "")
                ds = BAL.BALFactory.Instance.Bal_Report.GetDeliveryAndSalesFrom_To_UptoDate(Ob);
            else
                ds = BAL.BALFactory.Instance.Bal_Report.GetDeliveryAndSalesFrom_To_UptoDateByCustomer(Ob);
        }
        return ds;
    }
    protected void txtCName_TextChanged(object sender, EventArgs e)
    {
        string[] CustName = txtCName.Text.Split('-');
        hdnCustId.Value = CustName[0].ToString();
        txtInvoiceNo.Text = "";
        setCustvalue(CustName[0].ToString());
        btnShowReport_Click(null, null);
    }
    public void setCustvalue(string customerName)
    {
        DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
        if (DS_CustInfo.Tables[0].Rows.Count > 0)                  
            txtCName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();        
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
    "  <PageWidth>10in</PageWidth>" +
    "  <PageHeight>11in</PageHeight>" +
    "  <MarginTop>0.2in</MarginTop>" +
    "  <MarginLeft>0.3in</MarginLeft>" +
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
    


    

