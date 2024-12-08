using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Supplier
    {
        KayaliContext context = new KayaliContext();

        public async Task<List<Supplier>> SupplierSelect()
        {
            List<Supplier> suppliers = await context.Suppliers.ToListAsync();
            return suppliers;
        }


        public async Task<bool> SupplierInsert(Supplier supplier)
        {
            try
            {
                context.Add(supplier);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Supplier> SupplierDetails(int? id)
        {
            Supplier? suppliers = await context.Suppliers.FindAsync(id);
            return suppliers;
        }


        public bool SupplierEdit(Supplier supplier)
        {
            try
            {
                context.Update(supplier);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> SupplierDelete(int? id)
        {
            //this method UPDATES the choosen item's ACTIVE COLON as FALSE !            
            try
            {
                Supplier? sup = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
                sup.Active = false;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
