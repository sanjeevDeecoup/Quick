using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace QuickWeb.Factory
{
    public partial class PrintNote : System.Web.UI.Page
    {
        private ArrayList date = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPageForFactory(this.Page);
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtReportFrom.Text = Convert.ToDateTime(date[0].ToString()).ToString("dd MMM yyyy");
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2009; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                for (int j = 1; j <= 24; j++)
                {
                    drpPrintStartFrom.Items.Add(j.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2009;
                BindGridView(txtReportFrom.Text, txtReportUpto.Text, Globals.BranchID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
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
            BindGridView(strFromDate, strToDate, Globals.BranchID);
        }

        protected void BindGridView(string Date, string UptoDate, string BID)
        {
            grdReport.DataSource = null;
            btnallPrintSticker1.Visible = false;
            grdReport.DataBind();
            grdReport.DataSource = BAL.BALFactory.Instance.BL_WorkShopAllFunction.BindAllChallanBranchWise(Globals.BranchID, Date, UptoDate);
            grdReport.DataBind();
            if (grdReport.Rows.Count > 0)
            {
                grdReport.Visible = true;
                btnallPrintSticker1.Visible = true;
                btnallPrintChallan.Visible = true; 
            }
            else
            {
                grdReport.Visible = false;
                btnallPrintSticker1.Visible = false;
                btnallPrintChallan.Visible = false; 
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
            OpenNewWindow("frmWorkShopInChallanSummary.aspx?Challan=" + grdReport.Rows[row].Cells[2].Text + "&ShopId=" + Globals.BranchID + "");
        }


        protected void btnallPrintChallan_Click(object sender, EventArgs e)
        {
            string Chdetail = string.Empty;
            foreach (GridViewRow gvrow in grdReport.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkRow");
                if (chk.Checked)
                {
                    Chdetail += gvrow.Cells[2].Text + ",";
                }
            }
            string challanNum1 = string.Empty;
            int challanLength = Chdetail.Length;
            if (challanLength > 1)
            {
                challanNum1 = Chdetail.Substring(0, challanLength - 1);
            }
            OpenNewWindow("frmWorkShopInChallanSummary.aspx?Challan=" + challanNum1 + "&ShopId=" + Globals.BranchID + "");
        }
        protected void BtnPrintSticker(object sender, EventArgs e)
        {
            string ChNumber = string.Empty;
            foreach (GridViewRow gvrow in grdReport.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkRow");
                if (chk.Checked)
                {
                    ChNumber += gvrow.Cells[2].Text + ",";
                }
            }          
            int challanLength = ChNumber.Length;
            string challanNum1 = string.Empty;
            var printFrom = Int32.Parse(hdnStartValue.Value);
            if (challanLength > 1)
            {
                challanNum1 = ChNumber.Substring(0, challanLength - 1);
            }
            var challanNum = hdnChallanNum.Value;
            var chcount = hdncount.Value;
            if (chcount.ToString() != "1")
            {               
                BAL.BALFactory.Instance.BAL_ChallanIn.SaveInStickerTableDataFromWorkShop(Globals.BranchID, printFrom, challanNum);
                OpenNewWindow("../Bookings/printlabel1.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Factory/frmPackingSticker.aspx");
            }
            else
            {               
                BAL.BALFactory.Instance.BAL_ChallanIn.SaveInStickerTableDataFromWorkShop(Globals.BranchID, printFrom, challanNum1);
                OpenNewWindow("../Bookings/printlabel1.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Factory/frmPackingSticker.aspx");
            }
        }     
        
    }
}