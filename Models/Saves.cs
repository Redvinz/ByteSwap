using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteSwapFinalProject.Models
{
    [Table("Saves")]
    public class Saves
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Define the navigation properties for Users and Products
        public int UsersId { get; set; }
        [ForeignKey("UsersId")]
        public Users User { get; set; }

        public int ProductsId { get; set; }
        [ForeignKey("ProductsId")]
        public Products Product { get; set; }
    }
}
