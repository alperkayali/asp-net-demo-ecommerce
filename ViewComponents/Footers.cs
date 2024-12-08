using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_WebAPP_MVC_PROJE.ViewComponents
{
    public class Footers : ViewComponent
    {
        KayaliContext context = new KayaliContext();

        public IViewComponentResult Invoke()
        {
            List<Supplier> supplierList = context.Suppliers.OrderBy(s => s.BrandName).ToList();
            return View(supplierList);
        }
    }
}
