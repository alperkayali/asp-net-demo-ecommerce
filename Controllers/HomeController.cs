using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using AspNetCore_WebAPP_MVC_PROJE.Models.MVVM;
using AspNetCore_WebAPP_MVC_PROJE.Models.DbViews;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using PagedList.Core;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json;

namespace AspNetCore_WebAPP_MVC_PROJE.Controllers
{
    public class HomeController : Controller
    {
        KayaliContext context = new KayaliContext();
        MainPageModel mpm = new MainPageModel();
        cls_Product cp = new cls_Product();
        cls_Order co = new cls_Order();
        cls_User cu = new cls_User();
        cls_Favourite cf = new cls_Favourite();

        int mainpageCount = 0;

        public HomeController()
        {
            mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).mainpageCount;
        }

        //Any single product.
        public IActionResult ProductDetails(int id)
        {
            //LINQ
            mpm.DetailsOfProduct = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();
            //EF CORE
            //mpm.DetailsOfProduct = context.Products.FirstOrDefault(p => p.ProductID == id);


            //LINQ
            mpm.CategoryName = (from p in context.Products
                                join c in context.Categories
                                on p.CategoryID equals c.CategoryID
                                select c.CategoryName).FirstOrDefault();

            mpm.CategoryID = (from p in context.Products
                              join c in context.Categories
                              on p.CategoryID equals c.CategoryID
                              select c.CategoryID).FirstOrDefault();

            //EF CORE
            //        mpm.CategoryName = context.Products
            //.Join(context.Categories,
            //      p => p.CategoryID,
            //      c => c.CategoryID,
            //      (p, c) => c.CategoryName)
            //.FirstOrDefault();

            mpm.BrandName = (from p in context.Products
                             join s in context.Suppliers
                             on p.SupplierID equals s.SupplierID
                             where p.ProductID == id
                             select s.BrandName).FirstOrDefault();

            mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.DetailsOfProduct.Related && p.ProductID != id).ToList();

            cp.Highlighted_Increase(id);

