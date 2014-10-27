using System.Data;
using System.Xml;
using DAL;
using DTO;

namespace BAL
{
    public class BookingBAO
    {
        public string GetAllDefaults(int BranchID)
        {
            DataSet ds = DALFactory.Instance.BookingDAO.GetAllDefaults(BranchID);
            return GetXmlForDefaults(ds);
        }

        private string GetXmlForDefaults(DataSet ds)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement defaults = doc.CreateElement("defaults");
            //Misc Defaults
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement ele = doc.CreateElement("item");
                ele.InnerText = row["DefaultItemID"].ToString();
                defaults.AppendChild(ele);

                ele = doc.CreateElement("process");
                ele.InnerText = row["DefaultProcessID"].ToString();
                defaults.AppendChild(ele);

                ele = doc.CreateElement("duedate");
                ele.InnerText = row["DefaultDateOffset"].ToString();
                defaults.AppendChild(ele);

                ele = doc.CreateElement("duetime");
                ele.InnerText = row["DefaultTime"].ToString();
                defaults.AppendChild(ele);

                ele = doc.CreateElement("bookingnumber");
                ele.InnerText = row["StartBookingNumberFrom"].ToString();
                defaults.AppendChild(ele);
            }

            XmlElement urgentdesc = doc.CreateElement("urgentdescriptions");
            //Urgent Defaults
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                XmlElement urgent = doc.CreateElement("urgent");
                urgent.SetAttribute("seq", row["UrgentSeq"].ToString());

                XmlElement ele = doc.CreateElement("dateoffset");
                ele.InnerText = row["UrgentOffset"].ToString();
                urgent.AppendChild(ele);

                ele = doc.CreateElement("rate");
                ele.InnerText = row["UrgentRate"].ToString();
                urgent.AppendChild(ele);

                urgentdesc.AppendChild(urgent);
            }
            defaults.AppendChild(urgentdesc);

            doc.AppendChild(defaults);
            return doc.InnerXml;
        }

        public int SaveBooking(string bookingXml, int BranchID)
        {
            //Transform Xml into DTO
            BookingReceiptTransformer trans = new BookingReceiptTransformer(bookingXml, BranchID);
            BookingReceipt objReceipt = trans.Transform();
            return DALFactory.Instance.BookingDAO.SaveBooking(bookingXml, objReceipt, BranchID);
        }

        public string GetBookingXml(int bookingNumber, int BranchID)
        {
            DataSet ds = DALFactory.Instance.BookingDAO.GetBookingXml(bookingNumber, BranchID);
            return ds.Tables[0].Rows[0][0].ToString();
        }
    }
}