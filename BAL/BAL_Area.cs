using System.Data;

namespace BAL
{
    public class BAL_Area
    {
        public string SaveArea(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.SaveArea(Ob);
            //return null;
        }

        public string UpdateArea(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.UpdateArea(Ob);
            //return null;
        }

        public DataSet ShowAllArea(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.ShowAllArea(Ob);
            //return null;
        }

        public DataSet SearchArea(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.SearchArea(Ob);
            //return null;
        }

        public string CheckBlanckEntries(string Labelname, string rate, string DayOffset)
        {
            return DAL.DALFactory.Instance.DAL_Area.CheckBlanckEntries(Labelname, rate, DayOffset);
            //return null;
        }

        public string findAllArea(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Area.findAllArea(BID);
            //return null;
        }

        public string DeleteBooking(string BranchId, string BookingNo)
        {
            return DAL.DALFactory.Instance.DAL_Area.DeleteBooking(BranchId, BookingNo);
        }
        public DataSet FillWebsiteCustomerTextBoxes(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.FillWebsiteCustomerTextBoxes(Ob);
        }
        public string UpdateCustomerDetailWebsite(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.UpdateCustomerDetailWebsite(Ob);
        }
        public DataSet SetData(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.SetData(Ob);
        }
        public DataSet SetDataInvoiceWise(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.SetDataInvoiceWise(Ob);
        }
        public DataSet GetGridData(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Area.GetGridData(Ob);
        }
        public DataSet GetCustomerMobileno(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Area.GetCustomerMobileno(BID);
        }
        public DataSet GetuserName(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Area.GetuserName(BID);
        }
    }
}