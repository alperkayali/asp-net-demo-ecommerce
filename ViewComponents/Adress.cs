using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_WebAPP_MVC_PROJE.ViewComponents
{
    public class Adress : ViewComponent
    {
        KayaliContext context = new KayaliContext();
        public string Invoke()
        {
            string? address = context.Settings.FirstOrDefault(s => s.SettingID == 1).adress;
            return $"{address}";
        }
    }
}
