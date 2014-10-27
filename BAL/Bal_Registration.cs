using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BAL
{
    class Bal_Registration
    {
        public DataSet BindDropDown()
        {
            return DAL.DALFactory.Instance.Dal_Registration.BindDropDown();
        }

        public DataSet GetStatus(string BID, string UserName, string Password)
        {
            return DAL.DALFactory.Instance.Dal_Registration.GetStatus(BID, UserName, Password);
        
        }
        public string UpdatePassword(string BID, string UserName, string Password, string NewPassword)
        {
            return DAL.DALFactory.Instance.Dal_Registration.UpdatePassword(BID, UserName, Password, NewPassword);
        }
        public DataSet FindCustomerName(string BID,string CustID)
        { 
        return DAL.DALFactory.Instance.Dal_Registration.FindCustomerName(BID,CustID);
        }

       public DataSet BindGridCustomerSearch(string BID, string CustName)
        {
            return DAL.DALFactory.Instance.Dal_Registration.BindGridCustomerSearch(BID, CustName);
        
        }
       public DataSet BindDeliveryStatus(string BookingNumber, string BID)
       {
           return DAL.DALFactory.Instance.Dal_Registration.BindDeliveryStatus(BookingNumber, BID);
       }
        

    }
    }
}
