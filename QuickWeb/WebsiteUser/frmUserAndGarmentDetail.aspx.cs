using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading.Tasks;

namespace QuickWeb.Website_User
{
    public partial class frmUserAndGarmentDetail : BasePage
    {
        DTO.Common Ob = new DTO.Common();
        # region userControlControls

        TextBox ucTxtRptFrom, ucTxtRptTo;
        DropDownList ucDrpMnthLst, ucDrpYrLst;
        RadioButton ucRadFrom, ucRadMnth;
        CheckBox ucChkHome;

        # endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BID"] == null || Request.QueryString["BID"] == "" || Request.QueryString["CustCode"] == null || Request.QueryString["CustCode"] == "" || Request.QueryString["BranchName"] == null || Request.QueryString["BranchName"] == "")
            {
                Response.Redirect("~/WebsiteUser/UserLogin.aspx", false);
            }
            # region setUserControl

            ucTxtRptFrom = ((TextBox)uc1.FindControl("txtReportFrom"));
            ucTxtRptTo = ((TextBox)uc1.FindControl("txtReportUpto"));
            ucDrpMnthLst = ((DropDownList)uc1.FindControl("drpMonthList"));
            ucDrpYrLst = ((DropDownList)uc1.FindControl("drpYearList"));
            ucRadFrom = ((RadioButton)uc1.FindControl("radReportFrom"));
            ucRadMnth = ((RadioButton)uc1.FindControl("radReportMonthly"));
            ucChkHome = ((CheckBox)uc1.FindControl("chkShowOnlyHome"));

            # endregion
            if (!IsPostBack)
            {
                ucRadMnth.Checked = true;
                SettextBoxes();
                ShowGridData();
            }
        }

        private void SettextBoxes()
        {
            DataSet ds = new DataSet();
            Ob.BID = Request.QueryString["BID"].ToString();
            Ob.Input = Request.QueryString["CustCode"].ToString();
            ds = BAL.BALFactory.Instance.BAL_Area.FillWebsiteCustomerTextBoxes(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                txtMemID.Text = ds.Tables[0].Rows[0]["MemberShipId"].ToString();
                txtCustName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                txtCustAddress.Text = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                //drpCommPre.SelectedIndex = drpCommPre.Items.IndexOf(drpCommPre.Items.FindByValue(ds.Tables[0].Rows[0]["CommunicationMeans"].ToString()));
                PrjClass.SetItemInDropDown(drpCommPre, ds.Tables[0].Rows[0]["CommunicationMeans"].ToString(), false, false);
                txtPhone1.Text = ds.Tables[0].Rows[0]["CustomerMobile"].ToString();
                hdntempNo.Value = ds.Tables[0].Rows[0]["CustomerMobile"].ToString();
                txtPhone2.Text = ds.Tables[0].Rows[0]["CustomerPhone"].ToString();
                txtEmailID.Text = ds.Tables[0].Rows[0]["CustomerEmailId"].ToString();
                if (ds.Tables[0].Rows[0]["BirthDate"].ToString() != "01 Jan 1900")
                    txtBirthDaate.Text = ds.Tables[0].Rows[0]["BirthDate"].ToString();
                if (ds.Tables[0].Rows[0]["AnniversaryDate"].ToString() != "01 Jan 1900")
                    txtAnniversaryDate.Text = ds.Tables[0].Rows[0]["AnniversaryDate"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                btnAmount.InnerText = ds.Tables[1].Rows[0]["Balance"].ToString();
                btnCloth.InnerText = ds.Tables[1].Rows[0]["BalQty"].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                ddlRateList1.DataSource = ds.Tables[2];
                ddlRateList1.DataTextField = "CustomerMobile";
                ddlRateList1.DataValueField = "CustomerMobile";
                ddlRateList1.DataBind();
            }
        }

        protected void btnUpdateInformation_Click(object sender, EventArgs e)
        {
            Ob.BID = Request.QueryString["BID"].ToString();
            Ob.Input = Request.QueryString["CustCode"].ToString();
            Ob.LedgerName = drpCommPre.SelectedItem.Value;
            Ob.Path = txtPhone1.Text;
            Ob.Result = txtPhone2.Text;
            Ob.UserId = txtEmailID.Text;
            Ob.Id = txtBirthDaate.Text;
            Ob.ChangeName = txtAnniversaryDate.Text;
            string res = string.Empty;
            res = BAL.BALFactory.Instance.BAL_Area.UpdateCustomerDetailWebsite(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Record updated Successfully.";
            
            }
            SettextBoxes();
        }
        
        protected void btnShow_Click(object sender, EventArgs e)
        {
            Ob.BID = Request.QueryString["BID"].ToString();
            Ob.Input = Request.QueryString["CustCode"].ToString();
            string strFromDate = string.Empty, strToDate = string.Empty;
            grdOrderDetails.DataSource = null;
            grdOrderDetails.DataBind();
            if (chkBookingNo.Checked)
            {
                Ob.Result = txtBookingNo.Text;
                grdOrderDetails.DataSource = BAL.BALFactory.Instance.BAL_Area.SetDataInvoiceWise(Ob);
                grdOrderDetails.DataBind();
                txtBookingNo.Text = "";
                SettextBoxes();
                chkBookingNo.Checked = false;
            }
            else
            {
                if (ucRadFrom.Checked)
                {
                    if (ucTxtRptTo.Text == "") { ucTxtRptTo.Text = ucTxtRptFrom.Text; }
                    DateTime dt = DateTime.Parse(ucTxtRptTo.Text);
                    DateTime dt1 = DateTime.Parse(ucTxtRptFrom.Text);
                    Ob.Path = dt1.ToString("dd MMM yyyy");
                    Ob.LedgerName = dt.ToString("dd MMM yyyy");
                }
                else if (ucRadMnth.Checked)
                {
                    DateTime dt = new DateTime(int.Parse(ucDrpYrLst.SelectedItem.Text), int.Parse(ucDrpMnthLst.SelectedItem.Value), 1);
                    Ob.Path = dt.ToString("dd MMM yyyy");
                    Ob.LedgerName = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                }
                grdOrderDetails.DataSource = BAL.BALFactory.Instance.BAL_Area.SetData(Ob);
                grdOrderDetails.DataBind();
                SettextBoxes();
            }
            grdOrderDetails.Visible = true;
            divDetails.Visible = false;  
       
        }       

        protected void grdOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdOrderDetails.PageIndex = e.NewPageIndex;
            grdOrderDetails.Visible = true;
            btnShow_Click(null, null);
            divDetails.Visible = false;  
          
        }

