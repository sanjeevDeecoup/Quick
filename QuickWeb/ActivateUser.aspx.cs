using System;
using System.Data;
using System.Data.SqlClient;

public partial class ActivateUser : System.Web.UI.Page
{
    private SqlCommand cmd = null;
    private DataSet ds = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (CheckRecordExist() == true)
            {
                //XmlDocument xDoc = new XmlDocument();
                //xDoc.Load(HttpContext.Current.Server.MapPath("~/web.config"));
                //XmlElement root = xDoc.DocumentElement;
                //XmlNodeList connList = root.SelectNodes("//connectionStrings/add");
                //XmlElement elem;
                //foreach (XmlNode node in connList)
                //{
                //    elem = (XmlElement)node;

                //    switch (elem.GetAttribute("name"))
                //    {
                //        case "ConnectionString":
                //            elem.SetAttribute("connectionString", "Data Source=" + Environment.MachineName + "\\DeeCoup" + ";Initial Catalog=master;database=DRYSOFT;User ID=sa;Password=Start@#$");
                //            break;

                //    }
                //}
                //string path = Server.MapPath("~/web.config");
                //xDoc.Save(path);
            }
            string matchKey = string.Empty;
            try
            {
                txtKey.Focus();
                // NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                lblMACAddress.Text = PrjClass.getUniqueID("C");
                ds = new DataSet();
                cmd = new SqlCommand();
                cmd.CommandText = "prcTask";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", 3);
                ds = AppClass.GetData(cmd);
                string flag1 = "1";
                int i = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string checkKey = ds.Tables[0].Rows[i][0].ToString();
                        string str1 = EncodeMacAddress(checkKey);
                        string[] key1 = str1.Split('-');
                        matchKey = key1[1];
                        if (lblMACAddress.Text != matchKey)
                        {
                            flag1 = "0";
                        }
                        else
                        {
                            flag1 = "1";
                            break;
                        }
                    }
                    if (flag1 == "1")
                    {
                        string str = ds.Tables[0].Rows[0][0].ToString();
                        string ad = str.Substring(0, 8);
                        string ed = str.Substring(str.Length - 8, 8);
                        str = EncodeMacAddress(str);
                        string[] key = str.Split('-');
                        string ADate = key[0];
                        matchKey = key[1];
                        string EDate = key[2];
                        string Month = ADate.Substring(2, 3);
                        int AMonth = Convert.ToInt32((Month == "JAN" ? "1" : (Month == "FEB" ? "2" : (Month == "MAR" ? "3" : (Month == "APR" ? "4" : (Month == "MAY" ? "5" : (Month == "JUN" ? "6" : (Month == "Jul" ? "7" : (Month == "AUG" ? "8" : (Month == "SEP" ? "9" : (Month == "OCT" ? "10" : (Month == "NOV" ? "11" : (Month == "DEC" ? "12" : "0")))))))))))));
                        int DAY1 = Convert.ToInt32(ADate.Substring(0, 2));
                        int YEAR1 = Convert.ToInt32(ADate.Substring(5, 4));
                        DateTime activationDate = new DateTime(YEAR1, AMonth, DAY1);
                        string MonthA = ADate.Substring(2, 3);
                        int EMonth = Convert.ToInt32((MonthA == "JAN" ? "1" : (MonthA == "FEB" ? "2" : (MonthA == "MAR" ? "3" : (MonthA == "APR" ? "4" : (MonthA == "MAY" ? "5" : (MonthA == "JUN" ? "6" : (MonthA == "Jul" ? "7" : (MonthA == "AUG" ? "8" : (MonthA == "SEP" ? "9" : (MonthA == "OCT" ? "10" : (MonthA == "NOV" ? "11" : (MonthA == "DEC" ? "12" : "0")))))))))))));

                        DateTime expiryDate = new DateTime(Convert.ToInt32(EDate.Substring(5, 4)), EMonth, Convert.ToInt32(EDate.Substring(0, 2)));
                        TimeSpan span = expiryDate - activationDate;

                        if (span.Days == Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()))
                        {
                            lblMsg.Text = "Software Expired.";
                            string KEY_ID = ReadKey(ds.Tables[0].Rows[0][0].ToString());
                            if (KEY_ID == "")
                            {
                                SqlCommand dbCmd = new SqlCommand();
                                dbCmd.CommandText = "prcTodayDate";
                                dbCmd.CommandType = CommandType.StoredProcedure;
                                dbCmd.Parameters.AddWithValue("@ExpKey", ds.Tables[0].Rows[0][0].ToString());
                                dbCmd.Parameters.AddWithValue("@Flag", 3);
                                string res1 = AppClass.ExecuteNonQuery(dbCmd);
                            }
                            //btnSubmit.Enabled = false;
                            return;
                        }
                        else
                        {
                            SqlCommand cmd1 = new SqlCommand();
                            DataSet ds1 = new DataSet();
                            try
                            {
                                cmd1.CommandText = "prcTodayDate";
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@Key1", ds.Tables[0].Rows[0][0].ToString());
                                cmd1.Parameters.AddWithValue("@Flag", 2);
                                ds1 = AppClass.GetData(cmd1);
                            }
                            catch (Exception)
                            {
                            }

                            DateTime date;
                            int flag = 0;

                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                date = Convert.ToDateTime(ds1.Tables[0].Rows[0][0].ToString());

                                if (date.Equals(DateTime.Today))
                                {
                                    flag = 1;
                                }
                                else
                                {
                                    date = DateTime.Today;
                                    flag = 0;
                                }
                            }
                            else
                            {
                                date = DateTime.Today;
                                flag = 0;
                            }
                            if (flag == 0)
                            {
                                string res = string.Empty;
                                SqlCommand cmd2 = new SqlCommand();
                                try
                                {
                                    cmd2.CommandText = "prcTodayDate";
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@TodayDate", date);
                                    cmd2.Parameters.AddWithValue("@Key1", ds.Tables[0].Rows[0][0].ToString());
                                    cmd2.Parameters.AddWithValue("@Flag", 1);
                                    res = AppClass.ExecuteNonQuery(cmd2);
                                }
                                catch (Exception)
                                {
                                }
                                if (res != "Record Saved")
                                {
                                    lblMsg.Text = "Error in Data Saving Process.";
                                }
                                else
                                {
                                    Response.Redirect("~/Login.aspx", false);
                                }
                            }
                            else
                            {
                                Response.Redirect("~/Login.aspx", false);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public void SaveDataInFirstTime()
    {
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Dry_DefaultDataInMasters";
            cmd.CommandType = CommandType.StoredProcedure;
            AppClass.ExecuteNonQuery(cmd);
        }
        catch (Exception)
        { }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string res = string.Empty;
        SqlCommand cmd = new SqlCommand();
        try
        {
            string[] key = txtKey.Text.Split('-');
            string MACAddress = EncodeMacAddress(key[1]);
            string KEY_ID = ReadKey(txtKey.Text);
            if (KEY_ID != "")
            {
                lblMsg.Text = "Software Expired.Please enter new key for activation.";
                return;
            }

            if (lblMACAddress.Text == MACAddress)
            {
                cmd.CommandText = "prcTask";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Key1", txtKey.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@Key2", 0);
                cmd.Parameters.AddWithValue("@Flag", 1);
                res = AppClass.ExecuteNonQuery(cmd);
                if (res == "Record Saved")
                {
                    if (CheckRecordExist() == true)
                    {
                        SaveDataInFirstTime();
                    }
                    Response.Redirect("~/Login.aspx", false);
                }
            }
            else
            {
                lblMsg.Text = "Invalid key, Please enter valid key.";
            }
        }
        catch (Exception)
        {
            lblMsg.Text = "Invalid key, Please enter valid key.";
        }
    }

    public string ReadKey(string key)
    {
        string res = string.Empty;
        try
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;
            cmd.CommandText = "prcTodayDate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ExpKey", key);
            cmd.Parameters.AddWithValue("@Flag", 4);
            sdr = AppClass.ExecuteReader(cmd);
            if (sdr.Read())
            {
                res = "" + sdr.GetValue(1);
            }
            else res = "";
        }
        catch (Exception)
        { res = ""; }
        return res;
    }

    public bool CheckRecordExist()
    {
        bool status = true;
        try
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader sdr = null;

            cmd.CommandText = "sp_Dry_BarcodeMaster";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Flag", 19);
            sdr = AppClass.ExecuteReader(cmd);
            if (sdr.Read())
                status = false;
            else
                status = true;
        }
        catch (Exception)
        { }
        return status;
    }

    public string EncodeMacAddress(string MAC)
    {
        string encode = string.Empty;
        for (int i = 0; i < MAC.Length; i++)
        {
            encode += ReturnCode(MAC[i].ToString());
        }
        return encode;
    }

    public string ReturnCode(string enCodeText)
    {
        string code = string.Empty;
        switch (enCodeText)
        {
            case "A":
                code = "0";
                break;

            case "B":
                code = "1";
                break;

            case "C":
                code = "2";
                break;

            case "D":
                code = "3";
                break;

            case "E":
                code = "4";
                break;

            case "F":
                code = "5";
                break;

            case "G":
                code = "6";
                break;

            case "H":
                code = "7";
                break;

            case "I":
                code = "8";
                break;

            case "J":
                code = "9";
                break;

            case "1":
                code = "A";
                break;

            case "2":
                code = "B";
                break;

            case "3":
                code = "C";
                break;

            case "4":
                code = "D";
                break;

            case "5":
                code = "E";
                break;

            case "6":
                code = "F";
                break;

            case "7":
                code = "G";
                break;

            case "8":
                code = "H";
                break;

            case "9":
                code = "I";
                break;

            case "!":
                code = "J";
                break;

            case "@":
                code = "K";
                break;

            case "#":
                code = "L";
                break;

            case "$":
                code = "M";
                break;

            case "%":
                code = "N";
                break;

            case "^":
                code = "O";
                break;

            case "&":
                code = "P";
                break;

            case "*":
                code = "Q";
                break;

            case "(":
                code = "R";
                break;

            case ")":
                code = "S";
                break;

            case "_":
                code = "T";
                break;

            case "+":
                code = "U";
                break;

            case "=":
                code = "V";
                break;

            case "|":
                code = "W";
                break;

            case ":":
                code = "X";
                break;

            case "'":
                code = "Y";
                break;

            case "~":
                code = "Z";
                break;

            case "-":
                code = "-";
                break;
        }
        return code;
    }
}