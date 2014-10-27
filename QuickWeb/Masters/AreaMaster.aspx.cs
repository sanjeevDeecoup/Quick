using System;

namespace QuickWeb.Masters
{
    public partial class AreaMaster : System.Web.UI.Page
    {
        private DTO.Common Ob = new DTO.Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                txtArea.Focus();
            }
        }

        public DTO.Common SetValue()
        {
            Ob.Input = txtArea.Text;
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
            txtArea.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            Ob.Result = BAL.BALFactory.Instance.BAL_Area.SaveArea(Ob);
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
            if (txtArea.Text == "")
            {
                lblErr.Text = "Please enter shift name to save.";
                return;
            }
            SetValue();
            Ob.Result = BAL.BALFactory.Instance.BAL_Area.UpdateArea(Ob);
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
            if (txtArea.Text == "")
            {
                lblErr.Text = "Please Enter some text in Area name for searching.";
                return;
            }
            Ob = SetValue();
            //grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Area.SearchArea(Ob);
            grdSearchResult.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            Ob = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Area.ShowAllArea(Ob);
            grdSearchResult.DataBind();
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
            txtArea.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
            lblArea.Text = txtArea.Text;
            txtArea.Focus();
            txtArea.Attributes.Add("onfocus", "javascript:select();");
            btnEdit.Visible = true;
            btnSave.Visible = false;
        }

        protected void RefreshForm()
        {
            txtArea.Text = "";
            txtArea.Focus();
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