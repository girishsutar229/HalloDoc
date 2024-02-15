using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class PatientRegisterViewModel
    {
        [Key]

        [Required(ErrorMessage = "Please enter the email")]
        [StringLength(256)]
        public string Email { get; set; } = null!;

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please the EnterValid password")]
        [Required(ErrorMessage = "Password is required")]
        [Column(TypeName = "character varying")]
        [DataType(DataType.Password)]
        [MaxLength(255)]
        public string? PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [NotMapped] // This property won't be mapped to the database
        public string? ConfirmPassword { get; set; }
    }
}