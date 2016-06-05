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

            //Checking the URL entered by user is not duplicated in database
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


        //Get a unique shotened URL string
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

            //Checking the shortened URL is unique and is not duplicated in database
            if (existingShortUrl != null)
            {
                GetUniqueShortUrl();
            }

            return ShortenedUrl;
        }

        //Finding long URL by looking for shortened URL from database
        public ActionResult LongUrlFinder(string shortUrl)
        {
            var ActualUrl = (from c in db.Urls
                              where c.ShortUrl == shortUrl
                              select c).FirstOrDefault();

            if(ActualUrl == null)
            {
                //If no such URL redirect to Home
                return RedirectToAction("Index");
            } else
            {
                //If such URL found, redirect to it's website
                return Redirect(ActualUrl.LongUrl);
            }
        }
    }
}