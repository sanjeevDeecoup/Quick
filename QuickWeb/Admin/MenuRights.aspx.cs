using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class MenuRights : System.Web.UI.Page
{
    private DTO.Common Ob = new DTO.Common();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.DataBind();
            if (drpUserTypes.Items.Count > 0)
                drpUserTypes_SelectedIndexChanged(null, null);
        }
    }

    protected void drpUserTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnSelectedUserTypeId.Value = drpUserTypes.SelectedValue;
        Ob.BID = Globals.BranchID;
        DataSet ds = new DataSet();
        Ob.Id = drpUserTypes.SelectedValue;
        ds = BAL.BALFactory.Instance.BAL_ChallanIn.BindGrid(Ob);

        grdCustomer1.DataSource = ds.Tables[1];
        grdCustomer1.DataBind();
        grdCustomer2.DataSource = ds.Tables[2];
        grdCustomer2.DataBind();

        grdDrop1.DataSource = ds.Tables[3];
        grdDrop1.DataBind();
        grdDrop2.DataSource = ds.Tables[4];
        grdDrop2.DataBind();

        grdProcess1.DataSource = ds.Tables[5];
        grdProcess1.DataBind();
        grdProcess2.DataSource = ds.Tables[6];
        grdProcess2.DataBind();

        grdPickUp1.DataSource = ds.Tables[7];
        grdPickUp1.DataBind();
        grdPickUp2.DataSource = ds.Tables[8];
        grdPickUp2.DataBind();

        grdAccount1.DataSource = ds.Tables[9];
        grdAccount1.DataBind();
        grdAccount2.DataSource = ds.Tables[10];
        grdAccount2.DataBind();

        grdReport1.DataSource = ds.Tables[11];
        grdReport1.DataBind();
        grdReport2.DataSource = ds.Tables[12];
        grdReport2.DataBind();

        grdMasterData1.DataSource = ds.Tables[13];
        grdMasterData1.DataBind();
        grdMasterData2.DataSource = ds.Tables[14];
        grdMasterData2.DataBind();

        grdAdmin1.DataSource = ds.Tables[15];
        grdAdmin1.DataBind();
        grdAdmin2.DataSource = ds.Tables[16];
        grdAdmin2.DataBind();

        grdSpecialPart1.DataSource = ds.Tables[17];
        grdSpecialPart1.DataBind();
        grdSpecialPart2.DataSource = ds.Tables[18];
        grdSpecialPart2.DataBind();

    }
    private void SetRights(GridView grdMenuRights,SqlCommand cmd)
    {
        string strUserTypeId = hdnSelectedUserTypeId.Value;
        string strPageTitle = string.Empty, strAllow = string.Empty;
        for (int r = grdMenuRights.Rows.Count - 1; r >= 0; r--)
        {
            strPageTitle = "" + grdMenuRights.Rows[r].Cells[0].Text.Replace("&nbsp;", "");
            strAllow = "" + (((CheckBox)grdMenuRights.Rows[r].FindControl("chkAllow")).Checked ? "1" : "0");
            cmd.CommandText = "UPDATE EntMenuRights SET RightToView = '" + strAllow + "' WHERE UserTypeId='" + strUserTypeId + "' AND PageTitle = '" + strPageTitle + "' AND BranchId = '" + Globals.BranchID + "'";
            cmd.ExecuteNonQuery();
            if (strAllow == "0" && r < 6)
            {
                cmd.CommandText = "UPDATE EntMenuRights SET RightToView = '" + strAllow + "' WHERE UserTypeId='" + strUserTypeId + "' AND ParentMenu = '" + strPageTitle + "' AND BranchId = '" + Globals.BranchID + "'";
                cmd.ExecuteNonQuery();
            }
        }
    }  

    protected void btnUpdateRights_Click(object sender, EventArgs e)
    {       
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand();
        SqlTransaction stx = null;
        try
        {
            sqlcon.Open();
            stx = sqlcon.BeginTransaction();
            cmd.Transaction = stx;
            cmd.Connection = sqlcon;
            SetRights(grdCustomer1, cmd);
            SetRights(grdCustomer2, cmd);
            SetRights(grdDrop1, cmd);
            SetRights(grdDrop2, cmd);
            SetRights(grdProcess1, cmd);
            SetRights(grdProcess2, cmd);
            SetRights(grdPickUp1, cmd);
            SetRights(grdPickUp2, cmd);
            SetRights(grdAccount1, cmd);
            SetRights(grdAccount2, cmd);
            SetRights(grdReport1, cmd);
            SetRights(grdReport2, cmd);
            SetRights(grdMasterData1, cmd);
            SetRights(grdMasterData2, cmd);
            SetRights(grdAdmin1, cmd);
            SetRights(grdAdmin2, cmd);
            SetRights(grdSpecialPart1, cmd);
            SetRights(grdSpecialPart2, cmd);
            stx.Commit();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "User access permissions updated successfully.";
        }
        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Error (UpdateRights) : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
        }
    }
}