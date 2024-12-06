using System.Collections.Generic;
using ShoesForFeet.Models;

namespace ShoesForFeet.Data
{
    public class ProductRepository
    {
        // Static list to simulate a database
        private static readonly List<Product> _products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Running Shoes",
                ShoeSize = 10,
                Price = 79.99m,
                ImageUrl = "/Images/Products/Runners.jpg",
                Description = "Comfortable running shoes designed for performance."
            },
            new Product
            {
                Id = 2,
                Name = "Casual Sneakers",
                ShoeSize = 9,
                Price = 49.99m,
                ImageUrl = "/Images/Products/Sneakers.jpg",
                Description = "Versatile sneakers for everyday wear."
            }
        };

        // Get all products
        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        // Add a new product
        public void AddProduct(Product product)
        {
            product.Id = _products.Count > 0 ? _products[^1].Id + 1 : 1; // Assign a unique ID
            _products.Add(product);
        }

        // Get a product by ID
        public Product GetProductById(int id)
        {
            return _products.Find(p => p.Id == id);
        }

        // Update an existing product
        public void UpdateProduct(Product updatedProduct)
        {
            var existingProduct = _products.Find(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.ShoeSize = updatedProduct.ShoeSize;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.ImageUrl = updatedProduct.ImageUrl;
                existingProduct.Description = updatedProduct.Description;
            }
        }

        // Get products under a specific price (for specials)
        public IEnumerable<Product> GetSpecials(decimal maxPrice)
        {
            return _products.FindAll(p => p.Price < maxPrice);
        }

        // Search products by name or description
        public IEnumerable<Product> SearchProducts(string query)
        {
            return _products.FindAll(p =>
                p.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(query, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
