using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Accounts
{
    public partial class IncomeEntry : System.Web.UI.Page
    {
        private ArrayList date = new ArrayList();
        private DTO.Common Ob = new DTO.Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divNewLedgerCreation.Visible = false;
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                txtEntryDate.Text = date[0].ToString();
                ClearBoxesAfterExpSave();
                txtReportFrom.Text = date[0].ToString();
                txtReportUpto.Text = date[0].ToString();
                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2010; i < 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2010;           
                checkrights();
                btnShowReport_Click(null, null);
            }
        }
        private void checkrights()
        {
            bool rights = AppClass.CheckButtonRights(SpecialAccessRightName.LedgerRight);
            if (rights == true)
            {
                btnAddNewLedger.Visible = true;
            }
            else
            {
                btnAddNewLedger.Visible = false;
            }
        }
        public void BindGridView()
        {
            Ob.BID = Globals.BranchID;
            SqlGridSource.SelectCommand = "SELECT TransId, convert(VARCHAR,TransDate,106) As Expense_Date, Narration As Particulars, SUBSTRING(Particulars,4,LEN(Particulars)) As AccountType, SUM(Debit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'By %') AND BranchId='" + Globals.BranchID + "' AND TransId<>'' And SUBSTRING(Particulars,4,LEN(Particulars)) In (Select LedgerName from LedgerGroup where GroupName='Indirect Income') GROUP BY LedgerName, Particulars,Narration,TransDate,TransId order BY TransId desc";
            SqlGridSource.DataBind();     
        }

        public void BindDropDown()
        {
            drpLedger.Items.Clear();
            drpLedger.Items.Add("All");
            SDTExpenseLedgers.SelectCommand = "SELECT aa.LedgerName FROM LedgerMaster aa inner join LedgerGroup bb on aa.LedgerName=bb.LedgerName WHERE aa.BranchId='" + Globals.BranchID + "' AND ((aa.LedgerName NOT LIKE '%' + 'CASH' + '%') AND (aa.LedgerName NOT LIKE '%' + 'CUST' + '%') AND (aa.LedgerName NOT LIKE '%' + 'Sales' + '%')) and bb.GroupName='Indirect Income' ";
            SDTExpenseLedgers.DataBind();
        }

        protected void btnAddNewLedger_Click(object sender, EventArgs e)
        {
            divExpenseLedgerSelection.Visible = false;
            trPaidAmount.Visible = false;
            trRemark.Visible = false;
            btnAddNewExpense.Visible = false;
            divNewLedgerCreation.Visible = true;
            txtNewLedgerName.Focus();         
            txtRemark.Text = "";
            txtPaidAmount.Text = "";
        }

        protected void btnSaveNewLedger_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            //for (int c = 0; c < cmbDrExpenses1.Items.Count; c++)
            //{
            //    if (cmbDrExpenses1.Items[c].Text.ToUpper() == txtNewLedgerName.Text.Trim().ToUpper())
            //    {
            //    }
            //}
            if (txtNewLedgerName.Text.ToUpper() == "CASH" || txtNewLedgerName.Text.ToUpper() == "SALES")
            {
                Session["ReturnMsg"] = "Enter new ledger this ledger is already exist.";
                txtNewLedgerName.Focus();
                return;
            }
            else
            {
                bool found = false;
                SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
                DataSet ds = new DataSet();
                SqlDataAdapter sadp = new SqlDataAdapter();
                try
                {
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlcon;
                    cmd.CommandText = "SELECT * FROM LedgerMaster WHERE LedgerName Like '" + txtNewLedgerName.Text.Trim() + "' AND BranchId='" + Globals.BranchID + "'";
                    sadp = new SqlDataAdapter(cmd);
                    sadp.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            found = true;
                        }
                    }
                    if (found)
                    {
                        throw new Exception("Entered ledger name already exists. Enter another name.");
                    }

                    try
                    {
                        float.Parse(txtNewLedgerOpenBal.Text.Trim());
                    }
                    catch
                    {
                        throw new Exception("Invalid opening balance");
                    }
                    cmd.CommandText = "INSERT INTO LedgerMaster (LedgerName, OpenningBalance, CurrentBalance,BranchId) Values(@LedgerName, @OpBal,@CurBal," + Globals.BranchID + ")";
                    cmd.Parameters.Add(new SqlParameter("@LedgerName", txtNewLedgerName.Text.Trim()));
                    cmd.Parameters.Add(new SqlParameter("@OpBal", txtNewLedgerOpenBal.Text.Trim()));
                    cmd.Parameters.Add(new SqlParameter("@CurBal", txtNewLedgerOpenBal.Text.Trim()));
                    cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID));

                    int addedrow = cmd.ExecuteNonQuery();
                    sql = "Insert into LedgerGroup(GroupName,LedgerName,BranchId) values ('Indirect Income','" + txtNewLedgerName.Text + "','" + Globals.BranchID + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    if (addedrow > 0)
                    {
                        lblMsg.Text = "Ledger Added.";
                        cmbDrExpenses1.DataBind();
                        cmbDrExpenses1.SelectedValue = txtNewLedgerName.Text.Trim();
                        cmbDrExpenses1.Text = cmbDrExpenses1.SelectedValue;
                        txtNewLedgerName.Text = "";
                        txtNewLedgerOpenBal.Text = "0";
                        divNewLedgerCreation.Visible = false;
                        divExpenseLedgerSelection.Visible = true;
                        trPaidAmount.Visible = true;
                        trRemark.Visible = true;
                        btnAddNewExpense.Visible = true;
                        cmbDrExpenses1.Focus();
                        BindDropDown();
                        //btnShowReport_Click(null, null);
                    }
                }
                catch (Exception excp)
                {
                    Session["ReturnMsg"] = excp.Message;
                }
                finally
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
           
        }

        protected void btnAddNewExpense_Click(object sender, EventArgs e)
        {
            string time = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
            SqlTransaction stx = null;
            float fltCashPreBalance = 0, fltExpLedgerPreBalance = 0, fltAmount = 0, fltCashPostBalance = 0, fltExpLedgerPostBalance = 0;
            string strEntryDate = txtEntryDate.Text.Trim();
            string drName = cmbDrExpenses1.SelectedValue;
            SqlDataReader sdr = null; SqlCommand cmd = new SqlCommand();
            string sql = string.Empty;
            try
            {
                try
                {
                    fltAmount = float.Parse(txtPaidAmount.Text.Trim());
                }
                catch
                {
                    throw new Exception("Invalid Received amount");
                }
                if (fltAmount == 0)
                {
                    throw new Exception("Please Enter Valid Received Amount.");
                }

                sqlcon.Open();
                stx = sqlcon.BeginTransaction();
               
                cmd.Connection = sqlcon;
                cmd.Transaction = stx;

                cmd.CommandText = "select CurrentBalance from LedgerMaster where LedgerName='CASH' And BranchId='" + Globals.BranchID + "'";
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    fltCashPreBalance = float.Parse(sdr.GetValue(0).ToString());
                }
                sdr.Close();

                cmd.CommandText = "select CurrentBalance from LedgerMaster where LedgerName='" + drName + "' And BranchId='" + Globals.BranchID + "'";
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    fltExpLedgerPreBalance = float.Parse(sdr.GetValue(0).ToString());
                }
                sdr.Close();

                fltExpLedgerPostBalance = fltExpLedgerPreBalance + fltAmount;
                fltCashPostBalance = fltCashPreBalance - fltAmount;

                var TransId = PrjClass.getNewIDAccordingBID("EntLedgerEntries", "TransId", Globals.BranchID);

                sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,TransId,AcceptByUser,PaymentTime) Values('" + strEntryDate + "','CASH','By " + drName + "','" + fltCashPreBalance + "','" + fltAmount + "','0','" + fltCashPostBalance + "','" + txtRemark.Text + "','" + Globals.BranchID + "','" + TransId + "','" + Globals.UserName + "','" + time + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,TransId,AcceptByUser,PaymentTime) Values('" + strEntryDate + "','" + drName + "','To CASH','" + fltExpLedgerPreBalance + "','0','" + fltAmount + "','" + fltExpLedgerPostBalance + "','" + txtRemark.Text + "','" + Globals.BranchID + "','" + TransId + "','" + Globals.UserName + "','" + time + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update LedgerMaster Set CurrentBalance='" + fltCashPostBalance + "' Where LedgerName='CASH' And BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update LedgerMaster Set CurrentBalance='" + fltExpLedgerPostBalance + "' Where LedgerName='" + drName + "' And BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                stx.Commit();
                lblMsg.Text = "Income Saved Successfully";
                ClearBoxesAfterExpSave();
                btnShowReport_Click(null, null);
            }
            catch (Exception excp)
            {
                if (stx != null) { stx.Rollback(); }
                Session["ReturnMsg"] = excp.Message;
            }
            finally
            {
                sqlcon.Close();
                sqlcon.Dispose();
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();              
            }
        }

        private decimal total = 0;

        protected void grdSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPrice = (Label)e.Row.FindControl("lblAmount");
                decimal price = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
                lblPrice.Text = price.ToString();
                total += price;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.BackColor = System.Drawing.Color.GreenYellow;
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = total.ToString();
            }           
        }

        private void ClearBoxesAfterExpSave()
        {
            if (cmbDrExpenses1.Items.Count > 0)
            {
                cmbDrExpenses1.SelectedIndex = 0;
            }
            txtPaidAmount.Text = "0";
            txtRemark.Text = "";
            //BindGridView();
            BindDropDown();
            txtPaidAmount.Focus();
            btnAddNewExpense.Visible = true;
            btnDelete.Visible = false;
            btnUpdate.Visible = false;         
            txtRename.Text = "";
            txtRename.Visible = false;
            btnSaveRename.Visible = false;
            btnRename.Visible = false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("IncomeEntry.aspx", false);
        }

        protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUpdateId.Text = grdSearchResult.SelectedRow.Cells[1].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[1].Text;
            txtEntryDate.Text = grdSearchResult.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[2].Text;
            txtRemark.Text = grdSearchResult.SelectedRow.Cells[3].Text == "&nbsp;" ? "" : grdSearchResult.SelectedRow.Cells[3].Text;
            txtPaidAmount.Text = ((Label)grdSearchResult.SelectedRow.FindControl("lblAmount")).Text;
            PrjClass.SetItemInDropDown(cmbDrExpenses1, ((Label)grdSearchResult.SelectedRow.FindControl("lblAccountType")).Text, true, false);
            txtEntryDate.Attributes.Add("onfocus", "javascript:select();");
            txtEntryDate.Focus();
            btnAddNewExpense.Visible = false;
            btnDelete.Visible = true;
            btnUpdate.Visible = true;
            btnRename.Visible = true;
            bool rights = AppClass.CheckButtonRights(SpecialAccessRightName.LedgerRight);
            bool rights1 = AppClass.CheckButtonRights(SpecialAccessRightName.EditAndDelExp);
            if (rights == true)
            {
                btnRename.Visible = true;
            }
            else
            {
                btnRename.Visible = false;
            }
            if (rights1 == true)
            {
                btnDelete.Visible = true;
                btnUpdate.Visible = true;
            }
            else
            {
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string time = BAL.BALFactory.Instance.BAL_RemoveReason.GetDateTime(Globals.BranchID);
            string res = string.Empty;
            Ob.BID = Globals.BranchID;
            Ob.Id = lblUpdateId.Text;
            res = BAL.BALFactory.Instance.BAL_City.DeleteExpenses(Ob);
            if (res != "Record Saved")
            {
                lblErr.Text = res.ToString();
                return;
            }
            SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
            SqlTransaction stx = null;
            float fltCashPreBalance = 0, fltExpLedgerPreBalance = 0, fltAmount = 0, fltCashPostBalance = 0, fltExpLedgerPostBalance = 0;
            string strEntryDate = txtEntryDate.Text.Trim();
            string drName = cmbDrExpenses1.SelectedValue;
            SqlDataReader sdr = null; SqlCommand cmd = new SqlCommand();
            string sql = string.Empty;
            try
            {
                try
                {
                    fltAmount = float.Parse(txtPaidAmount.Text.Trim());
                }
                catch
                {
                    throw new Exception("Invalid Received Amount");
                }
                if (fltAmount == 0)
                {
                    throw new Exception("Can not accept amount zero.");
                }

                sqlcon.Open();
                stx = sqlcon.BeginTransaction();
               
                cmd.Connection = sqlcon;
                cmd.Transaction = stx;

                cmd.CommandText = "select CurrentBalance from LedgerMaster where LedgerName='CASH' And BranchId='" + Globals.BranchID + "'";
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    fltCashPreBalance = float.Parse(sdr.GetValue(0).ToString());
                }
                sdr.Close();

                cmd.CommandText = "select CurrentBalance from LedgerMaster where LedgerName='" + drName + "' And BranchId='" + Globals.BranchID + "'";
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    fltExpLedgerPreBalance = float.Parse(sdr.GetValue(0).ToString());
                }
                sdr.Close();

                fltExpLedgerPostBalance = fltExpLedgerPreBalance + fltAmount;
                fltCashPostBalance = fltCashPreBalance - fltAmount;

                sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,TransId,AcceptByUser,PaymentTime) Values('" + strEntryDate + "','CASH','By " + drName + "','" + fltCashPreBalance + "','" + fltAmount + "','0','" + fltCashPostBalance + "','" + txtRemark.Text + "','" + Globals.BranchID + "','" + lblUpdateId.Text + "','" + Globals.UserName + "','" + time + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Insert into EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId,TransId,AcceptByUser,PaymentTime) Values('" + strEntryDate + "','" + drName + "','To CASH','" + fltExpLedgerPreBalance + "','0','" + fltAmount + "','" + fltExpLedgerPostBalance + "','" + txtRemark.Text + "','" + Globals.BranchID + "','" + lblUpdateId.Text + "','" + Globals.UserName + "','" + time + "')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update LedgerMaster Set CurrentBalance='" + fltCashPostBalance + "' Where LedgerName='CASH' And BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = "Update LedgerMaster Set CurrentBalance='" + fltExpLedgerPostBalance + "' Where LedgerName='" + drName + "' And BranchId='" + Globals.BranchID + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                stx.Commit();
                lblMsg.Text = "Receipt Voucher Updated Successfully";
                ClearBoxesAfterExpSave();
                btnShowReport_Click(null, null);
            }
            catch (Exception excp)
            {
                if (stx != null) { stx.Rollback(); }
                Session["ReturnMsg"] = excp.Message;
            }
            finally
            {
                sqlcon.Close();
                sqlcon.Dispose();
                sdr.Close();
                sdr.Dispose();               
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            Ob.BID = Globals.BranchID;
            Ob.Id = lblUpdateId.Text;
            res = BAL.BALFactory.Instance.BAL_City.DeleteExpenses(Ob);
            if (res == "Record Saved")
            {
                lblMsg.Text = "Receipt Voucher Deleted Successfully";
                ClearBoxesAfterExpSave();
                btnShowReport_Click(null, null);
            }
            else
            {
                lblErr.Text = res.ToString();
            }            
        }

        protected void grdSearchResult_OnSorted(object sender, EventArgs e)
        {
            grdSearchResult.Visible = true;
            BindGridView();
        }

        private string strFromDate = string.Empty, strToDate = string.Empty, strReportType = string.Empty;

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (radReportFrom.Checked)
            {
                if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
                strFromDate = txtReportFrom.Text;
                strToDate = txtReportUpto.Text;
            }
            else if (radReportMonthly.Checked)
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                strFromDate = dt.ToString("dd MMM yyyy");
                strToDate = dt.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            strReportType = drpLedger.SelectedItem.Text;
            ShowBookingDetails(strFromDate, strToDate, strReportType);
        }

        private void ShowBookingDetails(string strStartDate, string strToDate, string strReportType)
        {
            ////DataSet dsMain = new DataSet();
            //grdSearchResult.DataSource = BAL.BALFactory.Instance.Bal_Report.GetExpenseReport(strStartDate, strToDate, strReportType, Globals.BranchID);
            //grdSearchResult.DataBind();

            //// With All Ledger
            if (strReportType == "All")
            {
                SqlGridSource.SelectCommand = "SELECT TransId, convert(VARCHAR,TransDate,106) As Expense_Date, Narration As Particulars,SUBSTRING(Particulars,4,LEN(Particulars)) As AccountType,SUM(Debit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'By %') AND BranchId='" + Globals.BranchID + "' AND TransId<>'' And SUBSTRING(Particulars,4,LEN(Particulars)) In (Select LedgerName from LedgerGroup where GroupName='Indirect Income' AND BranchId='" + Globals.BranchID + "') AND EntLedgerEntries.TransDate BETWEEN '" + strStartDate + "' AND '" + strToDate + "' GROUP BY LedgerName, Particulars,Narration,TransDate,TransId order BY TransId desc";
                SqlGridSource.DataBind();
            }
            else
            // ledger wise
            {
                strReportType = "By " + strReportType;
                SqlGridSource.SelectCommand = "SELECT TransId, convert(VARCHAR,TransDate,106) As Expense_Date, Narration As Particulars,SUBSTRING(Particulars,4,LEN(Particulars)) As AccountType,SUM(Debit) As Amount FROM EntLedgerEntries WHERE (LedgerName = 'CASH') AND (Particulars Like 'By %') AND BranchId='" + Globals.BranchID + "' AND TransId<>'' And SUBSTRING(Particulars,4,LEN(Particulars)) In (Select LedgerName from LedgerGroup where GroupName='Indirect Income' AND BranchId='" + Globals.BranchID + "') AND Particulars='" + strReportType + "' AND EntLedgerEntries.TransDate BETWEEN '" + strStartDate + "' AND '" + strToDate + "' GROUP BY LedgerName, Particulars,Narration,TransDate,TransId order BY TransId desc";
                SqlGridSource.DataBind();
            }
            bool rights = AppClass.CheckExportToExcelRightOnPage();
            if (rights)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
        }

        protected void btnReportRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("IncomeEntry.aspx", false);
        }

        protected void btnRename_Click(object sender, EventArgs e)
        {
            txtRename.Visible = true;
            btnSaveRename.Visible = true;
            txtRename.Focus();
        }

        protected void btnSaveRename_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            if (txtRename.Text.Trim() == "")
            {
                txtRename.Focus();
            }
            else
            {
                Ob.BID = Globals.BranchID;
                Ob.ChangeName = txtRename.Text;
                Ob.LedgerName = cmbDrExpenses1.SelectedItem.Text;
                ds = BAL.BALFactory.Instance.BAL_City.UpdateIncomeLedgerName(Ob);
                if (ds.Tables[0].Rows[0]["DupText"].ToString() == "Duplicate Ledger")
                {
                    lblErr.Text = "Duplicate Ledger";
                }
                else
                {
                    lblMsg.Text = "Income Rename Successfully";
                    ClearBoxesAfterExpSave();
                }
                txtRename.Text = "";
                txtRename.Visible = false;
                btnSaveRename.Visible = false;
                btnRename.Visible = false;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;

            var strDataToSaveInFile = AppClass.GetExcelContentForGridBooking(grdSearchResult, false, new[] { 0 });

            string strFilePathToSave = string.Empty;
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            var StreamWriter1 = new System.IO.StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }
    }
}