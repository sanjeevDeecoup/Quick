using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Management;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;


public class PrjClass : System.Web.UI.Page
{
    public static string sqlConStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

    //public static string sqlConStr = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
    public PrjClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string ExecuteScalar(SqlCommand cmd)
    {
        string saveSuccess = string.Empty;
        string execute = string.Empty;
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
            execute = cmd.ExecuteScalar().ToString();
            if (execute != "")
                saveSuccess = execute;
        }
        catch (Exception excp)
        {
            if (excp.Message.Contains("Violation of PRIMARY KEY constraint"))
                saveSuccess = PrjClass.UNIQUE;
            else if (excp.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                saveSuccess = PrjClass.ReferenceMessage;
            else if (excp.Message.Contains("Violation of UNIQUE KEY constraint"))
                saveSuccess = PrjClass.UNIQUE;
            else if (excp.Message.Contains("Object reference not set to an instance of an object"))
                saveSuccess = "Object are not set anywhere";
            else if (excp.Message.Contains("Error converting data type nvarchar to float."))
                saveSuccess = "Invalid opening balance.";
            else
                saveSuccess = excp.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return saveSuccess;
    }

    public static void SetButton(ImageButton save, ImageButton update, bool s1, bool s2)
    {
        save.Visible = s1;
        update.Visible = s2;
    }

    public static string getUniqueID(string drive)
    {
        if (drive == "" || drive == null)
        {
            drive = "C";
        }
        ManagementObject disk = new ManagementObject("Win32_LogicalDisk.DeviceID=\"" + drive + ":\"");
        disk.Get();
        return disk["VolumeSerialNumber"].ToString();
    }

    public static string ServiceTaxType = "SERVICE TAX,SERVICETAX,SER.TAX,SER. TAX,S. TAX,S TAX,ST,S.T.,S. T.";
    public static string BackFailMsg = "System data back up failed, Kindly contact your system administrator.";
    public static string STT = "SEC. TRANS. TAX,STT,STT DEL CHARGES";
    public static string SDTax = "STAMP & OTHER CHARGES,STAMP CHARGES";
    public static string CR = "NET CR,NET CR.,CR,CR.";
    public static string DR = "NET DR,NET DR.,DR,DR.";
    public static Color SetColor = System.Drawing.Color.SkyBlue;
    public static SqlDataReader sdr = null;
    public static SqlCommand commandForQuery = null;
    public static DataSet ds = new DataSet();

    public static void SetNullInCommand()
    {
        PrjClass.commandForQuery = null;
    }

    public static void SetButtonFunction(ImageButton Save, ImageButton Update, System.Web.UI.WebControls.TextBox NameOfTextboxForFocus, bool Status, bool Status1)
    {
        Save.Visible = Status;
        Update.Visible = Status1;
        NameOfTextboxForFocus.Focus();
    }

    public static string InvalidDate = "Invalid Date Please enter correct format";
    public static string NoProjectDefine = "No project are available.";
    public static string UNIQUE = "Record Already Exist";
    public static string SaveSuccess = "Saved Successfully.";
    public static string UpdateSuccess = "Updated Successfully.";
    public static string NotMoreThenCurrentDate = "Up to date can not be greater than Current date.";
    public static string NotMoreThenFromDate = "From date cannot be greater than Up to date.";
    public static string NoRecord = "No record found.";
    public static string DeleteSucess = "Deleted Successfully.";
    public static string ReferenceMessage = "This record can not be deleted as it has active reference to other module.";
    public static string NotSave = "Some problem occur during save or update into the  database.";

    public static bool chkrecord(SqlCommand cmd)
    {
        SqlConnection con = new SqlConnection(sqlConStr);
        SqlDataReader dr;
        bool blnFound = false;
        try
        {
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
            if (dr.Read())
            {
                blnFound = true;
            }
            dr.Close();
            dr.Dispose();
        }
        catch (Exception)
        {
            blnFound = true;
        }
        finally { con.Close();
        con.Dispose();
        }
        return blnFound;
    }

    public static string ReturnMonth(int Month)
    {
        string MonthName = "";
        if (Month == 1)
            MonthName = "JAN";
        else if (Month == 2)
            MonthName = "FEB";
        else if (Month == 3)
            MonthName = "MAR";
        else if (Month == 4)
            MonthName = "APR";
        else if (Month == 5)
            MonthName = "MAY";
        else if (Month == 6)
            MonthName = "JUN";
        else if (Month == 7)
            MonthName = "July";
        else if (Month == 8)
            MonthName = "AUG";
        else if (Month == 9)
            MonthName = "SEP";
        else if (Month == 10)
            MonthName = "OCT";
        else if (Month == 11)
            MonthName = "NOV";
        else if (Month == 12)
            MonthName = "DEC";
        return MonthName;
    }

    public static string GetMessage(string msg)
    {
        return "<script Language='javascript' type='text/javascript'>alert('" + msg + "');</" + "script>";
    }

    public static string GetAndGo(string msg, string target)
    {
        string str = "<script Language='javascript' type='text/javascript'>alert('" + msg + "');";
        if (!string.IsNullOrEmpty(target))
        {
            str += "document.location='" + target + "';";
        }
        str += "</" + "script>";
        return str;
    }

    public static string OpenNewWindow(string orgPage, string target)
    {
        var str = "<script Language='javascript' type='text/javascript'>";
        str += "document.location='" + orgPage + "';";
        return str;
    }

    public static string CheckFromDate(string Date)
    {
        string ReturnMSG = "";
        int Day = 0, Month = 0, Year = 0;
        string[] Fromdate = Date.Split('-');
        Day = Convert.ToInt32(Fromdate[0].ToString());
        Month = Convert.ToInt32(Fromdate[1].ToString());
        Year = Convert.ToInt32(Fromdate[2].ToString());
        if (Month >= 13 || Month <= 0)
        {
            ReturnMSG = "Please enter correct month DD-MM-YYYY From Date";
        }
        else
        {
            DateTime dt2 = new DateTime(Year, Month, 1);
            dt2 = dt2.AddMonths(1).AddDays(-1);
            if (Day >= 32 || Day <= 0)
            {
                ReturnMSG = "Please enter correct day DD-MM-YYYY From Date";
            }
            else
            {
                if (dt2.Day < Day)
                    ReturnMSG = "Please enter correct day DD-MM-YYYY From Date";
                else if (Month >= 13 || Month <= 0)
                {
                    ReturnMSG = "Please enter correct month DD-MM-YYYY From Date";
                }
                else if (Year <= 999)
                {
                    ReturnMSG = "Please enter correct year  DD-MM-YYYY From Date";
                }
                else
                    ReturnMSG = "False";
            }
        }
        return ReturnMSG;
    }

    public static string CheckUptoDate(string Date)
    {
        string ReturnMSG = "";
        int Day = 0, Month = 0, Year = 0;
        string[] Fromdate = Date.Split('-');
        Day = Convert.ToInt32(Fromdate[0].ToString());
        Month = Convert.ToInt32(Fromdate[1].ToString());
        Year = Convert.ToInt32(Fromdate[2].ToString());
        if (Month >= 13 || Month <= 0)
        {
            ReturnMSG = "Please enter correct month DD-MM-YYYY upto Date";
        }
        else
        {
            DateTime dt2 = new DateTime(Year, Month, 1);
            dt2 = dt2.AddMonths(1).AddDays(-1);
            if (Day >= 32 || Day <= 0)
            {
                ReturnMSG = "Please enter correct day DD-MM-YYYY Upto Date";
            }
            else
            {
                if (dt2.Day < Day)
                    ReturnMSG = "Please enter correct day DD-MM-YYYY Upto Date";
                else if (Month >= 13 || Month <= 0)
                {
                    ReturnMSG = "Please enter correct month DD-MM-YYYY Upto Date";
                }
                else if (Year <= 999)
                {
                    ReturnMSG = "Please enter correct year  DD-MM-YYYY Upto Date";
                }
                else
                    ReturnMSG = "False";
            }
        }
        return ReturnMSG;
    }

    public static void SetItemInDropDown(DropDownList Drp, string strMatchWith, bool matchText, bool CaseSensitiveMatch)
    {
        string strMatch = string.Empty;
        if (!CaseSensitiveMatch) strMatchWith = strMatchWith.ToUpper();
        for (int i = 0; i < Drp.Items.Count; i++)
        {
            if (matchText)
            {
                strMatch = Drp.Items[i].Text;
            }
            else
            {
                strMatch = Drp.Items[i].Value;
            }
            if (!CaseSensitiveMatch) strMatch = strMatch.ToUpper();

            if (string.Equals(strMatch, strMatchWith))
            {
                Drp.SelectedIndex = i;
                break;
            }
        }
    }

    public static string[] GenerateExcelReportFromGridView(GridView Grd, string strReportTitle)
    {
        //before using following function, use using Microsoft.Office.Interop.Excel;
        System.Web.UI.Page pg = new System.Web.UI.Page();
        string strCurrentDir = pg.Server.MapPath(".") + "\\";
        RemoveFiles(strCurrentDir); // utility method to clean up old files

        Application oXL = null;
        _Workbook oWB = null;
        _Worksheet oWS = null;
        string[] Message = { "0", "" };
        Int32 intTotalFields = 0, intTitleInRow = 1, intDateTimeInRow = 2, intTableHeaderInRow = 4, intDataRowStartFrom = 5, intDataRowUpto = 5;
        string T1 = "A" + intTitleInRow.ToString(), T2 = "Z" + intTitleInRow.ToString();
        string DT1 = "A" + intDateTimeInRow.ToString(), DT2 = "Z" + intDateTimeInRow.ToString();
        string TH1 = "A" + intTableHeaderInRow.ToString(), TH2 = "Z" + intTableHeaderInRow.ToString();
        string strCellValue = "";
        try
        {
            oXL = new Application();
            oXL.Visible = false;
            oWB = (_Workbook)oXL.Workbooks.Add(Missing.Value);
            oWS = (_Worksheet)oWB.ActiveSheet;
            //Set Report Title.
            oWS.Cells[intTitleInRow, 1] = strReportTitle;
            oWS.Cells[intDateTimeInRow, 1] = "Report Date/Time : " + DateTime.Now.ToString();

            //Start Getting Data From GridView.
            intTotalFields = Grd.HeaderRow.Cells.Count;

            for (int i = 0; i < intTotalFields; i++)
            {
                oWS.Cells[intTableHeaderInRow, i + 1] = Grd.Columns[i].HeaderText;
            }
            for (int r = 0; r < Grd.Rows.Count; r++)
            {
                for (int c = 0; c < intTotalFields; c++)
                {
                    strCellValue = "" + Grd.Rows[r].Cells[c].Text;
                    if (strCellValue == "")
                    {
                        strCellValue = ((System.Web.UI.WebControls.Label)Grd.Rows[r].Cells[c].Controls[1]).Text;
                    }
                    oWS.Cells[intDataRowUpto, c + 1] = strCellValue == "&nbsp;" ? "" : strCellValue;
                }
                intDataRowUpto++;
            }
            try
            {
                for (int fc = 0; fc < intTotalFields; fc++)
                {
                    oWS.Cells[intDataRowUpto, fc + 1] = Grd.FooterRow.Cells[fc].Text == "&nbsp;" ? "" : Grd.FooterRow.Cells[fc].Text;
                }
                intDataRowUpto++;
            }
            catch (Exception)
            {
            }
            if (intDataRowUpto == intDataRowStartFrom)
            { oWS.Cells[intDataRowStartFrom, 1] = "No Record Found."; }

            //Format the worksheet now (Row Number=1).
            //Title
            oWS.get_Range(T1, T2).Merge(null);
            oWS.get_Range(T1, T2).Font.Size = 20;
            oWS.get_Range(T1, T2).Font.Bold = true;
            oWS.get_Range(T1, T2).Font.Italic = true;
            oWS.get_Range(T1, T2).Font.Color = XlRgbColor.rgbBlue;

            //Format Data-Time Row.
            oWS.get_Range(DT1, DT2).Merge(null);

            //Table Header (Row number=3).
            oWS.get_Range(TH1, TH2).Font.Bold = true;
            oWS.get_Range(TH1, TH2).Font.Underline = true;
            oWS.get_Range(TH1, TH2).Font.Color = XlRgbColor.rgbRed;
            //oWS.get_Range("A3", "Z3").Borders.Color = System.Drawing.Color.Maroon.ToArgb;
            oWS.get_Range(TH1, TH2).EntireColumn.AutoFit();
            oWS.get_Range(TH1, TH2).Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Save the File
            string ReportFile = strReportTitle + " " + DateTime.Now.Ticks.ToString() + ".xls";
            oWB.SaveAs(strCurrentDir + ReportFile, XlFileFormat.xlWorkbookNormal, null, null, false, false, XlSaveAsAccessMode.xlShared, false, false, null, null, null);
            // Need all following code to clean up and extingush all references!!!
            // force final cleanup!
            Message[0] = "1";
            Message[1] = ReportFile;
        }
        catch (Exception excp)
        {
            String errorMessage;
            errorMessage = "Error: " + excp.Message;
            errorMessage = String.Concat(errorMessage, excp.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, excp.Source);
            Message[0] = "0";
            Message[1] = errorMessage;
        }
        finally
        {
            try
            {
                oWB.Close(null, null, null);
                oXL.Workbooks.Close();
                oXL.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWS);
                oXL = null;
                oWB = null;
                oWS = null;
                GC.Collect();
            }
            catch (Exception excp)
            {
                String errorMessage;
                errorMessage = "Error: " + excp.Message;
                errorMessage = String.Concat(errorMessage, excp.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, excp.Source);
                Message[0] = "0";
                Message[1] = errorMessage;
            }
        }
        return Message;
    }

