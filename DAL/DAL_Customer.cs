using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System;
using System.Web.UI.WebControls;
using System.Collections;

namespace DAL
{
    public class DAL_Customer
    {
        public string SaveNewCustomer(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "";
            if (CheckDuplicateCustomer(Ob) == true)
            {
                res = "Entered Customer details already exists. Enter New Detail.";
                return res;
            }
            if (CheckDuplicateMobileNo(Ob) == true)
            {
                res = "Entered Customer mobile no already exists.";
                return res;
            }
            if (CheckDuplicateMemberShipId(Ob) == true)
            {
                res = "Membership ID is already in Use, Please enter new ID.";
                return res;
            }
            if (CheckDuplicateBarcode(Ob) == true)
            {
                res = "Barcode is already in use, Please change the barcode.";
                return res;
            }
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", "");
            cmd.Parameters.AddWithValue("@CustomerSalutation", Ob.CustomerSalutation);
            cmd.Parameters.AddWithValue("@CustomerName", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", Ob.CustomerAddress);
            cmd.Parameters.AddWithValue("@CustomerPhone", Ob.CustomerPhone);
            cmd.Parameters.AddWithValue("@CustomerMobile", Ob.CustomerMobile);
            cmd.Parameters.AddWithValue("@CustomerEmailId", Ob.CustomerEmailId);
            cmd.Parameters.AddWithValue("@CustomerPriority", Ob.CustomerPriority);
            cmd.Parameters.AddWithValue("@CustomerRefferredBy", Ob.CustomerRefferredBy);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", Ob.DefaultDiscountRate);
            cmd.Parameters.AddWithValue("@Remarks", Ob.Remarks);
            cmd.Parameters.AddWithValue("@BirthDate", Ob.BirthDate);
            cmd.Parameters.AddWithValue("@City", Ob.City);
            cmd.Parameters.AddWithValue("@AreaLocation", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Area", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Profession", Ob.Profession);
            cmd.Parameters.AddWithValue("@CommunicationMeans", Ob.CommunicationMeans);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@IsWebsite", Ob.IsWebsite);
            cmd.Parameters.AddWithValue("@MemberShipId", Ob.MemberShipId);
            cmd.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            cmd.Parameters.AddWithValue("@RateListId", int.Parse(Ob.RateListId));
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }

        public string UpdateCustomer(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "";
            //if ((!string.IsNullOrEmpty(Ob.PrevBarcode) && Ob.PrevBarcode != Ob.BarCode) || (!string.IsNullOrEmpty(Ob.PrevMemberId) && Ob.PrevMemberId != Ob.BarCode))
            //{
            //    if (!CheckUniqueMemberShipId(ref Ob))
            //    {
            //        res = "Either membership id or barcode already exists. Enter New Detail.";
            //        return res;
            //    }
            //}
            if (Ob.PrevMobile != Ob.CustomerMobile)
            {
                if (CheckDuplicateMobileNo(Ob) == true)
                {
                    res = "Entered Customer mobile no already exists.";
                    return res;
                }
            }
            if (Ob.PrevMemberId != Ob.MemberShipId)
            {
                if (CheckDuplicateMemberShipId(Ob) == true)
                {
                    res = "Membership ID is already in Use, Please enter new ID.";
                    return res;
                }
            }
            if (Ob.PrevBarcode != Ob.BarCode)
            {
                if (CheckDuplicateBarcode(Ob) == true)
                {
                    res = "Barcode is already in use, Please change the barcode.";
                    return res;
                }
            }
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@CustomerSalutation", Ob.CustomerSalutation);
            cmd.Parameters.AddWithValue("@CustomerName", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", Ob.CustomerAddress);
            cmd.Parameters.AddWithValue("@CustomerPhone", Ob.CustomerPhone);
            cmd.Parameters.AddWithValue("@CustomerMobile", Ob.CustomerMobile);
            cmd.Parameters.AddWithValue("@CustomerEmailId", Ob.CustomerEmailId);
            cmd.Parameters.AddWithValue("@CustomerPriority", Ob.CustomerPriority);
            cmd.Parameters.AddWithValue("@CustomerRefferredBy", Ob.CustomerRefferredBy);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", Ob.DefaultDiscountRate);
            cmd.Parameters.AddWithValue("@Remarks", Ob.Remarks);
            cmd.Parameters.AddWithValue("@BirthDate", Ob.BirthDate);
            cmd.Parameters.AddWithValue("@AnniversaryDate", Ob.AnniversaryDate);
            cmd.Parameters.AddWithValue("@City", Ob.City);
            cmd.Parameters.AddWithValue("@AreaLocation", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Area", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Profession", Ob.Profession);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@IsWebsite", Ob.IsWebsite);
            cmd.Parameters.AddWithValue("@MemberShipId", Ob.MemberShipId);
            cmd.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            cmd.Parameters.AddWithValue("@RateListId", int.Parse(Ob.RateListId));
            cmd.Parameters.AddWithValue("@Flag", 3);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }

        private bool CheckDuplicateCustomer(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", Ob.CustomerName);
                cmd.Parameters.AddWithValue("@CustomerAddress", Ob.CustomerAddress);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 2);
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
        public bool CheckVerificationCode(string Code, string cookies, string BID)
        {
            string returnCode = string.Empty;
            bool status = false;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            try
            {
                cmd.CommandText = "proc_BindToMachine";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MacId", cookies);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 7);
                sdr = PrjClass.ExecuteReader(cmd);
                if (sdr.Read())
                {
                    returnCode = sdr.GetValue(0).ToString();
                    if (string.Equals(returnCode, Code))
                    {
                        VerifiedTrue(cookies,BID);
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                    status = false;
            }
            catch (Exception ex) { }
            finally
            {
                sdr.Dispose();
                sdr.Close();
                cmd.Dispose();
            }
            return status;
        }
        private void VerifiedTrue(string cookies,string BID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MacId", cookies);           
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 8);
            PrjClass.ExecuteNonQuery(cmd);
        }
        private string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }
        private void UpdateSucessTrue(string MacAdress,string BID)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MacId", MacAdress);
            cmd.Parameters.AddWithValue("@LastLoginDate", date[0]);
            cmd.Parameters.AddWithValue("@LastLoginTime", date[1]);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 5);
            PrjClass.ExecuteNonQuery(cmd);
        }
        public string DeleteBoundToMachineDetails(int Id)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
        public string GetRandomNumber()
        {
            Random random = new Random();
            int value = random.Next(10000);
            string text = value.ToString("000120");
            return text;
        }
        public string CreatePasswordToBoundMachine(string DeviceName,string cookies,string BID)
        {
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(BID);
            string res = string.Empty;           
            string generateCode = string.Empty;
            generateCode = GetRandomNumber();           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MacId", cookies);
            cmd.Parameters.AddWithValue("@SMSText", generateCode);
            cmd.Parameters.AddWithValue("@DeviceName", DeviceName);
            cmd.Parameters.AddWithValue("@Verified", false);
            cmd.Parameters.AddWithValue("@CreationDate", date[0]);
            cmd.Parameters.AddWithValue("@LastLoginDate", date[0]);
            cmd.Parameters.AddWithValue("@CreationTime", date[1]);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 3);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public bool CheckBoundToMachine(string cookies,string BID)
        {
            DataSet ds2 = new DataSet();
            bool status = false;
            string macAdress = string.Empty;
            string BoundtoMachine = string.Empty;
            //macAdress = GetMACAddress();
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "proc_BindToMachine";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@BranchId", BID);
            cmd1.Parameters.AddWithValue("@Flag", 6);
            ds2 = PrjClass.GetData(cmd1);
            bool IsBountactive = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsBoundToMachine"].ToString());
            bool IsonCloudactive = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsBackupActive"].ToString());
            if (IsBountactive)
            {
                if (IsonCloudactive)
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader sdr = null;
                    try
                    {
                        cmd.CommandText = "proc_BindToMachine";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MacId", cookies);
                        cmd.Parameters.AddWithValue("@BranchId", BID);
                        cmd.Parameters.AddWithValue("@Flag", 4);
                        sdr = PrjClass.ExecuteReader(cmd);
                        if (sdr.Read())
                        {
                            BoundtoMachine = sdr.GetValue(0).ToString();
                            if (BoundtoMachine == "True")
                            {
                                UpdateSucessTrue(cookies,BID);
                                status = true;
                            }
                            else
                            {
                                status = false;
                            }
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        sdr.Dispose();
                        sdr.Close();
                        cmd.Dispose();
                    }
                }
                else
                {
                   // UpdateSucessTrue(cookies);
                    status = true;
                }
            }
            else
            {
                status = true;
            }
            return status;
        }
        private bool CheckDuplicateMemberShipId(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberShipId", Ob.MemberShipId);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 20);
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
        private bool CheckDuplicateBarcode(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", Ob.BarCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 21);
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
        public bool CheckCustomerPackageActive(string CustCode, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", CustCode);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 22);
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
        public DataSet BindGridSearch(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerName", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", Ob.CustomerAddress);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 14);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet GetBoundToMachineDetails(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet GetData(string BID)
        {
            string query = "SELECT [CustomerCode], COALESCE(CustomerSalutation,'') + ' ' + [CustomerName] As CustomerName, [CustomerAddress], [CustomerPhone], [CustomerMobile], [CustomerEmailId], [Priority], [CustomerRefferredBy],round([DefaultDiscountRate],2) as DefaultDiscountRate,(Case When [IsWebsite]='True' Then 'Yes' Else 'No' End) As IsWebsite  FROM [CustomerMaster] LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityId AND CustomerMaster.BranchId = dbo.PriorityMaster.BranchId  WHERE CustomerMaster.BranchId='" + BID + "' AND PriorityMaster.BranchId='" + BID + "' order by ID desc";
            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(PrjClass.sqlConStr))
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                else
                {
                    con.Open();
                }
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }
        public DataSet BindGrid(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 5);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet FillTextBoxes(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet CheckDiscountPackage(DTO.CustomerMaster Ob)
        {

            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 23);
            ds = PrjClass.GetData(cmd);

            return ds;


        }
        public string DeleteCustomer(DTO.CustomerMaster Ob)
        {
            string res = "";
            if (CheckCustomerRecordIntheBookingTable(Ob) == true)
            {
                res = "booking(s) have been found for the selected customer. Customer can not be removed.";
                return res;
            }
            else if (CheckCustomerRecordIntheLedgerEntries(Ob) == true)
            {
                res = "ledger transaction(s) have been found for the selected customer. Customer can not be removed.";
                return res;
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 8);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        private bool CheckCustomerRecordIntheBookingTable(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 6);
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

        private bool CheckCustomerRecordIntheLedgerEntries(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 6);
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

        public string SavePriority(DTO.CustomerMaster Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Priority";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Priority", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ExportToExcel(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 9);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindPriority(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string SaveNewUser(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "";
            if (CheckDuplicateCustomer(Ob) == true)
            {
                res = "Entered Customer details already exists. Enter New Detail.";
                return res;
            }
            if (CheckDuplicateMobileNo(Ob) == true)
            {
                res = "Entered Customer mobile no already exists.";
                return res;
            }
            if (CheckDuplicateMemberShipId(Ob) == true)
            {
                res = "Membership ID is already in Use, Please enter new ID.";
                return res;
            }
            if (CheckDuplicateBarcode(Ob) == true)
            {
                res = "Barcode is already in use, Please change the barcode.";
                return res;
            }

            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", "");
            cmd.Parameters.AddWithValue("@CustomerSalutation", Ob.CustomerSalutation);
            cmd.Parameters.AddWithValue("@CustomerName", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", Ob.CustomerAddress);
            cmd.Parameters.AddWithValue("@CustomerPhone", Ob.CustomerPhone);
            cmd.Parameters.AddWithValue("@CustomerMobile", Ob.CustomerMobile);
            cmd.Parameters.AddWithValue("@CustomerEmailId", Ob.CustomerEmailId);
            cmd.Parameters.AddWithValue("@CustomerPriority", Ob.CustomerPriority);
            cmd.Parameters.AddWithValue("@CustomerRefferredBy", Ob.CustomerRefferredBy);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", Ob.DefaultDiscountRate);
            cmd.Parameters.AddWithValue("@Remarks", Ob.Remarks);
            cmd.Parameters.AddWithValue("@BirthDate", Ob.BirthDate);
            cmd.Parameters.AddWithValue("@AnniversaryDate", Ob.AnniversaryDate);
            cmd.Parameters.AddWithValue("@City", Ob.City);
            cmd.Parameters.AddWithValue("@AreaLocation", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Area", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Profession", Ob.Profession);
            cmd.Parameters.AddWithValue("@CommunicationMeans", Ob.CommunicationMeans);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@IsWebsite", Ob.IsWebsite);
            cmd.Parameters.AddWithValue("@MemberShipId", Ob.MemberShipId);
            cmd.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            cmd.Parameters.AddWithValue("@RateListId", int.Parse(Ob.RateListId));
            cmd.Parameters.AddWithValue("@Flag", 12);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }

        public string UpdateWebCustomer(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "";
            if (Ob.PrevMobile != Ob.CustomerMobile)
            {
                if (CheckDuplicateMobileNo(Ob) == true)
                {
                    res = "Entered Customer mobile no already exists.";
                    return res;
                }
            }
            if (Ob.PrevMemberId != Ob.MemberShipId)
            {
                if (CheckDuplicateMemberShipId(Ob) == true)
                {
                    res = "Membership ID is already in Use, Please enter new ID.";
                    return res;
                }
            }
            if (Ob.PrevBarcode != Ob.BarCode)
            {
                if (CheckDuplicateBarcode(Ob) == true)
                {
                    res = "Barcode is already in use, Please change the barcode.";
                    return res;
                }
            }
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@CustomerSalutation", Ob.CustomerSalutation);
            cmd.Parameters.AddWithValue("@CustomerName", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", Ob.CustomerAddress);
            cmd.Parameters.AddWithValue("@CustomerPhone", Ob.CustomerPhone);
            cmd.Parameters.AddWithValue("@CustomerMobile", Ob.CustomerMobile);
            cmd.Parameters.AddWithValue("@CustomerEmailId", Ob.CustomerEmailId);
            cmd.Parameters.AddWithValue("@CustomerPriority", Ob.CustomerPriority);
            cmd.Parameters.AddWithValue("@CustomerRefferredBy", Ob.CustomerRefferredBy);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", Ob.DefaultDiscountRate);
            cmd.Parameters.AddWithValue("@Remarks", Ob.Remarks);
            cmd.Parameters.AddWithValue("@BirthDate", Ob.BirthDate);
            cmd.Parameters.AddWithValue("@AnniversaryDate", Ob.AnniversaryDate);
            cmd.Parameters.AddWithValue("@City", Ob.City);
            cmd.Parameters.AddWithValue("@AreaLocation", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Area", Ob.AreaLocation);
            cmd.Parameters.AddWithValue("@Profession", Ob.Profession);
            cmd.Parameters.AddWithValue("@CommunicationMeans", Ob.CommunicationMeans);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@IsWebsite", Ob.IsWebsite);
            cmd.Parameters.AddWithValue("@MemberShipId", Ob.MemberShipId);
            cmd.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            cmd.Parameters.AddWithValue("@RateListId", int.Parse(Ob.RateListId));
            cmd.Parameters.AddWithValue("@Flag", 13);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }

        public string MergeDuplicateCustomer(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            string res = "";
            if (CheckCustomer(Ob.CustomerSalutation, Ob.BranchId) == false)
            {
                res = "Main customer not correct";
                return res;
            }
            if (CheckCustomer(Ob.CustomerName, Ob.BranchId) == false)
            {
                res = "Duplicate customer not correct";
                return res;
            }
            if (CheckCustomerInAssignTable(Ob.CustomerSalutation, Ob.BranchId) == true)
            {
                res = GetCustNameFromCode(Ob.CustomerSalutation, Ob.BranchId);
                res = res + " " + "has a Package Assigned, so it can't be merged.";
                return res;
            }
            if (CheckCustomerInAssignTable(Ob.CustomerName, Ob.BranchId) == true)
            {
                res = GetCustNameFromCode(Ob.CustomerName, Ob.BranchId);
                res = res + " " + "has a Package Assigned, so it can't be merged.";
                return res;
            }
            cmd.CommandText = "sp_MergeDuplicateCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MainCustomer", Ob.CustomerSalutation);
            cmd.Parameters.AddWithValue("@DuplicateCustomer", Ob.CustomerName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }

        private bool CheckCustomer(string CustCode, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_MergeDuplicateCustomer";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DuplicateCustomer", CustCode);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 2);
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

        private bool CheckCustomerInAssignTable(string CustCode, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_MergeDuplicateCustomer";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DuplicateCustomer", CustCode);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 3);
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

        public DataSet BindGridCustomerSearch(string CustName, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_AccountEntries";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", CustName);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string GetCustNameFromCode(string custCode, string BID)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_CustomerMaster",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CustomerCode", custCode);
            sqlCommand.Parameters.AddWithValue("@BranchId", BID);
            sqlCommand.Parameters.AddWithValue("@Flag", 16);
            return PrjClass.ExecuteScalar(sqlCommand);
        }

        public bool CheckUniqueMemberShipId(ref DTO.CustomerMaster Obj)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = "sp_CustomerMaster",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@MemberShipId", Obj.MemberShipId);
            sqlCommand.Parameters.AddWithValue("@BarCode", Obj.BarCode);
            sqlCommand.Parameters.AddWithValue("@BranchId", Obj.BranchId);
            sqlCommand.Parameters.AddWithValue("@CustomerCode", Obj.CustomerCode);
            sqlCommand.Parameters.AddWithValue("@Flag", 17);
            return bool.Parse(PrjClass.ExecuteScalar(sqlCommand).ToString());
        }

        public string CheckPrevious(DTO.CustomerMaster Ob)
        {
            var res = string.Empty;
            if ((!string.IsNullOrEmpty(Ob.PrevBarcode) && Ob.PrevBarcode != Ob.BarCode) || (!string.IsNullOrEmpty(Ob.PrevMemberId) && Ob.PrevMemberId != Ob.MemberShipId))
            {
                if (!CheckUniqueMemberShipId(ref Ob))
                {
                    res = "Either membership id or barcode already exists. Enter New Detail.";
                }
            }
            /*
            if ((Ob.PrevBarcode != Ob.BarCode) || (Ob.MemberShipId != Ob.PrevMemberId))
            {
                res = "Membership id or barcode cannot be changed.";
            }*/
            return res;
        }

        public string UpdateCustomerDetailJQuery(string CustomerCode, string CustomerSalutation, string CustomerName, string CustomerAddress, string CustomerPhone, string CustomerMobile, string CustomerEmailId, string CustomerPriority, string CustomerRefferredBy, string DefaultDiscountRate, string Remarks, string BirthDate, string AnniversaryDate, string AreaLocation, string CommunicationMeans, string MemberShipId, string BarCode, string RateListId, bool IsWebsite, string tempCustMobile, string tempMemberShipId, string tempBarCode, string BID)
        {
            DTO.CustomerMaster Ob = new DTO.CustomerMaster();
            Ob.CustomerMobile = CustomerMobile;
            Ob.MemberShipId = MemberShipId;
            Ob.BarCode = BarCode;
            Ob.BranchId = BID;
            SqlCommand cmd = new SqlCommand();
            string res = "";
            if (tempCustMobile != CustomerMobile)
            {
                if (CheckDuplicateMobileNo(Ob) == true)
                {
                    res = "Mobile no already in use.";
                    return res;
                }
            }
            if (tempMemberShipId != MemberShipId)
            {
                if (CheckDuplicateMemberShipId(Ob) == true)
                {
                    res = "Membership ID is already in Use, Please enter new ID.";
                    return res;
                }
            }
            if (tempBarCode != BarCode)
            {
                if (CheckDuplicateBarcode(Ob) == true)
                {
                    res = "Barcode is already in use, Please change the barcode.";
                    return res;
                }
            }

            //SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", CustomerCode);
            cmd.Parameters.AddWithValue("@CustomerSalutation", CustomerSalutation);
            cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", CustomerAddress);
            cmd.Parameters.AddWithValue("@CustomerPhone", CustomerPhone);
            cmd.Parameters.AddWithValue("@CustomerMobile", CustomerMobile);
            cmd.Parameters.AddWithValue("@CustomerEmailId", CustomerEmailId);
            cmd.Parameters.AddWithValue("@CustomerPriority", CustomerPriority);
            cmd.Parameters.AddWithValue("@CustomerRefferredBy", CustomerRefferredBy);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", (DefaultDiscountRate == "" ? 0 : float.Parse(DefaultDiscountRate)));
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@BirthDate", BirthDate);
            cmd.Parameters.AddWithValue("@AnniversaryDate", AnniversaryDate);
            cmd.Parameters.AddWithValue("@City", "");
            cmd.Parameters.AddWithValue("@AreaLocation", AreaLocation);
            cmd.Parameters.AddWithValue("@Area", AreaLocation);
            cmd.Parameters.AddWithValue("@Profession", "");
            cmd.Parameters.AddWithValue("@CommunicationMeans", CommunicationMeans);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@IsWebsite", IsWebsite);
            cmd.Parameters.AddWithValue("@MemberShipId", MemberShipId);
            cmd.Parameters.AddWithValue("@BarCode", BarCode);
            cmd.Parameters.AddWithValue("@RateListId", int.Parse(RateListId));
            cmd.Parameters.AddWithValue("@Flag", 13);
            cmd.CommandTimeout = 120;
            var priorityNRemarks = PrjClass.ExecuteNonQuery(cmd);
            return priorityNRemarks;
        }

        public string ResestWebsite(string arg, string BID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@CustomerCode", arg);
            cmd.Parameters.AddWithValue("@Flag", 24);
            var resetpassword = PrjClass.ExecuteNonQuery(cmd);
            return resetpassword;

        }

        public string SaveNewCustomerDetailJquery(string CustomerSalutation, string CustomerName, string CustomerAddress, string CustomerPhone, string CustomerMobile, string CustomerEmailId, string CustomerPriority, string CustomerRefferredBy, string DefaultDiscountRate, string Remarks, string BirthDate, string AnniversaryDate, string AreaLocation, string CommunicationMeans, string MemberShipId, string BarCode, string RateListId, bool IsWebsite, string BID)
        {

            SqlCommand cmd = new SqlCommand();
            string res = "";

            DTO.CustomerMaster Ob = new DTO.CustomerMaster();
            Ob.CustomerName = CustomerName;
            Ob.CustomerAddress = CustomerAddress;
            Ob.CustomerMobile = CustomerMobile;
            Ob.MemberShipId = MemberShipId;
            Ob.BarCode = BarCode;
            Ob.BranchId = BID;

            if (CheckDuplicateCustomer(Ob) == true)
            {
                res = "Entered details already exists. Enter New Detail.";
                return res;
            }
            if (CheckDuplicateMobileNo(Ob) == true)
            {
                res = "Mobile no already in use.";
                return res;
            }
            if (CheckDuplicateMemberShipId(Ob) == true)
            {
                res = "Membership ID is already in Use, Please enter new ID.";
                return res;
            }
            if (CheckDuplicateBarcode(Ob) == true)
            {
                res = "Barcode is already in use, Please change the barcode.";
                return res;
            }
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", "");
            cmd.Parameters.AddWithValue("@CustomerSalutation", CustomerSalutation);
            cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
            cmd.Parameters.AddWithValue("@CustomerAddress", CustomerAddress);
            cmd.Parameters.AddWithValue("@CustomerPhone", CustomerPhone);
            cmd.Parameters.AddWithValue("@CustomerMobile", CustomerMobile);
            cmd.Parameters.AddWithValue("@CustomerEmailId", CustomerEmailId);
            cmd.Parameters.AddWithValue("@CustomerPriority", CustomerPriority);
            cmd.Parameters.AddWithValue("@CustomerRefferredBy", CustomerRefferredBy);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", (DefaultDiscountRate == "" ? 0 : float.Parse(DefaultDiscountRate)));
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@BirthDate", BirthDate);
            cmd.Parameters.AddWithValue("@City", "");
            cmd.Parameters.AddWithValue("@AreaLocation", AreaLocation);
            cmd.Parameters.AddWithValue("@Area", AreaLocation);
            cmd.Parameters.AddWithValue("@Profession", "");
            cmd.Parameters.AddWithValue("@CommunicationMeans", CommunicationMeans);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@IsWebsite", IsWebsite);
            cmd.Parameters.AddWithValue("@MemberShipId", MemberShipId);
            cmd.Parameters.AddWithValue("@BarCode", BarCode);
            cmd.Parameters.AddWithValue("@RateListId", int.Parse(RateListId));
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);

            return res;
        }


        public string DeleteCustomerDetailJQuery(string CustomerCode, string BID)
        {
            string res = "";
            DTO.CustomerMaster Ob = new DTO.CustomerMaster();
            Ob.CustomerCode = CustomerCode;
            Ob.BranchId = BID;

            if (CheckCustomerRecordIntheBookingTable(Ob) == true)
            {
                res = "Selected customer is in use hence can't be deleted.";
                return res;
            }
            //else if (CheckCustomerRecordIntheLedgerEntries(Ob) == true)
            //{
            //    res = "ledger transaction(s) have been found for the selected customer. Customer can not be removed.";
            //    return res;
            //}
            else
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 8);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        public string BoundToMachineCheck(bool IsBoundToMachine, string BID)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IsBoundToMachine", IsBoundToMachine);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 10);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet GetPassword(string cookies,string BID)
        {          
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MacId", cookies);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        private bool CheckDuplicateMobileNo(DTO.CustomerMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_CustomerMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerMobile", Ob.CustomerMobile);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 19);
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

        public DataSet BindDropDown()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@Flag", 2);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet GetStatus(string BID, string UserName, string Password)
        {
            DataSet ds = new DataSet();
            SqlCommand CMD = new SqlCommand();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@UserName", UserName);
            CMD.Parameters.AddWithValue("@UserPassword", Password);
            CMD.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public string UpdatePassword(string BID, string UserName, string Password, string NewPassword)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_Registration";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@UserPassword", Password);
            cmd.Parameters.AddWithValue("@UserNewPassword", NewPassword);
            cmd.Parameters.AddWithValue("@Flag", 5);
            res = PrjClass.ExecuteNonQuery(cmd);


            return res;
        }

