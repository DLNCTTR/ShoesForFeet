using System.ComponentModel.DataAnnotations;

namespace ShoesForFeet.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Reviewer name cannot exceed 100 characters")]
        public string ReviewerName { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Comment { get; set; }

        // Foreign key to associate review with a product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}