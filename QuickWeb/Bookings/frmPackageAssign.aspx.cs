using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;


namespace QuickWeb.Bookings
{
    public partial class frmPackageAssign : System.Web.UI.Page
    {
        private DTO.PackageMaster Ob = new DTO.PackageMaster();
        private static string _memberShipId = string.Empty;
        private static string _barCode = string.Empty;
        private static string _pkgType = string.Empty;
        private static string _pkgBenefitType = string.Empty;
        private static string _pkgBenefitValue = string.Empty;
        private static string _pkgtaxType = string.Empty;
        // string[] _elements;
        /// <summary>
        /// / Print Package Variables 
        /// </summary>
        private static string _CustomerName = string.Empty;
        private static string _CustomerAddress = string.Empty;
        private static string _memberShipNo = string.Empty;
        private static string _packageName = string.Empty;
        private static string _PackageCost = string.Empty;
        private static string _PackageSaleDate = string.Empty;
        private static string _PackageStartDate = string.Empty;
        private static string _PackageEndDate = string.Empty;
        private static string _discountrate = string.Empty;
        private static string _checkdiscount = string.Empty;
        private static string _storename = string.Empty, _Businessname = string.Empty;
        public static string strPrinterName = string.Empty;   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();
                ResetAllField();
                txtStartValue.Enabled = false;
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Now.AddYears(1).ToString("dd MMM yyyy");
                bindMobileNo();
                binddrpsms();
                binddrpdefaultsms();
                var ds = BAL.BALFactory.Instance.BL_Branch.ShowBranch(Int32.Parse(Globals.BranchID));
                _storename = ds.Tables[0].Rows[0]["BranchName"].ToString();
                _Businessname = ds.Tables[0].Rows[0]["BusinessName"].ToString();
                strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
            }
            var btn = Request.Params["__EVENTTARGET"] as string;

