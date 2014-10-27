using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmColors : System.Web.UI.Page
    {
        private DTO.ColorMaster Ob = new DTO.ColorMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid("");
                txtColor.Focus();
                SetValue();
            }
        }

        public DTO.ColorMaster SetValue()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.ColorName = txtColor.Text;
            Ob.ImageName = upimg.Value;
            Ob.Active = 1;
            return Ob;
        }

        private void BindGrid(string strSearchBy)
        {
            Ob.ColorName = strSearchBy;
            Ob.BranchId = Globals.BranchID;
            grdColor.DataSource = BAL.BALFactory.Instance.BL_ColorMaster.BindGridView(Ob);
            grdColor.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;

            if (fltImage.Value != "")
            {
                btnupload_Click(null, null);
            }
            if (txtColor.Text == "")
            {
                lblErr.Text = "Please enter color name to save.";
                return;
            }
            SetValue();
            res = BAL.BALFactory.Instance.BL_ColorMaster.SaveColorMaster(Ob);
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
            if (fltImage.Value != "")
            {
                btnupload_Click(null, null);
            }
            if (txtColor.Text == "")
            {
                lblErr.Text = "Please enter color name to save.";
                return;
            }
            SetValue();
            Ob.ColorID = Convert.ToInt32(lblUpdateId.Text);
            res = BAL.BALFactory.Instance.BL_ColorMaster.UpdateColorMaster(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Reocrd updated";
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
            if (txtColor.Text == "")
            {
                lblErr.Text = "Please Enter some text in color name for searching.";
                return;
            }
            BindGrid(txtColor.Text);
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindGrid("");
        }

        protected void RefreshForm()
        {
            txtColor.Text = "";
            lblUpdateId.Text = "";
            //imgStudentPhoto.ImageUrl = "";
            txtColor.Focus();
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            BindGrid("");
            RefreshForm();
        }

        protected void grdComment_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdColor.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdColor.Rows[Rowno].FindControl("lblId")).Text;
            txtColor.Text = grdColor.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdColor.SelectedRow.Cells[1].Text;
            imgStudentPhoto.ImageUrl = "~/images/Colors/" + ((Label)grdColor.Rows[Rowno].FindControl("lblImage")).Text;
            upimg.Value = ((Label)grdColor.Rows[Rowno].FindControl("lblImage")).Text;
            txtColor.Attributes.Add("onfocus", "javascript:select();");
            txtColor.Focus();
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = false;
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

        protected void btnupload_Click(object sender, EventArgs e)
        {
            string PhotoFile = GetPhotoFileName();
            string imgpath = Server.MapPath("~/images/Colors") + "//" + PhotoFile;
            System.IO.File.Delete(imgpath);
            fltImage.PostedFile.SaveAs(imgpath);
            imgStudentPhoto.ImageUrl = "~/images/Colors/" + PhotoFile;
            upimg.Value = PhotoFile;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = SetValue();
            Ob.Active = 0;
            Ob.ColorID = Int32.Parse(lblUpdateId.Text);
            res = BAL.BALFactory.Instance.BL_ColorMaster.deleteColorMaster(Ob);

            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Deactivated";
                btnDelete.Visible = false;
                BindGrid("");
                RefreshForm();
                imgStudentPhoto.ImageUrl = "";
            }
            else
                lblErr.Text = res;
        }
    }
}