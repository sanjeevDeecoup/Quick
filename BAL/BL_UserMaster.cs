using System.Data;

namespace BAL
{
    public class BL_UserMaster
    {
        public string SaveUser(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.SaveUser(Ob);
        }

        public string UpdateUser(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.UpdateUser(Ob);
        }

        public DataSet BindGrid(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.BindGrid(Ob);
        }

        public DataSet SearchAndShowAll(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.SearchAndShowAll(Ob);
        }

        public DataSet FillTextBoxes(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.FillTextBoxes(Ob);
        }

        public string[] GetAllUsers(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.GetAllUsers(branchId);
        }

        public string SaveFactoryUser(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.SaveFactoryUser(Ob);
        }

        public string UpdateWorkshopUser(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.UpdateWorkshopUser(Ob);
        }

        public DataSet BindWorkShopUserGrid(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.BindWorkShopUserGrid(Ob);
        }
        public DataSet WorkshopSearchAndShowAll(DTO.UserMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_UserMaster.WorkshopSearchAndShowAll(Ob);
        }

    }
}