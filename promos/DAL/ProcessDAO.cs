using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAL
{
    public class ProcessDAO
    {
        public DataSet GetAllProcesses()
        {
            return SqlHelper.GetDS("Select ProcessID, ProcessImage, ProcessName, ProcessCode from ProcessMaster where Active = 1 order by ProcessID ");
        }
    }
}
