using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteSwapFinalProject.Models
{
    [Table("Conditions")]
    public class Conditions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public  string Name { get; set; }
        public  string Detail { get; set; }
    }
}
