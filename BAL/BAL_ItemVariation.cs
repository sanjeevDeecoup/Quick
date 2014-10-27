using System.Data;

namespace BAL
{
    public class BAL_ItemVariation
    {
        public int SaveItemVariation(DTO.Item_Variations Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemVariation.SaveItemVariation(Ob);
        }

        public DataSet GetAllItemVariations(DTO.Item_Variations Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemVariation.GetAllItemVariations(Ob);
        }

        public int DeleteItemVariation(DTO.Item_Variations Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemVariation.DeleteItemVariation(Ob);
        }
    }
}