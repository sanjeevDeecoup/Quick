using System.Data;

namespace BAL
{
    public class BAL_FactoryChallanHeader
    {
        public int SaveFactoryChallanHeader(DTO.FactoryChallanHeader Ob)
        {
            return DAL.DALFactory.Instance.DAL_FactoryChallanHeader.SaveFactoryChallanHeader(Ob);
        }

        public DataSet GetAllFactoryChallanHeader(DTO.FactoryChallanHeader Ob)
        {
            return DAL.DALFactory.Instance.DAL_FactoryChallanHeader.GetAllFactoryChallanHeader(Ob);
        }

        public int DeleteFactoryChallanHeader(DTO.FactoryChallanHeader Ob)
        {
            return DAL.DALFactory.Instance.DAL_FactoryChallanHeader.DeleteFactoryChallanHeader(Ob);
        }
    }
}