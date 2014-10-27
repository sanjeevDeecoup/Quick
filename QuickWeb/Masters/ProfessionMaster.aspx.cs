using System;

namespace QuickWeb
{
    public partial class ProfessionMaster : System.Web.UI.Page
    {
        private DTO.Common Ob = new DTO.Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                txtProfession.Focus();
            }
        }

        public DTO.Common SetValue()
        {
            Ob.Input = txtProfession.Text;
            Ob.Id = lblUpdateId.Text;
            Ob.BID = Globals.BranchID;
            return Ob;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            RefreshForm();
            btnSave.Text = "Save";
            btnSave.Visible = true;
            lblSaveOption.Text = "1";
            btnEdit.Visible = false;
            txtProfession.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            Ob.Result = BAL.BALFactory.Instance.BAL_Profession.SaveProfession(Ob);
            if (Ob.Result == "Record Saved")
            {
                lblMsg.Text = PrjClass.SaveSuccess;
                BindGrid();
                RefreshForm();
            }
            else
                lblErr.Text = Ob.Result;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtProfession.Text == "")
            {
                lblErr.Text = "Please enter shift name to save.";
                return;
            }
            SetValue();
            Ob.Result = BAL.BALFactory.Instance.BAL_Profession.UpdateProfession(Ob);
            if (Ob.Result == "Record Saved")
            {
                lblMsg.Text = "Record updated.";
                BindGrid();
                RefreshForm();
            }
            else
            {
                lblErr.Text = Ob.Result;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtProfession.Text == "")
            {
                lblErr.Text = "Please Enter some text in Profession name for searching.";
                return;
            }
            Ob = SetValue();
            //grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Profession.SearchProfession(Ob);
            grdSearchResult.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            Ob = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Profession.ShowAllProfession(Ob);
            grdSearchResult.DataBind();
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
            txtProfession.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
            lblProfession.Text = txtProfession.Text;
            txtProfession.Focus();
            txtProfession.Attributes.Add("onfocus", "javascript:select();");
            btnEdit.Visible = true;
            btnSave.Visible = false;
        }

        protected void RefreshForm()
        {
            txtProfession.Text = "";
            txtProfession.Focus();
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
}