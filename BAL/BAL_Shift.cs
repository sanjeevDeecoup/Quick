using System.Data;

namespace BAL
{
    public class BAL_Shift
    {
        public string SaveShift(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Shift.SaveShift(Ob);
        }

        public string UpdateShift(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Shift.UpdateShift(Ob);
        }

        public DataSet ShowAllShift(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Shift.ShowAllShift(Ob);
        }

        public DataSet SearchShift(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Shift.SearchShift(Ob);
        }

        public DataSet GetTaxDetails(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Shift.GetTaxDetails(BID);
        }
    }
}