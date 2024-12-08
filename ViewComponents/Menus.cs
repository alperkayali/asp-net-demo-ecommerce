using Microsoft.AspNetCore.Mvc;
using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;

namespace AspNetCore_WebAPP_MVC_PROJE.ViewComponents
{
    public class Menus: ViewComponent
    {
        KayaliContext context = new KayaliContext();

        public IViewComponentResult Invoke()
        {
            List<Category> categoryList =context.Categories.OrderBy(c=>c.CategoryName).ToList();
            return View(categoryList);
        }



    }
}
