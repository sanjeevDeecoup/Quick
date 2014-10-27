using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Collections;

namespace DAL
{
    public class DAL_NewBookingScreen
    {        
        XmlDocument doc = new XmlDocument();
        
        private DataTable EntBookingsDataTable()
        {
            DataTable booking = new DataTable();
            booking.Columns.Add("BookingNumber");
            booking.Columns.Add("BookingByCustomer");
            booking.Columns.Add("BookingAcceptedByUserId");
            booking.Columns.Add("IsUrgent");
            booking.Columns.Add("BookingDate");
            booking.Columns.Add("BookingDeliveryDate");
            booking.Columns.Add("BookingDeliveryTime");
            booking.Columns.Add("TotalCost");
            booking.Columns.Add("Discount");
            booking.Columns.Add("NetAmount");
            booking.Columns.Add("BookingRemarks");
            booking.Columns.Add("ItemTotalQuantity");
            booking.Columns.Add("HomeDelivery");
            booking.Columns.Add("CheckedByEmployee");
            booking.Columns.Add("DiscountAmt");
            booking.Columns.Add("DiscountOption");
            booking.Columns.Add("BranchId");
            booking.Columns.Add("IsNextTodayUrgent");
            booking.Columns.Add("AdvanceAmt");
            return booking;
        }
        private DataTable EntBookingDetailssDataTable()
        {
            DataTable booking = new DataTable();
            booking.Columns.Add("BookingNumber");
            booking.Columns.Add("ISN");
            booking.Columns.Add("ItemName");
            booking.Columns.Add("ItemTotalQuantity");
            booking.Columns.Add("ItemProcessType");
            booking.Columns.Add("ItemQuantityAndRate");
            booking.Columns.Add("ItemExtraProcessType1");
            booking.Columns.Add("ItemExtraProcessRate1");
            booking.Columns.Add("ItemExtraProcessType2");
            booking.Columns.Add("ItemExtraProcessRate2");
            booking.Columns.Add("ItemSubTotal");
            booking.Columns.Add("ItemRemark");
            booking.Columns.Add("ItemColor");
            booking.Columns.Add("STPAmt");
            booking.Columns.Add("STEP1Amt");
            booking.Columns.Add("STEP2Amt");
            booking.Columns.Add("BranchId");
            return booking;
        }
        private DataTable MakeProcessDataTable()
        {
            DataTable booking = new DataTable();
            booking.Clear();
            booking.Columns.Add("Id");
            booking.Columns.Add("process");
            booking.Columns.Add("rate");
            booking.Columns.Add("amount");
            booking.Columns.Add("text");
            booking.Columns.Add("MAINPROCESS");
            booking.Columns.Add("PROCESSRATE");
            booking.Columns.Add("EP1");
            booking.Columns.Add("EPR1");
            booking.Columns.Add("EP2");
            booking.Columns.Add("EPR2");
            booking.Columns.Add("ITEMSUBTOTAL");
            return booking;
        }
        private DataTable MakeColorDataTable()
        {
            DataTable booking = new DataTable();
            booking.Clear();
            booking.Columns.Add("Id");
            booking.Columns.Add("color");
            booking.Columns.Add("text");
            return booking;
        }
        private DataTable MakeItemDataTable()
        {
            DataTable booking = new DataTable();
            booking.Clear();
            booking.Columns.Add("Id");
            booking.Columns.Add("quantity");
            booking.Columns.Add("length");
            booking.Columns.Add("breadth");
            booking.Columns.Add("area1");
            booking.Columns.Add("remarks");
            booking.Columns.Add("brand");
            booking.Columns.Add("text");                
            return booking;
        }
        private DataTable GetProcessDetails(DTO.Common Ob)
        {
            DataTable dt = MakeProcessDataTable();           
            doc.Load(Ob.Path);
            int i = 0, j = 0, row = 0, temp = 0;
            string[] array;
            string Process = "";
            string Process2 = "";
            XmlNodeList nodes = doc.SelectNodes("/booking/lineitems/lineItem");
            foreach (XmlNode node in nodes)
            {
                //Read Line Item
                XmlNodeList processNodes = node.SelectNodes("./processes/process");
                XmlNodeList cList = doc.GetElementsByTagName("process");
                //Loop throu process nodes
                i++;
                row++;
                temp = 0;
                foreach (XmlNode process in processNodes)
                {
                    DataRow NewRow = dt.NewRow();
                    temp++;
                    if (temp > 3)
                    {
                        break;
                    }
                    else
                    {
                        NewRow["Id"] = i;
                        NewRow["process"] = cList.Item(j).Attributes["id"].InnerText;
                        NewRow["rate"] = process["rate"].InnerText;
                        NewRow["amount"] = process["amount"].InnerText;
                        NewRow["text"] = process["text"].InnerText;
                        Process = process["text"].InnerText;
                        array = Process.Split('@');
                        Process2 = array[0].ToString();
                        NewRow["MAINPROCESS"] = array[0].ToString();
                        dt.Rows.Add(NewRow);
                        dt.AcceptChanges();
                        DataRow[] dRow = dt.Select("Id=" + row + "");
                        if (dRow.Length == 1)
                        {
                            for (int iRow = 0; iRow < dRow.Length; iRow++)
                            {
                                NewRow["MAINPROCESS"] = array[0].ToString();
                                NewRow["PROCESSRATE"] = dRow[iRow]["rate"].ToString();
                                NewRow["EP1"] = "None";
                                NewRow["EPR1"] = "0";
                                NewRow["EP2"] = "None";
                                NewRow["EPR2"] = "0";
                            }
                        }
                        if (dRow.Length == 2)
                        {
                            bool status = false;
                            for (int iRow = 0; iRow < dRow.Length; iRow++)
                            {
                                if (status == false)
                                {
                                    NewRow["MAINPROCESS"] = array[0].ToString();
                                    NewRow["PROCESSRATE"] = dRow[iRow]["rate"].ToString();
                                    NewRow["EP2"] = "None";
                                    NewRow["EPR2"] = "0";
                                    status = true;
                                }
                                else
                                {
                                    NewRow["EP1"] = array[0].ToString();
                                    NewRow["EPR1"] = dRow[iRow]["rate"].ToString();
                                }
                            }
                        }
                        if (dRow.Length == 3)
                        {
                            bool status = false, status1 = false;
                            for (int iRow = 0; iRow < dRow.Length; iRow++)
                            {
                                if (status == false && status1 == false)
                                {
                                    NewRow["MAINPROCESS"] = array[0].ToString();
                                    NewRow["PROCESSRATE"] = dRow[iRow]["rate"].ToString();
                                    status = true;
                                }
                                else if (status == true && status1 == false)
                                {
                                    NewRow["EP1"] = array[0].ToString();
                                    NewRow["EPR1"] = dRow[iRow]["rate"].ToString();
                                    status1 = true;
                                }
                                else if (status == true && status1 == true)
                                {
                                    NewRow["EP2"] = array[0].ToString();
                                    NewRow["EPR2"] = dRow[iRow]["rate"].ToString();
                                }
                                else
                                { }
                            }
                        }
                        j++;
                    }
                }
            }           
            return dt;
        }
        private DataTable GetColors(DTO.Common Ob)
        {
            DataTable DC1 = MakeColorDataTable();
            doc.Load(Ob.Path);
            int i = 0, j = 0;
            bool status = false;
            XmlNodeList nodes = doc.SelectNodes("/booking/lineitems/lineItem");
            foreach (XmlNode node in nodes)
            {
                //Read Line Item
                XmlNodeList colornode = node.SelectNodes("./colors/color");
                XmlNodeList cList = doc.GetElementsByTagName("color");
                //Loop throu process nodes
                i++;
                status = false;
                DataRow NewRow = DC1.NewRow();
                foreach (XmlNode color in colornode)
                {
                    if (status == false)
                    {
                        NewRow["Id"] = i;
                        NewRow["color"] = cList.Item(j).Attributes["id"].InnerText;
                        NewRow["text"] = color["text"].InnerText;
                        DC1.Rows.Add(NewRow);
                        DC1.AcceptChanges();
                        j++;
                        status = true;
                    }
                    else
                    {
                        DC1.Rows[i - 1]["color"] = DC1.Rows[i - 1]["Id"].ToString() + "/" + cList.Item(j).Attributes["id"].InnerText;
                        DC1.Rows[i - 1]["text"] = DC1.Rows[i - 1]["text"].ToString() + "," + color["text"].InnerText;
                    }
                }
            }           
            return DC1;
        }
        private DataTable MainBooking(DTO.Common Ob)
        {
            DataTable DB1 = EntBookingsDataTable();
            doc.Load(Ob.Path);
            DataRow NewRow = DB1.NewRow();
            XmlNodeList bookList2 = doc.SelectNodes("/booking/receiptheader/iswalkin");
            XmlNodeList bookList1 = doc.SelectNodes("/booking/receiptheader/ishomebooking");
            XmlNodeList bookList = doc.SelectNodes("/booking/receiptheader");
            int j = 0, Quantity;
            foreach (XmlNode node in bookList)
            {
                NewRow["BookingNumber"] = bookList2.Item(0).SelectSingleNode("bookingnumber").InnerText;
                string HomeDelivery = bookList1.Item(0).SelectSingleNode("homeeceiptnumber").InnerText;
                NewRow["BookingByCustomer"] = node["customerid"].InnerText;
                NewRow["BookingAcceptedByUserId"] = Ob.UserId;
                NewRow["IsUrgent"] = node["isurgent"].InnerText;
                NewRow["BookingDate"] = DateTime.Today.ToString("dd-MMM-yyyy");
                NewRow["BookingDeliveryDate"] = node["duedate"].InnerText;
                NewRow["BookingDeliveryTime"] = node["duetime"].InnerText;
                NewRow["TotalCost"] = node["totalgrossamount"].InnerText;
                NewRow["Discount"] = node["totaldiscount"].InnerText;
                NewRow["AdvanceAmt"] = node["totaladvance"].InnerText;
                NewRow["NetAmount"] = float.Parse(node["totalgrossamount"].InnerText) - float.Parse(node["totaldiscount"].InnerText);
                NewRow["BookingRemarks"] = node["remarks"].InnerText;                
                Quantity=Convert.ToInt32(node["quantity"].InnerText.ToString());
                if ( Quantity == 0)
                {
                    Quantity = 1;
                }
                else
                {
                    NewRow["ItemTotalQuantity"] = Quantity;
                }
                NewRow["ItemTotalQuantity"] = Quantity;
                NewRow["HomeDelivery"] = HomeDelivery == "" ? "0" : "1";
                NewRow["CheckedByEmployee"] = node["checkedby"].InnerText;
                NewRow["DiscountAmt"] = node["totaldiscount"].InnerText;
                NewRow["DiscountOption"] = "";
                NewRow["BranchId"] = Ob.BID;
                NewRow["IsNextTodayUrgent"] = "";
            }
            DB1.Rows.Add(NewRow);
            DB1.AcceptChanges();
            j++;
            return DB1;
        }
        private DataTable GetData(DTO.Common Ob)
        {
            DataTable dt1 = EntBookingDetailssDataTable();
            DataTable DTBooking = MainBooking(Ob);
            DataTable DTProcess = GetProcessDetails(Ob);
            DataTable DTColors = GetColors(Ob);
            XmlDocument doc = new XmlDocument();
            doc.Load(Ob.Path);
            int i = 1, row = 0, ColorRow = 0,Quantity1;
            float Processrate = 0, ExtraProcessrate1 = 0, ExtraProcessrate2 = 0;

            XmlNodeList bookList1 = doc.SelectNodes("booking/lineitems/lineItem/item");
            foreach (XmlNode node in bookList1)
            {
                XmlElement bookElement1 = (XmlElement)node;
                DataRow NewRow1 = dt1.NewRow();
                NewRow1["BookingNumber"] = DTBooking.Rows[0]["BookingNumber"].ToString();
                NewRow1["ISN"] = i++;
                row++;
                ColorRow++;
                NewRow1["ItemName"] = node["text"].InnerText;
                Quantity1 = Convert.ToInt32(node["quantity"].InnerText.ToString());
                if (Quantity1 == 0)
                {
                    Quantity1 = 1;
                }
                else
                {
                    Quantity1 = Convert.ToInt32(node["quantity"].InnerText.ToString());
                }
                NewRow1["ItemTotalQuantity"] = Quantity1;
                NewRow1["ItemProcessType"] = node["text"].ToString();
                DataRow[] dRow = DTProcess.Select("Id=" + row + "");
                DataRow[] dColorRow = DTColors.Select("Id=" + row + "");
                if (dRow.Length == 1)
                {
                    for (int iRow = 0; iRow < dRow.Length; iRow++)
                    {
                        NewRow1["ItemProcessType"] = dRow[iRow]["MAINPROCESS"].ToString();
                        NewRow1["ItemQuantityAndRate"] =Quantity1  + "@" + dRow[iRow]["PROCESSRATE"].ToString();
                        NewRow1["ItemExtraProcessType1"] = dRow[iRow]["EP1"].ToString();
                        NewRow1["ItemExtraProcessRate1"] = dRow[iRow]["EPR1"].ToString();
                        NewRow1["ItemExtraProcessType2"] = dRow[iRow]["EP2"].ToString();
                        NewRow1["ItemExtraProcessRate2"] = dRow[iRow]["EPR2"].ToString();
                        Processrate = float.Parse(dRow[iRow]["PROCESSRATE"].ToString());
                        ExtraProcessrate1 = float.Parse(dRow[iRow]["EPR1"].ToString());
                        ExtraProcessrate2 = float.Parse(dRow[iRow]["EPR2"].ToString());
                    }
                }
                if (dRow.Length == 2)
                {
                    bool status = false;
                    for (int iRow = 0; iRow < dRow.Length; iRow++)
                    {
                        if (status == false)
                        {
                            NewRow1["ItemProcessType"] = dRow[iRow]["MAINPROCESS"].ToString();
                            NewRow1["ItemQuantityAndRate"] =Quantity1  + "@" + dRow[iRow]["PROCESSRATE"].ToString();
                            NewRow1["ItemExtraProcessType2"] = dRow[iRow]["EP2"].ToString();
                            NewRow1["ItemExtraProcessRate2"] = dRow[iRow]["EPR2"].ToString();
                            status = true;
                            Processrate = float.Parse(dRow[iRow]["PROCESSRATE"].ToString());
                            ExtraProcessrate1 = float.Parse(dRow[iRow]["EPR1"].ToString());
                            ExtraProcessrate2 = float.Parse(dRow[iRow]["EPR2"].ToString());

                        }
                        else
                        {
                            NewRow1["ItemExtraProcessType1"] = dRow[iRow]["EP1"].ToString();
                            NewRow1["ItemExtraProcessRate1"] = dRow[iRow]["EPR1"].ToString();
                            ExtraProcessrate1 = float.Parse(dRow[iRow]["EPR1"].ToString());
                            ExtraProcessrate2 = 0;
                        }
                    }
                }
                if (dRow.Length == 3)
                {
                    bool status = false, status1 = false;
                    for (int iRow = 0; iRow < dRow.Length; iRow++)
                    {
                        if (status == false && status1 == false)
                        {
                            NewRow1["ItemProcessType"] = dRow[iRow]["MAINPROCESS"].ToString();
                            NewRow1["ItemQuantityAndRate"] = Quantity1 + "@" + dRow[iRow]["PROCESSRATE"].ToString();
                            status = true;
                            Processrate = float.Parse(dRow[iRow]["PROCESSRATE"].ToString());
                            ExtraProcessrate1 = float.Parse(dRow[iRow]["EPR1"].ToString());
                            ExtraProcessrate2 = float.Parse(dRow[iRow]["EPR2"].ToString());
                        }
                        else if (status == true && status1 == false)
                        {
                            NewRow1["ItemExtraProcessType1"] = dRow[iRow]["EP1"].ToString();
                            NewRow1["ItemExtraProcessRate1"] = dRow[iRow]["EPR1"].ToString();
                            ExtraProcessrate1 = float.Parse(dRow[iRow]["EPR1"].ToString());
                            status1 = true;
                        }
                        else if (status == true && status1 == true)
                        {
                            NewRow1["ItemExtraProcessType2"] = dRow[iRow]["EP2"].ToString();
                            NewRow1["ItemExtraProcessRate2"] = dRow[iRow]["EPR2"].ToString();
                            ExtraProcessrate2 = float.Parse(dRow[iRow]["EPR2"].ToString());
                        }
                        else
                        { }
                    }
                }
                float Quantity = float.Parse(Quantity1.ToString());
                NewRow1["ItemSubTotal"] = Quantity * (Processrate) + (ExtraProcessrate1) + (ExtraProcessrate2);
                NewRow1["ItemRemark"] = node["remarks"].InnerText;
                NewRow1["ItemColor"] = DTColors.Rows[ColorRow - 1]["text"].ToString();
                NewRow1["BranchId"] = Ob.BID;
                dt1.Rows.Add(NewRow1);
                dt1.AcceptChanges();
            }
            return dt1;
        }
        private string SaveBooking(DTO.Common Ob)
        {
            string res = string.Empty;
            DataSet ds = new DataSet();
            DataTable DTBooking = MainBooking(Ob);
            SqlCommand CMD_Booking = new SqlCommand();
            CMD_Booking.CommandText = "sp_NewXmlSave";
            CMD_Booking.CommandType = CommandType.StoredProcedure;
            CMD_Booking.Parameters.AddWithValue("@BOOKINGNUMBER", DTBooking.Rows[0]["BookingNumber"].ToString());
            CMD_Booking.Parameters.AddWithValue("@BookingByCustomer", DTBooking.Rows[0]["BookingByCustomer"].ToString());
            CMD_Booking.Parameters.AddWithValue("@BookingAcceptedByUserId", DTBooking.Rows[0]["BookingAcceptedByUserId"].ToString());
            CMD_Booking.Parameters.AddWithValue("@IsUrgent", DTBooking.Rows[0]["IsUrgent"].ToString());
            //CMD_Booking.Parameters.AddWithValue("@BookingDate", DTBooking.Rows[0]["BookingDate"].ToString());
            CMD_Booking.Parameters.AddWithValue("@BookingDeliveryDate", DTBooking.Rows[0]["BookingDeliveryDate"].ToString());
            CMD_Booking.Parameters.AddWithValue("@BookingDeliveryTime", DTBooking.Rows[0]["BookingDeliveryTime"].ToString());
            CMD_Booking.Parameters.AddWithValue("@TotalCost", DTBooking.Rows[0]["TotalCost"].ToString());
            CMD_Booking.Parameters.AddWithValue("@Discount", DTBooking.Rows[0]["Discount"].ToString());
            CMD_Booking.Parameters.AddWithValue("@NetAmount", DTBooking.Rows[0]["NetAmount"].ToString());
            CMD_Booking.Parameters.AddWithValue("@BookingCancelReason", "");
            CMD_Booking.Parameters.AddWithValue("@BookingRemarks", DTBooking.Rows[0]["BookingRemarks"].ToString());
            CMD_Booking.Parameters.AddWithValue("@ItemTotalQuantity", DTBooking.Rows[0]["ItemTotalQuantity"].ToString());
            CMD_Booking.Parameters.AddWithValue("@HomeDelivery", DTBooking.Rows[0]["HomeDelivery"].ToString());
            CMD_Booking.Parameters.AddWithValue("@CheckedByEmployee", DTBooking.Rows[0]["CheckedByEmployee"].ToString());
            CMD_Booking.Parameters.AddWithValue("@DiscountAmt", DTBooking.Rows[0]["DiscountAmt"].ToString());
            CMD_Booking.Parameters.AddWithValue("@DiscountOption", DTBooking.Rows[0]["DiscountOption"].ToString());
            CMD_Booking.Parameters.AddWithValue("@BranchId", DTBooking.Rows[0]["BranchId"].ToString());
            CMD_Booking.Parameters.AddWithValue("@IsNextTodayUrgent", DTBooking.Rows[0]["IsNextTodayUrgent"].ToString());
            CMD_Booking.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(CMD_Booking);
            return res;
        }
        private string SaveAccountEntries(DTO.Common Ob)
        {
            string res = string.Empty;
            DataTable DTBooking = MainBooking(Ob);
            try
            {
                SqlCommand CMD_Priority = new SqlCommand();
                CMD_Priority.CommandText = "sp_NewXmlSave";
                CMD_Priority.CommandType = CommandType.StoredProcedure;
                CMD_Priority.Parameters.AddWithValue("@TotalCost", DTBooking.Rows[0]["TotalCost"].ToString());
                CMD_Priority.Parameters.AddWithValue("@CustomerCode", DTBooking.Rows[0]["BookingByCustomer"].ToString());
                CMD_Priority.Parameters.AddWithValue("@AdvanceAmt", DTBooking.Rows[0]["AdvanceAmt"].ToString());
                CMD_Priority.Parameters.AddWithValue("@BOOKINGNUMBER", DTBooking.Rows[0]["BookingNumber"].ToString());
                ArrayList date = new ArrayList();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BID);
                CMD_Priority.Parameters.AddWithValue("@DateTime", date[0].ToString());
                CMD_Priority.Parameters.AddWithValue("@Time", date[1].ToString());
                CMD_Priority.Parameters.AddWithValue("@BranchId", Ob.BID);
                CMD_Priority.Parameters.AddWithValue("@Flag", 3);
                res = PrjClass.ExecuteNonQuery(CMD_Priority);
            }
            catch (Exception ex)
            { res = ex.ToString(); }
            return res;
        }
        private string SaveBookingDetails(DTO.Common Ob)
        {
            string res = string.Empty;
            DataTable DTBookingDetails = GetData(Ob);
            for (int iRow = 0; iRow < DTBookingDetails.Rows.Count; iRow++)
            {
                SqlCommand CMD_BookingDetails = new SqlCommand();
                CMD_BookingDetails.CommandText = "sp_NewXmlSave";
                CMD_BookingDetails.CommandType = CommandType.StoredProcedure;
                CMD_BookingDetails.Parameters.AddWithValue("@ISN", DTBookingDetails.Rows[iRow]["ISN"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemName", DTBookingDetails.Rows[iRow]["ItemName"].ToString());
                if (DTBookingDetails.Rows[iRow]["ItemTotalQuantity"].ToString() == "0")
                {
                    DTBookingDetails.Rows[iRow]["ItemTotalQuantity"]= 1;
                }
                else
                {
                    CMD_BookingDetails.Parameters.AddWithValue("@ItemTotalQuantity", DTBookingDetails.Rows[iRow]["ItemTotalQuantity"].ToString());
                }
                
                CMD_BookingDetails.Parameters.AddWithValue("@ItemProcessType", DTBookingDetails.Rows[iRow]["ItemProcessType"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemQuantityAndRate", DTBookingDetails.Rows[iRow]["ItemQuantityAndRate"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessType1", DTBookingDetails.Rows[iRow]["ItemExtraProcessType1"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessRate1", DTBookingDetails.Rows[iRow]["ItemExtraProcessRate1"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessType2", DTBookingDetails.Rows[iRow]["ItemExtraProcessType2"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessRate2", DTBookingDetails.Rows[iRow]["ItemExtraProcessRate2"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemSubTotal", DTBookingDetails.Rows[iRow]["ItemSubTotal"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemRemark", DTBookingDetails.Rows[iRow]["ItemRemark"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@ItemColor", DTBookingDetails.Rows[iRow]["ItemColor"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@STPAmt", DTBookingDetails.Rows[iRow]["STPAmt"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@STEP1Amt", DTBookingDetails.Rows[iRow]["STEP1Amt"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@STEP2Amt", DTBookingDetails.Rows[iRow]["STEP2Amt"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@BOOKINGNUMBER", DTBookingDetails.Rows[iRow]["BookingNumber"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@BranchId", DTBookingDetails.Rows[iRow]["BranchId"].ToString());
                CMD_BookingDetails.Parameters.AddWithValue("@Flag", 2);
                res = PrjClass.ExecuteNonQuery(CMD_BookingDetails);
            }           
            return res;
        }
        private string SaveBarCode(DTO.Common Ob)
        {
            string res = string.Empty;
            DataTable DTBooking = MainBooking(Ob);
            SqlCommand CMD_Barcode = new SqlCommand();
            CMD_Barcode.CommandTimeout = 0;
            CMD_Barcode.CommandText = "sp_InsertIntoBarcodeTable";
            CMD_Barcode.CommandType = CommandType.StoredProcedure;
            CMD_Barcode.Parameters.AddWithValue("@BOOKINGNUMBER", DTBooking.Rows[0]["BookingNumber"].ToString());
            CMD_Barcode.Parameters.AddWithValue("@BranchId", DTBooking.Rows[0]["BranchId"].ToString());
            res = PrjClass.ExecuteNonQuery(CMD_Barcode);
            return res;
        }
        public string SaveRecordInTheDataBase(DTO.Common Ob)
        {
            string res = string.Empty;
            res = SaveBooking(Ob);
            if (res == "Record Saved")
            {
                res = SaveBookingDetails(Ob);
                if (res == "Record Saved")
                {
                    //sucess
                    res = SaveBarCode(Ob);
                    if (res == "Record Saved")
                    {
                        //sucess
                        res = SaveAccountEntries(Ob);
                        if (res == "Record Saved")
                        {
                            //sucess
                        }
                        else
                        {
                            return res;
                        }
                    }
                    else
                    {
                        return res;
                    }
                }
                else
                {
                    return res;
                }
            }
            else
            {
                
                return res;
            }
            return res;
        }
    }
}
