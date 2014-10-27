using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Web.UI.WebControls;

namespace DAL
{
    public class DAL_BarCodeSetting
    {
        private DTO.BarCodeSetting Ob = new DTO.BarCodeSetting();
      

        public void position(DropDownList drp)
        {
            Ob.BarCodePosition = 9;

            for (Ob.Loopi = 1; Ob.Loopi <= Ob.BarCodePosition; Ob.Loopi++)
            {
                drp.Items.Add(Ob.Loopi.ToString());
            }
        }

        public void Fontsetting(DropDownList drp)
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily font in fonts.Families)
            {
                drp.Items.Add(font.Name);
            }
        }

        public void BarCodeFontSize(DropDownList drp, DropDownList drp1)
        {
            drp1.Items.Clear();
            switch (drp.SelectedIndex)
            {
                //case 0:

                //    drp1.Items.Add("12");
                //    drp1.Items.Add("13");

                //    break;

                //case 1:

                //    drp1.Items.Add("15");
                //    break;

                //default:
                case 0:                    
                    drp1.Items.Add("26");
                    drp1.Items.Add("28");
                    drp1.Items.Add("30");
                    drp1.Items.Add("32");
                    drp1.Items.Add("34");
                    drp1.Items.Add("36");
                    drp1.Items.Add("38");
                    drp1.Items.Add("40");
                    drp1.Items.Add("42");
                    break;
            }
        }

        public void BookingFontSize(DropDownList drp)
        {
            for (Ob.Loopi = 36; Ob.Loopi >= 5; Ob.Loopi--)
            {
                drp.Items.Add(Ob.Loopi.ToString());
            }
        }

        public void ProcessFontSize(DropDownList drp)
        {
            for (Ob.Loopi = 32; Ob.Loopi >= 5; Ob.Loopi--)
            {
                drp.SelectedValue = "8";
                drp.Items.Add(Ob.Loopi.ToString());
            }
        }

        public void OthersFontSize(DropDownList drp)
        {
            for (Ob.Loopi = 18; Ob.Loopi >= 5; Ob.Loopi--)
            {
                drp.SelectedValue = "8";
                drp.Items.Add(Ob.Loopi.ToString());
            }
        }

        public void PrinterList(DropDownList drp)
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                drp.Items.Add(printer);
            }
        }

        public void CheckedRadio(RadioButton rdo, RadioButton rdo1)
        {
            if (rdo.Checked != true)
                rdo1.Checked = true;

            else
                rdo.Checked = true;
        }

        public void CheckCheckBox(CheckBox chk1, CheckBox chk2, CheckBox chk3, string Bold, string Italic, string Underline)
        {
            if (Bold != "")
                chk1.Checked = true;
            if (Italic != "")
                chk2.Checked = true;
            if (Underline != "")
                chk3.Checked = true;
        }

        public DataSet fetchbarcodeconfig1(DTO.BarCodeSetting Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 12);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DTO.BarCodeSetting OpeningData(DataSet ds1,string BID)
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                Ob.bdatebold = ds1.Tables[0].Rows[0]["BDateBold"].ToString();
                Ob.bdatefont = ds1.Tables[0].Rows[0]["BDateFont"].ToString();
                Ob.bdateitalic = ds1.Tables[0].Rows[0]["BDateItalic"].ToString();
                Ob.bdatealign = ds1.Tables[0].Rows[0]["BDateAlign"].ToString();
                Ob.bdatesize = ds1.Tables[0].Rows[0]["BDateSize"].ToString();
                Ob.bdateunderline = ds1.Tables[0].Rows[0]["BDateunderline"].ToString();
                Ob.bdateposition = ds1.Tables[0].Rows[0]["BDateposition"].ToString();

                Ob.OptBooking = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodebookingno"].ToString());
                Ob.BookingFont = ds1.Tables[0].Rows[0]["bookingfont"].ToString();
                Ob.BookingSize = ds1.Tables[0].Rows[0]["bookingsize"].ToString();
                Ob.BookingAlign = ds1.Tables[0].Rows[0]["bookingalign"].ToString();
                Ob.BookingBold = ds1.Tables[0].Rows[0]["bookingbold"].ToString();
                Ob.BookingItalic = ds1.Tables[0].Rows[0]["bookingitilic"].ToString();
                Ob.BookingUnderline = ds1.Tables[0].Rows[0]["bookingunderline"].ToString();
                Ob.OptProcess = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeprocess"].ToString());
                Ob.OptExtraProcess = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodexteraprocess"].ToString());
                Ob.OptExtraProcessSecond = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeextraprocesssecond"].ToString());
                Ob.OptSubtotal = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodesubtotal"].ToString());
                Ob.ProcessFont = ds1.Tables[0].Rows[0]["processfont"].ToString();
                Ob.ProcessSize = ds1.Tables[0].Rows[0]["processsize"].ToString();
                Ob.ProcessAlign = ds1.Tables[0].Rows[0]["processalign"].ToString();
                Ob.ProcessBold = ds1.Tables[0].Rows[0]["processbold"].ToString();
                Ob.ProcessItalic = ds1.Tables[0].Rows[0]["processitalic"].ToString();
                Ob.ProcessUnderline = ds1.Tables[0].Rows[0]["processunderline"].ToString();
                Ob.OptRemark = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcoderemark"].ToString());
                Ob.OptColour = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodecolour"].ToString());
                Ob.RemarkFont = ds1.Tables[0].Rows[0]["remarkfont"].ToString();
                Ob.RemarkSize = ds1.Tables[0].Rows[0]["remarksize"].ToString();
                Ob.RemarkAlign = ds1.Tables[0].Rows[0]["remarkremarkalign"].ToString();
                Ob.RemarkBold = ds1.Tables[0].Rows[0]["remarkbold"].ToString();
                Ob.RemarkItalic = ds1.Tables[0].Rows[0]["remarkitalic"].ToString();
                Ob.RemarkUnderline = ds1.Tables[0].Rows[0]["remarkunderline"].ToString();
                Ob.OptPrint = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeprint"].ToString());

                Ob.BarcodeAlign = ds1.Tables[0].Rows[0]["barcodealign"].ToString();
                Ob.BarCodeFontName = ds1.Tables[0].Rows[0]["BarCodeFontName"].ToString();
                Ob.BarCodeFontSize = ds1.Tables[0].Rows[0]["BarCodeFontSize"].ToString();
                Ob.OptItem = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeitem"].ToString());
                Ob.OptLogo = Convert.ToBoolean(ds1.Tables[0].Rows[0]["LogoOption"].ToString());
                Ob.OptShopOption = Convert.ToBoolean(ds1.Tables[0].Rows[0]["ShopOption"].ToString());
                Ob.OptDueDate = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeduedate"].ToString());
                Ob.OptTime = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodetime"].ToString());
                Ob.DueDateFont = ds1.Tables[0].Rows[0]["itemfont"].ToString();
                Ob.ItemFont = ds1.Tables[0].Rows[0]["itemfont1"].ToString();
                Ob.DueDateSize = ds1.Tables[0].Rows[0]["itemsize"].ToString();
                Ob.ItemSize = ds1.Tables[0].Rows[0]["itemsize1"].ToString();
                Ob.DueDateBold = ds1.Tables[0].Rows[0]["itembold"].ToString();
                Ob.ItemBold = ds1.Tables[0].Rows[0]["itembold1"].ToString();
                Ob.DueDateItalic = ds1.Tables[0].Rows[0]["itemitalic"].ToString();
                Ob.ItemItalic = ds1.Tables[0].Rows[0]["itemitalic1"].ToString();

                Ob.DueDateAlign = ds1.Tables[0].Rows[0]["itemalign"].ToString();
                Ob.ItemAlign = ds1.Tables[0].Rows[0]["itemalign1"].ToString();
                Ob.DueDateUnderline = ds1.Tables[0].Rows[0]["itemunderline"].ToString();
                Ob.ItemUnderline = ds1.Tables[0].Rows[0]["itemunderline1"].ToString();

                Ob.OptCustomer = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodecusname"].ToString());
                Ob.CustomerFont = ds1.Tables[0].Rows[0]["cusfont"].ToString();
                Ob.CustomerSize = ds1.Tables[0].Rows[0]["cussize"].ToString();
                Ob.CustomerAlign = ds1.Tables[0].Rows[0]["cusalign"].ToString();
                Ob.CustomerBold = ds1.Tables[0].Rows[0]["cusbold"].ToString();
                Ob.CustomerItalic = ds1.Tables[0].Rows[0]["cusitalic"].ToString();
                Ob.CustomerUnderline = ds1.Tables[0].Rows[0]["cusunderline"].ToString();

                Ob.BookingPosition = ds1.Tables[0].Rows[0]["bookingnoposition"].ToString();
                Ob.CustomerPosition = ds1.Tables[0].Rows[0]["cusposition"].ToString();
                Ob.ProcessPosition = ds1.Tables[0].Rows[0]["processposition"].ToString();
                Ob.RemarkPosition = ds1.Tables[0].Rows[0]["remarkposition"].ToString();
                Ob.BarPosition = ds1.Tables[0].Rows[0]["barcodeposition"].ToString();
                Ob.LogoPosition = ds1.Tables[0].Rows[0]["LogoPosition"].ToString();
                Ob.LogoAlign = ds1.Tables[0].Rows[0]["LogoAlign"].ToString();
                Ob.LogoUrl = ds1.Tables[0].Rows[0]["LogoUrl"].ToString();
                // This Variabale does not available in table
                Ob.ShopAlign = ds1.Tables[0].Rows[0]["ShopAlign"].ToString();
                Ob.ShopSize = ds1.Tables[0].Rows[0]["ShopSize"].ToString();

                Ob.DueDatePosition = ds1.Tables[0].Rows[0]["itemposition"].ToString();
                Ob.ItemPosition = ds1.Tables[0].Rows[0]["itemposition1"].ToString();

                Ob.AddressPosition = ds1.Tables[0].Rows[0]["Addressposition"].ToString();
                Ob.OptAddress = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeaddress"].ToString());
                Ob.AddressFont = ds1.Tables[0].Rows[0]["addfont"].ToString();
                Ob.AddressSize = ds1.Tables[0].Rows[0]["addsize"].ToString();
                Ob.AddressAlign = ds1.Tables[0].Rows[0]["addalign"].ToString();
                Ob.AddressBold = ds1.Tables[0].Rows[0]["addbold"].ToString();
                Ob.AddressItalic = ds1.Tables[0].Rows[0]["additalic"].ToString();
                Ob.AddressUnderline = ds1.Tables[0].Rows[0]["addunderline"].ToString();
                Ob.ShopName = ds1.Tables[0].Rows[0]["ShopName"].ToString();
                Ob.IsDueIncrease = Convert.ToBoolean(ds1.Tables[0].Rows[0]["IsDueIncrease"].ToString());
                Ob.OldBarcodeWidth = ds1.Tables[0].Rows[0]["barcodewidth"].ToString();
                Ob.OldBarcodeHeight = ds1.Tables[0].Rows[0]["barcodeheight"].ToString();
                Ob.LogoSize = ds1.Tables[0].Rows[0]["LogoSize"].ToString();
                Ob.daysValue = Convert.ToInt32(ds1.Tables[0].Rows[0]["daysValue"].ToString());
                Ob.wet = Convert.ToBoolean(ds1.Tables[0].Rows[0]["wet"].ToString());
                Ob.StrArray = Ob.OldBarcodeHeight.Split('i');
                Ob.Barcodeheight = Convert.ToDouble(Ob.StrArray[0]) - 0.5;
                Ob.DivHeight = Math.Round(Ob.Barcodeheight / 7, 2);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_rpt_barcodprint";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 6);
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.Connection = sqlcon;
                da.SelectCommand = cmd;
                da.Fill(ds);

                if (Ob.wet == true)
                {
                    //Ob.StrMainBarCode = "width: 100%; margin-left: 2px; background-position:center;float: left;padding: 0px 1px 0px 1px;";

                    Ob.child1 = "float:left;width:100%;height:1.5in;";
                    //Ob.child2 = "float:right;width:40%;height:2in";
                }
                else
                {
                    Ob.StrMainBarCode = "width: 32%; height: 32px; background-image: url(../Logo/Dry"+BID+".jpg);background-repeat: no-repeat; margin-left: 2px; background-position:center;float: left;text-align:" + Ob.LogoAlign + ";padding: 0px 1px 0px 1px;";
                }
                Ob.StrPreviewBarCode = "";

                Ob.ShopDiv = "width: 60%; float: left; overflow: hidden; font-size:" + Ob.ShopSize + "px; text-align:" + Ob.ShopAlign + ";vertical-align: middle; padding: 0px 1px 0px 1px;";

                Ob.BookingNo = "width: 100%; height: ; font-family:" + Ob.BookingFont + "; font-size:" + Ob.BookingSize + "px; font-weight:" + Ob.BookingBold + ";font-style:" + Ob.BookingItalic + "; text-decoration:" + Ob.BookingUnderline + "; text-align:" + Ob.BookingAlign + ";margin-bottom:-3%;";

                Ob.Customer = "width: 100%; height: ; font-family:" + Ob.CustomerFont + "; font-size:" + Ob.CustomerSize + "px; font-weight:" + Ob.CustomerBold + ";font-style:" + Ob.CustomerItalic + "; text-decoration:" + Ob.CustomerUnderline + "; text-align:" + Ob.CustomerAlign + "; text-transform: capitalize";

                Ob.Address = "width: 100%; height: ; font-family:" + Ob.AddressFont + "; font-size:" + Ob.AddressSize + "px; font-weight:" + Ob.AddressBold + ";font-style:" + Ob.AddressItalic + "; text-decoration:" + Ob.AddressUnderline + "; text-align:" + Ob.AddressAlign + ";";

                Ob.CustName = "width: 100%; height: ; font-family:" + Ob.ProcessFont + "; font-size:" + Ob.ProcessSize + "px; font-weight:" + Ob.ProcessBold + ";font-style:" + Ob.ProcessItalic + "; text-decoration:" + Ob.ProcessUnderline + "; text-align:" + Ob.ProcessAlign + ";";

                Ob.BarCode = "width: 100%; height: ; font-family:" + Ob.BarCodeFontName + "; font-size:" + Ob.BarCodeFontSize + "px;text-align:" + Ob.BarcodeAlign + ";";
                Ob.bookingdate = "width: 100%; height: ; font-family:" + Ob.bdatefont + "; font-size:" + Ob.bdatesize + "px;text-align:" + Ob.bdatealign + ";";
                Ob.Time = "width: 100%; height: ; font-family:" + Ob.DueDateFont + "; font-size:" + Ob.DueDateSize + "px; font-weight:" + Ob.DueDateBold + ";font-style:" + Ob.DueDateItalic + "; text-decoration:" + Ob.DueDateUnderline + "; text-align:" + Ob.DueDateAlign + ";";
                Ob.Item = "width: 100%; height: ; font-family:" + Ob.ItemFont + "; font-size:" + Ob.ItemSize + "px; font-weight:" + Ob.ItemBold + ";font-style:" + Ob.ItemItalic + "; text-decoration:" + Ob.ItemUnderline + "; text-align:" + Ob.ItemAlign + ";margin-top:-2%;";
                Ob.Date = "width: 100%; height: ; font-family:" + Ob.DueDateFont + "; font-size:" + Ob.DueDateSize + "px; font-weight:" + Ob.DueDateBold + ";font-style:" + Ob.DueDateItalic + "; text-decoration:" + Ob.DueDateUnderline + "; text-align:" + Ob.DueDateAlign + ";margin-top:-2%;";
                Ob.Remark = "width: 100%; height: ; font-family:" + Ob.RemarkFont + "; font-size:" + Ob.RemarkSize + "px; font-weight:" + Ob.RemarkBold + ";font-style:" + Ob.RemarkItalic + "; text-decoration:" + Ob.RemarkUnderline + "; text-align:" + Ob.RemarkAlign + ";";

                Ob.StrPreview += "<div style='" + Ob.StrMainBarCode + "'></div>";
                if (Ob.wet == true)
                {
                    DateTime tempduedate, tempBookingdate;

                    Ob.BookingNo = "width: 100%;margin-top:1px;font-family:Times New Roman;font-weight:bold;font-size:28px;text-align:left;padding-left:0.2in;";
                    Ob.Customer = "width: 100%;margin-top:1px;font-family:Arial;font-size:16px;text-align:left;text-transform: capitalize;padding-left:0.2in;";
                    Ob.Date = "width: 100%;margin-top:1px;font-family:Arial;font-size:12px;text-align:left;padding-left:0.2in;";
                    //Ob.BarCode = "width: 100%;Margin-Top:10px;vertical-aligh:middle; font-family:" + Ob.BarCodeFontName + ";font-size:16px;-webkit-transform:rotate(90deg);-moz-transform:rotate(90deg);-o-transform: rotate(90deg);filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);";
                    Ob.Item = "width: 100%;margin-top:1px;font-family:Arial;font-size:18px;text-align:left;padding-left:0.2in;";
                    Ob.StrPreviewBarCode += "<div style='" + Ob.child1 + "'>";
                    tempduedate = Convert.ToDateTime(ds.Tables[0].Rows[Ob.Loopi]["DueDate"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    tempBookingdate = Convert.ToDateTime(ds.Tables[0].Rows[Ob.Loopi]["BookingDate"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    Ob.StrPreviewBarCode += "<div style='" + Ob.BookingNo + "' >" + ds.Tables[0].Rows[0]["BookingNo"].ToString() + " " + tempduedate.ToString("ddd, d MMM") + "</div>";

                    Ob.StrPreviewBarCode += "<div style='" + Ob.Customer + "' >" + ds.Tables[0].Rows[0]["CustomerName"].ToString() + "</div>";

                    Ob.StrPreviewBarCode += "<div style='" + Ob.Item + "'>" + ds.Tables[0].Rows[0]["Item"].ToString() + "  " + ds.Tables[0].Rows[0]["ItemTotalAndSubTotal"].ToString() + "  " + ds.Tables[0].Rows[0]["Process"].ToString() + "  " + tempBookingdate.ToString("d MMM") + "</div>";
                    Ob.StrPreviewBarCode += "<div style='" + Ob.Date + "'>" + ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() + "</div></div>";

                    ///* Child Div */
                    //Ob.StrPreviewBarCode += "<div style='" + Ob.child2 + "'>";
                    //Ob.StrPreviewBarCode += "<div style='" + Ob.BarCode + "'>" + ds.Tables[0].Rows[0]["Barcode"].ToString() + "</div></div>";
                }
                else
                {
                    for (Ob.Loopi = 0; Ob.Loopi < ds.Tables[0].Rows.Count; Ob.Loopi++)
                    {
                        if (Ob.LogoSize == "Full")
                        {
                            Ob.StrMainBarCode = "width: 90%; height: 32px; background-image: url(../Logo/Dry" + BID + ".jpg);background-repeat: no-repeat; margin-left: 2px; background-position:center;float: left;text-align:" + Ob.LogoAlign + ";padding: 0px 1px 0px 1px;";
                            Ob.StrPreviewBarCode += "<div style='" + Ob.StrMainBarCode + "'></div>";
                        }
                        else
                        {
                            if (Ob.LogoAlign == "Left")
                            {
                                if (Ob.OptLogo == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.StrMainBarCode + "'></div>";
                                }
                                if (Ob.OptShopOption == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.ShopDiv + "'>" + Ob.ShopName + "</div>";
                                }
                            }
                            else
                            {
                                if (Ob.OptShopOption == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.ShopDiv + "'>" + Ob.ShopName + "</div>";
                                }
                                else
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.ShopDiv + "'>&nbsp;</div>";
                                }
                                if (Ob.OptLogo == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.StrMainBarCode + "'></div>";
                                }
                            }
                        }

                        for (Ob.Loopj = 1; Ob.Loopj <= 9; Ob.Loopj++)
                        {
                            if (Ob.Loopj == Int32.Parse(Ob.BookingPosition.ToString()))
                            {
                                if (Ob.OptBooking == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.BookingNo + "' >" + ds.Tables[0].Rows[Ob.Loopi]["BookingNo"].ToString() + "</div>";
                                }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.CustomerPosition.ToString()))
                            {
                                if (Ob.OptCustomer == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Customer + "' >" + ds.Tables[0].Rows[Ob.Loopi]["CustomerName"].ToString() + "</div>";
                                }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.AddressPosition.ToString()))
                            {
                                if (Ob.OptAddress == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Address + "' >" + ds.Tables[0].Rows[Ob.Loopi]["CustomerAddress"].ToString() + "</div>";
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.ProcessPosition.ToString()))
                            {
                                if (Ob.OptProcess == true)
                                {
                                    Ob.Process = ds.Tables[0].Rows[Ob.Loopi]["Process"].ToString();
                                }

                                if (Ob.OptExtraProcess == true)
                                {
                                    Ob.ExtProcess = ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType"].ToString();
                                }

                                if (Ob.OptExtraProcessSecond == true)
                                {
                                    Ob.ExtProcessSecond = ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType2"].ToString();
                                }

                                if (Ob.OptSubtotal == true)
                                {
                                    Ob.OptSubtotal1 = ds.Tables[0].Rows[Ob.Loopi]["ItemTotalAndSubTotal"].ToString();
                                }

                                if (Ob.OptProcess == true || Ob.OptExtraProcess == true || Ob.OptExtraProcessSecond == true || Ob.OptSubtotal == true)
                                {
                                    if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType"].ToString() != "None")
                                    {
                                        if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType2"].ToString() != "None")
                                            Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + Ob.Process + "" + Ob.ExtProcess + "" + Ob.ExtProcessSecond + "" + Ob.OptSubtotal1 + "</div>";
                                        else
                                            Ob.StrPreview += "<div  style='" + Ob.CustName + "' >" + Ob.Process + "" + Ob.ExtProcess + "" + Ob.OptSubtotal1 + "</div>";
                                    }
                                    else if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType2"].ToString() != "None")
                                    {
                                        if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType"].ToString() == "None")
                                            Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + Ob.Process + "" + Ob.ExtProcessSecond + "" + Ob.OptSubtotal1 + "</div>";
                                    }
                                    else
                                        Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + Ob.Process + "" + Ob.OptSubtotal1 + "</div>";
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.BarPosition.ToString()))
                            {
                                if (Ob.OptPrint == true)
                                {                                  
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.BarCode + "'>" + ds.Tables[0].Rows[Ob.Loopi]["Barcode"].ToString() + "</div>";
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.ItemPosition.ToString()))
                            {
                                if (Ob.OptItem == true)
                                {
                                    Ob.ItemName = ds.Tables[0].Rows[Ob.Loopi]["Item"].ToString();
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Item + "'>" + Ob.ItemName + "</div>";
                                }

                                else
                                { }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.DueDatePosition.ToString()))
                            {
                                if (Ob.OptDueDate == true)
                                {
                                    Ob.DueDate = ds.Tables[0].Rows[Ob.Loopi]["DueDate"].ToString();
                                }

                                if (Ob.OptTime == true)
                                {
                                    Ob.CurrentTime = ds.Tables[0].Rows[Ob.Loopi]["CurrentTime"].ToString();
                                }
                                if (Ob.OptDueDate == true || Ob.OptTime == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Time + "'>" + Ob.DueDate + " " + Ob.CurrentTime + "</div>";
                                }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.RemarkPosition.ToString()))
                            {
                                if (ds.Tables[0].Rows[Ob.Loopi]["Colour"].ToString() != "" && Ob.OptColour == true)
                                {
                                    if (ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() != "" && Ob.OptRemark == true)
                                    {
                                        Ob.StrPreviewBarCode += "<div style='" + Ob.Remark + "'>" + ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() + "," + ds.Tables[0].Rows[Ob.Loopi]["Colour"].ToString() + "</div>";
                                    }
                                    else
                                    {
                                        if (Ob.OptColour == true)
                                        {
                                            Ob.StrPreviewBarCode += "<div style='" + Ob.Remark + "'>" + ds.Tables[0].Rows[Ob.Loopi]["Colour"].ToString() + "</div>";
                                        }
                                    }
                                }
                                else
                                {
                                    if (ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() != "" && Ob.OptRemark == true)
                                    {
                                        Ob.StrPreviewBarCode += "<div style='" + Ob.Remark + "'>" + ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() + "</div>";
                                    }
                                }
                            }
                        }

                        //Ob.StrPreviewBarCode += "</div>";
                    }
                }
            }
            return Ob;
        }

        public DTO.BarCodeSetting DemoBarCodeDisplay(DTO.BarCodeSetting Ob)
        {
            Ob.StrPreview = "";
            if (Ob.wet == true)
            {
                Ob.StrMainBarCode = "width: 100%; margin-left: 2px; background-position:center;float: left;padding: 0px 1px 0px 1px;";
            }
            else
            {
                Ob.StrMainBarCode = "width: 32%; height: 32px; background-image: url(../Logo/Dry.jpg);background-repeat: no-repeat; margin-left: 2px; background-position:center;float: left;text-align:" + Ob.LogoAlign + "padding: 0px 1px 0px 1px;";
                Ob.ShopDiv = "width: 60%; float: left; overflow: hidden; font-size:" + Ob.ShopSize + "px; text-align:" + Ob.ShopAlign + ";vertical-align: middle; padding: 0px 1px 0px 1px;";
            }
            Ob.BookingNo = "width: 100%; height: ; font-family:" + Ob.BookingFont + "; font-size:" + Ob.BookingSize + "px; font-weight:" + Ob.BookingBold + ";font-style:" + Ob.BookingItalic + "; text-decoration:" + Ob.BookingUnderline + "; text-align:" + Ob.BookingAlign + ";";

            Ob.Customer = "width: 100%; height: ; font-family:" + Ob.CustomerFont + "; font-size:" + Ob.CustomerSize + "px; font-weight:" + Ob.CustomerBold + ";font-style:" + Ob.CustomerItalic + "; text-decoration:" + Ob.CustomerUnderline + "; text-align:" + Ob.CustomerAlign + "; text-transform: capitalize";

            Ob.Address = "width: 100%; height: ; font-family:" + Ob.AddressFont + "; font-size:" + Ob.AddressSize + "px; font-weight:" + Ob.AddressBold + ";font-style:" + Ob.AddressItalic + "; text-decoration:" + Ob.AddressUnderline + "; text-align:" + Ob.AddressAlign + ";";

            Ob.CustName = "width: 100%; height: ; font-family:" + Ob.ProcessFont + "; font-size:" + Ob.ProcessSize + "px; font-weight:" + Ob.ProcessBold + ";font-style:" + Ob.ProcessItalic + "; text-decoration:" + Ob.ProcessUnderline + "; text-align:" + Ob.ProcessAlign + ";";

            Ob.BarCode = "width: 100%; height: ; font-family:" + Ob.BarCodeFontName + "; font-size:" + Ob.BarCodeFontSize + "px;text-align:" + Ob.BarcodeAlign + ";";
            Ob.Time = "width: 100%; height: ; font-family:" + Ob.DueDateFont + "; font-size:" + Ob.DueDateSize + "px; font-weight:" + Ob.DueDateBold + ";font-style:" + Ob.DueDateItalic + "; text-decoration:" + Ob.DueDateUnderline + "; text-align:" + Ob.DueDateAlign + ";";
            Ob.Item = "width: 100%; height: ; font-family:" + Ob.ItemFont + "; font-size:" + Ob.ItemSize + "px; font-weight:" + Ob.ItemBold + ";font-style:" + Ob.ItemItalic + "; text-decoration:" + Ob.ItemUnderline + "; text-align:" + Ob.ItemAlign + ";";
            Ob.Date = "width: 100%; height: ; font-family:" + Ob.DueDateFont + "; font-size:" + Ob.DueDateSize + "px; font-weight:" + Ob.DueDateBold + ";font-style:" + Ob.DueDateItalic + "; text-decoration:" + Ob.DueDateUnderline + "; text-align:" + Ob.DueDateAlign + ";";
            Ob.Remark = "width: 100%; height: ; font-family:" + Ob.RemarkFont + "; font-size:" + Ob.RemarkSize + "px; font-weight:" + Ob.RemarkBold + ";font-style:" + Ob.RemarkItalic + "; text-decoration:" + Ob.RemarkUnderline + "; text-align:" + Ob.RemarkAlign + ";";

            if (Ob.wet == true)
            {
                Ob.StrPreview += "<div style='" + Ob.StrMainBarCode + "'></div>";
            }
            else
                if (Ob.LogoSize == "Full")
                {
                    Ob.StrMainBarCode = "width: 100%; height: 32px; background-image: url(../Logo/Dry.jpg);background-repeat: no-repeat; margin-left: 2px; background-position:center;float: left;text-align:" + Ob.LogoAlign + "padding: 0px 1px 0px 1px;";
                    Ob.StrPreview += "<div style='" + Ob.StrMainBarCode + "'></div>";
                }

                else
                {
                    if (Ob.LogoAlign == "Left")
                    {
                        if (Ob.OptLogo == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.StrMainBarCode + "'></div>";
                        }
                        if (Ob.OptShopOption == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.ShopDiv + "'>" + Ob.ShopName + "</div>";
                        }
                    }
                    else
                    {
                        if (Ob.OptShopOption == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.ShopDiv + "'>" + Ob.ShopName + "</div>";
                        }
                        else
                        {
                            Ob.StrPreview += "<div style='" + Ob.ShopDiv + "'>&nbsp;</div>";
                        }
                        if (Ob.OptLogo == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.StrMainBarCode + "'></div>";
                        }
                    }
                }

            if (Ob.wet == true)
            {
                Ob.StrPreview += "<div style='" + Ob.BookingNo + "' >" + "1234" + "</div>";
                Ob.StrPreview += "<div style='" + Ob.Time + "'>" + "<span style='" + Ob.Date + "'>" + "" + Ob.DueDate + "" + Ob.CurrentTime + "</span></div>";
            }
            else
            {
                for (Ob.Loopj = 1; Ob.Loopj <= 8; Ob.Loopj++)
                {
                    if (Ob.Loopj == Int32.Parse(Ob.BookingPosition.ToString()))
                    {
                        if (Ob.OptBooking == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.BookingNo + "' >" + "1234" + "</div>";
                        }
                    }
                    if (Ob.Loopj == Int32.Parse(Ob.CustomerPosition.ToString()))
                    {
                        if (Ob.OptCustomer == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.Customer + "' >" + "Sanjeev" + "</div>";
                        }
                    }
                    if (Ob.Loopj == Int32.Parse(Ob.AddressPosition.ToString()))
                    {
                        if (Ob.OptAddress == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.Address + "' >" + "E-166 Kamla Nagar" + "</div>";
                        }
                    }

                    if (Ob.Loopj == Int32.Parse(Ob.ProcessPosition.ToString()))
                    {
                        if (Ob.OptProcess == true)
                        {
                            Ob.Process = "DC";
                        }

                        if (Ob.OptExtraProcess == true)
                        {
                            Ob.ExtProcess = "LD";
                        }

                        if (Ob.OptExtraProcessSecond == true)
                        {
                            Ob.ExtProcessSecond = "WC";
                        }

                        if (Ob.OptSubtotal == true)
                        {
                            Ob.OptSubtotal1 = "1/1";
                        }

                        if (Ob.OptProcess == true || Ob.OptExtraProcess == true || Ob.OptExtraProcessSecond == true || Ob.OptSubtotal == true)
                        {
                            Ob.StrPreview += "<div  style='" + Ob.CustName + "' >" + Ob.Process + "" + Ob.ExtProcess + "" + Ob.OptSubtotal1 + "</div>";
                        }
                    }
                    if (Ob.Loopj == Int32.Parse(Ob.BarPosition.ToString()))
                    {
                        if (Ob.OptBarCode == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.BarCode + "'>" + "*1-1*" + "</div>";
                        }
                    }

                    if (Ob.Loopj == Int32.Parse(Ob.ItemPosition.ToString()))
                    {
                        if (Ob.OptItem == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.Item + "'>" + "SUIT" + "</div>";
                        }
                    }
                    if (Ob.Loopj == Int32.Parse(Ob.DueDatePosition.ToString()))
                    {
                        if (Ob.OptDueDate == true)
                        {
                            Ob.DueDate = "24/05/12";
                        }

                        if (Ob.OptTime == true)
                        {
                            Ob.CurrentTime = "1 AM";
                        }
                        if (Ob.OptDueDate == true || Ob.OptTime == true)
                        {
                            Ob.StrPreview += "<div style='" + Ob.Time + "'>" + "<span style='" + Ob.Date + "'>" + "" + Ob.DueDate + "" + Ob.CurrentTime + "</span></div>";
                        }
                    }
                    if (Ob.Loopj == Int32.Parse(Ob.RemarkPosition.ToString()))
                    {
                        if ("Magenta" != "" && Ob.OptColour == true)
                        {
                            if ("Cut marks" != "" && Ob.OptRemark == true)
                            {
                                Ob.StrPreview += "<div style='" + Ob.Remark + "'>" + "Cut marks" + "," + "Magenta" + "</div>";
                            }
                            else
                            {
                                if (Ob.OptColour == true)
                                {
                                    Ob.StrPreview += "<div style='" + Ob.Remark + "'>" + "Magenta" + "</div>";
                                }
                            }
                        }
                        else
                        {
                            if ("Cut marks" != "" && Ob.OptRemark == true)
                            {
                                Ob.StrPreview += "<div style='" + Ob.Remark + "'>" + "Cut marks" + "</div>";
                            }
                        }
                    }
                }
            }

            return Ob;
        }

        public string PreviewDemo(Label lbl1, DropDownList drp1, DropDownList drp2, DropDownList drp3, DropDownList drp4, DropDownList drp5, DropDownList drp6, DropDownList drp7, DropDownList drp8, DropDownList drp9)
        {
            Ob.bookingnoposition = drp1.SelectedItem.ToString();
            Ob.customerposition = drp2.SelectedItem.ToString();
            Ob.addressposition = drp3.SelectedItem.ToString();
            Ob.processposition = drp4.SelectedItem.ToString();
            Ob.remarkposition = drp5.SelectedItem.ToString();
            Ob.barcodeposition = drp6.SelectedItem.ToString();
            Ob.itemposition = drp7.SelectedItem.ToString();
            Ob.itemposition1 = drp8.SelectedItem.ToString();
            Ob.bdateposition = drp9.SelectedItem.ToString();
            Ob.msg = string.Empty;
            if (Ob.bookingnoposition == Ob.customerposition || Ob.addressposition == Ob.bookingnoposition || Ob.bookingnoposition == Ob.processposition || Ob.bookingnoposition == Ob.remarkposition || Ob.bookingnoposition == Ob.barcodeposition || Ob.bookingnoposition == Ob.itemposition || Ob.bookingnoposition == Ob.itemposition1 || Ob.BookingPosition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.bookingnoposition;
            }
            if (Ob.bookingnoposition == Ob.customerposition || Ob.addressposition == Ob.customerposition || Ob.customerposition == Ob.processposition || Ob.customerposition == Ob.remarkposition || Ob.customerposition == Ob.barcodeposition || Ob.customerposition == Ob.itemposition || Ob.customerposition == Ob.itemposition1 || Ob.customerposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.customerposition;
            }
            if (Ob.processposition == Ob.customerposition || Ob.addressposition == Ob.processposition || Ob.bookingnoposition == Ob.processposition || Ob.processposition == Ob.remarkposition || Ob.processposition == Ob.barcodeposition || Ob.processposition == Ob.itemposition || Ob.processposition == Ob.itemposition1 || Ob.processposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.processposition;
            }
            if (Ob.remarkposition == Ob.customerposition || Ob.addressposition == Ob.remarkposition || Ob.bookingnoposition == Ob.remarkposition || Ob.processposition == Ob.remarkposition || Ob.remarkposition == Ob.barcodeposition || Ob.remarkposition == Ob.itemposition || Ob.remarkposition == Ob.itemposition1 || Ob.remarkposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.remarkposition;
            }
            if (Ob.barcodeposition == Ob.customerposition || Ob.addressposition == Ob.barcodeposition || Ob.bookingnoposition == Ob.barcodeposition || Ob.barcodeposition == Ob.remarkposition || Ob.remarkposition == Ob.barcodeposition || Ob.barcodeposition == Ob.itemposition || Ob.barcodeposition == Ob.itemposition1 || Ob.barcodeposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.barcodeposition;
            }
            if (Ob.itemposition == Ob.customerposition || Ob.addressposition == Ob.itemposition || Ob.bookingnoposition == Ob.itemposition || Ob.itemposition == Ob.remarkposition || Ob.itemposition == Ob.barcodeposition || Ob.barcodeposition == Ob.itemposition || Ob.itemposition1 == Ob.itemposition || Ob.itemposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.itemposition;
            }
            if (Ob.itemposition1 == Ob.customerposition || Ob.addressposition == Ob.itemposition1 || Ob.bookingnoposition == Ob.itemposition1 || Ob.itemposition1 == Ob.remarkposition || Ob.itemposition1 == Ob.barcodeposition || Ob.barcodeposition == Ob.itemposition1 || Ob.itemposition1 == Ob.itemposition || Ob.itemposition1 == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.itemposition1;
            }
            if (Ob.addressposition == Ob.customerposition || Ob.bookingnoposition == Ob.addressposition || Ob.addressposition == Ob.itemposition || Ob.addressposition == Ob.remarkposition || Ob.addressposition == Ob.barcodeposition || Ob.processposition == Ob.addressposition || Ob.itemposition1 == Ob.addressposition || Ob.addressposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.addressposition;
            }
            if (Ob.bdateposition == Ob.customerposition || Ob.bookingnoposition == Ob.bdateposition || Ob.bdateposition == Ob.itemposition || Ob.bdateposition == Ob.remarkposition || Ob.bdateposition == Ob.barcodeposition || Ob.processposition == Ob.bdateposition || Ob.itemposition1 == Ob.bdateposition || Ob.addressposition == Ob.bdateposition)
            {
                Ob.msg = Ob.msg + Ob.bdateposition;
            }

            if (Ob.msg != "")
            {
                lbl1.Text = "Positons " + Ob.msg + " are Same";
                return lbl1.Text;
            }

            lbl1.Text = "";
            return lbl1.Text;
        }

        public string Updatebarcodewidthheight(DTO.BarCodeSetting Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@barcodewidth", Ob.BarcodeWidth);
            cmd.Parameters.AddWithValue("@barcodeheight", Ob.BarcodeHeight);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 9);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string Updatebarcodeconfig(DTO.BarCodeSetting Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@barcodeconfigbooking", Ob.OptBooking);
            cmd.Parameters.AddWithValue("@barcodebookingfont", Ob.BookingFont);
            cmd.Parameters.AddWithValue("@barcodebookingsize", Ob.BookingSize);
            cmd.Parameters.AddWithValue("@barcodebookingalign", Ob.BookingAlign);
            cmd.Parameters.AddWithValue("@barcodebookingbold", Ob.BookingBold);
            cmd.Parameters.AddWithValue("@barcodebookingitalic", Ob.BookingItalic);
            cmd.Parameters.AddWithValue("@barcodebookingunderline", Ob.BookingUnderline);

            cmd.Parameters.AddWithValue("@barcodeconfigprocess", Ob.OptProcess);
            cmd.Parameters.AddWithValue("@barcodeconfigextraprocess", Ob.OptExtraProcess);
            cmd.Parameters.AddWithValue("@barcodeconfigextraprocesssecond", Ob.OptExtraProcessSecond);
            cmd.Parameters.AddWithValue("@barcodeconfigprocesssubtotal", Ob.OptSubtotal);
            cmd.Parameters.AddWithValue("@barcodeprocessfont", Ob.ProcessFont);
            cmd.Parameters.AddWithValue("@barcodeprocesssize", Ob.ProcessSize);
            cmd.Parameters.AddWithValue("@barcodeprocessalign", Ob.ProcessAlign);
            cmd.Parameters.AddWithValue("@barcodeprocessbold", Ob.ProcessBold);
            cmd.Parameters.AddWithValue("@barcodeprocessitalic", Ob.ProcessItalic);
            cmd.Parameters.AddWithValue("@barcodeprocessunderline", Ob.ProcessUnderline);

            cmd.Parameters.AddWithValue("@barcodeconfigremark", Ob.OptRemark);
            cmd.Parameters.AddWithValue("@barcodeconfigcolour", Ob.OptColour);
            cmd.Parameters.AddWithValue("@barcoderemarkfont", Ob.RemarkFont);
            cmd.Parameters.AddWithValue("@barcodremarksize", Ob.RemarkSize);
            cmd.Parameters.AddWithValue("@barcoderemarkalign", Ob.RemarkAlign);
            cmd.Parameters.AddWithValue("@barcoderemarkbold", Ob.RemarkBold);
            cmd.Parameters.AddWithValue("@barcoderemarkitalic", Ob.RemarkItalic);
            cmd.Parameters.AddWithValue("@barcoderemarkunderline", Ob.RemarkUnderline);

            cmd.Parameters.AddWithValue("@barcodeconfigbarcode", Ob.OptBarCode);

            cmd.Parameters.AddWithValue("@barcodebarcodealign", Ob.BarcodeAlign);

            cmd.Parameters.AddWithValue("@barcodeconfigitem", Ob.OptItem);
            cmd.Parameters.AddWithValue("@barcodeconfigduedate", Ob.OptDueDate);
            cmd.Parameters.AddWithValue("@barcodeconfigtime", Ob.OptTime);
            cmd.Parameters.AddWithValue("@barcodeitemfont", Ob.DueDateFont);
            cmd.Parameters.AddWithValue("@barcodeitemfont1", Ob.ItemFont);
            cmd.Parameters.AddWithValue("@barcodeitemsize", Ob.DueDateSize);
            cmd.Parameters.AddWithValue("@barcodeitemsize1", Ob.ItemSize);
            cmd.Parameters.AddWithValue("@barcodeitemalign", Ob.DueDateAlign);
            cmd.Parameters.AddWithValue("@barcodeitemalign1", Ob.ItemAlign);
            cmd.Parameters.AddWithValue("@barcodeitembold", Ob.DueDateBold);
            cmd.Parameters.AddWithValue("@barcodeitembold1", Ob.ItemBold);
            cmd.Parameters.AddWithValue("@barcodeitemitalic", Ob.DueDateItalic);
            cmd.Parameters.AddWithValue("@barcodeitemitalic1", Ob.ItemItalic);
            cmd.Parameters.AddWithValue("@barcodeitemunderline", Ob.DueDateUnderline);
            cmd.Parameters.AddWithValue("@barcodeitemunderline1", Ob.ItemUnderline);

            cmd.Parameters.AddWithValue("@barcodeconfigcustomer", Ob.OptCustomer);
            cmd.Parameters.AddWithValue("@barcodecustomerfont", Ob.CustomerFont);
            cmd.Parameters.AddWithValue("@barcodecustomerize", Ob.CustomerSize);
            cmd.Parameters.AddWithValue("@barcodecustomeralign", Ob.CustomerAlign);
            cmd.Parameters.AddWithValue("@barcodecustomerbold", Ob.CustomerBold);
            cmd.Parameters.AddWithValue("@barcodecustomeritalic", Ob.CustomerItalic);
            cmd.Parameters.AddWithValue("@barcodecustomerunderline", Ob.CustomerUnderline);

            cmd.Parameters.AddWithValue("@bookingnoposition", Ob.BookingPosition);
            cmd.Parameters.AddWithValue("@customerposition", Ob.CustomerPosition);
            cmd.Parameters.AddWithValue("@processposition", Ob.ProcessPosition);
            cmd.Parameters.AddWithValue("@remarkposition", Ob.RemarkPosition);
            cmd.Parameters.AddWithValue("@barcodeposition", Ob.BarPosition);
            cmd.Parameters.AddWithValue("@itemposition", Ob.DueDatePosition);
            cmd.Parameters.AddWithValue("@itemposition1", Ob.ItemPosition);
            cmd.Parameters.AddWithValue("@addressposition", Ob.AddressPosition);

            cmd.Parameters.AddWithValue("@barcodeconfigaddressr", Ob.OptAddress);
            cmd.Parameters.AddWithValue("@barcodeaddressfont", Ob.AddressFont);
            cmd.Parameters.AddWithValue("@barcodeaddresssize", Ob.AddressSize);
            cmd.Parameters.AddWithValue("@barcodeaddressalign", Ob.AddressAlign);
            cmd.Parameters.AddWithValue("@barcodeaddressbold", Ob.AddressBold);
            cmd.Parameters.AddWithValue("@barcodeaddressitalic", Ob.AddressItalic);
            cmd.Parameters.AddWithValue("@barcodeaddressunderline", Ob.AddressUnderline);
            cmd.Parameters.AddWithValue("@LogoOption", Ob.OptLogo);
            cmd.Parameters.AddWithValue("@ShopOption", Ob.OptShopOption);
            cmd.Parameters.AddWithValue("@PrinterName", Ob.PrinterName);
            cmd.Parameters.AddWithValue("@ShopName", Ob.ShopName);
            cmd.Parameters.AddWithValue("@LogoPosition", Ob.LogoPosition);
            cmd.Parameters.AddWithValue("@ShopAlign", Ob.ShopAlign);
            cmd.Parameters.AddWithValue("@ShopSize", Ob.ShopSize);
            cmd.Parameters.AddWithValue("@BarCodeFontName", Ob.BarCodeFontName);
            cmd.Parameters.AddWithValue("@BarCodeFontSize", Ob.BarCodeFontSize);
            cmd.Parameters.AddWithValue("@LogoAlign", Ob.LogoAlign);
            cmd.Parameters.AddWithValue("@IsDueIncrease", Ob.IsDueIncrease);
            cmd.Parameters.AddWithValue("@LogoSize", Ob.LogoSize);
            cmd.Parameters.AddWithValue("@BookingDate", Ob.OptBookingDate);
            cmd.Parameters.AddWithValue("@BDateFont", Ob.bdatefont);
            cmd.Parameters.AddWithValue("@BDateSize", Ob.bdatesize);
            cmd.Parameters.AddWithValue("@BDateAlign", Ob.bdatealign);
            cmd.Parameters.AddWithValue("@BDateBold", Ob.bdatebold);
            cmd.Parameters.AddWithValue("@BDateItalic", Ob.bdateitalic);
            cmd.Parameters.AddWithValue("@BDateUnderline", Ob.bdateunderline);
            cmd.Parameters.AddWithValue("@BDatePosition", Ob.bdateposition);

            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@daysValue", Ob.daysValue);
            cmd.Parameters.AddWithValue("@pagebreak", Ob.pagebreak);
            cmd.Parameters.AddWithValue("@wet", Ob.wet);
            cmd.Parameters.AddWithValue("@Flag", 13);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DTO.BarCodeSetting BarCodeData(DataSet ds1, string Id, string BookingNo, string BranchId, string ImageUrl)
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                Ob.bdatebold = ds1.Tables[0].Rows[0]["BDateBold"].ToString();
                Ob.bdatefont = ds1.Tables[0].Rows[0]["BDateFont"].ToString();
                Ob.bdateitalic = ds1.Tables[0].Rows[0]["BDateItalic"].ToString();
                Ob.bdatealign = ds1.Tables[0].Rows[0]["BDateAlign"].ToString();
                Ob.bdatesize = ds1.Tables[0].Rows[0]["BDateSize"].ToString();
                Ob.bdateunderline = ds1.Tables[0].Rows[0]["BDateunderline"].ToString();
                Ob.bdateposition = ds1.Tables[0].Rows[0]["BDateposition"].ToString();
                Ob.OptBookingDate = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodebookingdate"].ToString());

                Ob.OptBooking = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodebookingno"].ToString());
                Ob.BookingFont = ds1.Tables[0].Rows[0]["bookingfont"].ToString();
                Ob.BookingSize = ds1.Tables[0].Rows[0]["bookingsize"].ToString();
                Ob.BookingAlign = ds1.Tables[0].Rows[0]["bookingalign"].ToString();
                Ob.BookingBold = ds1.Tables[0].Rows[0]["bookingbold"].ToString();
                Ob.BookingItalic = ds1.Tables[0].Rows[0]["bookingitilic"].ToString();
                Ob.BookingUnderline = ds1.Tables[0].Rows[0]["bookingunderline"].ToString();
                Ob.OptProcess = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeprocess"].ToString());
                Ob.OptExtraProcess = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodexteraprocess"].ToString());
                Ob.OptExtraProcessSecond = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeextraprocesssecond"].ToString());
                Ob.OptSubtotal = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodesubtotal"].ToString());
                Ob.ProcessFont = ds1.Tables[0].Rows[0]["processfont"].ToString();
                Ob.ProcessSize = ds1.Tables[0].Rows[0]["processsize"].ToString();
                Ob.ProcessAlign = ds1.Tables[0].Rows[0]["processalign"].ToString();
                Ob.ProcessBold = ds1.Tables[0].Rows[0]["processbold"].ToString();
                Ob.ProcessItalic = ds1.Tables[0].Rows[0]["processitalic"].ToString();
                Ob.ProcessUnderline = ds1.Tables[0].Rows[0]["processunderline"].ToString();
                Ob.OptRemark = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcoderemark"].ToString());
                Ob.OptColour = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodecolour"].ToString());
                Ob.RemarkFont = ds1.Tables[0].Rows[0]["remarkfont"].ToString();
                Ob.RemarkSize = ds1.Tables[0].Rows[0]["remarksize"].ToString();
                Ob.RemarkAlign = ds1.Tables[0].Rows[0]["remarkremarkalign"].ToString();
                Ob.RemarkBold = ds1.Tables[0].Rows[0]["remarkbold"].ToString();
                Ob.RemarkItalic = ds1.Tables[0].Rows[0]["remarkitalic"].ToString();
                Ob.RemarkUnderline = ds1.Tables[0].Rows[0]["remarkunderline"].ToString();
                Ob.OptPrint = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeprint"].ToString());

                Ob.BarcodeAlign = ds1.Tables[0].Rows[0]["barcodealign"].ToString();
                Ob.BarCodeFontName = ds1.Tables[0].Rows[0]["BarCodeFontName"].ToString();
                Ob.BarCodeFontSize = ds1.Tables[0].Rows[0]["BarCodeFontSize"].ToString();
                Ob.OptItem = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeitem"].ToString());
                Ob.OptLogo = Convert.ToBoolean(ds1.Tables[0].Rows[0]["LogoOption"].ToString());
                Ob.OptShopOption = Convert.ToBoolean(ds1.Tables[0].Rows[0]["ShopOption"].ToString());
                Ob.OptDueDate = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeduedate"].ToString());
                Ob.OptTime = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodetime"].ToString());
                Ob.DueDateFont = ds1.Tables[0].Rows[0]["itemfont"].ToString();
                Ob.ItemFont = ds1.Tables[0].Rows[0]["itemfont1"].ToString();
                Ob.DueDateSize = ds1.Tables[0].Rows[0]["itemsize"].ToString();
                Ob.ItemSize = ds1.Tables[0].Rows[0]["itemsize1"].ToString();
                Ob.DueDateBold = ds1.Tables[0].Rows[0]["itembold"].ToString();
                Ob.ItemBold = ds1.Tables[0].Rows[0]["itembold1"].ToString();
                Ob.DueDateItalic = ds1.Tables[0].Rows[0]["itemitalic"].ToString();
                Ob.ItemItalic = ds1.Tables[0].Rows[0]["itemitalic1"].ToString();

                Ob.DueDateAlign = ds1.Tables[0].Rows[0]["itemalign"].ToString();
                Ob.ItemAlign = ds1.Tables[0].Rows[0]["itemalign1"].ToString();
                Ob.DueDateUnderline = ds1.Tables[0].Rows[0]["itemunderline"].ToString();
                Ob.ItemUnderline = ds1.Tables[0].Rows[0]["itemunderline1"].ToString();

                Ob.OptCustomer = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodecusname"].ToString());
                Ob.CustomerFont = ds1.Tables[0].Rows[0]["cusfont"].ToString();
                Ob.CustomerSize = ds1.Tables[0].Rows[0]["cussize"].ToString();
                Ob.CustomerAlign = ds1.Tables[0].Rows[0]["cusalign"].ToString();
                Ob.CustomerBold = ds1.Tables[0].Rows[0]["cusbold"].ToString();
                Ob.CustomerItalic = ds1.Tables[0].Rows[0]["cusitalic"].ToString();
                Ob.CustomerUnderline = ds1.Tables[0].Rows[0]["cusunderline"].ToString();

                Ob.BookingPosition = ds1.Tables[0].Rows[0]["bookingnoposition"].ToString();
                Ob.CustomerPosition = ds1.Tables[0].Rows[0]["cusposition"].ToString();
                Ob.ProcessPosition = ds1.Tables[0].Rows[0]["processposition"].ToString();
                Ob.RemarkPosition = ds1.Tables[0].Rows[0]["remarkposition"].ToString();
                Ob.BarPosition = ds1.Tables[0].Rows[0]["barcodeposition"].ToString();
                Ob.LogoPosition = ds1.Tables[0].Rows[0]["LogoPosition"].ToString();
                Ob.LogoAlign = ds1.Tables[0].Rows[0]["LogoAlign"].ToString();
                Ob.LogoUrl = ds1.Tables[0].Rows[0]["LogoUrl"].ToString();
                // This Variabale does not available in table
                Ob.ShopAlign = ds1.Tables[0].Rows[0]["ShopAlign"].ToString();
                Ob.ShopSize = ds1.Tables[0].Rows[0]["ShopSize"].ToString();

                Ob.DueDatePosition = ds1.Tables[0].Rows[0]["itemposition"].ToString();
                Ob.ItemPosition = ds1.Tables[0].Rows[0]["itemposition1"].ToString();

                Ob.AddressPosition = ds1.Tables[0].Rows[0]["Addressposition"].ToString();
                Ob.OptAddress = Convert.ToBoolean(ds1.Tables[0].Rows[0]["barcodeaddress"].ToString());
                Ob.AddressFont = ds1.Tables[0].Rows[0]["addfont"].ToString();
                Ob.AddressSize = ds1.Tables[0].Rows[0]["addsize"].ToString();
                Ob.AddressAlign = ds1.Tables[0].Rows[0]["addalign"].ToString();
                Ob.AddressBold = ds1.Tables[0].Rows[0]["addbold"].ToString();
                Ob.AddressItalic = ds1.Tables[0].Rows[0]["additalic"].ToString();
                Ob.AddressUnderline = ds1.Tables[0].Rows[0]["addunderline"].ToString();
                Ob.ShopName = ds1.Tables[0].Rows[0]["ShopName"].ToString();
                Ob.PrinterName = ds1.Tables[0].Rows[0]["PrinterName"].ToString();
                Ob.IsDueIncrease = Convert.ToBoolean(ds1.Tables[0].Rows[0]["IsDueIncrease"].ToString());
                Ob.LogoSize = ds1.Tables[0].Rows[0]["LogoSize"].ToString();
                Ob.daysValue = Convert.ToInt32(ds1.Tables[0].Rows[0]["daysValue"].ToString());
                Ob.wet = Convert.ToBoolean(ds1.Tables[0].Rows[0]["wet"].ToString());
                string BWidth = ds1.Tables[0].Rows[0]["barcodewidth"].ToString().Substring(0, 3);
                string BHeight = ds1.Tables[0].Rows[0]["barcodeheight"].ToString().Substring(0, 3);
                double BarWidth = Math.Round(Convert.ToDouble(BWidth) + 0.01, 2);
                double BarHeight = Math.Round(Convert.ToDouble(BHeight) + 0.01, 2);
                Ob.pagebreak = Convert.ToInt32(ds1.Tables[0].Rows[0]["Pagebreak"].ToString());
                Ob.OldBarcodeWidth = BarWidth.ToString() + "in";
                Ob.OldBarcodeHeight = BarHeight.ToString() + "in";

                Ob.StrArray = Ob.OldBarcodeHeight.Split('i');
                Ob.Barcodeheight = Convert.ToDouble(Ob.StrArray[0]) - 0.5;
                Ob.DivHeight = Math.Round(Ob.Barcodeheight / 7, 2);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_rpt_barcodprint";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", BookingNo);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                if (Id != "0")
                    cmd.Parameters.AddWithValue("@RowIndex", Id);
                if (Ob.IsDueIncrease == true)
                {
                    cmd.Parameters.AddWithValue("@daysValue", Ob.daysValue);
                    cmd.Parameters.AddWithValue("@Flag", 7);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Flag", 4);
                }
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.Connection = sqlcon;
                da.SelectCommand = cmd;
                da.Fill(ds);
                string strMainBar = string.Empty;
                Ob.StrPreviewBarCode = "";
                if (Ob.wet == false && Ob.pagebreak == 0)
                    strMainBar = "width:" + Ob.OldBarcodeWidth + ";height:" + Ob.OldBarcodeHeight + ";font-size:6px;text-align:center;overflow:hidden;";
                else
                    if (Ob.wet == true && Ob.pagebreak == 0)
                        strMainBar = "width:" + Ob.OldBarcodeWidth + ";height:" + Ob.OldBarcodeHeight + ";font-size:6px;text-align:center;overflow:hidden;page-break-after:always;";
                    else
                        strMainBar = "width:" + Ob.OldBarcodeWidth + ";height:" + Ob.OldBarcodeHeight + ";font-size:6px;text-align:center;overflow:hidden;page-break-after:always;";
                if (Ob.wet == true)
                {
                    //Ob.StrMainBarCode = "width: 100%; margin-left: 2px; background-position:center;float: left;padding: 0px 1px 0px 1px;";
                    //strMainBar = "width:" + Ob.OldBarcodeWidth + ";height:" + Ob.OldBarcodeHeight + ";font-size:6px;text-align:center;overflow:hidden;page-break-after:always;";
                    Ob.child1 = "float:left;width:100%;height:1.0in;";
                }
                else
                {
                    if (Ob.LogoSize == "Full")
                    {
                        Ob.StrMainBarCode = "width: 100%; height: 32px; margin-left: 2px; background-position:center;float: left;text-align:Center;padding: 0px 1px 0px 1px;";
                    }
                    else
                    {
                        Ob.StrMainBarCode = "width: 32%; height: 32px; margin-left: 2px; background-position:center;float: left;text-align:" + Ob.LogoAlign + ";padding: 0px 1px 0px 1px;";
                    }
                    Ob.ShopDiv = "width: 60%; float: left; overflow: hidden; font-size:" + Ob.ShopSize + "px; text-align:" + Ob.ShopAlign + ";vertical-align: middle; padding: 0px 1px 0px 1px;";

                    Ob.BookingNo = "width: 100%; height: ; font-family:" + Ob.BookingFont + "; font-size:" + Ob.BookingSize + "px; font-weight:" + Ob.BookingBold + ";font-style:" + Ob.BookingItalic + "; text-decoration:" + Ob.BookingUnderline + "; text-align:" + Ob.BookingAlign + ";margin-bottom:-2%;";
                    Ob.bookingdate = "width: 100%; height: ; font-family:" + Ob.bdatefont + "; font-size:" + Ob.bdatesize + "px; font-weight:" + Ob.bdatebold + ";font-style:" + Ob.bdateitalic + "; text-decoration:" + Ob.bdateunderline + "; text-align:" + Ob.bdatealign + ";margin-top:-2%;";
                    Ob.Customer = "width: 100%; height: ; font-family:" + Ob.CustomerFont + "; font-size:" + Ob.CustomerSize + "px; font-weight:" + Ob.CustomerBold + ";font-style:" + Ob.CustomerItalic + "; text-decoration:" + Ob.CustomerUnderline + "; text-align:" + Ob.CustomerAlign + "; text-transform: capitalize";

                    Ob.Address = "width: 100%; height: ; font-family:" + Ob.AddressFont + "; font-size:" + Ob.AddressSize + "px; font-weight:" + Ob.AddressBold + ";font-style:" + Ob.AddressItalic + "; text-decoration:" + Ob.AddressUnderline + "; text-align:" + Ob.AddressAlign + ";margin-top:-2%;";

                    Ob.CustName = "width: 100%; height:25px ; font-family:" + Ob.ProcessFont + "; font-size:" + Ob.ProcessSize + "px; font-weight:" + Ob.ProcessBold + ";font-style:" + Ob.ProcessItalic + "; text-decoration:" + Ob.ProcessUnderline + "; text-align:" + ";" + "margin-left:6%;";

                    Ob.BarCode = "width: 100%; height: ; font-family:" + Ob.BarCodeFontName + "; font-size:" + Ob.BarCodeFontSize + "px;text-align:" + Ob.BarcodeAlign + ";";
                    Ob.Time = "width: 100%; height: ; font-family:" + Ob.DueDateFont + "; font-size:" + Ob.DueDateSize + "px; font-weight:" + Ob.DueDateBold + ";font-style:" + Ob.DueDateItalic + "; text-decoration:" + Ob.DueDateUnderline + "; text-align:" + Ob.DueDateAlign + ";";
                    Ob.Item = "width: 100%; height: ; font-family:" + Ob.ItemFont + "; font-size:" + Ob.ItemSize + "px; font-weight:" + Ob.ItemBold + ";font-style:" + Ob.ItemItalic + "; text-decoration:" + Ob.ItemUnderline + "; text-align:" + Ob.ItemAlign + ";margin-top:-2%;";
                    Ob.Date = "width: 100%; height: ; font-family:" + Ob.DueDateFont + "; font-size:" + Ob.DueDateSize + "px; font-weight:" + Ob.DueDateBold + ";font-style:" + Ob.DueDateItalic + "; text-decoration:" + Ob.DueDateUnderline + "; text-align:" + Ob.DueDateAlign + ";margin-top:-2%";
                    Ob.Remark = "width: 100%; height: ; font-family:" + Ob.RemarkFont + "; font-size:" + Ob.RemarkSize + "px; font-weight:" + Ob.RemarkBold + ";font-style:" + Ob.RemarkItalic + "; text-decoration:" + Ob.RemarkUnderline + "; text-align:" + Ob.RemarkAlign + ";margin-top:-2%;";
                }

                for (Ob.Loopi = 0; Ob.Loopi < ds.Tables[0].Rows.Count; Ob.Loopi++)
                {
                    if (Ob.wet == true)
                    {
                        DateTime tempduedate, tempBookingdate;

                        Ob.BookingNo = "width: 100%;margin-top:1px;font-family:Times New Roman;font-weight:bold;font-size:24px;text-align:left;padding-left:0in;";
                        Ob.Customer = "width: 100%;margin-top:1px;font-family:Arial;font-size:16px;text-align:left;text-transform: capitalize;padding-left:0in;";
                        if (ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() == "")
                        {
                            Ob.OldBarcodeHeight = "0.80in";
                            strMainBar = "width:" + Ob.OldBarcodeWidth + ";height:" + Ob.OldBarcodeHeight + ";font-size:6px;text-align:center;overflow:hidden;page-break-after:always;";
                            Ob.StrPreviewBarCode += "<div style='" + strMainBar + "'>";
                            Ob.Date = "width: 100%;margin-top:1px;font-family:Arial;font-size:11px;text-align:left;padding-left:0in;visibility:hidden";
                        }
                        else
                        {
                            Ob.OldBarcodeHeight = "1.00in";
                            strMainBar = "width:" + Ob.OldBarcodeWidth + ";height:" + Ob.OldBarcodeHeight + ";font-size:6px;text-align:center;overflow:hidden;page-break-after:always;";
                            Ob.StrPreviewBarCode += "<div style='" + strMainBar + "'>";
                            Ob.Date = "width: 100%;margin-top:1px;font-family:Arial;font-size:11px;text-align:left;padding-left:0in;";
                        }
                        //Ob.BarCode = "width: 100%;Margin-Top:10px;vertical-aligh:middle; font-family:" + Ob.BarCodeFontName + ";font-size:16px;-webkit-transform:rotate(90deg);-moz-transform:rotate(90deg);-o-transform: rotate(90deg);filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);";
                        Ob.Item = "width: 100%;margin-top:1px;font-family:Arial;font-size:13px;text-align:left;padding-left:0in;";
                        Ob.StrPreviewBarCode += "<div style='" + Ob.child1 + "'>";
                        tempduedate = Convert.ToDateTime(ds.Tables[0].Rows[Ob.Loopi]["DueDate"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                        tempBookingdate = Convert.ToDateTime(ds.Tables[0].Rows[Ob.Loopi]["BookingDate"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                        Ob.StrPreviewBarCode += "<div style='" + Ob.BookingNo + "' >" + ds.Tables[0].Rows[Ob.Loopi]["BookingNo"].ToString() + " " + tempduedate.ToString("ddd, d MMM") + "</div>";

                        Ob.StrPreviewBarCode += "<div style='" + Ob.Customer + "' >" + ds.Tables[0].Rows[Ob.Loopi]["CustomerName"].ToString() + "</div>";

                        Ob.StrPreviewBarCode += "<div style='" + Ob.Item + "'>" + ds.Tables[0].Rows[Ob.Loopi]["Item"].ToString() + "  " + ds.Tables[0].Rows[Ob.Loopi]["ItemTotalAndSubTotal"].ToString() + "  " + ds.Tables[0].Rows[Ob.Loopi]["Process"].ToString() + "  " + tempBookingdate.ToString("d MMM") + "</div>";

                        Ob.StrPreviewBarCode += "<div style='" + Ob.Date + "'>" + ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() + "</div>";

                        Ob.StrPreviewBarCode += "</div>";

                        ///* Child Div */
                        //Ob.StrPreviewBarCode += "<div style='" + Ob.child2 + "'>";
                        //Ob.StrPreviewBarCode += "<div style='" + Ob.BarCode + "'>" + ds.Tables[0].Rows[0]["Barcode"].ToString() + "</div></div>";
                        // Ob.StrPreviewBarCode += "<div style=line-height: 0px;>--------------------</div>";
                        Ob.StrPreviewBarCode += "</div>";
                    }
                    else
                    {
                        Ob.StrPreviewBarCode += "<div style='" + strMainBar + "'>";

                        if (Ob.LogoSize == "Full")
                        {
                            Ob.StrPreviewBarCode += "<div style='" + Ob.StrMainBarCode + "'><img src='../Logo/Dry" + BranchId + ".jpg' width='100' Height='32'></img></div>";
                        }
                        else
                        {
                            if (Ob.LogoAlign == "Left")
                            {
                                if (Ob.OptLogo == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.StrMainBarCode + "'><img src='../Logo/Dry" + BranchId + ".jpg' width='32' Height='32'></img></div>";
                                }
                                if (Ob.OptShopOption == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.ShopDiv + "'>" + Ob.ShopName + "</div>";
                                }
                            }
                            else
                            {
                                if (Ob.OptShopOption == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.ShopDiv + "'>" + Ob.ShopName + "</div>";
                                }
                                else
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.ShopDiv + "'></div>";
                                }
                                if (Ob.OptLogo == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.StrMainBarCode + "'><img src='../Logo/Dry" + BranchId + ".jpg' width='32' Height='32'></img></div>";
                                }
                            }
                        }
                        for (Ob.Loopj = 1; Ob.Loopj <= 9; Ob.Loopj++)
                        {
                            if (Ob.Loopj == Int32.Parse(Ob.BookingPosition.ToString()))
                            {
                                if (Ob.OptBooking == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.BookingNo + "' >" + ds.Tables[0].Rows[Ob.Loopi]["BookingNo"].ToString() + "</div>";
                                }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.CustomerPosition.ToString()))
                            {
                                if (Ob.OptCustomer == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Customer + "' >" + ds.Tables[0].Rows[Ob.Loopi]["CustomerName"].ToString() + "</div>";
                                }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.AddressPosition.ToString()))
                            {
                                if (Ob.OptAddress == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Address + "' >" + ds.Tables[0].Rows[Ob.Loopi]["CustomerAddress"].ToString() + "</div>";
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.bdateposition.ToString()))
                            {
                                if (Ob.OptBookingDate == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.bookingdate + "' >" + ds.Tables[0].Rows[Ob.Loopi]["BookingDate"].ToString() + "</div>";
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.ProcessPosition.ToString()))
                            {
                                if (Ob.OptProcess == true)
                                {
                                    Ob.Process = ds.Tables[0].Rows[Ob.Loopi]["Process"].ToString();
                                    Ob.ExtProcess = ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType"].ToString();
                                    Ob.ExtProcessSecond = ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType2"].ToString();
                                }

                                //if (Ob.OptExtraProcess == true)
                                //{
                               
                                //}

                                //if (Ob.OptExtraProcessSecond == true)
                                //{
                                
                                //}
                               
                                if (Ob.OptSubtotal == true)
                                {
                                    Ob.OptSubtotal1 = ds.Tables[0].Rows[Ob.Loopi]["ItemTotalAndSubTotal"].ToString();
                                   
                                }
                                
                                string Ext1Processimg = string.Empty, Ext2Processimg = string.Empty, MainProcessimg = string.Empty;
                                bool maprocess = false, ext1 = false, ext2 = false;
                                string abc = "<img src='../ProcessWiseLogo/" + Ob.Process + ".png'" + " width='20' Height='20' styles='vertical-align: middle;'></img>";
                                MainProcessimg = abc.Replace("&nbsp;", "");
                                string test = Ob.Process + ".png";
                                string tt = test.Replace("&nbsp;", "");
                                DirectoryInfo dir1 = new DirectoryInfo(ImageUrl);
                                FileInfo[] Folder1Files = dir1.GetFiles();
                                if (Folder1Files.Length > 0)
                                {
                                    foreach (FileInfo aFile in Folder1Files)
                                    {
                                        if (aFile.Name == tt)
                                        {
                                            maprocess = true;
                                        }
                                    }
                                }
                                if (maprocess == false)
                                {
                                    MainProcessimg = "";
                                }
                                if (Ob.OptProcess == true || Ob.OptExtraProcess == true || Ob.OptExtraProcessSecond == true || Ob.OptSubtotal == true)
                                {
                                    string def = "<img src='../ProcessWiseLogo/" + Ob.ExtProcess + ".png'" + " width='20' Height='20' styles='vertical-align: middle;'></img>";
                                    Ext1Processimg = def.Replace("&nbsp;", "");
                                    string ghi = "<img src='../ProcessWiseLogo/" + Ob.ExtProcessSecond + ".png'" + " width='20' Height='20' styles=vertical-align: middle;'></img>";
                                    Ext2Processimg = ghi.Replace("&nbsp;", "");
                                    string test1 = Ob.ExtProcess + ".png";
                                    string tt1 = test1.Replace("&nbsp;", "");
                                    string test2 = Ob.ExtProcessSecond + ".png";
                                    string tt2 = test2.Replace("&nbsp;", "");
                                    if (Folder1Files.Length > 0)
                                    {
                                        foreach (FileInfo aFile in Folder1Files)
                                        {
                                            if (aFile.Name == tt1)
                                            {
                                                ext1 = true;
                                            }
                                        }
                                    }
                                    if (ext1 == false)
                                    {
                                        Ext1Processimg = "";
                                    }
                                    if (Folder1Files.Length > 0)
                                    {
                                        foreach (FileInfo aFile in Folder1Files)
                                        {
                                            if (aFile.Name == tt2)
                                            {
                                                ext2 = true;
                                            }
                                        }
                                    }
                                    if (ext2 == false)
                                    {
                                        Ext2Processimg = "";
                                    }
                                    if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType"].ToString() != "None")
                                    {
                                        if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType2"].ToString() != "None")
                                        {
                                            Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + MainProcessimg + "<div style='position:relative;float:right;margin-top:4px;margin-right:10%;'>" + Ob.OptSubtotal1 + "</div>" + "<div style='float:right;margin-top:4px;margin-right:3%;padding:0px 0px 0px 0px;'>" + Ob.Process + "</div>" + Ext1Processimg + "<div style='float:right;margin-left:0px;margin-top:4px;margin-right:2%;'>" + Ob.ExtProcess + "</div>" + Ext2Processimg + "<div style='float:right;margin-left:1px;margin-top:4px;margin-right:2%'>" + Ob.ExtProcessSecond + "</div></div>";
                                        }
                                        else
                                        {
                                            Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + MainProcessimg + "<div style='position:relative;float:right;margin-top:4px;margin-right:20%;'>" + Ob.OptSubtotal1 + "</div>" + "<div style='float:right;position:relative;margin-top:4px;margin-right:3%;padding:0px 0px 0px 0px;'>" + Ob.Process + "</div>" + Ext1Processimg + "<div style='float:right;margin-left:2px;margin-right:1px;margin-top:4px;'>" + "  " + Ob.ExtProcess + "</div></div>";
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType2"].ToString() != "None")
                                    {
                                        if (ds.Tables[0].Rows[Ob.Loopi]["ItemExtraprocessType"].ToString() == "None")

                                            Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + MainProcessimg + "<div style='float:right;position:relative;margin-top:4px;margin-right:75px;padding-left:0px;'>" + Ob.Process + "</div>" + Ext2Processimg + "<div style='float:right;margin-left:2px;margin-right:1px;margin-top:-16px;'>" + " " + Ob.ExtProcessSecond + " " + Ob.OptSubtotal1 + "</div></div>";
                                    }
                                    else
                                    {
                                        Ob.StrPreviewBarCode += "<div  style='" + Ob.CustName + "' >" + MainProcessimg + "<div style='position:relative;float:right;margin-top:4px;margin-right:20%;'>" + Ob.OptSubtotal1 + "</div>"+"<div style='position:relative;float:right;margin-top:4px;margin-right:20%;'>" + Ob.Process + "</div></div>";
                                    }
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.BarPosition.ToString()))
                            {
                                if (Ob.OptPrint == true)
                                {                                   
                                   Ob.StrPreviewBarCode += "<div style='" + Ob.BarCode + "'>" + ds.Tables[0].Rows[Ob.Loopi]["Barcode"].ToString() + "</div>";
                                }
                            }

                            if (Ob.Loopj == Int32.Parse(Ob.ItemPosition.ToString()))
                            {
                                if (Ob.OptItem == true)
                                {
                                    Ob.ItemName = ds.Tables[0].Rows[Ob.Loopi]["Item"].ToString();
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Item + "'>" + Ob.ItemName + "</div>";
                                }

                                else
                                { }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.DueDatePosition.ToString()))
                            {
                                if (Ob.OptDueDate == true)
                                {
                                    Ob.DueDate = ds.Tables[0].Rows[Ob.Loopi]["DueDate"].ToString();
                                }

                                if (Ob.OptTime == true)
                                {
                                    Ob.CurrentTime = ds.Tables[0].Rows[Ob.Loopi]["CurrentTime"].ToString();
                                    //Ob.OptSubtotal1 = ds.Tables[0].Rows[Ob.Loopi]["ItemTotalAndSubTotal"].ToString();
                                }
                                if (Ob.OptDueDate == true || Ob.OptTime == true)
                                {
                                    Ob.StrPreviewBarCode += "<div style='" + Ob.Time + "'>" + Ob.DueDate + " " + Ob.CurrentTime + "</div>";
                                }
                            }
                            if (Ob.Loopj == Int32.Parse(Ob.RemarkPosition.ToString()))
                            {
                                if (ds.Tables[0].Rows[Ob.Loopi]["Colour"].ToString() != "" && Ob.OptColour == true)
                                {
                                    if (ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() != "" && Ob.OptRemark == true)
                                    {
                                        Ob.StrPreviewBarCode += "<div style='" + Ob.Remark + "'>" + ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() + "," + ds.Tables[0].Rows[Ob.Loopi]["Colour"].ToString() + "</div>";
                                    }
                                    else
                                    {
                                        if (Ob.OptColour == true)
                                        {
                                            Ob.StrPreviewBarCode += "<div style='" + Ob.Remark + "'>" + ds.Tables[0].Rows[Ob.Loopi]["Colour"].ToString() + "</div>";
                                        }
                                    }
                                }
                                else
                                {
                                    if (ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() != "" && Ob.OptRemark == true)
                                    {
                                        Ob.StrPreviewBarCode += "<div style='" + Ob.Remark + "'>" + ds.Tables[0].Rows[Ob.Loopi]["ItemRemark"].ToString() + "</div>";
                                    }
                                }
                            }
                        }
                        Ob.StrPreviewBarCode += "<div style=margin-top:-2px;>--------------------</div>";
                        Ob.StrPreviewBarCode += "</div>";
                        //Ob.StrPreviewBarCode += "<div style=height:5px;position:absolute;>" + "</div>";
                    }
                }
            }

            return Ob;
        }

    }
}