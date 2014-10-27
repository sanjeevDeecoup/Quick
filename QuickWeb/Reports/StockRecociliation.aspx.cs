using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace QuickWeb.Reports
{
    public partial class StockRecociliation : System.Web.UI.Page
    {
        ArrayList date = new ArrayList();
        DTO.Sub_Items Ob = new DTO.Sub_Items();
        DTO.Item ObItem = new DTO.Item();
        DTO.CustomerMaster ObCust = new DTO.CustomerMaster();
        protected void Page_Load(object sender, EventArgs e)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            if (!IsPostBack)
            {
                DrpItem();
                bindGrid();               
                txtScanner.Focus();
                txtScanner.Text = "";
            }
        }
        public void DrpItem()
        {
            Ob.BranchID = Globals.BranchID;
            drpItemNames.DataSource = BAL.BALFactory.Instance.Bal_Report.BindItem(Ob);
            drpItemNames.DataTextField = "SubItemName";
            drpItemNames.DataBind();
            drpItemNames.Items.Insert(0, new ListItem("All", "0"));
        }
        public void bindGrid()
        {
            ObItem.BranchId = Globals.BranchID;
            ObItem.ItemName = drpItemNames.SelectedItem.Text;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();           
                if (txtCName.Text=="")
                {
                    //ds = BAL.BALFactory.Instance.Bal_Report.BindStockReconcile(ObItem);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        grdReport.DataSource = ds;
                        grdReport.DataBind();
                        CalculateGridReport();
                        btnExport1.Visible = true;
                    }
                    else
                    {
                        lblMsg.Text = "No Records Found";
                        //grdReport.Visible = false;
                    }
                }
                else
                {
                    if (drpItemNames.SelectedItem.Text != "All")
                    {                    
                        cmd.CommandText = "sp_StockReconcile";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerCode", hdnCustId.Value);
                        cmd.Parameters.AddWithValue("@ItemName", drpItemNames.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                        cmd.Parameters.AddWithValue("@Flag", 7);                        
                    }
                    else
                    {                       
                        cmd.CommandText = "sp_StockReconcile";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerCode", hdnCustId.Value);
                        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                        cmd.Parameters.AddWithValue("@Flag", 8);                      
                    }
                    ds = PrjClass.GetData(cmd);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        grdReport.DataSource = ds;
                        grdReport.DataBind();
                        ViewState["SavedDS"] = ds;
                        btnExport1.Visible = true;
                        CalculateGridReport();
                    }
                    else
                    {
                        lblMsg.Text = "No Records Found";
                        
                    }
                   
                }            
        }
        string[] Bookingno, Rowno, array;
        protected void  txtScanner_TextChanged(object sender, EventArgs e)
        {          
            string res = "";
            Bookingno = txtScanner.Text.Split('-');
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", Bookingno[0]);
            cmd.Parameters.AddWithValue("@RowIndex", Bookingno[1]);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Cloths Found";
                bindGrid();
                txtScanner.Text = "";
                txtScanner.Focus();
                
            }
            else
            {
                lblMsg.Text = "No Cloth Found in Inventory But Found in System or Shop";                
                bindGrid();
                BindDetailGrid();
                txtScanner.Text = "";
                txtScanner.Focus();              
            }         
        }
        private void CalculateGridReport1()
        {
            try
            {
                int rc1 = grdDetails.Rows.Count;
                int cc1 = grdDetails.Columns.Count;
                float Paid = 0, St = 0, Ad = 0, Bal = 0, OrderCount1 = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0;
                for (int r1 = 0; r1 < rc1; r1++)
                {
                    OrderCount1++;

                }
                grdDetails.FooterRow.Cells[1].Text = OrderCount1.ToString();
            }
            catch (Exception ex)
            { }
        }
        private void CalculateGridReport()
        {
            try
            {
                int rc = grdReport.Rows.Count;
                int cc = grdReport.Columns.Count;
                float Paid = 0, St = 0, Ad = 0, Bal = 0, OrderCount = 0, TotalCostCount = 0, TotalPaid = 0, TotalDue = 0, BalanceAmount = 0;
                for (int r = 0; r < rc; r++)
                {
                    OrderCount++;
            
                }
                grdReport.FooterRow.Cells[1].Text = OrderCount.ToString();
                }
            catch (Exception ex)
            { }
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (!chkInvoice.Checked)
            {
                bindGrid();
                BindDetailGrid();
                btnExport.Visible = true;
                btnExport1.Visible = true;
                btnReconcile.Visible = true;
            }
            else
            {
                BindInvoice();
                BindDetailGrid();
                btnExport.Visible = true;
                btnExport1.Visible = true;
                btnReconcile.Visible = true;
            }
        }
        protected void chkInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInvoice.Checked)
            {
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Visible = true;
                txtInvoiceNo.Focus();
                txtCName.Text = "";
            }
            else
            {
                txtInvoiceNo.Visible = false;
            }
        }
        private void BindInvoice()
        {        
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", txtInvoiceNo.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grdReport.DataSource = ds;
                grdReport.DataBind();
                ViewState["SavedDS"] = ds;
                btnExport.Visible = true;
                CalculateGridReport();
            }
            else
            {
                lblMsg.Text = "No Records found";
               
                
            }            
        }      
        protected void txtCName_TextChanged(object sender, EventArgs e)
        {
            string[] CustName = txtCName.Text.Split('-');
            hdnCustId.Value = CustName[0].ToString();
            setCustvalue(CustName[0].ToString());
        }
        public void setCustvalue(string customerName)
        {
            DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
            if (DS_CustInfo.Tables[0].Rows.Count > 0)
                txtCName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
        }
        private void BindDetailGrid()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            if (lblMsg.Text != "No Cloth Found in Inventory But Found in System or Shop")
            {
                cmd.CommandText = "sp_StockReconcile";
                cmd.CommandType = CommandType.StoredProcedure;              
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 9);
                ds = PrjClass.GetData(cmd);
                if(ds.Tables[0].Rows.Count>0)
                {
                grdDetails.DataSource = ds;
                grdDetails.DataBind();
                btnExport1.Visible = true;
                CalculateGridReport1();
                }
            }
            else
            {
               
                cmd.CommandText = "sp_StockReconcile";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNumber", Bookingno[0]);
                cmd.Parameters.AddWithValue("@RowIndex", Bookingno[1]);
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 12);
                ds = PrjClass.GetData(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grdDetails.DataSource = ds;
                    grdDetails.DataBind();
                    ViewState["SavedDS"] = ds;
                    btnExport.Visible = true;
                    CalculateGridReport1();
                }
                else
                {
                    lblMsg.Text = "No Cloth found in System or Shop";
                }            
            }          
           
        }
        protected void btnExport_Click1(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdDetails);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);              
        }
        protected void btnReconcile_Click(object sender, EventArgs e)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 10);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                btnReconcile.Visible = false;
                Response.Redirect("~/Reports/StockRecociliation.aspx", false);
            }
        }
        protected void btnExport1_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
            GridView grd = new GridView();
            grd.DataSource = (DataSet)ViewState["SavedDS"];
            grd.DataBind();
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);        
        }
        
    }
}