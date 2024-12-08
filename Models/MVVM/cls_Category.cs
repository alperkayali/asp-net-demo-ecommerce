using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Category
    {
        KayaliContext context = new KayaliContext();

        public async Task<List<Category>> CategorySelectMain()
        {
            List<Category> categories = await context.Categories.ToListAsync();
            return categories;
        }


        public async Task<bool> CategoryInsert(Category category)
        {
            try
            {
                context.Add(category);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Category> CategoryDetails(int? id)
        {
            Category? categories = await context.Categories.FindAsync(id);
            return categories;
        }


        public async Task<bool> CategoryEdit(Category category)
        {
            try
            {
                List<Category> subCatList = await context.Categories.Where(c => c.ParentID == category.CategoryID).ToListAsync();
                foreach (var item in subCatList)
                {
                    item.Active = category.Active;
                }
                context.Update(category);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public async Task<bool> CategoryDelete(int? id)
        {
            //this method UPDATES the choosen item's ACTIVE COLON as FALSE !            
            try
            {
                Category? cat = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
                cat.Active = false;

                List<Category> subCatsList = context.Categories.Where(sc => sc.ParentID == id).ToList();
                foreach (var item in subCatsList)
                {
                    item.Active = false;
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region not using
        //public string DelMainCatg(int id)
        //{
        //    //First, to delete sub-categories BELONG to the MAIN CATEGORY:
        //    List<Category> subCategories = context.Categories.Where(c => c.ParentID == id).ToList();
        //    foreach (var item in subCategories)
        //    {
        //        context.Categories.Remove(item);
        //        context.SaveChanges();
        //    }

        //    //Then, to delete MAIN CATEGORY chosen
        //    Category? mainCategories = context.Categories.FirstOrDefault(c => c.CategoryID == id);
        //    if (mainCategories != null)
        //    {
        //        context.Categories.Remove(mainCategories);
        //        context.SaveChanges();
        //        return "Main Category and it's Sub-Categories are DELETED PERMANENTLY.";
        //    }
        //    else
        //    {
        //        return "Category is EMPTY !";
        //    }

        //}
        //public string DelSubCatg(int id)
        //{
        //    Category? subCategory = context.Categories.FirstOrDefault(c => c.CategoryID == id);
        //    if (subCategory != null)
        //    {
        //        context.Categories.Remove(subCategory);
        //        context.SaveChanges();
        //        return "Sub-Category is DELETED PERMANENTLY.";
        //    }
        //    else
        //    {
        //        return "Sub-Category is EMPTY !";
        //    }

        //}
        #endregion
    }
}
