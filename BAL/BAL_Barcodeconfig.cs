using System.Data;

namespace BAL
{
    public class BAL_Barcodeconfig
    {
        public string Updatebarcodeconfig(DTO.Barcodeconfig Ob)
        {
            return DAL.DALFactory.Instance.DAL_Barcodeconfig.Updatebarcodeconfig(Ob);
        }

        public DataSet fetchbarcodeconfig(DTO.Barcodeconfig Ob)
        {
            return DAL.DALFactory.Instance.DAL_Barcodeconfig.fetchbarcodeconfig(Ob);
        }

        public string Updatebarcodewidthheight(DTO.Barcodeconfig ob)
        {
            return DAL.DALFactory.Instance.DAL_Barcodeconfig.Updatebarcodewidthheight(ob);
        }
    }
}