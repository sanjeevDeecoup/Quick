using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using DTO;

namespace BAL
{
    public class BookingReceiptTransformer
    {
        private XmlDocument _bookingXml;
        private int _branchID;
        private int _bookingItemID;
        private int _bookingID;

        public BookingReceiptTransformer(string bookingXmlString, int branchID)
        {
            _bookingXml = new XmlDocument();
            _bookingXml.LoadXml(bookingXmlString);
            _branchID = branchID;
        }

        public BookingReceipt Transform()
        {
            BookingReceipt _objReceipt = new BookingReceipt();
            _objReceipt.ReceiptHeader = PopulateReceiptHeader();
            _objReceipt.LineItems = PopulateLineItems();
            return _objReceipt;
        }

        private List<BookingLineItem> PopulateLineItems()
        {
            List<BookingLineItem> objLineItems = new List<BookingLineItem>();
            XmlNodeList lineitems = _bookingXml.SelectNodes("//lineItem");
            foreach (XmlNode lineItem in lineitems)
            {
                BookingLineItem objLineItem = GetLineItem(lineItem);
                objLineItems.Add(objLineItem);
            }
            return objLineItems;
        }

        private BookingLineItem GetLineItem(XmlNode lineItem)
        {
            BookingLineItem obj = new BookingLineItem();
            //Item Details
            obj.ItemDetails = GetItemDetails(lineItem);
            //Patterns
            obj.Patterns = GetPatterns(lineItem);
            //Colors
            obj.Colors = GetColors(lineItem);
            //Brands
            obj.Brands = GetBrands(lineItem);
            //Comments
            obj.Comments = GetComments(lineItem);
            //SubItems
            obj.SubItems = GetSubItems(lineItem);
            //Processes
            obj.Processes = GetProcesses(lineItem);
            return obj;
        }

        private List<Booking_Items_Patterns> GetPatterns(XmlNode lineItem)
        {
            List<Booking_Items_Patterns> objPatterns = new List<Booking_Items_Patterns>();
            XmlNodeList patterns = lineItem.SelectNodes("patterns/pattern");
            foreach (XmlNode pattern in patterns)
            {
                string patternId = pattern.Attributes["id"].Value;
                if (patternId != "" && patternId != null && patternId != "0")
                {
                    Booking_Items_Patterns obj = new Booking_Items_Patterns();
                    obj.PatternID = Convert.ToInt32(patternId);
                    obj.Booking_ItemPatternID = GetIntValue(pattern, "@identity");
                    obj.BookingItemID = _bookingItemID;
                    PopulateCommonFields(obj.CommonFields);
                    objPatterns.Add(obj);
                }
            }
            return objPatterns;
        }

        private List<Booking_Items_Colors> GetColors(XmlNode lineItem)
        {
            List<Booking_Items_Colors> objColors = new List<Booking_Items_Colors>();
            XmlNodeList colors = lineItem.SelectNodes("colors/color");
            foreach (XmlNode color in colors)
            {
                string colorId = color.Attributes["id"].Value;
                if (colorId != "" && colorId != null && colorId != "0")
                {
                    Booking_Items_Colors obj = new Booking_Items_Colors();
                    obj.ColorID = Convert.ToInt32(colorId);
                    obj.Booking_ItemColorID = GetIntValue(color, "@identity");
                    obj.BookingItemID = _bookingItemID;
                    PopulateCommonFields(obj.CommonFields);
                    objColors.Add(obj);
                }
            }
            return objColors;
        }

        private List<Booking_Items_Brands> GetBrands(XmlNode lineItem)
        {
            List<Booking_Items_Brands> objBrands = new List<Booking_Items_Brands>();
            XmlNodeList brands = lineItem.SelectNodes("brands/brand");
            foreach (XmlNode brand in brands)
            {
                string brandId = brand.Attributes["id"].Value;
                if (brandId != "" && brandId != null && brandId != "0")
                {
                    Booking_Items_Brands obj = new Booking_Items_Brands();
                    obj.BrandID = Convert.ToInt32(brandId);
                    obj.Booking_ItemBrandID = GetIntValue(brand, "@identity");
                    obj.BookingItemID = _bookingItemID;
                    PopulateCommonFields(obj.CommonFields);
                    objBrands.Add(obj);
                }
            }
            return objBrands;
        }

