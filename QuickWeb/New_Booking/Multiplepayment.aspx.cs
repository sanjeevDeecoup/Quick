using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;

public partial class New_Booking_Multiplepayment : System.Web.UI.Page
{   
    
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            hdnDateFromAndTo.Value = date[0].ToString() + " - " + date[0].ToString();
            btnShowReport_Click(null, null);
        }
    }   
    protected void txtCName_TextChanged(object sender, EventArgs e)
    {
        string[] CustName = txtCName.Text.Split('-');
        hdnCustId.Value = CustName[0].ToString();
        setCustvalue(CustName[0].ToString());
        btnShowReport_Click(null, null);
    }
    public void setCustvalue(string customerName)
    {
        DataSet DS_CustInfo = BAL.BALFactory.Instance.BAL_New_Bookings.FillCustomer(customerName, Globals.BranchID);
        if (DS_CustInfo.Tables[0].Rows.Count > 0)
            txtCName.Text = DS_CustInfo.Tables[0].Rows[0]["CustName"].ToString();
    }    
    string strFromDate = string.Empty, strToDate = string.Empty;
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        Grdpayment.DataSource = null;
        Grdpayment.DataBind();        
        var DateFromAndTo = hdnDateFromAndTo.Value.Split('-');
        strFromDate = DateFromAndTo[0].Trim();
        strToDate = DateFromAndTo[1].Trim();
        hdnFromDate.Value = strFromDate;
        hdnToDate.Value = strToDate;
        btnExportExcel.Visible = false;        
        DTO.Report Ob = new DTO.Report();       
        Ob.FromDate = strFromDate;
        Ob.UptoDate = strToDate;
        Ob.CustId = hdnCustId.Value.Trim();
        Ob.BranchId = Globals.BranchID;
        if (txtCName.Text.Trim() != "")
        {
            if (drpSelectOption.SelectedValue == "1")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultipleDeliveryByCustomer(Ob);
                btnDeliverClothes.Visible = true;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = false;
            }
            if (drpSelectOption.SelectedValue == "2")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultiplePaymentByCustomer(Ob);
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = true;
                btnDeliverAndPayment.Visible = false;
            }
            if (drpSelectOption.SelectedValue == "3")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultipleBothOutPutByCustomer(Ob);
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = true;
            }
            if (drpSelectOption.SelectedValue == "4")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultipleOutstandingGarmentByCustomer(Ob);
                btnDeliverClothes.Visible = true;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = false;
            }
            if (drpSelectOption.SelectedValue == "5")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetOutstandingPaymentByCustomer(Ob);
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = true;
                btnDeliverAndPayment.Visible = false;
            }
        }
        else
        {
            if (drpSelectOption.SelectedValue == "1")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultipleDeliveryByFromToUptoDate(Ob);
                btnDeliverClothes.Visible = true;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = false;

            }
            if (drpSelectOption.SelectedValue == "2")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultiplePaymentByFromToUptoDate(Ob);
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = true;
                btnDeliverAndPayment.Visible = false;
            }
            if (drpSelectOption.SelectedValue == "3")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetMultipleBothOutPutFromToUptoDate(Ob);
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = true;
            }
            if (drpSelectOption.SelectedValue == "4")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetOutstandinGarmentFromToUptoDate(Ob);
                btnDeliverClothes.Visible = true;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = false;

            }
            if (drpSelectOption.SelectedValue == "5")
            {
                ds = BAL.BALFactory.Instance.Bal_Report.GetPendingPaymentFromToUptoDate(Ob);
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = true;
                btnDeliverAndPayment.Visible = false;
            }
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            Grdpayment.DataSource = ds;
            Grdpayment.DataBind();
            txtCName.Text = "";
            txtSearchInoviceNo.Focus();
           // btnExportExcel.Visible = true;
            bool blnRight = AppClass.CheckExportToExcelRightOnPage();
            if (blnRight)
            {
                btnExportExcel.Visible = true;
            }
            else
            {
                btnExportExcel.Visible = false;
            }
            CalculateGridReport();
           // tblBarcode.Visible = true;
        }
        else
        {
            btnDeliverClothes.Visible = false;
            btnAcceptPayment.Visible = false;
            btnDeliverAndPayment.Visible = false;
           // tblBarcode.Visible = false;
        }
        ButtonTrueFalse(drpSelectOption.SelectedValue);
    }    

    private void ButtonTrueFalse(string dropvalue)
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            cmd.CommandText = "SELECT RightToView FROM dbo.EntMenuRights WHERE (PageTitle = '" + SpecialAccessRightName.MulPaymentAcpRight+ "') AND (UserTypeId = '" + Session["UserType"].ToString() + "') AND (BranchId = '" + Globals.BranchID + "') ";
            cmd.CommandType = CommandType.Text;
            sdr = PrjClass.ExecuteReader(cmd);
            string statue = string.Empty;
            if (sdr.Read())
                statue = "" + sdr.GetValue(0);
            if (statue == "True")
            {
                if (drpSelectOption.SelectedValue == "1")
                {                   
                    btnDeliverClothes.Visible = true;
                    btnAcceptPayment.Visible = false;
                    btnDeliverAndPayment.Visible = false;

                }
                if (drpSelectOption.SelectedValue == "2")
                {                   
                    btnDeliverClothes.Visible = false;
                    btnAcceptPayment.Visible = true;
                    btnDeliverAndPayment.Visible = false;
                }
                if (drpSelectOption.SelectedValue == "3")
                {                   
                    btnDeliverClothes.Visible = false;
                    btnAcceptPayment.Visible = false;
                    btnDeliverAndPayment.Visible = true;
                }
            }
            else
            {
                btnDeliverClothes.Visible = false;
                btnAcceptPayment.Visible = false;
                btnDeliverAndPayment.Visible = false;
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
   
    protected void btnCheckGridBox_Click(object sender, EventArgs e)
    {
        bool status = false;
        for (int r = 0; r < Grdpayment.Rows.Count; r++)
        {
            if (((HyperLink)Grdpayment.Rows[r].Cells[1].FindControl("hypBtnShowDetails")).Text == txtSearchInoviceNo.Text)
            {
                ((CheckBox)Grdpayment.Rows[r].Cells[0].FindControl("chkSelect")).Checked = true;
                txtSearchInoviceNo.Text = "";
                txtSearchInoviceNo.Focus();
                status = true;
                break;
            }
        }
        if (status != true)
        {           
             ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = txtSearchInoviceNo.Text + " Booking no is not exist.";
            txtSearchInoviceNo.Text = "";
            txtSearchInoviceNo.Focus();
        }
    }
    protected void btnDeliverClothes_Click(object sender, EventArgs e)
    {
        if (!CheckEntry())
        {          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Kindly select atleast one checkbox.";
            txtSearchInoviceNo.Focus();
            return;
        }
        string res = string.Empty;
        if (BAL.BALFactory.Instance.Bal_Report.CheckClothStatus(Grdpayment) != "Done")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = BAL.BALFactory.Instance.Bal_Report.CheckClothStatus(Grdpayment);
        }
        res = BAL.BALFactory.Instance.Bal_Report.SaveMultiplePendingCloth(Grdpayment, Globals.BranchID, Globals.UserName);
        if (res == "Record Saved")
        {          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Ready garments have been delivered.";
            btnShowReport_Click(null, null);
        }
        else
        {          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Ready garments have been delivered.";
        }
    }

    protected void btnAccountDetails_Click(object sender, EventArgs e)
    {
        if ((drpSelectOption.SelectedItem.Text == "Pending Payment") || (drpSelectOption.SelectedItem.Text=="Outstanding Payment Only"))
        {
            btnAcceptPayment_Click(null, null);
        }
        else
        {
            btnDeliverAndPayment_Click(null, null);
        }
 
    }

    protected void btnAcceptPayment_Click(object sender, EventArgs e)
    {
        if (!CheckEntry())
        {         
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Please select atleast one checkbox.";
            txtSearchInoviceNo.Focus();
            return;
        }
        string res = string.Empty;
        res = BAL.BALFactory.Instance.Bal_Report.SaveMultiplePendingPayment(Grdpayment, Globals.BranchID, Globals.UserName, drpPaymentType.SelectedItem.Text, txtPaymentDetails.Text);
        if (res == "Record Saved")
        {         
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Payment received.";
            txtPaymentDetails.Text = "";
            btnShowReport_Click(null, null);
        }
        else
        {       
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = res.ToString();
        }
    }
    protected void btnDeliverAndPayment_Click(object sender, EventArgs e)
    {
        if (!CheckEntry())
        {           
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Please select atleast one checkbox.";
            txtSearchInoviceNo.Focus();
            return;
        }
        string res = string.Empty;
        if (BAL.BALFactory.Instance.Bal_Report.CheckClothStatus(Grdpayment) != "Done")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = BAL.BALFactory.Instance.Bal_Report.CheckClothStatus(Grdpayment);
        }
        res = BAL.BALFactory.Instance.Bal_Report.SaveDeliveryAndPayment(Grdpayment, Globals.BranchID, Globals.UserName, drpPaymentType.SelectedItem.Text, txtPaymentDetails.Text);
        if (res == "Record Saved")
        {          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            txtPaymentDetails.Text = "";
            lblMsg.Text = "Either payment or ready garments have been delivered.";
            btnShowReport_Click(null, null);
        }
        else
        {          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = res.ToString();
        }
    }
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;       
       // StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(Grdpayment);
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGridBookingNew(Grdpayment, hdnFromDate.Value, hdnToDate.Value, "Multiple Delivery And Payment", false);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave, false);
    }
    protected void txtSearchInoviceNo_TextChanged(object sender, EventArgs e)
    {
        bool status = false;
        string[] Bookingno = txtSearchInoviceNo.Text.Split('-');
        for (int r = 0; r < Grdpayment.Rows.Count; r++)
        {
            if (((HyperLink)Grdpayment.Rows[r].Cells[1].FindControl("hypBtnShowDetails")).Text == Bookingno[0].ToString().ToUpper())
            {
                ((CheckBox)Grdpayment.Rows[r].Cells[0].FindControl("chkSelect")).Checked = true;
                txtSearchInoviceNo.Text = "";
                txtSearchInoviceNo.Focus();
                status = true;
                break;
            }
        }
        if (status != true)
        {          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Order # " + txtSearchInoviceNo.Text + " does not belong to the selected customer.";
            txtSearchInoviceNo.Text = "";
            txtSearchInoviceNo.Focus();
        }
    }
    private void CalculateGridReport()
    {
        try
        {
            int rc = Grdpayment.Rows.Count;
            int cc = Grdpayment.Columns.Count;
            float TotalQty = 0, ReadyQty = 0, DelQty = 0, BalQty = 0, TotalPayment = 0, PaymentReceived = 0, BalancePayment = 0;
            for (int r = 0; r < rc; r++)
            {
                TotalQty += float.Parse("0" + Grdpayment.Rows[r].Cells[5].Text);
                ReadyQty += float.Parse("0" + Grdpayment.Rows[r].Cells[6].Text);
                DelQty += float.Parse("0" + Grdpayment.Rows[r].Cells[7].Text);
                BalQty += float.Parse("0" + Grdpayment.Rows[r].Cells[8].Text);
                TotalPayment += float.Parse("0" + Grdpayment.Rows[r].Cells[9].Text);
                PaymentReceived += float.Parse("0" + Grdpayment.Rows[r].Cells[10].Text);
                BalancePayment += float.Parse("0" + Grdpayment.Rows[r].Cells[11].Text);
            }
            Grdpayment.FooterRow.Cells[1].Text = "Total";
            Grdpayment.FooterRow.Cells[5].Text = TotalQty.ToString();
            Grdpayment.FooterRow.Cells[6].Text = ReadyQty.ToString();
            Grdpayment.FooterRow.Cells[7].Text = DelQty.ToString();
            Grdpayment.FooterRow.Cells[8].Text = BalQty.ToString();
            Grdpayment.FooterRow.Cells[9].Text = TotalPayment.ToString();
            Grdpayment.FooterRow.Cells[10].Text = PaymentReceived.ToString();
            Grdpayment.FooterRow.Cells[11].Text = BalancePayment.ToString();
            Grdpayment.FooterRow.Cells[2].Text = Grdpayment.Rows.Count.ToString();
        }
        catch (Exception ex)
        { }
    }
    private bool CheckEntry()
    {
        int totalSelected = 0;
        for (int r = 0; r < Grdpayment.Rows.Count; r++)
        {
            if (((CheckBox)Grdpayment.Rows[r].Cells[0].FindControl("chkSelect")).Checked) totalSelected++;
        }
        if (totalSelected > 0)
            return true;
        else
            return false;
    }
}
