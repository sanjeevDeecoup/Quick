using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;


namespace QuickWeb.TallyIntegration
{
    public partial class TallyBooking : System.Web.UI.Page
    {


        StreamWriter sw,sw1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                drpMonthList.SelectedIndex = DateTime.Today.Month - 1;
                drpYearList.Items.Clear();
                for (int i = 2000; i <= 2050; i++)
                {
                    drpYearList.Items.Add(i.ToString());
                }
                drpYearList.SelectedIndex = DateTime.Today.Year - 2000;
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {

            //Select Date 
            ViewState["exlquery"] = null;
            string strSqlQuery = string.Empty;
            bool status = false;
            string strFromDate = string.Empty, strToDate = string.Empty;
            if (radReportFrom.Checked)
            {
                //strFromDate = txtReportFrom.Text + " 00:00:00";
                if (txtReportUpto.Text == "") { txtReportUpto.Text = txtReportFrom.Text; }
                DateTime dt = DateTime.Parse(txtReportUpto.Text);
                DateTime dt3 = dt.AddDays(1);
                //strToDate = dt3.ToShortDateString() + " 00:00:00";
                DateTime dt1 = DateTime.Parse(txtReportFrom.Text);
                DateTime dt2 = DateTime.Parse(txtReportUpto.Text);
                strFromDate = txtReportFrom.Text;
                strToDate = txtReportUpto.Text;

                status = true;
            }
            else if (radReportMonthly.Checked)
            {
                DateTime dt = new DateTime(int.Parse(drpYearList.SelectedItem.Text), int.Parse(drpMonthList.SelectedItem.Value), 1);
                strFromDate = dt.ToShortDateString() + " 00:00:00";
                strToDate = dt.AddMonths(1).AddDays(-1).ToShortDateString() + " 00:00:00";

            }

            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "TallyBooking";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookDate1", strFromDate);
            cmd.Parameters.AddWithValue("@BookDate2", strToDate);
            cmd.Parameters.AddWithValue("@BranchId", Globals.BranchID);
            ds = AppClass.GetData(cmd);
            string strXMLfile = "";
            string strXMLMaster = "";
            string Narration = "On Credit New Booking Number ";
            strXMLfile = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";
            strXMLMaster = "<ENVELOPE><HEADER><VERSION>1</VERSION><TALLYREQUEST>Import</TALLYREQUEST><TYPE>Data</TYPE><ID>All Masters</ID></HEADER>";
            strXMLMaster += "<BODY><DESC><STATICVARIABLES><IMPORTDUPS>@@DUPCOMBINE</IMPORTDUPS></STATICVARIABLES></DESC> <DATA><TALLYMESSAGE>";
            for(int i=0;i<=ds.Tables[0].Rows.Count-1;i++)
            {
               
               strXMLMaster+="<Ledger Name="+ "\""+ ds.Tables[0].Rows[i]["CustCode"].ToString() + "\" "+ "Action = \"Create\"><NAME>Vivek Saini</NAME><PARENT>Sundry Debtors</PARENT><OPENINGBALANCE>0</OPENINGBALANCE></LEDGER>";
				
			

                // Automatic Import in Tally
                                                                                 
                    strXMLfile += "<TALLYMESSAGE xmlns:UDF=\"TallyUDF\"> <VOUCHER VCHTYPE=\"Sales\" ACTION=\"Create\"><DATE>" +ds.Tables[0].Rows[i]["BookingDate"].ToString() + "</DATE><NARRATION>"+ Narration+ds.Tables[0].Rows[i]["BookingNumber"].ToString() +"</NARRATION>";
                    strXMLfile += "<VOUCHERTYPENAME>Sales</VOUCHERTYPENAME><PARTYLEDGERNAME>" + ds.Tables[0].Rows[i]["CustCode"].ToString() + "</PARTYLEDGERNAME><PARTYNAME>" + ds.Tables[0].Rows[i]["CustCode"].ToString() + "</PARTYNAME><BASEPARTYNAME>" + ds.Tables[0].Rows[i]["CustCode"].ToString() + "</BASEPARTYNAME><CSTFORMISSUETYPE/><CSTFORMRECVTYPE/>";
                    strXMLfile += "<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE><BASICBUYERNAME>"+ds.Tables[0].Rows[i]["CustCode"].ToString() +"</BASICBUYERNAME>";
                    strXMLfile += "<DIFFACTUALQTY>No</DIFFACTUALQTY><AUDITED>No</AUDITED><FORJOBCOSTING>No</FORJOBCOSTING><ISOPTIONAL>No</ISOPTIONAL><EFFECTIVEDATE>"+ds.Tables[0].Rows[i]["BookingDate"].ToString()+"</EFFECTIVEDATE><USEFORINTEREST>No</USEFORINTEREST>";
                    strXMLfile += "<USEFORGAINLOSS>No</USEFORGAINLOSS><USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER><USEFORCOMPOUND>No</USEFORCOMPOUND><EXCISEOPENING>No</EXCISEOPENING><ISCANCELLED>No</ISCANCELLED><HASCASHFLOW>No</HASCASHFLOW>";
                    strXMLfile += "<ISPOSTDATED>No</ISPOSTDATED><USETRACKINGNUMBER>No</USETRACKINGNUMBER><ISINVOICE>Yes</ISINVOICE><MFGJOURNAL>No</MFGJOURNAL><HASDISCOUNTS>No</HASDISCOUNTS><ASPAYSLIP>No</ASPAYSLIP><ISDELETED>No</ISDELETED>";
                    strXMLfile += "<ASORIGINAL>No</ASORIGINAL><ALLLEDGERENTRIES.LIST><LEDGERNAME>"+ ds.Tables[0].Rows[i]["CustCode"].ToString() +"</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM>";
                    strXMLfile += "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER><AMOUNT>" + -Convert.ToDecimal(ds.Tables[0].Rows[i]["NetAmount"].ToString()) + "</AMOUNT></ALLLEDGERENTRIES.LIST><ALLLEDGERENTRIES.LIST><LEDGERNAME>Sales</LEDGERNAME>";
                    strXMLfile += "<GSTCLASS/><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM><REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>No</ISPARTYLEDGER><AMOUNT>" + Convert.ToDecimal(ds.Tables[0].Rows[i]["NetAmount"].ToString()) + "</AMOUNT>";
                    strXMLfile += "</ALLLEDGERENTRIES.LIST></VOUCHER></TALLYMESSAGE>";
               
                   
            }
            strXMLfile += "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";
            strXMLMaster += "</TALLYMESSAGE></DATA></BODY></ENVELOPE>";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = strXMLfile.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                sw = new StreamWriter(httpWebRequest.GetRequestStream());
                sw1 = new StreamWriter(httpWebRequest.GetRequestStream());

                sw.Write(strXMLfile);
                sw1.Write(strXMLMaster);
                Response.Write("Data inserted into Tally sucessfully");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                //Response.Write(ex.Message, ex.StackTrace);
            }
            finally
            {

                sw.Close();

            }


        }


    }
}