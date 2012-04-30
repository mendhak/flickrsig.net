using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.XPath;

namespace flickr.mendhak.com
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class f : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {



            //mode
            if (!String.IsNullOrEmpty(context.Request.QueryString["get"]) && !String.IsNullOrEmpty(context.Request.QueryString["nsid"]))
            {

                int latest = 1;

                if (!String.IsNullOrEmpty(context.Request.QueryString["latest"]))
                {
                    latest = Convert.ToInt32(context.Request.QueryString["latest"]);
                }


                //Get the data first, then parse later.
                string api = "http://api.flickr.com/services/rest/?method=flickr.people.getPublicPhotos&api_key=0000000000000000000&per_page=1&page=" + latest + "&user_id=" + context.Request.QueryString["nsid"];

                XPathDocument doc = new XPathDocument(api);
                XPathNavigator nav = doc.CreateNavigator();

                XPathNavigator status = nav.SelectSingleNode("rsp/@stat");

                XPathNavigator singlePhoto = nav.SelectSingleNode("/rsp/photos/photo[1]");

                //If we actually have an image now
                if (status.Value == "ok" && singlePhoto != null)
                {

                    //Get a URL
                    if (context.Request.QueryString["get"] == "url")
                    {
                        //http://www.flickr.com/photos/{0}/{1}
                        // userid, photo id
                        string url = String.Format("http://www.flickr.com/photos/{0}/{1}", context.Request.QueryString["nsid"], singlePhoto.SelectSingleNode("@id").Value);
                        context.Response.Redirect(url);

                    }
                    //Get a thumbnail
                    else if (context.Request.QueryString["get"] == "img" || context.Request.QueryString["get"]=="imgurl")
                    {
                        //http://farm{0}.static.flickr.com/{1}/{2}_{3}_t.jpg


                        string sizeSuffix = "s";

                        if (!String.IsNullOrEmpty(context.Request.QueryString["size"]) && String.Compare(context.Request.QueryString["size"], "large") == 0)
                        {
                            sizeSuffix = "b";
                        }
                        string thumb = String.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}_{4}.jpg", singlePhoto.SelectSingleNode("@farm").Value,
                            singlePhoto.SelectSingleNode("@server").Value, singlePhoto.SelectSingleNode("@id").Value, singlePhoto.SelectSingleNode("@secret").Value,
                            sizeSuffix
                            );


                        // farm id, server, id, secret
                      
						if(!String.IsNullOrEmpty(context.Request.QueryString["caching"]) &&
						   String.Compare(context.Request.QueryString["caching"],"true",StringComparison.OrdinalIgnoreCase)!=0
						   )
						{
							context.Response.AddHeader("pragma", "no-cache");
	                        context.Response.AddHeader("cache-control", "private");
	                        context.Response.CacheControl = "no-cache";
	                        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);	
						}
                        
                        if(context.Request.QueryString["get"]=="imgurl")
						{
							context.Response.Write(thumb);
						}
						else
						{
							context.Response.Redirect(thumb);
						}
						
                        
                    }
				
                }
             


            }

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
