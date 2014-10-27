using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Services;
namespace QuickWeb.New_Admin
{
    public partial class frmAllCustomerData : System.Web.UI.Page
    {
        private string status = string.Empty;
        public static bool blnRight = false;
        private static DTO.Report Ob1 = new DTO.Report();
        private DTO.CustomerMaster Ob = new DTO.CustomerMaster();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDefault();
                blnRight = AppClass.CheckExportToExcelRightOnPage();
                if (blnRight)
                {
                    btnExportExcel.Visible = true;
                }
                else
                {
                    btnExportExcel.Visible = false;
                }
            }
            if (Request.Params["__EVENTTARGET"] as string == "ctl00$ContentPlaceHolder1$btnExportExcel")
            {
                btnExportExcel_Click(null, EventArgs.Empty);
            }
        }

        private void BindDefault()
        {
            status = BAL.BALFactory.Instance.BAL_Profession.SetButtonAccordingMenuRights(Globals.BranchID, SpecialAccessRightName.AllowCustExcel, Session["UserType"].ToString());
            if (status == "True")
            {
                btnExportExcel.Visible = true;
            }
            else
            {
                btnExportExcel.Visible = false;
            }           
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dtTmp = new DataTable();
            DataSet ds = new DataSet();
            Ob.BranchId = Globals.BranchID;
            ds = BAL.BALFactory.Instance.BL_CustomerMaster.ExportToExcel(Ob);
            dtTmp = ds.Tables[0];
            if (dtTmp.Rows.Count > 0)
            {
                GridView grd = new GridView();
                grd.DataSource = dtTmp;
                grd.DataBind();
                string strFileName = "strReportFile.xls";
                Response.Expires = 0;
                Response.Buffer = true;
                StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grd);
                string strFilePathToSave = "";
                Response.ContentType = "application/vnd.ms-excel";
                strFilePathToSave = "~/Docs/" + strFileName;
                StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
                StreamWriter1.Write(strDataToSaveInFile);
                StreamWriter1.Close();
                Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
                Response.Redirect(strFilePathToSave, false);
            }
            else
            {
                lblErr.Text = "No customer record was found.";
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml)]
        public static string GetAllCustomer()
        {

            //SqlDataAdapter adap = new SqlDataAdapter();
            //string query = "SELECT [CustomerCode], COALESCE(CustomerSalutation,'') + ' ' + [CustomerName] As CustomerName, [CustomerAddress], [CustomerPhone], [CustomerMobile], [CustomerEmailId], [Priority], [CustomerRefferredBy],round([DefaultDiscountRate],2) as DefaultDiscountRate,(Case When [IsWebsite]='True' Then 'Yes' Else 'No' End) As IsWebsite  FROM [CustomerMaster] LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityId AND CustomerMaster.BranchId = dbo.PriorityMaster.BranchId  WHERE CustomerMaster.BranchId='" + Globals.BranchID + "' AND PriorityMaster.BranchId='" + Globals.BranchID + "' order by ID desc";
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_CustomerMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            cmd.Parameters.AddWithValue("@Flag", 9);
            DataSet data = GetData(cmd);
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
    
       
    }
}