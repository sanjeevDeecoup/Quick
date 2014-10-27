using System;
using System.Data;

namespace QuickWeb.Masters
{
    public partial class CustomerNewMaster : System.Web.UI.Page
    {
        private DTO.Common Common = new DTO.Common();
        private DTO.CustomerMaster Customer = new DTO.CustomerMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public DTO.CustomerMaster SetValue()
        {
            Customer.Id = Convert.ToInt32(hdnId.Value);
            Customer.Id = Convert.ToInt32(txtCustID.Value);
            Customer.CustomerName = txtCustName.Value;
            Customer.CustomerName = txtCustomerName.Value;
            Customer.CustomerPhone = txtPhone.Value;
            Customer.CustomerAddress = txtAddress.Value;
            Customer.CustomerEmailId = txtEmail.Value;
            Customer.BirthDate = txtBirDate.Value;
            Customer.AnniversaryDate = txtAnniversery.Value;
            Customer.CustomerRefferredBy = txtRefBy.Value;
            Customer.DefaultDiscountRate = Convert.ToInt32(txtDiscount.Value);
            Customer.CustomerPriority = txtPriority.Value;
            Customer.Remarks = txtRemarks.Value;
            Customer.BranchId = Globals.BranchID;
            return Customer;
        }

        public void clearAll()
        {
            txtAddress.Value = "";
            txtAnniversery.Value = "";
            txtBirDate.Value = "";
            txtBookingNumber.Value = "";
            txtCustID.Value = "";
            txtCustName.Value = "";
            txtCustomerName.Value = "";
            txtDiscount.Value = "";
            txtEmail.Value = "";
            txtPhone.Value = "";
            txtPriority.Value = "";
            txtRefBy.Value = "";
            txtRemarks.Value = "";
        }

        protected void btnCreateNewCustomer_ServerClick(object sender, EventArgs e)
        {
        }

        protected void btnSearchByInvoice_ServerClick(object sender, EventArgs e)
        {
            clearAll();
            hdnStatus.Value = "true";
            txtCustomerName.Visible = false;
            txtBookingNumber.Visible = true;
        }

        protected void btnSearchByCustomer_ServerClick(object sender, EventArgs e)
        {
            clearAll();
            hdnName.Value = "true";
            txtCustomerName.Visible = true;
            txtBookingNumber.Visible = false;
        }

        protected void btnEnter_ServerClick(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            if (hdnStatus.Value == "true")
            {
                Customer.CustomerName = "";
                Customer.BookingNumber = txtBookingNumber.Value;
                Customer.BranchId = Globals.BranchID;

                ds = BAL.BALFactory.Instance.BAL_NewCustomer.SearchCUstomer(Customer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtCustID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                    txtCustName.Value = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                    txtAddress.Value = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                    txtEmail.Value = ds.Tables[0].Rows[0]["CustomerEmailId"].ToString();
                    txtBirDate.Value = ds.Tables[0].Rows[0]["BirthDate"].ToString();
                    txtAnniversery.Value = ds.Tables[0].Rows[0]["AnniversaryDate"].ToString();
                    txtRefBy.Value = ds.Tables[0].Rows[0]["CustomerRefferredBy"].ToString();
                    txtRemarks.Value = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtPhone.Value = ds.Tables[0].Rows[0]["CustomerPhone"].ToString();
                    txtPriority.Value = ds.Tables[0].Rows[0]["Priority"].ToString();
                    txtDiscount.Value = ds.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
                }
            }
            else
            {
                Customer.CustomerName = txtCustomerName.Value;
                Customer.BranchId = Globals.BranchID;
                ds = BAL.BALFactory.Instance.BAL_NewCustomer.SearchCUstomer(Customer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtCustID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                    txtCustName.Value = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                    txtAddress.Value = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                    txtEmail.Value = ds.Tables[0].Rows[0]["CustomerEmailId"].ToString();
                    txtBirDate.Value = ds.Tables[0].Rows[0]["BirthDate"].ToString();
                    txtAnniversery.Value = ds.Tables[0].Rows[0]["AnniversaryDate"].ToString();
                    txtRefBy.Value = ds.Tables[0].Rows[0]["CustomerRefferredBy"].ToString();
                    txtRemarks.Value = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtPhone.Value = ds.Tables[0].Rows[0]["CustomerPhone"].ToString();
                    txtPriority.Value = ds.Tables[0].Rows[0]["Priority"].ToString();
                    txtDiscount.Value = ds.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
                }
            }
        }
    }
}