using System.Data;

namespace BAL
{
    public class BAL_Color
    {
        public string SaveColor(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Color.SaveColor(Ob);
        }

        public string UpdateColor(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Color.UpdateColor(Ob);
        }

        public DataSet ShowAllColor(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Color.ShowAllColor(Ob);
        }

        public DataSet SearchColor(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Color.SearchColor(Ob);
        }

        public string DeleteColor(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Color.DeleteColor(Ob);
        }

        public DataSet BindDropDown()
        {
            return DAL.DALFactory.Instance.DAL_Color.BindDropDown();
        }
        public DataSet GetStatus(string BID, string UserName, string Password)
        {
            return DAL.DALFactory.Instance.DAL_Color.GetStatus(BID, UserName, Password);
        }
        public string UpdatePassword(string BID, string UserName, string Password, string NewPassword)
        {
            return DAL.DALFactory.Instance.DAL_Color.UpdatePassword(BID, UserName, Password, NewPassword);
        }

        public DataSet FindCustomerName(string BID, string CustID)
        {
            return DAL.DALFactory.Instance.DAL_Color.FindCustomerName(BID, CustID);
        }
        public DataSet BindGridCustomerSearch(string BID, string CustName)
        {
            return DAL.DALFactory.Instance.DAL_Color.BindGridCustomerSearch(BID, CustName);
        }

        public DataSet BindDeliveryStatus(string BookingNumber, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Color.BindDeliveryStatus(BookingNumber, BID);
        }
        public DataSet GetStoreNameAddress(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Color.GetStoreNameAddress(BID);
        }
    }
}