    public static string[] GenerateExcelReportFromDataTable(System.Data.DataTable DT, string strReportTitle)
    {
        //before using following function, use using Microsoft.Office.Interop.Excel;
        System.Web.UI.Page pg = new System.Web.UI.Page();
        string strCurrentDir = pg.Server.MapPath(".") + "\\";
        RemoveFiles(strCurrentDir); // utility method to clean up old files

        Application oXL = null;
        _Workbook oWB = null;
        _Worksheet oWS = null;
        string[] Message = { "0", "" };
        Int32 intTotalFields = 0, intTitleInRow = 1, intDateTimeInRow = 2, intTableHeaderInRow = 4, intDataRowStartFrom = 5, intDataRowUpto = 5;
        string T1 = "A" + intTitleInRow.ToString(), T2 = "Z" + intTitleInRow.ToString();
        string DT1 = "A" + intDateTimeInRow.ToString(), DT2 = "Z" + intDateTimeInRow.ToString();
        string TH1 = "A" + intTableHeaderInRow.ToString(), TH2 = "Z" + intTableHeaderInRow.ToString();

        try
        {
            oXL = new Application();
            oXL.Visible = false;
            oWB = (_Workbook)oXL.Workbooks.Add(Missing.Value);
            oWS = (_Worksheet)oWB.ActiveSheet;
            //Set Report Title.
            oWS.Cells[intTitleInRow, 1] = strReportTitle;
            oWS.Cells[intDateTimeInRow, 1] = "Report Date/Time : " + DateTime.Now.ToString();

            //Start Getting Data From DataTable.
            intTotalFields = DT.Columns.Count;

            for (int i = 0; i < intTotalFields; i++)
            {
                oWS.Cells[intTableHeaderInRow, i + 1] = DT.Columns[i].ToString();
            }

            foreach (DataRow dr in DT.Rows)
            {
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    oWS.Cells[intDataRowUpto, i + 1] = dr[i].ToString();
                }
                intDataRowUpto++;
            }
            if (intDataRowUpto == intDataRowStartFrom)
            { oWS.Cells[intDataRowStartFrom, 1] = "No Record Found."; }

            //Format the worksheet now (Row Number=1).
            //Title
            oWS.get_Range(T1, T2).Merge(null);
            oWS.get_Range(T1, T2).Font.Size = 20;
            oWS.get_Range(T1, T2).Font.Bold = true;
            oWS.get_Range(T1, T2).Font.Italic = true;
            oWS.get_Range(T1, T2).Font.Color = XlRgbColor.rgbBlue;

            //Format Data-Time Row.
            oWS.get_Range(DT1, DT2).Merge(null);

            //Table Header (Row number=3).
            oWS.get_Range(TH1, TH2).Font.Bold = true;
            oWS.get_Range(TH1, TH2).Font.Underline = true;
            oWS.get_Range(TH1, TH2).Font.Color = XlRgbColor.rgbRed;
            //oWS.get_Range("A3", "Z3").Borders.Color = System.Drawing.Color.Maroon.ToArgb;
            oWS.get_Range(TH1, TH2).EntireColumn.AutoFit();
            oWS.get_Range(TH1, TH2).Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Save the File
            string ReportFile = strReportTitle + " " + DateTime.Now.Ticks.ToString() + ".xls";
            oWB.SaveAs(strCurrentDir + ReportFile, XlFileFormat.xlWorkbookNormal, null, null, false, false, XlSaveAsAccessMode.xlShared, false, false, null, null, null);
            // Need all following code to clean up and extingush all references!!!
            // force final cleanup!
            Message[0] = "1";
            Message[1] = ReportFile;
        }
        catch (Exception excp)
        {
            String errorMessage;
            errorMessage = "Error: " + excp.Message;
            errorMessage = String.Concat(errorMessage, excp.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, excp.Source);
            Message[0] = "0";
            Message[1] = errorMessage;
        }
        finally
        {
            try
            {
                oWB.Close(null, null, null);
                oXL.Workbooks.Close();
                oXL.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWS);
                oXL = null;
                oWB = null;
                oWS = null;
                GC.Collect();
            }
            catch (Exception excp)
            {
                String errorMessage;
                errorMessage = "Error: " + excp.Message;
                errorMessage = String.Concat(errorMessage, excp.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, excp.Source);
                Message[0] = "0";
                Message[1] = errorMessage;
            }
        }
        return Message;
    }

    public static string[] GenerateExcelReportFromQuery(String strSqlQuery, string strReportTitle)
    {
        //before using following function, use using Microsoft.Office.Interop.Excel;
        System.Web.UI.Page pg = new System.Web.UI.Page();
        string strCurrentDir = pg.Server.MapPath(".") + "\\";
        RemoveFiles(strCurrentDir); // utility method to clean up old files

        Application oXL = null;
        _Workbook oWB = null;
        _Worksheet oWS = null;
        string[] Message = { "0", "" };
        Int32 intTotalFields = 0, intTitleInRow = 1, intDateTimeInRow = 2, intTableHeaderInRow = 4, intDataRowStartFrom = 5, intDataRowUpto = 5;
        string T1 = "A" + intTitleInRow.ToString(), T2 = "Z" + intTitleInRow.ToString();
        string DT1 = "A" + intDateTimeInRow.ToString(), DT2 = "Z" + intDateTimeInRow.ToString();
        string TH1 = "A" + intTableHeaderInRow.ToString(), TH2 = "Z" + intTableHeaderInRow.ToString();

        try
        {
            oXL = new Application();
            oXL.Visible = false;
            oWB = (_Workbook)oXL.Workbooks.Add(Missing.Value);
            oWS = (_Worksheet)oWB.ActiveSheet;
            //Set Report Title.
            oWS.Cells[intTitleInRow, 1] = strReportTitle;
            oWS.Cells[intDateTimeInRow, 1] = "Report Date/Time : " + DateTime.Now.ToString();

            //Start Getting Data From Database.
            SqlConnection sqlcon = new SqlConnection(sqlConStr);
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSqlQuery, sqlcon);
            SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            intTotalFields = sdr.FieldCount;

            for (int i = 0; i < intTotalFields; i++)
            {
                oWS.Cells[intTableHeaderInRow, i + 1] = sdr.GetName(i).ToString();
            }

            while (sdr.Read())
            {
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    oWS.Cells[intDataRowUpto, i + 1] = sdr.GetValue(i).ToString();
                }
                intDataRowUpto++;
            }
            sdr.Close();
            if (intDataRowUpto == intDataRowStartFrom)
            { oWS.Cells[intDataRowStartFrom, 1] = "No Record Found."; }

            //Format the worksheet now (Row Number=1).
            //Title
            oWS.get_Range(T1, T2).Merge(null);
            oWS.get_Range(T1, T2).Font.Size = 20;
            oWS.get_Range(T1, T2).Font.Bold = true;
            oWS.get_Range(T1, T2).Font.Italic = true;
            oWS.get_Range(T1, T2).Font.Color = XlRgbColor.rgbBlue;

            //Format Data-Time Row.
            oWS.get_Range(DT1, DT2).Merge(null);

            //Table Header (Row number=3).
            oWS.get_Range(TH1, TH2).Font.Bold = true;
            oWS.get_Range(TH1, TH2).Font.Underline = true;
            oWS.get_Range(TH1, TH2).Font.Color = XlRgbColor.rgbRed;
            //oWS.get_Range("A3", "Z3").Borders.Color = System.Drawing.Color.Maroon.ToArgb;
            oWS.get_Range(TH1, TH2).EntireColumn.AutoFit();
            oWS.get_Range(TH1, TH2).Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Save the File
            string ReportFile = strReportTitle + " " + DateTime.Now.Ticks.ToString() + ".xls";
            oWB.SaveAs(strCurrentDir + ReportFile, XlFileFormat.xlWorkbookNormal, null, null, false, false, XlSaveAsAccessMode.xlShared, false, false, null, null, null);
            // Need all following code to clean up and extingush all references!!!
            // force final cleanup!
            Message[0] = "1";
            Message[1] = ReportFile;
        }
        catch (Exception excp)
        {
            String errorMessage;
            errorMessage = "Error: " + excp.Message;
            errorMessage = String.Concat(errorMessage, excp.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, excp.Source);
            Message[0] = "0";
            Message[1] = errorMessage;
        }
        finally
        {
            try
            {
                oWB.Close(null, null, null);
                oXL.Workbooks.Close();
                oXL.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWS);
                oXL = null;
                oWB = null;
                oWS = null;
                GC.Collect();
            }
            catch (Exception excp)
            {
                String errorMessage;
                errorMessage = "Error: " + excp.Message;
                errorMessage = String.Concat(errorMessage, excp.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, excp.Source);
                Message[0] = "0";
                Message[1] = errorMessage;
            }
        }
        return Message;
    }

    public static string[] GenerateCSVReportFromGridView(GridView Grd, string ReportTitle)
    {
        System.Web.UI.Page pg = new System.Web.UI.Page();
        string strCurrentDir = pg.Server.MapPath(".") + "\\";
        RemoveFiles(strCurrentDir);
        string[] Message = { "0", "" };
        string strSepValue = ",", strFileExtension = ".csv";
        try
        {
            StringBuilder sb = new StringBuilder(); // to hold csv/Excel text file
            // Create Header and sheet...
            string quoter = @"""""";
            sb.Append(ReportTitle + "\n");
            sb.Append("Report Date-Time : " + DateTime.Now.ToString() + "\n\n");
            for (int j = 0; j < Grd.Columns.Count; j++)
            {
                sb.Append(Grd.Columns[j].HeaderText); // headings
                sb.Append(strSepValue); // delimiter
            }
            sb.Append("\n");
            // build the csv contents
            string replVal = String.Empty;
            for (int r = 0; r < Grd.Rows.Count; r++)
            {
                for (int c = 0; c < Grd.Columns.Count; c++)
                {
                    string strCellValue = Grd.Rows[r].Cells[c].Text;
                    strCellValue = "" + Grd.Rows[r].Cells[c].Text;
                    if (strCellValue == "")
                    {
                        strCellValue = ((System.Web.UI.WebControls.Label)Grd.Rows[r].Cells[c].Controls[1]).Text;
                    }
                    strCellValue = strCellValue.Replace(",", ";");
                    if (strCellValue == "&nbsp;")
                    {
                        replVal = "";
                    }
                    else if (strCellValue.Contains("\r"))
                    {
                        replVal = strCellValue.Replace("\r", " ");
                        replVal = replVal.Replace("\n", "");
                    }
                    else
                        replVal = strCellValue.Replace("\"", quoter);
                    replVal += strSepValue;
                    sb.Append(replVal);
                }//end if
                sb.Append("\n"); // new row
            }// end while

            for (int c = 0; c < Grd.Columns.Count; c++)
            {
                string strText = Grd.FooterRow.Cells[c].Text;
                strText = strText.Replace(",", ";");
                if (strText == "&nbsp;")
                {
                    replVal = "";
                }
                else if (strText.Contains("\r"))
                {
                    replVal = strText.Replace("\r", " ");
                    replVal = replVal.Replace("\n", "");
                }
                else
                    replVal = strText.Replace("\"", quoter);
                replVal += strSepValue;
                sb.Append(replVal);
            }//end if
            sb.Append("\n");
            string strFile = ReportTitle + " " + DateTime.Now.Ticks.ToString() + strFileExtension;
            string strFileContent = sb.ToString();
            FileInfo fi = new FileInfo(pg.Server.MapPath(strFile));
            FileStream sWriter = fi.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            sWriter.Write(System.Text.Encoding.ASCII.GetBytes(strFileContent), 0, strFileContent.Length);
            sWriter.Flush();
            sWriter.Close();
            fi = null;
            sWriter = null;
            Message[0] = "1";
            Message[1] = strFile;
        }
        catch (Exception excp)
        {
            String errorMessage;
            errorMessage = "Error: " + excp.Message;
            errorMessage = String.Concat(errorMessage, excp.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, excp.Source);
            Message[0] = "0";
            Message[1] = errorMessage;
        }
        return Message;
    }

    public static string[] GenerateCSVReportFromDataTable(System.Data.DataTable DT, string ReportTitle)
    {
        System.Web.UI.Page pg = new System.Web.UI.Page();
        string strCurrentDir = pg.Server.MapPath(".") + "\\";
        RemoveFiles(strCurrentDir);
        string[] Message = { "0", "" };
        string strSepValue = ",", strFileExtension = ".csv";
        try
        {
            StringBuilder sb = new StringBuilder(); // to hold csv/Excel text file
            // Create Header and sheet...
            string quoter = @"""""";
            sb.Append(ReportTitle + "\n");
            sb.Append("Report Date-Time : " + DateTime.Now.ToString() + "\n\n");
            for (int j = 0; j < DT.Columns.Count; j++)
            {
                sb.Append(DT.Columns[j].ToString()); // headings
                sb.Append(strSepValue); // delimiter
            }
            sb.Append("\n");
            // build the csv contents
            string replVal = String.Empty;
            foreach (DataRow dr in DT.Rows)
            {
                for (int k = 0; k < DT.Columns.Count; k++)
                {
                    string strText = dr[k].ToString();
                    strText = strText.Replace(",", ";");
                    if (strText == "&nbsp;")
                    {
                        replVal = "";
                    }
                    else if (strText.Contains("\r"))
                    {
                        replVal = strText.Replace("\r", " ");
                        replVal = replVal.Replace("\n", "");
                    }
                    else
                        replVal = strText.Replace("\"", quoter);
                    replVal += " " + strSepValue;
                    sb.Append(replVal);
                }//end if
                sb.Append("\n"); // new row
            }// end while
            string strFile = ReportTitle + " " + DateTime.Now.Ticks.ToString() + strFileExtension;
            string strFileContent = sb.ToString();
            FileInfo fi = new FileInfo(pg.Server.MapPath(strFile));
            FileStream sWriter = fi.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            sWriter.Write(System.Text.Encoding.ASCII.GetBytes(strFileContent), 0, strFileContent.Length);
            sWriter.Flush();
            sWriter.Close();
            fi = null;
            sWriter = null;
            Message[0] = "1";
            Message[1] = strFile;
        }
        catch (Exception excp)
        {
            String errorMessage;
            errorMessage = "Error: " + excp.Message; ;
            errorMessage = String.Concat(errorMessage, excp.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, excp.Source);
            Message[0] = "0";
            Message[1] = errorMessage;
        }
        return Message;
    }

    public static string[] GenerateCSVReportFromQuery(string sqlquery, string ReportTitle)
    {
        System.Web.UI.Page pg = new System.Web.UI.Page();
        string strCurrentDir = pg.Server.MapPath(".") + "\\";
        RemoveFiles(strCurrentDir);
        string[] Message = { "0", "" };
        string strSepValue = ",", strFileExtension = ".csv";
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(sqlquery, sqlcon);
            SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            StringBuilder sb = new StringBuilder(); // to hold csv/Excel text file
            // Create Header and sheet...
            string quoter = @"""""";
            sb.Append(ReportTitle + "\n");
            sb.Append("Report Date-Time : " + DateTime.Now.ToString() + "\n\n");
            for (int j = 0; j < myReader.FieldCount; j++)
            {
                sb.Append(myReader.GetName(j).ToString()); // headings
                sb.Append(strSepValue); // delimiter
            }
            sb.Append("\n");
            // build the csv contents
            string replVal = String.Empty;
            while (myReader.Read())
            {
                for (int k = 0; k < myReader.FieldCount; k++)
                {
                    string strText = myReader.GetValue(k).ToString();
                    strText = strText.Replace(",", ";");
                    if (strText == "&nbsp;")
                    {
                        replVal = "";
                    }
                    else if (strText.Contains("\r"))
                    {
                        replVal = strText.Replace("\r", " ");
                        replVal = replVal.Replace("\n", "");
                    }
                    else
                        replVal = strText.Replace("\"", quoter);
                    replVal += " " + strSepValue;
                    sb.Append(replVal);
                }//end if
                sb.Append("\n"); // new row
            }// end while
            myReader.Close();
            myReader = null;
            string strFile = ReportTitle + " " + DateTime.Now.Ticks.ToString() + strFileExtension;
            string strFileContent = sb.ToString();
            FileInfo fi = new FileInfo(pg.Server.MapPath(strFile));
            FileStream sWriter = fi.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            sWriter.Write(System.Text.Encoding.ASCII.GetBytes(strFileContent), 0, strFileContent.Length);
            sWriter.Flush();
            sWriter.Close();
            fi = null;
            sWriter = null;
            Message[0] = "1";
            Message[1] = strFile;
            sqlcon.Close();
            sqlcon.Dispose();
        }
        catch (Exception excp)
        {
            String errorMessage;
            errorMessage = "Error: " + excp.Message;
            errorMessage = String.Concat(errorMessage, excp.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, excp.Source);
            Message[0] = "0";
            Message[1] = errorMessage;
        }
        return Message;
    }

    private static void RemoveFiles(string strPath)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(strPath);
        FileInfo[] fiArr = di.GetFiles();
        foreach (FileInfo fri in fiArr)
        {
            if (fri.Extension.ToString() == ".xls" || fri.Extension.ToString() == ".csv")
            {
                TimeSpan min = new TimeSpan(0, 0, 60, 0, 0);
                //if (fri.CreationTime < DateTime.Now.Subtract(min))
                {
                    fri.Delete();
                }
            }
        }
    }

    public static bool RecordAllreadyExists(string strTable, string strMatchField, string strMatchFieldText)
    {
        bool blnFound = false;
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "Select [" + strMatchField + "] From [" + strTable + "] Where  [" + strMatchField + "]='" + strMatchFieldText + "'";
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnFound = true;
            }
           
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
            //blnFound = true;
        }
        finally { sqlcon.Close();
        sqlcon.Dispose();
        }
        return blnFound;
    }

    public static System.Data.DataTable GetTableWithNewRow(System.Data.DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dr = null;
        return dt;
    }

    public static System.Data.DataTable MakeTableFromGridViewForPrinting(GridView grd)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        int grows = grd.Rows.Count;
        int gcols = grd.Columns.Count;

        for (int c = 0; c < gcols; c++)
        {
            dt.Columns.Add(grd.Columns[c].HeaderText);
        }
        for (int r = 0; r < grows; r++)
        {
            DataRow dr = dt.NewRow();
            for (int c = 0; c < gcols; c++)
            {
                dr[c] = grd.Rows[r].Cells[c].Text;
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static int generateNewID(string strTableName, string strIDColumn)
    {
        int ReturnID = 1;
        int MaxValue = 0;
        int MaxValueForReturn = 0;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;
        string strSql = "Select [" + strIDColumn + "] From [" + strTableName + "]";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string ReturnID1 = "" + sdr.GetValue(0);
                if (ReturnID1 != "")
                {
                    string[] Temp = ReturnID1.Split('-');
                    if (Convert.ToInt32(Temp[1]) > 0)
                    {
                        MaxValue = Convert.ToInt32(Temp[1]);
                        if (MaxValue > MaxValueForReturn)
                        {
                            MaxValue = Convert.ToInt32(Temp[1]);
                            MaxValueForReturn = MaxValue;
                        }
                    }
                }
                else
                {
                    ReturnID = int.Parse("0" + sdr.GetValue(0)) + 1;
                }
            }
        }
        catch
        {
            ReturnID = -1;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID = MaxValueForReturn + 1;
    }

    public static int getNewID(string strTableName, string strIDColumn)
    {
        int ReturnID = 1;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;
        string strSql = "Select Max([" + strIDColumn + "]) From [" + strTableName + "]";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                ReturnID = int.Parse("0" + sdr.GetValue(0)) + 1;
            }
        }
        catch
        {
            ReturnID = -1;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID;
    }

    public static int getNewIDAccordingBID(string strTableName, string strIDColumn, string BID)
    {
        int ReturnID = 1;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;
        string strSql = "Select Max([" + strIDColumn + "]) From [" + strTableName + "] where Branchid =" + BID + "";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                ReturnID = int.Parse("0" + sdr.GetValue(0)) + 1;
            }
        }
        catch
        {
            ReturnID = -1;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID;
    }

    public static bool getAccessRightMoveAllButtons(string strScreenName, string strTableName, string BID)
    {
        //int ReturnID = 1;
        bool ReturnID = false;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;
        string strSql = "Select [" + strScreenName + "] From [" + strTableName + "] where Branchid =" + BID + "";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                //ReturnID = sdr.GetValue(0).ToString();
                ReturnID = Convert.ToBoolean(sdr.GetValue(0));
            }
        }
        catch
        {
            ReturnID = false;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID;
    }

    public static bool getAccessRightMoveAllButtonsForFactory(string strScreenName, string BID, string WSUserID)
    {
        //int ReturnID = 1;
        bool ReturnID = false;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;     

        string strSql = " select RightToView from  EntMenuRights where UserTypeId=4  and BranchId=" + BID + " and PageTitle='" + strScreenName + "' and  WorkshopUserType=" + WSUserID + "";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                //ReturnID = sdr.GetValue(0).ToString();
                ReturnID = Convert.ToBoolean(sdr.GetValue(0));
            }
        }
        catch
        {
            ReturnID = false;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID;
    }

    public static int getNewIDAccordingEXBID(string strTableName, string strIDColumn, string BID)
    {
        int ReturnID = 1;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;
        string strSql = "Select Max([" + strIDColumn + "]) From [" + strTableName + "] where ExternalBranchId =" + BID + "";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                ReturnID = int.Parse("0" + sdr.GetValue(0)) + 1;
            }
        }
        catch
        {
            ReturnID = -1;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID;
    }

    public static int getMaxValue(string strTableName, string strIDColumn)
    {
        int ReturnID = 0;
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataReader sdr = null;
        string strSql = "Select Max([" + strIDColumn + "]) From [" + strTableName + "]";
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlcon);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                ReturnID = int.Parse("0" + sdr.GetValue(0));
            }
        }
        catch
        {
            ReturnID = -1;
        }
        finally
        {
            if (sdr != null) sdr.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return ReturnID;
    }

    public static string ConvertToDateFromDMYToMDY(string strDateToBeConvertedSeparatedByForwardSlash)
    {
        string strReturnValue = "";
        try
        {
            string[] strDT = strDateToBeConvertedSeparatedByForwardSlash.Split('/');
            if (strDT.Length == 3)
            {
                strReturnValue = strDT[1] + '-' + strDT[0] + '-' + strDT[2];
            }
        }
        catch { }
        return strReturnValue;
    }

    public static string ConvertToDateInToSystemFormat(string strDateToBeConvertedSeparatedByForwardSlash, string Saperator)
    {
        string strReturnDate = "", strDay = "", strMonth = "", strYear = "";
        if (Saperator == "-")
        {
            try
            {
                string[] strDT = strDateToBeConvertedSeparatedByForwardSlash.Split('/');
                strDay = strDT[0];
                strMonth = strDT[1];
                strYear = strDT[2];
                strReturnDate = strDay + "-" + strMonth + "-" + strYear;
            }
            catch { }
        }
        else if (Saperator == "/")
        {
            try
            {
                string[] strDT = strDateToBeConvertedSeparatedByForwardSlash.Split('-');
                strDay = strDT[0];
                strMonth = strDT[1];
                strYear = strDT[2];
                strReturnDate = strDay + "/" + strMonth + "/" + strYear;
            }
            catch { }
        }
        return strReturnDate;
    }

    public static string ConvertToDateFromMDYToDMY(string strDateToBeConvertedSeparatedByForwardSlash)
    {
        string strReturnValue = "";
        try
        {
            string[] strDT = strDateToBeConvertedSeparatedByForwardSlash.Split('/');
            if (strDT.Length == 3)
            {
                strReturnValue = (new DateTime(int.Parse(strDT[2]), int.Parse(strDT[0]), int.Parse(strDT[1])).ToShortDateString());
            }
        }
        catch { }
        return strReturnValue;
    }

    public static string ConvertToDate(string strDateToBeConvertedSeparatedByForwardSlash, string Saperator)
    {
        string strReturnDate = "", strDay = "", strMonth = "", strYear = "";
        if (Saperator == "-")
        {
            try
            {
                string[] strDT = strDateToBeConvertedSeparatedByForwardSlash.Split('-');
                strDay = strDT[0];
                strMonth = strDT[1];
                strYear = strDT[2];
                strReturnDate = strMonth + "-" + strDay + "-" + strYear;
            }
            catch { }
        }
        else if (Saperator == "/")
        {
            try
            {
                string[] strDT = strDateToBeConvertedSeparatedByForwardSlash.Split('/');
                strDay = strDT[0];
                strMonth = strDT[1];
                strYear = strDT[2];
                strReturnDate = strMonth + "/" + strDay + "/" + strYear;
            }
            catch { }
        }
        return strReturnDate;
    }

    public static StringBuilder ShowReport(GridView grd, string strFilePathToSave, string strCaption)
    {
        string strCellValue = "";
        StringBuilder strReportTable = new StringBuilder();
        if (grd.Rows.Count > 0)
        {
            strReportTable.Append("<table>");
            strReportTable.Append("<tr><td colspan='" + grd.HeaderRow.Cells.Count.ToString() + "'>" + strCaption + "</td></tr>");
            strReportTable.Append("<tr>");
            for (int c = 0; c < grd.HeaderRow.Cells.Count; c++)
            {
                strReportTable.Append("<th>" + grd.HeaderRow.Cells[c].Text + "</th>");
            }
            strReportTable.Append("</tr>");
            for (int r = 0; r < grd.Rows.Count; r++)
            {
                strReportTable.Append("<tr>");
                for (int c = 0; c < grd.HeaderRow.Cells.Count; c++)
                {
                    strCellValue = "";

                    if (grd.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        try
                        {
                            if (grd.Rows[r].Cells[c].Controls[0].GetType().ToString() == "System.Web.UI.LiteralControl")
                            {
                                strCellValue = ((System.Web.UI.WebControls.Label)grd.Rows[r].Cells[c].Controls[1]).Text;
                            }
                            else
                            {
                                strCellValue = ((System.Web.UI.WebControls.Label)grd.Rows[r].Cells[c].Controls[0]).Text;
                            }
                        }
                        catch
                        { }
                    }
                    if (strCellValue == "")
                    {
                        strCellValue = grd.Rows[r].Cells[c].Text;
                    }
                    strReportTable.Append("<td>" + strCellValue + "</td>");
                }
                strReportTable.Append("</tr>");
            }

            strReportTable.Append("</table>");
        }

        StreamWriter StreamWriter1 = new StreamWriter(strFilePathToSave);
        StreamWriter1.Write(strReportTable.ToString());
        StreamWriter1.Close();
        return strReportTable;
    }

    public static StringBuilder ShowReport1(GridView grd, string strFilePathToSave, string strCaption)
    {
        string strCellValue = "";
        StringBuilder strReportTable = new StringBuilder();
        if (grd.Rows.Count > 0)
        {
            strReportTable.Append("<table>");
            strReportTable.Append("<tr><td colspan='" + grd.HeaderRow.Cells.Count.ToString() + "'>" + strCaption + "</td></tr>");

            for (int r = 0; r < grd.Rows.Count; r++)
            {
                strReportTable.Append("<tr>");
                for (int c = 0; c < grd.HeaderRow.Cells.Count; c++)
                {
                    strCellValue = "";

                    if (grd.Rows[r].Cells[c].Controls.Count > 0)
                    {
                        try
                        {
                            if (grd.Rows[r].Cells[c].Controls[0].GetType().ToString() == "System.Web.UI.LiteralControl")
                            {
                                strCellValue = ((System.Web.UI.WebControls.Label)grd.Rows[r].Cells[c].Controls[1]).Text;
                            }
                            else
                            {
                                strCellValue = ((System.Web.UI.WebControls.Label)grd.Rows[r].Cells[c].Controls[0]).Text;
                            }
                        }
                        catch
                        { }
                    }
                    if (strCellValue == "")
                    {
                        strCellValue = grd.Rows[r].Cells[c].Text;
                    }
                    strReportTable.Append("<td>" + strCellValue + "</td>");
                }
                strReportTable.Append("</tr>");
            }

            strReportTable.Append("</table>");
        }

        StreamWriter StreamWriter1 = new StreamWriter(strFilePathToSave);
        StreamWriter1.Write(strReportTable.ToString());
        StreamWriter1.Close();
        return strReportTable;
    }

    public static void PrintWebControl(GridView GridViewNameForPrint)
    {
        PrintWebControl(GridViewNameForPrint, string.Empty);
    }

    public static void PrintWebControl(GridView ctrl, string Script)
    {
        StringWriter stringWrite = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
        if (ctrl is WebControl)
        {
            Unit w = new Unit(100, UnitType.Percentage); ((WebControl)ctrl).Width = w;
        }
        System.Web.UI.Page pg = new System.Web.UI.Page();
        pg.EnableEventValidation = false;
        if (Script != string.Empty)
        {
            pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
        }
        HtmlForm frm = new HtmlForm();
        pg.Controls.Add(frm);
        frm.Attributes.Add("runat", "server");
        frm.Controls.Add(ctrl);
        pg.DesignerInitialize();
        pg.RenderControl(htmlWrite);
        string strHTML = stringWrite.ToString();
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Write(strHTML);
        HttpContext.Current.Response.Write("<script>window.print();</script>");
        HttpContext.Current.Response.End();
    }

    public static string ExecuteNonQuery(SqlCommand cmd)
    {
        string saveSuccess = string.Empty;
        int execute = 0;
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
            execute = cmd.ExecuteNonQuery();
            if (execute > 0)
            {
                saveSuccess = "Record Saved";
            }
        }
        catch (Exception excp)
        {
            if (excp.Message.Contains("Violation of PRIMARY KEY constraint"))
                saveSuccess = PrjClass.UNIQUE;
            else if (excp.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                saveSuccess = PrjClass.ReferenceMessage;
            else if (excp.Message.Contains("Violation of UNIQUE KEY constraint"))
                saveSuccess = PrjClass.UNIQUE;
            else if (excp.Message.Contains("Object reference not set to an instance of an object"))
                saveSuccess = "Object are not set anywhere";
            else if (excp.Message.Contains("Error converting data type nvarchar to float."))
                saveSuccess = "Invalid opening balance.";
            else if (excp.Message.Contains("The ROLLBACK TRANSACTION request has no corresponding BEGIN TRANSACTION."))
                saveSuccess = "Phone no already exist.";
            else
                saveSuccess = excp.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return saveSuccess;
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
            ////con.Close();
        }
        return dr;
    }

    public static DataSet GetData(SqlCommand cmd)
    {
        SqlConnection con = new SqlConnection(sqlConStr);
        SqlDataAdapter adap = new SqlDataAdapter();
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        else
        {
            con.Open();
        }
        DataSet ds = new DataSet();
        cmd.Connection = con;
        cmd.CommandTimeout = 0;
        adap.SelectCommand = cmd;
        adap.Fill(ds, "table");
        adap.Dispose();
        
        con.Close();
        con.Dispose();
        return ds;
    }

    public static bool CheckRecordUsedInAnotherTable(string strTable, string strMatchField, string strMatchFieldText)
    {
        bool blnFound = false;
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "Select [" + strMatchField + "] From [" + strTable + "] Where  [" + strMatchField + "]='" + strMatchFieldText + "'";
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnFound = true;
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
            blnFound = true;
        }
        finally { sqlcon.Close();
        sqlcon.Dispose();
        }
        return blnFound;
    }
    public static string GetPrinterName(string BranchId)
    {
        string blnFound = string.Empty;
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@Flag", 14);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnFound = sdr.GetValue(0).ToString();
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
            
        }
        finally { sqlcon.Close();
        sqlcon.Dispose();
        }
        return blnFound;
    }
    public static string GetDeliveryPrinterName(string BranchId)
    {
        string blnFound = string.Empty;
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", BranchId);
            cmd.Parameters.AddWithValue("@Flag", 16);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnFound = sdr.GetValue(0).ToString();
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {

        }
        finally
        {
            sqlcon.Close();
            sqlcon.Dispose();
        }
        return blnFound;
    }

    public static string GetFromAndToDateOfCurrentYear()
    {
        string strDate = string.Empty;
        DateTime dt = DateTime.Today;
        dt = new DateTime(DateTime.Today.Year, 1, 1);
        strDate = dt.ToString("dd MMM yyyy").Trim() + " - " + dt.AddYears(+1).AddDays(-1).ToString("dd MMM yyyy").Trim();
        return strDate;
    }
    public static string GetFromAndToDateOfCurrentMonth()
    {
        string strDate = string.Empty;          
        DateTime today = DateTime.Today;
        int numberOfDaysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
        DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
        DateTime endOfMonth = new DateTime(today.Year, today.Month, numberOfDaysInMonth);
        var FromDate = startOfMonth.ToString("dd MMM yyyy");
        var ToDate = endOfMonth.ToString("dd MMM yyyy");
        strDate = FromDate + " - " + ToDate;  
        return strDate;
    }
    public static string GetSoftwareVersionName()
    {
        string blnFound = string.Empty;
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "sp_City";
            cmd.CommandType = CommandType.StoredProcedure;            
            cmd.Parameters.AddWithValue("@Flag", 15);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnFound = sdr.GetValue(0).ToString();
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {

        }
        finally { sqlcon.Close(); }
        return blnFound;
    }

    public static string GetReasonAccordingToScreenName(string Url)
    {
        string strMsg = string.Empty;

        if (Url.Contains("Reports/QuantityandPriceReport.aspx"))
        {
            strMsg = "Booking";
        }
        else if (Url.Contains("Reports/frmProcessWiseSummary.aspx"))
        {
            strMsg = "Service wise order summary";
        }
        else if (Url.Contains("Reports/frmItemWiseDetailReport.aspx"))
        {
            strMsg = "Item wise order summary";
        }
        else if (Url.Contains("Reports/frmServiceTaxReport.aspx"))
        {
            strMsg = "Service Tax";
        }
        else if (Url.Contains("Reports/frmRemoveCloth.aspx"))
        {
            strMsg = "Remove garments";
        }
        else if (Url.Contains("Reports/CancelQuantityandPriceReport.aspx"))
        {
            strMsg = "Cancel booking";
        }
        else if (Url.Contains("Reports/BookingReportWithoutSlip.aspx"))
        {
            strMsg = "Without ticket delivery";
        }
        else if (Url.Contains("Reports/AreaLocationReport.aspx"))
        {
            strMsg = "Area Location";
        }
        else if (Url.Contains("Reports/BookingByCustomerReport.aspx"))
        {
            strMsg = "Booking by Customer";
        }
        else if (Url.Contains("Reports/SearchByInvoice.aspx"))
        {
            strMsg = "Search Order";
        }

        return strMsg;
    }

    public static bool CheckRecordUsedInAnotherTableInProcess(string strTable, string strMatchField, string strMatchFieldText, string strMatchProject, string strMatchProjectCode)
    {
        bool blnFound = false;
        SqlConnection sqlcon = new SqlConnection(sqlConStr);
        try
        {
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlcon;
            cmd.CommandText = "Select [" + strMatchField + "] From [" + strTable + "] Where  [" + strMatchField + "]='" + strMatchFieldText + "' AND [" + strMatchProject + "]='" + strMatchProjectCode + "' ";
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                blnFound = true;
            }
            sdr.Close();
            sdr.Dispose();
        }
        catch (Exception)
        {
            blnFound = false;
        }
        finally { sqlcon.Close();
        sqlcon.Dispose();
        }
        return blnFound;
    }

    public static string GetReturnGroupName(string Code, string FYear)
    {
        string groupName = "";
        PrjClass.commandForQuery = new SqlCommand();
        PrjClass.commandForQuery.CommandText = "Sp_SoftTaxLogin";
        PrjClass.commandForQuery.CommandType = CommandType.StoredProcedure;
        PrjClass.commandForQuery.Parameters.AddWithValue("@Code", Code);
        PrjClass.commandForQuery.Parameters.AddWithValue("@FYearId", FYear);
        PrjClass.commandForQuery.Parameters.AddWithValue("@Flag", 5);
        PrjClass.sdr = PrjClass.ExecuteReader(PrjClass.commandForQuery);
        if (PrjClass.sdr.Read())
        {
            groupName = "" + PrjClass.sdr.GetValue(0);
        }
        return groupName;
    }

    private static string getVolumeSerial(string drive)
    {
        ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
        disk.Get();
        string volumeSerial = disk["VolumeSerialNumber"].ToString();
        disk.Dispose();
        return volumeSerial;
    }

    private static string getCPUID()
    {
        string cpuInfo = "";
        ManagementClass mc = new ManagementClass("win32_processor");
        ManagementObjectCollection moc = mc.GetInstances();
        foreach (ManagementObject mo in moc)
        {
            if (cpuInfo == "")
            {
                cpuInfo = mo.Properties["processorID"].Value.ToString();
                break;
            }
        }
        return cpuInfo;
    }

    public static bool IsGZipSupported()
    {
        string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

        if (!string.IsNullOrEmpty(AcceptEncoding) && AcceptEncoding.Contains("gzip"))
            return true;

        return false;
    }

    public static void ShowPdfFromRdlc(System.Web.UI.Page page, byte[] renderedBytes, string fileName)
    {
        var fileNameInner = Path.Combine("../Docs/", fileName);
        using (var fs = new FileStream(page.Server.MapPath(fileNameInner), FileMode.Create))
        {
            fs.Write(renderedBytes, 0, renderedBytes.Length);
            fs.Close();
        }

        OpenWindow(page, fileNameInner);
        
    }

    public static void ShowPdfFromRdlcDirect(System.Web.UI.Page page, byte[] renderedBytes, string fileName)
    {
        var fileNameInner = Path.Combine("../Docs/", fileName);
        using (var fs = new FileStream(page.Server.MapPath(fileNameInner), FileMode.Create))
        {
            fs.Write(renderedBytes, 0, renderedBytes.Length);
            fs.Close(); 
        }

        //OpenWindow(page, fileNameInner);
        
    }


    private static void OpenWindow(System.Web.UI.Page page, string url)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }
}

