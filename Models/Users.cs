
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ByteSwapFinalProject.Models
{
    public enum UserType
    {
        Admin= 0,
        User = 1
    }
    public class Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public  string Firstname { get; set; }
        public  string Lastname { get; set; }
        public  string Username { get; set; }
        public  string Password { get; set; }
        [NotMapped] // Not stored in the database
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public  string ContactNumber { get; set; }
        public UserType Type { get; set; }
        public  string Email { get; set; }
        public string? ProfilePicFN { get; set; }




        public IEnumerable<Contacts> UserContacts { get; set; }
        public Users()
        {
            // Initialize UserContacts to an empty list
            UserContacts = new List<Contacts>();
        }

    }
}
