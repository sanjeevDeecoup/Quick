using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Services;


namespace QuickWeb.New_Admin
{
    public partial class frmNewCustomer : System.Web.UI.Page
    {
        private DTO.CustomerMaster Ob = new DTO.CustomerMaster();

        private static DTO.Report Ob1 = new DTO.Report();
        private string status = string.Empty;
        private static string _memberShipId = string.Empty, _prevMobile = string.Empty;
        private static string _barCode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPage(this.Page);
                if (Session["UserBranch"] == null || Session["UserType"] == null || Session["UserName"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                txtCustomerSearch.Focus();
                if (Request.QueryString["CCode"] != null)
                {
                    hdnIsCustCode.Value = "1";
                    hdncustcode.Value = Request.QueryString["CCode"].ToString();
                }
                if (Request.QueryString["CName"] != null)
                {
                    hdnIsNewCustomer.Value = "1";
                    hdnNewCustName.Value = Request.QueryString["CName"].ToString();
                }
                if (Request.QueryString["AddNew"] != null)
                {
                    txtCustName.Focus();
                    hdnIsNew.Value = "1";
                }
                BindDefault();
                binddrpsms();
                binddrpdefaultsms();
                ShowDefaultDiscountButton();
            }            
        }

        private void BindDefault()
        {           
            BindPriroity();
            BindRateList();
            // BindDummyRow();
            //  divCustDetail.Visible = false;
        }

        protected void BindRateList()
        {
            ddlRateList.DataSource = BAL.BALFactory.Instance.BL_PriceList.BindAllListMasters(Globals.BranchID);
            ddlRateList.DataTextField = "name";
            ddlRateList.DataValueField = "rateListId";
            ddlRateList.DataBind();
        }

        protected void BindPriroity()
        {
            Ob.BranchId = Globals.BranchID;
            drpPriority.DataSource = BAL.BALFactory.Instance.BL_CustomerMaster.BindPriority(Ob);
            drpPriority.DataTextField = "Priority";
            drpPriority.DataValueField = "PriorityID";
            drpPriority.DataBind();
        }

        private void binddrpsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.ShowAll(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpsmstemplate.DataSource = ds.Tables[0];
                drpsmstemplate.DataTextField = "template";
                drpsmstemplate.DataValueField = "smsid";
                drpsmstemplate.DataBind();
            }
        }

        private void binddrpdefaultsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
            PrjClass.SetItemInDropDown(drpsmstemplate, ds.Tables[4].Rows[0]["Template"].ToString(), true, false);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public static string SendPendingSMS(string CustCode,string smsValue)
        {
            string res = string.Empty, pendingPcs = string.Empty, pendingAmount = string.Empty, strCustomerName = string.Empty, strCustomerMobileNo = string.Empty, strDump = string.Empty;
            Ob1.BranchId = Globals.BranchID;
            Ob1.CustId = CustCode;
            Ob1.StrCodes = "";
            DataSet ds = BAL.BALFactory.Instance.Bal_Report.GetCustomerWiseSummaryPendingSms(Ob1);
            var BID = Globals.BranchID;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCustomerName = ds.Tables[0].Rows[i]["CustomerName"].ToString();
                    strCustomerMobileNo = ds.Tables[0].Rows[i]["CustomerMobile"].ToString();
                    pendingPcs = ds.Tables[0].Rows[i]["Pending"].ToString();
                    pendingAmount = ds.Tables[0].Rows[i]["PendingAmount"].ToString();
                    if (pendingPcs == "0" && pendingAmount == "0")
                    {
                        res = "No pending amount and clothes found for this customer.";
                    }
                    else
                    {
                        if (strCustomerMobileNo != "")
                        {
                            AppClass.GoPendingStockMsg(BID, strCustomerMobileNo, smsValue, strCustomerName, pendingPcs, pendingAmount);
                            res = "True";
                        }
                    }
                }
            }
            else
            {
                res = "No pending amount and clothes found for this customer.";
            }
            return res;
        }
        public void ShowDefaultDiscountButton()
        {
            bool blnRight = false;
            blnRight = AppClass.CheckButtonRights(SpecialAccessRightName.DefaultDiscount);
            if (blnRight)
            {
                divDefaultDiscount.Attributes.Add("style", "display:inline");              
            }
            else
            {
                divDefaultDiscount.Attributes.Add("style", "display:none");               
            }
        }
    }
}