using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DTO;

namespace DAL
{
    public class BookingDAO
    {
        public DataSet GetAllDefaults(int BranchID)
        {
            SqlCommand com = SqlHelper.CreateSqlCommand("usp_GetBookingDefaults");
            com.Parameters.Add(new SqlParameter("@BranchID", BranchID));
            return (DataSet)SqlHelper.ExecuteStoredProc(com);
        }

        public int SaveBooking(string bookingXml, BookingReceipt receipt, int BranchID)
        {
            int retVal = 0;
            try
            {
                //Receipt Header
                int temp = SaveReceiptHeader(bookingXml, receipt.ReceiptHeader);
                if (receipt.ReceiptHeader.BookingID == 0 && temp > 0)
                {
                    receipt.ReceiptHeader.BookingID = temp;
                }
                //Insert LineItems
                SaveLineItems(receipt.LineItems, receipt.ReceiptHeader.BookingID);
                //Update Identity values in Xml
                bookingXml = UpdateIdentityValuesInXml(bookingXml, receipt);
                SaveBookingXml(bookingXml, receipt.ReceiptHeader.BookingID);
                //Manoj to add Old booking structure code

                retVal = InitializeNewBooking();
            }
            catch (Exception e)
            {
                retVal = -1;
            }
            return retVal;
        }

        private int InitializeNewBooking()
        {
            SqlCommand com = SqlHelper.CreateSqlCommand("usp_InitNewBooking");
            com.Parameters.Add(new SqlParameter("@Flag", FlagType.Insert));
            return (int)SqlHelper.ExecuteStoredProc(com);
        }

        private void SaveBookingXml(string bookingXml, int bookingID)
        {
            SqlCommand com = SqlHelper.CreateSqlCommand("usp_UpdateBookingXml");
            com.Parameters.Add(new SqlParameter("@Flag", FlagType.Update));
            com.Parameters.Add(new SqlParameter("@BookingXml", bookingXml));
            com.Parameters.Add(new SqlParameter("@BookingID", bookingID));
            SqlHelper.ExecuteStoredProc(com);
        }

        private string UpdateIdentityValuesInXml(string bookingXml, BookingReceipt receipt)
        {
            string retVal = bookingXml;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(bookingXml);
            //BookingID
            SetIdentityValue(xml, "//receiptheader", receipt.ReceiptHeader.BookingID);
            //xml.SelectSingleNode("//receiptheader").Attributes["identity"].Value = receipt.ReceiptHeader.BookingID.ToString();
            //LineItems
            foreach (BookingLineItem lineItem in receipt.LineItems)
            {
                //BookingItemID
                string lineItemPath = "//lineitems/lineItem[@sequence='" + lineItem.ItemDetails.Sequence.ToString() + "']";
                //Item
                string tempPath = "/item[@id='" + lineItem.ItemDetails.ItemID.ToString() + "']";
                SetIdentityValue(xml, lineItemPath + tempPath, lineItem.ItemDetails.BookingItemID);

                //Patterns
                foreach (Booking_Items_Patterns pattern in lineItem.Patterns)
                {
                    tempPath = "/patterns/pattern[@id='" + pattern.PatternID.ToString() + "']";
                    SetIdentityValue(xml, lineItemPath + tempPath, pattern.Booking_ItemPatternID);
                }
                //Colors
                foreach (Booking_Items_Colors color in lineItem.Colors)
                {
                    tempPath = "/colors/color[@id='" + color.ColorID.ToString() + "']";
                    SetIdentityValue(xml, lineItemPath + tempPath, color.Booking_ItemColorID);
                }
                //SubItems
                foreach (Booking_Items_SubItems subItem in lineItem.SubItems)
                {
                    tempPath = "/subItems/subItem[@id='" + subItem.SubItemID.ToString() + "']";
                    SetIdentityValue(xml, lineItemPath + tempPath, subItem.Booking_ItemSubItemID);
                }
                //Brands
                foreach (Booking_Items_Brands brand in lineItem.Brands)
                {
                    tempPath = "/brands/brand[@id='" + brand.BrandID.ToString() + "']";
                    SetIdentityValue(xml, lineItemPath + tempPath, brand.Booking_ItemBrandID);
                }
                //Comments
                foreach (Booking_Items_Comments comment in lineItem.Comments)
                {
                    tempPath = "/comments/comment[@id='" + comment.CommentID.ToString() + "']";
                    SetIdentityValue(xml, lineItemPath + tempPath, comment.Booking_ItemCommentID);
                }
                //Processes
                foreach (Booking_Items_Processes process in lineItem.Processes)
                {
                    tempPath = "/processes/process[@id='" + process.ProcessID.ToString() + "']";
                    SetIdentityValue(xml, lineItemPath + tempPath, process.Booking_ItemProcessID);
                }
            }
            retVal = xml.OuterXml;
            return retVal;
        }

        private void SetIdentityValue(XmlDocument xml, string nodePath, int value)
        {
            xml.SelectSingleNode(nodePath).Attributes["identity"].Value = value.ToString();
            if (xml.SelectNodes(nodePath).Count > 1)
            {
            }
        }

