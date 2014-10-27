using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

namespace QuickWeb.Reports
{
    public partial class PackageReport : System.Web.UI.Page
    {
        DTO.Report Obj = new DTO.Report();
        static string _allCustCode = string.Empty;
        static string _allPackage = string.Empty;
        DataTable _dsSource;
        public static string _storename = string.Empty;
        public static string _BusinessName = string.Empty;
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _allCustCode = string.Empty; _allPackage = string.Empty;
                //BindPackageTypes();
                grdReport.DataBind();
                SetReport();
                ShowReport();
                binddrpsms();
                binddrpdefaultsms();
            }           
        }

        private void BindPackageTypes()
        {
            var ds = BAL.BALFactory.Instance.BL_PackageMaster.FindAllPackageTypes(Globals.BranchID);
        }

        protected void txtCustName_TextChanged(object sender, EventArgs e)
        {
            _allPackage = string.Empty;
            if (string.IsNullOrEmpty(_allCustCode))
                _allCustCode = txtCustName.Text.Split('-')[0].Trim();
            else
                _allCustCode = _allCustCode + ", " + txtCustName.Text.Split('-')[0].Trim();
            SetReport();
            ShowReport();
            txtCustName.Text = "";
            txtCustName.Focus();
        }

        protected void txtPackageName_TextChanged(object sender, EventArgs e)
        {
            _allCustCode = string.Empty;
            if (string.IsNullOrEmpty(_allPackage))
                _allPackage = txtPackageName.Text.Trim();
            else
                _allPackage = _allPackage + ", " + txtPackageName.Text.Trim();
            SetReport();
            ShowReport();
            txtPackageName.Text = "";
            txtPackageName.Focus();
        }

        protected void drpPackageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetReport();
            ShowReport();
        }

        protected void grdReport_OnDataBinding(object sender, EventArgs e)
        {
            _dsSource = grdReport.DataSource as DataTable;
        }

        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet dsMain = new DataSet();
            DataSet dsMainDiscount = new DataSet();
            DataSet dsMainValue = new DataSet();
            try
            {
                if (_dsSource == null)
                    return;

                //if (e.Row.RowType != DataControlRowType.DataRow && e.Row.RowType != DataControlRowType.Header)
                { e.Row.Cells[2].Visible = false; e.Row.Cells[12].Visible = false; e.Row.Cells[3].Visible = false;  }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[4].Text = "Package";
                    e.Row.Cells[5].Text = "Start";
                    e.Row.Cells[6].Text = "Expiry";
                    e.Row.Cells[7].Text = "Cost";
                    e.Row.Cells[8].Text = "Value";
                    e.Row.Cells[9].Text = "Consumed";
                    e.Row.Cells[10].Text = "Pending";
                    e.Row.Cells[11].Text = "Active";
                }


                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = "Total";
                    e.Row.Cells[4].Text = (_dsSource.Rows.Count).ToString();
                }

                e.Row.Cells[2].Visible = false; e.Row.Cells[3].Visible = false; e.Row.Cells[12].Visible = false;
                //e.Row.Cells[9].Text = e.Row.te
                /*
                var control = new HyperLink();
                control.Text = e.Row.DataItem.ToString();
                control.NavigateUrl = "~/Reports/PackageDetails.apsx?CustId=" + e.Row.DataItem.ToString() + "";
                e.Row.Cells[0].TemplateControl.Controls.Add(control);
                 */
                # region formatAccordingToType

                // if discount type (first one)
                if (_dsSource.AsEnumerable().Any(p => p["PackageType"].ToString() == "Discount"))
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                        e.Row.Cells[8].Text = "Discount %";
                    // else, don't do anything, just bound

                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                }
                else if (_dsSource.AsEnumerable().Any(p => p["PackageType"].ToString() == "Qty / Item" || p["PackageType"].ToString() == "Flat Qty"))
                {
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;                                   
                }
                else if (_dsSource.AsEnumerable().Any(p => p["PackageType"].ToString() == "Value / Benefit"))
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                }
                // if discount type (second)
                // don't do anything

                // if it is value type
                // don't do anything
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;


                #endregion

                //if (e.Row.Cells[10].Text == "Yes" && e.Row.RowType != DataControlRowType.Footer)
                //    e.Row.BackColor = System.Drawing.Color.LightGreen;
                //else if (e.Row.Cells[10].Text == "No" && e.Row.RowType != DataControlRowType.Footer)
                //    e.Row.BackColor = System.Drawing.Color.OrangeRed;

                if (drpPackageType.SelectedItem.Text == "Qty / Item")
                {
                    ((HyperLink)e.Row.FindControl("hplNav")).NavigateUrl = "~/Reports/PQDR.aspx?CustId=" + e.Row.Cells[2].Text + "&PkgId=" + e.Row.Cells[12].Text + "&Type=" + drpPackageType.SelectedItem.Text + "&CloseMe=true";
                    e.Row.Cells[4].Attributes.Add("class", "linkClass");
                }
                else if (drpPackageType.SelectedItem.Text == "Flat Qty")
                {
                    dsMain = BAL.BALFactory.Instance.Bal_Processmaster.FlatPackageQty(Globals.BranchID, e.Row.Cells[12].Text, e.Row.Cells[2].Text);
                    if (dsMain.Tables[0].Rows.Count != 0)
                    {
                        ((HyperLink)e.Row.FindControl("hplNav")).NavigateUrl = "~/Reports/FQDR.aspx?CustId=" + e.Row.Cells[2].Text + "&PkgId=" + e.Row.Cells[12].Text + "&Type=" + drpPackageType.SelectedItem.Text + "&CloseMe=true";
                         e.Row.Cells[4].Attributes.Add("class", "linkClass");
                    }
                }
                else if (drpPackageType.SelectedItem.Text == "Value / Benefit")
                {
                    dsMainValue = BAL.BALFactory.Instance.Bal_Processmaster.ValuBenifitPackageDtl(Globals.BranchID, e.Row.Cells[12].Text, e.Row.Cells[2].Text);
                    if (dsMainValue.Tables[0].Rows.Count != 0)
                    {
                        ((HyperLink)e.Row.FindControl("hplNav")).NavigateUrl = "~/Reports/VBRD.aspx?CustId=" + e.Row.Cells[2].Text + "&PkgId=" + e.Row.Cells[12].Text + "&Type=" + drpPackageType.SelectedItem.Text + "&CloseMe=true";                        
                    }
                }
                else
                {
                    dsMainDiscount = BAL.BALFactory.Instance.Bal_Processmaster.DiscountPackageCheck(Globals.BranchID, e.Row.Cells[12].Text, e.Row.Cells[2].Text);
                    if (dsMainDiscount.Tables[0].Rows.Count != 0)
                    {
                       // ((HyperLink)e.Row.FindControl("hplNav")).NavigateUrl = "~/Reports/PackageReportDetails.aspx?CustId=" + e.Row.Cells[2].Text + "&PkgId=" + e.Row.Cells[12].Text + "&Type=" + drpPackageType.SelectedItem.Text;
                        ((HyperLink)e.Row.FindControl("hplNav")).NavigateUrl = "~/Reports/frmPackageDiscountReport.aspx?CustId=" + e.Row.Cells[2].Text + "&PkgId=" + e.Row.Cells[12].Text + "&Type=" + drpPackageType.SelectedItem.Text + "&CloseMe=true";                        
                    }
                   // ((HyperLink)e.Row.FindControl("hplNav")).NavigateUrl = "~/Reports/PackageReportDetails.aspx?CustId=" + e.Row.Cells[2].Text + "&PkgId=" + e.Row.Cells[12].Text + "&Type=" + drpPackageType.SelectedItem.Text;
                }
                ((HyperLink)e.Row.FindControl("hplNav")).Text = e.Row.Cells[3].Text;               
               // e.Row.Cells[3].Attributes.Add("class", "linkClass");
            }
            catch (Exception)
            {
                // who the f*** cares? Right?
            }
        }

        protected void grdReport_OnDataBound(object sender, EventArgs e)
        {
            try { grdReport.Columns[2].Visible = false; }
            catch (Exception) { }
        }

        protected void SetReport()
        {
            Obj.CustId = _allCustCode;
            Obj.StrCodes = _allPackage;
            Obj.Description = drpPackageType.SelectedItem.Text;
            Obj.InvoiceNo = ((string.IsNullOrEmpty(_allCustCode) && string.IsNullOrEmpty(_allPackage)) ? "21" :  (string.IsNullOrEmpty(_allPackage)) ? "22" : "23");

        }

        protected void ShowReport()
        {
            btnExport.Visible = false;
            var ds = BAL.BALFactory.Instance.Bal_Report.GetPackageReportSummary(Obj, Globals.BranchID);
            if (ds == null) return;
            if (ds.Tables.Count == 0) return;
            // if (ds.Tables[0].Rows.Count == 0) return;
            var dt = ds.Tables[0];

            var dt2 = dt.AsEnumerable().Where(p => p["PackageType"].ToString() == Obj.Description && p["IsActive"].ToString() == drpActive.SelectedItem.Text);
            var dd = ds.Tables[0].Clone();
            
            //dd.Rows.Add(new { dt2.ToArray() });
            for (var i = 0; i < dt2.Count(); i++)
            {
                dd.ImportRow(dt2.ElementAt(i)); // could also use => dd.Rows.Add(dt2.ElementAt(i).ItemArray);                
            }

            grdReport.DataSource = dd;
            grdReport.DataBind();
            if (grdReport.Rows.Count > 0)
            {
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string strFileName = "strReportFile.xls";
                Response.Expires = 0;
                Response.Buffer = true;
                GridView grd = new GridView();
                grd.DataSource = grdReport.DataSource; // (DataSet)ViewState["SavedDS"];
                grd.DataBind();
                /*var dsLocal = grd.DataSource as DataTable;
                if (dsLocal == null)
                    return;
                 * */

                var list = new List<int>();
                foreach (TableCell cell in grdReport.Rows[0].Cells)
                {
                    if (cell.Visible == false)
                        list.Add(grdReport.Rows[0].Cells.GetCellIndex(cell));
                }
                //var listOfHiddenCols = new List<int>().Add(
                StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingWithoutDate(grdReport,"Package Report - "+drpPackageType.SelectedItem+" ", false,list);
                string strFilePathToSave = "";
                Response.ContentType = "application/vnd.ms-excel";
                strFilePathToSave = "~/Docs/" + strFileName;
                StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
                StreamWriter1.Write(strDataToSaveInFile);
                StreamWriter1.Close();
                Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
                Response.Redirect(strFilePathToSave, false);
            }
            catch (Exception ex)
            {

            }
        }

        protected void drpActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetReport();
            ShowReport();
        }
        private void binddrpsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.ShowAll(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpsmstemplateRenewal.DataSource = ds.Tables[0];
                drpsmstemplateRenewal.DataTextField = "template";
                drpsmstemplateRenewal.DataValueField = "smsid";
                drpsmstemplateRenewal.DataBind();
                drpsmstemplateMarkcomplete.DataSource = ds.Tables[0];
                drpsmstemplateMarkcomplete.DataTextField = "template";
                drpsmstemplateMarkcomplete.DataValueField = "smsid";
                drpsmstemplateMarkcomplete.DataBind();
            }
        }
        private void binddrpdefaultsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
            PrjClass.SetItemInDropDown(drpsmstemplateRenewal, ds.Tables[7].Rows[0]["Template"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpsmstemplateMarkcomplete, ds.Tables[6].Rows[0]["Template"].ToString(), true, false);
        }
        protected void lnkSendSMS_Click(object sender, EventArgs e)
        {
           string strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
           var ds = BAL.BALFactory.Instance.BL_Branch.ShowBranch(Int32.Parse(Globals.BranchID));
           _storename = ds.Tables[0].Rows[0]["BranchName"].ToString();
           _BusinessName = ds.Tables[0].Rows[0]["BusinessName"].ToString();
            try
            {
                if (drpActive.SelectedItem.Text == "Yes")
                {
                    AppClass.sendPackageReportSms(grdReport,drpsmstemplateRenewal );
                    AppClass.sendPackageReportEmail(grdReport, "Renewal", strPrinterName, drpPackageType.SelectedItem.Text, _storename, _BusinessName);                   
                }
                else
                {
                    AppClass.sendPackageReportSms(grdReport, drpsmstemplateMarkcomplete);
                    AppClass.sendPackageReportEmail(grdReport, "Expire", strPrinterName, drpPackageType.SelectedItem.Text, _storename, _BusinessName);
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package expiry notification SMS and Email sent.";
            }
            catch (Exception ex)
            {

            }
        }
    }
}