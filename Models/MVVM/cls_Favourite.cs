using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Favourite
    {
        KayaliContext context = new KayaliContext();

        #region FAV CREATE
        public List<Favourite> FavProductSelect()
        {
            List<Favourite>? favProds = context.Favourites.ToList();
            return favProds;
        }

        public bool AddToFavourites(int id, int userID)
        {
            try
            {
                Product tobeFav = context.Products.FirstOrDefault(f => f.ProductID == id);
                Favourite FavListPrd = new Favourite();
                FavListPrd.ProductID = tobeFav.ProductID;
                FavListPrd.UserID = userID;
                FavListPrd.ProductName = tobeFav.ProductName;
                FavListPrd.UnitPrice = tobeFav.UnitPrice;
                FavListPrd.Stock = tobeFav.Stock;
                FavListPrd.PhotoPath = tobeFav.PhotoPath;
                FavListPrd.KDV = tobeFav.Kdv;
                FavListPrd.ActiveFav = true;

                context.Add(FavListPrd);
                context.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region FAV REMOVE
        public bool RemoveFromFavs(int FavID, int userID, int ProdID)
        {
            try
            {
                Favourite tobeDeleted = context.Favourites.FirstOrDefault(f => f.UserID == userID && f.ProductID == ProdID && f.FavouriteID == FavID);
                context.Favourites.Remove(tobeDeleted);
                context.SaveChanges();

                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }
        #endregion
    }
}
