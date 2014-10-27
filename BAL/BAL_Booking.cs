using System.Data;
using System.Xml;

namespace BAL
{
    //This class is redundant. To be removed
    public class BAL_Booking
    {
        public int SaveBookingData(DTO.Booking Ob)
        {
            return 0;
            //return DAL.DALFactory.Instance.DAL_Booking.SaveBookingData(Ob);
        }

        public DataSet GetDS(DTO.Booking Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking.GetDS(Ob);
        }

        public int DeleteBooking(DTO.Booking Ob)
        {
            return DAL.DALFactory.Instance.DAL_Booking.DeleteBooking(Ob);
        }

        public void SaveFeedback(string feedbackXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(feedbackXml);
            string name = doc.SelectSingleNode("//name").InnerText;
            string feedback = doc.SelectSingleNode("//feedback").InnerText;
            //int index = feedbackXml.IndexOf("~~||~~", 0);
            //DALFactory.Instance.DAL_Booking.SaveFeedback(feedbackXml.Substring(0, index), feedbackXml.Substring(index));
        }
    }
}