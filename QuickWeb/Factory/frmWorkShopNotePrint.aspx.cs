using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace QuickWeb.Factory
{
    public partial class frmWorkShopNotePrint : System.Web.UI.Page
    {
        private ArrayList date = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtReportFrom.Text = Convert.ToDateTime(date[0].ToString()).ToString("dd MMM yyyy");
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2009; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
                BindShopName();
                BindGridView(txtReportFrom.Text, txtReportUpto.Text, drpShopName.SelectedItem.Value);
            }
        }

        protected void BindShopName()
        {
            drpShopName.Items.Clear();
            drpShopName.DataSource = DAL.DALFactory.Instance.Dal_WorkShopAllFunction.BindShopName();
            drpShopName.DataTextField = "BranchName";
            drpShopName.DataValueField = "BranchId";
            drpShopName.DataBind();
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            btnExport.Visible = false;
            string strSqlQuery = string.Empty;
            ViewState["exlquery"] = null;
            string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;
            if (radReportFrom.Checked)
            {
                if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
                DateTime dt = DateTime.Parse(txtReportUpto.Text);
                DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
                DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
                strFromDate = txtReportFrom.Text;
                strToDate = txtReportUpto.Text;
            }
            else if (radReportMonthly.Checked)
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                strFromDate = dt.ToString("dd MMM yyyy");
                strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            BindGridView(strFromDate, strToDate, drpShopName.SelectedItem.Value);
            if (grdReport.Rows.Count > 0)
            {
                grdReport.Visible = true;
                ViewState["exlquery"] = strSqlQuery;
                btnExport.Visible = true;
            }
        }

        protected void drpShopName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // BindGridView(txtReportFrom.Text, txtReportUpto.Text, drpShopName.SelectedItem.Value);
            btnShowReport_Click(null, null);
        }

        protected void BindGridView(string Date, string UptoDate, string BID)
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            grdReport.DataSource = BAL.BALFactory.Instance.BL_WorkShopAllFunction.BindAllChallanBranchWise(drpShopName.SelectedItem.Value, Date, UptoDate);
            grdReport.DataBind();
            if (grdReport.Rows.Count > 0)
            {
                grdReport.Visible = true;
                btnExport.Visible = true;
            }
        }

        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }

        protected void btnShowChallan_Click(object sender, EventArgs e)
        {
            Button ddl = (Button)sender;
            GridViewRow gv = ((GridViewRow)ddl.NamingContainer);
            int row = gv.RowIndex;
            OpenNewWindow("frmWorkShopInChallanSummary.aspx?Date=" + grdReport.Rows[row].Cells[0].Text + "&Date1=" + grdReport.Rows[row].Cells[0].Text + "&Challan=" + grdReport.Rows[row].Cells[1].Text + "&ShopName=" + drpShopName.SelectedItem.Text + "&ShopId=" + drpShopName.SelectedItem.Value + "");
        }
    }
}