using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormsAuthFFClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var blah = User.Identity.IsAuthenticated;
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }
    }
}