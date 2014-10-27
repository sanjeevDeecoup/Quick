using System;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmBrandMaster : System.Web.UI.Page
    {
        private DTO.BrandMaster Ob = new DTO.BrandMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid("");
                txtBrand.Focus();
            }
        }

        public DTO.BrandMaster SetValue()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.BrandName = txtBrand.Text.ToUpper();
            Ob.Active = 1;
            return Ob;
        }

        private void BindGrid(string strSearchBy)
        {
            Ob.BrandName = strSearchBy;
            Ob.BranchId = Globals.BranchID;
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BL_BrandMaster.BindGridView(Ob);
            grdSearchResult.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BL_BrandMaster.SaveBrandMaster(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = res;
                BindGrid("");
                RefreshForm();
            }
            else
            {
                lblErr.Text = res;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.BrandID = Convert.ToInt32(lblUpdateId.Text);
            res = BAL.BALFactory.Instance.BL_BrandMaster.UpdateBrandMaster(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record updated.";
                BindGrid("");
                RefreshForm();
            }
            else
            {
                lblErr.Text = res;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(txtBrand.Text);
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindGrid("");
            RefreshForm();
        }

        protected void RefreshForm()
        {
            txtBrand.Text = "";
            lblUpdateId.Text = "";
            txtBrand.Focus();
            btnEdit.Visible = false;
            btnSave.Visible = true;
            btnDelete.Visible = false;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            BindGrid("");
            RefreshForm();
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdSearchResult.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdSearchResult.Rows[Rowno].FindControl("lblId")).Text;
            txtBrand.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
            txtBrand.Focus();
            txtBrand.Attributes.Add("onfocus", "javascript:select();");
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnDelete.Visible = true;
        }

        protected void grdSearchResult_OnSorted(object sender, EventArgs e)
        {
            grdSearchResult.Visible = true;
        }

        protected void grdSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSearchResult.PageIndex = e.NewPageIndex;
            BindGrid("");
            grdSearchResult.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.BrandID = Convert.ToInt32(lblUpdateId.Text);
            Ob.Active = 0;
            res = BAL.BALFactory.Instance.BL_BrandMaster.DeleteBrand(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Deactivated";
                btnDelete.Visible = false;
                BindGrid("");
                RefreshForm();
            }
            else
            {
                lblErr.Text = res;
            }
        }
    }
}