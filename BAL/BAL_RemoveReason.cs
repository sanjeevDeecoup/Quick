using System.Data;

namespace BAL
{
    public class BAL_RemoveReason
    {
        public string SaveReason(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.SaveReason(Ob);
        }

        public string UpdateReason(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.UpdateReason(Ob);
        }

        public DataSet ShowAllReason(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.ShowAllReason(Ob);
        }

        public DataSet SearchReason(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.SearchReason(Ob);
        }

        public string DeleteReason(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.DeleteReason(Ob);
        }

        public string CheckBlanckEntries(string Labelname, string rate, string DayOffset)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.CheckBlanckEntries(Labelname, rate, DayOffset);
        }

        public bool CheckCorrectRemoveReason(string BID, string Text)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.CheckCorrectRemoveReason(BID, Text);
        }

        public string DeleteReasonMain(DTO.Common Ob)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.DeleteReasonMain(Ob);
        }

        public bool CheckAcceptPaymentButtonAcess(string BID, string UserTypeId)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.CheckAcceptPaymentButtonAcess(BID, UserTypeId);
        }

        public string SaveTempIntoPaymentTable(string BookingNo, string DateTime, string Time, string BranchId)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.SaveTempIntoPaymentTable(BookingNo, DateTime, Time, BranchId);
        }

        public string GetDateTime(string BID)
        {
            return DAL.DALFactory.Instance.DAL_RemoveReason.GetDateTime(BID);
        }
    }
}