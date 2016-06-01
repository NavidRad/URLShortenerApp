using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URLShortenerApp.Models;

namespace URLShortenerApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Shortener(string Url)
        {
            UrlClass UrlClass = new UrlClass();
            UrlClass.Url = Url;


            return View(UrlClass);
        }

    }
}