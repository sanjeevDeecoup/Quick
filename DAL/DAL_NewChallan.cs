using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    public class DAL_NewChallan
    {
        private bool status;
        DataTable dtBooking = new DataTable();
        public string SaveRemoveChallan(string allRemoveReasonData, string DeliverItemStatus, string UserName, string BID, string Flag, string ScreenName)
        {
            string res = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            var allData = allRemoveReasonData.Split('_');
            for (int r = 0; r < allData.Length; r++)
            {                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_Dry_DrawlMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(GStatus.Delivered));
                cmd.Parameters.AddWithValue("@DeliverItemStatus", DeliverItemStatus);
                cmd.Parameters.AddWithValue("@ClothDeliveryDate", date[0]);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@BookingNumber", allData[r].Split(':')[0].ToString().Trim());
                cmd.Parameters.AddWithValue("@RowIndex", allData[r].Split(':')[1].ToString().Trim());
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", Flag);
                cmd.Parameters.AddWithValue("@EntStatusTime", date[1]);
                res = PrjClass.ExecuteNonQuery(cmd);
                //if (res == "Record Saved")
                //{
                //    InsertStatusIn(BID, UserName, date[0].ToString(), allData[r].Split(':')[1].ToString().Trim(), date[1].ToString(), allData[r].Split(':')[0].ToString().Trim());
                //}   
                if (res == "Record Saved")
                {
                    MakeHistoryRecord(allData[r].Split(':')[0].ToString().Trim().ToUpper(), allData[r].Split(':')[1].ToString().Trim(), BID, DeliverItemStatus, allData[r].Split(':')[2].ToString().Trim(), r);
                }
            }  
         
            Task t = Task.Factory.StartNew
             (() => { SaveHistoryData(BID, UserName,ScreenName); });

            return res;
        }
        public string SaveInStickerTableDataFromWorkShop(string BID, int printFrom, string ChallanNumber)
        {
            string res = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "proc_NewChallanXML";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchID", BID);
                cmd.Parameters.AddWithValue("@printFrom", printFrom);
                cmd.Parameters.AddWithValue("@Flag", 9);
                cmd.Parameters.AddWithValue("@PrintStickerTrue", true);
                cmd.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            catch (Exception)
            {

            }
            return res;
        }
        private string InsertStatusIn(string BID, string UserName, string DeliveryDate, string RowIndex, string deliverTime, string strBookingNumber)
        {
            SqlCommand cmd = new SqlCommand();            
            SqlDataReader sdr = null;
            string sql = string.Empty, sql1 = string.Empty, BookingId = string.Empty;
            sql1 = "Select Id From BarCodeTable Where BookingNo='" + strBookingNumber + "' AND RowIndex='" + RowIndex + "' AND StatusId='" + Convert.ToInt32(GStatus.Delivered) + "' AND BranchId = '" + BID + "'";
            cmd.CommandText = sql1;
            sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
            {
                BookingId = "" + sdr.GetValue(0);
            }
            sdr.Dispose();
            sdr.Close();
            cmd.Dispose();
            DeliveryEntStatus(BID, UserName, DeliveryDate, BookingId, deliverTime, Convert.ToInt32(GStatus.Delivered));
            return BookingId;
        }

        public string SaveBarcodeWiseChallan(GridView grdChallan, string Shift, string RowIndex, string BID, string challanNo, string ExternalBID)
        {

            string res = string.Empty;
            string urgent = string.Empty;
            //TODO - Passing a dummy string to remove compilation error
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            for (int r = 0; r < grdChallan.Rows.Count; r++)
            {
                urgent = "";
                if (((Label)grdChallan.Rows[r].FindControl("lblUrgent")).Text == "Yes")
                    urgent = "1";
                else
                    urgent = "0";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_ChallanInProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanBranchCode", "HO");
                cmd.Parameters.AddWithValue("@ChallanNumber", challanNo);
                cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
                cmd.Parameters.AddWithValue("@ChallanSendingShift", Shift);
                cmd.Parameters.AddWithValue("@BookingNumber", grdChallan.Rows[r].Cells[0].Text);
                cmd.Parameters.AddWithValue("@ItemSNo", RowIndex);
                cmd.Parameters.AddWithValue("@SubItemName", ((Label)grdChallan.Rows[r].FindControl("lblItemName")).Text);
                cmd.Parameters.AddWithValue("@ItemTotalQuantitySent", ((Label)grdChallan.Rows[r].FindControl("lblItemQty")).Text);
                cmd.Parameters.AddWithValue("@Urgent", urgent);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@RowIndex", RowIndex);
                cmd.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
                if (status == false)
                {
                    cmd.Parameters.AddWithValue("@CStatus", Convert.ToInt32(GStatus.In_Processing));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Cstatus", Convert.ToInt32(GStatus.Factory_In));
                }

                cmd.Parameters.AddWithValue("@Flag", 1);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        private bool GetStatus(string BID)
        {
            status = true;
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 2);
            ds = PrjClass.GetData(CMD);
            if ((bool)ds.Tables[0].Rows[0]["IsChallan"] == false)
            {
                status = false;
            }
            else
            {
                status = true;
            }

            return status;
        }

        public string SaveEntryWiseChallan(GridView grdChallan, string Shift, string BID, string challanNo, string ExternalBID)
        {
            string res = string.Empty;
            string urgent = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            status = GetStatus(BID);
            for (int r = 0; r < grdChallan.Rows.Count; r++)
            {
                if (((CheckBox)grdChallan.Rows[r].FindControl("chkSelect")).Checked)
                {
                    urgent = "";
                    if (((Label)grdChallan.Rows[r].FindControl("lblUrgent")).Text == "Yes")
                        urgent = "1";
                    else
                        urgent = "0";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_ChallanInProc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanBranchCode", "HO");
                    cmd.Parameters.AddWithValue("@ChallanNumber", challanNo);
                    cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
                    cmd.Parameters.AddWithValue("@ChallanSendingShift", Shift);
                    cmd.Parameters.AddWithValue("@BookingNumber", grdChallan.Rows[r].Cells[1].Text);
                    cmd.Parameters.AddWithValue("@ItemSNo", ((HiddenField)grdChallan.Rows[r].FindControl("hdnItemSNo")).Value);
                    cmd.Parameters.AddWithValue("@SubItemName", ((Label)grdChallan.Rows[r].FindControl("lblItemName")).Text);
                    cmd.Parameters.AddWithValue("@ItemTotalQuantitySent", ((Label)grdChallan.Rows[r].FindControl("lblItemQty")).Text);
                    cmd.Parameters.AddWithValue("@Urgent", urgent);
                    cmd.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
                    if (status == false)
                    {
                        cmd.Parameters.AddWithValue("@CStatus", Convert.ToInt32(GStatus.In_Processing));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Cstatus", Convert.ToInt32(GStatus.Factory_In));
                    }
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@RowIndex", ((HiddenField)grdChallan.Rows[r].FindControl("hdnItemSNo")).Value);
                    cmd.Parameters.AddWithValue("@Flag", 1);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            return res;
        }

        // workshopNote => Added on 3/Aug/13, for that note in workshop out screen
        private string SaveWorkShopChallanFromRowData(string rowData, string ExternalBID, string workshopNote)
        {
            string res = string.Empty, challanNo = string.Empty;
            var ChallanId = 0;
            var urgent = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(ExternalBID);

            // var IdnNum = GetChallanIdAndNumber(ExternalBID);

            /* New Code */
            var allRowData = rowData.Split('_');

            var data = allRowData.AsEnumerable();
            var newData = data.OrderBy(n => n.Split(':')[3].Replace('*', ' ').Trim()).ToList();

            var prvId = string.Empty;

            string BookingNumber, ItemSNo, SubItemName, Urgent, QtyWiseBID;

            var IdnNum = GetChallanIdAndNumber(ExternalBID);
            ChallanId = IdnNum.Item1;
            challanNo = IdnNum.Item2;

            for (var i = 0; i < newData.Count; i++)
            {
                BookingNumber = newData[i].Split(':')[0];
                ItemSNo = newData[i].Split(':')[1];
                SubItemName = newData[i].Split(':')[2];
                QtyWiseBID = newData[i].Split(':')[3].Replace('*', ' ');
                /*
                if (QtyWiseBID != prvId)
                {
                    var IdnNum = GetChallanIdAndNumber(ExternalBID);
                    ChallanId = IdnNum.Item1;
                    challanNo = IdnNum.Item2;
                }
                */
                Urgent = newData[i].Split(':')[4];
                if (Urgent == "")
                {
                    Urgent = "0";
                }
                else
                {
                    Urgent = "1";
                }
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_ChallanInProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanNumber", challanNo);
                cmd.Parameters.AddWithValue("@ChallanBranchCode", "HO");
                cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
                cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@ItemSNo", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@SubItemName", SubItemName.Trim());
                cmd.Parameters.AddWithValue("@Urgent", Urgent.Trim());
                cmd.Parameters.AddWithValue("@BranchId", QtyWiseBID);
                cmd.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
                cmd.Parameters.AddWithValue("@ChallanId", ChallanId);
                cmd.Parameters.AddWithValue("@workshopNote", workshopNote);   // to do, add it to proc
                cmd.Parameters.AddWithValue("@Flag", 20);
                res = PrjClass.ExecuteNonQuery(cmd);

                prvId = QtyWiseBID;

                /* Saved */
            }

            /* New Code */
            return res;
        }

        protected Tuple<int, string> GetChallanIdAndNumber(string ExternalBID)
        {
            var ChallanId = PrjClass.getNewIDAccordingEXBID("WorkShopChallan", "ChallanId", ExternalBID);
            /* ChallanId */
            var challanNo = string.Empty;
            SqlCommand cmd1 = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataReader sdr = null;
            try
            {
                cmd1.CommandText = "sp_Dry_DrawlMaster";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
                cmd1.Parameters.AddWithValue("@Flag", 38);
                sdr = PrjClass.ExecuteReader(cmd1);                
                if (sdr.Read())
                {
                    challanNo = sdr.GetValue(0).ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd1.Dispose();
            }
            return new Tuple<int, string>(ChallanId, challanNo);
        }

        public string SaveEntryWiseChallanFromRowData(string rowData, string Shift, string BID, string challanNo, string ExternalBID, string AcceptedByUser)
        {
            string res = string.Empty;
            string urgent = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            status = GetStatus(BID);
            /* ChallanId */

            SqlCommand cmd1 = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataReader sdr = null;
            try
            {
                cmd1.CommandText = "sp_Dry_DrawlMaster";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@BranchId", BID);
                cmd1.Parameters.AddWithValue("@Flag", 26);
                sdr = PrjClass.ExecuteReader(cmd1);
                if (sdr.Read())
                {
                    challanNo = sdr.GetValue(0).ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd1.Dispose();
            }
            /* New Code */
            var allRowData = rowData.Split('_');
            string BookingNumber, ItemSNo, SubItemName, ItemTotalQuantitySent, Urgent;
            for (var i = 0; i < allRowData.Length; i++)
            {
                BookingNumber = allRowData[i].Split(':')[0];
                ItemSNo = allRowData[i].Split(':')[1];
                SubItemName = allRowData[i].Split(':')[2];
                ItemTotalQuantitySent = allRowData[i].Split(':')[3];
                Urgent = allRowData[i].Split(':')[4];
                if (Urgent == "")
                {
                    Urgent = "0";
                }
                else
                {
                    Urgent = "1";
                }
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_ChallanInProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanBranchCode", "HO");
                cmd.Parameters.AddWithValue("@ChallanNumber", challanNo);
                cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
                cmd.Parameters.AddWithValue("@ChallanSendingShift", Shift);
                cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@ItemSNo", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@SubItemName", SubItemName.Trim());
                cmd.Parameters.AddWithValue("@ItemTotalQuantitySent", ItemTotalQuantitySent.Trim());
                cmd.Parameters.AddWithValue("@Urgent", Urgent.Trim());
                cmd.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
                if (status == false)
                {
                    cmd.Parameters.AddWithValue("@CStatus", Convert.ToInt32(GStatus.In_Processing));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Cstatus", Convert.ToInt32(GStatus.Factory_In));
                }
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@RowIndex", ItemSNo);
                cmd.Parameters.AddWithValue("@AcceptedByUser", AcceptedByUser);
                cmd.Parameters.AddWithValue("@EntStatusTime", date[1].ToString());
                cmd.Parameters.AddWithValue("@Flag", 1);
                res = PrjClass.ExecuteNonQuery(cmd);
                /* Saved */
            }

            /* New Code */
            return res;
        }
        public string XMLSaveEntryWiseChallanFromRowData(string rowData, string BID, string ExternalBID, string AcceptedByUser)
        {
            string res = string.Empty;
            string urgent = string.Empty, challanNo = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            status = GetStatus(BID);
            /* ChallanId */

            SqlCommand cmd1 = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataReader sdr = null;
            try
            {
                cmd1.CommandText = "sp_Dry_DrawlMaster";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@BranchId", BID);
                cmd1.Parameters.AddWithValue("@Flag", 26);
                sdr = PrjClass.ExecuteReader(cmd1);
                if (sdr.Read())
                {
                    challanNo = sdr.GetValue(0).ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd1.Dispose();
            }
            string BookingNumber, ItemSNo, SubItemName, ItemTotalQuantitySent, Urgent;
            var ItemWithRates = rowData;
            var doc = new XDocument();
            var root = new XElement("root");
            doc.Add(root);

            var allItemsWithRates = ItemWithRates.Split('_');
            var branchIdAttrib = new XAttribute("BranchId", BID);
            for (var i = 0; i <= allItemsWithRates.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(allItemsWithRates[i].Split(':')[0].Trim()))
                    continue;

                BookingNumber = allItemsWithRates[i].Split(':')[0];
                ItemSNo = allItemsWithRates[i].Split(':')[1];
                SubItemName = allItemsWithRates[i].Split(':')[2];
                ItemTotalQuantitySent = allItemsWithRates[i].Split(':')[3];
                Urgent = allItemsWithRates[i].Split(':')[4];
                if (Urgent == "")
                {
                    Urgent = "0";
                }
                else
                {
                    Urgent = "1";
                }
                if (SubItemName != "")
                {
                    var itemElement = new XElement("row", new XAttribute("BookingNumber", BookingNumber), new XAttribute("ItemSNo", ItemSNo), new XAttribute("SubItemName", SubItemName), new XAttribute("Urgent", Urgent), branchIdAttrib);
                    root.Add(itemElement);
                }

            }            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_NewChallanXML";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            cmd.Parameters.AddWithValue("@xml", doc.ToString());
            cmd.Parameters.AddWithValue("@ChallanNumber", challanNo);
            cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
            cmd.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
            if (status == false)
            {
                cmd.Parameters.AddWithValue("@CStatus", Convert.ToInt32(GStatus.In_Processing));
            }
            else
            {
                cmd.Parameters.AddWithValue("@Cstatus", Convert.ToInt32(GStatus.Factory_In));
            }
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@AcceptedByUser", AcceptedByUser);
            cmd.Parameters.AddWithValue("@EntStatusTime", date[1].ToString());
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);

            /* New Code */
            return res;
        }
        public string SaveEntryWiseChallanReturnRowData(string rowData, string BID, string Sataus, string AcceptedByUser)
        {
            string res = string.Empty;
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            /* New Code */
            var allRowData = rowData.Split('_');
            string BookingNumber = string.Empty, ItemSNo = string.Empty, SubItemName = string.Empty, ItemTotalQuantitySent = string.Empty;
            for (var i = 0; i < allRowData.Length; i++)
            {
                BookingNumber = allRowData[i].Split(':')[0];
                ItemSNo = allRowData[i].Split(':')[1];
                SubItemName = allRowData[i].Split(':')[2];
                //ItemTotalQuantitySent = allRowData[i].Split(':')[3];
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@ItemName", SubItemName.Trim());
                if (Sataus == "Save")
                    cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(GStatus.Cloth_Ready));
                else
                    cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(GStatus.SendTo_SteamPress));
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 35);
                cmd.Parameters.AddWithValue("@AcceptedByUser", AcceptedByUser);
                cmd.Parameters.AddWithValue("@EntStatusTime1", date[1].ToString());
                res = PrjClass.ExecuteNonQuery(cmd);
                /* Saved */
            }
            /* New Code */
            return res;
        }

        public string SaveInStickerTable(string rowData, string BID, int PrintFrom)
        {
            string res = string.Empty;
            //
            // dummy save set position
            var allRowData = rowData.Split('_');
            string BookingNumber = string.Empty, ItemSNo = string.Empty, SubItemName = string.Empty, ItemTotalQuantitySent = string.Empty;
            if (PrintFrom != 1)
            {
                for (int j = 1; j < PrintFrom; j++)
                {
                    BookingNumber = allRowData[0].Split(':')[0];
                    ItemSNo = allRowData[0].Split(':')[1];
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Proc_BarCodeLabels";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                    cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                    cmd.Parameters.AddWithValue("@Flag", 9);
                    PrjClass.ExecuteNonQuery(cmd);
                }
            }
            /* New Code */

            for (var i = 0; i < allRowData.Length; i++)
            {
                BookingNumber = allRowData[i].Split(':')[0];
                ItemSNo = allRowData[i].Split(':')[1];
                SubItemName = allRowData[i].Split(':')[2];
                //ItemTotalQuantitySent = allRowData[i].Split(':')[3];
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Proc_BarCodeLabels";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@Active", "1");
                cmd.Parameters.AddWithValue("@BarCode", BookingNumber.Trim() + "-" + ItemSNo.Trim() + "-" + BID);
                cmd.Parameters.AddWithValue("@Flag", 3);
                res = PrjClass.ExecuteNonQuery(cmd);
                /* Saved */
            }
            /* New Code */
            return res;
        }

        public string SaveInStickerTableFromFactory(string rowData, string BID, int PrintFrom)
        {
            string res = string.Empty;
            //
            // dummy save set position
            var allRowData = rowData.Split('_');
            string BookingNumber = string.Empty, ItemSNo = string.Empty, SubItemName = string.Empty, ItemTotalQuantitySent = string.Empty, BranchId = string.Empty;
            if (PrintFrom != 1)
            {
                for (int j = 1; j < PrintFrom; j++)
                {
                    BookingNumber = allRowData[0].Split(':')[0];
                    ItemSNo = allRowData[0].Split(':')[1];
                    BranchId = allRowData[0].Split(':')[3].Replace('*', ' ');
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Proc_BarCodeLabels";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                    cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                    cmd.Parameters.AddWithValue("@BranchId", BranchId.Trim());
                    cmd.Parameters.AddWithValue("@Flag", 10);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            /* New Code */

            for (var i = 0; i < allRowData.Length; i++)
            {
                BookingNumber = allRowData[i].Split(':')[0];
                ItemSNo = allRowData[i].Split(':')[1];
                SubItemName = allRowData[i].Split(':')[2];
                // this is actually the store id
                ItemTotalQuantitySent = allRowData[i].Split(':')[3].Replace('*', ' ');
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Proc_BarCodeLabels";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@Active", "1");
                cmd.Parameters.AddWithValue("@BarCode", BookingNumber.Trim() + "-" + ItemSNo.Trim() + "-" + ItemTotalQuantitySent.Trim());
                cmd.Parameters.AddWithValue("@BranchId", ItemTotalQuantitySent.Trim());
                cmd.Parameters.AddWithValue("@Flag", 11);
                res = PrjClass.ExecuteNonQuery(cmd);
                /* Saved */
            }
            /* New Code */
            return res;
        }

        public DataSet BindGrid(DTO.Common Ob)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_Dry_BarcodeMaster";
            CMD.Parameters.AddWithValue("@BranchId", Ob.BID);
            CMD.Parameters.AddWithValue("@UserTypeId", Ob.Id);
            CMD.Parameters.AddWithValue("@Flag", 30);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet BindDropDown(string BID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 3);

            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet BindMultiFactory()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public bool CheckIsFactory(string BID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 5);
            ds = PrjClass.GetData(CMD);
            if ((bool)ds.Tables[0].Rows[0]["IsChallan"] == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

        public bool DefaultFactory(string BID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(CMD);
            if ((bool)ds.Tables[0].Rows[0]["IsFactory"] == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

        public bool CheckStatus()
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "Proc_BarCodeLabels";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 14);
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

        public string SaveBarcodeWiseCheckOut(GridView grdChallan, string RowIndex, string BID)
        {
            string res = string.Empty;
            string urgent = string.Empty;
            //TODO - Passing a dummy string to remove compilation error
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            for (int r = 0; r < grdChallan.Rows.Count; r++)
            {
                urgent = "";
                if (((Label)grdChallan.Rows[r].FindControl("lblUrgent")).Text == "Yes")
                    urgent = "1";
                else
                    urgent = "0";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_ChallanInProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanBranchCode", "HO");

                cmd.Parameters.AddWithValue("@ChallanDate", date[0]);

                cmd.Parameters.AddWithValue("@BookingNumber", grdChallan.Rows[r].Cells[0].Text);
                cmd.Parameters.AddWithValue("@ItemSNo", RowIndex);
                cmd.Parameters.AddWithValue("@SubItemName", ((Label)grdChallan.Rows[r].FindControl("lblItemName")).Text);
                cmd.Parameters.AddWithValue("@ItemTotalQuantitySent", ((Label)grdChallan.Rows[r].FindControl("lblItemQty")).Text);
                cmd.Parameters.AddWithValue("@Urgent", urgent);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@RowIndex", RowIndex);
                cmd.Parameters.AddWithValue("@Cstatus", Convert.ToInt32(GStatus.Factory_Out));
                cmd.Parameters.AddWithValue("@Flag", 7);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        public string SaveEntryWiseCheckout(GridView grdChallan, string BID)
        {
            string res = string.Empty;
            string urgent = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);

            for (int r = 0; r < grdChallan.Rows.Count; r++)
            {
                if (((CheckBox)grdChallan.Rows[r].FindControl("chkSelect")).Checked)
                {
                    urgent = "";
                    if (((Label)grdChallan.Rows[r].FindControl("lblUrgent")).Text == "Yes")
                        urgent = "1";
                    else
                        urgent = "0";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_ChallanInProc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanBranchCode", "HO");
                    cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
                    cmd.Parameters.AddWithValue("@BookingNumber", grdChallan.Rows[r].Cells[1].Text);
                    cmd.Parameters.AddWithValue("@ItemSNo", ((HiddenField)grdChallan.Rows[r].FindControl("hdnItemSNo")).Value);
                    cmd.Parameters.AddWithValue("@SubItemName", ((Label)grdChallan.Rows[r].FindControl("lblItemName")).Text);
                    cmd.Parameters.AddWithValue("@ItemTotalQuantitySent", ((Label)grdChallan.Rows[r].FindControl("lblItemQty")).Text);
                    cmd.Parameters.AddWithValue("@Urgent", urgent);
                    cmd.Parameters.AddWithValue("@Cstatus", Convert.ToInt32(GStatus.Factory_Out));
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@RowIndex", ((HiddenField)grdChallan.Rows[r].FindControl("hdnItemSNo")).Value);
                    cmd.Parameters.AddWithValue("@Flag", 7);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            return res;
        }

        public DataSet BindAllProcess()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@Flag", 8);

            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet BindTotGrid()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@Flag", 9);
            ds = PrjClass.GetData(CMD);

            return ds;
        }

        public DataSet BindTotGridCheck()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@Flag", 12);
            ds = PrjClass.GetData(CMD);

            return ds;
        }

        public string SaveEntryWise(GridView grdChallan)
        {
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "sp_ChallanInProc";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Flag", 15);
            PrjClass.ExecuteNonQuery(cmd1);
            string res = string.Empty;
            string urgent = string.Empty;

            for (int r = 0; r < grdChallan.Rows.Count; r++)
            {
                if (((CheckBox)grdChallan.Rows[r].FindControl("chkSelect")).Checked)
                {
                    urgent = "";
                    if (((Label)grdChallan.Rows[r].FindControl("lblUrgent")).Text == "Yes")
                        urgent = "1";
                    else
                        urgent = "0";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_ChallanInProc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingNumber", grdChallan.Rows[r].Cells[1].Text);
                    cmd.Parameters.AddWithValue("@DueDate", grdChallan.Rows[r].Cells[2].Text);
                    cmd.Parameters.AddWithValue("@BarCode", ((Label)grdChallan.Rows[r].FindControl("lblBarcode")).Text);
                    cmd.Parameters.AddWithValue("@Customer", grdChallan.Rows[r].Cells[4].Text);
                    cmd.Parameters.AddWithValue("@SubItemName", ((Label)grdChallan.Rows[r].FindControl("lblItemName")).Text);
                    cmd.Parameters.AddWithValue("@Urgent", urgent);
                    cmd.Parameters.AddWithValue("@ItemProcessType", ((Label)grdChallan.Rows[r].FindControl("lblMainProcess")).Text);
                    cmd.Parameters.AddWithValue("@ItemExtraProcessType1", ((Label)grdChallan.Rows[r].FindControl("lblExtraProcess")).Text);
                    cmd.Parameters.AddWithValue("@ItemExtraProcessType2", ((Label)grdChallan.Rows[r].FindControl("lblExtraProcess1")).Text);
                    cmd.Parameters.AddWithValue("@GQty", ((Label)grdChallan.Rows[r].FindControl("lblItemQty")).Text);
                    cmd.Parameters.AddWithValue("@BranchName", grdChallan.Rows[r].Cells[11].Text);
                    cmd.Parameters.AddWithValue("@BranchCode", grdChallan.Rows[r].Cells[12].Text);
                    cmd.Parameters.AddWithValue("@BranchId", ((HiddenField)grdChallan.Rows[r].FindControl("hidBookingID")).Value);
                    //cmd.Parameters.AddWithValue("@RowIndex", ((HiddenField)grdChallan.Rows[r].FindControl("hdnItemSNo")).Value);
                    cmd.Parameters.AddWithValue("@Flag", 13);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            return res;
        }

        public DataSet BindTmpGrid(string BID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 14);
            ds = PrjClass.GetData(CMD);

            return ds;
        }

        public void DeleteFactoryGrid()
        {
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "sp_ChallanInProc";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Flag", 15);
            PrjClass.ExecuteNonQuery(cmd1);
        }

        /* All Process & All Branch and No Due Date (Bind Grid View in Factory Out) -  Start */

        public DataSet AllBPNoDue(String BID)
        {
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_dry_NewChallan";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 13);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        /* Only Invoice Number (Bind Grid View in Factory Out) -  Start */

        public DataSet AllBPNoDueInvoice(string BID, string Invoice)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@BookingNo", Invoice);
            cmd.Parameters.AddWithValue("@Flag", 14);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* Any Process & Any Branch With Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AnyBPWithDue(string BID, string ExBID, string processcode, string DueDate)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ExtBranchId", ExBID);
            cmd.Parameters.AddWithValue("@ProcessCode", processcode);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@Flag", 15);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* Any Process & All Branch With No Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AnyProcessNoDueBranch(string BID, string processcode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@ExtBranchId", BID);
            cmd.Parameters.AddWithValue("@ProcessCode", processcode);
            cmd.Parameters.AddWithValue("@Flag", 16);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* All Process & Any Branch With No Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AnyBranchNoDueProcess(string BID, string ExBID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ExtBranchId", ExBID);
            cmd.Parameters.AddWithValue("@Flag", 17);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* Any Process & Any Branch No Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AnyBPNoDue(string BID, string ExBID, string processcode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ExtBranchId", ExBID);
            cmd.Parameters.AddWithValue("@ProcessCode", processcode);
            cmd.Parameters.AddWithValue("@Flag", 18);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* All Branch & All Process With Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AllBPWithDue(string ExBID, string DueDate)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@BranchId", ExBID);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@Flag", 19);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* All Branch & Any Process With Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AllBAnyPWithDue(string ExBID, string ProcessCode, string DueDate)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@ExtBranchId", ExBID);
            cmd.Parameters.AddWithValue("@Processcode", ProcessCode);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@Flag", 20);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* Any Process & All Process With Due Date  (Bind Grid View in Factory Out) -  Start */

        public DataSet AnyBAllPWithDue(string BID, string ExBID, string DueDate)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ExtBranchId", ExBID);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@Flag", 21);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /* Save Challan in Factory */

        public string SaveEntryFactoryOut(GridView grdTemp, string BID, string challanNo, string ExternalBID)
        {
            string res = string.Empty;
            string urgent = string.Empty;
            String RowIndex = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            status = GetStatus(BID);
            for (int r = 0; r < grdTemp.Rows.Count; r++)
            {
                string[] gridbooking = ((Label)grdTemp.Rows[r].FindControl("lblBarcode")).Text.Split('-');

                RowIndex = gridbooking[1].Replace("*", "").Trim();

                urgent = "";
                if (((Label)grdTemp.Rows[r].FindControl("lblUrgent")).Text == "Yes")
                    urgent = "1";
                else
                    urgent = "0";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_ChallanInProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanNumber", challanNo);
                cmd.Parameters.AddWithValue("@ChallanDate", date[0]);
                cmd.Parameters.AddWithValue("@BookingNumber", grdTemp.Rows[r].Cells[0].Text);
                cmd.Parameters.AddWithValue("@ItemSNo", ((HiddenField)grdTemp.Rows[r].FindControl("hdnItemSNo")).Value);
                cmd.Parameters.AddWithValue("@SubItemName", ((Label)grdTemp.Rows[r].FindControl("lblItemName")).Text);
                cmd.Parameters.AddWithValue("@ItemTotalQuantitySent", ((Label)grdTemp.Rows[r].FindControl("lblItemQty1")).Text);
                cmd.Parameters.AddWithValue("@Urgent", urgent);
                cmd.Parameters.AddWithValue("@ExternalBranchId", ExternalBID);
                cmd.Parameters.AddWithValue("@CStatus", Convert.ToInt32(GStatus.In_Processing));

                cmd.Parameters.AddWithValue("@BranchId", BID);

                cmd.Parameters.AddWithValue("@RowIndex", RowIndex);
                cmd.Parameters.AddWithValue("@Flag", 16);
                res = PrjClass.ExecuteNonQuery(cmd);
            }

            return res;
        }

        public DataSet factoryChallanNo(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@Flag", 32);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetDataChallanReturnScreenFirst(string BID, string BookNumberFrom, string BookNumberUpto, string DueDate, string ChallanShift, string Process, string challanNumber = "")
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_ChallanReturnDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookNumberFrom", BookNumberFrom);
            cmd.Parameters.AddWithValue("@BookNumberUpto", BookNumberUpto);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@ChallanShift", ChallanShift);
            cmd.Parameters.AddWithValue("@Process", Process);
            cmd.Parameters.AddWithValue("@BranchID", BID);
            if (!string.IsNullOrEmpty(challanNumber.Trim()))
            {
                cmd.Parameters.AddWithValue("@Flag", 2);
                cmd.Parameters.AddWithValue("@ChallanNumber", challanNumber);
            }
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataSteamPressScreen(string BID, string BookNumberFrom, string DueDate, string Process)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_SteamPressDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@BookNumberFrom", BookNumberFrom);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@Process", Process);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataChallanReturnScreenSecond(string BID, string BookNumberFrom, string BookNumberUpto, string DueDate, string ProcessCode)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_ChallanReturnDetailsWithProcess";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookNumberFrom", BookNumberFrom);
            cmd.Parameters.AddWithValue("@BookNumberUpto", BookNumberUpto);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@ProcessCode", ProcessCode);
            cmd.Parameters.AddWithValue("@BranchID", BID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public string GetChallanNumberFromBookingAndItemSNo(string bookingNumber, string itemSerialNumber, string BID, bool isFactory = false)
        {
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 17);
            CMD.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            CMD.Parameters.AddWithValue("@ItemSNo", itemSerialNumber);
            var res = string.Empty;
            if (isFactory)
            {
                var ds = PrjClass.GetData(CMD);
                res = ds.Tables[1].Rows[0][0].ToString();
            }
            else
            {
                res = PrjClass.ExecuteScalar(CMD);
            }
            CMD.Dispose();
            return res;
        }

        public string findDefaultChallanType(string BID)
        {
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 18);
            var res = PrjClass.ExecuteScalar(CMD);
            return res;
        }

        public DataSet BindRightGrid(string BID,int status,string UserId)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.CommandType = CommandType.StoredProcedure;          
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@AcceptedByUser", UserId);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Flag", 23);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindGridView(string BID, DropDownList drpProcess, TextBox txtHolidayDate, HiddenField hdnCheckStatus, HiddenField hdnInvoiceNo, HiddenField hdnRowNo, bool challanStatus = false)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ChallanStatus", challanStatus);
            if (drpProcess.SelectedIndex == -1 || drpProcess.SelectedIndex == 0)
            {
                if (hdnCheckStatus.Value == "0")
                {
                    cmd.Parameters.AddWithValue("@DueDate", txtHolidayDate.Text);
                    cmd.Parameters.AddWithValue("@Flag", 1);
                }
                else if (hdnCheckStatus.Value == "1")
                {
                    cmd.Parameters.AddWithValue("@BookingNo", hdnInvoiceNo.Value);
                    cmd.Parameters.AddWithValue("@Flag", 2);
                }
                else if (hdnCheckStatus.Value == "2")
                {
                    cmd.Parameters.AddWithValue("@BookingNo", hdnInvoiceNo.Value);
                    cmd.Parameters.AddWithValue("@RowIndex", hdnRowNo.Value);
                    cmd.Parameters.AddWithValue("@Flag", 3);
                }
            }
            else
            {
                if (hdnCheckStatus.Value == "0")
                {
                    cmd.Parameters.AddWithValue("@ProcessCode", drpProcess.SelectedValue);
                    cmd.Parameters.AddWithValue("@DueDate", txtHolidayDate.Text);

                    cmd.Parameters.AddWithValue("@Flag", 10);
                }
                else if (hdnCheckStatus.Value == "1")
                {
                    cmd.Parameters.AddWithValue("@ProcessCode", drpProcess.SelectedValue);
                    cmd.Parameters.AddWithValue("@BookingNo", hdnInvoiceNo.Value);
                    cmd.Parameters.AddWithValue("@DueDate", txtHolidayDate.Text);

                    cmd.Parameters.AddWithValue("@Flag", 11);
                }
                else if (hdnCheckStatus.Value == "2")
                {
                    cmd.Parameters.AddWithValue("@ProcessCode", drpProcess.SelectedValue);
                    cmd.Parameters.AddWithValue("@BookingNo", hdnInvoiceNo.Value);
                    cmd.Parameters.AddWithValue("@RowIndex", hdnRowNo.Value);
                    cmd.Parameters.AddWithValue("@DueDate", txtHolidayDate.Text);

                    cmd.Parameters.AddWithValue("@Flag", 12);
                }
            }
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindFactoryInGrid(string BID, string EXBID, string BookingNo, string BookingDate, string DueDate, bool IsUrgent)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_FactoryIn";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ExtBranchId", EXBID);
            cmd.Parameters.AddWithValue("@BookNumberFrom", BookingNo);
            cmd.Parameters.AddWithValue("@BookingDate", BookingDate);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@IsUrgent", IsUrgent);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindCustomerWise(string BID, string CustomerName)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 22);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindFactoryOutGrid(string BID, string EXBID, string BookingNo, string BookingDate, string DueDate , bool IsUrgent)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Sel_FactoryOut";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@ExtBranchId", EXBID);
            cmd.Parameters.AddWithValue("@BookNumberFrom", BookingNo);
            cmd.Parameters.AddWithValue("@BookingDate", BookingDate);
            cmd.Parameters.AddWithValue("@DueDate", DueDate);
            cmd.Parameters.AddWithValue("@IsUrgent", IsUrgent);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public string SaveEntryWiseFactoryInRowData(string rowData, string BID, string AcceptedByUser)
        {
            string res = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            /* New Code */
            var allRowData = rowData.Split('_');
            string BookingNumber = string.Empty, ItemSNo = string.Empty, SubItemName = string.Empty, QtyWiseBID = string.Empty;
            for (var i = 0; i < allRowData.Length; i++)
            {
                BookingNumber = allRowData[i].Split(':')[0];
                ItemSNo = allRowData[i].Split(':')[1];
                SubItemName = allRowData[i].Split(':')[2];
                QtyWiseBID = allRowData[i].Split(':')[3].Replace('*', ' ');
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(GStatus.Factory_Out));
                cmd.Parameters.AddWithValue("@FBID", BID);
                cmd.Parameters.AddWithValue("@BranchId", QtyWiseBID.Trim());
                cmd.Parameters.AddWithValue("@AcceptedByUser", AcceptedByUser);
                cmd.Parameters.AddWithValue("@EntStatusTime1", date[1].ToString());
                cmd.Parameters.AddWithValue("@Flag", 36);
                res = PrjClass.ExecuteNonQuery(cmd);
                /* Saved */
            }

            return res;
        }

        public string SaveEntryWiseFactoryOutRowData(string rowData, string BID, string AcceptedByUser, string workshopNote)
        {
            string res = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            /* New Code */
            var allRowData = rowData.Split('_');

            string BookingNumber = string.Empty, ItemSNo = string.Empty, SubItemName = string.Empty, QtyWiseBID = string.Empty;
            for (var i = 0; i < allRowData.Length; i++)
            {
                BookingNumber = allRowData[i].Split(':')[0];
                ItemSNo = allRowData[i].Split(':')[1];
                SubItemName = allRowData[i].Split(':')[2];
                QtyWiseBID = allRowData[i].Split(':')[3].Replace('*', ' ');
                /* Save Data */
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", BookingNumber.Trim());
                cmd.Parameters.AddWithValue("@RowIndex", ItemSNo.Trim());
                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(GStatus.In_Processing));
                cmd.Parameters.AddWithValue("@FBID", BID);
                cmd.Parameters.AddWithValue("@BranchId", QtyWiseBID.Trim());
                cmd.Parameters.AddWithValue("@AcceptedByUser", AcceptedByUser);
                cmd.Parameters.AddWithValue("@EntStatusTime1", date[1].ToString());
                cmd.Parameters.AddWithValue("@Flag", 36);
                res = PrjClass.ExecuteNonQuery(cmd);
                /* Saved */
            }
            /* Makeing Challan */

            if (res == "Record Saved")
            {
                res = SaveWorkShopChallanFromRowData(rowData, BID, workshopNote);
            }
            return res;
        }

        public DataSet GetDataForPritingLablesForWorkshop(string challanNum, string externalBranchId)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@BranchId", externalBranchId);
                sqlCommand.Parameters.AddWithValue("@ChallanNumber", challanNum);
                sqlCommand.Parameters.AddWithValue("@Flag", 22);
                return PrjClass.GetData(sqlCommand);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet GetDataForAllPrintingLabelsForWorkshop(string challanNum, string externalBranchId)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@BranchId", externalBranchId);
                sqlCommand.Parameters.AddWithValue("@ChallanNumber", challanNum);
                sqlCommand.Parameters.AddWithValue("@Flag", 27);
                return PrjClass.GetData(sqlCommand);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public string AskForBarCodeAndPIN(string branchId, string UserId, string UserType)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Flag", 23);
                //sqlCommand.Parameters.AddWithValue("@Customer", UserId);
                sqlCommand.Parameters.AddWithValue("@Id", UserType);
                var sqlDataReader = PrjClass.ExecuteReader(sqlCommand);
                var BarcodeAndPin = string.Empty;
                while (sqlDataReader != null & sqlDataReader.Read())
                {
                    BarcodeAndPin += sqlDataReader.GetString(0) + "~";
                }
                if (sqlDataReader != null) 
                    sqlDataReader.Close();
                /*if (BarcodeAndPin.Length >= 2)
                    BarcodeAndPin = BarcodeAndPin.Substring(0, BarcodeAndPin.Length - 1); */
                // if the barcode is only this tilde, then it is because there is no valid data
                if (BarcodeAndPin == "~")
                    BarcodeAndPin = string.Empty;
                return BarcodeAndPin;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string SavePinInBarcode(string PIN, string barCodes, string branchId)
        {
            try
            {
                var codes = barCodes.Split('~');
                var codesStr = string.Empty; //"(";
                for (var i = 0; i < codes.Length; i++)
                {
                    codesStr += codes[i] + ", ";
                }
                if (codesStr.Length >= 2)
                    codesStr = codesStr.Substring(0, codesStr.Length - 2);

                //codesStr += ")";

                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Pin", PIN);
                sqlCommand.Parameters.AddWithValue("@Barcode", codesStr);
                sqlCommand.Parameters.AddWithValue("@Flag", 24);
                return PrjClass.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string DeliveryEntStatus(string branchId, string UserId, string DeliveryDate, string BookingId, string deliverTime, int StatusID)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@CurBarCodeTableId1", BookingId);
                sqlCommand.Parameters.AddWithValue("@Cstatus", StatusID);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserId);
                sqlCommand.Parameters.AddWithValue("@EntStatusTime1", deliverTime);
                sqlCommand.Parameters.AddWithValue("@DueDate", DeliveryDate);
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Flag", 26);
                return PrjClass.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet GetAllActiveChallans(string branchId)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Flag", 25);
                return PrjClass.GetData(sqlCommand);
            }
            catch (Exception)
            {
                return null;
            }
 
        }

        public bool ChangeChallanStatus(string barCodes, int challanStatus,string BID,string userId)
        {
            
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@Barcode", barCodes);
                sqlCommand.Parameters.AddWithValue("@ChallanStatus", challanStatus);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", userId);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@Flag", 30);
                sqlCommand.CommandTimeout = 120;
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveChallanDetails(string BID, string EBID, string strdate, string strTime, string UserName,bool PrintSticker,int Possition)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "Proc_NewChallanXML", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@ExternalBranchId", EBID);
                sqlCommand.Parameters.AddWithValue("@ChallanDate", strdate);
                sqlCommand.Parameters.AddWithValue("@EntStatusTime", strTime);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserName);
                sqlCommand.Parameters.AddWithValue("@PrintStickerTrue", PrintSticker);
                sqlCommand.Parameters.AddWithValue("@PrintFrom", Possition);
                sqlCommand.Parameters.AddWithValue("@Flag",1);               
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendForFinishingOrReadyDetail(string BID, string Status, string strdate, string strTime, string UserName, bool PrintSticker, int Possition, int ScreenStatus, string PinNo)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "Proc_NewChallanXML", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@CStatus", Status);
                sqlCommand.Parameters.AddWithValue("@ChallanDate", strdate);
                sqlCommand.Parameters.AddWithValue("@EntStatusTime", strTime);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserName);
                sqlCommand.Parameters.AddWithValue("@PrintStickerTrue", PrintSticker);
                sqlCommand.Parameters.AddWithValue("@PrintFrom", Possition);
                sqlCommand.Parameters.AddWithValue("@ScreenStatus", ScreenStatus);
                sqlCommand.Parameters.AddWithValue("@UserPin", PinNo);
                sqlCommand.Parameters.AddWithValue("@Flag", 3);
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveChallanAndWorkShopNoteAndPrintStickerDetail(string BID, string Status, string strdate, string strTime, string UserName, bool PrintSticker, int Possition, int ScreenStatus, string PinNo)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "Proc_NewChallanXML", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@CStatus", Status);
                sqlCommand.Parameters.AddWithValue("@ChallanDate", strdate);
                sqlCommand.Parameters.AddWithValue("@EntStatusTime", strTime);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserName);
                sqlCommand.Parameters.AddWithValue("@PrintStickerTrue", PrintSticker);
                sqlCommand.Parameters.AddWithValue("@PrintFrom", Possition);
                sqlCommand.Parameters.AddWithValue("@ScreenStatus", ScreenStatus);
                sqlCommand.Parameters.AddWithValue("@UserPin", PinNo);
                sqlCommand.Parameters.AddWithValue("@Flag", 3);
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeWorkshopChallanStatus(string barCodes, int challanStatus, string ExBID,string UserId)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@Barcode", barCodes);
                sqlCommand.Parameters.AddWithValue("@ChallanStatus", challanStatus);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserId);
                sqlCommand.Parameters.AddWithValue("@BranchId", ExBID);
                sqlCommand.Parameters.AddWithValue("@Flag", 31);
                sqlCommand.CommandTimeout = 120;
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataSet BindWorkshopRightGrid(string BID, int status,string UserId)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@AcceptedByUser", UserId);
            cmd.Parameters.AddWithValue("@Flag", 24);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public bool SaveReceiveFromStoreDetail(string BID, string Status, string strdate, string strTime, string UserName, int ScreenStatus)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "Proc_NewChallanXML", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@CStatus", Status);
                sqlCommand.Parameters.AddWithValue("@ChallanDate", strdate);
                sqlCommand.Parameters.AddWithValue("@EntStatusTime", strTime);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserName);              
                sqlCommand.Parameters.AddWithValue("@ScreenStatus", ScreenStatus);
                sqlCommand.Parameters.AddWithValue("@Flag", 4);
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveSendToStoreDetail(string BID, string Status, string strdate, string strTime, string UserName, bool PrintSticker, int Possition, int ScreenStatus, string WorkShopNote)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "Proc_NewChallanXML", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@CStatus", Status);
                sqlCommand.Parameters.AddWithValue("@ChallanDate", strdate);
                sqlCommand.Parameters.AddWithValue("@EntStatusTime", strTime);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@AcceptedByUser", UserName);
                sqlCommand.Parameters.AddWithValue("@PrintStickerTrue", PrintSticker);
                sqlCommand.Parameters.AddWithValue("@PrintFrom", Possition);
                sqlCommand.Parameters.AddWithValue("@ScreenStatus", ScreenStatus);
                sqlCommand.Parameters.AddWithValue("@workshopnote", WorkShopNote);
                sqlCommand.Parameters.AddWithValue("@Flag", 5);
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeChallanStatusForPrintStickerData(string barCodes, int challanStatus, string BID)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@Barcode", barCodes);
                sqlCommand.Parameters.AddWithValue("@ChallanStatus", challanStatus);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@Flag", 32);
                sqlCommand.CommandTimeout = 120;
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataSet BindRightGridForSticker(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);            
            cmd.Parameters.AddWithValue("@Flag", 25);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public string SaveInStickerTableData(string BID, int printFrom)
        {
            string res = string.Empty;
            try
            {               
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "proc_NewChallanXML";
                cmd.CommandType = CommandType.StoredProcedure;              
                cmd.Parameters.AddWithValue("@BranchID", BID);
                cmd.Parameters.AddWithValue("@printFrom", printFrom);               
                cmd.Parameters.AddWithValue("@Flag", 7);
                cmd.Parameters.AddWithValue("@PrintStickerTrue", true);
                res = PrjClass.ExecuteNonQuery(cmd);              
            }
            catch (Exception)
            { 
            
            }

            return res;
        }
        public bool ChangeChallanStatusForPrintStickerWSData(string barCodes, int challanStatus, string BID)
        {
            try
            {
                var sqlCommand = new SqlCommand { CommandText = "sp_ChallanInProc", CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.AddWithValue("@Barcode", barCodes);
                sqlCommand.Parameters.AddWithValue("@ChallanStatus", challanStatus);
                sqlCommand.Parameters.AddWithValue("@BranchId", BID);
                sqlCommand.Parameters.AddWithValue("@Flag", 33);
                sqlCommand.CommandTimeout = 120;
                if (PrjClass.ExecuteNonQuery(sqlCommand) == "Record Saved")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataSet BindRightGridForWSSticker(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_dry_NewChallan";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 26);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public string SaveInWorkShopStickerTableData(string BID, int printFrom)
        {
            string res = string.Empty;
            try
            {               
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "proc_NewChallanXML";
                cmd.CommandType = CommandType.StoredProcedure;              
                cmd.Parameters.AddWithValue("@BranchID", BID);
                cmd.Parameters.AddWithValue("@printFrom", printFrom);               
                cmd.Parameters.AddWithValue("@Flag", 8);
                cmd.Parameters.AddWithValue("@PrintStickerTrue", true);
                res = PrjClass.ExecuteNonQuery(cmd);              
            }
            catch (Exception)
            { 
            
            }

            return res;
        }
        public DataSet GetBookingNoAndPrefix(string BookingNo)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CheckBookingPrefix_New";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Bookingnumber", BookingNo);          
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindGridWorkshopMenuRight(DTO.Common Ob)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_Dry_BarcodeMaster";
            CMD.Parameters.AddWithValue("@BranchId", Ob.BID);
            CMD.Parameters.AddWithValue("@UserTypeId", Ob.Id);
            CMD.Parameters.AddWithValue("@Flag", 42);            

            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public void MakeHistoryRecord(string BookingNo, string itemSN, string BID, string Reason, string Garment, int Counter)
        {
            string oldData = string.Empty, currBarcode = string.Empty;
            int index = -1;

            if (Counter == 0)
            {
                dtBooking.Columns.Add("BookingNo");
                dtBooking.Columns.Add("ItemAndBarcode");
                dtBooking.Columns.Add("ReturnCause");
            }
            var RecordCount = dtBooking.Select("[BookingNo] ='" + BookingNo.Trim() + "'").Count();
            if (RecordCount > 0)
            {
                index = dtBooking.AsEnumerable().Select(row => row.Field<string>("BookingNo") == BookingNo).ToList().FindIndex(col => col);
                oldData = dtBooking.Rows[index]["ItemAndBarcode"].ToString();
                currBarcode = "," + Garment+" : " + "*" + BookingNo + "-" + itemSN + "-" + BID + "*";
                dtBooking.Rows[index]["ItemAndBarcode"] = oldData + currBarcode;
            }
            else
            {
                dtBooking.Rows.Add(BookingNo, Garment +" : "+ "*" + BookingNo + "-" + itemSN + "-" + BID + "*", Reason);
            }
            dtBooking.AcceptChanges();
        }
        public void SaveHistoryData(string BID, string UID , string ScreenName)
        {
            string strMsg = string.Empty;
            string strScreenMsg = string.Empty;
            int ScreenID = 0;
            if (ScreenName == "1")
            {
                strScreenMsg = "Workshop Note Creation";
                ScreenID = 2;
            }
            else if (ScreenName == "2")
            {
                strScreenMsg = "Garments received at Store";
                ScreenID = 5;
            }
            else if (ScreenName == "3")
            {
                strScreenMsg = "Garments Marked ready for delivery";
                ScreenID = 6;
            }
            if (dtBooking.Rows.Count > 0)
            {
                for (int i = 0; i < dtBooking.Rows.Count; i++)
                {
                    strMsg = "Garments returned unprocessed : " + dtBooking.Rows[i]["ItemAndBarcode"].ToString() + "  Reason : " + dtBooking.Rows[i]["ReturnCause"].ToString() + ".";
                    DALFactory.Instance.DAL_New_Bookings.InvoiceEventHistorySaveData(dtBooking.Rows[i]["BookingNo"].ToString(), UID, BID, strMsg, strScreenMsg, ScreenID);
                }
            }          
        }       
    }
}