using System.Collections;
using System.Data;

namespace BAL
{
    public class Bal_Processmaster
    {
        public string SaveProcessMaster(DTO.ProcessMaster Obj)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.SaveProcessMaster(Obj);
        }

        public string UpdateProcessMaster(DTO.ProcessMaster Obj)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.UpdateProcessMaster(Obj);
        }

        public string DeleteProcessMaster(DTO.ProcessMaster Obj)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.DeleteProcessMaster(Obj);
        }

        public DataSet BindProcessMaster(DTO.ProcessMaster Obj)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.BindProcessMaster(Obj);
        }

        public DataSet SearchProcessMaster(DTO.ProcessMaster Obj)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.SearchProcessMaster(Obj);
        }

        public ArrayList BindDataList(string mapPath)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.BindDataList(mapPath);
        }

        public string FindDisActive(string BID)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.FindDisActive(BID);
        }

        public string FindTaxActive(string BID)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.FindTaxActive(BID);
        }

        public DataSet BindToConfig(string BID)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.BindToConfig(BID);
        }

        public bool CheckCorrectItem(string BID, string ItemName)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.CheckCorrectItem(BID, ItemName);
        }

        public DataSet ShowCashDetails(string BID, string strStartDate, string strToDate)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ShowCashDetails(BID, strStartDate, strToDate);
        }

        public DataSet ShowDetailCashBook(string BID, string strStartDate, string strToDate)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ShowDetailCashBook(BID, strStartDate, strToDate);
        }

        public DataSet ShowBillDetail(string BID, string strStartDate, string strToDate)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ShowBillDetail(BID, strStartDate, strToDate);
        }

        public DataSet ShowBillDetail(string BID, string strStartDate, string strToDate, string challanNumber)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ShowBillDetail(BID, strStartDate, strToDate, challanNumber);
        }

        public DataSet ShowBillDetailwithCustomer(string BID, string strStartDate, string strToDate, string CustCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ShowBillDetailwithCustomer(BID, strStartDate, strToDate, CustCode);
        }

        public string GetItemId(string BID, string ItemName, string ItemCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.GetItemId(BID, ItemName, ItemCode);
        }

        public DataSet getItem(string itemid, string BID)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.getItem(itemid, BID);
        }

        public DataSet ShowPackageQty(string BID, string AID, string CustCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ShowPackageQty(BID, AID, CustCode);
        }

        public DataSet FlatPackageQty(string BID, string AID, string CustCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.FlatPackageQty(BID, AID, CustCode);
        }

        public DataSet GetAllProcess(string branchId)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.GetAllProcesses(branchId);
        }
        public DataSet DiscountPackageCheck(string BID, string AID, string CustCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.DiscountPackageCheck(BID, AID, CustCode);
        }
        public DataSet ValuBenifitPackageDtl(string BID, string AID, string CustCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.ValuBenifitPackageDtl(BID, AID, CustCode);
        }
        public DataSet DiscountPackageDtl(string BID, string AID, string CustCode)
        {
            return DAL.DALFactory.Instance.Dal_ProcessMaster.DiscountPackageDtl(BID, AID, CustCode);
        }
    }
}