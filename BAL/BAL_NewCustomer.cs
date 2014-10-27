using System.Data;

namespace BAL
{
    public class BAL_NewCustomer
    {
        public int SaveNewCustomer(DTO.CustomerMaster Ob)
        {
            //return DAL.DALFactory.Instance.DAL_NewCustomer.SaveNewCustomer(Ob);
            return 0;
        }

        public DataSet GetAllCustomers(DTO.CustomerMaster Ob)
        {
            //return DAL.DALFactory.Instance.DAL_NewCustomer.GetAllCustomers(Ob);
            return null;
        }

        public int DeleteCustomer(DTO.CustomerMaster Ob)
        {
            //return DAL.DALFactory.Instance.DAL_NewCustomer.DeleteCustomer(Ob);
            return 0;
        }

        public DataSet SearchCUstomer(DTO.CustomerMaster Ob)
        {
            //return DAL.DALFactory.Instance.DAL_NewCustomer.SearchCUstomer(Ob);
            return null;
        }
    }
}