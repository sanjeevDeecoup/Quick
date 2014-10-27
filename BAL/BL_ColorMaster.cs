using System.Data;

namespace BAL
{
    public class BL_ColorMaster
    {
        public string SaveColorMaster(DTO.ColorMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.SaveColorMaster(Ob);
        }

        public string UpdateColorMaster(DTO.ColorMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.UpdateColorMaster(Ob);
        }

        public DataSet BindGridView(DTO.ColorMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.BindGridView(Ob);
        }

        public string deleteColorMaster(DTO.ColorMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.deleteColorMaster(Ob);
        }

        public DataSet CheckRemote(string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.CheckRemote(BranchId);
        }

        public DataSet CheckBackupEmail(string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.CheckBackupEmail(BranchId);
        
        }
        public bool CheckCloud(string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.CheckCloud(BranchId);
        }
        public string SaveLoginHistoryData(string BID, string LoginDate, string LoginTime, string Success, string ReasonID, string UserID)
        {
            return DAL.DALFactory.Instance.DAL_ColorMaster.SaveLoginHistoryData(BID, LoginDate, LoginTime, Success, ReasonID, UserID);
        }
    }
}