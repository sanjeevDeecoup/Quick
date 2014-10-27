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
using System.Net.NetworkInformation;

namespace QuickWeb.New_Booking
{  
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["BackgroundColor"] != null)
            {
                ColorSelector.SelectedValue = Request.Cookies["BackgroundColor"].Value;
                BodyTag.Style["background-color"] = ColorSelector.SelectedValue;
            }
        }
        protected void ColorSelector_IndexChanged(object sender, EventArgs e)
        {
            BodyTag.Style["background-color"] = ColorSelector.SelectedValue;
            HttpCookie cookie = new HttpCookie("BackgroundColor");
            cookie.Value = ColorSelector.SelectedValue;
            cookie.Expires = DateTime.Now.AddHours(1);
            Response.SetCookie(cookie);
        }
        protected void btnclearcookies_Click(object sender, EventArgs e)
        {
            Response.Cookies["BackgroundColor"].Expires = DateTime.Now.AddDays(-1);
        }        
    }
}

    
  


               
 
  

