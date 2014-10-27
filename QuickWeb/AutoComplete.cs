using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Collections;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AutoComplete : WebService
{
    public static string sqlConStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    private DTO.NewBooking ObNewBooking = new DTO.NewBooking();

    public AutoComplete()
    {
    }

    [WebMethod(EnableSession = true)]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod(EnableSession = true)]
    /* [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
     * can't do [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
     * because Microsoft is a little piece of SHIT, and they thought they better not
     * employ any way for AutoCompleteExtender to use GET cause it could allow security breach
     * well you know what? FUCK YOU MICROSOFT
     * ******************* FUCK YOU MICROSOFT !!!! *************************
     */
    public string[] GetCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 11);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        //Session["A"] = ds;
        //Session["B"] = prefixText;
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            var flag = 11;
            switch (contextKey)
            {
                case "All": flag = 11;
                    break;

                case "Name": flag = 59;
                    break;

                case "Address": flag = 60;
                    break;

                case "Mobile": flag = 61;
                    break;

                case "Email": flag = 62;
                    break;

                case "MembershipId": flag = 76;
                    break;

                case "CustCode": flag = 77;
                    break;

                default: flag = 11;
                    break;
            }
            cmd.Parameters.AddWithValue("@Flag", flag);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        //Session["A"] = ds;
        //Session["B"] = prefixText;
        return items.ToArray();
    }

    // method to populate city list
    [WebMethod(EnableSession = true)]
    public string[] GetCityCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 34);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    // method to populate area list
    [WebMethod(EnableSession = true)]
    public string[] GetAreaCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 35);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    // method to populate cutomer profession list
    [WebMethod(EnableSession = true)]
    public string[] GetCustProessionCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 36);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    // Redudent Method, not used till now
    [WebMethod(EnableSession = true)]
    public string[] GetCompletionListForBooking(string prefixText)
    {
        List<string> items = new List<string>(10);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 11);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        //Session["A"] = ds;
        //Session["B"] = prefixText;
        return items.ToArray();
    }

    // Bind Method Item List
    [WebMethod(EnableSession = true)]
    public string[] GetCompletionListForItem(string prefixText)
    {
        List<string> items = new List<string>(10);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustomerName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 48);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        //Session["A"] = ds;
        //Session["B"] = prefixText;
        return items.ToArray();
    }

    // gets the item rate for selected item and process
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetItemRateForProcess(string arg, int rateListId)
    {
        double rate = 0;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        cmd.CommandText = "sp_NewBooking";
        try
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", arg.Split(':')[0]);
            cmd.Parameters.AddWithValue("@ProcessName", arg.Split(':')[1]);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", arg.Split(':').Count() == 2 ? 17 : 63);
            cmd.Parameters.AddWithValue("@RateListId", rateListId);
            sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
                return sdr.GetValue(0).ToString();
            // Convert.ToDouble(sdr.GetValue(0));
            else
                rate = 0;
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        return rate.ToString();
    }

    // Gets the Name, address, Priority, mobileNo and remark for selected customer
    // And now, we have also added comm pref and email, yeah that sucks!
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetPriorityAndRemarks(string arg)
    {
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustCode", arg);
            cmd.Parameters.AddWithValue("@Flag", 31);
            var pendingRemarks = BAL.BALFactory.Instance.Bal_Report.PendingReceiptParticularCustomer(arg, Globals.BranchID);
            var pendingOrder = BAL.BALFactory.Instance.Bal_Report.PendingOrderParticularCustomer(arg, Globals.BranchID);
            var priorityNRemarks = PrjClass.ExecuteScalar(cmd);
            var returnVal = priorityNRemarks + ":" + pendingRemarks;
            var sqlCommand = new SqlCommand { CommandText = "SELECT RateListId FROM CustomerMaster WHERE BranchId=" + Globals.BranchID + " AND CustomerCode = '" + arg + "'", CommandType = CommandType.Text };
            var rateListId = PrjClass.ExecuteScalar(sqlCommand);
            return returnVal + ":" + rateListId + ":" + pendingOrder;
        }
        catch (Exception)
        {
            return "";
        }
    }

    // Gets the Name, address, Priority, mobileNo and remark for selected customer
    // And now, we have also added comm pref and email, yeah that sucks!
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string SetCustomerDetailInCustomerScreen(string arg)
    {
        try
        {           
            ArrayList date = new ArrayList();
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@CustCode", arg);
            cmd.Parameters.AddWithValue("@CurrentDate", date[0].ToString());
            cmd.Parameters.AddWithValue("@Flag", 78);           
            var priorityNRemarks = PrjClass.ExecuteScalar(cmd);
            return priorityNRemarks;
        }
        catch (Exception)
        {
            return "";
        }
    }

    //Reset Website User's Password 
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string ResetWebsitePassword(string arg)
    {
        string res = string.Empty;
        try
        {
            res = BAL.BALFactory.Instance.BL_CustomerMaster.ResestWebsite(arg, Globals.BranchID);
        }
        catch (Exception ex)
        {
            res = ex.Message.ToString();
        }
        return res;
    }

    // Update the customer through J Query
    // bcoz cs code response very slow
    [WebMethod(EnableSession = true)]    
    public string UpdateCustomerJquery(string CustomerCode, string CustomerSalutation, string CustomerName, string CustomerAddress, string CustomerPhone, string CustomerMobile, string CustomerEmailId, string CustomerPriority, string CustomerRefferredBy, string DefaultDiscountRate, string Remarks, string BirthDate, string AnniversaryDate, string AreaLocation, string CommunicationMeans, string MemberShipId, string BarCode, string RateListId, bool IsWebsite, string tempCustMobile, string tempMemberShipId, string tempBarCode,string tempWebsite)
    {
        string res = string.Empty;
        try
        {
            res = BAL.BALFactory.Instance.BL_CustomerMaster.UpdateCustomerDetailJQuery(CustomerCode, CustomerSalutation, CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, CustomerEmailId, CustomerPriority, CustomerRefferredBy, DefaultDiscountRate, Remarks, BirthDate, AnniversaryDate, AreaLocation, CommunicationMeans, MemberShipId, BarCode, RateListId, IsWebsite, tempCustMobile, tempMemberShipId, tempBarCode, Globals.BranchID);

            if (IsWebsite == true)
            {
                if(tempWebsite=="False")
                {
                    GetDataforEmail(CustomerCode);
                }                
            }
        }
        catch (Exception ex)
        {
            res = ex.Message.ToString();
        }
        return res;
    }

    // Save new customer through J Query
    // bcoz cs code response very slow
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string SaveNewCustomerJquery(string CustomerSalutation, string CustomerName, string CustomerAddress, string CustomerPhone, string CustomerMobile, string CustomerEmailId, string CustomerPriority, string CustomerRefferredBy, string DefaultDiscountRate, string Remarks, string BirthDate, string AnniversaryDate, string AreaLocation, string CommunicationMeans, string MemberShipId, string BarCode, string RateListId, bool IsWebsite)
    {
        string res = string.Empty;
        try
        {
            res = BAL.BALFactory.Instance.BL_CustomerMaster.SaveNewCustomerDetailJquery(CustomerSalutation, CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, CustomerEmailId, CustomerPriority, CustomerRefferredBy, DefaultDiscountRate, Remarks, BirthDate, AnniversaryDate, AreaLocation, CommunicationMeans, MemberShipId, BarCode, RateListId, IsWebsite, Globals.BranchID);
                        
            if (IsWebsite == true)
            {
                if (res == "Record Saved")
                {
                    SqlCommand cmd = new SqlCommand();
                    DataSet ds = new DataSet();
                    cmd.CommandText = "proc_BindToMachine";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                    cmd.Parameters.AddWithValue("@Flag", 12);
                    ds = PrjClass.GetData(cmd);
                    string Custcode = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                     GetDataforEmail(Custcode);
                }
                
            }

            if (res == "Record Saved")
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 25);
                ds = PrjClass.GetData(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    res = ds.Tables[0].Rows[0]["custcode"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
            res = ex.Message.ToString();
        }
        return res;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string DeleteCustomerJquery(string CustomerCode)
    {
        string res = string.Empty;
        try
        {
            res = BAL.BALFactory.Instance.BL_CustomerMaster.DeleteCustomerDetailJQuery(CustomerCode,Globals.BranchID);
        }
        catch (Exception ex)
        {
            res = ex.Message.ToString();
        }
        return res;
    }


    // Checks if the given itemname exists this branch
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string CheckIfItemExists(string itemName)
    {
        string status = string.Empty;
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_NewBooking_SaveProc";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", itemName);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 6);
            var sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
                status = "true";
            else
                status = "false";
        }
        catch (Exception)
        {
            status = "false";
        }
        return status;
    }

    // Checks if the given process exists this branch
    [WebMethod(EnableSession = true)]
    public string CheckIfProcessExists(string ProcessName)
    {
        string status = string.Empty;
        try
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_NewBooking_SaveProc";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcessCode", ProcessName);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 7);
            var sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
                status = "true";
            else
                status = "false";
        }
        catch (Exception)
        {
            status = "false";
        }
        return status;
    }

    // adds items to master
    [WebMethod(EnableSession = true)]
    public string AddItemsToMaster(string itemName, string itemCode, int qtyOfSubItems, string[] subItems)
    {
        string returnMsg = string.Empty;

        // check if any is null, if yes return error
        if (itemName == "" || itemName == null)
            return "";

        SqlCommand CMD_Item = new SqlCommand();
        // check if qtyofsubItems is supplied and if yes, is it equal to count of subItems
        // on the other hand, we could just skip the qty para, and see is the count of string[]
        // subitems is greater than 0, then it has subitems, if no than not,
        // though not doing it now, because that won't gain that much performance difference..
        if (qtyOfSubItems == 1 || qtyOfSubItems == 0)
        {
            CMD_Item.CommandType = CommandType.Text;
            CMD_Item.CommandText = "Insert Into ItemMaster (ItemName, NumberOfSubItems,ItemCode,BranchId) Values('" + itemName + "','" + 1 + "','" + itemCode + "','" + Globals.BranchID + "')";
            returnMsg = PrjClass.ExecuteNonQuery(CMD_Item);
            if (returnMsg != "Record Saved")
                return returnMsg;

            CMD_Item.CommandText = "insert into EntSubItemDetails(ItemName,SubItemName,SubItemOrder,BranchId) Values('" + itemName + "','" + itemName + "','" + 1 + "','" + Globals.BranchID + "')";
            returnMsg = PrjClass.ExecuteNonQuery(CMD_Item);
            return returnMsg;
        }
        else if (qtyOfSubItems > 1 && qtyOfSubItems < 15)
        {
            CMD_Item.CommandType = CommandType.Text;
            CMD_Item.CommandText = "Insert Into ItemMaster (ItemName, NumberOfSubItems,ItemCode,BranchId) Values('" + itemName + "','" + qtyOfSubItems + "','" + itemCode + "','" + Globals.BranchID + "')";
            returnMsg = PrjClass.ExecuteNonQuery(CMD_Item);
            CMD_Item.CommandText = "DELETE FROM EntSubItemDetails WHERE ItemName = '" + itemName + "' AND BranchId='" + Globals.BranchID + "'";
            returnMsg = PrjClass.ExecuteNonQuery(CMD_Item);

            if (qtyOfSubItems != subItems.Count())
                returnMsg = "No. of subitems not valid";

            for (int i = 0; i < qtyOfSubItems; i++)
            {
                CMD_Item.CommandText = "insert into EntSubItemDetails(ItemName,SubItemName,SubItemOrder,BranchId) Values('" + itemName + "','" + subItems.ElementAt(i).ToString() + "','" + (i + 1) + "','" + Globals.BranchID + "')";
                returnMsg = PrjClass.ExecuteNonQuery(CMD_Item);
            }

            return returnMsg;
        }
        else
        {
            return "No. of subitems not valid";
        }
    }

    // loads default items and rate
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string[] LoadDefaultItemProcessAndRate(string rateListId)
    {
        try
        {
            string[] sl = new string[4];
            PrjClass.commandForQuery = new SqlCommand();
            PrjClass.commandForQuery.CommandText = "sp_newbooking";
            PrjClass.commandForQuery.CommandType = CommandType.StoredProcedure;
            PrjClass.commandForQuery.Parameters.AddWithValue("@flag", "32");
            HttpContext.Current.Items.Add(Session["UniqueIDPBU"].ToString(), true);
            PrjClass.commandForQuery.Parameters.AddWithValue("@BranchID", Globals.BranchID);
            PrjClass.commandForQuery.Parameters.AddWithValue("@RateListId", int.Parse(rateListId));
            var ds = PrjClass.ExecuteReader(PrjClass.commandForQuery);
            // DataSet ds2 = PrjClass.GetData(PrjClass.commandForQuery);
            while (ds.Read())
            {
                sl[0] = ds.GetValue(0).ToString();
                sl[1] = ds.GetValue(1).ToString();
                sl[2] = ds.GetValue(2).ToString();
            }
            ds.Close();           
            var bs = BAL.BALFactory.Instance.BAL_New_Bookings.SetQtySpaceOrOne(Globals.BranchID);
            if (bs)
            {
                sl[3] = "";
            }
            else
            {
                sl[3] = "1";
            }
            return sl;
        }
        catch (Exception ex)
        {
            return new String[] { "Error" };
        }
        finally
        {

        }
    }

    // find if tax is before or after
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool TaxBeforeOrAfter()
    {
        return bool.Parse(BAL.BALFactory.Instance.BAL_New_Bookings.FindTotalTaxActive(Globals.BranchID));
    }

    // adds the priority
    [WebMethod(EnableSession = true)]
    public string AddPriority(string arg)
    {
        if (arg == string.Empty)
            return "";

        DTO.NewBooking Obj = new DTO.NewBooking();
        Obj.BID = Globals.BranchID;
        Obj.AddPriority = arg;
        return BAL.BALFactory.Instance.BAL_New_Bookings.SavePriority(Obj);
    }

    // adds the customer
    [WebMethod(EnableSession = true)]
    public string AddCustomer(string[] args)
    {
        DTO.NewBooking Obj = new DTO.NewBooking
                                {
                                    AddPriority = args[0],
                                    Priority = args[1],
                                    CustAddress = args[2],
                                    CustAreaAndLocation = args[3],
                                    CustMobile = args[4],
                                    CustName = args[5],
                                    CustRemarks = args[6],
                                    CustTitle = args[7],
                                    Discount = args[8],
                                    // this is actually the communication means
                                    Remarks = args[9],
                                    RateListId = args[10],
                                    // this is actually the email, can be null
                                    SubItem1 = args[11]
                                };
        /*
        if (args.Length == 12)
            Obj.BDate = args[11];
        else
            Obj.BDate = string.Empty;

        if (args.Length == 13)
            Obj.ADate = args[12];
        else
            Obj.ADate = string.Empty;
        */
        Obj.BID = Globals.BranchID;

        // check if customer already exits
        // NO NEED TO CHECK ALREADY EXISTS
        //var prvExists = BAL.BALFactory.Instance.BAL_New_Bookings.CheckIfCustomerExists(Obj.CustName, Obj.CustAddress, Globals.BranchID);
        //if (prvExists)
        //    return "Exists!";

        var res = BAL.BALFactory.Instance.BAL_New_Bookings.SaveCustomer(Obj);
        // the result is in form of
        // select 'Cust' + ID as Custcode
        if (res.Tables.Count >= 1 && res.Tables[0].Rows.Count >= 1 && res.Tables[0].Rows[0]["CustCode"].ToString() != "")
        {
            // there is valid value, return true
            return "Added:" + res.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            // check if customer exits or this is some othe problem
            var prvExists = BAL.BALFactory.Instance.BAL_New_Bookings.CheckIfCustomerExists(Obj.CustName, Obj.CustAddress, Globals.BranchID);
            if (prvExists)
                return "Exists!";

            return "Error!";
        }
    }

    // this method returns if the color and desc are bind to master
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string checkDescAndColorForBinding()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.checkDescAndColorForBinding(Globals.BranchID);
    }

    // finds if the netAmount to be calc is in decimal or round
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool isNetAmountInDecimal()
    {
        PrjClass.commandForQuery = new SqlCommand();
        PrjClass.commandForQuery.CommandText = "sp_NewBooking";
        PrjClass.commandForQuery.CommandType = CommandType.StoredProcedure;
        PrjClass.commandForQuery.Parameters.AddWithValue("@BranchID", Globals.BranchID);
        PrjClass.commandForQuery.Parameters.AddWithValue("@Flag", 33);
        var rdr = PrjClass.ExecuteReader(PrjClass.commandForQuery);
        var _res = string.Empty;
        while (rdr.Read())
        {
            _res = rdr.GetString(0);
        }
        rdr.Close();
        return bool.Parse(_res);
    }

    // this will load the urgent rate, if any applied
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string LoadUrgentRate(string argFlag)
    {
        // return BAL.BALFactory.Instance.BAL_New_Bookings.GetNextDayRate(Globals.BranchID, argFlag);
        return BAL.BALFactory.Instance.BAL_New_Bookings.GetNextDayRateAndDayOffset(Globals.BranchID, argFlag);
    }

    // this finds the tax on process
    [WebMethod(EnableSession = true)]
    public double FindProcessTax(string argProcess, double argAmt, double argTaxRate, double argTaxRate1, double argTaxRate2)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcess(argProcess, argAmt, Globals.BranchID);
        // return BAL.BALFactory.Instance.BAL_New_Bookings.LoadAllTax(BID);
    }

    // this finds the tax on process if after discount
    [WebMethod(EnableSession = true)]
    public double FindProcessTaxAter(string argProcess, double argAmt, double argDiscount, double argTaxRate, double argTaxRate1, double argTaxRate2)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.getServiceTaxAccordingProcessWhenAfterCondition(argProcess, argAmt, Globals.BranchID, argDiscount);
    }

    // this finds weather the default discount type is perc or full
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string FindDefaultDiscountType()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultDiscountType(Globals.BranchID);
    }

    // this will find no of subitems that will be used to calcualate the qty
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public int CountNoOfSubItems(string argItemName)
    {
        var returnVal = BAL.BALFactory.Instance.BAL_New_Bookings.CountNoOfSubItem(argItemName, Globals.BranchID);
        return Int32.Parse(returnVal);
    }

    // this will add the new remark to the database
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void AddNewRemarks(string argRemarks)
    {
        DAL.DALFactory.Instance.DAL_New_Bookings.SaveRemarks(argRemarks, Globals.BranchID);
    }

    // this will add the new color to the database
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void AddNewColors(string argColors)
    {
        DAL.DALFactory.Instance.DAL_New_Bookings.SaveColors(argColors, Globals.BranchID);
    }

    // this will update the hidden field that is the source of remaks
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string FindRemarksSource()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.BindRemarksInUI(Globals.BranchID);
    }

    // this will update the hidden field that is the source of color
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string FindColorsSource()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.BindColorsInUINew(Globals.BranchID);
    }

    // this bind the priority in the AddCustomer Dialog
    [WebMethod(EnableSession = true)]
    public string[] GetPriorityList(string prefixText, int count)
    {
        try
        {
            List<string> str = new List<string>();
            DataSet ds = BAL.BALFactory.Instance.BAL_New_Bookings.BindPriorityCustom(Globals.BranchID, prefixText.ToUpperInvariant());
            if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Add(ds.Tables[0].Rows[i][1].ToString());
                }
            }
            //else if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
            //{
            //    str.Add(ds.Tables[0].Rows[0][1].ToString());
            //}
            return str.ToArray();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    // this will check if the passed item code exists
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool checkIfItemCodeExits(string argItemCode)
    {
        return BAL.BALFactory.Instance.BAL_Item.CheckIfItemCodeExits(argItemCode, Globals.BranchID);
    }

    // this will return the tax and discount
    // the value will be returned in the format
    // ProcessCode:IsActive
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string FindDisActive()
    {
        return BAL.BALFactory.Instance.Bal_Processmaster.FindDisActive(Globals.BranchID);
    }

    // this will return the tax and discount
    // the value will be returned in the format
    // ProcessCode:IsActive:TaxAmount
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string FindTaxActive()
    {
        return BAL.BALFactory.Instance.Bal_Processmaster.FindTaxActive(Globals.BranchID);
    }

    // this will check if the given customer exits
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool CheckIfCustomerExists(string argCustName)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.CheckIfCustomerExists(argCustName, Globals.BranchID);
    }

    // this finds all Priority
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string FindAllPriority()
    {
        return BAL.BALFactory.Instance.BAL_Priority.findAllPriority(Globals.BranchID);
    }

    // this loads all the items
    // so that we don't have to travel back to database
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string LoadAllItems()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.LoadAllItems(Globals.BranchID);
    }

    // this loads all the items
    // so that we don't have to travel back to database
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string LoadAllProcesses()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.LoadAllProcesses(Globals.BranchID);
    }

    // this returns all 3 taxes in the format of tax:tax1:tax2
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string LoadAllThreeTaxes()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.LoadAllTax(Globals.BranchID);
    }

    // finds weather tax is exclusive or inclusive
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string IsTaxExclusive()
    {
        var result = BAL.BALFactory.Instance.BAL_New_Bookings.LoadInclusiveExclusive(Globals.BranchID);
        if (result.ToUpperInvariant() == "INCLUSIVE")
            return "false";
        else
            return "true";
    }

    // this method returns if the color and desc is enabled
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string checkIfDesAndColorEnabled()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.checkIfDesAndColorEnabled(Globals.BranchID);
    }

    // this method returns if the color and desc are bind to master
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string CheckLenBredth(string argItemName)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.CheckLenBredth(Globals.BranchID, argItemName);
    }

    // this checks if confirmation is asked for delivery date
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool ConfirmDelivery()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.ConfirmDelivery(Globals.BranchID);
    }

    // this sets the process rate for given item
    [WebMethod(EnableSession = true)]
    public bool SetItemRateForProcess(string ItemName, string ProcessName, string argExtraProcess, string argExtraProcess2, double rate, double rate1, double rate2)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.SetItemRateForProcess(ItemName, ProcessName, argExtraProcess, argExtraProcess2, rate, rate1, rate2, Globals.BranchID);
    }

    // this checks weather to bind color with qty
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool CheckIfBindColorToQty()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.CheckIfBindColorToQty(Globals.BranchID);
    }

    // the sms configuration, to check the sms box in bk scrn
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool checkDefaultSMS()
    {
        return BAL.BALFactory.Instance.BAL_sms.SetSMSCheckBoxOnScreen(Globals.BranchID, "13");
    }

    // check if access rights are allowed
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool checkAcsRights(string pageTitle)
    {
        return BAL.BALFactory.Instance.BAL_Comment.checkAcsRights(pageTitle, Globals.UserType, Globals.BranchID);        
    }

    // check if booking number exits or not
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool checkIfBookingNumberExists(string bookingNumber)
    {
        return BAL.BALFactory.Instance.BAL_DateAndTime.CheckBookingNumber(bookingNumber, Globals.BranchID);
    }

    // check if booking number exits or not
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool CheckBookingNumberInFactory(string bookingNumber)
    {
        return BAL.BALFactory.Instance.BAL_DateAndTime.CheckBookingNumberInFactory(bookingNumber, Globals.BranchID);
    }

    // check is sms func is available from the cloth ready screen
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool checkIfSMSAvailable()
    {
        return BAL.BALFactory.Instance.BAL_sms.CheckReadyClothSendSms(Globals.BranchID);
    }

    // check deliver slip access right
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool CheckDeliverSlipViewRight(string BookingNo)
    {
        return BAL.BALFactory.Instance.BAL_sms.CheckDeliverSlipViewRight(Globals.BranchID, BookingNo, Globals.UserType);
    }

    // checks the means of communication, either by sms, mail or both
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public string CheckMeansOfCommunication(string customerCode)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.CheckMeansOfCommunication(customerCode, Globals.BranchID);
    }

    // this checks the details of package
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string CheckDetailsOfPackage(string customerCode, string bookingDate)
    {
        var res = BAL.BALFactory.Instance.BAL_New_Bookings.CheckDetailsOfPackage(customerCode, bookingDate, Globals.BranchID);
        return res;
    }

    // finds the workshop remark
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public string findWorkShopRemark(string bookingNumber)
    {
        var branchId = Globals.BranchID;
        return BAL.BALFactory.Instance.Bal_Report.findWorkShopRemark(bookingNumber, branchId);
    }

    // finds the password required to modify Item, rate and discount
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string LoadThePasswords()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.LoadThePassWords(Globals.BranchID);
    }

    // load the default search criteria for customer
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string LoadDefaultSearchCriteriaForCustomer()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.LoadDefaultSearchCriteriaForCustomer(Globals.BranchID);
    }

    // checks if editing remaks are to be saved
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public bool checkForEditRemarks()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.checkForEditRemarks(Globals.BranchID);
    }

    // checks if editing remaks are to be saved
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string checkForDelDiscountPwd()
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.checkForDelDiscountPwd(Globals.BranchID);
    }

    // gets the package name
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetPackageName(string prefixText, int count)
    {
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@PackageName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@Flag", 20);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    // check if mobile no is unique
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public bool IsMobileUnique(string mobileNo)
    {
        return BAL.BALFactory.Instance.BAL_New_Bookings.IsMobileUnique(mobileNo, Globals.BranchID);
    }

    // stock recon in case of 2nd grid, if the data doesn't matches for the first one!
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string[] stockReconNotMatched(string barcode)
    {
        var ds = BAL.BALFactory.Instance.Bal_Report.GetCorrectBaroceNo(barcode, Globals.BranchID);
        if (ds == null) return null;
        if (ds.Tables.Count == 0) return null;
        if (ds.Tables[0].Rows.Count == 0) return null;
        var str = new List<string>(6);

        str.Add(ds.Tables[0].Rows[0]["BookingDate"].ToString());
        str.Add(ds.Tables[0].Rows[0]["BookingNumber"].ToString());
        str.Add(ds.Tables[0].Rows[0]["CustomerName"].ToString());
        str.Add(ds.Tables[0].Rows[0]["item"].ToString());
        str.Add(ds.Tables[0].Rows[0]["status"].ToString());
        str.Add(ds.Tables[0].Rows[0]["BarCode"].ToString());

        return str.ToArray();
    }

    /// <summary>
    /// This serialize the string contents of a grid to file
    /// </summary>
    /// <param name="gridText">The grid text</param>
    /// <param name="fileName">The file name to save to</param>
    /// <param name="bOverride">override previous contents</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SerializeGridToFile(string gridText, string fileName, bool bOverride)
    {
        try
        {
            if (!bOverride)
                File.AppendAllText(Path.Combine(Server.MapPath("~/SerializedGrids"), fileName), gridText);
            else
                File.WriteAllText(Path.Combine(Server.MapPath("~/SerializedGrids"), fileName), gridText);
        }
        catch (System.IO.IOException)
        {
            return "Failed";
        }
        return "Done";
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SaveStockBarCodes(string BarCodes, string flag)
    {
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_stockreconcile";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@barcodeNo", BarCodes);
            if (flag == "yes")
            {
                cmd.Parameters.AddWithValue("@Flag", 16);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Flag", 17);
            }
            var str = PrjClass.ExecuteNonQuery(cmd);
            if (str == "Record Saved")
                return "Done";
            else
                return "Failed";
        }
        catch (Exception)
        {
            return "Failed";
        }
    }

    /// <summary>
    /// Deletes the serialized file
    /// </summary>
    /// <param name="fileName">the filename to delete</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string DeleteSerializedGridFile(string fileName)
    {
        try
        {
            File.Delete(Path.Combine(Server.MapPath("~/SerializedGrids"), fileName));
            var cmd = new SqlCommand
            {
                CommandText = "DELETE FROM StockMatch WHERE BranchId='" + Globals.BranchID + "'",
            };
            PrjClass.ExecuteNonQuery(cmd);
            cmd = new SqlCommand
            {
                CommandText = "DELETE FROM StockNotMatch WHERE BranchId='" + Globals.BranchID + "'",
            };
            PrjClass.ExecuteNonQuery(cmd);
        }
        catch (Exception)
        {
            return "Failed";
        }
        return "Done";
    }

    /// <summary>
    /// Updated the status of stock recon report
    /// </summary>
    /// <param name="barCodes">barcodes</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string UpdateConsolidatedStatus(string barCodes)
    {
        return BAL.BALFactory.Instance.Bal_Report.UpdateConsolidatedStatus(barCodes, Globals.BranchID);
    }

    /// <summary>
    /// deletes the barcodes for that stock recon report
    /// </summary>
    /// <param name="barCodes">barcode(s)</param>
    /// <param name="table">table to delete data from</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string DeleteBarCodesFrom(string barCodes, string table)
    {
        var cmd = new SqlCommand
        {
            CommandText = "DELETE FROM " + table + " WHERE BranchId='" + Globals.BranchID + "'",
        };
        return PrjClass.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// sets the right so that stock reocn page can't be opened anywhere else
    /// </summary>
    /// <param name="bSetFalse">set to true or false</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SetOnlyOneTab(bool bSetFalse)
    {
        var sqlCommand = new SqlCommand();
        sqlCommand.CommandText = "UPDATE EntMenuRights SET RightToView = '" + bSetFalse.ToString() + "' WHERE PageTitle = '" + SpecialAccessRightName.StockRecon+ "' AND BranchId = " + Globals.BranchID;
        return PrjClass.ExecuteNonQuery(sqlCommand);
    }

    /// <summary>
    /// sets the menu right so that page can/cannot be opened anywhere else, depending upon the bool param
    /// </summary>
    /// <param name="bSetFalse">set to true or false</param>
    /// <param name="branchId">branch id in case of session is expired</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SetOnlyOneTab(bool bSetFalse, string branchId)
    {
        var sqlCommand = new SqlCommand();
        sqlCommand.CommandText = "UPDATE EntMenuRights SET RightToView = '" + bSetFalse.ToString() + "' WHERE PageTitle = '" + SpecialAccessRightName.StockRecon+ "' AND BranchId = " + branchId;
        return PrjClass.ExecuteNonQuery(sqlCommand);
    }

    /// <summary>
    /// find the qty and items for the cust code
    /// </summary>
    /// <param name="bSetFalse"></param>
    /// <param name="branchId"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetQtyndItemsForPackage(string custCode, int assignId, int recurrenceId)
    {
        return BAL.BALFactory.Instance.BL_PackageMaster.GetQtyndItemsForPackage(custCode, assignId, recurrenceId, Globals.BranchID);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetQtyPackageDetails(int assignId)
    {
        return BAL.BALFactory.Instance.BL_PackageMaster.GetQtyPackageDetails(assignId, Globals.BranchID);
    }

    /// <summary>
    /// Validate if given no of recurren is valid for given dates
    /// </summary>
    /// <param name="assignId"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool ValidateRecurrenceCount(string startDate, string endDate, string recurrenceCount)
    {
        var dateStart = DateTime.Parse(startDate);
        var dateEnd = DateTime.Parse(endDate);
        var diff = dateEnd - dateStart;
        return diff.Days >= int.Parse(recurrenceCount);
    }

    /// <summary>
    /// Changes the challan status from new challan screen
    /// </summary>
    /// <param name="barCodes"></param>
    /// <param name="challanStatus"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool ChangeChallanStatus(string barCodes, int challanStatus)
    {
        var result = BAL.BALFactory.Instance.BAL_ChallanIn.ChangeChallanStatus(barCodes, challanStatus, Globals.BranchID, Globals.UserName);
        return result;
    }

    [WebMethod(EnableSession = true)]
    public string[] GetAddressList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CustomerAddress  FROM   dbo.CustomerMaster where BranchId='" + Globals.BranchID + "' AND CustomerAddress like '%" + prefixText.Trim() + "%' ";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetCustomerName(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CustomerName  FROM   dbo.CustomerMaster where BranchId='" + Globals.BranchID + "' AND CustomerName like '%" + prefixText.Trim() + "%' ";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetPhoneNoList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CustomerMobile  FROM   dbo.CustomerMaster where BranchId='" + Globals.BranchID + "' AND CustomerMobile like '%" + prefixText.Trim() + "%' ";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetItemCode(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CONVERT(VARCHAR, ItemCode) + '   ' + '[' + ItemName + ']' AS ItemName FROM ItemMaster WHERE BranchId='" + Globals.BranchID + "' AND ItemCode like '" + prefixText.Trim() + "%'  order by itemcode";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetMobileNo(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CustomerMobile FROM CustomerMaster WHERE BranchId='" + Globals.BranchID + "' AND CustomerMobile like '" + prefixText.Trim() + "%'";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetItemNameList(string prefixText, int count)
    {
        DataSet ds = new DataSet();
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemSearchName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 8);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetSubItemNameList(string prefixText, int count)
    {
        DataSet ds = new DataSet();
        if (count == 0)
        {
            count = 10;
        }

        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_NewBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemSearchName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 73);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetItemColor(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT COLORNAME FROM mstColor WHERE COLORNAME like '%" + prefixText.Trim() + "%' ";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetItemRemarks(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT REMARKS FROM mstRemark WHERE BranchId='" + Globals.BranchID + "' AND Id<>1 AND REMARKS like '%" + prefixText.Trim() + "%' ";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetProcessList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "SELECT UPPER(ProcessCode) FROM ProcessMaster where BranchId='" + Globals.BranchID + "' AND ProcessName like '%" + prefixText.Trim() + "%' UNION SELECT UPPER(ProcessCode) FROM ProcessMaster where BranchId='" + Globals.BranchID + "' AND ProcessCode like '%" + prefixText.Trim() + "%'";
            cmd.CommandText = "IF EXISTS( select UPPER(ProcessCode) FROM ProcessMaster where BranchId='" + Globals.BranchID + "' AND ProcessCode ='" + prefixText.Trim() + "') select UPPER(ProcessCode) FROM ProcessMaster where BranchId='" + Globals.BranchID + "' AND ProcessCode ='" + prefixText.Trim() + "' else SELECT UPPER(ProcessCode) FROM ProcessMaster where BranchId='" + Globals.BranchID + "' AND ProcessName like '%" + prefixText.Trim() + "%' UNION SELECT UPPER(ProcessCode) FROM ProcessMaster where BranchId='" + Globals.BranchID + "' AND ProcessCode like '%" + prefixText.Trim() + "%' ";
            sdr = ExecuteReader(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetFullDetailofCustomer(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustName", prefixText.ToString());
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 27);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetDetailOfArealocation(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AreaLocation", prefixText.ToString());
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 39);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetDetailRemoveReasonMaster(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }

        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RemoveReason", prefixText.ToString());
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 31);
            sdr = AppClass.ExecuteReader(cmd);
            ds = AppClass.GetData(cmd);
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }
        return items.ToArray();
    }

    public static SqlDataReader ExecuteReader(SqlCommand cmd)
    {
        SqlDataReader dr = null;
        SqlConnection con = null;
        try
        {
            con = new SqlConnection(sqlConStr);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            else
            {
                con.Open();
            }
            cmd.Connection = con;
            dr = cmd.ExecuteReader();
        }
        catch (Exception)
        {
        }
        finally
        {
            //con.Close();
        }
        return dr;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetCurrentDate()
    {  
        string CurrentDate=string.Empty;
        string active = string.Empty;
        string strData1 = string.Empty;
         ArrayList date = new ArrayList();
         DataSet ds1 = new DataSet();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            ds1 = BAL.BALFactory.Instance.BL_ColorMaster.CheckRemote(Globals.BranchID);
            CurrentDate = date[0].ToString();
             active = ds1.Tables[0].Rows[0]["IsBackupActive"].ToString();
            strData1=  CurrentDate +":"+ active;
        }
        catch (Exception)
        {
        }
        return strData1;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetGarmentStatus(string date)
    {
        string strData = string.Empty;
        DataSet dsData = new DataSet();
        try
        {
         dsData= BAL.BALFactory.Instance.BL_CustomerMaster.GetGarmentStatusDetail(date,Globals.BranchID);
         strData = dsData.Tables[0].Rows[0]["TotalPcs"].ToString() + ":" + dsData.Tables[0].Rows[0]["WorkshopIn"].ToString() + ":" + dsData.Tables[0].Rows[0]["Ready"].ToString() + ":" + dsData.Tables[0].Rows[0]["Delivered"].ToString() + ":" + dsData.Tables[0].Rows[0]["ReadyButPending"].ToString()+":"+ Globals.UserName;         
        }
        catch (Exception)
        {
        }
        return strData;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetPendingStatus(string date)
    {
        string strPending = string.Empty;
        DataSet dsData = new DataSet();
        try
        {
            dsData = BAL.BALFactory.Instance.BL_CustomerMaster.GetPendingStatusDetail(date, Globals.BranchID);
            strPending = dsData.Tables[0].Rows[0]["OverDue"].ToString() + ":" + dsData.Tables[0].Rows[0]["TodayDue"].ToString() + ":" + dsData.Tables[0].Rows[0]["FutureDue"].ToString();
        }
        catch (Exception)
        {
        }
        return strPending;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetStoreDetails()
    {
        string strData = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet dsStoredata = new DataSet();
        try
        {
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 18);
            dsStoredata = AppClass.GetData(cmd);
            if (dsStoredata.Tables[0].Rows.Count > 0)
            {
                strData = dsStoredata.Tables[0].Rows[0]["StoreName"].ToString() + ":" + dsStoredata.Tables[0].Rows[0]["Address"].ToString()+":"+Globals.UserName;
            }
        }
        catch (Exception)
        {
        }
        return strData;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetDetailForGraph()
    {
        string strData = string.Empty;
        string strdate = string.Empty;       
        string strProcess = string.Empty;        
        string strDateQty = string.Empty;
        string strFromAndToDate = string.Empty;
     
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            cmd.CommandText = "sp_ReadyAndDeliveryStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@date", date[0].ToString());
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {                
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strdate = "";
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            strdate = ds.Tables[0].Rows[i][j].ToString();                         
                        }
                        else
                        {
                            strdate = strdate +"," + ds.Tables[0].Rows[i][j].ToString();
                        } 
                    }           
                    strDateQty =strDateQty+ strdate + "@";
                }      
                for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                {
                    strProcess = strProcess + ds.Tables[1].Rows[k]["Process"].ToString()+",";
                }
                strDateQty = strDateQty.Substring(0, strDateQty.Length - 1);
                strProcess = strProcess.Substring(0, strProcess.Length - 1);
                var Count = ds.Tables[0].Rows.Count-1;
                strFromAndToDate = ds.Tables[0].Rows[0]["DueDate"].ToString() + "@" + ds.Tables[0].Rows[Count]["DueDate"].ToString();
            }
            strData = strDateQty + ":" + strProcess+":"+ strFromAndToDate;
        }
        catch (Exception)
        {
        }
        return strData;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public  string GetStoreData(string BID)
    {
        string strStoredata = string.Empty;
        DataSet dsStore = new DataSet();
        try
        {
            dsStore = BAL.BALFactory.Instance.BAL_Color.GetStoreNameAddress(BID);
            strStoredata = dsStore.Tables[0].Rows[0]["BranchName"].ToString() + ":" + dsStore.Tables[0].Rows[0]["BranchAddress"].ToString();
        }
        catch (Exception)
        {
        }
        return strStoredata;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetWebUserPassword(string BID,string UserName, string Password)
    {
        string strPwd = "False";
        DataSet ds = new DataSet();
        SqlCommand CMD = new SqlCommand();
        try
        {
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@UserName", UserName);
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 13);
            ds = PrjClass.GetData(CMD);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if ( Convert.ToString(ds.Tables[0].Rows[0]["UserPassword"]) == Password)
                {
                    strPwd = "True";
                }
                else
                {
                    strPwd = "False";
                }           
            
            }
        }
        catch (Exception)
        {
        }
        return strPwd;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetWorkshopWorkloadData(string date)
    {
        string strData = string.Empty;
        string strProcessAndQty = string.Empty;
        string strTotalPcs = string.Empty;

        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();      
        try
        {         
            cmd.CommandText = "sp_ReadyAndDeliveryStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strProcessAndQty =strProcessAndQty + ds.Tables[0].Rows[i]["Process"].ToString() + "," + ds.Tables[0].Rows[i]["Pcs"].ToString()+"@";                                        
                }
                strProcessAndQty = strProcessAndQty.Substring(0, strProcessAndQty.Length - 1);
                strTotalPcs = ds.Tables[1].Rows[0]["TotalPcs"].ToString();
            }
            strData = strProcessAndQty + ":" + strTotalPcs;
        }
        catch (Exception)
        {
        }
        return strData;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool SaveChallanData(string EBID)
    {
        var result = false;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate=date[0].ToString();
            var strTime=date[1].ToString();
            var BID =Globals.BranchID;
            var UserName = Globals.UserName;
            if (EBID == "undefined")
            {
                EBID = "";
            }
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveChallanDetails(BID, EBID, strdate, strTime, UserName,false,0);
           
        }
        catch (Exception)
        { 
            
        }
         return result;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool SaveChallanDataAndPrintSticker(string EBID, string Possition)
    {
        var result = false;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            if (EBID == "undefined")
            {
                EBID = "";
            }
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveChallanDetails(BID, EBID, strdate, strTime, UserName, true,Convert.ToInt32(Possition));

        }
        catch (Exception)
        {

        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SaveChallanAndWorkShopNote(string EBID)
    {
        var result = false;
        string strChallanNo = string.Empty;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            if (EBID == "undefined")
            {
                EBID = "";
            }
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveChallanDetails(BID, EBID, strdate, strTime, UserName, false, 0);
            if (result == true)
            {
                DataSet ds = new DataSet();
                SqlCommand CMD = new SqlCommand();
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.CommandText = "Proc_NewChallanXML";             
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 2);
                ds = PrjClass.GetData(CMD);
                strChallanNo = BID +"-"+ ds.Tables[0].Rows[0]["ChallanID"].ToString() + ":" + strdate;             
            }
        }
        catch (Exception)
        {

        }
        return strChallanNo;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string GetChallnNoData()
    {      
        string strChallanNo = string.Empty;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();           
            var BID = Globals.BranchID;
                DataSet ds = new DataSet();
                SqlCommand CMD = new SqlCommand();
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.CommandText = "Proc_NewChallanXML";
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 2);
                ds = PrjClass.GetData(CMD);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strChallanNo = BID + "-" + ds.Tables[0].Rows[0]["ChallanID"].ToString() + ":" + strdate;
                }
        }
        catch (Exception)
        {

        }
        return strChallanNo;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool SendForFinishingOrReady(string Status, string ScreenStatus, string PinNo, bool strSMS, string SMSType)
    {
        var result = false;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SendForFinishingOrReadyDetail(BID, Status, strdate, strTime, UserName, false, 0, Convert.ToInt32(ScreenStatus), PinNo);
            if (Status == "3" && strSMS==true)
            {
                SendSMSData(SMSType);
            }
        }
        catch (Exception)
        {

        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool SaveChallanAndWorkShopNoteAndPrintSticker(string Possition, string ScreenStatus, string PinNo, bool strSMS, string SMSType)
    {
        var result = false;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveChallanAndWorkShopNoteAndPrintStickerDetail(BID, "3", strdate, strTime, UserName, true, Convert.ToInt32(Possition), Convert.ToInt32(ScreenStatus), PinNo);
            if (strSMS == true)
            {
                SendSMSData(SMSType);
            }
        }
        catch (Exception)
        {

        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool ChangeWorkshopChallanStatusData(string barCodes, int challanStatus)
    {
        var result = BAL.BALFactory.Instance.BAL_ChallanIn.ChangeWorkshopChallanStatus(barCodes, challanStatus, Globals.BranchID, Globals.UserName);
        return result;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool SaveReceiveFromStoreData(string Status, string ScreenStatus)
    {
        var result = false;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveReceiveFromStoreDetail(BID, Status, strdate, strTime, UserName, Convert.ToInt32(ScreenStatus));

        }
        catch (Exception)
        {

        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool SaveSendToStoreData(string Possition, string ScreenStatus, string WorkShopNote)
    {
        var result = false;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveSendToStoreDetail(BID, "2", strdate, strTime, UserName, true, Convert.ToInt32(Possition), Convert.ToInt32(ScreenStatus), WorkShopNote);

        }
        catch (Exception)
        {

        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SaveSendToStoreDataAndPrint(string ScreenStatus, string WorkShopNote)
    {
        var result = false;
        string strChallanNo = string.Empty;
        ArrayList date = new ArrayList();
        try
        {
            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            var strdate = date[0].ToString();
            var strTime = date[1].ToString();
            var BID = Globals.BranchID;
            var UserName = Globals.UserName;
            result = BAL.BALFactory.Instance.BAL_ChallanIn.SaveSendToStoreDetail(BID, "2", strdate, strTime, UserName, false,0, Convert.ToInt32(ScreenStatus), WorkShopNote);
            if (result == true)
            {
                DataSet ds = new DataSet();
                SqlCommand CMD = new SqlCommand();
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.CommandText = "Proc_NewChallanXML";
                CMD.Parameters.AddWithValue("@BranchId", BID);
                CMD.Parameters.AddWithValue("@Flag", 6);
                ds = PrjClass.GetData(CMD);
                strChallanNo = BID + ":" + ds.Tables[0].Rows[0]["ChallanNumber"].ToString();
            }
        }
        catch (Exception)
        {

        }
        return strChallanNo;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string GetChallnNoDataWorkshopOut()
    {
        string strChallanNo = string.Empty;
        ArrayList date = new ArrayList();
        try
        {  
            var BID = Globals.BranchID;
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_NewChallanXML";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(CMD);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strChallanNo = BID + ":" + ds.Tables[0].Rows[0]["ChallanNumber"].ToString();
            }
        }
        catch (Exception)
        {

        }
        return strChallanNo;
    }

    public void GetDataforEmail(string CustomerCode)
    {

        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "proc_BindToMachine";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        cmd.Parameters.AddWithValue("@custcode", CustomerCode);
        cmd.Parameters.AddWithValue("@Flag", 11);
        ds = PrjClass.GetData(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string CustName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            string ToEmail = ds.Tables[0].Rows[0]["CustomerEmaiiID"].ToString();

            string path = Server.MapPath(@"ReceiptLogo/DRY" + Globals.BranchID + ".jpg");
            if (File.Exists(path) == false)
            {
                path = Server.MapPath(@"ReceiptLogo/DRY.jpg");
            }            
            LinkedResource logo = new LinkedResource(path);
            logo.ContentId = "companylogo";


            string mailBody = "<table style='width: 550px; text-align: justify; font-size: 14px'> <tr><td><img src=cid:companylogo  width='60px' height='70px' /> <h2 style='color: #FF622D'> Welcome to " + ds.Tables[0].Rows[0]["branchname"].ToString() + "!</h2><p> Dear " + CustName + ",<br/><br />  Welcome to " + ds.Tables[0].Rows[0]["branchname"].ToString() + "! You can now track the status of your orders through  your phone, tablet, laptop, or computer system from wherever you are. Click the  link to access your account.<br /><br /><span style='background-color: #DFDFDF;font-weight: bold;margin-left:20px'>User Name : " + CustomerCode + "</span><br /><span style='background-color: #DFDFDF;font-weight: bold;margin-left:20px' >Password&nbsp;&nbsp;&nbsp;: Hello123</span><br/><br/><u>" + ds.Tables[0].Rows[0]["websitelink"].ToString() + "</u><br /><br />Jump in and try some of these activities: </p><p style='margin-left: 40px'> 1.&nbsp;&nbsp;Track your orders<br />2.&nbsp;&nbsp;Update your information to help us serve you even better</p><p>Happy Cleaning,<br /><br/>The " + ds.Tables[0].Rows[0]["branchname"].ToString() + "<br />P.S. If the link does not work for you please enter the following url into your  browser: <u>" + ds.Tables[0].Rows[0]["websitelink"].ToString() + "</u><br /> <br /><span style='background-color: #FFFFD7'> " + ds.Tables[0].Rows[0]["branchname"].ToString() + " , " + ds.Tables[0].Rows[0]["BranchAddress"].ToString() + "</span> </p>  </td> </tr></table>";
            //string mailBody = "test data";
            int port = Convert.ToInt32(ds.Tables[1].Rows[0]["Port"]);
            string HostName = ds.Tables[1].Rows[0]["HostName"].ToString();
            string BranchEmail = ds.Tables[1].Rows[0]["BranchEmail"].ToString();
            string Password = ds.Tables[1].Rows[0]["branchPassword"].ToString();
            bool SSL = Convert.ToBoolean(ds.Tables[1].Rows[0]["SSL"]);

            Task t = Task.Factory.StartNew
                          (
                             () => { SendMail(ToEmail, BranchEmail, Password, mailBody, SSL, HostName, port, logo); }
                          );
            
        }
    }
    public string SendMail(string toList, string from, string Password, string body, bool SSL, string HostName, int port, LinkedResource logo)
    {
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();
        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(from);
            message.From = fromAddress;
            message.To.Add(toList);

            message.Subject = "Website User link";
            AlternateView av1 = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            av1.LinkedResources.Add(logo);

            message.IsBodyHtml = true;
           // message.Body = body;
            message.AlternateViews.Add(av1);
            smtpClient.Host = HostName;   
            smtpClient.Port = port;
            smtpClient.EnableSsl = SSL;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(from, Password);

            smtpClient.Send(message);
            msg = "Successful";
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        return msg;        
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool CheckCorrectRemoveReasonData(string retText)
    {
        bool result = false;
        try
        {
           result= BAL.BALFactory.Instance.BAL_RemoveReason.CheckCorrectRemoveReason(Globals.BranchID, retText);
        }
        catch (Exception)
        {

        }
        return result;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SaveRemoveChallanData(string retText, string DataToRemove, string flag, string ScreenName)
    {
        string res = string.Empty;
        try
        {
            res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveRemoveChallan(DataToRemove, retText, Globals.UserName, Globals.BranchID, flag, ScreenName);
            if (res == "Record Saved")
            {             
            }
            else
            {
                res.ToString();
            }
        }
        catch (Exception)
        {

        }
        return res;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string PackingSticker(string possition)
    {
        string res = string.Empty;
        try
        {
            string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();          
            var printFrom = Int32.Parse(possition);
            res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveInStickerTableData(Globals.BranchID, printFrom);           
            if (res == "Record Saved")
            {
                res = "Record Saved";
            }
            else
            {
                res.ToString();
            }
        }
        catch (Exception)
        {

        }
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool ChangeChallanStatusForPrintSticker(string barCodes, int challanStatus)
    {
        var result = BAL.BALFactory.Instance.BAL_ChallanIn.ChangeChallanStatusForPrintStickerData(barCodes, challanStatus, Globals.BranchID);
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string SendSMS(string smsEvent)
    {
        string res = string.Empty;
        try
        {
            string status = string.Empty;
            DataSet ds = new DataSet();
            string BookingNo = string.Empty, BookingPrefix = string.Empty;
            ds = BAL.BALFactory.Instance.BAL_sms.ReadyClothScreenSms(Globals.BranchID);          
            var Temp = Globals.BranchID;
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {   
                    BookingNo=ds.Tables[0].Rows[r]["BookingNumber"].ToString();
                    BookingPrefix = ds.Tables[0].Rows[r]["BookingPrefix"].ToString().Trim().ToUpper();
                    AppClass.GoMsg(Temp, BookingPrefix+BookingNo, smsEvent);
            }           
            if (ds.Tables[0].Rows.Count > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_smsconfig";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
                cmd.Parameters.AddWithValue("@Flag", 16);
                PrjClass.ExecuteNonQuery(cmd);
                res = "Done";
            }
            else
            {
                res = "Faild";
            }
        }
        catch (Exception)
        {

        }
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public bool ChangeChallanStatusForWorkshopPrintSticker(string barCodes, int challanStatus)
    {
        var result = BAL.BALFactory.Instance.BAL_ChallanIn.ChangeChallanStatusForPrintStickerWSData(barCodes, challanStatus, Globals.BranchID);
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string WorkShopPackingSticker(string possition)
    {
        string res = string.Empty;
        try
        {            
            var printFrom = Int32.Parse(possition);
            res = BAL.BALFactory.Instance.BAL_ChallanIn.SaveInWorkShopStickerTableData(Globals.BranchID, printFrom);
            if (res == "Record Saved")
            {
                res = "Record Saved";
            }
            else
            {
                res.ToString();
            }
        }
        catch (Exception)
        {

        }
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string GetCustomersData()
    {
        string res = string.Empty;
        try
        {
            return BAL.BALFactory.Instance.BL_CustomerMaster.GetData(Globals.BranchID).GetXml();
        }
        catch (Exception)
        {

        }
        return res;
    }


    public void SendSMSData(string smsEvent)
    {
        string res = string.Empty;
        try
        {
            string status = string.Empty;
            DataSet ds = new DataSet();
            string BookingNo = string.Empty, BookingPrefix = string.Empty;
            ds = BAL.BALFactory.Instance.BAL_sms.ReadyClothScreenSms(Globals.BranchID);
            var Temp = Globals.BranchID;
            Task t = Task.Factory.StartNew (
                            () => { 
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        BookingNo = ds.Tables[0].Rows[r]["BookingNumber"].ToString();
                        BookingPrefix = ds.Tables[0].Rows[r]["BookingPrefix"].ToString().Trim().ToUpper();
                        AppClass.GoMsg(Temp, BookingPrefix + BookingNo, smsEvent);
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "SP_smsconfig";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BranchId", Temp);
                        cmd.Parameters.AddWithValue("@Flag", 16);
                        PrjClass.ExecuteNonQuery(cmd);
                    }
                }
                );
        }
        catch (Exception)
        {
        }       
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetDataForAllStatus(string Date)
    {
        string strData = string.Empty;
        string strStatus = string.Empty;
        string strProcess = string.Empty;
        string strStatusQty = string.Empty;     

        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();      
        try
        {           
            cmd.CommandText = "sp_ReadyAndDeliveryStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.Parameters.AddWithValue("@Flag", 5);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strStatus = "";
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            strStatus = ds.Tables[0].Rows[i][j].ToString();
                        }
                        else
                        {
                            strStatus = strStatus + "," + ds.Tables[0].Rows[i][j].ToString();
                        }
                    }
                    strStatusQty = strStatusQty + strStatus + "@";
                }
                for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                {
                    strProcess = strProcess + ds.Tables[1].Rows[k]["Process"].ToString() + ",";
                }
                strStatusQty = strStatusQty.Substring(0, strStatusQty.Length - 1);
                strProcess = strProcess.Substring(0, strProcess.Length - 1);                             
            }
            strData = strStatusQty + ":" + strProcess;
        }
        catch (Exception)
        {
        }
        return strData;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool checkAcsRightsForFactory(string pageTitle)
    {
        return BAL.BALFactory.Instance.BAL_Comment.checkAcsRightsForFactory(pageTitle, Globals.WorkshopUserType, Globals.BranchID);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public bool checkBookingNumberExist(string bookingNumber, string BID)
    { 
        return BAL.BALFactory.Instance.BAL_DateAndTime.CheckBookingNumber(bookingNumber, Globals.BranchID);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public int checkBookingNoCalcel(string bookingNumber, string BID)
    {
        int status = 0;      

        SqlCommand cmd = new SqlCommand();
        DataSet dsbookingNo = new DataSet();
        cmd.CommandText = "sp_Dry_EmployeeMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 11);
        cmd.Parameters.AddWithValue("@BookingNumber", bookingNumber);
        cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
        dsbookingNo = PrjClass.GetData(cmd);
        if (dsbookingNo.Tables[0].Rows.Count > 0)
        {
            status = Convert.ToInt32(dsbookingNo.Tables[0].Rows[0]["BookingStatus"].ToString());        
        }
        return  status;        
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetDetailofCustomer()
    { 
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;
        DataSet ds = new DataSet();
        string strCustData = string.Empty;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;          
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 44);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strCustData = ds.Tables[0].Rows[0]["CustName"].ToString();
            }          
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //      strCustData =  ds.Tables[0].Rows[i]["CustName"].ToString();
            //    }
            //    else
            //    {
            //        strCustData =strCustData+":"+ds.Tables[0].Rows[i]["CustName"].ToString();
            //    }
            //}
           
        }
        catch (Exception)
        {
        }
        return strCustData;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public string FindBookingNumber(string bookingNumber, string BID)
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr = null;
        string status = "false";
        try
        {
            cmd.CommandText = "sp_Dry_DrawlMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookingNumber", bookingNumber);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 33);
            sdr = PrjClass.ExecuteReader(cmd);
            if (sdr.Read())
                status = "true";
            else
                status = "false";
        }
        catch (Exception ex) { }
        finally
        {
            sdr.Close();
            sdr.Dispose();
            cmd.Dispose();
        }
        return status;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetUserDetailsData(string Status)
    {
        string strData = string.Empty;
        string strUserAndTotalGarment = string.Empty;       

        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        try
        {
            cmd.CommandText = "Proc_NewChallanXML";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@cstatus", Status);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strUserAndTotalGarment = strUserAndTotalGarment + ds.Tables[0].Rows[i]["UserName"].ToString() + "," + ds.Tables[0].Rows[i]["TotalGarment"].ToString() + "@";
                }
                strUserAndTotalGarment = strUserAndTotalGarment.Substring(0, strUserAndTotalGarment.Length - 1);               
            }
            strData = strUserAndTotalGarment;
        }
        catch (Exception)
        {
        }
        return strData;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetUserDetailsDataForWorkshop(string Status)
    {
        string strData = string.Empty;
        string strUserAndTotalGarment = string.Empty;

        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        try
        {
            cmd.CommandText = "Proc_NewChallanXML";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@cstatus", Status);
            cmd.Parameters.AddWithValue("@Flag", 11);
            ds = PrjClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strUserAndTotalGarment = strUserAndTotalGarment + ds.Tables[0].Rows[i]["UserName"].ToString() + "," + ds.Tables[0].Rows[i]["TotalGarment"].ToString() + "@";
                }
                strUserAndTotalGarment = strUserAndTotalGarment.Substring(0, strUserAndTotalGarment.Length - 1);
            }
            strData = strUserAndTotalGarment;
        }
        catch (Exception)
        {
        }
        return strData;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string CheckFectory()
    {
        SqlCommand con = new SqlCommand();       
        DataSet dsFactory = new DataSet();
        string strFactory = "False";
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_BranchMaster";
            cmd.CommandType = CommandType.StoredProcedure;          
            cmd.Parameters.AddWithValue("@Flag", 14);
            dsFactory = AppClass.GetData(cmd);
            if (dsFactory.Tables[0].Rows.Count > 0)
            {
                int NoCount = Convert.ToInt32(dsFactory.Tables[0].Rows[0]["NoOfFactory"].ToString());
                if (NoCount > 0)
                {
                    strFactory = "True";
                }
                else
                {
                    strFactory = "False";                   
                }
            }
            else
            {
                strFactory = "False";            
            }
        }
        catch (Exception)
        {
        }
        return strFactory;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string GetStoreUserPassword(string Password)
    {
        string strPwd = "False";
        DataSet ds = new DataSet();
        SqlCommand CMD = new SqlCommand();
        try
        {
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@UserName", Globals.UserName);
            CMD.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            CMD.Parameters.AddWithValue("@Flag", 18);
            ds = PrjClass.GetData(CMD);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["UserPassword"]) == Password)
                {
                    strPwd = "True";
                }
                else
                {
                    strPwd = "False";
                }

            }
        }
        catch (Exception)
        {
        }
        return strPwd;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public string CheckCompareTime(string startTime,string EndTime)
    {
        string strTime = "False";
        DataSet ds = new DataSet();
        SqlCommand CMD = new SqlCommand();
        try
        {
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_BranchMaster";
            CMD.Parameters.AddWithValue("@StartTime", startTime);
            CMD.Parameters.AddWithValue("@EndTime", EndTime);
            CMD.Parameters.AddWithValue("@Flag", 18);
            ds = PrjClass.GetData(CMD);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["LoginTimeValue"]) == "True")
                {
                    strTime = "True";
                }
                else
                {
                    strTime = "False";
                }

            }
        }
        catch (Exception)
        {
        }
        return strTime;
    }
   [WebMethod(EnableSession = true)]
   public string[] GetUserIDCompletionList(string prefixText, int count)
   {
        if (count == 0)
        {
            count = 10;
        }
        Random random = new Random();
        List<string> items = new List<string>(count);
        SqlCommand con = new SqlCommand();
        SqlDataReader sdr = null;       
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_LoginHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@UserID", prefixText.ToString().Trim());
            cmd.Parameters.AddWithValue("@Flag", 2);
            sdr = AppClass.ExecuteReader(cmd);           
            while (sdr.Read())
            {
                items.Add("" + sdr.GetValue(0));
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
        }       
        return items.ToArray();
    }

   [WebMethod(EnableSession = true)]
   public string[] GetReasonList(string prefixText, int count)
   {
       if (count == 0)
       {
           count = 10;
       }
       Random random = new Random();
       List<string> items = new List<string>(count);
       SqlCommand con = new SqlCommand();
       SqlDataReader sdr = null;
       try
       {
           SqlCommand cmd = new SqlCommand();

           cmd.CommandText = "sp_LoginHistory";
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
           cmd.Parameters.AddWithValue("@UserID", prefixText.ToString().Trim());
           cmd.Parameters.AddWithValue("@Flag", 3);
           sdr = AppClass.ExecuteReader(cmd);
           while (sdr.Read())
           {
               items.Add("" + sdr.GetValue(0));
           }
           sdr.Close();
           sdr.Dispose();
       }
       catch (Exception)
       {
       }
       return items.ToArray();
   }
   [WebMethod(EnableSession = true)]
   public string[] GetUserName(string prefixText, int count)
   {
       if (count == 0)
       {
           count = 10;
       }
       Random random = new Random();
       List<string> Users = new List<string>(count);
       SqlCommand con = new SqlCommand();
       SqlDataReader sdr = null;
       try
       {
           SqlCommand cmd = new SqlCommand();

           cmd.CommandText = "Sp_Sel_QuantityandBooking";
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
           cmd.Parameters.AddWithValue("@UserID", prefixText.ToString().Trim());
           cmd.Parameters.AddWithValue("@Flag", 12);
           sdr = AppClass.ExecuteReader(cmd);
           while (sdr.Read())
           {
               Users.Add("" + sdr.GetValue(0));
           }
           sdr.Close();
           sdr.Dispose();
       }
       catch (Exception)
       {
       }
       return Users.ToArray();
   }

   [WebMethod(EnableSession = true)]
   [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
   public string GetAmoutWithTax(string Cost)
   {      
       DataSet dsTaxDetail = new DataSet();
       float actualCost = float.Parse(Cost);
       var computedCost = 0.0;
       try
       {          
           dsTaxDetail = BAL.BALFactory.Instance.BAL_Shift.GetTaxDetails(Globals.BranchID);
            computedCost =
                       actualCost // the actual cost
                       +
                       actualCost * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate1"].ToString())) / 100 // first tax
                       +
                       actualCost * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate1"].ToString()) / 100) * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate2"].ToString())) / 100 // second tax
                       +
                       actualCost * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate1"].ToString()) / 100) * (float.Parse(dsTaxDetail.Tables[0].Rows[0]["ServiceTaxRate3"].ToString())) / 100; // third tax
       }
       catch (Exception)
       {
       }
       return computedCost.ToString();
   }
   [WebMethod(EnableSession = true)]
   [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
   public bool IsPackaAssignToCustomer(string CustCode)
   {
       SqlCommand cmd = new SqlCommand();
       SqlDataReader sdr = null;
       bool status = false;
       try
       {
           cmd.CommandText = "sp_PackageMaster";
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@CustomerCode", CustCode);
           cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
           cmd.Parameters.AddWithValue("@Flag", 14);
           sdr = PrjClass.ExecuteReader(cmd);
           if (sdr.Read())
               status = true;
           else
               status = false;
       }
       catch (Exception ex) { }
       finally
       {
           sdr.Close();
           sdr.Dispose();
           cmd.Dispose();
       }
       return status;
       
   }
   [WebMethod(EnableSession = true)]
   [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
   public void InsertInvoiceHistory(string BookingNo, string ItemRowIndex)
   {
       string res = string.Empty;
       string BID = Globals.BranchID;
       string UserName = Globals.UserName;
       Task t = Task.Factory.StartNew
       (
       () => { BAL.BALFactory.Instance.BAL_New_Bookings.InsertInvoiceHistoryData(BookingNo, ItemRowIndex, BID, UserName); }
       );
   }
}