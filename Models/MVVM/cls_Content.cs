using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Content
    {
        KayaliContext context = new KayaliContext();

        public async Task<List<Content>> ContentList()
        {
            List<Content> contentList = await context.Contents.ToListAsync();
            return contentList;
        }

        public async Task<bool> ContentInsert(Content content)
        {
            try
            {
                context.Add(content);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Content> ContentDetails(int? id)
        {
            Content? contentDets = await context.Contents.FindAsync(id);
            return contentDets;
        }

        public bool ContentEdit(Content content)
        {
            try
            {
                context.Update(content);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ContentDelete(int? id)
        {
            //this method UPDATES the choosen item's ACTIVE COLON as FALSE !            
            try
            {
                Content? cont = await context.Contents.FirstOrDefaultAsync(c => c.ContentID == id);
                cont.Active = false;
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
