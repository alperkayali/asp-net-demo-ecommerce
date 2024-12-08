using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using AspNetCore_WebAPP_MVC_PROJE.Models.DbViews;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Product
    {
        #region -- PROPS --

        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }

        #endregion

        KayaliContext context = new KayaliContext();

        int subpageCount = 0;

        #region PRODUCT CRUD METHODS
        public async Task<List<Product>> ProductSelect()
        {
            List<Product>? products = await context.Products.ToListAsync();
            return products;
        }
        public async Task<bool> ProductInsert(Product product)
        {
            try
            {
                context.Add(product);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Product> ProductDetails(int? id)
        {
            Product? product = await context.Products.FindAsync(id);
            return product;
        }
        public bool ProductEdit(Product product)
        {
            try
            {
                context.Update(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ProductDelete(int? id)
        {
            try
            {
                Product? product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
                product.Active = false;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public Product SpecialProductDetails(string mainPageName)
        {
            Product? product = context.Products.FirstOrDefault(p => p.StatusID == 6);

            return product;
        }
        public List<Product> ProductSelectByCatID(int id)
        {
            List<Product>? products = context.Products.Where(p => p.CategoryID == id).ToList();
            return products;
        }
        public List<Product> ProductSelectBySupID(int id)
        {
            List<Product>? productsOfSupplier = context.Products.Where(p => p.SupplierID == id).ToList();
            return productsOfSupplier;
        }

        //Highlighted column increase whenever Product is viewed
        public void Highlighted_Increase(int id)
        {
            Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
            product.HighLighted += 1;
            context.Update(product);
            context.SaveChanges();
        }

        #region METHOD QUALIFIED (HEADER) PRODUCT PAGES
        public List<Product> ProductSelect(string mainPageName, int mainpageCount, string subpageName, int pageNumber)
        {
            subpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).subpageCount;

            List<Product> productList;

            //takes New Products, added by AddDate property.
            if (mainPageName == "New")
            {
                if (subpageName == "")
                {
                    //Home/Index
                    productList = context.Products.OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pageNumber == 0)
                    {
                        // ~/Home/NewProducts page (load more items button)
                        productList = context.Products.OrderByDescending(p => p.AddDate).Take(subpageCount).ToList();
                    }
                    else
                    {
                        //AJAX
                        productList = context.Products.OrderByDescending(p => p.AddDate).Skip(pageNumber * 4).Take(subpageCount).ToList();
                    }
                }
            }

            else if (mainPageName == "Special")
            {
                if (subpageName == "")
                {
                    productList = context.Products.Where(p => p.StatusID == 2).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pageNumber == 0)
                    {
                        productList = context.Products.Where(p => p.StatusID == 2).OrderBy(p => p.ProductName).Take(subpageCount).ToList();
                    }
                    else
                    {
                        productList = context.Products.Where(p => p.StatusID == 2).OrderBy(p => p.ProductName).Skip(pageNumber * 4).Take(subpageCount).ToList();
                    }
                }
            }

            //takes Discounted Products, descending by discount rate
            else if (mainPageName == "Discount")
            {
                if (subpageName == "")
                {
                    //Home/Index
                    productList = context.Products.OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pageNumber == 0)
                    {
                        productList = context.Products.OrderByDescending(p => p.Discount).Take(subpageCount).ToList();
                    }
                    else
                    {
                        //ajax
                        productList = context.Products.OrderByDescending(p => p.Discount).Skip(pageNumber * 4).Take(subpageCount).ToList();
                    }
                }
            }

            //takes Highlighted Products
            else if (mainPageName == "Highlighted")
            {
                if (subpageName == "")
                {
                    //Home/Index
                    productList = context.Products.OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pageNumber == 0)
                    {
                        productList = context.Products.OrderByDescending(p => p.HighLighted).Take(subpageCount).ToList();
                    }
                    else
                    {
                        productList = context.Products.OrderByDescending(p => p.HighLighted).Skip(pageNumber * 4).Take(subpageCount).ToList();
                    }
                }
            }

            //takes TopSeller Products
            else if (mainPageName == "TopSeller")
            {
                if (subpageName == "")
                {
                    //Home/Index
                    productList = context.Products.OrderByDescending(p => p.TopSeller).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pageNumber == 0)
                    {
                        productList = context.Products.OrderByDescending(p => p.TopSeller).Take(subpageCount).ToList();
                    }
                    else
                    {
                        productList = context.Products.OrderByDescending(p => p.TopSeller).Skip(pageNumber * 4).Take(subpageCount).ToList();
                    }
                }
            }

            //takes Slider Products
            else if (mainPageName == "Slider")
            {
                productList = context.Products.Where(p => p.StatusID == 1).Take(mainpageCount).ToList();
            }

            //takes Starred Products
            else if (mainPageName == "Starred")
            {
                productList = context.Products.Where(p => p.StatusID == 3).Take(mainpageCount).ToList();
            }

            //takes Opportunity Products
            else if (mainPageName == "Opportunity")
            {
                productList = context.Products.Where(p => p.StatusID == 4).Take(mainpageCount).ToList();
            }

            //takes Attentioned Products
            else if (mainPageName == "Attentioned")
            {
                productList = context.Products.Where(p => p.StatusID == 5).Take(mainpageCount).ToList();
            }

            //this wont show up
            else
            {
                productList = context.Products.ToList();
            }
            return productList;
        }
        #endregion

        //DETAILED PRODUCT SEARCHING
        public List<cls_Product> DetailedProductSearch(string query)
        {
            List<cls_Product> products = new List<cls_Product>();
            SqlConnection sqlConnection = Connection.GetConnect;
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                cls_Product product = new cls_Product();
                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();

                products.Add(product);
            }
            return products;
        }

        //MAIN PAGE SEARCH INPUT METHOD
        public List<sp_Search> GetSearchedProducts(string id)
        {
            var products = context.sp_Search.FromSqlRaw($"sp_search {id}").ToList();
            return products;
        }


    }
}
