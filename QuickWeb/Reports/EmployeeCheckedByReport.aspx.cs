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

public partial class EmployeeCheckedByReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)     
            txtReportFrom.Focus();     
    }   

    protected void btnShowReport_Click(object sender, EventArgs e)
    {      
        ShowBookingDetails(txtReportFrom.Text);       
        if (grdReport.Rows.Count > 0)
        {           
            btnExport.Visible = true;
            grdReport.Visible = true;                  
        }
        else
        {
            btnExport.Visible = false;
        }
    }

    private void ShowBookingDetails(string InvoiceNo)
    {
        grdReport.DataSource = null;
        grdReport.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_EmployeeCheckedByReport";
        cmd.Parameters.Add(new SqlParameter("@BookingNo", InvoiceNo));
        cmd.Parameters.Add(new SqlParameter("@BranchId", Globals.BranchID)); 

        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlDataAdapter sadp = new SqlDataAdapter();
        DataSet dsMain = new DataSet();
        try
        {
            sqlcon.Open();
            cmd.Connection = sqlcon;
            sadp.SelectCommand = cmd;
            sadp.Fill(dsMain);
            if (dsMain.Tables.Count > 0)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    if (dsMain.Tables[0].Columns[0].ColumnName == "Error")
                    {
                        throw new Exception(dsMain.Tables[0].Rows[0][1].ToString());
                    }
                    grdReport.DataSource = dsMain.Tables[0];
                    grdReport.DataBind();
                    grdReport.Visible = true;
                    ViewState["SavedDS"]=dsMain;
                }
            }
        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
            sqlcon.Dispose();
        }

    }   

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;
		GridView grd=new GridView();
		grd.DataSource=(DataSet)ViewState["SavedDS"];
		grd.DataBind();
        StringBuilder strDataToSaveInFile = AppClass.GetExcelContentForGrid(grdReport);

        string strFilePathToSave = "";
        Response.ContentType = "application/vnd.ms-excel";
        strFilePathToSave = "~/Docs/" + strFileName;
        StreamWriter StreamWriter1 = new StreamWriter(Server.MapPath(strFilePathToSave));
        StreamWriter1.Write(strDataToSaveInFile);
        StreamWriter1.Close();
        Response.AddHeader("Content-Disposition", "inline; filename=" + strFileName);
        Response.Redirect(strFilePathToSave,false);
    }  

}
