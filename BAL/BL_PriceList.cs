using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BL_PriceList
    {
        public DataSet BindItemDropDown(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.BindItemDropDown(Ob);
        }

        public DataSet BindGrid(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.BindGrid(Ob);
        }

        public DataSet BindRemoveDrop(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.BindRemoveDrop(Ob);
        }

        public SqlDataReader ReadProcessRate(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.ReadProcessRate(Ob);
        }

        public string SaveDataInDataBase(GridView grdTable, DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.SaveDataInDataBase(grdTable, Ob);
        }

        public DataSet PaymentTypeGetDetails(string strAdvance, string strDeliverString, string FromDate, string UptoDate, bool status, string Others, string BID, string PackageSale, string PackageBooking)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.PaymentTypeGetDetails(strAdvance, strDeliverString, FromDate, UptoDate, status, Others, BID, PackageSale, PackageBooking);
        }

        public string SaveNewList(int copyListId, string newListName, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.SaveNewList(copyListId, newListName, branchId);
        }

        public DataSet BindAllListMasters(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_PriceList.BindAllListMaster(branchId);
        }
    }
}