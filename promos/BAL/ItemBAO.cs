using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Xml;
using System.Configuration;

namespace BAL
{
    public class ItemBAO
    {
        public string GetAllItems()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllItems();
            return GetStringForAllItems(ds);
        }
        private string GetStringForAllItems(DataSet ds)
        {
            string imagePath = ConfigurationManager.AppSettings["itemImagePath"];
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("items");
            int j = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                j++;
                XmlElement item = doc.CreateElement("item");
                item.SetAttribute("id", row["ItemID"].ToString());
                //item.SetAttribute("catid", j.ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["ItemName"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Code");
                ele.InnerText = row["ItemCode"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Image");
                ele.InnerText = imagePath + row["ItemImage"].ToString();
                item.AppendChild(ele);

                //Add Categories
                item.SetAttribute("catids", GetItemDetailsText(ds.Tables[1], Convert.ToInt32(row["ItemID"]), "CategoryID"));

                //Add Variations
                item.SetAttribute("varids", GetItemDetailsText(ds.Tables[2], Convert.ToInt32(row["ItemID"]), "VariationID"));

                //Add SubItems
                item.SetAttribute("subitemids", GetItemDetailsText(ds.Tables[3], Convert.ToInt32(row["ItemID"]), "SubItemRefID"));

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        private string GetItemDetailsText(DataTable table, int itemID, string fieldName)
        {
            string retStr = "";
            string sep1 = ConfigurationManager.AppSettings["sep1"];
            string sep2 = ConfigurationManager.AppSettings["sep2"];
            int count = (from row in table.AsEnumerable()
                         where (int)row["ItemID"] == itemID
                         select row).Count();
            if (count > 0)
            {
                retStr = (from row in table.AsEnumerable()
                          where (int)row["ItemID"] == itemID
                          select row[fieldName].ToString() + sep1 + row["DefaultSelected"].ToString()
                        ).ToList().Aggregate((i, j) => i + sep2 + j);
            }
            return retStr;

        }
        public string GetAllCategories()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllCategories();
            string imagePath = ConfigurationManager.AppSettings["categoryImagePath"];
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("categories");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement item = doc.CreateElement("category");
                item.SetAttribute("id", row["CategoryID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["CategoryName"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Code");
                ele.InnerText = row["CategoryCode"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Image");
                ele.InnerText = imagePath + row["CategoryImage"].ToString();
                item.AppendChild(ele);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        public string GetAllPatterns()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllPatterns();
            string imagePath = ConfigurationManager.AppSettings["patternImagePath"];
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("patterns");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement item = doc.CreateElement("pattern");
                item.SetAttribute("id", row["PatternID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["PatternName"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Code");
                ele.InnerText = row["PatternCode"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Image");
                ele.InnerText = imagePath + row["PatternImage"].ToString();
                item.AppendChild(ele);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        public string GetAllColors()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllColors();
            string imagePath = ConfigurationManager.AppSettings["colorImagePath"];
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("colors");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement item = doc.CreateElement("color");
                item.SetAttribute("id", row["ColorID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["ColorName"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Code");
                ele.InnerText = row["ColorCode"].ToString();
                item.AppendChild(ele);

                ele = doc.CreateElement("Image");
                ele.InnerText = imagePath + row["ColorImage"].ToString();
                item.AppendChild(ele);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        public string GetAllBrands()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllBrands();
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("brands");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement item = doc.CreateElement("brand");
                item.SetAttribute("id", row["BrandID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["BrandName"].ToString();
                item.AppendChild(ele);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        public string GetAllVariations()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllVariations();
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("variations");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement item = doc.CreateElement("variation");
                item.SetAttribute("id", row["VariationID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["VariationName"].ToString();
                item.AppendChild(ele);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        public string GetAllComments()
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllComments();
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("comments");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement item = doc.CreateElement("comment");
                item.SetAttribute("id", row["CommentID"].ToString());

                XmlElement ele = doc.CreateElement("Name");
                ele.InnerText = row["CommentName"].ToString();
                item.AppendChild(ele);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        public string GetLineItemTemplate()
        {
            return "<lineItem sequence=\"1\">"
                       + "<item id=\"0\">"
                        + "<quantity></quantity>"
                        + "<length></length>"
                        + "<breadth></breadth>"
                        + "<area1></area1>"
                        + "<remarks></remarks>"
                        + "<brand></brand>"
                        + "<text></text>"
                      + "</item>"
                      + "<patterns>"
                        + "<pattern id=\"0\">"
                          + "<text></text>"
                        + "</pattern>"
                      + "</patterns>"
                      + "<colors>"
                        + "<color id=\"0\">"
                          + "<text></text>"
                        + "</color>"
                      + "</colors>"
                      + "<subItems>"
                        + "<subItem id=\"0\">"
                          + "<text></text>"
                        + "</subItem>"
                      + "</subItems>"
                      + "<categories>"
                        + "<category id=\"0\">"
                          + "<text></text>"
                        + "</category>"
                      + "</categories>"
                      + "<brands>"
                        + "<brand id=\"0\">"
                          + "<text></text>"
                        + "</brand>"
                      + "</brands>"
                      + "<variations>"
                        + "<variation id=\"0\">"
                          + "<text></text>"
                        + "</variation>"
                      + "</variations>"
                      + "<comments>"
                        + "<comment id=\"0\">"
                          + "<text></text>"
                        + "</comment>"
                      + "</comments>"                         
                      + "<processes>"
                        + "<process id=\"0\">"
                          + "<rate></rate>"
                          + "<amount></amount>"
                          + "<text></text>"
                        + "</process>"
                      + "</processes>"
                    + "</lineItem>";
        }

        public string GetReceiptHeaderTemplate()
        {
            return "<receiptheader>" +
                      "<iswalkin>" +
                        "<bookingnumber></bookingnumber>" +
                      "</iswalkin>" +
                      "<ishomebooking>" +
                        "<homeeceiptnumber></homeeceiptnumber>" +
                      "</ishomebooking>" +
                      "<customerid></customerid>" +
                      "<duedate></duedate>" +
                      "<duetime></duetime>" +
                      "<isurgent></isurgent>" +
                      "<issms></issms>" +
                      "<isemail></isemail>" +
                      "<remarks></remarks>" +
                      "<salesman></salesman>" +
                      "<checkedby></checkedby>" +
                      "<quantity></quantity>" +
                      "<totalgrossamount></totalgrossamount>" +
                      "<totaldiscount></totaldiscount>" +
                      "<totaltax></totaltax>" +
                      "<totaladvance></totaladvance>" +
                    "</receiptheader>";
        }

        public void SaveFeedback(string feedbackXml)
        {
            
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(feedbackXml);
            //string name = doc.SelectSingleNode("//name").InnerText;
            //string feedback = doc.SelectSingleNode("//feedback").InnerText;
            int index = feedbackXml.IndexOf("~~||~~",0);
            DALFactory.Instance.ItemDAO.SaveFeedback(feedbackXml.Substring(0, index), feedbackXml.Substring(index));
        }
    }
}
