using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            var newProduct = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (newProduct != null)
            {
                newProduct.Title = product.Title;
                newProduct.Description = product.Description;
                newProduct.Category = product.Category;
                newProduct.ISBN = product.ISBN;
                newProduct.Price = product.Price;
                newProduct.Price50 = product.Price50;   
                newProduct.Price100 = product.Price100;
                newProduct.Author = product.Author; 
                newProduct.Id = product.Id;
                if(product.ImageUrl != null)
                {
                    newProduct.ImageUrl = product.ImageUrl;
                }
            }

        }
    }
}
