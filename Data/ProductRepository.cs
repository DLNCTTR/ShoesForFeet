using System.Collections.Generic;
using ShoesForFeet.Models;

namespace ShoesForFeet.Data
{
    public class ProductRepository
    {
        // Static list to simulate a database
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Running Shoes", ShoeSize = 10, Price = 79.99m },
            new Product { Id = 2, Name = "Casual Sneakers", ShoeSize = 9, Price = 49.99m }
        };

        // Get all products
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        // Add a new product
        public void Add(Product product)
        {
            product.Id = _products.Count + 1;
            _products.Add(product);
        }
    }
}