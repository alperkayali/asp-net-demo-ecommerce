using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Settings
    {
        KayaliContext context = new KayaliContext();

        public async Task<List<Setting>> SettingsList()
        {
            List<Setting>? settings = await context.Settings.ToListAsync();
            return settings;
        }

        public async Task<bool> SettingInsert(Setting setting)
        {
            try
            {
                context.Add(setting);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Setting> SettingDetails(int? id)
        {
            Setting? setting = await context.Settings.FindAsync(id);
            return setting;
        }


        public async Task<bool> SettingEdit(Setting setting)
        {
            context.Update(setting);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
