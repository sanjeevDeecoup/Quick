using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace DAL
{
    public class DAL_Booking
    {
        public int SaveBookingData(string bookingXml, DTO.Booking Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking");
            FlagType flag;
            if (Ob.BookingID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_Booking(cmd, flag, Ob);
            cmd.Parameters.Add(new SqlParameter("@booking_Xml", bookingXml));
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_Booking(SqlCommand cmd, FlagType flag, DTO.Booking Ob)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@BookingNumber", Ob.BookingNumber));
            cmd.Parameters.Add(new SqlParameter("@IsHomeReceipt", Ob.IsHomeReceipt));
            cmd.Parameters.Add(new SqlParameter("@HomeReceiptNumber", Ob.HomeReceiptNumber));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", Ob.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@DueDate", Ob.DueDate));
            cmd.Parameters.Add(new SqlParameter("@DueTime", Ob.DueTime));
            cmd.Parameters.Add(new SqlParameter("@IsUrgent", Ob.IsUrgent));
            cmd.Parameters.Add(new SqlParameter("@IsSMS", Ob.IsSMS));
            cmd.Parameters.Add(new SqlParameter("@IsEmail", Ob.IsEmail));
            cmd.Parameters.Add(new SqlParameter("@ReceiptRemarks", Ob.ReceiptRemarks));
            cmd.Parameters.Add(new SqlParameter("@SalesManUserID", Ob.SalesManUserID));
            cmd.Parameters.Add(new SqlParameter("@CheckedByUserID", Ob.CheckedByUserID));
            cmd.Parameters.Add(new SqlParameter("@TotalGrossAmount", Ob.TotalGrossAmount));
            cmd.Parameters.Add(new SqlParameter("@TotalDiscount", Ob.TotalDiscount));
            cmd.Parameters.Add(new SqlParameter("@TotalTax", Ob.TotalTax));
            cmd.Parameters.Add(new SqlParameter("@TotalAdvance", Ob.TotalAdvance));
            cmd.Parameters.Add(new SqlParameter("@ReceiptStatus", Ob.ReceiptStatus));
            cmd.Parameters.Add(new SqlParameter("@Active", Ob.CommonFields.Active));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", Ob.CommonFields.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateModified", Ob.CommonFields.DateModified));
            cmd.Parameters.Add(new SqlParameter("@CreatedBy", Ob.CommonFields.CreatedBy));
            cmd.Parameters.Add(new SqlParameter("@ModifiedBy", Ob.CommonFields.ModifiedBy));
            cmd.Parameters.Add(new SqlParameter("@BranchId", Ob.CommonFields.BranchId));
            cmd.Parameters.Add(new SqlParameter("@XMLName", Ob.XMLName));
            return cmd;
        }

        public DataSet GetDS(DTO.Booking Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking");
            cmd = AddParameters_Booking(cmd, FlagType.Select, Ob);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }

        public int DeleteBooking(DTO.Booking Ob)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_Booking");
            FlagType flag;
            flag = FlagType.Delete;
            cmd = AddParameters_Booking(cmd, flag, Ob);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        public void SaveFeedback(string name, string feedback)
        {
            XmlDocument doc = new XmlDocument();

            //doc.Load(Server.MapPath("~/App_Data/1lineitem.xml"));
            XmlNode root = doc.DocumentElement;
        }
    }
}