using Microsoft.EntityFrameworkCore;
using ProductDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDAL.Context
{
    public class ProductContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> photos { get; set; }



        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        public static async Task CheckDatabaseAsync(DbContextOptions<ProductContext> options)
        {
            using var context = new ProductContext(options);
            var isDeleted = await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

   


    }
}
