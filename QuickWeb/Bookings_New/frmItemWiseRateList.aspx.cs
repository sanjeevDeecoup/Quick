using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickWeb;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;

namespace QuickWeb.Bookings_New
{
    public partial class frmItemWiseRateList : System.Web.UI.Page
    {
        private string status = string.Empty;
        public static string col = string.Empty;
        public static string strRateListID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            var forEdit = (Request.QueryString["ForInput"] ?? "").ToString();
            var rateListName = (Request.QueryString["RateListName"] ?? "Default").ToString();
            lblRateListName.Text = rateListName + " Rate List";
            if (forEdit == string.Empty || forEdit == "false")
            {
                hdnForEdit.Value = "false";
                btnSave.Visible = false;
                btnExport.Visible = true;
            }
            else if (forEdit == "true")
            {
                hdnForEdit.Value = "true";
                btnExport.Visible = false;
            }

            var rateListId = (Request.QueryString["RateListId"] ?? "0").ToString();
            strRateListID = rateListId;

            DataSet dsData1 = GetDataForExcel();
            col = string.Empty;
            foreach (DataColumn column in dsData1.Tables[0].Columns)
            {
                col = col + ":" + column.ColumnName;
            }           
            SetTheProcesses();
            status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.PrintRateList, Session["UserType"].ToString());
            if (status == "True")
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
        }

        private void SetTheProcesses()
        {
            var sqlCommand = new SqlCommand { CommandText = "sp_ItemWiseRate_new", CommandType = CommandType.StoredProcedure };
            sqlCommand.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            sqlCommand.Parameters.AddWithValue("@Flag", 2);
            var processList = string.Empty;
            var sqlDataReader = PrjClass.ExecuteReader(sqlCommand);
            while (sqlDataReader != null && sqlDataReader.Read())
            {
                processList += sqlDataReader.GetString(0) + ":";
            }
            if (sqlDataReader != null)
                sqlDataReader.Close();

            if (processList.Length >= 1)
                processList = processList.Substring(0, processList.Length - 1);

            hdnProcesses.Value = processList;
        }
       

        protected void BtnSaveClick(Object sender, EventArgs e)
        {
            var rateListId = int.Parse((Request.QueryString["RateListId"] ?? "0").ToString());
            var rateListName = (Request.QueryString["RateListName"] ?? "Default").ToString();
            BAL.BALFactory.Instance.Bal_Report.SaveItemWiseRateList(hdnProcesses.Value, hdnRates.Value, rateListId, Globals.BranchID);
            Response.Redirect("~/Bookings_New/frmItemWiseRateList.aspx?RateListId=" + rateListId + "&ForInput=false&RateListName=" + rateListName, false);
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script language=\"Javascript\">window.opener = self;window.close();</script>");
        }

        protected void GrdDetailsRowEdit(Object sender, GridViewEditEventArgs e)
        {

        }

        protected void GrdDetailsRowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml)]
        public static string GetRateListData()
        {
            var ds = new DataSet();
            var cmd = new SqlCommand();
            DataSet dsReport = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_ItemWiseRate_new";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@RateListId", strRateListID);
        
            // Populate the DataSet.
            DataSet data = GetData(cmd);
           // DataSet data = PrjClass.GetData(cmd);

            col = string.Empty;
            foreach (DataColumn column in data.Tables[0].Columns)
            {
                col = col + ":" + column.ColumnName;
            }
            // return  table as XML.
            System.IO.StringWriter writer = new System.IO.StringWriter();
            data.Tables[0].WriteXml(writer, XmlWriteMode.WriteSchema, false);
            return writer.ToString();
        }

        private static DataSet GetData(SqlCommand cmd)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        sda.Dispose();
                        con.Close();
                        con.Dispose();
                        return ds;                        
                    }                   
                }
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFileName = "Rate_list.xls";
            Response.Expires = 0;
            Response.Buffer = true;

            GridView grdAllData = new GridView();
            DataSet dsData = GetDataForExcel();
            grdAllData.DataSource = dsData.Tables[0];
            grdAllData.DataBind();

            StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdAllData);
            string strFilePathToSave = "";
            Response.ContentType = "application/vnd.ms-excel";
            strFilePathToSave = "~/Docs/" + strFileName;
            StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
            StreamWriter1.Write(strDataToSaveInFile);
            StreamWriter1.Close();
            Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
            Response.Redirect(strFilePathToSave, false);
        }

        public DataSet GetDataForExcel()
        {

            var rateListId1 = (Request.QueryString["RateListId"] ?? "0").ToString();
            var ds = new DataSet();
            var cmd = new SqlCommand();
            DataSet dsReport = new ChallanDataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_ItemWiseRate_new";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@RateListId", rateListId1);
            dsReport = PrjClass.GetData(cmd);
            return dsReport;
        }
    }
}