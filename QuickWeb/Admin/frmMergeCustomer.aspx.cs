using System;

namespace QuickWeb.Admin
{
    public partial class frmMergeCustomer : System.Web.UI.Page
    {
        private DTO.CustomerMaster Ob = new DTO.CustomerMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCustomerSearch.Focus();
            }
        }

        private string[] customerName, DuplicateCustomer;

        protected void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                customerName = txtCustomerSearch.Text.Split('-');
                hdnNewCustomer.Value = customerName[0].ToString();
                txtCustomerSearch.Text = customerName[1].ToString();
                txtDuplicateCustomer.Focus();
                grdCustomerSearch.DataSource = null;
                grdCustomerSearch.DataBind();
                grdCustomerSearch.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.BindGridCustomerSearch(customerName[0].ToString().Trim(), Globals.BranchID);
                grdCustomerSearch.DataBind();
            }
            catch (Exception ex)
            {
                Session["ReturnMsg"] = "Please enter valid customer.";
                txtCustomerSearch.Focus();
                txtCustomerSearch.Attributes.Add("onfocus", "javascript:select();");
            }
        }

        protected void txtDuplicateCustomer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DuplicateCustomer = txtDuplicateCustomer.Text.Split('-');
                hdnOldCustomer.Value = DuplicateCustomer[0].ToString();
                txtDuplicateCustomer.Text = DuplicateCustomer[1].ToString();
                btnMerge.ForeColor.GetBrightness();
                btnMerge.Focus();
                GridViewDuplicate.DataSource = null;
                GridViewDuplicate.DataBind();
                GridViewDuplicate.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.BindGridCustomerSearch(DuplicateCustomer[0].ToString().Trim(), Globals.BranchID);
                GridViewDuplicate.DataBind();
            }
            catch (Exception ex)
            {
                Session["ReturnMsg"] = "Please enter valid customer.";
                txtDuplicateCustomer.Focus();
                txtDuplicateCustomer.Attributes.Add("onfocus", "javascript:select();");
            }
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            SetValue();
            string res = string.Empty;
            if (hdnNewCustomer.Value == hdnOldCustomer.Value)
            {
                Session["ReturnMsg"] = "Both customer are same.";
                return;
            }
            res = BAL.BALFactory.Instance.BL_CustomerMaster.MergeDuplicateCustomer(Ob);
            if (res == "Record Saved")
            {
                Session["ReturnMsg"] = "Customer merge sucessfully";
                txtCustomerSearch.Text = "";
                txtDuplicateCustomer.Text = "";
                txtCustomerSearch.Focus();
                GridViewDuplicate.DataSource = null;
                GridViewDuplicate.DataBind();
                grdCustomerSearch.DataSource = null;
                grdCustomerSearch.DataBind();
            }
            else
            {
                lblError.Text = res.ToString();
            }
        }

        public DTO.CustomerMaster SetValue()
        {
            Ob.CustomerSalutation = hdnNewCustomer.Value;
            Ob.CustomerName = hdnOldCustomer.Value;
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }
    }
}