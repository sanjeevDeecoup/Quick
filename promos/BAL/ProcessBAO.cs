using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
using System.Xml;
using System.Configuration;

namespace BAL
{
    public class ProcessBAO
    {
        public string GetAllProcesses()
        {
            DataSet ds = DALFactory.Instance.ProcessDAO.GetAllProcesses();
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

                root.AppendChild(process);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }
    }
}
