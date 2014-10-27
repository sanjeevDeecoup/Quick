using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace QuickWeb
{
    public partial class ProcessMaster : System.Web.UI.Page
    {
        protected string ImagePath = "";
        DTO.ProcessMaster Obj = new DTO.ProcessMaster();
        DataSet ds = new DataSet();
        private string _LabelTax1;
        private string _LabelTax2;
        private string _LabelTax3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                         
                RefreshForm();
                ShowServicetaxData();               
                if (grdSearchResult.Rows.Count > 0)
                {
                    var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
                    _LabelTax1 = allTax.Split(':')[0];
                    _LabelTax2 = allTax.Split(':')[1];
                    _LabelTax3 = allTax.Split(':')[2];
                    grdSearchResult.HeaderRow.Cells[8].Text = _LabelTax1.ToString();
                    grdSearchResult.HeaderRow.Cells[9].Text = _LabelTax2.ToString();
                    grdSearchResult.HeaderRow.Cells[10].Text = _LabelTax3.ToString();
                }              
            }
            var btn = Request.Params["__EVENTTARGET"] as string;

            if (btn != null && btn == "ctl00$ContentPlaceHolder1$chkServiceTax")
            {
                ShowServicetaxData();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSave")
            {
                btnSave_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnUpdate")
            {
                btnEdit_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnDelete")
            {
                btnDelete_Click(null, EventArgs.Empty);
            } 
        }

        private void BindToConfig()
        {
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.Bal_Processmaster.BindToConfig(Globals.BranchID);
            lblFirstTax.Text = ds.Tables[0].Rows[0]["ServiceTaxText1"].ToString();
            lblSecondTax.Text = ds.Tables[0].Rows[0]["ServiceTaxText2"].ToString();
            lblThird.Text = ds.Tables[0].Rows[0]["ServiceTaxText3"].ToString();
            if (hdntest.Value == "0")
            {
                txtServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTaxRate1"].ToString();
                txtCSS.Text = ds.Tables[0].Rows[0]["ServiceTaxRate2"].ToString();
                txtECESJ.Text = ds.Tables[0].Rows[0]["ServiceTaxRate3"].ToString();
            }
        }
   
        protected void btnSave_Click(object sender, EventArgs e)
        {
            clearMsg();
            if (txtProcessCode.Text == "" || txtProcessName.Text == "")
            {
                lblErr.Text = "Please enter process code, name to save.";
                return;
            }
            try
            {
                string res = "";
                SetValueIntoProperties();               
                res = BAL.BALFactory.Instance.Bal_Processmaster.SaveProcessMaster(Obj);
                if (res == "Record Saved")
                {                  
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                    lblMsg.Text = "New service created sucessfully";                
                    RefreshForm();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = res;
                }
            }
            catch (Exception) { }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            clearMsg();
            if (txtProcessCode.Text == "" || txtProcessName.Text == "")
            {
                lblErr.Text = "Please enter process code, name to save.";
                return;
            }
            try
            {
                string res = "";
                SetValueIntoProperties();               
                Obj.ProcessCode = hdnSelectedProcessCode.Value;
                res = BAL.BALFactory.Instance.Bal_Processmaster.UpdateProcessMaster(Obj);
                if (res == "Record Saved")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                    lblMsg.Text = "Service information updated sucessfully.";                                   
                    RefreshForm();
                    if (grdSearchResult.Rows.Count > 0)
                    {
                        var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
                        _LabelTax1 = allTax.Split(':')[0];
                        _LabelTax2 = allTax.Split(':')[1];
                        _LabelTax3 = allTax.Split(':')[2];
                        grdSearchResult.HeaderRow.Cells[8].Text = _LabelTax1.ToString();
                        grdSearchResult.HeaderRow.Cells[9].Text = _LabelTax2.ToString();
                        grdSearchResult.HeaderRow.Cells[10].Text = _LabelTax3.ToString();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                    lblErr.Text = res;
                }
            }
            catch (Exception) { }
        }
        public void SetValueIntoProperties()
        {         
            Obj.BID = Globals.BranchID;
            Obj.ProcessCode = txtProcessCode.Text.ToUpperInvariant();
            Obj.ProcessName = txtProcessName.Text;
            Obj.UsedForvendorReport = "1";
            Obj.Discount = "1";
            Obj.ServiceTax = (txtServiceTax.Text == "" ? 0 : Convert.ToDouble(txtServiceTax.Text));
            Obj.CssTax = (txtCSS.Text == "" ? 0 : Convert.ToDouble(txtCSS.Text));
            Obj.EcesjTax = (txtECESJ.Text == "" ? 0 : Convert.ToDouble(txtECESJ.Text));
            Obj.IsActiveServiceTax = (chkServiceTax.Checked ? "1" : "0");
            Obj.IsDiscount = (chkDiscount1.Checked ? "1" : "0");
            Obj.IsChallanApplicable = (chkChallan.Checked ? "1" : "0");
            Obj.IsReady = (ChkReady.Checked ? "1" : "0");
           
        }       
        private void ShowGrid(string strProcessNameLike)
        {
            Obj.ProcessName = strProcessNameLike;
            Obj.BID = Globals.BranchID;
            ds = BAL.BALFactory.Instance.Bal_Processmaster.SearchProcessMaster(Obj);

            ViewState["GridData"] = null;
            ViewState["GridData"]=ds.Tables[0];

            grdSearchResult.DataSource = ds;
            grdSearchResult.DataBind();

            //SqlGridSource.SelectCommand = "SELECT ProcessCode, ProcessName, ProcessUsedForVendorReport, Discount, CASE WHEN ServiceTax = '0' THEN '' ELSE ServiceTax END As ServiceTax,case WHEN   IsActiveServiceTax =0 THEN 'No' else 'Yes' END IsActiveServiceTax,case WHEN   IsDiscount =0 THEN 'No' else 'Yes' END IsDiscount,'~/ProcessWiseLogo/'+ImagePath as ImagePath,case WHEN   IsChallan =0 THEN 'No' else 'Yes' END IsChallan,CssTax,EcesjTax FROM dbo.ProcessMaster WHERE BranchId='" + Globals.BranchID + "' AND ProcessName LIKE '%'+'" + strProcessNameLike + "'+'%' ORDER BY ProcessName";
            //SqlGridSource.DataBind();
        }       
        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnSelectedProcessCode.Value = grdSearchResult.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
            GetProcessDetails(hdnSelectedProcessCode.Value);
        }
        private void GetProcessDetails(string strProcessCode)
        {
            Obj.ProcessCode = strProcessCode;
            Obj.BID = Globals.BranchID;
            ds = BAL.BALFactory.Instance.Bal_Processmaster.BindProcessMaster(Obj);
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtProcessCode.Text = ds.Tables[0].Rows[0]["ProcessCode"].ToString();
                    txtProcessName.Text = ds.Tables[0].Rows[0]["ProcessName"].ToString();
                    chkUseForVendorReport.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ProcessUsedForVendorReport"].ToString());
                    chkDiscount.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Discount"].ToString());
                    txtServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                    txtCSS.Text = ds.Tables[0].Rows[0]["CssTax"].ToString();
                    txtECESJ.Text = ds.Tables[0].Rows[0]["EcesjTax"].ToString();
                    chkServiceTax.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActiveServiceTax"].ToString());
                    chkDiscount1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDiscount"].ToString());
                    chkChallan.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsChallan"].ToString());
                    ChkReady.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsChallanByPass"].ToString());
                    txtProcessCode.Enabled = false;
                    if (chkChallan.Checked == true)
                    {
                        ChallanTxt.Attributes.Add("class", "");                        
                    }
                    else
                    {
                        ChallanTxt.Attributes.Add("class", "txtColor");
                    }
                    if (chkDiscount1.Checked == true)
                    {
                        DisTxt.Attributes.Add("class", "");
                    }
                    else
                    {
                        DisTxt.Attributes.Add("class", "txtColor");                        
                    }

                    if (ChkReady.Checked == true)
                    {
                        readyTxt.Attributes.Add("class", "");
                        chkChallan.Disabled = true;
                    }
                    else
                    {                        
                        readyTxt.Attributes.Add("class", "txtColor");
                        chkChallan.Disabled = false;
                    }
                    if (chkServiceTax.Checked)
                    {
                        hdntest.Value = "1";
                        ServiceTxt.Attributes.Add("class", "");
                        divServiceTax.Attributes.Add("style", "display:inline");                       
                    }
                    else
                    {
                        hdntest.Value = "0";
                        ServiceTxt.Attributes.Add("class", "txtColor");
                        divServiceTax.Attributes.Add("style", "display:none");
                    }
                    ShowServicetaxData();
                }
                btnEdit.Visible = true;
                btnSave.Visible = false;
                btnDelete.Visible = true;
                txtProcessName.Focus();
                txtProcessName.Attributes.Add("onfocus", "javascript:select();");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setCheckBoxesText();", true);
                if (grdSearchResult.Rows.Count > 0)
                {
                    var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
                    _LabelTax1 = allTax.Split(':')[0];
                    _LabelTax2 = allTax.Split(':')[1];
                    _LabelTax3 = allTax.Split(':')[2];
                    grdSearchResult.HeaderRow.Cells[8].Text = _LabelTax1.ToString();
                    grdSearchResult.HeaderRow.Cells[9].Text = _LabelTax2.ToString();
                    grdSearchResult.HeaderRow.Cells[10].Text = _LabelTax3.ToString();
                }
            }
            catch (Exception excp)
            {
                lblErr.Text = "Error (GetProcessDetails()): " + excp.Message;
            }
            finally
            {

            }
        }
       
        protected void grdSearchResult_OnSorted(object sender, EventArgs e)
        {
            grdSearchResult.Visible = true;
            ShowGrid("");
        }
        protected void RefreshForm()
        {
            txtProcessName.Focus();
            txtProcessCode.Text = "";
            txtProcessName.Text = "";
            txtServiceTax.Text = "";
            txtCSS.Text = "";
            txtECESJ.Text = "";
            txtProcessCode.Enabled = true;
            chkServiceTax.Checked = false;
            chkUseForVendorReport.Checked = true;
            chkDiscount.Checked = false;
            chkChallan.Checked = false;
            chkDiscount1.Checked = false;
            ChkReady.Checked = false;
            ChallanTxt.Attributes.Add("class", "txtColor");
            DisTxt.Attributes.Add("class", "txtColor");
            readyTxt.Attributes.Add("class", "txtColor");
            divServiceTax.Attributes.Add("style", "display:none");
            hdnSelectedProcessCode.Value = "";
            ShowGrid("");
            ShowServicetaxData();
            btnEdit.Visible = false;
            btnSave.Visible = true;
            btnDelete.Visible = false;
            ImagePath = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            hdntest.Value = "0";
            if (grdSearchResult.Rows.Count > 0)
            {
                var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
                _LabelTax1 = allTax.Split(':')[0];
                _LabelTax2 = allTax.Split(':')[1];
                _LabelTax3 = allTax.Split(':')[2];
                grdSearchResult.HeaderRow.Cells[8].Text = _LabelTax1.ToString();
                grdSearchResult.HeaderRow.Cells[9].Text = _LabelTax2.ToString();
                grdSearchResult.HeaderRow.Cells[10].Text = _LabelTax3.ToString();
            }
            BindToConfig();
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setCheckBoxesText();", true);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = "";
            clearMsg();
            Obj.ProcessCode = hdnSelectedProcessCode.Value;
            Obj.BID = Globals.BranchID;
            res = BAL.BALFactory.Instance.Bal_Processmaster.DeleteProcessMaster(Obj);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Service deleted sucessfully.";               
                RefreshForm();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
                if (grdSearchResult.Rows.Count > 0)
                {
                    var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
                    _LabelTax1 = allTax.Split(':')[0];
                    _LabelTax2 = allTax.Split(':')[1];
                    _LabelTax3 = allTax.Split(':')[2];
                    grdSearchResult.HeaderRow.Cells[8].Text = _LabelTax1.ToString();
                    grdSearchResult.HeaderRow.Cells[9].Text = _LabelTax2.ToString();
                    grdSearchResult.HeaderRow.Cells[10].Text = _LabelTax3.ToString();
                }
                BindToConfig();
            }
        }
        protected void ShowServicetaxData()
        {
            if (chkServiceTax.Checked)
            {

                //divServiceTax.Visible = true;
                ServiceTxt.Attributes.Add("class", "");               
            }
            else
            {
                //divServiceTax.Visible = false;
                ServiceTxt.Attributes.Add("class", "txtColor");                
            }
            BindToConfig();
        }      

        protected void grdSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSearchResult.PageIndex = e.NewPageIndex;
            grdSearchResult.Visible = true;
            ShowGrid("");
        }

        public void  clearMsg()
        {
            lblErr.Text = "";
            lblMsg.Text = "";
        }
        protected void grdSearchResult_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                // DataTable sourceTable = grdReport.DataSource as DataTable;

                var sourceTable =(DataTable) ViewState["GridData"];
              
                DataView view = new DataView(sourceTable);
              
                if (ViewState["sortExpression"] == null)
                    ViewState["sortExpression"] = e.SortExpression + " " + "ASC";


                string[] sortData = ViewState["sortExpression"].ToString().Trim().Split(' ');
                if (e.SortExpression == sortData[0])
                {
                    if (sortData[1] == "ASC")
                    {
                        view.Sort = e.SortExpression + " " + "DESC";
                        this.ViewState["sortExpression"] = e.SortExpression + " " + "DESC";
                    }
                    else
                    {
                        view.Sort = e.SortExpression + " " + "ASC";
                        this.ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
                    }
                }
                else
                {
                    view.Sort = e.SortExpression + " " + "ASC";
                    this.ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
                }
                grdSearchResult.DataSource = null;
                grdSearchResult.DataBind();
                grdSearchResult.DataSource = view;
                grdSearchResult.DataBind();
                if (grdSearchResult.Rows.Count > 0)
                {
                    var allTax = BAL.BALFactory.Instance.Bal_Report.FindTaxLabels(Globals.BranchID);
                    _LabelTax1 = allTax.Split(':')[0];
                    _LabelTax2 = allTax.Split(':')[1];
                    _LabelTax3 = allTax.Split(':')[2];
                    grdSearchResult.HeaderRow.Cells[8].Text = _LabelTax1.ToString();
                    grdSearchResult.HeaderRow.Cells[9].Text = _LabelTax2.ToString();
                    grdSearchResult.HeaderRow.Cells[10].Text = _LabelTax3.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void grdSearchResult_Sorted(object sender, EventArgs e)
        {
            grdSearchResult.Visible = true;          
        }
    }
}