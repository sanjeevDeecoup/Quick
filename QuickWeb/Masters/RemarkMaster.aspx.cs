using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RemarkMaster : System.Web.UI.Page
{
    private DTO.Common Ob = new DTO.Common();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            txtJobType.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Ob = SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Remark.SaveRemark(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "New description created sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
        }
    }

    public DTO.Common SetValue()
    {
        Ob.Input = txtJobType.Text.Trim();
        Ob.Id = lblUpdateId.Text;
        Ob.BID = Globals.BranchID;
        return Ob;
    }

    private void BindGrid()
    {
        Ob = SetValue();
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Remark.ShowAllRemarks(Ob);
        grdSearchResult.DataBind();
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {
       // lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
        lblUpdateId.Text = grdSearchResult.SelectedDataKey.Value.ToString(); 
        txtJobType.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : HttpUtility.HtmlDecode(grdSearchResult.SelectedRow.Cells[1].Text);
        txtJobType.Attributes.Add("onfocus", "javascript:select();");
        lblShift.Text = txtJobType.Text;
        btnSave.Visible = false;
        btnUpdate.Visible = true;
        btnDelete.Visible = true;
        txtJobType.Focus();
    }

    protected void RefreshForm()
    {
        txtJobType.Text = "";
        txtJobType.Focus();
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        btnDelete.Visible = false;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Ob = SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Remark.UpdateRemarks(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Description information updated sucessfully.";
            BindGrid();
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Ob.Result;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Ob = SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Remark.DeleteRemarks(Ob);
        if (Ob.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Description deleted sucessfully.";
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