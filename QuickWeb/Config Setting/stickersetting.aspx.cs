using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;

namespace QuickWeb.Config_Setting
{
    public partial class stickersetting : System.Web.UI.Page
    {
        public string strPreview = string.Empty;
        public string strPreviewbarcode = string.Empty;
        public string barcodewidth = string.Empty;
        public string barcodeheight = string.Empty;
        public string oldbarcodewidth = string.Empty;
        public string oldbarcodehight = string.Empty;
        public double linehight = 0.00;
        private DTO.Sticker Ob = new DTO.Sticker();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                barcodeid.Visible = false;
                bookingid.Visible = false;
                customerid.Visible = false;
                addressid.Visible = false;
                processid.Visible = false;
                remarkid.Visible = false;
                itemid.Visible = false;
                dividerid.Visible = false;
                widthid.Visible = false;
                barcodeid.Visible = false;
                barcodealignid.Visible = false;
                bookingyesid.Visible = false;
                barcodeyesid.Visible = false;
                barcodepanel.Visible = true;
                panelwidth.Visible = true;
                barcodepaneldisplay.Visible = true;
                BindFont();
                for (int j = 18; j >= 5; j--)
                {
                    drpcussize.Items.Add(j.ToString());
                    Dropremarksize.Items.Add(j.ToString());
                    Dropitemsize.Items.Add(j.ToString());
                    Drpnamesize.Items.Add(j.ToString());
                    Drpaddresssize.Items.Add(j.ToString());
                }

                for (int j = 36; j >= 5; j--)
                {
                    drpBookingNosize.Items.Add(j.ToString());
                }

                for (int k = 1; k <= 7; k++)
                {
                    Drpbookinfposition.Items.Add(k.ToString());
                    Drpcusposition.Items.Add(k.ToString());
                    Drpprocessposition.Items.Add(k.ToString());
                    drpremarkposition.Items.Add(k.ToString());
                    drpbarcodeposition.Items.Add(k.ToString());
                    Drpitemposition.Items.Add(k.ToString());
                    drpaddressposition.Items.Add(k.ToString());
                }

                SetfetchValue();
                fetchbarcodevalue();

