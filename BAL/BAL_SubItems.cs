using System.Data;

namespace BAL
{
    public class BAL_SubItems
    {
        public int SaveSubItem(DTO.Sub_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_SubItem.SaveSubItem(Ob);
        }

        public DataSet GetAllSubItems(DTO.Sub_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_SubItem.GetAllSubItems(Ob);
        }

        public int DeleteSubItems(DTO.Sub_Items Ob)
        {
            return DAL.DALFactory.Instance.DAL_SubItem.DeleteSubItems(Ob);
        }
    }
}