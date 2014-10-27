using System.Data;

namespace BAL
{
    public class BAL_Variation
    {
        public string SaveVariation(DTO.Variation Ob)
        {
            return DAL.DALFactory.Instance.DAL_Variation.SaveVariation(Ob);
        }

        public string UpdateVariation(DTO.Variation Ob)
        {
            return DAL.DALFactory.Instance.DAL_Variation.UpdateVariation(Ob);
        }

        public DataSet BindGrid(DTO.Variation Ob)
        {
            return DAL.DALFactory.Instance.DAL_Variation.BindGrid(Ob);
        }

        public DataSet BindGridView(DTO.Variation Ob)
        {
            return DAL.DALFactory.Instance.DAL_Variation.BindGridView(Ob);
        }

        public string Deletevariation(DTO.Variation Ob)
        {
            return DAL.DALFactory.Instance.DAL_Variation.Deletevariation(Ob);
        }
    }
}