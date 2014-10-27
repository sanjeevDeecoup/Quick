using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BL_PackageMaster
    {
        public string SavePackage(DTO.PackageMaster Ob, GridView grdQtyDetail)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.SavePackage(Ob, grdQtyDetail);
        }

        public string UpdatePackage(DTO.PackageMaster Ob, GridView grdQtyDetail)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.UpdatePackage(Ob, grdQtyDetail);
        }

        public DataSet ShowAllPackage(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.ShowAllPackage(Ob);
        }

        public DataSet SearchPackage(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.SearchPackage(Ob);
        }

        public string DeletePackage(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.DeletePackage(Ob);
        }

        public DataSet BindPackageDropDown(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.BindPackageDropDown(Ob);
        }

        public DataSet GetAllPackgaeDetail(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.GetAllPackgaeDetail(Ob);
        }

        public string SaveAssignPackage(DTO.PackageMaster Ob, bool isQtyItemBased = false)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.SaveAssignPackage(Ob, isQtyItemBased);
        }

        public DataSet ShowAllAssignPackage(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.ShowAllAssignPackage(Ob);
        }

        public bool CheckOrginalCustomer(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.CheckOrginalCustomer(Ob);
        }
        public bool CheckOrginalUser(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.CheckOrginalUser(Ob);
        }
        public DataSet GetCustomerAddress(string CustomerCode, string BID)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.GetCustomerAddress(CustomerCode, BID);
        }

        public DataSet GetAssignDetails(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.GetAssignDetails(Ob);
        }

        public bool CheckPackageAssignInBookingTable(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.CheckPackageAssignInBookingTable(Ob);
        }

        public bool CheckPackageAssignToCustomer(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.CheckPackageAssignToCustomer(Ob);
        }

        public string UpdateAssignPackage(DTO.PackageMaster Ob, bool isQtyItemBased = false)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.UpdateAssignPackage(Ob, isQtyItemBased);
        }

        public string DeleteAssignPackage(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.DeleteAssignPackage(Ob);
        }

        public string UpdateMarkComplete(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.UpdateMarkComplete(Ob);
        }

        public bool CheckPackageInAssignTable(DTO.PackageMaster Ob)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.CheckPackageInAssignTable(Ob);
        }

        public DataSet GetPackageQtyDetail(string PackageId, string BID)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.GetPackageQtyDetail(PackageId, BID);
        }

        public string GetQtyndItemsForPackage(string custCode, int assignId, int recurrenceId, string BID)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.GetQtyndItemsForPackage(custCode, assignId, recurrenceId, BID);
        }

        //public string SaveInRecurrence(DateTime startDate, int cycleDuration, int noOfRecurrence, string branchId)
        //{
        //    return DAL.DALFactory.Instance.DAL_PackageMaster.SaveInRecurrence(startDate, cycleDuration, noOfRecurrence, branchId);
        //}

        public object FindAllPackageTypes(string branchId)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.FindAllPackageTypes(branchId);
        }

        public string GetQtyPackageDetails(int assignId, string branchId)
        {
            return DAL.DALFactory.Instance.DAL_PackageMaster.GetQtyPackageDetails(assignId, branchId);
        }
    }
}