        private int SaveReceiptHeader(string bookingXml, BookingReceiptHeader bookingReceiptHeader)
        {
            return DALFactory.Instance.DAL_Booking.SaveBookingData(bookingXml, bookingReceiptHeader);
        }

        private void SaveLineItems(List<BookingLineItem> lineItems, int bookingID)
        {
            foreach (BookingLineItem lineItem in lineItems)
            {
                //Item Details
                if (bookingID != null && bookingID > 0)
                {
                    lineItem.ItemDetails.BookingID = bookingID;
                }
                int temp = DALFactory.Instance.DAL_Booking_Items.SaveBookingItemsData(lineItem.ItemDetails);
                if (lineItem.ItemDetails.BookingItemID == 0 && temp > 0)
                {
                    lineItem.ItemDetails.BookingItemID = temp;
                }
                int bookingItemID = lineItem.ItemDetails.BookingItemID;
                //Patterns
                SavePatterns(lineItem.Patterns, bookingItemID);
                //Colors
                SaveColors(lineItem.Colors, bookingItemID);
                //SubItems
                SaveSubItems(lineItem.SubItems, bookingItemID);
                //Brands
                SaveBrands(lineItem.Brands, bookingItemID);
                //Comments
                SaveComments(lineItem.Comments, bookingItemID);
                //Processes
                SaveProcesses(lineItem.Processes, bookingItemID);
            }
        }

        private void SavePatterns(List<Booking_Items_Patterns> patterns, int bookingItemID)
        {
            foreach (Booking_Items_Patterns pattern in patterns)
            {
                if (bookingItemID != null && bookingItemID > 0)
                {
                    pattern.BookingItemID = bookingItemID;
                }
                int temp = DALFactory.Instance.DAL_Booking_Patterns.SaveBookingItemPatterns(pattern);
                if (pattern.Booking_ItemPatternID == 0 && temp > 0)
                {
                    pattern.Booking_ItemPatternID = temp;
                }
            }
        }

        private void SaveColors(List<Booking_Items_Colors> colors, int bookingItemID)
        {
            foreach (Booking_Items_Colors color in colors)
            {
                if (bookingItemID != null && bookingItemID > 0)
                {
                    color.BookingItemID = bookingItemID;
                }
                int temp = DALFactory.Instance.DAL_BookingItem_Colors.SaveBookingItemColors(color);
                if (color.Booking_ItemColorID == 0 && temp > 0)
                {
                    color.Booking_ItemColorID = temp;
                }
            }
        }

        private void SaveSubItems(List<Booking_Items_SubItems> subItems, int bookingItemID)
        {
            foreach (Booking_Items_SubItems subItem in subItems)
            {
                if (bookingItemID != null && bookingItemID > 0)
                {
                    subItem.BookingItemID = bookingItemID;
                }
                int temp = DALFactory.Instance.DAL_BookingItem_SubItems.SaveBookingItem_SubItems(subItem);
                if (subItem.Booking_ItemSubItemID == 0 && temp > 0)
                {
                    subItem.Booking_ItemSubItemID = temp;
                }
            }
        }

        private void SaveBrands(List<Booking_Items_Brands> brands, int bookingItemID)
        {
            foreach (Booking_Items_Brands brand in brands)
            {
                if (bookingItemID != null && bookingItemID > 0)
                {
                    brand.BookingItemID = bookingItemID;
                }
                int temp = DALFactory.Instance.DAL_Booking_ItemBrands.SaveBookingItemBrand(brand);
                if (brand.Booking_ItemBrandID == 0 && temp > 0)
                {
                    brand.Booking_ItemBrandID = temp;
                }
            }
        }

        private void SaveProcesses(List<Booking_Items_Processes> processes, int bookingItemID)
        {
            foreach (Booking_Items_Processes process in processes)
            {
                if (bookingItemID != null && bookingItemID > 0)
                {
                    process.BookingItemID = bookingItemID;
                }
                int temp = DALFactory.Instance.DAL_Booking_Items_Processes.SaveBookingItemsData(process);
                if (process.Booking_ItemProcessID == 0 && temp > 0)
                {
                    process.Booking_ItemProcessID = temp;
                }
            }
        }

        private void SaveComments(List<Booking_Items_Comments> comments, int bookingItemID)
        {
            foreach (Booking_Items_Comments comment in comments)
            {
                if (bookingItemID != null && bookingItemID > 0)
                {
                    comment.BookingItemID = bookingItemID;
                }
                int temp = DALFactory.Instance.DAL_BookingItemComment.SaveBookingItemComments(comment);
                if (comment.Booking_ItemCommentID == 0 && temp > 0)
                {
                    comment.Booking_ItemCommentID = temp;
                }
            }
        }

        public DataSet GetBookingXml(int bookingNumber, int branchID)
        {
            SqlCommand com = SqlHelper.CreateSqlCommand("usp_GetBookingXml");
            com.Parameters.Add(new SqlParameter("@Flag", FlagType.Select));
            com.Parameters.Add(new SqlParameter("@BranchID", branchID));
            com.Parameters.Add(new SqlParameter("@BookingNumber", bookingNumber));
            return (DataSet)SqlHelper.ExecuteStoredProc(com);
        }
    }
}