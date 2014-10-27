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


public partial class StockReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowReport("0","", grdReport);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        string strUserType = "" + Session["UserType"];
        if (strUserType != "1")
        {
            grdReport.ShowFooter = false;
            btnExport.Visible = false;
        }
    }

    private void ShowReport(string strFlag, string strItemName, GridView grd)
    {
        grd.DataSource = null;
        grd.DataBind();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "Sp_Sel_StockReport";
        cmd.Parameters.Add(new SqlParameter("@Flag", strFlag));
        cmd.Parameters.Add(new SqlParameter("@ItemName", strItemName));
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
                    grd.DataSource = dsMain.Tables[0];
                    grd.DataBind();
                    grd.Visible = true;
                    CalculateGridReport1();
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

    private void CalculateGridReport1()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float OrderCount = 0, TotalCostCount = 0;
        for (int r = 0; r < rc; r++)
        {
            OrderCount++;
            if (grdReport.Rows[r].Cells[2].Text.Trim().ToUpper() != "CANCELLED")
            {
                TotalCostCount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text);
                
            }
        }
        grdReport.FooterRow.Cells[0].Text = "Total Item";
        grdReport.FooterRow.Cells[1].Text = TotalCostCount.ToString();
        
    }

    protected void grdReport_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CMDShowDetails")
        {
            string strItemName = e.CommandArgument.ToString();
            ShowReport("1", strItemName, grdStockDetails);
            grdStockDetails.Caption = strItemName + " Details";
        }
    }

    private void CalculateGridReport()
    {
        int rc = grdReport.Rows.Count;
        int cc = grdReport.Columns.Count;
        float TotalNetPaidCount = 0;
        for (int r = 0; r < rc; r++)
        {
            TotalNetPaidCount += float.Parse("0" + grdReport.Rows[r].Cells[1].Text.Replace("&nbsp;", ""));
        }
        grdReport.FooterRow.Cells[1].Text = TotalNetPaidCount.ToString();  
    }

       
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "strReportFile.xls";
        Response.Expires = 0;
        Response.Buffer = true;

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
