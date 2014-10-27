using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace DAL
{
   public class DAL_NewCustomer
    {
       public int SaveNewCustomer(DTO.CustomerMaster Ob)
       {
           SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_CustomerNewMaster");
           FlagType flag;
           if (Ob.CustID  > 0 )
           {
               flag = FlagType.Update;
           }

           else
           {
               flag = FlagType.Insert;
           }
           cmd = AddParameters_Customer(cmd, flag, Ob);
           return (int)SqlHelper.ExecuteStoredProc(cmd);
       }
       public SqlCommand AddParameters_Customer(SqlCommand cmd, FlagType flag, DTO.CustomerMaster Ob)
       {
          cmd.Parameters.Add(new SqlParameter("@Flag",flag));
          cmd.Parameters.Add(new SqlParameter("@CustId", Ob.Id));
          cmd.Parameters.Add(new SqlParameter("@CustomerCode", Ob.CustomerCode));
          cmd.Parameters.Add(new SqlParameter("@CustomerSalutation", Ob.CustomerSalutation));
          cmd.Parameters.Add(new SqlParameter("@CustomerName", Ob.CustomerName));
          cmd.Parameters.Add(new SqlParameter("@CustomerAddress", Ob.CustomerAddress));
          cmd.Parameters.Add(new SqlParameter("@CustomerPhone", Ob.CustomerAddress));
          cmd.Parameters.Add(new SqlParameter("@CustomerMobile", Ob.CustomerMobile));
          cmd.Parameters.Add(new SqlParameter("@CustomerEmailId", Ob.CustomerEmailId));
          cmd.Parameters.Add(new SqlParameter("@CustomerPriority", Ob.CustomerPriority));
          cmd.Parameters.Add(new SqlParameter("@CustomerRefferredBy", Ob.CustomerRefferredBy));
          cmd.Parameters.Add(new SqlParameter("@Remarks", Ob.Remarks));
          cmd.Parameters.Add(new SqlParameter("@BirthDate", Ob.BirthDate));
          cmd.Parameters.Add(new SqlParameter("@AnniversaryDate", Ob.AnniversaryDate));
          cmd.Parameters.Add(new SqlParameter("@AreaLocation", Ob.AreaLocation));
          cmd.Parameters.Add(new SqlParameter("@DefaultDiscountRate", Ob.DefaultDiscountRate));
          cmd.Parameters.Add(new SqlParameter("BranchID", Ob.BranchId));
          return cmd;
       }
       public DataSet GetAllCustomers(DTO.CustomerMaster Ob)
       {
           SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_CustomerNewMaster");
           cmd = AddParameters_Customer(cmd, FlagType.Select, Ob);
           return (DataSet)SqlHelper.ExecuteStoredProc(cmd);

       }
       public int DeleteCustomer(DTO.CustomerMaster Ob)
       {
           SqlCommand cmd = SqlHelper.CreateSqlCommand("sp_CustomerNewMaster");
           cmd = AddParameters_Customer(cmd, FlagType.Delete, Ob);
           return (int)SqlHelper.ExecuteStoredProc(cmd);
       }
       public DataSet SearchCUstomer(DTO.CustomerMaster Ob)
       {
           SqlCommand cmd = new SqlCommand();
           DataSet ds = new DataSet();
           cmd.CommandText = "sp_CustomerNewMaster";
           cmd.CommandType = CommandType.StoredProcedure;
           if (Ob.CustomerName!= "")
           {
               cmd.Parameters.Add("@BranchId", Ob.BranchId);
               cmd.Parameters.Add("@CustomerName", Ob.CustomerName);
               cmd.Parameters.Add("@Flag", 5);
               
           }
           else
           {
               cmd.Parameters.Add("@BranchId", Ob.BranchId);
               cmd.Parameters.Add("@BookingNumber", Ob.BookingNumber);
               cmd.Parameters.Add("@flag", 4);
           }
           ds = PrjClass.GetData(cmd);
           return ds;
       }

    }
}
