using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URLShortenerApp.DataContexts;
using URLShortenerApp.Models;

namespace URLShortenerApp.Controllers
{
    public class HomeController : Controller
    {
        private UrlsDb db = new UrlsDb();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UrlShortener(string InputUrl)
        {
            var existingLongUrl = (from c in db.Urls
                                   where c.LongUrl == InputUrl
                                   select c).FirstOrDefault();

            if (existingLongUrl == null)
            {
                string shortenedUrl = GetUniqueShortUrl();

                db.Urls.Add(new Url
                {
                    LongUrl = InputUrl,
                    ShortUrl = shortenedUrl,
                });

                db.SaveChanges();

                ViewBag.ShortUrl = shortenedUrl;
            } else
            {
                var shortenedUrl = (from c in db.Urls
                                       where c.LongUrl == InputUrl
                                       select c.ShortUrl).FirstOrDefault();

                ViewBag.ShortUrl = shortenedUrl;
            }

            ViewBag.LongUrl = InputUrl;

            return View();
        }

        public string GetUniqueShortUrl()
        {
            //Random String for shorten Url
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string ShortenedUrl = new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var existingShortUrl = (from c in db.Urls
                                    where c.ShortUrl == ShortenedUrl
                                    select c).FirstOrDefault();

            if (existingShortUrl != null)
            {
                GetUniqueShortUrl();
            }

            return ShortenedUrl;
        }

        public ActionResult LongUrlFinder(string shortUrl)
        {
            var ActualUrl = (from c in db.Urls
                              where c.ShortUrl == shortUrl
                              select c).FirstOrDefault();

            if(ActualUrl == null)
            {
                return RedirectToAction("Index");
            } else
            {
                return Redirect("http://" + ActualUrl.LongUrl);
            }
        }
    }
}