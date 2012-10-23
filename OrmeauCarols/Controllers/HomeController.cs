using System;
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

        //[OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            var twitterAccess = new WebClient();
            var xml = twitterAccess.DownloadString(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=ormeaucarols"));
            var xmlTweets = XElement.Parse(xml);
            var tweets = from tweet in xmlTweets.Descendants("status")
                         select new Tweet
                                    {
                                        Message = (string) tweet.Element("text"),
                                        User = (string) tweet.Element("user").Element("screen_name"),
                                        LinkUrl = "https://twitter.com/OrmeauCarols/status/" + (string)tweet.Element("id"),
                                    };
            return
                View(new IndexViewModel
                         {
                             Tweets = tweets.ToArray()
                         });
        }
    }

    public class IndexViewModel
    {
        public Tweet[] Tweets { get; set; }
    }

    public class Tweet
    {
        public string User { get; set; }
        public string Message { get; set; }
        public string LinkUrl { get; set; }
    }
}
