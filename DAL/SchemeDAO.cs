using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SchemeDAO
    {
        //public DataSet GetAllTemplatesData()
        //{
        //    //Get all records from SchemeTemplates table
        //    //return SqlHelper.GetDS("Select TemplateID, SchemeName, SchemeXML from SchemeTemplates where Active = 1");
        //}

        public int SavePromoData(string promoXml, string promoName, string promoDesc, int schemeTemplateID, int promoID)
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("usp_PromoSchemes");
            FlagType flag;
            if (promoID > 0)
            {
                flag = FlagType.Update;
            }
            else
            {
                flag = FlagType.Insert;
            }
            cmd = AddParameters_Promos(cmd, flag, promoXml, promoName, promoDesc, schemeTemplateID, promoID);
            return (int)SqlHelper.ExecuteStoredProc(cmd);
        }

        private SqlCommand AddParameters_Promos(SqlCommand cmd, FlagType flag, string promoXml, string promoName, string promoDesc, int schemeTemplateID, int promoID)
        {
            cmd.Parameters.Add(new SqlParameter("@Flag", flag));
            cmd.Parameters.Add(new SqlParameter("@promoID", promoID));
            cmd.Parameters.Add(new SqlParameter("@SchemeTemplateID", schemeTemplateID));
            cmd.Parameters.Add(new SqlParameter("@PromoName", promoName));
            cmd.Parameters.Add(new SqlParameter("@PromoDesc", promoDesc));
            cmd.Parameters.Add(new SqlParameter("@PromoXML", promoXml));
            return cmd;
        }

        public DataSet GetAllPromos()
        {
            SqlCommand cmd = SqlHelper.CreateSqlCommand("usp_PromoSchemes");
            cmd = AddParameters_Promos(cmd, FlagType.Select, "", "", "", 0, 0);
            return (DataSet)SqlHelper.ExecuteStoredProc(cmd);
        }
    }
}