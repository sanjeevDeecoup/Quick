using System.Data;

namespace BAL
{
    public class BAL_Remark
    {
        public string SaveRemark(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Remarks.SaveRemark(Ob);
        }

        public string UpdateRemarks(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Remarks.UpdateRemarks(Ob);
        }

        public DataSet ShowAllRemarks(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Remarks.ShowAllRemarks(Ob);
        }

        public DataSet SearchRemarks(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Remarks.SearchRemarks(Ob);
        }

        public string DeleteRemarks(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Remarks.DeleteRemarks(Ob);
        }
    }
}