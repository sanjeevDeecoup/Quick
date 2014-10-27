using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmCategoryMaster : System.Web.UI.Page
    {
        private DTO.Category Ob = new DTO.Category();

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
            res = BAL.BALFactory.Instance.BL_CategoryMaster.SaveCategoryMaster(Ob);
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

            res = BAL.BALFactory.Instance.BL_CategoryMaster.UpdateCategoryMaster(Ob);
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
            grdCategory.DataSource = BAL.BALFactory.Instance.BL_CategoryMaster.BindGridView(Ob);
            grdCategory.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            setDefault();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCategoryMaster.aspx", false);
        }

        public void setDefault()
        {
            btnSave.Visible = true;
            btnEdit.Visible = false;
            txtCategory.Focus();
            txtCategory.Text = "";
            Ob = SetValue();
            grdCategory.DataSource = BAL.BALFactory.Instance.BL_CategoryMaster.ShowAll(Ob);
            grdCategory.DataBind();
        }

        public DTO.Category SetValue()
        {
            Ob = new DTO.Category();
            Ob.CategoryName = txtCategory.Text.ToUpper();
            Ob.Active = 1;
            Ob.CategoryID = lblUpdateId.Text;
            Ob.BranchId = Globals.BranchID;
            Ob.ImageName = hdnImageName.Value;
            return Ob;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = SetValue();
            Ob.Active = 0;
            res = BAL.BALFactory.Instance.BL_CategoryMaster.DeleteCategory(Ob);
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

        protected void grdCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdCategory.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdCategory.Rows[Rowno].FindControl("lblId")).Text;
            txtCategory.Text = grdCategory.SelectedRow.Cells[1].Text;
            img.ImageUrl = "~/images/Categories/" + ((Label)grdCategory.Rows[Rowno].FindControl("lblImage")).Text;
            hdnImageName.Value = ((Label)grdCategory.Rows[Rowno].FindControl("lblImage")).Text;
            txtCategory.Focus();
            txtCategory.Attributes.Add("onfocus", "javascript:select();");
            txtCategory.Focus();
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnDelete.Visible = true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string PhotoFile = GetPhotoFileName();
            hdnImageName.Value = PhotoFile;
            string imgpath = Server.MapPath("~/images/Categories") + "//" + PhotoFile;
            System.IO.File.Delete(imgpath);
            fltImage.PostedFile.SaveAs(imgpath);
            img.ImageUrl = "~/images/Categories/" + PhotoFile;
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
    }
}