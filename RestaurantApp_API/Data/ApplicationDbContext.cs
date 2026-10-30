using Microsoft.EntityFrameworkCore;
using RestaurantApp_API.Model;

namespace RestaurantApp_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        // override the OnModelCreating method to configure the model : 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the relationship between MenuItem and Category
            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure the precision for the decimal Price column :
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);
            // seed data for the database :

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Appetizers", Description = "Start your meal with our delicious appetizers.", CreatedDate = new DateTime(2023, 1, 1) },
                new Category { Id = 2, Name = "Main Courses", Description = "Satisfy your hunger with our hearty main courses.", CreatedDate = new DateTime(2023, 1, 1) },
                new Category { Id = 3, Name = "Desserts", Description = "Indulge in our sweet and delightful desserts.", CreatedDate = new DateTime(2023, 1, 1) }
            );
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Name = "Spring Rolls", Description = "Crispy spring rolls filled with vegetables.", Price = 5.99m, ImageUrl = "", IsAvailable = true, CategoryId = 1, CreatedDate = new DateTime(2023, 1, 1) },
                new MenuItem { Id = 2, Name = "Grilled Chicken", Description = "Juicy grilled chicken served with vegetables.", Price = 12.99m, ImageUrl = "", IsAvailable = true, CategoryId = 2, CreatedDate = new DateTime(2023, 1, 1) },
                new MenuItem { Id = 3, Name = "Chocolate Cake", Description = "Rich and moist chocolate cake.", Price = 6.99m, ImageUrl = "", IsAvailable = true, CategoryId = 3, CreatedDate = new DateTime(2023, 1, 1) }
            ); 

        }

    }

    
}