                fetchbarcodeconfigsetting();
            }
        }

        // -----------------------------barcode display--------------------------
        public void fetchbarcodeconfigsetting()
        {
            DataSet dset = new DataSet();
            dset = BAL.BALFactory.Instance.BAL_Sticker.fetchbarcodeconfig(Ob);
            if (dset.Tables[0].Rows.Count > 0)
            {
                bool barcodebookingno = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodebookingno"].ToString());
                string bookingfont = dset.Tables[0].Rows[0]["bookingfont"].ToString();
                string bookingsize = dset.Tables[0].Rows[0]["bookingsize"].ToString();
                string bookingalign = dset.Tables[0].Rows[0]["bookingalign"].ToString();
                string bookingbold = dset.Tables[0].Rows[0]["bookingbold"].ToString();
                string bookingitilic = dset.Tables[0].Rows[0]["bookingitilic"].ToString();
                string bookingunderline = dset.Tables[0].Rows[0]["bookingunderline"].ToString();
                bool barcodeprocess = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeprocess"].ToString());
                bool barcodexteraprocess = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodexteraprocess"].ToString());
                bool barcodeextraprocesssecond = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeextraprocesssecond"].ToString());
                bool barcodesubtotal = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodesubtotal"].ToString());
                string processfont = dset.Tables[0].Rows[0]["processfont"].ToString();
                string processsize = dset.Tables[0].Rows[0]["processsize"].ToString();
                string processalign = dset.Tables[0].Rows[0]["processalign"].ToString();
                string processbold = dset.Tables[0].Rows[0]["processbold"].ToString();
                string processitalic = dset.Tables[0].Rows[0]["processitalic"].ToString();
                string processunderline = dset.Tables[0].Rows[0]["processunderline"].ToString();
                bool barcoderemark = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcoderemark"].ToString());
                bool barcodecolour = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodecolour"].ToString());
                string remarkfont = dset.Tables[0].Rows[0]["remarkfont"].ToString();
                string remarksize = dset.Tables[0].Rows[0]["remarksize"].ToString();
                string remarkremarkalign = dset.Tables[0].Rows[0]["remarkremarkalign"].ToString();
                string remarkbold = dset.Tables[0].Rows[0]["remarkbold"].ToString();
                string remarkitalic = dset.Tables[0].Rows[0]["remarkitalic"].ToString();
                string remarkunderline = dset.Tables[0].Rows[0]["remarkunderline"].ToString();
                bool barcodeprint = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeprint"].ToString());
                // string barcodesize = dset.Tables[0].Rows[0]["barcodesize"].ToString();
                string barcodealign = dset.Tables[0].Rows[0]["barcodealign"].ToString();
                bool barcodeitem = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeitem"].ToString());
                bool barcodeduedate = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeduedate"].ToString());
                bool barcodetime = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodetime"].ToString());
                string itemfont = dset.Tables[0].Rows[0]["itemfont"].ToString();
                string itemsize = dset.Tables[0].Rows[0]["itemsize"].ToString();
                string itembold = dset.Tables[0].Rows[0]["itembold"].ToString();
                string itemitalic = dset.Tables[0].Rows[0]["itemitalic"].ToString();
                string itemalign = dset.Tables[0].Rows[0]["itemalign"].ToString();
                string itemunderline = dset.Tables[0].Rows[0]["itemunderline"].ToString();

                bool barcodecustomer = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodecusname"].ToString());
                string customerfont = dset.Tables[0].Rows[0]["cusfont"].ToString();
                string customersize = dset.Tables[0].Rows[0]["cussize"].ToString();
                string customeralign = dset.Tables[0].Rows[0]["cusalign"].ToString();
                string customerbold = dset.Tables[0].Rows[0]["cusbold"].ToString();
                string customeritilic = dset.Tables[0].Rows[0]["cusitalic"].ToString();
                string customerunderline = dset.Tables[0].Rows[0]["cusunderline"].ToString();

                string bookingnoposition = dset.Tables[0].Rows[0]["bookingnoposition"].ToString();
                string customerposition = dset.Tables[0].Rows[0]["cusposition"].ToString();
                string processposition = dset.Tables[0].Rows[0]["processposition"].ToString();
                string remarkposition = dset.Tables[0].Rows[0]["remarkposition"].ToString();
                string barcodeposition = dset.Tables[0].Rows[0]["barcodeposition"].ToString();
                string itemposition = dset.Tables[0].Rows[0]["itemposition"].ToString();

                string addressposition = dset.Tables[0].Rows[0]["Addressposition"].ToString();
                bool barcodeaddress = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeaddress"].ToString());
                string addressfont = dset.Tables[0].Rows[0]["addfont"].ToString();
                string addresssize = dset.Tables[0].Rows[0]["addsize"].ToString();
                string addressalign = dset.Tables[0].Rows[0]["addalign"].ToString();
                string addressbold = dset.Tables[0].Rows[0]["addbold"].ToString();
                string addressitilic = dset.Tables[0].Rows[0]["additalic"].ToString();
                string addressunderline = dset.Tables[0].Rows[0]["addunderline"].ToString();
                bool barcodedivider = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodedivider"].ToString());

                oldbarcodewidth = dset.Tables[0].Rows[0]["barcodewidth"].ToString();
                oldbarcodehight = dset.Tables[0].Rows[0]["barcodeheight"].ToString();

                string[] strArr1 = oldbarcodehight.Split('i');
                double barchight = Convert.ToDouble(strArr1[0]) - 0.5;
                double divhight = Math.Round(barchight / 7, 2);

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

                string custName = string.Empty;
                string strMainBarcode = "width:" + oldbarcodewidth + ";height:" + oldbarcodehight + ";font-size:6px;text-align:center;overflow:hidden;";
                string bookingNo = "width:" + oldbarcodewidth + ";position: relative;font-family:" + bookingfont + ";font-size:" + bookingsize + "px;height:35px;font-weight:" + bookingbold + ";font-style:" + bookingitilic + "; text-decoration:" + bookingunderline + ";text-align:" + bookingalign + ";line-height:125%;";
                string customer = "width:" + oldbarcodewidth + ";position: relative;font-family:" + customerfont + ";font-size:" + customersize + "px;height:15px;font-weight:" + customerbold + ";font-style:" + customeritilic + "; text-decoration:" + customerunderline + ";text-align:" + customeralign + ";text-transform:capitalize;line-height:125%;";
                string address = "width:" + oldbarcodewidth + ";position: relative;font-family:" + addressfont + ";font-size:" + addresssize + "px;height:16px;font-weight:" + addressbold + ";font-style:" + addressitilic + "; text-decoration:" + addressunderline + ";text-align:" + addressalign + ";text-transform:capitalize;";
                string divider = "width:" + oldbarcodewidth + ";position: relative;height:;line-height:20%;margin-top:1px;font-weight:bold;font-family:Arial Black";
                custName = "width:" + oldbarcodewidth + ";position: relative;font-family:" + processfont + ";font-size:" + processsize + "px;height:15px;font-weight:" + processbold + ";font-style:" + processitalic + "; text-decoration:" + processunderline + ";text-align:" + processalign + ";text-transform:capitalize;";
                string barcode = "width:" + oldbarcodewidth + "; position: relative;font-family: MRV Code39extMA;height:.5in; font-size:12px;top:4px;text-align:" + barcodealign + ";";
                string time = string.Empty;
                time = "width:" + oldbarcodewidth + ";position:relative;font-size:" + itemsize + "px;top:1.5px;text-align:" + itemalign + ";height:15px;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";word-spacing:-3px; font-family:" + itemfont + ";";
                string date = "width:" + oldbarcodewidth + ";position: relative;font-family:" + itemfont + ";font-size:" + itemsize + "px;height:15px;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";text-align:" + itemalign + ";word-spacing:-1px;";
                string remark = "width:" + oldbarcodewidth + ";position: relative;font-family:" + remarkfont + ";font-size:" + remarksize + "px;top:2.5px;height:15px;word-spacing:-2px;text-align:" + remarkremarkalign + ";font-weight:" + remarkbold + ";font-style:" + remarkitalic + "; text-decoration:" + remarkunderline + ";text-transform:capitalize;";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strPreviewbarcode += "<div style='" + strMainBarcode + "'>";
                    if (barcodedivider == true)
                    {
                        strPreviewbarcode += "<div style='" + divider + "' >" + "------------------------" + "</div>";
                    }
                    else
                    {
                    }
                    for (int j = 1; j <= 7; j++)
                    {
                        if (j == Int32.Parse(bookingnoposition.ToString()))
                        {
                            if (barcodebookingno == true)
                            {
                                strPreviewbarcode += "<div style='" + bookingNo + "' >" + ds.Tables[0].Rows[i]["BookingNo"].ToString() + "</div>";
                            }
                            else
                            {
                                //strPreviewbarcode += "<div style='" + bookingNo + "' >" + "</div>";
                            }
                        }
                        if (j == Int32.Parse(customerposition.ToString()))
                        {
                            if (barcodecustomer == true)
                            {
                                strPreviewbarcode += "<div style='" + customer + "' >" + ds.Tables[0].Rows[i]["CustomerName"].ToString() + "</div>";
                            }
                            else
                            {
                                //strPreviewbarcode += "<div style='" + customer + "' >" + "</div>";
                            }
                        }
                        if (j == Int32.Parse(addressposition.ToString()))
                        {
                            if (barcodeaddress == true)
                            {
                                strPreviewbarcode += "<div style='" + address + "' >" + ds.Tables[0].Rows[i]["CustomerAddress"].ToString() + "</div>";
                            }
                            else
                            {
                                //strPreviewbarcode += "<div style='" + address + "' >" + "</div>";
                            }
                        }

                        string process = string.Empty;
                        string extraprocess = string.Empty;
                        string extraprocesssecond = string.Empty;
                        string subtotal = string.Empty;
                        if (j == Int32.Parse(processposition.ToString()))
                        {
                            if (barcodeprocess == true)
                            {
                                process = "&nbsp;" + ds.Tables[0].Rows[i]["Process"].ToString();
                            }

                            if (barcodexteraprocess == true)
                            {
                                extraprocess = "&nbsp;" + ds.Tables[0].Rows[i]["ItemExtraprocessType"].ToString();
                            }

                            if (barcodeextraprocesssecond == true)
                            {
                                extraprocesssecond = "&nbsp;" + ds.Tables[0].Rows[i]["ItemExtraprocessType2"].ToString();
                            }

                            if (barcodesubtotal == true)
                            {
                                subtotal = "&nbsp;" + ds.Tables[0].Rows[i]["ItemTotalAndSubTotal"].ToString();
                            }

                            if (barcodeprocess == true || barcodexteraprocess == true || barcodeextraprocesssecond == true || barcodesubtotal == true)
                            {
                                if (ds.Tables[0].Rows[i]["ItemExtraprocessType"].ToString() != "None")
                                {
                                    if (ds.Tables[0].Rows[i]["ItemExtraprocessType2"].ToString() != "None")
                                        strPreviewbarcode += "<div  style='" + custName + "' >" + process + "" + extraprocess + "" + extraprocesssecond + "" + subtotal + "</div>";
                                    else
                                        strPreview += "<div  style='" + custName + "' >" + process + "" + extraprocess + "" + subtotal + "</div>";
                                }
                                else if (ds.Tables[0].Rows[i]["ItemExtraprocessType2"].ToString() != "None")
                                {
                                    if (ds.Tables[0].Rows[i]["ItemExtraprocessType"].ToString() == "None")
                                        strPreviewbarcode += "<div  style='" + custName + "' >" + process + "" + extraprocesssecond + "" + subtotal + "</div>";
                                }
                                else
                                    strPreviewbarcode += "<div  style='" + custName + "' >" + process + "" + subtotal + "</div>";
                            }
                        }

                        if (j == Int32.Parse(barcodeposition.ToString()))
                        {
                            if (barcodeprint == true)
                            {
                                strPreviewbarcode += "<div style='" + barcode + "'>" + ds.Tables[0].Rows[i]["Barcode"].ToString() + "</div>";
                            }
                        }
                        string item = string.Empty;
                        if (j == Int32.Parse(itemposition.ToString()))
                        {
                            if (barcodeitem == true)
                            {
                                item = ds.Tables[0].Rows[i]["Item"].ToString();
                            }
                            else
                            { }
                            string duedate = string.Empty;
                            if (barcodeduedate == true)
                            {
                                duedate = "&nbsp;&nbsp;" + ds.Tables[0].Rows[i]["DueDate"].ToString();
                            }
                            string currenttime = string.Empty;
                            if (barcodetime == true)
                            {
                                currenttime = "&nbsp;&nbsp;" + ds.Tables[0].Rows[i]["CurrentTime"].ToString();
                            }
                            if (barcodeitem == true || barcodeduedate == true || barcodetime == true)
                            {
                                strPreviewbarcode += "<div style='" + time + "'>" + item + "&nbsp;" + "<spam style='" + date + "'>" + "" + duedate + "" + currenttime + "</spam></div>";
                            }
                        }
                        if (j == Int32.Parse(remarkposition.ToString()))
                        {
                            if (ds.Tables[0].Rows[i]["Colour"].ToString() != "" && barcodecolour == true)
                            {
                                if (ds.Tables[0].Rows[i]["ItemRemark"].ToString() != "" && barcoderemark == true)
                                {
                                    strPreviewbarcode += "<div style='" + remark + "'>" + ds.Tables[0].Rows[i]["ItemRemark"].ToString() + "," + ds.Tables[0].Rows[i]["Colour"].ToString() + "</div>";
                                }
                                else
                                {
                                    if (barcodecolour == true)
                                    {
                                        strPreviewbarcode += "<div style='" + remark + "'>" + ds.Tables[0].Rows[i]["Colour"].ToString() + "</div>";
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[i]["ItemRemark"].ToString() != "" && barcoderemark == true)
                                {
                                    strPreviewbarcode += "<div style='" + remark + "'>" + ds.Tables[0].Rows[i]["ItemRemark"].ToString() + "</div>";
                                }
                                else
                                {
                                    //strPreviewbarcode += "<div style='" + remark + "'>" + "</div>";
                                }
                            }
                        }
                    }

                    strPreviewbarcode += "</div>";
                }
                sqlcon.Close();
                sqlcon.Dispose();
            }
        }

        //----------------------------end--------------------------------------------

        public void BindFont()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily font in fonts.Families)
            {
                DropBookingNofont.Items.Add(font.Name);
                drpcusfont.Items.Add(font.Name);
                Dropremarkfont.Items.Add(font.Name);
                Dropitemfont.Items.Add(font.Name);
                drpfontname.Items.Add(font.Name);
                Drpaddressfont.Items.Add(font.Name);
            }
        }

        public DTO.Sticker SetfetchValue()
        {
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        public DTO.Sticker SetValue()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.barcodeconfigbooking = (RadioBookingNoyes.Checked ? "1" : "0");
            Ob.barcodebookingfont = DropBookingNofont.SelectedItem.ToString();
            Ob.barcodebookingsize = drpBookingNosize.SelectedItem.ToString();
            Ob.barcodebookingalign = DropBookingNoalign.SelectedItem.ToString();
            Ob.barcodebookingbold = (CheckBookingNobould.Checked ? "Bold" : "");
            Ob.barcodebookingitalic = (CheckBookingNoitilice.Checked ? "Italic" : "");
            Ob.barcodebookingunderline = (CheckBookingNounderline.Checked ? "Underline" : "");

            Ob.barcodeconfigprocess = (RadioProcessyes.Checked ? "1" : "0");
            //Ob.barcodeconfigextraprocess = (RadioExtraprocessyes.Checked ? "1" : "0");
            //Ob.barcodeconfigextraprocesssecond = (RadioExtraprocesstype2yes.Checked ? "1" : "0");
            Ob.barcodeconfigprocesssubtotal = (RadioItemTotalandSubTotalyes.Checked ? "1" : "0");
            Ob.barcodeprocessfont = drpcusfont.SelectedItem.ToString();
            Ob.barcodeprocesssize = drpcussize.SelectedItem.ToString();
            Ob.barcodeprocessalign = Dropcuscolour.SelectedItem.ToString();
            Ob.barcodeprocessbold = (Checkbould.Checked ? "Bold" : "");
            Ob.barcodeprocessitalic = (Checkitilace.Checked ? "Italic" : "");
            Ob.barcodeprocessunderline = (Checkunderline.Checked ? "Underline" : "");

            Ob.barcodeconfigremark = (Radioremarkyes.Checked ? "1" : "0");
            Ob.barcodeconfigcolour = (Radiocoloursyes.Checked ? "1" : "0");
            Ob.barcoderemarkfont = Dropremarkfont.SelectedItem.ToString();
            Ob.barcodremarksize = Dropremarksize.SelectedItem.ToString();
            Ob.barcoderemarkalign = Dropremarkalign.SelectedItem.ToString();
            Ob.barcoderemarkbold = (Checkremarkbould.Checked ? "Bold" : "");
            Ob.barcoderemarkitalic = (Checkremarkitilice.Checked ? "Italic" : "");
            Ob.barcoderemarkunderline = (Checkremarkunderline.Checked ? "Underline" : "");

            Ob.barcodeconfigbarcode = (RadioBarcodeyes.Checked ? "1" : "0");
            // Ob.barcodebarcodesize = Dropbarcodesize.SelectedItem.ToString();
            Ob.barcodebarcodealign = Dropbarcodealign.SelectedItem.ToString();

            Ob.barcodeconfigitem = (RadioItemyes.Checked ? "1" : "0");
            Ob.barcodeconfigduedate = (RadioDateyes.Checked ? "1" : "0");
            Ob.barcodeconfigtime = (Radiotimeyes.Checked ? "1" : "0");
            Ob.barcodeitemfont = Dropitemfont.SelectedItem.ToString();
            Ob.barcodeitemsize = Dropitemsize.SelectedItem.ToString();
            Ob.barcodeitemalign = Dropitemalign.SelectedItem.ToString();
            Ob.barcodeitembold = (Checkitembould.Checked ? "Bold" : "");
            Ob.barcodeitemitalic = (Checkitemitilice.Checked ? "Italic" : "");
            Ob.barcodeitemunderline = (Checkitemunderline.Checked ? "Underline" : "");

            Ob.barcodeconfigcustomer = (radiocusyes.Checked ? "1" : "0");
            Ob.barcodecustomerfont = drpfontname.SelectedItem.ToString();
            Ob.barcodecustomerize = Drpnamesize.SelectedItem.ToString();
            Ob.barcodecustomeralign = Drpcusalign.SelectedItem.ToString();
            Ob.barcodecustomerbold = (Checknamebold.Checked ? "Bold" : "");
            Ob.barcodecustomeritalic = (Checknameitalac.Checked ? "Italic" : "");
            Ob.barcodecustomerunderline = (Checknameunderline.Checked ? "Underline" : "");

            Ob.bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            Ob.customerposition = Drpcusposition.SelectedItem.ToString();
            Ob.processposition = Drpprocessposition.SelectedItem.ToString();
            Ob.remarkposition = drpremarkposition.SelectedItem.ToString();
            Ob.barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            Ob.itemposition = Drpitemposition.SelectedItem.ToString();
            Ob.addressposition = drpaddressposition.SelectedItem.ToString();
            Ob.barcodeconfigaddress = (Radioaddressyes.Checked ? "1" : "0");
            Ob.barcodeaddressfont = Drpaddressfont.SelectedItem.ToString();
            Ob.barcodeaddresssize = Drpaddresssize.SelectedItem.ToString();
            Ob.barcodeaddressalign = Drpaddressalign.SelectedItem.ToString();
            Ob.barcodeaddressbold = (Checkaddressbold.Checked ? "Bold" : "");
            Ob.barcodeaddressitalic = (Checkaddressitalic.Checked ? "Italic" : "");
            Ob.barcodeaddressunderline = (Checkaddressunderline.Checked ? "Underline" : "");
            Ob.barcodedivider = (Radiodivideryes.Checked ? "1" : "0");
            return Ob;
        }

        public void fetchbarcodevalue()
        {
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_Sticker.fetchbarcodeconfig(Ob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RadioBookingNoyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodebookingno"].ToString());
                if (RadioBookingNoyes.Checked != true)
                {
                    RadioBookingNorno.Checked = true;
                }
                else
                {
                    RadioBookingNoyes.Checked = true;
                }
                PrjClass.SetItemInDropDown(DropBookingNofont, ds.Tables[0].Rows[0]["bookingfont"].ToString(), true, false);
                PrjClass.SetItemInDropDown(drpBookingNosize, ds.Tables[0].Rows[0]["bookingsize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(DropBookingNoalign, ds.Tables[0].Rows[0]["bookingalign"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["bookingbold"].ToString() != "")
                {
                    CheckBookingNobould.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["bookingitilic"].ToString() != "")
                {
                    CheckBookingNoitilice.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["bookingunderline"].ToString() != "")
                {
                    CheckBookingNounderline.Checked = true;
                }

                RadioProcessyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeprocess"].ToString());
                if (RadioProcessyes.Checked != true)
                {
                    RadioProcessno.Checked = true;
                }
                else
                {
                    RadioProcessyes.Checked = true;
                }

                RadioItemTotalandSubTotalyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodesubtotal"].ToString());
                if (RadioItemTotalandSubTotalyes.Checked != true)
                {
                    RadioItemTotalandSubTotalno.Checked = true;
                }
                else
                {
                    RadioItemTotalandSubTotalyes.Checked = true;
                }
                PrjClass.SetItemInDropDown(drpcusfont, ds.Tables[0].Rows[0]["processfont"].ToString(), true, false);
                PrjClass.SetItemInDropDown(drpcussize, ds.Tables[0].Rows[0]["processsize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropcuscolour, ds.Tables[0].Rows[0]["processalign"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["processbold"].ToString() != "")
                {
                    Checkbould.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["processitalic"].ToString() != "")
                {
                    Checkitilace.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["processunderline"].ToString() != "")
                {
                    Checkunderline.Checked = true;
                }

                Radioremarkyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcoderemark"].ToString());
                if (Radioremarkyes.Checked != true)
                {
                    Radioremarkno.Checked = true;
                }
                else
                {
                    Radioremarkyes.Checked = true;
                }
                Radiocoloursyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodecolour"].ToString());
                if (Radiocoloursyes.Checked != true)
                {
                    Radiocoloursno.Checked = true;
                }
                else
                {
                    Radiocoloursyes.Checked = true;
                }
                PrjClass.SetItemInDropDown(Dropremarkfont, ds.Tables[0].Rows[0]["remarkfont"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropremarksize, ds.Tables[0].Rows[0]["remarksize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropremarkalign, ds.Tables[0].Rows[0]["remarkremarkalign"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["remarkbold"].ToString() != "")
                {
                    Checkremarkbould.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["remarkitalic"].ToString() != "")
                {
                    Checkremarkitilice.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["remarkunderline"].ToString() != "")
                {
                    Checkremarkunderline.Checked = true;
                }

                RadioBarcodeyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeprint"].ToString());
                if (RadioBarcodeyes.Checked != true)
                {
                    RadioBarcodeno.Checked = true;
                }
                else
                {
                    RadioBarcodeyes.Checked = true;
                }
                // PrjClass.SetItemInDropDown(Dropbarcodesize, ds.Tables[0].Rows[0]["barcodesize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropbarcodealign, ds.Tables[0].Rows[0]["barcodealign"].ToString(), true, false);

                RadioItemyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeitem"].ToString());
                if (RadioItemyes.Checked != true)
                {
                    RadioItemno.Checked = true;
                }
                else
                {
                    RadioItemyes.Checked = true;
                }
                RadioDateyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeduedate"].ToString());
                if (RadioDateyes.Checked != true)
                {
                    RadioDateno.Checked = true;
                }
                else
                {
                    RadioDateyes.Checked = true;
                }
                Radiotimeyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodetime"].ToString());
                if (Radiotimeyes.Checked != true)
                {
                    Radiotimeno.Checked = true;
                }
                else
                {
                    Radiotimeyes.Checked = true;
                }
                PrjClass.SetItemInDropDown(Dropitemfont, ds.Tables[0].Rows[0]["itemfont"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropitemsize, ds.Tables[0].Rows[0]["itemsize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropitemalign, ds.Tables[0].Rows[0]["itemalign"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["itembold"].ToString() != "")
                {
                    Checkitembould.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itemitalic"].ToString() != "")
                {
                    Checkitemitilice.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itemunderline"].ToString() != "")
                {
                    Checkitemunderline.Checked = true;
                }

                radiocusyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodecusname"].ToString());
                if (radiocusyes.Checked != true)
                {
                    Radiocusno.Checked = true;
                }
                else
                {
                    radiocusyes.Checked = true;
                }
                PrjClass.SetItemInDropDown(drpfontname, ds.Tables[0].Rows[0]["cusfont"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpnamesize, ds.Tables[0].Rows[0]["cussize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpcusalign, ds.Tables[0].Rows[0]["cusalign"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["cusbold"].ToString() != "")
                {
                    Checknamebold.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["cusitalic"].ToString() != "")
                {
                    Checknameitalac.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["cusunderline"].ToString() != "")
                {
                    Checknameunderline.Checked = true;
                }

                Radiodivideryes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodedivider"].ToString());
                if (Radiodivideryes.Checked != true)
                {
                    Radiodividerno.Checked = true;
                }
                else
                {
                    Radiodivideryes.Checked = true;
                }

                Radioaddressyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeaddress"].ToString());
                if (Radioaddressyes.Checked != true)
                {
                    Radioaddressno.Checked = true;
                }
                else
                {
                    Radioaddressyes.Checked = true;
                }
                PrjClass.SetItemInDropDown(Drpaddressfont, ds.Tables[0].Rows[0]["addfont"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpaddresssize, ds.Tables[0].Rows[0]["addsize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpaddressalign, ds.Tables[0].Rows[0]["addalign"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["addbold"].ToString() != "")
                {
                    Checkaddressbold.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["additalic"].ToString() != "")
                {
                    Checkaddressitalic.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["addunderline"].ToString() != "")
                {
                    Checkaddressunderline.Checked = true;
                }

                PrjClass.SetItemInDropDown(Drpbookinfposition, ds.Tables[0].Rows[0]["bookingnoposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpcusposition, ds.Tables[0].Rows[0]["cusposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpprocessposition, ds.Tables[0].Rows[0]["processposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(drpremarkposition, ds.Tables[0].Rows[0]["remarkposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(drpbarcodeposition, ds.Tables[0].Rows[0]["barcodeposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Drpitemposition, ds.Tables[0].Rows[0]["itemposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(drpaddressposition, ds.Tables[0].Rows[0]["Addressposition"].ToString(), true, false);
                PrjClass.SetItemInDropDown(barcodehight, ds.Tables[0].Rows[0]["barcodeheight"].ToString(), true, false);
                PrjClass.SetItemInDropDown(barcodewirth, ds.Tables[0].Rows[0]["barcodewidth"].ToString(), true, false);
            }
        }

        protected void btbarcodesave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            string bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            string customerposition = Drpcusposition.SelectedItem.ToString();
            string processposition = Drpprocessposition.SelectedItem.ToString();
            string remarkposition = drpremarkposition.SelectedItem.ToString();
            string barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            string itemposition = Drpitemposition.SelectedItem.ToString();
            string addressposition = drpaddressposition.SelectedItem.ToString();
            string msg = string.Empty;
            if (bookingnoposition == customerposition || addressposition == bookingnoposition || bookingnoposition == processposition || bookingnoposition == remarkposition || bookingnoposition == barcodeposition || bookingnoposition == itemposition)
            {
                msg = msg + bookingnoposition + "&nbsp;";
                //lblMsgbarcode.Text =bookingnoposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (bookingnoposition == customerposition || addressposition == customerposition || customerposition == processposition || customerposition == remarkposition || customerposition == barcodeposition || customerposition == itemposition)
            {
                msg = msg + customerposition + "&nbsp;";

                //lblMsgbarcode.Text =customerposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (processposition == customerposition || addressposition == processposition || bookingnoposition == processposition || processposition == remarkposition || processposition == barcodeposition || processposition == itemposition)
            {
                msg = msg + processposition + "&nbsp;";
                //lblMsgbarcode.Text =processposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (remarkposition == customerposition || addressposition == remarkposition || bookingnoposition == remarkposition || processposition == remarkposition || remarkposition == barcodeposition || remarkposition == itemposition)
            {
                msg = msg + remarkposition + "&nbsp;";
                //lblMsgbarcode.Text = remarkposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (barcodeposition == customerposition || addressposition == barcodeposition || bookingnoposition == barcodeposition || barcodeposition == remarkposition || remarkposition == barcodeposition || barcodeposition == itemposition)
            {
                msg = msg + barcodeposition + "&nbsp;";
                //lblMsgbarcode.Text =barcodeposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (itemposition == customerposition || addressposition == itemposition || bookingnoposition == itemposition || itemposition == remarkposition || itemposition == barcodeposition || barcodeposition == itemposition)
            {
                msg = msg + itemposition + "&nbsp;";
                //lblMsgbarcode.Text =itemposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (addressposition == customerposition || bookingnoposition == addressposition || addressposition == itemposition || addressposition == remarkposition || addressposition == barcodeposition || processposition == addressposition)
            {
                msg = msg + addressposition + "&nbsp;";
                //lblMsgbarcode.Text =addressposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (msg != "")
            {
                lblMsgbarcode.Text = "Positons " + msg + "are Same.Please select different Positions.";
                fetchdemoandoriginalbarcode();
            }
            else
            {
                SetWidthheight();
                res = BAL.BALFactory.Instance.BAL_Sticker.Updatebarcodewidthheight(Ob);
                SetValue();
                res = BAL.BALFactory.Instance.BAL_Sticker.Updatebarcodeconfig(Ob);
                if (res == "Record Saved")
                {
                    lblMsgbarcode.Text = "Sticker Configration saved sucessfully";
                    demobarcodedisply();
                    SetfetchValue();
                    fetchbarcodeconfigsetting();
                }
                else
                {
                    lblMsgbarcode.Text = res;
                }
            }
        }

        private void fetchrecordcontrol()
        {
            string res = string.Empty;
            SetValue();
            res = BAL.BALFactory.Instance.BAL_Sticker.Updatebarcodeconfig(Ob);
            if (res == "Record Saved")
            {
                SetfetchValue();
                fetchbarcodeconfigsetting();
            }
        }

        //-----------------------------barcode demy display--------------------------
        public void demobarcodedisply()
        {
            barcodewidth = barcodewirth.SelectedItem.ToString();
            barcodeheight = barcodehight.SelectedItem.ToString();
            string[] strArr1 = barcodeheight.Split('i');
            linehight = Math.Round(Convert.ToDouble(strArr1[0]) / 7, 2);
            double barchight = Convert.ToDouble(strArr1[0]) - 0.5;
            double divhight = Math.Round(barchight / 7, 2);
            bool barcodebookingno = (RadioBookingNoyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            string bookingfont = DropBookingNofont.SelectedItem.ToString();
            string bookingsize = drpBookingNosize.SelectedItem.ToString();
            string bookingalign = DropBookingNoalign.SelectedItem.ToString();
            string bookingbold = (CheckBookingNobould.Checked ? "Bold" : "");
            string bookingitilic = (CheckBookingNoitilice.Checked ? "Italic" : "");
            string bookingunderline = (CheckBookingNounderline.Checked ? "underline" : "");
            bool barcodeprocess = (RadioProcessyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            bool barcodexteraprocess = (RadioProcessyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            bool barcodeextraprocesssecond = (RadioProcessyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            bool barcodesubtotal = (RadioItemTotalandSubTotalyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            string processfont = drpcusfont.SelectedItem.ToString();
            string processsize = drpcussize.SelectedItem.ToString();
            string processalign = Dropcuscolour.SelectedItem.ToString();
            string processbold = (Checkbould.Checked ? "Bold" : "");
            string processitalic = (Checkitilace.Checked ? "Italic" : "");
            string processunderline = (Checkunderline.Checked ? "underline" : "");
            bool barcoderemark = (Radioremarkyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            bool barcodecolour = (Radiocoloursyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            string remarkfont = Dropremarkfont.SelectedItem.ToString();
            string remarksize = Dropremarksize.SelectedItem.ToString();
            string remarkremarkalign = Dropremarkalign.SelectedItem.ToString();
            string remarkbold = (Checkremarkitilice.Checked ? "Italic" : "");
            string remarkitalic = (Checkremarkitilice.Checked ? "Italic" : "");
            string remarkunderline = (Checkremarkunderline.Checked ? "underline" : "");
            bool barcodeprint = (RadioBarcodeyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            //  string barcodesize = Dropbarcodesize.SelectedItem.ToString();
            string barcodealign = Dropbarcodealign.SelectedItem.ToString();
            bool barcodeitem = (RadioItemyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            bool barcodeduedate = (RadioDateyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            bool barcodetime = (Radiotimeyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            string itemfont = Dropitemfont.SelectedItem.ToString();
            string itemsize = Dropitemsize.SelectedItem.ToString();
            string itembold = (Checkitembould.Checked ? "Bold" : "");
            string itemitalic = (Checkitemitilice.Checked ? "Italic" : "");
            string itemalign = Dropitemalign.SelectedItem.ToString();
            string itemunderline = (Checkitemunderline.Checked ? "underline" : "");
            bool barcodecustomer = (radiocusyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            string customerfont = drpfontname.SelectedItem.ToString();
            string customersize = Drpnamesize.SelectedItem.ToString();
            string customeralign = Drpcusalign.SelectedItem.ToString();
            string customerbold = (Checknamebold.Checked ? "Bold" : "");
            string customeritilic = (Checknameitalac.Checked ? "Italic" : "");
            string customerunderline = (Checknameunderline.Checked ? "underline" : "");

            bool barcodeaddress = (Radioaddressyes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));
            string addressfont = Drpaddressfont.SelectedItem.ToString();
            string addresssize = Drpaddresssize.SelectedItem.ToString();
            string addressalign = Drpaddressalign.SelectedItem.ToString();
            string addressbold = (Checkaddressbold.Checked ? "Bold" : "");
            string addressitilic = (Checkaddressitalic.Checked ? "Italic" : "");
            string addressunderline = (Checkaddressunderline.Checked ? "underline" : "");

            bool barcodedivider = (Radiodivideryes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));

            //string bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            //string customerposition = Drpcusposition.SelectedItem.ToString();
            //string processposition = Drpprocessposition.SelectedItem.ToString();
            //string remarkposition = drpremarkposition.SelectedItem.ToString();
            //string barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            //string itemposition = Drpitemposition.SelectedItem.ToString();
            //string addressposition = drpaddressposition.SelectedItem.ToString();

            string bookingnoposition = Drpbookinfposition.SelectedValue.ToString();
            string customerposition = Drpcusposition.SelectedValue.ToString();
            string processposition = Drpprocessposition.SelectedValue.ToString();
            string remarkposition = drpremarkposition.SelectedValue.ToString();
            string barcodeposition = drpbarcodeposition.SelectedValue.ToString();
            string itemposition = Drpitemposition.SelectedValue.ToString();
            string addressposition = drpaddressposition.SelectedValue.ToString();

            string custName = string.Empty;
            string strMainBarcode = "width:" + barcodewidth + ";height:" + barcodeheight + ";font-size:6px;text-align:center;overflow:hidden;";
            string bookingNo = "width:" + barcodewidth + ";position: relative;font-family:" + bookingfont + ";font-size:" + bookingsize + "px;height:35px;font-weight:" + bookingbold + ";font-style:" + bookingitilic + "; text-decoration:" + bookingunderline + ";text-align:" + bookingalign + ";line-height:125%;";
            string customer = "width:" + barcodewidth + ";position: relative;font-family:" + customerfont + ";font-size:" + customersize + "px;height:15px;font-weight:" + customerbold + ";font-style:" + customeritilic + "; text-decoration:" + customerunderline + ";text-align:" + customeralign + ";text-transform:capitalize;line-height:125%;";

            string address = "width:" + barcodewidth + ";position: relative;font-family:" + addressfont + ";font-size:" + addresssize + "px;height:15px;font-weight:" + addressbold + ";font-style:" + addressitilic + "; text-decoration:" + addressunderline + ";text-align:" + addressalign + ";text-transform:capitalize;";
            string divider = "width:" + barcodewidth + ";position: relative;height:;line-height:20%;margin-top:1px;font-weight:bold;font-family:Arial Black";
            custName = "width:" + barcodewidth + ";position: relative;font-family:" + processfont + ";font-size:" + processsize + "px;height:15px;font-weight:" + processbold + ";font-style:" + processitalic + "; text-decoration:" + processunderline + ";text-align:" + processalign + ";text-transform:capitalize;";

            string barcode = "width:" + barcodewidth + "; position: relative;font-family:MRV Code39extMA;height:.5in; font-size:12px;top:4px;text-align:" + barcodealign + ";";
            string time = string.Empty;

            time = "width:" + barcodewidth + ";position:relative;font-size:" + itemsize + "px;top:1.5px;text-align:" + itemalign + ";height:15px;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";word-spacing:-3px; font-family:" + itemfont + ";";

            string date = "width:" + barcodewidth + ";position: relative;font-family:" + itemfont + ";font-size:" + itemsize + "px;height:15px;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";text-align:" + itemalign + ";word-spacing:-1px;";
            string remark = "width:" + barcodewidth + ";position: relative;font-family:" + remarkfont + ";font-size:" + remarksize + "px;top:2.5px;height:15px;word-spacing:-2px;text-align:" + remarkremarkalign + ";font-weight:" + remarkbold + ";font-style:" + remarkitalic + "; text-decoration:" + remarkunderline + ";text-transform:capitalize;";

            strPreview += "<div style='" + strMainBarcode + "'>";
            if (barcodedivider == true)
            {
                strPreview += "<div style='" + divider + "' >" + "------------------------" + "</div>";
            }
            else
            {
            }

            for (int j = 1; j <= 7; j++)
            {
                if (j == Int32.Parse(bookingnoposition.ToString()))
                {
                    if (barcodebookingno == true)
                    {
                        strPreview += "<div style='" + bookingNo + "' >" + "1234" + "</div>";
                    }
                    else
                    {
                        //strPreview += "<div style='" + bookingNo + "' >" + "</div>";
                    }
                }
                if (j == Int32.Parse(customerposition.ToString()))
                {
                    if (barcodecustomer == true)
                    {
                        strPreview += "<div style='" + customer + "' >" + "SHARMA" + "</div>";
                    }
                    else
                    {
                        //strPreview += "<div style='" + customer + "' >" + "</div>";
                    }
                }

                if (j == Int32.Parse(addressposition.ToString()))
                {
                    if (barcodeaddress == true)
                    {
                        strPreview += "<div style='" + address + "' >" + "E-166 Kamla Nagar" + "</div>";
                    }
                    else
                    {
                        //strPreview += "<div style='" + address + "' >" + "</div>";
                    }
                }
                string process = string.Empty;
                string extraprocess = string.Empty;
                string extraprocesssecond = string.Empty;
                string subtotal = string.Empty;
                if (j == Int32.Parse(processposition.ToString()))
                {
                    if (barcodeprocess == true)
                    {
                        process = "&nbsp;" + "DC";
                    }

                    if (barcodexteraprocess == true)
                    {
                        extraprocess = "&nbsp;" + "LD";
                    }

                    if (barcodeextraprocesssecond == true)
                    {
                        extraprocesssecond = "&nbsp;" + "WC";
                    }

                    if (barcodesubtotal == true)
                    {
                        subtotal = "&nbsp;" + "1/1";
                    }

                    if (barcodeprocess == true || barcodexteraprocess == true || barcodeextraprocesssecond == true || barcodesubtotal == true)
                    {
                        strPreview += "<div  style='" + custName + "' >" + process + "" + extraprocess + "" + extraprocesssecond + "" + subtotal + "</div>";
                    }
                }

                if (j == Int32.Parse(barcodeposition.ToString()))
                {
                    if (barcodeprint == true)
                    {
                        strPreview += "<div style='" + barcode + "'>" + "*1-1*" + "</div>";
                    }
                }
                string item = string.Empty;
                if (j == Int32.Parse(itemposition.ToString()))
                {
                    if (barcodeitem == true)
                    {
                        item = "SUIT";
                    }

                    string duedate = string.Empty;
                    if (barcodeduedate == true)
                    {
                        duedate = "&nbsp;&nbsp;" + "24/05/12";
                    }
                    string currenttime = string.Empty;
                    if (barcodetime == true)
                    {
                        currenttime = "&nbsp;&nbsp;" + "1 AM";
                    }
                    if (barcodeitem == true || barcodeduedate == true || barcodetime == true)
                    {
                        strPreview += "<div style='" + time + "'>" + item + "&nbsp;" + "<spam style='" + date + "'>" + "" + duedate + "" + currenttime + "</spam></div>";
                    }
                }
                if (j == Int32.Parse(remarkposition.ToString()))
                {
                    if ("Magenta" != "" && barcodecolour == true)
                    {
                        if ("Cut marks" != "" && barcoderemark == true)
                        {
                            strPreview += "<div style='" + remark + "'>" + "Magenta" + "," + "Cut marks" + "</div>";
                        }
                        else
                        {
                            if (barcodecolour == true)
                            {
                                strPreview += "<div style='" + remark + "'>" + "Magenta" + "</div>";
                            }
                            else
                            {
                            }
                        }
                    }
                    else
                    {
                        if ("Cut marks" != "" && barcoderemark == true)
                        {
                            strPreview += "<div style='" + remark + "'>" + "Cut marks" + "</div>";
                        }
                        else
                        {
                            //strPreview += "<div style='" + remark + "'>" + "</div>";
                        }
                    }
                }
            }

            strPreview += "</div>";
        }

        //----------------------------end------------------------
        private void fetchdemoandoriginalbarcode()
        {
            demobarcodedisply();
            SetfetchValue();
            fetchbarcodeconfigsetting();
        }

        public DTO.Sticker SetWidthheight()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.barcodewidth = barcodewirth.SelectedItem.ToString();
            Ob.barcodeheight = barcodehight.SelectedItem.ToString();
            return Ob;
        }

        protected void btset_Click(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            barcodepanel.Visible = true;
            panelwidth.Visible = true;
            barcodepaneldisplay.Visible = true;
        }

        private string bookingnoposition = string.Empty;
        private string customerposition = string.Empty;
        private string processposition = string.Empty;
        private string remarkposition = string.Empty;
        private string barcodeposition = string.Empty;
        private string itemposition = string.Empty;
        private string addressposition = string.Empty;

        protected void btpreview_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();
            string msg = string.Empty;
            if (bookingnoposition == customerposition || addressposition == bookingnoposition || bookingnoposition == processposition || bookingnoposition == remarkposition || bookingnoposition == barcodeposition || bookingnoposition == itemposition)
            {
                msg = msg + bookingnoposition + "&nbsp;";
                //lblMsgbarcode.Text =bookingnoposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (bookingnoposition == customerposition || addressposition == customerposition || customerposition == processposition || customerposition == remarkposition || customerposition == barcodeposition || customerposition == itemposition)
            {
                msg = msg + customerposition + "&nbsp;";

                //lblMsgbarcode.Text =customerposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (processposition == customerposition || addressposition == processposition || bookingnoposition == processposition || processposition == remarkposition || processposition == barcodeposition || processposition == itemposition)
            {
                msg = msg + processposition + "&nbsp;";
                //lblMsgbarcode.Text =processposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (remarkposition == customerposition || addressposition == remarkposition || bookingnoposition == remarkposition || processposition == remarkposition || remarkposition == barcodeposition || remarkposition == itemposition)
            {
                msg = msg + remarkposition + "&nbsp;";
                //lblMsgbarcode.Text = remarkposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (barcodeposition == customerposition || addressposition == barcodeposition || bookingnoposition == barcodeposition || barcodeposition == remarkposition || remarkposition == barcodeposition || barcodeposition == itemposition)
            {
                msg = msg + barcodeposition + "&nbsp;";
                //lblMsgbarcode.Text =barcodeposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (itemposition == customerposition || addressposition == itemposition || bookingnoposition == itemposition || itemposition == remarkposition || itemposition == barcodeposition || barcodeposition == itemposition)
            {
                msg = msg + itemposition + "&nbsp;";
                //lblMsgbarcode.Text =itemposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (addressposition == customerposition || bookingnoposition == addressposition || addressposition == itemposition || addressposition == remarkposition || addressposition == barcodeposition || processposition == addressposition)
            {
                msg = msg + addressposition + "&nbsp;";
                //lblMsgbarcode.Text =addressposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (msg != "")
            {
                lblMsgbarcode.Text = "Positons " + msg + "are Same.Please select different Positions.";
                fetchdemoandoriginalbarcode();
            }
            else

                fetchdemoandoriginalbarcode();
        }

        protected void drpbarcodeposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (barcodeposition == customerposition)
            {
                Drpcusposition.SelectedValue = "8";
            }
            else if (addressposition == barcodeposition)
            {
                drpaddressposition.SelectedValue = "8";
            }
            else if (barcodeposition == processposition)
            {
                Drpprocessposition.SelectedValue = "8";
            }
            else if (barcodeposition == remarkposition)
            {
                drpremarkposition.SelectedValue = "8";
            }
            else if (bookingnoposition == barcodeposition)
            {
                Drpbookinfposition.SelectedValue = "8";
            }

            else if (barcodeposition == itemposition)
            {
                Drpitemposition.SelectedValue = "8";
            }
            else
            {
            }
        }

        protected void Drpbookinfposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (bookingnoposition == customerposition)
            {
                Drpcusposition.SelectedValue = "8";
            }
            else if (addressposition == bookingnoposition)
            {
                drpaddressposition.SelectedValue = "8";
            }
            else if (bookingnoposition == processposition)
            {
                Drpprocessposition.SelectedValue = "8";
            }
            else if (bookingnoposition == remarkposition)
            {
                drpremarkposition.SelectedValue = "8";
            }
            else if (bookingnoposition == barcodeposition)
            {
                drpbarcodeposition.SelectedValue = "8";
            }

            else if (bookingnoposition == itemposition)
            {
                Drpitemposition.SelectedValue = "8";
            }
            else
            {
            }
        }

        protected void Drpcusposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (bookingnoposition == customerposition)
            {
                Drpbookinfposition.SelectedValue = "8";
            }
            else if (addressposition == customerposition)
            {
                drpaddressposition.SelectedValue = "8";
            }
            else if (customerposition == processposition)
            {
                Drpprocessposition.SelectedValue = "8";
            }
            else if (customerposition == remarkposition)
            {
                drpremarkposition.SelectedValue = "8";
            }
            else if (customerposition == barcodeposition)
            {
                drpbarcodeposition.SelectedValue = "8";
            }

            else if (customerposition == itemposition)
            {
                Drpitemposition.SelectedValue = "8";
            }
            else
            {
            }
        }

        protected void drpaddressposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (bookingnoposition == addressposition)
            {
                Drpbookinfposition.SelectedValue = "8";
            }
            else if (addressposition == customerposition)
            {
                Drpcusposition.SelectedValue = "8";
            }
            else if (addressposition == processposition)
            {
                Drpprocessposition.SelectedValue = "8";
            }
            else if (addressposition == remarkposition)
            {
                drpremarkposition.SelectedValue = "8";
            }
            else if (addressposition == barcodeposition)
            {
                drpbarcodeposition.SelectedValue = "8";
            }

            else if (addressposition == itemposition)
            {
                Drpitemposition.SelectedValue = "8";
            }
            else
            {
            }
        }

        protected void Drpprocessposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (bookingnoposition == processposition)
            {
                Drpbookinfposition.SelectedValue = "8";
            }
            else if (processposition == customerposition)
            {
                Drpcusposition.SelectedValue = "8";
            }
            else if (addressposition == processposition)
            {
                drpaddressposition.SelectedValue = "8";
            }
            else if (processposition == remarkposition)
            {
                drpremarkposition.SelectedValue = "8";
            }
            else if (processposition == barcodeposition)
            {
                drpbarcodeposition.SelectedValue = "8";
            }

            else if (processposition == itemposition)
            {
                Drpitemposition.SelectedValue = "8";
            }
            else
            {
            }
        }

        protected void drpremarkposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (bookingnoposition == remarkposition)
            {
                Drpbookinfposition.SelectedValue = "8";
            }
            else if (remarkposition == customerposition)
            {
                Drpcusposition.SelectedValue = "8";
            }
            else if (addressposition == remarkposition)
            {
                drpaddressposition.SelectedValue = "8";
            }
            else if (processposition == remarkposition)
            {
                Drpprocessposition.SelectedValue = "8";
            }
            else if (remarkposition == barcodeposition)
            {
                drpbarcodeposition.SelectedValue = "8";
            }

            else if (remarkposition == itemposition)
            {
                Drpitemposition.SelectedValue = "8";
            }
            else
            {
            }
        }

        protected void Drpitemposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchdemoandoriginalbarcode();
            bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            customerposition = Drpcusposition.SelectedItem.ToString();
            processposition = Drpprocessposition.SelectedItem.ToString();
            remarkposition = drpremarkposition.SelectedItem.ToString();
            barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            itemposition = Drpitemposition.SelectedItem.ToString();
            addressposition = drpaddressposition.SelectedItem.ToString();

            if (bookingnoposition == itemposition)
            {
                Drpbookinfposition.SelectedValue = "8";
            }
            else if (itemposition == customerposition)
            {
                Drpcusposition.SelectedValue = "8";
            }
            else if (addressposition == itemposition)
            {
                drpaddressposition.SelectedValue = "8";
            }
            else if (processposition == itemposition)
            {
                Drpprocessposition.SelectedValue = "8";
            }
            else if (itemposition == barcodeposition)
            {
                drpbarcodeposition.SelectedValue = "8";
            }

            else if (remarkposition == itemposition)
            {
                drpremarkposition.SelectedValue = "8";
            }
            else
            {
            }
        }
    }
}