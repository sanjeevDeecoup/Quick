using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmployeeMaster : System.Web.UI.Page
{
    private DTO.Common Common = new DTO.Common();
    private DTO.Employee Employee = new DTO.Employee();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Employee = SetValue();
            grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Employee.ShowAllCustomer(Employee);
            grdSearchResult.DataBind();
            txtCustomerName.Focus();
        }
    }

    public DTO.Employee SetValue()
    {
        Employee.BID = Globals.BranchID;
        Employee.Title = drpCustSalutation.SelectedItem.Text;
        Employee.EmpName = txtCustomerName.Text;
        Employee.Address = txtCustomerAddress.Text;
        Employee.PhoneNo = txtCustomerPhone.Text;
        Employee.Mobile = txtCustomerMobile.Text;
        Employee.EmailId = txtCustomerEmailId.Text;
        Employee.EmpCode = lblCustomerCode.Text;
        return Employee;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Employee = SetValue();
        if (BAL.BALFactory.Instance.BAL_Employee.RecordAllreadyExists(Employee))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = "Entered employee details already exists.";
            return;
        }
        Common.Result = BAL.BALFactory.Instance.BAL_Employee.SaveEmployee(Employee);
        if (Common.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Employee created sucessfully.";
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Common.Result;
        }
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Employee.ShowAllCustomer(Employee);
        grdSearchResult.DataBind();
    }       

    protected void FillRecordsFromGridView(int RowNum)
    {
        DataSet dsMain = new DataSet();
        Employee = SetValue();
        Employee.EmpCode = ((HiddenField)grdSearchResult.Rows[RowNum].Cells[0].FindControl("lnkbtnCode")).Value == "&nbsp;" ? "" : ((HiddenField)grdSearchResult.Rows[RowNum].Cells[0].FindControl("lnkbtnCode")).Value;
        try
        {
            dsMain = BAL.BALFactory.Instance.BAL_Employee.FillEmployee(Employee);
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                if (dsMain.Tables[0].Rows[0]["EmployeeSalutation"].ToString() != "")
                {
                    drpCustSalutation.SelectedValue = "" + dsMain.Tables[0].Rows[0]["EmployeeSalutation"].ToString();
                }
                txtCustomerName.Text = "" + dsMain.Tables[0].Rows[0]["EmployeeName"].ToString();
                txtCustomerAddress.Text = "" + dsMain.Tables[0].Rows[0]["EmployeeAddress"].ToString();
                txtCustomerPhone.Text = "" + dsMain.Tables[0].Rows[0]["EmployeePhone"].ToString();
                txtCustomerMobile.Text = "" + dsMain.Tables[0].Rows[0]["EmployeeMobile"].ToString();
                txtCustomerEmailId.Text = "" + dsMain.Tables[0].Rows[0]["EmployeeEmailId"].ToString();
                lblCustomerCode.Text = "" + dsMain.Tables[0].Rows[0]["EmployeeCode"].ToString();
            }
        }
        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = excp.Message;
        }
        finally
        {
        }
        btnDelete.Visible = true;
    }

    protected void grdSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSearchResult.PageIndex = e.NewPageIndex;
    }

    protected void grdSearchResult_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SelectCustomer")
        {
            int rownum = int.Parse(e.CommandArgument.ToString());
            if (rownum < grdSearchResult.Rows.Count)
            {
                FillRecordsFromGridView(rownum);
                btnUpdate.Visible = true;
                btnSave.Visible = false;
            }
        }
    }

    protected void RefreshForm()
    {
        txtCustomerName.Text = "";
        txtCustomerAddress.Text = "";
        txtCustomerPhone.Text = "";
        txtCustomerMobile.Text = "";
        txtCustomerEmailId.Text = "";
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        btnDelete.Visible = false;
        txtCustomerName.Focus();
    }

    protected void btnDeleteCustomer_Click(object sender, EventArgs e)
    {
        Employee = SetValue();
        Common.Result = BAL.BALFactory.Instance.BAL_Employee.DeleteEmployee(Employee);
        if (Common.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Employee deleted sucessfully.";
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = PrjClass.ReferenceMessage;
        }
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Employee.ShowAllCustomer(Employee);
        grdSearchResult.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Employee = SetValue();
        Common.Result = BAL.BALFactory.Instance.BAL_Employee.UpdateEmployee(Employee);
        if (Common.Result == "Record Saved")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
            lblMsg.Text = "Employee information updated sucessfully.";
            RefreshForm();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
            lblErr.Text = Common.Result;
        }
        grdSearchResult.DataSource = BAL.BALFactory.Instance.BAL_Employee.ShowAllCustomer(Employee);
        grdSearchResult.DataBind();
    }
}