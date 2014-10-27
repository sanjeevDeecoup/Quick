using System.Data;

namespace BAL
{
    public class BAL_Comment
    {
        public string SaveCommentMaster(DTO.Comment Ob)
        {
            return DAL.DALFactory.Instance.DAL_Comment.SaveCommentMaster(Ob);
        }

        public string UpdateCommentMaster(DTO.Comment Ob)
        {
            return DAL.DALFactory.Instance.DAL_Comment.UpdateCommentMaster(Ob);
        }

        public DataSet BindGridView(DTO.Comment Ob)
        {
            return DAL.DALFactory.Instance.DAL_Comment.BindGridView(Ob);
        }

        public DataSet ShowAll(DTO.Comment Ob)
        {
            return DAL.DALFactory.Instance.DAL_Comment.ShowAll(Ob);
        }

        public string DeleteComment(DTO.Comment Ob)
        {
            return DAL.DALFactory.Instance.DAL_Comment.DeleteComment(Ob);
        }

        public bool checkAcsRights(string pageTitle, string userId, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Comment.checkAcsRights(pageTitle, userId, BID);
        }

        public bool checkAcsRightsForFactory(string pageTitle, string userId, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Comment.checkAcsRightsForFactory(pageTitle, userId, BID);
        }
    }
}