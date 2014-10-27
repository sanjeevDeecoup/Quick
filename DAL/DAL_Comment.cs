using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Comment
    {
        public string SaveCommentMaster(DTO.Comment Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_CommentMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CommentName", Ob.CommentName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateCommentMaster(DTO.Comment Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_CommentMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CommentName", Ob.CommentName);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@CommentID", Ob.CommentID);
            cmd.Parameters.AddWithValue("@DateModified", Ob.DateModified);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindGridView(DTO.Comment Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CommentMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CommentName", Ob.CommentName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowAll(DTO.Comment Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CommentMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteComment(DTO.Comment Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "sp_CommentMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CommentID", Ob.CommentID);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public bool checkAcsRights(string pageTitle, string userId, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageTitle", pageTitle);
            cmd.Parameters.AddWithValue("@UserTypeId", userId);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 34);
            res = PrjClass.ExecuteScalar(cmd);
            if (res == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkAcsRightsForFactory(string pageTitle, string userId, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            string res = string.Empty;
            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageTitle", pageTitle);
            cmd.Parameters.AddWithValue("@UserTypeId", userId);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 43);
            res = PrjClass.ExecuteScalar(cmd);
            if (res == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}