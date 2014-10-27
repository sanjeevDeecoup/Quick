using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace QuickWeb.New_Booking
{
    public partial class TemXml : System.Web.UI.Page, ITest
    {
        //const string FILE_NAME = "C:/Users/shilpi/Desktop/quick/QuickWeb/App_Code/2lineitems.xml";
        //XPathDocument doc;
        //XPathNavigator nav;
        //XPathExpression expr;
        //XPathNodeIterator iterator;
        //string oldTitle = "";
        private void LoadXml(string FILE_NAME)
        {
            //if (!File.Exists(FILE_NAME))
            //{
            //    Console.WriteLine("{0} does not exist.", FILE_NAME);
            //    return;
            //}
            //StreamReader sr = File.OpenText(FILE_NAME);
            //String input;

            //input = sr.ReadToEnd();
            //sr.Close();
            //TextBox1.Text = input;
            //XmlDocument doc = new XmlDocument();
            //doc.Load(FILE_NAME);
            //XPathNavigator xnav = doc.CreateNavigator();
            //xnav.Evaluate("count(//@*)");

        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            Rec(0);
        }

        private static void Rec(int i)
        {
            Console.WriteLine(i);
            if (i < int.MaxValue)
            {
                Rec(i + 1);
            }
        }

        private static void Bar<T>() where T : new()
        {
            var t = new T();
            Console.WriteLine(t.ToString());
            Console.WriteLine(t.GetHashCode());
            Console.WriteLine(t.Equals(t));

            Console.WriteLine(t.GetType());
        }

        private static void UnCanny()
        {
            HowIsThisHappening<MyFunnyType>();
        }

        private static void HowIsThisHappening<T>() where T : class, new()
        {
            var instance = new T(); // new() on a ref-type; should be non-null, then
            if (instance != null)
            {
                Console.WriteLine("How did we break the CLR?");
            }
            else
            {
                Console.WriteLine("fine!");
            }
        }

        //private void ReadXMLFileAndFillCombos()
        //{
        //    try
        //    {
        //        doc = new XPathDocument(FILE_NAME);
        //        XmlTextReader objXmlTextReader = new XmlTextReader(FILE_NAME);
        //        string sName = "";
        //        while (objXmlTextReader.Read())
        //        {
        //            switch (objXmlTextReader.NodeType)
        //            {
        //                case XmlNodeType.Element:
        //                    sName = objXmlTextReader.Name;
        //                    break;
        //                case XmlNodeType.Text:
        //                    switch (sName)
        //                    {
        //                        case "bookingnumber":

        //                            drpbooking.Items.Add(objXmlTextReader.Value);

        //                            DrpBookingNumber.Items.Add(objXmlTextReader.Value);

        //                            break;

        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //protected DataTable MakeNewDataTable()
        //{
        //    DataTable booking = new DataTable();
        //    booking.Clear();
        //    booking.Columns.Add("Id");
        //    booking.Columns.Add("BookingNumber");
        //    booking.Columns.Add("IsHomeReceipt");
        //    booking.Columns.Add("HomeReceiptNumber");
        //    booking.Columns.Add("CustomerID");
        //    booking.Columns.Add("DueDate");
        //    booking.Columns.Add("DueTime");
        //    booking.Columns.Add("IsUrgent");
        //    booking.Columns.Add("IsSMS");
        //    booking.Columns.Add("IsEmail");
        //    booking.Columns.Add("ReceiptRemarks");
        //    booking.Columns.Add("SalesManUserId");
        //    booking.Columns.Add("CheckedByUserId");
        //    booking.Columns.Add("Quantity");
        //    booking.Columns.Add("TotalGrossAmount");
        //    booking.Columns.Add("TotalDiscount");
        //    booking.Columns.Add("TotalTax");
        //    booking.Columns.Add("ReceiptStatus");
        //    booking.Columns.Add("Active");
        //    booking.Columns.Add("DateModified");
        //    booking.Columns.Add("CreatedBy");
        //    booking.Columns.Add("ModifiedBy");
        //    return booking;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ReadXMLFileAndFillCombos();

                // DrpBookingNumber_SelectedIndexChanged(null, null);
                //XmlNodeList list = xdoc.SelectNodes("//Booking");
                //DrpItem.Text = list.ToString();
                BindDataList();
                Method(() => "");
            }
            //System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
            //lblPrinter.Text = ps.PrinterName + " : " + ps.IsDefaultPrinter.ToString() + " : " + ps.DefaultPageSettings.ToString() +
            //    Page.ResolveUrl("/New_Booking/temxml.aspx").ToString() + " : " + "/New_Booking/temxml.aspx" +
            //    " : " + Request.Url.ToString() + " : " + Request.RawUrl.ToString() + " : " + Path.GetFileName(Request.Url.LocalPath);
            //System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            //pd.PrinterSettings = ps;
            //ps.PrintFileName = Path.GetFileName(Request.Url.LocalPath);
            //pd.PrinterSettings = ps;
            //pd.Print();
        }

        //protected void DrpBookingNumber_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = MakeNewDataTable();
        //    XmlTextReader objXmlTextReader = new XmlTextReader(FILE_NAME);
        //    drpbooking.Items.Clear();
        //    drpHeader.Items.Clear();
        //    DrpBookingNumber.Items.Clear();
        //    DrpItem.Items.Clear();
        //    drpColor.Items.Clear();
        //    string Sdetails = "";
        //    while (objXmlTextReader.Read())
        //    {
        //        DataRow NewRow = dt.NewRow();
        //        switch (objXmlTextReader.NodeType)
        //        {
        //            case XmlNodeType.Element:
        //                Sdetails = objXmlTextReader.Name;
        //                break;
        //            case XmlNodeType.Text:
        //                switch (Sdetails)
        //                {
        //                    case "Booking":
        //                        drpbooking.Items.Add(objXmlTextReader.Value);
        //                        break;
        //                    case "receiptheader":
        //                        drpHeader.Items.Add(objXmlTextReader.Value);
        //                        break;
        //                    case "iswalkin":
        //                        drpHeader.Items.Add(objXmlTextReader.Value);
        //                        break;
        //                    case "duedate":
        //                        DrpItem.Items.Add(objXmlTextReader.Value);
        //                        NewRow["DueDate"] = DrpItem.Text;
        //                        break;
        //                    case "bookingnumber":
        //                        DrpBookingNumber.Items.Add(objXmlTextReader.Value);
        //                        NewRow["bookingNumber"] = DrpBookingNumber.Text;
        //                        break;
        //                    case "duetime":
        //                        drpColor.Items.Add(objXmlTextReader.Value);
        //                        NewRow["DueTime"] = drpColor.Text;
        //                        break;
        //                }
        //                dt.Rows.Add(NewRow);
        //                dt.AcceptChanges();
        //                dt.Columns["Id"].AutoIncrementSeed = dt.Rows.Count + 1;
        //                break;

        //        }
        //        dt.Rows.Add(NewRow);
        //        dt.AcceptChanges();
        //        //dt.Columns["Id"].AutoIncrementSeed = dt.Rows.Count + 1;
        //        break;

        //    }
        //}

        protected void BindDataList()
        {
            //ArrayList listItems = new ArrayList();
            //listItems = BAL.BALFactory.Instance.Bal_Processmaster.BindDataList("");

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Remarks";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", "1");
            cmd.Parameters.AddWithValue("@Flag", "3");
            ds = PrjClass.GetData(cmd);
            gvUserInfo.DataSource = ds;
            gvUserInfo.DataBind();
        }

        public void Method(Func<string> func)
        {
            var sb1 = new StringBuilder("hey");
            var sb2 = new StringBuilder("hey");
            if (sb1 == sb2)
            {
            }
            else
            {
                if (sb1.Equals(sb2))
                {
                }
            }

            var s1 = "hi";
            var s2 = "hi";
            if (s1 == s2)
            {
            }
            if (s1.Equals(s2))
            {
            }

            var me = new Teaser();
            me.Foo();
            Bar<StringBuilder>();
            UnCanny();
        }

        protected void doMe(object sender, EventArgs e)
        {
            /*
            var me = new System.Xml.XmlDocument();
            var settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            var writer = XmlWriter.Create("test.xml", settings);
            var sqlCmdParent = new SqlCommand();
            sqlCmdParent.CommandText = "SELECT * FROM EntMenuRights WHERE BranchId = " + Globals.BranchID +
                " AND UserTypeId = 1 AND ParentMenu = 'NONE' ORDER BY MenuPositio";
            var sqlCmdChild = new SqlCommand();
            sqlCmdChild.CommandText = "SELECT * FROM EntMenuRights WHERE BranchId = " + Globals.BranchID +
                " AND UserTypeId = 1 AND ParentMenu = @ParentMenu ORDER BY MenuPositio";
            var rdrParent = PrjClass.ExecuteReader(sqlCmdParent);
            var rdrChild = PrjClass.ExecuteReader(sqlCmdChild);
            writer.WriteStartDocument(true);
            while (rdrParent.Read())
            {
                writer.WriteStartElement("siteMap");
                //writer
            }
            */
            // setFromDtToSQL();
            BulkInsertInAuxilllaryTable();
        }

        protected void setFromDtToSQL()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var connection = new SqlConnection("Data Source=192.168.1.101;Initial Catalog=DRYSOFT1; User Id=sa; pwd=start;");
            connection.Open();

            // repeat for each table in data set
            var adapterForTable1 = new SqlDataAdapter("select * from EntPackageConsume", connection);
            var builderForTable1 = new SqlCommandBuilder(adapterForTable1);
            var dt = new DataTable();
            adapterForTable1.Fill(dt);
            for (var i = 0; i < 1000; i++)
            {
                var dr = dt.NewRow();
                dr["TID"] = 1;
                dr["RID"] = 23;
                dr["AssignId"] = 4;
                dr["ItemName"] = "SHIRT";
                dr["ConsumeQty"] = 2;
                dr["ConsumeDate"] = DateTime.Now.ToString("dd MMM yyyy");
                dr["BookingNumber"] = 454;
                dr["BalQty"] = 3;
                dr["BranchId"] = 1;
                dt.Rows.Add(dr);
            }
            adapterForTable1.Update(dt);
            watch.Stop();
            Label1.Text = Label1.Text + watch.ElapsedMilliseconds + " <=> ";

            watch.Restart();
            dt = new DataTable();
            dt.Columns.Add("TID");
            dt.Columns.Add("RID");
            dt.Columns.Add("AssignId");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ConsumeQty");
            dt.Columns.Add("ConsumeDate");
            dt.Columns.Add("BookingNumber");
            dt.Columns.Add("BalQty");
            dt.Columns.Add("BranchId");
            for (var i = 0; i < 1000; i++)
            {
                var dr = dt.NewRow();
                dr["TID"] = 1;
                dr["RID"] = 23;
                dr["AssignId"] = 4;
                dr["ItemName"] = "SHIRT";
                dr["ConsumeQty"] = 2;
                dr["ConsumeDate"] = DateTime.Now.ToString("dd MMM yyyy");
                dr["BookingNumber"] = 454;
                dr["BalQty"] = 3;
                dr["BranchId"] = 1;
                dt.Rows.Add(dr);
            }
            using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                bulkCopy.DestinationTableName = "EntPackageConsume";
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulkCopy.WriteToServer(dt);
            }
            watch.Stop();
            Label1.Text = Label1.Text + watch.ElapsedMilliseconds + " <=> ";
        }

        protected void BulkInsertInAuxilllaryTable()
        {
            # region sql

            var connection = new SqlConnection("Data Source=192.168.1.101;Initial Catalog=DRYSOFT1; User Id=sa; pwd=start;");
            connection.Open();
            var adapterForTable1 = new SqlDataAdapter("select * from T1", connection);
            var builderForTable1 = new SqlCommandBuilder(adapterForTable1);
            var dt = new DataTable();
            adapterForTable1.Fill(dt);

            # endregion

            // specify the staring value
            var first = 10;
            // the end
            var next = 20;
            // the no of blocks, and no to start from
            var noOfBlocks = 0;
            var noToStartFrom = 0;

            // the rand no generator
            var rand = new Random();

            // the no of continuous blocks to insert
            noOfBlocks = rand.Next(0, next - first);
            noToStartFrom = rand.Next(first, next);

            dt = new DataTable();
            dt.Columns.Add("col1");

            var stringBuilder = new StringBuilder();

            // for 1 to 1 max

            for (var i = 0; i < 10000; i++)
            {
                if (next >= Int32.MaxValue / 1000)
                    break;

                /*
                File.AppendAllText(@"D:\SQL 2012\StringSQLCount.log",
                                   Environment.NewLine + Environment.NewLine + "BEFORE :: i => " + i + ", first => " + first + ", next " + next +
                                   ", noOfBlocks => " + noOfBlocks + ", noTostartfrom => " + noToStartFrom + " <=> " + stringBuilder);
                */
                //stringBuilder.Append(Environment.NewLine + i + " => start : " + first + ", end : " + next + " <--->");
                for (var j = 0; j < noOfBlocks; j++)
                {
                    //  stringBuilder.Append(((noToStartFrom++ <= next) ? (noToStartFrom-1) : (noToStartFrom -1 - next + first - 1)) + ",");
                    //stringBuilder.Append("(" + ((++noToStartFrom <= next) ? (noToStartFrom - 1) : (noToStartFrom - 1 - next + first)) + "),");
                    var dr = dt.NewRow();
                    dr["Col1"] = ((++noToStartFrom <= next) ? (noToStartFrom - 1) : (noToStartFrom - 1 - next + first));
                    dt.Rows.Add(dr);
                }

                first = next;
                next = rand.Next(first + 1, next + rand.Next(1, first));
                noOfBlocks = ((next - first - 1) == 0) ? 0 : rand.Next(1, (next - first - 1));
                noToStartFrom = rand.Next(first, next);

                /*
                File.AppendAllText(@"D:\SQL 2012\StringSQLCount.log",
                                   Environment.NewLine + " i => " + i + ", first => " + first + ", next " + next +
                                   ", noOfBlocks => " + noOfBlocks + ", noTostartfrom => " + noToStartFrom + " <=> " + stringBuilder);
                //*
                // commit after 100
                /*
                if (i%10 != 0) continue;
                //File.AppendAllText(@"D:\SQL 2012\StringSQL.log", Environment.NewLine+ sqlCommand.CommandText + stringBuilder.Remove(stringBuilder.Length-1, 1));
                // as sql won't allow update to more then 1000 insert, do it here
                if (stringBuilder.Length == 0) continue;
                if (stringBuilder[stringBuilder.Length - 1] == ',')
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                for (var k = 0; k < stringBuilder.ToString().Split(',').Length % 1000; k++)
                {
                    if (stringBuilder.ToString().Split(',').Count() < k*1000)
                        break;

                    var last = stringBuilder.ToString().Split(',').Skip(k*1000).Take(1000).LastOrDefault();
                    var fst = stringBuilder.ToString().Split(',').Skip(k*1000).Take(1000).FirstOrDefault();
                    if (last != null && fst != null)
                    {
                        var indexOf = stringBuilder.ToString().IndexOf(last, StringComparison.Ordinal);
                        var fstIdx = stringBuilder.ToString().IndexOf(fst, StringComparison.Ordinal);
                    /*    sqlCommand.CommandText = commandText +
                                                 stringBuilder.ToString().Substring(fstIdx,
                                                                                    indexOf - fstIdx + last.Length);*/
            }
            /*
            if (sqlCommand.CommandText[sqlCommand.CommandText.Length - 1] == ',')
                sqlCommand.CommandText = sqlCommand.CommandText.Substring(0, sqlCommand.CommandText.Length - 1);
            if (sqlCommand.ExecuteNonQuery() == 0) throw new Exception();
             *
    */

            //sqlCommand.CommandText = sqlCommand.CommandText.Substring(0, sqlCommand.CommandText.Length - 1);
            //sqlCommand.CommandText = commandText + stringBuilder;

            //stringBuilder.Length = 0;
            GC.Collect();
            using (SqlBulkCopy bulkCopy =
                              new SqlBulkCopy(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                bulkCopy.DestinationTableName = "T1";
                bulkCopy.BatchSize = 1000;
                bulkCopy.BulkCopyTimeout = 120;
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulkCopy.WriteToServer(dt);
            }
        }
    }

    internal interface ITest
    {
    }

    public struct Teaser
    {
        public void Foo()
        {
            this = new Teaser();
        }
    }

    public class MyFunnyProxyAttribute : ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            return null;
        }
    }

    [MyFunnyProxy]
    public class MyFunnyType : ContextBoundObject
    {
    }

}