using System.ComponentModel.DataAnnotations;

namespace Euroleague.Models
{
    public class User
    {
        public int Id { get; set; }  

        [Required]
        [StringLength(100)]
        public string Username { get; set; }  

        [Required]
        public string PasswordHash { get; set; } 

        [StringLength(200)]
        public string FullName { get; set; }  

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