        protected void grdOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataSet ds = new DataSet();
            string BookingNo = string.Empty;
            int rownum = int.Parse(e.CommandArgument.ToString());
            string DueDate = string.Empty;
            if (e.CommandName == "LinkButton")
            {
                if (rownum < grdOrderDetails.Rows.Count)
                {
                    BookingNo = ((LinkButton)grdOrderDetails.Rows[rownum].FindControl("hypBtnShowDetails")).Text;
                    DueDate = ((HiddenField)grdOrderDetails.Rows[rownum].FindControl("hdnDueDate")).Value;
                    lblPcs.Text = ((HiddenField)grdOrderDetails.Rows[rownum].FindControl("hdnpcs")).Value;
                    lblAmount.Text = ((HiddenField)grdOrderDetails.Rows[rownum].FindControl("hdnAmount")).Value;
                  //  string MainPage = "frmGarmentAndPaymentDetail.aspx?BID=" + Request.QueryString["BID"].ToString() + "&CustCode=" + Request.QueryString["CustCode"].ToString() + "&BranchName=" + Request.QueryString["BranchName"].ToString() + "&BranchAdress=" + Request.QueryString["BranchAdress"].ToString() + "&BN=" + BookingNo + "&DueDate=" + DueDate;
                    ds = BAL.BALFactory.Instance.BAL_City.DeliveryDetail(Request.QueryString["BID"].ToString(), BookingNo.ToString());
                    grdClothDetails.DataSource = ds.Tables[1];
                    grdClothDetails.DataBind();
                    grdPaymentDetails.DataSource = ds.Tables[4];
                    grdPaymentDetails.DataBind();

                    lblInvoiceNumber.Text = BookingNo.ToString();
                    lblDeliveryDate.Text = DueDate;


                    grdOrderDetails.Visible = false;
                    divDetails.Visible = true;      
                    
                    
                    
                    SettextBoxes();
                   // OpenUrlFromBasePage(MainPage);
                }
            }
        }

        public void ShowGridData()
        {
            Ob.BID = Request.QueryString["BID"].ToString();
            Ob.Input = Request.QueryString["CustCode"].ToString();
            if (ucRadMnth.Checked)
            {
                ucDrpMnthLst.SelectedIndex = DateTime.Today.Month - 1;
                ucDrpYrLst.Items.Clear();
                for (int i = 2000; i <= 2050; i++)
                {
                    ucDrpYrLst.Items.Add(i.ToString());
                }
                ucDrpYrLst.SelectedIndex = DateTime.Today.Year - 2000;

               DateTime dt = new DateTime(int.Parse(ucDrpYrLst.SelectedItem.Text), int.Parse(ucDrpMnthLst.SelectedItem.Value), 1);
                Ob.Path = dt.ToString("dd MMM yyyy");
                Ob.LedgerName = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }

            grdOrderDetails.DataSource = null;
            grdOrderDetails.DataBind();
            grdOrderDetails.DataSource = BAL.BALFactory.Instance.BAL_Area.GetGridData(Ob);
            grdOrderDetails.DataBind();

            grdOrderDetails.Visible = true;
        
        
        }
        //protected void OpenUrlFromBasePage(string urlToOpen)
        //{
        //    OpenWindow(this.Page, urlToOpen);
        //}       
    }
}