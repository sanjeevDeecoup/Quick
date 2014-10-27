using System;
using System.Data;
using System.Web.UI.WebControls;

namespace QuickWeb.Factory
{
    public partial class WorkshopOut : System.Web.UI.Page
    {
        private DataSet dsGrdNewData = new DataSet();
        private string strPrevBooking = string.Empty;
        private string strCurBooking = string.Empty;
        private int iBkCount, iCounter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppClass.CheckUserRightOnPageForFactory(this.Page);
                ShowMoveAllButton();
                BindGridView();               
                hdnDynamicStores.Value = "";
                for (int j = 1; j <= 24; j++)
                {
                    drpPrintStartFrom.Items.Add(j.ToString());
                }
            }
            else
            {
                if (hdnDynamicStores.Value != string.Empty)
                {                    
                    ltDynamic.Text = hdnDynamicStores.Value;
                }
            }
            var btn = Request.Params["__EVENTTARGET"] as string;
            if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSave")
            {
                btnSave_Click(btnSave, EventArgs.Empty);
                hdnAddedHeader.Value = "false";
                hdnRemovedEmptyMessage.Value = "false";
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnRefresh")
            {
                Response.Redirect("WorkshopOut.aspx");
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnPrint")
            {
                PrintChallan();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSticker")
            {
                //btnSticker_Click(btnSticker, EventArgs.Empty);
                hdnAddedHeader.Value = "false";
                hdnRemovedEmptyMessage.Value = "false";
            }
            else if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$btnSaveAndPrintNew")
            {
                btnSaveAndPrintNew_Click(null, EventArgs.Empty);
                hdnAddedHeader.Value = "false";
                hdnRemovedEmptyMessage.Value = "false";
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$txtBookingDate")
            {
                txtBarcode_TextChanged(txtBookingDate, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$txtDelDate")
            {
                txtBarcode_TextChanged(txtDelDate, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$achrClearFilter")
            {
                showGrid();
            }
        }

        private void BindGridView()
        {
            bool IsUrgent = false;
            if (chkUrgent.Checked)
            {
                IsUrgent = true;
            }
            else
            {
                IsUrgent = false;
            }
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindFactoryOutGrid(drpBranch.SelectedItem.Value, Globals.BranchID, txtBarcode.Text, txtBookingDate.Text, txtDelDate.Text, IsUrgent);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void BindRightGrid()
        {
            var dsNew = new DataSet();
            dsNew = BAL.BALFactory.Instance.BAL_ChallanIn.BindWorkshopRightGrid(Globals.BranchID, 30, Globals.UserName);

            grdSelectedCloth.DataSource = dsNew;
            grdSelectedCloth.DataBind();
            if (dsNew.Tables[0].Rows.Count != 0)
            {
                hdnAddedHeader.Value = "true";
                hdnRemovedEmptyMessage.Value = "true";
            }
            else
            {
                hdnAddedHeader.Value = "false";
                hdnRemovedEmptyMessage.Value = "false";
            }
        }

        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            BindGridView();
        }

        protected void grdNewChallan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells[1].Attributes.Add("style", "display: none");
                # region dataRow
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (dsGrdNewData is DataSet)
                    {
                        if (dsGrdNewData.Tables[0].Rows.Count != 0)
                        {
                            strCurBooking = dsGrdNewData.Tables[0].Rows[iCounter]["BookingNumber"].ToString();
                            if (strPrevBooking == null || strPrevBooking == string.Empty || strPrevBooking == strCurBooking)
                            {
                                iBkCount++;
                            }
                            else
                            {
                                var strBkCount = hdnBookingCount.Value;
                                if (strBkCount == string.Empty)
                                {
                                    strBkCount = strPrevBooking + ":" + iBkCount;
                                }
                                else
                                {
                                    strBkCount = strBkCount + "_" + strPrevBooking + ":" + (++iBkCount);
                                }
                                hdnBookingCount.Value = strBkCount;
                                var _tmpBooking = hdnAllBookingNumber.Value;
                                if (_tmpBooking == string.Empty)
                                {
                                    _tmpBooking = strPrevBooking;
                                }
                                else
                                {
                                    _tmpBooking = _tmpBooking + "_" + strPrevBooking;
                                }
                                hdnAllBookingNumber.Value = _tmpBooking;
                                var _tmpBkCount = hdnAllBookingCount.Value;
                                if (_tmpBkCount == string.Empty)
                                {
                                    _tmpBkCount = iCounter.ToString();
                                }
                                else
                                {
                                    _tmpBkCount = _tmpBkCount + "_" + iCounter;
                                }
                                hdnAllBookingCount.Value = _tmpBkCount;
                                iBkCount = 0;
                            }
                            strPrevBooking = strCurBooking;
                            iCounter++;
                            ((Label)e.Row.Cells[1].FindControl("lblRowNumber")).Text = iCounter.ToString().Trim();
                        }
                    }
                }
                # endregion

                # region Footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    /* *** */
                    e.Row.Cells[2].Text = "Total";
                    e.Row.Cells[3].Text = iCounter.ToString();
                    e.Row.BackColor = System.Drawing.Color.GreenYellow;
                    e.Row.Attributes.Add("style", "display: none");
                }
                # endregion
            }
            catch (Exception ex)
            {
            }
        }

        protected void grdNewChallan_DataBinding(object sender, EventArgs e)
        {
            dsGrdNewData = grdNewChallan.DataSource as DataSet;
            iCounter = 0;
            iBkCount = 0;
            hdnBookingCount.Value = "";
            hdnAllBookingNumber.Value = "";
            hdnAllBookingCount.Value = "";
            strPrevBooking = "";
            strCurBooking = "";
        }

        protected void grdNewChallan_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (hdnBookingCount.Value == string.Empty)
                {
                    hdnBookingCount.Value = strPrevBooking + ":" + iBkCount;
                }
                else
                {
                    hdnBookingCount.Value = hdnBookingCount.Value + "_" + strPrevBooking + ":" + iBkCount;
                }
                /* ** */
                if (hdnAllBookingNumber.Value == string.Empty)
                {
                    hdnAllBookingNumber.Value = strPrevBooking;
                }
                else
                {
                    hdnAllBookingNumber.Value = hdnAllBookingNumber.Value + "_" + strPrevBooking;
                }
                /* ** */
                if (hdnAllBookingCount.Value == string.Empty)
                {
                    hdnAllBookingCount.Value = strPrevBooking;
                }
                else
                {
                    hdnAllBookingCount.Value = hdnAllBookingCount.Value + "_" + iCounter;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var rowData = hdnAllData.Value;
                GroupData(rowData);
                var res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveEntryWiseFactoryOutRowData(rowData, Globals.BranchID, Globals.UserId, txtWorkShopNote.Text);
                if (res == "Record Saved")
                {
                    lblStatusCloth.InnerText = "Checkout Successful";
                    hdnAllHtml.Value = "-1";
                    BindGridView();
                    txtBarcode.Focus();
                    txtWorkShopNote.Text = "";
                }
                else
                {
                    lblStatusCloth.InnerText = res;
                }
            }
            catch (Exception excp)
            {
                lblStatusCloth.InnerText = "Error : " + excp.ToString();
            }
            finally
            {
                BindGridView();
            }
        }

        protected void btnSticker_Click(object sender, EventArgs e)
        {
            try
            {
                string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();
                var rowData = hdnAllData.Value;
                var res = string.Empty;
                var button = sender as Button;
                if (button != null)
                {
                    if (button.ID == "btnSaveAndPrintNew")
                        res = "Record Saved";
                    else
                        res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveEntryWiseFactoryOutRowData(rowData, Globals.BranchID, Globals.UserId, txtWorkShopNote.Text);
                }
                else
                {
                    res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveEntryWiseFactoryOutRowData(rowData, Globals.BranchID, Globals.UserId, txtWorkShopNote.Text);
                }

                if (res == "Record Saved")
                {
                    var printFrom = int.Parse(hdnStartValue.Value);
                    var resNew = BAL.BALFactory.Instance.BAL_ChallanIn.SaveInStickerTableFromFactory(rowData, Globals.BranchID, printFrom);
                    BindGridView();
                    // hdnCloseMe.Value = "true";
                    OpenNewWindow("../Bookings/printlabel1.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Factory/WorkshopOut.aspx");
                }
                else
                {
                    lblStatusCloth.InnerText = res;
                }
            }
            catch (Exception excp)
            {
                lblStatusCloth.InnerText = "Error : " + excp.ToString();
            }
            finally
            {
                BindGridView();
                txtWorkShopNote.Text = "";
            }
        }

        protected void grdSelectedCloth_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells[1].Attributes.Add("style", "display: none");
                # region dataRow

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    iCounter++;
                    ((Label)e.Row.Cells[1].FindControl("lblRowNumber")).Text = iCounter.ToString().Trim();

                }

                # endregion

                # region Footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    /* *** */
                    /*
                    e.Row.Cells[2].Text = "Total";
                    e.Row.Cells[3].Text = iCounter.ToString();
                    e.Row.BackColor = System.Drawing.Color.GreenYellow;*/
                    e.Row.Attributes.Add("style", "display: none");
                }
                # endregion
            }
            catch (Exception ex)
            {
            }
        }

        public void OpenNewWindow(string url)
        {
            if (!ClientScript.IsStartupScriptRegistered(this.GetType(), "newWindow"))
                ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}', '_blank');</script>", url));
            else
                ClientScript.RegisterStartupScript(this.GetType(), "newWindow2", String.Format("<script>window.open('{0}', '_blank');</script>", url));
        }

        private void GroupData(string rowData)
        {
        }

        protected void drpBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView();
        }

        protected void btnSaveAndPrintNew_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (chkPrintChallan.Checked)
            {
                PrintChallan();
            }

            if (chkPrintSticker.Checked)
            {
                // save the challan only if not already saved, that would be the case if chkPrintChallan is not checked //
                if (!chkPrintChallan.Checked)
                    btnSave_Click(btnSaveAndPrintNew, EventArgs.Empty);

                btnSticker_Click(btnSaveAndPrintNew, EventArgs.Empty);
            }
        }

        protected void PrintChallan()
        {
            btnSave_Click(btnSave, EventArgs.Empty);
            hdnAddedHeader.Value = "false";
            hdnRemovedEmptyMessage.Value = "false";

            var allRowData = hdnAllData.Value.Split('_');
            var bkNum = allRowData[0].Split(':')[0];
            var bkItemSNo = allRowData[0].Split(':')[1];
            var challanNum = BAL.BALFactory.Instance.BAL_ChallanIn.GetChallanNumberFromBookingAndItemSNo(bkNum, bkItemSNo, Globals.BranchID, true);

            OpenNewWindow("frmWorkShopInChallanSummary.aspx?Challan=" + challanNum + "&ShopId=" + Globals.BranchID + "");
        }

        private void showGrid()
        {

            BindGridView();
            if (Request.Params["__EVENTTARGET"].ToString() == "ctl00$ContentPlaceHolder1$achrClearFilter")
            {
                var tt = Request.Params["__EVENTARGUMENT"] as string;
                if (tt != null)
                {
                    if (tt != string.Empty)
                    {
                        hdnAllHtml.Value = tt;
                    }
                }
            }
            txtBarcode.Focus();

        }
        protected void chkUrgent_CheckedChanged(object sender, EventArgs e)
        {
            BindGridView();
        }

        public void ShowMoveAllButton()
        {
            if (PrjClass.getAccessRightMoveAllButtonsForFactory("Send to Store Move All", Globals.BranchID, Globals.WorkshopUserType) == true)
            {
                btnMoveRightAll.Attributes.Add("style", "display:block");
                btnMoveLeftAll.Attributes.Add("style", "display:block");
                lblMoveAllLbl.Attributes.Add("style", "display:block");
            }
            else
            {
                btnMoveRightAll.Attributes.Add("style", "display:none");
                btnMoveLeftAll.Attributes.Add("style", "display:none");
                lblMoveAllLbl.Attributes.Add("style", "display:none");
            }
        }
    }
}