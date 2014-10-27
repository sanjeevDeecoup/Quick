using System.Data;

namespace BAL
{
    public class BL_CategoryMaster
    {
        public string SaveCategoryMaster(DTO.Category Ob)
        {
            return DAL.DALFactory.Instance.DAL_CategoryMaster.SaveCategoryMaster(Ob);
        }

        public string UpdateCategoryMaster(DTO.Category Ob)
        {
            return DAL.DALFactory.Instance.DAL_CategoryMaster.UpdateCategoryMaster(Ob);
        }

        public DataSet BindGridView(DTO.Category Ob)
        {
            return DAL.DALFactory.Instance.DAL_CategoryMaster.BindGridView(Ob);
        }

        public DataSet ShowAll(DTO.Category Ob)
        {
            return DAL.DALFactory.Instance.DAL_CategoryMaster.ShowAll(Ob);
        }

        public string DeleteCategory(DTO.Category Ob)
        {
            return DAL.DALFactory.Instance.DAL_CategoryMaster.DeleteCategory(Ob);
        }
    }
}