using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;

namespace DAL
{
    public class DAL_PackageMaster
    {
        public string SavePackage(DTO.PackageMaster Ob, GridView grdQtyDetail)
        {
            string res = "", res1 = "";
            int TotalQty = 0;
            double PackageRat = 0;
            if (CheckDuplicatePackage(Ob) == true)
            {
                res = "Package " + Ob.PackageName + " already exists.";
                return res;
            }
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            int PackageId = PrjClass.getNewIDAccordingBID("mstPackageMaster", "PackageId", Ob.BranchId);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PackageId", PackageId);
            cmd.Parameters.AddWithValue("@PackageName", Ob.PackageName.Trim());
            cmd.Parameters.AddWithValue("@PackageType", Ob.PackageType);
            cmd.Parameters.AddWithValue("@PackageCost", Ob.PackageCost);
            cmd.Parameters.AddWithValue("@BenefitType", Ob.BenefitType);
            cmd.Parameters.AddWithValue("@BenefitValue", Ob.BenefitValue);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@CreateDate", date[0]);
            cmd.Parameters.AddWithValue("@TaxType", Ob.TaxType);
            cmd.Parameters.AddWithValue("@TotalQty", Ob.TotalQty);

            cmd.Parameters.AddWithValue("@Recurrence", Ob.Recurrence);
            cmd.Parameters.AddWithValue("@StartDate", Ob.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", Ob.EndDate);

            if (grdQtyDetail.Rows.Count > 0)
            {
                if (Ob.PackageType != "Flat Qty")
                {
                    for (int iRow = 0; iRow < grdQtyDetail.Rows.Count; iRow++)
                    {
                        TotalQty += Convert.ToInt32(((Label)grdQtyDetail.Rows[iRow].Cells[1].FindControl("lblQty")).Text);
                    }
                }
                else
                {
                    TotalQty = Ob.TotalQty;
                }
                PackageRat = Ob.PackageCost / (TotalQty * int.Parse(Ob.Recurrence));
            }
            cmd.Parameters.AddWithValue("@PackageRate", Math.Round(PackageRat, 2));
            cmd.Parameters.AddWithValue("@Flag", 1);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (Ob.PackageType != "Qty / Item" && Ob.PackageType != "Flat Qty")
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "sp_PackageMaster";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@PackageId", PackageId);
                cmd1.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd1.Parameters.AddWithValue("@Flag", 41);
                PrjClass.ExecuteNonQuery(cmd1);
                grdQtyDetail.DataSource = null;
                grdQtyDetail.DataBind();
            }
            if (grdQtyDetail.Rows.Count > 0)
            {
                res1 = SaveInPackageQtyDetail(grdQtyDetail, PackageId, Ob.BranchId, Ob.TotalQty, Ob.PackageType);
            }
            return res;
        }

        private string SaveInPackageQtyDetail(GridView grdQtyDetail, int PackageId, string BID, int TotalQty, string PackageType)
        {
            string res = string.Empty;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "sp_PackageMaster";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@PackageId", PackageId);
            cmd1.Parameters.AddWithValue("@BranchId", BID);
            cmd1.Parameters.AddWithValue("@Flag", 41);
            PrjClass.ExecuteNonQuery(cmd1);
            for (int iRow = 0; iRow < grdQtyDetail.Rows.Count; iRow++)
            {
                if (((Label)grdQtyDetail.Rows[iRow].Cells[0].FindControl("lblSrNo")).Text != "0")
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_PackageMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PackageId", PackageId);
                    cmd.Parameters.AddWithValue("@SNo", ((Label)grdQtyDetail.Rows[iRow].Cells[0].FindControl("lblSrNo")).Text);
                    cmd.Parameters.AddWithValue("@ItemName", ((Label)grdQtyDetail.Rows[iRow].Cells[0].FindControl("lblDescription")).Text);
                    if (PackageType == "Flat Qty")
                        cmd.Parameters.AddWithValue("@Qty", TotalQty);
                    else
                        cmd.Parameters.AddWithValue("@Qty", ((Label)grdQtyDetail.Rows[iRow].Cells[1].FindControl("lblQty")).Text);
                    cmd.Parameters.AddWithValue("@BranchId", BID);
                    cmd.Parameters.AddWithValue("@Flag", 40);
                    res = PrjClass.ExecuteNonQuery(cmd);
                }
            }
            return res;
        }

