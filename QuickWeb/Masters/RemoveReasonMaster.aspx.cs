using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RemoveReasonMaster : System.Web.UI.Page
{
    private DTO.Common Ob = new DTO.Common();
    protected string ClothReturnCauses = ClothReturnCause.ClothReturnCauses;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RefreshForm();
        }
    }

    public DTO.Common SetValue()
    {
        Ob.Input = txtReason.Text.Trim();
        Ob.Id = lblUpdateId.Text;
        Ob.BID = Globals.BranchID;
        Ob.Path = lblReason.Text;
        return Ob;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Ob = SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_RemoveReason.SaveReason(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Cloth return cause created sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
            txtReason.Focus();
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtReason.Text == "")
        {
            lblErr.Text = "Please enter the reason!";
            return;
        }
        SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_RemoveReason.UpdateReason(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Cloth return cause information updated sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
            txtReason.Focus();
        }
    }    

    private void BindGrid()
    {
        Ob = SetValue();
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_RemoveReason.ShowAllReason(Ob);
        grdSearchResult.DataBind();
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {
       // lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
        lblUpdateId.Text = grdSearchResult.SelectedDataKey.Value.ToString(); 
        txtReason.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
        lblReason.Text = txtReason.Text;
        txtReason.Focus();
        txtReason.Attributes.Add("onfocus", "javascript:select();");
        btnEdit.Visible = true;
        btnSave.Visible = false;
        btnDelete.Visible = true;
    }

    protected void RefreshForm()
    {
        txtReason.Text = "";
        txtReason.Focus();
        btnSave.Visible = true;
        btnEdit.Visible = false;
        btnDelete.Visible = false;
        BindGrid();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_RemoveReason.DeleteReasonMain(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Cloth return cause deleted sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
        }
    }
}