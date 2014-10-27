using System.Data;

namespace BAL
{
    public class BAL_Patterns
    {
        public string SavePatternMaster(DTO.Patterns Ob)
        {
            return DAL.DALFactory.Instance.DAL_Patterns.SavePatternMaster(Ob);
        }

        public string UpdatePatternMaster(DTO.Patterns Ob)
        {
            return DAL.DALFactory.Instance.DAL_Patterns.UpdatePatternMaster(Ob);
        }

        public DataSet BindGridView(DTO.Patterns Ob)
        {
            return DAL.DALFactory.Instance.DAL_Patterns.BindGridView(Ob);
        }

        public DataSet ShowAll(DTO.Patterns Ob)
        {
            return DAL.DALFactory.Instance.DAL_Patterns.ShowAll(Ob);
        }

        public string DeletePatterns(DTO.Patterns Ob)
        {
            return DAL.DALFactory.Instance.DAL_Patterns.DeletePatterns(Ob);
        }
    }
}