            return View(mpm);
        }

        #region MainPageModel methods for Index Page
        public IActionResult Index()
        {
            mpm.SliderProducts = cp.ProductSelect("Slider", mainpageCount, "", 0);
            mpm.ProductOfTheDay = cp.SpecialProductDetails("ProductOfTheDay");//kontrol paneline günün ürünü belirlemek için input koymak lazım.
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "", 0);
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "", 0);
            mpm.DiscountProducts = cp.ProductSelect("Discount", mainpageCount, "", 0);
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "", 0);
            mpm.TopSellerProducts = cp.ProductSelect("TopSeller", mainpageCount, "", 0);
            mpm.StarredProducts = cp.ProductSelect("Starred", mainpageCount, "", 0);
            mpm.OpportunityProducts = cp.ProductSelect("Opportunity", mainpageCount, "", 0);
            mpm.AttentionedProducts = cp.ProductSelect("Attentioned", mainpageCount, "", 0);
            return View(mpm);
        }
        #endregion

        #region NEW PRODUCTS AREA
        public IActionResult NewProducts()
        {
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "New", 0);
            return View(mpm);
        }


        public PartialViewResult _PartialNewProducts(string pageno)
        {
            int pageNumber = Convert.ToInt32(pageno);
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "New", pageNumber);
            return PartialView(mpm);
        }
        #endregion

        #region SPECIAL PRODUCTS AREA
        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "Special", 0);
            return View(mpm);
        }

        public PartialViewResult _PartialSpecialProducts(string pageno)
        {
            int pageNumber = Convert.ToInt32(pageno);
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "Special", pageNumber);
            return PartialView(mpm);
        }
        #endregion

        #region DISCOUNT PRODUCTS AREA
        public IActionResult DiscountProducts()
        {
            mpm.DiscountProducts = cp.ProductSelect("Discount", mainpageCount, "Discount", 0);
            return View(mpm);
        }

        public PartialViewResult _PartialDiscountProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountProducts = cp.ProductSelect("Discount", mainpageCount, "Discount", pagenumber);
            return PartialView(mpm);
        }
        #endregion

        #region HIGHLIGHTED PRODUCTS AREA
        public IActionResult HighlightedProducts()
        {
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "Highlighted", 0);
            return View(mpm);
        }

        public PartialViewResult _PartialHighlightedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "Highlighted", pagenumber);
            return PartialView(mpm);
        }
        #endregion

        #region TOP SELLER PRODUCTS AREA
        //NuGet - pagedlist.core & pagedlist.core.mvc
        public IActionResult TopSellerProducts(int page = 1, int pageSize = 4)
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, pageSize);
            return View("TopSellerProducts", model); //sadece model yazsak da olur.
        }
        #endregion

        #region CATEGORY - SUPPLIER PAGES
        public IActionResult CategoryPage(int id)
        {
            string catName = context.Categories.FirstOrDefault(c => c.CategoryID == id).CategoryName.ToUpper();
            ViewBag.CategoryName = catName;

            List<Product> products = cp.ProductSelectByCatID(id).OrderBy(p => p.ProductName).ToList();
            return View(products);
        }

        public IActionResult SupplierPage(int id)
        {
            string supName = context.Suppliers.FirstOrDefault(c => c.SupplierID == id).BrandName.ToUpper();
            ViewBag.SupplierName = supName;

            List<Product> productsOfSupplier = cp.ProductSelectBySupID(id).OrderBy(p => p.ProductName).ToList();
            return View(productsOfSupplier);
        }
        #endregion

        #region -- CART --
        //NuGet - microsoft.aspnet.core.http
        public IActionResult CartProcess(int id)
        {
            #region explanation
            //we will keep products and quantities like:
            //  10=1&20=3&30=7...
            // ProductID=Qantity&ProductID=Qantity&ProductID=Qantity...
            #endregion


            cp.Highlighted_Increase(id);

            co.ProductID = id;
            co.Quantity = 1;

            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["myCart"]; //cookie read

            if (cookie == null)
            {
                cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1); //cookies will stay max 1 day along.
                cookieOptions.Path = "/";

                co.MyCart = "";
                co.AddtoMyCart(id.ToString());

                Response.Cookies.Append("myCart", co.MyCart, cookieOptions); //cookies are sending to browser
            }

            else //if the Cart is not empty
            {
                co.MyCart = cookie; //Cart info that browser has, appended to MyCart property.
                if (co.AddtoMyCart(id.ToString()) == false) //Product had not been added to Cart. Let's add it.
                {
                    Response.Cookies.Append("myCart", co.MyCart, cookieOptions);
                    cookieOptions.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Session.SetString("Message", "The Product has been added to Shopping Cart.");
                    TempData["Message"] = "The Product has been added to Shopping Cart.";
                }

                else
                {
                    HttpContext.Session.SetString("Message", "You already have this Product in your Shopping Cart. The quantity can be changed in Shopping Cart page.");
                    HttpContext.Session.GetString("Message");
                    TempData["Message"] = "You already have this Product in your Shopping Cart. The quantity can be changed in Shopping Cart page.";
                }
            }
            string url = Request.Headers["Referer"].ToString();
            return Redirect(url);
        }

        public IActionResult Cart()
        {
            List<cls_Order> cart;

            if (HttpContext.Request.Query["scid"].ToString() != "")
            {
                //after clicking DELETE BUTTON in the cart page.
                string? scid = HttpContext.Request.Query["scid"];

                co.MyCart = Request.Cookies["myCart"]; //assigns the values in the cookies to the property.
                co.DeleteFromMyCart(scid); //then triggers the deleting method

                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("myCart", co.MyCart, cookieOptions); //sends the property with the new values to the browser cookie.
                //Now our Shopping Cart is renewed.
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                TempData["Message"] = "The Product has been REMOVED from Shopping Cart.";

                //Now we should send the last updated data to the cshtml page:
                cart = co.SelectMyCart();
                ViewBag.MyCart = cart;
                ViewBag.Cart_Table_Details = cart;
            }

            else
            {
                //after clicking CART HREF on top right corner.
                var cookie = Request.Cookies["MyCart"];
                if (cookie == null)
                {
                    co.MyCart = "";
                    cart = co.SelectMyCart();
                    ViewBag.MyCart = cart;
                    ViewBag.Cart_Table_Details = cart;
                }
                else
                {
                    var cookieOptions = new CookieOptions();
                    co.MyCart = Request.Cookies["myCart"];
                    cart = co.SelectMyCart();
                    ViewBag.MyCart = cart;
                    ViewBag.Cart_Table_Details = cart;
                }
            }

            if (cart.Count == 0)
            {
                ViewBag.MyCart = null;
            }

            return View();
        }
        #endregion

        #region USER LOGIN
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            string answer = cu.MemberControl(user);

            if (answer == "not found")
            {
                TempData["Message"] = "Wrong E-Mail adress.";

                return View();
            }

            else if (answer == "admin")
            {
                HttpContext.Session.SetString("UserInfo", answer);
                HttpContext.Session.SetString("Admin", answer);

                return RedirectToAction("GeneralIndex", "Admin");
            }

            else
            {
                HttpContext.Session.SetString("UserInfo", answer);
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region ORDER - PROCEED CHECKOUT
        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("UserInfo") != null)
            {
                //Session had already started. Get the user info.
                User? usr = cu.SelectUserInfo(HttpContext.Session.GetString("UserInfo"));
                return View(usr);
            }

            else
            {
                //Session had not started yet. Need to login and start session.
                return RedirectToAction(nameof(Login));
            }
        }


        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {                        //or instead (IFormCollection frm); (string frm)
                                 //then get the data as: Request.Form["CreditCartNo"]; etc
            string txt_individual = frm["txt_individual"];
            string txt_corporate = frm["txt_corporate"];

            if (txt_individual != null)
            {
                //DigitalPlanet codes here
                co.tckimlik_or_vergino = txt_individual;
                co.InvoiceCreate();
            }

            else
            {
                co.tckimlik_or_vergino = txt_corporate;
                co.InvoiceCreate();
            }

            string CreditCartNo = frm["CreditCartNo"];
            string ExpireMonth = frm["ExpireMonth"];
            string ExpireYear = frm["ExpireYear"];
            string CVV = frm["CVV"];

            #region EXAMPLE PAYU PAYMENT CODES will be like that:

            //NameValueCollection data = new NameValueCollection();
            //string url = "https://www.alperkayali.com/backhref";

            //data.Add("BACK_REF", url);
            //data.Add("CC_CVV", CVV);
            //data.Add("CC_NUMBER", CreditCartNo);
            //data.Add("EXP_MONTH", ExpireMonth);
            //data.Add("EXP_YEAR", "20" + ExpireYear);

            //var val = "";
            //foreach (var item in data)
            //{
            //    var value = item as string;
            //    var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
            //    val += byteCount + data.Get(value);
            //}

            //var signatureKey = "SECRET_KEY"; //key will be provided by PAYU like firms.
            //var hash = HashWithSignature(val, signatureKey);

            //data.Add("ORDER_HASH", hash);

            //var x = POSTFormPAYU("https://secure.payu.com.tr/order/.....", data); //we can get this link from DB dynamically.

            //if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            //{
            //    //virtual cart payment.
            //}
            //else
            //{
            //    //real cart payment.
            //}
            #endregion

            return RedirectToAction("backhref");
        }
        #endregion

        #region ORDER CONCLUSION
        public IActionResult backhref()
        {
            Confirm_Payment();
            return RedirectToAction("PayConclusion");
        }

        public static string OrderGroupGUID = "";
        public IActionResult Confirm_Payment()
        {
            //Will be deleted from cart cookie and added to order table.
            //E-Invoice method will be called.
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["myCart"];
            if (cookie != null)
            {
                co.MyCart = cookie;
                OrderGroupGUID = co.OrderCreate(HttpContext.Session.GetString("UserInfo").ToString());

                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("myCart");

                //Don't have mail or sms sending subscription yet. So they're closed for now.
                //cu.SendSMS(OrderGroupGUID);
                //cu.SendEMAIL(OrderGroupGUID);
            }

            return RedirectToAction("PayConclusion");
        }
        #endregion

        #region EXAMPLE PAYU METHODS FOR PAYMENT
        //public string HashWithSignature(string val, string signatureKey)
        //{
        //    return "";
        //}

        //public string POSTFormPAYU(string url, NameValueCollection data)
        //{
        //    return "";
        //}
        #endregion

        #region PAYMENT CONCLUSION
        public IActionResult PayConclusion()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }
        #endregion

        #region MY ORDERS
        public IActionResult MyOrders()
        {
            if (HttpContext.Session.GetString("UserInfo") != null)
            {
                List<vw_MyOrders> orders = co.SelectMyOrders(HttpContext.Session.GetString("UserInfo").ToString());
                return View(orders);
            }

            else
            {
                return RedirectToAction("Login");
            }
        }
        #endregion

        #region USER REGISTER
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (cu.RegisterEmailControl(user) == false)
            {
                bool answer = cu.AddUser(user);

                if (answer)
                {
                    TempData["Message"] = "You have successfully Signed-Up to alperkayali.com !";
                    return RedirectToAction(nameof(Login));
                }
            }

            else
            {
                TempData["Message"] = "The E-Mail adress given is already registered.";
            }

            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region USER LOGOUT
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("UserInfo");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region USER INFO UPDATE
        [HttpGet]
        public IActionResult UserInfoUpdate(int id)
        {

            if (id == null || context.Users == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = cu.SelectUserDetails(id);

            return View(user);
        }

        [HttpPost]
        public IActionResult UserInfoUpdate(User? user)
        {
            User? usr = context.Users.FirstOrDefault(s => s.UserID == user.UserID);
            user.IsAdmin = usr.IsAdmin;
            user.Active = usr.Active;

            if (user.NameSurname == null)
            {
                string? nameSurname = context.Users.FirstOrDefault(s => s.UserID == user.UserID).NameSurname;
                user.NameSurname = nameSurname;
            }

            if (user.Email == null)
            {
                string? email = context.Users.FirstOrDefault(u => u.UserID == user.UserID).Email;
                user.Email = email;
            }

            if (user.Telephone == null)
            {
                string? telephone = context.Users.FirstOrDefault(u => u.UserID == user.UserID).Telephone;
                user.Telephone = telephone;
            }

            if (user.InvoicesAdress == null)
            {
                string? invoiceAdress = context.Users.FirstOrDefault(u => u.UserID == user.UserID).InvoicesAdress;
                user.InvoicesAdress = invoiceAdress;
            }

            if (cls_User.MD5PassConverter(user.Password) == usr.Password || cls_User.MD5PassConverter(user.Password) == null)
            {
                string? password = context.Users.FirstOrDefault(s => s.UserID == user.UserID).Password;
                user.Password = password;
            }

            else
            {
                user.Password = cls_User.MD5PassConverter(user.Password);
            }

            bool answer = cu.UpdateUserInfo(user);
            if (answer == true)
            {
                TempData["Message"] = "User informations have been UPDATED successfully.";
                return RedirectToAction(nameof(Order));
            }
            else
            {
                TempData["Message"] = "ERROR ! User informations have been not been EDITED.";
                return RedirectToAction(nameof(Order));
            }
        }
        #endregion

        #region DETAILED SEARCH
        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers = context.Suppliers.ToList();
            return View();
        }

        public IActionResult DetSearchedProducts(int CategoryID, string[] SupplierID, string price, string IsInStock)
        {
            price = price.Replace(" ", "");
            string[] PriceArray = price.Split("-");
            string startPrice = PriceArray[0];
            string endPrice = PriceArray[1];

            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }

            int count = 0;
            string supplierValue = "";
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count == 0)
                {
                    supplierValue = "SupplierID =" + SupplierID[i];
                    count++;
                }

                else
                {
                    supplierValue += " or SupplierID =" + SupplierID[i];
                }
            }

            string query = " select * from Products where CategoryID = " + CategoryID + " and (" + supplierValue + ") and (UnitPrice " + sign + " " + startPrice + " and UnitPrice < " + endPrice + ") and Stock " + sign + " 0 order by ProductName";

            ViewBag.Products = cp.DetailedProductSearch(query);
            return View();
        }
        #endregion

        #region INDEX SEARCH AREA
        public PartialViewResult GetProducts(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<sp_Search> uList = cp.GetSearchedProducts(id);
            string json = JsonConvert.SerializeObject(uList);
            var response = JsonConvert.DeserializeObject<List<Search>>(json);

            return PartialView(response);
        }
        #endregion

        #region FAVOURITED ITEMS
        public IActionResult AddToFavourites(int id)
        {
            if (HttpContext.Session.GetString("UserInfo") != null)
            {
                User? usr = cu.SelectUserInfo(HttpContext.Session.GetString("UserInfo"));
                int userID = context.Users.FirstOrDefault(u => u.Email == usr.Email).UserID;

                bool isExists = context.Favourites.Any(f => f.ProductID == id);
                if (isExists == true)
                {
                    TempData["Message"] = "This products is already in your Favourited List.";
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    bool answer = cf.AddToFavourites(id, userID);

                    if (answer)
                    {
                        cp.Highlighted_Increase(id);
                        TempData["Message"] = "The Product has been saved as Favourited.";
                        return RedirectToAction(nameof(Favourites));
                    }

                    else
                    {
                        TempData["Message"] = "ERROR ! The Product has not been saved as Favourited.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            else
            {
                TempData["Message"] = "You should LOGIN first !";
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult RemoveFromFavourites(int id)
        {
            if (HttpContext.Session.GetString("UserInfo") != null)
            {
                int FavID = context.Favourites.FirstOrDefault(f => f.ProductID == id).FavouriteID;
                int ProdID = id;

                string? userMail = HttpContext.Session.GetString("UserInfo");
                int userID = context.Users.FirstOrDefault(u => u.Email == userMail).UserID;

                bool answer = cf.RemoveFromFavs(FavID, userID, ProdID);

                if (answer)
                {
                    TempData["Message"] = "The product has been removed from Favourites.";
                    return RedirectToAction(nameof(Favourites));
                }

                else
                {
                    TempData["Message"] = "ERROR ! The product has not been removed from Favourites.";
                    return RedirectToAction(nameof(Favourites));
                }
            }

            else
            {
                TempData["Message"] = "You should LOGIN first !";
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult Favourites()
        {
            if (HttpContext.Session.GetString("UserInfo") != null)
            {
                string userMail = HttpContext.Session.GetString("UserInfo");
                int userID = context.Users.FirstOrDefault(u => u.Email == userMail).UserID;

                List<Favourite>? favList = context.Favourites.Where(f => f.UserID == userID && f.ActiveFav == true).ToList();

                if (favList.Count() == 0)
                {
                    TempData["Message"] = "Seems like you haven not saved any products as FAVOURITE.";
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    return View(favList);
                }

            }
            else
            {
                TempData["Message"] = "You should LOGIN first !";
                return RedirectToAction(nameof(Login));
            }
        }
        #endregion

        #region ABOUT US
        public IActionResult AboutUs()
        {
            return View();
        }
        #endregion

        #region CONTACT US
        public IActionResult ContactUs()
        {
            return View();
        }
        #endregion

    }
}