using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;

namespace QuickWeb.Reports
{
    public partial class configbarcode : System.Web.UI.Page
    {
        public string strPreview = string.Empty;
        public string strPreviewbarcode = string.Empty;
        public string barcodewidth = string.Empty;
        public string barcodeheight = string.Empty;
        public string oldbarcodewidth = string.Empty;
        public string oldbarcodehight = string.Empty;

        public double linehight = 0.00;

        private DTO.Barcodeconfig Ob = new DTO.Barcodeconfig();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                    Dropitemsize1.Items.Add(j.ToString());
                }

                for (int j = 36; j >= 5; j--)
                {
                    drpBookingNosize.Items.Add(j.ToString());
                }

                for (int k = 1; k <= 8; k++)
                {
                    Drpbookinfposition.Items.Add(k.ToString());
                    Drpcusposition.Items.Add(k.ToString());
                    Drpprocessposition.Items.Add(k.ToString());
                    drpremarkposition.Items.Add(k.ToString());
                    drpbarcodeposition.Items.Add(k.ToString());
                    Drpitemposition.Items.Add(k.ToString());
                    Drpitemposition1.Items.Add(k.ToString());
                    drpaddressposition.Items.Add(k.ToString());
                }
                //drpBookingNosize.SelectedValue = "9";
                //Drpnamesize.SelectedValue = "9";
                //Drpaddresssize.SelectedValue = "9";
                //drpcussize.SelectedValue = "9";
                //Dropremarksize.SelectedValue = "9";
                //Dropitemsize.SelectedValue = "9";

                //Drpbookinfposition.SelectedValue = "1";
                //Drpcusposition.SelectedValue = "2";
                //Drpprocessposition.SelectedValue = "3";
                //drpremarkposition.SelectedValue = "6";
                //drpbarcodeposition.SelectedValue = "4";
                //Drpitemposition.SelectedValue = "5";
                //drpaddressposition.SelectedValue = "7";
                //DropBookingNofont.SelectedValue = "Arial";
                //drpfontname.SelectedValue = "Arial";
                //drpcusfont.SelectedValue = "Arial";
                //Dropremarkfont.SelectedValue = "Arial";
                //Dropitemfont.SelectedValue = "Arial";
                //Drpaddressfont.SelectedValue = "Arial";
                SetfetchValue();
                fetchbarcodevalue();

