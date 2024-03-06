using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSwapFinalProject.Models
{
    public enum Status
    {
        Available = 0,
        Sold = 1,
        Reserved = 2,
        Unavailable = 3
    }

    [Table("Products")]
    public class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        // Define the navigation property for Category
        public int CategoriesId { get; set; }
        [ForeignKey("CategoriesId")]
        public Categories Category { get; set; }

        public int UsersId { get; set; }
        [ForeignKey("UsersId")]
        public Users Users { get; set; }
        public Status Status { get; set; }
        public string Title { get; set; }
        public string? Brand { get; set; } 
        public int ConditionsId { get; set; } // Ensure this matches the database schema
        [ForeignKey("ConditionsId")]
        public Conditions Conditions { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Method { get; set; }
        public DateTime DatePosted { get; set; }

        [InverseProperty("Product")]
        public ICollection<Images> ProductImages { get; set; }

        public Products()
        {
            Status = Status.Available; // Set default status
            DatePosted = DateTime.UtcNow; // Set default date (UTC time when the product is created)
            Brand = "No Brand";
        }
    }
}
