using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

namespace QuickWeb.Reports
{
    public partial class frmServiceAndGarment : System.Web.UI.Page
    {
        DTO.Report Ob = new DTO.Report();
        public static bool blnRight = false;
        ArrayList date = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            }
            SDTProcesses.SelectCommand = "SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] WHERE BranchId='" + Globals.BranchID + "' ORDER BY [ProcessName]";
            SDTProcesses.DataBind();           
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();
            var processes = SetProcesses();
            Ob.CustId = hdnCustId.Value;
            ShowReport(strFromDate, strToDate, processes.Item1, processes.Item2, Ob.CustId);         
        }       
        protected Tuple<string, string> SetProcesses()
        {
            if (string.Compare(drpProcess.SelectedItem.Text, "All", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var prcCodes = string.Empty;
                for (var i = 0; i < drpProcess.Items.Count - 1; i++)
                {
                    if (string.Compare(drpProcess.Items[i].Text, "All", StringComparison.OrdinalIgnoreCase) == 0)
                        continue;

                    prcCodes += drpProcess.Items[i].Value + ", ";
                }
                if (prcCodes.IndexOf(", ") != -1)
                    prcCodes = prcCodes.Substring(0, prcCodes.Length - 2);

                return Tuple.Create(prcCodes, "All");
            }
            else
                return Tuple.Create(drpProcess.SelectedItem.Value, drpProcess.SelectedItem.Text);
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string[] CustName = txtCustomerName.Text.Split('-');
            hdnCustId.Value = CustName[0].ToString();
            setCustvalue(CustName[0].ToString());
            btnShowReport_Click(null, EventArgs.Empty);
        }
        public void setCustvalue(string customerName)
        {
            DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
            if (DS_CustInfo.Tables[0].Rows.Count > 0)
                txtCustomerName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString() + " " + DS_CustInfo.Tables[0].Rows[0]["CustAddress"].ToString();
        }

        # region old
        public void DateWiseAllProcess(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWiseDayByDay";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);               
        }
        public void MonthlyReport(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);          
        }
        public void DateWiseReport(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWiseDateWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@PROCESSNAME", drpProcess.SelectedValue);
            cmd.Parameters.AddWithValue("@PROCESSCODE", drpProcess.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);         
        }
        public void DayByDayWiseReport(string date, string date1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_AllAmountProcessWiseDateWiseDay";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDATE", date);
            cmd.Parameters.AddWithValue("@UDATE", date1);
            cmd.Parameters.AddWithValue("@PROCESSNAME", drpProcess.SelectedValue);
            cmd.Parameters.AddWithValue("@PROCESSCODE", drpProcess.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);         
        }
        # endregion

        public void ShowReport(string fromDate, string uptoDate, string processCode, string processName, string custId)
        {
            string res = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            var dataSet = BAL.BALFactory.Instance.Bal_Report.GetServiceAndGarment(new DTO.Report
            {
                FromDate = fromDate,
                UptoDate = uptoDate,
                InvoiceNo = "1",
                BranchId = Globals.BranchID,
                CustCodeStr = processCode,
                CustId = custId
            });
            if (dataSet == null)
                return;

            if (dataSet.Tables.Count == 0)
                return;
            ReportViewer1.LocalReport.ReportPath = "RDLC/ServiceAndGarmentReport.rdlc";
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                ReportViewer1.ShowExportControls = true;
            }
            else
            {
                ReportViewer1.ShowExportControls = false;
            }  
            bool rights = AppClass.GetShowFooterRightsUser();
            string rvalue = string.Empty;
            if (rights == true)
                rvalue = "1";
            else
                rvalue = "0";
            ReportParameter[] parameters = new ReportParameter[7];
            string str = Request.Url.Authority;
            string newstr = str + Request.ApplicationPath;
            parameters[0] = new ReportParameter("FromDate", fromDate);
            parameters[1] = new ReportParameter("ToDate", uptoDate);
            parameters[2] = new ReportParameter("StrLink", newstr);
            parameters[3] = new ReportParameter("UserName", Globals.UserName);
            parameters[4] = new ReportParameter("GeneratedDate", "@" + " " + DateTime.Today.ToString("dd MMM yyyy") + " " + res);
            parameters[5] = new ReportParameter("UserTypeId", Globals.UserType);
            parameters[6] = new ReportParameter("TotalFooter", rvalue);
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "Service_Garment";
            rds.Value = dataSet.Tables[0];
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                ReportViewer1.Visible = true;
                btnPrint.Visible = true;
            }
            else
            {
                ReportViewer1.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "No Record Found.";
                btnPrint.Visible = false;
            }
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
            Session["PcsSummary_fromDate"] = fromDate;
            Session["PcsSummary_uptoDate"] = uptoDate;
        
            Session.Remove("PcsSummary_fromDate");
            Session.Remove("PcsSummary_uptoDate");
        } 
        protected void btnExport_Click1(object sender, EventArgs e)
        {
            //string strFileName = "strReportFile.xls";
            //Response.Expires = 0;
            //Response.Buffer = true;
            //StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);
            //string strFilePathToSave = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //strFilePathToSave = "~/Docs/" + strFileName;
            //StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            //StreamWriter1.Write(strDataToSaveInFile);
            //StreamWriter1.Close();
            //Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            //Response.Redirect(strFilePathToSave, false);
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
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
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
        }        
    }
}