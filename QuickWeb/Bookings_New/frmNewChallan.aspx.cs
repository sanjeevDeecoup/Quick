using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Booking_New
{
    public partial class frmNewChallan : System.Web.UI.Page
    {
        private ArrayList strCurrentDate = new ArrayList();
        private ArrayList date = new ArrayList();
        private DTO.Common Obj = new DTO.Common();
        private bool status;
        private DataSet dsGrdNewData = new DataSet();
        private string strPrevBooking = string.Empty;
        private string strCurBooking = string.Empty;
        private int iBkCount, iCounter;

        protected void Page_Load(object sender, EventArgs e)
        {
            # region before
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            if (Session["UserBranch"] == null || Session["UserType"] == null || Session["UserName"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$btnTemp")
            {
                btnTemp_Click(null, EventArgs.Empty);
            }            
            else if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$drpProcess")
            {
                drpProcess_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$txtCustomerName")
            {
                txtCustomerName_TextChanged(null, EventArgs.Empty);
            }
            else
            {
                if (AppClass.CheckButtonRights(SpecialAccessRightName.SendtoWSMoveAll) == true)
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
                if (!IsPostBack)
                {                 
                    Obj.BID = Globals.BranchID;
                    if (!IsPostBack)
                    {
                        status = DAL.DALFactory.Instance.DAL_NewChallan.CheckIsFactory(Globals.BranchID);
                        if (status == true)
                        {
                            drpMulti.Visible = true;
                            lblSent.Visible = true;
                        }
                        else
                        {
                            drpMulti.Visible = false;
                            lblSent.Visible = false;
                        }
                        DeafultBind();
                        SDTShifts.SelectCommand = "SELECT [ShiftID], [ShiftName] FROM [ShiftMaster] WHERE BranchId='" + Globals.BranchID + "'";
                        SDTShifts.DataBind();
                        for (int j = 1; j <= 24; j++)
                        {
                            drpPrintStartFrom.Items.Add(j.ToString());
                        }
                    }                   
                }
            }
            chkRemove.Checked = false;
            # endregion
        }

        public void BindDrop()
        {
            drpProcess.Items.Clear();

            drpProcess.DataSource = DAL.DALFactory.Instance.DAL_NewChallan.BindDropDown(Globals.BranchID);

            drpProcess.DataTextField = "ProcessCode";
            drpProcess.DataValueField = "ProcessCode";

            drpProcess.DataBind();
            drpProcess.Items.Insert(0, new ListItem("All - Services"));
        }

        public void BindMultiFactory()
        {
            drpMulti.Items.Clear();
            drpMulti.DataSource = DAL.DALFactory.Instance.DAL_NewChallan.BindMultiFactory();
            drpMulti.DataTextField = "BranchName";
            drpMulti.DataValueField = "BranchId";
            drpMulti.DataBind();
            if (drpMulti.Items.Count > 1)
            {
                drpMulti.Items.Insert(0, new ListItem("Select Workshop"));
            }
            if (drpMulti.Items.Count == 0)
            {
                drpMulti.Items.Insert(0, new ListItem("Select Workshop"));
            }
        }

        public void DeafultBind()
        {
            BindGridView();
            Page.DataBind();
            BindDrop();
            BindMultiFactory();
            txtBarcode.Focus();
        }

        private void BindGridView()
        {
            DataSet dsNew = new DataSet();
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindGridView(Globals.BranchID, drpProcess, txtHolidayDate, hdnCheckStatus, hdnInvoiceNo, hdnRowNo);
            grdNewChallan.DataBind();

            BindRightGrid();
        }

        private void BindRightGrid()
        {
            var dsNew = new DataSet();
            dsNew = BAL.BALFactory.Instance.BAL_ChallanIn.BindRightGrid(Globals.BranchID, 1, Globals.UserName);

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

        private string shiftvalue = string.Empty;

        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            txtCustomerName.Text = "";
            txtHolidayDate.Text = "";
            drpProcess.SelectedIndex = 0;

            hdnInvoiceNo.Value = txtBarcode.Text;
            hdnCheckStatus.Value = "1";
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindGridView(Globals.BranchID, drpProcess, txtHolidayDate, hdnCheckStatus, hdnInvoiceNo, hdnRowNo);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewChallan.aspx", false);
        }

        protected void drpProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCustomerName.Text = "";
            txtHolidayDate.Text = "";
            txtBarcode.Text = "";

            hdnCheckStatus.Value = "0";
            hdnInvoiceNo.Value = "";
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindGridView(Globals.BranchID, drpProcess, txtHolidayDate, hdnCheckStatus, hdnInvoiceNo, hdnRowNo);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Focus();
        }

        protected void btnPrintPrv_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/ChallanReport.aspx", false);
        }

        public void OpenNewWindow(string url)
        {
            //hdnCloseMe.Value = "true";
            if (!ClientScript.IsStartupScriptRegistered(this.GetType(), "newWindow"))
                ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}', '_blank');</script>", url));
            else
                ClientScript.RegisterStartupScript(this.GetType(), "newWindow2", String.Format("<script>window.open('{0}', '_blank');</script>", url));
        }

        //protected void btnSaveRemoveChallan_Click(object sender, EventArgs e)
        //{
        //    if (hdnRmvReason.Value != "")
        //    {
        //        if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckCorrectRemoveReason(Globals.BranchID, hdnRmvReason.Value) == true)
        //        {
        //            //string res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveRemoveChallan(grdNewChallan, hdnRmvReason.Value, Globals.UserName, Globals.BranchID);
        //            string res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveRemoveChallan(hdnRmvReasonData.Value, hdnRmvReason.Value, Globals.UserName, Globals.BranchID, "34");
        //            if (res == "Record Saved")
        //            {
        //                lblMsg.Text = "Cloth Returned Unprocessed.";
        //                hdnRmvReason.Value = "";
        //                DeafultBind();
        //                chkRemove.Checked = false;
        //                if (Request.Params["__EVENTTARGET"].ToString() == "txtRemoverChallan")
        //                {
        //                    var tt = Request.Params["__EVENTARGUMENT"] as string;
        //                    if (tt != null)
        //                    {
        //                        if (tt != string.Empty)
        //                        {
        //                            hdnAllHtml.Value = tt;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                lblErr.Text = res.ToString();
        //                if (Request.Params["__EVENTTARGET"].ToString() == "txtRemoverChallan")
        //                {
        //                    var tt = Request.Params["__EVENTARGUMENT"] as string;
        //                    if (tt != null)
        //                    {
        //                        if (tt != string.Empty)
        //                        {
        //                            hdnAllHtml.Value = tt;
        //                        }
        //                    }
        //                }
        //            }
        //            //                chkRemove_CheckedChanged(null, null);
        //        }
        //        else
        //        {
        //            Session["ReturnMsg"] = "Reason not available in pre defined cause list.";
        //            txtRemoverChallan.Focus();
        //        }
        //    }
        //    else
        //    {
        //    }
        //    SDTShifts.SelectCommand = "SELECT [ShiftID], [ShiftName] FROM [ShiftMaster] WHERE BranchId='" + Globals.BranchID + "'";
        //    SDTShifts.DataBind();
        //}

        protected void btnTemp_Click(object sender, EventArgs e)
        {
            txtCustomerName.Text = "";           
            drpProcess.SelectedIndex = 0;
            txtBarcode.Text = "";

            hdnCheckStatus.Value = "0";
            hdnInvoiceNo.Value = "";
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindGridView(Globals.BranchID, drpProcess, txtHolidayDate, hdnCheckStatus, hdnInvoiceNo, hdnRowNo);
            grdNewChallan.DataBind();
            BindRightGrid(); 
            txtBarcode.Focus();
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

        protected void grdSelectedCloth_DataBinding(object sender, EventArgs e)
        {
            iCounter = 0;
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

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            
            txtHolidayDate.Text = "";
            drpProcess.SelectedIndex = 0;
            txtBarcode.Text = "";

            if (txtCustomerName.Text != "")
            {
                string[] CustName = txtCustomerName.Text.Split('-');
                lblCustomerCode.Text = CustName[0].ToString().Trim();
                txtCustomerName.Text = CustName[1].ToString().Trim();
            }
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindCustomerWise(Globals.BranchID, lblCustomerCode.Text);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }       
   }
}