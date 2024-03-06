using ByteSwapFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ByteSwapFinalProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Conditions> Conditions { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Saves> Saves { get; set; }
        public DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Products>()
                .HasMany(p => p.ProductImages)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Adjust precision and scale as needed

            // Additional configurations or relationships can be added here
            // if needed for your application
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Categories>().ToTable("Categories");
        //    modelBuilder.Entity<Contacts>().ToTable("Contacts");
        //    modelBuilder.Entity<Images>().ToTable("Images");
        //    modelBuilder.Entity<Products>().ToTable("Products");
        //    modelBuilder.Entity<Saves>().ToTable("Saves");
        //    modelBuilder.Entity<Users>().ToTable("Users");
        //}

    }
}
