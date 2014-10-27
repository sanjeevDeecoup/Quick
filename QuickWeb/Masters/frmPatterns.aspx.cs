using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmPatterns : System.Web.UI.Page
    {
        private DTO.Patterns Ob = new DTO.Patterns();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDefault();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (fltImage.Value != "")
                btnUpload_Click(null, null);
            string res = "";
            Ob = SetValue();
            res = BAL.BALFactory.Instance.BAL_Patterns.SavePatternMaster(Ob);
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
            if (fltImage.Value != "")
                btnUpload_Click(null, null);
            string res = "";
            Ob = SetValue();
            res = BAL.BALFactory.Instance.BAL_Patterns.UpdatePatternMaster(Ob);
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
            grdComment.DataSource = BAL.BALFactory.Instance.BAL_Patterns.BindGridView(Ob);
            grdComment.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            setDefault();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPatterns.aspx", false);
        }

        public void setDefault()
        {
            btnSave.Visible = true;
            btnEdit.Visible = false;
            txtPatterns.Focus();
            txtPatterns.Text = "";
            Ob = SetValue();
            grdComment.DataSource = BAL.BALFactory.Instance.BAL_Patterns.ShowAll(Ob);
            grdComment.DataBind();
        }

        public DTO.Patterns SetValue()
        {
            Ob = new DTO.Patterns();
            Ob.PatternName = txtPatterns.Text.ToUpper();
            Ob.Active = 1;
            Ob.DateModified = Globals.date[0].ToString();
            Ob.PatternID = lblUpdateId.Text;
            Ob.BranchId = Globals.BranchID;
            Ob.ImageName = hdnImageName.Value;
            return Ob;
        }

        protected void grdComment_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdComment.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdComment.Rows[Rowno].FindControl("lblId")).Text;
            txtPatterns.Text = grdComment.SelectedRow.Cells[1].Text;
            img.ImageUrl = "~/images/Patterns/" + ((Label)grdComment.Rows[Rowno].FindControl("lblImage")).Text;
            hdnImageName.Value = ((Label)grdComment.Rows[Rowno].FindControl("lblImage")).Text;
            txtPatterns.Attributes.Add("onfocus", "javascript:select();");
            txtPatterns.Focus();
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnDelete.Visible = true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string PhotoFile = GetPhotoFileName();
            hdnImageName.Value = PhotoFile;
            string imgpath = Server.MapPath("~/images/Patterns") + "//" + PhotoFile;
            System.IO.File.Delete(imgpath);
            fltImage.PostedFile.SaveAs(imgpath);
            img.ImageUrl = "~/images/Patterns/" + PhotoFile;
        }

        private string GetPhotoFileName()
        {
            HtmlInputFile fupPhoto = ((HtmlInputFile)fltImage);
            string PhotoFile = "";
            if (fupPhoto.Value != "")
            {
                string fname = fupPhoto.PostedFile.FileName;
                if (fname.Contains("\\"))
                    fname = fname.Substring(fname.LastIndexOf("\\"));
                if (fname.StartsWith("\\")) fname = fname.Substring(1);
                PhotoFile = fname;
            }
            return PhotoFile;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = SetValue();
            Ob.Active = 0;
            res = BAL.BALFactory.Instance.BAL_Patterns.DeletePatterns(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Deactivated";
                btnDelete.Visible = false;
                setDefault();
                img.ImageUrl = "";
            }
            else
                lblErr.Text = res;
        }
    }
}