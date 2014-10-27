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
    public partial class frmStockReconcilation : System.Web.UI.Page
    {
        ArrayList date = new ArrayList();       
        bool status;
        DataSet dsGrdNewData = new DataSet();
        string strPrevBooking = string.Empty;
        string strCurBooking = string.Empty;
        int iBkCount, iCounter;
        Label _lbl;
        public static bool blnRight = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                BindGridView();
                txtInvoiceNo.Focus();
                AppClass.CheckUserRightOnStockPage(this.Page);
                SetOnlyOneTab();
            }
            hdnPrintValue.Value = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);
            hdnBranchId.Value = Globals.BranchID;
            if (Request.Params["__EVENTTARGET"] as string == "drpStatus")
            {
                drpStatus_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }       
        private void BindGridView()
        {                             
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.Bal_Report.BindStockReconcile(drpStatus.SelectedItem.Value, Globals.BranchID);
            grdAllClothes.DataSource = ds;
            grdAllClothes.DataBind();
            grdMatchClothes.DataSource = BAL.BALFactory.Instance.Bal_Report.BindStockMatchReconcile(Globals.BranchID);
            grdMatchClothes.DataBind();
            grdMatchNotFound.DataSource = BAL.BALFactory.Instance.Bal_Report.BindStockMatchNotReconcile(Globals.BranchID);
            grdMatchNotFound.DataBind();
            blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnAllClothesExport.Visible = true;
                btnMatchClothesExport.Visible = true;
                btnNotMatchClothesExport.Visible = true;
            }
            else
            {
                btnAllClothesExport.Visible = false;
                btnMatchClothesExport.Visible = false;
                btnNotMatchClothesExport.Visible = false;
            }

        }       

        protected void grdAllClothes_RowDataBound(object sender, GridViewRowEventArgs e)
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

                            _lbl = (Label)e.Row.Cells[1].FindControl("lblRowNumber");
                            if (_lbl != null)
                            {
                                _lbl.Text = iCounter.ToString().Trim();
                            }

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

        protected void grdMatchClothes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row != null)
                if (e.Row.Cells.Count > 1)
                {
                    e.Row.Cells[1].Attributes.Add("style", "display: none");
                }
        }

        protected void grdMatchNotFound_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row != null)
                if (e.Row.Cells.Count > 1)
                {
                    e.Row.Cells[1].Attributes.Add("style", "display: none");
                }
        }

        protected void grdAllClothes_DataBinding(object sender, EventArgs e)
        {
            dsGrdNewData = grdAllClothes.DataSource as DataSet;
            iCounter = 0;
            iBkCount = 0;
            hdnBookingCount.Value = "";
            hdnAllBookingNumber.Value = "";
            hdnAllBookingCount.Value = "";
            strPrevBooking = "";
            strCurBooking = "";

        }
        
        protected void grdAllClothes_DataBound(object sender, EventArgs e)
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
                grdAllCount.Text = grdAllClothes.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAllClothesExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;
           
            //StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdAllClothes);
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentWithoutDate(grdAllClothes, "Garments in Inventory / Garments physically not available", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        protected void btnMatchClothesExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;

            //StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(hdnHead.Value, hdnData.Value, hdnFoot.Value);
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentWithoutDate(grdMatchClothes, "Garments found in inventory and physically available", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        protected void btnNotMatchClothesExport_Click(object sender, EventArgs e)
        {
            string strFileName = "strReportFile.xls";
            Response.Expires = 0;
            Response.Buffer = true;

            //StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(hdnHead.Value, hdnData.Value, hdnFoot.Value);
            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentWithoutDate(grdMatchNotFound, "Garments not in inventory but physically available", false);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
           
        }

        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView();
            txtInvoiceNo.Focus();
            if (Request.Params["__EVENTTARGET"].ToString() == "drpStatus")
            {
                var tt = Request.Params["__EVENTARGUMENT"] as string;
                if (tt != null)
                {
                    var allHtms = tt.Split('#');
                    if (!string.IsNullOrEmpty(allHtms[0]))
                        hdnAllHtml.Value = allHtms[0];
                    if (allHtms.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(allHtms[1]))
                            hdnAllHtmlNot.Value = allHtms[1];
                    }
                }

            }
        }

        protected void DoneReconClick(object sender, EventArgs e)
        {
            BAL.BALFactory.Instance.Bal_Report.ResetAllGrid(Globals.BranchID);
            Response.Redirect("~/Reports/frmStockReconcilation.aspx");
        }

        protected void UpdateStausClick(object sender, EventArgs e)
        {
            BAL.BALFactory.Instance.Bal_Report.UpdateStatus(Globals.BranchID);
            hdnAllHtml.Value = "-1";
            hdnAllHtmlNot.Value = "-1";
            BindGridView();
            txtInvoiceNo.Focus();
        }

        protected void btnSendToMainGrid_Click(object sender, EventArgs e)
        {
            BAL.BALFactory.Instance.Bal_Report.ResetRightGrid(Globals.BranchID);
            hdnAllHtmlNot.Value = "-1";
            BindGridView();
            txtInvoiceNo.Focus();
        }

        protected void SetOnlyOneTab(bool bSetFalse = false)
        {
            var sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "UPDATE EntMenuRights SET RightToView = '" + bSetFalse.ToString() + "' WHERE PageTitle = '" + SpecialAccessRightName.StockRecon+ "' AND BranchId = " + Globals.BranchID;
            if (PrjClass.ExecuteNonQuery(sqlCommand) != "Record Saved")
                throw new Exception("Error in setting right to view on single tab");
        }
    }
}