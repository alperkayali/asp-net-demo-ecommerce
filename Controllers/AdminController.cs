using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Core;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using AspNetCore_WebAPP_MVC_PROJE.Models.MVVM;
using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;

namespace AspNetCore_WebAPP_MVC_PROJE.Controllers
{
    public class AdminController : Controller
    {
        #region INSTANCES
        KayaliContext context = new KayaliContext();

        cls_User u = new cls_User();
        cls_Product p = new cls_Product();
        cls_Category c = new cls_Category();
        cls_Supplier s = new cls_Supplier();
        cls_Status st = new cls_Status();
        cls_Settings cs = new cls_Settings();
        cls_Content cc = new cls_Content();
        #endregion

        #region FILLER METHODS
        async Task CategoryDropDownFill()
        {
            List<Category> categories = await context.Categories.ToListAsync();
            ViewData["CategoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        void CategoryDropDownFillMain()
        {
            List<Category> categories = context.Categories.Where(c => c.ParentID == 0).ToList();
            ViewData["MainCategoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }
        async Task SupplierDropDownFill()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["SupplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }
        async Task StatusDropDownFill()
        {
            List<Status> statuses = await st.StatusSelect();
            ViewData["StatusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
        }
        #endregion

        #region LOGIN
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
        {
            if (ModelState.IsValid)
            {
                User? usr = await u.LoginControl(user);
                if (usr != null)
                {
                    return RedirectToAction("GeneralIndex");
                }
            }

            else
            {
                ViewBag.error = "Wrong Login ID or Password combination !";
            }
            return View();
        }
        #endregion

        public IActionResult GeneralIndex()
        {
            return View();
        }

        #region CATEGORY
        public async Task<IActionResult> CategoryIndex()
        {
            List<Category> categories = await c.CategorySelectMain();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryCreate()
        {
            List<Category> categories = await c.CategorySelectMain();
            CategoryDropDownFillMain();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryCreate(Category category)
        {
            bool answer = await c.CategoryInsert(category);
            if (answer)
            {
                TempData["Message"] = "Category has been added successfully.";
            }
            else
            {
                TempData["Message"] = "ERROR ! Category has not been added !";
            }
            return RedirectToAction(nameof(CategoryIndex));
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetails(int? id)
        {


            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var catDets = await context.Categories.FirstOrDefaultAsync(e => e.CategoryID == id);

            if (catDets == null)
            {
                return NotFound();
            }
            return View(catDets);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryEdit(int? id)
        {
            CategoryDropDownFillMain();
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var toBeEdited = await c.CategoryDetails(id);
            if (toBeEdited == null)
            {
                return NotFound();
            }
            return View(toBeEdited);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryEdit(Category category)
        {
            bool answer = await c.CategoryEdit(category);
            if (answer)
            {
                TempData["Message"] = "Category has been EDITED successfull..";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Category has not been EDITED !";
                return RedirectToAction(nameof(CategoryEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var tobeDeleted = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            if (tobeDeleted == null)
            {
                return NotFound();
            }
            return View(tobeDeleted);
        }

        [HttpPost, ActionName("CategoryDelete")]
        public async Task<IActionResult> CategoryDel(int? id)
        {
            bool answer = await c.CategoryDelete(id);

            if (answer)
            {
                TempData["Message"] = "Category has been DEACTIVATED successfull.";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Category has not been DEACTIVATED !";
                return RedirectToAction(nameof(CategoryDelete));
            }
        }
        //CATEGORIES PAGES END//
        #endregion

        #region SUPPLIER
        //SUPPLIER PAGES BEGIN//
        public async Task<IActionResult> SupplierIndex()
        {
            List<Supplier> categories = await s.SupplierSelect();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> SupplierCreate()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            return View(suppliers);
        }

        [HttpPost]
        public async Task<IActionResult> SupplierCreate(Supplier supplier)
        {
            bool answer = await s.SupplierInsert(supplier);
            if (answer)
            {
                TempData["Message"] = "Supplier has been added successfully.";
            }
            else
            {
                TempData["Message"] = "ERROR ! Supplier has not been added !";
            }
            return RedirectToAction(nameof(SupplierIndex));
        }

        [HttpGet]
        public async Task<IActionResult> SupplierDetails(int? id)
        {


            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supDets = await context.Suppliers.FirstOrDefaultAsync(e => e.SupplierID == id);

            if (supDets == null)
            {
                return NotFound();
            }
            return View(supDets);
        }

        [HttpGet]
        public async Task<IActionResult> SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await s.SupplierDetails(id);

            return View(supplier);
        }

        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {
            if (supplier.PhotoPath == null)
            {
                string? PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath;
                supplier.PhotoPath = PhotoPath;
            }

            bool answer = s.SupplierEdit(supplier);
            if (answer == true)
            {
                TempData["Message"] = "Supplier has been EDITED successfull..";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Supplier has not been EDITED.";
                return RedirectToAction(nameof(SupplierEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var tobeDeleted = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
            if (tobeDeleted == null)
            {
                return NotFound();
            }
            return View(tobeDeleted);
        }

        [HttpPost, ActionName("SupplierDelete")]
        public async Task<IActionResult> SupDel(int? id)
        {
            bool answer = await s.SupplierDelete(id);

            if (answer)
            {
                TempData["Message"] = "Supplier has been DEACTIVATED successfull.";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Supplier has not been DEACTIVATED !";
                return RedirectToAction(nameof(SupplierDelete));
            }
        }
        #endregion

        #region STATUS
        //STATUS PAGES BEGIN//
        public async Task<IActionResult> StatusIndex()
        {
            List<Status> statuses = await st.StatusSelect();
            return View(statuses);
        }

        public async Task<IActionResult> StatusCreate()
        {
            List<Status> statuses = await st.StatusSelect();
            return View(statuses);
        }

        [HttpPost]
        public async Task<IActionResult> StatusCreate(Status status)
        {
            bool answer = await st.StatusInsert(status);
            if (answer)
            {
                TempData["Message"] = "Status has been added successfully.";
            }
            else
            {
                TempData["Message"] = "ERROR ! Status has not been added !";
            }
            return RedirectToAction(nameof(StatusIndex));
        }

        [HttpGet]
        public async Task<IActionResult> StatusDetails(int? id)
        {


            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var stDets = await context.Statuses.FirstOrDefaultAsync(e => e.StatusID == id);

            if (stDets == null)
            {
                return NotFound();
            }
            return View(stDets);
        }

        [HttpGet]
        public async Task<IActionResult> StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var toBeEdited = await st.StatusDetails(id);
            if (toBeEdited == null)
            {
                return NotFound();
            }
            return View(toBeEdited);
        }

        [HttpPost]
        public async Task<IActionResult> StatusEdit(Status status)
        {

            bool answer = await st.StatusEdit(status);
            if (answer)
            {
                TempData["Message"] = "Status has been EDITED successfull..";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Status has not been EDITED !";
                return RedirectToAction(nameof(StatusEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> StatusDelete(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var tobeDeleted = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);
            if (tobeDeleted == null)
            {
                return NotFound();
            }
            return View(tobeDeleted);
        }

        [HttpPost, ActionName("StatusDelete")]
        public async Task<IActionResult> StatsDel(int? id)
        {
            bool answer = await st.StatusDelete(id);

            if (answer)
            {
                TempData["Message"] = "Supplier has been DEACTIVATED successfull.";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Supplier has not been DEACTIVATED !";
                return RedirectToAction(nameof(StatusDelete));
            }
        }
        #endregion

        #region PRODUCT
        public async Task<IActionResult> ProductIndex()
        {
            List<Product> products = await p.ProductSelect();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            List<Product> products = await p.ProductSelect();

            await CategoryDropDownFill();
            await SupplierDropDownFill();
            await StatusDropDownFill();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(Product product)
        {
            bool answer = await p.ProductInsert(product);
            if (answer)
            {
                TempData["Message"] = "Product has been added successfully.";
            }
            else
            {
                TempData["Message"] = "ERROR ! Product has not been added !";
            }
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var prdDets = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (prdDets == null)
            {
                return NotFound();
            }
            return View(prdDets);
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(int? id)
        {
            CategoryDropDownFill();
            SupplierDropDownFill();
            StatusDropDownFill();

            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var supplier = await p.ProductDetails(id);

            return View(supplier);
        }

        [HttpPost]
        public IActionResult ProductEdit(Product product)
        {
            //DataBase'ten kaydı çektik ve formdan gelmeyecek bazı kolonları null yerine mevcuttaki bilgiyi tekrar bassın.
            //Edit'lerken değiştirilmeyecek kayıtları olduğu gibi bırakmak için. ÖRN:
            Product? prd = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID);
            product.AddDate = prd.AddDate;
            product.HighLighted = prd.HighLighted;
            product.TopSeller = prd.TopSeller;

            if (product.PhotoPath == null)
            {
                string? PhotoPath = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
                product.PhotoPath = PhotoPath;
            }

            bool answer = p.ProductEdit(product);
            if (answer == true)
            {
                TempData["Message"] = "The product has been EDITED successfully.";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "ERROR ! Product has not been EDITED.";
                return RedirectToAction(nameof(ProductEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var tobeDeleted = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (tobeDeleted == null)
            {
                return NotFound();
            }
            return View(tobeDeleted);
        }

        [HttpPost, ActionName("ProductDelete")]
        public async Task<IActionResult> ProdDel(int? id)
        {
            bool answer = await p.ProductDelete(id);

            if (answer)
            {
                TempData["Message"] = "Product has been DEACTIVATED successfull.";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Product has not been DEACTIVATED !";
                return RedirectToAction(nameof(ProductDelete));
            }
        }
        #endregion

        #region SETTING
        //SETTINGS PAGES BEGIN//

        public async Task<IActionResult> SettingsIndex()
        {
            List<Setting> settings = await cs.SettingsList();
            return View(settings);
        }


        public async Task<IActionResult> SettingCreate()
        {
            List<Setting> settings = await cs.SettingsList();

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> SettingCreate(Setting setting)
        {
            bool answer = await cs.SettingInsert(setting);
            if (answer)
            {
                TempData["Message"] = "Setting has been added successfully.";
            }
            else
            {
                TempData["Message"] = "ERROR ! Setting has not been added !";
            }
            return RedirectToAction(nameof(SettingsIndex));
        }

        [HttpGet]
        public async Task<IActionResult> SettingDetails(int? id)
        {
            if (id == null || context.Settings == null)
            {
                return NotFound();
            }

            var settingDets = await context.Settings.FirstOrDefaultAsync(e => e.SettingID == id);

            if (settingDets == null)
            {
                return NotFound();
            }
            return View(settingDets);
        }

        [HttpGet]
        public async Task<IActionResult> SettingEdit(int? id)
        {
            if (id == null || context.Settings == null)
            {
                return NotFound();
            }

            var toBeEdited = await cs.SettingDetails(id);
            if (toBeEdited == null)
            {
                return NotFound();
            }
            return View(toBeEdited);
        }

        [HttpPost]
        public async Task<IActionResult> SettingEdit(Setting setting)
        {

            bool answer = await cs.SettingEdit(setting);
            if (answer)
            {
                TempData["Message"] = "Setting has been EDITED successfull..";
                return RedirectToAction(nameof(SettingsIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Setting has not been EDITED !";
                return RedirectToAction(nameof(SettingEdit));
            }
        }
        //SETTINGS PAGES END//
        #endregion

        #region CONTENT
        public async Task<IActionResult> ContentIndex()
        {
            List<Content> contents = await cc.ContentList();
            return View(contents);
        }

        public async Task<IActionResult> ContentCreate()
        {
            List<Content>? contents = await cc.ContentList();
            return View(contents);
        }

        [HttpPost]
        public async Task<IActionResult> ContentCreate(Content content)
        {
            bool answer = await cc.ContentInsert(content);
            if (answer)
            {
                TempData["Message"] = "Content has been added successfully.";
            }
            else
            {
                TempData["Message"] = "ERROR ! Content has not been added !";
            }
            return RedirectToAction(nameof(ContentIndex));
        }

        [HttpGet]
        public async Task<IActionResult> ContentEdit(int? id)
        {
            if (id == null || context.Contents == null)
            {
                return NotFound();
            }

            var toBeEdited = await cc.ContentDetails(id);
            if (toBeEdited == null)
            {
                return NotFound();
            }
            return View(toBeEdited);
        }

        [HttpPost]
        public IActionResult ContentEdit(Content content)
        {

            bool answer = cc.ContentEdit(content);
            if (answer)
            {
                TempData["Message"] = "Content has been EDITED successfull..";
                return RedirectToAction(nameof(ContentIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Content has not been EDITED !";
                return RedirectToAction(nameof(ContentEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ContentDelete(int? id)
        {
            if (id == null || context.Contents == null)
            {
                return NotFound();
            }

            var tobeDeleted = await context.Contents.FirstOrDefaultAsync(c => c.ContentID == id);
            if (tobeDeleted == null)
            {
                return NotFound();
            }
            return View(tobeDeleted);
        }

        [HttpPost, ActionName("ContentDelete")]
        public async Task<IActionResult> ContDel(int? id)
        {
            bool answer = await cc.ContentDelete(id);

            if (answer)
            {
                TempData["Message"] = "Content has been DEACTIVATED successfull.";
                return RedirectToAction(nameof(ContentIndex));
            }
            else
            {
                TempData["Message"] = "ERROR ! Content has not been DEACTIVATED !";
                return RedirectToAction(nameof(ContentDelete));
            }
        }
        #endregion
    }
}