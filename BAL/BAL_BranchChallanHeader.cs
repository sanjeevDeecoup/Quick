using System.Data;

namespace BAL
{
    public class BAL_BranchChallanHeader
    {
        public int SaveBranchChallanHeader(DTO.BranchChallanHeader Ob)
        {
            return DAL.DALFactory.Instance.DAL_BranchChallanHeader.SaveBranchChallanHeader(Ob);
        }

        public DataSet GetAllBranchChallanHeader(DTO.BranchChallanHeader Ob)
        {
            return DAL.DALFactory.Instance.DAL_BranchChallanHeader.GetAllBranchChallanHeader(Ob);
        }

        public int DeleteBranchChallanHeader(DTO.BranchChallanHeader Ob)
        {
            return DAL.DALFactory.Instance.DAL_BranchChallanHeader.DeleteBranchChallanHeader(Ob);
        }
    }
}