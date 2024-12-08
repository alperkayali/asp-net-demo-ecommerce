using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_WebAPP_MVC_PROJE.Controllers
{
	public class FooterController : Controller
	{
		public IActionResult InformationSecurityPolicy()
		{
			return View();
		}

		public IActionResult OccupationalSafetyPolicy()
		{
            return View();
        }
	}
}
