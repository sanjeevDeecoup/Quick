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

public partial class Reports_DeliveryBill : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtReportFrom.Text = DateTime.Today.ToShortDateString();
        txtReportUpto.Text = DateTime.Today.ToShortDateString();
        drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
        drpYearList.Items.Clear();
        for (int i = 2000; i <= 2050; i++)
        {
            drpYearList.Items.Add(i.ToString());
        }
        drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

    }
}
