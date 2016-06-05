using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using URLShortenerApp.Models;

namespace URLShortenerApp.DataContexts
{
    public class UrlsDb : DbContext
    {
        public UrlsDb() : base("UrlDBContext")
        {
        }
        public DbSet<Url> Urls { get; set; }
    }
}