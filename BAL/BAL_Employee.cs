using System.Data;

namespace BAL
{
    public class BAL_Employee
    {
        public string SaveEmployee(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.SaveEmployee(Ob);
        }

        public bool RecordAllreadyExists(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.RecordAllreadyExists(Ob);
        }

        public DataSet ShowAllCustomer(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.ShowAllCustomer(Ob);
        }

        public DataSet FillEmployee(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.FillEmployee(Ob);
        }

        public string UpdateEmployee(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.UpdateEmployee(Ob);
        }

        public DataSet SearchEmployee(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.SearchEmployee(Ob);
        }

        public string DeleteEmployee(DTO.Employee Ob)
        {
            return DAL.DALFactory.Instance.DAL_Employee.DeleteEmployee(Ob);
        }

        public DataSet GetTodayPending(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Employee.GetTodayPending(Date, BID);
        }

        public DataSet ShowCustomerBirthday(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Employee.ShowCustomerBirthday(Date, BID);
        }

        public DataSet ShowCustomerAnniversary(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Employee.ShowCustomerAnniversary(Date, BID);
        }

        public DataSet ShowHomeDelivery(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Employee.ShowHomeDelivery(Date, BID);
        }

        public DataSet ShowUrguntBooking(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Employee.ShowUrguntBooking(Date, BID);
        }
    }
}