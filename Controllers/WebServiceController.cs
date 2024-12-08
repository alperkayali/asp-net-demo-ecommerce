using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_WebAPP_MVC_PROJE.Controllers
{
    public class WebServiceController : Controller
    {
        public static string tckimlik_or_vergino = "";

        public IActionResult Index()
        {
            return View();
        }
    }
}
