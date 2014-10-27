using System.Data;

namespace BAL
{
    public class BL_WorkShopAllFunction
    {
        public DataSet BindShopName()
        {
            return DAL.DALFactory.Instance.Dal_WorkShopAllFunction.BindShopName();
            //return null;
        }

        public DataSet BindAllChallanBranchWise(string BID, string Date1, string Date2)
        {
            return DAL.DALFactory.Instance.Dal_WorkShopAllFunction.BindAllChallanBranchWise(BID, Date1, Date2);
        }
        public bool FindAndKillProcess(string name)
        {
            return DAL.DALFactory.Instance.Dal_WorkShopAllFunction.FindAndKillProcess(name);
        }
    }
}