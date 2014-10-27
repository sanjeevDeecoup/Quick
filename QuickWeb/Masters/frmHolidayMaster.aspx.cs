using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmHolidayMaster : System.Web.UI.Page
    {
        private DTO.Holiday Ob = new DTO.Holiday();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid("");
                txtHolidayDate.Focus();
            }
        }

        public DTO.Holiday SetValue()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.weeklyoff = drpWeekend.SelectedItem.Text;
            Ob.date = txtHolidayDate.Text;
            Ob.description = txtHolidayDescription.Text;
            Ob.holidayid = Convert.ToInt32(hdnId.Value);
            Ob.Active = 1;
            return Ob;
        }

        private void BindGrid(string strSearchBy)
        {
            Ob.description = strSearchBy;
            Ob.BranchId = Globals.BranchID;
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BL_HolidayMaster.BindGridView(Ob);
            grdHoliday.DataSource = BAL.BALFactory.Instance.BL_HolidayMaster.BindGridView(Ob);
            grdHoliday.DataBind();
            if (ds.Tables[1].Rows.Count > 0)
                PrjClass.SetItemInDropDown(drpWeekend, ds.Tables[1].Rows[0]["weeklyoff"].ToString(), true, false);
        }

        protected void btnHolidaySave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BL_HolidayMaster.SaveHolidayMaster(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblHolidaySucess.Text = "Holiday event created sucessfully";
                BindGrid("");
                Reset();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblHolidayErr.Text = res;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BL_HolidayMaster.UpdateHolidayMaster(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblHolidaySucess.Text = "Holiday event information updated sucessfully.";
                BindGrid("");
                Reset();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblHolidayErr.Text = res;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.Active = 0;
            res = BAL.BALFactory.Instance.BL_HolidayMaster.deleteHolidayMaster(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblHolidaySucess.Text = "Holiday  event deleted sucessfully.";
                BindGrid("");
                Reset();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblHolidayErr.Text = res;
            }
        }
        protected void grdHoliday_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdHoliday.SelectedRow;
            int Rowno = row.RowIndex;
            hdnId.Value = ((Label)grdHoliday.Rows[Rowno].FindControl("lblId")).Text;
            PrjClass.SetItemInDropDown(drpWeekend, ((Label)grdHoliday.Rows[Rowno].FindControl("lblWeek")).Text, true, false);
            txtHolidayDescription.Text = grdHoliday.SelectedRow.Cells[3].Text;
            txtHolidayDate.Text = grdHoliday.SelectedRow.Cells[4].Text;
            btnHolidaySave.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
        }

        private void Reset()
        {
            txtHolidayDate.Text = "";
            txtHolidayDescription.Text = "";
            btnHolidaySave.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            hdnId.Value = "0";
            txtHolidayDate.Focus();
        }

        protected void btnWeekendSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BL_HolidayMaster.UpdateWeekend(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblHolidaySucess.Text = "Weekly off configured successfully.";
                BindGrid("");
                Reset();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblHolidayErr.Text = res;
            }
        }
    }
}