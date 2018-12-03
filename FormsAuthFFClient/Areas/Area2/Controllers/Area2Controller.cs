using FormsAuthFFClient.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormsAuthFFClient.Areas.Area2.Controllers
{
    public class Area2Controller : Controller
    {
        // GET: Area2/Area2
        [CustomAuthAttribute(AuthArea = "/Area2")]
        public ActionResult Index()
        {
            return View();
        }
    }
}