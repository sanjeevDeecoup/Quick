using System.Data;

namespace BAL
{
    public class BAL_Profession
    {
        public string SaveProfession(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Profession.SaveProfession(Ob);
            //return null;
        }

        public string UpdateProfession(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Profession.UpdateProfession(Ob);
            //return null;
        }

        public DataSet ShowAllProfession(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Profession.ShowAllProfession(Ob);
            //return null;
        }

        public DataSet SearchProfession(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_Profession.SearchProfession(Ob);
            //return null;
        }

        public string CheckBlanckEntries(string Labelname, string rate, string DayOffset)
        {
            return DAL.DALFactory.Instance.DAL_Profession.CheckBlanckEntries(Labelname, rate, DayOffset);
            //return null;
        }

        public string findAllProfession(string BID)
        {
            return DAL.DALFactory.Instance.DAL_Profession.findAllProfession(BID);
            //return null;
        }

        public string SetButtonAccordingMenuRights(string BID, string PageName, string UID)
        {
            return DAL.DALFactory.Instance.DAL_Profession.SetButtonAccordingMenuRights(BID, PageName, UID);
        }
    }
}