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

public partial class GetChallanByNumbers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtStartChallanNumber.Focus();
        }
    }

    protected void btnShowChallan_Click(object sender, EventArgs e)
    {
        btnPrint.Visible = false;
        btnExport.Visible = false;
        string sql = "SELECT DISTINCT EntChallan.ChallanNumber, EntChallan.BookingNumber, EntChallan.Urgent, EntBookingDetails.ItemName, CASE WHEN ItemProcessType = 'None' THEN '' ELSE ItemProcessType END AS ItemProcessType, CASE WHEN ItemExtraProcessType1 = 'None' THEN '' WHEN ItemProcessType = 'None' THEN 'O' + ItemExtraProcessType1 ELSE ItemExtraProcessType1 END AS ItemExtraProcessType1, EntBookingDetails.ItemTotalQuantity * ItemMaster.NumberOfSubItems AS TotalQtySent FROM EntChallan INNER JOIN EntBookingDetails ON EntChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntChallan.ItemSNo = EntBookingDetails.ISN INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName INNER JOIN EntBookings ON EntChallan.BookingNumber = EntBookings.BookingNumber";
        sql += " Where (ChallanNumber Between '" + txtStartChallanNumber.Text + "' AND '" + txtEndChallanNumber.Text + "') And EntBookings.BookingStatus<>5";
        if (drpProcessNames.SelectedIndex > 0)
        {
            sql += " And (ItemProcessType='" + drpProcessNames.SelectedItem.Value + "' OR ItemExtraProcessType1='" + drpProcessNames.SelectedItem.Value + "')";
        }
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand(sql, sqlcon);
        SqlDataAdapter sadp = new SqlDataAdapter(cmd);
        DataSet dsMain = new DataSet();
        try
        {
            sqlcon.Open();
            sadp.Fill(dsMain);
            grdChallan.DataSource = dsMain;
            grdChallan.DataBind();
            grdChallan.Visible = true;

        }
        catch (Exception excp)
        {
            lblMsg.Text = "Error : " + excp.Message;
        }
        finally
        {
            sqlcon.Close();
        }

        if (grdChallan.Rows.Count > 0)
        {
            CalculateGrid();
            ShowProcessCount();
            btnPrint.Visible = true;
            btnExport.Visible = true;
        }

    }

    private void ShowProcessCount()
    {
        string targetString = "";
        string NewVal = "";
        int count = 0;
        ArrayList arr = new ArrayList();
        for (int r = 0; r < grdChallan.Rows.Count; r++)
        {
            NewVal = "" + grdChallan.Rows[r].Cells[1].Text;
            int pos = -1;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() == NewVal)
                {
                    pos = i;
                    break;
                }
            }
            if (pos < 0)
            {
                count = 0;
                for (int row = 0; row < grdChallan.Rows.Count; row++)
                {
                    if (grdChallan.Rows[row].Cells[1].Text == NewVal)
                    {
                        count += int.Parse(grdChallan.Rows[row].Cells[6].Text.Replace("&nbsp;",""));
                    }
                }
                targetString += NewVal + "(" + count + "),";
                arr.Add(NewVal);
            }
        }
    }

    private void CalculateGrid()
    {
        float totalorders = 0, totalquantitysent = 0;
        for (int r = 0; r < grdChallan.Rows.Count; r++)
        {
            totalorders++;
            totalquantitysent += float.Parse("0" + grdChallan.Rows[r].Cells[6].Text.Replace("&nbsp;",""));
        }
        grdChallan.FooterRow.Cells[0].Text = "Total"; //totalorders.ToString();
        grdChallan.FooterRow.Cells[6].Text = totalquantitysent.ToString() + " Pcs";
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strRepTitle = grdChallan.Caption;
        string[] Resp = PrjClass.GenerateExcelReportFromGridView(grdChallan, strRepTitle);
        if (Resp[0] == "1")
            Response.Redirect(Resp[1]);
        else
        {
            lblMsg.Text = Resp[1];
            Resp = PrjClass.GenerateCSVReportFromGridView(grdChallan, strRepTitle);
            if (Resp[0] == "1")
                Response.Redirect(Resp[1]);
            else
                lblMsg.Text = " Could not provide Report at the time. Please try after some time." + Resp[1];
        }
    }
}
