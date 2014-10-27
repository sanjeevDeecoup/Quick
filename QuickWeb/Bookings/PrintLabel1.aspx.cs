using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace QuickWeb.Bookings
{
    public partial class PrintLabel1 : System.Web.UI.Page
    {
         private int m_currentPageIndex;
        private IList<Stream> m_streams;

        private DTO.BacodeLable BarcodeLable = new DTO.BacodeLable();
        private DTO.Sticker Ob = new DTO.Sticker();

        public DTO.Sticker SetfetchValue()
        {
            Ob.BranchId = Globals.BranchID;
            return Ob;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //----------------------------
            SetfetchValue();
            DataSet dset = new DataSet();
            dset = BAL.BALFactory.Instance.BAL_Sticker.fetchbarcodeconfig(Ob);
            bool barcodebookingno = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodebookingno"].ToString());
            string bookingfont = dset.Tables[0].Rows[0]["bookingfont"].ToString();
            string bookingsize = dset.Tables[0].Rows[0]["bookingsize"].ToString() + "pt";
            string bookingalign = dset.Tables[0].Rows[0]["bookingalign"].ToString();
            string bookingbold = dset.Tables[0].Rows[0]["bookingbold"].ToString();
            string bookingitilic = dset.Tables[0].Rows[0]["bookingitilic"].ToString();
            string bookingunderline = dset.Tables[0].Rows[0]["bookingunderline"].ToString();
            bool barcodeprocess = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeprocess"].ToString());
            bool barcodexteraprocess = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodexteraprocess"].ToString());
            bool barcodeextraprocesssecond = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeextraprocesssecond"].ToString());
            bool barcodesubtotal = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodesubtotal"].ToString());
            string processfont = dset.Tables[0].Rows[0]["processfont"].ToString();
            string processsize = dset.Tables[0].Rows[0]["processsize"].ToString() + "pt";
            string processalign = dset.Tables[0].Rows[0]["processalign"].ToString();
            string processbold = dset.Tables[0].Rows[0]["processbold"].ToString();
            string processitalic = dset.Tables[0].Rows[0]["processitalic"].ToString();
            string processunderline = dset.Tables[0].Rows[0]["processunderline"].ToString();
            bool barcoderemark = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcoderemark"].ToString());
            bool barcodecolour = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodecolour"].ToString());
            string remarkfont = dset.Tables[0].Rows[0]["remarkfont"].ToString();
            string remarksize = dset.Tables[0].Rows[0]["remarksize"].ToString() + "pt";
            string remarkremarkalign = dset.Tables[0].Rows[0]["remarkremarkalign"].ToString();
            string remarkbold = dset.Tables[0].Rows[0]["remarkbold"].ToString();
            string remarkitalic = dset.Tables[0].Rows[0]["remarkitalic"].ToString();
            string remarkunderline = dset.Tables[0].Rows[0]["remarkunderline"].ToString();
            bool barcodeprint = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeprint"].ToString());
            // string barcodesize = dset.Tables[0].Rows[0]["barcodesize"].ToString();
            string barcodealign = dset.Tables[0].Rows[0]["barcodealign"].ToString();
            bool barcodeitem = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeitem"].ToString());
            bool barcodeduedate = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeduedate"].ToString());
            bool barcodetime = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodetime"].ToString());
            string itemfont = dset.Tables[0].Rows[0]["itemfont"].ToString();
            string itemsize = dset.Tables[0].Rows[0]["itemsize"].ToString() + "pt";
            string itembold = dset.Tables[0].Rows[0]["itembold"].ToString();
            string itemitalic = dset.Tables[0].Rows[0]["itemitalic"].ToString();
            string itemalign = dset.Tables[0].Rows[0]["itemalign"].ToString();
            string itemunderline = dset.Tables[0].Rows[0]["itemunderline"].ToString();

            bool barcodecustomer = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodecusname"].ToString());
            string customerfont = dset.Tables[0].Rows[0]["cusfont"].ToString();
            string customersize = dset.Tables[0].Rows[0]["cussize"].ToString() + "pt";
            string customeralign = dset.Tables[0].Rows[0]["cusalign"].ToString();
            string customerbold = dset.Tables[0].Rows[0]["cusbold"].ToString();
            string customeritilic = dset.Tables[0].Rows[0]["cusitalic"].ToString();
            string customerunderline = dset.Tables[0].Rows[0]["cusunderline"].ToString();

            string bookingnoposition = dset.Tables[0].Rows[0]["bookingnoposition"].ToString();
            string customerposition = dset.Tables[0].Rows[0]["cusposition"].ToString();
            string processposition = dset.Tables[0].Rows[0]["processposition"].ToString();
            string remarkposition = dset.Tables[0].Rows[0]["remarkposition"].ToString();
            string barcodeposition = dset.Tables[0].Rows[0]["barcodeposition"].ToString();
            string itemposition = dset.Tables[0].Rows[0]["itemposition"].ToString();

            string addressposition = dset.Tables[0].Rows[0]["Addressposition"].ToString();
            bool barcodeaddress = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodeaddress"].ToString());
            string addressfont = dset.Tables[0].Rows[0]["addfont"].ToString();
            string addresssize = dset.Tables[0].Rows[0]["addsize"].ToString() + "pt";
            string addressalign = dset.Tables[0].Rows[0]["addalign"].ToString();
            string addressbold = dset.Tables[0].Rows[0]["addbold"].ToString();
            string addressitilic = dset.Tables[0].Rows[0]["additalic"].ToString();
            string addressunderline = dset.Tables[0].Rows[0]["addunderline"].ToString();
            bool barcodedivider = Convert.ToBoolean(dset.Tables[0].Rows[0]["barcodedivider"].ToString());

            //-----------------------------

            string res = string.Empty;
            BarcodeLable = new DTO.BacodeLable();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            cmd.CommandText = "Proc_BarCodeLabels";
            cmd.CommandType = CommandType.StoredProcedure;
            ds1 = BAL.BALFactory.Instance.BAL_BarcodeLable.SearchBarcodeLabel(BarcodeLable);
            var redirectURL = Request.QueryString["RedirectBack"] as string;
            if (redirectURL != null)
            {
                if (redirectURL.ToString().IndexOf("Factory") != -1)
                {
                    cmd.Parameters.AddWithValue("@Flag", 15);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Flag", 4);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@Flag", 4);
            }
            cmd.Parameters.AddWithValue("@branchid", Globals.BranchID);
            ds = AppClass.GetData(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ReportViewer1.LocalReport.ReportPath = "RDLC/DynamicBarCodeReport1.rdlc";
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet1";
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    BarcodeLable.BNO = ds1.Tables[0].Rows[i]["BookingNo"].ToString();
                    BarcodeLable.RowIndex = ds1.Tables[0].Rows[i]["RowIndex"].ToString();
                    res = BAL.BALFactory.Instance.BAL_BarcodeLable.UpdateBarcodeLabel(BarcodeLable);
                }
                ReportParameter[] parameters = new ReportParameter[30];
                if (barcodecustomer == true)
                {
                    parameters[0] = new ReportParameter("cusfont", customerfont);
                    parameters[1] = new ReportParameter("cussize", customersize);
                    parameters[3] = new ReportParameter("cusbold", customerbold);
                    parameters[4] = new ReportParameter("cusitalic", customeritilic);
                    parameters[5] = new ReportParameter("cusunderline", customerunderline);
                    parameters[2] = new ReportParameter("cusalign", customeralign);
                }
                else
                {
                    parameters[0] = new ReportParameter("cusfont", bookingfont);
                    parameters[1] = new ReportParameter("cussize", bookingsize);
                    parameters[3] = new ReportParameter("cusbold", bookingbold);
                    parameters[4] = new ReportParameter("cusitalic", bookingitilic);
                    parameters[5] = new ReportParameter("cusunderline", bookingunderline);
                    parameters[2] = new ReportParameter("cusalign", bookingalign);
                }
                parameters[6] = new ReportParameter("bookingfont", bookingfont);
                parameters[7] = new ReportParameter("bookingsize", bookingsize);
                parameters[8] = new ReportParameter("bookingbold", bookingbold);
                parameters[9] = new ReportParameter("bookingitalic", bookingitilic);
                parameters[10] = new ReportParameter("bookingunderling", bookingunderline);
                parameters[11] = new ReportParameter("bookingalign", bookingalign);
                if (barcodeprocess == true)
                {
                    parameters[12] = new ReportParameter("processfont", processfont);
                    parameters[13] = new ReportParameter("processsize", processsize);
                    parameters[14] = new ReportParameter("processbold", processbold);
                    parameters[15] = new ReportParameter("processitalice", processitalic);
                    parameters[16] = new ReportParameter("processunderline", processunderline);
                    parameters[17] = new ReportParameter("proceaaaline", processalign);
                }
                else
                {
                    parameters[12] = new ReportParameter("processfont", bookingfont);
                    parameters[13] = new ReportParameter("processsize", bookingsize);
                    parameters[14] = new ReportParameter("processbold", bookingbold);
                    parameters[15] = new ReportParameter("processitalice", bookingitilic);
                    parameters[16] = new ReportParameter("processunderline", bookingunderline);
                    parameters[17] = new ReportParameter("proceaaaline", bookingalign);
                }
                if (barcodeitem == true && barcodeduedate == true)
                {
                    parameters[18] = new ReportParameter("itemfont", itemfont);
                    parameters[19] = new ReportParameter("itemsize", itemsize);
                    parameters[20] = new ReportParameter("itembold", itembold);
                    parameters[21] = new ReportParameter("itemitalic", itemitalic);
                    parameters[22] = new ReportParameter("itemunderline", itemunderline);
                    parameters[23] = new ReportParameter("itemalign", itemalign);
                }
                else
                {
                    parameters[18] = new ReportParameter("itemfont", remarkfont);
                    parameters[19] = new ReportParameter("itemsize", remarksize);
                    parameters[20] = new ReportParameter("itembold", remarkbold);
                    parameters[21] = new ReportParameter("itemitalic", remarkitalic);
                    parameters[22] = new ReportParameter("itemunderline", remarkunderline);
                    parameters[23] = new ReportParameter("itemalign", remarkremarkalign);
                }
                parameters[24] = new ReportParameter("remarkfont", remarkfont);
                parameters[25] = new ReportParameter("remarksize", remarksize);
                parameters[26] = new ReportParameter("remarkbold", remarkbold);
                parameters[27] = new ReportParameter("remarkitalic", remarkitalic);
                parameters[28] = new ReportParameter("remarkunderline", remarkunderline);
                parameters[29] = new ReportParameter("remarkalign", remarkremarkalign);

                # region unusedCode
                /*
                ReportViewer1.LocalReport.SetParameters(parameters);
                rds.Value = ds.Tables[0];
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.DataBind();
                //ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.Refresh();

                // string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();

                //string reportType = "PDF";
                //string mimeType = string.Empty;
                //string encoding = string.Empty;
                //string fileNameExtension = string.Empty;
                string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>EMF</OutputFormat>" +
            "  <PageWidth>210mm</PageWidth>" +
            "  <PageHeight>297mm</PageHeight>" +
            " <Columns>3</Columns>" +
            " <ColumnSpacing>2.5mm</ColumnSpacing>" +
            "  <MarginTop>13.00mm</MarginTop>" +
            "  <MarginLeft>6.50mm</MarginLeft>" +
            "  <MarginRight>0in</MarginRight>" +
            "  <MarginBottom>0in</MarginBottom>" +

            "</DeviceInfo>";
                Warning[] warnings;
                m_streams = new List<Stream>();
                //string[] streams;
                //byte[] renderedBytes;
                //renderedBytes = ReportViewer1.LocalReport.Render(
                //reportType,
                //deviceInfo,
                //out mimeType,
                //out encoding,
                //out fileNameExtension,
                //out streams,
                //out warnings);
                ReportViewer1.LocalReport.Render("Image", deviceInfo, CreateStream, out warnings);
                foreach (Stream stream in m_streams)
                    stream.Position = 0;
                //Response.Clear();
                //Response.ContentType = mimeType;
                //Response.BinaryWrite(renderedBytes);
                if (Request.QueryString["DirectPrint"] != null && Request.QueryString["DirectPrint"].ToString() == "true")
                {
                    //if (saveCurReport(renderedBytes))
                    //{
                    string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();
                    makePrint();
                    Dispose();
                    var redirectUrl = Request.QueryString["RedirectBack"] as string;
                    Response.Redirect(redirectUrl);
                    //}
                }
                //Response.End();
                 */
                # endregion
                ReportViewer1.LocalReport.SetParameters(parameters);
                rds.Value = ds.Tables[0];
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.DataBind();
                //ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.Refresh();

                // string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();

                string reportType = "PDF";
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string fileNameExtension = string.Empty;
                string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>210mm</PageWidth>" +
            "  <PageHeight>297mm</PageHeight>" +
            " <Columns>3</Columns>" +
            " <ColumnSpacing>2.5mm</ColumnSpacing>" +
            "  <MarginTop>13.00mm</MarginTop>" +
            "  <MarginLeft>6.50mm</MarginLeft>" +
            "  <MarginRight>0in</MarginRight>" +
            "  <MarginBottom>0in</MarginBottom>" +

            "</DeviceInfo>";
                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes;
                renderedBytes = ReportViewer1.LocalReport.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                Response.Clear();
                Response.ContentType = mimeType;
                Response.BinaryWrite(renderedBytes);
                string res1 = BAL.BALFactory.Instance.BAL_BarcodeLable.delbarcodelable();
                Response.End();
            }
        }

        protected bool saveCurReport(byte[] savingArray)
        {
            var stream = new System.IO.FileStream(Server.MapPath("~/SQL") + "//" + "printLabels.pdf", System.IO.FileMode.Create);
            var binaryWriter = new System.IO.BinaryWriter(stream);
            binaryWriter.Write(savingArray);
            binaryWriter.Flush();

            // Create the reader using the stream from the writer.
            BinaryReader binReader =
                new BinaryReader(binaryWriter.BaseStream);

            // Set Position to the beginning of the stream.
            binReader.BaseStream.Position = 0;

            // Read and verify the data.
            byte[] verifyArray = binReader.ReadBytes(savingArray.Length);
            if (verifyArray.Length != savingArray.Length)
            {
                // Console.WriteLine("Error writing the data.");
                return false;
            }
            /* VERIFYING EACH BYTE OF DATA */
            /*
            for (int i = 0; i < savingArray.Length; i++)
            {
                if (verifyArray[i] != renderedBytes[i])
                {
                    Console.WriteLine("Error writing the data.");
                    return;
                }
            }
            */
            stream.Close();
            return true;
        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            string file = Server.MapPath("~/SQL");
            Stream stream = new FileStream(file + name +
               "." + fileNameExtension, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }

        protected void makePrint()
        {
            var setPrinter = BAL.BALFactory.Instance.BAL_New_Bookings.FindDefaultPrinter(Globals.BranchID);

            if (m_streams == null || m_streams.Count == 0)
                return;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = setPrinter;
            //if (!printDoc.PrinterSettings.IsValid)
            //{
            //    string msg = String.Format(
            //       "Can't find printer \"{0}\".", setPrinter);
            //    MessageBox.Show(msg, "Print Error");
            //    return;
            //}
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();

            //Process proc = new Process();

            //string file = Server.MapPath("~/SQL") + "\\" + "invoiceBasedDetailed.pdf";
            //PrinterSettings ps = new PrinterSettings();
            //string printer = ps.PrinterName;

            //Process.Start(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion" + @"\App Paths\AcroRd32.exe").GetValue("").ToString(), string.Format("/h /t \"{0}\" \"{1}\"", file, setPrinter));
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
    
}