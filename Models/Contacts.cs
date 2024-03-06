using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteSwapFinalProject.Models
{
    [Table("Contacts")]
    public class Contacts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign key
        [ForeignKey("User")]
        public int UsersId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        // Navigation property to represent the related Users entity
        public Users User { get; set; }
    }
}
