using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using DAL;
using DTO;

namespace BAL
{
    public class ItemBAO
    {
        public string GetAllItems(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllItems(BranchID);
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

        public string GetAllCategories(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllCategories(BranchID);
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

        public string GetAllPatterns(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllPatterns(BranchID);
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

        public string GetAllColors(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllColors(BranchID);
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

        public string GetAllBrands(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllBrands(BranchID);
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

        public string GetAllVariations(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllVariations(BranchID);
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

        public string GetAllComments(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetAllComments(BranchID);
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

        public string GetLineItemTemplate(int BranchID)
        {
            return BookingLineItemTemplate.GetString();
        }

        public string GetReceiptHeaderTemplate(int BranchID)
        {
            return BookingReceiptHeaderTemplate.GetString();
        }

        public void SaveFeedback(string feedbackXml)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(feedbackXml);
            //string name = doc.SelectSingleNode("//name").InnerText;
            //string feedback = doc.SelectSingleNode("//feedback").InnerText;
            int index = feedbackXml.IndexOf("~~||~~", 0);
            DALFactory.Instance.ItemDAO.SaveFeedback(feedbackXml.Substring(0, index), feedbackXml.Substring(index));
        }

        public string GetPriceList(int BranchID)
        {
            DataSet ds = DALFactory.Instance.ItemDAO.GetPriceList(BranchID);
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("pricelist");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                //Add ItemID
                string customid = row["ItemID"].ToString();
                customid += "_" + row["CategoryID"].ToString();
                customid += "_" + row["VariationID"].ToString();
                customid += "_" + row["SubItemID"].ToString();
                customid += "_" + row["ProcessID"].ToString();
                XmlElement item = doc.CreateElement("price");
                item.SetAttribute("customid", customid);
                item.InnerText = Convert.ToDouble(row["Price"]).ToString("0.00", CultureInfo.InvariantCulture);

                root.AppendChild(item);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }
    }
}