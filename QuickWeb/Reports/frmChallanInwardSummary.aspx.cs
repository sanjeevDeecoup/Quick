using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using Microsoft.Reporting.WebForms;
using QuickWeb;

namespace QuickWeb.Reports
{
    public partial class frmChallanInwardSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            DataSet dsReport = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_ChallanInwardSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("BranchId", Globals.BranchID);
            dsReport = PrjClass.GetData(cmd);
            ds = PrjClass.GetData(cmd);
           // RptChallan.LocalReport.ReportPath = "RDLC/ChallanInward.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "ChallanInwardDataSet_sp_Inward";
            //ReportParameter[] parameters = new ReportParameter[4];
            //parameters[0] = new ReportParameter("FDate", Request.QueryString["Date"].ToString());
            //parameters[1] = new ReportParameter("UDate", Request.QueryString["Date1"].ToString());
           // if (ds.Tables[1].Rows.Count > 0)
            //{
                //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //    lblStoreName.Text += ds.Tables[1].Rows[i]["Item"].ToString();
                //lblQuantity.Text += ds.Tables[2].Rows[0]["QTY"].ToString();
           // }

            //parameters[2] = new ReportParameter("details", lblStoreName.Text);
            //parameters[3] = new ReportParameter("Qty", lblQuantity.Text);

            // ReportViewer2.LocalReport.SetParameters(parameters);
            rds.Value = dsReport.Tables[0];
            //RptChallan.LocalReport.DataSources.Clear();
            //RptChallan.LocalReport.DataSources.Add(rds);
            //RptChallan.LocalReport.Refresh();
        }

    }
}