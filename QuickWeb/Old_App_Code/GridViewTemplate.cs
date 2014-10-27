using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


//A customized class for displaying the Template Column
public class GridViewTemplate : ITemplate
{
    //A variable to hold the type of ListItemType.
    ListItemType _templateType;

    //A variable to hold the column name.
    string _columnName;

    //Constructor where we define the template type and column name.
    public GridViewTemplate(ListItemType type, string colname)
    {
        //Stores the template type.
        _templateType = type;

        //Stores the column name.
        _columnName = colname;
    }
    int i = 1;
    DTO.NewPriceLists Ob = new DTO.NewPriceLists();

    public DTO.NewPriceLists SetfetchValue()
    {
        Ob.BranchId = "1";
        //Globals.BranchID;
        return Ob;
    }
    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    {
       
            //SetfetchValue();
            //DataSet ds3 = new DataSet();
            //ds3 = BAL.BALFactory.Instance.BAL_Newpricelists.fetchpricelistcoloumvaluecount(Ob);
            //string pro = ds3.Tables[0].Rows[i]["name"].ToString();
            //string[] strArr1 = pro.Split('_');

            switch (_templateType)
            {
                case ListItemType.Header:
                    //Creates a new label control and add it to the container.
                    Label lbl = new Label();            //Allocates the new label object.
                    //lbl.Text = _columnName;             //Assigns the name of the column in the lable.               
                    lbl.DataBinding += new EventHandler(lb1_DataBinding);

                    container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                    break;

                case ListItemType.Item:
                    //Creates a new text box control and add it to the container.
                  
                        TextBox tb1 = new TextBox();                            //Allocates the new text box object.
                        tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                        tb1.Columns = 4;                                        //Creates a column with size 4.
                        tb1.Attributes.Add("onkeypress", "return isNumberKey(event);");
                       // tb1.ID = "tb" + (i) + (_columnName);

                        //  i = i + 1;
                        if ("Item" == _columnName || "ItemID" == _columnName || "SubItemRefID" == _columnName || "Subitem" == _columnName || "VariationID" == _columnName || "Variation" == _columnName || "Category" == _columnName || "CategoryID" == _columnName)
                        {

                        }
                        else
                        {
                            container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                        }

                        Label lab = new Label();                            //Allocates the new text box object.
                        lab.DataBinding += new EventHandler(lab_DataBinding);   //Attaches the data binding event.
                        // lab.Columns = 4;                                        //Creates a column with size 4.
                        if ("Item" == _columnName || "ItemID" == _columnName || "SubItemRefID" == _columnName || "Subitem" == _columnName || "VariationID" == _columnName || "Variation" == _columnName || "Category" == _columnName || "CategoryID" == _columnName)
                        {
                            container.Controls.Add(lab);
                        }
                        else
                        {

                        }
               
                    break;

                case ListItemType.EditItem:
                    //As, I am not using any EditItem, I didnot added any code here.
                    break;

                case ListItemType.Footer:
                    CheckBox chkColumn = new CheckBox();
                    chkColumn.ID = "Chk" + _columnName;
                    container.Controls.Add(chkColumn);
                    break;
            }
        
    }

    /// <summary>
    /// This is the event, which will be raised when the binding happens.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    void lab_DataBinding(object sender, EventArgs e)
    {
        Label labe = (Label)sender;
        GridViewRow container = (GridViewRow)labe.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);

