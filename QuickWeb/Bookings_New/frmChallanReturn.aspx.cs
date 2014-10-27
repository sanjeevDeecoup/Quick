using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


namespace QuickWeb.Bookings_New
{
    public partial class frmChallanReturn : System.Web.UI.Page
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
            if (AppClass.CheckButtonRights(SpecialAccessRightName.RecvFromWSMoveAll) == true)
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
            if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$drpProcess")
            {
                drpProcess_SelectedIndexChanged1(null, EventArgs.Empty);
            }                      
            else if (!IsPostBack)
            {
                var isFactory = DAL.DALFactory.Instance.DAL_NewChallan.CheckIsFactory(Globals.BranchID);
                if (isFactory)
                {
                    ddlChallanNumber.Visible = true;                    
                }
                else
                {
                    ddlChallanNumber.Visible = false;                    
                }
                DeafultBind();
                CheckSMSDelivery();
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
        private bool CheckEntry()
        {
            int totalSelected = 0;
            for (int r = 0; r < grdNewChallan.Rows.Count; r++)
            {
                if (((CheckBox)grdNewChallan.Rows[r].Cells[0].FindControl("chkSelect")).Checked) totalSelected++;
            }
            if (totalSelected > 0)
                return true;
            else
                return false;
        }
        protected void btnShowChallanForSelectedNumber_Click(object sender, EventArgs e)
        {
            hdnSPShiftVal.Value = "";
            if (hdnSPShiftVal.Value == "0") hdnSPShiftVal.Value = "";
            hdnSPBookingFrom.Value = txtBarcode.Text.Trim();
            hdnSPBookingUpto.Value = "";
            BindGrid();
        }
        public void BindDrop()
        {
            drpProcess.Items.Clear();

            drpProcess.DataSource = DAL.DALFactory.Instance.DAL_NewChallan.BindDropDown(Globals.BranchID);

            drpProcess.DataTextField = "ProcessCode";
            drpProcess.DataValueField = "ProcessCode";

            drpProcess.DataBind();
            drpProcess.Items.Insert(0, new ListItem("All - Services"));

            var dataSet = BAL.BALFactory.Instance.BAL_ChallanIn.GetAllActiveChallans(Globals.BranchID);
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                ddlChallanNumber.DataSource = null;
            else
            {
                ddlChallanNumber.DataSource = dataSet;
                ddlChallanNumber.DataTextField = "ChallanNumber";
                ddlChallanNumber.DataValueField = "ChallanNumber";
            }
            ddlChallanNumber.DataBind();
        }
        // change this to DAL BAL like that in newChallan
        private void BindGrid(string challanNumber = "")
        {
            grdNewChallan.DataSource = null;
            grdNewChallan.DataBind();

            var drpValue = string.Empty;
            if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
            {
                drpValue = string.Empty;
            }
            else
            {
                drpValue = drpProcess.SelectedItem.Value;
            }
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.GetDataChallanReturnScreenFirst(Globals.BranchID, hdnSPBookingFrom.Value, hdnSPBookingUpto.Value, txtHolidayDate.Text, hdnSPShiftVal.Value, drpValue, challanNumber);
            grdNewChallan.DataBind();
            BindRightGrid();
        }
        private void BindRightGrid()
        {
            var dsNew = new DataSet();
            dsNew = BAL.BALFactory.Instance.BAL_ChallanIn.BindRightGrid(Globals.BranchID, 2, Globals.UserName);

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
        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            drpProcess.SelectedIndex = 0;           
            txtHolidayDate.Text = "";
            var drpValue = string.Empty;
            if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
            {
                drpValue = string.Empty;
            }
            else
            {
                drpValue = drpProcess.SelectedItem.Value;
            }
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.GetDataChallanReturnScreenFirst(Globals.BranchID, txtBarcode.Text, txtBarcode.Text, txtHolidayDate.Text, hdnSPShiftVal.Value, drpValue);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }             
       
        public void OpenNewWindow(string url)
        {
            //hdnCloseMe.Value = "true";
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}', '_blank');</script>", url));
        }
        protected void drpProcess_SelectedIndexChanged1(object sender, EventArgs e)
        {
            txtHolidayDate.Text = "";
            txtBarcode.Text = "";
            var drpValue = string.Empty;
            if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
            {
                drpValue = string.Empty;
            }
            else
            {
                drpValue = drpProcess.SelectedItem.Value;
            }
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.GetDataChallanReturnScreenFirst(Globals.BranchID, txtBarcode.Text, txtBarcode.Text, txtHolidayDate.Text, hdnSPShiftVal.Value, drpValue);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        protected void btnTemp_Click(object sender, EventArgs e)
        {
            drpProcess.SelectedIndex = 0;            
            txtBarcode.Text = "";
            var drpValue = string.Empty;
            if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
            {
                drpValue = string.Empty;
            }
            else
            {
                drpValue = drpProcess.SelectedItem.Value;
            }
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.GetDataChallanReturnScreenFirst(Globals.BranchID, txtBarcode.Text, txtBarcode.Text, txtHolidayDate.Text, hdnSPShiftVal.Value, drpValue);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
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
        protected void DdlChallanNumberSelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Equals(ddlChallanNumber.SelectedItem.Text.Trim(), "All - WorkshopNote", StringComparison.Ordinal))
                BindGrid();
            else
                BindGrid(ddlChallanNumber.SelectedItem.Text);
        }
        public void CheckSMSDelivery()
        {
            if (BAL.BALFactory.Instance.BAL_sms.SetSMSCheckBoxOnScreen(Globals.BranchID, "18") == true)
            {
                chkSendsms.Checked = true;
            }
            else
            {
                chkSendsms.Checked = false;
            }
        }
    }
}