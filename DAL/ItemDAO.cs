using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ItemDAO
    {
        public DataSet GetAllItems(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllItems", BranchID);
        }

        public DataSet GetAllCategories(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllCategories", BranchID);
        }

        public DataSet GetAllPatterns(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllPatterns", BranchID);
        }

        public DataSet GetAllColors(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllColors", BranchID);
        }

        public DataSet GetAllBrands(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllBrands", BranchID);
        }

        public DataSet GetAllVariations(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllVariations", BranchID);
        }

        public DataSet GetAllComments(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetAllComments", BranchID);
        }

        public void SaveFeedback(string name, string feedback)
        {
            name = name.Replace("\"", "\"\"");
            feedback = feedback.Replace("\"", "\"\"");
            SqlHelper.ExecuteInsertQuery("insert into feedback(Active, name, comments) values (1, '" + name + "', '" + feedback + "')");
        }

        private DataSet CallStoredProcWithBranchParam(string spName, int BranchID)
        {
            SqlCommand com = SqlHelper.CreateSqlCommand(spName);
            com.Parameters.Add(new SqlParameter("@BranchID", BranchID));
            return (DataSet)SqlHelper.ExecuteStoredProc(com);
        }

        public DataSet GetPriceList(int BranchID)
        {
            return CallStoredProcWithBranchParam("usp_GetPriceList", BranchID);
        }
    }
}