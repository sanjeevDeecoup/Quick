using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AppClass.CheckUserRightOnPage(this.Page);
            txtPwd.Focus();
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        string sql = "Update UserMaster Set UserPassword='" + txtPasswordNew.Text + "' Where UserId='" + Globals.UserId + "' And BranchId='" + Globals.BranchID + "'";
        SqlCommand cmd = new SqlCommand(sql, sqlcon);
        try
        {
            sqlcon.Open();         
            cmd.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Password updated successfully.";
        }
        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = excp.ToString();
        }
        finally { 
        sqlcon.Close();
        sqlcon.Dispose();
        cmd.Dispose();
        }
    }
}