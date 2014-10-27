using System.Data;
using System.Xml;
using DAL;

namespace BAL
{
    public class CustomerBAO
    {
        public string GetAllCustomers(int BranchID)
        {
            DataSet ds = DALFactory.Instance.CustomerDAO.GetAllCustomers(BranchID);
            return GetStringForAllCustomers(ds);
        }

        private string GetStringForAllCustomers(DataSet ds)
        {
            XmlDocument mainDoc = new XmlDocument();
            XmlElement mainEle = mainDoc.CreateElement("customers");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                XmlElement customer = mainDoc.CreateElement("customer");
                customer.SetAttribute("id", row["CustomerID"].ToString());

                XmlElement ele = mainDoc.CreateElement("name");
                ele.InnerText = row["Name"].ToString();
                customer.AppendChild(ele);

                ele = mainDoc.CreateElement("phone");
                ele.InnerText = row["Phone"].ToString();
                customer.AppendChild(ele);

                ele = mainDoc.CreateElement("address");
                ele.InnerText = row["Address"].ToString();
                customer.AppendChild(ele);

                ele = mainDoc.CreateElement("priority");
                ele.InnerText = row["Priority"].ToString();
                customer.AppendChild(ele);

                ele = mainDoc.CreateElement("remarks");
                ele.InnerText = row["Remarks"].ToString();
                customer.AppendChild(ele);

                ele = mainDoc.CreateElement("discount");
                ele.InnerText = row["Discount"].ToString();
                customer.AppendChild(ele);

                mainEle.AppendChild(customer);
            }
            mainDoc.AppendChild(mainEle);
            return mainDoc.InnerXml;
        }
    }
}