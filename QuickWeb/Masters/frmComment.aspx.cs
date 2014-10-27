using System;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmComment : System.Web.UI.Page
    {
        private DTO.Comment Ob = new DTO.Comment();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDefault();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = SetValue();
            res = BAL.BALFactory.Instance.BAL_Comment.SaveCommentMaster(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = res;
                setDefault();
            }
            else
                lblErr.Text = res;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = SetValue();
            res = BAL.BALFactory.Instance.BAL_Comment.UpdateCommentMaster(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = res;
                setDefault();
            }
            else
                lblErr.Text = res;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            grdComment.DataSource = BAL.BALFactory.Instance.BAL_Comment.BindGridView(Ob);
            grdComment.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            setDefault();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmComment.aspx", false);
        }

        public void setDefault()
        {
            btnSave.Visible = true;
            btnEdit.Visible = false;
            txtComment.Focus();
            txtComment.Text = "";
            Ob = SetValue();
            grdComment.DataSource = BAL.BALFactory.Instance.BAL_Comment.ShowAll(Ob);
            grdComment.DataBind();
        }

        public DTO.Comment SetValue()
        {
            Ob = new DTO.Comment();
            Ob.CommentName = txtComment.Text.ToUpper();
            Ob.Active = 1;
            Ob.DateModified = Globals.date[0].ToString();
            Ob.CommentID = lblUpdateId.Text;
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        protected void grdComment_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdComment.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdComment.Rows[Rowno].FindControl("lblId")).Text;
            txtComment.Text = grdComment.SelectedRow.Cells[1].Text;
            txtComment.Focus();
            txtComment.Attributes.Add("onfocus", "javascript:select();");
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnDelete.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.CommentID = lblUpdateId.Text;
            Ob.Active = 0;
            res = BAL.BALFactory.Instance.BAL_Comment.DeleteComment(Ob);
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