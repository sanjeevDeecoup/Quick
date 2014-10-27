using System;

public partial class ShiftMaster : System.Web.UI.Page
{
    private DTO.Common Ob = new DTO.Common();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid("");
            txtShift.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtShift.Text == "")
        {
            lblErr.Text = "Please enter shift name to save.";
            return;
        }
        SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Shift.SaveShift(Ob);
        if (Ob.Result == "Record Saved")
        {
            lblMsg.Text = Ob.Result;
            BindGrid("");
            RefreshForm();
        }
        else
        {
            lblErr.Text = Ob.Result;
        }
    }

    public DTO.Common SetValue()
    {
        Ob.BID = Globals.BranchID;
        Ob.Input = txtShift.Text;
        Ob.Id = lblUpdateId.Text;
        return Ob;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtShift.Text == "")
        {
            lblErr.Text = "Please enter shift name to save.";
            return;
        }
        SetValue();
        Ob.Result = BAL.BALFactory.Instance.BAL_Shift.UpdateShift(Ob);
        if (Ob.Result == "Record Saved")
        {
            lblMsg.Text = "Record updated.";
            BindGrid("");
            RefreshForm();
        }
        else
        {
            lblErr.Text = Ob.Result;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtShift.Text == "")
        {
            lblErr.Text = "Please Enter some text in shift name for searching.";
            return;
        }
        BindGrid(txtShift.Text);
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        BindGrid("");
    }

    private void BindGrid(string strSearchBy)
    {
        Ob.Input = strSearchBy;
        Ob.BID = Globals.BranchID;
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Shift.SearchShift(Ob);
        grdSearchResult.DataBind();
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
        txtShift.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
        lblShift.Text = txtShift.Text;
        txtShift.Attributes.Add("onfocus", "javascript:select();");
        btnEdit.Visible = true;
        btnSave.Visible = false;
    }

    protected void grdSearchResult_PageIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdSearchResult_OnSorted(object sender, EventArgs e)
    {
    }

    protected void RefreshForm()
    {
        txtShift.Text = "";
        lblUpdateId.Text = "";
        txtShift.Focus();
        btnEdit.Visible = false;
        btnSave.Visible = true;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        BindGrid("");
        RefreshForm();
    }
}