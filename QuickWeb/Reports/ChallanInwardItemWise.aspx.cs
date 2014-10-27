using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using QuickWeb;

namespace QuickWeb.Reports
{
    public partial class ChallanInwardItemWise : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = true;
                
            }            

        }
        public void save()
        {
            SqlCommand cmd = new SqlCommand();
            DataSet dsReport = new ChallanDataSet();
            DataSet ds = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_ChallanInwardSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("BranchId", Globals.BranchID);
            dsReport = PrjClass.GetData(cmd);
            ds = PrjClass.GetData(cmd);
            grdTmp.DataSource = ds;
            grdTmp.DataBind();
            if (dsReport.Tables[0].Rows.Count > 0)
            {
                ReportViewer1.Visible = true;
            }
            ReportViewer1.LocalReport.ReportPath = "RDLC/ChallanInward.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "ChallanInwardDataSet";
            ReportParameter[] parameters = new ReportParameter[2];
            //parameters[0] = new ReportParameter("FDate", Request.QueryString["Date"].ToString());
            //parameters[1] = new ReportParameter("UDate", Request.QueryString["Date1"].ToString());
            parameters[0] = new ReportParameter("details", lblStoreName.Text);
            parameters[1] = new ReportParameter("Quantity", lblQuantity.Text);
            ReportViewer1.LocalReport.SetParameters(parameters);
            rds.Value = dsReport.Tables[0];
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            save();
        }
    }
}