        if ("Item" == _columnName || "ItemID" == _columnName || "SubItemRefID" == _columnName || "Subitem" == _columnName || "VariationID" == _columnName || "Variation" == _columnName || "Category" == _columnName || "CategoryID" == _columnName)
        {
            labe.Text = dataValue.ToString();
        }
        else
        {

        }
    }



    void tb1_DataBinding(object sender, EventArgs e)
    {
        TextBox txtdata = (TextBox)sender;
        GridViewRow container = (GridViewRow)txtdata.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);

        if ("Item" == _columnName || "ItemID" == _columnName || "SubItemRefID" == _columnName || "Subitem" == _columnName || "VariationID" == _columnName || "Variation" == _columnName || "Category" == _columnName || "CategoryID" == _columnName)
        {

        }
        else
        {
            if (dataValue != DBNull.Value)
            {
                txtdata.Text = dataValue.ToString();
            }
        }
    }

    void lb1_DataBinding(object sender, EventArgs e)
    {
        Label txtdata = (Label)sender;
        GridViewRow container = (GridViewRow)txtdata.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);
        if (dataValue != null)
        {
            if (dataValue != DBNull.Value)
            {
                txtdata.Text = dataValue.ToString();
            }
        }
        else
        {
            txtdata.Text = _columnName;
        }
    }


    //---------------------------sir---------------------------------------

    //private string _sep = "||";

    ////A variable to hold the type of ListItemType.
    //ListItemType _templateType;
    ////A variable to hold the column name.
    //string _columnName;
    //ControlType _controlType;
    ////Constructor where we define the template type and column name.
    //public GridViewTemplate(ListItemType type, string colname)
    //{
    //    //Stores the template type.
    //    _templateType = type;
    //    //Stores the column name.
    //    _columnName = colname;
    //}
    //public GridViewTemplate(ListItemType type, string colname, ControlType controlType)
    //{
    //    //Stores the template type.
    //    _templateType = type;
    //    //Stores the column name.
    //    _columnName = colname;
    //    _controlType = controlType;
    //}
    //void ITemplate.InstantiateIn(System.Web.UI.Control container)
    //{
    //    switch (_templateType)
    //    {
    //        case ListItemType.Header:
    //            //Creates a new label control and add it to the container.
    //            Label lbl = new Label();            //Allocates the new label object.
    //            if (_columnName.Contains(_sep))
    //            {
    //                lbl.Text = _columnName.Split(_sep.ToCharArray())[0];
    //                lbl.Attributes.Add("DBID", _columnName.Split(_sep.ToCharArray())[2]);
    //            }
    //            else
    //            {
    //                lbl.Text = _columnName;             //Assigns the name of the column in the lable.
    //            }
    //            if (_columnName == "FeeHeads")
    //            {
    //                lbl.Width = Unit.Pixel(200);
    //            }
    //            container.Controls.Add(lbl);        //Adds the newly created label control to the container.
    //            break;
    //        case ListItemType.Item:

    //            //Creates a new text box control and add it to the container.
    //            switch (_controlType)
    //            {
    //                case ControlType.TextBox:
    //                    TextBox tb1 = new TextBox();                            //Allocates the new text box object.
    //                    tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
    //                    tb1.Columns = 7;                                        //Creates a column with size 4.
    //                    tb1.MaxLength = 10;
    //                    container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
    //                    break;
    //                case ControlType.Hidden:
    //                    HiddenField hdn = new HiddenField();
    //                    hdn.DataBinding += new EventHandler(hdn_DataBinding);
    //                    container.Controls.Add(hdn);
    //                    break;
    //                case ControlType.Label:
    //                    Label lbl1 = new Label();
    //                    lbl1.DataBinding += new EventHandler(lbl_DataBinding);
    //                    container.Controls.Add(lbl1);
    //                    break;
    //            }
    //            break;
    //        case ListItemType.EditItem:
    //            //As, I am not using any EditItem, I didnot added any code here.
    //            break;
    //        case ListItemType.Footer:
    //            CheckBox chkColumn = new CheckBox();
    //            chkColumn.ID = "Chk" + _columnName;
    //            container.Controls.Add(chkColumn);
    //            break;
    //    }
    //}
    ///// <summary>
    ///// This is the event, which will be raised when the binding happens.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //void tb1_DataBinding(object sender, EventArgs e)
    //{
    //    TextBox txtdata = (TextBox)sender;
    //    GridViewRow container = (GridViewRow)txtdata.NamingContainer;
    //    object dataValue = DataBinder.Eval(container.DataItem, _columnName);
    //    if (dataValue != DBNull.Value)
    //    {
    //        txtdata.Text = dataValue.ToString();//.Split(_sep.ToCharArray())[0];
    //        //txtdata.Attributes.Add("DBID", dataValue.ToString().Split(_sep.ToCharArray())[2]);
    //        //txtdata.DBID = Convert.ToInt32(dataValue.ToString().Split(_sep.ToCharArray())[2]);
    //    }
    //}

    //void lbl_DataBinding(object sender, EventArgs e)
    //{
    //    Label lblData = (Label)sender;
    //    GridViewRow container = (GridViewRow)lblData.NamingContainer;
    //    object dataValue = DataBinder.Eval(container.DataItem, _columnName);
    //    if (dataValue != DBNull.Value)
    //    {
    //        lblData.Text = dataValue.ToString(); //.Split(_sep.ToCharArray())[0];
    //        //lblData.Attributes.Add("DBID", dataValue.ToString().Split(_sep.ToCharArray())[2]);
    //        //lblData.Text = dataValue.ToString();
    //    }
    //}

    //void hdn_DataBinding(object sender, EventArgs e)
    //{
    //    HiddenField txtdata = (HiddenField)sender;
    //    GridViewRow container = (GridViewRow)txtdata.NamingContainer;
    //    object dataValue = DataBinder.Eval(container.DataItem, _columnName);
    //    if (dataValue != DBNull.Value)
    //    {
    //        txtdata.Value = dataValue.ToString().Split(_sep.ToCharArray())[2];
    //    }
    //}


    //--------------------------sir end--------------------------------------

}