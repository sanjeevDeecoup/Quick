using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

public partial class Accounts_Daybook : System.Web.UI.Page
{

    private ArrayList date = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();

            btnShowReport_Click(null, null);           
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        //btnPrintDBook.Visible = false;
        string strSqlQuery = string.Empty;
        ViewState["exlquery"] = null;
        string strFromDate = string.Empty, strToDate = string.Empty, strGridCap = string.Empty;

        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        hdnFromDate.Value = strFromDate;
        hdnToDate.Value = strToDate;
        strGridCap = "Cash book from " + strFromDate + " to " + strToDate;

        ShowCashDetails(strFromDate, strToDate);
        if (grdReport.Rows.Count > 0)
        {
            //CalculateGridReport();
            //grdReport.Caption = strGridCap;
           // btnExport.Visible = true;
            grdReport.Visible = true;
            ViewState["exlquery"] = strSqlQuery;
            bool blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
        }
        else
        {
            btnExport.Visible = false;
            grdReport.Caption = "";
        }       
    }

    private void ShowCashDetails(string strStartDate, string strToDate)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        grdReport.DataSource = BAL.BALFactory.Instance.Bal_Processmaster.ShowCashDetails(Globals.BranchID, strStartDate, strToDate);
        grdReport.DataBind();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        string strFileName = "Cash book from " + hdnFromDate.Value + " to " + hdnToDate.Value + ".xls";
        Response.Expires = 0;
        Response.Buffer = true;

        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(grdReport, hdnFromDate.Value, hdnToDate.Value, "Cash book Report", false);
        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);

        //string strRepTitle = grdReport.Caption;
        //string[] Resp = PrjClass.GenerateExcelReportFromGridView(grdReport, strRepTitle);
        //if (Resp[0] == "1")
        //    Response.Redirect(Resp[1]);
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
        //    lblMsg.Text = Resp[1];
        //    Resp = PrjClass.GenerateCSVReportFromGridView(grdReport, strRepTitle);
        //    if (Resp[0] == "1")
        //        Response.Redirect(Resp[1]);
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
        //        lblMsg.Text = " Could not provide Report at the time. Please try after some time." + Resp[1];
        //    }
        //}
    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        decimal OrderCount = 0, OpBal = 0, Received = 0, Payment = 0, ClBal = 0;
        try
        {
            for (int r = 0; r < rc; r++)
            {
                OrderCount++;
                if (grdReport.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
                {
                    OpBal += decimal.Parse("0" + grdReport.Rows[r].Cells[1].Text);
                    Received += decimal.Parse("0" + grdReport.Rows[r].Cells[2].Text);
                    Payment += decimal.Parse("0" + grdReport.Rows[r].Cells[3].Text);
                    ClBal += decimal.Parse("0" + grdReport.Rows[r].Cells[4].Text);
                }
            }
            grdReport.FooterRow.Cells[1].Text = OpBal.ToString();
            grdReport.FooterRow.Cells[2].Text = Received.ToString();
            grdReport.FooterRow.Cells[3].Text = Payment.ToString();
            grdReport.FooterRow.Cells[4].Text = ClBal.ToString();
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdReport_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}