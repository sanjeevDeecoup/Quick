using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace QuickWeb.Masters
{
    public partial class frmpricelist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loadDynamicGridWithTemplateColumn();
        }

        private DTO.NewPriceLists Ob = new DTO.NewPriceLists();

        private void loadDynamicGridWithTemplateColumn()
        {
            #region Code for preparing the DataTable

            //Create an instance of DataTable
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_itempricelist";
            cmd.Parameters.AddWithValue("@BranchId", "1");

            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@BranchId", Ob.BranchId);
            ds = AppClass.GetData(cmd);
            dt = ds.Tables[0];

            #endregion Code for preparing the DataTable

            //Iterate through the columns of the datatable to set the data bound field dynamically.

            foreach (DataColumn col in dt.Columns)
            {
                //Declare the bound field and allocate memory for the bound field.
                TemplateField bfield = new TemplateField();

                //Initalize the DataField value.

                if (col.ColumnName == "Categories" || col.ColumnName == "Items" || col.ColumnName == "SubItems" || col.ColumnName == "Variations")
                {
                    bfield.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                    bfield.ItemTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                }
                else
                {
                    bfield.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);

                    //Initialize the HeaderText field value.
                    bfield.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                }
                //Add the newly created bound field to the GridView.

                GrdDynamic.Columns.Add(bfield);
            }

            //Initialize the DataSource
            GrdDynamic.DataSource = dt;

            //Bind the datatable with the GridView.
            GrdDynamic.DataBind();
            GrdDynamic.Columns[4].Visible = false;
            GrdDynamic.Columns[5].Visible = false;
            GrdDynamic.Columns[6].Visible = false;
            GrdDynamic.Columns[7].Visible = false;
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            //    DataSet ds = new DataSet();

            //    ds = BAL.BALFactory.Instance.BAL_NewPriceLists.fetchpricelist(Ob);
            //     string res = string.Empty;

            //     for (int i = 0; i < GrdDynamic.Rows.Count; i++)
            //     {
            //         for(int j=0;j<GrdDynamic.Columns.Count;j++)
            //         {
            //         Ob.SubItemRefID = ds.Tables[0].Rows[i]["SubItemRefID"].ToString();
            //         Ob.CategoryID = ds.Tables[0].Rows[i]["CategoryID"].ToString();
            //         Ob.BranchId = "1";
            //         Ob.ItemCode = ds.Tables[0].Rows[i]["ItemID"].ToString();
            //         Ob.VariationId = ds.Tables[0].Rows[i]["VariationID"].ToString();
            //         string pro = ds.Tables[0].Rows[j + 8]["name"].ToString();
            //         string[] strArr1 = pro.Split('_');
            //         Ob.Processid = strArr1[1];
            //         //Ob.Price = ar1[l].ToString();
            //         Ob.DateCreated = "06/06/2012";
            //         Ob.DateModified = "06/06/2012";
            //         Ob.Active = "1";
            //         //res = BAL.BALFactory.Instance.BAL_NewPriceLists.SaveNewItemprice(Ob);
            //         }

            //     }
        }
    }
}