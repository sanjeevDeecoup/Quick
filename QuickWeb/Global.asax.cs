using System;
using System.IO;
using System.Web;
using System.Web.Security;

namespace QuickWeb
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            HttpCookieCollection cookies = context.Request.Cookies;
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (cookies["StartTime"] == null)
            {
                HttpCookie cookie = new HttpCookie("starttime", DateTime.Now.ToString());
                cookie.Path = "/";
                context.Response.Cookies.Add(cookie);
            }
            else
            {
                if (url.Contains("WebsiteUser"))
                {
                    context.Response.Redirect("~/websiteuser/UserLogin.aspx");
                }
                else
                {
                    context.Response.Redirect(FormsAuthentication.LoginUrl);
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /*
            HttpContext context = ((HttpApplication)sender).Context;

            string AcceptEncoding = context.Request.Headers["Accept-Encoding"];
            if ((!string.IsNullOrEmpty(AcceptEncoding) && AcceptEncoding.Contains("gzip")) &&
                    (!context.Request.Url.LocalPath.Contains("Resource.axd")) &&
                        (!context.Request.Url.LocalPath.Contains(".png")))
            {
                context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                context.Response.AddHeader("Content-Encoding", "gzip");
            }
             * */
            /*
            if (HttpContext.Current.Request.IsSecureConnection.Equals(false) /*&& HttpContext.Current.Request.IsLocal.Equals(false)**)
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"]
            + HttpContext.Current.Request.RawUrl);
            }*/
            /*
            switch (Request.Url.Scheme)
            {
                case "https":
                    Response.AddHeader("Strict-Transport-Security", "max-age=300");
                    break;

                case "http":
                    var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", path);
                    break;
            }
             * */
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // some error occurred, just log out now, but later redirect to 404 page
            try
            {                
                var filePath = Path.Combine(Server.MapPath("~"), "LogFile.log");
                File.AppendAllText(filePath, Environment.NewLine);
                File.AppendAllText(filePath, Environment.NewLine);
                File.AppendAllText(filePath, "====================================================");
                File.AppendAllText(filePath, Environment.NewLine + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                File.AppendAllText(filePath, Environment.NewLine + HttpContext.Current.Server.GetLastError().InnerException.ToString());
                File.AppendAllText(filePath, Environment.NewLine + HttpContext.Current.Server.GetLastError().Message);
                File.AppendAllText(filePath, Environment.NewLine + "*****************************************************");
                File.AppendAllText(filePath, Environment.NewLine);
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                // check if the error is because of Globals.BranchId is set to null, that is the disable page problem
                if (Server.GetLastError().InnerException.TargetSite.Module.ToString() == "QuickWeb.dll" && Server.GetLastError().InnerException.TargetSite.GetMethodBody().LocalVariables[0].LocalType.Name.ToString() == "ArrayList"
                    && Session["BID"] == null)
                {
                    if (url.Contains("WebsiteUser"))
                    {
                        Response.Redirect("~/websiteuser/UserLogin.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Login.aspx");
                    }
                }
                else
                {
                    if (url.Contains("WebsiteUser"))
                    {
                        Response.Redirect("~/websiteuser/UserLogin.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/ErrorPage.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}