                fetchbarcodeconfigsetting();
            }
        }

        // -----------------------------barcode display--------------------------
        public void fetchbarcodeconfigsetting()
        {
            DataSet dset = new DataSet();
            dset = BAL.BALFactory.Instance.BAL_Barcodeconfig.fetchbarcodeconfig(Ob);
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
                string itemfont1 = dset.Tables[0].Rows[0]["itemfont1"].ToString();
                string itemsize = dset.Tables[0].Rows[0]["itemsize"].ToString();
                string itemsize1 = dset.Tables[0].Rows[0]["itemsize1"].ToString();
                string itembold = dset.Tables[0].Rows[0]["itembold"].ToString();
                string itembold1 = dset.Tables[0].Rows[0]["itembold1"].ToString();
                string itemitalic = dset.Tables[0].Rows[0]["itemitalic"].ToString();
                string itemitalic1 = dset.Tables[0].Rows[0]["itemitalic1"].ToString();

                string itemalign = dset.Tables[0].Rows[0]["itemalign"].ToString();
                string itemalign1 = dset.Tables[0].Rows[0]["itemalign1"].ToString();
                string itemunderline = dset.Tables[0].Rows[0]["itemunderline"].ToString();
                string itemunderline1 = dset.Tables[0].Rows[0]["itemunderline1"].ToString();

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
                string itemposition1 = dset.Tables[0].Rows[0]["itemposition1"].ToString();

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
                string strMainBarcode = "width:" + barcodewidth + ";height:" + barcodeheight + ";font-size:6px;text-align:center;overflow:hidden;";
                string bookingNo = "width:" + barcodewidth + ";position: relative;font-family:" + bookingfont + ";font-size:" + bookingsize + "px;height:;font-weight:" + bookingbold + ";font-style:" + bookingitilic + "; text-decoration:" + bookingunderline + ";text-align:" + bookingalign + ";line-height:;";
                string customer = "width:" + barcodewidth + ";position: relative;font-family:" + customerfont + ";font-size:" + customersize + "px;height:;font-weight:" + customerbold + ";font-style:" + customeritilic + "; text-decoration:" + customerunderline + ";text-align:" + customeralign + ";text-transform:capitalize;line-height:;";

                string address = "width:" + barcodewidth + ";position: relative;font-family:" + addressfont + ";font-size:" + addresssize + "px;height:;font-weight:" + addressbold + ";font-style:" + addressitilic + "; text-decoration:" + addressunderline + ";text-align:" + addressalign + ";text-transform:capitalize;";
                //string divider = "width:" + barcodewidth + ";position: relative;height:;line-height:20%;margin-top:1px;font-weight:bold;font-family:Arial Black";
                custName = "width:" + barcodewidth + ";position: relative;font-family:" + processfont + ";font-size:" + processsize + "px;height:;font-weight:" + processbold + ";font-style:" + processitalic + "; text-decoration:" + processunderline + ";text-align:" + processalign + ";text-transform:capitalize;";

                string barcode = "width:" + barcodewidth + "; position: relative;font-family:c39hrp24dhtt;height:; font-size:32px;top:;text-align:" + barcodealign + ";";
                string time = string.Empty;

                time = "width:" + barcodewidth + ";position:relative;font-size:" + itemsize + "px;top:;text-align:" + itemalign + ";height:;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";word-spacing:-3px; font-family:" + itemfont + ";";
                string item = "width:" + barcodewidth + ";position: relative;font-family:" + itemfont1 + ";font-size:" + itemsize1 + "px;height:;font-weight:" + itembold1 + ";font-style:" + itemitalic1 + "; text-decoration:" + itemunderline1 + ";text-align:" + itemalign1 + ";line-height:;";

                string date = "width:" + barcodewidth + ";position: relative;font-family:" + itemfont + ";font-size:" + itemsize + "px;height:;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";text-align:" + itemalign + ";word-spacing:-1px;";
                string remark = "width:" + barcodewidth + ";position: relative;font-family:" + remarkfont + ";font-size:" + remarksize + "px;top:;height:;word-spacing:-2px;text-align:" + remarkremarkalign + ";font-weight:" + remarkbold + ";font-style:" + remarkitalic + "; text-decoration:" + remarkunderline + ";text-transform:capitalize;";

                strPreview += "<div style='" + strMainBarcode + "'>";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strPreviewbarcode += "<div style='" + strMainBarcode + "'>";
                    //if (barcodedivider == true)
                    //{
                    //    strPreviewbarcode += "<div style='" + divider + "' >" + "------------------------" + "</div>";
                    //}
                    //else
                    //{
                    //}
                    for (int j = 1; j <= 8; j++)
                    {
                        if (j == Int32.Parse(bookingnoposition.ToString()))
                        {
                            if (barcodebookingno == true)
                            {
                                strPreviewbarcode += "<div style='" + bookingNo + "' >" + ds.Tables[0].Rows[i]["BookingNo"].ToString() + "</div>";
                            }
                            //else
                            //{
                            //    //strPreviewbarcode += "<div style='" + bookingNo + "' >" + "</div>";
                            //}
                        }
                        if (j == Int32.Parse(customerposition.ToString()))
                        {
                            if (barcodecustomer == true)
                            {
                                strPreviewbarcode += "<div style='" + customer + "' >" + ds.Tables[0].Rows[i]["CustomerName"].ToString() + "</div>";
                            }
                            //else
                            //{
                            //    //strPreviewbarcode += "<div style='" + customer + "' >" + "</div>";
                            //}
                        }
                        if (j == Int32.Parse(addressposition.ToString()))
                        {
                            if (barcodeaddress == true)
                            {
                                strPreviewbarcode += "<div style='" + address + "' >" + ds.Tables[0].Rows[i]["CustomerAddress"].ToString() + "</div>";
                            }
                            //else
                            //{
                            //    //strPreviewbarcode += "<div style='" + address + "' >" + "</div>";
                            //}
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
                        string itemname = string.Empty;
                        if (j == Int32.Parse(itemposition1.ToString()))
                        {
                            if (barcodeitem == true)
                            {
                                itemname = ds.Tables[0].Rows[i]["Item"].ToString();
                                strPreviewbarcode += "<div style='" + item + "'>" + itemname + "</div>";
                            }

                            else
                            { }
                        }
                        if (j == Int32.Parse(itemposition.ToString()))
                        {
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
                            if (barcodeduedate == true || barcodetime == true)
                            {
                                strPreviewbarcode += "<div style='" + time + "'>" + "&nbsp;" + "<spam style='" + date + "'>" + "" + duedate + "" + currenttime + "</spam></div>";
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
                                //else
                                //{
                                //    //strPreviewbarcode += "<div style='" + remark + "'>" + "</div>";
                                //}
                            }
                        }
                    }

                    strPreviewbarcode += "</div>";
                }
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
                Dropitemfont1.Items.Add(font.Name);
                drpfontname.Items.Add(font.Name);
                Drpaddressfont.Items.Add(font.Name);
            }
        }

        public DTO.Barcodeconfig SetfetchValue()
        {
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        public DTO.Barcodeconfig SetValue()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.barcodeconfigbooking = (RadioBookingNoyes.Checked ? "1" : "0");
            Ob.barcodebookingfont = DropBookingNofont.SelectedItem.ToString();
            Ob.barcodebookingsize = drpBookingNosize.SelectedItem.ToString();
            Ob.barcodebookingalign = DropBookingNoalign.SelectedItem.ToString();
            Ob.barcodebookingbold = (CheckBookingNobould.Checked ? "Bold" : "");
            Ob.barcodebookingitalic = (CheckBookingNoitilice.Checked ? "Italic" : "");
            Ob.barcodebookingunderline = (CheckBookingNounderline.Checked ? "underline" : "");

            Ob.barcodeconfigprocess = (RadioProcessyes.Checked ? "1" : "0");
            //Ob.barcodeconfigextraprocess = (RadioExtraprocessyes.Checked ? "1" : "0");
            //Ob.barcodeconfigextraprocesssecond = (RadioExtraprocesstype2yes.Checked ? "1" : "0");
            Ob.barcodeconfigprocesssubtotal = (RadioItemTotalandSubTotalyes.Checked ? "1" : "0");
            Ob.barcodeprocessfont = drpcusfont.SelectedItem.ToString();
            Ob.barcodeprocesssize = drpcussize.SelectedItem.ToString();
            Ob.barcodeprocessalign = Dropcuscolour.SelectedItem.ToString();
            Ob.barcodeprocessbold = (Checkbould.Checked ? "Bold" : "");
            Ob.barcodeprocessitalic = (Checkitilace.Checked ? "Italic" : "");
            Ob.barcodeprocessunderline = (Checkunderline.Checked ? "underline" : "");

            Ob.barcodeconfigremark = (Radioremarkyes.Checked ? "1" : "0");
            Ob.barcodeconfigcolour = (Radiocoloursyes.Checked ? "1" : "0");
            Ob.barcoderemarkfont = Dropremarkfont.SelectedItem.ToString();
            Ob.barcodremarksize = Dropremarksize.SelectedItem.ToString();
            Ob.barcoderemarkalign = Dropremarkalign.SelectedItem.ToString();
            Ob.barcoderemarkbold = (Checkremarkbould.Checked ? "Bold" : "");
            Ob.barcoderemarkitalic = (Checkremarkitilice.Checked ? "Italic" : "");
            Ob.barcoderemarkunderline = (Checkremarkunderline.Checked ? "underline" : "");

            Ob.barcodeconfigbarcode = (RadioBarcodeyes.Checked ? "1" : "0");
            // Ob.barcodebarcodesize = Dropbarcodesize.SelectedItem.ToString();
            Ob.barcodebarcodealign = Dropbarcodealign.SelectedItem.ToString();

            Ob.barcodeconfigitem = (RadioItemyes.Checked ? "1" : "0");
            Ob.barcodeconfigduedate = (RadioDateyes.Checked ? "1" : "0");
            Ob.barcodeconfigtime = (Radiotimeyes.Checked ? "1" : "0");
            Ob.barcodeitemfont = Dropitemfont.SelectedItem.ToString();
            Ob.barcodeitemfont1 = Dropitemfont1.SelectedItem.ToString();
            Ob.barcodeitemsize = Dropitemsize.SelectedItem.ToString();
            Ob.barcodeitemsize1 = Dropitemsize1.SelectedItem.ToString();
            Ob.barcodeitemalign = Dropitemalign.SelectedItem.ToString();
            Ob.barcodeitemalign1 = Dropitemalign1.SelectedItem.ToString();

            Ob.barcodeitembold = (Checkitembould.Checked ? "Bold" : "");
            Ob.barcodeitembold1 = (Checkitembould1.Checked ? "Bold" : "");
            Ob.barcodeitemitalic = (Checkitemitilice.Checked ? "Italic" : "");
            Ob.barcodeitemitalic1 = (Checkitemitilice1.Checked ? "Italic" : "");
            Ob.barcodeitemunderline = (Checkitemunderline.Checked ? "underline" : "");
            Ob.barcodeitemunderline1 = (Checkitemunderline1.Checked ? "underline" : "");

            Ob.barcodeconfigcustomer = (radiocusyes.Checked ? "1" : "0");
            Ob.barcodecustomerfont = drpfontname.SelectedItem.ToString();
            Ob.barcodecustomerize = Drpnamesize.SelectedItem.ToString();
            Ob.barcodecustomeralign = Drpcusalign.SelectedItem.ToString();
            Ob.barcodecustomerbold = (Checknamebold.Checked ? "Bold" : "");
            Ob.barcodecustomeritalic = (Checknameitalac.Checked ? "Italic" : "");
            Ob.barcodecustomerunderline = (Checknameunderline.Checked ? "underline" : "");

            Ob.bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            Ob.customerposition = Drpcusposition.SelectedItem.ToString();
            Ob.processposition = Drpprocessposition.SelectedItem.ToString();
            Ob.remarkposition = drpremarkposition.SelectedItem.ToString();
            Ob.barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            Ob.itemposition = Drpitemposition.SelectedItem.ToString();
            Ob.itemposition1 = Drpitemposition1.SelectedItem.ToString();
            Ob.addressposition = drpaddressposition.SelectedItem.ToString();
            Ob.barcodeconfigaddress = (Radioaddressyes.Checked ? "1" : "0");
            Ob.barcodeaddressfont = Drpaddressfont.SelectedItem.ToString();
            Ob.barcodeaddresssize = Drpaddresssize.SelectedItem.ToString();
            Ob.barcodeaddressalign = Drpaddressalign.SelectedItem.ToString();
            Ob.barcodeaddressbold = (Checkaddressbold.Checked ? "Bold" : "");
            Ob.barcodeaddressitalic = (Checkaddressitalic.Checked ? "Italic" : "");
            Ob.barcodeaddressunderline = (Checkaddressunderline.Checked ? "underline" : "");

            return Ob;
        }

        public void fetchbarcodevalue()
        {
            DataSet ds = new DataSet();
            ds = BAL.BALFactory.Instance.BAL_Barcodeconfig.fetchbarcodeconfig(Ob);
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
                //RadioExtraprocessyes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodexteraprocess"].ToString());
                //if (RadioExtraprocessyes.Checked != true)
                //{
                //    RadioExtraprocessno.Checked = true;
                //}
                //else
                //{
                //    RadioExtraprocessyes.Checked = true;
                //}
                //RadioExtraprocesstype2yes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodeextraprocesssecond"].ToString());
                //if (RadioExtraprocesstype2yes.Checked != true)
                //{
                //    RadioExtraprocesstype2no.Checked = true;
                //}
                //else
                //{
                //    RadioExtraprocesstype2yes.Checked = true;
                //}
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
                PrjClass.SetItemInDropDown(Dropitemfont1, ds.Tables[0].Rows[0]["itemfont1"].ToString(), true, false);

                PrjClass.SetItemInDropDown(Dropitemsize, ds.Tables[0].Rows[0]["itemsize"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropitemsize1, ds.Tables[0].Rows[0]["itemsize1"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropitemalign, ds.Tables[0].Rows[0]["itemalign"].ToString(), true, false);
                PrjClass.SetItemInDropDown(Dropitemalign1, ds.Tables[0].Rows[0]["itemalign1"].ToString(), true, false);
                if (ds.Tables[0].Rows[0]["itembold"].ToString() != "")
                {
                    Checkitembould.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itembold1"].ToString() != "")
                {
                    Checkitembould1.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itemitalic"].ToString() != "")
                {
                    Checkitemitilice.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itemitalic1"].ToString() != "")
                {
                    Checkitemitilice1.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itemunderline"].ToString() != "")
                {
                    Checkitemunderline.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["itemunderline1"].ToString() != "")
                {
                    Checkitemunderline1.Checked = true;
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

                //Radiodivideryes.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["barcodedivider"].ToString());
                //if (Radiodivideryes.Checked != true)
                //{
                //    Radiodividerno.Checked = true;
                //}
                //else
                //{
                //    Radiodivideryes.Checked = true;
                //}

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
                PrjClass.SetItemInDropDown(Drpitemposition1, ds.Tables[0].Rows[0]["itemposition1"].ToString(), true, false);
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
            string itemposition1 = Drpitemposition1.SelectedItem.ToString();
            string addressposition = drpaddressposition.SelectedItem.ToString();
            string msg = string.Empty;
            if (bookingnoposition == customerposition || addressposition == bookingnoposition || bookingnoposition == processposition || bookingnoposition == remarkposition || bookingnoposition == barcodeposition || bookingnoposition == itemposition || bookingnoposition == itemposition1)
            {
                msg = msg + bookingnoposition + "&nbsp;";
                //lblMsgbarcode.Text =bookingnoposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (bookingnoposition == customerposition || addressposition == customerposition || customerposition == processposition || customerposition == remarkposition || customerposition == barcodeposition || customerposition == itemposition || customerposition == itemposition1)
            {
                msg = msg + customerposition + "&nbsp;";

                //lblMsgbarcode.Text =customerposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (processposition == customerposition || addressposition == processposition || bookingnoposition == processposition || processposition == remarkposition || processposition == barcodeposition || processposition == itemposition || processposition == itemposition1)
            {
                msg = msg + processposition + "&nbsp;";
                //lblMsgbarcode.Text =processposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (remarkposition == customerposition || addressposition == remarkposition || bookingnoposition == remarkposition || processposition == remarkposition || remarkposition == barcodeposition || remarkposition == itemposition || remarkposition == itemposition1)
            {
                msg = msg + remarkposition + "&nbsp;";
                //lblMsgbarcode.Text = remarkposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (barcodeposition == customerposition || addressposition == barcodeposition || bookingnoposition == barcodeposition || barcodeposition == remarkposition || remarkposition == barcodeposition || barcodeposition == itemposition || barcodeposition == itemposition1)
            {
                msg = msg + barcodeposition + "&nbsp;";
                //lblMsgbarcode.Text =barcodeposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (itemposition == customerposition || addressposition == itemposition || bookingnoposition == itemposition || itemposition == remarkposition || itemposition == barcodeposition || barcodeposition == itemposition || itemposition == itemposition1)
            {
                msg = msg + itemposition + "&nbsp;";
                //lblMsgbarcode.Text =itemposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }

            if (itemposition1 == customerposition || addressposition == itemposition1 || bookingnoposition == itemposition1 || itemposition1 == remarkposition || itemposition1 == barcodeposition || barcodeposition == itemposition1 || itemposition == itemposition1)
            {
                msg = msg + itemposition1 + "&nbsp;";
                //lblMsgbarcode.Text =itemposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (addressposition == customerposition || bookingnoposition == addressposition || addressposition == itemposition || addressposition == remarkposition || addressposition == barcodeposition || processposition == addressposition || addressposition == itemposition1)
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
                res = BAL.BALFactory.Instance.BAL_Barcodeconfig.Updatebarcodewidthheight(Ob);
                SetValue();
                res = BAL.BALFactory.Instance.BAL_Barcodeconfig.Updatebarcodeconfig(Ob);
                if (res == "Record Saved")
                {
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
            res = BAL.BALFactory.Instance.BAL_Barcodeconfig.Updatebarcodeconfig(Ob);
            if (res == "Record Saved")
            {
                SetfetchValue();
                fetchbarcodeconfigsetting();
            }
        }

        //protected void DropBookingNofont_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void drpBookingNosize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //-----------------------------barcode demo display--------------------------
        public void demobarcodedisply()
        {
            barcodewidth = barcodewirth.SelectedItem.ToString();
            barcodeheight = barcodehight.SelectedItem.ToString();
            string[] strArr1 = barcodeheight.Split('i');
            linehight = Math.Round(Convert.ToDouble(strArr1[0]) / 8, 2);
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
            string itemfont1 = Dropitemfont1.SelectedItem.ToString();

            string itemsize = Dropitemsize.SelectedItem.ToString();
            string itemsize1 = Dropitemsize1.SelectedItem.ToString();
            string itembold = (Checkitembould.Checked ? "Bold" : "");
            string itembold1 = (Checkitembould1.Checked ? "Bold" : "");
            string itemitalic = (Checkitemitilice.Checked ? "Italic" : "");
            string itemitalic1 = (Checkitemitilice1.Checked ? "Italic" : "");
            string itemalign = Dropitemalign.SelectedItem.ToString();
            string itemalign1 = Dropitemalign1.SelectedItem.ToString();
            string itemunderline = (Checkitemunderline.Checked ? "underline" : "");
            string itemunderline1 = (Checkitemunderline1.Checked ? "underline" : "");
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

            //bool barcodedivider = (Radiodivideryes.Checked ? Convert.ToBoolean(1) : Convert.ToBoolean(0));

            string bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            string customerposition = Drpcusposition.SelectedItem.ToString();
            string processposition = Drpprocessposition.SelectedItem.ToString();
            string remarkposition = drpremarkposition.SelectedItem.ToString();
            string barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            string itemposition = Drpitemposition.SelectedItem.ToString();
            string itemposition1 = Drpitemposition1.SelectedItem.ToString();
            string addressposition = drpaddressposition.SelectedItem.ToString();

            string custName = string.Empty;
            string strMainBarcode = "width:" + barcodewidth + ";height:" + barcodeheight + ";font-size:6px;text-align:center;overflow:hidden;";
            string bookingNo = "width:" + barcodewidth + ";position: relative;font-family:" + bookingfont + ";font-size:" + bookingsize + "px;height:;font-weight:" + bookingbold + ";font-style:" + bookingitilic + "; text-decoration:" + bookingunderline + ";text-align:" + bookingalign + ";line-height:;";
            string customer = "width:" + barcodewidth + ";position: relative;font-family:" + customerfont + ";font-size:" + customersize + "px;height:;font-weight:" + customerbold + ";font-style:" + customeritilic + "; text-decoration:" + customerunderline + ";text-align:" + customeralign + ";text-transform:capitalize;line-height:;";

            string address = "width:" + barcodewidth + ";position: relative;font-family:" + addressfont + ";font-size:" + addresssize + "px;height:;font-weight:" + addressbold + ";font-style:" + addressitilic + "; text-decoration:" + addressunderline + ";text-align:" + addressalign + ";text-transform:capitalize;";
            //string divider = "width:" + barcodewidth + ";position: relative;height:;line-height:20%;margin-top:1px;font-weight:bold;font-family:Arial Black";
            custName = "width:" + barcodewidth + ";position: relative;font-family:" + processfont + ";font-size:" + processsize + "px;height:;font-weight:" + processbold + ";font-style:" + processitalic + "; text-decoration:" + processunderline + ";text-align:" + processalign + ";text-transform:capitalize;";

            string barcode = "width:" + barcodewidth + "; position: relative;font-family:c39hrp24dhtt;height:; font-size:32px;top:;text-align:" + barcodealign + ";";
            string time = string.Empty;

            time = "width:" + barcodewidth + ";position:relative;font-size:" + itemsize + "px;top:;text-align:" + itemalign + ";height:;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";word-spacing:-3px; font-family:" + itemfont + ";";
            string item = "width:" + barcodewidth + ";position: relative;font-family:" + itemfont1 + ";font-size:" + itemsize1 + "px;height:;font-weight:" + itembold1 + ";font-style:" + itemitalic1 + "; text-decoration:" + itemunderline1 + ";text-align:" + itemalign1 + ";line-height:;";

            string date = "width:" + barcodewidth + ";position: relative;font-family:" + itemfont + ";font-size:" + itemsize + "px;height:;font-weight:" + itembold + ";font-style:" + itemitalic + "; text-decoration:" + itemunderline + ";text-align:" + itemalign + ";word-spacing:-1px;";
            string remark = "width:" + barcodewidth + ";position: relative;font-family:" + remarkfont + ";font-size:" + remarksize + "px;top:;height:;word-spacing:-2px;text-align:" + remarkremarkalign + ";font-weight:" + remarkbold + ";font-style:" + remarkitalic + "; text-decoration:" + remarkunderline + ";text-transform:capitalize;";

            strPreview += "<div style='" + strMainBarcode + "'>";
            //if (barcodedivider == true)
            //{
            //    strPreview += "<div style='" + divider + "' >" + "------------------------" + "</div>";
            //}
            //else
            //{
            //}

            for (int j = 1; j <= 8; j++)
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
                string itemname = string.Empty;
                if (j == Int32.Parse(itemposition1.ToString()))
                {
                    if (barcodeitem == true)
                    {
                        itemname = "SUIT";
                        strPreview += "<div style='" + item + "'>" + itemname + "</div>";
                    }
                }
                if (j == Int32.Parse(itemposition.ToString()))
                {
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
                    if (barcodeduedate == true || barcodetime == true)
                    {
                        strPreview += "<div style='" + time + "'>" + "&nbsp;" + "<spam style='" + date + "'>" + "" + duedate + "" + currenttime + "</spam></div>";
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

        protected void btpreview_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            string bookingnoposition = Drpbookinfposition.SelectedItem.ToString();
            string customerposition = Drpcusposition.SelectedItem.ToString();
            string processposition = Drpprocessposition.SelectedItem.ToString();
            string remarkposition = drpremarkposition.SelectedItem.ToString();
            string barcodeposition = drpbarcodeposition.SelectedItem.ToString();
            string itemposition = Drpitemposition.SelectedItem.ToString();
            string itemposition1 = Drpitemposition1.SelectedItem.ToString();
            string addressposition = drpaddressposition.SelectedItem.ToString();
            string msg = string.Empty;
            if (bookingnoposition == customerposition || addressposition == bookingnoposition || bookingnoposition == processposition || bookingnoposition == remarkposition || bookingnoposition == barcodeposition || bookingnoposition == itemposition || bookingnoposition == itemposition1)
            {
                msg = msg + bookingnoposition + "&nbsp;";
                //lblMsgbarcode.Text =bookingnoposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (bookingnoposition == customerposition || addressposition == customerposition || customerposition == processposition || customerposition == remarkposition || customerposition == barcodeposition || customerposition == itemposition || customerposition == itemposition1)
            {
                msg = msg + customerposition + "&nbsp;";

                //lblMsgbarcode.Text =customerposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (processposition == customerposition || addressposition == processposition || bookingnoposition == processposition || processposition == remarkposition || processposition == barcodeposition || processposition == itemposition || processposition == itemposition1)
            {
                msg = msg + processposition + "&nbsp;";
                //lblMsgbarcode.Text =processposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (remarkposition == customerposition || addressposition == remarkposition || bookingnoposition == remarkposition || processposition == remarkposition || remarkposition == barcodeposition || remarkposition == itemposition || remarkposition == itemposition1)
            {
                msg = msg + remarkposition + "&nbsp;";
                //lblMsgbarcode.Text = remarkposition+"Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (barcodeposition == customerposition || addressposition == barcodeposition || bookingnoposition == barcodeposition || barcodeposition == remarkposition || remarkposition == barcodeposition || barcodeposition == itemposition || barcodeposition == itemposition1)
            {
                msg = msg + barcodeposition + "&nbsp;";
                //lblMsgbarcode.Text =barcodeposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (itemposition == customerposition || addressposition == itemposition || bookingnoposition == itemposition || itemposition == remarkposition || itemposition == barcodeposition || barcodeposition == itemposition || itemposition1 == itemposition)
            {
                msg = msg + itemposition + "&nbsp;";
                //lblMsgbarcode.Text =itemposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (itemposition1 == customerposition || addressposition == itemposition1 || bookingnoposition == itemposition1 || itemposition1 == remarkposition || itemposition1 == barcodeposition || barcodeposition == itemposition1 || itemposition1 == itemposition)
            {
                msg = msg + itemposition1 + "&nbsp;";
                //lblMsgbarcode.Text =itemposition+ "Positions cannot be Same";
                //fetchdemoandoriginalbarcode();
            }
            if (addressposition == customerposition || bookingnoposition == addressposition || addressposition == itemposition || addressposition == remarkposition || addressposition == barcodeposition || processposition == addressposition || itemposition1 == addressposition)
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

        //protected void DropBookingNoalign_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();

        //}

        //protected void Drpbookinfposition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();

        //}

        //protected void CheckBookingNobould_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void CheckBookingNoitilice_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void CheckBookingNounderline_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioBookingNoyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioBookingNorno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void radiocusyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiocusno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void drpfontname_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpcusalign_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpcusposition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checknamebold_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checknameitalac_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checknameunderline_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpnamesize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioProcessyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioProcessno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void drpcusfont_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void drpcussize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioItemTotalandSubTotalyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioItemTotalandSubTotalno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropcuscolour_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkbould_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkitilace_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkunderline_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpprocessposition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radioremarkyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();

        //}

        //protected void Radioremarkno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropremarkfont_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropremarksize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiocoloursyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiocoloursno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void drpremarkposition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropremarkalign_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkremarkbould_CheckedChanged(object sender, EventArgs e)
        //{
        //    demobarcodedisply();
        //}

        //protected void Checkremarkitilice_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkremarkunderline_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioBarcodeyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioBarcodeno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        ////protected void Dropbarcodesize_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        //////    fetchdemoandoriginalbarcode();
        //////}

        ////protected void Dropbarcodealign_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    fetchdemoandoriginalbarcode();
        ////}

        ////protected void drpbarcodeposition_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    fetchdemoandoriginalbarcode();
        ////}

        //protected void RadioItemyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioItemno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropitemfont_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropitemsize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioDateyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void RadioDateno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiotimeyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiotimeno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Dropitemalign_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkitembould_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkitemitilice_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkitemunderline_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpitemposition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radioaddressyes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radioaddressno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void drpaddressposition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpaddressfont_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpaddressalign_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Drpaddresssize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkaddressbold_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkaddressitalic_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Checkaddressunderline_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiodivideryes_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        //protected void Radiodividerno_CheckedChanged(object sender, EventArgs e)
        //{
        //    fetchdemoandoriginalbarcode();
        //}

        public DTO.Barcodeconfig SetWidthheight()
        {
            Ob.BranchId = Globals.BranchID;
            Ob.barcodewidth = barcodewirth.SelectedItem.ToString();
            Ob.barcodeheight = barcodehight.SelectedItem.ToString();
            return Ob;
        }

        protected void btset_Click(object sender, EventArgs e)
        {
            //SetWidthheight();
            //string res = string.Empty;
            //res = BAL.BALFactory.Instance.BAL_Barcodeconfig.Updatebarcodewidthheight(Ob);
            //if (res == "Record Saved")
            //{
            fetchdemoandoriginalbarcode();
            barcodepanel.Visible = true;
            panelwidth.Visible = true;
            barcodepaneldisplay.Visible = true;
            // }
        }
    }
}