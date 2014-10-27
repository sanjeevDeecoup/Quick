<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;


public class Handler : IHttpHandler
{


    DTO.NewPriceLists Ob = new DTO.NewPriceLists();
    

    public DTO.NewPriceLists  SetfetchValue()
    {
        Ob.BranchId = "1";
        //Globals.BranchID;
        return Ob;
    }
    DataSet ds = new DataSet();
    //DataSet ds1 = new DataSet();
    //DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    //DataSet ds4 = new DataSet();
    string res = string.Empty;
    public void save()
    {
        SetfetchValue();
        ds = BAL.BALFactory.Instance.BAL_NewPriceLists.fetchpricelist(Ob);
        

        ////SetfetchValue();
        
        //ds1 = BAL.BALFactory.Instance.BAL_NewPriceLists.fetchpricelistcoloum(Ob);
        ////SetfetchValue();
       
        //ds2 = BAL.BALFactory.Instance.BAL_NewPriceLists.fetchpricelistcoloumcount(Ob);
        //int count = Int32.Parse(ds2.Tables[0].Rows[0]["count"].ToString());

        ////SetfetchValue();

        ds3 = BAL.BALFactory.Instance.BAL_NewPriceLists.fetchpricelistcoloumvaluecount(Ob);

        ////SetfetchValue();
        //ds4 = BAL.BALFactory.Instance.BAL_NewPriceLists.fetchpricelistcoloumvaluecountvalue(Ob);
       
        //int countvalue = Int32.Parse(ds4.Tables[0].Rows[0]["count"].ToString());


    }

    
    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";

        string[] ar = context.Request.QueryString["ar"].Split('|');

        for (int i = 0; i < ar.Length-1; i++)
        {

            string[] ar1 = ar[i].Split(',');


            for (int l = 0; l < ar1.Length - 1; l++)
            {
                save();
                Ob.SubItemRefID = ds.Tables[0].Rows[i]["SubItemRefID"].ToString();
                Ob.CategoryID = ds.Tables[0].Rows[i]["CategoryID"].ToString();
                Ob.BranchId = "1";
                Ob.ItemCode = ds.Tables[0].Rows[i]["ItemID"].ToString();
                Ob.VariationId = ds.Tables[0].Rows[i]["VariationID"].ToString();
                string pro = ds3.Tables[0].Rows[l+8]["name"].ToString();
                string[] strArr1 = pro.Split('_');
                Ob.Processid = strArr1[1];
                Ob.Price = ar1[l].ToString();
                Ob.DateCreated = "06/06/2012";
                Ob.DateModified = "06/06/2012";
                Ob.Active = "1";
                res = BAL.BALFactory.Instance.BAL_NewPriceLists.SaveNewItemprice(Ob);
                

            }
         
        }   

        context.Response.Write("updated");

 }

 

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}