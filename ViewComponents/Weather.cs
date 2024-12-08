using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AspNetCore_WebAPP_MVC_PROJE.ViewComponents
{
    public class Weather : ViewComponent
    {
        public string Invoke()
        {
            string apikey = "api-key";
            string city = "izmir";
            string url = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&lang=tr&units=metric&appid=" + apikey;

            XDocument weather = XDocument.Load(url);
            var temperature = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            return $"{city.ToUpper()} - {temperature} degrees C";
        }
    }
}
