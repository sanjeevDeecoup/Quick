using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DAL
{
    public class Dal_ProcessMaster
    {
        public string SaveProcessMaster(DTO.ProcessMaster Obj)
        {
            string res = "";
            try
            {
                if (CheckDuplicateProcess(Obj) == true)
                {
                    res = "Service code already exists kindly enter new service code";
                    return res;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_sp_ProcessMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                    cmd.Parameters.AddWithValue("@ProcessName", Obj.ProcessName);
                    cmd.Parameters.AddWithValue("@ProcessUsedForVendorReport", Obj.UsedForvendorReport);
                    cmd.Parameters.AddWithValue("@Discount", Obj.Discount);
                    cmd.Parameters.AddWithValue("@ServiceTax", Obj.ServiceTax);
                    cmd.Parameters.AddWithValue("@IsActiveServiceTax", Obj.IsActiveServiceTax);
                    cmd.Parameters.AddWithValue("@IsDiscount", Obj.IsDiscount);
                    cmd.Parameters.AddWithValue("@IsChallan", Obj.IsChallanApplicable);
                    cmd.Parameters.AddWithValue("@ImagePath", Obj.ImagePath);
                    cmd.Parameters.AddWithValue("@CssTax", Obj.CssTax);
                    cmd.Parameters.AddWithValue("@EcesjTax", Obj.EcesjTax);
                    cmd.Parameters.AddWithValue("@IsChallanByPass", Obj.IsReady);
                    cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                    cmd.Parameters.AddWithValue("@Flag", 1);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception) { res = ""; }
            return res;
        }

        public string UpdateProcessMaster(DTO.ProcessMaster Obj)
        {
            string res = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                cmd.Parameters.AddWithValue("@ProcessName", Obj.ProcessName);
                cmd.Parameters.AddWithValue("@ProcessUsedForVendorReport", Obj.UsedForvendorReport);
                cmd.Parameters.AddWithValue("@Discount", Obj.Discount);
                cmd.Parameters.AddWithValue("@ServiceTax", Obj.ServiceTax);
                cmd.Parameters.AddWithValue("@IsActiveServiceTax", Obj.IsActiveServiceTax);
                cmd.Parameters.AddWithValue("@IsDiscount", Obj.IsDiscount);
                cmd.Parameters.AddWithValue("@IsChallan", Obj.IsChallanApplicable);
                cmd.Parameters.AddWithValue("@ImagePath", Obj.ImagePath);
                cmd.Parameters.AddWithValue("@CssTax", Obj.CssTax);
                cmd.Parameters.AddWithValue("@EcesjTax", Obj.EcesjTax);
                cmd.Parameters.AddWithValue("@IsChallanByPass", Obj.IsReady);
                cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                cmd.Parameters.AddWithValue("@Flag", 2);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            catch (Exception) { res = ""; }
            return res;
        }

        public string DeleteProcessMaster(DTO.ProcessMaster Obj)
        {
            string res = string.Empty;
            int TotalItems = 0;
            try
            {
                TotalItems = CountTotalBooking(Obj);
                if (TotalItems > 0)
                {
                    res = "Selected service is in use hence can't be deleted.";
                    return res;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_sp_ProcessMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                    cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                    cmd.Parameters.AddWithValue("@Flag", 3);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception) { res = ""; }
            return res;
        }

        public DataSet BindProcessMaster(DTO.ProcessMaster Obj)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                cmd.Parameters.AddWithValue("@Flag", 4);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet SearchProcessMaster(DTO.ProcessMaster Obj)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessName", Obj.ProcessName);
                cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                cmd.Parameters.AddWithValue("@Flag", 5);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        private int CountTotalBooking(DTO.ProcessMaster Obj)
        {
            SqlDataReader sdr = null; 
            SqlCommand cmd = new SqlCommand();
            int TotalItems = 0;
            try
            {               
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                cmd.Parameters.AddWithValue("@Flag", 6);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                    TotalItems = int.Parse("0" + sdr.GetValue(0));
                else
                    TotalItems = 0;
            }
            catch (Exception) { TotalItems = 0; }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return TotalItems;
        }

        public ArrayList BindDataList(string mapPath)
        {
            DirectoryInfo dir = new DirectoryInfo(mapPath);
            FileInfo[] files = dir.GetFiles();
            ArrayList listItems = new ArrayList();
            foreach (FileInfo info in files)
            {
                listItems.Add(info);
            }
            return listItems;
        }

        public bool CheckDuplicateProcess(DTO.ProcessMaster Obj)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", Obj.ProcessCode);
                cmd.Parameters.AddWithValue("@BranchId", Obj.BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
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

        public bool CheckCorrectItem(string BID, string ItemName)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", ItemName);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 50);
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

        public string GetItemId(string BID, string ItemName, string ItemCode)
        {
            SqlDataReader sdr = null;
            string ItemId = string.Empty;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", ItemName);
                cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 56);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    ItemId = sdr.GetValue(0).ToString();
                }
                else
                {
                    ItemId = "";
                }
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Close();
                sdr.Dispose();
                cmd.Dispose();
            }
            return ItemId;
        }

        public DataSet getItem(string itemid, string BID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_Sel_RecForItemIdUpdate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", itemid);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public string FindDisActive(string BID)
        {
            string resultPrc = string.Empty;
            string resultApply = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            string result = string.Empty;
            try
            {
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 8);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    resultPrc += sdr.GetString(0).ToUpperInvariant() + ":";
                    resultApply += sdr.GetBoolean(1).ToString().ToUpperInvariant() + ":";
                }
                resultPrc = resultPrc.Substring(0, resultPrc.Length - 1);
                resultApply = resultApply.Substring(0, resultApply.Length - 1);
                result = resultPrc + "_" + resultApply;
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
            return result;
        }

        public string FindTaxActive(string BID)
        {
            string resultPrc = string.Empty, result = string.Empty;
            string resultApply = string.Empty;
            string resultRate = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 9);
                sdr = PrjClass.ExecuteReader(cmd);
                while (sdr.Read())
                {
                    resultPrc += sdr.GetString(0).ToUpperInvariant() + ":";
                    resultApply += sdr.GetValue(1).ToString().ToUpperInvariant() + ":";
                    resultRate += sdr.GetValue(2).ToString().ToUpperInvariant() + ":";
                }
                resultPrc = resultPrc.Substring(0, resultPrc.Length - 1);
                resultApply = resultApply.Substring(0, resultApply.Length - 1);
                resultRate = resultRate.Substring(0, resultRate.Length - 1);
                result = resultPrc + "_" + resultApply + "_" + resultRate;

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
            return result;
        }

        public DataSet BindToConfig(string BID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 10);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet ShowCashDetails(string BID, string strStartDate, string strToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Sp_CashBookReport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@FromDate", strStartDate);
                cmd.Parameters.AddWithValue("@ToDate", strToDate);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet ShowDetailCashBook(string BID, string strStartDate, string strToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_DetailCashReport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@FromDate", strStartDate);
                cmd.Parameters.AddWithValue("@ToDate", strToDate);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet ShowBillDetail(string BID, string strStartDate, string strToDate)
        {
            return ShowBillDetail(BID, strStartDate, strToDate, string.Empty);
        }

        public DataSet ShowBillDetail(string BID, string strStartDate, string strToDate, string challanNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Sp_BillDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@FDate", strStartDate);
                cmd.Parameters.AddWithValue("@UDate", strToDate);
                if (string.IsNullOrEmpty(challanNumber))
                {
                    cmd.Parameters.AddWithValue("@Flag", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Flag", 3);
                    cmd.Parameters.AddWithValue("@ChallanNumber", challanNumber);
                }
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet ShowBillDetailwithCustomer(string BID, string strStartDate, string strToDate, string CustCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Sp_BillDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@FDate", strStartDate);
                cmd.Parameters.AddWithValue("@UDate", strToDate);
                cmd.Parameters.AddWithValue("@CustCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 2);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet ShowPackageQty(string BID, string AID, string CustCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_PackageQtyDetail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@AssignId", AID);
                cmd.Parameters.AddWithValue("@CustCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 1);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet FlatPackageQty(string BID, string AID, string CustCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_PackageQtyDetail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@AssignId", AID);
                cmd.Parameters.AddWithValue("@CustCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 2);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet GetAllProcesses(string branchId)
        {
            try
            {
                var sqlCommand = new SqlCommand
                {
                    CommandText = "sp_sp_ProcessMaster",
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Flag", 11);
                return PrjClass.GetData(sqlCommand);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataSet DiscountPackageCheck(string BID, string AID, string CustCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@PackageId", AID);
                cmd.Parameters.AddWithValue("@CustomerCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 71);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }

        public DataSet ValuBenifitPackageDtl(string BID, string AID, string CustCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_PackageQtyDetail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@AssignId", AID);
                cmd.Parameters.AddWithValue("@CustCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 3);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }
        public DataSet DiscountPackageDtl(string BID, string AID, string CustCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_PackageQtyDetail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@AssignId", AID);
                cmd.Parameters.AddWithValue("@CustCode", CustCode);
                cmd.Parameters.AddWithValue("@Flag", 4);
                ds = PrjClass.GetData(cmd);
            }
            catch (Exception) { }
            return ds;
        }
    }
}