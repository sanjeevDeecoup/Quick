using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class DAL_Item
    {
        private DTO.Common Common = new DTO.Common();

        public string SaveItem(DTO.Item Ob)
        {
            if (CheckIfItemCodeExits(Ob.ItemCode, Ob.BranchId) == false)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_Item";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
                cmd.Parameters.AddWithValue("@NoOfItem", Ob.NoOfItem);
                cmd.Parameters.AddWithValue("@ItemCode", Ob.ItemCode);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Unit", Ob.VariationId);
                cmd.Parameters.AddWithValue("@Flag", "1");
                Common.Result = PrjClass.ExecuteNonQuery(cmd);
            }
            else
            {
                Common.Result = "Item code already exists Please Provide a Unique Item Code";
            }
            return Common.Result;
        }

        public string ItemRename(DTO.Common Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Proc_RenameItem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OldItem", Ob.Input);
            cmd.Parameters.AddWithValue("@NewItem", Ob.ChangeName);          
            cmd.Parameters.AddWithValue("@BranchId", Ob.BID);           
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public string SaveNewItem(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@ItemCode", Ob.ItemCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@ItemImage", Ob.ItemImage);
            cmd.Parameters.AddWithValue("@ItemSubItemRef", Ob.SubItemRefID);
            cmd.Parameters.AddWithValue("@ItemCategoryId", Ob.CategoryID);
            cmd.Parameters.AddWithValue("@ItemVariationId", Ob.VariationId);
            cmd.Parameters.AddWithValue("@Flag", "8");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public string UpdateNewItem(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@ItemCode", Ob.ItemCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@ItemId", Ob.ID);
            cmd.Parameters.AddWithValue("@ItemImage", Ob.ItemImage);
            cmd.Parameters.AddWithValue("@ItemSubItemRef", Ob.SubItemRefID);
            cmd.Parameters.AddWithValue("@ItemCategoryId", Ob.CategoryID);
            cmd.Parameters.AddWithValue("@ItemVariationId", Ob.VariationId);
            cmd.Parameters.AddWithValue("@Flag", "12");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public string UpdateItem(DTO.Item Ob)
        {
            if (Ob.ItemImage != Ob.ItemCode)
            {
                if (CheckIfItemCodeExits(Ob.ItemCode, Ob.BranchId) == true)
                {
                    Common.Result = "Item code already exists Please Provide a Unique Item Code";
                    return Common.Result; ;
                }
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@NoOfItem", Ob.NoOfItem);
            cmd.Parameters.AddWithValue("@ItemCode", Ob.ItemCode);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@ItemID", Ob.ID);
            cmd.Parameters.AddWithValue("@Unit", Ob.VariationId);
            cmd.Parameters.AddWithValue("@Flag", "4");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public bool CheckItemStatus(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_Item";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
                cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
                cmd.Parameters.AddWithValue("@Flag", "14");
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

        public bool CheckCorrectItem(string ItemName, string BID, bool isSubItem = false)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_Item";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", ItemName);
                cmd.Parameters.AddWithValue("@BranchId", BID);
                cmd.Parameters.AddWithValue("@Flag", 18);
                if (isSubItem)
                    cmd.Parameters["@Flag"].Value = 20;
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

        public string UpdateExitingItem(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@OldItemName", Ob.OldItemName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "4");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public string DeleteSubItem(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OldItemName", Ob.OldItemName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "5");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public string SaveSubItem(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@SubItemName", Ob.SubItemName);
            cmd.Parameters.AddWithValue("@SubItemOrder", Ob.SubItemOrder);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "2");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public DataSet ShowItem(DTO.Item Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "6");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet SearchItem(DTO.Item Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", Ob.ItemName);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 3);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet ShowAllItem(DTO.Item Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "6");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet BindGrid(DTO.Item Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", "9");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public DataSet FillTextBox(DTO.Item Ob)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@ItemId", Ob.ID);
            cmd.Parameters.AddWithValue("@Flag", "10");
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string DeleteNewItem(DTO.Item Ob)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Item";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@ItemId", Ob.ID);
            cmd.Parameters.AddWithValue("@ItemSubItemRef", Ob.SubItemRefID);
            cmd.Parameters.AddWithValue("@ItemCategoryId", Ob.CategoryID);
            cmd.Parameters.AddWithValue("@ItemVariationId", Ob.VariationId);
            cmd.Parameters.AddWithValue("@Flag", "11");
            Common.Result = PrjClass.ExecuteNonQuery(cmd);
            return Common.Result;
        }

        public bool CheckIfItemCodeExits(string code, string BranchID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_Item";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BranchID);
                cmd.Parameters.AddWithValue("@ItemCode", code);
                cmd.Parameters.AddWithValue("@Flag", "13");
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

        public bool CheckIfProcessCodeExits(string code, string BranchID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            bool status = false;
            try
            {
                cmd.CommandText = "sp_Item";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", BranchID);
                cmd.Parameters.AddWithValue("@PrcCode", code);
                cmd.Parameters.AddWithValue("@Flag", 17);
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

        public void SaveDefPrcAndRate(ref DTO.Item Item, string branchId, bool isUpdating, int rateListId)
        {
            SqlCommand sqlCommand = new SqlCommand
                                            {
                                                CommandText = "sp_Item",
                                                CommandType = CommandType.StoredProcedure
                                            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@ItemName", Item.ItemName.ToUpperInvariant());
            sqlCommand.Parameters.AddWithValue("@PrcCode", Item.DefaultPrc);
            sqlCommand.Parameters.AddWithValue("@ItemRate", Item.DefaultRate);
            sqlCommand.Parameters.AddWithValue("@RateListId", rateListId);
            sqlCommand.Parameters.AddWithValue("@Flag", !isUpdating ? 15 : 16);
            if (PrjClass.ExecuteNonQuery(sqlCommand) != "Record Saved")
                throw new DataException("Couldn't' save the default records!");
        }

        public string[] GetAllItemsDetailed(string branchId)
        {
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText = "sp_Item",
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@BranchId", branchId);
            sqlCommand.Parameters.AddWithValue("@Flag", 21);
            var sqlDataReader = PrjClass.ExecuteReader(sqlCommand);
            var allItems = new List<string>();
            while (sqlDataReader != null && sqlDataReader.Read())
            {
                allItems.Add(sqlDataReader.GetString(0));
            }
            if (sqlDataReader != null) 
                sqlDataReader.Close();
            return allItems.ToArray();
        }
    }
}