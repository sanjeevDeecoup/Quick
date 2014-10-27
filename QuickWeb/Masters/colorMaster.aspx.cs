using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class colorMaster : System.Web.UI.Page
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
            Ob.Result = BAL.BALFactory.Instance.BAL_Color.SaveColor(Ob);
            if (Ob.Result == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "New color created sucessfully.";
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
            Ob.Path = lblShift.Text;
            return Ob;
        }
        private void BindGrid()
        {
            Ob = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Color.ShowAllColor(Ob);
            grdSearchResult.DataBind();
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
            lblUpdateId.Text = grdSearchResult.SelectedDataKey.Value.ToString(); 
            txtJobType.Text = HttpUtility.HtmlDecode(grdSearchResult.SelectedRow.Cells[1].Text) == "&nbsp;" ? "" : HttpUtility.HtmlDecode(grdSearchResult.SelectedRow.Cells[1].Text);
            lblShift.Text = HttpUtility.HtmlDecode(grdSearchResult.SelectedRow.Cells[1].Text) == "&nbsp;" ? "" : HttpUtility.HtmlDecode(grdSearchResult.SelectedRow.Cells[1].Text);
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
            Ob.Result = BAL.BALFactory.Instance.BAL_Color.UpdateColor(Ob);
            if (Ob.Result == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Color information updated sucessfully.";
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
            Ob.Result = BAL.BALFactory.Instance.BAL_Color.DeleteColor(Ob);
            if (Ob.Result == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Color deleted sucessfully.";
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
}