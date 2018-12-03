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

        [Route("Home/About")]
        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        [Route("Home1/Contacts")]
        [Authorize]
        public ActionResult Contacts()
        {
            return View();
        }
    }
}