using System.Data;

namespace BAL
{
    public class BL_BrandMaster
    {
        public string SaveBrandMaster(DTO.BrandMaster Ob)
        {
            return DAL.DALFactory.Instance.Dal_BrandMaster.SaveBrandMaster(Ob);
        }

        public string UpdateBrandMaster(DTO.BrandMaster Ob)
        {
            return DAL.DALFactory.Instance.Dal_BrandMaster.UpdateBrandMaster(Ob);
        }

        public DataSet BindGridView(DTO.BrandMaster Ob)
        {
            return DAL.DALFactory.Instance.Dal_BrandMaster.BindGridView(Ob);
        }

        public string DeleteBrand(DTO.BrandMaster Ob)
        {
            return DAL.DALFactory.Instance.Dal_BrandMaster.DeleteBrand(Ob);
        }
    }
}