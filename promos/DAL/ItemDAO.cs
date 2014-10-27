using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class ItemDAO
    {
        public DataSet GetAllItems()
        {
            return (DataSet)SqlHelper.ExecuteStoredProc(new SqlCommand("usp_GetAllItemsDetails"));
        }
        public DataSet GetAllCategories()
        {
            return SqlHelper.GetDS("select CategoryID, CategoryName, '' as CategoryCode, CategoryImage from CategoriesMaster where Active=1");
        }

        public DataSet GetAllPatterns()
        {
            return SqlHelper.GetDS("select PatternID, PatternName, '' as PatternCode, PatternImage from PatternsMaster where Active = 1");
        }

        public DataSet GetAllColors()
        {
            return SqlHelper.GetDS("select ColorID, ColorName, '' as ColorCode, ColorImage from ColorsMaster where Active = 1");
        }

        public DataSet GetAllBrands()
        {
            return SqlHelper.GetDS("select BrandID, BrandName from BrandsMaster where Active = 1");
        }

        public DataSet GetAllVariations()
        {
            return SqlHelper.GetDS("select VariationID, VariationName from VariationsMaster where Active = 1");
        }

        public DataSet GetAllComments()
        {
            return SqlHelper.GetDS("select CommentID, CommentName from CommentsMaster where Active = 1");
        }

        public void SaveFeedback(string name, string feedback)
        {
            name = name.Replace("\"", "\"\"");
            feedback = feedback.Replace("\"", "\"\"");
            SqlHelper.ExecuteInsertQuery("insert into feedback(Active, name, comments) values (1, '" + name + "', '" + feedback + "')");
        }
    }
}
