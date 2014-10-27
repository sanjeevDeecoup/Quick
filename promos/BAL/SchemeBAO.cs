using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Xml;

namespace BAL
{
    public class SchemeBAO
    {
        public string GetAllTemplatesData()
        {
            //Query database to get all scheme templates
            DataSet ds = DALFactory.Instance.SchemeDAO.GetAllTemplatesData();
            //Process this dataset to return an XML string
            return GetStringXmlForPromoData(ds.Tables[0], "SchemeXml");
        }
        private string GetStringXmlForPromoData(DataTable dt, string xmlColumnName)
        {
            XmlDocument mainDoc = new XmlDocument();
            XmlElement root = mainDoc.CreateElement("schemeTemplates");
            mainDoc.AppendChild(root);
            XmlDocument schemeDoc;
            foreach (DataRow dr in dt.Rows)
            {
                schemeDoc = new XmlDocument();
                schemeDoc.LoadXml(dr[xmlColumnName].ToString());
                mainDoc.ChildNodes[0].InnerXml += schemeDoc.InnerXml;
            }
            return mainDoc.InnerXml;
        }

        public int SavePromoData(string promoXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(promoXml);
            int promoID = Convert.ToInt32(doc.SelectNodes("//schemeTemplate")[0].Attributes["promoid"].Value);
            int schemeTemplateID = Convert.ToInt32(doc.SelectNodes("//schemeTemplate")[0].Attributes["id"].Value);
            string promoName = doc.SelectNodes("//f5/filldata/uidata[@seqid=1]/uivalue")[0].InnerText;
            string promoDesc = doc.SelectNodes("//f5/filldata/uidata[@seqid=2]/uivalue")[0].InnerText;
            return DALFactory.Instance.SchemeDAO.SavePromoData(promoXml, promoName, promoDesc, schemeTemplateID, promoID);
        }

        public string GetAllPromos()
        {
            DataSet ds = DALFactory.Instance.SchemeDAO.GetAllPromos();
            //Change promoid attr of the promoxml
            DataTable dt = ds.Tables[0];
            foreach(DataRow row in dt.Rows) 
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(row["promoXml"].ToString());
                doc.SelectSingleNode("//schemeTemplate").Attributes["promoid"].Value = row["promoid"].ToString();
                row["promoxml"] = doc.InnerXml;
            }
            return GetStringXmlForPromoData(dt, "promoXml");
        }
    }
}
