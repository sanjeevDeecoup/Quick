using System.Data;

namespace BAL
{
    public class BAL_Priority
    {
        public string SavePriority(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Priority.SavePriority(Ob);
        }

        public string UpdatePriority(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Priority.UpdatePriority(Ob);
        }

        public DataSet ShowAllPriority(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Priority.ShowAllPriority(Ob);
        }

        public DataSet SearchPriority(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Priority.SearchPriority(Ob);
        }

        public string CheckBlanckEntries(string Labelname, string rate, string DayOffset)
        {
            return DAL.DALFactory.Instance.DAL_Priority.CheckBlanckEntries(Labelname, rate, DayOffset);
        }

        public string findAllPriority(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Priority.findAllPriority(BID);
        }
    }
}