public class myModule : IHttpModule
{
    public void Init(HttpApplication app)
    {
        //app.BeginRequest += new EventHandler(app_BeginRequest);
        //app.AcquireRequestState += new EventHandler(app_AcquireRequestState);
        //app.PreRequestHandlerExecute += new EventHandler(app_PreRequestHandlerExecute);
    }

    private void app_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        /*
        var ctx = ((HttpApplication)sender).Context;
        var req = ((HttpApplication)sender).Request;
        var ss = ((HttpApplication)sender).Session;
        var res = ((HttpApplication)sender).Response;

        //Only access session state if it is available
        if (ctx.Handler is IRequiresSessionState || ctx.Handler is IReadOnlySessionState)
        {
            //If we are authenticated AND we dont have a session here.. redirect to login page.
            HttpCookie authenticationCookie = req.Cookies[FormsAuthentication.FormsCookieName];
            if (authenticationCookie != null)
            {
                FormsAuthenticationTicket authenticationTicket = FormsAuthentication.Decrypt(authenticationCookie.Value);
                if (!authenticationTicket.Expired)
                {
                    //of course.. replace ANYKNOWNVALUEHERETOCHECK with "UserId" or something you set on the login that you can check here to see if its empty.
                    if (ss["UserType"] == null)
                    {
                        //This means for some reason the session expired before the authentication ticket. Force a login.
                        FormsAuthentication.SignOut();
                        res.Redirect(FormsAuthentication.LoginUrl, true);
                        return;
                    }
                }
            }
        }*/
    }

    private void app_AcquireRequestState(object sender, EventArgs e)
    {
        /*
        // check if handler is valid
        var ss = ((HttpApplication)sender).Session;
        var res = ((HttpApplication)sender).Response;
        var req = ((HttpApplication)sender).Request;
        var usr = ((HttpApplication)sender).User;
        if (ss == null || ss.IsNewSession)
        {
            string szCookieHeader = req.Headers["Cookie"];
            if ((szCookieHeader != null) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
            {
                if (usr.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    res.Redirect(req.RawUrl);
                }
            }
        }*/
    }

    private void app_BeginRequest(object sender, EventArgs e)
    {
        /*
        var ctx = ((HttpApplication)sender).Context;
        var req = ((HttpApplication)sender).Request;
        var ss = ((HttpApplication)sender).Session;
        var res = ((HttpApplication)sender).Response;

        //Only access session state if it is available
        if (ctx.Handler is IRequiresSessionState || ctx.Handler is IReadOnlySessionState)
        {
            //If we are authenticated AND we dont have a session here.. redirect to login page.
            HttpCookie authenticationCookie = req.Cookies[FormsAuthentication.FormsCookieName];
            if (authenticationCookie != null)
            {
                FormsAuthenticationTicket authenticationTicket = FormsAuthentication.Decrypt(authenticationCookie.Value);
                if (!authenticationTicket.Expired)
                {
                    //of course.. replace ANYKNOWNVALUEHERETOCHECK with "UserId" or something you set on the login that you can check here to see if its empty.
                    if (ss["UserType"] == null)
                    {
                        //This means for some reason the session expired before the authentication ticket. Force a login.
                        FormsAuthentication.SignOut();
                        res.Redirect(FormsAuthentication.LoginUrl, true);
                        return;
                    }
                }
            }
        }

        HttpContext context = ((HttpApplication)sender).Context;

        string AcceptEncoding = context.Request.Headers["Accept-Encoding"];
        if ((!string.IsNullOrEmpty(AcceptEncoding) && AcceptEncoding.Contains("gzip")) &&
                (!context.Request.Url.LocalPath.Contains("Resource.axd")) &&
                    (!context.Request.Url.LocalPath.Contains(".png")))
        {
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
            context.Response.AddHeader("Content-Encoding", "gzip");
        }
        */
    }

    public void Dispose()
    {
    }

    public bool IsResuable()
    {
        return false;
    }
}

public class myHandler : IHttpHandler
{
    public void ProcessRequest(System.Web.HttpContext context)
    {
        string AcceptEncoding = context.Request.Headers["Accept-Encoding"];
        if (!string.IsNullOrEmpty(AcceptEncoding) && AcceptEncoding.Contains("gzip"))
        {
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
            context.Response.AddHeader("Content-Encoding", "gzip");
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}