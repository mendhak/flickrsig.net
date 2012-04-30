using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace flickr.mendhak.com
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //string m = Request.Url.ToString();  //segments[1]

            if (!Request.Url.ToString().ToLower().Contains("default.aspx") 
                && !Request.Url.ToString().ToLower().Contains("ashx") 
                && !Request.Url.ToString().ToLower().Contains("html"))
            {
                
                //Context.RewritePath("~/View.aspx?color=" + Request.Url.Segments[1]);  //Segments contains the color

               // Response.Write(Request.Url.ToString());

                //foreach (string seg in Request.Url.Segments)
                //{
                //    Response.Write(seg + "<br />");
                //}
                //Response.Write(Request.Url.Segments[1]);











            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}