using System;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class FrmVariation : System.Web.UI.Page
    {
        private DTO.Variation Ob = new DTO.Variation();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDefault();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BAL_Variation.SaveVariation(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = res;
                setDefault();
            }
            else
            {
                lblErr.Text = res;
            }
        }

        public DTO.Variation SetValue()
        {
            Ob = new DTO.Variation();
            Ob.VariationName = txtVariName.Text.ToUpper();
            Ob.Active = 1;
            Ob.DateModified = Globals.date[0].ToString();
            Ob.BranchId = Globals.BranchID;
            Ob.VariationId = lblvariation.Text;
            return Ob;
        }

        public void setDefault()
        {
            btnSave.Visible = true;
            btnEdit.Visible = false;
            txtVariName.Focus();
            txtVariName.Text = "";
            Ob = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Variation.BindGrid(Ob);
            grdSearchResult.DataBind();
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdSearchResult.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdSearchResult.Rows[Rowno].FindControl("lblId")).Text;
            txtVariName.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
            txtVariName.Focus();
            txtVariName.Attributes.Add("onfocus", "javascript:select();");
            btnSave.Visible = false;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
        }

        protected void btnEdit_Click1(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.VariationId = lblUpdateId.Text;
            res = BAL.BALFactory.Instance.BAL_Variation.UpdateVariation(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Updated";
                setDefault();
            }
            else
            {
                lblErr.Text = res;
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Frmvariation.aspx", false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Variation.BindGridView(Ob);
            grdSearchResult.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            setDefault();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.Active = 0;
            Ob.VariationId = lblUpdateId.Text;
            res = BAL.BALFactory.Instance.BAL_Variation.Deletevariation(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Deactivated";
                btnDelete.Visible = false;
                setDefault();
            }
            else
            {
                lblErr.Text = res;
            }
        }
    }
}