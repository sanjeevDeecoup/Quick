using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace QuickWeb.Config_Setting
{
    public partial class smsconfig : System.Web.UI.Page
    {
        private DTO.sms Ob = new DTO.sms();
        protected string Defaultsms = ConfigLabel.Defaultsms;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDefault();

                for (int i = 2; i <= 6; i++)
                {
                    Drpsender.Items.Add(i.ToString());
                    Drpusername.Items.Add(i.ToString());
                    Drppassword.Items.Add(i.ToString());
                    Drpmassage.Items.Add(i.ToString());
                    Drpmobile.Items.Add(i.ToString());
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = SetValue();
            res = BAL.BALFactory.Instance.BAL_sms.Savesmsconfig(Ob);
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
            res = BAL.BALFactory.Instance.BAL_sms.Updatesmsconfigr(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record Updated";
                setDefault();
            }
            else
                lblErr.Text = res;
            if (res == "")
            { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Ob = SetValue();
            grdsms.DataSource = BAL.BALFactory.Instance.BAL_sms.BindGridView(Ob);
            grdsms.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            setDefault();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("smsconfig.aspx", false);
        }

        public void setDefault()
        {
            btnSave.Visible = true;
            btnEdit.Visible = false;
            txttemplate.Enabled = true;
            txttemplate.Focus();
            txttemplate.Text = "";
            txtmessage.Text = "";
            drpDefaultMsg.SelectedIndex = -1;
            Ob = SetValue();
            grdsms.DataSource = BAL.BALFactory.Instance.BAL_sms.ShowAll(Ob);
            grdsms.DataBind();
            Select1.SelectedIndex = -1;
        }

        public DTO.sms SetValue()
        {
            Ob = new DTO.sms();
            Ob.Template = txttemplate.Text;
            Ob.Massage = txtmessage.Text;
            Ob.Active = 1;
            Ob.DateModified = Globals.date[0].ToString();
            Ob.SmsId = lblUpdateId.Text;
            Ob.BranchId = Globals.BranchID;
            Ob.DefaultMsg = drpDefaultMsg.SelectedItem.Text;
            return Ob;
        }

        protected void grdsms_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdsms.SelectedRow;
            int Rowno = row.RowIndex;
            lblUpdateId.Text = ((Label)grdsms.Rows[Rowno].FindControl("lblId")).Text;
            PrjClass.SetItemInDropDown(drpDefaultMsg, ((Label)grdsms.Rows[Rowno].FindControl("lblDefaultMsg")).Text, true, false);
            txttemplate.Text = grdsms.SelectedRow.Cells[1].Text;
            if (txttemplate.Text == "No Message")
            {
                txttemplate.Enabled = false;
            }
            else
            {
                txttemplate.Enabled = true;
            }
            txtmessage.Text = grdsms.SelectedRow.Cells[3].Text;
            txttemplate.Focus();
            txttemplate.Attributes.Add("onfocus", "javascript:select();");
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnDelete.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SetValue();
            Ob.SmsId = lblUpdateId.Text;
            Ob.Active = 0;
            res = BAL.BALFactory.Instance.BAL_sms.Deletesms(Ob);
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

        protected void btnsmstemplate_Click(object sender, EventArgs e)
        {
            tdSetsmstemplate.Visible = true;
            tdapi.Visible = false;
            tddefaultsms.Visible = false;
        }

        protected void btnsmsserversetting_Click(object sender, EventArgs e)
        {
            tdSetsmstemplate.Visible = false;
            tdapi.Visible = true;
            tddefaultsms.Visible = false;
            fetchapisetting();
        }

        private void fetchapisetting()
        {
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtuserName.Text = ds.Tables[0].Rows[0]["userid"].ToString();
                txtpassword.Text = ds.Tables[0].Rows[0]["password"].ToString();
                txtapi.Text = ds.Tables[0].Rows[0]["api"].ToString();
                Txtsenderid.Text = ds.Tables[0].Rows[0]["senderid"].ToString();

                Txtsenderidpreview.Text = ds.Tables[0].Rows[0]["senderdemo"].ToString();
                txtusernamepreview.Text = ds.Tables[0].Rows[0]["useriddemo"].ToString();
                txtpasswordpreview.Text = ds.Tables[0].Rows[0]["passworddemo"].ToString();
                Txtmassagepreview.Text = ds.Tables[0].Rows[0]["massagedemo"].ToString();
                txtmobile.Text = ds.Tables[0].Rows[0]["mobiledemo"].ToString();
                PrjClass.SetItemInDropDown(Drpsender, ds.Tables[0].Rows[0]["senderposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpusername, ds.Tables[0].Rows[0]["userposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drppassword, ds.Tables[0].Rows[0]["passwordposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpmobile, ds.Tables[0].Rows[0]["mobileposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpmassage, ds.Tables[0].Rows[0]["massageposition"].ToString(), true, false);
            }
        }

        public DTO.sms getvalueapi()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchapi(Ob);
            Ob.userid = txtuserName.Text;
            if (txtpassword.Text == "")
            {
                Ob.password = ds.Tables[0].Rows[0]["password"].ToString();
            }
            else
            {
                Ob.password = txtpassword.Text;
            }
            Ob.api = txtapi.Text;
            Ob.senderid = Txtsenderid.Text;

            Ob.senderdemo = Txtsenderidpreview.Text;
            Ob.massagedemo = Txtmassagepreview.Text;
            Ob.mobiledemo = txtmobile.Text;
            Ob.useriddemo = txtusernamepreview.Text;
            Ob.passworddemo = txtpasswordpreview.Text;
            Ob.senderposition = Drpsender.SelectedItem.ToString();
            Ob.mobileposition = Drpmobile.SelectedItem.ToString();
            Ob.passwordposition = Drppassword.SelectedItem.ToString();
            Ob.massageposition = Drpmassage.SelectedItem.ToString();
            Ob.userposition = Drpusername.SelectedItem.ToString();
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        protected void btnapisettting_Click(object sender, EventArgs e)
        {
            Ob = getvalueapi();
            string res = BAL.BALFactory.Instance.BAL_sms.apiupdate(Ob);
            if (res == "Record Saved")
            {
                lblapiSuccess.Text = "Update Sucessfully";
            }
            else
            {
                lblapiError.Text = "Error";
            }
        }

        protected void btndefaultsms_Click(object sender, EventArgs e)
        {
            tdSetsmstemplate.Visible = false;
            tdapi.Visible = false;
            tddefaultsms.Visible = true;
            binddrpsms();
            binddrpdefault();
        }

        private void binddrpdefault()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
            PrjClass.SetItemInDropDown(Drpnewbooking, ds.Tables[3].Rows[0]["Template"].ToString(), true, false);
            PrjClass.SetItemInDropDown(Drpclothready, ds.Tables[1].Rows[0]["Template"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpdelivery, ds.Tables[2].Rows[0]["Template"].ToString(), true, false);
            if (ds.Tables[0].Rows.Count > 0)
            {
                btpreview_Click(null, null);
            }
        }

        private void binddrpsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.ShowAll(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Drpnewbooking.DataSource = ds.Tables[0];
                Drpnewbooking.DataTextField = "template";
                Drpnewbooking.DataValueField = "smsid";
                Drpnewbooking.DataBind();

                Drpclothready.DataSource = ds.Tables[0];
                Drpclothready.DataTextField = "template";
                Drpclothready.DataValueField = "smsid";
                Drpclothready.DataBind();

                drpdelivery.DataSource = ds.Tables[0];
                drpdelivery.DataTextField = "template";
                drpdelivery.DataValueField = "smsid";
                drpdelivery.DataBind();
            }
        }

        public DTO.sms previewdropfetch()
        {
            Ob = new DTO.sms();
            Ob.bookingsms = Drpnewbooking.SelectedValue.ToString();
            Ob.clothsms = Drpclothready.SelectedValue.ToString();
            Ob.deliverysms = drpdelivery.SelectedValue.ToString();
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        protected void btpreview_Click(object sender, EventArgs e)
        {
            DTO.sms Ob = new DTO.sms();
            Ob = previewdropfetch();
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_sms.previewbooking(Ob);
            //Lbbooking.Text = ds.Tables[0].Rows[0]["massage"].ToString();
            //Lbclothready.Text = ds.Tables[1].Rows[0]["massage"].ToString();
            //Lbdelivery.Text = ds.Tables[2].Rows[0]["massage"].ToString();

            string databasemessage = ds.Tables[0].Rows[0]["massage"].ToString();
            string databasemessage1 = ds.Tables[1].Rows[0]["massage"].ToString();
            string databasemessage2 = ds.Tables[2].Rows[0]["massage"].ToString();

            Dictionary<string, string> replacements = new Dictionary<string, string>()
            {
            {"[Customer Name]",ds.Tables[3].Rows[0]["CustomerName"].ToString()},
            {"[Booking No]",ds.Tables[3].Rows[0]["BookingNumber"].ToString()},
            {"[Amount]",ds.Tables[3].Rows[0]["TotalAmount"].ToString()},
            {"[Quantity]",ds.Tables[3].Rows[0]["TotalQty"].ToString()},
            {"[Delivery Date]", ds.Tables[3].Rows[0]["duedate"].ToString()}
            };

            Dictionary<string, string> replacements1 = new Dictionary<string, string>()
            {
            {"[Customer Name]",ds.Tables[3].Rows[0]["CustomerName"].ToString()},
            {"[Booking No]",ds.Tables[3].Rows[0]["BookingNumber"].ToString()},
            {"[Amount]",ds.Tables[3].Rows[0]["TotalAmount"].ToString()},
            {"[Quantity]",ds.Tables[3].Rows[0]["TotalQty"].ToString()},
            {"[Delivery Date]", ds.Tables[3].Rows[0]["duedate"].ToString()}
            };

            Dictionary<string, string> replacements2 = new Dictionary<string, string>()
            {
            {"[Customer Name]",ds.Tables[3].Rows[0]["CustomerName"].ToString()},
            {"[Booking No]",ds.Tables[3].Rows[0]["BookingNumber"].ToString()},
            {"[Amount]",ds.Tables[3].Rows[0]["TotalAmount"].ToString()},
            {"[Quantity]",ds.Tables[3].Rows[0]["TotalQty"].ToString()},
            {"[Delivery Date]", ds.Tables[3].Rows[0]["duedate"].ToString()}
            };

            foreach (var r in replacements)
            {
                databasemessage = databasemessage.Replace(r.Key, r.Value);
            }
            foreach (var r in replacements1)
            {
                databasemessage1 = databasemessage1.Replace(r.Key, r.Value);
            }
            foreach (var r in replacements2)
            {
                databasemessage2 = databasemessage2.Replace(r.Key, r.Value);
            }

            Lbbooking.Text = databasemessage;
            Lbclothready.Text = databasemessage1;
            Lbdelivery.Text = databasemessage2;
        }

        public DTO.sms defaultsmsSetValue()
        {
            Ob = new DTO.sms();
            Ob.bookingsms = Drpnewbooking.SelectedValue.ToString();
            Ob.clothsms = Drpclothready.SelectedValue.ToString();
            Ob.deliverysms = drpdelivery.SelectedValue.ToString();
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        protected void btdefaultsmsupdate_Click(object sender, EventArgs e)
        {
            string res = "";
            Ob = defaultsmsSetValue();
            res = BAL.BALFactory.Instance.BAL_sms.defaultsmsupdate(Ob);
            if (res == "Record Saved")
            {
                Lbdefaultsmssuccess.Text = "Record Updated";
            }
            else
            {
                Lbdefaultsmserror.Text = "Error";
            }
        }
    }
}