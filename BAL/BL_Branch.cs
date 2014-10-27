using System.Data;

namespace BAL
{
    public class BL_Branch
    {
        public string SaveBranch(DTO.BranchMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Branch.SaveBranch(Ob);
        }

        public string UpdateBranch(DTO.BranchMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Branch.UpdateBranch(Ob);
        }

        public DataSet BindGrid(DTO.BranchMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Branch.BindGrid(Ob);
        }

        public string DeleteBranch(DTO.BranchMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Branch.DeleteBranch(Ob);
        }

        public DataSet FillTextBoxes(DTO.BranchMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Branch.FillTextBoxes(Ob);
        }

        public DataSet ShowBranch()
        {
            return DAL.DALFactory.Instance.DAL_Branch.ShowBranch();
        }

        public string GetWorkShopName(string BID, string ChallanNo)
        {
            return DAL.DALFactory.Instance.DAL_Branch.GetWorkShopName(BID, ChallanNo);
        }

        public DataSet ShowBranch(int BranchId)
        {
            return DAL.DALFactory.Instance.DAL_Branch.ShowBranch(BranchId);
        }
    }
}