        private List<Booking_Items_Comments> GetComments(XmlNode lineItem)
        {
            List<Booking_Items_Comments> objComments = new List<Booking_Items_Comments>();
            XmlNodeList comments = lineItem.SelectNodes("comments/comment");
            foreach (XmlNode comment in comments)
            {
                string commentId = comment.Attributes["id"].Value;
                if (commentId != "" && commentId != null && commentId != "0")
                {
                    Booking_Items_Comments obj = new Booking_Items_Comments();
                    obj.CommentID = Convert.ToInt32(commentId);
                    obj.Booking_ItemCommentID = GetIntValue(comment, "@identity");
                    obj.BookingItemID = _bookingItemID;
                    PopulateCommonFields(obj.CommonFields);
                    objComments.Add(obj);
                }
            }
            return objComments;
        }

        private List<Booking_Items_SubItems> GetSubItems(XmlNode lineItem)
        {
            List<Booking_Items_SubItems> objSubItems = new List<Booking_Items_SubItems>();
            XmlNodeList subItems = lineItem.SelectNodes("subItems/subItem");
            foreach (XmlNode subItem in subItems)
            {
                string subItemId = subItem.Attributes["id"].Value;
                if (subItemId != "" && subItemId != null && subItemId != "0")
                {
                    Booking_Items_SubItems obj = new Booking_Items_SubItems();
                    obj.SubItemID = Convert.ToInt32(subItemId);
                    obj.Booking_ItemSubItemID = GetIntValue(subItem, "@identity");
                    obj.BookingItemID = _bookingItemID;
                    PopulateCommonFields(obj.CommonFields);
                    objSubItems.Add(obj);
                }
            }
            return objSubItems;
        }

        private List<Booking_Items_Processes> GetProcesses(XmlNode lineItem)
        {
            List<Booking_Items_Processes> objProcesses = new List<Booking_Items_Processes>();
            XmlNodeList processes = lineItem.SelectNodes("processes/process");
            foreach (XmlNode process in processes)
            {
                string processId = process.Attributes["id"].Value;
                if (processId != null && processId != "" && processId != "0")
                {
                    Booking_Items_Processes obj = new Booking_Items_Processes();
                    obj.ProcessID = Convert.ToInt32(processId);
                    obj.ProcessRate = GetDoubleValue(process, "rate");
                    obj.Booking_ItemProcessID = GetIntValue(process, "@identity");
                    obj.BookingItemID = _bookingItemID;
                    PopulateCommonFields(obj.CommonFields);
                    objProcesses.Add(obj);
                }
            }
            return objProcesses;
        }

        private Booking_Items GetItemDetails(XmlNode lineItem)
        {
            Booking_Items obj = new Booking_Items();
            obj.ItemID = GetIntValue(lineItem, "item/@id");
            obj.SubItemCount = lineItem.SelectNodes("subItems/subItem[@id!='0' and @id!='']").Count;
            obj.ProcessCount = lineItem.SelectNodes("processes/process[@id!='0' and @id!='']").Count;
            obj.Sequence = GetIntValue(lineItem, "@sequence");
            obj.CategoryID = GetIntValue(lineItem, "categories/category/@id");
            obj.VariationId = GetIntValue(lineItem, "variations/variation/@id");
            obj.Quantity = GetIntValue(lineItem, "item/quantity");

            _bookingItemID = GetIntValue(lineItem, "item/@identity");
            obj.BookingItemID = _bookingItemID;
            obj.BookingID = _bookingID;
            PopulateCommonFields(obj.CommonFields);
            return obj;
        }

