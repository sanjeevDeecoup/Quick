using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
namespace QuickWeb.Admin
{
    public partial class UpdateWebsite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            bool internetconnection = NetworkInterface.GetIsNetworkAvailable();
            string connString = string.Empty;
            int i;
            this.imgLoading.Visible = true;
            Label1.Text = "";
            bool result = false;
            if (internetconnection == true)
            {
                DataSet ds = new DataSet();
                ds = BAL.BALFactory.Instance.BAL_City.RegDatabase();
                string[] strArray = new string[] { "BarCodeTable", "EntBookings", "EntBookingDetails", "EntPayment", "PriorityMaster", "mstPackageMaster", "AssignPackage", "mstreceiptConfig", "MstConfigSettings", "CustomerMaster", "WebsiteUsers", "EntledgerEntries" };

                connString = "Data Source=" + ds.Tables[0].Rows[0]["ServerName"].ToString() + ";Initial Catalog=" + ds.Tables[0].Rows[0]["DatabaseName"].ToString() + "; User Id=" + ds.Tables[0].Rows[0]["UserName"].ToString() + "; pwd=" + ds.Tables[0].Rows[0]["Password"].ToString() + ";packet size=4096;persist security info=True;Max Pool Size=1000;Connection Lifetime=5";
                SqlConnection sqlcon1 = new SqlConnection(PrjClass.sqlConStr);
                SqlConnection sqlcon2 = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon1;
                for (i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].ToString() == "WebsiteUsers")
                    {
                        sqlcon1.Open();
                        cmd.CommandText = "select * from " + strArray[i].ToString() + " where UserStatus<>2 and CustId not in (Select custid from CC." + ds.Tables[0].Rows[0]["DatabaseName"].ToString() + ".dbo.WebsiteUsers) and BranchId='" + Globals.BranchID + "'";
                        //SqlCommand cmd = new SqlCommand("select * from " + strArray[i].ToString() + " where UserStatus<>2", sqlcon1);
                    }
                    else
                    {
                        cmd.CommandText = "select * from " + strArray[i].ToString();
                        //SqlCommand cmd = new SqlCommand("select * from " + strArray[i].ToString(), sqlcon1);
                        sqlcon1.Open();

                        SqlCommand cmd1 = new SqlCommand("delete from " + strArray[i].ToString(), sqlcon2);
                        sqlcon2.Open();
                        cmd1.ExecuteNonQuery();
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlcon2.ConnectionString);
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.NotifyAfter = 1500;
                    //bulkCopy.SqlRowsCopied+=new SqlRowsCopiedEventHandler(bulkCopy_SqlRowsCopied);
                    bulkCopy.DestinationTableName = strArray[i].ToString();
                    bulkCopy.WriteToServer(reader);
                    reader.Close();
                    if (strArray[i].ToString() == "WebsiteUsers")
                    {
                        SqlCommand cmd2 = new SqlCommand("Update WebsiteUsers set UserStatus=2", sqlcon1);
                        cmd2.ExecuteNonQuery();
                        SqlCommand cmd3 = new SqlCommand("delete from CC." + ds.Tables[0].Rows[0]["DatabaseName"].ToString() + ".dbo.WebSiteUsers where CustId not in (Select CustId from WebSiteUsers) and BranchId='" + Globals.BranchID + "'", sqlcon1);
                        cmd3.ExecuteNonQuery();
                    }
                    sqlcon1.Close();
                    sqlcon2.Close();
                    result = true;
                }

                if (result == true)
                {
                    Label1.Text = "WebSite Data Sync Properly";
                    this.imgLoading.Visible = false;
                }
                else
                {
                    Label1.Text = "Website Data Sync Not Properly";
                    this.imgLoading.Visible = false;
                }
            }
            else
            {
                Label1.Text = "Please Interenet Connect";
            }
        }
    }
}