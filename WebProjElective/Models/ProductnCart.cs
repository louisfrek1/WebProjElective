using Microsoft.EntityFrameworkCore;

namespace WebProjElective.Models
{
    public class ProductnCart : DbContext
    {
        public ProductnCart(DbContextOptions<ProductnCart> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Cart>()
        //        .HasKey(c => c.CartId);  // Define CartId as the primary key
        //}
    }
}
