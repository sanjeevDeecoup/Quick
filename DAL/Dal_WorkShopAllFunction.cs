using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL
{
    public class Dal_WorkShopAllFunction
    {
        public DataSet BindShopName()
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "sp_ChallanInProc";
            CMD.Parameters.AddWithValue("@Flag", 21);
            ds = PrjClass.GetData(CMD);
            return ds;
        }

        public DataSet BindAllChallanBranchWise(string BID, string Date1, string Date2)
        {
            SqlCommand CMD = new SqlCommand();
            DataSet ds = new DataSet();
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.CommandText = "Sp_Report_ChallanReport";
            CMD.Parameters.AddWithValue("@ChallanDate1", Date1);
            CMD.Parameters.AddWithValue("@ChallanDate2", Date2);
            CMD.Parameters.AddWithValue("@BranchId", BID);
            CMD.Parameters.AddWithValue("@Flag", 8);
            ds = PrjClass.GetData(CMD);
            return ds;
        }
        public bool FindAndKillProcess(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes by using the StartsWith Method,
                //this prevents us from incluing the .EXE for the process we're looking for.
                //. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    //since we found the proccess we now need to use the
                    //Kill Method to kill the process. Remember, if you have
                    //the process running more than once, say IE open 4
                    //times the loop thr way it is now will close all 4,
                    //if you want it to just close the first one it finds
                    //then add a return; after the Kill
                    clsProcess.Kill();
                    //process killed, return true
                    return true;
                }
            }
            //process not found, return false
            return false;
        }
    }
}