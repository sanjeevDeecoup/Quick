using System;

public partial class BackUp_backUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Redirect("~/Masters/Default.aspx?Backup=" + "BackUp" + "");
        }
    }

    protected void drpListofDrive_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void drpProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}