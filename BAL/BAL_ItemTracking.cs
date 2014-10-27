using System.Data;

namespace BAL
{
    public class BAL_ItemTracking
    {
        public int SaveItemTracking(DTO.Item_Tracking Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemTracking.SaveItemTracking(Ob);
        }

        public DataSet GetAllItemTracking(DTO.Item_Tracking Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemTracking.GetAllItemTracking(Ob);
        }

        public int DeleteItemtracking(DTO.Item_Tracking Ob)
        {
            return DAL.DALFactory.Instance.DAL_ItemTracking.DeleteItemtracking(Ob);
        }
    }
}