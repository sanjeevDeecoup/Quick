using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BAL;
using System.Web;

namespace appServices
{
    public class PromoSchemeService : IPromoSchemeService
    {
        public string GetAllTemplatesData()
        {
            return BALFactory.Instance.SchemeBAO.GetAllTemplatesData();
        }

        public string GetAllProcesses()
        {
            return BALFactory.Instance.ProcessBAO.GetAllProcesses();
        }
        public string GetAllItems()
        {
            return BALFactory.Instance.ItemBAO.GetAllItems();
        }

        public int SavePromoData(string promoXml)
        {
            return BALFactory.Instance.SchemeBAO.SavePromoData(HttpUtility.UrlDecode(promoXml));
        }

        public string GetAllPromos()
        {
            return BALFactory.Instance.SchemeBAO.GetAllPromos();
        }

        public string GetAllCategories()
        {
            return BALFactory.Instance.ItemBAO.GetAllCategories();
        }

        public string GetAllPatterns()
        {
            return BALFactory.Instance.ItemBAO.GetAllPatterns();
        }

        public string GetAllColors()
        {
            return BALFactory.Instance.ItemBAO.GetAllColors();
        }

        public string GetAllBrands()
        {
            return BALFactory.Instance.ItemBAO.GetAllBrands();
        }

        public string GetAllVariations()
        {
            return BALFactory.Instance.ItemBAO.GetAllVariations();
        }

        public string GetAllComments()
        {
            return BALFactory.Instance.ItemBAO.GetAllComments();
        }

        public string GetLineItemTemplate()
        {
            return BALFactory.Instance.ItemBAO.GetLineItemTemplate();
        }

        public void SaveFeedback(string feedbackXml)
        {
            BALFactory.Instance.ItemBAO.SaveFeedback(HttpUtility.UrlDecode(feedbackXml));
        }

        public string GetReceiptHeaderTemplate()
        {
            return BALFactory.Instance.ItemBAO.GetReceiptHeaderTemplate();
        }

        public string GetAllCustomers()
        {
            return BALFactory.Instance.CustomerBAO.GetAllCustomers();
        }
    }
}
