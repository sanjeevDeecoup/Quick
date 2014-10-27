using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DAL
{
    public class DAL_PriceList
    {
        public DataSet BindItemDropDown(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ItemWisePriceList";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindGrid(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ItemWisePriceList";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 2);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindRemoveDrop(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Proc_ReturnChoth";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public SqlDataReader ReadProcessRate(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            cmd.CommandText = "sp_ItemWisePriceList";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ItemName", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 3);
            sdr = PrjClass.ExecuteReader(cmd);
            return sdr;
        }

        public string SaveDataInDataBase(GridView grdTable, DTO.Common Ob)
        {
            string res = "";
            DeleteItemRate(Ob);
            for (int r = 0; r < grdTable.Rows.Count; r++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_ItemWisePriceList";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", Ob.Input);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
                cmd.Parameters.AddWithValue("@ProcessCode", ((HiddenField)grdTable.Rows[r].FindControl("hdnProcessCode")).Value);
                cmd.Parameters.AddWithValue("@ProcessRate", ((TextBox)grdTable.Rows[r].Cells[1].FindControl("txtProcessRate")).Text.Trim());
                cmd.Parameters.AddWithValue("@Flag", 5);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        private bool DeleteItemRate(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "";
            cmd.CommandText = "sp_ItemWisePriceList";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);
            cmd.Parameters.AddWithValue("@ItemName", Ob.Input);
            cmd.Parameters.AddWithValue("@Flag", 4);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (res == "Record Saved")
                return true;
            else
                return false;
        }

        public DataSet PaymentTypeGetDetails(string strAdvance, string strDeliverString, string FromDate, string UptoDate, bool status, string Others, string BID, string PackageSale, string PackageBooking)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "Sp_Report_PaymentTypeReport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", FromDate);
            cmd.Parameters.AddWithValue("@BookDate2", UptoDate);
            cmd.Parameters.AddWithValue("@SearchText", strDeliverString);
            cmd.Parameters.AddWithValue("@ForAdvance", strAdvance);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Others", Others);
            cmd.Parameters.AddWithValue("@SaleBooking", PackageSale);
            cmd.Parameters.AddWithValue("@SaleNewBooking", PackageBooking);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string SaveNewList(int copyListId, string newListName, string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_ItemWisePriceList",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 7);
            if (copyListId == -1)
            {
                sqlCommand.Parameters.AddWithValue("@prevList", 0);
                sqlCommand.Parameters.AddWithValue("@IsInsertingBlankList", true);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@prevList", copyListId);
            }
            sqlCommand.Parameters.AddWithValue("@newList", newListName);
            return PrjClass.ExecuteNonQuery(sqlCommand);
        }

        public DataSet BindAllListMaster(string branchId)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_ItemWisePriceList",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 6);
            return PrjClass.GetData(sqlCommand);
        }
    }
}
