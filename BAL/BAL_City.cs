using System.Data;

namespace BAL
{
    public class BAL_City
    {
        public string SaveCity(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.SaveCity(Ob);
            //return null;
        }

        public string UpdateCity(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.UpdateCity(Ob);
            //return null;
        }

        public DataSet ShowAllCity(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.ShowAllCity(Ob);
            //return null;
        }

        public DataSet SearchCity(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.SearchCity(Ob);
            //return null;
        }

        public string CheckBlanckEntries(string Labelname, string rate, string DayOffset)
        {
            return DAL.DALFactory.Instance.DAL_City.CheckBlanckEntries(Labelname, rate, DayOffset);
            //return null;
        }

        public string findAllCity(string BID)
        {
            return DAL.DALFactory.Instance.DAL_City.findAllCity(BID);
            //return null;
        }

        public DataSet ShowAllExpense(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.ShowAllExpense(Ob);
        }

        public string DeleteExpenses(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.DeleteExpenses(Ob);
        }

        public DataSet DeliveryDetail(string BID, string BNo)
        {
            return DAL.DALFactory.Instance.DAL_City.DeliveryDetail(BID, BNo);
        }

        public DataSet UpdateLedgerName(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.UpdateLedgerName(Ob);
        }

        public DataSet UpdateIncomeLedgerName(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_City.UpdateIncomeLedgerName(Ob);
        }

        public DataSet RegDatabase()
        {
            return DAL.DALFactory.Instance.DAL_City.RegDatabase();
        }

        public DataSet RedoGarment(string BID, string BNo)
        {
            return DAL.DALFactory.Instance.DAL_City.RedoGarment(BID, BNo);
        }

        public float getPriceAccordingProcess(string BID, string Process, string ItemName, int rateListId)
        {
            return DAL.DALFactory.Instance.DAL_City.getPriceAccordingProcess(BID, Process, ItemName, rateListId);
        }

        public string getCorrectProcess(string BID, string Process)
        {
            return DAL.DALFactory.Instance.DAL_City.getCorrectProcess(BID, Process);
        }

        public string SaveRedo(string BID, string BNo, int rowindex, string ProcessCode,string duedate,int TotalQty)
        {
            return DAL.DALFactory.Instance.DAL_City.SaveRedo(BID, BNo, rowindex, ProcessCode,duedate,TotalQty);
        }

        public string GetDefaultProcessAndRateForRateList(string itemName, int rateListId, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_City.GetDefaultProcessAndRateForRateList(itemName, rateListId, branchId);
        }
    }
}