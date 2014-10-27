using System;
using System.Data;
using System.Web.UI.WebControls;

namespace QuickWeb.Factory
{
    public partial class WorkshopIN : System.Web.UI.Page
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
            }
            var btn = Request.Params["__EVENTTARGET"] as string;      
            
            if (btn != null && btn == "ctl00$ContentPlaceHolder1$txtBookingDate")
            {
                txtBarcode_TextChanged(txtBookingDate, EventArgs.Empty);              
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$txtDelDate")
            {
                txtBarcode_TextChanged(txtDelDate, EventArgs.Empty);
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
            grdNewChallan.DataSource = BAL.BALFactory.Instance.BAL_ChallanIn.BindFactoryInGrid(drpBranch.SelectedItem.Value, Globals.BranchID, txtBarcode.Text, txtBookingDate.Text, txtDelDate.Text, IsUrgent);
            grdNewChallan.DataBind();
            BindRightGrid();
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void BindRightGrid()
        {
            var dsNew = new DataSet();
            dsNew = BAL.BALFactory.Instance.BAL_ChallanIn.BindWorkshopRightGrid(Globals.BranchID, 20,Globals.UserName);

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

        protected void drpBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView();
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

        protected void chkUrgent_CheckedChanged(object sender, EventArgs e)
        {
            BindGridView();
        }

        public void ShowMoveAllButton()
        {
            if (PrjClass.getAccessRightMoveAllButtonsForFactory("Receive from Store Move All", Globals.BranchID, Globals.WorkshopUserType) == true)
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