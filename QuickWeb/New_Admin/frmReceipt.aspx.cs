using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace QuickWeb.New_Admin
{
    public partial class frmReceipt : System.Web.UI.Page
    {
        private DTO.Report Ob = new DTO.Report();
        public string stylebutton = "~/Logo/DRY.jpg";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                BindDropDown1();
                abc.Src = "../Logo/DRY" + Globals.BranchID + ".jpg";
                //BAL.BALFactory.Instance.BAL_Barcodesetting.PrinterList(drpDefaultPrinter);
                FillExistingRecord();
                BindFont();
                rdrLogoAndTest_CheckedChanged(null, null);
                FillLastRecordOnControls("Yes");
                txtDefaultTime.Items.Clear();
                for (int i = 1; i < 13; i++)
                {
                    txtDefaultTime.Items.Add(i.ToString());
                }
                DataForDefaultSetting();
                System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> TimeZoneColl = TimeZoneInfo.GetSystemTimeZones();
                drpZoneList.DataSource = TimeZoneColl;
                drpZoneList.DataTextField = "DisplayName";
                drpZoneList.DataValueField = "Id";
                drpZoneList.DataBind();
                BindDropDown();
                string access = string.Empty;
                if (Request.QueryString["Config"] != null)
                {
                    access = Request.QueryString["Config"].ToString();
                    SetTabContainer(false);
                }

                if (access == "Display")
                {
                    setActive(tdDisplay);
                }
                if (access == "TaxRate")
                {
                    setActive(tdServiceTax);
                }
                if (access == "TimeZone")
                {
                    setActive(tdSetTimeZone);
                }
                if (access == "Receipt")
                {
                    setActive(tdReceipt);
                }
                if (access == "PickUp")
                {
                    setActive(tdDefaultSetting);
                }
                if (access == "Email")
                {
                    setActive(tdEmail);
                }
                if (access == "General")
                {
                    setActive(tdGeneralSetting);
                }
                if (access == "Backup")
                {
                    setActive(tdbackup);
                }
                lblHeaderTextMsg.Text = "Receipt";
            }

            var btn = Request.Params["__EVENTTARGET"] as string;
            if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSetTimeZone")
            {
                lblHeaderTextMsg.Text = "Time Zone";
                btnSetTimeZone_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnDefaultSetting")
            {
                lblHeaderTextMsg.Text = "Pick Up/ New Order - Choose default for order screen";
                btnDefaultSetting_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnDispaly")
            {
                lblHeaderTextMsg.Text = "Store";
                btnDispaly_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnGeneralSetting")
            {
                btnGeneralSetting_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSetEmail")
            {
                lblHeaderTextMsg.Text = "Email";
                btnSetEmail_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnBackUp")
            {
                btnBackUp_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnServiceTac")
            {
                lblHeaderTextMsg.Text = "Default Tax Rates";
                btnServiceTac_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnReceipt")
            {
                lblHeaderTextMsg.Text = "Receipt";
                btnReceipt_Click(null, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnZoneSave")
            {
                btnZoneSave_Click(btnZoneSave, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnDefault")
            {
                btnDefault_Click(btnDefault, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnReset")
            {
                btnReset_Click(btnReset, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$Button1")
            {
                btnSave_Click(Button1, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$BtnCancel")
            {
                BtnCancel_Click(BtnCancel, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnSave")
            {
                btnSave_Click1(btnSave, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnGeneralSetting1")
            {
                btnGeneralSetting1_Click(btnGeneralSetting1, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnEmail")
            {
                btnEmail_Click(btnEmail, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btbackupsave")
            {
                btbackupsave_Click(btbackupsave, EventArgs.Empty);
            }
            else if (btn != null && btn == "ctl00$ContentPlaceHolder1$btnServiceTaxSave")
            {
                btnServiceTaxSave_Click(btnServiceTaxSave, EventArgs.Empty);
            }

            
        }

        private void SetTabContainer(bool AccessToTrue)
        {
            tdReceipt.Visible = AccessToTrue;
            tdDisplay.Visible = AccessToTrue;
            tdDefaultSetting.Visible = AccessToTrue;
            tdSetTimeZone.Visible = AccessToTrue;
            tdGeneralSetting.Visible = AccessToTrue;
            tdEmail.Visible = AccessToTrue;
            tdbackup.Visible = AccessToTrue;
            tdServiceTax.Visible = AccessToTrue;
        }

        private void setActive(object access)
        {
            HtmlTableCell fupFile = ((HtmlTableCell)access);
            fupFile.Visible = true;
        }

        private void BindDropDown()
        {
            Ob.BranchId = Globals.BranchID;
            drpMainProcesses.DataSource = BAL.BALFactory.Instance.Bal_Report.BindProcessDropDown(Ob);
            drpMainProcesses.DataTextField = "ProcessName";
            drpMainProcesses.DataValueField = "ProcessCode";
            drpMainProcesses.DataBind();
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.Bal_Report.BinItemDropDown(Ob);
            drpItems.DataSource = ds.Tables[0];
            drpItems.DataTextField = "ItemName";
            drpItems.DataValueField = "ItemID";
            drpItems.DataBind();
            drpColorName.DataSource = ds.Tables[1];
            drpColorName.DataTextField = "ColorName";
            drpColorName.DataValueField = "ID";
            drpColorName.DataBind();
        }

        protected void drpZoneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sel = drpZoneList.SelectedValue;
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(sel);
            try
            {
                DateTime tstTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, tst);
                //lblSetTimeZoneSucess.Text = tstTime.ToShortDateString() + " " + tstTime.ToLongTimeString();
                lblSetTimeZoneSucess.Text = tstTime.ToString("dd MMM yyyy") + " " + tstTime.ToLongTimeString();
            }
            catch (Exception)
            {
            }
        }

        public void BindFont()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily font in fonts.Families)
            {
                drpAFontName.Items.Add(font.Name);
                drpFontName.Items.Add(font.Name);
                drpHeaderFontName.Items.Add(font.Name);
                drpFooterFontName.Items.Add(font.Name);
                drpNameFontStyle.Items.Add(font.Name);
                drpBookingNoFontStyle.Items.Add(font.Name);
            }
        }

        protected void drpFont_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void txtFontSize_TextChanged(object sender, EventArgs e)
        {
            drpFont_SelectedIndexChanged(null, null);
        }

        protected void rdrLogoAndTest_CheckedChanged(object sender, EventArgs e)
        {
            //tblLogo.Visible = true;
            tblLogo.Attributes.Add("style", "display:block");
        }

        protected void rdrBanner_CheckedChanged(object sender, EventArgs e)
        {
           // tblLogo.Visible = false;
            tblLogo.Attributes.Add("style", "display:none");
        }

        protected void btnReceipt_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = true;
            tdDisplay.Visible = false;
            tdDefaultSetting.Visible = false;
            tdSetTimeZone.Visible = false;
            tdGeneralSetting.Visible = false;
            tdEmail.Visible = false;
            tdbackup.Visible = false;
            tdServiceTax.Visible = false;
          //  tdPassword.Visible = false;
        }

        protected void btnDispaly_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = true;
            tdDefaultSetting.Visible = false;
            tdSetTimeZone.Visible = false;
            tdGeneralSetting.Visible = false;
            tdEmail.Visible = false;
            tdbackup.Visible = false;
            tdServiceTax.Visible = false;
         //   tdPassword.Visible = false;
        }

        protected void btnSetTimeZone_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = false;
            tdDefaultSetting.Visible = false;
            tdSetTimeZone.Visible = true;
            tdGeneralSetting.Visible = false;
            tdEmail.Visible = false;
            tdbackup.Visible = false;
            tdServiceTax.Visible = false;
          //  tdPassword.Visible = false;
            PrjClass.SetItemInDropDown(drpZoneList, setTimeZone(), false, false);
            drpZoneList_SelectedIndexChanged(null, null);
        }

        //protected void btnPassWordProtected_Click(object sender, EventArgs e)
        //{
        //    tdReceipt.Visible = false;
        //    tdDisplay.Visible = false;
        //    tdDefaultSetting.Visible = false;
        //    tdSetTimeZone.Visible = false;
        //    tdGeneralSetting.Visible = false;
        //    tdEmail.Visible = false;
        //    tdbackup.Visible = false;
        //    tdServiceTax.Visible = false;
        //   // tdPassword.Visible = true;
        //}

        public string setTimeZone()
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {

                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 2);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = "" + sdr.GetValue(54);
            }
            catch (Exception)
            {
                res = "India Standard Time";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        private void FillExistingRecord()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 18);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtWebsiteName.Text = ds.Tables[0].Rows[0]["WebsiteName"].ToString();
                txtStoreName.Text = ds.Tables[0].Rows[0]["StoreName"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtTiming.Text = ds.Tables[0].Rows[0]["Timing"].ToString();
                txtFooterName.Text = ds.Tables[0].Rows[0]["FooterName"].ToString();
                txtSSlipInch.Text = ds.Tables[0].Rows[0]["SetSlipInch"].ToString();
                chkImage.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Printing"].ToString());
                chkSetting.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Configuration"].ToString());
                txtWebsiteLink.Text = ds.Tables[0].Rows[0]["WebSiteLink"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string PhotoFile = GetLogoFileName();
            if (PhotoFile != "")
            {
                string imgpath = Server.MapPath("~/Logo") + "//" + PhotoFile;
                System.IO.File.Delete(imgpath);
                fupStudentPhoto.PostedFile.SaveAs(imgpath);
                abc.Src = "../Logo/DRY" + Globals.BranchID + ".jpg";
            }
            SqlCommand cmd = new SqlCommand();
            string res = "";
            cmd.CommandText = "Sp_InsUpd_FirstTimeConfigSettings";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@WebsiteName", txtWebsiteName.Text);
            cmd.Parameters.AddWithValue("@StoreName", txtStoreName.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@Timing", txtTiming.Text);
            cmd.Parameters.AddWithValue("@FooterName", txtFooterName.Text);
            cmd.Parameters.AddWithValue("@Printing", chkImage.Checked ? "1" : "0");
            cmd.Parameters.AddWithValue("@SetSlipInch", txtSSlipInch.Text);
            cmd.Parameters.AddWithValue("@Configuration", chkSetting.Checked ? "1" : "0");
            cmd.Parameters.AddWithValue("@WebSiteLink", txtWebsiteLink.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            res = AppClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowStoreMsg('Green', '#999999');", true);
                lblSuccess.Text = "Store information updated successfully.";                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowStoreMsg('Red', '#999999');", true);
                lblErr.Text = res.ToString();
            }
        }

        private string GetLogoFileName()
        {
            HtmlInputFile fupFile = ((HtmlInputFile)fupStudentPhoto);
            string[] fileName;
            string fname = "";
            if (fupFile.Value != "")
            {
                fileName = fupFile.Value.Split('.');
                if (fileName[1] == "jpg" || fileName[1] == "png" || fileName[1] == "gif" || fileName[1] == "JPG" || fileName[1] == "PNG" || fileName[1] == "GIF" || fileName[1] == "jpeg" || fileName[1] == "JPEG")
                {
                    fname = fupFile.PostedFile.FileName;
                    if (fname.Contains("\\"))
                        fname = fname.Substring(fname.LastIndexOf("\\"));
                    if (fname.StartsWith("\\")) fname = fname.Substring(1);
                    fname = "DRY" + Globals.BranchID + ".jpg";
                }
                else
                    Session["ReturnMsg"] = "Invalid filename format, please select only (jpg,jpeg,gif,png) format";
            }
            return fname;
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Default.aspx");
        }

        private string GetLogoFileName1()
        {
            HtmlInputFile fupFile = ((HtmlInputFile)fupLogo);
            string[] fileName;
            string fname = "";
            if (fupFile.Value != "")
            {
                fileName = fupFile.Value.Split('.');
                if (fileName[1] == "jpg" || fileName[1] == "png" || fileName[1] == "gif" || fileName[1] == "JPG" || fileName[1] == "PNG" || fileName[1] == "GIF" || fileName[1] == "jpeg" || fileName[1] == "JPEG" || fileName[1] == "ico" || fileName[1] == "ICO")
                {
                    fname = fupFile.PostedFile.FileName;
                    if (fname.Contains("\\"))
                        fname = fname.Substring(fname.LastIndexOf("\\"));
                    if (fname.StartsWith("\\")) fname = fname.Substring(1);
                    fname = "DRY" + Globals.BranchID + ".jpg";
                }
                else
                {
                    Session["ReturnMsg"] = "Invalid filename format, please select only (jpg,jpeg,gif,png,ico) format";
                }
            }
            return fname;
        }

        private void SetSlipType(string FileType)
        {
            string DeleteFilePath = Server.MapPath("~/Bookings") + "//" + "BookingSlip.aspx";
            string DeleteFilePathCs = Server.MapPath("~/Bookings") + "//" + "BookingSlip.cs";
            string DeleteFilePath1 = Server.MapPath("~/Bookings") + "//" + "DeliverySlip.aspx";
            string DeleteFilePath1Cs = Server.MapPath("~/Bookings") + "//" + "DeliverySlip.cs";
            string DeleteFilePathPackageaspx = Server.MapPath("~/Bookings") + "//" + "PackageSlip.aspx";
            string DeleteFilePathPackagecs = Server.MapPath("~/Bookings") + "//" + "PackageSlip.cs";
            string LaserFile = Server.MapPath("~/LaserPrinter");
            string DotMatrix = Server.MapPath("~/DotMatrixPrinter");
            string Thermal = Server.MapPath("~/ThermalPrinter");
            string SaveFolder = Server.MapPath("~/Bookings");
            System.IO.File.Delete(DeleteFilePath);
            System.IO.File.Delete(DeleteFilePathCs);
            System.IO.File.Delete(DeleteFilePath1);
            System.IO.File.Delete(DeleteFilePath1Cs);
            System.IO.File.Delete(DeleteFilePathPackageaspx);
            System.IO.File.Delete(DeleteFilePathPackagecs);
            if (FileType == "Laser")
            {
                string[] filePaths = System.IO.Directory.GetFiles(LaserFile);
                foreach (String fileName in filePaths)
                {
                    string targetFolder = SaveFolder;
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                    fi.CopyTo(System.IO.Path.Combine(targetFolder, fi.Name), true);
                }
            }
            if (FileType == "DotMatrix")
            {
                string[] filePaths = System.IO.Directory.GetFiles(DotMatrix);
                foreach (String fileName in filePaths)
                {
                    string targetFolder = SaveFolder;
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                    fi.CopyTo(System.IO.Path.Combine(targetFolder, fi.Name), true);
                }
            }
            if (FileType == "Thermal")
            {
                string[] filePaths = System.IO.Directory.GetFiles(Thermal);
                foreach (String fileName in filePaths)
                {
                    string targetFolder = SaveFolder;
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                    fi.CopyTo(System.IO.Path.Combine(targetFolder, fi.Name), true);
                }
            }
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            string PhotoFile = GetLogoFileName1();
            if (PhotoFile != "")
            {
                string imgpath = Server.MapPath("~/ReceiptLogo") + "//" + PhotoFile;
                System.IO.File.Delete(imgpath);
                fupLogo.PostedFile.SaveAs(imgpath);
            }
            SqlCommand cmd = new SqlCommand();
            string res = "";
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HeaderText", txtName.Text);
            cmd.Parameters.AddWithValue("@HeaderFontName", drpFontName.SelectedItem.Text);
           // cmd.Parameters.AddWithValue("@HeaderFontSize", drpFontsize.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@HeaderFontSize", TxtHdrFontsize.Text.Trim());
            cmd.Parameters.AddWithValue("@HeaderFontStyle", (chkNameBold.Checked ? "Bold" : ""));
            cmd.Parameters.AddWithValue("@AddressText", txtLogoAddress.Text);
            cmd.Parameters.AddWithValue("@AddressFontName", drpAFontName.SelectedItem.Text);
           // cmd.Parameters.AddWithValue("@AddressFontSize", drpAFontSize.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@AddressFontSize", txtAddFontSize.Text.Trim());
            cmd.Parameters.AddWithValue("@AddressFontStyle", (chkAddressBold.Checked ? "Bold" : ""));
            cmd.Parameters.AddWithValue("@LogoLeftRight", (rdrLAlign.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@LogoHeight", drpLogoHeight.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@LogoWeight", txtLogoWeirht.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@SloganText", txtSloganName.Text);
            cmd.Parameters.AddWithValue("@SloganFontName", drpHeaderFontName.SelectedItem.Text);
            //cmd.Parameters.AddWithValue("@SloganFontSize", drpHeaderFontSize.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@SloganFontSize", TxtHeaderFontSize.Text.Trim());
            cmd.Parameters.AddWithValue("@SloganFontStyle", (chkHeaderBold.Checked ? "Bold" : ""));
            if (rdrThermal.Checked)
            {
                cmd.Parameters.AddWithValue("@Barcode", "0");
            }
            else
            {
                cmd.Parameters.AddWithValue("@Barcode", (rdrBarcodeTrue.Checked ? "1" : "0"));
            }
            cmd.Parameters.AddWithValue("@PreviewDue", (rdbPreviousTrue.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@Term1", txtTerms1.Text);
            cmd.Parameters.AddWithValue("@Term2", txtTerms2.Text);
            cmd.Parameters.AddWithValue("@Term3", txtTerms3.Text);
            cmd.Parameters.AddWithValue("@Term4", txtTerms4.Text);
            cmd.Parameters.AddWithValue("@FooterSloganText", txtFooterSloganName.Text);
            cmd.Parameters.AddWithValue("@FooterSloganFontName", drpFooterFontName.SelectedItem.Text);
            //cmd.Parameters.AddWithValue("@FooterSloganFontSize", drpFooterFontSize.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@FooterSloganFontSize", TxtFooterFontSize.Text.Trim());
            cmd.Parameters.AddWithValue("@FooterSloganFontStyle", (chkFooterBold.Checked ? "Bold" : ""));
            cmd.Parameters.AddWithValue("@ST", (rdbServicetaxTrue.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@PrinterLaserOrDotMatrix", (rdrLaser.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@PrintLogoonReceipt", (rdbShowOnReceiptTrue.Checked ? "1" : "0"));
            if (rdbShowOnReceiptTrue.Checked)
            {
                cmd.Parameters.AddWithValue("@ImagePath", "../Logo/DRY.jpg");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ImagePath", "");
            }
            cmd.Parameters.AddWithValue("@PrePrinted", (rdrLogoAndTest.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@TextFontStyleUL", (chkNameUL.Checked ? "underline" : ""));
            cmd.Parameters.AddWithValue("@TextFontItalic", (chkNameItalic.Checked ? "italic" : ""));
            cmd.Parameters.AddWithValue("@TextAddressUL", (chkAddressUL.Checked ? "underline" : ""));
            cmd.Parameters.AddWithValue("@TextAddressItalic", (chkAddressItalic.Checked ? "italic" : ""));
            cmd.Parameters.AddWithValue("@HeaderSloganUL", (chkHeaderUL.Checked ? "underline" : ""));
            cmd.Parameters.AddWithValue("@HeaderSloganItalic", (chkHeaderItalic.Checked ? "italic" : ""));
            cmd.Parameters.AddWithValue("@FooterSloganUL", (chkFooterUL.Checked ? "underline" : ""));
            cmd.Parameters.AddWithValue("@FooterSloganItalic", (chkFooterItalic.Checked ? "italic" : ""));
            cmd.Parameters.AddWithValue("@ShowHeaderSlogan", (rdbHeaderSloganTrue.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@ShowFooterSlogan", (rdbFooterSloganTrue.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@TermsAndConditionTrue", (rdbTermConditionTrue.Checked ? "1" : "0"));
            if (rdrDotMatrix.Checked == true)
                cmd.Parameters.AddWithValue("@DotMatrixPrinter40Cloumn", (rdbDotMatrix40.Checked ? "1" : "0"));
            else
            {
                cmd.Parameters.AddWithValue("@DotMatrixPrinter40Cloumn", "0");
            }
            cmd.Parameters.AddWithValue("@TableBorder", (rdbTableBorderTrue.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@NDB", (chkBookingB.Checked ? "Bold" : ""));
            cmd.Parameters.AddWithValue("@NDI", (chkBookingI.Checked ? "Italic" : ""));
            cmd.Parameters.AddWithValue("@NDU", (chkBookingU.Checked ? "underline" : ""));
            cmd.Parameters.AddWithValue("@NDFName", drpBookingNoFontStyle.SelectedItem.Text);
            //cmd.Parameters.AddWithValue("@NDFSize", drpFontBookingNoSize.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@NDFSize", txtFontBookingNoSize.Text.Trim());
            cmd.Parameters.AddWithValue("@NAB", (chkNameB.Checked ? "Bold" : ""));
            cmd.Parameters.AddWithValue("@NAI", (chkNameI.Checked ? "Italic" : ""));
            cmd.Parameters.AddWithValue("@NAU", (chkNameU.Checked ? "underline" : ""));
            cmd.Parameters.AddWithValue("@NAFName", drpNameFontStyle.SelectedItem.Text);
            //cmd.Parameters.AddWithValue("@NAFSize", drpNameFontSize.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@NAFSize", txtNameFontSize.Text.Trim());
            cmd.Parameters.AddWithValue("@CurrencyType", txtCurrencyType.Text);
            cmd.Parameters.AddWithValue("@StoreCopy", (chkStoreCopy.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@ThermalPrinter", (rdrThermal.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@DotMatrixPrinter", (rdrDotMatrix.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@DeliveryDotMatrixPrinter", (rdrDeliveryLaser.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintProcess", (rdbProcessTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintRate", (rdbRateTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintDueDate", (rdbPrintDueDateTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintTermConditionOnStoreCopy", (rdbPrintTermsConditonTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@Term5", txtTerms5.Text);
            cmd.Parameters.AddWithValue("@Term6", txtTerms6.Text);
            cmd.Parameters.AddWithValue("@Term7", txtTerms7.Text);
            cmd.Parameters.AddWithValue("@Term8", txtTerms8.Text);
            cmd.Parameters.AddWithValue("@Term9", txtTerms9.Text);
            cmd.Parameters.AddWithValue("@Term10", txtTerms10.Text);
            cmd.Parameters.AddWithValue("@Term11", txtTerms11.Text);
            cmd.Parameters.AddWithValue("@Term12", txtTerms12.Text);
            cmd.Parameters.AddWithValue("@Term13", txtTerms13.Text);
            cmd.Parameters.AddWithValue("@Term14", txtTerms14.Text);
            cmd.Parameters.AddWithValue("@Term15", txtTerms15.Text);
            cmd.Parameters.AddWithValue("@LeftMessage", txtleftMsg.Text);
            cmd.Parameters.AddWithValue("@RightMessage", txtRightMsg.Text);
            if (hdnPrint.Value != "")
            {
                cmd.Parameters.AddWithValue("@BookingSlipPrinter", hdnPrint.Value);
                cmd.Parameters.AddWithValue("@Flag", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Flag", 15);
            }

            cmd.Parameters.AddWithValue("@DeliveryPrinterName", hdnDeliveryPrinter.Value);
            cmd.Parameters.AddWithValue("@PrintSubItem", (rdbSubItemTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintCustomerSignature", (rdbCustomerSignatureTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintPhoneNo", (rdbPhoneNoTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@PrintTaxDetail", (rdbTaxDetailTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@IsBookingTime", (rdbBookingTimeTrue.Checked ? "true" : "false"));            
            res = AppClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowReceiptMsg('Green', '#999999');", true);
                lblMsg.Text = "Receipt configuration updated successfully.";

                //if (rdrLaser.Checked)
                //{
                //    SetSlipType("Laser");
                //}
                //else if (rdrDotMatrix.Checked)
                //{
                //    SetSlipType("Thermal");
                //}
                //else if (rdrThermal.Checked)
                //{
                //    SetSlipType("DotMatrix");
                //}
                if (hdnPrint.Value == "")
                {
                    FillLastRecordOnControls("No");
                }
                else
                {
                    FillLastRecordOnControls("Yes");
                }
            }
            else
                Session["ReturnMsg"] = res.ToString();
        }

        protected void FillLastRecordOnControls(string status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_ReceiptConfigSetting";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 2);
                ds = AppClass.GetData(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["HeaderText"].ToString();
                    txtCurrencyType.Text = ds.Tables[0].Rows[0]["CurrencyType"].ToString();
                    PrjClass.SetItemInDropDown(drpFontName, ds.Tables[0].Rows[0]["HeaderFontName"].ToString(), true, false);
                   // PrjClass.SetItemInDropDown(drpFontsize, ds.Tables[0].Rows[0]["HeaderFontSize"].ToString(), true, false);
                    TxtHdrFontsize.Text = ds.Tables[0].Rows[0]["HeaderFontSize"].ToString();
                    txtLogoAddress.Text = ds.Tables[0].Rows[0]["AddressText"].ToString();
                    PrjClass.SetItemInDropDown(drpAFontName, ds.Tables[0].Rows[0]["AddressFontName"].ToString(), true, false);
                   // PrjClass.SetItemInDropDown(drpAFontSize, ds.Tables[0].Rows[0]["AddressFontSize"].ToString(), true, false);
                    txtAddFontSize.Text = ds.Tables[0].Rows[0]["AddressFontSize"].ToString();
                    PrjClass.SetItemInDropDown(drpLogoHeight, ds.Tables[0].Rows[0]["LogoHeight"].ToString(), true, false);
                    PrjClass.SetItemInDropDown(txtLogoWeirht, ds.Tables[0].Rows[0]["LogoWeight"].ToString(), true, false);
                    txtSloganName.Text = ds.Tables[0].Rows[0]["SloganText"].ToString();
                    PrjClass.SetItemInDropDown(drpHeaderFontName, ds.Tables[0].Rows[0]["SloganFontName"].ToString(), true, false);
                  //  PrjClass.SetItemInDropDown(drpHeaderFontSize, ds.Tables[0].Rows[0]["SloganFontSize"].ToString(), true, false);
                    TxtHeaderFontSize.Text = ds.Tables[0].Rows[0]["SloganFontSize"].ToString();
                    txtTerms1.Text = ds.Tables[0].Rows[0]["Term1"].ToString();
                    txtTerms2.Text = ds.Tables[0].Rows[0]["Term2"].ToString();
                    txtTerms3.Text = ds.Tables[0].Rows[0]["Term3"].ToString();
                    txtTerms4.Text = ds.Tables[0].Rows[0]["Term4"].ToString();
                    txtTerms5.Text = ds.Tables[0].Rows[0]["Term5"].ToString();
                    txtTerms6.Text = ds.Tables[0].Rows[0]["Term6"].ToString();
                    txtTerms7.Text = ds.Tables[0].Rows[0]["Term7"].ToString();
                    txtTerms8.Text = ds.Tables[0].Rows[0]["Term8"].ToString();
                    txtTerms9.Text = ds.Tables[0].Rows[0]["Term9"].ToString();
                    txtTerms10.Text = ds.Tables[0].Rows[0]["Term10"].ToString();
                    txtTerms11.Text = ds.Tables[0].Rows[0]["Term11"].ToString();
                    txtTerms12.Text = ds.Tables[0].Rows[0]["Term12"].ToString();
                    txtTerms13.Text = ds.Tables[0].Rows[0]["Term13"].ToString();
                    txtTerms14.Text = ds.Tables[0].Rows[0]["Term14"].ToString();
                    txtTerms15.Text = ds.Tables[0].Rows[0]["Term15"].ToString();
                    txtleftMsg.Text = ds.Tables[0].Rows[0]["LeftMessage"].ToString();
                    txtRightMsg.Text = ds.Tables[0].Rows[0]["RightMessage"].ToString();
                    txtFooterSloganName.Text = ds.Tables[0].Rows[0]["FooterSloganText"].ToString();
                    PrjClass.SetItemInDropDown(drpFooterFontName, ds.Tables[0].Rows[0]["FooterSloganFontName"].ToString(), true, false);
                   // PrjClass.SetItemInDropDown(drpFooterFontSize, ds.Tables[0].Rows[0]["FooterSloganFontSize"].ToString(), true, false);
                    TxtFooterFontSize.Text = ds.Tables[0].Rows[0]["FooterSloganFontSize"].ToString();
                    PrjClass.SetItemInDropDown(drpBookingNoFontStyle, ds.Tables[0].Rows[0]["NDFName"].ToString(), true, false);
                  //  PrjClass.SetItemInDropDown(drpFontBookingNoSize, ds.Tables[0].Rows[0]["NDFSize"].ToString(), true, false);
                    txtFontBookingNoSize.Text = ds.Tables[0].Rows[0]["NDFSize"].ToString();
                    PrjClass.SetItemInDropDown(drpNameFontStyle, ds.Tables[0].Rows[0]["NAFName"].ToString(), true, false);
                    //PrjClass.SetItemInDropDown(drpNameFontSize, ds.Tables[0].Rows[0]["NAFSize"].ToString(), true, false);
                    txtNameFontSize.Text = ds.Tables[0].Rows[0]["NAFSize"].ToString();
                    //PrjClass.SetItemInDropDown(drpDefaultPrinter, ds.Tables[0].Rows[0]["BookingSlipPrinter"].ToString(), true, false);
                    if (status == "Yes")
                    {
                        hdnValue.Value = ds.Tables[0].Rows[0]["BookingSlipPrinter"].ToString();
                    }
                    ////////
                    //general
                    hdnDelPrintValue.Value = ds.Tables[0].Rows[0]["DeliveryPrinterName"].ToString();

                    rdrLaser.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrinterLaserOrDotMatrix"].ToString());
                    rdrDotMatrix.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DotMatrixPrinter"].ToString());
                    rdrThermal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ThermalPrinter"].ToString());
                    rdbProcessTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintProcess"].ToString());
                    rdbRateTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintRate"].ToString());
                    rdbSendToWorkShopTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SendToWorkShop"].ToString());
                    rdbReceiveTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ReceiveWorkShop"].ToString());
                    rdbMarkDeliveryTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["MarkDelivery"].ToString());
                    rdbWorkRecieveTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["WorkShopRecieve"].ToString());
                    rdbWorkReadyTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["WorkShopReady"].ToString());

                    rdrDeliveryLaser.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DeliveryDotMatrixPrinter"].ToString());
                    if (rdrDeliveryLaser.Checked)
                    {
                        rdrDeliveryLaser.Checked = true;
                        rdrDeliveryDotMatrix.Checked = false;
                    }
                    else
                    {
                        rdrDeliveryDotMatrix.Checked = true;
                        rdrDeliveryLaser.Checked = false;
                    }


                    if (rdbSendToWorkShopTrue.Checked)
                    {
                        rdbSendToWorkShopTrue.Checked = true;
                        rdbSendToWorkShopFalse.Checked = false;
                    }
                    else
                    {
                        rdbSendToWorkShopTrue.Checked = false;
                        rdbSendToWorkShopFalse.Checked = true;
                    }

                    if (rdbReceiveTrue.Checked)
                    {
                        rdbReceiveTrue.Checked = true;
                        rdbReceiveFalse.Checked = false;
                    }
                    else
                    {
                        rdbReceiveTrue.Checked = false;
                        rdbReceiveFalse.Checked = true;
                    }

                    if (rdbMarkDeliveryTrue.Checked)
                    {
                        rdbMarkDeliveryTrue.Checked = true;
                        rdbMarkDeliveryFalse.Checked = false;
                    }
                    else
                    {
                        rdbMarkDeliveryTrue.Checked = false;
                        rdbMarkDeliveryFalse.Checked = true;
                    }

                    if (rdbWorkRecieveTrue.Checked)
                    {
                        rdbWorkRecieveTrue.Checked = true;
                        rdbWorkRecieveFalse.Checked = false;
                    }
                    else
                    {
                        rdbWorkRecieveTrue.Checked = false;
                        rdbWorkRecieveFalse.Checked = true;
                    }
                    if (rdbWorkReadyTrue.Checked)
                    {
                        rdbWorkReadyTrue.Checked = true;
                        rdbWorkReadyFalse.Checked = false;
                    }
                    else
                    {
                        rdbWorkReadyTrue.Checked = false;
                        rdbWorkReadyFalse.Checked = true;
                    }
                    if (rdbProcessTrue.Checked)
                    {
                        rdbProcessTrue.Checked = true;
                       // rdbProcessFalse.Checked = false;
                    }
                    else
                    {
                        rdbProcessTrue.Checked = false;
                       // rdbProcessFalse.Checked = true;
                    }
                    if (rdbRateTrue.Checked)
                    {
                        rdbRateTrue.Checked = true;
                      //  rdbRateFalse.Checked = false;
                    }
                    else
                    {
                        rdbRateTrue.Checked = false;
                       // rdbRateFalse.Checked = true;
                    }
                    if (rdrLaser.Checked == true)
                    {
                        rdrLaser.Checked = true;
                        rdrLaser_CheckedChanged(null, null);
                    }
                    if (rdrDotMatrix.Checked == true)
                    {
                        rdrDotMatrix.Checked = true;
                        rdrDotMatrix_CheckedChanged(null, null);
                    }
                    if (rdrThermal.Checked)
                    {
                        rdrThermal.Checked = true;
                        rdrThermal_CheckedChanged(null, null);
                    }
                    rdrBarcodeTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Barcode"].ToString());
                    if (rdrBarcodeTrue.Checked != true)
                    {
                       // rdrBarcodeFalse.Checked = true;
                        rdrBarcodeTrue.Checked = false;
                    }
                    else
                    {
                        rdrBarcodeTrue.Checked = true;
                    }
                    rdbPrintDueDateTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintDueDate"].ToString());
                    if (rdbPrintDueDateTrue.Checked != true)
                    {
                       // rdbPrintDueDateFalse.Checked = true;
                        rdbPrintDueDateTrue.Checked = false;
                    }
                    else
                    {
                        rdbPrintDueDateTrue.Checked = true;
                    }
                    rdbPrintTermsConditonTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintTermConditionOnStoreCopy"].ToString());
                    if (rdbPrintTermsConditonTrue.Checked != true)
                    {
                        //rdbPrintTermsConditonFalse.Checked = true;
                        rdbPrintTermsConditonTrue.Checked = false;
                    }
                    else
                    {
                        rdbPrintTermsConditonTrue.Checked = true;
                    }
                    rdbPreviousTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PreviewDue"].ToString());
                    if (rdbPreviousTrue.Checked != true)
                    {
                        //rdbPreviousFalse.Checked = true;
                        rdbPreviousTrue.Checked = false;
                    }
                    else
                    { rdbPreviousTrue.Checked = true; }
                    rdbServicetaxTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ST"].ToString());
                    if (rdbServicetaxTrue.Checked != true)
                    {
                       // rdbServicetaxFalse.Checked = true;
                        rdbServicetaxTrue.Checked = false;
                    }
                    else
                    {
                        rdbServicetaxTrue.Checked = true;
                    }
                    rdbShowOnReceiptTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrintLogoonReceipt"].ToString());
                    if (rdbShowOnReceiptTrue.Checked != true)
                    {
                        //rdbShowOnReceiptFalse.Checked = true;
                        rdbShowOnReceiptTrue.Checked = false;
                    }
                    else
                    {
                        rdbShowOnReceiptTrue.Checked = true;
                       // rdbShowOnReceiptFalse.Checked = false;
                    }
                    rdbHeaderSloganTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowHeaderSlogan"].ToString());
                    if (rdbHeaderSloganTrue.Checked != true)
                    {
                       // rdbHeaderSloganFalse.Checked = true;
                        rdbHeaderSloganTrue.Checked = false;
                    }
                    else
                    { rdbHeaderSloganTrue.Checked = true; }
                    rdbFooterSloganTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowFooterSlogan"].ToString());
                    if (rdbFooterSloganTrue.Checked != true)
                    {
                        rdbFooterSloganFalse.Checked = true;
                    }
                    else
                    { rdbFooterSloganTrue.Checked = true; }
                    rdbTermConditionTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["TermsAndConditionTrue"].ToString());
                    if (rdbTermConditionTrue.Checked != true)
                    {
                       // rdbTermConditionFalse.Checked = true;
                        rdbTermConditionTrue.Checked = false;
                    }
                    else
                    { rdbTermConditionTrue.Checked = true; }
                    rdbTableBorderTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["TableBorder"].ToString());
                    if (rdbTableBorderTrue.Checked)
                    {
                        rdbTableBorderTrue.Checked = true;
                    }
                    else
                    {
                       // rdbTableBorderFalse.Checked = true;
                        rdbTableBorderTrue.Checked = false;
                    }
                    rdrLogoAndTest.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PrePrinted"].ToString());
                    if (rdrLogoAndTest.Checked == true)
                    {
                        //tblLogo.Visible = true;
                        tblLogo.Attributes.Add("style", "display:block");
                    }
                    else
                    {
                       // tblLogo.Visible = false;
                        tblLogo.Attributes.Add("style", "display:none");
                       // rdrBanner.Checked = true;
                        rdrLogoAndTest.Checked = false;
                    }
                    string LogoLeftRight = ds.Tables[0].Rows[0]["LogoLeftRight"].ToString();
                    if (LogoLeftRight == "1")
                    {
                        rdrLAlign.Checked = true;
                    }
                    else
                    {
                        rdrRAlign.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() != "")
                    {
                        chkNameBold.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() != "")
                    {
                        chkNameBold.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["HeaderFontStyle"].ToString() != "")
                    {
                        chkNameBold.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["TextFontStyleUL"].ToString() != "")
                    {
                        chkNameUL.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["TextFontItalic"].ToString() != "")
                    {
                        chkNameItalic.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["AddressFontStyle"].ToString() != "")
                    {
                        chkAddressBold.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["TextAddressUL"].ToString() != "")
                    {
                        chkAddressUL.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["TextAddressItalic"].ToString() != "")
                    {
                        chkAddressItalic.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["SloganFontStyle"].ToString() != "")
                    {
                        chkHeaderBold.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["HeaderSloganUL"].ToString() != "")
                    {
                        chkHeaderUL.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["HeaderSloganItalic"].ToString() != "")
                    {
                        chkHeaderItalic.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["FooterSloganFontStyle"].ToString() != "")
                    {
                        chkFooterBold.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["FooterSloganUL"].ToString() != "")
                    {
                        chkFooterUL.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["FooterSloganItalic"].ToString() != "")
                    {
                        chkFooterItalic.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["NDB"].ToString() != "")
                    {
                        chkBookingB.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["NDI"].ToString() != "")
                    {
                        chkBookingI.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["NDU"].ToString() != "")
                    {
                        chkBookingU.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["NAB"].ToString() != "")
                    {
                        chkNameB.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["NAI"].ToString() != "")
                    {
                        chkNameI.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["NAU"].ToString() != "")
                    {
                        chkNameU.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["StoreCopy"].ToString() == "True")
                    {
                        chkStoreCopy.Checked = true;
                    }
                    else
                    {
                        chkStoreCopy.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["SSL"].ToString() == "True")
                    {
                        chkSSL.Checked = true;
                    }
                    else
                    {
                        chkSSL.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["PrintSubItem"].ToString() == "True")
                    {
                        rdbSubItemTrue.Checked = true;
                    }
                    else
                    {
                       // rdbSubItemFalse.Checked = true;
                        rdbSubItemTrue.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["PrintCustomerSignature"].ToString() == "True")
                    {
                        rdbCustomerSignatureTrue.Checked = true;
                    }
                    else
                    {
                      //  rdbCustomerSignatureFalse.Checked = true;
                        rdbCustomerSignatureTrue.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["PrintPhoneNo"].ToString() == "True")
                    {
                        rdbPhoneNoTrue.Checked = true;
                    }
                    else
                    {
                        //rdbPhoneNoFalse.Checked = true;
                        rdbPhoneNoTrue.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["PrintTaxDetail"].ToString() == "True")
                    {
                        rdbTaxDetailTrue.Checked = true;
                    }
                    else
                    {
                       // rdbTaxDetailFalse.Checked = true;
                        rdbTaxDetailTrue.Checked = false;
                    }
                    PrjClass.SetItemInDropDown(drpChallanType, ds.Tables[0].Rows[0]["ChallanType"].ToString(), true, false);
                    txtHostName.Text = ds.Tables[0].Rows[0]["HostName"].ToString();
                    txtStatusEmailID.Text = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    txtBranchEmail.Text = ds.Tables[0].Rows[0]["BranchEmail"].ToString();
                 //   txtBranchPassword.Text = ds.Tables[0].Rows[0]["BranchPassword"].ToString();
                    txtBranchPassword.Attributes.Add("Value", ds.Tables[0].Rows[0]["BranchPassword"].ToString());
                    hdnPassword.Value = ds.Tables[0].Rows[0]["BranchPassword"].ToString();
                    chkmailActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsEmailActive"].ToString());
                    PrjClass.SetItemInDropDown(drpPort, ds.Tables[0].Rows[0]["Port"].ToString(), true, false);
                    if(chkmailActive.Checked == true)
                    {
                        txtStatusEmailID.Enabled = true;
                    }
                    else
                    {
                        txtStatusEmailID.Enabled = false;
                    }
                    rdbBookingTimeTrue.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBookingTime"].ToString());
                    if (rdbBookingTimeTrue.Checked != true)
                    {
                       // rdbBookingTimeFalse.Checked = true;
                        rdbBookingTimeTrue.Checked = false;
                    }
                    else
                    {
                        rdbBookingTimeTrue.Checked = true;
                    } 
                }
            }
            catch (Exception ex)
            { }
        }

        protected void rdrLaser_CheckedChanged(object sender, EventArgs e)
        {
            //rdbRateTrue.Enabled = true;
            rdbRateTrue.Disabled = false;
           // rdbRateFalse.Enabled = true;
           // rdbTableBorderTrue.Enabled = true;
            rdbTableBorderTrue.Disabled = false;
           // rdbTableBorderFalse.Enabled = true;
            //rdbPrintTermsConditonTrue.Enabled = false;
            rdbPrintTermsConditonTrue.Disabled = true;
           // rdbPrintTermsConditonFalse.Enabled = false;
           // rdbPhoneNoFalse.Enabled = false;
            //rdbPhoneNoTrue.Enabled = false;
            rdbPhoneNoTrue.Disabled = true;
           // rdbCustomerSignatureFalse.Enabled = false;
           // rdbCustomerSignatureTrue.Enabled = false;
            rdbCustomerSignatureTrue.Disabled = true;
           // rdbProcessTrue.Enabled = true;
            rdbProcessTrue.Disabled = false;
           // rdbProcessFalse.Enabled = true;
            //rdbSubItemTrue.Enabled = true;
            rdbSubItemTrue.Disabled = false;
           // rdbSubItemFalse.Enabled = true;
            //rdbPreviousTrue.Enabled = true;
            rdbPreviousTrue.Disabled = false;
           // rdbPreviousFalse.Enabled = true;
            //trStoreCopy.Visible = false;
            //rdrLogoAndTest.Enabled = true;
            rdrLogoAndTest.Disabled = false;
            //rdrBanner.Enabled = true;
          //  rdrBanner.Disabled = false;
            tdName.Visible = true;
            tdName1.Visible = true;
            tdName2.Visible = true;
            tdName3.Visible = true;
            tdName4.Visible = true;
            tdName5.Visible = true;
            tdName6.Visible = true;
            tdName7.Visible = true;
            tdName8.Visible = true;
            tdName9.Visible = true;
            tdName10.Visible = true;
            tdName11.Visible = true;
            tdName12.Visible = true;
            tdName13.Visible = true;
            tdName14.Visible = true;
            tdName15.Visible = true;
            tdName16.Visible = true;
            tdName17.Visible = true;
            tdName18.Visible = true;
            tdName19.Visible = true;
            tdName20.Visible = true;
            tdName21.Visible = true;
            tdName22.Visible = true;
            tdName23.Visible = true;
            tdName24.Visible = true;
            tdName25.Visible = true;
            tdName26.Visible = true;
            tdName27.Visible = true;
            tdName28.Visible = true;
            tdName29.Visible = true;
            tdName30.Visible = true;
            tdName31.Visible = true;
            tdName32.Visible = true;
            tdName33.Visible = true;
            tdName34.Visible = true;
            tdName35.Visible = true;
            //rdrLAlign.Enabled = true;
            rdrLAlign.Disabled = false;
           // rdrRAlign.Enabled = true;
            rdrRAlign.Disabled = false;
            tdName36.Visible = true;
            tdName37.Visible = true;
            tdName38.Visible = true;
            tdName39.Visible = true;
            tdName40.Visible = true;
            tdName41.Visible = true;
            tdName42.Visible = true;
            tdName43.Visible = true;
           // rdbShowOnReceiptTrue.Enabled = true;
            rdbShowOnReceiptTrue.Disabled = false;
           // rdbShowOnReceiptFalse.Enabled = true;
            //rdbShowOnReceiptFalse.Checked = true;
            rdbShowOnReceiptTrue.Checked = false;
           // rdrBarcodeTrue.Enabled = true;
            rdrBarcodeTrue.Disabled = false;
            //rdrBarcodeFalse.Enabled = true;
            tdName100.Visible = false;
            tdName101.Visible = false;
            tdName102.Visible = false;
            tdName103.Visible = false;
            SetTermsAndConditiontextBox(false);
        }

        protected void rdrDotMatrix_CheckedChanged(object sender, EventArgs e)
        {
            //rdbRateTrue.Enabled = false;
            rdbRateTrue.Disabled = true;
           // rdbRateFalse.Enabled = false;
           // rdbPreviousFalse.Checked = true;
            rdbPreviousTrue.Checked = false;
           // rdbTableBorderTrue.Enabled = false;
            rdbTableBorderTrue.Disabled = true;
           // rdbTableBorderFalse.Enabled = false;
            //rdbSubItemTrue.Enabled = false;
            rdbSubItemTrue.Disabled = true;
           // rdbSubItemFalse.Enabled = false;
           // rdbTableBorderFalse.Checked = true;
            rdbTableBorderTrue.Checked = false;
            //rdbPreviousTrue.Enabled = false;
            rdbPreviousTrue.Disabled = true;
           // rdbPreviousFalse.Enabled = false;
           // rdbPreviousFalse.Checked = true;
            rdbPreviousTrue.Checked = false;
            //rdbProcessTrue.Enabled = false;
            rdbProcessTrue.Disabled = true;
           // rdbProcessFalse.Enabled = false;
           // rdbPrintTermsConditonTrue.Enabled = true;
            rdbPrintTermsConditonTrue.Disabled = false;
            //rdbPrintTermsConditonFalse.Enabled = true;
           // rdbPhoneNoFalse.Enabled = true;
           // rdbPhoneNoTrue.Enabled = true;
            rdbPhoneNoTrue.Disabled = false;
           // rdbCustomerSignatureFalse.Enabled = true;
           // rdbCustomerSignatureTrue.Enabled = true;
            rdbCustomerSignatureTrue.Disabled = false;
            //trStoreCopy.Visible = true;
            //rdrLogoAndTest.Enabled = false;
            rdrLogoAndTest.Disabled = true;
           // rdrBanner.Enabled = false;
           // rdrBanner.Disabled = true;
            tdName18.Visible = false;
            tdName19.Visible = false;
            tdName20.Visible = false;
            tdName21.Visible = false;
            tdName22.Visible = false;
            tdName23.Visible = false;
            tdName24.Visible = false;
            tdName25.Visible = false;
            tdName26.Visible = false;
            tdName27.Visible = false;
            tdName28.Visible = false;
            tdName29.Visible = false;
            tdName30.Visible = false;
            tdName31.Visible = false;
            tdName32.Visible = false;
            tdName33.Visible = false;
            tdName34.Visible = false;
            tdName35.Visible = false;
           // rdrLAlign.Enabled = false;
            rdrLAlign.Disabled = true;
           // rdrRAlign.Enabled = false;
            rdrRAlign.Disabled = true;
            rdrRAlign.Checked = true;
            tdName36.Visible = true;
            tdName37.Visible = true;
            tdName38.Visible = true;
            tdName39.Visible = true;
            tdName40.Visible = true;
            tdName41.Visible = true;
            tdName42.Visible = true;
            tdName43.Visible = true;
            tdName100.Visible = true;
            tdName101.Visible = true;
            tdName102.Visible = true;
            tdName103.Visible = true;
            tdName.Visible = true;
            tdName1.Visible = true;
            tdName2.Visible = true;
            tdName3.Visible = true;
            tdName4.Visible = true;
            tdName5.Visible = true;
            tdName6.Visible = true;
            tdName7.Visible = true;
            tdName8.Visible = true;
            tdName9.Visible = true;
            tdName10.Visible = true;
            tdName11.Visible = true;
            tdName12.Visible = true;
            tdName13.Visible = true;
            tdName14.Visible = true;
            tdName15.Visible = true;
            tdName16.Visible = true;
            tdName17.Visible = true;
            //rdrBarcodeTrue.Enabled = true;
            rdrBarcodeTrue.Disabled = false;
           // rdrBarcodeFalse.Enabled = true;
           // rdbShowOnReceiptTrue.Enabled = true;
            rdbShowOnReceiptTrue.Disabled = false;
           // rdbShowOnReceiptFalse.Enabled = true;
            rdbShowOnReceiptTrue.Checked = true;
            rdrBarcodeTrue.Checked = true;
           // rdrBarcodeFalse.Checked = false;
            rdrBarcodeTrue.Checked = true;
            //rdbShowOnReceiptFalse.Checked = false;
            rdbShowOnReceiptTrue.Checked = true;
            SetTermsAndConditiontextBox(true);
        }

        protected void SetTermsAndConditiontextBox(bool status)
        {
            terms4.Visible = status;
            terms5.Visible = status;
            terms6.Visible = status;
            terms7.Visible = status;
            terms8.Visible = status;
            terms9.Visible = status;
            terms10.Visible = status;
            terms11.Visible = status;
            terms12.Visible = status;
            terms13.Visible = status;
            terms14.Visible = status;
            terms15.Visible = status;
        }

        protected void rdrThermal_CheckedChanged(object sender, EventArgs e)
        {
            tdName36.Visible = false;
            tdName37.Visible = false;
            tdName38.Visible = false;
            tdName39.Visible = false;
            tdName40.Visible = false;
            tdName41.Visible = false;
            tdName42.Visible = false;
            tdName43.Visible = false;
           // rdbShowOnReceiptTrue.Enabled = false;
            rdbShowOnReceiptTrue.Disabled = true;
           // rdbShowOnReceiptFalse.Enabled = false;
            rdbShowOnReceiptTrue.Checked = false;
           // rdrBarcodeTrue.Enabled = false;
            rdrBarcodeTrue.Disabled = true;
           // rdrBarcodeFalse.Enabled = false;
            //rdrBarcodeFalse.Checked = true;
            rdrBarcodeTrue.Checked = false;
            //rdbShowOnReceiptFalse.Checked = true;
            rdbShowOnReceiptTrue.Checked = false;
        }

        protected void btnDefaultSetting_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = false;
            tdSetTimeZone.Visible = false;
            tdDefaultSetting.Visible = true;
            tdEmail.Visible = false;
            tdGeneralSetting.Visible = false;
            tdbackup.Visible = false;
            tdServiceTax.Visible = false;
        //    tdPassword.Visible = false;
            drpMainProcesses.Focus();
            DataForDefaultSetting();
        }

        protected void btnDefault_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "", searchType = "", Urgunt1 = "", Urgunt2 = "";
            Urgunt1 = BAL.BALFactory.Instance.BAL_Priority.CheckBlanckEntries(txtUrgunt1.Text, txtUrgunt1Rate.Text, txtUrgunt1Day.Text);
            Urgunt2 = BAL.BALFactory.Instance.BAL_Priority.CheckBlanckEntries(txtUrgunt2.Text, txtUrgunt2Rate.Text, txtUrgunt2Day.Text);
            if (Urgunt1 != "Done")
            {
                if (Urgunt1 == "label name")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                    lblDefaultErr.Text = "Please enter urgent1 label name.";
                    txtUrgunt1.Focus();
                    return;
                }
                else if (Urgunt1 == "rate")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                    lblDefaultErr.Text = "Please enter urgent1 rate.";
                    txtUrgunt1Rate.Focus();
                    return;
                }
                else if (Urgunt1 == "day off set")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                    lblDefaultErr.Text = "Please enter urgent1 dayoffset.";
                    txtUrgunt1Day.Focus();
                    return;
                }
            }
            if (Urgunt2 != "Done")
            {
                if (Urgunt2 == "label name")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                    lblDefaultErr.Text = "Please enter urgent2 label name.";
                    txtUrgunt2.Focus();
                    return;
                }
                else if (Urgunt2 == "rate")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                    lblDefaultErr.Text = "Please enter urgent2 rate.";
                    txtUrgunt2Rate.Focus();
                    return;
                }
                else if (Urgunt2 == "day off set")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                    lblDefaultErr.Text = "Please enter urgent2 dayoffset.";
                    txtUrgunt2Day.Focus();
                    return;
                }
            }
            if (txtDefaultDateSet.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                lblDefaultErr.Text = "Please enter default date set.";
                txtDefaultDateSet.Focus();
                return;
            }
            if (txtStartBookingNo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                lblDefaultErr.Text = "Booking no cannot left blank.";
                txtStartBookingNo.Focus();
                return;
            }
            if (txtStartBookingNo.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                lblDefaultErr.Text = "Booking no greater than zero";
                txtStartBookingNo.Focus();
                return;
            }
            if (rdbName.Checked)
                searchType = "Name";
            else if (rdbAddress.Checked)
                searchType = "Address";
            else if (rdbMobileNo.Checked)
                searchType = "Mobile";
            cmd.CommandText = "Sp_InsUpd_ConfigSettings";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DefMainProcessCode", drpMainProcesses.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@DefItemId", drpItems.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@DefaultColorName", drpColorName.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@DefaultTime", txtDefaultTime.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@DefaultAmPm", drpAMPM.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@DeliveryDateOffset", txtDefaultDateSet.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            if (BAL.BALFactory.Instance.BAL_New_Bookings.CheckInvoiceNo(txtStartBookingNo.Text, Globals.BranchID, drpBookingPreFix.SelectedItem.Value) != true)
                cmd.Parameters.AddWithValue("@StartBookingNumberFrom", txtStartBookingNo.Text);
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                lblDefaultErr.Text = "Enter booking number already exist";
                txtStartBookingNo.Focus();
                return;
            }
            cmd.Parameters.AddWithValue("@DefaultSearchCriteria", drpDefaultCustomerSearch.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@AmountType", (rdbFlat.Checked ? "False" : "True"));
            cmd.Parameters.AddWithValue("@Today", txtUrgunt1.Text);
            cmd.Parameters.AddWithValue("@NextDay", txtUrgunt2.Text);
            cmd.Parameters.AddWithValue("@TodayRate", txtUrgunt1Rate.Text);
            cmd.Parameters.AddWithValue("@NextDayRate", txtUrgunt2Rate.Text);
            cmd.Parameters.AddWithValue("@TodayExtendDay", txtUrgunt1Day.Text);
            cmd.Parameters.AddWithValue("@NextDayExtendDay", txtUrgunt2Day.Text);
            cmd.Parameters.AddWithValue("@DescriptionEnabled", (rdbEnterRemarkTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@ColorEnabled", (rdbEnterColorTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@BindColor", (rdbBindColorToMasterTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@BindDesc", (rdbBindToDescriptionTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@SetQty", drpSetQty.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@DefaultDiscountType", drpDefaultDiscountType.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@ConfirmDeliveryDate", (rdbConfirmDateTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@SaveRateInMaster", (rdbSaveRateInItemMasterTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@SaveEditRemarks", (rdbSaveEditRemarksTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@BindColorToQty", (rdbBindColorQtyTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@BookingPreFix", drpBookingPreFix.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@ChallanType", drpChallanType.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@AndroidEditable", (chkAndroid.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@Flag", "1");
            res = AppClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Green', '#999999');", true);
                lblDefaultSucess.Text = "Default setting of  Pick Up/ New Order updated successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowMsgDefaultSetting('Red', '#999999');", true);
                lblDefaultErr.Text = res.ToString();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            DataForDefaultSetting();
            //Reset  the controls on default setting tab bar
        }

        protected void DataForDefaultSetting()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            string searchType = "";
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 28);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                PrjClass.SetItemInDropDown(drpMainProcesses, ds.Tables[0].Rows[0]["DefaultProcessCode"].ToString(), false, false);
                PrjClass.SetItemInDropDown(drpItems, ds.Tables[0].Rows[0]["DefaultItemId"].ToString(), false, false);
                PrjClass.SetItemInDropDown(drpColorName, ds.Tables[0].Rows[0]["DefaultColorName"].ToString(), false, false);
                PrjClass.SetItemInDropDown(drpAMPM, ds.Tables[0].Rows[0]["DefaultAmPm"].ToString(), false, false);
                PrjClass.SetItemInDropDown(txtDefaultTime, ds.Tables[0].Rows[0]["DefaultTime"].ToString(), false, false);
                PrjClass.SetItemInDropDown(drpServiceTaxType, ds.Tables[0].Rows[0]["InclusiveExclusive"].ToString(), true, false);
                PrjClass.SetItemInDropDown(drpSetQty, ds.Tables[0].Rows[0]["SetQty"].ToString(), false, false);
                PrjClass.SetItemInDropDown(drpDefaultDiscountType, ds.Tables[0].Rows[0]["DefaultDiscountType"].ToString(), true, false);
                txtDefaultDateSet.Text = ds.Tables[0].Rows[0]["DeliveryDateOffset"].ToString();
                txtStartBookingNo.Text = ds.Tables[0].Rows[0]["StartBookingNumberFrom"].ToString();
                txtUrgunt1.Text = ds.Tables[0].Rows[0]["Today"].ToString();
                txtUrgunt1Day.Text = ds.Tables[0].Rows[0]["TodayExtendDay"].ToString();
                txtUrgunt1Rate.Text = ds.Tables[0].Rows[0]["TodayRate"].ToString();
                txtUrgunt2.Text = ds.Tables[0].Rows[0]["NextDay"].ToString();
                txtUrgunt2Day.Text = ds.Tables[0].Rows[0]["NextDayExtendDay"].ToString();
                txtUrgunt2Rate.Text = ds.Tables[0].Rows[0]["NextDayRate"].ToString();

                txtServiceTaxRate1.Text = ds.Tables[0].Rows[0]["ServiceTaxRate1"].ToString();
                txtServiceTaxRate2.Text = ds.Tables[0].Rows[0]["ServiceTaxRate2"].ToString();
                txtServiceTaxRate3.Text = ds.Tables[0].Rows[0]["ServiceTaxRate3"].ToString();
                txtServiceTaxText1.Text = ds.Tables[0].Rows[0]["ServiceTaxText1"].ToString();
                txtServiceText2.Text = ds.Tables[0].Rows[0]["ServiceTaxText2"].ToString();
                txtServiceText3.Text = ds.Tables[0].Rows[0]["ServiceTaxText3"].ToString();

                searchType = ds.Tables[0].Rows[0]["DefaultSearchCriteria"].ToString();
                if (searchType == "Name")
                {
                    rdbName.Checked = true;
                    rdbAddress.Checked = false;
                    rdbMobileNo.Checked = false;
                }
                else if (searchType == "Address")
                {
                    rdbName.Checked = false;
                    rdbAddress.Checked = true;
                    rdbMobileNo.Checked = false;
                }
                else if (searchType == "Mobile")
                {
                    rdbName.Checked = false;
                    rdbAddress.Checked = false;
                    rdbMobileNo.Checked = true;
                }
                PrjClass.SetItemInDropDown(drpDefaultCustomerSearch, ds.Tables[0].Rows[0]["DefaultSearchCriteria"].ToString(), false, false);
                PrjClass.SetItemInDropDown(drpBookingPreFix, ds.Tables[0].Rows[0]["BookingPreFix"].ToString(), false, false);
                string AmountType = ds.Tables[0].Rows[0]["AmountType"].ToString();
                if (AmountType == "False")
                    rdbFlat.Checked = true;
                else
                    rdbFloat.Checked = true;
                if (ds.Tables[0].Rows[0]["TaxCalCulation"].ToString() == "True")
                {
                    rdbTaxBefore.Checked = true;
                }
                else
                {
                    rdbTaxAfter.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["DescriptionEnabled"].ToString() == "True")
                {
                    rdbEnterRemarkTrue.Checked = true;
                }
                else
                {
                   // rdbEnterRemarkFalse.Checked = true;
                    rdbEnterRemarkTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["ColorEnabled"].ToString() == "True")
                {
                    rdbEnterColorTrue.Checked = true;
                }
                else
                {
                   // rdbEnterColorFalse.Checked = true;
                    rdbEnterColorTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["BindColorToMaster"].ToString() == "True")
                {
                    rdbBindColorToMasterTrue.Checked = true;
                }
                else
                {
                    //rdbBindColorToMasterFalse.Checked = true;
                    rdbBindColorToMasterTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["BindDescToMaster"].ToString() == "True")
                {
                    rdbBindToDescriptionTrue.Checked = true;
                }
                else
                {
                    //rdbBindToDescriptionFalse.Checked = true;
                    rdbBindToDescriptionTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["ConfirmDeliveryDate"].ToString() == "True")
                {
                    rdbConfirmDateTrue.Checked = true;
                }
                else
                {
                   // rdbConfirmDateFalse.Checked = true;
                    rdbConfirmDateTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["SaveRateInMaster"].ToString() == "True")
                {
                    rdbSaveRateInItemMasterTrue.Checked = true;
                }
                else
                {
                    rdbSaveRateInItemMasterFalse.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["SaveEditRemarks"].ToString() == "True")
                {
                    rdbSaveEditRemarksTrue.Checked = true;
                }
                else
                {
                   // rdbSaveEditRemarksFalse.Checked = true;
                    rdbSaveEditRemarksTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["BindColorToQty"].ToString() == "True")
                {
                    rdbBindColorQtyTrue.Checked = true;
                }
                else
                {
                   // rdbBindColorQtyFalse.Checked = true;
                    rdbBindColorQtyTrue.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["AndroidEditable"].ToString() == "True")
                {
                    chkAndroid.Checked = true;
                }
                else
                {                  
                    chkAndroid.Checked = false;
                }

                if (ds.Tables[0].Rows[0]["ItemCreationPassword"].ToString() != "")
                {
                    rdbItemCreationTrue.Checked = true;
                    txtPasswordItemCreation.Attributes.Add("Value", ds.Tables[0].Rows[0]["ItemCreationPassword"].ToString());
                    txtPasswordItemCreation.Attributes.Add("style", "display:block");
                    txtPasswordItemCreation.Attributes.Add("hidden", "false");
                }
                else
                {
                    rdbItemCreationFalse.Checked = true;
                    txtPasswordItemCreation.Attributes.Add("style", "display:none");
                }
                if (ds.Tables[0].Rows[0]["RateChangePassword"].ToString() != "")
                {
                    rdbRateChangeTrue.Checked = true;
                    txtRateChange.Attributes.Add("Value", ds.Tables[0].Rows[0]["RateChangePassword"].ToString());
                    txtRateChange.Attributes.Add("style", "display:block");
                    txtRateChange.Attributes.Add("hidden", "false");
                }
                else
                {
                    rdbRateChangeFalse.Checked = true;
                    txtRateChange.Attributes.Add("style", "display:none");
                }
                if (ds.Tables[0].Rows[0]["DiscountChangePassword"].ToString() != "")
                {
                    rdbDiscountChangeTrue.Checked = true;
                    txtDiscountChange.Attributes.Add("Value", ds.Tables[0].Rows[0]["DiscountChangePassword"].ToString());
                    txtDiscountChange.Attributes.Add("style", "display:block");
                    txtDiscountChange.Attributes.Add("hidden", "false");
                }
                else
                {
                    rdbDiscountChangeFalse.Checked = true;
                    txtDiscountChange.Attributes.Add("style", "display:none");
                }
                if (ds.Tables[0].Rows[0]["DiscountDelChangePassword"].ToString() != "")
                {
                    rdbDiscountDelChangeTrue.Checked = true;
                    txtDiscountDelChange.Attributes.Add("Value", ds.Tables[0].Rows[0]["DiscountDelChangePassword"].ToString());
                    txtDiscountDelChange.Attributes.Add("style", "display:block");
                    txtDiscountDelChange.Attributes.Add("hidden", "false");
                }
                else
                {
                    rdbDiscountDelChangeFalse.Checked = true;
                    txtDiscountDelChange.Attributes.Add("style", "display:none");
                }
            }
        }

        protected void btnZoneSave_Click(object sender, EventArgs e)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 3);
            cmd.Parameters.AddWithValue("@TimeZone", drpZoneList.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            res = AppClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowTimeZoneMsg('Green', '#999999');", true);
                lblSetTimeZoneSuccess.Text = "Time zone configured successfully.";
                btnSetTimeZone_Click(null, null);
            }
        }

        public void BindDropDown1()
        {
            Drpdrive.Focus();
            string[] DriveList = Environment.GetLogicalDrives();
            for (int i = 0; i < DriveList.Length; i++)
            {
                Drpdrive.Items.Add(new ListItem(DriveList[i].ToString(), DriveList[i].ToString()));
            }
        }

        protected void btnGeneralSetting_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = false;
            tdSetTimeZone.Visible = false;
            tdDefaultSetting.Visible = false;
            tdGeneralSetting.Visible = true;
            tdbackup.Visible = false;
            tdEmail.Visible = false;
            tdServiceTax.Visible = false;
          //  tdPassword.Visible = false;
            drpChallanType.Focus();
            if (hdnPrint.Value == "")
            {
                FillLastRecordOnControls("No");
            }
            else
            {
                FillLastRecordOnControls("Yes");
            }
        }

        protected void btnGeneralSetting1_Click(object sender, EventArgs e)
        {
            string res = "";
            string respwd = string.Empty;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 4);
            cmd.Parameters.AddWithValue("@ChallanType", drpChallanType.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@SendToWorkShop", (rdbSendToWorkShopTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@ReceiveWorkShop", (rdbReceiveTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@MarkDelivery", (rdbMarkDeliveryTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@WorkShopRecieve", (rdbWorkRecieveTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@WorkShopReady", (rdbWorkReadyTrue.Checked ? "true" : "false"));
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            res = AppClass.ExecuteNonQuery(cmd);
            //if (res == "Record Saved")
            //{
            //    generalSuccess.Text = "Record save sucessfully";
            //}
            //----------------------------------------------------------------
          
            SqlCommand cmdPwd = new SqlCommand();
            cmdPwd.CommandText = "Sp_InsUpd_ConfigSettings";
            cmdPwd.CommandType = CommandType.StoredProcedure;
            if (rdbItemCreationTrue.Checked)
                cmdPwd.Parameters.AddWithValue("@ItemCreationPassword", txtPasswordItemCreation.Text);
            else
                cmdPwd.Parameters.AddWithValue("@ItemCreationPassword", "");
            if (rdbRateChangeTrue.Checked)
                cmdPwd.Parameters.AddWithValue("@RateChangePassword", txtRateChange.Text);
            else
                cmdPwd.Parameters.AddWithValue("@RateChangePassword", "");
            if (rdbDiscountChangeTrue.Checked)
                cmdPwd.Parameters.AddWithValue("@DiscountChangePassword", txtDiscountChange.Text);
            else
                cmdPwd.Parameters.AddWithValue("@DiscountChangePassword", "");
            if (rdbDiscountDelChangeTrue.Checked)
                cmdPwd.Parameters.AddWithValue("@DiscountDelChangePassword", txtDiscountDelChange.Text);
            else
                cmdPwd.Parameters.AddWithValue("@DiscountDelChangePassword", "");
            cmdPwd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmdPwd.Parameters.AddWithValue("@Flag", 3);
            respwd = AppClass.ExecuteNonQuery(cmdPwd);

            if ((respwd == "Record Saved") && (res == "Record Saved"))
            {
                generalSuccess.Text = "Record save sucessfully";
                DataForDefaultSetting();
            }
            //else
            //{
            //    lblPasswordErr.Text = res.ToString();
            //}
            //----------------------------------------------------

           
        }

        protected void btnSetEmail_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = false;
            tdSetTimeZone.Visible = false;
            tdDefaultSetting.Visible = false;
            tdGeneralSetting.Visible = false;
            tdbackup.Visible = false;
            tdEmail.Visible = true;
            tdServiceTax.Visible = false;
         //   tdPassword.Visible = false;
            if (hdnPrint.Value == "")
            {
                FillLastRecordOnControls("No");
            }
            else
            {
                FillLastRecordOnControls("Yes");
            }
            txtHostName.Focus();
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 5);
            cmd.Parameters.AddWithValue("@HostName", txtHostName.Text);
            cmd.Parameters.AddWithValue("@BranchEmail", txtBranchEmail.Text);
            cmd.Parameters.AddWithValue("@EmailId", txtStatusEmailID.Text);
            cmd.Parameters.AddWithValue("@SSL", (chkSSL.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@IsEmailActive", (chkmailActive.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@Port", drpPort.SelectedItem.Text);

            if (txtBranchPassword.Text != "")
                cmd.Parameters.AddWithValue("@BranchPassword", txtBranchPassword.Text);
            res = AppClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowEmailMsg('Green', '#999999');", true);
                lblEmailSuccess.Text = "Email configuration updated successfully.";
            }

            //string res = "";
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "sp_ReceiptConfigSetting";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            //cmd.Parameters.AddWithValue("@Flag", 5);
            //cmd.Parameters.AddWithValue("@HostName", txtHostName.Text);
            //cmd.Parameters.AddWithValue("@BranchEmail", txtBranchEmail.Text);
            //cmd.Parameters.AddWithValue("@SSL", (chkSSL.Checked ? "1" : "0"));
            //if (txtBranchPassword.Text != "")
            //    cmd.Parameters.AddWithValue("@BranchPassword", txtBranchPassword.Text);
            //res = AppClass.ExecuteNonQuery(cmd);
            //if (res == "Record Saved")
            //{
            //    lblEmailSuccess.Text = "Record save sucessfully";
            //    XmlDocument xDoc = new XmlDocument();
            //    xDoc.Load(HttpContext.Current.Server.MapPath("~/web.config"));
            //    XmlElement root = xDoc.DocumentElement;
            //    XmlNodeList connList = root.SelectNodes("//smtp");
            //    XmlNodeList cList = root.SelectNodes("//network");
            //    XmlElement elem, elem1;
            //    foreach (XmlNode node in connList)
            //    {
            //        elem = (XmlElement)node;
            //        elem.SetAttribute("deliveryMethod", "Network");
            //        elem.SetAttribute("from", txtBranchEmail.Text);
            //    }
            //    foreach (XmlNode node1 in cList)
            //    {
            //        elem1 = (XmlElement)node1;
            //        elem1.SetAttribute("host", txtHostName.Text);
            //        elem1.SetAttribute("userName", txtBranchEmail.Text);
            //        if (txtBranchPassword.Text == "")
            //            elem1.SetAttribute("password", hdnPassword.Value);
            //        else
            //            elem1.SetAttribute("password", hdnPassword.Value);
            //        elem1.SetAttribute("port", "587");
            //    }

            //    string path = Server.MapPath("~/web.config");
            //    xDoc.Save(path);
            //}
        }

        protected void btnBackUp_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = false;
            tdSetTimeZone.Visible = false;
            tdDefaultSetting.Visible = false;
            tdGeneralSetting.Visible = false;
            tdEmail.Visible = false;
            tdbackup.Visible = true;
            tdServiceTax.Visible = false;
          //  tdPassword.Visible = false;
            BindDropDown();
            // Drpdrive_SelectedIndexChanged(null, null);
            fetchdrivepath();
        }

        private void fetchdrivepath()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            PrjClass.SetItemInDropDown(Drpdrive, ds.Tables[0].Rows[0]["backupdrive"].ToString(), true, false);
            // PrjClass.SetItemInDropDown(Drpfloder, ds.Tables[0].Rows[0]["backuppath"].ToString(), true, false);
        }

        protected void btbackupsave_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@backupdrive", Drpdrive.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@backuppath", "backup");
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 10);
            string res = PrjClass.ExecuteNonQuery(cmd);
            lbpathsucess.Text = res;
        }

        protected void btnServiceTaxSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_InsUpd_ConfigSettings";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ServiceTaxText1", txtServiceTaxText1.Text);
            cmd.Parameters.AddWithValue("@ServiceTaxText2", txtServiceText2.Text);
            cmd.Parameters.AddWithValue("@ServiceTaxText3", txtServiceText3.Text);
            cmd.Parameters.AddWithValue("@ServiceTaxRate1", txtServiceTaxRate1.Text);
            cmd.Parameters.AddWithValue("@ServiceTaxRate2", txtServiceTaxRate2.Text);
            cmd.Parameters.AddWithValue("@ServiceTaxRate3", txtServiceTaxRate3.Text);
            cmd.Parameters.AddWithValue("@TaxCalCulation", (rdbTaxBefore.Checked ? "1" : "0"));
            cmd.Parameters.AddWithValue("@InclusiveExclusive", drpServiceTaxType.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", "2");
            res = AppClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowServiceTaxMsg('Green', '#999999');", true);
                lblSucessServiceTax.Text = "Tax configuration updated successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ShowServiceTaxMsg('Red', '#999999');", true);
                lblErrServiceTax.Text = res.ToString();
            }
        }

        protected void btnServiceTaxReset_Click(object sender, EventArgs e)
        {
            txtServiceTaxRate1.Text = "";
            txtServiceTaxRate2.Text = "";
            txtServiceTaxRate3.Text = "";
            txtServiceTaxText1.Text = "";
            txtServiceText2.Text = "";
            txtServiceText3.Text = "";
        }

        protected void btnServiceTac_Click(object sender, EventArgs e)
        {
            tdReceipt.Visible = false;
            tdDisplay.Visible = false;
            tdSetTimeZone.Visible = false;
            tdDefaultSetting.Visible = false;
            tdGeneralSetting.Visible = false;
            tdEmail.Visible = false;
            tdbackup.Visible = false;
            tdServiceTax.Visible = true;
            DataForDefaultSetting();
        //    tdPassword.Visible = false;
        }
        //protected void chkmailActive_CheckChanged(object sender, EventArgs e)
        //{
        //    if (chkmailActive.Checked)
        //    {
        //        txtStatusEmailID.Enabled = true;
        //        txtStatusEmailID.Focus();
        //    }
        //    else
        //    {
        //        txtStatusEmailID.Enabled = false;
        //    }
        //}

        protected void btnPasswordSave_Click(object sender, EventArgs e)
        {
            //string res = string.Empty;
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "Sp_InsUpd_ConfigSettings";
            //cmd.CommandType = CommandType.StoredProcedure;
            //if (rdbItemCreationTrue.Checked)
            //    cmd.Parameters.AddWithValue("@ItemCreationPassword", txtPasswordItemCreation.Text);
            //else
            //    cmd.Parameters.AddWithValue("@ItemCreationPassword", "");
            //if (rdbRateChangeTrue.Checked)
            //    cmd.Parameters.AddWithValue("@RateChangePassword", txtRateChange.Text);
            //else
            //    cmd.Parameters.AddWithValue("@RateChangePassword", "");
            //if (rdbDiscountChangeTrue.Checked)
            //    cmd.Parameters.AddWithValue("@DiscountChangePassword", txtDiscountChange.Text);
            //else
            //    cmd.Parameters.AddWithValue("@DiscountChangePassword", "");
            //if (rdbDiscountDelChangeTrue.Checked)
            //    cmd.Parameters.AddWithValue("@DiscountDelChangePassword", txtDiscountDelChange.Text);
            //else
            //    cmd.Parameters.AddWithValue("@DiscountDelChangePassword", "");
            //cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            //cmd.Parameters.AddWithValue("@Flag", 3);
            //res = AppClass.ExecuteNonQuery(cmd);
            //if (res == "Record Saved")
            //{
            //    lblPasswordSucess.Text = "Password saved sucessfully.";
            //    DataForDefaultSetting();
            //}
            //else
            //{
            //    lblPasswordErr.Text = res.ToString();
            //}
        }
     
    }
}