        private BookingReceiptHeader PopulateReceiptHeader()
        {
            BookingReceiptHeader obj = new BookingReceiptHeader();
            obj.BookingNumber = _bookingXml.SelectSingleNode("booking/receiptheader/iswalkin/bookingnumber").InnerText;
            _bookingID = GetIntValue(_bookingXml, "booking/receiptheader/@identity");
            obj.BookingID = _bookingID;
            if (obj.BookingNumber != "")
            {
                obj.IsHomeReceipt = 0;
                obj.HomeReceiptNumber = "";
            }
            else
            {
                obj.IsHomeReceipt = 1;
                obj.HomeReceiptNumber = _bookingXml.SelectSingleNode("booking/receiptheader/ishomebooking/homeeceiptnumber").InnerText; ;
            }
            obj.CustomerID = GetIntValue(_bookingXml, "booking/receiptheader/customerid");
            string date = _bookingXml.SelectSingleNode("booking/receiptheader/duedate").InnerText;
            DateTimeFormatInfo objFormat = new DateTimeFormatInfo();
            objFormat.ShortDatePattern = "dd-MM-yyyy";
            objFormat.DateSeparator = "-";
            obj.DueDate = Convert.ToDateTime(date.Substring(4), objFormat);
            obj.DueTime = _bookingXml.SelectSingleNode("booking/receiptheader/duetime").InnerText;
            obj.IsUrgent = GetIntValue(_bookingXml, "booking/receiptheader/isurgent");
            obj.IsSMS = GetIntValue(_bookingXml, "booking/receiptheader/issms");
            obj.IsEmail = GetIntValue(_bookingXml, "booking/receiptheader/isemail");

            obj.ReceiptRemarks = _bookingXml.SelectSingleNode("booking/receiptheader/remarks").InnerText;
            obj.SalesManUserID = GetIntValue(_bookingXml, "booking/receiptheader/salesman");
            obj.CheckedByUserID = GetIntValue(_bookingXml, "booking/receiptheader/checkedby");
            obj.ReceiptStatus = 1; //TODO - Change to Enum

            //Payment Details - TODO to be added in Payment Table
            obj.TotalGrossAmount = GetDoubleValue(_bookingXml, "booking/receiptheader/totalgrossamount");
            obj.TotalDiscount = GetDoubleValue(_bookingXml, "booking/receiptheader/totaldiscount");
            obj.TotalTax = GetDoubleValue(_bookingXml, "booking/receiptheader/totaltax");
            obj.TotalAdvance = GetDoubleValue(_bookingXml, "booking/receiptheader/totaladvance");

            PopulateCommonFields(obj.CommonFields);

            return obj;
        }

        private void PopulateCommonFields(CommonFields objCommon)
        {
            objCommon.Active = 1;
            objCommon.CreatedBy = 1; //TODO - Get user ID
            objCommon.ModifiedBy = 1; //TODO - Get user ID
            objCommon.DateCreated = DateTime.Now;
            objCommon.DateModified = DateTime.Now;
            objCommon.BranchId = _branchID.ToString();
        }

        private int GetIntValue(XmlNode node, string xpath)
        {
            int retVal = 0;
            //Read Attribute value
            string temp = node.SelectSingleNode(xpath).Value;
            //else read node's innertext
            if (temp == null)
            {
                temp = node.SelectSingleNode(xpath).InnerText;
            }
            if (temp != "")
            {
                int.TryParse(temp, out retVal);
            }
            if (node.SelectNodes(xpath).Count > 1)
            {
            }
            return retVal;
        }

        private double GetDoubleValue(XmlNode node, string xpath)
        {
            double retVal = 0;
            //Read Attribute value
            string temp = node.SelectSingleNode(xpath).Value;
            //else read node's innertext
            if (temp == null)
            {
                temp = node.SelectSingleNode(xpath).InnerText;
            }
            if (temp != "")
            {
                double.TryParse(temp, out retVal);
            }
            if (node.SelectNodes(xpath).Count > 1)
            {
            }
            return retVal;
        }
    }
}