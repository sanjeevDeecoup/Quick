using System.Data;

namespace BAL
{
    public class BAL_Item
    {
        private DTO.Common Common = new DTO.Common();

        public string SaveItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.SaveItem(Ob);
        }

        public string SaveSubItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.SaveSubItem(Ob);
        }

        public DataSet ShowItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.ShowItem(Ob);
        }

        public string UpdateItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.UpdateItem(Ob);
        }

        public string DeleteSubItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.DeleteSubItem(Ob);
        }

        public DataSet ShowAllItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.ShowAllItem(Ob);
        }

        public DataSet SearchItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.SearchItem(Ob);
        }

        public string UpdateExitingItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.UpdateExitingItem(Ob);
        }

        public string SaveNewItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.SaveNewItem(Ob);
        }

        public DataSet BindGrid(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.BindGrid(Ob);
        }

        public DataSet FillTextBox(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.FillTextBox(Ob);
        }

        public string DeleteNewItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.DeleteNewItem(Ob);
        }

        public string UpdateNewItem(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.UpdateNewItem(Ob);
        }

        public bool CheckIfItemCodeExits(string code, string branchID)
        {
            return DAL.DALFactory.Instance.DAL_Item.CheckIfItemCodeExits(code, branchID);
        }

        public bool CheckItemStatus(DTO.Item Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.CheckItemStatus(Ob);
        }

        public bool CheckIfProcessCodeExits(string code, string BranchID)
        {
            return DAL.DALFactory.Instance.DAL_Item.CheckIfProcessCodeExits(code, BranchID);
        }

        public bool CheckCorrectItem(string ItemName, string BID, bool isSubItem = false)
        {
            return DAL.DALFactory.Instance.DAL_Item.CheckCorrectItem(ItemName, BID, isSubItem);
        }

        public string[] GetAllItemsDetailed(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_Item.GetAllItemsDetailed(branchId);
        }
        public string ItemRename(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Item.ItemRename(Ob);
        }
    }
}