        private bool CheckDuplicatePackage(DTO.PackageMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PackageName", Ob.PackageName);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 18);
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

        public bool CheckPackageInAssignTable(DTO.PackageMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PackageId", Ob.PackageId);
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

        public string UpdatePackage(DTO.PackageMaster Ob, GridView grdQtyDetail)
        {
            string res = "", res1 = "";
            int TotalQty = 0;
            double PackageRat = 0;
            ArrayList date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Ob.BranchId);
            if (Ob.CustomerCode != Ob.PackageName)
            {
                if (CheckDuplicatePackage(Ob) == true)
                {
                    res = "Package name already exist.";
                    return res;
                }
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PackageId", Ob.PackageId);
            cmd.Parameters.AddWithValue("@PackageName", Ob.PackageName.Trim());
            cmd.Parameters.AddWithValue("@PackageType", Ob.PackageType);
            cmd.Parameters.AddWithValue("@PackageCost", Ob.PackageCost);
            cmd.Parameters.AddWithValue("@BenefitType", Ob.BenefitType);
            cmd.Parameters.AddWithValue("@BenefitValue", Ob.BenefitValue);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@CreateDate", date[0]);
            cmd.Parameters.AddWithValue("@TaxType", Ob.TaxType);
            cmd.Parameters.AddWithValue("@TotalQty", Ob.TotalQty);

            cmd.Parameters.AddWithValue("@Recurrence", Ob.Recurrence);
            cmd.Parameters.AddWithValue("@StartDate", Ob.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", Ob.EndDate);

            if (grdQtyDetail.Rows.Count > 0)
            {
                if (Ob.PackageType != "Flat Qty")
                {
                    for (int iRow = 0; iRow < grdQtyDetail.Rows.Count; iRow++)
                    {
                        TotalQty += Convert.ToInt32(((Label)grdQtyDetail.Rows[iRow].Cells[1].FindControl("lblQty")).Text);
                    }
                }
                else
                {
                    TotalQty = Ob.TotalQty;
                }
                PackageRat = Ob.PackageCost / (TotalQty * int.Parse(Ob.Recurrence));
            }
            cmd.Parameters.AddWithValue("@PackageRate", Math.Round(PackageRat, 2));
            cmd.Parameters.AddWithValue("@Flag", 2);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (Ob.PackageType != "Qty / Item" && Ob.PackageType != "Flat Qty")
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "sp_PackageMaster";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@PackageId", Ob.PackageId);
                cmd1.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd1.Parameters.AddWithValue("@Flag", 41);
                PrjClass.ExecuteNonQuery(cmd1);
                grdQtyDetail.DataSource = null;
                grdQtyDetail.DataBind();
            }
            if (grdQtyDetail.Rows.Count > 0)
            {
                res1 = SaveInPackageQtyDetail(grdQtyDetail, Convert.ToInt32(Ob.PackageId), Ob.BranchId, Ob.TotalQty, Ob.PackageType);
            }
            return res;
        }

        public DataSet ShowAllPackage(DTO.PackageMaster Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "3");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetPackageQtyDetail(string PackageId, string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PackageId", PackageId);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 42);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchPackage(DTO.PackageMaster Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@PackageName", Ob.PackageName);
            cmd.Parameters.AddWithValue("@Flag", "4");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeletePackage(DTO.PackageMaster Ob)
        {
            string res = "";
            if (CheckPackageInAssignTable(Ob) == true)
            {
                res = "Package in use In Assign Screen.";
                return res;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PackageId", Ob.PackageId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "5");
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet BindPackageDropDown(DTO.PackageMaster Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet GetAllPackgaeDetail(DTO.PackageMaster Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@PackageId", Ob.PackageId);
            cmd.Parameters.AddWithValue("@Flag", 8);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        /// <summary>
        /// Assign Package Function
        /// </summary>
        /// <param name="Ob"></param>
        /// <returns></returns>
        public string SaveAssignPackage(DTO.PackageMaster Ob, bool isQtyItemBased = false)
        {
            string res = "";
            int AssignId = PrjClass.getNewIDAccordingBID("AssignPackage", "AssignId", Ob.BranchId);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssignId", AssignId);
            cmd.Parameters.AddWithValue("@PackageId", Ob.PackageId);
            cmd.Parameters.AddWithValue("@StartValue", Ob.StartValue);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@StartDate", Ob.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", Ob.EndDate);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@PaymentTypes", Ob.PaymentTypes);
            cmd.Parameters.AddWithValue("@PaymentDetail", Ob.PaymentDetails);
            cmd.Parameters.AddWithValue("@MembershipId", Ob.MembershipId);
            cmd.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", Ob.CurDiscount);
            cmd.Parameters.AddWithValue("@Recurrence", Ob.Recurrence);
            cmd.Parameters.AddWithValue("@PackageTotalCost", Ob.PackageTotalCost);
            cmd.Parameters.AddWithValue("@CustEmail", Ob.CustomerEmailID);
            cmd.Parameters.AddWithValue("@Custmobile", Ob.CustomerMobile);
            cmd.Parameters.AddWithValue("@Flag", 9);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (isQtyItemBased && res == "Record Saved")
            {
                var dt = DateTime.Parse(Ob.StartDate);
                var dt2 = DateTime.Parse(Ob.EndDate);
                var dd = dt2 - dt;
                var _res = SaveInRecurrence(AssignId, dt, (int)dd.TotalDays + 1, int.Parse(Ob.Recurrence), Ob.BranchId);
                if (_res != "Record Saved")
                    return _res;
            }
            return res;
        }

        public string UpdateAssignPackage(DTO.PackageMaster Ob, bool isQtyItemBased = false)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssignId", Ob.AssignId);
            cmd.Parameters.AddWithValue("@PackageId", Ob.PackageId);
            cmd.Parameters.AddWithValue("@StartValue", Ob.StartValue);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@StartDate", Ob.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", Ob.EndDate);
            cmd.Parameters.AddWithValue("@Active", Ob.Active);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@PaymentTypes", Ob.PaymentTypes);
            cmd.Parameters.AddWithValue("@PaymentDetail", Ob.PaymentDetails);
            cmd.Parameters.AddWithValue("@MembershipId", Ob.MembershipId);
            cmd.Parameters.AddWithValue("@BarCode", Ob.BarCode);
            cmd.Parameters.AddWithValue("@DefaultDiscountRate", Ob.CurDiscount);
            cmd.Parameters.AddWithValue("@Recurrence", Ob.Recurrence);
            cmd.Parameters.AddWithValue("@PackageTotalCost", Ob.PackageTotalCost);
            cmd.Parameters.AddWithValue("@CustEmail", Ob.CustomerEmailID);
            cmd.Parameters.AddWithValue("@Custmobile", Ob.CustomerMobile);
            cmd.Parameters.AddWithValue("@Flag", 15);
            res = PrjClass.ExecuteNonQuery(cmd);
            if (isQtyItemBased && res == "Record Saved")
            {
                var dt = DateTime.Parse(Ob.StartDate);
                var dt2 = DateTime.Parse(Ob.EndDate);
                var dd = dt2 - dt;
                var _res = SaveInRecurrence(int.Parse(Ob.AssignId), dt, (int)dd.TotalDays + 1, int.Parse(Ob.Recurrence), Ob.BranchId);
                if (_res != "Record Saved")
                    return _res;
            }
            return res;
        }

        public string DeleteAssignPackage(DTO.PackageMaster Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@AssignId", Ob.AssignId);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 16);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public string UpdateMarkComplete(DTO.PackageMaster Ob)
        {
            string res = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssignId", Ob.AssignId);
            cmd.Parameters.AddWithValue("@PackageComplete", Ob.PackageComplete);
            cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 17);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet ShowAllAssignPackage(DTO.PackageMaster Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 10);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public bool CheckOrginalCustomer(DTO.PackageMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 11);
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
        public bool CheckOrginalUser(DTO.PackageMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 72);
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

        public DataSet GetAssignDetails(DTO.PackageMaster Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@AssignId", Ob.AssignId);
            cmd.Parameters.AddWithValue("@Flag", 13);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public bool CheckPackageAssignInBookingTable(DTO.PackageMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AssignId", Ob.AssignId);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", 12);
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

        public DataSet GetCustomerAddress(string CustomerCode,string BID)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustCode", CustomerCode);
            cmd.Parameters.AddWithValue("@BranchId", BID);
            cmd.Parameters.AddWithValue("@Flag", 70);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public bool CheckPackageAssignToCustomer(DTO.PackageMaster Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerCode", Ob.CustomerCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
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

        public string GetQtyndItemsForPackage(string custCode, int assignId, int recurrenceId, string BID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;
            var strItem = new StringBuilder();
            var strQty = new StringBuilder();
            try
            {
                cmd.CommandText = "sp_shiftMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@custCode", custCode);
                cmd.Parameters.AddWithValue("@assignIdp", assignId);
                cmd.Parameters.AddWithValue("@RID", recurrenceId);
                cmd.Parameters.AddWithValue("@Flag", 12);
                rdr = PrjClass.ExecuteReader(cmd);
                while (rdr.Read())
                {
                    strItem.Append(rdr["ItemName"].ToString().ToUpperInvariant() + ":");
                    strQty.Append(rdr["Qty"].ToString() + ":");
                }
                if (strItem.Length > 2)
                    strItem.Remove(strItem.Length - 1, 1);
                if (strQty.Length > 2)
                    strQty.Remove(strQty.Length - 1, 1);
            }
            catch (Exception ex) { }
            finally
            {
                rdr.Close();
                rdr.Dispose();
                cmd.Dispose();
            }
            return strItem.ToString() + "_" + strQty.ToString();
        }

        public string SaveInRecurrence(int assignId, DateTime startDate, int cycleDuration, int noOfRecurrence, string branchId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_PackageMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssignId", assignId);
            cmd.Parameters.AddWithValue("@Recurrence", noOfRecurrence);
            cmd.Parameters.AddWithValue("@PageDate", startDate);
            cmd.Parameters.AddWithValue("@NoofcycleDuration", cycleDuration);
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            cmd.Parameters.AddWithValue("@Flag", 43);
            return PrjClass.ExecuteNonQuery(cmd);
        }

        public object FindAllPackageTypes(string branchId)
        {
            throw new NotImplementedException();
        }

        public string GetQtyPackageDetails(int assignId, string branchId)
        {
            var str = new StringBuilder();
            SqlDataReader rdr = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "sp_PackageMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AssignId", assignId);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@Flag", 44);
                rdr = PrjClass.ExecuteReader(cmd);
                while (rdr != null && rdr.Read())
                {
                    str.Append(rdr["ItemName"] + ":");
                    str.Append(rdr["Qty"] + "_");
                }
                if (str.Length != 0)
                    str.Remove(str.Length - 1, 1);
            }
            catch (Exception ex) { }
            finally
            {
                rdr.Close();
                rdr.Dispose();
                cmd.Dispose();
            }
            return str.ToString();
        }
    }
}