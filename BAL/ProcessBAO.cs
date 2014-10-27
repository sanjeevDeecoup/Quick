using System.Configuration;
using System.Data;
using System.Xml;
using DAL;

namespace BAL
{
    public class ProcessBAO
    {
        public string GetAllProcesses(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ProcessDAO.GetAllProcesses(BranchID);
            return GetStringForAllProceses(ds.Tables[0]);
        }

        private string GetStringForAllProceses(DataTable dt)
        {
            string imagePath = ConfigurationManager.AppSettings["processImagePath"];
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("processes");
            foreach (DataRow row in dt.Rows)
            {
                XmlElement process = doc.CreateElement("process");
                process.SetAttribute("id", row["ProcessID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["ProcessName"].ToString();
                process.AppendChild(ele);

                ele = doc.CreateElement("Code");
                ele.InnerText = row["ProcessCode"].ToString();
                process.AppendChild(ele);

                ele = doc.CreateElement("Image");
                ele.InnerText = imagePath + row["ProcessImage"].ToString();
                process.AppendChild(ele);

                ele = doc.CreateElement("isdiscount");
                ele.InnerText = row["IsDiscount"].ToString();
                process.AppendChild(ele);

                ele = doc.CreateElement("tax");
                ele.InnerText = row["ServiceTax"].ToString();
                process.AppendChild(ele);

                root.AppendChild(process);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }
    }
}