            if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSaveOnly")
            {
                btnSaveOnly_Click(null, EventArgs.Empty);
                truecheckbox();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSave")
            {
                btnSave_Click(null, EventArgs.Empty);
                truecheckbox();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnUpdate")
            {
                btnUpdate_Click(null, EventArgs.Empty);
                truecheckbox();
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnDelete")
            {
                btnDelete_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnMarkComplete")
            {
                btnMarkComplete_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnPrintSlip")
            {
                btnPrintSlip_Click(null, EventArgs.Empty);
            }
        }

        private void BindDropDown()
        {
            try
            {
                DataSet ds = new DataSet();
                Ob.BranchId = Globals.BranchID;
                drpPackageName.DataSource = BAL.BALFactory.Instance.BL_PackageMaster.BindPackageDropDown(Ob);
                drpPackageName.DataTextField = "PackageName";
                drpPackageName.DataValueField = "PackageId";
                drpPackageName.DataBind();
            }
            catch (Exception ex)
            {
                drpPackageName.Items.Add("--No Package Available--");
            }
        }

        public void ResetAllField()
        {
            Ob.BranchId = Globals.BranchID;
            DataSet DsData = new DataSet();
            DsData = BAL.BALFactory.Instance.BL_PackageMaster.ShowAllAssignPackage(Ob);
            grdEntry.DataSource = DsData.Tables[0];
            grdEntry.DataBind();
            grdPkgCompleted.DataSource = DsData.Tables[1];
            grdPkgCompleted.DataBind();

            txtCustomerSearch.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtMemberShip.Text = "";
            txtBarCode.Text = "";
            drpPackageName.SelectedIndex = 0;
            txtRecurrence.Text = "";
            drpPackageName_SelectedIndexChanged(null, null);
            btnSave.Visible = true;
            btnSaveOnly.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnMarkComplete.Visible = false;
            SetTextBox(true);
            txtEndDate.Enabled = true;
            drpPackageName.Enabled = true;
            drpPaymentType.SelectedIndex = 0;
            drpPaymentType.Enabled = true;
            txtPaymentDetails.Enabled = true;
            txtPaymentDetails.Text = "";
            txtMemberShip.Enabled = true;
            drpPackageName.Focus();
            btnPrintSlip.Visible = false;
            txtCustomerEmailId.Text = "";
            txtCustomerMobile.Text = "";
        }

        public DTO.PackageMaster IntialiazeValueInGlobal()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.AssignId = lblAssignId.Text;
            Ob.PackageId = drpPackageName.SelectedItem.Value;
            Ob.StartValue = float.Parse(txtStartValue.Text);
            Ob.CustomerCode = lblCustomerCode.Text;
            Ob.StartDate = txtStartDate.Text;
            Ob.EndDate = hdnEndDate.Value;
            _PackageSaleDate = txtStartDate.Text;
            _PackageStartDate = txtStartDate.Text;
            _PackageEndDate = hdnEndDate.Value;
            Ob.MembershipId = txtMemberShip.Text;
            Ob.BarCode = txtBarCode.Text;
            Ob.PrevMemberShipId = _memberShipId;
            Ob.PrevBarCode = _barCode;
            if (_pkgType.Contains("Value / Benefit") && _pkgBenefitType == "Amount")
            {
                var bv =0.0;
                if (_pkgtaxType == "Yes")
                {
                    bv = float.Parse(lblValue.Text) - float.Parse(lblTotalAmountValue.Text);
                }
                else
                {
                    bv = float.Parse(lblValue.Text) - float.Parse(lblCost.Text);                
                }               
                Ob.CurDiscount = (bv * 100 / float.Parse(lblValue.Text)).ToString();
            }
            else
                Ob.CurDiscount = lblValue.Text;
            Ob.Active = "True";
            Ob.PaymentTypes = drpPaymentType.SelectedItem.Text;
            Ob.Recurrence = string.IsNullOrEmpty(txtRecurrence.Text) ? "0" : txtRecurrence.Text;
            Ob.PaymentDetails = txtPaymentDetails.Text;
            Ob.PackageTotalCost = float.Parse(lblTotalAmountValue.Text);
            if (ChkEmail.Checked == true)
            {
                Ob.CustomerEmailID = txtCustomerEmailId.Text;
            }
            else
            {
                Ob.CustomerEmailID = hdntempEmailid.Value;
            }

            if (chkSMS.Checked == true)
            {
                Ob.CustomerMobile = txtCustomerMobile.Text;
            }
            else
            {
                Ob.CustomerMobile = hdntempNo.Value;
            }           
            return Ob;
        }

        protected void drpPackageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTotalQtyValue.Visible = false;
            lblTotalQtyText.Visible = false;
            lblTotalAmount.Visible = false;
            lblTotalAmountValue.Visible = false;
            lblTotalAmountValue.Text = "0";
            lblValue.Visible = true;
            lblValueName.Visible = true;
            double val = 0;
            DataSet ds = new DataSet();
            DataSet dsTaxDetail = new DataSet();
            dsTaxDetail = BAL.BALFactory.Instance.BAL_Shift.GetTaxDetails(Globals.BranchID);
            grdQtyDetails.DataSource = null;
            grdQtyDetails.DataBind();
            grdQtyDetails.Visible = false;
            pnlGrid.Visible = true;
            Ob.BranchId = Globals.BranchID;
            try
            {
                Ob.PackageId = drpPackageName.SelectedItem.Value;
            }
            catch (Exception ex)
            {
                drpPackageName.Items.Add("--No Package Available--");
                return;
            }
            ds = BAL.BALFactory.Instance.BL_PackageMaster.GetAllPackgaeDetail(Ob);
            _pkgType = ds.Tables[0].Rows[0]["PackageType"].ToString();
            _pkgBenefitType = ds.Tables[0].Rows[0]["BenefitType"].ToString();
            _pkgBenefitValue = ds.Tables[0].Rows[0]["BenefitValue"].ToString();
            _pkgtaxType = ds.Tables[0].Rows[0]["TaxType"].ToString();
            txtRecurrence.Text = ds.Tables[0].Rows[0]["Recurrence"].ToString();
            try
            {
                txtStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Now.AddDays((DateTime.Parse(ds.Tables[0].Rows[0]["EndDate"].ToString()) - DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString())).TotalDays).ToString("dd MMM yyyy");
                hdnDateDifference.Value = ds.Tables[0].Rows[0]["Differ"].ToString();
                hdnEndDate.Value = txtEndDate.Text;
            }
            catch (Exception ex)
            {
                txtStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                hdnEndDate.Value = txtEndDate.Text;
            }
            lblCost.Text = ds.Tables[0].Rows[0]["PackageCost"].ToString();
            if (_pkgType != "Discount")
            {
                if (_pkgtaxType == "Yes")
                {
                    var actualCost = float.Parse(ds.Tables[0].Rows[0]["PackageCost"].ToString());
                    var computedCost =
                        actualCost // the actual cost
                        +
                        actualCost * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate1"].ToString())) / 100 // first tax
                        +
                        actualCost * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate1"].ToString()) / 100) * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate2"].ToString())) / 100 // second tax
                        +
                        actualCost * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate1"].ToString()) / 100) * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate3"].ToString())) / 100; // third tax
                    lblTotalAmountValue.Text = Math.Round(computedCost, 2).ToString();
                }
                else
                {
                    lblTotalAmountValue.Text = ds.Tables[0].Rows[0]["PackageCost"].ToString();
                }
            }
            else
            {
                lblTotalAmountValue.Text = ds.Tables[0].Rows[0]["PackageCost"].ToString();
            }
            double PackageCost = 0, BenefitValue = 0, StartValue = 0;
            if (ds.Tables[0].Rows[0]["BenefitType"].ToString() == "Amount")
            {
                lblValue.Text = ds.Tables[0].Rows[0]["BenefitValue"].ToString();
                txtStartValue.Text = ds.Tables[0].Rows[0]["BenefitValue"].ToString();
                txtStartValue.Focus();
                txtStartValue.Attributes.Add("onfocus", "javascript:select();");
                txtStartValue.Enabled = true;
                lblValueName.Text = "Value";
                lblstar.Text = "";
                txtStartValue.Enabled = true;
            }
            else
            {
                lblValue.Text = ds.Tables[0].Rows[0]["BenefitValue"].ToString();
                PackageCost = Convert.ToDouble(ds.Tables[0].Rows[0]["PackageCost"].ToString());
                BenefitValue = Convert.ToDouble(ds.Tables[0].Rows[0]["BenefitValue"].ToString());
                StartValue = (PackageCost * 100) / (100 - BenefitValue);
                txtStartValue.Text = Convert.ToDouble(Math.Ceiling(StartValue)).ToString();
                txtStartValue.Enabled = false;
                lblValueName.Text = "Discount";
                lblstar.Visible = true;
                if (_pkgType == "Discount")
                    lblstar.Text = "(%)";
                else
                    lblstar.Text = "";
            }
            if (_pkgType == "Qty / Item" || _pkgType == "Flat Qty")
            {
                DataSet dsMain = new DataSet();
                DataTable dt = new DataTable();
                grdQtyDetails.Visible = true;
                pnlGrid.Visible = true;
                dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetPackageQtyDetail(drpPackageName.SelectedItem.Value, Globals.BranchID);
                grdQtyDetails.Columns[2].Visible = true;
                grdQtyDetails.DataSource = dsMain;
                grdQtyDetails.DataBind();
                divRecurrence.Visible = true;
                lblValue.Visible = false;
                lblstar.Visible = false;
                lblValueName.Visible = false;
                if (_pkgType == "Flat Qty")
                {
                    grdQtyDetails.Columns[2].Visible = false;
                    lblTotalQtyValue.Text = ds.Tables[0].Rows[0]["TotalQty"].ToString();
                    lblTotalQtyValue.Visible = true;
                    lblTotalQtyText.Visible = true;
                    lblTotalAmount.Visible = true;
                    lblTotalAmountValue.Visible = true;
                    // val = Convert.ToDouble(lblCost.Text);
                    // lblTotalAmountValue.Text = val.ToString();
                }
                else
                {
                    lblTotalQtyValue.Visible = false;
                    lblTotalQtyText.Visible = false;
                }
            }
            else
            {
                grdQtyDetails.DataSource = null;
                grdQtyDetails.DataBind();
                grdQtyDetails.Visible = false;
                pnlGrid.Visible = false;
                divRecurrence.Visible = false;
            }
            if (_pkgType == "Discount")
                txtStartValue.Text = "0";
            if (_pkgType == "Value / Benefit" && _pkgBenefitType == "Amount")
            {
                txtStartValue.Visible = true;
                lblStartValue.Visible = true;
            }
            else
            {
                txtStartValue.Visible = false;
                lblStartValue.Visible = false;
            }
            txtCustomerSearch.Focus();
        }

        private string[] customerName;

        protected void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                customerName = txtCustomerSearch.Text.Split('-');
                lblCustomerCode.Text = customerName[0].ToString().Trim();
                txtCustomerSearch.Text = customerName[1].ToString().Trim();
                Ob.BranchId = Globals.BranchID;
                Ob.CustomerCode = lblCustomerCode.Text;
                drpPackageName_SelectedIndexChanged(txtCustomerSearch, EventArgs.Empty);
                if (BAL.BALFactory.Instance.BL_PackageMaster.CheckOrginalCustomer(Ob) != true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Please select a valid customer.";
                    txtCustomerSearch.Text = "";
                    txtCustomerSearch.Focus();
                }
                FillMemberShipAndBarCode(Ob.CustomerCode, Ob.BranchId);
                txtCustomerMobile.Focus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Please select a valid customer.";
                txtCustomerSearch.Text = "";
                txtCustomerSearch.Focus();
            }
        }

        private void FillMemberShipAndBarCode(string customerCode, string branchId)
        {
            try
            {
                var sqlCommand = new SqlCommand
                {
                    CommandText = "sp_customermaster",
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Flag", 18);
                var data = PrjClass.GetData(sqlCommand);
                txtMemberShip.Text = data.Tables[0].Rows[0]["MembershipId"].ToString();
                txtBarCode.Text = data.Tables[0].Rows[0]["CustomerBarcode"].ToString();
                txtCustomerMobile.Text = data.Tables[0].Rows[0]["CustomerMobile"].ToString();
                txtCustomerEmailId.Text = data.Tables[0].Rows[0]["CustomerEmailId"].ToString();
                hdntempNo.Value = data.Tables[0].Rows[0]["CustomerMobile"].ToString();
                hdntempEmailid.Value = data.Tables[0].Rows[0]["CustomerEmailId"].ToString();
                _memberShipId = txtMemberShip.Text;
                _barCode = txtBarCode.Text;
                if (txtMemberShip.Text != "")
                    txtMemberShip.Enabled = false;
                else
                    txtMemberShip.Enabled = true;
            }
            catch (Exception ex)
            {
                // who cares, eh?
            }
        }

        private bool CheckDuplicateMemberShipId(string MID, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberShipId", MID);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 20);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status= true;
                else
                    status= false;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }

        private bool CheckDuplicateBarcode(string Barcode, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", Barcode);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 21);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status = true;
                else
                    status = false;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();
            strPrinterName = PrjClass.GetPrinterName(Globals.BranchID);
            string res = string.Empty;
            DataSet dsMain = new DataSet();
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignToCustomer(Ob) == true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Not more than one package can be assigned to a customer at any given point in time. A package has already been assigned to the customer.";
                txtCustomerSearch.Text = "";

                txtCustomerSearch.Focus();
                return;
            }
            if (txtMemberShip.Enabled == true)
            {
                if (CheckDuplicateMemberShipId(txtMemberShip.Text, Globals.BranchID) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Membership id is already in use. Please select a new id.";
                    return;
                }
            }
            if (txtBarCode.Text != _barCode)
            {
                if (CheckDuplicateBarcode(txtBarCode.Text, Globals.BranchID) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Barcode is already in use. Please select a new barcode value.";
                    return;
                }
            }
            if (_pkgType == "Qty / Item" || _pkgType == "Flat Qty")
                res = BAL.BALFactory.Instance.BL_PackageMaster.SaveAssignPackage(Ob, true);
            else
                res = BAL.BALFactory.Instance.BL_PackageMaster.SaveAssignPackage(Ob);
            if (res == "Record Saved")
            {
                if (drpPaymentType.SelectedItem.Text != "None")
                {
                    int AssignId = PrjClass.getNewIDAccordingBID("AssignPackage", "AssignId", Ob.BranchId);
                    if (AssignId != 1)
                    {
                        AssignId = AssignId - 1;
                    }
                    SaveAccountEntries(AssignId);
                }
                SetPrintToSlipVariables();
                dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(lblCustomerCode.Text, Globals.BranchID);
                _CustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
                _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
                _discountrate = dsMain.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
                if (_pkgType == "Discount")
                {
                    _checkdiscount = "true";
                    OpenNewWindow("../" + strPrinterName + "/PackageSlip.aspx?CN=" + _CustomerName + "&CA=" + _CustomerAddress + "&MN=" + _memberShipNo + "&PSD=" + _PackageSaleDate + "&PSTD=" + _PackageStartDate + "&PED=" + _PackageEndDate + "&PN=" + _packageName + "&PC=" + _PackageCost + "&DIS=" + _discountrate + "&CHDIS=" + _checkdiscount + "&PKGTYPE=" + _pkgType + "&PKGBVALUE=" + _pkgBenefitValue + "&DirectPrint=true&RedirectBack=true&closeWindow=true");
                }
                else
                {
                    _checkdiscount = "false";
                    OpenNewWindow("../" + strPrinterName + "/PackageSlip.aspx?CN=" + _CustomerName + "&CA=" + _CustomerAddress + "&MN=" + _memberShipNo + "&PSD=" + _PackageSaleDate + "&PSTD=" + _PackageStartDate + "&PED=" + _PackageEndDate + "&PN=" + _packageName + "&PC=" + _PackageCost + "&CHDIS=" + _checkdiscount + "&PKGTYPE=" + _pkgType + "&PKGBVALUE=" + _pkgBenefitValue + "&DirectPrint=true&RedirectBack=true&closeWindow=true");
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package activated successfully.";
                var BID = Globals.BranchID;
                if (chkSMS.Checked)
                {
                    Task t = Task.Factory.StartNew
                         (
                            () => { AppClass.GoAssignPackagekMsg(BID, txtCustomerMobile.Text, drpsmstemplate.SelectedItem.Value, _CustomerName, drpPackageName.SelectedItem.Text, Convert.ToString(Ob.PackageTotalCost), txtEndDate.Text); }
                         );                    
                }
                if (ChkEmail.Checked)
                {
                    string slipdetail = string.Empty, subject = string.Empty;
                    subject = "" + _Businessname + " - " + _storename + " | Your Package Details – Happy Saving";
                    slipdetail = AppClass.MakeLaserPackageSlip(_CustomerName, _CustomerAddress, _memberShipNo, _PackageSaleDate, _PackageStartDate, _PackageEndDate, _packageName, _PackageCost, _checkdiscount, Globals.BranchID, "Creation", _storename, _Businessname, _pkgType, _pkgBenefitValue);
                    sendMail(slipdetail, txtCustomerEmailId.Text, subject);
                }
                ResetAllField();
            }
            if (res != "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res.ToString();
            }
        }

        private void sendMail(string mailBody, string eMail,string subject)
        {
            bool SSL = false;
            SqlCommand cmd1 = new SqlCommand();
            DataSet ds1 = new DataSet();
            try
            {
                cmd1.CommandText = "sp_ReceiptConfigSetting";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd1.Parameters.AddWithValue("@Flag", 2);
                ds1 = AppClass.GetData(cmd1);

                string FEmail = eMail;
                SSL = Convert.ToBoolean(ds1.Tables[0].Rows[0]["SSL"].ToString());
                int port = Convert.ToInt32(ds1.Tables[0].Rows[0]["Port"].ToString());
                Task tuy = Task.Factory.StartNew
                         (
                            () => { AppClass.SendMail(FEmail, subject, mailBody, true, port, ds1.Tables[0].Rows[0]["HostName"].ToString(), ds1.Tables[0].Rows[0]["BranchEmail"].ToString(), ds1.Tables[0].Rows[0]["BranchPassword"].ToString(), SSL); }
                         );
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
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
                drpsmstemplateMarkcomplete.DataSource = ds.Tables[0];
                drpsmstemplateMarkcomplete.DataTextField = "template";
                drpsmstemplateMarkcomplete.DataValueField = "smsid";
                drpsmstemplateMarkcomplete.DataBind();
            }
        }
        private void binddrpdefaultsms()
        {
            DTO.sms Ob = new DTO.sms();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BAL_sms.fetchdropdelivery(Ob);
            PrjClass.SetItemInDropDown(drpsmstemplate, ds.Tables[5].Rows[0]["Template"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpsmstemplateMarkcomplete, ds.Tables[6].Rows[0]["Template"].ToString(), true, false);
        }
        protected void btnSaveOnly_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();

            string res = string.Empty;
            DataSet dsMain = new DataSet();
            dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(lblCustomerCode.Text, Globals.BranchID);
            _CustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
            _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
            _discountrate = dsMain.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignToCustomer(Ob) == true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "Not more than one package can be assigned to a customer at any given point in time. A package has already been assigned to the customer.";
                txtCustomerSearch.Text = "";

                txtCustomerSearch.Focus();
                return;
            }

            if (txtMemberShip.Enabled == true)
            {
                if (CheckDuplicateMemberShipId(txtMemberShip.Text, Globals.BranchID) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Membership id is already in use. Please select a new id.";
                    return;
                }
            }
            if (txtBarCode.Text != _barCode)
            {
                if (CheckDuplicateBarcode(txtBarCode.Text, Globals.BranchID) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Barcode is already in use. Please select a new barcode value.";
                    return;
                }
            }
            if (_pkgType == "Qty / Item" || _pkgType == "Flat Qty")
                res = BAL.BALFactory.Instance.BL_PackageMaster.SaveAssignPackage(Ob, true);
            else
                res = BAL.BALFactory.Instance.BL_PackageMaster.SaveAssignPackage(Ob);
            if (res == "Record Saved")
            {
                if (drpPaymentType.SelectedItem.Text != "None")
                {
                    int AssignId = PrjClass.getNewIDAccordingBID("AssignPackage", "AssignId", Ob.BranchId);
                    if (AssignId != 1)
                    {
                        AssignId = AssignId - 1;
                    }
                    SaveAccountEntries(AssignId);
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package activated successfully.";
                SetPrintToSlipVariables();
                var BID = Globals.BranchID;
                if (chkSMS.Checked)
                {
                    Task tt = Task.Factory.StartNew
                          (
                             () => { AppClass.GoAssignPackagekMsg(BID, txtCustomerMobile.Text, drpsmstemplate.SelectedItem.Value, _CustomerName, drpPackageName.SelectedItem.Text, Convert.ToString(Ob.PackageTotalCost), txtEndDate.Text); }
                          );     
                }
                if (ChkEmail.Checked)
                {
                    string slipdetail = string.Empty, subject = string.Empty;
                    subject = "" + _Businessname + " - " + _storename + " | Your Package Details – Happy Saving";
                    slipdetail = AppClass.MakeLaserPackageSlip(_CustomerName, _CustomerAddress, _memberShipNo, _PackageSaleDate, _PackageStartDate, _PackageEndDate, _packageName, _PackageCost, _checkdiscount, Globals.BranchID, "Creation", _storename,_Businessname,_pkgType,_pkgBenefitValue);
                    sendMail(slipdetail, txtCustomerEmailId.Text, subject);
                }
                ResetAllField();
            }            
            if (res != "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res.ToString();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();
            DataSet dsMain = new DataSet();
            dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(lblCustomerCode.Text, Globals.BranchID);
            _CustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
            _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
            _discountrate = dsMain.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
            if (lblDuplicateCustomer.Text != Ob.CustomerCode)
            {
                if (BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignToCustomer(Ob) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Not more than one package can be assigned to a customer at any given point in time. A package has already been assigned to the customer.";
                    txtCustomerSearch.Text = "";
                    txtCustomerSearch.Focus();
                    return;
                }
            }
            string res = string.Empty;
            if (txtBarCode.Text != _barCode)
            {
                if (CheckDuplicateBarcode(txtBarCode.Text, Globals.BranchID) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Barcode is already in use. Please select a new barcode value.";
                    return;
                }
            }

            if (_pkgType == "Qty / Item" || _pkgType == "Flat Qty")
                res = BAL.BALFactory.Instance.BL_PackageMaster.UpdateAssignPackage(Ob, true);
            else
                res = BAL.BALFactory.Instance.BL_PackageMaster.UpdateAssignPackage(Ob);

            if (res == "Record Saved")
            {
                if (drpPaymentType.SelectedItem.Text != "None")
                {
                    SaveAccountEntries(Convert.ToInt32(lblAssignId.Text));
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package updated successfully."; 
                SetPrintToSlipVariables();
                var BID = Globals.BranchID;
                if (chkSMS.Checked)
                {
                    Task ttt = Task.Factory.StartNew
                          (
                             () => { AppClass.GoAssignPackagekMsg(BID, txtCustomerMobile.Text, drpsmstemplate.SelectedItem.Value, _CustomerName, drpPackageName.SelectedItem.Text, Convert.ToString(Ob.PackageTotalCost), txtEndDate.Text); }
                          );     
                }
                if (ChkEmail.Checked)
                {
                    string slipdetail = string.Empty, subject = string.Empty;
                    subject = "" + _Businessname + " - " + _storename + " | Your Package Details – Happy Saving";
                    slipdetail = AppClass.MakeLaserPackageSlip(_CustomerName, _CustomerAddress, _memberShipNo, _PackageSaleDate, _PackageStartDate, _PackageEndDate, _packageName, _PackageCost, _checkdiscount, Globals.BranchID, "Creation", _storename, _Businessname, _pkgType, _pkgBenefitValue);
                    sendMail(slipdetail, txtCustomerEmailId.Text, subject);
                }
                ResetAllField();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
            }
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();
            if (BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignInBookingTable(Ob) == true)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = "This Package used.";
                return;
            }
            string res = BAL.BALFactory.Instance.BL_PackageMaster.DeleteAssignPackage(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package deleted successfully.";
                ResetAllField();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
            }
        }

        protected void btnMarkComplete_Click(object sender, EventArgs e)
        {
            Ob = IntialiazeValueInGlobal();            
            Ob.PackageComplete = "True";
            DataSet dsMain = new DataSet();
            dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(lblCustomerCode.Text, Globals.BranchID);
            _CustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
            _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
            _discountrate = dsMain.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
            if (lblDuplicateCustomer.Text != Ob.CustomerCode)
            {
                if (BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignToCustomer(Ob) == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                    lblErr.Text = "Not more than one package can be assigned to a customer at any given point in time. A package has already been assigned to the customer.";
                    txtCustomerSearch.Text = "";
                    txtCustomerSearch.Focus();
                    return;
                }
            }
            string res = BAL.BALFactory.Instance.BL_PackageMaster.UpdateMarkComplete(Ob);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Green', '#999999');", true);
                lblMsg.Text = "Package concluded. Please note, once concluded, this package will not be available for use for any future transactions.";
                SetPrintToSlipVariables();
                var BID = Globals.BranchID;
                if (dsMain.Tables[0].Rows[0]["CustomerMobile"].ToString() != "")
                {
                    Task trr = Task.Factory.StartNew
                          (
                             () => { AppClass.GoAssignPackagekMsg(BID, dsMain.Tables[0].Rows[0]["CustomerMobile"].ToString(), drpsmstemplateMarkcomplete.SelectedItem.Value, _CustomerName, _packageName, Convert.ToString(Ob.PackageTotalCost), _PackageEndDate); }
                          );     
                }
                if (dsMain.Tables[0].Rows[0]["CustomerEmailId"].ToString() != "")
                {
                    string slipdetail = string.Empty, subject = string.Empty;
                    subject = "" + _Businessname + " - " + _storename + " | Package Expiry Notification: " + _PackageEndDate + "";
                    slipdetail = AppClass.MakeLaserPackageSlip(_CustomerName, _CustomerAddress, _memberShipNo, _PackageSaleDate, _PackageStartDate, _PackageEndDate, _packageName, _PackageCost, _checkdiscount, Globals.BranchID, "Expire", _storename, _Businessname, _pkgType, _pkgBenefitValue);
                    sendMail(slipdetail, dsMain.Tables[0].Rows[0]["CustomerEmailId"].ToString(), subject);                    
                }
                ResetAllField();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "setDivMouseOver('Red', '#999999');", true);
                lblErr.Text = res;
            }
        }

        protected void grdEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _PaymentTypes = string.Empty;
            lblAssignId.Text = ((Label)grdEntry.SelectedRow.FindControl("lblAssignId")).Text;
            Ob.BranchId = Globals.BranchID;
            Ob.AssignId = lblAssignId.Text;
            btnPrintSlip.Visible = true;
            DataSet dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetAssignDetails(Ob);
            if (dsMain.Tables[0].Rows.Count != 0)
            {
                txtRecurrence.Text = dsMain.Tables[0].Rows[0]["Recurrence"].ToString();
                PrjClass.SetItemInDropDown(drpPackageName, dsMain.Tables[0].Rows[0]["PackageId"].ToString(), false, false);
                drpPackageName_SelectedIndexChanged(null, null);
                txtStartValue.Text = dsMain.Tables[0].Rows[0]["StartValue"].ToString();
                txtCustomerSearch.Text = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
                txtStartDate.Text = dsMain.Tables[0].Rows[0]["StartDate"].ToString();
                txtEndDate.Text = dsMain.Tables[0].Rows[0]["EndDate"].ToString();
                _PackageStartDate = dsMain.Tables[0].Rows[0]["StartDate"].ToString();
                _PackageEndDate = dsMain.Tables[0].Rows[0]["EndDate"].ToString();
                lblCustomerCode.Text = dsMain.Tables[0].Rows[0]["CustomerCode"].ToString();
                lblDuplicateCustomer.Text = dsMain.Tables[0].Rows[0]["CustomerCode"].ToString();
                _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
                PrjClass.SetItemInDropDown(drpPaymentType, dsMain.Tables[0].Rows[0]["PaymentTypes"].ToString(), true, false);
                txtPaymentDetails.Text = dsMain.Tables[0].Rows[0]["PaymentDetail"].ToString();
                var gId = Globals.BranchID;
                _PaymentTypes = dsMain.Tables[0].Rows[0]["PaymentTypes"].ToString();
                if (_PaymentTypes == "Credit Card/Debit Card" || _PaymentTypes == "Cheque/Bank")
                {
                    txtPaymentDetails.Attributes.Add("style", "display:block");
                    lblPackageDetails.Attributes.Add("style", "display:block");
                    divPaymentDetails.Attributes.Add("style", "display:block");
                }
                else
                {
                    txtPaymentDetails.Attributes.Add("style", "display:none");
                    lblPackageDetails.Attributes.Add("style", "display:none");
                    divPaymentDetails.Attributes.Add("style", "display:none");
                }
                FillMemberShipAndBarCode(dsMain.Tables[0].Rows[0]["CustomerCode"].ToString(), gId);
                /*Task.Factory.StartNew(() => FillMemberShipAndBarCode(dsMain.Tables[0].Rows[0]["CustomerCode"].ToString(), gId)).ContinueWith((task) =>
                {
                    if (task.Status == TaskStatus.RanToCompletion) { System.Windows.Forms.MessageBox.Show("fucked up"); };
                });*/
            }
            var bkStatus = BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignInBookingTable(Ob);
            if (bkStatus == true)
            {
                SetTextBox(false);
                txtEndDate.Attributes.Add("onfocus", "javascript:select();");
                txtEndDate.Focus();
                btnSave.Visible = false;
                btnSaveOnly.Visible = false;
                btnUpdate.Visible = true;
                btnDelete.Visible = false;
                btnMarkComplete.Visible = true;
                txtEndDate.Enabled = true;
                btnUpdate.Visible = false;              
                hdntmp.Value = "1";
            }
            else
            {
                SetTextBox(true);
                txtStartValue.Focus();
                txtStartValue.Attributes.Add("onfocus", "javascript:select();");
                btnSave.Visible = false;
                btnSaveOnly.Visible = false;
                btnUpdate.Visible = true;
                btnMarkComplete.Visible = true;
                btnDelete.Visible = true;
                txtEndDate.Enabled = true;
                if (chkSMS.Checked == true)
                {
                    txtCustomerMobile.Enabled = true; 
                }
                else
                {
                    txtCustomerMobile.Enabled = false;                
                }
                if (ChkEmail.Checked == true)
                {
                    txtCustomerEmailId.Enabled = true;
                }
                else
                {
                    txtCustomerEmailId.Enabled = false;
                }
            }
            if (grdEntry.SelectedRow.Cells[7].Text == "Completed")
            {
                btnSave.Visible = false;
                btnSaveOnly.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnMarkComplete.Visible = false;
                SetTextBox(false);
                txtEndDate.Enabled = false;
            }
            SetPrintToSlipVariables();
            if (BAL.BALFactory.Instance.BL_CustomerMaster.CheckCustomerPackageActive(lblCustomerCode.Text, Globals.BranchID) == true)
                txtMemberShip.Enabled = false;
            else
                txtMemberShip.Enabled = true;
            if (_pkgType == "Qty / Item" && bkStatus)
                txtEndDate.Enabled = false;
            if (_pkgType == "Flat Qty" && bkStatus)
                txtEndDate.Enabled = false;
            if (txtMemberShip.Text != "")
                txtMemberShip.Enabled = false;
            else
                txtMemberShip.Enabled = true;
        }

        private void SetTextBox(bool Status)
        {
            txtStartValue.Enabled = Status;
            txtCustomerSearch.Enabled = Status;
            txtStartDate.Enabled = Status;
            drpPackageName.Enabled = Status;
            //txtMemberShip.Enabled = Status;
            txtBarCode.Enabled = Status;
            drpPaymentType.Enabled = Status;
            txtPaymentDetails.Enabled = Status;
            txtRecurrence.Enabled = Status;
            txtCustomerMobile.Enabled = Status;
            txtCustomerEmailId.Enabled = Status;            
        }

        public string SaveAccountEntries(int AssignId)
        {
            string res = string.Empty;
            try
            {
                SqlCommand CMD_Priority = new SqlCommand();
                CMD_Priority.CommandText = "sp_NewBooking_SaveProc";
                CMD_Priority.CommandType = CommandType.StoredProcedure;
                if (txtRecurrence.Text == "0" || txtRecurrence.Text == "")
                {
                    CMD_Priority.Parameters.AddWithValue("@TotalCost", lblCost.Text);
                    CMD_Priority.Parameters.AddWithValue("@AdvanceAmt", lblCost.Text);
                }
                else
                {
                    CMD_Priority.Parameters.AddWithValue("@TotalCost", lblTotalAmountValue.Text);
                    CMD_Priority.Parameters.AddWithValue("@AdvanceAmt", lblTotalAmountValue.Text);
                }
                CMD_Priority.Parameters.AddWithValue("@CustomerCode", lblCustomerCode.Text);
                CMD_Priority.Parameters.AddWithValue("@AcceptByUser", Globals.UserName);
                CMD_Priority.Parameters.AddWithValue("@PackageName", drpPackageName.SelectedItem.Text);
                ArrayList date = new ArrayList();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
                CMD_Priority.Parameters.AddWithValue("@DateTime", date[0].ToString());
                CMD_Priority.Parameters.AddWithValue("@Time", date[1].ToString());
                CMD_Priority.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                CMD_Priority.Parameters.AddWithValue("@AssignId", AssignId);
                CMD_Priority.Parameters.AddWithValue("@PaymentMode", drpPaymentType.SelectedItem.Text);
                CMD_Priority.Parameters.AddWithValue("@BankDetails", txtPaymentDetails.Text);
                CMD_Priority.Parameters.AddWithValue("@MID", txtMemberShip.Text);
                CMD_Priority.Parameters.AddWithValue("@Flag", 11);
                res = PrjClass.ExecuteNonQuery(CMD_Priority);
            }
            catch (Exception ex)
            { res = ex.ToString(); }
            return res;
        }      
        protected void btnPrintSlip_Click(object sender, EventArgs e)
        {
            DataSet dsMain = new DataSet();         
            dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetCustomerAddress(lblCustomerCode.Text, Globals.BranchID);
            _CustomerName = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
            _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
            _discountrate = dsMain.Tables[0].Rows[0]["DefaultDiscountRate"].ToString();
            if (_pkgType == "Discount")
            {
                _checkdiscount = "true";
                OpenNewWindow("../" + strPrinterName + "/PackageSlip.aspx?CN=" + _CustomerName + "&CA=" + _CustomerAddress + "&MN=" + _memberShipNo + "&PSD=" + _PackageSaleDate + "&PSTD=" + _PackageStartDate + "&PED=" + _PackageEndDate + "&PN=" + _packageName + "&PC=" + _PackageCost + "&DIS=" + _discountrate + "&CHDIS=" + _checkdiscount + "&PKGTYPE=" + _pkgType + "&PKGBVALUE=" + _pkgBenefitValue + "&DirectPrint=true&RedirectBack=true&closeWindow=true");
            }
            else
            {
                _checkdiscount = "false";
                OpenNewWindow("../" + strPrinterName + "/PackageSlip.aspx?CN=" + _CustomerName + "&CA=" + _CustomerAddress + "&MN=" + _memberShipNo + "&PSD=" + _PackageSaleDate + "&PSTD=" + _PackageStartDate + "&PED=" + _PackageEndDate + "&PN=" + _packageName + "&PC=" + _PackageCost + "&CHDIS=" + _checkdiscount + "&PKGTYPE=" + _pkgType + "&PKGBVALUE=" + _pkgBenefitValue + "&DirectPrint=true&RedirectBack=true&closeWindow=true");
            }
            //OpenNewWindow("../Bookings/PackageSlip.aspx?CN=" + _CustomerName + "&CA=" + _CustomerAddress + "&MN=" + _memberShipNo + "&PSD=" + _PackageSaleDate + "&PSTD=" + _PackageStartDate + "&PED=" + _PackageEndDate + "&PN=" + _packageName + "&PC=" + _PackageCost + "&DirectPrint=true&RedirectBack=true&closeWindow=true");
        }
        private void SetPrintToSlipVariables()
        {
            _memberShipNo = txtMemberShip.Text;
            //_PackageSaleDate = txtStartDate.Text;
            //_PackageStartDate = txtStartDate.Text;
            //_PackageEndDate = txtEndDate.Text;
            _packageName = drpPackageName.SelectedItem.Text;
            if (_pkgType == "Value / Benefit")
            {
                _PackageCost = lblCost.Text;
            }
            else
            {
                _PackageCost = lblTotalAmountValue.Text;
            }           
        }
        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }

        public void bindMobileNo()
        {
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_Area.GetCustomerMobileno(Globals.BranchID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlRateList1.DataSource = ds.Tables[0];
                ddlRateList1.DataTextField = "CustomerMobile";
                ddlRateList1.DataValueField = "CustomerMobile";
                ddlRateList1.DataBind();
            }
        }
        public void  truecheckbox()
        {
            chkSMS.Checked = true;
            ChkEmail.Checked = true;
        
        }
        protected void grdPkgCompleted_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _PaymentTypes = string.Empty;
            lblAssignId.Text = ((Label)grdPkgCompleted.SelectedRow.FindControl("lblAssignId")).Text;
            Ob.BranchId = Globals.BranchID;
            Ob.AssignId = lblAssignId.Text;
            btnPrintSlip.Visible = true;
            DataSet dsMain = BAL.BALFactory.Instance.BL_PackageMaster.GetAssignDetails(Ob);
            if (dsMain.Tables[0].Rows.Count != 0)
            {
                txtRecurrence.Text = dsMain.Tables[0].Rows[0]["Recurrence"].ToString();
                PrjClass.SetItemInDropDown(drpPackageName, dsMain.Tables[0].Rows[0]["PackageId"].ToString(), false, false);
                drpPackageName_SelectedIndexChanged(null, null);
                txtStartValue.Text = dsMain.Tables[0].Rows[0]["StartValue"].ToString();
                txtCustomerSearch.Text = dsMain.Tables[0].Rows[0]["CustomerName"].ToString();
                txtStartDate.Text = dsMain.Tables[0].Rows[0]["StartDate"].ToString();
                txtEndDate.Text = dsMain.Tables[0].Rows[0]["EndDate"].ToString();
                _PackageStartDate = dsMain.Tables[0].Rows[0]["StartDate"].ToString();
                _PackageEndDate = dsMain.Tables[0].Rows[0]["EndDate"].ToString();
                lblCustomerCode.Text = dsMain.Tables[0].Rows[0]["CustomerCode"].ToString();
                lblDuplicateCustomer.Text = dsMain.Tables[0].Rows[0]["CustomerCode"].ToString();
                _CustomerAddress = dsMain.Tables[0].Rows[0]["CustomerAddress"].ToString();
                PrjClass.SetItemInDropDown(drpPaymentType, dsMain.Tables[0].Rows[0]["PaymentTypes"].ToString(), true, false);
                txtPaymentDetails.Text = dsMain.Tables[0].Rows[0]["PaymentDetail"].ToString();
                var gId = Globals.BranchID;
                _PaymentTypes = dsMain.Tables[0].Rows[0]["PaymentTypes"].ToString();
                if (_PaymentTypes == "Credit Card/Debit Card" || _PaymentTypes == "Cheque/Bank")
                {
                    txtPaymentDetails.Attributes.Add("style", "display:block");
                    lblPackageDetails.Attributes.Add("style", "display:block");
                    divPaymentDetails.Attributes.Add("style", "display:block");
                }
                else
                {
                    txtPaymentDetails.Attributes.Add("style", "display:none");
                    lblPackageDetails.Attributes.Add("style", "display:none");
                    divPaymentDetails.Attributes.Add("style", "display:none");
                }
                FillMemberShipAndBarCode(dsMain.Tables[0].Rows[0]["CustomerCode"].ToString(), gId);
                /*Task.Factory.StartNew(() => FillMemberShipAndBarCode(dsMain.Tables[0].Rows[0]["CustomerCode"].ToString(), gId)).ContinueWith((task) =>
                {
                    if (task.Status == TaskStatus.RanToCompletion) { System.Windows.Forms.MessageBox.Show("fucked up"); };
                });*/
            }
            var bkStatus = BAL.BALFactory.Instance.BL_PackageMaster.CheckPackageAssignInBookingTable(Ob);
            if (bkStatus == true)
            {
                SetTextBox(false);
                txtEndDate.Attributes.Add("onfocus", "javascript:select();");
                txtEndDate.Focus();
                btnSave.Visible = false;
                btnSaveOnly.Visible = false;
                btnUpdate.Visible = true;
                btnDelete.Visible = false;
                btnMarkComplete.Visible = true;
                txtEndDate.Enabled = true;
                btnUpdate.Visible = false;
                hdntmp.Value = "1";
            }
            else
            {
                SetTextBox(true);
                txtStartValue.Focus();
                txtStartValue.Attributes.Add("onfocus", "javascript:select();");
                btnSave.Visible = false;
                btnSaveOnly.Visible = false;
                btnUpdate.Visible = true;
                btnMarkComplete.Visible = true;
                btnDelete.Visible = true;
                txtEndDate.Enabled = true;
                if (chkSMS.Checked == true)
                {
                    txtCustomerMobile.Enabled = true;
                }
                else
                {
                    txtCustomerMobile.Enabled = false;
                }
                if (ChkEmail.Checked == true)
                {
                    txtCustomerEmailId.Enabled = true;
                }
                else
                {
                    txtCustomerEmailId.Enabled = false;
                }
            }
            if (grdPkgCompleted.SelectedRow.Cells[7].Text == "Completed")
            {
                btnSave.Visible = false;
                btnSaveOnly.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnMarkComplete.Visible = false;
                SetTextBox(false);
                txtEndDate.Enabled = false;
            }
            SetPrintToSlipVariables();
            if (BAL.BALFactory.Instance.BL_CustomerMaster.CheckCustomerPackageActive(lblCustomerCode.Text, Globals.BranchID) == true)
                txtMemberShip.Enabled = false;
            else
                txtMemberShip.Enabled = true;
            if (_pkgType == "Qty / Item" && bkStatus)
                txtEndDate.Enabled = false;
            if (_pkgType == "Flat Qty" && bkStatus)
                txtEndDate.Enabled = false;
            if (txtMemberShip.Text != "")
                txtMemberShip.Enabled = false;
            else
                txtMemberShip.Enabled = true;
        }
    }
}