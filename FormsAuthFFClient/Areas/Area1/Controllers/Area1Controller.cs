using FormsAuthFFClient.Filters;
using System.Web.Mvc;

namespace FormsAuthFFClient.Areas.Area1.Controllers
{
    public class Area1Controller : Controller
    {
        [CustomAuthAttribute(AuthArea = "/Area1")]
        // GET: Area1/Area1
        public ActionResult Index()
        {
            return View();
        }
    }
}