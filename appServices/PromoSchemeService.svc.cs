using System.Web;
using BAL;

namespace appServices
{
    public class PromoSchemeService : IPromoSchemeService
    {
        public string GetAllTemplatesData()
        {
            return null;
            //return BALFactory.Instance.SchemeBAO.GetAllTemplatesData();
        }

        public string GetAllProcesses()
        {
            return BALFactory.Instance.ProcessBAO.GetAllProcesses(SessionHelper.BranchID);
        }

        public string GetAllItems()
        {
            return BALFactory.Instance.ItemBAO.GetAllItems(SessionHelper.BranchID);
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
            return BALFactory.Instance.ItemBAO.GetAllCategories(SessionHelper.BranchID);
        }

        public string GetAllPatterns()
        {
            return BALFactory.Instance.ItemBAO.GetAllPatterns(SessionHelper.BranchID);
        }

        public string GetAllColors()
        {
            return BALFactory.Instance.ItemBAO.GetAllColors(SessionHelper.BranchID);
        }

        public string GetAllBrands()
        {
            return BALFactory.Instance.ItemBAO.GetAllBrands(SessionHelper.BranchID);
        }

        public string GetAllVariations()
        {
            return BALFactory.Instance.ItemBAO.GetAllVariations(SessionHelper.BranchID);
        }

        public string GetAllComments()
        {
            return BALFactory.Instance.ItemBAO.GetAllComments(SessionHelper.BranchID);
        }

        public string GetLineItemTemplate()
        {
            return BALFactory.Instance.ItemBAO.GetLineItemTemplate(SessionHelper.BranchID);
        }

        public void SaveFeedback(string feedbackXml)
        {
            BALFactory.Instance.ItemBAO.SaveFeedback(HttpUtility.UrlDecode(feedbackXml));
        }

        public string GetReceiptHeaderTemplate()
        {
            return BALFactory.Instance.ItemBAO.GetReceiptHeaderTemplate(SessionHelper.BranchID);
        }

        public string GetAllCustomers()
        {
            return BALFactory.Instance.CustomerBAO.GetAllCustomers(SessionHelper.BranchID);
        }

        public string GetAllDefaults()
        {
            return BALFactory.Instance.BokingBAO.GetAllDefaults(SessionHelper.BranchID);
        }

        public string GetPriceList()
        {
            return BALFactory.Instance.ItemBAO.GetPriceList(SessionHelper.BranchID);
        }

        public int SaveBooking(string bookingXml)
        {
            return BALFactory.Instance.BokingBAO.SaveBooking(HttpUtility.UrlDecode(bookingXml), SessionHelper.BranchID);
        }

        public string GetBookingXml(int bookingNumber)
        {
            return BALFactory.Instance.BokingBAO.GetBookingXml(bookingNumber, SessionHelper.BranchID);
        }
    }
}