        public DataSet FindCustomerName(string BID, string CustID)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Proc_Registration";
            CMD.Parameters.AddWithValue("@CustomerCode", CustID);
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 6);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet BindGridWebsiteCustomerSearch(string BID, string CustName)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_Sel_QuantityandBooking";
            cmd.Parameters.AddWithValue("@CustId", CustName);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 10);

            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindDeliveryStatus(string BookingNumber, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeliveryStatus";
            cmd.Parameters.AddWithValue("@BookingNumber", BookingNumber);
            cmd.Parameters.AddWithValue("@BranchId", BID);


            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public bool bountToMachineStatus(string BID)
        {
            DataSet ds2 = new DataSet();
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "proc_BindToMachine";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@BranchId", BID);
            cmd1.Parameters.AddWithValue("@Flag", 6);
            ds2 = PrjClass.GetData(cmd1);
            bool IsBountactive = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsBoundToMachine"].ToString());
            return IsBountactive;
        }

        public DataSet GetGarmentStatusDetail(string Date, string BID)
        {         
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReadyAndDeliveryStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.Parameters.AddWithValue("@Flag", 1);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet GetPendingStatusDetail(string Date, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReadyAndDeliveryStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.Parameters.AddWithValue("@Flag", 2);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteBoundToMachineData(string  BID)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 14);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
        public DataSet GetStoreEmaiAndMobile(string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "proc_BindToMachine";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 15);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
        public DataSet BindGridLoginHistory(string strFromDate, string strToDate, string UserID,string Reason ,string Status,string BID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_LoginHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", strFromDate);
            cmd.Parameters.AddWithValue("@Todate", strToDate);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@ReasonID", Reason);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 4);
            ds = PrjClass.GetData(cmd);
            return ds;
        }
    }
}