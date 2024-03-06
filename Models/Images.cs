using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteSwapFinalProject.Models
{
    [Table("Images")]
    public class Images
    {
        public int Id { get; set; }

        public int ProductsId { get; set; } // Ensure this matches the database schema
        [ForeignKey("ProductsId")]
        public Products Product { get; set; }
        public string Filename { get; set; }

    }
}