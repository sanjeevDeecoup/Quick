using System;
using System.Data;
using System.Web;
using System.Net;

namespace QuickWeb.Config_Setting
{
    public partial class BarCodeSetting : System.Web.UI.Page
    {
        private DTO.BarCodeSetting Ob1 = new DTO.BarCodeSetting();
        private DTO.Barcodeconfig Ob = new DTO.Barcodeconfig();
        private DTO.BarCodeSetting ob2 = new DTO.BarCodeSetting();
        private DataSet ds = new DataSet();
        public string OldBarCodeWidth = string.Empty;
        public string OldBarCodeHeight = string.Empty;
        public string StrPreview = string.Empty;
        public string strPreviewbarcode = string.Empty;
        public string BarCodeWidth = string.Empty;
        public string BarCodeHeight = string.Empty;
        public string prtName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Ob1.BranchId = Globals.BranchID;
                BAL.BALFactory.Instance.BAL_Barcodesetting.BarCodeFontSize(drpBarCodeFonts, drpBarCodeSize);
                BindPosition();
                BindFont();
                BindFontsize();
                //BAL.BALFactory.Instance.BAL_Barcodesetting.PrinterList(drpPrinterlist);
                FetchAllValue();
                FetchBarCodeSetting();
                drpBlank_SelectedIndexChanged(null, null);
            }
        }

        public void BindPosition()
        {
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpBarCodePosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpBookingPosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpCustomerPosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(DrpAddressPosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpProcessPosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpRemarkPosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpDueDatePosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpItemNamePosition);
            BAL.BALFactory.Instance.BAL_Barcodesetting.position(drpBDatePosition);
        }

        public void BindFont()
        {
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpBookingFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpCustomerFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpAddressFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpProcessFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpRemarkFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpDueDateFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpItemNameFont);
            BAL.BALFactory.Instance.BAL_Barcodesetting.Fontsetting(drpBDateFont);
        }

        public void BindFontsize()
        {
            BAL.BALFactory.Instance.BAL_Barcodesetting.BookingFontSize(drpBookingSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpCustomerSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpAddressSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.ProcessFontSize(drpProcessSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpRemarkSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpDueDateSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpItemNameSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpShopSize);
            BAL.BALFactory.Instance.BAL_Barcodesetting.OthersFontSize(drpBDateSize);
        }

        protected void drpBarCodeFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            BAL.BALFactory.Instance.BAL_Barcodesetting.BarCodeFontSize(drpBarCodeFonts, drpBarCodeSize);
            lblmsg.Text = BAL.BALFactory.Instance.BAL_Barcodesetting.PreviewDemo(lblmsg, drpBookingPosition, drpCustomerPosition, DrpAddressPosition, drpProcessPosition, drpRemarkPosition, drpBarCodePosition, drpDueDatePosition, drpItemNamePosition, drpBDatePosition);
            if (lblmsg.Text != "")
                BarCodeDisplay();
            else
                BarCodeDisplay();
        }

        public void FetchBarCodeSetting()
        {
            ds = BAL.BALFactory.Instance.BAL_Barcodesetting.fetchbarcodeconfig1(Ob1);
            Ob1 = BAL.BALFactory.Instance.BAL_Barcodesetting.OpeningData(ds,Globals.BranchID);
            OldBarCodeWidth = Ob1.OldBarcodeWidth;
            OldBarCodeHeight = Ob1.OldBarcodeHeight;

            strPreviewbarcode = Ob1.StrPreviewBarCode;
        }

        public void FetchAllValue()
        {
            ds = BAL.BALFactory.Instance.BAL_Barcodesetting.fetchbarcodeconfig1(Ob1);
            int autocutter;
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtday.Text = ds.Tables[0].Rows[0]["daysValue"].ToString();
                ChkOneDay.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDueIncrease"].ToString());
                chkWet.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["wet"].ToString());
                autocutter=Convert.ToInt32(ds.Tables[0].Rows[0]["pagebreak"].ToString());
                if (ChkOneDay.Checked == true)
                    txtday.Enabled = true;
                else
                    txtday.Enabled = false;

                if (autocutter == 1)
                    ChkAutoCuttor.Checked = true;
                else
                    ChkAutoCuttor.Checked = false;

                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkBookingBold, ChkBookingItalic, ChkBookingUnderline, ds.Tables[0].Rows[0]["bookingbold"].ToString(), ds.Tables[0].Rows[0]["bookingitilic"].ToString(), ds.Tables[0].Rows[0]["bookingunderline"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkProcessBold, ChkProcessItalic, ChkProcessUnderline, ds.Tables[0].Rows[0]["processbold"].ToString(), ds.Tables[0].Rows[0]["processitalic"].ToString(), ds.Tables[0].Rows[0]["processunderline"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkRemarkBold, ChkRemarkItalic, ChkRemarkUnderline, ds.Tables[0].Rows[0]["remarkbold"].ToString(), ds.Tables[0].Rows[0]["remarkItalic"].ToString(), ds.Tables[0].Rows[0]["remarkunderline"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkDueDateBold, ChkDueDateItalic, ChkDueDateUnderline, ds.Tables[0].Rows[0]["itembold"].ToString(), ds.Tables[0].Rows[0]["itemitalic"].ToString(), ds.Tables[0].Rows[0]["itemunderline"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkItemNameBold, ChkItemNameItalic, ChkItemNameUnderline, ds.Tables[0].Rows[0]["itembold1"].ToString(), ds.Tables[0].Rows[0]["itemitalic1"].ToString(), ds.Tables[0].Rows[0]["itemunderline1"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkCustomerBold, ChkCustomerItalic, ChkCustomerUnderline, ds.Tables[0].Rows[0]["cusbold"].ToString(), ds.Tables[0].Rows[0]["cusitalic"].ToString(), ds.Tables[0].Rows[0]["cusunderline"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkAddressBold, ChkAddressItalic, ChkAddressUnderline, ds.Tables[0].Rows[0]["addbold"].ToString(), ds.Tables[0].Rows[0]["additalic"].ToString(), ds.Tables[0].Rows[0]["addunderline"].ToString());
                BAL.BALFactory.Instance.BAL_Barcodesetting.CheckCheckBox(ChkBDateBold, ChkBDateItalic, ChkBDateUnderline, ds.Tables[0].Rows[0]["BdateBold"].ToString(), ds.Tables[0].Rows[0]["BdateItalic"].ToString(), ds.Tables[0].Rows[0]["BdateUnderline"].ToString());
                FetchRadio(ds);
                FetchDropDownFont(ds);
                FetchDropDownPosition(ds);
            }
        }

        public void FetchRadio(DataSet ds)
        {
            RdoBookingNo1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodebookingno"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoBookingNo1, RdoBookingNo2);

            RdoProcess1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeprocess"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoProcess1, RdoProcess2);

            RdoItemTotal1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodesubtotal"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoItemTotal1, RdoItemTotal2);

            RdoRemark1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcoderemark"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoRemark1, RdoRemark2);

            RdoColour1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodecolour"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoColour1, RdoColour2);

            RdoBarcode1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeprint"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoBarcode1, RdoBarcode2);

            RdoItemName1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeitem"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoItemName1, RdoItemName2);

            RdoDueDate1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeduedate"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoDueDate1, RdoDueDate2);

            RdoDateTime1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodetime"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoDateTime1, RdoDateTime2);

            RdoCustomer1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodecusname"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoCustomer1, RdoCustomer2);

            RdoAddress1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeaddress"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoAddress1, RdoAddress2);

            RdoBDate.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodebookingdate"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoBDate, RdoBDate1);

            RdoShop1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShopOption"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoShop1, RdoShop2);

            RdoLogo1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LogoOption"].ToString());
            BAL.BALFactory.Instance.BAL_Barcodesetting.CheckedRadio(RdoLogo1, RdoLogo2);

            txtShopName.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
        }

        public void FetchDropDownPosition(DataSet ds)
        {
            PrjClass.SetItemInDropDown(drpBookingPosition, ds.Tables[0].Rows[0]["bookingnoposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpCustomerPosition, ds.Tables[0].Rows[0]["cusposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpProcessPosition, ds.Tables[0].Rows[0]["processposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpRemarkPosition, ds.Tables[0].Rows[0]["remarkposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBarCodePosition, ds.Tables[0].Rows[0]["barcodeposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpDueDatePosition, ds.Tables[0].Rows[0]["itemposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpItemNamePosition, ds.Tables[0].Rows[0]["itemposition1"].ToString(), true, false);
            PrjClass.SetItemInDropDown(DrpAddressPosition, ds.Tables[0].Rows[0]["Addressposition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBDatePosition, ds.Tables[0].Rows[0]["BDatePosition"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpHeight, ds.Tables[0].Rows[0]["barcodeheight"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpWidth, ds.Tables[0].Rows[0]["barcodewidth"].ToString(), true, false);
            //PrjClass.SetItemInDropDown(drpPrinterlist, ds.Tables[0].Rows[0]["PrinterName"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBlank, ds.Tables[0].Rows[0]["LogoSize"].ToString(), true, false);
            hdnValue.Value = ds.Tables[0].Rows[0]["PrinterName"].ToString();
        }

        public void FetchDropDownFont(DataSet ds)
        {
            /* Drop Down Booking fill through Database */

            PrjClass.SetItemInDropDown(drpBookingFont, ds.Tables[0].Rows[0]["bookingfont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBookingSize, ds.Tables[0].Rows[0]["bookingsize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBookingAlign, ds.Tables[0].Rows[0]["bookingalign"].ToString(), true, false);

            /* Drop Down Process fill through Database */
            PrjClass.SetItemInDropDown(drpProcessFont, ds.Tables[0].Rows[0]["processfont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpProcessSize, ds.Tables[0].Rows[0]["processsize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpRemarkAlign, ds.Tables[0].Rows[0]["processalign"].ToString(), true, false);

            /* Drop Down Remarks fill through Database */
            PrjClass.SetItemInDropDown(drpRemarkFont, ds.Tables[0].Rows[0]["remarkfont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpRemarkSize, ds.Tables[0].Rows[0]["remarksize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpRemarkAlign, ds.Tables[0].Rows[0]["remarkremarkalign"].ToString(), true, false);

            /* Drop Down BarCode fill through Database */
            PrjClass.SetItemInDropDown(drpBarCodeAlign, ds.Tables[0].Rows[0]["barcodealign"].ToString(), true, false);

            /* Drop Down DueDate fill through Database */
            PrjClass.SetItemInDropDown(drpDueDateFont, ds.Tables[0].Rows[0]["itemfont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpDueDateAlign, ds.Tables[0].Rows[0]["itemalign"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpDueDateSize, ds.Tables[0].Rows[0]["itemsize"].ToString(), true, false);

            /* Drop Down Items fill through Database */
            PrjClass.SetItemInDropDown(drpItemNameFont, ds.Tables[0].Rows[0]["itemfont1"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpItemNameSize, ds.Tables[0].Rows[0]["itemsize1"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpItemNameAlign, ds.Tables[0].Rows[0]["itemalign1"].ToString(), true, false);

            /* Drop Down Customer fill through Database */
            PrjClass.SetItemInDropDown(drpCustomerFont, ds.Tables[0].Rows[0]["cusfont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpCustomerSize, ds.Tables[0].Rows[0]["cussize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpCustomerAlign, ds.Tables[0].Rows[0]["cusalign"].ToString(), true, false);

            /* Drop Down Address fill through Database */
            PrjClass.SetItemInDropDown(drpAddressFont, ds.Tables[0].Rows[0]["addfont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpAddressSize, ds.Tables[0].Rows[0]["addsize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpAddressAlign, ds.Tables[0].Rows[0]["addalign"].ToString(), true, false);

            /* Drop Down BookingDate fill through Database */
            PrjClass.SetItemInDropDown(drpBDateFont, ds.Tables[0].Rows[0]["BDateFont"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBDateSize, ds.Tables[0].Rows[0]["BDateSize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpBDateAlign, ds.Tables[0].Rows[0]["BdateAlign"].ToString(), true, false);

            PrjClass.SetItemInDropDown(drpShopSize, ds.Tables[0].Rows[0]["ShopSize"].ToString(), true, false);
            PrjClass.SetItemInDropDown(drpShopSize, ds.Tables[0].Rows[0]["ShopAlign"].ToString(), true, false);

            PrjClass.SetItemInDropDown(drpBarCodeFonts, ds.Tables[0].Rows[0]["BarCodeFontName"].ToString(), true, false);
            BAL.BALFactory.Instance.BAL_Barcodesetting.BarCodeFontSize(drpBarCodeFonts, drpBarCodeSize);
            PrjClass.SetItemInDropDown(drpBarCodeSize, ds.Tables[0].Rows[0]["BarCodeFontSize"].ToString(), true, false);
        }

        public DTO.BarCodeSetting SetValue()
        {
            Ob1.BranchId = Globals.BranchID;
            Ob1.OptBooking = (RdoBookingNo1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptBookingDate = (RdoBDate.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.BookingFont = drpBookingFont.SelectedItem.ToString();
            Ob1.BookingSize = drpBookingSize.SelectedItem.ToString();
            Ob1.BookingAlign = drpBookingAlign.SelectedItem.ToString();
            Ob1.BookingBold = (ChkBookingBold.Checked ? "Bold" : "");
            Ob1.BookingItalic = (ChkBookingItalic.Checked ? "Italic" : "");
            Ob1.BookingUnderline = (ChkBookingUnderline.Checked ? "Underline" : "");
            Ob1.bdatefont = drpBDateFont.SelectedItem.ToString();
            Ob1.bdatesize = drpBDateSize.SelectedItem.ToString();
            Ob1.bdatealign = drpBDateAlign.SelectedItem.ToString();
            Ob1.bdateposition = drpBDatePosition.SelectedItem.ToString();
            Ob1.bdatebold = (ChkBDateBold.Checked ? "Bold" : "");
            Ob1.bdateitalic = (ChkBDateItalic.Checked ? "Italic" : "");
            Ob1.bdateunderline = (ChkBDateUnderline.Checked ? "Underline" : "");
            Ob1.IsDueIncrease = (ChkOneDay.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptProcess = (RdoProcess1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptSubtotal = (RdoItemTotal1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.ProcessFont = drpProcessFont.SelectedItem.ToString();
            Ob1.ProcessAlign = drpProcessAlign.SelectedItem.ToString();
            Ob1.ProcessSize = drpProcessSize.SelectedItem.ToString();

            Ob1.ProcessBold = (ChkProcessBold.Checked ? "Bold" : "");
            Ob1.ProcessItalic = (ChkProcessItalic.Checked ? "Italic" : "");
            Ob1.ProcessUnderline = (ChkProcessUnderline.Checked ? "Underline" : "");

            Ob1.OptRemark = (RdoRemark1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptColour = (RdoColour1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));

            Ob1.RemarkFont = drpRemarkFont.SelectedItem.ToString();
            Ob1.RemarkAlign = drpRemarkAlign.SelectedItem.ToString();
            Ob1.RemarkSize = drpRemarkSize.SelectedItem.ToString();
            Ob1.RemarkBold = (ChkRemarkBold.Checked ? "Bold" : "");
            Ob1.RemarkItalic = (ChkRemarkItalic.Checked ? "Italic" : "");
            Ob1.RemarkUnderline = (ChkRemarkUnderline.Checked ? "Underline" : "");
            Ob1.OptBarCode = (RdoBarcode1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.BarcodeAlign = drpBarCodeAlign.SelectedItem.ToString();
            Ob1.wet = (chkWet.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            if (Ob1.wet == true)
                Ob1.BarCodeFontName = "IDAutomationHC39M";
            else
                Ob1.BarCodeFontName = drpBarCodeFonts.SelectedItem.ToString();
            Ob1.BarCodeFontSize = drpBarCodeSize.SelectedItem.ToString();
            Ob1.OptDueDate = (RdoDueDate1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptItem = (RdoItemName1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptTime = (RdoDateTime1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));

            Ob1.DueDateFont = drpDueDateFont.SelectedItem.ToString();
            Ob1.DueDateAlign = drpDueDateAlign.SelectedValue.ToString();
            Ob1.DueDateSize = drpDueDateSize.SelectedItem.ToString();
            Ob1.ItemFont = drpItemNameFont.SelectedItem.ToString();
            Ob1.ItemSize = drpItemNameSize.SelectedItem.ToString();
            Ob1.ItemAlign = drpItemNameAlign.SelectedItem.ToString();

            Ob1.DueDateBold = (ChkDueDateBold.Checked ? "Bold" : "");
            Ob1.DueDateItalic = (ChkDueDateItalic.Checked ? "Italic" : "");
            Ob1.DueDateUnderline = (ChkDueDateUnderline.Checked ? "Underline" : "");
            Ob1.ItemBold = (ChkItemNameBold.Checked ? "Bold" : "");
            Ob1.ItemItalic = (ChkItemNameItalic.Checked ? "Italic" : "");
            Ob1.ItemUnderline = (ChkItemNameUnderline.Checked ? "Underline" : "");
            Ob1.OptCustomer = (RdoCustomer1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.CustomerFont = drpCustomerFont.SelectedItem.ToString();
            Ob1.CustomerSize = drpCustomerSize.SelectedItem.ToString();
            Ob1.CustomerAlign = drpCustomerAlign.SelectedItem.ToString();
            Ob1.CustomerBold = (ChkCustomerBold.Checked ? "Bold" : "");
            Ob1.CustomerItalic = (ChkCustomerItalic.Checked ? "Italic" : "");
            Ob1.CustomerUnderline = (ChkCustomerUnderline.Checked ? "Underline" : "");
            Ob1.LogoAlign = drpLogoAlign.SelectedItem.ToString();
            Ob1.LogoPosition = txtLogoPosition.Text;
            Ob1.ShopName = txtShopName.Text;
            Ob1.OptAddress = (RdoAddress1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.AddressFont = drpAddressFont.SelectedItem.ToString();
            Ob1.AddressAlign = drpAddressAlign.SelectedItem.ToString();
            Ob1.AddressSize = drpAddressSize.SelectedItem.ToString();
            Ob1.AddressBold = (ChkAddressBold.Checked ? "Bold" : "");
            Ob1.AddressItalic = (ChkAddressItalic.Checked ? "Italic" : "");
            Ob1.AddressUnderline = (ChkAddressUnderline.Checked ? "Underline" : "");
            Ob1.BookingPosition = drpBookingPosition.SelectedItem.ToString();
            Ob1.CustomerPosition = drpCustomerPosition.SelectedItem.ToString();
            Ob1.ProcessPosition = drpProcessPosition.SelectedItem.ToString();
            Ob1.RemarkPosition = drpRemarkPosition.SelectedItem.ToString();
            Ob1.BarPosition = drpBarCodePosition.SelectedItem.ToString();
            Ob1.DueDatePosition = drpDueDatePosition.SelectedItem.ToString();
            Ob1.ItemPosition = drpItemNamePosition.SelectedItem.ToString();
            Ob1.AddressPosition = DrpAddressPosition.SelectedItem.ToString();
            //string wi=drpWidth.Items.ToString();
            Ob1.BarcodeWidth = drpWidth.SelectedItem.ToString();
            Ob1.BarcodeHeight = drpHeight.SelectedItem.ToString();
            Ob1.StrArray = Ob1.BarcodeHeight.Split('i');
            Ob1.LineHeight = Math.Round(Convert.ToDouble(Ob1.StrArray[0]) / 8, 2);

            Ob1.Barcodeheight = Convert.ToDouble(Ob1.StrArray[0]) - 0.5;
            Ob1.DivHeight = Math.Round(Ob1.Barcodeheight / 7, 2);
            Ob1.OptLogo = (RdoLogo1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.OptShopOption = (RdoShop1.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            Ob1.ShopName = txtShopName.Text;
            //Ob1.LogoAlign = drpLogoAlign.SelectedItem.ToString();
            Ob1.ShopSize = drpShopSize.SelectedItem.ToString();
            Ob1.ShopAlign = drpShopAlign.SelectedItem.ToString();
            Ob1.LogoSize = drpBlank.SelectedItem.ToString();
            Ob1.daysValue = (ChkOneDay.Checked ? Convert.ToInt32(txtday.Text) : 0);
            Ob1.pagebreak = (ChkAutoCuttor.Checked ? 1 : 0);

            if (Ob1.wet == true)
            {
                Ob1.OptBooking = false;
                Ob1.OptBookingDate = false;
                Ob1.OptProcess = false;
                Ob1.OptSubtotal = false;
                Ob1.OptRemark = false;
                Ob1.OptColour = false;
                Ob1.OptBarCode = false;
                Ob1.OptDueDate = false;
                Ob1.OptCustomer = false;
                Ob1.OptAddress = false;
                Ob1.OptLogo = false;
                Ob1.OptShopOption = false;
            }
            if (hdnPrint.Value != "")
            {
                Ob1.PrinterName = hdnPrint.Value;
            }
            else
            {
                Ob1.PrinterName = "";
            }

            return Ob1;
        }

        public void BarCodeDisplay()
        {
            ob2 = SetValue();
            Ob1 = BAL.BALFactory.Instance.BAL_Barcodesetting.DemoBarCodeDisplay(ob2);

            strPreviewbarcode = Ob1.StrPreview;
            OldBarCodeWidth = Ob1.BarcodeWidth;
            OldBarCodeHeight = Ob1.BarcodeHeight;
            //FetchBarCodeSetting();
        }

        //protected void btnSet_Click(object sender, EventArgs e)
        //{
        //    BarCodeDisplay();
        //}

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            lblmsg.Text = BAL.BALFactory.Instance.BAL_Barcodesetting.PreviewDemo(lblmsg, drpBookingPosition, drpCustomerPosition, DrpAddressPosition, drpProcessPosition, drpRemarkPosition, drpBarCodePosition, drpDueDatePosition, drpItemNamePosition, drpBDatePosition);
            if (lblmsg.Text != "")
                BarCodeDisplay();
            else
                BarCodeDisplay();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            if (ChkOneDay.Checked)
            {
                if (txtday.Text == "" || txtday.Text == "0")
                {
                    res = "Please Enter any value greater than ZERO";
                    lblmsg.Text = res;
                    txtday.Focus();
                    return;
                }
            }
            res = BAL.BALFactory.Instance.BAL_Barcodesetting.PreviewDemo(lblmsg, drpBookingPosition, drpCustomerPosition, DrpAddressPosition, drpProcessPosition, drpRemarkPosition, drpBarCodePosition, drpDueDatePosition, drpItemNamePosition, drpBDatePosition);
            if (lblmsg.Text != "")
            {
                lblmsg.Text = res;
            }
            else
            {
                ob2 = SetValue();

                res = BAL.BALFactory.Instance.BAL_Barcodesetting.Updatebarcodewidthheight(ob2);
                res = BAL.BALFactory.Instance.BAL_Barcodesetting.Updatebarcodeconfig(ob2);
                if (res == "Record Saved")
                {
                    Ob1 = BAL.BALFactory.Instance.BAL_Barcodesetting.DemoBarCodeDisplay(ob2);
                    FetchBarCodeSetting();
                    lblmsg.Text = res;
                    hdnValue.Value = ds.Tables[0].Rows[0]["PrinterName"].ToString();
                }
                else
                {
                    lblmsg.Text = res;
                }
            }
        }

        protected void drpBlank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBlank.SelectedIndex == 1)
            {
                RdoShop1.Enabled = false;
                RdoShop2.Enabled = false;
                txtShopName.Enabled = false;
                drpShopSize.Enabled = false;
                drpShopAlign.Enabled = false;
                txtposition.Enabled = false;
                RdoShop2.Checked = true;
            }
            else
            {
                RdoShop1.Enabled = true;
                RdoShop2.Enabled = true;
                txtShopName.Enabled = true;
                drpShopSize.Enabled = true;
                drpShopAlign.Enabled = true;
            }
        }

        protected void ChkOneDay_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkOneDay.Checked)
            {
                txtday.Enabled = true;
                txtday.Focus();
            }
            else
            {
                txtday.Enabled = false;
                txtday.Text = "0";
            }
        }
        protected void btnFont_Click(object sender, EventArgs e)
        {
            string strURL = Server.MapPath("~/FONT/C39HrP36DmTt.ttf");
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.ContentType = "application/ttf";
            response.AddHeader("Content-Disposition", "attachment;filename=\"C39HrP36DmTt.ttf\"");
            byte[] data = req.DownloadData(strURL);
            response.BinaryWrite(data);
            response.End();
        }
    }
}