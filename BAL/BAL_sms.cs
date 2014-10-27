using System.Data;

namespace BAL
{
    public class BAL_sms
    {
        public string Savesmsconfig(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.Savesmsconfig(Ob);
        }

        public string Updatesmsconfigr(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.Updatesmsconfigr(Ob);
        }

        public DataSet BindGridView(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.BindGridView(Ob);
        }

        public DataSet ShowAll(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.ShowAll(Ob);
        }

        public string Deletesms(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.Deletesms(Ob);
        }

        public string apiupdate(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.apiupdate(Ob);
        }

        public DataSet fetchapi(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.fetchapi(Ob);
        }

        public string defaultsmsupdate(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.defaultsmsupdate(Ob);
        }

        public DataSet fetchdropbooking(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.fetchdropbooking(Ob);
        }

        public DataSet fetchdropcloth(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.fetchdropcloth(Ob);
        }

        public DataSet fetchdropdelivery(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.fetchdropdelivery(Ob);
        }

        public DataSet previewbooking(DTO.sms Ob)
        {
            return DAL.DALFactory.Instance.DAL_sms.previewbooking(Ob);
        }

        public DataSet ReadyClothScreenSms(string BID)
        {
            return DAL.DALFactory.Instance.DAL_sms.ReadyClothScreenSms(BID);
        }

        public bool SetSMSCheckBoxOnScreen(string BID, string Flag)
        {
            return DAL.DALFactory.Instance.DAL_sms.SetSMSCheckBoxOnScreen(BID, Flag);
        }

        public bool CheckReadyClothSendSms(string BID)
        {
            return DAL.DALFactory.Instance.DAL_sms.CheckReadyClothSendSms(BID);
        }

        public bool CheckDeliverSlipViewRight(string BID, string BookingNo, string UserTypeId)
        {
            return DAL.DALFactory.Instance.DAL_sms.CheckDeliverSlipViewRight(BID, BookingNo, UserTypeId);
        }

        public bool CheckValidBookingNo(string BID, string BookingNo)
        {
            return DAL.DALFactory.Instance.DAL_sms.CheckValidBookingNo(BID, BookingNo);
        }

        public bool CheckDeliverSMSStatus(string BID, string BookingNo)
        {
            return DAL.DALFactory.Instance.DAL_sms.CheckDeliverSMSStatus(BID, BookingNo);
        }

        public string GetsmsTemplateName(string BID, string smsId)
        {
            return DAL.DALFactory.Instance.DAL_sms.GetsmsTemplateName(BID, smsId);
        }
    }
}