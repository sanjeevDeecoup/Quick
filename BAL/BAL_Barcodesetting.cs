using System.Data;
using System.Web.UI.WebControls;

namespace BAL
{
    public class BAL_Barcodesetting
    {
        public void position(DropDownList drp)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.position(drp);
        }

        public void Fontsetting(DropDownList drp)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.Fontsetting(drp);
        }

        public void BarCodeFontSize(DropDownList drp, DropDownList drp1)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.BarCodeFontSize(drp, drp1);
        }

        public void BookingFontSize(DropDownList drp)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.BookingFontSize(drp);
        }

        public void OthersFontSize(DropDownList drp)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.OthersFontSize(drp);
        }

        public void ProcessFontSize(DropDownList drp)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.ProcessFontSize(drp);
        }

        public void PrinterList(DropDownList drp)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.PrinterList(drp);
        }

        public void CheckedRadio(RadioButton rdo, RadioButton rdo1)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.CheckedRadio(rdo, rdo1);
        }

        public void CheckCheckBox(CheckBox chk1, CheckBox chk2, CheckBox chk3, string Bold, string Italic, string Underline)
        {
            DAL.DALFactory.Instance.Dal_BarCodeSetting.CheckCheckBox(chk1, chk2, chk3, Bold, Italic, Underline);
        }

        public DTO.BarCodeSetting OpeningData(DataSet ds1 , string BID)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.OpeningData(ds1, BID);
        }

        public DataSet fetchbarcodeconfig1(DTO.BarCodeSetting Ob)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.fetchbarcodeconfig1(Ob);
        }

        public DTO.BarCodeSetting DemoBarCodeDisplay(DTO.BarCodeSetting Ob)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.DemoBarCodeDisplay(Ob);
        }

        public string PreviewDemo(Label lbl1, DropDownList drp1, DropDownList drp2, DropDownList drp3, DropDownList drp4, DropDownList drp5, DropDownList drp6, DropDownList drp7, DropDownList drp8, DropDownList drp9)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.PreviewDemo(lbl1, drp1, drp2, drp3, drp4, drp5, drp6, drp7, drp8, drp9);
        }

        public string Updatebarcodewidthheight(DTO.BarCodeSetting Ob)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.Updatebarcodewidthheight(Ob);
        }

        public string Updatebarcodeconfig(DTO.BarCodeSetting Ob)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.Updatebarcodeconfig(Ob);
        }

        public DTO.BarCodeSetting BarCodeData(DataSet ds1, string Id, string BookingNo, string BranchId, string ImageUrl)
        {
            return DAL.DALFactory.Instance.Dal_BarCodeSetting.BarCodeData(ds1, Id, BookingNo, BranchId, ImageUrl);
        }

        public string BranchId { get; set; }
    }
}