using System;
using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BAL_FactoryChallanDetail
    {
        public int SaveFactoryChallanDetail(DTO.FactoryChallanDetail Ob)
        {
            return DAL.DALFactory.Instance.DAL_FactoryChallanDetail.SaveFactoryChallanDetail(Ob);
        }

        public DataSet GetAllFactoryChallanDetail(DTO.FactoryChallanDetail Ob)
        {
            return DAL.DALFactory.Instance.DAL_FactoryChallanDetail.GetAllFactoryChallanDetail(Ob);
        }

        public int DeleteFactoryChallanDetail(DTO.FactoryChallanDetail Ob)
        {
            return DAL.DALFactory.Instance.DAL_FactoryChallanDetail.DeleteFactoryChallanDetail(Ob);
        }

        public void SaveTempRecord(GridView grdNewChallan)
        {
            DAL.DALFactory.Instance.DAL_FactoryChallanDetail.SaveTempRecord(grdNewChallan);
        }

        public string SaveEntryWise(GridView grdChallan)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryWise(grdChallan);
        }

        public DataSet BindTmpGrid(string BID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.BindTmpGrid(BID);
        }

        public void DeleteFactoryGrid()
        {
            DAL.DALFactory.Instance.DAL_NewChallan.DeleteFactoryGrid();
        }

        public DataSet AllBPNoDue(String BID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AllBPNoDue(BID);
        }

        public DataSet AllBPNoDueInvoice(string BID, string Invoice)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AllBPNoDueInvoice(BID, Invoice);
        }

        public DataSet AnyBPWithDue(string BID, string ExBID, string processcode, string DueDate)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AnyBPWithDue(BID, ExBID, processcode, DueDate);
        }

        public DataSet AnyProcessNoDueBranch(string BID, string processcode)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AnyProcessNoDueBranch(BID, processcode);
        }

        public DataSet AnyBranchNoDueProcess(string BID, string ExBID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AnyBranchNoDueProcess(BID, ExBID);
        }

        public DataSet AnyBPNoDue(string BID, string ExBID, string processcode)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AnyBPNoDue(BID, ExBID, processcode);
        }

        public DataSet AllBPWithDue(string ExBID, string DueDate)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AllBPWithDue(ExBID, DueDate);
        }

        public DataSet AllBAnyPWithDue(string ExBID, string ProcessCode, string DueDate)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AllBAnyPWithDue(ExBID, ProcessCode, DueDate);
        }

        public DataSet AnyBAllPWithDue(string BID, string ExBID, string DueDate)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.AnyBAllPWithDue(BID, ExBID, DueDate);
        }

        public DataSet factoryChallanNo(string BID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.factoryChallanNo(BID);
        }

        public string SaveEntryFactoryOut(GridView grdTemp, string BID, string challanNo, string ExternalBID)
        {
            return DAL.DALFactory.Instance.DAL_NewChallan.SaveEntryFactoryOut(grdTemp, BID, challanNo, ExternalBID);
        }
    }
}