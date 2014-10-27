using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DTO;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_Report
    {
        public DataSet GetDeliveryFrom_To_UptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_ClothDelivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserId", Ob.UserID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataMainReportwithHome(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_QuantityandBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookingNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            if (Ob.Description == "7")
            {
                cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
            }
            if (Ob.Description == "6")
            {
                cmd.Parameters.AddWithValue("@HNumber", Ob.HNumber);
            }
            cmd.Parameters.AddWithValue("@Flag", Ob.Description);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDailyCustomerAdditionReport(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_DailyCustomerAddition";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@uptodate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@Branchid", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", Int32.Parse(Ob.InvoiceNo));
            cmd.Parameters.AddWithValue("@customercodeSupplied", Ob.CustId);
            cmd.Parameters.AddWithValue("@customercodeinlist", Ob.StrCodes);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetInvoiceStatement(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_InvoiceStatement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@uptodate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@Branchid", Ob.BranchId);           
            cmd.Parameters.AddWithValue("@customercodeSupplied", Ob.CustId);
           
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetInvoiceStatementForCustomer(String BranchID, string Fromdate,string  UptoDate,string CustCode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_InvoiceStatementForCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", Fromdate);
            cmd.Parameters.AddWithValue("@uptodate", UptoDate);
            cmd.Parameters.AddWithValue("@Branchid", BranchID);
            cmd.Parameters.AddWithValue("@customercodeSupplied", CustCode);

            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataMainReport(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_QuantityandBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookingNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserID", Ob.UserID);
            //if (Ob.Description == "2")
            //{
                cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
           // }
            cmd.Parameters.AddWithValue("@Flag", Ob.Description);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataMainReportBookingDiscount(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_QuantityandBookingDiscount";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookingNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserID", Ob.UserID);
            //if (Ob.Description == "2")
            //{
                cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
           // }
            cmd.Parameters.AddWithValue("@Flag", Ob.Description);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataMainReportDeliveryDiscount(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_QuantityandDeliveryDiscount";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookingNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserID", Ob.UserID);
            //if (Ob.Description == "2")
            //{
                cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
           // }
            cmd.Parameters.AddWithValue("@Flag", Ob.Description);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public DataSet GetDataMainReportAreaLocation(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_QuantityandBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@AreaLocation", Ob.CustCodeStr);  
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);           
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public DataSet GetCancelDataMainReport(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_CancelQuantityandBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookingNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            if (Ob.Description == "2")
            {
                cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
            }
            cmd.Parameters.AddWithValue("@Flag", Ob.Description);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDataMainReportEditRecord(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_QuantityandBookingEditRecord";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BookingNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            if (Ob.Description == "2")
            {
                cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
            }
            cmd.Parameters.AddWithValue("@Flag", Ob.Description);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetExpenseReport(string strStartDate, string strToDate, string strReportType, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_StockReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", strStartDate);
            cmd.Parameters.AddWithValue("@Enddate", strToDate);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            if (strReportType == "All")
                cmd.Parameters.AddWithValue("@Flag", 3);
            else
            {
                cmd.Parameters.AddWithValue("@Ledgername", strReportType);
                cmd.Parameters.AddWithValue("@Flag", 2);
            }
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindServiceTax(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Report_ServiceTaxNewReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", Ob.InvoiceNo);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindWorkShopChallan(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_WorkShopPackageQtyDetail";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ChallanNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDeliveryByInvoiceNo(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_ClothDelivery_InvoiceNo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@Date", Ob.Date);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserId", Ob.UserID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetSaleFrom_To_UptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_SalesPeriodReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserId", Ob.UserID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetSaleByInvoiceNo(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_SalesPeriodReport_InvoiceNo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@Date", Ob.Date);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserId", Ob.UserID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet BindProcessDropDown(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", 17));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            cmd.Dispose();
            return dsMain;
        }

        public DataSet BinItemDropDown(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", 18));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            cmd.Dispose();
            return dsMain;
        }

        public DataSet GetDeliveryAndSalesFrom_To_UptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_SalesandDelivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@UDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDeliveryAndSalesInvoiceNo(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_SalesandDeliveryByInvoiceNo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@InvoiceNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@Date", Ob.Date);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public bool CheckOrginalUser(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.UserID);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 72);
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

        public DataSet GetDeliveryByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_ClothDeliveryByCustomerId";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserId", Ob.UserID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetSaleFrom_To_UptoDateByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_SalesPeriodReport_ByCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@UserId", Ob.UserID);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetDeliveryAndSalesFrom_To_UptoDateByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_SalesandDeliveryByCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@UDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@CustId", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet ImportRateList(string Path)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Import";
            cmd.Parameters.AddWithValue("@Path", Path);
            cmd.Parameters.AddWithValue("@Flag", 1);
            cmd.CommandType = CommandType.StoredProcedure;
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetSlipDetails(string BookingNo, string BID, string procName)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = procName;
            if (procName == "Sp_Sel_BookingDetailsForReceiptBackUp")
                cmd.Parameters.AddWithValue("@BookingBackUpId", BookingNo);
            else
                cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);
            cmd.Parameters.AddWithValue("@BranchId", BID);          
            cmd.CommandType = CommandType.StoredProcedure;
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public string SaveRateList(DataSet ds, ArrayList PName, string BID)
        {
            string res = "";

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                for (int i = 0; i < PName.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Sp_Import";
                    cmd.Parameters.AddWithValue("@ItemName", ds.Tables[0].Rows[row]["ITEMNAME"].ToString());
                    cmd.Parameters.AddWithValue("@ProcessPrice", ds.Tables[0].Rows[row][PName[i].ToString().Replace(' ', '_')].ToString());
                    cmd.Parameters.AddWithValue("@ProcessName", PName[i].ToString());
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 2);
                    cmd.CommandType = CommandType.StoredProcedure;
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            return res;
        }

        public DataSet GetReasonToRemove(DTO.Report Ob, string Type, string ID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_ReturnChoth";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@UDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@InvoiceNo", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            if (Type == "1" || Type == "2")
            {
                if (ID == "1")
                {
                    if (Ob.CustId == "")
                        cmd.Parameters.AddWithValue("@Flag", "1");
                    else
                        cmd.Parameters.AddWithValue("@Flag", "2");
                }
                if (ID == "2")
                {
                    if (Ob.CustId == "")
                        cmd.Parameters.AddWithValue("@Flag", "4");
                    else
                        cmd.Parameters.AddWithValue("@Flag", "5");
                }
                if (ID == "3")
                {
                    if (Ob.CustId == "")
                        cmd.Parameters.AddWithValue("@Flag", "7");
                    else
                        cmd.Parameters.AddWithValue("@Flag", "8");
                }
            }
            if (Type == "3")
            {
                if (ID == "1")
                    cmd.Parameters.AddWithValue("@Flag", "3");
                if (ID == "2")
                    cmd.Parameters.AddWithValue("@Flag", "6");
                if (ID == "3")
                    cmd.Parameters.AddWithValue("@Flag", "9");
            }
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetMultipleDeliveryByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultipleClothDelivery_ByCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetMultipleDeliveryByFromToUptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultipleClothDelivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetMultiplePaymentByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultiplePaymentCustomerWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetMultiplePaymentByFromToUptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultiplePendingPayment";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetMultipleBothOutPutByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultipleBothCustomerWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public DataSet GetMultipleBothOutPutFromToUptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultipleBothDatetoDate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }

        public string SaveMultiplePendingCloth(GridView grdPendingCloth, string BID, string UserName)
        {
            string res = string.Empty, res1 = string.Empty,strItems = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            for (int iRow = 0; iRow < grdPendingCloth.Rows.Count; iRow++)
            {
                if (((CheckBox)grdPendingCloth.Rows[iRow].Cells[0].FindControl("chkSelect")).Checked)
                {
                    strItems = "";
                    strItems= GetItemNameAndBarcode(((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text,BID);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_SaveDataFromMultiplePaymentAndDeliveryReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingNumber", ((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text);
                    cmd.Parameters.AddWithValue("@DateTime", date[0].ToString());
                    cmd.Parameters.AddWithValue("@AcceptByUser", UserName);
                    cmd.Parameters.AddWithValue("@BookingTime", date[1].ToString());
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 1);
                    res = PrjClass.ExecuteNonQuery(cmd);

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "sp_CheckBookingStatus";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@BookingNumber", ((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text);
                    cmd1.Parameters.AddWithValue("@BranchId", BID);
                    res1 = PrjClass.ExecuteNonQuery(cmd1);
                    if (res == "Record Saved")
                    {
                        MakeEntryForHistory(((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text, BID, UserName, date[0].ToString(), grdPendingCloth.Rows[iRow].Cells[6].Text, strItems);
                    }
        
                }
            }
            return res;
        }

        public string CheckClothStatus(GridView grdPendingCloth)
        {
            string res = string.Empty;

            for (int iRow = 0; iRow < grdPendingCloth.Rows.Count; iRow++)
            {
                int TotalQty = 0, BalQty = 0, DelQty = 0;
                if (((CheckBox)grdPendingCloth.Rows[iRow].Cells[0].FindControl("chkSelect")).Checked)
                {
                    TotalQty = Convert.ToInt32(grdPendingCloth.Rows[iRow].Cells[5].Text);
                    BalQty = Convert.ToInt32(grdPendingCloth.Rows[iRow].Cells[8].Text);
                    DelQty = Convert.ToInt32(grdPendingCloth.Rows[iRow].Cells[7].Text);
                    if (TotalQty != DelQty + BalQty)
                    {
                        if (res == "")
                            res = ((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text;
                        else
                            res += " , " + ((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text;
                    }
                }
            }
            if (res == "")
                res = "Done";
            else
                res = res + " " + "booking no clothes not ready to deliver.";
            return res;
        }

        public string SaveMultiplePendingPayment(GridView grdPendingCloth, string BID, string userName,string PaymentMode,string PaymentDetails)
        {
            string res = string.Empty, res1 = string.Empty;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            for (int iRow = 0; iRow < grdPendingCloth.Rows.Count; iRow++)
            {
                if (((CheckBox)grdPendingCloth.Rows[iRow].Cells[0].FindControl("chkSelect")).Checked)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_SaveDataFromMultiplePaymentAndDeliveryReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingNumber", ((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text);
                    cmd.Parameters.AddWithValue("@DateTime", date[0].ToString());
                    cmd.Parameters.AddWithValue("@TotalCost", grdPendingCloth.Rows[iRow].Cells[11].Text);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@AcceptByUser", userName);
                    cmd.Parameters.AddWithValue("@Time", date[1].ToString());
                    cmd.Parameters.AddWithValue("@PaymentType", PaymentMode);
                    cmd.Parameters.AddWithValue("@PaymentRemarks", PaymentDetails);
                    cmd.Parameters.AddWithValue("@Flag", 2);
                    res = PrjClass.ExecuteNonQuery(cmd);

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "sp_CheckBookingStatus";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@BookingNumber", ((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text);
                    cmd1.Parameters.AddWithValue("@BranchId", BID);
                    res1 = PrjClass.ExecuteNonQuery(cmd1);
                    if ((res == "Record Saved") && (Convert.ToDouble(grdPendingCloth.Rows[iRow].Cells[11].Text) !=0))
                    {
                        MakeEntryForPaymentHistory(((HyperLink)grdPendingCloth.Rows[iRow].Cells[1].FindControl("hypBtnShowDetails")).Text, BID, userName, date[0].ToString(), grdPendingCloth.Rows[iRow].Cells[11].Text, PaymentMode, PaymentDetails);
                    }
                }
            }
            return res;
        }

        public string SaveDeliveryAndPayment(GridView grdPendingCloth, string BID, string userName, string PaymentMode, string PaymentDetails)
        {
            string res = string.Empty;
            res = SaveMultiplePendingCloth(grdPendingCloth, BID, userName);
            //if (res == "Record Saved")
            //{
            res = string.Empty;
            res = SaveMultiplePendingPayment(grdPendingCloth, BID, userName, PaymentMode, PaymentDetails);
            //}
            return res;
        }

        public DataSet GetPendingReceiptParticularCustomer(DTO.Report Ob, string BID)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 12);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetMonthlyStausByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_MonthItemWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookingDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetMonthlyStausByDetailed(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_MonthSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookingDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetMonthlyStausDetailed(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_MonthAllDetail";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookingDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetMonthlStatusSubItem(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_MonthAllSubItem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookingDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetTodayDeliverysummary(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_Delivery";
            cmd.Parameters.Add(new SqlParameter("@BookingDate1", Ob.FromDate));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", Ob.InvoiceNo));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            return dsMain;
        }

        public DataSet GetTodayDeliverydetailed(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_Delivery_Summary";
            cmd.Parameters.Add(new SqlParameter("@BookingDate1", Ob.FromDate));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", Ob.InvoiceNo));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            return dsMain;
        }

        public DataSet GetTodayDeliveryUpdatesummary(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_DeliveryUpdate";
            cmd.Parameters.Add(new SqlParameter("@BookingDate1", Ob.FromDate));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", Ob.InvoiceNo));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            return dsMain;
        }

        public DataSet GetTodayDeliveryUpdatedetailed(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_DeliveryUpdateSummary";
            cmd.Parameters.Add(new SqlParameter("@BookingDate1", Ob.FromDate));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", Ob.InvoiceNo));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            return dsMain;
        }

        public bool BookingDateNotEarlierDeliveryDate(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            bool status = false;
            DataSet ds = new DataSet();
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            cmd.CommandText = "sp_Dry_EmployeeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@DeliveryDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@QueryType", Ob.CustId);
            cmd.Parameters.AddWithValue("@TodayDate", date[0].ToString());
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 15);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Status"].ToString() == "TRUE")
                    status = true;
                else
                    status = false;
            }
            return status;
        }

        public DataSet PrintBarcodeDropDown(DTO.Report Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Dry_BindPrintBarcodeDropDown";
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.BranchId));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            return dsMain;
        }

        public DataSet MatchClothesStockreconcilation(string BranchId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_StockReconcile";
            cmd.Parameters.Add(new SqlParameter("@BranchId", BranchId));
            cmd.Parameters.Add(new SqlParameter("@Flag", 14));
            DataSet dsMain = new DataSet();
            dsMain = PrjClass.GetData(cmd);
            return dsMain;
        }

        public bool CheckDiscountOnBookingNumber(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            bool status = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 30);
            ds = PrjClass.GetData(cmd);
            for (int iRow = 0; iRow < ds.Tables[0].Rows.Count; iRow++)
            {
                if (ds.Tables[0].Rows[iRow]["Discount"].ToString() == "TRUE")
                {
                    status = true;
                    break;
                }
                else
                    status = false;
            }
            return status;
        }

        public DataSet BindItem(DTO.Sub_Items Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindStockReconcile(string Id, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            if (Id != "0")
            {
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@StatusId", Id);
                cmd.Parameters.AddWithValue("@Flag", 15); ;
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 4);
            }
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public bool GetStatusNegativeEntry(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            bool status = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@BookingNumber", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@Flag", 31);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["status"].ToString() == "TRUE")
                    status = true;
                else
                    status = false;
            }
            return status;
        }

        public DataSet GetDataItemReport(DTO.Report Ob, bool bIsAll)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_ItemWiseReportSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", Ob.FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            if (!string.IsNullOrEmpty(Ob.StartBkNum))
                cmd.Parameters.AddWithValue("@StartBkNo", Ob.StartBkNum);
            if (!string.IsNullOrEmpty(Ob.EndDate))
                cmd.Parameters.AddWithValue("@EndBkNo", Ob.EndDate);
            cmd.Parameters.AddWithValue("@ItemName", Ob.CustId);
            if (Ob.CustId == "All" && string.IsNullOrEmpty(Ob.CustCodeStr))
                cmd.Parameters.AddWithValue("@Flag", 2);
            else if (string.IsNullOrEmpty(Ob.CustCodeStr))
                cmd.Parameters.AddWithValue("@Flag", 1);
            else
            { cmd.Parameters.AddWithValue("@Flag", 3); cmd.Parameters.AddWithValue("@ItemNamesStr", Ob.CustCodeStr); }
            ds = PrjClass.GetData(cmd);
            // if (bIsAll)
            // {
            /*
                DataTable dt = ds.Tables[0];
                Ob.QtyTotal = 0;
                Ob.AmountTotal = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ob.QtyTotal += float.Parse(dt.Rows[i][1].ToString());
                    Ob.AmountTotal += float.Parse(dt.Rows[i][2].ToString());
                }
                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "Total";
                    dr[1] = Ob.QtyTotal.ToString();
                    dr[2] = Ob.AmountTotal.ToString();
                    ds.Tables[0].Rows.Add(dr);
                }*/
            // }
            return ds;
        }

        public DataSet GetCustomerWiseSummary(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "dbo.sp_CustomerSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@uptodate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@Branchid", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", Int32.Parse(Ob.InvoiceNo));
            cmd.Parameters.AddWithValue("@customercodeSupplied", Ob.CustId);
            cmd.Parameters.AddWithValue("@customercodeinlist", Ob.StrCodes);
            ds = PrjClass.GetData(cmd);
            //if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return ds;
            /*else
            {
                DataSet dsWithTotal = ComputeTotal(Ob, ds, 2, ds.Tables[0].Columns.Count - 2);
                return dsWithTotal;
            }*/
        }

        public DataSet GetCustomerWiseSummaryPendingSms(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_CustomerWiseAccountRecivableSummary";
            cmd.CommandType = CommandType.StoredProcedure;            
            cmd.Parameters.AddWithValue("@Branchid", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 2);
            cmd.Parameters.AddWithValue("@customercodeSupplied", Ob.CustId);
            cmd.Parameters.AddWithValue("@customercodeinlist", Ob.StrCodes);
            ds = PrjClass.GetData(cmd);           
            return ds;            
        }

        public DataSet GetDurationWiseReport(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_sel_consolidatedBusinessVolumeReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@year", Ob.Year);
            cmd.Parameters.AddWithValue("@month", Ob.Month);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        private DataSet ComputeTotal(DTO.Report Ob, DataSet ds, int startingCell, int uptoCell)
        {
            try
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0 || startingCell == 0)
                    return ds;

                Ob.FltTotals = new List<float>();
                var dict = new Dictionary<int, string>();
                Ob.FltTotalsCounter = 0;
                var floatValue = 0.0f;
                var str = string.Empty;
                for (int j = startingCell; j < uptoCell; j++)
                {
                    Ob.FltTotalsCounter = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (float.TryParse(dt.Rows[i][j].ToString(), out floatValue))
                            Ob.FltTotalsCounter += floatValue;
                        else
                        {
                            dict.Add(j, string.Empty);
                            break;
                        }
                    }
                    Ob.FltTotals.Add((float)Math.Round(Ob.FltTotalsCounter, 2));
                }

                // return ds;

                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[startingCell - 1] = "Total";
                    for (int j = startingCell; j < uptoCell; j++)
                    {
                        if (dict.ContainsKey(j))
                            dr[j] = dict[j];
                        else
                            dr[j] = Math.Round((Ob.FltTotals.ElementAt((j - startingCell))), 2);
                    }
                    ds.Tables[0].Rows.Add(dr);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet GetProcessWiseSummary(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_ServiceWiseReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@StartDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@EndDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@ProcessCodes", Ob.CustCodeStr);
            ds = PrjClass.GetData(cmd);
            return ds;
            /*if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return ds;
            else
            {
                DataSet dsWithTotal = ComputeTotal(Ob, ds, 4, ds.Tables[0].Columns.Count);
                return dsWithTotal;
            }*/
        }

        public DataSet GetServiceAndGarment(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ServiceAndGarment";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@StartDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@EndDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@ProcessCodes", Ob.CustCodeStr);
            cmd.Parameters.AddWithValue("@customercodeSupplied", Ob.CustId);
            ds = PrjClass.GetData(cmd);
            return ds;            
        }

        public string FindTaxLabels(string BID)
        {
            var lbl1 = string.Empty;
            var lbl2 = string.Empty;
            var lbl3 = string.Empty;
            SqlDataReader allLbls = null;
            SqlCommand cmd = new SqlCommand();
            try
            {                
                cmd.CommandText = "sp_sp_ProcessMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 10);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                allLbls = PrjClass.ExecuteReader(cmd);
                if (allLbls.Read())
                {
                    lbl1 = allLbls.GetValue(0).ToString();
                    lbl2 = allLbls.GetValue(1).ToString();
                    lbl3 = allLbls.GetValue(2).ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                allLbls.Close();
                allLbls.Dispose();
                cmd.Dispose();
            }
            // return in the format of lbl1:lbl2:lbl3
            return lbl1 + ":" + lbl2 + ":" + lbl3;
        }

        public DataSet BindUserTypeDropDown(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UserRightsReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindUserRightsReport(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UserRightsReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserTypeCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@fromDate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@toDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            if (Ob.StrCodes == "Booking")
                cmd.Parameters.AddWithValue("@Flag", 2);
            else
                cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public void ChangeStatusAccordingBooking(string BookingNo, string BID)
        {
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "sp_CheckBookingStatus";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@BookingNumber", BookingNo);
            cmd1.Parameters.AddWithValue("@BranchId", BID);
            PrjClass.ExecuteNonQuery(cmd1);
        }

        public string PendingReceiptParticularCustomer(string CustId, string BID)
        {
            var lbl1 = 0f;
            var lbl2 = 0f;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader allLbls = null;
            try
            {
                cmd.CommandText = "sp_BranchMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustCode", CustId);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 10);
                allLbls = PrjClass.ExecuteReader(cmd);
                while (allLbls.Read())
                {
                    lbl1 = lbl1 + allLbls.GetInt32(0);
                    lbl2 = (float)(lbl2 + allLbls.GetDouble(1));
                }
            }
            catch (Exception ex) { }
            finally
            {
                allLbls.Close();
                allLbls.Dispose();
                cmd.Dispose();
            }
            return lbl1.ToString() + ":" + lbl2.ToString();
        }
        public string PendingReceiptParticularCustomerwithBookingnumber(string CustId, string BID,string BookingNumber)
        {
            var lbl1 = 0f;
            var lbl2 = 0f;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader allLbls = null;
            try
            {
                cmd.CommandText = "sp_Dry_PendingClothAndPaymentCustomerWiseBookingWise";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustCode", CustId);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@BNO", BookingNumber);
                allLbls = PrjClass.ExecuteReader(cmd);
                while (allLbls.Read())
                {
                    lbl1 = lbl1 + allLbls.GetInt32(5);
                    lbl2 = (float)(lbl2 + allLbls.GetDouble(8));
                }
            }
            catch (Exception ex) { }
            finally
            {
                allLbls.Close();
                allLbls.Dispose();
                cmd.Dispose();
            }
            return lbl1.ToString() + ":" + lbl2.ToString();
        }

        public string SetBusinessVolumeDefaultView(string defaultView, string BID)
        {
            var sqlCommand = new SqlCommand
                            {
                                CommandText = "UPDATE MstConfigSettings SET rptYearWiseDefView='" + defaultView + "' WHERE BranchId = " + BID
                            };
            return PrjClass.ExecuteNonQuery(sqlCommand).ToString();
        }

        public string GetBusinessVolumeDefaultView(string BID)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "SELECT rptYearWiseDefView FROM MstConfigSettings WHERE BranchId = " + BID
            };
            return PrjClass.ExecuteScalar(sqlCommand).ToString();
        }

        public string findWorkShopRemark(string bookingNumber, string branchId)
        {
            var sqlCommand = new SqlCommand
                                {
                                    CommandText = "sp_Dry_BarcodeMaster",
                                    CommandType = CommandType.StoredProcedure
                                };

            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            if (bookingNumber.Contains("_"))
            {
                sqlCommand.Parameters.AddWithValue("@BookingNo", bookingNumber.Split('_')[0]);
                sqlCommand.Parameters.AddWithValue("@flag", 38);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@BookingNo", bookingNumber);
                sqlCommand.Parameters.AddWithValue("@flag", 37);
            }
            return PrjClass.ExecuteScalar(sqlCommand).ToString();
        }

        public DataSet GetPendingStockReport(DTO.Report Ob, bool bIsAll)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Sel_pendingItemWiseReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@day", Ob.Day);
            cmd.Parameters.AddWithValue("@ItemName", Ob.Description);
            if (!bIsAll)
            {
                if (Ob.CustCodeStr.IndexOf(',') != -1)
                { cmd.Parameters.AddWithValue("@Flag", 3); cmd.Parameters["@ItemName"].Value = Ob.CustCodeStr; }
                else
                { cmd.Parameters.AddWithValue("@Flag", 2); cmd.Parameters["@ItemName"].Value = Ob.CustCodeStr; }
            }
            else if (string.IsNullOrEmpty(Ob.CustCodeStr))
                cmd.Parameters.AddWithValue("@Flag", 1);

            cmd.Parameters.AddWithValue("@cusname", Ob.CustId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Date", Ob.Date);
            cmd.Parameters.AddWithValue("@StartDate", Ob.StartBkNum);
            cmd.Parameters.AddWithValue("@EndDate", Ob.EndBkNum);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet StockPendingInvoice(DTO.Report Ob, bool bIsAll)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "stockpendingbooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@booking", Ob.InvoiceNo);
            if (!string.IsNullOrEmpty(Ob.StartBkNum))
                cmd.Parameters.AddWithValue("@StartBkNo", Ob.StartBkNum);
            if (!string.IsNullOrEmpty(Ob.EndDate))
                cmd.Parameters.AddWithValue("@EndBkNo", Ob.EndDate);
            cmd.Parameters.AddWithValue("@ItemNamesStr", Ob.CustId);
            if (!bIsAll)
            {
                if (Ob.CustCodeStr.IndexOf(',') != -1)
                { cmd.Parameters.AddWithValue("@Flag", 3); cmd.Parameters["@ItemNamesStr"].Value = Ob.CustCodeStr; }
                else
                { cmd.Parameters.AddWithValue("@Flag", 2); cmd.Parameters["@ItemNamesStr"].Value = Ob.CustCodeStr; }
            }
            else // if (string.IsNullOrEmpty(Ob.CustCodeStr))
                cmd.Parameters.AddWithValue("@Flag", 1);

            cmd.Parameters.AddWithValue("@StartDate", Ob.StartBkNum);
            cmd.Parameters.AddWithValue("@EndDate", Ob.EndBkNum);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetItemWiseSummary(Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_ItemWiseDetailedReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@fromdate", Ob.FromDate);
            cmd.Parameters.AddWithValue("@uptodate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@itemname", Ob.StrCodes);
            if (!string.IsNullOrEmpty(Ob.StartBkNum))
                cmd.Parameters.AddWithValue("@StartBkNum", Ob.StartBkNum);
            if (!string.IsNullOrEmpty(Ob.EndBkNum))
                cmd.Parameters.AddWithValue("@EndBkNum", Ob.EndBkNum);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet LoadBookingHistoryForBookingNumber(string bookingNumber, string branchId, string status, string StartDate, string EndDate)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BookingHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            if (status == "Booking")
            {
                cmd.Parameters.AddWithValue("@Flag", 1);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@BookingNo", bookingNumber);
            }
            else if (status == "BookingNo")
            {
                cmd.Parameters.AddWithValue("@Flag", 3);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@BookingNo", bookingNumber);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Flag", 2);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
            }
            return PrjClass.GetData(cmd);
        }

        public DataSet GetPackageReportSummary(Report Obj, string branchId)
        {
            var ds = new DataSet();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "sp_PackageMaster",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Flag", Obj.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            cmd.Parameters.AddWithValue("@CustomerCode", Obj.CustId);
            cmd.Parameters.AddWithValue("@PackageName", Obj.StrCodes);
            return PrjClass.GetData(cmd);
        }

        public DataSet GetPackageReportDetail(Report Ob)
        {
            var ds = new DataSet();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "sp_PackageMaster",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Flag", Ob.InvoiceNo);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@PackageId", Ob.StrCodes);
            ds = PrjClass.GetData(cmd);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return ds;
            else
            {
                DataSet dsWithTotal = ComputeTotal(Ob, ds, 1, ds.Tables[0].Columns.Count);
                return dsWithTotal;
            }
        }

        public bool IsPrinterLaser(string branchId)
        {
            var ds = new DataSet();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "sp_Dry_EmployeeMaster",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Flag", 19);
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            return bool.Parse(PrjClass.ExecuteScalar(cmd).ToString());
        }

        public DataSet GetListOfEditedBookings(string strFromDate, string strToDate, string branchId)
        {
            var ds = new DataSet();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "proc_BookingHistory",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Flag", 2);
            cmd.Parameters.AddWithValue("@branchId", branchId);
            cmd.Parameters.AddWithValue("@StartDate", strFromDate);
            cmd.Parameters.AddWithValue("@EndDate", strToDate);
            return PrjClass.GetData(cmd);
        }

        public DataSet GetStockReconcilationReport(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "StockReconcilation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetCorrectBaroceNo(string Barcode, string branchId)
        {
            var ds = new DataSet();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "sp_StockReconcile",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Flag", 13);
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            cmd.Parameters.AddWithValue("@BarcodeNo", Barcode);
            return PrjClass.GetData(cmd);
        }

        public void AndroidFirst()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_firstAndroid";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 360;

            PrjClass.ExecuteNonQuery(cmd);
        }

        public void AndroidSecond()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DetailAndroid";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 360;
            PrjClass.ExecuteNonQuery(cmd);
        }

        public void AndroidThird()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "BarCodeAndroid";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 360;
            PrjClass.ExecuteNonQuery(cmd);
        }

        public string UpdateConsolidatedStatus(string barCodes, string branchID)
        {           
            SqlCommand cmd = new SqlCommand();
            var res = new StringBuilder();
            SqlDataReader reader = null;
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_ShiftMaster";
                cmd.Parameters.AddWithValue("@Flag", 10);
                cmd.Parameters.AddWithValue("@BranchId", branchID);
                cmd.Parameters.AddWithValue("@SearchText", barCodes);
                reader = PrjClass.ExecuteReader(cmd);
                while (reader.Read())
                {
                    res.Append(reader.GetValue(0).ToString() + ",");
                }
                if (res.Length > 1)
                    res.Remove(res.Length - 1, 1);
            }
            catch (Exception ex) { }
            finally
            {
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
            }
            return res.ToString();
        }

        public void ResetRightGrid(string branchId)
        {
            var ds = new DataSet();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "sp_StockReconcile",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Flag", 18);
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            var reader = PrjClass.ExecuteNonQuery(cmd);
        }

        public DataSet BindStockMatchReconcile(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 19);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindStockMatchNotReconcile(string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_StockReconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 20);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public void ResetAllGrid(string branchId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM StockMatch WHERE BranchId = " + branchId + "";
            PrjClass.ExecuteNonQuery(cmd);
            cmd.CommandText = "DELETE FROM StockNotMatch WHERE BranchId = " + branchId + "";
            PrjClass.ExecuteNonQuery(cmd);
        }

        public double ReturnCustomerPendingBalance(string CustCode, string BID)
        {
            double pending = 0;
            string[] pendingaaray;
            string pendingAmountCustomerWise = "";
            pendingAmountCustomerWise = PendingReceiptParticularCustomer(CustCode, BID);
            pendingaaray = pendingAmountCustomerWise.Split(':');
            pending = Convert.ToDouble(pendingaaray[1]);
            return pending;
        }
        public double ReturnCustomerPendingBalanceBookingWise(string CustCode, string BID,string BookingNo)
        {
            double pending = 0;
            string[] pendingaaray;
            string pendingAmountCustomerWise = "";
            pendingAmountCustomerWise = PendingReceiptParticularCustomerwithBookingnumber(CustCode, BID, BookingNo);
            pendingaaray = pendingAmountCustomerWise.Split(':');
            pending = Convert.ToDouble(pendingaaray[1]);
            return pending;
        }

        public void UpdateStatus(string branchId)
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                CommandText = "sp_UpdateStatusStockReconcile",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.Add("@BranchId", branchId);
            PrjClass.ExecuteNonQuery(sqlCommand);
        }

        public DataSet GetGarmentReadyReport(Report Obj)
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                CommandText = "proc_NewGarmentStatusReport",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@StartDate", Obj.FromDate);
            sqlCommand.Parameters.AddWithValue("@EndDate", Obj.UptoDate);
            if (!string.IsNullOrEmpty(Obj.StartBkNum))
                sqlCommand.Parameters.AddWithValue("@StartBkNum", Obj.StartBkNum);
            if (!string.IsNullOrEmpty(Obj.EndBkNum))
                sqlCommand.Parameters.AddWithValue("@EndBkNum", Obj.EndBkNum);
            sqlCommand.Parameters.AddWithValue("@BranchId", Obj.BranchId);
            sqlCommand.Parameters.AddWithValue("@Flag", Obj.InvoiceNo);
            sqlCommand.Parameters.AddWithValue("@ProcessType", Obj.StrCodes);
            sqlCommand.Parameters.AddWithValue("@HomesDelivery", Obj.CustCodeStr);
            sqlCommand.Parameters.AddWithValue("@Users", Obj.CustId);
            return PrjClass.GetData(sqlCommand);
        }

        public DataSet GetMarkeReadyData(Report Obj)
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                CommandText = "proc_GarmentStatusReport",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@StartDate", Obj.FromDate);
            sqlCommand.Parameters.AddWithValue("@EndDate", Obj.UptoDate);
            if (!string.IsNullOrEmpty(Obj.StartBkNum))
                sqlCommand.Parameters.AddWithValue("@StartBkNum", Obj.StartBkNum);
            if (!string.IsNullOrEmpty(Obj.EndBkNum))
                sqlCommand.Parameters.AddWithValue("@EndBkNum", Obj.EndBkNum);
            sqlCommand.Parameters.AddWithValue("@BranchId", Obj.BranchId);
            sqlCommand.Parameters.AddWithValue("@Flag", Obj.InvoiceNo);
            sqlCommand.Parameters.AddWithValue("@Users", Obj.CustCodeStr);
            sqlCommand.Parameters.AddWithValue("@Items", Obj.StrCodes);
            return PrjClass.GetData(sqlCommand);
        }

        public void SaveItemWiseRateList(string processes, string ItemWithRates, int rateListId, string branchId)
        {
            try
            {
                var doc = new XDocument();
                var root = new XElement("root");
                doc.Add(root);


                var allProcesses = processes.Split(':');
                var allItemsWithRates = ItemWithRates.Split(',');

                // check to verify if processes count is same as allItemsWithRates.split(':') - 1 or not
                if (allProcesses.Length != allItemsWithRates[0].Split(':').Length - 1)
                    return;

                var item = string.Empty;
                var prc = string.Empty;
                var rate = 0.0f;

                // since branch id is same, we can cut the over head but just creating it once
                var branchIdAttrib = new XAttribute("branchId", branchId);

                for (var i = 0; i <= allItemsWithRates.Length - 1; i++)
                {
                    if (string.IsNullOrEmpty(allItemsWithRates[i].Split(':')[0].Trim()))
                        continue;

                    for (var j = 0; j <= allProcesses.Length - 1; j++)
                    {
                        item = allItemsWithRates[i].Split(':')[0];
                        prc = allProcesses[j];
                        rate = float.Parse(allItemsWithRates[i].Split(':')[j + 1]);

                        var itemElement = new XElement("item", new XAttribute("itemName", item), new XAttribute("process", prc), new XAttribute("rate", rate), branchIdAttrib);
                        root.Add(itemElement);
                    }

                }

                var sqlCommand = new SqlCommand()
                {
                    CommandText = "sp_ItemWiseRate",
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
                sqlCommand.Parameters.AddWithValue("@Flag", 3);
                sqlCommand.Parameters.AddWithValue("@sqlParam", doc.ToString());
                sqlCommand.Parameters.AddWithValue("@rateListId", rateListId);
                PrjClass.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception)
            {

            }
        }
        public string PendingOrderParticularCustomer(string CustId, string BID)
        {
            var lbl3 = 0f;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader allLbls = null;
            try
            {
                cmd.CommandText = "sp_BranchMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustCode", CustId);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 13);
                allLbls = PrjClass.ExecuteReader(cmd);
                while (allLbls.Read())
                {
                    lbl3 = (int)(lbl3 + allLbls.GetInt32(0));
                }
            }
            catch (Exception ex) { }
            finally
            {
                allLbls.Close();
                allLbls.Dispose();
                cmd.Dispose();
            }
            return lbl3.ToString();
        }
        public DataSet GetMultipleOutstandingGarmentByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultipleClothDelivery_ByCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public DataSet GetOutstandinGarmentFromToUptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultipleClothDelivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public DataSet GetPendingPaymentFromToUptoDate(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultiplePendingPayment";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            cmd.Dispose();
            return ds;
        }
        public DataSet GetOutstandingPaymentByCustomer(DTO.Report Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_MultiplePaymentCustomerWise";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", Ob.CustId);
            cmd.Parameters.AddWithValue("@Date", Ob.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Ob.UptoDate);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        private void MakeEntryForHistory(string BookingNo, string BID, string UID, string Date, string totalQty, string ItemDetails)
        {
            string strItemDetails = string.Empty;
            Task t2 = Task.Factory.StartNew
            (() => { DALFactory.Instance.DAL_New_Bookings.InvoiceEventHistorySaveData(BookingNo.ToUpper(), UID, BID, "Garment delivered to/picked up by client : " + totalQty + " Garment details : " + ItemDetails + ".", ScreenName.MultipleDel, 11); }
            );
        }
        public string GetItemNameAndBarcode(string BookingNo, string BID)
        {
            string strResult = string.Empty;
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EditInvoiceHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNo);          
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strResult = ds.Tables[0].Rows[0]["ItemNameAndBarcode"].ToString();
            }
            return strResult;
        }

        private void MakeEntryForPaymentHistory(string BookingNo, string BID, string UID, string Date, string TotalCost, string PMode, string PaymentDetails)
        {
            string strMsg = string.Empty;
            if (PaymentDetails == "")
            {
                strMsg = "payment mode : " + PMode;
            }
            else
            {
                strMsg = "payment mode : " + PMode + " payment details : " + PaymentDetails;
            }
            Task t2 = Task.Factory.StartNew
            (() => { DALFactory.Instance.DAL_New_Bookings.InvoiceEventHistorySaveData(BookingNo.ToUpper(), UID, BID, "Payment accepted : " + TotalCost + " and " + strMsg + ".", ScreenName.MultipleDel,11); }
            );
        }
        public DataSet GetInvoiceHistoeyDetails(string bookingNumber, string branchId, string isBookingNo,string ScreenID,string Username)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_EditInvoiceHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            cmd.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            cmd.Parameters.AddWithValue("@ScreenID", Convert.ToInt32(ScreenID));            
            if (isBookingNo == "True")
            {
                cmd.Parameters.AddWithValue("@Flag", 3);
            }           
            else
            {
                cmd.Parameters.AddWithValue("@UserName", Username);
                cmd.Parameters.AddWithValue("@Flag", 6);
            }

            ds = PrjClass.GetData(cmd);
            return ds;
        }
    }

}