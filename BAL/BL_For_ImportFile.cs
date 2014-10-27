namespace BAL
{
    public class BL_For_ImportFile
    {
        public string ReadItemTableFile(string FileName)
        {
            return DAL.DALFactory.Instance.DAL_For_ImportFile.ReadItemTableFile(FileName);
        }

        public string ReadSubItemDetailsExcelFile(string FileName)
        {
            return DAL.DALFactory.Instance.DAL_For_ImportFile.ReadSubItemDetailsExcelFile(FileName);
        }

        public string ReadMenuRightsExcelFile(string FileName)
        {
            return DAL.DALFactory.Instance.DAL_For_ImportFile.ReadMenuRightsExcelFile(FileName);
        }
    }
}