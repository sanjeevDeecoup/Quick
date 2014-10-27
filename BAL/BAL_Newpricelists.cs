using System.Data;

namespace BAL
{
    public class BAL_NewPriceLists
    {
        public DataSet fetchpricelist(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.fetchpricelist(Ob);
        }

        public DataSet fetchpricelistcoloum(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.fetchpricelistcoloum(Ob);
        }

        public DataSet fetchpricelistcoloumcount(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.fetchpricelistcoloumcount(Ob);
        }

        public DataSet fetchpricelistcoloumvaluecount(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.fetchpricelistcoloumvaluecount(Ob);
        }

        public DataSet fetchpricelistcoloumvaluecountvalue(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.fetchpricelistcoloumvaluecountvalue(Ob);
        }

        public string SaveNewItemprice(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.SaveNewItemprice(Ob);
        }

        public DataSet fetchpricevalue(DTO.NewPriceLists Ob)
        {
            return DAL.DALFactory.Instance.DAL_NewPriceLists.fetchpricevalue(Ob);
        }
    }
}