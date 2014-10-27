using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace QuickWeb.Reports
{
    public partial class frmLoginHistory : System.Web.UI.Page
    {
        public static string strFromDate = string.Empty, strToDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
                ArrayList date = new ArrayList();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();               
                txtUserID.Focus();
                btnExport.Visible = false;
                BindGrid();
            }
        }

        protected void txtUserID_TextChanged(object sender, EventArgs e)
        {
            try
            {
              txtReason.Text = "";
              BindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);             
                lblMsg.Text = "Please enter valid UserID.";
                txtUserID.Focus();
                txtUserID.Attributes.Add("onfocus", "javascript:select();");
            }
        }

        private void BindGrid()
        {          

            grdReport.DataSource = null;
            grdReport.DataBind();
            var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
            strFromDate = DateFromAndTo[0].Trim();
            strToDate = DateFromAndTo[1].Trim();
            grdReport.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.BindGridLoginHistory(strFromDate, strToDate, txtUserID.Text, txtReason.Text, drpActive.SelectedValue, Globals.BranchID);            
            grdReport.DataBind();           
            bool blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
                if (grdReport.Rows.Count > 0)
                {
                    btnExport.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                }
            }
            else
            {
                btnExport.Visible = false;
            }
        
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void drpActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, strFromDate, strToDate, "Log History Report", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }
        protected void txtReason_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtUserID.Text = "";
                BindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblMsg.Text = "Please enter valid reason.";
                txtReason.Focus();
                txtReason.Attributes.Add("onfocus", "javascript:select();");
            }
        }
    }
}