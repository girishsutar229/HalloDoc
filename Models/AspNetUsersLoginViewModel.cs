using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class AspNetUsersLoginViewModel
    {
        [Key]

        [Required(ErrorMessage = "Please enter the email")]
        [StringLength(256)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter the password")]
        [Column(TypeName = "character varying")]
        public string? PasswordHash { get; set; } = null!;
    }
}
