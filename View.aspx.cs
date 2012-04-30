using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Xml.XPath;
using System.Net;

namespace flickr.mendhak.com
{
    public partial class View : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string color = "#000000";

        
            if (!String.IsNullOrEmpty(Request.QueryString["color"]) && (Request.UrlReferrer != null))
            {

                string potentialColor = Request.QueryString["color"];
                string hexadecimalPattern = "^[0-9A-Fa-f]{3,6}$";
                string rgbPattern = "^[0-9]{1,3},[0-9]{1,3},[0-9]{1,3}$";

                Match hexadecimalMatch = Regex.Match(potentialColor, hexadecimalPattern, RegexOptions.IgnoreCase);
                Match rgbMatch = Regex.Match(potentialColor, rgbPattern, RegexOptions.IgnoreCase);



                if (String.Compare(potentialColor, "guess", true) == 0 ||String.Compare(potentialColor, "magic", true) == 0)
                {
                    //Download medium image.
                    //rgb
                    //average
                    //set
                    string flickrPattern = "^http://www\\.flickr\\.com/photos/[^\\/]+/([0-9]+)";

                    Match flickrMatch = Regex.Match(Request.UrlReferrer.ToString().ToLower(), flickrPattern);

                    if (flickrMatch.Success)
                    {
                        string photoId = flickrMatch.Groups[1].Value;

                        string api = "http://api.flickr.com/services/rest/?method=flickr.photos.getInfo&photo_id=" + photoId + "&api_key=b4fe2a004c947c42b2be8f2796796105";

                        XPathDocument doc = new XPathDocument(api);
                        XPathNavigator nav = doc.CreateNavigator();

                        XPathNavigator singlePhoto = nav.SelectSingleNode("/rsp/photo");

                        string mediumImageUrl = String.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}.jpg", singlePhoto.SelectSingleNode("@farm").Value,
                                        singlePhoto.SelectSingleNode("@server").Value, singlePhoto.SelectSingleNode("@id").Value, singlePhoto.SelectSingleNode("@secret").Value);


                        System.Drawing.Image mediumImage = GetImage(mediumImageUrl);
                        Bitmap mediumBitmap = new Bitmap(mediumImage);
                        UnsafeBitmap ub = new UnsafeBitmap(mediumBitmap);

                        ub.LockImage();

                        int r = 0;
                        int g = 0;
                        int b = 0;

                        for (int h = 0; h < mediumBitmap.Height; h++)
                        {
                            for (int w = 0; w < mediumBitmap.Width; w++)
                            {
                                Color c = ub.GetPixel(w, h);

                                r += c.R;
                                g += c.G;
                                b += c.B;
                            }
                        }

                        int totalPixels = mediumBitmap.Height * mediumBitmap.Width;

                        r = r / totalPixels;
                        g = g / totalPixels;
                        b = b / totalPixels;

                        color = RGBToHex(r, g, b);

                        ub.UnlockImage();
                        mediumBitmap.Dispose();

                    }
                }
                else if (hexadecimalMatch.Success)
                {
                    color = "#" + hexadecimalMatch.Groups[0].Value;
                }
                else if (rgbMatch.Success)
                {
                    string[] rgbValues = potentialColor.Split(',');
                    int r = Convert.ToInt32(rgbValues[0]);
                    int g = Convert.ToInt32(rgbValues[1]);
                    int b = Convert.ToInt32(rgbValues[2]);

                    color = RGBToHex(r, g, b);

                }
                else
                {
                    //Try matching with a known color name

                    Color known = Color.FromName(potentialColor);
                    if (known.IsKnownColor)
                    {
                        color = RGBToHex(known.R, known.G, known.B);
                    }
                }
            }


            if (Request.UrlReferrer != null)
            {
                string flickrPattern = "^http://www\\.flickr\\.com/photos/[^\\/]+/([0-9]+)";

                Match flickrMatch = Regex.Match(Request.UrlReferrer.ToString().ToLower(), flickrPattern);

                if (flickrMatch.Success)
                {
                    string photoId = flickrMatch.Groups[1].Value;

                    string api = "http://api.flickr.com/services/rest/?method=flickr.photos.getInfo&photo_id=" + photoId + "&api_key=b4fe2a004c947c42b2be8f2796796105";

                    XPathDocument doc = new XPathDocument(api);
                    XPathNavigator nav = doc.CreateNavigator();

                    XPathNavigator singlePhoto = nav.SelectSingleNode("/rsp/photo");

                    string largeImage = String.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}_b.jpg", singlePhoto.SelectSingleNode("@farm").Value,
                                    singlePhoto.SelectSingleNode("@server").Value, singlePhoto.SelectSingleNode("@id").Value, singlePhoto.SelectSingleNode("@secret").Value);

                    HyperLinkFlickr.ImageUrl = largeImage;

                    string title = nav.SelectSingleNode("/rsp/photo/title").Value;

                    string nsid = nav.SelectSingleNode("/rsp/photo/owner/@nsid").Value;

                    string photoUrl = String.Format("http://www.flickr.com/photos/{0}/{1}", nsid, singlePhoto.SelectSingleNode("@id").Value);

                    HyperLinkFlickr.NavigateUrl = photoUrl;
                    HyperLinkFlickr.ToolTip = title;
                    Page.Title = title;

                }

            }
            else
            {
                Literal literalErrorMessage = new Literal();
                literalErrorMessage.Text = "<div style=\"font-family:Arial;color:white;\">O hai, it looks like your browser hasn't sent us the referring web address. You might <br />1. be using a crappy browser, in which case you'll need to check your settings and enable referring URLs. <br />2. have refreshed the page, in which case the photo will disappear! Press the browser back button to go back to the flickr photo page.</div>";
                this.form1.Controls.Add(literalErrorMessage);
                //this.Controls.Add(literalErrorMessage);

            }
            



            Body1.Style.Add("background-color", color);

        }

        private System.Drawing.Image GetImage(string url)
        {
            // Create the requests.
            WebRequest requestPic = WebRequest.Create(url);

            WebResponse responsePic = requestPic.GetResponse();

            System.Drawing.Image webImage = System.Drawing.Image.FromStream(responsePic.GetResponseStream());

            return webImage;
        }

        private string RGBToHex(int r, int g, int b)
        {
            if (r >= 255) { r = 255; }
            if (g >= 255) { g = 255; }
            if (b >= 255) { b = 255; }

            string rHex = r.ToString("X");
            string gHex = g.ToString("X");
            string bHex = b.ToString("X");

            rHex = rHex.PadLeft(2, '0');
            gHex = gHex.PadLeft(2, '0');
            bHex = bHex.PadLeft(2, '0');

            string hexColor = "#" + rHex + gHex + bHex;
            return hexColor;


        }
    }
}
