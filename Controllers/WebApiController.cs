using AspNetCore_WebAPP_MVC_PROJE.Models.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspNetCore_WebAPP_MVC_PROJE.Controllers
{
    public class WebApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region WEB API PAGES

        public IActionResult Pharmacies()
        {
            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");
            var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);
            return View(pharmacy);
        }

        public IActionResult ArtAndCulture()
        {
            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");
            var artculture = JsonConvert.DeserializeObject<List<ArtAndCulture>>(json);
            return View(artculture);
        }
        #endregion

    }
}
