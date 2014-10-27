using System.Data;

namespace BAL
{
    public class BL_CustomerMaster
    {
        public string SaveNewCustomer(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.SaveNewCustomer(Ob);
        }

        public string UpdateCustomer(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.UpdateCustomer(Ob);
        }

        public DataSet BindGridSearch(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.BindGridSearch(Ob);
        }

        public DataSet FillTextBoxes(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.FillTextBoxes(Ob);
        }
        public DataSet CheckDiscountPackage(DTO.CustomerMaster Ob)
        { 
        return DAL.DALFactory.Instance.DAL_Customer.CheckDiscountPackage(Ob);
        }

        public string DeleteCustomer(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.DeleteCustomer(Ob);
        }

        public DataSet ExportToExcel(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.ExportToExcel(Ob);
        }

        public DataSet BindPriority(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.BindPriority(Ob);
        }

        public string SavePriority(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.SavePriority(Ob);
        }

        public string SaveNewUser(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.SaveNewUser(Ob);
        }

        public string UpdateWebCustomer(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.UpdateWebCustomer(Ob);
        }

        public DataSet BindGrid(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.BindGrid(Ob);
        }

        public string MergeDuplicateCustomer(DTO.CustomerMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_Customer.MergeDuplicateCustomer(Ob);
        }

        public DataSet BindGridCustomerSearch(string CustName, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.BindGridCustomerSearch(CustName, BID);
        }

        public string GetCustNameFromCode(string custCode, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetCustNameFromCode(custCode, BID);
        }

        public bool CheckCustomerPackageActive(string CustCode, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.CheckCustomerPackageActive(CustCode, BID);
        }

        public string UpdateCustomerDetailJQuery(string CustomerCode, string CustomerSalutation, string CustomerName, string CustomerAddress, string CustomerPhone, string CustomerMobile, string CustomerEmailId, string CustomerPriority, string CustomerRefferredBy, string DefaultDiscountRate, string Remarks, string BirthDate, string AnniversaryDate, string AreaLocation, string CommunicationMeans, string MemberShipId, string BarCode, string RateListId, bool IsWebsite, string tempCustMobile, string tempMemberShipId, string tempBarCode, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.UpdateCustomerDetailJQuery( CustomerCode,  CustomerSalutation,  CustomerName,  CustomerAddress,  CustomerPhone,  CustomerMobile,  CustomerEmailId,  CustomerPriority,  CustomerRefferredBy,  DefaultDiscountRate,  Remarks,  BirthDate,  AnniversaryDate,  AreaLocation,  CommunicationMeans,  MemberShipId,  BarCode,  RateListId,  IsWebsite,  tempCustMobile,  tempMemberShipId,  tempBarCode,BID);
        }

        public string ResestWebsite(string arg, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.ResestWebsite(arg, BID);
        }

        public string SaveNewCustomerDetailJquery(string CustomerSalutation, string CustomerName, string CustomerAddress, string CustomerPhone, string CustomerMobile, string CustomerEmailId, string CustomerPriority, string CustomerRefferredBy, string DefaultDiscountRate, string Remarks, string BirthDate, string AnniversaryDate, string AreaLocation, string CommunicationMeans, string MemberShipId, string BarCode, string RateListId, bool IsWebsite, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.SaveNewCustomerDetailJquery(CustomerSalutation, CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, CustomerEmailId, CustomerPriority, CustomerRefferredBy, DefaultDiscountRate, Remarks, BirthDate, AnniversaryDate, AreaLocation, CommunicationMeans, MemberShipId, BarCode, RateListId, IsWebsite, BID);
        }
        public string DeleteCustomerDetailJQuery(string CustomerCode,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.DeleteCustomerDetailJQuery(CustomerCode,BID);
        }
        public DataSet GetData(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetData(BID);
        }        
        public bool CheckBoundToMachine(string cookies,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.CheckBoundToMachine(cookies, BID);
        }
        public string CreatePasswordToBoundMachine(string DeviceName,string cookies,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.CreatePasswordToBoundMachine(DeviceName, cookies, BID);
        }
        public bool CheckVerificationCode(string Code,string cookies,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.CheckVerificationCode(Code, cookies, BID);
        }
        public DataSet GetBoundToMachineDetails(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetBoundToMachineDetails(BID);
        }
        public string DeleteBoundToMachineDetails(int Id)
        {
            return DAL.DALFactory.Instance.DAL_Customer.DeleteBoundToMachineDetails(Id);
        }

        public DataSet BindDropDown()
        {

            return DAL.DALFactory.Instance.DAL_Customer.BindDropDown();
        }

        public DataSet GetStatus(string BID, string UserName, string Password)
        {

            return DAL.DALFactory.Instance.DAL_Customer.GetStatus(BID, UserName, Password);

        }
        public string UpdatePassword(string BID, string UserName, string Password, string NewPassword)
        {

            return DAL.DALFactory.Instance.DAL_Customer.UpdatePassword(BID, UserName, Password, NewPassword);
        }
        public DataSet FindCustomerName(string BID, string CustID)
        {

            return DAL.DALFactory.Instance.DAL_Customer.FindCustomerName(BID, CustID);
        }

        public DataSet BindGridWebsiteCustomerSearch(string BID, string CustName)
        {

            return DAL.DALFactory.Instance.DAL_Customer.BindGridWebsiteCustomerSearch(BID, CustName);

        }
        public DataSet BindDeliveryStatus(string BookingNumber, string BID)
        {

            return DAL.DALFactory.Instance.DAL_Customer.BindDeliveryStatus(BookingNumber, BID);
        }

        public string BoundToMachineCheck(bool IsBoundToMachine,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.BoundToMachineCheck(IsBoundToMachine, BID);
        }
        public DataSet GetPassword(string cookies,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetPassword(cookies, BID);
        }
        public bool bountToMachineStatus(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.bountToMachineStatus(BID);
        }
        public DataSet GetGarmentStatusDetail(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetGarmentStatusDetail(Date, BID);
        }
        public DataSet GetPendingStatusDetail(string Date, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetPendingStatusDetail(Date, BID);
        }
        public string DeleteBoundToMachineData(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.DeleteBoundToMachineData(BID);
        }
        public DataSet GetStoreEmaiAndMobile(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.GetStoreEmaiAndMobile(BID);
        }
        public DataSet BindGridLoginHistory(string strFromDate, string strToDate, string UserID, string Reason, string Status, string BID)
        {
            return DAL.DALFactory.Instance.DAL_Customer.BindGridLoginHistory(strFromDate, strToDate, UserID,  Reason,  Status, BID);
        }

    }
}