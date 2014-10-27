using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace QuickWeb.New_Booking
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        public static string sqlConStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TID");
            dt.Columns.Add("RID");
            dt.Columns.Add("AssignId");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ConsumeQty");
            dt.Columns.Add("ConsumeDate");
            dt.Columns.Add("BookingNumber");
            dt.Columns.Add("BalQty");
            dt.Columns.Add("BranchId");
            var dr = dt.NewRow();
            dr["TID"] = 1;
            dr["RID"] = 23;
            dr["AssignId"] = 4;
            dr["ItemName"] = "SHIRT";
            dr["ConsumeQty"] = 2;
            dr["ConsumeDate"] = DateTime.Now.ToString("dd MMM yyyy");
            dr["BookingNumber"] = 454;
            dr["BalQty"] = 3;
            dr["BranchId"] = 1;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["TID"] = 1;
            dr["RID"] = 23;
            dr["AssignId"] = 4;
            dr["ItemName"] = "SHIRT";
            dr["ConsumeQty"] = 2;
            dr["ConsumeDate"] = DateTime.Now.ToString("dd MMM yyyy");
            dr["BookingNumber"] = 454;
            dr["BalQty"] = 3;
            dr["BranchId"] = 1;
            dt.Rows.Add(dr);
            using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(sqlConStr))
            {
                bulkCopy.DestinationTableName = "EntPackageConsume";
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulkCopy.WriteToServer(dt);
            } 
        }

        //public void test()
        //{
        //    var connection = new SqlConnection("Data Source=192.168.1.101;Initial Catalog=DRYSOFT1; User Id=sa; pwd=start;");
        //    connection.Open();

        //    // repeat for each table in data set
        //    var adapterForTable1 = new SqlDataAdapter("select * from EntPackageConsume", connection);
        //    var builderForTable1 = new SqlCommandBuilder(adapterForTable1);
        //    var dt = new DataTable();
        //    adapterForTable1.Fill(dt);
        //    var dr = dt.NewRow();
        //    dr["TID"] = 1;
        //    dr["RID"] = 23;
        //    dr["AssignId"] = 4;
        //    dr["ItemName"] = "SHIRT";
        //    dr["ConsumeQty"] = 2;
        //    dr["ConsumeDate"] = DateTime.Now.ToString("dd MMM yyyy");
        //    dr["BookingNumber"] = 454;
        //    dr["BalQty"] = 3;
        //    dr["BranchId"] = 1;
        //    dt.Rows.Add(dr);
        //    adapterForTable1.Update(dt);
        //}
    }
}