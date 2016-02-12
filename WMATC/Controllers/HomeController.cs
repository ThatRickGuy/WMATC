using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMATC.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Warmachine/Hordes - Americas Team Championship.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Information.";

            return View();
        }
    }
}