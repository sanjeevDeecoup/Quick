using System.Data;

namespace BAL
{
    public class BAL_ItemCategory
    {
        public int SaveItemCategory(DTO.Item_Categories Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemCategories.SaveItemCategory(Ob);
        }

        public DataSet GetAllItemCategory(DTO.Item_Categories Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemCategories.GetAllItemCategory(Ob);
        }

        public int DeleteItemCategory(DTO.Item_Categories Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemCategories.DeleteItemCategory(Ob);
        }
    }
}