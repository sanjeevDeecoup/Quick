using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Security;

namespace QuickWeb.Factory
{
    public partial class frmFactoryBoundToMachine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {              
                AppClass.CheckUserRightOnPageForFactory(this.Page);
                BindGrid();
                showData();
                storeData();
            }
            var btn = Request.Params["__EVENTTARGET"] as string;
            if (btn != null && btn == "ctl00$ContentPlaceHolder1$rdrBoundToMachine")
            {
                BoundToMachine();
            }
        }
        private void BindGrid()
        {
            grdBoudToMachine.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.GetBoundToMachineDetails(Globals.BranchID);
            grdBoudToMachine.DataBind();
        }
        protected void grdSearchResult_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int BoundId = 0;
            int rownum = int.Parse(e.CommandArgument.ToString());
            if (rownum < grdBoudToMachine.Rows.Count)
            {
                BoundId = Convert.ToInt32(((HiddenField)grdBoudToMachine.Rows[rownum].FindControl("hdnID")).Value);
                string devicename = grdBoudToMachine.Rows[rownum].Cells[0].Text;
                DeleteData(BoundId, devicename);
            }
        }

        private void DeleteData(int BoundId, string devicename)
        {
            string res = string.Empty;
            res = BAL.BALFactory.Instance.BL_CustomerMaster.DeleteBoundToMachineDetails(BoundId);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Access revoked for " + devicename + " device.";
            }
            BindGrid();
            divBoudToMachine.Visible = true;
        }

        private void BoundToMachine()
        {
            try
            {
                bool IsBoundToMachine = false;
                string res = string.Empty;
                if (rdrBoundToMachine.Checked)
                {
                    IsBoundToMachine = true;
                }
                else
                {
                    IsBoundToMachine = false;
                    res = BAL.BALFactory.Instance.BL_CustomerMaster.DeleteBoundToMachineData(Globals.BranchID);
                }
                res = BAL.BALFactory.Instance.BL_CustomerMaster.BoundToMachineCheck(IsBoundToMachine, Globals.BranchID);
                if (res == "Record Saved")
                {
                    if (rdrBoundToMachine.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                        lblMsg.Text = " Enable authentication kindly login again. ";
                        divBoudToMachine.Visible = true;
                        FormsAuthentication.SignOut();
                        Response.Redirect(FormsAuthentication.LoginUrl, false);
                    }
                    else
                    {
                        divBoudToMachine.Visible = false;
                    }
                }
                else
                {
                    lblMsg.Text = res;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();

            }
        }
        private void showData()
        {
            bool status = false;
            status = BAL.BALFactory.Instance.BL_CustomerMaster.bountToMachineStatus(Globals.BranchID);
            if (status == true)
            {
                rdrBoundToMachine.Checked = true;
                divBoudToMachine.Visible = true;
            }
            else
            {
                rdrBoundToMachine.Checked = false;
                divBoudToMachine.Visible = false;

            }

        }
        private void storeData()
        {
            DataSet ds1 = BAL.BALFactory.Instance.BL_CustomerMaster.GetStoreEmaiAndMobile(Globals.BranchID);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                hdnMobileNo.Value = ds1.Tables[0].Rows[0]["BranchMobile"].ToString();
                hdnEmailID.Value = ds1.Tables[0].Rows[0]["BranchEmail"].ToString();
            }
        }
    }
}