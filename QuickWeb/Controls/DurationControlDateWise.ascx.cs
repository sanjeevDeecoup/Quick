using System;
using System.Collections;
using System.Web.UI;

namespace QuickWeb.Controls
{
    public partial class DurationControlDateWise : System.Web.UI.UserControl
    {
        private ArrayList date = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            date = DAL.DALFactory.Instance.DAL_DateAndTime.getDateAndTimeAccordingToZoneTime(Globals.BranchID);
            txtReportFrom.Text = date[0].ToString();
            txtReportUpto.Text = date[0].ToString();
            drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
            drpYearList.Items.Clear();
            for (int i = 2010; i <= 2050; i++)
            {
                drpYearList.Items.Add(i.ToString());
            }
            drpYearList.SelectedIndex = DateTime.Today.Year - 2010;
        }
    }
}