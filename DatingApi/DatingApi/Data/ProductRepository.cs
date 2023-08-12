using DatingApi.Interface;
using Microsoft.EntityFrameworkCore;
using ProductDAL.Context;
using ProductDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DatingApi.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _productContext;
        public ProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }
        public async Task AddAsync(Product product)
        {

            await _productContext.Products.AddAsync(product);

            await _productContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productContext.Products.ToListAsync();
        }
    }
}
