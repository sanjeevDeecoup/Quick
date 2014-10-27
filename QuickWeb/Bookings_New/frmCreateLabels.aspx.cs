using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace QuickWeb.Bookings_New
{
    public partial class frmCreateLabels : System.Web.UI.Page
    {
        private ArrayList strCurrentDate = new ArrayList();
        private ArrayList date = new ArrayList();
        private DTO.BacodeLable BarcodeLable = new DTO.BacodeLable();
        private bool status;
        private DataSet dsGrdNewData = new DataSet();
        private string strPrevBooking = string.Empty;
        private string strCurBooking = string.Empty;
        private int iBkCount, iCounter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserBranch"] == null || Session["UserType"] == null || Session["UserName"] == null)
                Response.Redirect("~/Login.aspx");
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            if (!IsPostBack)
            {
                DeafultBind();
                BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();
                for (int j = 1; j <= 24; j++)
                {
                    drpprintstart.Items.Add(j.ToString());
                }
                BindDropDown();
            }
        }

        public void DeafultBind()
        {
            BindGridView();
            Page.DataBind();
            txtBarcode.Focus();
        }

        private void BindGridView()
        {
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_Sticker.GetDataStickerScreen(txtBarcode.Text, Globals.BranchID, txtHolidayDate.Text, "");
            grdNewChallan.DataSource = ds;
            grdNewChallan.DataBind();
            BindRightGridView();
        }

        private string res = string.Empty;
        private string[] Bookingno;

        private DataSet ds = new DataSet();

        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Bookingno = txtBarcode.Text.Split('-');
                if (Bookingno.Length == 1)
                {
                    DataSet ds = new DataSet();
                    ds = BAL.BALFactory.Instance.BAL_Sticker.GetDataStickerScreen(Bookingno[0].ToString(), Globals.BranchID, txtHolidayDate.Text, "");
                    grdNewChallan.DataSource = ds;
                    grdNewChallan.DataBind();
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
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            catch (Exception ex)
            { }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();
            var rowData = hdnAllData.Value;
            var printFrom = Int32.Parse(hdnStartValue.Value);
            var resNew = BAL.BALFactory.Instance.BAL_ChallanIn.SaveInStickerTable(rowData, Globals.BranchID, printFrom);
            DeafultBind();
            drpprintstart.SelectedIndex = 0;
            //hdnCloseMe.Value = "true";
            OpenNewWindow("../Bookings/printlabels.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Bookings/CreateLabels.aspx");
        }

        public void OpenNewWindow(string url)
        {
            //hdnCloseMe.Value = "true";
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}', '_blank');</script>", url));
        }

        protected void btrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCreateLabels.aspx");
        }

        protected void btnTemp_Click(object sender, EventArgs e)
        {
            BindGridView();
            txtBarcode.Text = "";
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
                            strCurBooking = dsGrdNewData.Tables[0].Rows[iCounter]["BookingNo"].ToString();
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            grdNewChallan.DataSource = null;
            grdNewChallan.DataBind();
            DataSet ds = new DataSet();

            string strBookingFrom = string.Empty;
            string strBookingTo = string.Empty;
            strBookingFrom = drpBookingPreFix.SelectedItem.Text.Trim() + txtInvoiceFrom.Text;
            strBookingTo = drpBookingPreFix.SelectedItem.Text.Trim() + txtInvoiceUpto.Text;

            ds = BAL.BALFactory.Instance.BAL_Sticker.GetDataStickerScreen(strBookingFrom, Globals.BranchID, txtHolidayDate.Text, strBookingTo);
            grdNewChallan.DataSource = ds;
            grdNewChallan.DataBind();
        }
        private void BindRightGridView()
        {
            var dsNew = new DataSet();
            dsNew = BAL.BALFactory.Instance.BAL_ChallanIn.BindRightGridForSticker(Globals.BranchID);
            grdSelectedCloth.DataSource = dsNew;
            grdSelectedCloth.DataBind();
        }
        protected void grdSelectedCloth_RowDataBound(object sender, GridViewRowEventArgs e)
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
                            strCurBooking = dsGrdNewData.Tables[0].Rows[iCounter]["BookingNo"].ToString();
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

        private void BindDropDown()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 28);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                PrjClass.SetItemInDropDown(drpBookingPreFix, ds.Tables[0].Rows[0]["BookingPreFix"].ToString(), false, false);
            }
        }
    }
}