using System;

namespace QuickWeb.Masters
{
    public partial class CityMaster : System.Web.UI.Page
    {
        private DTO.Common Ob = new DTO.Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                txtCity.Focus();
            }
        }

        public DTO.Common SetValue()
        {
            Ob.Input = txtCity.Text;
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
            txtCity.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            Ob.Result = BAL.BALFactory.Instance.BAL_City.SaveCity(Ob);
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
            if (txtCity.Text == "")
            {
                lblErr.Text = "Please enter shift name to save.";
                return;
            }
            SetValue();
            Ob.Result = BAL.BALFactory.Instance.BAL_City.UpdateCity(Ob);
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
            if (txtCity.Text == "")
            {
                lblErr.Text = "Please Enter some text in City name for searching.";
                return;
            }
            Ob = SetValue();
            //grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_City.SearchCity(Ob);
            grdSearchResult.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            Ob = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_City.ShowAllCity(Ob);
            grdSearchResult.DataBind();
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
            txtCity.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
            lblCity.Text = txtCity.Text;
            txtCity.Focus();
            txtCity.Attributes.Add("onfocus", "javascript:select();");
            btnEdit.Visible = true;
            btnSave.Visible = false;
        }

        protected void RefreshForm()
        {
            txtCity.Text = "";
            txtCity.Focus();
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