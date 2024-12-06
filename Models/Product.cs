using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesForFeet.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Shoe Size is required.")]
        [Range(1, 50, ErrorMessage = "Shoe Size must be between 1 and 50.")]
        public int ShoeSize { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        // Image URL stored in the database, optional for edit scenarios
        public string? ImageUrl { get; set; }

        // Not mapped to the database, used for file uploads
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}