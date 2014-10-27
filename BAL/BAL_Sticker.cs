using System.Data;

namespace BAL
{
    public class BAL_Sticker
    {
        public string Updatebarcodeconfig(DTO.Sticker Ob)
        {
            return DAL.DALFactory.Instance.DAL_Sticker.Updatebarcodeconfig(Ob);
        }

        public DataSet fetchbarcodeconfig(DTO.Sticker Ob)
        {
            return DAL.DALFactory.Instance.DAL_Sticker.fetchbarcodeconfig(Ob);
        }

        public string Updatebarcodewidthheight(DTO.Sticker ob)
        {
            return DAL.DALFactory.Instance.DAL_Sticker.Updatebarcodewidthheight(ob);
        }

        public DataSet GetDataStickerScreen(string BookingNo, string BID, string DueDate, string BookingUpto)
        {
            return DAL.DALFactory.Instance.DAL_Sticker.GetDataStickerScreen(BookingNo, BID, DueDate, BookingUpto);
        }

        public DataSet GetDataFactoryStickerScreen(string BookingNo, string BID, string DueDate)
        {
            return DAL.DALFactory.Instance.DAL_Sticker.GetDataFactoryStickerScreen(BookingNo, BID, DueDate);
        }

        public DataSet GetBookingDetailsData(string BookingNo, string EBID,string BID)
        {
            return DAL.DALFactory.Instance.DAL_Sticker.GetBookingDetailsData(BookingNo, EBID, BID);
        }
    }
}