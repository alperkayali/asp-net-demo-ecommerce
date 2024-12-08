using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Status
    {
        KayaliContext context = new KayaliContext();
        public async Task<List<Status>> StatusSelect()
        {
            List<Status> statuses = await context.Statuses.ToListAsync();
            return statuses;
        }

        public async Task<bool> StatusInsert(Status status)
        {
            try
            {
                context.Add(status);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Status> StatusDetails(int? id)
        {
            Status? status = await context.Statuses.FindAsync(id);
            return status;
        }


        public async Task<bool> StatusEdit(Status status)
        {
            context.Update(status);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StatusDelete(int? id)
        {
            //this method UPDATES the choosen item's ACTIVE COLON as FALSE !            
            try
            {
                Status? status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);
                status.Active = false;
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
