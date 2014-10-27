using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;

using Excel = Microsoft.Office.Interop.Excel;

namespace DAL
{
    public class DAL_For_ImportFile
    {
        private System.Data.DataTable MakeItemHeader()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("NumberOfSubItems");
            dt.Columns.Add("ItemCode");
            return dt;
        }

        private System.Data.DataTable MakeSubItemHeader()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ItemName");
            dt.Columns.Add("SubItemName");
            dt.Columns.Add("SubItemOrder");
            return dt;
        }

        private System.Data.DataTable MakeMenuRightsHeader()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("UserTypeId");
            dt.Columns.Add("PageTitle");
            dt.Columns.Add("FileName");
            dt.Columns.Add("RightToView");
            dt.Columns.Add("MenuItemLevel");
            dt.Columns.Add("MenuPosition");
            dt.Columns.Add("ParentMenu");
            return dt;
        }

        public string ReadItemTableFile(string FileName)
        {
            DataRow dr = null;
            string res = "";
            System.Data.DataTable dtTop = MakeItemHeader();
            dr = dtTop.NewRow();
            // Path for the test excel application
            //string FileName = "~/ImportExcelFile/Files/ItemMaster.xlsx";
            //string temp = @"D:\ItemMaster.XLSX";
            // Initialize the Excel Application class
            Excel.Application app = new Application();
            //ApplicationClass app = new ApplicationClass();
            // Create the workbook object by opening the excel file.
            Excel.Workbook workBook = app.Workbooks.Open(FileName,
                                                         0,
                                                         true,
                                                         5,
                                                         "",
                                                         "",
                                                         true,
                                                         Excel.XlPlatform.xlWindows,
                                                         "\t",
                                                         false,
                                                         false,
                                                         0,
                                                         true,
                                                         1,
                                                         0);
            // Get the active worksheet using sheet name or active sheet
            Excel.Worksheet workSheet = (Excel.Worksheet)workBook.ActiveSheet;

            // This row,column index should be changed as per your need.
            // i.e. which cell in the excel you are interesting to read.
            int index = 1;
            object rowIndex = 1;
            object colIndex1 = 1;
            object colIndex2 = 2;
            object colIndex3 = 3;
            object colIndex4 = 4;
            string ItemCode = "";
            try
            {
                while (((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2 != null)
                {
                    dr = dtTop.NewRow();
                    // Read the Cells to get the required value.
                    string ItemID = ((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2.ToString();
                    string ItemName = ((Excel.Range)workSheet.Cells[rowIndex, colIndex2]).Value2.ToString();
                    string NumberOfSubItems = ((Excel.Range)workSheet.Cells[rowIndex, colIndex3]).Value2.ToString();
                    if (((Excel.Range)workSheet.Cells[rowIndex, colIndex4]).Value2 != null)
                    {
                        ItemCode = ((Excel.Range)workSheet.Cells[rowIndex, colIndex4]).Value2.ToString();
                    }
                    else
                    {
                        ItemCode = "";
                    }
                    index++;
                    rowIndex = index;
                    dr[0] = ItemID;
                    dr[1] = ItemName;
                    dr[2] = NumberOfSubItems;
                    dr[3] = ItemCode;
                    dtTop.Rows.Add(dr);
                }
                res = SaveInTheItemTable(dtTop);
            }
            catch (Exception)
            {
                // Log the exception and quit...
                app.Quit();
            }
            return res;
        }

        public string ReadSubItemDetailsExcelFile(string FileName)
        {
            DataRow dr = null;
            string res = "";
            System.Data.DataTable dtTop = MakeSubItemHeader();
            dr = dtTop.NewRow();
            // Path for the test excel application
            //string FileName = @"D:\SubItemDetails.XLSX";
            // Initialize the Excel Application class
            Excel.Application app = new Application();
            //Excel.ApplicationClass app = new ApplicationClass();
            // Create the workbook object by opening the excel file.
            Excel.Workbook workBook = app.Workbooks.Open(FileName,
                                                         0,
                                                         true,
                                                         5,
                                                         "",
                                                         "",
                                                         true,
                                                         Excel.XlPlatform.xlWindows,
                                                         "\t",
                                                         false,
                                                         false,
                                                         0,
                                                         true,
                                                         1,
                                                         0);
            // Get the active worksheet using sheet name or active sheet
            Excel.Worksheet workSheet = (Excel.Worksheet)workBook.ActiveSheet;

            // This row,column index should be changed as per your need.
            // i.e. which cell in the excel you are interesting to read.
            int index = 1;
            object rowIndex = 1;
            object colIndex1 = 1;
            object colIndex2 = 2;
            object colIndex3 = 3;

            try
            {
                while (((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2 != null)
                {
                    dr = dtTop.NewRow();
                    // Read the Cells to get the required value.
                    string ItemName = ((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2.ToString();
                    string SubItemName = ((Excel.Range)workSheet.Cells[rowIndex, colIndex2]).Value2.ToString();
                    string SubItemOrder = ((Excel.Range)workSheet.Cells[rowIndex, colIndex3]).Value2.ToString();

                    index++;
                    rowIndex = index;
                    dr[0] = ItemName;
                    dr[1] = SubItemName;
                    dr[2] = SubItemOrder;
                    dtTop.Rows.Add(dr);
                }
                res = SaveInTheSubItemTable(dtTop);
            }
            catch (Exception)
            {
                // Log the exception and quit...
                app.Quit();
            }
            return res;
        }

        public string ReadMenuRightsExcelFile(string FileName)
        {
            DataRow dr = null;
            string res = "";
            System.Data.DataTable dtTop = MakeMenuRightsHeader();
            dr = dtTop.NewRow();
            // Path for the test excel application
            //string FileName = @"D:\EntMenuRights.XLSX";
            // Initialize the Excel Application class
            Excel.Application app = new Application();
            //Excel.ApplicationClass app = new ApplicationClass();
            // Create the workbook object by opening the excel file.
            Excel.Workbook workBook = app.Workbooks.Open(FileName,
                                                         0,
                                                         true,
                                                         5,
                                                         "",
                                                         "",
                                                         true,
                                                         Excel.XlPlatform.xlWindows,
                                                         "\t",
                                                         false,
                                                         false,
                                                         0,
                                                         true,
                                                         1,
                                                         0);
            // Get the active worksheet using sheet name or active sheet
            Excel.Worksheet workSheet = (Excel.Worksheet)workBook.ActiveSheet;

            // This row,column index should be changed as per your need.
            // i.e. which cell in the excel you are interesting to read.
            int index = 1;
            object rowIndex = 1;
            object colIndex1 = 1;
            object colIndex2 = 2;
            object colIndex3 = 3;
            object colIndex4 = 4;
            object colIndex5 = 5;
            object colIndex6 = 6;
            object colIndex7 = 7;

            try
            {
                while (((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2 != null)
                {
                    dr = dtTop.NewRow();
                    // Read the Cells to get the required value.
                    string UserTypeId = ((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2.ToString();
                    string PageTitle = ((Excel.Range)workSheet.Cells[rowIndex, colIndex2]).Value2.ToString();
                    string FileName1 = ((Excel.Range)workSheet.Cells[rowIndex, colIndex3]).Value2.ToString();
                    string RightToView = ((Excel.Range)workSheet.Cells[rowIndex, colIndex4]).Value2.ToString();
                    string MenuItemLevel = ((Excel.Range)workSheet.Cells[rowIndex, colIndex5]).Value2.ToString();
                    string MenuPosition = ((Excel.Range)workSheet.Cells[rowIndex, colIndex6]).Value2.ToString();
                    string ParentMenu = ((Excel.Range)workSheet.Cells[rowIndex, colIndex7]).Value2.ToString();

                    index++;
                    rowIndex = index;
                    dr[0] = UserTypeId;
                    dr[1] = PageTitle;
                    dr[2] = FileName1;
                    dr[3] = RightToView;
                    dr[4] = MenuItemLevel;
                    dr[5] = MenuPosition;
                    dr[6] = ParentMenu;
                    dtTop.Rows.Add(dr);
                }
                res = SaveInTheEntMenuRightsTable(dtTop);
            }
            catch (Exception)
            {
                // Log the exception and quit...
                app.Quit();
            }
            return res;
        }

        private string SaveInTheItemTable(System.Data.DataTable dto)
        {
            string res = "";
            for (int iRow = 0; iRow < dto.Rows.Count; iRow++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SaveItemName", dto.Rows[iRow]["ItemName"].ToString());
                cmd.Parameters.AddWithValue("@NumberOfSubItems", dto.Rows[iRow]["NumberOfSubItems"].ToString());
                cmd.Parameters.AddWithValue("@ItemCode", dto.Rows[iRow]["ItemCode"].ToString());
                cmd.Parameters.AddWithValue("@Flag", 20);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        private string SaveInTheSubItemTable(System.Data.DataTable dto)
        {
            string res = "";
            for (int iRow = 0; iRow < dto.Rows.Count; iRow++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SaveItemName", dto.Rows[iRow]["ItemName"].ToString());
                cmd.Parameters.AddWithValue("@SubItem2", dto.Rows[iRow]["SubItemName"].ToString());
                cmd.Parameters.AddWithValue("@SubItemOrder", dto.Rows[iRow]["SubItemOrder"].ToString());
                cmd.Parameters.AddWithValue("@Flag", 21);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }

        private string SaveInTheEntMenuRightsTable(System.Data.DataTable dto)
        {
            string res = "";
            for (int iRow = 0; iRow < dto.Rows.Count; iRow++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_NewBooking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserTypeId", dto.Rows[iRow]["UserTypeId"].ToString());
                cmd.Parameters.AddWithValue("@PageTitle", dto.Rows[iRow]["PageTitle"].ToString());
                cmd.Parameters.AddWithValue("@FileName", dto.Rows[iRow]["FileName"].ToString());
                cmd.Parameters.AddWithValue("@RightToView", dto.Rows[iRow]["RightToView"].ToString());
                cmd.Parameters.AddWithValue("@MenuItemLevel", dto.Rows[iRow]["MenuItemLevel"].ToString());
                cmd.Parameters.AddWithValue("@MenuPosition", dto.Rows[iRow]["MenuPosition"].ToString());
                cmd.Parameters.AddWithValue("@ParentMenu", dto.Rows[iRow]["ParentMenu"].ToString());
                cmd.Parameters.AddWithValue("@Flag", 22);
                res = PrjClass.ExecuteNonQuery(cmd);
            }
            return res;
        }
    }
}