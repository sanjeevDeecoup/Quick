using System.Collections;

namespace BAL
{
    public class BAL_DateAndTime
    {
        public ArrayList getDateAndTimeAccordingToZoneTime(string BID)
        {
            return DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
        }

        public bool CheckBookingNumber(string BookingNumber, string BID)
        {
            return DAL.DALFactory.Instance.DAL_DateAndTime.CheckBookingNumber(BookingNumber, BID);
        }

        public bool CheckBookingNumberInFactory(string bookingNumber, string ExternalBID)
        {
            return DAL.DALFactory.Instance.DAL_DateAndTime.CheckBookingNumberInFactory(bookingNumber, ExternalBID);
        }
    }
}