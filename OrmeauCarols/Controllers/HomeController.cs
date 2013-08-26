using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OrmeauCarols.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [OutputCache(Duration = 600)]
        public ActionResult Index()
        {

            var facebookClient = new WebClient();
            facebookClient.Headers.Add("user-agent", "fiddler"); // To stop Facebook from wrapping the RSS in HTML
            var facebookXml = facebookClient.DownloadString(new Uri("https://www.facebook.com/feeds/page.php?id=246744065349913&format=rss20"));
            var xStatuses = XElement.Parse(facebookXml);
            var statuses = from status in xStatuses.Descendants("item").Take(5)
                           select new FacebookStatus
                                      {
                                          Title = (string) status.Element("title"),
                                          Author = (string) status.Element("author"),
                                          LinkUrl = (string) status.Element("link")
                                      };

            //var twitterClient = new WebClient();
            //var twitterXml = twitterClient.DownloadString(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=ormeaucarols"));
            //var xTweets = XElement.Parse(twitterXml);
            //var tweets = from tweet in xTweets.Descendants("status")
            //             select new Tweet
            //                        {
            //                            Message = (string) tweet.Element("text"),
            //                            User = (string) tweet.Element("user").Element("screen_name"),
            //                            LinkUrl = "https://twitter.com/OrmeauCarols/status/" + (string)tweet.Element("id"),
            //                        };
            return
                View(new IndexViewModel
                         {
                             Tweets = new Tweet[] {},
                             FacebookStatuses = statuses.ToArray()
                         });
        }
    }

    public class FacebookStatus
    {
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string Author { get; set; }
    }

    public class IndexViewModel
    {
        public Tweet[] Tweets { get; set; }
        public FacebookStatus[] FacebookStatuses { get; set; }
    }

    public class Tweet
    {
        public string User { get; set; }
        public string Message { get; set; }
        public string LinkUrl { get; set; }
    }
}
