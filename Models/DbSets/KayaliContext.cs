using AspNetCore_WebAPP_MVC_PROJE.Models.DbViews;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class KayaliContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:KayaliConnection"]);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<vw_MyOrders> vw_MyOrders { get; set; }
        public DbSet<sp_Search> sp_Search { get; set; }

    }
}
