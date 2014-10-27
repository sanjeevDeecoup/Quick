using System.ServiceModel;

namespace appServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPromoSchemeService" in both code and config file together.
    [ServiceContract]
    public interface IPromoSchemeService
    {
        [OperationContract]
        string GetAllTemplatesData();

        [OperationContract]
        string GetAllProcesses();

        [OperationContract]
        string GetAllItems();

        [OperationContract]
        int SavePromoData(string promoXml);

        [OperationContract]
        string GetAllPromos();

        [OperationContract]
        string GetAllCategories();

        [OperationContract]
        string GetAllPatterns();

        [OperationContract]
        string GetAllColors();

        [OperationContract]
        string GetAllBrands();

        [OperationContract]
        string GetAllVariations();

        [OperationContract]
        string GetAllComments();

        [OperationContract]
        string GetLineItemTemplate();

        [OperationContract]
        void SaveFeedback(string feedbackXml);

        [OperationContract]
        string GetReceiptHeaderTemplate();

        [OperationContract]
        string GetAllCustomers();

        [OperationContract]
        string GetAllDefaults();

        [OperationContract]
        string GetPriceList();

        [OperationContract]
        int SaveBooking(string bookingXml);

        [OperationContract]
        string GetBookingXml(int bookingNumber);
    }
}