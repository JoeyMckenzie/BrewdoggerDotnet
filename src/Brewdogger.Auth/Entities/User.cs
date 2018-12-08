using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewdogger.Auth.Entities
{
    [Table("Users")]
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(32)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(32)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(16)]
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        [EmailAddress(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
    }
}