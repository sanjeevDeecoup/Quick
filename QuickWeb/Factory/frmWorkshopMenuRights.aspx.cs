using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Factory
{
    public partial class frmWorkshopMenuRights : System.Web.UI.Page
    {
        private DTO.Common Ob = new DTO.Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPageForFactory(this.Page);
                Page.DataBind();
                if (drpUserTypes.Items.Count > 0)
                {                  
                     drpUserTypes_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void drpUserTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnSelectedUserTypeId.Value = drpUserTypes.SelectedValue;
            Ob.BID = Globals.BranchID;
            Ob.Id = drpUserTypes.SelectedValue;
            grdMenuRights.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindGridWorkshopMenuRight(Ob);
            grdMenuRights.DataBind();
        }

        protected void grdMenuRights_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UPDATERIGHTS")
                UpdateRights();
        }

        private void UpdateRights()
        {
            string strUserTypeId = hdnSelectedUserTypeId.Value;
            string strPageTitle = string.Empty, strAllow = string.Empty;
            SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction stx = null;
            try
            {
                sqlcon.Open();
                stx = sqlcon.BeginTransaction();
                cmd.Transaction = stx;
                cmd.Connection = sqlcon;
                for (int r = grdMenuRights.Rows.Count - 1; r >= 0; r--)
                {
                    strPageTitle = "" + grdMenuRights.Rows[r].Cells[0].Text.Replace("&nbsp;", "");
                    strAllow = "" + (((CheckBox)grdMenuRights.Rows[r].FindControl("chkAllow")).Checked ? "1" : "0");
                    cmd.CommandText = "UPDATE EntMenuRights SET RightToView = '" + strAllow + "' WHERE  UserTypeId=4  and  WorkshopUserType='" + strUserTypeId + "' AND PageTitle = '" + strPageTitle + "' AND BranchId = '" + Globals.BranchID + "'";
                    cmd.ExecuteNonQuery();
                    if (strAllow == "0" && r < 6)
                    {
                        cmd.CommandText = "UPDATE EntMenuRights SET RightToView = '" + strAllow + "' WHERE UserTypeId=4 and  WorkshopUserType='" + strUserTypeId + "' AND ParentMenu = '" + strPageTitle + "' AND BranchId = '" + Globals.BranchID + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
                stx.Commit();
                lblMsg.Text = "Rights updated";
            }
            catch (Exception excp)
            {
                lblErr.Text = "Error (UpdateRights) : " + excp.Message;
            }
            finally
            {
                sqlcon.Close();
            }
        }        
    }
}