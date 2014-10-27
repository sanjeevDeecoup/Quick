using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace DAL
{
    public class DAL_New_Bookings
    {
        ArrayList date = new ArrayList();
        public DataSet BindPriority(string BID)
        {
            DataSet DS_Priority = new DataSet();
            SqlCommand CMD_Priority = new SqlCommand();
            CMD_Priority.CommandText = "sp_NewBooking";
            CMD_Priority.CommandType = CommandType.StoredProcedure;
            CMD_Priority.Parameters.AddWithValue("@BranchId", BID);
            CMD_Priority.Parameters.AddWithValue("@Flag", 1);
            DS_Priority = PrjClass.GetData(CMD_Priority);
            return DS_Priority;
        }

        public DataSet BindPriorityCustom(string BID, string txtPriority)
        {
            DataSet DS_Priority = new DataSet();
            SqlCommand CMD_Priority = new SqlCommand();
            CMD_Priority.CommandText = "sp_NewBooking";
            CMD_Priority.CommandType = CommandType.StoredProcedure;
            CMD_Priority.Parameters.AddWithValue("@BranchId", BID);
            CMD_Priority.Parameters.AddWithValue("@Flag", 44);
            CMD_Priority.Parameters.AddWithValue("@Priority", txtPriority);
            DS_Priority = PrjClass.GetData(CMD_Priority);
            return DS_Priority;
        }

        public double RetrunRate(double rate1, double rate2)
        {
            double rate = 0;
            rate = rate1 + ((rate1 * rate2) / 100);
            return rate;
        }

        public string SavePriority(DTO.NewBooking Obj)
        {
            string res = "";
            try
            {
                SqlCommand CMD_Priority = new SqlCommand();
                CMD_Priority.CommandText = "sp_Priority";
                CMD_Priority.CommandType = CommandType.StoredProcedure;
                CMD_Priority.Parameters.AddWithValue("@Priority", Obj.AddPriority);
                CMD_Priority.Parameters.AddWithValue("@BranchId", Obj.BID);
                CMD_Priority.Parameters.AddWithValue("@Flag", 8);
                res = PrjClass.ExecuteScalar(CMD_Priority);
            }
            catch (Exception) { res = ""; }
            return res;
        }

        public bool CheckEditBookingRights(string Userid, string BID)
        {
            bool res = false;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 17);
                cmd.Parameters.AddWithValue("@UserTypeId", Userid);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = sdr.GetBoolean(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        public DataSet SaveCustomer(DTO.NewBooking Obj)
        {
            DataSet DS_CustInfo = new DataSet();
            try
            {
                SqlCommand sqlCommand = new SqlCommand
                                        {
                                            CommandText = "sp_NewBooking",
                                            CommandType = CommandType.StoredProcedure
                                        };
                sqlCommand.Parameters.AddWithValue("@CustPriority", Obj.Priority);
                sqlCommand.Parameters.AddWithValue("@CustAddress", Obj.CustAddress);
                sqlCommand.Parameters.AddWithValue("@CustAreaLocation", Obj.CustAreaAndLocation);
                sqlCommand.Parameters.AddWithValue("@CustMobile", Obj.CustMobile);
                sqlCommand.Parameters.AddWithValue("@CustName", Obj.CustName);
                sqlCommand.Parameters.AddWithValue("@CustRemarks", Obj.CustRemarks);
                sqlCommand.Parameters.AddWithValue("@CustTitle", Obj.CustTitle);
                sqlCommand.Parameters.AddWithValue("@BDate", Obj.BDate);
                sqlCommand.Parameters.AddWithValue("@ADate", Obj.ADate);
                sqlCommand.Parameters.AddWithValue("@DefDiscount", Obj.Discount);
                // As this is the customer saved from booking screen, Obj.Remarks is actually the communication means
                sqlCommand.Parameters.AddWithValue("@Remarks", Obj.Remarks);
                // Again, Obj.SubItem1 is the customer's email
                sqlCommand.Parameters.AddWithValue("@RateListId", Obj.RateListId);
                sqlCommand.Parameters.AddWithValue("@SubItem1", Obj.SubItem1);
                sqlCommand.Parameters.AddWithValue("@BranchId", Obj.BID);
                sqlCommand.Parameters.AddWithValue("@Flag", 3);
                DS_CustInfo = PrjClass.GetData(sqlCommand);
            }
            catch (Exception) { }
            return DS_CustInfo;
        }

        public DataSet FillCustomer(string Obj, string BID)
        {
            DataSet DS_CustInfo = new DataSet();
            try
            {
                SqlCommand CMD_Priority = new SqlCommand();
                CMD_Priority.CommandText = "sp_NewBooking";
                CMD_Priority.CommandType = CommandType.StoredProcedure;
                CMD_Priority.Parameters.AddWithValue("@CustCode", Obj);
                CMD_Priority.Parameters.AddWithValue("@BranchId", BID);
                CMD_Priority.Parameters.AddWithValue("@Flag", 4);
                DS_CustInfo = PrjClass.GetData(CMD_Priority);
            }
            catch (Exception) { }
            return DS_CustInfo;
        }

        public DataSet FillHeaderInfo(string BID)
        {
            DataSet DS_Header = new DataSet();
            try
            {
                SqlCommand CMD_Priority = new SqlCommand();
                CMD_Priority.CommandText = "sp_NewBooking";
                CMD_Priority.CommandType = CommandType.StoredProcedure;
                CMD_Priority.Parameters.AddWithValue("@BranchId", BID);
                CMD_Priority.Parameters.AddWithValue("@Flag", 6);
                DS_Header = PrjClass.GetData(CMD_Priority);
            }
            catch (Exception) { }
            return DS_Header;
        }

        public DataSet BindCheckedBy(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand DBCommand = new SqlCommand();
            DBCommand.CommandType = CommandType.StoredProcedure;
            DBCommand.CommandText = "sp_Dry_EmployeeMaster";
            DBCommand.Parameters.AddWithValue("@BranchId", BID);
            DBCommand.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(DBCommand);
            return ds;
        }

        public DataTable BindGridView(ArrayList GridItems, string Flag, string BID)
        {
            string ItemName = "", ProcessName = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds = FirstTimeDefaultData(BID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                ProcessName = ds.Tables[0].Rows[0]["ProcessName"].ToString();
            }
            DataRow NewRow = dt.NewRow();
            for (int r = 0; r < GridItems.Count; r++)
            {
                if (r == 0)
                    dt.Columns.Add(GridItems[r].ToString(), typeof(System.Int32));
                else
                    dt.Columns.Add(GridItems[r].ToString());
            }
            if (Flag == "1")
            {
                NewRow[GridItems[0].ToString()] = 1;
                NewRow[GridItems[1].ToString()] = "1";
                NewRow[GridItems[2].ToString()] = ItemName;
                NewRow[GridItems[5].ToString()] = ProcessName;
                NewRow[GridItems[4].ToString()] = "0";
                dt.Rows.Add(NewRow);
                dt.AcceptChanges();
            }
            if (Flag == "2")
            {
                NewRow[GridItems[0].ToString()] = 1;
                NewRow[GridItems[1].ToString()] = "";
                dt.Rows.Add(NewRow);
                dt.AcceptChanges();
            }
            return dt;
        }

        public DataSet FirstTimeDefaultData(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string SetCustSearchCriteria()
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 5);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        public string GetDefaultColor()
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 19);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        public DataSet SetCustSearchGrid(DTO.NewBooking Obj)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerName", Obj.CustName);
            cmd.Parameters.AddWithValue("@CustAddress", Obj.CustAddress);
            cmd.Parameters.AddWithValue("@CustMobile", Obj.CustMobile);
            cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string SaveItemMaster(DTO.NewBooking Obj, ListBox lst)
        {
            string res = string.Empty;
            string sql = string.Empty;
            try
            {
                SqlCommand CMD_Item = new SqlCommand();
                CMD_Item.CommandType = CommandType.Text;
                CMD_Item.CommandText = "Insert Into ItemMaster (ItemName, NumberOfSubItems,ItemCode,BranchId) Values('" + Obj.ItemName + "','" + Obj.SubItems + "','" + Obj.ItemCode + "','" + Obj.BID + "')";
                res = PrjClass.ExecuteNonQuery(CMD_Item);
                CMD_Item.CommandText = "DELETE FROM EntSubItemDetails WHERE ItemName = '" + Obj.ItemName + "' AND BranchId='" + Obj.BID + "'";
                res = PrjClass.ExecuteNonQuery(CMD_Item);
                if (Obj.SubItems == "1")
                {
                    CMD_Item.CommandText = "INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder,BranchId) VALUES ('" + Obj.ItemName + "','" + Obj.ItemName + "','1','" + Obj.BID + "')";
                    res = PrjClass.ExecuteNonQuery(CMD_Item);
                }

                if (Convert.ToInt32(Obj.SubItems) > 1)
                {
                    for (int i = 0; i < lst.Items.Count; i++)
                    {
                        int k = i + 1;
                        CMD_Item.CommandText = "insert into EntSubItemDetails(ItemName,SubItemName,SubItemOrder,BranchId) Values('" + Obj.ItemName + "','" + lst.Items[i].ToString().Trim() + "','" + k + "','" + Obj.BID + "')";
                        res = PrjClass.ExecuteNonQuery(CMD_Item);
                    }
                }

                ////CMD_Item.CommandText = "sp_NewBooking";
                //CMD_Item.Parameters.AddWithValue("@ItemName", Obj.ItemName);
                //CMD_Item.Parameters.AddWithValue("@NumberOfSubItems", Obj.SubItems);
                //CMD_Item.Parameters.AddWithValue("@ItemCode", Obj.ItemCode);
                //CMD_Item.Parameters.AddWithValue("@SubItem1", Obj.SubItem1);
                //CMD_Item.Parameters.AddWithValue("@SubItem2", Obj.SubItem2);
                //CMD_Item.Parameters.AddWithValue("@SubItem3", Obj.SubItem3);
                //CMD_Item.Parameters.AddWithValue("@Flag", 9);
                //res = PrjClass.ExecuteNonQuery(CMD_Item);
            }
            catch (Exception)
            {
                res = "";
            }
            return res;
        }

        private bool CheckItemCodeDuplicate(DTO.NewBooking Obj)
        {
            bool status = false;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                cmd.Parameters.AddWithValue("@Flag", 13);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status = true;
                else
                    status = false;
            }
            catch (Exception)
            { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }
        public void SaveRemarks(string Remarks, string BID)
        {
            try
            {
                string res = string.Empty, text = string.Empty;
                string[] array = Remarks.Split(',');
                ListBox lstSubItem = new ListBox();
                lstSubItem.DataSource = array;
                lstSubItem.DataBind();
                int inc = 0;
                for (int i = 1; i <= lstSubItem.Items.Count; i++)
                {
                    text = lstSubItem.Items[inc].Text;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_NewBooking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Remarks", text);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 14);
                    res = PrjClass.ExecuteNonQuery(cmd);
                    inc++;
                    if (inc == lstSubItem.Items.Count)
                        break;
                }
            }
            catch (Exception)
            { }
        }

        public void SaveColors(string Colors, string BID)
        {
            try
            {
                string res = string.Empty, text = string.Empty;
                string[] array = Colors.Split(',');
                ListBox lstSubItem = new ListBox();
                lstSubItem.DataSource = array;
                lstSubItem.DataBind();
                int inc = 0;
                for (int i = 1; i <= lstSubItem.Items.Count; i++)
                {
                    text = lstSubItem.Items[inc].Text;
                    if (text.Contains('/'))
                    {
                        inc++;
                        continue;
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_NewBooking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ColorName", text);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 52);
                    res = PrjClass.ExecuteNonQuery(cmd);
                    inc++;
                    if (inc == lstSubItem.Items.Count)
                        break;
                }
            }
            catch (Exception)
            { }
        }

        public void SaveColor(string ColorName)
        {
            string res = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ColorName", ColorName);
                cmd.Parameters.AddWithValue("@Flag", 15);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            catch (Exception)
            { }
        }

        public int GetCustomerDiscount(string CustCode)
        {
            SqlCommand cmd = new SqlCommand();
            int discount = 0;
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 16);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())

                    discount = Convert.ToInt32(sdr.GetValue(0));
                else
                    discount = 0;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return discount;
        }

        public double GetItemRateAccordingProcess(string ItemName, string Process, string BID)
        {
            double rate = 0;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", ItemName);
                cmd.Parameters.AddWithValue("@ProcessName", Process);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 17);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())

                    rate = Convert.ToDouble(sdr.GetValue(0));
                else
                    rate = 0;
            }
            catch (Exception ex)
            { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return rate;
        }

        public double GridTotal(GridView grdSender, string LabelName)
        {
            double Total = 0;
            for (int r = 0; r < grdSender.Rows.Count; r++)
            {
                Total += double.Parse("0" + ((Label)grdSender.Rows[r].FindControl(LabelName)).Text);
            }
            return Total;
        }

        public string GetLastBookinNumber(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            string lastBooking = "0";
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);                
                cmd.Parameters.AddWithValue("@Flag", 18);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    lastBooking = "" + sdr.GetValue(0);
            }
            catch (Exception ex)
            { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return lastBooking;
        }

        public double getServiceTaxAccordingProcess(string Process, double Amt, string BID)
        {
            double serviceTax = 0, STAmt = 0;
            if (CheckActiveServiceTax(Process, BID) == true)
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader sdr = null;
                try
                {
                    cmd.CommandText = "sp_Dry_BarcodeMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Process", Process);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 16);
                    sdr = PrjClass.ExecuteReader(cmd);
                    if (sdr.Read())
                    {
                        serviceTax = Convert.ToDouble(sdr.GetValue(0));
                        STAmt = Convert.ToDouble((Amt * serviceTax) / 100);
                        serviceTax = STAmt;
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
            return serviceTax;
        }

        public string getAllServiceTaxAccordingProcess(double amt, double rate, double rate1, double rate2)
        {
            double stMain = 0;
            stMain = ((amt * rate) / 100);
            double stCess = 0;
            stCess = ((stMain * rate1) / 100);
            double stLast = 0;
            stLast = ((stMain * rate2) / 100);
            double stTotal = stMain + stCess + stLast;
            return stMain.ToString() + ":" + stCess.ToString() + ":" + stLast.ToString() + ":" + stTotal.ToString();
            //double serviceTax = 0, STAmt = 0;
            //SqlCommand cmd = new SqlCommand();
            //SqlDataReader sdr = null;
            //cmd.CommandText = "sp_Dry_BarcodeMaster";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Process", Process);
            //cmd.Parameters.AddWithValue("@BranchId", BID);
            //cmd.Parameters.AddWithValue("@Flag", 16);
            //sdr = PrjClass.ExecuteReader(cmd);
            //if (sdr.Read())
            //{
            //    serviceTax = Convert.ToDouble(sdr.GetValue(0));
            //    STAmt = Convert.ToDouble((Amt * serviceTax) / 100);
            //    serviceTax = STAmt;
            //}
        }

        //public string getAllServiceTaxAccordingProcess(double amt, double rate, double rate1, double rate2)
        //{
        //    double stMain = 0;
        //    stMain = ((amt * rate) / 100);
        //    double stCess = 0;
        //    stCess = ((stMain * rate1) / 100);
        //    double stLast = 0;
        //    stLast = ((stMain * rate2) / 100);
        //    double stTotal = stMain + stCess + stLast;
        //    return stMain.ToString() + ":" + stCess.ToString() + ":" + stLast.ToString() + ":" + stTotal.ToString();
        //}
        public double getServiceTaxAccordingProcessWhenAfterCondition(string Process, double Amt, string BID, double disAmt)
        {
            double serviceTax = 0, STAmt = 0, disc = 0, Total = 0;
            if (CheckActiveServiceTax(Process, BID) == true)
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader sdr = null;
                try
                {
                    cmd.CommandText = "sp_Dry_BarcodeMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Process", Process);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 16);
                    sdr = PrjClass.ExecuteReader(cmd);
                    if (sdr.Read())
                    {
                        serviceTax = Convert.ToDouble(sdr.GetValue(0));
                        if (disAmt != 0)
                        {
                            disc = (Amt * disAmt) / 100;
                            Total = Amt - disc;
                        }
                        else
                        {
                            Total = Amt;
                        }
                        STAmt = Convert.ToDouble((Total * serviceTax) / 100);
                        serviceTax = STAmt;
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
            return serviceTax;
        }

        private bool CheckActiveServiceTax(string Process, string BID)
        {
            bool status = false;
            string temp = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_Dry_BarcodeMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Process", Process);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 15);
                ds = PrjClass.GetData(cmd);
                temp = ds.Tables[0].Rows[0]["IsActiveServiceTax"].ToString();
                if (temp != "False")
                    status = true;
                else
                    status = false;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public double CalculatAllTax(DataTable dt)
        {
            double AllTax = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AllTax += Convert.ToDouble(dt.Rows[i]["STPAmt"].ToString());
                AllTax += Convert.ToDouble(dt.Rows[i]["STEP1Amt"].ToString());
                AllTax += Convert.ToDouble(dt.Rows[i]["STEP2Amt"].ToString());
            }
            return AllTax;
        }

        private string ReturnDueDate(DTO.NewBooking Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "prcTask";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DueDate", Ob.BookingDeliveryDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            ds = PrjClass.GetData(cmd);
            res = ds.Tables[0].Rows[0]["Date"].ToString();
            return res;
        }

        public DataSet SaveBooking(DTO.NewBooking Ob)
        {
            DataSet ds = new DataSet();
            string DueDate = string.Empty;
            DueDate = ReturnDueDate(Ob);
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BID);
            SqlCommand CMD_Booking = new SqlCommand();
            CMD_Booking.CommandText = "sp_NewBooking_SaveProc";
            CMD_Booking.CommandType = CommandType.StoredProcedure;
            CMD_Booking.Parameters.AddWithValue("@BookingByCustomer", Ob.BookingByCustomer);
            CMD_Booking.Parameters.AddWithValue("@BookingAcceptedByUserId", Ob.BookingAcceptedByUserId);
            CMD_Booking.Parameters.AddWithValue("@IsUrgent", Ob.IsUrgent);
            CMD_Booking.Parameters.AddWithValue("@BookingDate", Ob.BookingDate);
            CMD_Booking.Parameters.AddWithValue("@BookingDeliveryDate", DueDate);
            CMD_Booking.Parameters.AddWithValue("@BookingDeliveryTime", Ob.BookingDeliveryTime);
            CMD_Booking.Parameters.AddWithValue("@TotalCost", Ob.TotalCost);
            CMD_Booking.Parameters.AddWithValue("@Discount", Ob.Discount);
            CMD_Booking.Parameters.AddWithValue("@NetAmount", Ob.NetAmount);
            CMD_Booking.Parameters.AddWithValue("@BookingStatus", Convert.ToInt32(GStatus.Booking));
            CMD_Booking.Parameters.AddWithValue("@BookingCancelReason", Ob.BookingCancelReason);
            CMD_Booking.Parameters.AddWithValue("@BookingRemarks", Ob.BookingRemarks);
            CMD_Booking.Parameters.AddWithValue("@ItemTotalQuantity", Ob.ItemTotalQuantity);
            CMD_Booking.Parameters.AddWithValue("@HomeDelivery", Ob.HomeDelivery);
            CMD_Booking.Parameters.AddWithValue("@CheckedByEmployee", Ob.CheckedByEmployee);
            CMD_Booking.Parameters.AddWithValue("@DiscountAmt", Ob.DiscountAmt);
            CMD_Booking.Parameters.AddWithValue("@DiscountOption", Ob.DiscountOption);
            CMD_Booking.Parameters.AddWithValue("@BranchId", Ob.BID);
            CMD_Booking.Parameters.AddWithValue("@IsNextTodayUrgent", Ob.TodaNext);
            CMD_Booking.Parameters.AddWithValue("@TaxableAmt", Ob.TaxableAmt);
            CMD_Booking.Parameters.AddWithValue("@TaxType", Ob.TaxType);
            CMD_Booking.Parameters.AddWithValue("@UserId", Ob.UserBookingId);
            CMD_Booking.Parameters.AddWithValue("@WorkshopNote", Ob.WorkshopNote);
            CMD_Booking.Parameters.AddWithValue("@AssignId", Ob.AssignId);
            CMD_Booking.Parameters.AddWithValue("@RateListId", Ob.RateListId);
            CMD_Booking.Parameters.AddWithValue("@BookingTime", date[1].ToString());           
            if (Ob.AssignId == "0" || Ob.isDiscountPackge)
            {
                CMD_Booking.Parameters.AddWithValue("@IsPackage", "False");
            }
            else
            {
                CMD_Booking.Parameters.AddWithValue("@IsPackage", "True");
            }
            CMD_Booking.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(CMD_Booking);
            return ds;
        }

        public string SaveAccountEntries(DTO.NewBooking Ob)
        {
            string res = string.Empty;
            try
            {
                SqlCommand CMD_Priority = new SqlCommand();
                CMD_Priority.CommandText = "sp_NewBooking_SaveProc";
                CMD_Priority.CommandType = CommandType.StoredProcedure;
                CMD_Priority.Parameters.AddWithValue("@TotalCost", (Convert.ToDouble(Ob.TotalCost) + Convert.ToDouble(Ob.STTax)).ToString());
                CMD_Priority.Parameters.AddWithValue("@CustomerCode", Ob.BookingByCustomer);
                CMD_Priority.Parameters.AddWithValue("@AcceptByUser", Ob.BookingAcceptedByUserId);
                CMD_Priority.Parameters.AddWithValue("@PaymentMode", Ob.PaymentMode);
                CMD_Priority.Parameters.AddWithValue("@BOOKINGNUMBER", Ob.BOOKINGNUMBER);
                ArrayList date = new ArrayList();
                date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BID);
                CMD_Priority.Parameters.AddWithValue("@DateTime", date[0].ToString());
                CMD_Priority.Parameters.AddWithValue("@Time", date[1].ToString());
                CMD_Priority.Parameters.AddWithValue("@BranchId", Ob.BID);
                CMD_Priority.Parameters.AddWithValue("@AssignId", Ob.AssignId);                
                if (Ob.PaymentMode.ToString() == "Package")
                {
                    CMD_Priority.Parameters.AddWithValue("@Flag", 10);
                    CMD_Priority.Parameters.AddWithValue("@AdvanceAmt", Ob.NetAmount);
                }
                else
                {
                    CMD_Priority.Parameters.AddWithValue("@Flag", 3);
                    CMD_Priority.Parameters.AddWithValue("@AdvanceAmt", Ob.AdvanceAmt);
                }
                res = PrjClass.ExecuteNonQuery(CMD_Priority);
            }
            catch (Exception ex)
            { res = ex.ToString(); }
            return res;
        }

        public string SaveBookingDetails(DTO.NewBooking Ob)
        {
            string res = string.Empty;
            SqlCommand CMD_BookingDetails = new SqlCommand();
            CMD_BookingDetails.CommandText = "sp_NewBooking_SaveProc";
            CMD_BookingDetails.CommandType = CommandType.StoredProcedure;
            CMD_BookingDetails.Parameters.AddWithValue("@ISN", Ob.ISN);

            CMD_BookingDetails.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemTotalQuantity", Ob.ItemTotalQuantity);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemProcessType", Ob.ItemProcessType);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemQuantityAndRate", Ob.ItemQuantityAndRate);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessType1", Ob.ItemExtraProcessType1);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessRate1", Ob.ItemExtraProcessRate1);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessType2", Ob.ItemExtraProcessType2);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemExtraProcessRate2", Ob.ItemExtraProcessRate2);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemSubTotal", Ob.ItemSubTotal);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemStatus", Convert.ToInt32(GStatus.Booking));
            CMD_BookingDetails.Parameters.AddWithValue("@ItemRemark", Ob.ItemRemark);
            CMD_BookingDetails.Parameters.AddWithValue("@ItemColor", Ob.Color);
            // ******************* //
            // added code //
            CMD_BookingDetails.Parameters.AddWithValue("@STPAmt", Ob.STPAmt);
            CMD_BookingDetails.Parameters.AddWithValue("@STPAmtEcess", Ob.STPAmtEcess);
            CMD_BookingDetails.Parameters.AddWithValue("@STPAmtSHECess", Ob.STPAmtSHECess);
            // added code //
            CMD_BookingDetails.Parameters.AddWithValue("@STEP1Amt", Ob.STEP1Amt);
            CMD_BookingDetails.Parameters.AddWithValue("@STP1AmtEcess", Ob.STP1AmtEcess);
            CMD_BookingDetails.Parameters.AddWithValue("@STPAmt1SHECess", Ob.STPAmt1SHECess);
            // added code //
            CMD_BookingDetails.Parameters.AddWithValue("@STEP2Amt", Ob.STEP2Amt);
            CMD_BookingDetails.Parameters.AddWithValue("@STP2AmtEcess", Ob.STP2AmtEcess);
            CMD_BookingDetails.Parameters.AddWithValue("@STPAmt2SHECess", Ob.STPAmt2SHECess);
            // **************** //
            CMD_BookingDetails.Parameters.AddWithValue("@UnitDesc", Ob.UnitDesc);
            CMD_BookingDetails.Parameters.AddWithValue("@BOOKINGNUMBER", Ob.BOOKINGNUMBER);            
            CMD_BookingDetails.Parameters.AddWithValue("@BranchId", Ob.BID);
            CMD_BookingDetails.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(CMD_BookingDetails);
            return res;
        }

        public string SaveBarCode(DTO.NewBooking Ob)
        {
            string res = string.Empty;
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BID);
            SqlCommand CMD_Barcode = new SqlCommand();
            CMD_Barcode.CommandTimeout = 0;
            CMD_Barcode.CommandText = "sp_InsertIntoBarcodeTable";
            CMD_Barcode.CommandType = CommandType.StoredProcedure;
            CMD_Barcode.Parameters.AddWithValue("@BOOKINGNUMBER", Ob.BOOKINGNUMBER);
            CMD_Barcode.Parameters.AddWithValue("@BranchId", Ob.BID);
            CMD_Barcode.Parameters.AddWithValue("@AcceptedByUser", Ob.UserBookingId);
            CMD_Barcode.Parameters.AddWithValue("@EntStatusTime", date[1].ToString());
            CMD_Barcode.Parameters.AddWithValue("@StatusDate", date[0].ToString());
            res = PrjClass.ExecuteNonQuery(CMD_Barcode);
            return res;
        }

        public string ReadColorCode(string colorName)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking_SaveProc";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@ColorName", colorName);
                CMD.Parameters.AddWithValue("@Flag", 4);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string ReadColorName(string colorCode)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking_SaveProc";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@ColorCode", colorCode);
                CMD.Parameters.AddWithValue("@Flag", 5);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }


        public DataSet ReadDataForEdit(string bookingNumber, string BID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandText = "sp_EditRecord";
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.Parameters.AddWithValue("@BOOKINGNUMBER", bookingNumber);          
            CMD.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet EditBooking(DTO.NewBooking Ob)
        {
            string DueDate = string.Empty;
            DataSet ds = new DataSet();
            DueDate = ReturnDueDate(Ob);
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BID);
            SqlCommand CMD_Booking = new SqlCommand();
            CMD_Booking.CommandText = "sp_EditBooking_SaveProc";
            CMD_Booking.CommandType = CommandType.StoredProcedure;
            CMD_Booking.Parameters.AddWithValue("@BookingByCustomer", Ob.BookingByCustomer);
            CMD_Booking.Parameters.AddWithValue("@BookingAcceptedByUserId", Ob.BookingAcceptedByUserId);
            CMD_Booking.Parameters.AddWithValue("@BOOKINGNUMBER", Ob.BOOKINGNUMBER);
            CMD_Booking.Parameters.AddWithValue("@IsUrgent", Ob.IsUrgent);
            CMD_Booking.Parameters.AddWithValue("@BookingDeliveryDate", DueDate);
            CMD_Booking.Parameters.AddWithValue("@BookingDeliveryTime", Ob.BookingDeliveryTime);
            CMD_Booking.Parameters.AddWithValue("@TotalCost", Ob.TotalCost);
            CMD_Booking.Parameters.AddWithValue("@Discount", Ob.Discount);
            CMD_Booking.Parameters.AddWithValue("@NetAmount", Ob.NetAmount);
            CMD_Booking.Parameters.AddWithValue("@BookingRemarks", Ob.BookingRemarks);
            CMD_Booking.Parameters.AddWithValue("@ItemTotalQuantity", Ob.ItemTotalQuantity);
            CMD_Booking.Parameters.AddWithValue("@HomeDelivery", Ob.HomeDelivery);
            CMD_Booking.Parameters.AddWithValue("@CheckedByEmployee", Ob.CheckedByEmployee);
            CMD_Booking.Parameters.AddWithValue("@DiscountAmt", Ob.DiscountAmt);
            CMD_Booking.Parameters.AddWithValue("@DiscountOption", Ob.DiscountOption);
            CMD_Booking.Parameters.AddWithValue("@BranchId", Ob.BID);
            CMD_Booking.Parameters.AddWithValue("@TaxableAmt", Ob.TaxableAmt);
            CMD_Booking.Parameters.AddWithValue("@TaxType", Ob.TaxType);
            CMD_Booking.Parameters.AddWithValue("@UserId", Ob.UserBookingId);
            CMD_Booking.Parameters.AddWithValue("@WorkshopNote", Ob.WorkshopNote);
            CMD_Booking.Parameters.AddWithValue("@AssignId", Ob.AssignId);
            CMD_Booking.Parameters.AddWithValue("@RateListId", Ob.RateListId);
            CMD_Booking.Parameters.AddWithValue("@BookingTime", date[1].ToString());            
            if (Ob.AssignId == "0" || Ob.isDiscountPackge)
            {
                CMD_Booking.Parameters.AddWithValue("@IsPackage", "False");
            }
            else
            {
                CMD_Booking.Parameters.AddWithValue("@IsPackage", "True");
            }
            ds = PrjClass.GetData(CMD_Booking);
            return ds;
        }

        public enum CheckError
        {
            Item,
            Process,
            Process1,
            Process2,
            Rate,
            Rate1,
            Rate2,
            Discount_Amount,
            Advance_Amount
        }

        public string CheckGridEntries(string ItemName, string ProcessName, string ExtP1, string ExtP2, string rate, string rate1, string rate2, string amt, string advance, string BID)
        {
            string res = "Done";
            if (CheckItemName(ItemName, BID) != true)
            {
                res = CheckError.Item.ToString();
                return res;
            }
            if (CheckProcessName(ProcessName, BID) != true)
            {
                res = CheckError.Process.ToString();
                return res;
            }
            if (ExtP1 != "")
            {
                if (CheckProcessName(ExtP1, BID) != true)
                {
                    res = CheckError.Process1.ToString();
                    return res;
                }
            }
            if (ExtP2 != "")
            {
                if (CheckProcessName(ExtP2, BID) != true)
                {
                    res = CheckError.Process2.ToString();
                    return res;
                }
            }
            if (rate != "")
            {
                double check = 0;
                try
                {
                    check = Convert.ToDouble(rate);
                }
                catch (Exception)
                {
                    res = CheckError.Rate.ToString();
                    return res;
                }
            }
            if (rate1 != "")
            {
                double check = 0;
                try
                {
                    check = Convert.ToDouble(rate1);
                }
                catch (Exception)
                {
                    res = CheckError.Rate1.ToString();
                    return res;
                }
            }
            if (rate2 != "")
            {
                double check = 0;
                try
                {
                    check = Convert.ToDouble(rate2);
                }
                catch (Exception)
                {
                    res = CheckError.Rate2.ToString();
                    return res;
                }
            }
            if (amt != "")
            {
                double check = 0;
                try
                {
                    check = Convert.ToDouble(amt);
                }
                catch (Exception)
                {
                    res = CheckError.Discount_Amount.ToString();
                    return res;
                }
            }
            if (advance != "")
            {
                double check = 0;
                try
                {
                    check = Convert.ToDouble(advance);
                }
                catch (Exception)
                {
                    res = CheckError.Advance_Amount.ToString();
                    return res;
                }
            }
            return res;
        }

        private bool CheckItemName(string ItemName, string BID)
        {
            bool status = false;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_NewBooking_SaveProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", ItemName);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 6);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status = true;
                else
                    status = false;
            }
            catch (Exception)
            {
                status = false;
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }

        private bool CheckProcessName(string ProcessName, string BID)
        {
            bool status = false;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            string temp = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_NewBooking_SaveProc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", ProcessName);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status = true;
                else
                    status = false;
            }
            catch (Exception)
            {
                status = false;
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }

        public string CheckItemCode(string ItemCode, string ItemName, string BID)
        {
            string status = string.Empty;
            if (CheckItemName(ItemName, BID) == true)
            {
                status = "Item name already exist";
                return status;
            }
            else
            {
                status = "done";
            }
            if (ItemCode != "")
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader sdr = null;
                try
                {
                    cmd.CommandText = "sp_Dry_DrawlMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 22);
                    sdr = PrjClass.ExecuteReader(cmd);
                    if (sdr.Read())
                    {
                        status = "Item code already exist";
                        return status;
                    }
                    else
                    { status = "done"; }
                }
                catch (Exception ex) { }
                finally
                {
                    sdr.Close();
                    sdr.Dispose();
                    cmd.Dispose();
                }
            }
            else
            { status = "done"; }
            return status;
        }

        public string SaveProcessMaster(DTO.NewBooking Obj)
        {
            string res = "";
            try
            {
                if (Obj.ProcessCode != "")
                {
                    if (Obj.ProcessName != "")
                    {
                        if (CheckItemCodeDuplicate(Obj) != true)
                        {
                            SqlCommand CMD_ProcessMaster = new SqlCommand();
                            CMD_ProcessMaster.CommandText = "sp_NewBooking";
                            CMD_ProcessMaster.CommandType = CommandType.StoredProcedure;
                            CMD_ProcessMaster.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                            CMD_ProcessMaster.Parameters.AddWithValue("@ProcessName", Obj.ProcessName);
                            CMD_ProcessMaster.Parameters.AddWithValue("@BranchId", Obj.BID);
                            CMD_ProcessMaster.Parameters.AddWithValue("@Flag", 12);
                            res = PrjClass.ExecuteNonQuery(CMD_ProcessMaster);
                        }
                        else
                            res = "Process code allready exist.";
                    }
                    else
                        res = "Please enter process name.";
                }
                else
                    res = "Please enter process code.";
            }
            catch (Exception)
            {
                res = "";
            }
            return res;
        }

        public DataSet GetSMSInformation(string BookingNo, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 28);
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string GetItemNameWhenDataSave(string BID)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 23);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public bool CheckInvoiceNo(string InvoiceNo, string BID, string BookingPrefix)
        {
            bool status = false;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingNo", InvoiceNo);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@BookingPrefix", BookingPrefix);
                cmd.Parameters.AddWithValue("@Flag", 24);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    status = true;
                else
                    status = false;
            }
            catch (Exception)
            {
                status = false;
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return status;
        }
        public double GetDiscountAmountForDeliveryForm(double Amt, double Discount)
        {
            double NetAmt = 0.0;
            try
            {
                if (Discount != 0.0)
                {
                    NetAmt = Convert.ToDouble((Amt * Discount) / 100);
                }
            }
            catch (Exception) { NetAmt = 0.0; }
            return NetAmt;
        }

        public bool CheckDiscountApplicationOnProces(string ProcessName, string BID)
        {
            bool res = false;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {

                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 25);
                cmd.Parameters.AddWithValue("@ProcessCode", ProcessName);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    res = sdr.GetBoolean(0);
            }
            catch (Exception)
            {
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        public string DisplayNetAmountFlatOrFolat(string BID)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 27);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string DisplayAmountFormat(string BID)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 26);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string GetNoOfClothesReceived(string itemName, string ISN, string bookingNo, string BranchId)
        {
            string res = string.Empty, remove = string.Empty, Reason = string.Empty, remove1 = string.Empty, Reason1 = string.Empty, Date = string.Empty;
            int i = 0, j = 0;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BookingItemName ", itemName);
                CMD.Parameters.AddWithValue("@BarcodeISN", ISN);
                CMD.Parameters.AddWithValue("@BookingNo", bookingNo);
                CMD.Parameters.AddWithValue("@BranchId", BranchId);
                CMD.Parameters.AddWithValue("@Flag", 28);
                sdr = PrjClass.ExecuteReader(CMD);
                while (sdr.Read())
                {
                    res = "" + sdr.GetValue(0);
                    if (Date == "")
                    {
                        Date = "" + sdr.GetValue(4);
                    }
                    if (res == "True")
                        i++;
                    j++;
                    if ("" + sdr.GetValue(2) == "INWARD")
                    {
                        remove += "," + sdr.GetValue(3);
                        Reason = " " + sdr.GetValue(1);
                    }
                    else if ("" + sdr.GetValue(2) == "OUTWARD")
                    {
                        remove1 += "," + sdr.GetValue(3);
                        Reason1 = " " + sdr.GetValue(1);
                    }
                }
                if (remove != "")
                {
                    res = i.ToString() + "/" + j.ToString() + "/" + remove + " [ " + Reason + " ]" + " /" + Date;
                    if (remove1 != "")
                        res += " " + remove1 + "[" + Reason1 + "]";
                }
                else if (remove1 != "")
                {
                    res = i.ToString() + "/" + j.ToString() + "/" + remove1 + " [ " + Reason1 + " ]" + " /" + Date;
                }
                else
                    res = i.ToString() + "/" + j.ToString() + "/" + " /" + Date;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string GetNoOfClothesReceivedForComarping(string itemName, string BranchId)
        {
            string res = "//";
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@ItemName", itemName);
                CMD.Parameters.AddWithValue("@BranchId", BranchId);
                CMD.Parameters.AddWithValue("@Flag", 29);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                {
                    res = "0/" + sdr.GetValue(0).ToString() + "/";
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string CountNoOfSubItem(string ItemName, string BID)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@ItemName", ItemName);
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 29);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string FindTotalTaxActive(string BID)
        {
            string res = string.Empty;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 30);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    res = "" + sdr.GetValue(0);
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return res;
        }

        public string BindRemarksInUI(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Remarks";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", "3");
            ds = PrjClass.GetData(cmd);
            string strTmp = string.Empty;
            ds = PrjClass.GetData(cmd);
            if (ds == null || ds.Tables.Count == 0)
                return strTmp;

            if (ds.Tables[0].Rows.Count == 1)
                return ds.Tables[0].Rows[0][1].ToString();

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                strTmp += ds.Tables[0].Rows[i][1].ToString() + ",";
            }
            strTmp = strTmp.Substring(0, strTmp.Length - 1);
            return strTmp;
        }

        public string BindColorsInUI(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Colors";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", "3");
            ds = PrjClass.GetData(cmd);
            string strTmp = string.Empty;
            ds = PrjClass.GetData(cmd);
            if (ds == null || ds.Tables.Count == 0)
                return strTmp;

            if (ds.Tables[0].Rows.Count == 1)
                return ds.Tables[0].Rows[0][1].ToString();

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                strTmp += ds.Tables[0].Rows[i][1].ToString() + ",";
            }
            strTmp = strTmp.Substring(0, strTmp.Length - 1);
            return strTmp;
        }

        public string BindColorsInUINew(string BID)
        {
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", "53");
            ds = PrjClass.GetData(cmd);
            string strTmp = string.Empty;
            ds = PrjClass.GetData(cmd);
            if (ds == null || ds.Tables.Count == 0)
                return strTmp;

            if (ds.Tables[0].Rows.Count == 1)
                return ds.Tables[0].Rows[0][1].ToString();

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                strTmp += ds.Tables[0].Rows[i][1].ToString() + ",";
            }
            strTmp = strTmp.Substring(0, strTmp.Length - 1);
            return strTmp;
        }

        public double GetNextDayRate(string BID, string Flag)
        {
            double NetAmt = 0.0;
            SqlCommand CMD = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {

                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", Flag);
                sdr = PrjClass.ExecuteReader(CMD);
                if (sdr.Read())
                    NetAmt = Convert.ToDouble(sdr.GetValue(0));
            }
            catch (Exception) { NetAmt = 0.0; }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                CMD.Dispose();
            }
            return NetAmt;
        }

        public string GetNextDayRateAndDayOffset(string BID, string Flag)
        {
            // double NetAmt = 0.0;
            try
            {
                SqlCommand CMD = new SqlCommand();
                CMD.CommandText = "sp_NewBooking";
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", Flag);
                return PrjClass.ExecuteScalar(CMD);
            }
            catch (Exception) { return ""; }
            // return NetAmt;
        }

        public string FindDefaultPrinter(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 39);
            var res = PrjClass.ExecuteScalar(cmd);
            return res;
        }

        public bool CheckIfCustomerExists(string CustName, string BranchID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BranchID);

            if (CustName.IndexOf(':') > -1)
            {
                cmd.Parameters.AddWithValue("@CustName", CustName.Split(':')[0]);
                switch (CustName.Split(':')[1])
                {
                    case "All":
                        cmd.Parameters.AddWithValue("@Flag", 40);
                        break;

                    case "Name":
                        cmd.Parameters.AddWithValue("@Flag", 40);
                        break;

                    case "Address":
                        cmd.Parameters.AddWithValue("@Flag", 70);
                        break;

                    case "Mobile":
                        cmd.Parameters.AddWithValue("@Flag", 71);
                        break;

                    case "Email":
                        cmd.Parameters.AddWithValue("@Flag", 72);
                        break;

                    case "MembershipId":
                        cmd.Parameters.AddWithValue("@Flag", 74);
                        break;

                    case "CustCode":
                        cmd.Parameters.AddWithValue("@Flag", 75);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@Flag", 40);
                cmd.Parameters.AddWithValue("@CustName", CustName);
            }
            sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
                return true;
            }
            else
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
                return false;
            }
        }

        public bool CheckIfCustomerExists(string CustName, string Addr, string BranchID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BranchID);
                cmd.Parameters.AddWithValue("@Flag", 41);
                cmd.Parameters.AddWithValue("@CustName", CustName);
                cmd.Parameters.AddWithValue("@CustAddress", Addr);
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

        public string LoadAllItems(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            var allItems = string.Empty;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 42);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    allItems += sdr.GetString(0).ToUpperInvariant() + ":";
                }
                allItems = allItems.Substring(0, allItems.Length - 1);
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return allItems;
        }

        public string LoadAllTax(string BID)
        {
            var allItax = string.Empty;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 45);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    allItax += sdr.GetValue(0).ToString().ToUpperInvariant() + ":";
                    allItax += sdr.GetValue(1).ToString().ToUpperInvariant() + ":";
                    allItax += sdr.GetValue(2).ToString().ToUpperInvariant() + ":";
                }
                allItax = allItax.Substring(0, allItax.Length - 1);
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return allItax;
        }

        public string LoadInclusiveExclusive(string BID)
        {
            var Inclusive = string.Empty;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 46);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    Inclusive += sdr.GetString(0).ToUpperInvariant() + ":";
                }
                Inclusive = Inclusive.Substring(0, Inclusive.Length - 1);
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return Inclusive;
        }

        public string LoadAllProcesses(string BID)
        {
            var allPrc = string.Empty;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 43);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    allPrc += sdr.GetString(0).ToUpperInvariant() + ":";
                }
                allPrc = allPrc.Substring(0, allPrc.Length - 1);
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return allPrc;
        }

        public bool BackDateBookingAvailable(string UserID, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "SELECT RightToView FROM dbo.EntMenuRights WHERE (PageTitle = '" + SpecialAccessRightName.BackDateBooking+ "') AND (UserTypeId = '" + UserID + "') AND (BranchId = '" + BID + "') ";
                cmd.CommandType = CommandType.Text;
                sdr = PrjClass.ExecuteReader(cmd);
                string statue = string.Empty;
                if (sdr.Read())
                    statue = "" + sdr.GetValue(0);
                if (statue == "True")
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

        public string checkIfDesAndColorEnabled(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_newbooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@Flag", 47);
            return PrjClass.ExecuteScalar(cmd);
        }

        public string checkDescAndColorForBinding(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_newbooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@Flag", 49);
            return PrjClass.ExecuteScalar(cmd);
        }

        public string ChangeDescAndColorBinding(string BID, bool bindDesc, bool bindColor)
        {
            int _bindDescVal, _bindColorVal;
            if (bindDesc == true)
            {
                _bindDescVal = 1;
            }
            else
            {
                _bindDescVal = 0;
            }
            if (bindColor == true)
            {
                _bindColorVal = 1;
            }
            else
            {
                _bindColorVal = 0;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_newbooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@Flag", 51);
            cmd.Parameters.AddWithValue("@BindDesc", _bindDescVal);
            cmd.Parameters.AddWithValue("@BindColor", _bindColorVal);
            return PrjClass.ExecuteNonQuery(cmd);
        }

        public bool SetQtySpaceOrOne(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "SELECT SetQty FROM dbo.MstConfigSettings WHERE (BranchId = '" + BID + "') ";
                cmd.CommandType = CommandType.Text;
                sdr = PrjClass.ExecuteReader(cmd);
                string statue = string.Empty;
                if (sdr.Read())
                    statue = "" + sdr.GetValue(0);
                if (statue == "1")
                    status= false;
                else
                    status= true;
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

        public string FindDefaultDiscountType(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT DefaultDiscountType FROM dbo.MstConfigSettings WHERE (BranchId = '" + BID + "') ";
            cmd.CommandType = CommandType.Text;
            var sr = PrjClass.ExecuteScalar(cmd);
            return sr;
        }

        public bool ConfirmDelivery(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            bool status = false;
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "sp_newbooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchID", BID);
                cmd.Parameters.AddWithValue("@Flag", 54);
                sdr = PrjClass.ExecuteReader(cmd);
                sdr.Read();
                if (sdr.GetBoolean(0))
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

        public bool SetItemRateForProcess(string ItemName, string ProcessName, string argExtraProcess, string argExtraProcess2, double rate, double rate1, double rate2, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_newbooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@Flag", 55);
            cmd.Parameters.AddWithValue("@ItemName", ItemName);
            // call fist time without checking, cause it will obviously have fist process
            cmd.Parameters.AddWithValue("@ProcessCode", ProcessName);
            cmd.Parameters.AddWithValue("@ItemWiseRate", rate);
            var sr = PrjClass.ExecuteNonQuery(cmd);
            if (sr.ToUpperInvariant() != "RECORD SAVED")
                return false;

            // first one done,
            // check if second one is not blank
            if (argExtraProcess == "")
                return true;

            cmd.Parameters["@ProcessCode"].Value = argExtraProcess;
            cmd.Parameters["@ItemWiseRate"].Value = rate1;
            sr = PrjClass.ExecuteNonQuery(cmd);
            if (sr.ToUpperInvariant() != "RECORD SAVED")
                return false;

            // second one done,
            // check if third is blank, return
            if (argExtraProcess2 == "")
                return true;

            cmd.Parameters["@ProcessCode"].Value = argExtraProcess2;
            cmd.Parameters["@ItemWiseRate"].Value = rate2;
            sr = PrjClass.ExecuteNonQuery(cmd);
            if (sr.ToUpperInvariant() != "RECORD SAVED")
                return false;

            // if we are here, then all records are saved
            // return true;
            return true;
        }

        public string CheckLenBredth(string BID, string argItemName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_newbooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchID", BID);
            cmd.Parameters.AddWithValue("@CustomerName", argItemName);
            cmd.Parameters.AddWithValue("@Flag", 57);
            var sr = PrjClass.ExecuteScalar(cmd);
            return sr;
        }

        public bool CheckIfBindColorToQty(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_newbooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchID", BID);
                cmd.Parameters.AddWithValue("@Flag", 58);
                sdr = PrjClass.ExecuteReader(cmd);
                sdr.Read();
                if (sdr.GetBoolean(0))
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

        public string CheckMeansOfCommunication(string customerCode, string BID)
        {
            SqlCommand sqlCommand = new SqlCommand
                        {
                            CommandText = "sp_CustomerMaster",
                            CommandType = System.Data.CommandType.StoredProcedure,
                        };
            sqlCommand.Parameters.AddWithValue("@BranchID", BID);
            sqlCommand.Parameters.AddWithValue("@Flag", 15);
            sqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
            var communicationMeans = PrjClass.ExecuteScalar(sqlCommand);
            if (communicationMeans == string.Empty)
                communicationMeans = "NA";
            return communicationMeans;
        }

        public string CheckDetailsOfPackage(string customerCode, string bookingDate, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            var allPkg = string.Empty;
            SqlDataReader sdr = null;
            if (string.IsNullOrEmpty(bookingDate)==true)
            {
                bookingDate = "1 Jan 1990";
            }
            try
            {
                cmd.CommandText = "sp_PackageAssignToCustomer";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@CustCode", customerCode);
                cmd.Parameters.AddWithValue("@dt", DateTime.Parse(bookingDate));
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    var cnt = 0;
                    while (cnt < sdr.FieldCount)
                    {
                        allPkg += sdr.GetValue(cnt).ToString() + ":";
                        ++cnt;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                if (bookingDate != "undefined")
                {
                    sdr.Close();
                    sdr.Dispose();
                    cmd.Dispose();
                }
            }
            if (string.IsNullOrEmpty(allPkg)) return allPkg;
            return allPkg.Substring(0, allPkg.Length - 1);
        }

        public void UpdateCustomer(string CustId, string mobileNo, string emailId, string commPref, string BID)
        {
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText = "sp_NewBooking_SaveProc",
                CommandType = System.Data.CommandType.StoredProcedure,
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", BID);
            sqlCommand.Parameters.AddWithValue("@Flag", 9);
            sqlCommand.Parameters.AddWithValue("@CustomerCode", CustId);
            sqlCommand.Parameters.AddWithValue("@CommunicationMeans", commPref);
            sqlCommand.Parameters.AddWithValue("@CustomerMobile", mobileNo);
            sqlCommand.Parameters.AddWithValue("@CustomerEmailId", emailId);
            var communicationMeans = PrjClass.ExecuteNonQuery(sqlCommand);
            // return communicationMeans;
        }

        public string LoadThePassWords(string BID)
        {
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText = "sp_NewBooking",
                CommandType = System.Data.CommandType.StoredProcedure,
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", BID);
            sqlCommand.Parameters.AddWithValue("@Flag", 65);
            return PrjClass.ExecuteScalar(sqlCommand);
        }

        public void BackUpBookingDetails(string bookingNumber, string userName, string branchId)
        {
            var sqlCommand = new SqlCommand
                            {
                                CommandText = "Proc_BackUpBooking",
                                CommandType = CommandType.StoredProcedure
                            };
            sqlCommand.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@UserName", userName);          
            sqlCommand.Parameters.AddWithValue("@Flag", 2);
            var res = PrjClass.ExecuteNonQuery(sqlCommand);
            if (res != "Record Saved") throw new ArgumentException(res);
        }

        public void BackUpBookings(string bookingNumber, string userName, string editBookingRemarks, string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "Proc_BackUpBooking",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@UserName", userName);
            sqlCommand.Parameters.AddWithValue("@EditBookingRemarks", editBookingRemarks);            
            sqlCommand.Parameters.AddWithValue("@Flag", 1);
            var res = PrjClass.ExecuteNonQuery(sqlCommand);
            if (res != "Record Saved") throw new ArgumentException(res);
        }

        public string LoadDefaultSearchCriteriaForCustomer(string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_NewBooking",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 66);
            return PrjClass.ExecuteScalar(sqlCommand);
        }

        public bool checkForEditRemarks(string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_NewBooking",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 67);
            return bool.Parse(PrjClass.ExecuteScalar(sqlCommand));
        }

        public string checkForDelDiscountPwd(string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_NewBooking",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 68);
            return PrjClass.ExecuteScalar(sqlCommand);
        }

        public void BackUpPayment(string bookingNumber, string userName, string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "Proc_BackUpBooking",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@UserName", userName);            
            sqlCommand.Parameters.AddWithValue("@Flag", 3);
            var res = PrjClass.ExecuteNonQuery(sqlCommand);
            if (res != "Record Saved") throw new ArgumentException(res);
        }

        public bool IsMobileUnique(string mobileNo, string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_NewBooking",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CustMobile", mobileNo);
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 69);
            if (Int32.Parse(PrjClass.ExecuteScalar(sqlCommand)) == 0)
                return true;
            else
                return false;
        }

        public void BackUpWholeBooking(string bookingNumber, string userName, string editingRemark, string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "Proc_BackUpBooking",
                CommandType = CommandType.StoredProcedure
            };
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(branchId);
            sqlCommand.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@UserName", userName);
            sqlCommand.Parameters.AddWithValue("@EditBookingRemarks", editingRemark);
            sqlCommand.Parameters.AddWithValue("@EditBookingDate", date[0].ToString());   
            sqlCommand.Parameters.AddWithValue("@Flag", 4);
            var res = PrjClass.ExecuteNonQuery(sqlCommand);
            if (res != "Record Saved") throw new ArgumentException(res);
        }
        public DataSet GetBookingPrefix(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmdBookingPre = new SqlCommand();
            cmdBookingPre.CommandType = CommandType.StoredProcedure;
            cmdBookingPre.CommandText = "sp_Dry_Barcodemaster";
            cmdBookingPre.Parameters.AddWithValue("@BranchId", BID);
            cmdBookingPre.Parameters.AddWithValue("@Flag", 41);
            ds = PrjClass.GetData(cmdBookingPre);
            return ds;
        }
        public string InvoiceEventHistorySaveData(string bookingNumber, string userName, string BID, string EventMsg,string ScreenName,int ScreenID)
        {
            string res = string.Empty;
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_NewBooking_SaveProc";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@BOOKINGNUMBER", bookingNumber);
            cmd.Parameters.AddWithValue("@BookingAcceptedByUserId", userName);
            cmd.Parameters.AddWithValue("@BookingDate", date[0].ToString());
            cmd.Parameters.AddWithValue("@BookingTime", date[1].ToString());
            cmd.Parameters.AddWithValue("@EventReason", EventMsg);
            cmd.Parameters.AddWithValue("@ScreenName", ScreenName);
            cmd.Parameters.AddWithValue("@ScreenId", ScreenID);
            cmd.Parameters.AddWithValue("@Flag", 12);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
        public DataSet GetInvoiceBookingData(string BID, string BookingNo)
        {
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_NewBooking_SaveProc";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@BOOKINGNUMBER", BookingNo);
            CMD.Parameters.AddWithValue("@Flag", 13);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public string updateInvoiceHistoryDeleteData(string bookingNumber, string BID)
        {
            string res = string.Empty; 
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EditInvoiceHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@BOOKINGNUMBER", bookingNumber);          
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
        public string InsertInvoiceHistoryData(string BookingNo, string ItemRowIndex,string BID ,string UserName)
        {
            string res = string.Empty;
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EditInvoiceHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@ItemRowIndex", ItemRowIndex);
            cmd.Parameters.AddWithValue("@ActionDate", date[0].ToString());
            cmd.Parameters.AddWithValue("@ActionTime", date[1].ToString());
            cmd.Parameters.AddWithValue("@Flag", 7);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

    }
}