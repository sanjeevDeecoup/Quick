using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PriorityMaster : System.Web.UI.Page
{
    private DTO.Common Ob = new DTO.Common();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            txtPriority.Focus();
        }
    }

    public DTO.Common SetValue()
    {
        Ob.Input = txtPriority.Text;
        Ob.Id = lblUpdateId.Text;
        Ob.BID = Globals.BranchID;
        return Ob;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Ob = SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Priority.SavePriority(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Customer preference created sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtPriority.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Please enter Customer preference to save.";
            return;
        }
        SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Priority.UpdatePriority(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Customer preference information updated sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
        }
    }    


    private void BindGrid()
    {
        Ob = SetValue();
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Priority.ShowAllPriority(Ob);
        grdSearchResult.DataBind();
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
        lblUpdateId.Text = grdSearchResult.SelectedDataKey.Value.ToString(); 
        txtPriority.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : HttpUtility.HtmlDecode(grdSearchResult.SelectedRow.Cells[1].Text);
        lblPriority.Text = txtPriority.Text;
        txtPriority.Focus();
        txtPriority.Attributes.Add("onfocus", "javascript:select();");
        btnEdit.Visible = true;
        btnSave.Visible = false;
    }

    protected void RefreshForm()
    {
        txtPriority.Text = "";
        txtPriority.Focus();
        btnSave.Visible = true;
        btnEdit.Visible = false;
    }

    protected void ResetView()
    {
        btnSave.Text = "Save";
        btnSave.Visible = false;
        btnEdit.Visible = false;
    }
}