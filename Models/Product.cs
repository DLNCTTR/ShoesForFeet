using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesForFeet.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Price must be between $0.01 and $10,000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Range(1, 50, ErrorMessage = "Shoe size must be between 1 and 50")]
        public int ShoeSize { get; set; }

        public string ImageUrl { get; set; } // URL to the product image

        // Navigation property for related reviews
        public ICollection<Review> Reviews { get; set; }
    }
}