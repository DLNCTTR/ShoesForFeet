using System.ComponentModel.DataAnnotations;

namespace ShoesForFeet.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Shoe size is required.")]
        [Range(1, 15, ErrorMessage = "Shoe size must be between 1 and 15.")]
        public int ShoeSize { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Price must be between €0.01 and €1000.")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image URL is required.")]
        public string ImageUrl { get; set; }
    }
}
    