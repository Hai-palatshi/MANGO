using Mango.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;
using Mango.Services.ShoppingCartAPI.Models.Dto;
namespace Mango.Services.ShoppingCartAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CardDetail> CardDetail { get; set; }
        public DbSet<CartHeader> CartHeader { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>().HasNoKey(); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
