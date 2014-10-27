using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickWeb.Bookings
{
    public partial class frmSteamPressScreen : System.Web.UI.Page
    {
        private ArrayList date = new ArrayList();
        private DTO.Common Ob = new DTO.Common();
        private DataSet dsGrdNewData = new DataSet();
        private string strPrevBooking = string.Empty;
        private string strCurBooking = string.Empty;
        private int iBkCount, iCounter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserName"] == null)
                Response.Redirect("~/Login.aspx");
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnAskForBarCode.Value = BAL.BALFactory.Instance.BAL_ChallanIn.AskForBarCodeAndPIN(Globals.BranchID, Globals.UserId, Globals.UserType);
            if (PrjClass.getAccessRightMoveAllButtons("MarkDelivery", "mstReceiptConfig", Globals.BranchID) == true)
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
            for (int j = 1; j <= 24; j++)
            {
                drpPrintStartFrom.Items.Add(j.ToString());
            }
            if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$btnTemp")
            {
                btnTemp_Click(null, EventArgs.Empty);
            }
            if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$btnSaveAndPrintNew")
            {
                btnSaveAndPrintNew_Click(null, EventArgs.Empty);
            }
            if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$drpProcess")
            {
                drpProcess_SelectedIndexChanged1(null, EventArgs.Empty);
            }
            if (Request.Params["__EVENTTARGET"] as string == "btnSaveChallanReturn")
            {
                btnSaveChallanReturn_Click(btnSaveChallanReturn, EventArgs.Empty);
            }
            else if (Request.Params["__EVENTTARGET"] as string == "txtRemoverChallan")
            {
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                btnSaveRemoveChallan_Click(null, EventArgs.Empty);
            }
            else if (!IsPostBack)
            {
                DeafultBind();
            }
        }

        public void DeafultBind()
        {
            txtBarcode.Focus();
            Page.DataBind();
            BindGrid();
            BindDrop();
            binddrpsms();
            binddrpdefaultsms();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "SELECT RightToView FROM dbo.EntMenuRights WHERE (PageTitle = '" + SpecialAccessRightName.RemoveChallan+ "') AND (UserTypeId = '" + Session["UserType"].ToString() + "') AND (BranchId = '" + Globals.BranchID + "')";
                cmd.CommandType = CommandType.Text;
                sdr = PrjClass.ExecuteReader(cmd);
                string statue = string.Empty;
                if (sdr.Read())
                    statue = "" + sdr.GetValue(0);
                if (statue == "True")
                {
                    chkRemove.Attributes.Add("style", "display: inline");
                }
                else
                {
                    chkRemove.Attributes.Add("style", "display: none");
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
        }

        protected void grdNewChallan_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        private void UpdateChallan(object sender, EventArgs e)
        {
            string bookingnumber = string.Empty, itemname = string.Empty, itemsno = string.Empty;
            try
            {
                var rowData = hdnAllData.Value;
                var res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveEntryWiseChallanReturnRowData(rowData, Globals.BranchID, "Save", Globals.UserId);
                if (res == "Record Saved")
                {
                    /* save the pin */
                    SavePin(ref rowData);
                    /* SAVE AND PRINT */
                    /* Not checking the wrong condition, cause that won't print, so the label would just show */
                    var btn = sender as Button;
                    if (btn != null)
                    {
                        if (btn.ID == "btnPrint")
                        {
                            return;
                        }
                    }
                    lblMsg.Text = "Cloths Marked Ready";
                    hdnAllHtml.Value = "-1";
                    BindGrid();
                    txtBarcode.Focus();
                }
                else
                {
                    lblErr.Text = res;
                }
            }
            catch (Exception excp)
            {
                lblErr.Text = "Error : " + excp.ToString();
            }
            finally
            {
                DeafultBind();
            }
        }

        private void SavePin(ref string rowData)
        {
            /**** WARNING! REGEX AHEAD ****/

            /*** Could NOT use look behind!!! Proceed with caution **/
            /* Could have easily done it in one step, but that would have been really hard for someone new to read and understand */
            var barCodes = Regex.Replace(rowData, @"(?<=\r\s+)(\d+)(\r\s*:)(\d+)(.*)", "*$1-$3-" + Globals.BranchID + "*~");
            barCodes = Regex.Replace(barCodes, @"(\r\n\s+)", "");
            barCodes = Regex.Replace(barCodes, @"(\n\s+)", "");

            // remove the last trailing "~"
            if (barCodes.Length >= 2)
                barCodes = barCodes.Substring(0, barCodes.Length - 1);

            /* save the pin */
            var result = BAL.BALFactory.Instance.BAL_ChallanIn.SavePinInBarcode(hdnReadyByPin.Value, barCodes, Globals.BranchID);
        }

        public void BindDrop()
        {
            drpProcess.Items.Clear();

            drpProcess.DataSource = DAL.DALFactory.Instance.DAL_NewChallan.BindDropDown(Globals.BranchID);

            drpProcess.DataTextField = "ProcessCode";
            drpProcess.DataValueField = "ProcessCode";

            drpProcess.DataBind();
            drpProcess.Items.Insert(0, new ListItem("All"));
        }

        // change this to DAL BAL like that in newChallan
        private void BindGrid()
        {
            grdNewChallan.DataSource = null;
            grdNewChallan.DataBind();
            DataSet dsMain = new DataSet();
            try
            {
                var drpValue = string.Empty;
                if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
                {
                    drpValue = string.Empty;
                }
                else
                {
                    drpValue = drpProcess.SelectedItem.Value;
                }
                dsMain = BAL.BALFactory.Instance.BAL_ChallanIn.GetDataSteamPressScreen(Globals.BranchID, hdnSPBookingFrom.Value, txtHolidayDate.Text, drpValue);

                grdNewChallan.DataSource = dsMain;
                grdNewChallan.DataBind();
            }
            catch (Exception excp)
            {
            }
            finally
            {
            }
        }

        private string res = "1";
        private string[] Bookingno, Rowno;

        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Bookingno = txtBarcode.Text.Split('-');
                if (Bookingno.Length == 1)
                {
                    DataSet ds = new DataSet();
                    var drpValue = string.Empty;
                    if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
                    {
                        drpValue = string.Empty;
                    }
                    else
                    {
                        drpValue = drpProcess.SelectedItem.Value;
                    }
                    grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.GetDataSteamPressScreen(Globals.BranchID, Bookingno[0], txtHolidayDate.Text, drpValue);
                    grdNewChallan.DataBind();
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
                else
                {
                    txtBarcode.Focus();
                    return;
                }

                if (Request.Params["__EVENTTARGET"].ToString() == "ctl00$ContentPlaceHolder1$txtBarcode")
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
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmSteamPressScreen.aspx", false);
        }

        protected void btnSaveChallanReturn_Click(object sender, EventArgs e)
        {
            UpdateChallan(sender, e);
        }

        protected void btnSaveRemoveChallan_Click(object sender, EventArgs e)
        {
            if (grdNewChallan.Rows.Count == 0)
            {
                Session["ReturnMsg"] = "No Record Found";
                return;
            }
            if (hdnRmvReason.Value != "")
            {
                if (BAL.BALFactory.Instance.BAL_RemoveReason.CheckCorrectRemoveReason(Globals.BranchID, hdnRmvReason.Value) == true)
                {
                    # region changeForSave
                    string res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveRemoveChallan(hdnRmvReasonData.Value, hdnRmvReason.Value, Globals.UserName, Globals.BranchID, "35","3");
                    if (res == "Record Saved")
                    {
                        lblMsg.Text = "Cloth Returned.";
                        hdnRmvReason.Value = "";
                        DeafultBind();
                        chkRemove.Checked = false;
                        if (Request.Params["__EVENTTARGET"].ToString() == "txtRemoverChallan")
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
                    }
                    else
                    {
                        lblErr.Text = res.ToString();
                        if (Request.Params["__EVENTTARGET"].ToString() == "txtRemoverChallan")
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
                    }
                    # endregion
                }
                else
                {
                    Session["ReturnMsg"] = "Reason not available in pre defined cause list.";
                    txtRemoverChallan.Focus();
                }
            }
            else
            {
            }
        }

        protected void chkRemove_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            // code to print and save
        }

        protected void drpProcess_SelectedIndexChanged1(object sender, EventArgs e)
        {
            BindGrid();
            txtBarcode.Focus();
            if (Request.Params["__EVENTTARGET"].ToString() == "ctl00$ContentPlaceHolder1$drpProcess")
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
        }

        protected void btnTemp_Click(object sender, EventArgs e)
        {
            BindGrid();
            txtBarcode.Focus();
            if (Request.Params["__EVENTTARGET"].ToString() == "ctl00$ContentPlaceHolder1$btnTemp")
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

        protected void btnSaveAndPrintNew_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            var rowData = hdnAllData.Value;
            var res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveEntryWiseChallanReturnRowData(rowData, Globals.BranchID, "Save", Globals.UserId);
            if (res == "Record Saved")
            {
                // save the pin
                SavePin(ref rowData);

                var printFrom = Int32.Parse(hdnStartValue.Value);
                var resNew = BAL.BALFactory.Instance.BAL_ChallanIn.SaveInStickerTable(rowData, Globals.BranchID, printFrom);
                //hdnCloseMe.Value = "true";
                BindGrid();
                OpenNewWindow("../Bookings/printlabels.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Bookings/frmSteamPressScreen.aspx");
                BindGrid();
            }
        }

        public void OpenNewWindow(string url)
        {
            //hdnCloseMe.Value = "true";
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}', '_blank');</script>", url));
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
            PrjClass.SetItemInDropDown(drpsmstemplate, ds.Tables[1].Rows[0]["Template"].ToString(), true, false);
        }

        protected void btnSms_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_sms.ReadyClothScreenSms(Globals.BranchID);
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                AppClass.GoMsg(Globals.BranchID, ds.Tables[0].Rows[r]["BookingNumber"].ToString(), drpsmstemplate.SelectedValue);
                status = "done";
                SqlCommand cmd = new SqlCommand();
                string res = string.Empty;
                cmd.CommandText = "SP_smsconfig";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 16);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            if (status == "done")
            {
                lblMsg.Text = "Message sent successfully for all the orders with all cloths ready.";
            }
            else
            {
                lblErr.Text = "No booking found to send sms.";
            }
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
    }
}