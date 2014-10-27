using System.Data;

namespace BAL
{
    public class BL_HolidayMaster
    {
        public string SaveHolidayMaster(DTO.Holiday Ob)
        {
            return DAL.DALFactory.Instance.DAL_HolidayMaster.SaveHolidayMaster(Ob);
        }

        public string UpdateHolidayMaster(DTO.Holiday Ob)
        {
            return DAL.DALFactory.Instance.DAL_HolidayMaster.UpdateHolidayMaster(Ob);
        }

        public DataSet BindGridView(DTO.Holiday Ob)
        {
            return DAL.DALFactory.Instance.DAL_HolidayMaster.BindGridView(Ob);
        }

        public string deleteHolidayMaster(DTO.Holiday Ob)
        {
            return DAL.DALFactory.Instance.DAL_HolidayMaster.deleteHolidayMaster(Ob);
        }

        public string UpdateWeekend(DTO.Holiday Ob)
        {
            return DAL.DALFactory.Instance.DAL_HolidayMaster.UpdateWeekend(Ob);
        }
    }
}