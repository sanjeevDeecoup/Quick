using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BAL_BarcodeLable
    {
        public string SaveBarcodeLable(DTO.BacodeLable Ob)
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.SaveBarcodeLable(Ob);
        }

        public string UpdateBarcodeLabel(DTO.BacodeLable ob)
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.UpdateBarcodeLabel(ob);
        }

        public DataSet SearchBarcodeLabel(DTO.BacodeLable ob)
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.SearchBarcodeLabel(ob);
        }

        public string delbarcodelable()
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.delbarcodelable();
        }

        public string savedemo(DTO.BacodeLable Ob)
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.savedemo(Ob);
        }

        public bool CheckAllCheckBoxesInGrid(GridView grd)
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.CheckAllCheckBoxesInGrid(grd);
        }

        public bool CheckRecord()
        {
            return DAL.DALFactory.Instance.DAL_BarcodeLable.CheckRecord();
        }
    }
}