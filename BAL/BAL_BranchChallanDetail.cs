using System.Data;

namespace BAL
{
    public class BAL_BranchChallanDetail
    {
        public int SaveBranchChallanDetail(DTO.BranchChallanDetail Ob)
        {
            return DAL.DALFactory.Instance.DAL_BranchChallanDetail.SaveBranchChallanDetail(Ob);
        }

        public DataSet GetAllBranchChallanDetail(DTO.BranchChallanDetail Ob)
        {
            return DAL.DALFactory.Instance.DAL_BranchChallanDetail.GetAllBranchChallanDetail(Ob);
        }

        public int DeleteBranchChallanDetail(DTO.BranchChallanDetail Ob)
        {
            return DAL.DALFactory.Instance.DAL_BranchChallanDetail.DeleteBranchChallanDetail(Ob);
        }
    }
}