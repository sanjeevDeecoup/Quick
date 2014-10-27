using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Barcodeconfig
    {
        public string Updatebarcodeconfig(DTO.Barcodeconfig Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@barcodeconfigbooking", Ob.barcodeconfigbooking);
            cmd.Parameters.AddWithValue("@barcodebookingfont", Ob.barcodebookingfont);
            cmd.Parameters.AddWithValue("@barcodebookingsize", Ob.barcodebookingsize);
            cmd.Parameters.AddWithValue("@barcodebookingalign", Ob.barcodebookingalign);
            cmd.Parameters.AddWithValue("@barcodebookingbold", Ob.barcodebookingbold);
            cmd.Parameters.AddWithValue("@barcodebookingitalic", Ob.barcodebookingitalic);
            cmd.Parameters.AddWithValue("@barcodebookingunderline", Ob.barcodebookingunderline);

            cmd.Parameters.AddWithValue("@barcodeconfigprocess", Ob.barcodeconfigprocess);
            cmd.Parameters.AddWithValue("@barcodeconfigextraprocess", Ob.barcodeconfigprocess);
            cmd.Parameters.AddWithValue("@barcodeconfigextraprocesssecond", Ob.barcodeconfigprocess);
            cmd.Parameters.AddWithValue("@barcodeconfigprocesssubtotal", Ob.barcodeconfigprocesssubtotal);
            cmd.Parameters.AddWithValue("@barcodeprocessfont", Ob.barcodeprocessfont);
            cmd.Parameters.AddWithValue("@barcodeprocesssize", Ob.barcodeprocesssize);
            cmd.Parameters.AddWithValue("@barcodeprocessalign", Ob.barcodeprocessalign);
            cmd.Parameters.AddWithValue("@barcodeprocessbold", Ob.barcodeprocessbold);
            cmd.Parameters.AddWithValue("@barcodeprocessitalic", Ob.barcodeprocessitalic);
            cmd.Parameters.AddWithValue("@barcodeprocessunderline", Ob.barcodeprocessunderline);

            cmd.Parameters.AddWithValue("@barcodeconfigremark", Ob.barcodeconfigremark);
            cmd.Parameters.AddWithValue("@barcodeconfigcolour", Ob.barcodeconfigcolour);
            cmd.Parameters.AddWithValue("@barcoderemarkfont", Ob.barcoderemarkfont);
            cmd.Parameters.AddWithValue("@barcodremarksize", Ob.barcodremarksize);
            cmd.Parameters.AddWithValue("@barcoderemarkalign", Ob.barcoderemarkalign);
            cmd.Parameters.AddWithValue("@barcoderemarkbold", Ob.barcoderemarkbold);
            cmd.Parameters.AddWithValue("@barcoderemarkitalic", Ob.barcoderemarkitalic);
            cmd.Parameters.AddWithValue("@barcoderemarkunderline", Ob.barcoderemarkunderline);

            cmd.Parameters.AddWithValue("@barcodeconfigbarcode", Ob.barcodeconfigbarcode);
            // cmd.Parameters.AddWithValue("@barcodebarcodesize", Ob.barcodebarcodesize);
            cmd.Parameters.AddWithValue("@barcodebarcodealign", Ob.barcodebarcodealign);

            cmd.Parameters.AddWithValue("@barcodeconfigitem", Ob.barcodeconfigitem);
            cmd.Parameters.AddWithValue("@barcodeconfigduedate", Ob.barcodeconfigduedate);
            cmd.Parameters.AddWithValue("@barcodeconfigtime", Ob.barcodeconfigtime);
            cmd.Parameters.AddWithValue("@barcodeitemfont", Ob.barcodeitemfont);
            cmd.Parameters.AddWithValue("@barcodeitemfont1", Ob.barcodeitemfont1);
            cmd.Parameters.AddWithValue("@barcodeitemsize", Ob.barcodeitemsize);
            cmd.Parameters.AddWithValue("@barcodeitemsize1", Ob.barcodeitemsize1);
            cmd.Parameters.AddWithValue("@barcodeitemalign", Ob.barcodeitemalign);
            cmd.Parameters.AddWithValue("@barcodeitemalign1", Ob.barcodeitemalign1);
            cmd.Parameters.AddWithValue("@barcodeitembold", Ob.barcodeitembold);
            cmd.Parameters.AddWithValue("@barcodeitembold1", Ob.barcodeitembold1);
            cmd.Parameters.AddWithValue("@barcodeitemitalic", Ob.barcodeitemitalic);
            cmd.Parameters.AddWithValue("@barcodeitemitalic1", Ob.barcodeitemitalic1);
            cmd.Parameters.AddWithValue("@barcodeitemunderline", Ob.barcodeitemunderline);
            cmd.Parameters.AddWithValue("@barcodeitemunderline1", Ob.barcodeitemunderline1);

            cmd.Parameters.AddWithValue("@barcodeconfigcustomer", Ob.barcodeconfigcustomer);
            cmd.Parameters.AddWithValue("@barcodecustomerfont", Ob.barcodecustomerfont);
            cmd.Parameters.AddWithValue("@barcodecustomerize", Ob.barcodecustomerize);
            cmd.Parameters.AddWithValue("@barcodecustomeralign", Ob.barcodecustomeralign);
            cmd.Parameters.AddWithValue("@barcodecustomerbold", Ob.barcodecustomerbold);
            cmd.Parameters.AddWithValue("@barcodecustomeritalic", Ob.barcodecustomeritalic);
            cmd.Parameters.AddWithValue("@barcodecustomerunderline", Ob.barcodecustomerunderline);

            cmd.Parameters.AddWithValue("@bookingnoposition", Ob.bookingnoposition);
            cmd.Parameters.AddWithValue("@customerposition", Ob.customerposition);
            cmd.Parameters.AddWithValue("@processposition", Ob.processposition);
            cmd.Parameters.AddWithValue("@remarkposition", Ob.remarkposition);
            cmd.Parameters.AddWithValue("@barcodeposition", Ob.barcodeposition);
            cmd.Parameters.AddWithValue("@itemposition", Ob.itemposition);
            cmd.Parameters.AddWithValue("@itemposition1", Ob.itemposition1);
            cmd.Parameters.AddWithValue("@addressposition", Ob.addressposition);

            cmd.Parameters.AddWithValue("@barcodeconfigaddressr", Ob.barcodeconfigaddress);
            cmd.Parameters.AddWithValue("@barcodeaddressfont", Ob.barcodeaddressfont);
            cmd.Parameters.AddWithValue("@barcodeaddresssize", Ob.barcodeaddresssize);
            cmd.Parameters.AddWithValue("@barcodeaddressalign", Ob.barcodeaddressalign);
            cmd.Parameters.AddWithValue("@barcodeaddressbold", Ob.barcodeaddressbold);
            cmd.Parameters.AddWithValue("@barcodeaddressitalic", Ob.barcodeaddressitalic);
            cmd.Parameters.AddWithValue("@barcodeaddressunderline", Ob.barcodeaddressunderline);
            cmd.Parameters.AddWithValue("@barcodedivider", Ob.barcodedivider);

            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 6);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }

        public DataSet fetchbarcodeconfig(DTO.Barcodeconfig Ob)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 7);
            ds = PrjClass.GetData(cmd);
            return ds;
        }

        public string Updatebarcodewidthheight(DTO.Barcodeconfig Ob)
        {
            string res = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReceiptConfigSetting";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@barcodewidth", Ob.barcodewidth);
            cmd.Parameters.AddWithValue("@barcodeheight", Ob.barcodeheight);
            cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            cmd.Parameters.AddWithValue("@Flag", 9);
            res = PrjClass.ExecuteNonQuery(cmd);
            return res;
        }
    }
}